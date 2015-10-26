using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.IO;

namespace MyAniApp
{
    public partial class Form1 : Form
    {
        public Form1(string[] args)
        {
            InitializeComponent();
            if (args.Length > 0)
            {
                strDir = args[0];
            }
        }
        Dictionary<string, Image> liss = new Dictionary<string, Image>();
        string strDir {get{return textBox1.Text;}set{
            if (textBox1.Text != value && Directory.Exists(value) && !bDirPriv)
            {
                timer1.Enabled = false; liss.Clear(); i = 0; bForward = true;
                bAbort = true;
                Thread.Sleep(500);
                textBox1.Text = value;
                strDir = value;
                liss.Clear();
                bAbort = false;
                LoadFiles();
            }
            else if(textBox1.Text != value && Directory.Exists(value))
            {
                textBox1.Text = value;
                strDir = value;
                liss.Clear();
            }
            bDirPriv = false;

        }}
        bool bDirPriv = false;
        bool bAbort = false;

        private void LoadHourFiles()
        {
            ThreadPool.QueueUserWorkItem(LoadHourFiles, toolStripHourText.Text);
        }
        private void LoadFiles()
        {
            ThreadPool.QueueUserWorkItem(LoadFiles, null);
        }
        private void LoadFiles(object state)
        {
            try
            {
                if (!System.IO.Directory.Exists(strDir))
                    return;
                fileSystemWatcher1.Path = strDir;
                float fontSize = 48;
                Font font = new Font(DefaultFont.FontFamily,fontSize);
                int icnt = 0;
                int ir = 1;
                int.TryParse(everyNPicture.Text+"",out ir);

                int imax = 200000;
                this.Invoke((MethodInvoker)delegate
                {
                    this.UseWaitCursor = true;
                });

                SettingsPopup sp = new SettingsPopup(strDir,imax);
                DialogResult dr = sp.ShowDialog();
                if(dr==DialogResult.Cancel)
                    throw new Exception("Cancel");
                List<string> files = sp.Files;
                double dir = double.Parse(files.Count.ToString());
                ir = sp.EveryN;
                everyNPicture.Text = ir+"";
                subfoldersToolStripMenuItem.Checked = sp.SubFolders;

                //Aint gonna happen!
                if (false && dir > 300)
                {
                    dir = dir / 300;
                    dir = Math.Floor(dir);
                    if (dir > 0)
                        ir = int.Parse(dir.ToString());
                }
                
                foreach (string fil in files)
                {
                    if(bAbort)
                        break;
                    icnt++;
                    if (ir>1 && icnt % ir != 0)
                        continue;

                    TaskbarManager.Instance.SetProgressValue(icnt, files.Count);
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);


                    if (!liss.ContainsKey(fil))
                    {
                        if((new FileInfo(fil)).Length>10000000)
                            continue;
                        try
                        {
                            using (Image iss = Bitmap.FromFile(fil))
                            {
                                double dw = iss.Width;
                                double dh = iss.Height;

                                double dratio = (dh / dw);
                                dw = Math.Min(640, iss.Width);
                                dh = dw * dratio;

                                int width = int.Parse(Math.Round(dw, 0).ToString());
                                int height = int.Parse(Math.Round(dh, 0).ToString());

                                Size newSize = new Size(width, height);
                                Bitmap b = new Bitmap(iss, newSize);
                                System.GC.Collect();
                                liss.Add(fil, b.GetThumbnailImage(b.Width, b.Height, (Image.GetThumbnailImageAbort)delegate() { return false; }, IntPtr.Zero));
                            }
                        }
                        catch { }
                    }
                    if(bAbort)
                        break;
                }
            }
            catch (Exception ec)
            {
                this.Invoke((MethodInvoker)delegate
                { this.Text = ec.Message; });

                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
            }

