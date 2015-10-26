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

namespace HCFolderMonitor
{
    public partial class FolderMonitor : Form
    {
        public FolderMonitor()
        {
            InitializeComponent();
        }
        Timer timer1 = new Timer();
        FSW[] fsws = new FSW[10];
        static int itxt = 0;
        static int icb = itxt + 1;
        static int ibtn1 = icb + 1;
        static int itxt2 = ibtn1 + 1;
        static int itxt3 = itxt2 + 1;
        static int icb2 = itxt3 + 1;
        static int ibtn2 = icb2 + 1;
        static int ilb = ibtn2 + 1;

        double dblWidthRatio = 0;
        private void FolderMonitor_Load(object sender, EventArgs e)
        {
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 100;
            int idriv = 0;
            for (int idx = 0; idx < fsws.Length; idx++)
            {
                fsws[idx] = new FSW();
                fsws[idx].Changed += new FileSystemEventHandler(FolderMonitor_Changed);
                fsws[idx].Created += new FileSystemEventHandler(FolderMonitor_Created);
                fsws[idx].Deleted += new FileSystemEventHandler(FolderMonitor_Deleted);
                fsws[idx].Renamed += new RenamedEventHandler(FolderMonitor_Renamed);
                fsws[idx].EnableRaisingEvents = false;
                fsws[idx].IncludeSubdirectories = true;
                fsws[idx].Filter = "*.*";
                fsws[idx].Tag = idx;

                TextBox tb = new TextBox();
                tb.Width = 200;
                tb.Tag = idx;
                if ((idx + idriv) < DriveInfo.GetDrives().Length)
                {
                    DriveInfo di = DriveInfo.GetDrives()[idx + idriv];
                    while (!di.IsReady)
                    {
                        idriv++;
                        if ((idx + idriv) >= DriveInfo.GetDrives().Length)
                            break;

                        di = DriveInfo.GetDrives()[idx + idriv];
                        
                    }
                    tb.Text = di.Name;
                }
                CheckBox cb = new CheckBox();
                cb.Text = "Monitor";
                cb.Tag = idx;
                cb.CheckStateChanged += new EventHandler(cb_CheckStateChanged);
                cb.Width = 70;
                cb.Enabled = false;

                Button btn = new Button();
                btn.Width = 25;
                btn.Tag = idx;
                btn.Text = "...";
                btn.Click += new EventHandler(btn_Click);

                TextBox tb3 = new TextBox();
                tb3.Width = 30;
                tb3.Tag = idx;
                tb3.Text = "*.*";

                TextBox tb2 = new TextBox();
                tb2.Width = tb.Width - (tb3.Width + 6);
                tb2.Tag = idx;

                CheckBox cb2 = new CheckBox();
                cb2.Text = "Run";
                cb2.Tag = idx;
                cb2.CheckStateChanged += new EventHandler(cb2_CheckStateChanged);
                cb2.Width = cb.Width;
                cb2.Enabled = false;

                Button btn2 = new Button();
                btn2.Width = btn.Width;
                btn2.Tag = idx;
                btn2.Text = "...";
                btn2.Click += new EventHandler(btn2_Click);

                ListBox lb = new ListBox();
                lb.BorderStyle = BorderStyle.FixedSingle;
                lb.BackColor = Color.White;
                lb.HorizontalScrollbar = true;
                lb.Height = 100;


                FlowLayoutPanel flp = new FlowLayoutPanel();
                flp.Padding = new Padding(0);
                flp.Width = tb.Width + cb.Width + btn.Width + 20;
                flp.Height = (Math.Max(Math.Max(tb.Height, cb.Height), Math.Max(btn.Height, 0)) * 2) + lb.Height + 15;
                lb.Width = flp.Width - 10;
                //lb.Height += 10;
                dblWidthRatio = ((double)tb.Width / (double)flp.Width);
                flp.FlowDirection = FlowDirection.LeftToRight;
                flp.Controls.Add(tb);
                flp.Controls.Add(cb);
                flp.Controls.Add(btn);
                flp.Controls.Add(tb2);
                flp.Controls.Add(tb3);
                flp.Controls.Add(cb2);
                flp.Controls.Add(btn2);
                flp.Controls.Add(lb);
                flp.Tag = idx;

                flp.BorderStyle = BorderStyle.FixedSingle;
                flp.Resize += new EventHandler(flp_Resize);

                flowPanel.Controls.Add(flp);
            }
            timer1.Enabled = true;
        }

