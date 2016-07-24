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
        public string Len { get; private set; }
        public bool Pointer { get; private set; }
        public bool Const { get; private set; }
        public int PointerLevels { get; private set; }

        public Parameter(string name, string type, string group, string len, bool pointer, bool _const, int levels) {
            Name = name;
            Type = type;
            Group = group;
            Len = len;
            Pointer = pointer;
            Const = _const;
            PointerLevels = levels;
        }
    }
}
