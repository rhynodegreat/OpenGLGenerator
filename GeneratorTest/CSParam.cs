using System;
using System.Collections.Generic;

using OpenGLGenerator;

namespace GeneratorTest {
    public class CSParam {
        public string Name { get; set; }
        public string Type { get; set; }
        public ParamType ParamType { get; set; } = ParamType.Normal;
        public CSCommand Command { get; set; }
        public int HardcodedLength { get; set; } = -1;

        string group;
        public string Group {
            get {
                return group;
            }
            set {
                Type = value;
                group = value;
            }
        }

        public CSParam(CSCommand command, Parameter par) {
            Command = command;
            Name = par.Name;
            Group = par.Group;

            if (par.Pointer) {
                ConfigPointer(par);
            } else {
                Type = GetType(par.Type);
            }
        }

        void ConfigPointer(Parameter par) {
            if (par.Type.Contains("GLchar*")) {
                if (par.PointerLevels == 1) {
                    if (par.Const) {
                        ParamType = ParamType.Normal;
                        Type = "string";
                    } else {
                        ParamType = ParamType.Normal;
                        Type = "StringBuilder";
                    }
                } else {
                    ParamType = ParamType.Array;
                    Type = "string";
                }
                return;
            }
            string len = par.Len;
            if (len != null) {
                ParamType = ParamType.Array;
                Type = GetType(par.Type.Substring(0, par.Type.Length - 1));

                int size;
                if (int.TryParse(len, out size)) {
                    HardcodedLength = size;
                    if (HardcodedLength == 1) {
                        Type = GetType(par.Type.Substring(0, par.Type.Length - 1));
                        ParamType = ParamType.Ref;
                    }
                }
            } else {
                if (par.Const) {
                    ParamType = ParamType.Ref;
                    Type = GetType(par.Type.Substring(0, par.Type.Length - 1));
                } else {
                    ParamType = ParamType.Out;
                    Type = GetType(par.Type.Substring(0, par.Type.Length - 1));
                }
            }
        }

        string GetType(string input) {
            switch (input) {
                case "GLbyte": return "sbyte";
                case "GLshort": return "short";
                case "GLint": return "int";
                case "GLint64": return "long";
                case "GLfloat": return "float";
                case "GLboolean": return "bool";
                case "GLubyte": return "byte";
                case "GLushort": return "ushort";
                case "GLuint": return "uint";
                case "GLuint64": return "ulong";
                case "GLdouble": return "double";
                case "void*": return "IntPtr";
                case "GLsizei": return "int";
                case "GLintptr": return "int";
                case "GLsizeiptr": return "int";
                case "GLenum": {
                        if (Group != null) {
                            return Group;
                        } else {
                            return "uint";
                        }
                    }
                case "GLbitfield": {
                        if (Group != null) {
                            return Group;
                        } else {
                            return "uint";
                        }
                    }
                default:
                    return input;
            }
        }
    }

    public enum ParamType {
        Normal,
        Ref,
        Out,
        Array
    }
}
