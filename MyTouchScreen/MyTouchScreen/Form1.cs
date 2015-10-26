using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace MyTouchScreen
{
    public partial class Form1 : Form
    {
        Capture _capture;
        Board board = new Board();

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            Image img = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(img);

            Screen s = Screen.FromHandle(board.Handle);            
            g.CopyFromScreen(0,0,0,0,new Size(s.WorkingArea.Width,s.WorkingArea.Height));
            //pictureBox1.Image = img;
            
            Image<Bgr, Byte> frame = _capture.QueryFrame();
            //pictureBox1.Image = frame.Bitmap;

            EyeOpen.Imaging.ComparableImage ci = new EyeOpen.Imaging.ComparableImage(img as Bitmap);
            double dblSim = ci.CalculateSimilarity(new EyeOpen.Imaging.ComparableImage(frame.Bitmap));
            this.Text = "Similarity:" + dblSim + "_" + s.DeviceName;
            if(dblSim>.6)
                pictureBox1.Image = frame.Bitmap;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _capture = new Capture(1);

            Screen s = Screen.FromHandle(board.Handle); 
            _capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, s.WorkingArea.Width);
            _capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, s.WorkingArea.Height);
            timer1.Enabled = true;
            board.Show();
        }
    }
}
