using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGenerator {
    public class Parameter {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Group { get; set; }
        public bool Pointer { get; set; }

        public Parameter(string name, string type, string group, bool pointer) {
            Name = name;
            Type = type;
            Group = group;
            Pointer = pointer;
        }
    }
}
