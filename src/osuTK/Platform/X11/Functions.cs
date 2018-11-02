/* Licensed under the MIT/X11 license.
 * Copyright (c) 2006-2008 the osuTK Team.
 * This notice may not be removed from any source distribution.
 * See license.txt for licensing detailed licensing details.
 */

using System;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;

namespace osuTK.Platform.X11
{
    // using XID = System.Int32;
    using Window = System.IntPtr;
    using Drawable = System.IntPtr;
    using Font = System.IntPtr;
    using Pixmap = System.IntPtr;
    using Cursor = System.IntPtr;
    using Colormap = System.IntPtr;
    using GContext = System.IntPtr;
    using KeySym = System.IntPtr;
    using Mask = System.IntPtr;
    using Atom = System.IntPtr;
    using VisualID = System.IntPtr;
    using Time = System.IntPtr;
    using KeyCode = System.Byte;    // Or maybe ushort?

    using Display = System.IntPtr;
    using XPointer = System.IntPtr;

    // Randr and Xrandr
    using Bool = System.Boolean;
    using XRRScreenConfiguration = System.IntPtr; // opaque datatype
    using Rotation = System.UInt16;
    using Status = System.Int32;
    using SizeID = System.UInt16;



    internal static partial class Functions
    {
        public static readonly object Lock = API.Lock;

        private const string lib = "libX11.so.6";

        [DllImport(lib, EntryPoint = "XOpenDisplay")]
        private extern static IntPtr sys_XOpenDisplay(IntPtr display);
        public static IntPtr XOpenDisplay(IntPtr display)
        {
            lock (Lock)
            {
                return sys_XOpenDisplay(display);
            }
        }

        [DllImport(lib, EntryPoint = "XCloseDisplay")]
        public extern static int XCloseDisplay(IntPtr display);
        [DllImport(lib, EntryPoint = "XSynchronize")]
        public extern static IntPtr XSynchronize(IntPtr display, bool onoff);

        [DllImport(lib, EntryPoint = "XCreateWindow")]
        public extern static IntPtr XCreateWindow(IntPtr display, IntPtr parent, int x, int y, int width, int height, int border_width, int depth, int xclass, IntPtr visual, IntPtr valuemask, ref XSetWindowAttributes attributes);
        
        [DllImport(lib, EntryPoint = "XCreateWindow")]
        public unsafe extern static IntPtr XCreateWindow(IntPtr display, IntPtr parent, int x, int y, int width, int height, int border_width, int depth, int xclass, IntPtr visual, IntPtr valuemask, XSetWindowAttributes* attributes);

        [DllImport(lib, EntryPoint = "XCreateSimpleWindow")]//, CLSCompliant(false)]
        public extern static IntPtr XCreateSimpleWindow(IntPtr display, IntPtr parent, int x, int y, int width, int height, int border_width, UIntPtr border, UIntPtr background);
        [DllImport(lib, EntryPoint = "XCreateSimpleWindow")]
        public extern static IntPtr XCreateSimpleWindow(IntPtr display, IntPtr parent, int x, int y, int width, int height, int border_width, IntPtr border, IntPtr background);

        [DllImport(lib, EntryPoint = "XMapWindow")]
        public extern static int XMapWindow(IntPtr display, IntPtr window);
        [DllImport(lib, EntryPoint = "XUnmapWindow")]
        public extern static int XUnmapWindow(IntPtr display, IntPtr window);
        [DllImport(lib, EntryPoint = "XMapSubwindows")]
        public extern static int XMapSubindows(IntPtr display, IntPtr window);
        [DllImport(lib, EntryPoint = "XUnmapSubwindows")]
        public extern static int XUnmapSubwindows(IntPtr display, IntPtr window);
        [DllImport(lib, EntryPoint = "XRootWindow")]
        public extern static IntPtr XRootWindow(IntPtr display, int screen_number);

