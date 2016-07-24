using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGenerator {
    public class EnumList {
        public string Name { get; private set; }
        //public string Group { get; private set; }
        //public string Namespace { get; private set; }
        //public string Vendor { get; private set; }
        public List<Enum> Values { get; private set; }

        public EnumList(string name) {
            Name = name;
            Values = new List<Enum>();
        }
    }

    public class Enum {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public Enum(string name, string value) {
            Name = name;
            Value = value;
        }
    }
}
