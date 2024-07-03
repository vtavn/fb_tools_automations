using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Tool_Request.Helpers
{
    internal class User32
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref User32.RECT rect);

        public struct RECT
        {
            public int left;

            public int top;

            public int right;

            public int bottom;
        }
    }
}
