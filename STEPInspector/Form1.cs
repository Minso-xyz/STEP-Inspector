using System.Text.RegularExpressions;

namespace STEPInspector
{
    public partial class Form1 : Form
    {
        string filePath;
        string[] lines;
        Dictionary<string, string> entityMap;

        int count_CARTESIAN_POINT;
        int count_VERTEX_POINT;
        int count_EDGE_CURVE;
        int count_ADVANCED_FACE;
        int count_MANIFOLD_SOLID_BREP;

        public Form1()
        {
            InitializeComponent();
            entityMap = new Dictionary<string, string>();
        }

        private void button_OpenSTEPFile_Click(object sender, EventArgs e)
        {
            // Update the status
            label_Status.Text = "Selecting a STEP file...";

            // Retrieve the step file path
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }

            // Update the status
            label_Status.Text = "Parsing the STEP file...";

            // Read all the lines
            lines = File.ReadAllLines(filePath);

            // Count the entities
            CountAndReadEntities();

            // Update the tree view
            UpdateTreeView();

            // Update the status
            label_Status.Text = "The STEP file parsing has been completed.";
        }

        private void CountAndReadEntities()
        {
            // Clear the previous records
            count_CARTESIAN_POINT = 0;
            count_VERTEX_POINT = 0;
            count_EDGE_CURVE = 0;
            count_ADVANCED_FACE = 0;
            count_MANIFOLD_SOLID_BREP = 0;

            entityMap.Clear();

            // Count the entities
            foreach (string line in lines)
            {
                if (line.StartsWith("#") && line.Contains("CARTESIAN_POINT"))
                {
                    count_CARTESIAN_POINT = count_CARTESIAN_POINT + 1;
                    entityMap.Add(line.Split('=')[0], line.Split('=')[1]);
                }
                else if (line.StartsWith("#") && line.Contains("VERTEX_POINT"))
                {
                    count_VERTEX_POINT = count_VERTEX_POINT + 1;
                    entityMap.Add(line.Split('=')[0], line.Split('=')[1]);
                }
                else if (line.StartsWith("#") && line.Contains("EDGE_CURVE"))
                {
                    count_EDGE_CURVE = count_EDGE_CURVE + 1;
                    entityMap.Add(line.Split('=')[0], line.Split('=')[1]);
                }
                else if (line.StartsWith("#") && line.Contains("ADVANCED_FACE"))
                {
                    count_ADVANCED_FACE = count_ADVANCED_FACE + 1;
                    entityMap.Add(line.Split('=')[0], line.Split('=')[1]);
                }
                else if (line.StartsWith("#") && line.Contains("MANIFOLD_SOLID_BREP"))
                {
                    count_MANIFOLD_SOLID_BREP = count_MANIFOLD_SOLID_BREP + 1;
                    entityMap.Add(line.Split('=')[0], line.Split('=')[1]);
                }
            }
        }

        private void UpdateTreeView()
        {
            // Clear the TreeView
            treeView_STEP.Nodes.Clear();

            // Add the root node
            int countTotal = count_CARTESIAN_POINT + count_VERTEX_POINT + count_EDGE_CURVE + count_ADVANCED_FACE;
            TreeNode root = treeView_STEP.Nodes.Add("STEP FILE = (" + countTotal + ")");

            // Add each type of the entities
            TreeNode pointsNode = root.Nodes.Add("CARTESIAN_POINT = (" + count_CARTESIAN_POINT + ")");
            TreeNode vertexNode = root.Nodes.Add("VERTEX_POINT = (" + count_VERTEX_POINT + ")");
            TreeNode edgeNode = root.Nodes.Add("EDGE_CURVE = (" + count_EDGE_CURVE + ")");
            TreeNode faceNode = root.Nodes.Add("ADVANCED_FACE = (" + count_ADVANCED_FACE + ")");
            TreeNode solidBRepNode = root.Nodes.Add("MANIFOLD_SOLID_BREP = (" + count_MANIFOLD_SOLID_BREP + ")");

            // Add the actual entities
            // Read all the lines
            lines = File.ReadAllLines(filePath);

            // Count the entities
            foreach (string line in lines)
            {
                string idText;

                if (line.StartsWith("#") && line.Contains("CARTESIAN_POINT"))
                {
                    idText = line.Split('=')[0];
                    TreeNode subNode = pointsNode.Nodes.Add(idText);
                    AddSubTreeView(line, subNode);
                }
                else if (line.StartsWith("#") && line.Contains("VERTEX_POINT"))
                {
                    idText = line.Split('=')[0];
                    TreeNode subNode = vertexNode.Nodes.Add(idText);
                    AddSubTreeView(line, subNode);
                }
                else if (line.StartsWith("#") && line.Contains("EDGE_CURVE"))
                {
                    idText = line.Split('=')[0];
                    TreeNode subNode = edgeNode.Nodes.Add(idText);
                    AddSubTreeView(line, subNode);
                }
                else if (line.StartsWith("#") && line.Contains("ADVANCED_FACE"))
                {
                    idText = line.Split('=')[0];
                    TreeNode subNode = faceNode.Nodes.Add(idText);
                    AddSubTreeView(line, subNode);
                }
                else if (line.StartsWith("#") && line.Contains("MANIFOLD_SOLID_BREP"))
                {
                    idText = line.Split('=')[0];
                    TreeNode subNode = solidBRepNode.Nodes.Add(idText);
                    AddSubTreeView(line, subNode);
                }
            }
        }

        private void treeView_STEP_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string nodeText = e.Node.Text;

            foreach (string line in lines)
            {
                if (line.StartsWith(nodeText + "="))
                {
                    richTextBox_Entity.Text = line;
                    break;
                }
            }
        }

        private List<string> GetReferences(string line)
        {
            List<string> refs = new List<string>();
            MatchCollection matches = Regex.Matches(line, @"#\d+");

            // Add matches to ids except the first value
            for (int i=1; i<matches.Count; i++)
            {
                refs.Add(matches[i].Value);
            }
            return refs;
        }

        private void AddSubTreeView(String line, TreeNode treeNode)
        {
            List<string> subElements = GetReferences(line);
            foreach(String subElement in subElements)
            {
                treeNode.Nodes.Add(subElement);
            }
        }
    }
}
