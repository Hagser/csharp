using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;

using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Net.Sockets;
using System.IO;
using System.Net;
using Emgu.CV.CvEnum;
using System.Diagnostics;
using System.Threading;

namespace MyIPWebCam
{
    public partial class Form1 : Form
    {
        Capture _capture;
        TcpListener _server;
        string strFormText;
        public Form1()
        {
            InitializeComponent();
            System.Net.NetworkInformation.IPGlobalProperties ipgp = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties();
            System.Net.IPEndPoint[] tcplist = ipgp.GetActiveTcpListeners();
            IPEndPoint iep = tcplist.FirstOrDefault(o => o.Port == 139);
            if (iep != null)
            {
                IPAddress ipa = IPAddress.Parse(iep.Address.ToString());

                strFormText = this.Text + " " + ipa.ToString();
                _server = new TcpListener(ipa, 8080);
            }
            else
            {
                _server = new TcpListener(8080);
                strFormText = this.Text;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var list = DirectShowLib.DsDevice.GetDevicesOfCat(DirectShowLib.FilterCategory.VideoInputDevice);
                int idev = 0;
                foreach (var dev in list)
                {
                    var tsi = new ToolStripMenuItem(dev.Name);
                    tsi.Tag = idev;
                    tsi.CheckOnClick = true;
                    tsi.Click += (a, b) => {
                        var ttt = (ToolStripMenuItem)a;
                        if (ttt != null && ttt.Checked)
                        {
                            foreach (ToolStripMenuItem tm in deviceToolStripMenuItem.DropDownItems)
                                tm.Checked = false;
                            int nTries = 0;
                            timer1.Enabled = false;
                            while (nTries < 5)
                            {
                                try
                                {
                                    _capture = new Capture(Convert.ToInt32(ttt.Tag));
                                    _capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 1280);
                                    _capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 720);
                                    nTries = 20;
                                    ttt.Checked = true;
                                }
                                catch {
                                    Thread.Sleep(100);
                                    nTries++;
                                }

                            }

                            timer1.Enabled = true;
                        }                        
                    };
                    idev++;
                    deviceToolStripMenuItem.DropDownItems.Add(tsi);
                }
            }
            catch { }
            string[] inums = new string[] { "1 sek", "2 sek", "5 sek", "10 sek", "20 sek", "30 sek", "1 min", "2 min", "5 min", "10 min", "20 min", "30 min", "1 tim", "2 tim", "5 tim", "8 tim", "10 tim", "12 tim", "24 tim" };

