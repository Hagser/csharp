using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Drawing.Imaging;

namespace MyIPWebcamTimeLapse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<string> allServers = new List<string>();
        List<string> blockedServers = new List<string>();
        string strext = ".jpg";
        ImageFormat format = ImageFormat.Jpeg;
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (string str in allServers.Where(x=>!blockedServers.Contains(x)))
            {
                WebClient wc = new WebClient();

                try
                {
                    DateTime dtnow = DateTime.Now;
                    string strdirpath = Application.CommonAppDataPath + "\\" + str + "\\" + System.DateTime.Today.ToString("yyyyMMdd") + "\\";
                    if(!Directory.Exists(strdirpath))
                        Directory.CreateDirectory(strdirpath);
                    string strpath = strdirpath + DateTime.Now.ToString("HHmmss") + strext;
                    System.GC.Collect();
                    wc.DownloadFileCompleted += (x, y) =>
                    {
                        dynamic d = y.UserState as dynamic;
                        if (y.Error == null)
                        {
                            
                            try
                            {
                                if (fireTorchToolStripMenuItem.Checked)
                                    wc.DownloadString("http://" + d.Server + ":8080/disabletorch");
                            }
                            catch { }
                            
                            if (File.Exists(d.FilePath))
                            {
                                try
                                {
                                    Image img = Bitmap.FromFile(d.FilePath);
                                    float fontSize = getFontSize(img);
                                    Font font = new Font(DefaultFont.FontFamily,fontSize);
                                    MyPictureBox pb = getServerPictureBox(d.Server);
                                    bool bFound = (pb!=null);
                                    
                                    if (bFound)
                                    {
                                        pbInfo pbi = pb.pbInfo;
                                        pbi.FilePath = d.FilePath;
                                        pbi.Server = d.Server;
                                        pbi.Error = null;
                                        PointF pf = new PointF(2, img.Height - (fontSize));
                                        if (pbi.Rotate > 0)
                                        {
                                            img.RotateFlip(Stuff.getRotateFlipType(pbi.Rotate));
                                            if (pbi.Rotate == 90 || pbi.Rotate == 270)
                                            {
                                                pb.Size = new Size(270, 320);
                                                pf = new PointF(2, img.Height - fontSize);
                                            }
                                        }
                                        else
                                        {
                                            pb.Size = new Size(320, 270);
                                        }
                                        if (writeTimestampToolStripMenuItem.Checked)
                                        {
                                            Graphics g = Graphics.FromImage(img);

                                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                            g.DrawString(d.Now.ToString("yyyy-MM-dd HH:mm:ss"), font, new SolidBrush(Color.OrangeRed), pf);
                                            g.Save();
                                        }
                                        pb.Image = img;
                                        pbi.FilePath = d.FilePath;
                                        pb.Refresh();
                                        flowLayoutPanel1.Refresh();
                                    }
                                    else
                                    {
                                        pb = new MyPictureBox(img, new pbInfo() { Server = d.Server, FilePath = d.FilePath });
                                        pb.Size = new System.Drawing.Size(320, 270);
                                        pb.pbInfo.FilePath = d.FilePath;
                                        pb.pbInfo.Server = d.Server;
                                        pb.Cursor = Cursors.Hand;
                                        if (writeTimestampToolStripMenuItem.Checked)
                                        {
                                            Graphics g = Graphics.FromImage(img);
                                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                            g.DrawString(d.Now.ToString("yyyy-MM-dd HH:mm:ss"), font, new SolidBrush(Color.OrangeRed), new PointF(2, img.Height - fontSize));
                                            g.Save();
                                        }
                                        pb.Image = img;
                                        pb.PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                                        pb.PictureBox.ContextMenuStrip = createContextMenuStrip(pb);
                                        pb.Refresh();
                                        flowLayoutPanel1.Controls.Add(pb);
                                    }
                                    if (saveFilesToolStripMenuItem.Checked)
                                    {
                                        if(writeTimestampToolStripMenuItem.Checked)
                                            img.Save(d.FilePath.Replace(strext, "~" + strext), format);
                                    }
                                    else
                                    {
                                        string dirpath = (new FileInfo(d.FilePath).DirectoryName);

                                        foreach (string fi in Directory.EnumerateFiles(dirpath, "*" + strext,SearchOption.TopDirectoryOnly))
                                        {
                                            try
                                            {
                                                if (File.Exists(fi))
                                                {
                                                    File.Delete(fi);
                                                }
                                            }
                                            catch { }
                                        }
                                    }
                                }
                                catch(Exception ex) { 
                                
                                }
                            }
                        }
                        else
                        {
                            MyPictureBox pb = getServerPictureBox(d.Server);
                            if (pb != null)
                                pb.pbInfo.Error = y.Error;
                                //flowLayoutPanel1.Controls.Remove(pb);
                        }
                    };
                    try
                    {
                        if(fireTorchToolStripMenuItem.Checked)
                            wc.DownloadString("http://" + str + ":8080/enabletorch");
                    }
                    catch { }
                    wc.DownloadFileAsync(new Uri("http://" + str + ":8080/photo.jpg"), strpath,new {FilePath=strpath,Server=str,Now=dtnow});
                    
                }
                catch(Exception ex) {
                    string s = ex.Message;
                }
            }
        }

        private float getFontSize(Image img)
        {
            double dbl = img.Height * img.Width;
            string str = dbl.ToString();
            switch (str.Length)
            { 
                case 6:
                    return 12;
                case 7:
                    return 48;
            }
            return 12;

        }

        private MyPictureBox getServerPictureBox(string Server)
        {
            foreach (MyPictureBox pb in flowLayoutPanel1.Controls.Cast<MyPictureBox>())
            {
                if (pb.pbInfo != null && pb.pbInfo.Server.ToString().Equals(Server))
                {
                    return pb;
                }
            }
            return null;
        }



        private ContextMenuStrip createContextMenuStrip(MyPictureBox pb)
        {
            ContextMenuStrip contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            contextMenuStrip1.SuspendLayout();
            ToolStripMenuItem serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem torch = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem focus = new System.Windows.Forms.ToolStripMenuItem();

            ToolStripMenuItem rotateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem rotate0 = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem rotate90 = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem rotate180 = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripMenuItem rotate270 = new System.Windows.Forms.ToolStripMenuItem();
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            serverToolStripMenuItem,rotateToolStripMenuItem});
            contextMenuStrip1.Size = new System.Drawing.Size(153, 48);
            // 
            // rotateToolStripMenuItem
            // 
            rotateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            rotate0,
            rotate90,
            rotate180,
            rotate270});
            rotateToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            rotateToolStripMenuItem.Text = "Rotate";


            torch.Size = new System.Drawing.Size(152, 22);
            torch.Text = "LED";
            torch.CheckOnClick = true;
            torch.CheckedChanged += (s, d) =>
            {
                try
                {
                    WebClient wc = new WebClient();
                    wc.DownloadString("http://" + (s as ToolStripMenuItem).OwnerItem.Text + ":8080/" + (!torch.Checked ? "disable" : "enable") + "torch");
                }
                catch { }
            };
            focus.Size = new System.Drawing.Size(152, 22);
            focus.Text = "Focus";
            focus.CheckOnClick = true;
            focus.CheckedChanged += (s, d) =>
            {
                try
                {
                    WebClient wc = new WebClient();
                    wc.DownloadString("http://" + (s as ToolStripMenuItem).OwnerItem.Text + ":8080/" + (!focus.Checked ? "no" : "") + "focus");
                }
                catch { }
            };

            serverToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            serverToolStripMenuItem.Text = pb.pbInfo.Server;

            //http://192.168.1.8:8080/enabletorch
            //http://192.168.1.8:8080/focus
            //http://192.168.1.8:8080/disabletorch

            //http://192.168.1.8:8080/enabletorch


            serverToolStripMenuItem.Click += (s, d) => {
                Process.Start("http://" + (s as ToolStripMenuItem).Text + ":8080");
            };
            serverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            torch,
            focus});


            // 
            // rotate0
            // 
            rotate0.Size = new System.Drawing.Size(152, 22);
            rotate0.Text = "0";
            rotate0.Click += (a, b) =>
            {
                pbInfo pbi = pb.pbInfo;
                pb.Image.RotateFlip(Stuff.getRotateFlipType(360-pbi.Rotate));
                pbi.Rotate = 0;
                pb.Size = new Size(320, 240);
                pb.Refresh();
            };
            // 
            // rotate90
            // 
            rotate90.Size = new System.Drawing.Size(152, 22);
            rotate90.Text = "90";
            rotate90.Click += (a, b) => {
                pbInfo pbi = pb.pbInfo;
                pb.Image.RotateFlip(Stuff.getRotateFlipType(360 - pbi.Rotate));
                pbi.Rotate = 90;
                pb.Size = new Size(240, 320);
                pb.Refresh();            
            };
            // 
            // rotate180
            // 
            rotate180.Size = new System.Drawing.Size(152, 22);
            rotate180.Text = "180";
            rotate180.Click += (a, b) =>
            {
                pbInfo pbi = pb.pbInfo;
                pb.Image.RotateFlip(Stuff.getRotateFlipType(360 - pbi.Rotate));
                pbi.Rotate = 180;
                pb.Size = new Size(320, 240);
                pb.Refresh();
            };
            // 
            // rotate270
            // 
            rotate270.Size = new System.Drawing.Size(152, 22);
            rotate270.Text = "270";
            rotate270.Click += (a, b) =>
            {
                pbInfo pbi = pb.pbInfo;
                pb.Image.RotateFlip(Stuff.getRotateFlipType(360 - pbi.Rotate));
                pbi.Rotate = 270;
                pb.Size = new Size(240, 320);
                pb.Refresh();
            };
            contextMenuStrip1.ResumeLayout();
            return contextMenuStrip1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            findServers();
        }

        private void findServers()
        {
            for (int i = 2; i < 255; i++)
            {
                string strip = "192.168.1." + i;
                ThreadPool.QueueUserWorkItem((WaitCallback)TryToConnect, strip);
            }
        }
        private void TryToConnect(object state)
        {
            string strip = state.ToString();
            try
            {
                System.Net.Sockets.TcpClient tc = new System.Net.Sockets.TcpClient(strip, 8080);
                if (tc.Connected)
                {
                    tc.Close();
                    if (!allServers.Contains(strip))
                        allServers.Add(strip);
                }

            }
            catch
            {
                if (allServers.Contains(strip))
                    allServers.Remove(strip);
            }
            finally
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.Text = allServers.Count + " server(s) found.";
                });
            }
        }
        private int getInterval(string inum)
        {
            string[] s = inum.Split(' ');
            int i = int.Parse(s[0]);
            if (s.Length > 1)
            {
                switch (s[1])
                {
                    case "sek":
                        i = i * 1;
                        break;
                    case "min":
                        i = i * 60;
                        break;
                    case "tim":
                        i = i * 60 * 60;
                        break;
                }
            }
            return i*1000;
        }
        private void runningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = runningToolStripMenuItem.Checked;
            if (runningToolStripMenuItem.Checked)
                timer1_Tick(sender, EventArgs.Empty);
        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            timer1.Interval = getInterval(toolStripComboBox1.Text);
            flowLayoutPanel1.Focus();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            foreach (string str in allServers)
            {
                string strdirpath = Application.CommonAppDataPath + "\\" + str + "\\";
                foreach (string fi in Directory.EnumerateFiles(strdirpath,"*",SearchOption.AllDirectories))
                {
                    try
                    {
                        string np = fi.Replace(strext, "~" + strext);
                        if (File.Exists(np))
                        {
                            File.Delete(fi);
                            File.Move(np, fi);
                        }
                    }
                    catch { }
                }
            
            }
        }

        private void rotate_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            IContainerControl icc = tsmi.Owner.GetContainerControl();
            PictureBox pb = tsmi.Owner.GetContainerControl() as PictureBox;
            pbInfo pbi = (pb.Parent as MyPictureBox).pbInfo;
            pbi.Rotate = float.Parse(tsmi.Text);
            Graphics g = Graphics.FromImage(pb.Image);
            g.RotateTransform(pbi.Rotate);
            pb.Refresh();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            findServers();
            deleteEmptyFiles();
        }

        private void deleteEmptyFiles()
        {
            string dirpath = Application.CommonAppDataPath + "\\";
            foreach (string fi in Directory.EnumerateFiles(dirpath, "*" + strext, SearchOption.AllDirectories))
            {
                try
                {
                    if (File.Exists(fi) && (new FileInfo(fi)).Length == 0)
                    {
                        File.Delete(fi);
                    }
                }
                catch { }
            }
            foreach (string di in Directory.EnumerateDirectories(dirpath,"*", SearchOption.AllDirectories))
            {
                try
                {
                    if (Directory.Exists(di) && getTotalDirSize(new DirectoryInfo(di))==0)
                    {
                        Directory.Delete(di);
                    }
                }
                catch { }
            }
        }

        private long getTotalDirSize(DirectoryInfo di)
        {
            return di.EnumerateFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length);
        }

        private void flowLayoutPanel1_ControlRemoved(object sender, ControlEventArgs e)
        {
            MyPictureBox pb = e.Control as MyPictureBox;
            if (pb != null)
            {
                if (pb.pbInfo != null && !blockedServers.Contains(pb.pbInfo.Server))
                    blockedServers.Add(pb.pbInfo.Server);
            }
        }

        private void resetBlockedServersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            blockedServers.Clear();
        }

        private void flowLayoutPanel1_MouseEnter(object sender, EventArgs e)
        {
            menuStrip1.Visible = true;
        }

        private void flowLayoutPanel1_MouseLeave(object sender, EventArgs e)
        {
            if (menuStrip1.Visible)
            {
                System.Timers.Timer tim = new System.Timers.Timer(3000);
                tim.Elapsed += (a, b) =>
                {
                    tim.Enabled = false;
                    this.Invoke((MethodInvoker)delegate
                    {
                        menuStrip1.Visible = false;
                    });
                };
                tim.Start();
            }
        }

    }
}
