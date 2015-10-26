using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using N = System.Net;
using System.IO;
using System.Configuration;
using Gma.UserActivityMonitor;

namespace ScreenShot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static string strDummy="";
        Hashtable htClients = new Hashtable();
        static AppSettingsReader asr = new AppSettingsReader();
        static string sIP = (string)asr.GetValue("ip", strDummy.GetType());
        static int iPort = int.Parse((string)asr.GetValue("port", strDummy.GetType()));
        static string stoken = (string)asr.GetValue("stoken", strDummy.GetType());
        static string etoken = (string)asr.GetValue("etoken", strDummy.GetType());
        static float ifps = float.Parse((string)asr.GetValue("fps", strDummy.GetType()));
        static string picpath = (string)asr.GetValue("picpath", strDummy.GetType());
        System.Net.Sockets.TcpListener server = new System.Net.Sockets.TcpListener(N.IPAddress.Parse(sIP), iPort);

        static int xstart = 200;
        static int ystart = 200;
        static int iwidth = 200;
        static int iheight = 200;
        static int mx = 0;
        static int my = 0;
        bool bstartpart = false;
        private void button1_Click(object sender, EventArgs e)
        {

        }
        System.Drawing.Imaging.ImageFormat getImageFormat(string in_ext)
        {
            System.Drawing.Imaging.ImageFormat if_ret = new System.Drawing.Imaging.ImageFormat(Guid.NewGuid());
            switch (in_ext.ToLower())
            { 
                case "bmp":
                    if_ret = System.Drawing.Imaging.ImageFormat.Bmp;
                    break;
                case "gif":
                    if_ret = System.Drawing.Imaging.ImageFormat.Gif;
                    break;
                case "jpg":
                    if_ret = System.Drawing.Imaging.ImageFormat.Jpeg;
                    break;
                case "ico":
                    if_ret = System.Drawing.Imaging.ImageFormat.Icon;
                    break;
                case "png":
                    if_ret = System.Drawing.Imaging.ImageFormat.Png;
                    break;
                case "tif":
                    if_ret = System.Drawing.Imaging.ImageFormat.Tiff;
                    break;
            }
            return if_ret;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                ScreenCapture sc = new ScreenCapture();
                // capture entire screen, and save it to a file
                Image img = sc.CaptureWindow(this.Handle);
                if (partToolStripMenuItem.Checked)
                {
                    img = sc.CaptureWindow(this.Handle,xstart,ystart,iwidth,iheight);
                }
                if (!thisToolStripMenuItem.Checked)
                {
                    if (partToolStripMenuItem.Checked)
                    {
                        img = sc.CaptureScreen(xstart, ystart, iwidth, iheight);
                    }
                    else
                    {
                        img = sc.CaptureScreen();
                    }
                }
                
                if (saveToolStripMenuItem.Checked)
                {
                    string strpicpath = "";
                    if (Directory.Exists(picpath) || (picpath.IndexOf(@":\") != -1))
                    {
                        strpicpath = picpath;
                    }
                    else if (picpath.IndexOf(@":\") == -1)
                    {
                        strpicpath = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\" + picpath;
                    }
                    if (!Directory.Exists(strpicpath))
                    {
                        Directory.CreateDirectory(strpicpath);
                    }
                    img.Save(strpicpath + "\\Cap_" + System.DateTime.Now.Ticks.ToString() + "." + toolStripComboBox1.Text, getImageFormat(toolStripComboBox1.Text));
                }
                if (showToolStripMenuItem.Checked)
                {
                    // display image in a Picture control named imageDisplay
                    this.pictureBox1.Image = img;
                    if (partToolStripMenuItem.Checked && sameSizeToolStripMenuItem.Checked)
                    {
                        this.Width = pictureBox1.Width + 10;
                        this.Height = pictureBox1.Height + (statusStrip1.Height*2) + 10;
                    }
                }
                
                if (server.Pending())
                {
                    N.Sockets.TcpClient client = server.AcceptTcpClient();
                    htClients.Add(client, client);

                    sendImgToClient(client, img);

                }
                if (htClients.Count > 0)
                {
                    toolStripStatusLabel1.Text = "Connection made (" + htClients.Count.ToString() + ")";
                }
                else
                {
                    toolStripStatusLabel1.Text = "server started";
                }
                
                foreach (object k in htClients.Keys)
                {
                    N.Sockets.TcpClient client = (N.Sockets.TcpClient)k;
                    if (client.Connected)
                    {
                        sendImgToClient(client,img);
                    }
                    else
                    {
                        client.Client.Disconnect(false);
                        htClients.Remove(k);
                    }
                }
                
                GC.Collect();
            }
            catch(Exception ex)
            {
                toolStripStatusLabel1.Text += ex.Message;
                
            }
            pictureBox1.Refresh();
            this.Refresh();

        }
        private void sendImgToClient(N.Sockets.TcpClient in_client,Image in_img)
        {
            StreamReader SR = new System.IO.StreamReader(in_client.GetStream());
            StreamWriter SW = new System.IO.StreamWriter(in_client.GetStream());
            MemoryStream ms1 = new MemoryStream();
            MemoryStream ms2 = new MemoryStream();
            in_img.Save(ms2, getImageFormat(toolStripComboBox1.Text));

            byte[] bimg = ms2.ToArray();

            ms1.Write(System.Text.Encoding.Default.GetBytes(stoken), 0, stoken.Length);
            ms1.Write(bimg, 0, bimg.Length);
            ms1.Write(System.Text.Encoding.Default.GetBytes(etoken), 0, etoken.Length);

            byte[] bsend = ms1.ToArray();
            SR.BaseStream.Write(bsend, 0, bsend.Length);
            /*
            SR.Dispose();
            SW.Dispose();
            ms1.Dispose();
            ms2.Dispose();
            in_img.Dispose();
            */
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (object k in htClients.Keys)
            {
                N.Sockets.TcpClient client = (N.Sockets.TcpClient)k;
                if (client.Connected)
                {
                    client.Close();
                }
            }
        }

        private void chkShow_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Move(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            this.Refresh();
        }


        //##################################################################
        #region Check boxes to set or remove particular event handlers.
        /*
        private void checkBoxOnMouseMove_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOnMouseMove.Checked)
            {
                HookManager.MouseMove += HookManager_MouseMove;
            }
            else
            {
                HookManager.MouseMove -= HookManager_MouseMove;
            }
        }

        private void checkBoxOnMouseClick_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOnMouseClick.Checked)
            {
                HookManager.MouseClick += HookManager_MouseClick;
            }
            else
            {
                HookManager.MouseClick -= HookManager_MouseClick;
            }
        }

        private void checkBoxOnMouseUp_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOnMouseUp.Checked)
            {
                HookManager.MouseUp += HookManager_MouseUp;
            }
            else
            {
                HookManager.MouseUp -= HookManager_MouseUp;
            }
        }

        private void checkBoxOnMouseDown_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOnMouseDown.Checked)
            {
                HookManager.MouseDown += HookManager_MouseDown;
            }
            else
            {
                HookManager.MouseDown -= HookManager_MouseDown;
            }
        }

        private void checkBoxMouseDoubleClick_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMouseDoubleClick.Checked)
            {
                HookManager.MouseDoubleClick += HookManager_MouseDoubleClick;
            }
            else
            {
                HookManager.MouseDoubleClick -= HookManager_MouseDoubleClick;
            }
        }

        private void checkBoxMouseWheel_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMouseWheel.Checked)
            {
                HookManager.MouseWheel += HookManager_MouseWheel;
            }
            else
            {
                HookManager.MouseWheel -= HookManager_MouseWheel;
            }
        }

        private void checkBoxKeyDown_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxKeyDown.Checked)
            {
                HookManager.KeyDown += HookManager_KeyDown;
            }
            else
            {
                HookManager.KeyDown -= HookManager_KeyDown;
            }
        }


        private void checkBoxKeyUp_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxKeyUp.Checked)
            {
                HookManager.KeyUp += HookManager_KeyUp;
            }
            else
            {
                HookManager.KeyUp -= HookManager_KeyUp;
            }
        }

        private void checkBoxKeyPress_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxKeyPress.Checked)
            {
                HookManager.KeyPress += HookManager_KeyPress;
            }
            else
            {
                HookManager.KeyPress -= HookManager_KeyPress;
            }
        }
        */
        #endregion

        //##################################################################
        #region Event handlers of particular events. They will be activated when an appropriate checkbox is checked.

            private void HookManager_KeyDown(object sender, KeyEventArgs e)
            {
                if ((e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey) && !bstartpart)
                {
                    xstart = mx;
                    ystart = my;
                    bstartpart = true;
                    toolStripStatusLabel1.Text = String.Format("xstart:{0},ystart:{1},iwidth:{2},iheight:{3},mx:{4},my:{5}", xstart.ToString(), ystart.ToString(), iwidth.ToString(), iheight.ToString(), mx.ToString(), my.ToString());
                }
                //toolStripStatusLabel1.Text = String.Format("KeyCode:{0},bstartpart:{1}", e.KeyCode.ToString(), bstartpart.ToString());
                //textBoxLog.AppendText(string.Format("KeyDown - {0}\n", e.KeyCode));
                //textBoxLog.ScrollToCaret();
            }

            private void HookManager_KeyUp(object sender, KeyEventArgs e)
            {
                if ((e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey) && bstartpart)
                {
                    iwidth = mx - xstart;
                    iheight = my - ystart;
                    bstartpart = false;
                    toolStripStatusLabel1.Text = String.Format("xstart:{0},ystart:{1},iwidth:{2},iheight:{3},mx:{4},my:{5}", xstart.ToString(), ystart.ToString(), iwidth.ToString(), iheight.ToString(), mx.ToString(), my.ToString());
                    HookManager.MouseMove -= HookManager_MouseMove;
                    HookManager.KeyDown -= HookManager_KeyDown;
                    HookManager.KeyUp -= HookManager_KeyUp;
                }
                
                //textBoxLog.AppendText(string.Format("KeyUp - {0}\n", e.KeyCode));
                //textBoxLog.ScrollToCaret();
            }

            private void HookManager_MouseMove(object sender, MouseEventArgs e)
            {
                mx = e.X;
                my = e.Y;
                if (bstartpart)
                {
                    toolStripStatusLabel1.Text = String.Format("xstart:{0},ystart:{1},iwidth:{2},iheight:{3},mx:{4},my:{5}", xstart.ToString(), ystart.ToString(), iwidth.ToString(), iheight.ToString(), mx.ToString(), my.ToString());
                }
                //labelMousePosition.Text = string.Format("x={0:0000}; y={1:0000}", e.X, e.Y);
            }

            #region unused
            private void HookManager_KeyPress(object sender, KeyPressEventArgs e)
            {
                //textBoxLog.AppendText(string.Format("KeyPress - {0}\n", e.KeyChar));
                //textBoxLog.ScrollToCaret();
            }



            private void HookManager_MouseClick(object sender, MouseEventArgs e)
            {
                //textBoxLog.AppendText(string.Format("MouseClick - {0}\n", e.Button));
                //textBoxLog.ScrollToCaret();
            }


            private void HookManager_MouseUp(object sender, MouseEventArgs e)
            {
                //textBoxLog.AppendText(string.Format("MouseUp - {0}\n", e.Button));
                //textBoxLog.ScrollToCaret();
            }


            private void HookManager_MouseDown(object sender, MouseEventArgs e)
            {
                //textBoxLog.AppendText(string.Format("MouseDown - {0}\n", e.Button));
                //textBoxLog.ScrollToCaret();
            }


            private void HookManager_MouseDoubleClick(object sender, MouseEventArgs e)
            {
                //textBoxLog.AppendText(string.Format("MouseDoubleClick - {0}\n", e.Button));
                //textBoxLog.ScrollToCaret();
            }


            private void HookManager_MouseWheel(object sender, MouseEventArgs e)
            {
                //labelWheel.Text = string.Format("Wheel={0:000}", e.Delta);
            }
            #endregion
        #endregion

        private void chkPart_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void chkTopmost_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void topmostToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Form.ActiveForm.TopMost = topmostToolStripMenuItem.Checked;
            }
            catch { }
        }

        private void partToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (partToolStripMenuItem.Checked)
            {
                HookManager.MouseMove += HookManager_MouseMove;
                HookManager.KeyDown += HookManager_KeyDown;
                HookManager.KeyUp += HookManager_KeyUp;
            }
            else
            {
                HookManager.MouseMove -= HookManager_MouseMove;
                HookManager.KeyDown -= HookManager_KeyDown;
                HookManager.KeyUp -= HookManager_KeyUp;
            }
        }

        private void sameSizeToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (sameSizeToolStripMenuItem.Checked)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            }
            else
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;            
            }
        }

        private void showToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (!showToolStripMenuItem.Checked)
            {
                pictureBox1.Image = null;
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (startToolStripMenuItem.Text == "Start")
            {
                startToolStripMenuItem.Text = "Stop";
                server.Start();
                toolStripStatusLabel1.Text = "server started";
                htClients.Clear();
                timer1.Enabled = true;
            }
            else
            {
                startToolStripMenuItem.Text = "Start";
                foreach (object k in htClients.Keys)
                {
                    N.Sockets.TcpClient client = (N.Sockets.TcpClient)k;
                    if (client.Connected)
                    {
                        client.Client.Close(1);
                        client.Close();
                    }
                }
                htClients.Clear();
                server.Stop();
                toolStripStatusLabel1.Text = "server stopped";
                timer1.Enabled = false;
            }
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            float flo = (1 / ifps);
            flo = flo * 1000;

            double dbl = Math.Round(double.Parse(flo.ToString()), 0);
            timer1.Interval = int.Parse(dbl.ToString());
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

    }
}
