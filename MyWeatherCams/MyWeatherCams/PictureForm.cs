using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyWeatherCams
{
    public partial class PictureForm : Form
    {
        public System.Windows.Forms.Timer tim = new System.Windows.Forms.Timer();
        FileInfo[] files=new FileInfo[1];
        public PictureForm(FileInfo[] _files)
        {
            InitializeComponent();
            files = _files;
            Load += PictureForm_Load;
        }

        void PictureForm_Load(object sender, EventArgs e)
        {
            int idx = 0;
            GC.Collect();
            tim.Tick += (a, b) =>
            {
                if (idx < files.Length)
                {
                    FileInfo file = files[idx];
                    this.Text = string.Format("{0}/{1}", idx, files.Length);
                    if (this.Controls.Count > 0)
                    {
                        try
                        {
                            pictureBox1.Image = Bitmap.FromFile(file.FullName);
                            Graphics g = Graphics.FromImage(pictureBox1.Image);
                            g.FillRectangle(Brushes.Silver, 0, pictureBox1.Image.Height - 22, 97, 22);
                            g.DrawString(file.LastWriteTime.ToString("yyyy-MM-dd HH:mm"), this.Font, Brushes.Black, new PointF(2, pictureBox1.Image.Height - 18));
                        }
                        catch { }
                        GC.Collect();
                        idx++;
                        if (idx > files.Length)
                        {
                            idx = 0;
                        }
                    }
                    else
                    {
                        tim.Enabled = false;
                        tim = null;
                    }
                }
                else
                {
                    idx = 0;
                }
                GC.Collect();
            };
            this.FormClosing += (a, b) =>
            {
                if (tim != null) { tim.Enabled = false; tim = null; GC.Collect(); }
            };
            tim.Interval = 50;
            tim.Enabled = true;

            GC.Collect();
        }
    }
}
