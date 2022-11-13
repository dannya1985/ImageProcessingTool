using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ScreenShotUtil
{
    static class DisplayTools
    {
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        private enum DeviceCap
        {
            Desktopvertres = 117,
            Desktophorzres = 118
        }

        public enum SystemMetric
        {
            VirtualScreenWidth = 78, // CXVIRTUALSCREEN 0x0000004E 
            VirtualScreenHeight = 79, // CYVIRTUALSCREEN 0x0000004F 
        }

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(SystemMetric metric);

        public static Size GetVirtualDisplaySize()
        {
            var width = GetSystemMetrics(SystemMetric.VirtualScreenWidth);
            var height = GetSystemMetrics(SystemMetric.VirtualScreenHeight);

            return new Size(width, height);
        }

        public static Size GetPhysicalDisplaySize()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();

            int physicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.Desktopvertres);
            int physicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.Desktophorzres);

            return new Size(physicalScreenWidth, physicalScreenHeight);
        }
    }
}
