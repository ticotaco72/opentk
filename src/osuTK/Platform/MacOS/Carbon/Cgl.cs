//
// Cgl.cs
//
// Author:
//       Stefanos A. <stapostol@gmail.com>
//
// Copyright (c) 2006-2014 Stefanos Apostolopoulos
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using System;
using System.Runtime.InteropServices;

namespace osuTK.Platform.MacOS
{
    using CGLPixelFormat = IntPtr;
    using CGLContext = IntPtr;

    internal static class Cgl
    {
        internal enum PixelFormatBool
        {
            None = 0,
            AllRenderers = 1,
            Doublebuffer = 5,
            Stereo = 6,
            AuxBuffers = 7,
            MinimumPolicy = 51,
            MaximumPolicy = 52,
            Offscreen = 53,
            AuxDepthStencil = 57,
            ColorFloat = 58,
            Multisample = 59,
            Supersample = 60,
            SampleALpha = 61,
            SingleRenderer = 71,
            NoRecovery = 72,
            Accelerated = 73,
            ClosestPolicy = 74,
            BackingStore = 76,
            Window = 80,
            Compliant = 83,
            PBuffer = 90,
            RemotePBuffer = 91,
        }

        internal enum PixelFormatInt
        {
            ColorSize = 8,
            AlphaSize = 11,
            DepthSize = 12,
            StencilSize = 13,
            AccumSize = 14,
            SampleBuffers = 55,
            Samples = 56,
            RendererID = 70,
            DisplayMask = 84,
            OpenGLProfile = 99,
            VScreenCount = 128,
        }

        internal enum OpenGLProfileVersion
        {
            Legacy = 0x100,
            Core3_2 = 0x3200,
        }

        internal enum ParameterNames
        {
            SwapInterval = 222,
        }

        internal enum Error
        {
            None = 0x000,
        }

        // OpenGL

        private const string cgl = "/System/Library/Frameworks/OpenGL.framework/Versions/Current/OpenGL";
        private static readonly Dylib cglLibrary = Dylib.Load(cgl);

        private delegate Error CGLGetErrorDelegate();
        private static readonly CGLGetErrorDelegate cglGetError = cglLibrary?.GetFunction<CGLGetErrorDelegate>("glGetError");
        internal static Error GetError() => cglGetError();

        private delegate IntPtr CGLErrorStringDelegate(Error code);
        private static readonly CGLErrorStringDelegate cglErrorString = cglLibrary?.GetFunction<CGLErrorStringDelegate>("CGLErrorString");
        internal static string ErrorString(Error code) => Marshal.PtrToStringAnsi(cglErrorString(code));

        private delegate Error CGLChoosePixelFormatDelegate(int[] attribs, ref CGLPixelFormat format, ref int numPixelFormats);
        private static readonly CGLChoosePixelFormatDelegate cglChoosePixelFormat = cglLibrary?.GetFunction<CGLChoosePixelFormatDelegate>("CGLChoosePixelFormat");
        internal static Error ChoosePixelFormat(int[] attribs, ref CGLPixelFormat format, ref int numPixelFormats) => cglChoosePixelFormat(attribs, ref format, ref numPixelFormats);

        private delegate Error CGLDescribePixelFormatIntDelegate(CGLPixelFormat pix, int pix_num, PixelFormatInt attrib, out int value);
        private static readonly CGLDescribePixelFormatIntDelegate cglDescribePixelFormatInt = cglLibrary?.GetFunction<CGLDescribePixelFormatIntDelegate>("CGLDescribePixelFormat");
        internal static Error DescribePixelFormat(CGLPixelFormat pix, int pix_num, PixelFormatInt attrib, out int value) => cglDescribePixelFormatInt(pix, pix_num, attrib, out value);

        private delegate Error CGLDescribePixelFormatBoolDelegate(CGLPixelFormat pix, int pix_num, PixelFormatBool attrib, out bool value);
        private static readonly CGLDescribePixelFormatBoolDelegate cglDescribePixelFormatBool = cglLibrary?.GetFunction<CGLDescribePixelFormatBoolDelegate>("CGLDescribePixelFormat");
        internal static Error DescribePixelFormat(CGLPixelFormat pix, int pix_num, PixelFormatBool attrib, out bool value) => cglDescribePixelFormatBool(pix, pix_num, attrib, out value);

        private delegate CGLPixelFormat CGLGetPixelFormatDelegate(CGLContext context);
        private static readonly CGLGetPixelFormatDelegate cglGetPixelFormat = cglLibrary?.GetFunction<CGLGetPixelFormatDelegate>("CGLGetPixelFormat");
        internal static CGLPixelFormat GetPixelFormat(CGLContext context) => cglGetPixelFormat(context);

