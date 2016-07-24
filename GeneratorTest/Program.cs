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
                spec = new Spec(doc, "gl", "core", 4, 5);
            }

            CSSpec cs = new CSSpec(spec);

            Console.WriteLine("Done loading");

            CSGenerator gen = new CSGenerator(cs);
            gen.Generate();
            Console.WriteLine("Done generating");
            Console.ReadLine();
        }
    }
}
