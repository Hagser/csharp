using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace AVIModifier
{
    public partial class Form1 : Form
    {
        public Form1(string[] args)
        {
            InitializeComponent();
        }
        string strAviFile = "";
        AviReader ar = new AviReader();
        AviWriter aw = new AviWriter();
        DateTime dt = DateTime.Now;
        Bitmap[] bmps_avi = new Bitmap[0];


        private void btnCreate_Click(object sender, EventArgs e)
        {
            closeVideo();
            lblError.Text = "";
            saveFileDialog1.FileName = strAviFile;
            saveFileDialog1.Filter = "AVI|*.avi";
            DialogResult dr = saveFileDialog1.ShowDialog();
            strAviFile = saveFileDialog1.FileName;
            
            if (dr==DialogResult.OK && strAviFile != "")
            {
                if (File.Exists(strAviFile))
                {
                    File.Delete(strAviFile);
                }
                openFileDialog1.FileName = "";
                openFileDialog1.Filter = "Images|*.jpg;*.bmp;*.gif";
                openFileDialog1.Multiselect = true;
                dr = openFileDialog1.ShowDialog();

                if (dr == DialogResult.OK && openFileDialog1.FileNames.Length > 0)
                {
                    
                    try
                    {
                        progressBar1.Value = 0;
                        progressBar1.Maximum = openFileDialog1.FileNames.Length;
                        uint uifr = uint.Parse(txtFrameRate.Text.Length > 0 ? txtFrameRate.Text : "20");
                        double rsize = double.Parse(txtWidth.Text.Length > 0 ? txtWidth.Text : "0");
                        aw.Open(strAviFile, uifr);
                        foreach (string strFile in openFileDialog1.FileNames)
                        {
                            try
                            {
                                Bitmap bmp = new Bitmap(strFile);
                                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                                double fsize = double.Parse(txtSize.Text.Length > 0 ? txtSize.Text : "0");
                                double dblW = double.Parse(bmp.Width.ToString());
                                double dblH = double.Parse(bmp.Height.ToString());
                                if (fsize > 0)
                                {
                                    fsize = fsize / 100;
                                    dblH = dblH * fsize;
                                    dblW = dblW * fsize;
                                }
                                else if(rsize>0)
                                {
                                    double dratio = (dblH/dblW);
                                    dblW = rsize;
                                    dblH = dblW * dratio;
                                }
                                bmp = new Bitmap(bmp, int.Parse(Math.Round(dblW, 0).ToString()), int.Parse(Math.Round(dblH, 0).ToString()));
                                //bmp = new Bitmap(bmp.GetThumbnailImage(int.Parse(Math.Round(dblW, 0).ToString()), int.Parse(Math.Round(dblH, 0).ToString()), myCallback, IntPtr.Zero));
                                if (!chkCompress.Checked)
                                {
                                    aw.AddFrame(bmp);
                                }
                                else
                                {
                                    aw.AddFrame(bmp,500,16);
                                }
                                progressBar1.Value++;
                                TaskbarManager.Instance.SetProgressValue(progressBar1.Value, progressBar1.Maximum);
                                bmp.Dispose();
                                GC.Collect();
                                Application.DoEvents();
                            }
                            catch (Exception ex) {lblError.Text=ex.Message;}
                        }

                        
                    }
                    catch (Exception ex) {lblError.Text=ex.Message;}
                    finally { aw.Close(); }
                }
                if (File.Exists(strAviFile))
                {
                    //mpVideo.currentMedia = mpVideo.newMedia(strAviFile);
                    pBox.Visible = false;
                    //mpVideo.Visible = true;
                }
            }

        }
        private bool ThumbnailCallback()
        {
            return true;
        }

        private void btnExtract_Click(object sender, EventArgs e)
        {
            closeVideo();
            lblError.Text = "";
            openFileDialog1.FileName = strAviFile;
            openFileDialog1.Filter = "AVI|*.avi";
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileNames.Length > 0)
            {
                strAviFile = openFileDialog1.FileName;
                saveFileDialog1.Filter = "Image|*.bmp";
                saveFileDialog1.ShowDialog();
                string strNewFile = saveFileDialog1.FileName;
                try
                {
                    ar.Open(strAviFile);

                    progressBar1.Value = 0;
                    progressBar1.Maximum = ar.CountFrames;

                    for (int i = 0; i < ar.CountFrames; i++)
                    {
                        try
                        {
                            ar.ExportBitmap(i, Path.GetFileNameWithoutExtension(strNewFile) + i.ToString().PadLeft(4, "0".ToCharArray()[0]) + ".bmp");
                            progressBar1.Value++;
                            TaskbarManager.Instance.SetProgressValue(progressBar1.Value, progressBar1.Maximum);
                        }
                        catch (Exception ex) {lblError.Text=ex.Message;}
                    }
                    ar.Close();
                }
                catch (Exception ex) {lblError.Text=ex.Message;}
            }
        }

        private void btnOpenAvi_Click(object sender, EventArgs e)
        {
            closeVideo();
            lblError.Text = "";
            openFileDialog1.FileName = strAviFile;
            openFileDialog1.Filter = "AVI|*.avi";
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileNames.Length > 0)
            {
                try
                {
                    strAviFile=openFileDialog1.FileName;
                    ar.Open(strAviFile);
                    hSB.Value = 0;
                    hSB.Maximum = ar.CountFrames;

                    progressBar1.Value = 0;
                    progressBar1.Maximum = ar.CountFrames;
                    bmps_avi = new Bitmap[ar.CountFrames];
                    for (int i = 0; i < ar.CountFrames; i++)
                    {
                        try
                        {
                            string strBmpFile = Application.UserAppDataPath + "\\" + Path.GetFileNameWithoutExtension(strAviFile) + "_" + dt.Ticks.ToString() + "_" + i.ToString().PadLeft(4, "0".ToCharArray()[0]) + ".bmp";
                            if (File.Exists(strBmpFile))
                            {
                                File.Delete(strBmpFile);
                            }
                            if (!File.Exists(strBmpFile))
                            {
                                ar.ExportBitmap(i, strBmpFile);
                                bmps_avi.SetValue(new Bitmap(strBmpFile), i);
                                //File.Delete(strBmpFile);
                            } 
                            progressBar1.Value++;
                            TaskbarManager.Instance.SetProgressValue(progressBar1.Value, progressBar1.Maximum);
                        }
                        catch (Exception ex) { lblError.Text = ex.Message; }
                        GC.Collect();
                    }
                    loadFrame(0);
                    pBox.Visible = true;
                    //mpVideo.Visible = false;
                }
                catch (Exception ex) {lblError.Text=ex.Message;}
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            closeVideo();
            lblError.Text = "";
            openFileDialog1.FileName = strAviFile;
            openFileDialog1.Filter = "AVI|*.avi";
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileNames.Length > 0)
            {
                strAviFile = openFileDialog1.FileName;
                //mpVideo.currentMedia = mpVideo.newMedia(strAviFile);
                pBox.Visible = false;
                //mpVideo.Visible = true;
            }
        }
        void closeVideo()
        {
            aw.Close();
            ar.Close();
            //mpVideo.close();
            bmps_avi = new Bitmap[0];
            GC.Collect();
            pBox.Image = null;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeVideo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            closeVideo();
        }
        void loadFrame(int ifr)
        {
            if (bmps_avi.Length >= ifr)
            {
                pBox.Image = bmps_avi[ifr];
            }
        }
        private void hSB_ValueChanged(object sender, EventArgs e)
        {
            lblError.Text = "";
            try
            {
                loadFrame(hSB.Value);
            }
            catch (Exception ex) {lblError.Text=ex.Message;}
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            closeVideo();
            lblError.Text = "";
            saveFileDialog1.FileName = strAviFile;
            saveFileDialog1.Filter = "AVI|*.avi";
            DialogResult dr = saveFileDialog1.ShowDialog();
            strAviFile = saveFileDialog1.FileName;
            FileInfo fiavi = new FileInfo(strAviFile);
            if (dr == DialogResult.OK && strAviFile != "")
            {
                if (File.Exists(strAviFile))
                {
                    File.Delete(strAviFile);
                }
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.SelectedPath = fiavi.DirectoryName;
                dr = fbd.ShowDialog();

                if (dr == DialogResult.OK && Directory.Exists(fbd.SelectedPath))
                {

                    try
                    {
                        progressBar1.Value = 0;
                        DirectoryInfo di = new DirectoryInfo(fbd.SelectedPath);
                        FileInfo[] filelist = di.GetFiles("*.JPG", chkSF.Checked?SearchOption.AllDirectories:SearchOption.TopDirectoryOnly).OrderBy(x=>x.CreationTime).ToArray();
                        progressBar1.Maximum = filelist.Length;

                        uint uifr = uint.Parse(txtFrameRate.Text.Length > 0 ? txtFrameRate.Text : "20");
                        double rsize = double.Parse(txtWidth.Text.Length > 0 ? txtWidth.Text : "0");
                        aw.Open(strAviFile, uifr);
                        int iskip = int.Parse(txtSkip.Text);
                        int icnt = 0;
                        foreach (FileInfo fi in filelist)
                        {
                            if (icnt % iskip != 0)
                            {
                                icnt++;
                                continue;
                            }
                            icnt++;
                            try
                            {
                                string strFile = fi.FullName;
                                Bitmap bmp = new Bitmap(strFile);
                                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                                double fsize = double.Parse(txtSize.Text.Length > 0 ? txtSize.Text : "0");
                                double dblW = double.Parse(bmp.Width.ToString());
                                double dblH = double.Parse(bmp.Height.ToString());
                                if (fsize > 0)
                                {
                                    fsize = fsize / 100;
                                    dblH = dblH * fsize;
                                    dblW = dblW * fsize;
                                }
                                else if (rsize > 0)
                                {
                                    double dratio = (dblH / dblW);
                                    dblW = rsize;
                                    dblH = dblW * dratio;
                                }
                                bmp = new Bitmap(bmp, int.Parse(Math.Round(dblW, 0).ToString()), int.Parse(Math.Round(dblH, 0).ToString()));
                                //bmp = new Bitmap(bmp.GetThumbnailImage(int.Parse(Math.Round(dblW, 0).ToString()), int.Parse(Math.Round(dblH, 0).ToString()), myCallback, IntPtr.Zero));
                                if (!chkCompress.Checked)
                                {
                                    aw.AddFrame(bmp);
                                }
                                else
                                {
                                    aw.AddFrame(bmp, 500, 16);
                                }
                                progressBar1.Value = icnt;
                                TaskbarManager.Instance.SetProgressValue(progressBar1.Value, progressBar1.Maximum);
                                bmp.Dispose();
                                GC.Collect();
                                Application.DoEvents();
                            }
                            catch (Exception ex) { lblError.Text = ex.Message; }
                        }


                    }
                    catch (Exception ex) { lblError.Text = ex.Message; }
                    finally { aw.Close(); }
                }
                if (File.Exists(strAviFile))
                {
                    //mpVideo.currentMedia = mpVideo.newMedia(strAviFile);
                    pBox.Visible = false;
                    //mpVideo.Visible = true;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(txtWidth.Text!="")
                txtSize.Text = "";
        }

        private void txtSize_TextChanged(object sender, EventArgs e)
        {
            if (txtSize.Text != "")
                txtWidth.Text = "";
        }


    }
}
