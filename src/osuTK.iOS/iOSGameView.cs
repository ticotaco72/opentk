// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using System;
using System.ComponentModel;
using System.Drawing;
using CoreAnimation;
using Foundation;
using ObjCRuntime;
using OpenGLES;
using osuTK;
using osuTK.Graphics;
using osuTK.Graphics.ES30;
using osuTK.Input;
using osuTK.Platform;
using UIKit;
using FramebufferAttachment = osuTK.Graphics.ES30.FramebufferAttachment;
using FramebufferTarget = osuTK.Graphics.ES30.FramebufferTarget;
using GL = osuTK.Graphics.ES30.GL;
using RectangleF = System.Drawing.RectangleF;
using RenderbufferTarget = osuTK.Graphics.ES30.RenderbufferTarget;

namespace osuTK.iOS
{
    public class iOSGameView : UIView, IGameWindow
    {
        public int FrameBuffer { get; private set; }
        public int RenderBuffer { get; private set; }

        private bool disposed;

        // EAGL

        private EAGLContext eaglContext;

        public IGraphicsContext GraphicsContext { get; private set; }

        public virtual bool LayerRetainsBacking => false;

        public virtual NSString LayerColorFormat => EAGLColorFormat.RGBA8;

        public virtual EAGLRenderingAPI ContextRenderingAPI => EAGLRenderingAPI.OpenGLES3;

        // UIKit

        [Export("initWithCoder:")]
        public iOSGameView(NSCoder coder)
            : base(coder)
        {
        }

        [Export("initWithFrame:")]
        public iOSGameView(RectangleF frame)
            : base(frame)
        {
            initaliseGLES();
        }

        [Export("layerClass")]
        public static Class GetLayerClass()
        {
            return new Class(typeof(CAEAGLLayer));
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (eaglContext == null)
                return;

            var bounds = Bounds;
            DestroyFrameBuffer();
            CreateFrameBuffer();
        }

        // OpenGL

        private void initaliseGLES()
        {
            CAEAGLLayer layer = (CAEAGLLayer)Layer;
            layer.DrawableProperties = NSDictionary.FromObjectsAndKeys(
                new NSObject[]
                {
                    NSNumber.FromBoolean(LayerRetainsBacking),
                    LayerColorFormat
                },
                new NSObject[]
                {
                    EAGLDrawableProperty.RetainedBacking,
                    EAGLDrawableProperty.ColorFormat
                });

            layer.Opaque = true;

            ExclusiveTouch = true;
            MultipleTouchEnabled = true;
            UserInteractionEnabled = true;

            eaglContext = new EAGLContext(ContextRenderingAPI);
            GraphicsContext = new iOSGraphicsContext(new ContextHandle(eaglContext.Handle));
        }

        protected virtual void CreateFrameBuffer()
        {
            CAEAGLLayer layer = (CAEAGLLayer)Layer;
            FrameBuffer = GL.GenFramebuffer();

            RenderBuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RenderBuffer);

            if (!eaglContext.RenderBufferStorage((uint)All.Renderbuffer, layer))
            {
                // error
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameBuffer);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, RenderbufferTarget.Renderbuffer, RenderBuffer);