        [DllImport(lib, EntryPoint = "XNextEvent")]
        public extern static IntPtr XNextEvent(IntPtr display, ref XEvent xevent);
        [DllImport(lib)]
        public extern static Bool XWindowEvent(Display display, Window w, EventMask event_mask, ref XEvent event_return);
        [DllImport(lib)]
        public extern static Bool XCheckWindowEvent(Display display, Window w, EventMask event_mask, ref XEvent event_return);
        [DllImport(lib)]
        public extern static Bool XCheckTypedWindowEvent(Display display, Window w, XEventName event_type, ref XEvent event_return);
        [DllImport(lib)]
        public extern static Bool XCheckTypedEvent(Display display, XEventName event_type, out XEvent event_return);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate Bool EventPredicate(IntPtr display, ref XEvent e, IntPtr arg);
        [DllImport(lib)]
        public extern static Bool XIfEvent(Display display, ref XEvent e, IntPtr predicate, IntPtr arg );
        [DllImport(lib)]
        public extern static Bool XCheckIfEvent(Display display, ref XEvent e, IntPtr predicate, IntPtr arg );

        [DllImport(lib)]
        public extern static int XConnectionNumber(IntPtr diplay);
        [DllImport(lib)]
        public extern static int XPending(IntPtr diplay);

        [DllImport(lib, EntryPoint = "XSelectInput")]
        public extern static int XSelectInput(IntPtr display, IntPtr window, IntPtr mask);

        [DllImport(lib, EntryPoint = "XDestroyWindow")]
        public extern static int XDestroyWindow(IntPtr display, IntPtr window);

        [DllImport(lib, EntryPoint = "XReparentWindow")]
        public extern static int XReparentWindow(IntPtr display, IntPtr window, IntPtr parent, int x, int y);

        [DllImport(lib, EntryPoint = "XMoveResizeWindow")]
        public extern static int XMoveResizeWindow(IntPtr display, IntPtr window, int x, int y, int width, int height);

        [DllImport(lib, EntryPoint = "XMoveWindow")]
        public extern static int XMoveWindow(IntPtr display, IntPtr w, int x, int y);

        [DllImport(lib, EntryPoint = "XResizeWindow")]
        public extern static int XResizeWindow(IntPtr display, IntPtr window, int width, int height);

        [DllImport(lib, EntryPoint = "XGetWindowAttributes")]
        public extern static int XGetWindowAttributes(IntPtr display, IntPtr window, ref XWindowAttributes attributes);

        [DllImport(lib, EntryPoint = "XFlush")]
        public extern static int XFlush(IntPtr display);

        [DllImport(lib, EntryPoint = "XSetWMName")]
        public extern static int XSetWMName(IntPtr display, IntPtr window, ref XTextProperty text_prop);

        [DllImport(lib, EntryPoint = "XStoreName")]
        public extern static int XStoreName(IntPtr display, IntPtr window, string window_name);

        [DllImport(lib, EntryPoint = "XFetchName")]
        public extern static int XFetchName(IntPtr display, IntPtr window, ref IntPtr window_name);

        [DllImport(lib, EntryPoint = "XSendEvent")]
        public extern static int XSendEvent(IntPtr display, IntPtr window, bool propagate, IntPtr event_mask, ref XEvent send_event);

        public static int XSendEvent(IntPtr display, IntPtr window, bool propagate, EventMask event_mask, ref XEvent send_event)
        {
            return XSendEvent(display, window, propagate, new IntPtr((int)event_mask), ref send_event);
        }

        [DllImport(lib, EntryPoint = "XQueryTree")]
        public extern static int XQueryTree(IntPtr display, IntPtr window, out IntPtr root_return, out IntPtr parent_return, out IntPtr children_return, out int nchildren_return);

        [DllImport(lib, EntryPoint = "XFree")]
        public extern static int XFree(IntPtr data);

        [DllImport(lib, EntryPoint = "XRaiseWindow")]
        public extern static int XRaiseWindow(IntPtr display, IntPtr window);

