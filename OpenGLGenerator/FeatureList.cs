using System;
using System.Collections.Generic;

namespace OpenGLGenerator {
    public class Features {
        public string API { get; set; }
        public string Name { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }
        public List<FeatureList> Lists { get; set; }

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
        public string Profile { get; set; }
        public string Action { get; set; }
        public List<Feature> List { get; set; }

        public FeatureList(string action,string profile) {
            Profile = profile;
            Action = action;
            List = new List<Feature>();
        }
    }

    public class Feature {
        public FeatureType Type { get; set; }
        public string Name { get; set; }

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
