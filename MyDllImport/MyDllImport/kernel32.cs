using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyDllImport
{
    public static class kernel32
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Beep(uint dwFreq, uint dwDuration);

        public static void beep(uint dwFreq, uint dwDuration)
        {
            Beep(dwFreq, dwDuration);
        }


        [DllImport("kernel32.dll")]
        static extern int GetCurrentThreadId();
        public static int getCurrentThreadId()
        {
            return GetCurrentThreadId();
        }

        #region GetLastError
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetLastError();
        public static IntPtr getLastError()
        {
            return GetLastError();
        }
        #endregion


        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);
        public static IntPtr loadLibrary(string lpFileName)
        {
            return LoadLibrary(lpFileName);
        }

    }
}
