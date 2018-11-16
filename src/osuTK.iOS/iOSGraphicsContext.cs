using System;
using ObjCRuntime;
using OpenGLES;
using osuTK.Graphics;
using osuTK.Graphics.ES30;
using osuTK.Platform;
using osuTK.Platform.MacOS;

namespace osuTK.iOS
{
    public class iOSGraphicsContext : EmbeddedGraphicsContext, IGraphicsContextInternal
    {
        private const string gles = "/System/Library/Frameworks/OpenGLES.framework/OpenGLES";
        private static Dylib glesLibrary = Dylib.Load(gles);

        public EAGLContext EAGLContext { get; private set; }

        internal iOSGraphicsContext(ContextHandle h)
        {
            EAGLContext = (EAGLContext)Runtime.GetNSObject(h.Handle);
            Handle = new ContextHandle(EAGLContext.Handle);
        }

        internal iOSGraphicsContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext sharedContext, int major, int minor, GraphicsContextFlags flags)
        {
            // ignore mode, window
            iOSGraphicsContext shared = sharedContext as iOSGraphicsContext;

            EAGLRenderingAPI version = 0;
            if (major == 1)
                version = EAGLRenderingAPI.OpenGLES1;
            else if (major == 2)
                version = EAGLRenderingAPI.OpenGLES2;
            else if (major == 3)
                version = EAGLRenderingAPI.OpenGLES3;
            else
                throw new ArgumentException(string.Format("Unsupported GLES version {0}.{1}.", major, minor));

            EAGLContext = shared != null && shared.EAGLContext != null
                ? new EAGLContext(version, shared.EAGLContext.ShareGroup)
                : new EAGLContext(version);
            Handle = new ContextHandle(EAGLContext.Handle);
        }

        public override void SwapBuffers()
        {
            if (!EAGLContext.PresentRenderBuffer((uint)All.Renderbuffer))
                throw new InvalidOperationException("EAGLContext.PresentRenderbuffer failed.");
        }

        public override void MakeCurrent(IWindowInfo window)
        {
            if (!EAGLContext.SetCurrentContext(EAGLContext))
                throw new InvalidOperationException("Unable to change current EAGLContext.");
        }

        public override bool IsCurrent => EAGLContext.CurrentContext == EAGLContext;

        public override void Update(IWindowInfo window) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;
            if (EAGLContext != null)
                EAGLContext.Dispose();
            EAGLContext = null;
            IsDisposed = true;
        }

        ~iOSGraphicsContext()
        {
            Dispose(false);
        }

        public override int SwapInterval
        {
            get => 0;
            set { }
        }

        public override IntPtr GetAddress(IntPtr function) => glesLibrary.GetAddress(function);
    }
}
