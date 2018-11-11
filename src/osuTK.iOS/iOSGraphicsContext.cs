using System;
using ObjCRuntime;
using OpenGLES;
using osuTK.Graphics;
using osuTK.Graphics.ES30;
using osuTK.Platform;
using osuTK.Platform.MacOS;

namespace osuTK.iOS
{
    public class iOSGraphicsContext : IGraphicsContext, IGraphicsContextInternal
    {
        private const string gles = "/System/Library/Frameworks/OpenGLES.framework/OpenGLES";
        private static Dylib glesLibrary = Dylib.Load(gles);

        public EAGLContext EAGLContext { get; private set; }

        internal iOSGraphicsContext(ContextHandle h)
        {
            EAGLContext = (EAGLContext)Runtime.GetNSObject(h.Handle);
        }

        internal iOSGraphicsContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext sharedContext, int major, int minor, GraphicsContextFlags flags)
        {
            // ignore mode, window
            iOSGraphicsContext shared = sharedContext as iOSGraphicsContext;

            EAGLRenderingAPI version = 0;
            if (major == 1 && minor == 1)
                version = EAGLRenderingAPI.OpenGLES1;
            else if (major == 2 && minor == 0)
                version = EAGLRenderingAPI.OpenGLES2;
            else
                throw new ArgumentException(string.Format("Unsupported GLES version {0}.{1}.", major, minor));

            EAGLContext = shared != null && shared.EAGLContext != null
                ? new EAGLContext(version, shared.EAGLContext.ShareGroup)
                : new EAGLContext(version);
            Context = new ContextHandle(EAGLContext.Handle);
        }

        public void SwapBuffers()
        {
            if (!EAGLContext.PresentRenderBuffer((uint)All.Renderbuffer))
                throw new InvalidOperationException("EAGLContext.PresentRenderbuffer failed.");
        }

        public void MakeCurrent(IWindowInfo window)
        {
            if (!EAGLContext.SetCurrentContext(EAGLContext))
                throw new InvalidOperationException("Unable to change current EAGLContext.");
        }

        public bool IsCurrent => EAGLContext.CurrentContext == EAGLContext;

        public bool IsDisposed => EAGLContext == null;

        public bool VSync
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public void Update(IWindowInfo window) => throw new NotSupportedException();

        public GraphicsMode GraphicsMode { get; private set; }

        public bool ErrorChecking
        {
            get => false;
            set { }
        }

        public IGraphicsContext Implementation => this;

        public void LoadAll()
        {
        }

        public ContextHandle Context { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (EAGLContext != null)
                EAGLContext.Dispose();
            EAGLContext = null;
        }

        ~iOSGraphicsContext()
        {
            Dispose(false);
        }

        public int SwapInterval
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public IntPtr GetAddress(string function) => glesLibrary.GetAddress(function);

        public IntPtr GetAddress(IntPtr function) => glesLibrary.GetAddress(function);
    }
}
