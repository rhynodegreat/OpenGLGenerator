using System;
using System.Collections.Generic;

using OpenGLGenerator;

namespace GeneratorTest {
    public class CSCommand {
        public string Name { get; set; }
        public List<CSParam> Parameters { get; set; }
        public string ReturnType { get; set; }
        public string ReturnGroup { get; set; }

        public CSCommand(Command c) {
            Name = c.Name;
            ReturnType = GetType(c.ReturnType);
            ReturnGroup = c.ReturnGroup;
            Parameters = new List<CSParam>(c.Parameters.Count);

            for (int i = 0; i < c.Parameters.Count; i++) {
                Parameters.Add(new CSParam(this, c.Parameters[i]));
            }
        }

        public CSParam GetParam(string name) {
            for (int i = 0; i < Parameters.Count; i++) {
                if (Parameters[i].Name == name) {
                    return Parameters[i];
                }
            }
            return null;
        }

        string GetType(string input) {
            switch (input) {
                case "void*": return "IntPtr";
                default: return input;
            }
        }
    }
}
