using System;
using System.Collections.Generic;
using System.Xml;

namespace OpenGLGenerator {
    public class Spec {
        public List<Group> Groups { get; private set; }

        public List<EnumList> Enums { get; private set; }
        public Dictionary<string, Enum> EnumMap { get; private set; }

        public List<Command> Commands { get; private set; }
        public Dictionary<string, Command> CommandMap { get; private set; }

        public List<Features> Features { get; private set; }

        public Spec(XmlDocument doc) {
            Groups = new List<Group>();

            Enums = new List<EnumList>();
            EnumMap = new Dictionary<string, Enum>();

            Commands = new List<Command>();
            CommandMap = new Dictionary<string, Command>();

            Features = new List<Features>();

            XmlNode root = doc.ChildNodes[1];
            Load(root);
        }

        void Load(XmlNode root) {
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
                    case "feature":
                        AddFeatures(node);
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
            Enums.Add(list);

            for (int i = 0; i < root.ChildNodes.Count; i++) {
                XmlNode node = root.ChildNodes[i];
                if (node is XmlComment) continue;
                if (node.Name == "unused") continue;
                string eName = node.Attributes["name"].Value;
                string eValue = node.Attributes["value"].Value;
                Enum e = new Enum(eName, eValue);
                list.Values.Add(e);
                if (EnumMap.ContainsKey(eName)) {
                    if (!(EnumMap[eName].Value == e.Value)) {
                        Console.WriteLine("ENUM DEFINED TWICE: {0}", eName);
                    }
                } else {
                    string api = node.Attributes["api"]?.Value;
                    if (api == null || api == "gl") {
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

            XmlNode ptype = node["ptype"];
            XmlAttribute len = node.Attributes["len"];
            if (ptype == null || (len != null)) {
                type = "void*";
            } else {
                type = ptype.InnerText;
            }
            return new Parameter(name, type, group);
        }

        void AddFeatures(XmlNode root) {
            string api = root.Attributes["api"].Value;
            string version = root.Attributes["name"].Value;
            string number = root.Attributes["number"].Value;

            Features features = new Features(api, version, number);
            for (int i = 0; i < root.ChildNodes.Count; i++) {
                XmlNode action = root.ChildNodes[i];
                if (action is XmlComment) continue;
                string name = action.Name;
                string profile = action.Attributes["profile"]?.Value;
                FeatureList fl = new FeatureList(name, profile);
                AddFeatures(action, fl);
                features.Lists.Add(fl);
            }

            Features.Add(features);
        }

        void AddFeatures(XmlNode node, FeatureList list) {
            for (int i = 0; i < node.ChildNodes.Count; i++) {
                XmlNode feature = node.ChildNodes[i];
                if (feature is XmlComment) continue;
                string name = feature.Attributes["name"].Value;
                FeatureType type;
                if (feature.Name == "command") {
                    type = FeatureType.Command;
                } else {
                    type = FeatureType.Enum;
                }
                list.List.Add(new Feature(type, name));
            }
        }
    }
}
