using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DebugTools.Common.Warpper.API
{
    public class WinAPIWarpper
    {

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(string lpDriverName, string lpDeviceName, string lpOutput, string lpInitData);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern int BitBlt(IntPtr desthDC, int srcX, int srcY, int srcW, int srcH, IntPtr srchDC, int destX, int destY, int op);
        [DllImport("gdi32.dll")]
        public static extern int DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern int DeleteObject(IntPtr hObj);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool InvalidateRect(IntPtr hWnd, ComRect rect, bool erase);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool RedrawWindow(IntPtr hwnd, ComRect rcUpdate, IntPtr hrgnUpdate, int flags);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool UpdateWindow(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]
        public static extern IntPtr WindowFromPoint(int xPoint, int yPoint);
        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]//获取鼠标坐标 
        public static extern int GetCursorPos(ref APIPointer lpPoint);
    }
}
