using MyDllImport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace MySendKeys
{
    public partial class Form1 : Form
    {




        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        Bitmap bmp;
        //globalKeyboardHook gkh = new globalKeyboardHook();
        Dictionary<long, int> histKeys = new Dictionary<long, int>();
        public Form1()
        {
            InitializeComponent();
            bmp = Bitmap.FromFile(Application.StartupPath + "\\hit.png") as Bitmap;
            bmp = Bitmap.FromFile(Application.StartupPath + "\\shift.png") as Bitmap;
            bmp = Bitmap.FromFile(Application.StartupPath + "\\fight.png") as Bitmap;
            
            //iwidth = bmp.Width;
            //iheight = bmp.Height;
            //this.Width = iwidth + 16;
            //this.Height = iheight + 23;
        }

        //static int xstart = 306;
        //static int ystart = 475;
        //static int iwidth = 45;
        //static int iheight = 25;
        //static int xstart = 886;
        //static int ystart = 196;
        //static int iwidth = 152;
        //static int iheight = 144;
        static int xstart = 1033;
        static int ystart = 705;
        static int iwidth = 214;
        static int iheight = 71;

        int icnt = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            ScreenCapture sc = new ScreenCapture();
            if (xstart > 0 && ystart > 0 && iwidth > 0 && iheight > 0 && !setSizeToolStripMenuItem.Checked)
            {
                if (icnt > 150)
                {
                    //pictureBox1.Image = null;
                }
                Image img = sc.CaptureScreen(xstart, ystart, iwidth, iheight);
                
                EyeOpen.Imaging.ComparableImage ci = new EyeOpen.Imaging.ComparableImage(bmp);
                double dblSim = ci.CalculateSimilarity(new EyeOpen.Imaging.ComparableImage(img as Bitmap));
                if (dblSim > .1)
                {
                    pictureBox1.Image = img;
                }
                bool bWhat = dblSim > .82;
                    
                if (bWhat)// && icnt>5)
                {
                    this.Text = Math.Round(dblSim, 4).ToString() + ":" + System.DateTime.Now.ToString("HH:mm:ss")+"_"+icnt;
                    Bitmap b = CreateBitmap(Color.Red, new Point(pictureBox1.Width, pictureBox1.Height));
                    pictureBox1.Image = b;
                    
                    timer1.Enabled = false;
                    
                    user32.setCursorPos(xstart + 20, ystart + 20);
                    user32.sendInput(new[]{
                        new user32.MOUSEINPUT() { dwFlags = user32.MOUSEEVENTF.LEFTDOWN,dwExtraInfo=(UIntPtr)user32.Messages.WM_LBUTTONDOWN, dx = xstart + 20, dy = ystart + 20 },
                        new user32.MOUSEINPUT() { dwFlags = user32.MOUSEEVENTF.LEFTUP,dwExtraInfo=(UIntPtr)user32.Messages.WM_LBUTTONUP, dx = xstart + 20, dy = ystart + 20 }
                    });

                    System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
                    t.Interval = 2000;
                    t.Enabled = true;
                    t.Tick += (a,x) => {
                        user32.setCursorPos(xstart - 20, ystart - 20);
                        timer1.Enabled = true;
                        t.Enabled = false;
                    };

                    //mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, xstart, ystart, icnt, icnt);
                    icnt++;
                }
                if (saveToolStripMenuItem.Checked)
                {
                    img.Save(Application.CommonAppDataPath + "\\" + System.DateTime.Now.Ticks.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
                //icnt++;
            }
        }

        private Bitmap CreateBitmap(Color color,Point size)
        {
            Bitmap b = new Bitmap(size.X, size.Y);
            for (int x = 0; x < b.Size.Width; x++)
            {
                for (int y = 0; y < b.Size.Height; y++)
                {
                    b.SetPixel(x, y, color);
                }
            }
            return b;
        }

        private bool CompareImages(Bitmap bmp1, Bitmap bmp2)
        {
            for (int x = 0; x < bmp.Size.Width; x++)
            {
                for (int y = 0; y < bmp.Size.Height; y++)
                {
                    if (bmp1.GetPixel(x, y) != bmp2.GetPixel(x, y))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            if (setSizeToolStripMenuItem.Checked)
            {
                xstart = this.Left;
                ystart = this.Top;
                iwidth = this.Width;
                iheight = this.Height;
            }
        }

        private void setSizeToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            this.Opacity = (setSizeToolStripMenuItem.Checked ? .3 : 1);
            this.TopMost = setSizeToolStripMenuItem.Checked;
            if (!setSizeToolStripMenuItem.Checked)
            {
                pictureBox1.Image = null;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                timer3.Enabled = false;

            switch (e.KeyCode)
            { 
                case Keys.Up:
                    if (e.Control)
                    {
                        iheight += 5;
                    }
                    else
                    {
                        ystart -= 5;
                    }
                    break;
                case Keys.Down:
                    if (e.Control)
                    {
                        iheight -= 5;
                    }
                    else
                    {
                        ystart += 5;
                    }
                    break;
                case Keys.Left:
                    if (e.Control)
                    {
                        iwidth -= 5;
                    }
                    else
                    {
                        xstart -= 5;
                    }
                    break;
                case Keys.Right:
                    if (e.Control)
                    {
                        iwidth += 5;
                    }
                    else
                    {
                        xstart += 5;
                    }
                    break;
            }
            /*
            string pn = "nfsw";
            IntPtr nh = Process.GetProcessesByName(pn).Count() > 0 ? Process.GetProcessesByName(pn)[0].MainWindowHandle : IntPtr.Zero;
            if (nh != IntPtr.Zero)
            {
                UInt32 kd = user32.Messages.WM_CHAR.ToChar();
                this.Text = nh+"";
                PostAMessage(nh, kd, (char)e.KeyValue, 0);
            }
            */
        }
        private int le { get { return Marshal.GetLastWin32Error(); } }
        private void timer2_Tick(object sender, EventArgs e)
        {
            string pn = "nfsw";
            IntPtr ah =  user32.getActiveWindow();
            if (ah == IntPtr.Zero)
                ah = user32.getForegroundWindow();
            IntPtr nh = Process.GetProcessesByName(pn).Count()>0 ? Process.GetProcessesByName(pn)[0].MainWindowHandle:IntPtr.Zero;
            IntPtr fw = user32.findWindowByCaption(Process.GetProcessesByName(pn).Count() > 0 ? Process.GetProcessesByName(pn)[0].MainWindowTitle : "");
            //user32.flashWindow(nh, true);

            Point p = user32.getCursorPos();
            this.Text = ah + ":" + nh + ":" + fw + "_" + p.X + "x" + p.Y;
            //user32.setCursorPos(p.X + 100, p.Y + 100);
            //p = user32.getCursorPos();
            //this.Text += "_" + p.X + "x" + p.Y;
            if (ah != IntPtr.Zero && nh != IntPtr.Zero && ah == nh)
            {

                UInt32 kd = user32.Messages.WM_KEYDOWN.ToChar();//0x0100;// key_Down
                UInt32 ku = user32.Messages.WM_KEYUP.ToChar();//0x0101;// key_Up                

                UInt32 shift = Keys.LShiftKey.ToChar();
                UInt32 ctrl = Keys.LControlKey.ToChar();
                UInt32 right = Keys.Right.ToChar();
                UInt32 left = Keys.Left.ToChar();
                UInt32 up = Keys.Up.ToChar();
                UInt32 down = Keys.Down.ToChar();
                user32.sendInput(new user32.KEYBDINPUT() { dwFlags = user32.KEYEVENTF.SCANCODE, wScan = user32.ScanCodeShort.CONTROL, wVk = user32.VirtualKeyShort.CONTROL });
                user32.sendInput(new user32.KEYBDINPUT() { dwFlags = user32.KEYEVENTF.KEYUP, wScan = user32.ScanCodeShort.CONTROL, wVk = user32.VirtualKeyShort.CONTROL });
                /*
                //Shift 
                if (PostAMessage(nh, kd, shift, 0x02A00001))
                {
                    PostAMessage(nh, ku, shift, 0xC2A00001);
                }
                //Ctrl 
                if (PostAMessage(nh, kd, ctrl, 0x01D00001))
                {
                    PostAMessage(nh, ku, ctrl, 0xC1D00001);
                }
                //Right 
                if (PostAMessage(nh, kd, right, 0x014D0001))
                {
                    PostAMessage(nh, ku, right, 0xC14D0001);
                }
                //Left
                if (PostAMessage(nh, kd, left, 0x014B0001))
                {
                    PostAMessage(nh, ku, left, 0xC14B0001);
                }
                //Up
                if (PostAMessage(nh, kd, up, 0x01480001))
                {
                    PostAMessage(nh, ku, up, 0x41480001);
                }
                //Down
                if (PostAMessage(nh, kd, down, 0x01500001))
                {
                    PostAMessage(nh, ku, down, 0xC1500001);
                }
                */
                //this.Text += "_" + System.DateTime.Now.ToString("HH:mm:ss");
            }
        }
        private bool PostAMessage(IntPtr hWnd, UInt32 Msg, UInt32 wParam, UInt32 lParam)
        {
            bool bret = user32.postMessage(hWnd, Msg, wParam, lParam);

            System.Threading.Thread.Sleep(100);
            string s = new Win32Exception(Marshal.GetLastWin32Error()).Message;

            this.Text += "_" + s;
            return bret;
        }
        private void topmostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = topmostToolStripMenuItem.Checked;
        }
        int ipscnt = 0;
        UInt32[] ips = new UInt32[]{
            0x023A03EA,
            0x023A03EA,
            0x023A03EA,
            0x01A103AF,
            0x022B03BE
        };
        Point[] ps = new Point[]{
            new Point(0x023A,0x03EA),
            new Point(0x023A,0x03EA),
            new Point(0x023A,0x03EA),
            new Point(0x01A1,0x03AF),
            new Point(0x022B,0x03BE)
        };
        private void timer3_Tick(object sender, EventArgs e)
        {

            IntPtr ah = user32.getActiveWindow();
            if (ah == IntPtr.Zero)
                ah = user32.getForegroundWindow();

            
            var input = new user32.KEYBDINPUT[]
            {
                new user32.KEYBDINPUT() { dwFlags = user32.KEYEVENTF.SCANCODE, wScan = user32.ScanCodeShort.SPACE, wVk = user32.VirtualKeyShort.SPACE },
                new user32.KEYBDINPUT() { dwFlags = user32.KEYEVENTF.KEYUP, wScan = user32.ScanCodeShort.SPACE, wVk = user32.VirtualKeyShort.SPACE }
            };
            if (ah == (IntPtr)787622)
                user32.sendInput(input);           

        }

        private void dragToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer3.Enabled = dragToolStripMenuItem.Checked;
            if (!timer3.Enabled)
                ipscnt = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {/*
            gkh.KeyDown += (a, b) =>
            {
                if(!histKeys.ContainsKey(DateTime.Now.Ticks))
                    histKeys.Add(DateTime.Now.Ticks, b.KeyValue);
                this.Text = "Down:" + b.KeyValue;
                //b.Handled = true;
            };
            gkh.KeyUp += (a, b) =>
            {
                this.Text += " Up:" + b.KeyValue;
                //b.Handled = true;
            };
          */
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (setSizeToolStripMenuItem.Checked)
            {
                xstart = this.Left;
                ystart = this.Top;
                iwidth = this.Width;
                iheight = this.Height;
            }
        }


    }
    public static class KeysExt
    {
        public static char ToChar(this Keys k)
        {
            return (char)k;
        }
    }
}
