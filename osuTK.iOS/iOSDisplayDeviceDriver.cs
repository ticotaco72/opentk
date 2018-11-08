using System;
using System.Collections.Generic;
using osuTK.Platform;

namespace osuTK.iOS
{
    internal class iOSDisplayDeviceDriver : IDisplayDeviceDriver
    {
        private static DisplayDevice dev;
        static iOSDisplayDeviceDriver()
        {
            dev = new DisplayDevice();
            dev.IsPrimary = true;
            dev.BitsPerPixel = 16;
        }

        public List<DisplayDevice> AvailableDevices => new List<DisplayDevice>(new DisplayDevice[] { dev });

        public DisplayDevice GetDisplay(DisplayIndex displayIndex)
        {
            return (displayIndex == DisplayIndex.First || displayIndex == DisplayIndex.Primary) ? dev : null;
        }

        public bool TryChangeResolution(DisplayDevice device, DisplayResolution resolution)
        {
            return false;
        }

        public bool TryRestoreResolution(DisplayDevice device)
        {
            return false;
        }
    }

}
