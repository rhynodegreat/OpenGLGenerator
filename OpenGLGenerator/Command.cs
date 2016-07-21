using System;
using System.Collections.Generic;

namespace OpenGLGenerator {
    public class Command {
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public string ReturnGroup { get; set; }
        public List<Parameter> Parameters { get; set; }

        public Command(string name, string returnType, string returnGroup) {
            Name = name;
            ReturnType = returnType;
            ReturnGroup = returnGroup;
            Parameters = new List<Parameter>();
        }
    }
}
