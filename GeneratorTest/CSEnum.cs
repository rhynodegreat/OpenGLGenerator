using System;
using System.Collections.Generic;
using System.Text;

using OpenGLGenerator;

namespace GeneratorTest {
    public class CSEnum {
        public string Name { get; set; }
        public List<string> Names { get; set; }
        public List<string> Values { get; set; }

        public CSEnum(string name, List<string> names, Spec spec) {
            Name = name;
            Names = names;

            Values = new List<string>(Names.Count);

            for (int i = 0; i < this.Names.Count; i++) {
                Values.Add(spec.EnumMap[Names[i]].Value);
                Names[i] = ToCamelCase(Names[i]);
            }
        }

        string ToCamelCase(string input) {
            StringBuilder builder = new StringBuilder();
            string[] tokens = input.Split('_');
            int start = 0;
            if (tokens[0] == "GL") start = 1;

            for (int i = start; i < tokens.Length; i++) {
                char first = tokens[i][0];
                if (char.IsDigit(first)) {
                    builder.Append('_');
                }

                if (tokens[i].Contains("RG")) {
                    builder.Append(tokens[i]);
                } else {
                    builder.Append(first);
                    builder.Append(tokens[i].Substring(1).ToLower());
                }
            }

            return builder.ToString();
        }
    }
}