        [DllImport(lib, EntryPoint = "XLowerWindow")]//, CLSCompliant(false)]
        public extern static uint XLowerWindow(IntPtr display, IntPtr window);

        [DllImport(lib, EntryPoint = "XConfigureWindow")]//, CLSCompliant(false)]
        public extern static uint XConfigureWindow(IntPtr display, IntPtr window, ChangeWindowAttributes value_mask, ref XWindowChanges values);

        [DllImport(lib, EntryPoint = "XInternAtom")]
        public extern static IntPtr XInternAtom(IntPtr display, string atom_name, bool only_if_exists);

        [DllImport(lib, EntryPoint = "XInternAtoms")]
        public extern static int XInternAtoms(IntPtr display, string[] atom_names, int atom_count, bool only_if_exists, IntPtr[] atoms);

        [DllImport(lib, EntryPoint = "XGetAtomName")]
        public extern static IntPtr XGetAtomName(IntPtr display, IntPtr atom);

        [DllImport(lib, EntryPoint = "XSetWMProtocols")]
        public extern static int XSetWMProtocols(IntPtr display, IntPtr window, IntPtr[] protocols, int count);

        [DllImport(lib, EntryPoint = "XGrabPointer")]
        public extern static int XGrabPointer(IntPtr display, IntPtr window, bool owner_events, EventMask event_mask, GrabMode pointer_mode, GrabMode keyboard_mode, IntPtr confine_to, IntPtr cursor, IntPtr timestamp);

        [DllImport(lib, EntryPoint = "XUngrabPointer")]
        public extern static int XUngrabPointer(IntPtr display, IntPtr timestamp);

        [DllImport(lib, EntryPoint = "XGrabButton")]
        public extern static int XGrabButton(IntPtr display,
            int button, uint modifiers, Window grab_window,
            Bool owner_events, EventMask event_mask,
            GrabMode pointer_mode, GrabMode keyboard_mode,
            Window confine_to, Cursor cursor);

        [DllImport(lib, EntryPoint = "XUngrabButton")]
        public extern static int XUngrabButton(IntPtr display, uint button, uint
              modifiers, Window grab_window);

        [DllImport(lib, EntryPoint = "XQueryPointer")]
        public extern static bool XQueryPointer(IntPtr display, IntPtr window, out IntPtr root, out IntPtr child, out int root_x, out int root_y, out int win_x, out int win_y, out int keys_buttons);

        [DllImport(lib, EntryPoint = "XTranslateCoordinates")]
        public extern static bool XTranslateCoordinates(IntPtr display, IntPtr src_w, IntPtr dest_w, int src_x, int src_y, out int intdest_x_return, out int dest_y_return, out IntPtr child_return);


        [DllImport(lib)]
        public extern static int XGrabKey(IntPtr display, int keycode, uint modifiers,
            Window grab_window, bool owner_events, GrabMode pointer_mode, GrabMode keyboard_mode);

        [DllImport(lib)]
        public extern static int XUngrabKey(IntPtr display, int keycode, uint modifiers, Window grab_window);

        [DllImport(lib, EntryPoint = "XGrabKeyboard")]
        public extern static int XGrabKeyboard(IntPtr display, IntPtr window, bool owner_events,
            GrabMode pointer_mode, GrabMode keyboard_mode, IntPtr timestamp);

        [DllImport(lib, EntryPoint = "XUngrabKeyboard")]
        public extern static int XUngrabKeyboard(IntPtr display, IntPtr timestamp);

        [DllImport(lib)]
        public extern static int XAllowEvents(IntPtr display, EventMode event_mode, Time time);

        [DllImport(lib, EntryPoint = "XGetGeometry")]
        public extern static bool XGetGeometry(IntPtr display, IntPtr window, out IntPtr root, out int x, out int y, out int width, out int height, out int border_width, out int depth);

