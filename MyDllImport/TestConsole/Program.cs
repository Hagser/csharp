using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using MyDllImport;
using System.Drawing;
using System.Windows.Forms;

namespace TestConsole
{
    class Program
    {
        private static void Main(string[] args)
        {
            #region Göm
            /*
            List<user32.WindowInfo> windows = new List<user32.WindowInfo>();
            user32.CallBack cb = new user32.CallBack((a, b) => { windows.Add(user32.getWindowInfo(a)); return true; });
            user32.getWindows(cb, IntPtr.Zero);


            Console.WriteLine("windows.Count:"+windows.Count);

            List<Process> processes = new List<Process>(Process.GetProcesses());

            foreach (user32.WindowInfo wi in windows.Where(x => x.rcWindow.Width != 0 && x.rcWindow.Height != 0))
            {
                Process p = processes.FirstOrDefault(x => x.MainWindowHandle == wi.handle);
                Console.WriteLine(wi.handle + ":" + (p != null ? p.ProcessName : "nope"));
            }


            Console.WriteLine("getActiveWindow:"+user32.getActiveWindow());
            IntPtr fw = user32.getForegroundWindow();
            Console.WriteLine("getForegroundWindow:"+fw);
            user32.Rect r;
            user32.getWindowRect(fw,out r);
            Console.WriteLine(r.Bottom + "/" + r.Top+"/"+r.Left + "/" + r.Right);
            Thread.Sleep(1000);

            r.Width = r.Width - 50;
            r.Height = r.Height - 50;

            user32.setWindowPos(fw, IntPtr.Zero, r, user32.SetWindowPosFlags.AsynchronousWindowPosition);
            Thread.Sleep(1000);
            user32.getWindowRect(fw, out r);
            Console.WriteLine(r.Bottom + "/" + r.Top + "/" + r.Left + "/" + r.Right);
            Console.WriteLine(kernel32.getLastError());
            */
            /*
            Dictionary<System.DateTime, user32.POINT> points = new Dictionary<DateTime, user32.POINT>();
            for (int i = 0; i < 1000; i++)
            {
                user32.POINT p = user32.getCursorPos();
                points.Add(System.DateTime.Now,p);
                Console.WriteLine(p.X + ":" + p.Y);
                Thread.Sleep(10);
            }
            for (int x = 0; x < 5; x++)
            {
                Console.WriteLine("Redo!");
                Thread.Sleep(3000);
                IEnumerable<user32.POINT> psod = (x % 1 == 0) ? points.OrderByDescending(y => y.Key).Select(z => z.Value) : points.OrderBy(y => y.Key).Select(z => z.Value);
                foreach (user32.POINT p in psod)
                {
                    user32.setCursorPos(p);
                    Console.WriteLine(p.X + ":" + p.Y);
                    Thread.Sleep(10);
                }
            }
            */
            /*
            var hWindow = user32.findWindow("notepad", null);
            user32.flashWindow(hWindow, false);
            user32.setForegroundWindow(hWindow);
            Thread.Sleep(500);
            var pInputs = new[] { 
                new user32.INPUT()
                {                    
                    type = user32.INPUT_KEYBOARD,
                    inputunion = new user32.InputUnion(){
                        keyboardinput= new user32.KEYBDINPUT()
                        {
                            wScan = user32.ScanCodeShort.KEY_S,
                            wVk = user32.VirtualKeyShort.KEY_S
                        }
                    }
                },
            
                new user32.INPUT()
                {                    
                    type = user32.INPUT_KEYBOARD,
                    inputunion = new user32.InputUnion(){
                        keyboardinput= new user32.KEYBDINPUT()
                        {
                            wScan = user32.ScanCodeShort.HOME,
                            wVk = user32.VirtualKeyShort.HOME
                        }
                    }
                },
                new user32.INPUT()
                {                    
                    type = user32.INPUT_KEYBOARD,
                    inputunion = new user32.InputUnion(){
                        keyboardinput= new user32.KEYBDINPUT()
                        {
                            wScan = user32.ScanCodeShort.KEY_0,
                            wVk = user32.VirtualKeyShort.KEY_0
                        }
                    }
                },
                new user32.INPUT()
                {                    
                    type = user32.INPUT_KEYBOARD,
                    inputunion = new user32.InputUnion(){
                        keyboardinput= new user32.KEYBDINPUT()
                        {
                            wScan = user32.ScanCodeShort.END,
                            wVk = user32.VirtualKeyShort.END
                        }
                    }
                },
                new user32.INPUT()
                {                    
                    type = user32.INPUT_KEYBOARD,
                    inputunion = new user32.InputUnion(){
                        keyboardinput= new user32.KEYBDINPUT()
                        {
                            wScan = user32.ScanCodeShort.KEY_9,
                            wVk = user32.VirtualKeyShort.KEY_9
                        }
                    }
                },
            };
            user32.sendInput(pInputs);
            */
            #endregion

        }


    }
}
