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

namespace MyTakeABreak
{
    public partial class Form1 : Form
    {
        public Form1()
        {
#if !DEBUG
            this.ControlBox = false;
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
#endif
            InitializeComponent();
        }
        int panic = 0;
        int rows = 0;
        int cols = 0;
        int height = 0;
        int width = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            int min = 30;
            
#if !DEBUG
            int sec = 60;
#else            
            int sec = 1;
#endif
            int ms = 1000;
            timer1.Interval = min * sec * ms;
            timer2.Interval = 5 * sec * ms;
            timer1.Enabled = true;
            Rectangle ssize = Screen.PrimaryScreen.Bounds;
            height = ssize.Height;
            width = ssize.Width;
            rows = int.Parse(Math.Floor(double.Parse(height.ToString()) / 5).ToString());
            cols = int.Parse(Math.Floor(double.Parse(width.ToString()) / 5).ToString());
            pictureBox1.Image = new Bitmap(ssize.Width, ssize.Height);

            //ThreadPool.QueueUserWorkItem(BlankScreen, Brushes.Silver);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            timer1.Enabled = false;
            timer2.Enabled = true;
            ThreadPool.QueueUserWorkItem(BlankScreen, Brushes.Black);
        }

        private void BlankScreen(object state)
        {
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            Random rnd = new Random(int.Parse(DateTime.Now.Millisecond.ToString()));
            double iall = rows * cols;
            double icnt = 0;
            double operc = 0;
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < cols; col++)
                {
                    if (bClose)
                        return;
                    Thread.Sleep(1);

                    g.FillRectangle(state as Brush, new Rectangle(rnd.Next(cols) * 5, rnd.Next(rows) * 5, 5, 5));

                    if(col%50==0)
                        this.Invoke((MethodInvoker)delegate{if(pictureBox1!=null)pictureBox1.Refresh();});

                    icnt++;
                    /*
                    double perc = icnt / iall;
                    if(operc!=perc)
                        this.Invoke((MethodInvoker)delegate { this.Opacity = perc; });

                    operc = perc;
                     */
                }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            timer2.Enabled = false;
            timer1.Enabled = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        bool bClose = false;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            bClose = true;
        }
    }
}