        [DllImport(lib, EntryPoint = "XGetGeometry")]
        public extern static bool XGetGeometry(IntPtr display, IntPtr window, IntPtr root, out int x, out int y, out int width, out int height, IntPtr border_width, IntPtr depth);

        [DllImport(lib, EntryPoint = "XGetGeometry")]
        public extern static bool XGetGeometry(IntPtr display, IntPtr window, IntPtr root, out int x, out int y, IntPtr width, IntPtr height, IntPtr border_width, IntPtr depth);

        [DllImport(lib, EntryPoint = "XGetGeometry")]
        public extern static bool XGetGeometry(IntPtr display, IntPtr window, IntPtr root, IntPtr x, IntPtr y, out int width, out int height, IntPtr border_width, IntPtr depth);

        [DllImport(lib, EntryPoint = "XWarpPointer")]//, CLSCompliant(false)]
        public extern static uint XWarpPointer(IntPtr display, IntPtr src_w, IntPtr dest_w, int src_x, int src_y, uint src_width, uint src_height, int dest_x, int dest_y);

        [DllImport(lib, EntryPoint = "XClearWindow")]
        public extern static int XClearWindow(IntPtr display, IntPtr window);

        [DllImport(lib, EntryPoint = "XClearArea")]
        public extern static int XClearArea(IntPtr display, IntPtr window, int x, int y, int width, int height, bool exposures);

        // Colormaps
        [DllImport(lib, EntryPoint = "XDefaultScreenOfDisplay")]
        public extern static IntPtr XDefaultScreenOfDisplay(IntPtr display);

        [DllImport(lib, EntryPoint = "XScreenNumberOfScreen")]
        public extern static int XScreenNumberOfScreen(IntPtr display, IntPtr Screen);

        [DllImport(lib, EntryPoint = "XDefaultVisual")]
        public extern static IntPtr XDefaultVisual(IntPtr display, int screen_number);

        [DllImport(lib, EntryPoint = "XDefaultDepth")]//, CLSCompliant(false)]
        public extern static uint XDefaultDepth(IntPtr display, int screen_number);

        [DllImport(lib, EntryPoint = "XDefaultScreen")]
        public extern static int XDefaultScreen(IntPtr display);

        [DllImport(lib, EntryPoint = "XDefaultColormap")]
        public extern static IntPtr XDefaultColormap(IntPtr display, int screen_number);

        [DllImport(lib, EntryPoint = "XLookupColor")]//, CLSCompliant(false)]
        public extern static int XLookupColor(IntPtr display, IntPtr Colormap, string Coloranem, ref XColor exact_def_color, ref XColor screen_def_color);

        [DllImport(lib, EntryPoint = "XAllocColor")]//, CLSCompliant(false)]
        public extern static int XAllocColor(IntPtr display, IntPtr Colormap, ref XColor colorcell_def);

        [DllImport(lib, EntryPoint = "XSetTransientForHint")]
        public extern static int XSetTransientForHint(IntPtr display, IntPtr window, IntPtr prop_window);

        [DllImport(lib, EntryPoint = "XChangeProperty")]
        public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, ref MotifWmHints data, int nelements);