        private delegate Error CGLCreateContextDelegate(CGLPixelFormat format, CGLContext share, ref CGLContext context);
        private static readonly CGLCreateContextDelegate cglCreateContext = cglLibrary?.GetFunction<CGLCreateContextDelegate>("CGLCreateContext");
        internal static Error CreateContext(CGLPixelFormat format, CGLContext share, ref CGLContext context) => cglCreateContext(format, share, ref context);

        private delegate Error CGLDestroyPixelFormatDelegate(CGLPixelFormat format);
        private static readonly CGLDestroyPixelFormatDelegate cglDestroyPixelFormat = cglLibrary?.GetFunction<CGLDestroyPixelFormatDelegate>("CGLDestroyPixelFormat");
        internal static Error DestroyPixelFormat(CGLPixelFormat format) => cglDestroyPixelFormat(format);

        private delegate CGLContext CGLGetCurrentContextDelegate();
        private static readonly CGLGetCurrentContextDelegate cglGetCurrentContext = cglLibrary?.GetFunction<CGLGetCurrentContextDelegate>("CGLGetCurrentContext");
        internal static CGLContext GetCurrentContext() => cglGetCurrentContext();

        private delegate Error CGLSetCurrentContextDelegate(CGLContext context);
        private static readonly CGLSetCurrentContextDelegate cglSetCurrentContext = cglLibrary?.GetFunction<CGLSetCurrentContextDelegate>("CGLSetCurrentContext");
        internal static Error SetCurrentContext(CGLContext context) => cglSetCurrentContext(context);

        private delegate Error CGLDestroyContextDelegate(CGLContext context);
        private static readonly CGLDestroyContextDelegate cglDestroyContext = cglLibrary?.GetFunction<CGLDestroyContextDelegate>("CGLDestroyContext");
        internal static Error DestroyContext(CGLContext context) => cglDestroyContext(context);

        private delegate Error CGLSetParameterDelegate(CGLContext context, int parameter, ref int value);
        private static readonly CGLSetParameterDelegate cglSetParameter = cglLibrary?.GetFunction<CGLSetParameterDelegate>("CGLSetParameter");
        internal static Error SetParameter(CGLContext context, int parameter, ref int value) => cglSetParameter(context, parameter, ref value);

        private delegate Error CGLFlushDrawableDelegate(CGLContext context);
        private static readonly CGLFlushDrawableDelegate cglFlushDrawable = cglLibrary?.GetFunction<CGLFlushDrawableDelegate>("CGLFlushDrawable");
        internal static Error FlushDrawable(CGLContext context) => cglFlushDrawable(context);

        private delegate Error CGLSetSurfaceDelegate(CGLContext context, int conId, int winId, int surfId);
        private static readonly CGLSetSurfaceDelegate cglSetSurface = cglLibrary?.GetFunction<CGLSetSurfaceDelegate>("CGLSetSurface");
        internal static Error SetSurface(CGLContext context, int conId, int winId, int surfId) => cglSetSurface(context, conId, winId, surfId);

        private delegate Error CGLUpdateContextDelegate(CGLContext context);
        private static readonly CGLUpdateContextDelegate cglUpdateContext = cglLibrary?.GetFunction<CGLUpdateContextDelegate>("CGLUpdateContext");
        internal static Error UpdateContext(CGLContext context) => cglUpdateContext(context);

        // Carbon

        private const string cgs = "/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon";
        private static readonly Dylib cgsLibrary = Dylib.Load(cgs);

        private delegate int CGSMainConnectionIDDelegate();
        private static readonly CGSMainConnectionIDDelegate cgsMainConnection = cgsLibrary?.GetFunction<CGSMainConnectionIDDelegate>("CGSMainConnectionID");
        internal static int MainConnectionID() => cgsMainConnection();

        private delegate Error CGSGetSurfaceCountDelegate(int conId, int winId, ref int count);
        private static readonly CGSGetSurfaceCountDelegate cgsGetSurfaceCount = cgsLibrary?.GetFunction<CGSGetSurfaceCountDelegate>("CGSGetSurfaceCount");
        internal static Error GetSurfaceCount(int conId, int winId, ref int count) => cgsGetSurfaceCount(conId, winId, ref count);

        private delegate Error CGSGetSurfaceListDelegate(int conId, int winId, int count, ref int ids, ref int filled);
        private static readonly CGSGetSurfaceListDelegate cgsGetSurfaceList = cgsLibrary?.GetFunction<CGSGetSurfaceListDelegate>("CGSGetSurfaceList");
        internal static Error GetSurfaceList(int conId, int winId, int count, ref int ids, ref int filled) => cgsGetSurfaceList(conId, winId, count, ref ids, ref filled);
    }
}
