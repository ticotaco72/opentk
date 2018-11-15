using System;
using osuTK.Graphics;
using osuTK.Platform;

namespace osuTK.Android
{
    public class AndroidGraphicsContext : IGraphicsContext, IGraphicsContextInternal
    {
        internal AndroidGraphicsContext(ContextHandle h)
        {
            // TODO: assign handle for existing context
        }

        internal AndroidGraphicsContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext sharedContext, int major, int minor, GraphicsContextFlags flags)
        {
            // TODO: create new opengles context
        }

        // TODO: check whether the context is current
        public bool IsCurrent => throw new NotImplementedException();

        // TODO: check whether the context is disposed
        public bool IsDisposed => throw new NotImplementedException();

        public int SwapInterval
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public GraphicsMode GraphicsMode => throw new NotImplementedException();

        public bool ErrorChecking
        {
            get => false;
            set { }
        }

        public IGraphicsContext Implementation => this;

        public ContextHandle Context { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // TODO: free opengles context
        }

        ~AndroidGraphicsContext()
        {
            Dispose(false);
        }

        public IntPtr GetAddress(string function)
        {
            // TODO: get opengles function address
            return IntPtr.Zero;
        }

        public IntPtr GetAddress(IntPtr function)
        {
            // TODO: get opengles function address
            return IntPtr.Zero;
        }

        public void LoadAll()
        {
        }

        public void MakeCurrent(IWindowInfo window)
        {
            // TODO: make opengles context current
            throw new NotImplementedException();
        }

        public void SwapBuffers()
        {
            // TODO: swap/present renderbuffer
            throw new NotImplementedException();
        }

        public void Update(IWindowInfo window)
        {
            // TODO: not sure if necessary for android
            throw new NotSupportedException();
        }
    }
}
