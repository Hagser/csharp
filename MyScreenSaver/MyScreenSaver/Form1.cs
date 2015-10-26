using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BC=Box2DX.Common;

namespace MyScreenSaver
{
    public partial class Form1 : Form
    {
        #region Preview API's

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion
        bool IsPreviewMode;
        PerformanceCounter Counter;
        //This constructor is passed the bounds this form is to show in
        //It is used when in normal mode
        public Form1(Rectangle Bounds,PerformanceCounter counter)
        {
            InitializeComponent();
            this.Bounds = Bounds;
            this.Location = Bounds.Location;
            this.Counter = counter;
            //hide the cursor
            Cursor.Hide();

        }

        //This constructor is the handle to the select 
        //screensaver dialog preview window
        //It is used when in preview mode (/p)
        public Form1(IntPtr PreviewHandle)
        {
            InitializeComponent();

            //set the preview window as the parent of this window
            SetParent(this.Handle, PreviewHandle);

            //make this a child window, so when the select 
            //screensaver dialog closes, this will also close
            SetWindowLong(this.Handle, -16,
              new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            //set our window's size to the size of our window's new parent
            Rectangle ParentRect;
            GetClientRect(PreviewHandle, out ParentRect);
            this.Size = ParentRect.Size;

            //set our location at (0, 0)
            this.Location = new Point(0, 0);

            IsPreviewMode = true;
        }
        #region User Input

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //** take this if statement out if your not doing a preview
            if (!IsPreviewMode) //disable exit functions for preview
            {
                Application.Exit();
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            //** take this if statement out if your not doing a preview
            if (!IsPreviewMode) //disable exit functions for preview
            {
                Application.Exit();
            }
        }

        //start off OriginalLoction with an X and Y of int.MaxValue, because
        //it is impossible for the cursor to be at that position. That way, we
        //know if this variable has been set yet.
        Point OriginalLocation = new Point(int.MaxValue, int.MaxValue);

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //** take this if statement out if your not doing a preview
            if (!IsPreviewMode) //disable exit functions for preview
            {
                //see if originallocation has been set
                if (OriginalLocation.X == int.MaxValue &
                    OriginalLocation.Y == int.MaxValue)
                {
                    OriginalLocation = e.Location;
                }
                //see if the mouse has moved more than 20 pixels 
                //in any direction. If it has, close the application.
                if (Math.Abs(e.X - OriginalLocation.X) > 20 |
                    Math.Abs(e.Y - OriginalLocation.Y) > 20)
                {
                    Application.Exit();
                }
            }
        }
        #endregion
        int icnt = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(this.Width, this.Height);
            if (Counter != null && !string.IsNullOrEmpty(Counter.CategoryName) && !string.IsNullOrEmpty(Counter.CounterName))
            {
                timer1.Enabled = true;
                float f = Counter.NextValue();
                values.Add(long.Parse(Math.Floor(f).ToString()));
                averages.Add(long.Parse(Math.Floor(values.Average()).ToString()));
                Draw();
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Counter != null)
            {
                float f = Counter.NextValue();
                values.Add(long.Parse(Math.Floor(f).ToString()));
                averages.Add(long.Parse(Math.Floor(values.Average()).ToString()));
                Draw();
            }
        }
        List<long> values = new List<long>();
        List<long> averages = new List<long>();
        private void Draw()
        {
            long ilastval = 0;
            long imax = Counter.CounterName.Contains("%") ? 100 : values.Max(x => x) + 1;// GetCounterMax(Counter.CounterName);
            int imod = 100;
            double iw = pictureBox1.Image.Width;
            double ih = pictureBox1.Image.Height;
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, this.Width, this.Height));

            Font f = new Font(this.Font.FontFamily,24,FontStyle.Bold);
            g.DrawString(Counter.CategoryName + "-" + Counter.InstanceName + "-" + Counter.CounterName+": "+values.Last(), f, Brushes.Gray, new PointF(20, this.Height / 8));
            if (values.Count > 2)
            {
                icnt=0;
                Point op = Point.Empty;
                List<Point> points = new List<Point>();
                foreach (long ival in values.Take(imod - 1))
                {
                    double dicnt = icnt % imod;
                    double dival = ival;

                    int h = int.Parse(Math.Floor((ih - ((dival / imax) * ih)) - 10).ToString());
                    int w = int.Parse(Math.Floor((dicnt / imod) * iw).ToString());
                        
                    Point p1 = new Point(w, h<0?1:h);
                    if (!op.IsEmpty)
                    {
                        points.Add(op);
                        points.Add(p1);

                        op = p1;
                        p1 = new Point(op.X+5, op.Y);
                        points.Add(p1);
                    }
                    op = p1;
                    icnt++;
                    ilastval = ival;
                }

                g.DrawLines(Pens.LightYellow, points.ToArray());
                
                if (values.Count > imod)
                {
                    values.Clear();
                    values.Add(ilastval);
                    System.GC.Collect();
                }
                icnt = 0;
                points.Clear();
                op = Point.Empty;

                foreach (long ival in averages.Take(imod - 1))
                {
                    double dicnt = icnt % imod;
                    double dival = ival;

                    int h = int.Parse(Math.Floor((ih - ((dival / imax) * ih)) - 10).ToString());
                    int w = int.Parse(Math.Floor((dicnt / imod) * iw).ToString());

                    Point p1 = new Point(w, h < 0 ? 1 : h);
                    if (!op.IsEmpty)
                    {
                        points.Add(op);
                        points.Add(p1);

                        op = p1;
                        p1 = new Point(op.X + 5, op.Y);
                        points.Add(p1);
                    }
                    op = p1;
                    icnt++;
                    ilastval = ival;
                }

                g.DrawLines(Pens.LightBlue, points.ToArray());
                g.Flush();
                pictureBox1.Refresh();
            }

            if (averages.Count > imod)
            {
                averages.Clear();
                averages.Add(ilastval);
                System.GC.Collect();
            }
        }

        private int GetCounterMax(string p)
        {
            if(p.Contains("%"))
                return 100;
            if(p.Replace(" ","").ToLower().Contains("kb/s"))
                return 1000;
            if (p.Replace(" ", "").ToLower().Contains("bytes/s"))
                return 10000000;

            return 1000;

        }
    }
}
