using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEPInspector
{
    public class StepMetadata
    {
        public string FileName { get; set; }
        public int EntityCount { get; set; }

        public Dictionary<string, int> EntityTypes { get; set; } = new Dictionary<string, int>();

    }
}
