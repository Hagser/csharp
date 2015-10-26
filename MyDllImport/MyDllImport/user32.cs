using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace MyDllImport
{
    public static class user32
    {
        #region EnumWindows
        public delegate bool CallBack(IntPtr hwnd, IntPtr lParam);
        [DllImport("user32", SetLastError = true)]
        private static extern IntPtr EnumWindows(CallBack x, IntPtr y);
        public static void getWindows(CallBack x, IntPtr y)
        {
            EnumWindows(x, y);
        }
        #endregion
        #region GetActiveWindow
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetActiveWindow();
        public static IntPtr getActiveWindow()
        {
            return GetActiveWindow();
        }
        #endregion
        #region SetForegroundWindow
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        public static bool setForegroundWindow(IntPtr hWnd)
        {
            return SetForegroundWindow(hWnd);
        }
        #endregion
        #region GetForgroundWindow
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();
        public static IntPtr getForegroundWindow()
        {
            return GetForegroundWindow();
        }
        #endregion
        #region FindWindow

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        public static IntPtr findWindow(string lpClassName,string lpWindowName)
        {
            return FindWindow(lpClassName, lpWindowName);
        }
        #endregion
        #region FindWindowByCaption
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
        public static IntPtr findWindowByCaption(string lpWindowName)
        {
            return FindWindowByCaption(IntPtr.Zero,lpWindowName);
        }
        #endregion
        #region PostMessage
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, UInt32 wParam, UInt32 lParam);
        public static bool postMessage(IntPtr hWnd, UInt32 Msg, UInt32 wParam, UInt32 lParam)
        {
            return PostMessage(hWnd, Msg, wParam, lParam);
        }
        #endregion
        #region GetWindowRect
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);
        public static bool getWindowRect(IntPtr hWnd, out Rect lpRect)
        {
            return GetWindowRect(hWnd, out lpRect);
        }
        #endregion
        #region GetWindowInfo
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowInfo(IntPtr hwnd, ref WindowInfo pwi);
        public static WindowInfo getWindowInfo(IntPtr hwnd)
        {
            WindowInfo info = new WindowInfo();
            //info.cbSize = (uint)Marshal.SizeOf(info);
            GetWindowInfo(hwnd, ref info);
            info.handle = hwnd;
            return info;
        }
        #endregion
        #region SetWindowPos
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        public static bool setWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, Rect r, SetWindowPosFlags uFlags)
        {
            return setWindowPos(hWnd, hWndInsertAfter, r.Top, r.Left, r.Width, r.Height, uFlags);
        }
        public static bool setWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int top, int left, int width, int height, SetWindowPosFlags uFlags)
        {
            return SetWindowPos(hWnd, hWndInsertAfter, top, left, width, height, uFlags);
        }
        #endregion
        #region FlashWindow
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool FlashWindow(IntPtr hwnd, bool invert);
        public static bool flashWindow(IntPtr hwnd, bool invert)
        {
            return FlashWindow(hwnd, invert);
        }
        #endregion
        #region mouse_event

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData,UIntPtr dwExtraInfo);

        #endregion
        #region SendInput

        [DllImport("user32.dll",SetLastError=true)]
        static extern uint SendInput(uint nInputs,[MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs,int cbSize);
        public static uint sendInput([MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs)
        {
            return SendInput((uint)pInputs.Length, pInputs, INPUT.Size);
        }
        public static uint sendInput(MOUSEINPUT[] inputs)
        {
            INPUT[] pInputs = new INPUT[inputs.Length];
            for(int i=0;i<inputs.Length;i++)
            {
                pInputs[i] = new INPUT()
                {
                    type = 2,
                    inputunion = new InputUnion()
                    {
                        mouseinput = inputs[i]
                    }
                };
            }
            return sendInput(pInputs);
        }
        public static uint sendInput(MOUSEINPUT input)
        {
            var pInputs = new[] { 
                new INPUT()
                {
                    type=2,
                    inputunion=new InputUnion()
                    {
                        mouseinput=input
                    }
                }
            };
            return sendInput(pInputs);
        }

        public static uint sendInput(KEYBDINPUT[] inputs)
        {
            INPUT[] pInputs = new INPUT[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                pInputs[i] = new INPUT()
                {
                    type = INPUT_KEYBOARD,
                    inputunion = new InputUnion()
                    {
                        keyboardinput = inputs[i]
                    }
                };
            }
            return sendInput(pInputs);
        }
        public static uint sendInput(KEYBDINPUT input)
        {
            var pInputs = new[] { 
                new INPUT()
                {
                    type=INPUT_KEYBOARD,
                    inputunion=new InputUnion()
                    {
                        keyboardinput=input
                    }
                }
            };
            return sendInput(pInputs);
        }
        public static uint sendInput(HARDWAREINPUT[] inputs)
        {
            INPUT[] pInputs = new INPUT[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                pInputs[i] = new INPUT()
                {
                    type = 3,
                    inputunion = new InputUnion()
                    {
                        hardwareinput = inputs[i]
                    }
                };
            }
            return sendInput(pInputs);
        }
        public static uint sendInput(HARDWAREINPUT input)
        {
            var pInputs = new[] { 
                new INPUT()                                                                    
                {
                    type=3,
                    inputunion=new InputUnion()
                    {
                        hardwareinput=input
                    }
                }
            };
            return sendInput(pInputs);
        }
        #endregion
        #region GetCursorPos
        [DllImport("user32.dll",SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);
        public static POINT getCursorPos()
        {
            POINT p = new POINT();

            GetCursorPos(out p);

            return p;// new Point(int.Parse(p.x.ToString()), int.Parse(p.y.ToString()));
        }
        #endregion
        #region SetCursorPos
        [DllImport("user32.dll",SetLastError=true)]
        static extern bool SetCursorPos(int X, int Y);

        public static bool setCursorPos(POINT p)
        {
            return SetCursorPos(p.X, p.Y);
        }

        public static bool setCursorPos(Point p)
        {
            return SetCursorPos(p.X, p.Y);
        }

        public static bool setCursorPos(int X, int Y)
        {
            return SetCursorPos(X, Y);
        }
        #endregion



        public delegate IntPtr HookProc(int code, IntPtr wParam, ref keyboardHookStruct lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UnhookWindowsHookEx(IntPtr hhk);

        public static bool unhookWindowsHookEx(IntPtr hhk)
        {
            return UnhookWindowsHookEx(hhk);
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(HookType code, HookProc func, IntPtr hInstance, int threadID);

        public static IntPtr setWindowsHookEx(HookType code, HookProc func, IntPtr hInstance, int threadID)
        { 
            return SetWindowsHookEx(code, func, hInstance, threadID);
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, ref keyboardHookStruct lParam);
        public static IntPtr callNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, ref keyboardHookStruct lParam)
        { 
            return CallNextHookEx(hhk,nCode, wParam,ref lParam);
        }

        

        #region structenums


        public struct keyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        public enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        public const int INPUT_KEYBOARD = 1;
        [Flags]
        public enum MouseEventFlags : uint
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010,
            WHEEL = 0x00000800,
            XDOWN = 0x00000080,
            XUP = 0x00000100
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public POINT(System.Drawing.Point pt) : this(pt.X, pt.Y) { }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }
        public struct Rect
        {
            public Rect(System.Drawing.Rectangle rectangle)
            {
                Left = rectangle.Left;
                Top = rectangle.Top;
                Right = rectangle.Right;
                Bottom = rectangle.Bottom;
            }
            public Rect(System.Drawing.Point location, System.Drawing.Size size)
            {
                Left = location.X;
                Top = location.Y;
                Right = location.X + size.Width;
                Bottom = location.Y + size.Height;
            }
            public System.Int32 Left;
            public System.Int32 Top;
            public System.Int32 Right;
            public System.Int32 Bottom;

            public System.Int32 Width { get { return Right - Left; } set { Right = Left + value; } }
            public System.Int32 Height { get { return Bottom - Top; } set { Bottom = Top + value; } }
        }
        public struct WindowInfo
        {
            public IntPtr handle;
            public uint cbSize;
            public Rect rcWindow;
            public Rect rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;
            public ushort wCreatorVersion;

            public WindowInfo(Boolean? filler)
                : this()
            {
                cbSize = (UInt32)(Marshal.SizeOf(typeof(WindowInfo)));
            }

        }
        [Flags()]
        public enum SetWindowPosFlags : uint
        {
            /// <summary>If the calling thread and the thread that owns the window are attached to different input queues, 
            /// the system posts the request to the thread that owns the window. This prevents the calling thread from 
            /// blocking its execution while other threads process the request.</summary>
            /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
            AsynchronousWindowPosition = 0x4000,
            /// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
            /// <remarks>SWP_DEFERERASE</remarks>
            DeferErase = 0x2000,
            /// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
            /// <remarks>SWP_DRAWFRAME</remarks>
            DrawFrame = 0x0020,
            /// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to 
            /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE 
            /// is sent only when the window's size is being changed.</summary>
            /// <remarks>SWP_FRAMECHANGED</remarks>
            FrameChanged = 0x0020,
            /// <summary>Hides the window.</summary>
            /// <remarks>SWP_HIDEWINDOW</remarks>
            HideWindow = 0x0080,
            /// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the 
            /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter 
            /// parameter).</summary>
            /// <remarks>SWP_NOACTIVATE</remarks>
            DoNotActivate = 0x0010,
            /// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid 
            /// contents of the client area are saved and copied back into the client area after the window is sized or 
            /// repositioned.</summary>
            /// <remarks>SWP_NOCOPYBITS</remarks>
            DoNotCopyBits = 0x0100,
            /// <summary>Retains the current position (ignores X and Y parameters).</summary>
            /// <remarks>SWP_NOMOVE</remarks>
            IgnoreMove = 0x0002,
            /// <summary>Does not change the owner window's position in the Z order.</summary>
            /// <remarks>SWP_NOOWNERZORDER</remarks>
            DoNotChangeOwnerZOrder = 0x0200,
            /// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to 
            /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent 
            /// window uncovered as a result of the window being moved. When this flag is set, the application must 
            /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
            /// <remarks>SWP_NOREDRAW</remarks>
            DoNotRedraw = 0x0008,
            /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
            /// <remarks>SWP_NOREPOSITION</remarks>
            DoNotReposition = 0x0200,
            /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
            /// <remarks>SWP_NOSENDCHANGING</remarks>
            DoNotSendChangingEvent = 0x0400,
            /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
            /// <remarks>SWP_NOSIZE</remarks>
            IgnoreResize = 0x0001,
            /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
            /// <remarks>SWP_NOZORDER</remarks>
            IgnoreZOrder = 0x0004,
            /// <summary>Displays the window.</summary>
            /// <remarks>SWP_SHOWWINDOW</remarks>
            ShowWindow = 0x0040,
        }
        [Flags()]
        public enum Messages : uint
        { 
             WM_USER = 0x400,
             CB_GETEDITSEL = 0x140,
             CB_LIMITTEXT = 0x141,
             CB_SETEDITSEL = 0x142,
             CB_ADDSTRING = 0x143,
             CB_DELETESTRING = 0x144,
             CB_DIR = 0x145,
             CB_GETCOUNT = 0x146,
             CB_GETCURSEL = 0x147,
             CB_GETLBTEXT = 0x148,
             CB_GETLBTEXTLEN = 0x149,
             CB_INSERTSTRING = 0x14A,
             CB_RESETCONTENT = 0x14B,
             CB_FINDSTRING = 0x14C,
             CB_SELECTSTRING = 0x14D,
             CB_SETCURSEL = 0x14E,
             CB_SHOWDROPDOWN = 0x14F,
             CB_GETITEMDATA = 0x150,
             CB_SETITEMDATA = 0x151,
             CB_GETDROPPEDCONTROLRECT = 0x152,
             CB_SETITEMHEIGHT = 0x153,
             CB_GETITEMHEIGHT = 0x154,
             CB_SETEXTENDEDUI = 0x155,
             CB_GETEXTENDEDUI = 0x156,
             CB_GETDROPPEDSTATE = 0x157,
             CB_FINDSTRINGEXACT = 0x158,
             CB_SETLOCALE = 0x159,
             CB_GETLOCALE = 0x15A,
             CB_GETTOPINDEX = 0x15B,
             CB_SETTOPINDEX = 0x15C,
             CB_GETHORIZONTALEXTENT = 0x15D,
             CB_SETHORIZONTALEXTENT = 0x15E,
             CB_GETDROPPEDWIDTH = 0x15F,
             CB_SETDROPPEDWIDTH = 0x160,
             CB_INITSTORAGE = 0x161,
             CB_MSGMAX = 0x162,
             EM_CANUNDO = 0xC6,
             EM_EMPTYUNDOBUFFER = 0xCD,
             EM_FMTLINES = 0xC8,
             EM_FORMATRANGE = WM_USER + 57,
             EM_GETFIRSTVISIBLELINE = 0xCE,
             EM_GETHANDLE = 0xBD,
             EM_GETLINE = 0xC4,
             EM_GETLINECOUNT = 0xBA,
             EM_GETMODIFY = 0xB8,
             EM_GETPASSWORDCHAR = 0xD2,
             EM_GETRECT = 0xB2,
             EM_GETSEL = 0xB0,
             EM_GETTHUMB = 0xBE,
             EM_GETWORDBREAKPROC = 0xD1,
             EM_LIMITTEXT = 0xC5,
             EM_LINEFROMCHAR = 0xC9,
             EM_LINEINDEX = 0xBB,
             EM_LINELENGTH = 0xC1,
             EM_LINESCROLL = 0xB6,
             EM_REPLACESEL = 0xC2,
             EM_SCROLL = 0xB5,
             EM_SCROLLCARET = 0xB7,
             EM_SETHANDLE = 0xBC,
             EM_SETMODIFY = 0xB9,
             EM_SETPASSWORDCHAR = 0xCC,
             EM_SETREADONLY = 0xCF,
             EM_SETRECT = 0xB3,
             EM_SETRECTNP = 0xB4,
             EM_SETSEL = 0xB1,
             EM_SETTABSTOPS = 0xCB,
             EM_SETTARGETDEVICE = WM_USER + 72,
             EM_SETWORDBREAKPROC = 0xD0,
             EM_UNDO = 0xC7,
             HDS_HOTTRACK = 0x4,
             HDI_BITMAP = 0x10,
             HDI_IMAGE = 0x20,
             HDI_ORDER = 0x80,
             HDI_FORMAT = 0x4,
             HDI_TEXT = 0x2,
             HDI_WIDTH = 0x1,
             HDI_HEIGHT = HDI_WIDTH,
             HDF_LEFT = 0,
             HDF_RIGHT = 1,
             HDF_IMAGE = 0x800,
             HDF_BITMAP_ON_RIGHT = 0x1000,
             HDF_BITMAP = 0x2000,
             HDF_STRING = 0x4000,
             HDM_FIRST = 0x1200,
             HDM_SETITEM = (HDM_FIRST + 4),
             LB_ADDFILE = 0x196,
             LB_ADDSTRING = 0x180,
             LB_DELETESTRING = 0x182,
             LB_DIR = 0x18D,
             LB_FINDSTRING = 0x18F,
             LB_FINDSTRINGEXACT = 0x1A2,
             LB_GETANCHORINDEX = 0x19D,
             LB_GETCARETINDEX = 0x19F,
             LB_GETCOUNT = 0x18B,
             LB_GETCURSEL = 0x188,
             LB_GETHORIZONTALEXTENT = 0x193,
             LB_GETITEMDATA = 0x199,
             LB_GETITEMHEIGHT = 0x1A1,
             LB_GETITEMRECT = 0x198,
             LB_GETLOCALE = 0x1A6,
             LB_GETSEL = 0x187,
             LB_GETSELCOUNT = 0x190,
             LB_GETSELITEMS = 0x191,
             LB_GETTEXT = 0x189,
             LB_GETTEXTLEN = 0x18A,
             LB_GETTOPINDEX = 0x18E,
             LB_INSERTSTRING = 0x181,
             LB_MSGMAX = 0x1A8,
             LB_OKAY = 0,
             LB_RESETCONTENT = 0x184,
             LB_SELECTSTRING = 0x18C,
             LB_SELITEMRANGE = 0x19B,
             LB_SELITEMRANGEEX = 0x183,
             LB_SETANCHORINDEX = 0x19C,
             LB_SETCARETINDEX = 0x19E,
             LB_SETCOLUMNWIDTH = 0x195,
             LB_SETCOUNT = 0x1A7,
             LB_SETCURSEL = 0x186,
             LB_SETHORIZONTALEXTENT = 0x194,
             LB_SETITEMDATA = 0x19A,
             LB_SETITEMHEIGHT = 0x1A0,
             LB_SETLOCALE = 0x1A5,
             LB_SETSEL = 0x185,
             LB_SETTABSTOPS = 0x192,
             LB_SETTOPINDEX = 0x197,
             LBN_DBLCLK = 2,
             LBN_KILLFOCUS = 5,
             LBN_SELCANCEL = 3,
             LBN_SELCHANGE = 1,
             LBN_SETFOCUS = 4,
             LVM_FIRST = 0x1000,
             LVM_GETHEADER = (LVM_FIRST + 31),
             LVM_GETBKCOLOR = (LVM_FIRST + 0),
             LVM_SETBKCOLOR = (LVM_FIRST + 1),
             LVM_GETIMAGELIST = (LVM_FIRST + 2),
             LVM_SETIMAGELIST = (LVM_FIRST + 3),
             LVM_GETITEMCOUNT = (LVM_FIRST + 4),
             LVM_GETITEMA = (LVM_FIRST + 5),
             LVM_GETITEM = LVM_GETITEMA,
             LVM_SETITEMA = (LVM_FIRST + 6),
             LVM_SETITEM = LVM_SETITEMA,
             LVM_INSERTITEMA = (LVM_FIRST + 7),
             LVM_INSERTITEM = LVM_INSERTITEMA,
             LVM_DELETEITEM = (LVM_FIRST + 8),
             LVM_DELETEALLITEMS = (LVM_FIRST + 9),
             LVM_GETCALLBACKMASK = (LVM_FIRST + 10),
             LVM_SETCALLBACKMASK = (LVM_FIRST + 11),
             LVM_GETNEXTITEM = (LVM_FIRST + 12),
             LVM_FINDITEMA = (LVM_FIRST + 13),
             LVM_FINDITEM = LVM_FINDITEMA,
             LVM_GETITEMRECT = (LVM_FIRST + 14),
             LVM_SETITEMPOSITION = (LVM_FIRST + 15),
             LVM_GETITEMPOSITION = (LVM_FIRST + 16),
             LVM_GETSTRINGWIDTHA = (LVM_FIRST + 17),
             LVM_GETSTRINGWIDTH = LVM_GETSTRINGWIDTHA,
             LVM_HITTEST = (LVM_FIRST + 18),
             LVM_ENSUREVISIBLE = (LVM_FIRST + 19),
             LVM_SCROLL = (LVM_FIRST + 20),
             LVM_REDRAWITEMS = (LVM_FIRST + 21),
             LVM_ARRANGE = (LVM_FIRST + 22),
             LVM_EDITLABELA = (LVM_FIRST + 23),
             LVM_EDITLABEL = LVM_EDITLABELA,
             LVM_GETEDITCONTROL = (LVM_FIRST + 24),
             LVM_GETCOLUMNA = (LVM_FIRST + 25),
             LVM_GETCOLUMN = LVM_GETCOLUMNA,
             LVM_SETCOLUMNA = (LVM_FIRST + 26),
             LVM_SETCOLUMN = LVM_SETCOLUMNA,
             LVM_INSERTCOLUMNA = (LVM_FIRST + 27),
             LVM_INSERTCOLUMN = LVM_INSERTCOLUMNA,
             LVM_DELETECOLUMN = (LVM_FIRST + 28),
             LVM_GETCOLUMNWIDTH = (LVM_FIRST + 29),
             LVM_SETCOLUMNWIDTH = (LVM_FIRST + 30),
             LVM_CREATEDRAGIMAGE = (LVM_FIRST + 33),
             LVM_GETVIEWRECT = (LVM_FIRST + 34),
             LVM_GETTEXTCOLOR = (LVM_FIRST + 35),
             LVM_SETTEXTCOLOR = (LVM_FIRST + 36),
             LVM_GETTEXTBKCOLOR = (LVM_FIRST + 37),
             LVM_SETTEXTBKCOLOR = (LVM_FIRST + 38),
             LVM_GETTOPINDEX = (LVM_FIRST + 39),
             LVM_GETCOUNTPERPAGE = (LVM_FIRST + 40),
             LVM_GETORIGIN = (LVM_FIRST + 41),
             LVM_UPDATE = (LVM_FIRST + 42),
             LVM_SETITEMSTATE = (LVM_FIRST + 43),
             LVM_GETITEMSTATE = (LVM_FIRST + 44),
             LVM_GETITEMTEXTA = (LVM_FIRST + 45),
             LVM_GETITEMTEXT = LVM_GETITEMTEXTA,
             LVM_SETITEMTEXTA = (LVM_FIRST + 46),
             LVM_SETITEMTEXT = LVM_SETITEMTEXTA,
             LVM_SETITEMCOUNT = (LVM_FIRST + 47),
             LVM_SORTITEMS = (LVM_FIRST + 48),
             LVM_SETITEMPOSITION32 = (LVM_FIRST + 49),
             LVM_GETSELECTEDCOUNT = (LVM_FIRST + 50),
             LVM_GETITEMSPACING = (LVM_FIRST + 51),
             LVM_GETISEARCHSTRINGA = (LVM_FIRST + 52),
             LVM_GETISEARCHSTRING = LVM_GETISEARCHSTRINGA,
             LVM_SETICONSPACING = (LVM_FIRST + 53),
             LVM_SETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 54),
             LVM_GETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 55),
             LVM_GETSUBITEMRECT = (LVM_FIRST + 56),
             LVM_SUBITEMHITTEST = (LVM_FIRST + 57),
             LVM_SETCOLUMNORDERARRAY = (LVM_FIRST + 58),
             LVM_GETCOLUMNORDERARRAY = (LVM_FIRST + 59),
             LVM_SETHOTITEM = (LVM_FIRST + 60),
             LVM_GETHOTITEM = (LVM_FIRST + 61),
             LVM_SETHOTCURSOR = (LVM_FIRST + 62),
             LVM_GETHOTCURSOR = (LVM_FIRST + 63),
             LVM_APPROXIMATEVIEWRECT = (LVM_FIRST + 64),
             LVS_EX_FULLROWSELECT = 0x20,
             WM_ACTIVATE = 0x6,
             WM_ACTIVATEAPP = 0x1C,
             WM_ASKCBFORMATNAME = 0x30C,
             WM_CANCELJOURNAL = 0x4B,
             WM_CANCELMODE = 0x1F,
             WM_CHANGECBCHAIN = 0x30D,
             WM_CHAR = 0x102,
             WM_CHARTOITEM = 0x2F,
             WM_CHILDACTIVATE = 0x22,
             WM_CHOOSEFONT_GETLOGFONT = (WM_USER + 1),
             WM_CHOOSEFONT_SETFLAGS = (WM_USER + 102),
             WM_CHOOSEFONT_SETLOGFONT = (WM_USER + 101),
             WM_CLEAR = 0x303,
             WM_CLOSE = 0x10,
             WM_COMMAND = 0x111,
             WM_COMMNOTIFY = 0x44,
             WM_COMPACTING = 0x41,
             WM_COMPAREITEM = 0x39,
             WM_CONVERTREQUESTEX = 0x108,
             WM_COPY = 0x301,
             WM_COPYDATA = 0x4A,
             WM_CREATE = 0x1,
             WM_CTLCOLORBTN = 0x135,
             WM_CTLCOLORDLG = 0x136,
             WM_CTLCOLOREDIT = 0x133,
             WM_CTLCOLORLISTBOX = 0x134,
             WM_CTLCOLORMSGBOX = 0x132,
             WM_CTLCOLORSCROLLBAR = 0x137,
             WM_CTLCOLORSTATIC = 0x138,
             WM_CUT = 0x300,
             WM_DDE_FIRST = 0x3E0,
             WM_DDE_ACK = (WM_DDE_FIRST + 4),
             WM_DDE_ADVISE = (WM_DDE_FIRST + 2),
             WM_DDE_DATA = (WM_DDE_FIRST + 5),
             WM_DDE_EXECUTE = (WM_DDE_FIRST + 8),
             WM_DDE_INITIATE = (WM_DDE_FIRST),
             WM_DDE_LAST = (WM_DDE_FIRST + 8),
             WM_DDE_POKE = (WM_DDE_FIRST + 7),
             WM_DDE_REQUEST = (WM_DDE_FIRST + 6),
             WM_DDE_TERMINATE = (WM_DDE_FIRST + 1),
             WM_DDE_UNADVISE = (WM_DDE_FIRST + 3),
             WM_DEADCHAR = 0x103,
             WM_DELETEITEM = 0x2D,
             WM_DESTROY = 0x2,
             WM_DESTROYCLIPBOARD = 0x307,
             WM_DEVMODECHANGE = 0x1B,
             WM_DRAWCLIPBOARD = 0x308,
             WM_DRAWITEM = 0x2B,
             WM_DROPFILES = 0x233,
             WM_ENABLE = 0xA,
             WM_ENDSESSION = 0x16,
             WM_ENTERIDLE = 0x121,
             WM_ENTERMENULOOP = 0x211,
             WM_ERASEBKGND = 0x14,
             WM_EXITMENULOOP = 0x212,
             WM_FONTCHANGE = 0x1D,
             WM_GETFONT = 0x31,
             WM_GETDLGCODE = 0x87,
             WM_GETHOTKEY = 0x33,
             WM_GETMINMAXINFO = 0x24,
             WM_GETTEXT = 0xD,
             WM_GETTEXTLENGTH = 0xE,
             WM_HOTKEY = 0x312,
             WM_HSCROLL = 0x114,
             WM_HSCROLLCLIPBOARD = 0x30E,
             WM_ICONERASEBKGND = 0x27,
             WM_IME_CHAR = 0x286,
             WM_IME_COMPOSITION = 0x10F,
             WM_IME_COMPOSITIONFULL = 0x284,
             WM_IME_CONTROL = 0x283,
             WM_IME_ENDCOMPOSITION = 0x10E,
             WM_IME_KEYDOWN = 0x290,
             WM_IME_KEYLAST = 0x10F,
             WM_IME_KEYUP = 0x291,
             WM_IME_NOTIFY = 0x282,
             WM_IME_SELECT = 0x285,
             WM_IME_SETCONTEXT = 0x281,
             WM_IME_STARTCOMPOSITION = 0x10D,
             WM_INITDIALOG = 0x110,
             WM_INITMENU = 0x116,
             WM_INITMENUPOPUP = 0x117,
             WM_KEYDOWN = 0x100,
             WM_KEYFIRST = 0x100,
             WM_KEYLAST = 0x108,
             WM_KEYUP = 0x101,
             WM_KILLFOCUS = 0x8,
             WM_LBUTTONDBLCLK = 0x203,
             WM_LBUTTONDOWN = 0x201,
             WM_LBUTTONUP = 0x202,
             WM_MBUTTONDBLCLK = 0x209,
             WM_MBUTTONDOWN = 0x207,
             WM_MBUTTONUP = 0x208,
             WM_MDIACTIVATE = 0x222,
             WM_MDICASCADE = 0x227,
             WM_MDICREATE = 0x220,
             WM_MDIDESTROY = 0x221,
             WM_MDIGETACTIVE = 0x229,
             WM_MDIICONARRANGE = 0x228,
             WM_MDIMAXIMIZE = 0x225,
             WM_MDINEXT = 0x224,
             WM_MDIREFRESHMENU = 0x234,
             WM_MDIRESTORE = 0x223,
             WM_MDISETMENU = 0x230,
             WM_MDITILE = 0x226,
             WM_MEASUREITEM = 0x2C,
             WM_MENUCHAR = 0x120,
             WM_MENUSELECT = 0x11F,
             WM_MOUSEACTIVATE = 0x21,
             WM_MOUSEFIRST = 0x200,
             WM_MOUSELAST = 0x209,
             WM_MOUSEMOVE = 0x200,
             WM_MOVE = 0x3,
             WM_NCACTIVATE = 0x86,
             WM_NCCALCSIZE = 0x83,
             WM_NCCREATE = 0x81,
             WM_NCDESTROY = 0x82,
             WM_NCHITTEST = 0x84,
             WM_NCLBUTTONDBLCLK = 0xA3,
             WM_NCLBUTTONDOWN = 0xA1,
             WM_NCLBUTTONUP = 0xA2,
             WM_NCMBUTTONDBLCLK = 0xA9,
             WM_NCMBUTTONDOWN = 0xA7,
             WM_NCMBUTTONUP = 0xA8,
             WM_NCMOUSEMOVE = 0xA0,
             WM_NCPAINT = 0x85,
             WM_NCRBUTTONDBLCLK = 0xA6,
             WM_NCRBUTTONDOWN = 0xA4,
             WM_NCRBUTTONUP = 0xA5,
             WM_NEXTDLGCTL = 0x28,
             WM_NULL = 0x0,
             WM_PAINT = 0xF,
             WM_PAINTCLIPBOARD = 0x309,
             WM_PAINTICON = 0x26,
             WM_PALETTECHANGED = 0x311,
             WM_PALETTEISCHANGING = 0x310,
             WM_PARENTNOTIFY = 0x210,
             WM_PASTE = 0x302,
             WM_PENWINFIRST = 0x380,
             WM_PENWINLAST = 0x38F,
             WM_POWER = 0x48,
             WM_PSD_ENVSTAMPRECT = (WM_USER + 5),
             WM_PSD_FULLPAGERECT = (WM_USER + 1),
             WM_PSD_GREEKTEXTRECT = (WM_USER + 4),
             WM_PSD_MARGINRECT = (WM_USER + 3),
             WM_PSD_MINMARGINRECT = (WM_USER + 2),
             WM_PSD_PAGESETUPDLG = (WM_USER),
             WM_PSD_YAFULLPAGERECT = (WM_USER + 6),
             WM_QUERYDRAGICON = 0x37,
             WM_QUERYENDSESSION = 0x11,
             WM_QUERYNEWPALETTE = 0x30F,
             WM_QUERYOPEN = 0x13,
             WM_QUEUESYNC = 0x23,
             WM_QUIT = 0x12,
             WM_RBUTTONDBLCLK = 0x206,
             WM_RBUTTONDOWN = 0x204,
             WM_RBUTTONUP = 0x205,
             WM_RENDERALLFORMATS = 0x306,
             WM_RENDERFORMAT = 0x305,
             WM_SETCURSOR = 0x20,
             WM_SETFOCUS = 0x7,
             WM_SETFONT = 0x30,
             WM_SETHOTKEY = 0x32,
             WM_SETREDRAW = 0xB,
             WM_SETTEXT = 0xC,
             WM_SHOWWINDOW = 0x18,
             WM_SIZE = 0x5,
             WM_SIZECLIPBOARD = 0x30B,
             WM_SPOOLERSTATUS = 0x2A,
             WM_SYSCHAR = 0x106,
             WM_SYSCOLORCHANGE = 0x15,
             WM_SYSCOMMAND = 0x112,
             WM_SYSDEADCHAR = 0x107,
             WM_SYSKEYDOWN = 0x104,
             WM_SYSKEYUP = 0x105,
             WM_TIMECHANGE = 0x1E,
             WM_TIMER = 0x113,
             WM_UNDO = 0x304,
             WM_VKEYTOITEM = 0x2E,
             WM_VSCROLL = 0x115,
             WM_VSCROLLCLIPBOARD = 0x30A,
             WM_WINDOWPOSCHANGED = 0x47,
             WM_WINDOWPOSCHANGING = 0x46,
             WM_WININICHANGE = 0x1A,
             WS_BORDER = 0x800000,
             WS_CAPTION = 0xC00000 ,
             WS_CHILD = 0x40000000,
             WS_CHILDWINDOW = (WS_CHILD),
             WS_CLIPCHILDREN = 0x2000000,
             WS_CLIPSIBLINGS = 0x4000000,
             WS_DISABLED = 0x8000000,
             WS_DLGFRAME = 0x400000,
             WS_EX_ACCEPTFILES = 0x10,
             WS_EX_DLGMODALFRAME = 0x1,
             WS_EX_NOPARENTNOTIFY = 0x4,
             WS_EX_TOPMOST = 0x8,
             WS_EX_TRANSPARENT = 0x20,
             WS_GROUP = 0x20000,
             WS_HSCROLL = 0x100000,
             WS_MINIMIZE = 0x20000000,
             WS_ICONIC = WS_MINIMIZE,
             WS_MAXIMIZE = 0x1000000,
             WS_MAXIMIZEBOX = 0x10000,
             WS_MINIMIZEBOX = 0x20000,
             WS_SYSMENU = 0x80000,
             WS_THICKFRAME = 0x40000,
             WS_OVERLAPPED = 0x0,
             WS_POPUP = 0x80000000,
             WS_SIZEBOX = WS_THICKFRAME,
             WS_TABSTOP = 0x10000,
             WS_TILED = WS_OVERLAPPED,
             WS_VISIBLE = 0x10000000,
             WS_VSCROLL = 0x200000,
             LBS_DISABLENOSCROLL = 0x1000,
             LBS_EXTENDEDSEL = 0x800,
             LBS_HASSTRINGS = 0x40,
             LBS_MULTICOLUMN = 0x200,
             LBS_MULTIPLESEL = 0x8,
             LBS_NODATA = 0x2000,
             LBS_NOINTEGRALHEIGHT = 0x100,
             LBS_NOREDRAW = 0x4,
             LBS_NOTIFY = 0x1,
             LBS_OWNERDRAWFIXED = 0x10,
             LBS_OWNERDRAWVARIABLE = 0x20,
             LBS_SORT = 0x2,
             LBS_USETABSTOPS = 0x80,
             LBS_WANTKEYBOARDINPUT = 0x400,
             TB_ENABLEBUTTON = (WM_USER + 1),
             TB_CHECKBUTTON = (WM_USER + 2),
             TB_PRESSBUTTON = (WM_USER + 3),
             TB_HIDEBUTTON = (WM_USER + 4),
             TB_INDETERMINATE = (WM_USER + 5),
             TB_MARKBUTTON = (WM_USER + 6),
             TB_ISBUTTONENABLED = (WM_USER + 9),
             TB_ISBUTTONCHECKED = (WM_USER + 10),
             TB_ISBUTTONPRESSED = (WM_USER + 11),
             TB_ISBUTTONHIDDEN = (WM_USER + 12),
             TB_ISBUTTONINDETERMINATE = (WM_USER + 13),
             TB_ISBUTTONHIGHLIGHTED = (WM_USER + 14),
             TB_SETSTATE = (WM_USER + 17),
             TB_GETSTATE = (WM_USER + 18),
             TB_ADDBITMAP = (WM_USER + 19),
             TB_ADDBUTTONSA = (WM_USER + 20),
             TB_INSERTBUTTONA = (WM_USER + 21),
             TB_ADDBUTTONS = (WM_USER + 20),
             TB_INSERTBUTTON = (WM_USER + 21),
             TB_DELETEBUTTON = (WM_USER + 22),
             TB_GETBUTTON = (WM_USER + 23),
             TB_BUTTONCOUNT = (WM_USER + 24),
             TB_COMMANDTOINDEX = (WM_USER + 25),
             TB_SAVERESTOREA = (WM_USER + 26),
             TB_SAVERESTOREW = (WM_USER + 76),
             TB_CUSTOMIZE = (WM_USER + 27),
             TB_ADDSTRINGA = (WM_USER + 28),
             TB_ADDSTRINGW = (WM_USER + 77),
             TB_GETITEMRECT = (WM_USER + 29),
             TB_BUTTONSTRUCTSIZE = (WM_USER + 30),
             TB_SETBUTTONSIZE = (WM_USER + 31),
             TB_SETBITMAPSIZE = (WM_USER + 32),
             TB_AUTOSIZE = (WM_USER + 33),
             TB_GETTOOLTIPS = (WM_USER + 35),
             TB_SETTOOLTIPS = (WM_USER + 36),
             TB_SETPARENT = (WM_USER + 37),
             TB_SETROWS = (WM_USER + 39),
             TB_GETROWS = (WM_USER + 40),
             TB_SETCMDID = (WM_USER + 42),
             TB_CHANGEBITMAP = (WM_USER + 43),
             TB_GETBITMAP = (WM_USER + 44),
             TB_GETBUTTONTEXTA = (WM_USER + 45),
             TB_GETBUTTONTEXTW = (WM_USER + 75),
             TB_REPLACEBITMAP = (WM_USER + 46),
             TB_SETINDENT = (WM_USER + 47),
             TB_SETIMAGELIST = (WM_USER + 48),
             TB_GETIMAGELIST = (WM_USER + 49),
             TB_LOADIMAGES = (WM_USER + 50),
             TB_GETRECT = (WM_USER + 51),
             TB_SETHOTIMAGELIST = (WM_USER + 52),
             TB_GETHOTIMAGELIST = (WM_USER + 53),
             TB_SETDISABLEDIMAGELIST = (WM_USER + 54),
             TB_GETDISABLEDIMAGELIST = (WM_USER + 55),
             TB_SETSTYLE = (WM_USER + 56),
             TB_GETSTYLE = (WM_USER + 57),
             TB_GETBUTTONSIZE = (WM_USER + 58),
             TB_SETBUTTONWIDTH = (WM_USER + 59),
             TB_SETMAXTEXTROWS = (WM_USER + 60),
             TB_GETTEXTROWS = (WM_USER + 61),
             TBSTYLE_BUTTON = 0x0,
             TBSTYLE_SEP = 0x1,
             TBSTYLE_CHECK = 0x2,
             TBSTYLE_GROUP = 0x4,
             TBSTYLE_DROPDOWN = 0x8,
             TBSTYLE_AUTOSIZE = 0x10,
             TBSTYLE_NOPREFIX = 0x20,
             TBSTYLE_TOOLTIPS = 0x100,
             TBSTYLE_WRAPABLE = 0x200,
             TBSTYLE_ALTDRAG = 0x400,
             TBSTYLE_FLAT = 0x800,
             TBSTYLE_LIST = 0x1000,
             TBSTYLE_CUSTOMERASE = 0x2000,
             TBSTYLE_REGISTERDROP = 0x4000,
             TBSTYLE_TRANSPARENT = 0x8000,
             TBSTYLE_EX_DRAWDDARROWS = 0x1

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public uint type;
            public InputUnion inputunion;
            public static int Size
            {
                get { return Marshal.SizeOf(typeof(INPUT)); }
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mouseinput;
            [FieldOffset(0)]
            public KEYBDINPUT keyboardinput;
            [FieldOffset(0)]
            public HARDWAREINPUT hardwareinput;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public MouseEventDataXButtons mouseData;
            public MOUSEEVENTF dwFlags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }
        [Flags]
        public enum MouseEventDataXButtons : uint
        {
            Nothing = 0x00000000,
            XBUTTON1 = 0x00000001,
            XBUTTON2 = 0x00000002
        }

        [Flags]
        public enum MOUSEEVENTF : uint
        {
            ABSOLUTE = 0x8000,
            HWHEEL = 0x01000,
            MOVE = 0x0001,
            MOVE_NOCOALESCE = 0x2000,
            LEFTDOWN = 0x0002,
            LEFTUP = 0x0004,
            RIGHTDOWN = 0x0008,
            RIGHTUP = 0x0010,
            MIDDLEDOWN = 0x0020,
            MIDDLEUP = 0x0040,
            VIRTUALDESK = 0x4000,
            WHEEL = 0x0800,
            XDOWN = 0x0080,
            XUP = 0x0100
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public VirtualKeyShort wVk;
            public ScanCodeShort wScan;
            public KEYEVENTF dwFlags;
            public int time;
            public UIntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }
        [Flags]
        public enum KEYEVENTF : uint
        {
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            SCANCODE = 0x0008,
            UNICODE = 0x0004
        }
        public enum VirtualKeyShort : short
        {
            ///<summary>
            ///Left mouse button
            ///</summary>
            LBUTTON = 0x01,
            ///<summary>
            ///Right mouse button
            ///</summary>
            RBUTTON = 0x02,
            ///<summary>
            ///Control-break processing
            ///</summary>
            CANCEL = 0x03,
            ///<summary>
            ///Middle mouse button (three-button mouse)
            ///</summary>
            MBUTTON = 0x04,
            ///<summary>
            ///Windows 2000/XP: X1 mouse button
            ///</summary>
            XBUTTON1 = 0x05,
            ///<summary>
            ///Windows 2000/XP: X2 mouse button
            ///</summary>
            XBUTTON2 = 0x06,
            ///<summary>
            ///BACKSPACE key
            ///</summary>
            BACK = 0x08,
            ///<summary>
            ///TAB key
            ///</summary>
            TAB = 0x09,
            ///<summary>
            ///CLEAR key
            ///</summary>
            CLEAR = 0x0C,
            ///<summary>
            ///ENTER key
            ///</summary>
            RETURN = 0x0D,
            ///<summary>
            ///SHIFT key
            ///</summary>
            SHIFT = 0x10,
            ///<summary>
            ///CTRL key
            ///</summary>
            CONTROL = 0x11,
            ///<summary>
            ///ALT key
            ///</summary>
            MENU = 0x12,
            ///<summary>
            ///PAUSE key
            ///</summary>
            PAUSE = 0x13,
            ///<summary>
            ///CAPS LOCK key
            ///</summary>
            CAPITAL = 0x14,
            ///<summary>
            ///Input Method Editor (IME) Kana mode
            ///</summary>
            KANA = 0x15,
            ///<summary>
            ///IME Hangul mode
            ///</summary>
            HANGUL = 0x15,
            ///<summary>
            ///IME Junja mode
            ///</summary>
            JUNJA = 0x17,
            ///<summary>
            ///IME final mode
            ///</summary>
            FINAL = 0x18,
            ///<summary>
            ///IME Hanja mode
            ///</summary>
            HANJA = 0x19,
            ///<summary>
            ///IME Kanji mode
            ///</summary>
            KANJI = 0x19,
            ///<summary>
            ///ESC key
            ///</summary>
            ESCAPE = 0x1B,
            ///<summary>
            ///IME convert
            ///</summary>
            CONVERT = 0x1C,
            ///<summary>
            ///IME nonconvert
            ///</summary>
            NONCONVERT = 0x1D,
            ///<summary>
            ///IME accept
            ///</summary>
            ACCEPT = 0x1E,
            ///<summary>
            ///IME mode change request
            ///</summary>
            MODECHANGE = 0x1F,
            ///<summary>
            ///SPACEBAR
            ///</summary>
            SPACE = 0x20,
            ///<summary>
            ///PAGE UP key
            ///</summary>
            PRIOR = 0x21,
            ///<summary>
            ///PAGE DOWN key
            ///</summary>
            NEXT = 0x22,
            ///<summary>
            ///END key
            ///</summary>
            END = 0x23,
            ///<summary>
            ///HOME key
            ///</summary>
            HOME = 0x24,
            ///<summary>
            ///LEFT ARROW key
            ///</summary>
            LEFT = 0x25,
            ///<summary>
            ///UP ARROW key
            ///</summary>
            UP = 0x26,
            ///<summary>
            ///RIGHT ARROW key
            ///</summary>
            RIGHT = 0x27,
            ///<summary>
            ///DOWN ARROW key
            ///</summary>
            DOWN = 0x28,
            ///<summary>
            ///SELECT key
            ///</summary>
            SELECT = 0x29,
            ///<summary>
            ///PRINT key
            ///</summary>
            PRINT = 0x2A,
            ///<summary>
            ///EXECUTE key
            ///</summary>
            EXECUTE = 0x2B,
            ///<summary>
            ///PRINT SCREEN key
            ///</summary>
            SNAPSHOT = 0x2C,
            ///<summary>
            ///INS key
            ///</summary>
            INSERT = 0x2D,
            ///<summary>
            ///DEL key
            ///</summary>
            DELETE = 0x2E,
            ///<summary>
            ///HELP key
            ///</summary>
            HELP = 0x2F,
            ///<summary>
            ///0 key
            ///</summary>
            KEY_0 = 0x30,
            ///<summary>
            ///1 key
            ///</summary>
            KEY_1 = 0x31,
            ///<summary>
            ///2 key
            ///</summary>
            KEY_2 = 0x32,
            ///<summary>
            ///3 key
            ///</summary>
            KEY_3 = 0x33,
            ///<summary>
            ///4 key
            ///</summary>
            KEY_4 = 0x34,
            ///<summary>
            ///5 key
            ///</summary>
            KEY_5 = 0x35,
            ///<summary>
            ///6 key
            ///</summary>
            KEY_6 = 0x36,
            ///<summary>
            ///7 key
            ///</summary>
            KEY_7 = 0x37,
            ///<summary>
            ///8 key
            ///</summary>
            KEY_8 = 0x38,
            ///<summary>
            ///9 key
            ///</summary>
            KEY_9 = 0x39,
            ///<summary>
            ///A key
            ///</summary>
            KEY_A = 0x41,
            ///<summary>
            ///B key
            ///</summary>
            KEY_B = 0x42,
            ///<summary>
            ///C key
            ///</summary>
            KEY_C = 0x43,
            ///<summary>
            ///D key
            ///</summary>
            KEY_D = 0x44,
            ///<summary>
            ///E key
            ///</summary>
            KEY_E = 0x45,
            ///<summary>
            ///F key
            ///</summary>
            KEY_F = 0x46,
            ///<summary>
            ///G key
            ///</summary>
            KEY_G = 0x47,
            ///<summary>
            ///H key
            ///</summary>
            KEY_H = 0x48,
            ///<summary>
            ///I key
            ///</summary>
            KEY_I = 0x49,
            ///<summary>
            ///J key
            ///</summary>
            KEY_J = 0x4A,
            ///<summary>
            ///K key
            ///</summary>
            KEY_K = 0x4B,
            ///<summary>
            ///L key
            ///</summary>
            KEY_L = 0x4C,
            ///<summary>
            ///M key
            ///</summary>
            KEY_M = 0x4D,
            ///<summary>
            ///N key
            ///</summary>
            KEY_N = 0x4E,
            ///<summary>
            ///O key
            ///</summary>
            KEY_O = 0x4F,
            ///<summary>
            ///P key
            ///</summary>
            KEY_P = 0x50,
            ///<summary>
            ///Q key
            ///</summary>
            KEY_Q = 0x51,
            ///<summary>
            ///R key
            ///</summary>
            KEY_R = 0x52,
            ///<summary>
            ///S key
            ///</summary>
            KEY_S = 0x53,
            ///<summary>
            ///T key
            ///</summary>
            KEY_T = 0x54,
            ///<summary>
            ///U key
            ///</summary>
            KEY_U = 0x55,
            ///<summary>
            ///V key
            ///</summary>
            KEY_V = 0x56,
            ///<summary>
            ///W key
            ///</summary>
            KEY_W = 0x57,
            ///<summary>
            ///X key
            ///</summary>
            KEY_X = 0x58,
            ///<summary>
            ///Y key
            ///</summary>
            KEY_Y = 0x59,
            ///<summary>
            ///Z key
            ///</summary>
            KEY_Z = 0x5A,
            ///<summary>
            ///Left Windows key (Microsoft Natural keyboard) 
            ///</summary>
            LWIN = 0x5B,
            ///<summary>
            ///Right Windows key (Natural keyboard)
            ///</summary>
            RWIN = 0x5C,
            ///<summary>
            ///Applications key (Natural keyboard)
            ///</summary>
            APPS = 0x5D,
            ///<summary>
            ///Computer Sleep key
            ///</summary>
            SLEEP = 0x5F,
            ///<summary>
            ///Numeric keypad 0 key
            ///</summary>
            NUMPAD0 = 0x60,
            ///<summary>
            ///Numeric keypad 1 key
            ///</summary>
            NUMPAD1 = 0x61,
            ///<summary>
            ///Numeric keypad 2 key
            ///</summary>
            NUMPAD2 = 0x62,
            ///<summary>
            ///Numeric keypad 3 key
            ///</summary>
            NUMPAD3 = 0x63,
            ///<summary>
            ///Numeric keypad 4 key
            ///</summary>
            NUMPAD4 = 0x64,
            ///<summary>
            ///Numeric keypad 5 key
            ///</summary>
            NUMPAD5 = 0x65,
            ///<summary>
            ///Numeric keypad 6 key
            ///</summary>
            NUMPAD6 = 0x66,
            ///<summary>
            ///Numeric keypad 7 key
            ///</summary>
            NUMPAD7 = 0x67,
            ///<summary>
            ///Numeric keypad 8 key
            ///</summary>
            NUMPAD8 = 0x68,
            ///<summary>
            ///Numeric keypad 9 key
            ///</summary>
            NUMPAD9 = 0x69,
            ///<summary>
            ///Multiply key
            ///</summary>
            MULTIPLY = 0x6A,
            ///<summary>
            ///Add key
            ///</summary>
            ADD = 0x6B,
            ///<summary>
            ///Separator key
            ///</summary>
            SEPARATOR = 0x6C,
            ///<summary>
            ///Subtract key
            ///</summary>
            SUBTRACT = 0x6D,
            ///<summary>
            ///Decimal key
            ///</summary>
            DECIMAL = 0x6E,
            ///<summary>
            ///Divide key
            ///</summary>
            DIVIDE = 0x6F,
            ///<summary>
            ///F1 key
            ///</summary>
            F1 = 0x70,
            ///<summary>
            ///F2 key
            ///</summary>
            F2 = 0x71,
            ///<summary>
            ///F3 key
            ///</summary>
            F3 = 0x72,
            ///<summary>
            ///F4 key
            ///</summary>
            F4 = 0x73,
            ///<summary>
            ///F5 key
            ///</summary>
            F5 = 0x74,
            ///<summary>
            ///F6 key
            ///</summary>
            F6 = 0x75,
            ///<summary>
            ///F7 key
            ///</summary>
            F7 = 0x76,
            ///<summary>
            ///F8 key
            ///</summary>
            F8 = 0x77,
            ///<summary>
            ///F9 key
            ///</summary>
            F9 = 0x78,
            ///<summary>
            ///F10 key
            ///</summary>
            F10 = 0x79,
            ///<summary>
            ///F11 key
            ///</summary>
            F11 = 0x7A,
            ///<summary>
            ///F12 key
            ///</summary>
            F12 = 0x7B,
            ///<summary>
            ///F13 key
            ///</summary>
            F13 = 0x7C,
            ///<summary>
            ///F14 key
            ///</summary>
            F14 = 0x7D,
            ///<summary>
            ///F15 key
            ///</summary>
            F15 = 0x7E,
            ///<summary>
            ///F16 key
            ///</summary>
            F16 = 0x7F,
            ///<summary>
            ///F17 key  
            ///</summary>
            F17 = 0x80,
            ///<summary>
            ///F18 key  
            ///</summary>
            F18 = 0x81,
            ///<summary>
            ///F19 key  
            ///</summary>
            F19 = 0x82,
            ///<summary>
            ///F20 key  
            ///</summary>
            F20 = 0x83,
            ///<summary>
            ///F21 key  
            ///</summary>
            F21 = 0x84,
            ///<summary>
            ///F22 key, (PPC only) Key used to lock device.
            ///</summary>
            F22 = 0x85,
            ///<summary>
            ///F23 key  
            ///</summary>
            F23 = 0x86,
            ///<summary>
            ///F24 key  
            ///</summary>
            F24 = 0x87,
            ///<summary>
            ///NUM LOCK key
            ///</summary>
            NUMLOCK = 0x90,
            ///<summary>
            ///SCROLL LOCK key
            ///</summary>
            SCROLL = 0x91,
            ///<summary>
            ///Left SHIFT key
            ///</summary>
            LSHIFT = 0xA0,
            ///<summary>
            ///Right SHIFT key
            ///</summary>
            RSHIFT = 0xA1,
            ///<summary>
            ///Left CONTROL key
            ///</summary>
            LCONTROL = 0xA2,
            ///<summary>
            ///Right CONTROL key
            ///</summary>
            RCONTROL = 0xA3,
            ///<summary>
            ///Left MENU key
            ///</summary>
            LMENU = 0xA4,
            ///<summary>
            ///Right MENU key
            ///</summary>
            RMENU = 0xA5,
            ///<summary>
            ///Windows 2000/XP: Browser Back key
            ///</summary>
            BROWSER_BACK = 0xA6,
            ///<summary>
            ///Windows 2000/XP: Browser Forward key
            ///</summary>
            BROWSER_FORWARD = 0xA7,
            ///<summary>
            ///Windows 2000/XP: Browser Refresh key
            ///</summary>
            BROWSER_REFRESH = 0xA8,
            ///<summary>
            ///Windows 2000/XP: Browser Stop key
            ///</summary>
            BROWSER_STOP = 0xA9,
            ///<summary>
            ///Windows 2000/XP: Browser Search key 
            ///</summary>
            BROWSER_SEARCH = 0xAA,
            ///<summary>
            ///Windows 2000/XP: Browser Favorites key
            ///</summary>
            BROWSER_FAVORITES = 0xAB,
            ///<summary>
            ///Windows 2000/XP: Browser Start and Home key
            ///</summary>
            BROWSER_HOME = 0xAC,
            ///<summary>
            ///Windows 2000/XP: Volume Mute key
            ///</summary>
            VOLUME_MUTE = 0xAD,
            ///<summary>
            ///Windows 2000/XP: Volume Down key
            ///</summary>
            VOLUME_DOWN = 0xAE,
            ///<summary>
            ///Windows 2000/XP: Volume Up key
            ///</summary>
            VOLUME_UP = 0xAF,
            ///<summary>
            ///Windows 2000/XP: Next Track key
            ///</summary>
            MEDIA_NEXT_TRACK = 0xB0,
            ///<summary>
            ///Windows 2000/XP: Previous Track key
            ///</summary>
            MEDIA_PREV_TRACK = 0xB1,
            ///<summary>
            ///Windows 2000/XP: Stop Media key
            ///</summary>
            MEDIA_STOP = 0xB2,
            ///<summary>
            ///Windows 2000/XP: Play/Pause Media key
            ///</summary>
            MEDIA_PLAY_PAUSE = 0xB3,
            ///<summary>
            ///Windows 2000/XP: Start Mail key
            ///</summary>
            LAUNCH_MAIL = 0xB4,
            ///<summary>
            ///Windows 2000/XP: Select Media key
            ///</summary>
            LAUNCH_MEDIA_SELECT = 0xB5,
            ///<summary>
            ///Windows 2000/XP: Start Application 1 key
            ///</summary>
            LAUNCH_APP1 = 0xB6,
            ///<summary>
            ///Windows 2000/XP: Start Application 2 key
            ///</summary>
            LAUNCH_APP2 = 0xB7,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_1 = 0xBA,
            ///<summary>
            ///Windows 2000/XP: For any country/region, the '+' key
            ///</summary>
            OEM_PLUS = 0xBB,
            ///<summary>
            ///Windows 2000/XP: For any country/region, the ',' key
            ///</summary>
            OEM_COMMA = 0xBC,
            ///<summary>
            ///Windows 2000/XP: For any country/region, the '-' key
            ///</summary>
            OEM_MINUS = 0xBD,
            ///<summary>
            ///Windows 2000/XP: For any country/region, the '.' key
            ///</summary>
            OEM_PERIOD = 0xBE,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_2 = 0xBF,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard. 
            ///</summary>
            OEM_3 = 0xC0,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard. 
            ///</summary>
            OEM_4 = 0xDB,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard. 
            ///</summary>
            OEM_5 = 0xDC,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard. 
            ///</summary>
            OEM_6 = 0xDD,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard. 
            ///</summary>
            OEM_7 = 0xDE,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_8 = 0xDF,
            ///<summary>
            ///Windows 2000/XP: Either the angle bracket key or the backslash key on the RT 102-key keyboard
            ///</summary>
            OEM_102 = 0xE2,
            ///<summary>
            ///Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key
            ///</summary>
            PROCESSKEY = 0xE5,
            ///<summary>
            ///Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes.
            ///The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information,
            ///see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
            ///</summary>
            PACKET = 0xE7,
            ///<summary>
            ///Attn key
            ///</summary>
            ATTN = 0xF6,
            ///<summary>
            ///CrSel key
            ///</summary>
            CRSEL = 0xF7,
            ///<summary>
            ///ExSel key
            ///</summary>
            EXSEL = 0xF8,
            ///<summary>
            ///Erase EOF key
            ///</summary>
            EREOF = 0xF9,
            ///<summary>
            ///Play key
            ///</summary>
            PLAY = 0xFA,
            ///<summary>
            ///Zoom key
            ///</summary>
            ZOOM = 0xFB,
            ///<summary>
            ///Reserved 
            ///</summary>
            NONAME = 0xFC,
            ///<summary>
            ///PA1 key
            ///</summary>
            PA1 = 0xFD,
            ///<summary>
            ///Clear key
            ///</summary>
            OEM_CLEAR = 0xFE
        }
        public enum ScanCodeShort : short
        {
            LBUTTON = 0,
            RBUTTON = 0,
            CANCEL = 70,
            MBUTTON = 0,
            XBUTTON1 = 0,
            XBUTTON2 = 0,
            BACK = 14,
            TAB = 15,
            CLEAR = 76,
            RETURN = 28,
            SHIFT = 42,
            CONTROL = 29,
            MENU = 56,
            PAUSE = 0,
            CAPITAL = 58,
            KANA = 0,
            HANGUL = 0,
            JUNJA = 0,
            FINAL = 0,
            HANJA = 0,
            KANJI = 0,
            ESCAPE = 1,
            CONVERT = 0,
            NONCONVERT = 0,
            ACCEPT = 0,
            MODECHANGE = 0,
            SPACE = 57,
            PRIOR = 73,
            NEXT = 81,
            END = 79,
            HOME = 71,
            LEFT = 75,
            UP = 72,
            RIGHT = 77,
            DOWN = 80,
            SELECT = 0,
            PRINT = 0,
            EXECUTE = 0,
            SNAPSHOT = 84,
            INSERT = 82,
            DELETE = 83,
            HELP = 99,
            KEY_0 = 11,
            KEY_1 = 2,
            KEY_2 = 3,
            KEY_3 = 4,
            KEY_4 = 5,
            KEY_5 = 6,
            KEY_6 = 7,
            KEY_7 = 8,
            KEY_8 = 9,
            KEY_9 = 10,
            KEY_A = 30,
            KEY_B = 48,
            KEY_C = 46,
            KEY_D = 32,
            KEY_E = 18,
            KEY_F = 33,
            KEY_G = 34,
            KEY_H = 35,
            KEY_I = 23,
            KEY_J = 36,
            KEY_K = 37,
            KEY_L = 38,
            KEY_M = 50,
            KEY_N = 49,
            KEY_O = 24,
            KEY_P = 25,
            KEY_Q = 16,
            KEY_R = 19,
            KEY_S = 31,
            KEY_T = 20,
            KEY_U = 22,
            KEY_V = 47,
            KEY_W = 17,
            KEY_X = 45,
            KEY_Y = 21,
            KEY_Z = 44,
            LWIN = 91,
            RWIN = 92,
            APPS = 93,
            SLEEP = 95,
            NUMPAD0 = 82,
            NUMPAD1 = 79,
            NUMPAD2 = 80,
            NUMPAD3 = 81,
            NUMPAD4 = 75,
            NUMPAD5 = 76,
            NUMPAD6 = 77,
            NUMPAD7 = 71,
            NUMPAD8 = 72,
            NUMPAD9 = 73,
            MULTIPLY = 55,
            ADD = 78,
            SEPARATOR = 0,
            SUBTRACT = 74,
            DECIMAL = 83,
            DIVIDE = 53,
            F1 = 59,
            F2 = 60,
            F3 = 61,
            F4 = 62,
            F5 = 63,
            F6 = 64,
            F7 = 65,
            F8 = 66,
            F9 = 67,
            F10 = 68,
            F11 = 87,
            F12 = 88,
            F13 = 100,
            F14 = 101,
            F15 = 102,
            F16 = 103,
            F17 = 104,
            F18 = 105,
            F19 = 106,
            F20 = 107,
            F21 = 108,
            F22 = 109,
            F23 = 110,
            F24 = 118,
            NUMLOCK = 69,
            SCROLL = 70,
            LSHIFT = 42,
            RSHIFT = 54,
            LCONTROL = 29,
            RCONTROL = 29,
            LMENU = 56,
            RMENU = 56,
            BROWSER_BACK = 106,
            BROWSER_FORWARD = 105,
            BROWSER_REFRESH = 103,
            BROWSER_STOP = 104,
            BROWSER_SEARCH = 101,
            BROWSER_FAVORITES = 102,
            BROWSER_HOME = 50,
            VOLUME_MUTE = 32,
            VOLUME_DOWN = 46,
            VOLUME_UP = 48,
            MEDIA_NEXT_TRACK = 25,
            MEDIA_PREV_TRACK = 16,
            MEDIA_STOP = 36,
            MEDIA_PLAY_PAUSE = 34,
            LAUNCH_MAIL = 108,
            LAUNCH_MEDIA_SELECT = 109,
            LAUNCH_APP1 = 107,
            LAUNCH_APP2 = 33,
            OEM_1 = 39,
            OEM_PLUS = 13,
            OEM_COMMA = 51,
            OEM_MINUS = 12,
            OEM_PERIOD = 52,
            OEM_2 = 53,
            OEM_3 = 41,
            OEM_4 = 26,
            OEM_5 = 43,
            OEM_6 = 27,
            OEM_7 = 40,
            OEM_8 = 0,
            OEM_102 = 86,
            PROCESSKEY = 0,
            PACKET = 0,
            ATTN = 0,
            CRSEL = 0,
            EXSEL = 0,
            EREOF = 93,
            PLAY = 0,
            ZOOM = 98,
            NONAME = 0,
            PA1 = 0,
            OEM_CLEAR = 0,
        }


#endregion

    }
    public static class MessageExt
    {
        public static char ToChar(this user32.Messages m)
        {
            return (char)m;
        }
    }
}
