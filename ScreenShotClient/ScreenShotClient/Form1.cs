using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;
using N = System.Net;
using System.Collections;
using System.Runtime.InteropServices;
using System.Configuration;

namespace ScreenShotClient
{
    public partial class Form1 : Form
    {
        N.Sockets.TcpClient TC = new N.Sockets.TcpClient();
        public Form1()
        {
            InitializeComponent();
        }
        static string strDummy = "";
        static AppSettingsReader asr = new AppSettingsReader();
        static string sIP = (string)asr.GetValue("ip", strDummy.GetType());
        static int iPort = int.Parse((string)asr.GetValue("port", strDummy.GetType()));
        static string stoken = (string)asr.GetValue("stoken", strDummy.GetType());
        static string etoken = (string)asr.GetValue("etoken", strDummy.GetType());
        static float ifps = float.Parse((string)asr.GetValue("fps", strDummy.GetType()));
        String data = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            float flo = (1 / ifps);
            flo = flo * 1000;

            double dbl = Math.Round(double.Parse(flo.ToString()), 0);
            timer1.Interval = int.Parse(dbl.ToString());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (TC.Connected && TC.GetStream() != null)
                {
                    toolStripStatusLabel1.Text = "C:" + TC.Connected.ToString();
                    toolStripStatusLabel1.Text += ",DA:" + TC.GetStream().DataAvailable.ToString();
                    toolStripStatusLabel1.Text += ",CR:" + TC.GetStream().CanRead.ToString();
                    toolStripStatusLabel1.Text += ",CW:" + TC.GetStream().CanWrite.ToString();
                    N.Sockets.NetworkStream NS = TC.GetStream();
                    if (NS.DataAvailable)
                    {
                        StreamReader sr = new StreamReader(NS);
                        
                        Byte[] bytes = new Byte[4096];
                        
                        
                        int i;
                        int icnt = 0;

                        // Loop to receive all the data sent by the client.
                        try
                        {
                            while (NS.DataAvailable && (i = sr.BaseStream.Read(bytes, 0, bytes.Length)) != 0)
                            {
                                Application.DoEvents();
                                // Translate data bytes to a ASCII string.

                                data += System.Text.Encoding.Default.GetString(bytes, 0, i);
                                data = togglePic(data);

                                // Process the data sent by the client.
                                //data = data.ToUpper();
                                //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);


                                // Send back a response.
                                //sr.BaseStream.Write(msg, 0, msg.Length);

                                //GC.Collect();
                                if (icnt % 50 == 0 && icnt > 0)
                                {
                                    data = "";
                                }
                                icnt++;
                            }
                            data = togglePic(data);
                        }
                        catch (Exception ex)
                        {
                            //toolStripStatusLabel1.Text = ",ERR:" + ex.Message;
                            //Console.Write(ex.Message);
                        }
                        //listBox1.Items.Add(data);
                        //listBox1.SelectedIndex = listBox1.Items.Count-1;

                    }
                }
                else if (!TC.Connected)
                {
                    try
                    {
                        TC.Connect(sIP, iPort);
                    }
                    catch(Exception ex)
                    {
                        //toolStripStatusLabel1.Text += ",ERR:" + ex.Message;
                        //Console.Write(ex.Message);
                    }
                
                }
                
            }
            catch (Exception ex)
            {
                //toolStripStatusLabel1.Text += ",ERR:" + ex.Message;
                //Console.Write(ex.Message);
            }
            GC.Collect();
        }
        private string togglePic(string in_data)
        {
            string r_data = in_data;
            int fGIF89 = r_data.IndexOf(stoken);
            if (fGIF89 != -1)
            {
                int fGIFEnd1 = r_data.IndexOf(stoken, fGIF89 + 1);
                int fGIFEnd2 = r_data.IndexOf(etoken, fGIF89 + 1);
                if (fGIFEnd1 != -1 || fGIFEnd2 != -1)
                {
                    r_data = r_data.Substring(fGIF89 + stoken.Length, r_data.Length - (fGIF89 + stoken.Length));
                    if (fGIFEnd1 != -1 && fGIFEnd1 < (r_data.Length) && fGIFEnd2 > fGIFEnd1)
                    {
                        r_data = r_data.Substring(0, fGIFEnd1);
                    }
                    else if (fGIFEnd2 != -1 && fGIFEnd2 < (r_data.Length))
                    {
                        r_data = r_data.Substring(0, fGIFEnd2);
                    }

                    byte[] msg = System.Text.Encoding.Default.GetBytes(r_data);
                    //toolStripStatusLabel1.Text += ",DL:" + r_data.Length;
                    MemoryStream ms = new MemoryStream(msg);
                    pictureBox1.Image = Bitmap.FromStream(ms, true, true);
                    pictureBox1.Refresh();
                    this.Refresh();
                    r_data = "";
                }
            }
            return r_data;
        }


        private void autosizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            autosizeToolStripMenuItem.Checked = true;
            stretchimageToolStripMenuItem.Checked = false;
            centerimageToolStripMenuItem.Checked = false;
            zoomToolStripMenuItem.Checked = false;
            normalToolStripMenuItem.Checked = false;
        }

        private void stretchimageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            autosizeToolStripMenuItem.Checked = false;
            stretchimageToolStripMenuItem.Checked = true;
            centerimageToolStripMenuItem.Checked = false;
            zoomToolStripMenuItem.Checked = false;
            normalToolStripMenuItem.Checked = false;
        }

        private void centerimageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            autosizeToolStripMenuItem.Checked = false;
            stretchimageToolStripMenuItem.Checked = false;
            centerimageToolStripMenuItem.Checked = true;
            zoomToolStripMenuItem.Checked = false;
            normalToolStripMenuItem.Checked = false;
        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;            
            autosizeToolStripMenuItem.Checked = false;
            stretchimageToolStripMenuItem.Checked = false;
            centerimageToolStripMenuItem.Checked = false;
            zoomToolStripMenuItem.Checked = true;
            normalToolStripMenuItem.Checked = false;
        }

        private void timerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerToolStripMenuItem.Checked = !timerToolStripMenuItem.Checked;
            timer1.Enabled = timerToolStripMenuItem.Checked;
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            autosizeToolStripMenuItem.Checked = false;
            stretchimageToolStripMenuItem.Checked = false;
            centerimageToolStripMenuItem.Checked = false;
            zoomToolStripMenuItem.Checked = false;
            normalToolStripMenuItem.Checked = true;
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!TC.Connected)
            {
                try
                {
                    TC.Connect(sIP, iPort);
                    MessageBox.Show("Connected to:" + sIP + "("+ iPort.ToString() +")");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERR-connecting:" + ex.Message);
                }

            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TC.Connected)
            {
                TC.Client.Disconnect(true);
            }
        }
    }
}