            foreach(string inum in inums)
            {
                ToolStripItem tsi = intervallToolStripMenuItem.DropDownItems.Add(inum);
                tsi.Click += (a, b) => {
                    setInterval((a as ToolStripMenuItem).Text);
                    foreach (ToolStripMenuItem t in intervallToolStripMenuItem.DropDownItems)
                    {
                        t.Checked = false;
                    }
                    (a as ToolStripMenuItem).Checked = true;
                };
                if (intervallToolStripMenuItem.DropDownItems.Count == 1)
                {
                    (intervallToolStripMenuItem.DropDownItems[0] as ToolStripMenuItem).Checked = true;
                }
                ToolStripItem tsi2 = intervallToolStripMenuItem1.DropDownItems.Add(inum);
                tsi2.Click += (a, b) =>
                {
                    setRInterval((a as ToolStripMenuItem).Text);
                    foreach (ToolStripMenuItem t in intervallToolStripMenuItem1.DropDownItems)
                    {
                        t.Checked = false;
                    }
                    (a as ToolStripMenuItem).Checked = true;
                };
                if (intervallToolStripMenuItem1.DropDownItems.Count == 1)
                {
                    (intervallToolStripMenuItem1.DropDownItems[0] as ToolStripMenuItem).Checked = true;
                }
            }
        }

        private void setRInterval(string inum)
        {
            string[] s = inum.Split(' ');
            int i = int.Parse(s[0]);
            switch (s[1])
            {
                case "sek":
                    i = i * 1000;
                    break;
                case "min":
                    i = i * 1000 * 60;
                    break;
                case "tim":
                    i = i * 1000 * 60 * 60;
                    break;
            }
            timer1.Interval = i;
        }
        private void setInterval(string inum)
        {
            string[] s = inum.Split(' ');
            int i = int.Parse(s[0]);
            switch(s[1])
            {
                case "sek":
                    i = i * 1000;
                    break;
                case "min":
                    i = i * 1000 * 60;
                    break;
                case "tim":
                    i = i * 1000 * 60 * 60;
                    break;
            }
            timer2.Interval = i;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //_capture = new Capture();
                try
                {
                    //_capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 1280);
                    //_capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 720);
                }

                catch { }
                Image<Bgr, Byte> frame = _capture.QueryFrame();
                if (refreshToolStripMenuItem.Checked)
                    pictureBox1.Image = frame.Bitmap;
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.Width = frame.Width / 2;
                    this.Height = frame.Height / 2;
                }
                if (runServerToolStripMenuItem.Checked)
                {
                    if (_server.Pending())
                    {
                        TcpClient client = _server.AcceptTcpClient();
                        if (client.Connected)
                        {
                            using (StreamReader SR = new System.IO.StreamReader(client.GetStream()))
                            {
                                string str = SR.ReadLine();
                                if (!string.IsNullOrEmpty(str))
                                {
                                    string[] strParts = str.Split(' ');
                                    string strPart = strParts[1];
                                    if (strPart.Contains("/photo.jpg"))
                                        sendImgToClient(client, frame.Bitmap);
                                    else if (strPart.Contains("/frames"))
                                        sendFileToClient(client, Resources.frames);
                                    else
                                        sendFileToClient(client, Resources.index);
                                }
                                else
                                {
                                    sendEmptyToClient(client);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pictureBox1.Image = null;
                //_capture.Dispose();
            }
            finally
            {
                System.GC.Collect();
            }
        }

        private void sendFileToClient(TcpClient in_client, string file)
        {
            if (in_client.Connected)
            {
                StreamReader SR = new System.IO.StreamReader(in_client.GetStream());
                MemoryStream ms1 = new MemoryStream();
                MemoryStream ms2 = new MemoryStream();

                byte[] bimg = ms2.ToArray();

                string strHeader = "HTTP/1.1 200 Ok\nConnection: close\nServer: My Webcam Server v0.1\nCache-Control: no-cache, must-revalidate, pre-check=0, post-check=0, max-age=0\nPragma: no-cache\nExpires: -1\nAccess-Control-Allow-Origin: *\nContent-Type: text/html\r\n\r\n";
                strHeader += file;

                ms1.Write(System.Text.Encoding.Default.GetBytes(strHeader), 0, strHeader.Length);
                ms1.Write(bimg, 0, bimg.Length);

                byte[] bsend = ms1.ToArray();
                SR.BaseStream.Write(bsend, 0, bsend.Length);
                in_client.Close();
            }
        }

        private void sendImgToClient(TcpClient in_client, Image in_img)
        {
            if (in_client.Connected)
            {
                StreamReader SR = new System.IO.StreamReader(in_client.GetStream());
                MemoryStream ms1 = new MemoryStream();
                MemoryStream ms2 = new MemoryStream();
                in_img.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);

                byte[] bimg = ms2.ToArray();

                string strHeader = "HTTP/1.1 200 Ok\nConnection: close\nServer: My Webcam Server v0.1\nCache-Control: no-cache, must-revalidate, pre-check=0, post-check=0, max-age=0\nPragma: no-cache\nExpires: -1\nAccess-Control-Allow-Origin: *\nContent-Type: image/jpeg\r\n\r\n";

                ms1.Write(System.Text.Encoding.Default.GetBytes(strHeader), 0, strHeader.Length);
                ms1.Write(bimg, 0, bimg.Length);

                byte[] bsend = ms1.ToArray();
                SR.BaseStream.Write(bsend, 0, bsend.Length);
                in_client.Close();
            }
        }


        private void sendEmptyToClient(TcpClient in_client)
        {
            if (in_client.Connected)
            {
                StreamReader SR = new System.IO.StreamReader(in_client.GetStream());
                MemoryStream ms1 = new MemoryStream();
                MemoryStream ms2 = new MemoryStream();

                byte[] bimg = ms2.ToArray();

                string strHeader = "HTTP/1.1 302 Ok\nConnection: close\nServer: My Webcam Server v0.1\nLocation:/photo.jpg\nCache-Control: no-cache, must-revalidate, pre-check=0, post-check=0, max-age=0\nPragma: no-cache\nExpires: -1\nAccess-Control-Allow-Origin: *\nContent-Type: image/jpeg\r\n\r\n";

                ms1.Write(System.Text.Encoding.Default.GetBytes(strHeader), 0, strHeader.Length);

                byte[] bsend = ms1.ToArray();
                SR.BaseStream.Write(bsend, 0, bsend.Length);
                in_client.Close();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _server.Stop();
            }
            catch { }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            DateTime dtnow = DateTime.Now;
            string strdirpath = Application.CommonAppDataPath + "\\" + System.DateTime.Today.ToString("yyyyMMdd") + "\\";
            if (!Directory.Exists(strdirpath))
                Directory.CreateDirectory(strdirpath);

            string strpath = strdirpath + DateTime.Now.ToString("HHmmss") + ".jpg";

            Image<Bgr, Byte> frame = _capture.QueryFrame();
            frame.Save(strpath);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start(Application.CommonAppDataPath);
        }
        int itim1int = 0;
        private void runServerToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (runServerToolStripMenuItem.Checked)
                {
                    _server.Start(); this.Text = strFormText + " - started";
                    itim1int = timer1.Interval;
                    timer1.Interval = 200;
                    timer1.Enabled = true;
                    timer1_Tick(sender, e);
                }
                else
                {
                    timer1.Interval = itim1int;
                    _server.Stop(); this.Text = strFormText + " - stopped";
                    timer1.Enabled = true;
                }
            }
            catch { }
        }

        private void saveFilesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            timer2.Enabled = saveFilesToolStripMenuItem.Checked;
            if (timer2.Enabled)
                timer2_Tick(sender, e);
        }

        private void openWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                IPEndPoint ipep = _server.LocalEndpoint as IPEndPoint;
                Process.Start("http://"+ipep.Address.ToString() + ":" + ipep.Port.ToString());
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
    }
}
