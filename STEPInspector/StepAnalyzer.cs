using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace STEPInspector
{
    //File.ReadAllLines()
    //ReadEntities()
    //GetEntityType()
    //GetReferences()
    //entity count
    //metadata

    public class StepAnalyzer
    {
        Dictionary<string, string> entityMap;
        string[] lines;

        // Get the list of entities to read
        string[] supportedEntityTypes = File.ReadAllLines(@"Schemas\Custom.txt");

        public StepMetadata Analyze(string filePath)
        {
            StepMetadata stepMetadata = new StepMetadata();
            entityMap = new Dictionary<string, string>();

            // 1. Reading STEP file
            lines = File.ReadAllLines(filePath);

            // 2. Making entityMap
            ReadEntities();

            // 3. Basic Metadata
            stepMetadata.FileName = Path.GetFileName(filePath);
            stepMetadata.EntityCount = entityMap.Count;

            // 4. Counting each entity type
            foreach (string entityType in supportedEntityTypes)
            {
                int count = entityMap.Values.Count(line => GetEntityType(line) == entityType);

                if (count>0)
                {
                    stepMetadata.EntityTypes[entityType] = count;
                }
            }
            return stepMetadata;
        }

        private void ReadEntities()
        {
            entityMap.Clear();

            foreach (string originalLine in lines)
            {
                string line = originalLine.Trim();

                if (!line.StartsWith("#"))   // Skip the non-entity related lines
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

        private string GetEntityType(string line)
        {
            int entityTypeIndex = line.IndexOf('(');

            if (entityTypeIndex < 0)
            {
                return line;
            }
            return line.Substring(0, entityTypeIndex);
        }


}


    }