            this.Invoke((MethodInvoker)delegate
            {
                this.UseWaitCursor = false;
                timer1.Enabled = liss.Count > 0;
                trackBar1.Maximum = liss.Count;
            });
        }
        int i = 0;
        bool bForward = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!timer1.Enabled ||liss.Count==0)
                return;
            trackBar1.Maximum = liss.Count;
            trackBar1.Value = i>=trackBar1.Minimum&&i<=trackBar1.Maximum?i:0;
            if (i>=0 && i < liss.Count)
            {
                Image img = liss.Select(x => x.Value).ToArray()[i];
                pictureBox1.Image = img;

                this.Text = "MyAniApp - " + i + "/" + liss.Count;
                System.GC.Collect();
            }
            if (bForward)
                i++;
            else
                i--;

            if (i >= liss.Count-1)
                bForward = false;
            else if (i <= 0)
                bForward = true;
                
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;liss.Clear();i = 0;bForward = true;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = strDir;
            bAbort = true;
            if (DialogResult.OK == fbd.ShowDialog())
            {
                strDir = fbd.SelectedPath;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (liss.Count > trackBar1.Value)
            {
                Image img = liss.Select(x => x.Value).ToArray()[trackBar1.Value];
                pictureBox1.Image = img;
                this.Text = "MyAniApp - " + trackBar1.Value + "/" + liss.Count;
            }
            System.GC.Collect();
        }

        private void trackBar1_Enter(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void trackBar1_Leave(object sender, EventArgs e)
        {
            timer1.Enabled = liss.Count > 0;
        }

        private void watchForChangesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            fileSystemWatcher1.Path = strDir;
            fileSystemWatcher1.IncludeSubdirectories = subfoldersToolStripMenuItem.Checked;
            fileSystemWatcher1.EnableRaisingEvents = watchForChangesToolStripMenuItem.Checked;
        }

        private void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
        {
            string fil = e.FullPath;
            try
            {
                using (Image iss = Bitmap.FromFile(fil))
                {
                    double dw = iss.Width;
                    double dh = iss.Height;

                    double dratio = (dh / dw);
                    dw = Math.Min(640, iss.Width);
                    dh = dw * dratio;

                    int width = int.Parse(Math.Round(dw, 0).ToString());
                    int height = int.Parse(Math.Round(dh, 0).ToString());

                    Size newSize = new Size(width, height);
                    Bitmap b = new Bitmap(iss, newSize);
                    System.GC.Collect();
                    liss.Add(fil, b.GetThumbnailImage(b.Width, b.Height, (Image.GetThumbnailImageAbort)delegate() { return false; }, IntPtr.Zero));
                }
            }
            catch { }
        }

        private void subfoldersToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            fileSystemWatcher1.IncludeSubdirectories = subfoldersToolStripMenuItem.Checked;
        }

        private void fileSystemWatcher1_Deleted(object sender, FileSystemEventArgs e)
        {
            string fil = e.FullPath;
            if (liss.ContainsKey(fil))
                liss.Remove(fil);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false; liss.Clear(); i = 0; bForward = true;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = strDir;
            bAbort = true;
            bDirPriv = true;
            if (DialogResult.OK == fbd.ShowDialog())
            {
                strDir = fbd.SelectedPath;
            }
            bAbort = false;
            LoadHourFiles();
        }

        private void LoadHourFiles(object state)
        {
            string hour = state + "";
            int h = int.Parse(hour.Split(':')[0]);
            int m = int.Parse(hour.Split(':')[1]);
            try
            {
                if (!System.IO.Directory.Exists(strDir))
                    return;
                fileSystemWatcher1.Path = strDir;
                float fontSize = 48;
                Font font = new Font(DefaultFont.FontFamily, fontSize);
                int icnt = 0;

                this.Invoke((MethodInvoker)delegate
                {
                    this.UseWaitCursor = true;
                });

                List<FileInfo> files = System.IO.Directory.EnumerateFiles(strDir, "*", SearchOption.AllDirectories).Select(x => new FileInfo(x)).OrderBy(x => x.LastWriteTime).ToList();
                files = files.Where(x => x.LastWriteTime.Hour == h).ToList();
                files = files.Where(x => ((x.LastWriteTime.Minute - m) < 5 || (x.LastWriteTime.Minute - m) > 5)).ToList();
                double dir = double.Parse(files.Count.ToString());
                List<DateTime> addeddates = new List<DateTime>();
                foreach (FileInfo fil in files)
                {

                    if (bAbort)
                        break;
                    if (!addeddates.Contains(fil.LastWriteTime.Date))
                        addeddates.Add(fil.LastWriteTime.Date);
                    else
                        continue;
                    icnt++;

                    TaskbarManager.Instance.SetProgressValue(icnt, files.Count);
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);

                    if (!liss.ContainsKey(fil.FullName))
                    {
                        if (fil.Length > 10000000)
                            continue;
                        try
                        {
                            using (Image iss = Bitmap.FromFile(fil.FullName))
                            {
                                double dw = iss.Width;
                                double dh = iss.Height;

                                double dratio = (dh / dw);
                                dw = Math.Min(640, iss.Width);
                                dh = dw * dratio;

                                int width = int.Parse(Math.Round(dw, 0).ToString());
                                int height = int.Parse(Math.Round(dh, 0).ToString());

                                Size newSize = new Size(width, height);
                                Bitmap b = new Bitmap(iss, newSize);
                                System.GC.Collect();
                                liss.Add(fil.FullName, b.GetThumbnailImage(b.Width, b.Height, (Image.GetThumbnailImageAbort)delegate() { return false; }, IntPtr.Zero));
                            }
                        }
                        catch(Exception ex) {
                            string apa = ex.Message;
                            if (apa.Length > 1)
                            {
                                string sisiiskk = "";
                            }
                        }
                    }
                    if (bAbort)
                        break;
                }
            }
            catch (Exception ec)
            {
                this.Invoke((MethodInvoker)delegate
                { this.Text = ec.Message; });

                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
            }

            this.Invoke((MethodInvoker)delegate
            {
                this.UseWaitCursor = false;
                timer1.Enabled = liss.Count > 0;
                trackBar1.Maximum = liss.Count;
            });
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

    }
}
