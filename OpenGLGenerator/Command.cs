using System;
using System.Collections.Generic;

namespace OpenGLGenerator {
    public class Command {
        public string Name { get; private set; }
        public string ReturnType { get; private set; }
        public string ReturnGroup { get; private set; }
        public List<Parameter> Parameters { get; private set; }

        public Command(string name, string returnType, string returnGroup) {
            Name = name;
            ReturnType = returnType;
            ReturnGroup = returnGroup;
            Parameters = new List<Parameter>();
        }
    }
}
