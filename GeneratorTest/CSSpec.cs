using System;
using System.Collections.Generic;

using OpenGLGenerator;

namespace GeneratorTest {
    public class CSSpec {
        Spec spec;
        public List<CSEnum> Enums { get; set; }
        public List<CSCommand> Commands { get; set; }

        public CSSpec(Spec spec) {
            this.spec = spec;
            Enums = new List<CSEnum>();
            Commands = new List<CSCommand>();

            LoadEnums(spec);
            LoadCommands(spec);
        }

        void LoadCommands(Spec spec) {
            foreach (var c in spec.Commands) {
                if (!spec.IncludedCommands.Contains(c.Name)) continue;

                Commands.Add(new CSCommand(c));
            }
        }

        void LoadEnums(Spec spec) {
            foreach (var g in spec.Groups) {
                List<string> includedValues = new List<string>();

                foreach (var n in g.EnumNames) {
                    if (spec.IncludedEnums.Contains(n)) {
                        includedValues.Add(n);
                    }
                }

                if (includedValues.Count > 0) {
                    Enums.Add(new CSEnum(g.Name, includedValues, spec));
                }
            }
        }

        public void AddEnum(string name, params string[] eNames) {
            List<string> names = new List<string>(eNames);
            Enums.Add(new CSEnum(name, names, spec));
        }
    }
}
