using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGenerator {
    public class Parameter {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string Group { get; private set; }

        public Parameter(string name, string type, string group) {
            Name = name;
            Type = type;
            Group = group;
        }
    }
}
