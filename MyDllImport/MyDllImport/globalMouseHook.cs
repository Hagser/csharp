using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace MyDllImport
{
    public class globalMouseHook
    {
        #region Constant, Structure and Delegate Definitions
        /// <summary>
        /// defines the callback type for the hook
        /// </summary>
        private delegate int mouseHookProc(int code, IntPtr wParam, IntPtr lParam);
        mouseHookProc MouseHookProcedure;

        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class mouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        const int WH_MOUSE_LL = 14;
        const int WH_MOUSE = 7;
        #endregion

        #region Instance Variables
        /// <summary>
        /// The collections of keys to watch for
        /// </summary>
        public List<Keys> HookedKeys = new List<Keys>();
        /// <summary>
        /// Handle to the hook, need this to unhook and call the next hook
        /// </summary>
        IntPtr hhook = IntPtr.Zero;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when one of the hooked keys is pressed
        /// </summary>
        public event MouseEventHandler MouseEvent;
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="globalMouseHook"/> class and installs the mouse hook.
        /// </summary>
        public globalMouseHook()
        {
            hook();
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="globalMouseHook"/> is reclaimed by garbage collection and uninstalls the mouse hook.
        /// </summary>
        ~globalMouseHook()
        {
            unhook();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Installs the global hook
        /// </summary>
        public void hook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            MouseHookProcedure = new mouseHookProc(hookProc);
            hhook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookProcedure, hInstance, 0);
        }

        /// <summary>
        /// Uninstalls the global hook
        /// </summary>
        public void unhook()
        {
            UnhookWindowsHookEx(hhook);
        }

        /// <summary>
        /// The callback for the mouse hook
        /// </summary>
        /// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
        /// <param name="wParam">The event type</param>
        /// <param name="lParam">The keyhook event information</param>
        /// <returns></returns>
        public int hookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            mouseHookStruct MyMouseHookStruct = (mouseHookStruct)Marshal.PtrToStructure(lParam, typeof(mouseHookStruct));

            if (code >= 0)
            {
                MouseEventArgs mea = new MouseEventArgs(MouseButtons.None, MyMouseHookStruct.dwExtraInfo, MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y,wParam.ToInt32());
                if(MouseEvent!=null)
                    MouseEvent(this, mea);
                return code;
            }
            Thread.Sleep(10);
            return CallNextHookEx(hhook, code, wParam, lParam);
        }
        #endregion

        #region DLL imports
        /// <summary>
        /// Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
        /// </summary>
        /// <param name="idHook">The id of the event you want to hook</param>
        /// <param name="callback">The callback.</param>
        /// <param name="hInstance">The handle you want to attach the event to, can be null</param>
        /// <param name="threadId">The thread you want to attach the event to, can be null</param>
        /// <returns>a handle to the desired hook</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto,
 CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr SetWindowsHookEx(int idHook, mouseHookProc callback, IntPtr hInstance, uint threadId);

        /// <summary>
        /// Unhooks the windows hook.
        /// </summary>
        /// <param name="hInstance">The hook handle that was returned from SetWindowsHookEx</param>
        /// <returns>True if successful, false otherwise</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto,
 CallingConvention = CallingConvention.StdCall)]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        /// <summary>
        /// Calls the next hook.
        /// </summary>
        /// <param name="idHook">The hook id</param>
        /// <param name="nCode">The hook code</param>
        /// <param name="wParam">The wparam.</param>
        /// <param name="lParam">The lparam.</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto,
 CallingConvention = CallingConvention.StdCall)]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Loads the library.
        /// </summary>
        /// <param name="lpFileName">Name of the library</param>
        /// <returns>A handle to the library</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto,
 CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr LoadLibrary(string lpFileName);
        #endregion
        
    }
}
