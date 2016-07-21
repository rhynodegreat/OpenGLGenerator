using System;
using System.Collections.Generic;

namespace OpenGLGenerator {
    public class Group {
        public string Name { get; set; }
        public List<string> EnumNames { get; set; }

        public Group(string name) {
            Name = name;
            EnumNames = new List<string>();
        }
    }
}
