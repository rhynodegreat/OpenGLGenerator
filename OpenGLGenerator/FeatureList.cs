using System;
using System.Collections.Generic;

namespace OpenGLGenerator {
    public class Features {
        public string API { get; private set; }
        public string Name { get; private set; }
        public int VersionMajor { get; private set; }
        public int VersionMinor { get; private set; }
        public List<FeatureList> Lists { get; private set; }

        public Features(string api, string name, string number) {
            API = api;
            Name = name;
            string[] tokens = number.Split('.');
            VersionMajor = int.Parse(tokens[0]);
            VersionMinor = int.Parse(tokens[1]);
            Lists = new List<FeatureList>();
        }
    }

    public class FeatureList {
        public string Profile { get; private set; }
        public string Action { get; private set; }
        public List<Feature> List { get; private set; }

        public FeatureList(string action,string profile) {
            Profile = profile;
            Action = action;
            List = new List<Feature>();
        }
    }

    public class Feature {
        public FeatureType Type { get; private set; }
        public string Name { get; private set; }

        public Feature(FeatureType type, string name) {
            Type = type;
            Name = name;
        }
    }

    public enum FeatureType {
        Command,
        Enum
    }
}
