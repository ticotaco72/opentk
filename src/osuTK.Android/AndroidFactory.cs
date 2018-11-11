using System;
using osuTK.Graphics;
using osuTK.Input;
using osuTK.Platform;

namespace osuTK.Android
{
    public class AndroidFactory : PlatformFactoryBase
    {
        public override IGraphicsContext CreateGLContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags) =>
            new AndroidGraphicsContext(mode, window, shareContext, major, minor, flags);

        public override IGraphicsContext CreateGLContext(ContextHandle handle, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags) =>
            new AndroidGraphicsContext(handle);

        public override GraphicsContext.GetCurrentContextDelegate CreateGetCurrentGraphicsContext()
        {
            return () => {
                // TODO: return a handle to the current context
                throw new NotImplementedException();
            };
        }

        public override INativeWindow CreateNativeWindow(int x, int y, int width, int height, string title, GraphicsMode mode, GameWindowFlags options, DisplayDevice device) => throw new NotImplementedException();

        public override IDisplayDeviceDriver CreateDisplayDeviceDriver() => new AndroidDisplayDeviceDriver();

        public override IKeyboardDriver2 CreateKeyboardDriver() => throw new NotImplementedException();

        public override IMouseDriver2 CreateMouseDriver() => throw new NotImplementedException();

        public override IJoystickDriver2 CreateJoystickDriver() => throw new NotImplementedException();
    }
}
