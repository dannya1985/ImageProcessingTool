using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ScreenShotUtil
{
    public class ScreenCapture
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        public static Image CaptureDesktop()
        {
            return CaptureWindow(GetDesktopWindow(), true);
        }

        public static Bitmap CaptureActiveWindow()
        {
            return CaptureWindow(GetForegroundWindow());
        }

        public static Bitmap CaptureWindow(IntPtr handle, bool handleDisplayScalingAutomatically = false)
        {
            var rect = new Rect();
            GetWindowRect(handle, ref rect);
            
            var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

            //Due to display scaling, often the window will report smaller rectangle sizes than the size of the display
            //causing the screenshot to be smaller than desired.
            //This flag tells the screenshot to force the full display size.
            //NOTE: This seems to work off of the primary display, the secondary display did not negatively affect this (its a smaller resolution on my 2nd monitor).
            if (handleDisplayScalingAutomatically)
            {
                var derp = DisplayTools.GetPhysicalDisplaySize();
                if (bounds.Width < derp.Width && bounds.Height < derp.Height)
                {
                    bounds = new Rectangle(rect.Left, rect.Top, derp.Width, derp.Height);
                }
            }

            var result = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return result;
        }
    }
}
