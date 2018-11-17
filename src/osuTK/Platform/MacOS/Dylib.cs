using System;
using System.Runtime.InteropServices;

namespace osuTK.Platform.MacOS
{
    public class Dylib
    {
        public static Dylib Load(string name)
        {
            IntPtr handle = NS.dlopen(name, 2);
            if (handle == IntPtr.Zero)
                return null;
            return new Dylib(handle);
        }

        public IntPtr Handle { get; private set; }

        private Dylib(IntPtr handle)
        {
            Handle = handle;
        }

        ~Dylib()
        {
            NS.dlclose(Handle);
        }

        public IntPtr GetAddress(string name) => NS.dlsym(Handle, name);

        public IntPtr GetAddress(IntPtr function) => NS.dlsym(Handle, function);

        public T GetFunction<T>(string name)
        {
            IntPtr ptr = GetAddress(name);
            if (ptr == IntPtr.Zero)
                throw new InvalidOperationException($"Unknown function '{name}'");
            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }
    }
}
