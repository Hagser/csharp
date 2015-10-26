using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyAutoBrightness
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //BrightnessAPI.SetBrightness(short.Parse(trackBar1.Value+""));
            byte b = (byte)trackBar1.Value;

            BrightnessWMI.SetBrightness(b);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //pictureBox1.Image = null;            
        }
    }
}
