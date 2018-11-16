/* Licensed under the MIT/X11 license.
 * Copyright (c) 2011 Xamarin, Inc.
 * Copyright 2013 Xamarin Inc
 * This notice may not be removed from any source distribution.
 * See license.txt for licensing detailed licensing details.
 */

using System.Collections.Generic;

namespace osuTK.Platform.Android
{
    internal class AndroidDisplayDeviceDriver : IDisplayDeviceDriver
    {
        private static DisplayDevice dev;
        static AndroidDisplayDeviceDriver ()
        {
            dev = new DisplayDevice ();
            dev.IsPrimary = true;
            dev.BitsPerPixel = 16;
        }

        public List<DisplayDevice> AvailableDevices => new List<DisplayDevice>(new DisplayDevice[] { dev });

        public DisplayDevice GetDisplay(DisplayIndex displayIndex) =>
            (displayIndex == DisplayIndex.First || displayIndex == DisplayIndex.Primary) ? dev : null;

        public bool TryChangeResolution(DisplayDevice device, DisplayResolution resolution) => false;

        public bool TryRestoreResolution(DisplayDevice device) => false;
    }
}