            Size newSize = new Size((int)Math.Round(layer.Bounds.Size.Width), (int)Math.Round(layer.Bounds.Size.Height));
            Size = newSize;
        }

        protected virtual void DestroyFrameBuffer()
        {
            if (EAGLContext.CurrentContext != eaglContext)
                MakeCurrent();

            GL.DeleteFramebuffer(FrameBuffer);
            GL.DeleteRenderbuffer(RenderBuffer);
            FrameBuffer = RenderBuffer = 0;
        }

        // Helpers

        private UIViewController findViewController()
        {
            UIResponder current = this;
            while (current != null)
            {
                if (current is UIViewController)
                    return current as UIViewController;
                current = current.NextResponder;
            }
            return null;
        }

        protected virtual void OnMove(EventArgs e) => Move?.Invoke(this, e);
        protected virtual void OnResize(EventArgs e) => Resize?.Invoke(this, e);
        protected virtual void OnClosing(CancelEventArgs e) => Closing?.Invoke(this, e);
        protected virtual void OnClosed(EventArgs e) => Closed?.Invoke(this, e);
        protected virtual void OnDisposed(EventArgs e) => Disposed?.Invoke(this, e);
        protected virtual void OnIconChanged(EventArgs e) => IconChanged?.Invoke(this, e);
        protected virtual void OnTitleChanged(EventArgs e) => TitleChanged?.Invoke(this, e);
        protected virtual void OnVisibleChanged(EventArgs e) => VisibleChanged?.Invoke(this, e);
        protected virtual void OnFocusedChanged(EventArgs e) => FocusedChanged?.Invoke(this, e);
        protected virtual void OnWindowBorderChanged(EventArgs e) => WindowBorderChanged?.Invoke(this, e);
        protected virtual void OnWindowStateChanged(EventArgs e) => WindowStateChanged?.Invoke(this, e);
        protected virtual void OnKeyDown(KeyboardKeyEventArgs e) => KeyDown?.Invoke(this, e);
        protected virtual void OnKeyPress(KeyPressEventArgs e) => KeyPress?.Invoke(this, e);
        protected virtual void OnKeyUp(KeyboardKeyEventArgs e) => KeyUp?.Invoke(this, e);
        protected virtual void OnMouseLeave(EventArgs e) => MouseLeave?.Invoke(this, e);
        protected virtual void OnMouseEnter(EventArgs e) => MouseEnter?.Invoke(this, e);
        protected virtual void OnMouseDown(MouseButtonEventArgs e) => MouseDown?.Invoke(this, e);
        protected virtual void OnMouseUp(MouseButtonEventArgs e) => MouseUp?.Invoke(this, e);
        protected virtual void OnMouseMove(MouseMoveEventArgs e) => MouseMove?.Invoke(this, e);
        protected virtual void OnMouseWheel(MouseWheelEventArgs e) => MouseWheel?.Invoke(this, e);
        protected virtual void OnFileDrop(FileDropEventArgs e) => FileDrop?.Invoke(this, e);

        protected virtual void OnLoad(EventArgs e) => Load?.Invoke(this, e);
        protected virtual void OnUnload(EventArgs e) => Unload?.Invoke(this, e);

        // IDisposable
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
                DestroyFrameBuffer();
            base.Dispose(disposing);
            disposed = true;
            if (disposing)
                OnDisposed(EventArgs.Empty);
        }

        // INativeWindow

        public void Close() => OnClosed(EventArgs.Empty);
        public void ProcessEvents() => throw new NotSupportedException();
        public Point PointToClient(Point point) => point;
        public Point PointToScreen(Point point) => point;

        public event EventHandler<EventArgs> Move;
        public event EventHandler<EventArgs> Resize;
        public event EventHandler<CancelEventArgs> Closing;
        public event EventHandler<EventArgs> Closed;
        public event EventHandler<EventArgs> Disposed;
        public event EventHandler<EventArgs> IconChanged;
        public event EventHandler<EventArgs> TitleChanged;
        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> FocusedChanged;
        public event EventHandler<EventArgs> WindowBorderChanged;
        public event EventHandler<EventArgs> WindowStateChanged;
        public event EventHandler<KeyboardKeyEventArgs> KeyDown;
        public event EventHandler<KeyPressEventArgs> KeyPress;
        public event EventHandler<KeyboardKeyEventArgs> KeyUp;
        public event EventHandler<EventArgs> MouseLeave;
        public event EventHandler<EventArgs> MouseEnter;
        public event EventHandler<MouseButtonEventArgs> MouseDown;
        public event EventHandler<MouseButtonEventArgs> MouseUp;
        public event EventHandler<MouseMoveEventArgs> MouseMove;
        public event EventHandler<MouseWheelEventArgs> MouseWheel;
        public event EventHandler<FileDropEventArgs> FileDrop;

        public bool CursorGrabbed
        {
            get => true;
            set { }
        }

        public Icon Icon { get; set; }

        public string Title
        {
            get
            {
                var viewController = findViewController();
                if (viewController != null)
                    return viewController.Title;
                throw new NotSupportedException();
            }
            set
            {
                var viewController = findViewController();
                if (viewController != null)
                {
                    if (viewController.Title != value)
                    {
                        viewController.Title = value;
                        OnTitleChanged(EventArgs.Empty);
                    }
                }
                else
                    throw new NotSupportedException();
            }
        }

        public bool Visible
        {
            get => !Hidden;
            set
            {
                if (Hidden == value)
                    return;
                Hidden = !value;
                OnVisibleChanged(EventArgs.Empty);
            }
        }

        public bool Exists => true;

        private IWindowInfo windowInfo = Utilities.CreateDummyWindowInfo();
        public IWindowInfo WindowInfo => windowInfo;

        public WindowState WindowState { get; set; }

        public WindowBorder WindowBorder
        {
            get => WindowBorder.Hidden;
            set { }
        }

        Rectangle INativeWindow.Bounds
        {
            get => new Rectangle((int)Layer.Frame.X, (int)Layer.Frame.Y, (int)Layer.Frame.Width, (int)Layer.Frame.Height);
            set { }
        }

        public Point Location
        {
            get => new Point((int)Layer.Frame.X, (int)Layer.Frame.Y);
            set { }
        }

        public Size Size
        {
            get => new Size((int)Layer.Frame.Width, (int)Layer.Frame.Height);
            set
            {
                if (value.Width == Layer.Frame.Width && value.Height == Layer.Frame.Height)
                    return;
                // TODO: resize layer
                OnResize(EventArgs.Empty);
            }
        }

        public int X
        {
            get => (int)Layer.Frame.X;
            set { }
        }

        public int Y
        {
            get => (int)Layer.Frame.Y;
            set { }
        }

        public int Width
        {
            get => (int)Layer.Frame.Width;
            set { }
        }

        public int Height
        {
            get => (int)Layer.Frame.Height;
            set { }
        }

        public Rectangle ClientRectangle
        {
            get => new Rectangle(0, 0, (int)Layer.Bounds.Size.Width, (int)Layer.Bounds.Size.Height);
            set { }
        }

        public Size ClientSize
        {
            get => new Size((int)Layer.Frame.Width, (int)Layer.Frame.Height);
            set { }
        }

        public MouseCursor Cursor { get; set; }
        public bool CursorVisible { get; set; }

        // IGameWindow

        public void Run() => Run(60.0);

        public void Run(double updateRate)
        {
            CreateFrameBuffer();
            OnLoad(EventArgs.Empty);
        }

        public void MakeCurrent() => EAGLContext.SetCurrentContext(eaglContext);

        public void SwapBuffers()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RenderBuffer);
            eaglContext.PresentRenderBuffer((uint)All.Renderbuffer);
        }

        public event EventHandler<EventArgs> Load;
        public event EventHandler<EventArgs> Unload;
        public event EventHandler<FrameEventArgs> UpdateFrame;
        public event EventHandler<FrameEventArgs> RenderFrame;


    }
}
