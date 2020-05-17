#region Header

// OathToOrder >OathToOrder >NativeMethods.cs\n Copyright (C) Adonis Deliannis, 2020\nCreated 18 04, 2020

#endregion

using System.Runtime.InteropServices;

namespace OathToOrder {
    internal static class NativeMethods {
        [DllImport("kernel32")]
        public static extern void AllocConsole();

        [DllImport("kernel32")]
        public static extern void FreeConsole();
    }
}