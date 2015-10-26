using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace MyIPWebcamTimeLapse
{
    public partial class MyPictureBox : UserControl
    {

        public MyPictureBox(Image img,pbInfo _pbInfo)
        {
            InitializeComponent();
            pictureBox.Image = img;
            pbInfo = _pbInfo;
            label.Text = pbInfo.Server;
            pbInfo.pbInfoChanged += (a, b) =>
            { 
                switch(b.PropertyName)
                {
                    case "FilePath":
                        RefreshImage(pbInfo.FilePath);
                        break;
                    case "Rotation":
                        comboBoxRotate.Text = pbInfo.Rotate.ToString();
                        break;
                    case "Server":
                        label.Text = pbInfo.Server;
                        label.BackColor = System.Drawing.Color.Transparent;
                        label.ForeColor = System.Drawing.Color.Black;
                        break;
                    case "Error":
                        if (pbInfo.Error != null)
                        {
                            label.Text = pbInfo.Error.Message;
                            label.BackColor = System.Drawing.Color.Red;
                            label.ForeColor = System.Drawing.Color.White;
                        }
                        break;
                }
            };
            comboBoxRotate.TextChanged += (a, b) => {
                PointF pf = new PointF(2, img.Height - 20);
                if (pbInfo.Rotate > 0)
                {
                    img.RotateFlip(Stuff.getRotateFlipType(pbInfo.Rotate));
                    if (pbInfo.Rotate == 90 || pbInfo.Rotate == 270)
                    {
                        pictureBox.Size = new Size(240, 320);
                        pf = new PointF(2, img.Height - 20);
                    }
                }
                else
                {
                    pictureBox.Size = new Size(320, 240);
                }
            };
        }
        public void RefreshImage(string FilePath)
        {
            if(System.IO.File.Exists(FilePath))
                RefreshImage(Bitmap.FromFile(FilePath));
        }
        public void RefreshImage(Image image)
        {
            pictureBox.Image = image;
            pictureBox.Refresh();
            Refresh();
        }
        public PictureBox PictureBox
        {
            get { return pictureBox; }
        }
        public Image Image {
            get { return pictureBox.Image; }
            set { pictureBox.Image = value; }
        }
        private pbInfo _Tag;
        public pbInfo pbInfo
        {
            get { return _Tag; }
            set { _Tag = value; }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            FileInfo fi = new FileInfo(pbInfo.FilePath);
            Process.Start(fi.DirectoryName);
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            statusStrip1.Visible = true;
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            if (statusStrip1.Visible)
            {
                System.Timers.Timer tim = new System.Timers.Timer(3000);
                tim.Elapsed += (a, b) =>
                {
                    tim.Enabled = false;
                    this.Invoke((MethodInvoker)delegate
                    {
                        statusStrip1.Visible = false;
                    });
                };
                tim.Start();
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

    }
}
