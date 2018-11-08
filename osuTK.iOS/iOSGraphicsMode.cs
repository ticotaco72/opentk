using System;
using osuTK.Graphics;

namespace osuTK.iOS
{
    public class iOSGraphicsMode : IGraphicsMode
    {
        public GraphicsMode SelectGraphicsMode(ColorFormat color, int depth, int stencil, int samples, ColorFormat accum, int buffers, bool stereo)
        {
            return new GraphicsMode((IntPtr)1, color, depth, stencil, samples, accum, buffers, stereo);
        }
    }
}
