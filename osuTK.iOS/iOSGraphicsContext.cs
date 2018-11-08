using System;
using ObjCRuntime;
using OpenGLES;
using osuTK.Graphics;
using osuTK.Graphics.ES30;
using osuTK.Platform;

namespace osuTK.iOS
{
    public class iOSGraphicsContext : IGraphicsContext, IGraphicsContextInternal
    {
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
            contextHandle = new ContextHandle(EAGLContext.Handle);
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

        public bool IsCurrent
        {
            get { return EAGLContext.CurrentContext == EAGLContext; }
        }

        public bool IsDisposed
        {
            get { return EAGLContext == null; }
        }

        public bool VSync
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public void Update(IWindowInfo window)
        {
            throw new NotSupportedException();
        }

        public GraphicsMode GraphicsMode { get; private set; }

        public bool ErrorChecking
        {
            get { return false; }
            set { }
        }

        IGraphicsContext IGraphicsContextInternal.Implementation
        {
            get { return this; }
        }

        public void LoadAll()
        {
        }

        ContextHandle contextHandle;
        ContextHandle IGraphicsContextInternal.Context
        {
            get { return contextHandle; }
        }

        IntPtr IGraphicsContextInternal.GetAddress(string function)
        {
            return IntPtr.Zero;
        }

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

        public IntPtr GetAddress(IntPtr function) => IntPtr.Zero;
    }
}
