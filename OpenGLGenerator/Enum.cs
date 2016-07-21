using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGenerator {
    public class EnumList {
        public string Name { get; set; }
        //public string Group { get; set; }
        //public string Namespace { get; set; }
        //public string Vendor { get; set; }
        public List<Enum> Values { get; set; }

        public EnumList(string name) {
            Name = name;
            Values = new List<Enum>();
        }
    }

    public class Enum {
        public string Name { get; set; }
        public string Value { get; set; }

        public Enum(string name, string value) {
            Name = name;
            Value = value;
        }
    }
}
