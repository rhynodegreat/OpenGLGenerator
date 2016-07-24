using System;
using System.Collections.Generic;

namespace OpenGLGenerator {
    public class Group {
        public string Name { get; private set; }
        public List<string> EnumNames { get; private set; }

        public Group(string name) {
            Name = name;
            EnumNames = new List<string>();
        }
    }
}
