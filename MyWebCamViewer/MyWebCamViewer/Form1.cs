using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace MyWebCamViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<FileInfo> distlist = new List<FileInfo>();
        string strPath = @"C:\Users\jh\AppData\Roaming\MyWeatherCams\MyWeatherCams\1.0.0.0\webcams\";
        private void Form1_Load(object sender, EventArgs e)
        {
            this.fileSystemWatcher1.Path = strPath;

            ThreadPool.QueueUserWorkItem(getAllImages);

        }
        private void getAllImages(object sender)
        {
            getAllImages();
        }
        private void getAllImages()
        {
            List<FileInfo> list = new List<FileInfo>();
            distlist.Clear();
            System.GC.Collect();
            foreach (string strDir in Directory.GetDirectories(strPath))
            {
                list.Clear();
                foreach (string strFile in Directory.GetFiles(strDir+"\\"+DateTime.Today.ToString("yyyyMMdd"), "*.jpg"))
                {
                    list.Add(new FileInfo(strFile));
                }
                distlist.Add(list.OrderByDescending(x => x.LastWriteTime).FirstOrDefault());
            }
            FixPictures();
        }

        private void flowLayoutPanel1_Click(object sender, EventArgs e)
        {
            FixPictures();
        }

        private void FixPictures()
        {
            foreach (FileInfo fi in distlist.Where(x=>x!=null).OrderBy(x=>x.Directory.Parent.Name))
            {
                if (fi != null)
                {
                    try
                    {
                        UpdateOrCreate(fi);
                    }
                    catch { }
                }
            }
        }
        
        private void UpdateOrCreate(object p)
        {
            FileInfo fi = new FileInfo(p.ToString());
            UpdateOrCreate(fi);
        }
        private void UpdateOrCreate(FileInfo p)
        {
            var list = flowLayoutPanel1.Controls.Where(x => x.GetType() == typeof(PictureBox) && (x as PictureBox).Tag != null && ((x as PictureBox).Tag as FileInfo).Directory.Parent.Name.Equals(p.Directory.Parent.Name));
            if (list.Count > 0)
            {
                PictureBox pb = list.FirstOrDefault() as PictureBox;
                FileInfo fi = pb.Tag as FileInfo;

                Application.DoEvents();
                if (p.FullName.Equals(fi.FullName))
                    return;
                Image img1 = FromFile(p.FullName);
                if (img1 != null)
                    pb.Image = img1;
                pb.Tag = p;
                this.Invoke((MethodInvoker)delegate
                {
                    pb.Refresh();
                    //flowLayoutPanel1.Refresh();
                });
                Application.DoEvents();
                return;
            }
            if (flowLayoutPanel1.Controls.Count > 190)
                return;

            PictureBox pbr = new PictureBox();
            pbr.Margin = new Padding(0);
            pbr.Cursor = Cursors.Hand;
            pbr.Width = 100;
            pbr.Height = 100;
            Image imgr = FromFile(p.FullName);
            if (imgr != null)
                pbr.Image = imgr;
            pbr.Tag = p;
            pbr.SizeMode = PictureBoxSizeMode.StretchImage;
            pbr.Click += (a, b) => {
                ProcessStartInfo psi = new ProcessStartInfo(@"C:\Users\jh\Documents\hcdev\trunk\MyAnimator\MyAniApp\bin\Release\MyAniApp.exe");
                psi.Arguments=(((PictureBox)a).Tag as FileInfo).DirectoryName;
                Process.Start(psi);
            };
            pbr.MouseMove += (a, b) => { FileInfo fi = (((PictureBox)a).Tag as FileInfo); this.Text = fi.Directory.Name + ":" + fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm"); };
            this.Invoke((MethodInvoker)delegate
            {
                flowLayoutPanel1.Controls.Add(pbr);
            });

        }
        Dictionary<string, List<Exception>> errors = new Dictionary<string, List<Exception>>();
        private Image FromFile(string p)
        {
            try
            {
                using (FileStream fs = new FileStream(p, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    return Bitmap.FromStream(fs);
                }
            }
            catch (IOException ioex)
            {
                if (errors.ContainsKey(p))
                    errors[p].Add(ioex);
                else
                {
                    errors.Add(p, new List<Exception>());
                    errors[p].Add(ioex);
                }
                ThreadPool.QueueUserWorkItem(UpdateOrCreate, p);
            }
            catch (ExternalException eex)
            {
                if (errors.ContainsKey(p))
                    errors[p].Add(eex);
                else
                {
                    errors.Add(p, new List<Exception>());
                    errors[p].Add(eex);
                }

                ThreadPool.QueueUserWorkItem(UpdateOrCreate, p);
            }
            catch (Exception ex)
            {
                if (errors.ContainsKey(p))
                    errors[p].Add(ex);
                else
                {
                    errors.Add(p, new List<Exception>());
                    errors[p].Add(ex);
                }

            }
            return null;

            Bitmap bmp = new Bitmap(150, 150);
            Graphics g = Graphics.FromImage(bmp);
            float fst = 0;
            float fen= 360;
            g.DrawArc(new Pen(Color.Blue), new Rectangle(75, 75, 50, 50), fst, fen);
            g.Save();

            return bmp;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseReason s = e.CloseReason;
            //e.Cancel = true;
        }

        private void fileSystemWatcher1_Renamed(object sender, RenamedEventArgs e)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(UpdateOrCreate,e.FullPath);
            }
            catch(Exception ex) {
                string s = ex.Message;
            }
        }

    }
    public static class ControlExt
    { 
        public static List<Control> Where(this Control.ControlCollection cc,Func<Control,bool> func)
        {
            List<Control> list = new List<Control>();
            foreach (Control cic in cc)
            {
                list.Add(cic);
            }
            return list.Where(func).ToList();
        }
    }
}