        [DllImport(lib, EntryPoint = "XChangeProperty")]//, CLSCompliant(false)]
        public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, ref uint value, int nelements);
        [DllImport(lib, EntryPoint = "XChangeProperty")]
        public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, ref int value, int nelements);

        [DllImport(lib, EntryPoint = "XChangeProperty")]//, CLSCompliant(false)]
        public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, ref IntPtr value, int nelements);

        [DllImport(lib, EntryPoint = "XChangeProperty")]//, CLSCompliant(false)]
        public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, uint[] data, int nelements);

        [DllImport(lib, EntryPoint = "XChangeProperty")]
        public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, int[] data, int nelements);

        [DllImport(lib, EntryPoint = "XChangeProperty")]
        public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, IntPtr[] data, int nelements);

        [DllImport(lib, EntryPoint = "XChangeProperty")]
        public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, IntPtr atoms, int nelements);

        [DllImport(lib, EntryPoint = "XChangeProperty", CharSet = CharSet.Ansi)]
        public extern static int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type, int format, PropertyMode mode, string text, int text_length);

        [DllImport(lib, EntryPoint = "XDeleteProperty")]
        public extern static int XDeleteProperty(IntPtr display, IntPtr window, IntPtr property);

        // Drawing
        [DllImport(lib, EntryPoint = "XCreateGC")]
        public extern static IntPtr XCreateGC(IntPtr display, IntPtr window, IntPtr valuemask, XGCValues[] values);

        [DllImport(lib, EntryPoint = "XFreeGC")]
        public extern static int XFreeGC(IntPtr display, IntPtr gc);

        [DllImport(lib, EntryPoint = "XSetFunction")]
        public extern static int XSetFunction(IntPtr display, IntPtr gc, GXFunction function);

        [DllImport(lib, EntryPoint = "XSetLineAttributes")]
        public extern static int XSetLineAttributes(IntPtr display, IntPtr gc, int line_width, GCLineStyle line_style, GCCapStyle cap_style, GCJoinStyle join_style);

        [DllImport(lib, EntryPoint = "XDrawLine")]
        public extern static int XDrawLine(IntPtr display, IntPtr drawable, IntPtr gc, int x1, int y1, int x2, int y2);

        [DllImport(lib, EntryPoint = "XDrawRectangle")]
        public extern static int XDrawRectangle(IntPtr display, IntPtr drawable, IntPtr gc, int x1, int y1, int width, int height);

        [DllImport(lib, EntryPoint = "XFillRectangle")]
        public extern static int XFillRectangle(IntPtr display, IntPtr drawable, IntPtr gc, int x1, int y1, int width, int height);

        [DllImport(lib, EntryPoint = "XSetWindowBackground")]
        public extern static int XSetWindowBackground(IntPtr display, IntPtr window, IntPtr background);

        [DllImport(lib, EntryPoint = "XCopyArea")]
        public extern static int XCopyArea(IntPtr display, IntPtr src, IntPtr dest, IntPtr gc, int src_x, int src_y, int width, int height, int dest_x, int dest_y);

        [DllImport(lib, EntryPoint = "XGetWindowProperty")]
        public extern static int XGetWindowProperty(IntPtr display, IntPtr window, IntPtr atom, IntPtr long_offset, IntPtr long_length, bool delete, IntPtr req_type, out IntPtr actual_type, out int actual_format, out IntPtr nitems, out IntPtr bytes_after, ref IntPtr prop);

        [DllImport(lib, EntryPoint = "XSetInputFocus")]
        public extern static int XSetInputFocus(IntPtr display, IntPtr window, RevertTo revert_to, IntPtr time);

        [DllImport(lib, EntryPoint = "XIconifyWindow")]
        public extern static int XIconifyWindow(IntPtr display, IntPtr window, int screen_number);

        [DllImport(lib, EntryPoint = "XDefineCursor")]
        public extern static int XDefineCursor(IntPtr display, IntPtr window, IntPtr cursor);

        [DllImport(lib, EntryPoint = "XUndefineCursor")]
        public extern static int XUndefineCursor(IntPtr display, IntPtr window);

        [DllImport(lib, EntryPoint = "XFreeCursor")]
        public extern static int XFreeCursor(IntPtr display, IntPtr cursor);

        [DllImport(lib, EntryPoint = "XCreateFontCursor")]
        public extern static IntPtr XCreateFontCursor(IntPtr display, CursorFontShape shape);

        [DllImport(lib, EntryPoint = "XCreatePixmapCursor")]//, CLSCompliant(false)]
        public extern static IntPtr XCreatePixmapCursor(IntPtr display, IntPtr source, IntPtr mask, ref XColor foreground_color, ref XColor background_color, int x_hot, int y_hot);

        [DllImport(lib, EntryPoint = "XCreatePixmapFromBitmapData")]
        public extern static IntPtr XCreatePixmapFromBitmapData(IntPtr display, IntPtr drawable, byte[] data, int width, int height, IntPtr fg, IntPtr bg, int depth);

        [DllImport(lib, EntryPoint = "XCreatePixmap")]
        public extern static IntPtr XCreatePixmap(IntPtr display, IntPtr d, int width, int height, int depth);

        [DllImport(lib, EntryPoint = "XFreePixmap")]
        public extern static IntPtr XFreePixmap(IntPtr display, IntPtr pixmap);

        [DllImport(lib, EntryPoint = "XQueryBestCursor")]
        public extern static int XQueryBestCursor(IntPtr display, IntPtr drawable, int width, int height, out int best_width, out int best_height);

        [DllImport(lib, EntryPoint = "XQueryExtension")]
        public extern static int XQueryExtension(IntPtr display, string extension_name, out int major, out int first_event, out int first_error);

        [DllImport(lib, EntryPoint = "XWhitePixel")]
        public extern static IntPtr XWhitePixel(IntPtr display, int screen_no);

        [DllImport(lib, EntryPoint = "XBlackPixel")]
        public extern static IntPtr XBlackPixel(IntPtr display, int screen_no);

        [DllImport(lib, EntryPoint = "XGrabServer")]
        public extern static void XGrabServer(IntPtr display);

        [DllImport(lib, EntryPoint = "XUngrabServer")]
        public extern static void XUngrabServer(IntPtr display);

        [DllImport(lib, EntryPoint = "XGetWMNormalHints")]
        public extern static int XGetWMNormalHints(IntPtr display, IntPtr window, ref XSizeHints hints, out IntPtr supplied_return);

        [DllImport(lib, EntryPoint = "XSetWMNormalHints")]
        public extern static void XSetWMNormalHints(IntPtr display, IntPtr window, ref XSizeHints hints);

        [DllImport(lib, EntryPoint = "XSetZoomHints")]
        public extern static void XSetZoomHints(IntPtr display, IntPtr window, ref XSizeHints hints);

        [DllImport(lib)]
        public static extern IntPtr XGetWMHints(Display display, Window w); // returns XWMHints*

        [DllImport(lib)]
        public static extern void XSetWMHints(Display display, Window w, ref XWMHints wmhints);

        [DllImport(lib)]
        public static extern IntPtr XAllocWMHints();

        [DllImport(lib, EntryPoint = "XGetIconSizes")]
        public extern static int XGetIconSizes(IntPtr display, IntPtr window, out IntPtr size_list, out int count);

        [DllImport(lib, EntryPoint = "XSetErrorHandler")]
        public extern static IntPtr XSetErrorHandler(XErrorHandler error_handler);

        [DllImport(lib, EntryPoint = "XGetErrorText")]
        public extern static IntPtr XGetErrorText(IntPtr display, byte code, StringBuilder buffer, int length);

        [DllImport(lib, EntryPoint = "XInitThreads")]
        public extern static int XInitThreads();

        [DllImport(lib, EntryPoint = "XConvertSelection")]
        public extern static int XConvertSelection(IntPtr display, IntPtr selection, IntPtr target, IntPtr property, IntPtr requestor, IntPtr time);

        [DllImport(lib, EntryPoint = "XGetSelectionOwner")]
        public extern static IntPtr XGetSelectionOwner(IntPtr display, IntPtr selection);

        [DllImport(lib, EntryPoint = "XSetSelectionOwner")]
        public extern static int XSetSelectionOwner(IntPtr display, IntPtr selection, IntPtr owner, IntPtr time);

        [DllImport(lib, EntryPoint = "XSetPlaneMask")]
        public extern static int XSetPlaneMask(IntPtr display, IntPtr gc, IntPtr mask);

        [DllImport(lib, EntryPoint = "XSetForeground")]//, CLSCompliant(false)]
        public extern static int XSetForeground(IntPtr display, IntPtr gc, UIntPtr foreground);
        [DllImport(lib, EntryPoint = "XSetForeground")]
        public extern static int XSetForeground(IntPtr display, IntPtr gc, IntPtr foreground);

        [DllImport(lib, EntryPoint = "XSetBackground")]//, CLSCompliant(false)]
        public extern static int XSetBackground(IntPtr display, IntPtr gc, UIntPtr background);
        [DllImport(lib, EntryPoint = "XSetBackground")]
        public extern static int XSetBackground(IntPtr display, IntPtr gc, IntPtr background);

        [DllImport(lib, EntryPoint = "XBell")]
        public extern static int XBell(IntPtr display, int percent);

        [DllImport(lib, EntryPoint = "XChangeActivePointerGrab")]
        public extern static int XChangeActivePointerGrab(IntPtr display, EventMask event_mask, IntPtr cursor, IntPtr time);

        [DllImport(lib, EntryPoint = "XFilterEvent")]
        public extern static bool XFilterEvent(ref XEvent xevent, IntPtr window);

        [DllImport(lib)]
        public extern static void XPeekEvent(IntPtr display, ref XEvent xevent);

        [DllImport(lib, EntryPoint = "XGetVisualInfo")]
        private static extern IntPtr XGetVisualInfoInternal(IntPtr display, IntPtr vinfo_mask, ref XVisualInfo template, out int nitems);

        public static IntPtr XGetVisualInfo(IntPtr display, XVisualInfoMask vinfo_mask, ref XVisualInfo template, out int nitems)
        {
            return XGetVisualInfoInternal(display, (IntPtr)(int)vinfo_mask, ref template, out nitems);
        }

        [DllImport(lib)]
        public static extern IntPtr XCreateColormap(Display display, Window window, IntPtr visual, int alloc);

        [DllImport(lib)]
        public static extern void XLockDisplay(Display display);

        [DllImport(lib)]
        public static extern void XUnlockDisplay(Display display);

        [DllImport(lib)]
        public static extern Status XGetTransientForHint(Display display, Window w, out Window prop_window_return);

        [DllImport(lib)]
        public static extern void XSync(Display display, bool discard);

        [DllImport(lib)]
        public static extern void XAutoRepeatOff(IntPtr display);

        [DllImport(lib)]
        public static extern void XAutoRepeatOn(IntPtr display);

        [DllImport(lib)]
        public static extern IntPtr XDefaultRootWindow(IntPtr display);

        [DllImport(lib)]
        public static extern int XBitmapBitOrder(Display display);

        [DllImport(lib)]
        public static extern IntPtr XCreateImage(Display display, IntPtr visual,
            uint depth, ImageFormat format, int offset, byte[] data, uint width, uint height,
            int bitmap_pad, int bytes_per_line);

        [DllImport(lib)]
        public static extern IntPtr XCreateImage(Display display, IntPtr visual,
            uint depth, ImageFormat format, int offset, IntPtr data, uint width, uint height,
            int bitmap_pad, int bytes_per_line);

        [DllImport(lib)]
        public static extern void XPutImage(Display display, IntPtr drawable,
            IntPtr gc, IntPtr image, int src_x, int src_y, int dest_x, int dest_y, uint width, uint height);

        [DllImport(lib)]
        public static extern int XLookupString(ref XKeyEvent event_struct, [Out] byte[] buffer_return,
            int bytes_buffer, [Out] KeySym[] keysym_return, IntPtr status_in_out);

        [DllImport(lib)]
        public static extern KeyCode XKeysymToKeycode(IntPtr display, KeySym keysym);

        [DllImport(lib)]
        public static extern KeySym XKeycodeToKeysym(IntPtr display, KeyCode keycode, int index);

        [DllImport(lib)]
        public static extern int XRefreshKeyboardMapping(ref XMappingEvent event_map);

        [DllImport(lib)]
        public static extern int XGetEventData(IntPtr display, ref XGenericEventCookie cookie);

        [DllImport(lib)]
        public static extern void XFreeEventData(IntPtr display, ref XGenericEventCookie cookie);

        [DllImport(lib)]
        public static extern void XSetClassHint(IntPtr display, IntPtr window, ref XClassHint hint);

        private static readonly IntPtr CopyFromParent = IntPtr.Zero;

        public static void SendNetWMMessage(X11WindowInfo window, IntPtr message_type, IntPtr l0, IntPtr l1, IntPtr l2)
        {
            XEvent xev;

            xev = new XEvent();
            xev.ClientMessageEvent.type = XEventName.ClientMessage;
            xev.ClientMessageEvent.send_event = true;
            xev.ClientMessageEvent.window = window.Handle;
            xev.ClientMessageEvent.message_type = message_type;
            xev.ClientMessageEvent.format = 32;
            xev.ClientMessageEvent.ptr1 = l0;
            xev.ClientMessageEvent.ptr2 = l1;
            xev.ClientMessageEvent.ptr3 = l2;

            XSendEvent(window.Display, window.RootWindow, false,
                       new IntPtr((int)(EventMask.SubstructureRedirectMask | EventMask.SubstructureNotifyMask)),
                       ref xev);
        }

        public static void SendNetClientMessage(X11WindowInfo window, IntPtr message_type,
                                                IntPtr l0, IntPtr l1, IntPtr l2)
        {
            XEvent xev;

            xev = new XEvent();
            xev.ClientMessageEvent.type = XEventName.ClientMessage;
            xev.ClientMessageEvent.send_event = true;
            xev.ClientMessageEvent.window = window.Handle;
            xev.ClientMessageEvent.message_type = message_type;
            xev.ClientMessageEvent.format = 32;
            xev.ClientMessageEvent.ptr1 = l0;
            xev.ClientMessageEvent.ptr2 = l1;
            xev.ClientMessageEvent.ptr3 = l2;

            XSendEvent(window.Display, window.Handle, false, new IntPtr((int)EventMask.NoEventMask), ref xev);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct  Pixel
        {
            public byte A, R, G, B;
            public Pixel(byte a, byte r, byte g, byte b)
            {
                A = a;
                R = r;
                G = g;
                B = b;
            }
            public static implicit operator Pixel(int argb)
            {
                return new Pixel(
                    (byte)((argb >> 24) & 0xFF),
                    (byte)((argb >> 16) & 0xFF),
                    (byte)((argb >> 8) & 0xFF),
                    (byte)(argb & 0xFF));
            }
        }
        public static IntPtr CreatePixmapFromImage(Display display, Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;

            BitmapData data = image.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            IntPtr ximage = XCreateImage(display, CopyFromParent, 24, ImageFormat.ZPixmap,
                0, data.Scan0, (uint)width, (uint)height, 32, 0);
            IntPtr pixmap = XCreatePixmap(display, XDefaultRootWindow(display),
                width, height, 24);
            IntPtr gc = XCreateGC(display, pixmap, IntPtr.Zero, null);

            XPutImage(display, pixmap, gc, ximage, 0, 0, 0, 0, (uint)width, (uint)height);

            XFreeGC(display, gc);
            image.UnlockBits(data);

            return pixmap;
        }

        public static IntPtr CreateMaskFromImage(Display display, Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            int stride = (width + 7) >> 3;
            byte[] mask = new byte[stride * height];
            bool msbfirst = (XBitmapBitOrder(display) == 1); // 1 = MSBFirst

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    byte bit = (byte)(1 << (msbfirst ? (7 - (x & 7)) : (x & 7)));
                    int offset = y * stride + (x >> 3);

                    if (image.GetPixel(x, y).A >= 128)
                    {
                        mask[offset] |= bit;
                    }
                }
            }

            Pixmap pixmap = XCreatePixmapFromBitmapData(display, XDefaultRootWindow(display),
                mask, width, height, new IntPtr(1), IntPtr.Zero, 1);

            return pixmap;
        }
    }
}
