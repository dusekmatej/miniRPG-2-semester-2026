// Helpers/NativeMethods.cs
using System.Runtime.InteropServices;

namespace miniRPG.Helpers;

// NOTE: This class was completely made with AI

internal static class NativeMethods
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Message
    {
        public IntPtr hWnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point pt;
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool PeekMessage(out Message msg, IntPtr hWnd,
        uint messageFilterMin, uint messageFilterMax, uint flags);
}