using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace SS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int xstart = 0;
        int ystart = 0;
        int width = 0;
        int height = 0;
        ArrayList alImages = new ArrayList();
        AviWriter aw = new AviWriter();
        ScreenCapture sc = new ScreenCapture();

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!trackLocationToolStripMenuItem.Checked)
            {
                try
                {
                    Image img1 = sc.CaptureScreen(xstart, ystart, width, height);

                    if (showToolStripMenuItem.Checked)
                    {
                        pictureBox1.Image = img1;
                    }
                    if (fileToolStripMenuItem.Checked)
                    {
                        string strPath = Application.CommonAppDataPath + "\\" + System.DateTime.Now.Ticks.ToString() + ".png";
                        sc.CaptureScreenToFile(strPath, System.Drawing.Imaging.ImageFormat.Png, xstart, ystart, width, pictureBox1.Height);
                        alImages.Add(strPath);
                        toolStripStatusLabel2.Text = "Files:";
                    }
                    else if (memoryToolStripMenuItem.Checked)
                    {
                        alImages.Add(img1);
                        toolStripStatusLabel2.Text = "Memory:";
                    }
                    toolStripStatusLabel2.Text += (alImages.Count).ToString();
                }
                catch(Exception ex) {
                    toolStripStatusLabel1.Text = "Error:" + ex.Message;
                }

            }
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            if (trackLocationToolStripMenuItem.Checked)
            {
                xstart = this.Left;
                ystart = this.Top+30;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.Down)
                {
                    this.Opacity -= .1;
                }
                else if (e.KeyCode == Keys.PageDown)
                {
                    this.Opacity -= .3;
                }
                else if (e.KeyCode == Keys.Up)
                {
                    this.Opacity += .1;
                }
                else if (e.KeyCode == Keys.PageUp)
                {
                    this.Opacity += .3;                
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    this.Opacity = 1;
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (trackLocationToolStripMenuItem.Checked)
            {
                width = this.Width;
                height = this.Height-30;
            }
        }

        private void topmostToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            this.TopMost = ((ToolStripMenuItem)sender).Checked;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            width = this.Width;
            height = this.Height;
            xstart = this.Left;
            ystart = this.Top;

            for (int i = 1; i <= 4; i++)
            {
                string strText = "1".PadRight(i, "0".ToCharArray()[0]);
                ToolStripItem tsi = toolStripDropDownInterval.DropDownItems.Add(strText);
                ToolStripItem tsi2 = intervalToolStripMenuItem.DropDownItems.Add(strText);
                tsi.Tag = int.Parse(strText);
                tsi.Click += new EventHandler(tsi_Click);
                tsi2.Tag = int.Parse(strText);
                tsi2.Click += new EventHandler(tsi_Click);
            }
        }

        void tsi_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                ToolStripItem tsi = (ToolStripItem)sender;
                if (tsi.Tag != null)
                {
                    timer1.Interval = int.Parse(tsi.Tag.ToString());
                    toolStripStatusInterval.Text = timer1.Interval.ToString();
                }
            }
        }

        private void trackLocationToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            captureToolStripMenuItem.Enabled = !trackLocationToolStripMenuItem.Checked;
        }

        private void saveAVIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strAviFile = "";
            saveFileDialog1.FileName = strAviFile;
            saveFileDialog1.Filter = "AVI|*.avi";
            DialogResult dr = saveFileDialog1.ShowDialog();
            strAviFile = saveFileDialog1.FileName;

            if (dr == DialogResult.OK && strAviFile != "")
            {
                if (File.Exists(strAviFile))
                {
                    File.Delete(strAviFile);
                }
                uint uifr = uint.Parse("20");
                aw.Open(strAviFile, uifr);
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Value = 0;
                toolStripProgressBar1.Maximum = alImages.Count;

                foreach (object img in alImages)
                {
                    toolStripProgressBar1.Value++;
                    try
                    {
                        if (fileToolStripMenuItem.Checked)
                        {
                            string strFile = img.ToString();
                            Bitmap bmp = new Bitmap(strFile);
                            aw.AddFrame(bmp);
                            if (File.Exists(strFile))
                            {
                                File.Delete(strFile);
                            }
                        }
                        else if (memoryToolStripMenuItem.Checked)
                        {
                            Bitmap bmp = new Bitmap((Image)img);
                            aw.AddFrame(bmp);
                        }
                        GC.Collect();
                        Application.DoEvents();

                    }
                    catch (Exception ex) {toolStripStatusLabel1.Text = ex.Message; }
                }
                if (fileToolStripMenuItem.Checked)
                {
                    foreach (string strFile in alImages)
                    {
                        if (File.Exists(strFile))
                        {
                            File.Delete(strFile);
                        }
                    }
                }
                alImages.Clear();
                aw.Close();
                GC.Collect();

            }
        }

        private void captureToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            timer1.Enabled = captureToolStripMenuItem.Checked;
            fileToolStripMenuItem.Enabled = !captureToolStripMenuItem.Checked;
            memoryToolStripMenuItem.Enabled = !captureToolStripMenuItem.Checked;
            saveAVIToolStripMenuItem.Enabled = !captureToolStripMenuItem.Checked && alImages.Count>0;
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileToolStripMenuItem.Checked = !fileToolStripMenuItem.Checked;
            memoryToolStripMenuItem.Checked = !fileToolStripMenuItem.Checked;
            alImages.Clear();
        }

        private void memoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            memoryToolStripMenuItem.Checked = !memoryToolStripMenuItem.Checked;
            fileToolStripMenuItem.Checked = !memoryToolStripMenuItem.Checked;
            alImages.Clear();
        }

    }
}
