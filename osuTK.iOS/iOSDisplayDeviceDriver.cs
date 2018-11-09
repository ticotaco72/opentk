using System;
using System.Collections.Generic;
using osuTK.Platform;

namespace osuTK.iOS
{
    public class iOSDisplayDeviceDriver : IDisplayDeviceDriver
    {
        private static DisplayDevice dev;
        static iOSDisplayDeviceDriver()
        {
            dev = new DisplayDevice();
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
