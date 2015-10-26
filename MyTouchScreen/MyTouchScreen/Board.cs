using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyTouchScreen
{
    public partial class Board : Form
    {
        public Board()
        {
            InitializeComponent();
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            drawLines();
            this.Text = pictureBox1.Width+"";
        }
        public void drawLines()
        {

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            Pen pen = new Pen(Color.Black, 5);
            int iw = int.Parse(Math.Round(double.Parse(pictureBox1.Width.ToString()) / 10).ToString());
            int ih = int.Parse(Math.Round(double.Parse(pictureBox1.Width.ToString()) / 10).ToString());
            for (int w = 0; w <= iw; w++)
            {
                Point p1 = new Point((w * iw), 0);
                Point p2 = new Point((w * iw), pictureBox1.Height - 40);
                g.DrawLine(pen, p1, p2);
            }
            g.Save();
            pictureBox1.Image = bmp;
        }
        private void Board_Resize(object sender, EventArgs e)
        {
            pictureBox1.Width = this.Width - 10;
            pictureBox1.Height = this.Height - 10;
        }

    }
}
