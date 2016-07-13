using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using OpenGLGenerator;

namespace GeneratorTest {
    class Program {
        static void Main(string[] args) {
            Spec spec;
            using (var fs = File.OpenRead("gl.xml")) {
                XmlDocument doc = new XmlDocument();
                doc.Load(fs);
                spec = new Spec(doc);
            }
            Console.WriteLine("Done loading");
            CSGenerator gen = new CSGenerator(spec);
            gen.Generate();
            Console.ReadLine();
        }
    }
}
