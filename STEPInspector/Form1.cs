using STEPInspector.Entities;
using System.Runtime.Serialization.Formatters;
using System.Text.RegularExpressions;

namespace STEPInspector
{
    public partial class Form1 : Form
    {
        string filePath;
        string[] lines;
        Dictionary<string, string> entityMap;

        public Form1()
        {
            InitializeComponent();
            entityMap = new Dictionary<string, string>();
        }

        private readonly string[] supportedEntityTypes =
        {
            "CARTESIAN_POINT",
            "VERTEX_POINT",
            "EDGE_CURVE",
            "ADVANCED_FACE",
            "MANIFOLD_SOLID_BREP",
            "LINE",
            "VECTOR",
            "CIRCLE",
            "PLANE"
        };

        private void button_OpenSTEPFile_Click(object sender, EventArgs e)
        {
            // Update the status
            label_Status.Text = "Selecting a STEP file...";

            // Retrieve the step file path
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "STEP files (*.stp;*.step)|*.stp;*.step|All files (*.*)|*.*";

            DialogResult result = openFileDialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                label_Status.Text = "No STEP file selected.";
                return;
            }

            filePath = openFileDialog.FileName;

            // Update the status
            label_Status.Text = "Parsing the STEP file...";

            // Read all the lines
            lines = File.ReadAllLines(filePath);

            // Read the entities
            ReadEntities();

            // Update the tree view
            UpdateTreeView();

            // Update the status
            label_Status.Text = "The STEP file parsing has been completed.";
        }

        private void ReadEntities()
        {
            entityMap.Clear();

            foreach(string originalLine in lines)
            {
                string line = originalLine.Trim();

                if(!line.StartsWith("#"))   // Skip the non-entity related lines
                {
                    continue;
                }

                int equalIndex = line.IndexOf('=');   // Locate the "=" in the line 

                if (equalIndex < 0)
                {
                    continue;   // Skip the line with out "="
                }

                string id = line.Substring(0, equalIndex).Trim();    // Extract the entity number (e.g. #76), the string in front of "="

                // Add the line with the extracted entity number as a key in the dictionary "entityMap" 
                // entityMap["#76"] = "CARTESIAN_POINT('',(0.,0.,0.))"
                entityMap[id] = line.Substring(equalIndex + 1).Trim();
            }
        }

        private void UpdateTreeView()
        {
            // Clear the TreeView
            treeView_STEP.Nodes.Clear();

            TreeNode root = treeView_STEP.Nodes.Add("STEP FILE");

            Dictionary<string, TreeNode> categoryNodes = new Dictionary<string, TreeNode>();
            int countTotal = 0;

            foreach (string entityType in supportedEntityTypes)
            {
                int count = entityMap.Values.Count(line => GetEntityType(line) == entityType);

                TreeNode categoryNode = root.Nodes.Add($"{entityType} ({count})");

                categoryNodes[entityType] = categoryNode;
                countTotal = countTotal + count;
            }

            root.Text = $"STEP FILE ({countTotal})";  // Update the root node text with the total count number

            foreach (KeyValuePair<string, string> entity in entityMap)
            {
                string id = entity.Key;
                string line = entity.Value;
                string entityType = GetEntityType(line);

                if (!categoryNodes.ContainsKey(entityType))
                {
                    continue;
                }

                TreeNode entityNode = categoryNodes[entityType].Nodes.Add($"{id} [{entityType}]");

                entityNode.Tag = id;

                AddReferenceNodes(line, entityNode);
            }
            root.Expand();
        }

        private void AddReferenceNodes(string line, TreeNode parentNode)
        {
            List<string> references = GetReferences(line);

            foreach (string referenceId in references)
            {
                string displayText = referenceId;

                if (entityMap.TryGetValue(referenceId, out string referencedLine))
                {
                    string referencedType = GetEntityType(referencedLine);

                    displayText = $"{referenceId} [{referencedType}]";
                }

                TreeNode referenceNode = parentNode.Nodes.Add(displayText);

                referenceNode.Tag = referenceId;
            }
        }

        private string GetEntityType(string line)
        {
            int entityTypeIndex = line.IndexOf('(');

            if (entityTypeIndex < 0)
            {
                return line;
            }
            return line.Substring(0, entityTypeIndex);
        }

        private void treeView_STEP_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(e.Node.Tag == null) { return; }

            string id = e.Node.Tag.ToString();

            if (entityMap.ContainsKey(id))
            {
                if (entityMap[id].Contains("CARTESIAN_POINT"))
                {
                    CartesianPoint cartesianPoint = CartesianPoint.ParseCartesianPoint(id + "=" + entityMap[id]);

                    richTextBox_Entity.Text =
                        $"Entity Type : CARTESIAN_POINT\n" +
                        $"ID : {cartesianPoint.Id}\n\n" +
                        $"X : {cartesianPoint.X}\n" +
                        $"Y : {cartesianPoint.Y}\n" +
                        $"Z : {cartesianPoint.Z}\n";
                }
                else
                {
                    richTextBox_Entity.Text = entityMap[id];
                }
            }
        }

        private List<string> GetReferences(string line)
        {
            List<string> refs = new List<string>();
            MatchCollection matches = Regex.Matches(line, @"#\d+");

            for (int i = 0; i < matches.Count; i++)
            {
                refs.Add(matches[i].Value);
            }
            return refs;
        }
    }
}
