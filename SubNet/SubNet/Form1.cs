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

namespace SubNet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string strUrlStart = "http://www.submachineworld.com/";
        string strUrlBase = "http://www.submachineworld.com/subnet_data/";
        CoordFiles cfiles = new CoordFiles();
        string strDownloadPath = Application.UserAppDataPath;
        List<string> added = new List<string>();
        
        int icntDone {
            get { return added.Count(); }
            set { 
            if (added.Count() >= cfiles.Count)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            btnRefresh.Enabled = true; btnReloadFiles.Enabled = true; this.UseWaitCursor = false; ;
                        });
                    }
                    else
                    {
                        btnRefresh.Enabled = true; btnReloadFiles.Enabled = true; this.UseWaitCursor = false; ;
                    }
                }
            }
        }
        
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            added.Clear();
            btnRefresh.Enabled = false;
            btnReloadFiles.Enabled = false;
            Cursor = Cursors.WaitCursor;
            foreach(CoordFile cf in cfiles)
            {
                ThreadPool.QueueUserWorkItem(DownIfChanged,cf.Url);                
            }
        }

        private void DownIfChanged(object state)
        {
            string strFile = state.ToString();

            CoordFile cf = cfiles.FirstOrDefault(f => f.Url.Equals(strFile));

            HttpWebRequest wrq = WebRequest.Create(strFile) as HttpWebRequest;
            wrq.Method = "GET";
            wrq.IfModifiedSince = (cf != null ? cf.LastModified : DateTime.MinValue);
            try
            {
                using (WebResponse wr = wrq.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                    {
                        string[] parts = strFile.Replace("http://", "").Split('/');
                        string strFilePath = strDownloadPath + "\\" + (parts.Length >= 3 ? parts[2] : parts[1]);
                        if (cf != null)
                            strFilePath = cf.FullName;

                        if (File.Exists(strFilePath))
                            File.Delete(strFilePath);

                        using (FileStream fs = new FileStream(strFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                        {
                            byte[] buffer = Encoding.Default.GetBytes(sr.ReadToEnd());
                            fs.Write(buffer, 0, buffer.Length);
                            fs.Close();
                        }

                        if (cf == null)
                        {
                            cfiles.AddCf(new CoordFile(new FileInfo(strFilePath)));
                        }
                        cf = cfiles.FirstOrDefault(f => f.FullName.Equals(strFilePath));
                        cf.LastModified = DateTime.Parse(wr.Headers["Last-Modified"]);
                        cf.LastRefreshed = DateTime.Now;

                        sr.Close();
                    }
                    wr.Close();
                }
            }
            catch (WebException we)
            {

            }
            finally
            {
                if (cf != null)
                {
                    cf.LastRefreshed = DateTime.Now;
                }
                lock(added)
                {
                    added.Add(strFile);
                }
                
                if (icntDone >= cfiles.Count())
                {
                    this.UseWaitCursor = false;
                    //Cursor = Cursors.Default;
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(strDownloadPath + "\\coordfiles.xml"))
                return;
            string xml = File.ReadAllText(strDownloadPath + "\\coordfiles.xml");
            cfiles = (SeDes.ToObj(xml, cfiles) as CoordFiles);

            dataGridView1.DataSource = getFiles();

            cfiles.PropertyChanged += (a, b) => {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        dataGridView1.DataSource = getFiles();
                    });                
                }
                else
                {
                    dataGridView1.DataSource = getFiles();
                }
            };

            
        }

        private List<CoordFile> getFiles()
        {
            return cfiles.OrderByDescending(f=>f.LastModified).ToList<CoordFile>();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string xmlser = SeDes.ToXml(cfiles);
            File.WriteAllText(strDownloadPath + "\\coordfiles.xml", xmlser);
        }

        private void btnReloadFiles_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 1000; i++)
            {
                string strFile = strUrlBase + i.ToString().PadLeft(3, '0') + ".swf";
                ThreadPool.QueueUserWorkItem(DownIfChanged, strFile);
            }
            
            string[] karmas = new string[] {"btn", "cat", "chm", "chr", "fma", "glx", "hpl", "ixt", "jul", "kol", "mnt", "oxt", "ptl", "v12", "wrt" , "zwo"};
            
            foreach (string k in karmas)
            {
                string strFile = strUrlBase + k + ".swf";
                ThreadPool.QueueUserWorkItem(DownIfChanged, strFile);
            }

            ThreadPool.QueueUserWorkItem(DownIfChanged, strUrlStart + "./subnet_start.swf");

        }

        private void btnOpenfolder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(strDownloadPath);
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            var lastchanged = cfiles.OrderByDescending(f => f.LastModified).Take(10).ToList<CoordFile>();
            Font font = new Font("Arial",9);
            for (int i = 0; i < lastchanged.Count; i++)
            {
                CoordFile cf = lastchanged[i];
                int ic = (lastchanged.Count*2) - (i + 5);
                Color color = Color.FromArgb(ic*7,ic*15,ic*15);
                if (IsToday(cf.LastModified))
                {
                    color = Color.DarkGreen;
                }
                dataGridView1.Rows[i].DefaultCellStyle = new DataGridViewCellStyle() {Font=font, BackColor = color,ForeColor=Color.FromArgb((255-color.R),(255-color.G),(255-color.B)) };
            }
            dataGridView1.Columns["FullName"].Visible = false;
            dataGridView1.Columns["Url"].Visible = false;
            dataGridView1.AutoResizeColumns();
        }

        private bool IsToday(DateTime dateTime)
        {
            return (new TimeSpan(DateTime.Now.Ticks - dateTime.Ticks)).Days < 2;
        }

    }
}
