using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyRoofMeasurer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool bPaint = false;
        Point pStart = new Point();
        Point pEnd = new Point();
        List<Point> points = new List<Point>();
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            bPaint = true;
            pStart = e.Location;
            this.Text = "Down";
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            this.Text = "Move";
            if (bPaint)
            {
                this.Text = "Paint";
                if (pictureBox1.Image != null)
                {
                    Graphics g = Graphics.FromImage(pictureBox1.Image);
                    g.DrawLine(Pens.Gray, pStart, e.Location);
                    g.Flush();
                    g.Dispose();
                    pictureBox1.Refresh();
                    this.Text = "Draw";
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            bPaint = false;
            pEnd = e.Location;
            points.Add(pStart);
            points.Add(pEnd);
            this.Text = "Up";
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            points.Clear();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (points.Count > 0)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g = Graphics.FromImage(pictureBox1.Image);
                g.DrawLines(Pens.Black, points.ToArray());
                g.Flush();
                g.Dispose();
                pictureBox1.Refresh();
            }
        }
    }
}
