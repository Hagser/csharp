using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;

namespace MyScreenCapture
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ArrayList al = getOutDevs();
            foreach (WaveInCaps wic in getRecDevs())
            {
                ToolStripMenuItem tsi = new ToolStripMenuItem();
                tsi.Text = new string(wic.szPname);
                tsi.Tag = wic;
                tsi.CheckOnClick = true;
                tsi.Click += (a, b) => {
                    foreach (ToolStripMenuItem tsmidd in recDevsToolStripMenuItem.DropDownItems)
                    {
                        if(!tsmidd.Text.Equals((a as ToolStripMenuItem).Text))
                            tsmidd.Checked = !(a as ToolStripMenuItem).Checked;
                    }
                };
                recDevsToolStripMenuItem.DropDownItems.Add(tsi);
            }
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct WaveInCaps
        {
            public int uDeviceID;
            public short wMid;
            public short wPid;
            public int vDriverVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] szPname;
            public uint dwFormats;
            public short wChannels;
            public short wReserved1;
        }

        [DllImport("winmm.dll")]
        public static extern int waveOutGetNumDevs();
        [DllImport("winmm.dll", EntryPoint = "waveOutGetDevCaps")]
        public static extern int waveOutGetDevCapsA(int uDeviceID,
                             ref WaveInCaps lpCaps, int uSize);

        [DllImport("winmm.dll")]
        public static extern int waveInGetNumDevs();
        [DllImport("winmm.dll", EntryPoint = "waveInGetDevCaps")]
        public static extern int waveInGetDevCapsA(int uDeviceID,
                             ref WaveInCaps lpCaps, int uSize);
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        int x, y;
        public ArrayList getRecDevs() //fill sound recording devices array
        {
            ArrayList arrLst = new ArrayList();
            int waveInDevicesCount = waveInGetNumDevs(); //get total
            if (waveInDevicesCount > 0)
            {
                for (int uDeviceID = 0; uDeviceID < waveInDevicesCount; uDeviceID++)
                {
                    WaveInCaps waveInCaps = new WaveInCaps();
                    waveInGetDevCapsA(uDeviceID,ref waveInCaps, 
                                      Marshal.SizeOf(typeof(WaveInCaps)));
                    waveInCaps.uDeviceID = uDeviceID;
                    waveInCaps.szPname = new string(waveInCaps.szPname).Remove(new string(waveInCaps.szPname).IndexOf('\0')).Trim().ToCharArray();
                    arrLst.Add(waveInCaps);
                    //arrLst.Add(new string(waveInCaps.szPname).Remove(
                               //new string(waveInCaps.szPname).IndexOf('\0')).Trim());
                }
            }
            return arrLst;
        }
        public ArrayList getOutDevs() //fill sound recording devices array
        {
            ArrayList arrLst = new ArrayList();
            int waveOutDevicesCount = waveOutGetNumDevs(); //get total
            if (waveOutDevicesCount > 0)
            {
                for (int uDeviceID = 0; uDeviceID < waveOutDevicesCount; uDeviceID++)
                {
                    WaveInCaps waveInCaps = new WaveInCaps();
                    waveOutGetDevCapsA(uDeviceID, ref waveInCaps,
                                      Marshal.SizeOf(typeof(WaveInCaps)));
                    waveInCaps.uDeviceID = uDeviceID;
                    waveInCaps.szPname = new string(waveInCaps.szPname).Remove(new string(waveInCaps.szPname).IndexOf('\0')).Trim().ToCharArray();
                    arrLst.Add(waveInCaps);
                    //arrLst.Add(new string(waveInCaps.szPname).Remove(
                    //new string(waveInCaps.szPname).IndexOf('\0')).Trim());
                }
            }
            return arrLst;
        } 
        private void timer1_Tick(object sender, EventArgs e)
        {
            Image img = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(img);
            Screen s = Screen.FromHandle(this.Handle);

            g.CopyFromScreen(x,y, 0, 0, new Size(this.Width, this.Height));
            

            if (saveFilesToolStripMenuItem.Checked)
                img.Save(Application.CommonAppDataPath + "\\" + DateTime.Now.Ticks.ToString()+".jpg");

            if(showImageToolStripMenuItem.Checked)
                pictureBox1.Image = img;
        }

        private void saveFilesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if(saveFilesToolStripMenuItem.Checked)
            {
                audioToolStripMenuItem.Checked = true;
                videoToolStripMenuItem.Checked = true;
            }
        }

        private void runToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = runToolStripMenuItem.Checked;
            if (audioToolStripMenuItem.Checked)
            {
                if (runToolStripMenuItem.Checked)
                {
                    startRecordingAudio();
                }
                else
                {
                    stopRecordingAudioAndSave();
                }
            }
        }
        private void startRecordingAudio()
        {
            int iinfo = 0;
            iinfo=mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
            iinfo=mciSendString("record recsound", "", 0, 0);
        }
        private void stopRecordingAudioAndSave()
        {
            string strSound = Application.CommonAppDataPath + "\\" + DateTime.Now.Ticks.ToString() + ".wav";
            int iinfo = 0;
            iinfo=mciSendString("save recsound " + strSound, "", 0, 0);
            iinfo=mciSendString("close recsound ", "", 0, 0);        
        }


        private void Form1_Move(object sender, EventArgs e)
        {
            if (setLocationToolStripMenuItem.Checked)
            {
                x = this.Location.X;
                y = this.Location.Y;
            }
        }
    }
}
