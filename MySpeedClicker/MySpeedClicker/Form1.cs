using MyDllImport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySpeedClicker
{
    public partial class Form1 : Form
    {

        globalMouseHook gmh = new globalMouseHook();
        globalKeyboardHook gkh = new globalKeyboardHook();
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            gkh.KeyDown += g_KeyDown;
            //gkh.KeyUp += g_KeyUp;
            //gmh.MouseEvent += gmh_MouseEvent;
            

        }
        bool bMouseDown = false;
        void gmh_MouseEvent(object sender, MouseEventArgs e)
        {
            MyPoint pp = new MyPoint(e.X, e.Y);
            switch (e.Delta)
            { 
                case 513://LeftDown
                    pp.Click = user32.MOUSEEVENTF.LEFTDOWN;
                    bMouseDown = true;
                    break;
                case 514://LeftUp
                    pp.Click = user32.MOUSEEVENTF.LEFTUP;
                    bMouseDown = false;
                    break;
                case 519://MiddleDown
                    pp.Click = user32.MOUSEEVENTF.MIDDLEDOWN;
                    break;
                case 520://MiddleUp
                    pp.Click = user32.MOUSEEVENTF.MIDDLEUP;
                    break;
                case 516://RightDown
                    pp.Click = user32.MOUSEEVENTF.RIGHTDOWN;
                    break;
                case 517://RightDown
                    pp.Click = user32.MOUSEEVENTF.RIGHTUP;
                    break;
            }
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            g.DrawLine(Pens.Black, op.AsPoint, pp.AsPoint);
            g.Save();
            g.Dispose();
            pictureBox1.Refresh();

            TimeSpan ts = new TimeSpan(pp.Tick - (op != null ? op.Tick : 0));
            op = pp;

            if(ts.TotalMilliseconds>5||pp.Click.HasValue)
                macro.Add(pp);
        }
        int ipoint = 0;
        List<MyPoint> macro = new List<MyPoint>();
        bool? bRecorded;
        DateTime dtLast = new DateTime();
        DateTime dtLastSend = new DateTime();
        void g_KeyDown(object sender, KeyEventArgs e)
        {
            this.Text = "down:" + e.KeyValue;
            if (e.KeyCode == Keys.C && catchCToolStripMenuItem.Checked)
            {
                TimeSpan tsdiff = new TimeSpan(DateTime.Now.Ticks - dtLast.Ticks);
                if (tsdiff.TotalSeconds > 1)
                {
                    doClicksToolStripMenuItem.Checked = !doClicksToolStripMenuItem.Checked;
                    dtLast = DateTime.Now;
                }
            }
            else {
                if (e.KeyCode == Keys.M)
                {
                    if (!bRecorded.HasValue || (bRecorded.HasValue && bRecorded.Value))
                    {
                        bRecorded=false;
                        macro = new List<MyPoint>();
                        //timer1.Enabled = true;
                        this.Text = "Recording";

                        gmh.MouseEvent += gmh_MouseEvent;
                    }
                    else if (bRecorded.HasValue && !bRecorded.Value)
                    {
                        this.Text = "Recorded";

                        gmh.MouseEvent -= gmh_MouseEvent;
                        bRecorded = true;

                        var tsi = macrosToolStripMenuItem.DropDownItems.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        tsi.Tag = macro.ToList();
                        tsi.MouseHover += (a, b) =>
                        {
                            this.Text = tsi.Text;
                        };
                        tsi.Click += (a, b) =>
                        {
                            var t = a as ToolStripItem;
                            if (t != null && t.Tag != null && (t.Tag as List<MyPoint>) != null)
                            {
                                bRecorded = true;
                                macro = t.Tag as List<MyPoint>;
                            }
                        };
                    }
                    else
                    {
                        this.Text = "!Recorded";

                        bRecorded = null;
                    }
                }
                else if (e.KeyCode == Keys.P)
                {
                    if (bRecorded.HasValue && bRecorded.Value && macro.Count > 1)
                    {
                        ipoint = 0;
                        bDoPlay = true;
                        this.Text = "Playing";
                        ThreadPool.QueueUserWorkItem(StartPlay);


                    }
                
                }
                else if (e.KeyCode == Keys.S)
                {
                    this.Text = "Stopped";
                    bDoPlay = false;
                }/*
                else if (e.KeyCode == Keys.Q)
                {
                    this.Text = "Click";

                    if (bRecorded.HasValue && !bRecorded.Value)
                    {
                        macro[macro.Count-1].Click = true;
                    }
                }*/
            }
            this.Text += "," + doClicksToolStripMenuItem.Checked + ".lost:" + bLostFocus;
        }
        bool bDoPlay;
        private void StartPlay(object state)
        {
            while (ipoint < macro.Count && bDoPlay)
            {
                var mstart = macro[ipoint];
                if (ipoint < macro.Count - 1)
                {
                    var mnext = macro[ipoint + 1];
                    TimeSpan ts = new TimeSpan(mnext.Tick - mstart.Tick);
                    DoMouseEvent(mstart);
                    Thread.Sleep(int.Parse(Math.Floor(ts.TotalMilliseconds).ToString()));
                    ipoint++;
                }
                else {
                    DoMouseEvent(mstart);
                }
            }
            bDoPlay = false;
        }

        private void DoMouseEvent(MyPoint p)
        {
            user32.setCursorPos(p.AsPoint);
            if (p.Click.HasValue)
            {
                user32.sendInput(new[]{
                        new user32.INPUT()
                        {
                            inputunion=new user32.InputUnion()
                            {
                                mouseinput = new user32.MOUSEINPUT() { dwFlags = p.Click.Value, dx = p.X, dy = p.Y }
                            }
                        }
                    });
            }
        }
        MyPoint op = new MyPoint();
        bool bLostFocus;
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                user32.POINT p = user32.getCursorPos();
                MyPoint pp = new MyPoint(p.X, p.Y);
                if (bRecorded.HasValue && !bRecorded.Value)
                {
                    macro.Add(pp);
                }
                Graphics g = Graphics.FromImage(pictureBox1.Image);
                g.DrawLine(Pens.Black, op.AsPoint, pp.AsPoint);
                op = pp;
                g.Save();
                g.Dispose();
                pictureBox1.Refresh();

                if (doClicksToolStripMenuItem.Checked && bLostFocus && bMouseDown)
                {
                    TimeSpan tsdiff = new TimeSpan(DateTime.Now.Ticks - dtLastSend.Ticks);
                    if (tsdiff.TotalMilliseconds > timer1.Interval)
                    {
                        dtLastSend = DateTime.Now;
                        try
                        {

                            user32.sendInput(
                                new[]{
                        new user32.INPUT()
                        {
                            inputunion=new user32.InputUnion(){
                            mouseinput = new user32.MOUSEINPUT() { dwFlags = user32.MOUSEEVENTF.LEFTDOWN, dx = p.X, dy = p.Y }
                        }
                        },
                        new user32.INPUT()
                        {
                            inputunion=new user32.InputUnion(){
                            mouseinput = new user32.MOUSEINPUT() { dwFlags = user32.MOUSEEVENTF.LEFTUP, dx = p.X, dy = p.Y }
                        }
                        }
                        }
                                );
                        }
                        catch (Exception ex)
                        {
                            this.Text = ex.Message;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.Text = ex.Message;
            }
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            bLostFocus = true;
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            bLostFocus = false;
        }

        private void doClicksToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = doClicksToolStripMenuItem.Checked;
            if (timer1.Enabled)
                pictureBox1.Image = new Bitmap(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            return;
            try
            {
                if (timer1.Enabled)
                {
                    gkh.KeyDown += g_KeyDown;
                }
                else
                {
                    gkh.KeyDown -= g_KeyDown;
                }

            }
            catch (Exception ex)
            {
                this.Text = ex.Message;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            gkh.unhook();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            ipoint++;
            if (ipoint >= macro.Count)
                ipoint = 0;

            try
            {
                MyPoint p = macro[ipoint];
                DoMouseEvent(p);
            }
            catch (Exception ex)
            {
                string sösldföl = ex.Message;
            }
        }

    }

    public class MyPoint {
        public int X { get; set; }
        public int Y { get; set; }
        public user32.MOUSEEVENTF? Click { get; set; }
        public long Tick { get; set; }
        public Point AsPoint { get { return new Point(X, Y); } set { } }
        public MyPoint()
        {
            Tick = DateTime.Now.Ticks;
        }
        public MyPoint(int x,int y)
        {
            X = x;
            Y = y;
            Tick = DateTime.Now.Ticks;
        }
    }
}
