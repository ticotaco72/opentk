using System;
using OpenGLES;
using osuTK.Graphics;
using osuTK.Input;
using osuTK.Platform;

namespace osuTK.iOS
{
    public class iOSFactory : PlatformFactoryBase
    {
        public override IGraphicsContext CreateGLContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
        {
            return new iOSGraphicsContext(mode, window, shareContext, major, minor, flags);
        }

        public override IGraphicsContext CreateGLContext(ContextHandle handle, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
        {
            return new iOSGraphicsContext(handle);
        }

        public override GraphicsContext.GetCurrentContextDelegate CreateGetCurrentGraphicsContext()
        {
            return () => {
                EAGLContext c = EAGLContext.CurrentContext;
                IntPtr h = IntPtr.Zero;
                if (c != null)
                {
                    h = c.Handle;
                }
                return new ContextHandle(h);
            };
        }

        public override INativeWindow CreateNativeWindow(int x, int y, int width, int height, string title, GraphicsMode mode, GameWindowFlags options, DisplayDevice device)
        {
            throw new NotImplementedException();
        }

        public override IDisplayDeviceDriver CreateDisplayDeviceDriver()
        {
            return new iOSDisplayDeviceDriver();
        }

        public override IKeyboardDriver2 CreateKeyboardDriver()
        {
            throw new NotImplementedException();
        }

        public override IMouseDriver2 CreateMouseDriver()
        {
            throw new NotImplementedException();
        }

        public override IJoystickDriver2 CreateJoystickDriver()
        {
            throw new NotImplementedException();
        }

    }
}
