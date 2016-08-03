using System;
using System.Collections.Generic;
using System.Xml;

namespace OpenGLGenerator {
    public class Spec {
        public List<Group> Groups { get; private set; }
        
        public Dictionary<string, Enum> EnumMap { get; private set; }

        public List<Command> Commands { get; private set; }
        public Dictionary<string, Command> CommandMap { get; private set; }

        public string API { get; private set; }
        public string Profile { get; private set; }
        public int Major { get; private set; }
        public int Minor { get; private set; }

        public HashSet<string> IncludedEnums { get; private set; }
        public HashSet<string> IncludedCommands { get; private set; }

        public Spec(XmlDocument doc, string api, string profile, int major, int minor) {
            API = api;
            Profile = profile;
            Major = major;
            Minor = minor;

            Groups = new List<Group>();
            
            EnumMap = new Dictionary<string, Enum>();

            Commands = new List<Command>();
            CommandMap = new Dictionary<string, Command>();

            IncludedCommands = new HashSet<string>();
            IncludedEnums = new HashSet<string>();

            XmlNode root = doc.ChildNodes[1];
            Load(root);
        }

        void ResolveFeatures(XmlNode root) {
            for (int i = 0; i < root.ChildNodes.Count; i++) {
                var node = root.ChildNodes[i];

                if (node.Name != "feature") continue;
                if (node.Attributes["api"].Value != API) continue;
                string[] tokens = node.Attributes["number"].Value.Split('.');
                int maj = int.Parse(tokens[0]);
                int min = int.Parse(tokens[1]);
                if ((min > Minor && maj == Major) || maj > Major) {
                    continue;
                }

                for (int j = 0; j < node.ChildNodes.Count; j++) {
                    var r = node.ChildNodes[j];
                    if (r is XmlComment) continue;
                    string profile = r.Attributes["profile"]?.Value;

                    if (profile != null && profile != Profile) continue;

                    for (int k = 0; k  < r.ChildNodes.Count; k++) {
                        var f = r.ChildNodes[k];

                        if (f is XmlComment) continue;

                        if (r.Name == "require") {
                            if (f.Name == "enum") {
                                IncludedEnums.Add(f.Attributes["name"].Value);
                            } else if (f.Name == "command") {
                                IncludedCommands.Add(f.Attributes["name"].Value);
                            }
                        } else {
                            if (f.Name == "enum") {
                                IncludedEnums.Remove(f.Attributes["name"].Value);
                            } else if (f.Name == "command") {
                                IncludedCommands.Remove(f.Attributes["name"].Value);
                            }
                        }
                    }
                }
            }
        }

        void Load(XmlNode root) {
            ResolveFeatures(root);
            for (int i = 0; i < root.ChildNodes.Count; i++) {
                XmlNode node = root.ChildNodes[i];
                string name = node.Name;
                switch (name) {
                    case "groups":
                        LoadGroups(node);
                        break;
                    case "enums":
                        AddEnums(node);
                        break;
                    case "commands":
                        LoadCommands(node);
                        break;
                }
            }
        }

        void LoadGroups(XmlNode root) {
            for (int i = 0; i < root.ChildNodes.Count; i++) {
                XmlNode node = root.ChildNodes[i];
                string groupName = node.Attributes["name"].Value;
                Group group = new Group(groupName);

                for (int j = 0; j < node.ChildNodes.Count; j++) {
                    XmlNode e = node.ChildNodes[j];
                    string eName = e.Attributes["name"].Value;
                    group.EnumNames.Add(eName);
                }

                Groups.Add(group);
            }
        }

        void AddEnums(XmlNode root) {
            if (root.ChildNodes.Count == 0) return;

            string group = root.Attributes["group"]?.Value;
            string vendor = root.Attributes["vendor"]?.Value;
            string _namespace = root.Attributes["namespace"]?.Value;
            
            EnumList list = new EnumList(group);

            for (int i = 0; i < root.ChildNodes.Count; i++) {
                XmlNode node = root.ChildNodes[i];
                if (node is XmlComment) continue;
                if (node.Name == "unused") continue;

                string eName = node.Attributes["name"].Value;

                if (!IncludedEnums.Contains(eName)) continue;

                string eValue = node.Attributes["value"].Value;
                Enum e = new Enum(eName, eValue);
                list.Values.Add(e);
                if (!EnumMap.ContainsKey(eName)) {
                    string api = node.Attributes["api"]?.Value;
                    if (api == null || api == API) {
                        EnumMap.Add(eName, e);
                    }
                }
            }
        }

        void LoadCommands(XmlNode root) {
            for (int i = 0; i < root.ChildNodes.Count; i++) {
                LoadCommand(root.ChildNodes[i]);
            }
        }

        void LoadCommand(XmlNode node) {
            string name;
            string returnType;
            string returnGroup;

            name = node["proto"]["name"].InnerText;

            XmlNode ptype = node["proto"]["ptype"];
            if (ptype == null) {
                returnType = "void";
                if (node["proto"].InnerText.Contains("*")) returnType += "*";
            } else {
                returnType = ptype.InnerText;
            }

            returnGroup = node.Attributes["group"]?.Value;

            Command command = new Command(name, returnType, returnGroup);

            for (int i = 0; i < node.ChildNodes.Count; i++) {
                XmlNode param = node.ChildNodes[i];
                if (param.Name == "param") {
                    command.Parameters.Add(LoadParameter(param));
                }
            }

            Commands.Add(command);
            CommandMap.Add(name, command);
        }

        Parameter LoadParameter(XmlNode node) {
            string name = node["name"].InnerText;
            string type;
            string group = node.Attributes["group"]?.Value;
            bool pointer = false;
            bool _const = false;
            int levels = 0;

            XmlNode ptype = node["ptype"];
            string len = node.Attributes["len"]?.Value;
            if (ptype == null) {
                type = "void*";
            } else {
                type = ptype.InnerText;
                string search = node.InnerText;
                foreach (char c in search) {
                    if (c == '*') {
                        type += c;
                        levels++;
                        pointer = true;
                    }
                }
            }
            if (node.InnerText.Contains("const")) _const = true;
            return new Parameter(name, type, group, len, pointer, _const, levels);
        }
    }
}
