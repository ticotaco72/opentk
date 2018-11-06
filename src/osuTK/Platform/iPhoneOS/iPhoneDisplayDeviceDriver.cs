#if IPHONE

using System;
using System.Collections.Generic;
using System.Text;
using osuTK.Platform;

namespace osuTK.Platform.iPhoneOS
{
    internal class AndroidDisplayDeviceDriver : IDisplayDeviceDriver
    {
        private static DisplayDevice dev;
        static AndroidDisplayDeviceDriver()
        {
            dev = new DisplayDevice();
            dev.IsPrimary = true;
            dev.BitsPerPixel = 16;
        }

        public List<DisplayDevice> AvailableDevices { get; } = new List<DisplayDevice> { dev };

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

#endif