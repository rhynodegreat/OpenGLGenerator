using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OpenGL.Managed.{0}_{1}_{2}_{3} {{
    public static partial class GL {{
        const string[] names = {{{4}}};

        public static void Load(Func<string, IntPtr> LOADFUNC) {{
            Type type = typeof(OpenGL.Unmanaged.GL);
            foreach (var name in names) {{
                FieldInfo field = type.GetField(name);
                IntPtr ptr = LOADFUNC(name);

                if (ptr == IntPtr.Zero) throw new OpenGLException(string.Format("Could not load OpenGL function '{0}'", name));

                Delegate del = Marshal.GetDelegateForFunctionPointer(ptr, field.FieldType);
                field.SetValue(null, del);
            }}
        }}
    }}

    public class OpenGLException : Exception {{
        public OpenGLException(string message) : base(message) {{ }}
    }}
}}