        void timer1_Tick(object sender, EventArgs e)
        {

            foreach (FlowLayoutPanel fl in flowPanel.Controls)
            {
                Application.DoEvents();
                int idx = int.Parse(fl.Tag.ToString());
                TextBox tb = ((TextBox)fl.Controls[itxt]);
                TextBox tb2 = ((TextBox)fl.Controls[itxt2]);
                TextBox tb3 = ((TextBox)fl.Controls[itxt3]);

                this.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        fsws[idx].Path = Directory.Exists(tb.Text) ? tb.Text : "";
                    }
                    catch { }
                });
                fsws[idx].Filter = tb3.Text.IndexOf("*.") != -1 && tb3.Text.Length > 3 ? tb3.Text : "";
                ((CheckBox)fl.Controls[icb]).Enabled = (tb.Text != "" && tb3.Text != "");
                ((CheckBox)fl.Controls[icb2]).Enabled = (tb2.Text != "");

            }
        }

        void flp_Resize(object sender, EventArgs e)
        {

            FlowLayoutPanel fl = (FlowLayoutPanel)sender;
            int idx = int.Parse(fl.Tag.ToString());
            TextBox tb = ((TextBox)fl.Controls[itxt]);
            TextBox tb2 = ((TextBox)fl.Controls[itxt2]);
            TextBox tb3 = ((TextBox)fl.Controls[itxt3]);
            ListBox lb = ((ListBox)fl.Controls[ilb]);
            tb.Width = int.Parse(Math.Round(((double)fl.Width * dblWidthRatio), 0).ToString());
            tb2.Width = tb.Width - (tb3.Width + 6);

            lb.Width = fl.Width - 10;

        }


        void cb_CheckStateChanged(object sender, EventArgs e)
        {
            int idx = int.Parse(((CheckBox)sender).Tag.ToString());
            FlowLayoutPanel fl = (FlowLayoutPanel)flowPanel.Controls[idx];
            if (((TextBox)fl.Controls[itxt]).Text != "" && Directory.Exists(((TextBox)fl.Controls[itxt]).Text))
            {
                fsws[idx].Path = ((TextBox)fl.Controls[itxt]).Text;
                fsws[idx].EnableRaisingEvents = (((CheckBox)sender).CheckState == CheckState.Checked);
            }
            else
            {
                ((CheckBox)sender).CheckState = CheckState.Unchecked;
            }
            //MessageBox.Show(fsws[idx].Path + ":" + fsws[idx].EnableRaisingEvents,idx.ToString());
        }

        void cb2_CheckStateChanged(object sender, EventArgs e)
        {
            int idx = int.Parse(((CheckBox)sender).Tag.ToString());
            FlowLayoutPanel fl = (FlowLayoutPanel)flowPanel.Controls[idx];

            if ((((CheckBox)sender).CheckState == CheckState.Checked))
            {
                if (((TextBox)fl.Controls[itxt2]).Text.Equals(""))
                {
                    ((CheckBox)sender).CheckState = CheckState.Unchecked;
                }
            }

            //MessageBox.Show(fsws[idx].Path + ":" + fsws[idx].EnableRaisingEvents, idx.ToString());
        }

        void btn2_Click(object sender, EventArgs e)
        {
            int idx = int.Parse(((Button)sender).Tag.ToString());
            FlowLayoutPanel fl = (FlowLayoutPanel)flowPanel.Controls[idx];
            oFD.ShowDialog();
            if (File.Exists(oFD.FileName))
            {
                ((TextBox)fl.Controls[itxt2]).Text = oFD.FileName;
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            fBD.ShowDialog();
            int idx = int.Parse(((Button)sender).Tag.ToString());
            FlowLayoutPanel fl = (FlowLayoutPanel)flowPanel.Controls[idx];
            ((TextBox)fl.Controls[itxt]).Text = fBD.SelectedPath;
            if (fBD.SelectedPath != "")
            {
                fsws[idx].Path = fBD.SelectedPath;
            }
        }
        void addToLB(ListBox lb, string in_txt)
        {
            if (lb.Items.Count > 10000)
            {
                lb.Items.RemoveAt(0);
            }
            if (in_txt.ToLower().Contains("windows\\csc\\"))
                return;

            lb.Items.Add(lb.Items.Count.ToString().PadLeft(5).Replace(" ".ToCharArray()[0],"0".ToCharArray()[0]) + "\t" + System.DateTime.Now.ToLongTimeString() + "." + System.DateTime.Now.Millisecond.ToString().PadLeft(3).Replace(" ".ToCharArray()[0], "0".ToCharArray()[0]) + "\t" + in_txt);
            lb.SelectedIndex = lb.Items.Count - 1;
        }
        void FolderMonitor_Renamed(object sender, RenamedEventArgs e)
        {
            FSW fsw = (FSW)sender;
            int idx = int.Parse(fsw.Tag.ToString());
            FlowLayoutPanel fl = (FlowLayoutPanel)flowPanel.Controls[idx];
            this.Invoke((MethodInvoker)delegate
            {
                ListBox lb = ((ListBox)fl.Controls[ilb]);
                addToLB(lb, e.OldFullPath + " " + e.ChangeType + " to " + e.FullPath);
            });

        }

        void FolderMonitor_Deleted(object sender, FileSystemEventArgs e)
        {
            FSW fsw = (FSW)sender;
            int idx = int.Parse(fsw.Tag.ToString());
            FlowLayoutPanel fl = (FlowLayoutPanel)flowPanel.Controls[idx];
            this.Invoke((MethodInvoker)delegate
            {
                ListBox lb = ((ListBox)fl.Controls[ilb]);
                addToLB(lb, e.FullPath + ":" + e.ChangeType);
            });
        }

        void FolderMonitor_Created(object sender, FileSystemEventArgs e)
        {
            FSW fsw = (FSW)sender;
            int idx = int.Parse(fsw.Tag.ToString());
            FlowLayoutPanel fl = (FlowLayoutPanel)flowPanel.Controls[idx];
            this.Invoke((MethodInvoker)delegate
            {
                ListBox lb = ((ListBox)fl.Controls[ilb]);
                addToLB(lb, e.FullPath + ":" + e.ChangeType);
            });
        }
DateTime dtLastRun;
string strLastRun;
        void FolderMonitor_Changed(object sender, FileSystemEventArgs e)
        {
            FSW fsw = (FSW)sender;
            int idx = int.Parse(fsw.Tag.ToString());
            FlowLayoutPanel fl = (FlowLayoutPanel)flowPanel.Controls[idx];
            this.Invoke((MethodInvoker)delegate
            {
                ListBox lb = ((ListBox)fl.Controls[ilb]);
                addToLB(lb, e.FullPath + ":" + e.ChangeType);
                string strPath = ((TextBox)fl.Controls[itxt2]).Text;
                if (strPath != "" && ((CheckBox)fl.Controls[icb2]).CheckState == CheckState.Checked)
                {
                    if (strPath.ToLower().EndsWith(".vbs"))
                    {
                        ProcessStartInfo psi = new ProcessStartInfo(@"C:\WINDOWS\system32\wscript.exe");
                        psi.Arguments = strPath + " " + e.FullPath;
                        FileInfo fi = new FileInfo(e.FullPath);
                        psi.WorkingDirectory = fi.Directory.FullName;
                        Process.Start(psi);
                    }
                    else
                    {
						if(strLastRun.Equals(e.FullPath))
						{
							DateTime dtNow = DateTime.Now;
							TimeSpan ts = new TimeSpan(dtNow.Ticks-dtLastRun.Ticks);
							if(ts.TotalSeconds<5)
							{
								return;
							}
							dtLastRun=dtNow;
						}
						else
						{
							strLastRun=e.FullPath;
						}
                        Process.Start(strPath, e.FullPath);
                    }
                    addToLB(lb, strPath + ":Run");
                }
            });
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool b = closeForm();
            if (b)
            {
                Application.Exit();
            }
        }

        private bool closeForm()
        {
            if (MessageBox.Show("Do you want to exit?", "Access standby?", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                
                return true;
            }
            return false;

        }

        private void MonAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (FlowLayoutPanel fl in flowPanel.Controls)
            {
                if (((CheckBox)fl.Controls[icb]).Enabled)
                {
                    ((CheckBox)fl.Controls[icb]).CheckState = CheckState.Checked;
                }
            }
        }

        private void MonNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (FlowLayoutPanel fl in flowPanel.Controls)
            {
                ((CheckBox)fl.Controls[icb]).CheckState = CheckState.Unchecked;
            }

        }

        private void RunAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (FlowLayoutPanel fl in flowPanel.Controls)
            {
                if (((CheckBox)fl.Controls[icb2]).Enabled)
                {
                    ((CheckBox)fl.Controls[icb2]).CheckState = CheckState.Checked;
                }
            }
        }

        private void RunNoneToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (FlowLayoutPanel fl in flowPanel.Controls)
            {
                ((CheckBox)fl.Controls[icb2]).CheckState = CheckState.Unchecked;
            }
        }

        private void FolderMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel=!closeForm();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
