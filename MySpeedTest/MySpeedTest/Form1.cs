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
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;
using System.Xml;

namespace MySpeedTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            bks.SettingsRefreshed += (a, b) =>
            {
                refreshMap();
            };
            timer1.Interval = 1000 * 60 * 5;
            System.Net.NetworkInformation.NetworkChange.NetworkAddressChanged += (a, b) =>
            {
                reloadServers(settingserver);
            };
            foreach (PropertyInfo pi in typeof(Speed).GetProperties().Where(x => x.CanRead && !x.Name.Equals("When")))
            {
                comboBox2.Items.Add(pi.Name);
            }
            comboBox2.SelectedIndex = 0;
            this.Text = Application.ProductName + " " + Application.ProductVersion;
            settingservers.Add(new SettingServer() { name = "Bredbandskollen", configurl = "http://www.speedtest.net/speedtest-config.php?x=", serverurl = "http://www.bredbandskollen.se/settings.php?x=" });
            settingservers.Add(new SettingServer() { name = "Speedtest", configurl="http://www.speedtest.net/speedtest-config.php?x=", serverurl = "http://www.speedtest.net/speedtest-servers.php?x=" });
            //settingservers.Add(new SettingServer() { name = "MySpeedtest", configurl = "http://192.168.1.4:8181/settings?x=", serverurl = "http://192.168.1.4:8181/settings?x=" });
            cBSettings.DataSource = settingservers;
            cBSettings.DisplayMember="name";
        }
        int ow = 0;
        int oh = 0;
        string markers = "";
        private void refreshMap()
        {
            if (wBrowse.Width != ow || wBrowse.Height != oh)
            {
                ow = wBrowse.Width;
                oh = wBrowse.Height;
                markers = "";
                string strUrl = string.Format("http://maps.hagser.se/mst.asp?width={0}&height={1}{2}{3}{4}", wBrowse.Width - 20, wBrowse.Height, getCenter(),getCurrent(),getMapMarkers());
                
                wBrowse.Navigated += (a, b) =>
                {
                    if (wBrowse.Document != null && wBrowse.Document.Window != null)
                    {
                        wBrowse.Document.Window.Load += (c, d) =>
                        {
                            UpdateMarkers();
                        };
                    }
                };
                
                GC.Collect();
                if(checkBoxUpdateMap.Checked)
                    wBrowse.Navigate(strUrl);

            }
            else
            {
                UpdateMarkers();
            }

            //string strMapUrl = "http://maps.googleapis.com/maps/api/staticmap?{2}{3}&zoom={4}&size={0}x{1}&sensor=false";

        }

        private string getCurrent()
        {
            if (selectedServer != null && selectedServer.lat!=""&&selectedServer.lon!="")
            {
                return "&current=" + selectedServer.id;
            }
            return "";
        }

        private string getCenter()
        {
            if (bks.client.lat != "" && bks.client.lon != "")
            {
                return "&center=" + bks.client.lat + "," + bks.client.lon;
            }
            return "";
        }

        private string getMapMarkers()
        {
            string ret = "";
            int icnt = 0;
            foreach (Server s in bks.ClosestServers.Take(50).ToList())
            {
                if (selectedServer == null || (selectedServer != null && s.id != selectedServer.id))
                {
                    if (s.lat != "" && s.lon != "")
                    {
                        ret += s.id + "|" + s.name + "|" + s.lat + "|" + s.lon + ";";
                        icnt++;
                    }
                }
            }
            if (icnt<100 || settingserver.name.Equals("Bredbandskollen"))
            {
                ret = "&markers=" + ret;
            }
            ret = (ret.Length > 0 ? ret.Substring(0, ret.Length - 1) : ret);
            return ret;
        }

        SettingServer settingserver = new SettingServer() { configurl = "http://www.bredbandskollen.se/settings.php?x=" };
        List<SettingServer> settingservers = new List<SettingServer>();
        bool Loaded = false;
        Server selectedServer;
        bksettings bks=new bksettings();
        SpeedInfo si = new SpeedInfo();

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartSpeedTest();
        }

        private void StartSpeedTest()
        {
            WebClient wc = new WebClient();
            TimeSpan ots = new TimeSpan(DateTime.Now.Ticks);
            long ticks= System.DateTime.Now.Ticks;
            string[] strUrls = GetImageUrls();
            int icnt = 0;
            int ierror=0;
            wc.DownloadFileCompleted += (a, b) =>
            {

                if (b.Error == null)
                {
                    icnt++;
                    TimeSpan ts = new TimeSpan(DateTime.Now.Ticks);
                    TimeSpan tsdiff = new TimeSpan((ts.Ticks - ots.Ticks));
                    FileInfo fi = new FileInfo(Application.UserAppDataPath + "\\temp.tmp");
                    double speed = Math.Round(fi.Length / tsdiff.TotalMilliseconds);
                    if (tsdiff.TotalSeconds > 0)
                    {
                        AddToListView("x", "", bks.ispname, selectedServer.id + "-" + selectedServer.name, fi.Length, speed, tsdiff.TotalSeconds.Round());
                        ierror = 0;
                    }
                }
                else
                {
                    System.Threading.Thread.Sleep(3000);
                    ierror++;
                }
                    
                ots = new TimeSpan(DateTime.Now.Ticks);
                ticks = System.DateTime.Now.Ticks;

                if (ierror < 5)
                {
                    if (icnt < strUrls.Length)
                    {
                        wc.DownloadFileAsync(new Uri(selectedServer.downurl + string.Format(strUrls[icnt], ticks)), Application.UserAppDataPath + "\\temp.tmp");
                    }
                    else
                    {
                        UploadTest();
                    }
                }
            };
            ots = new TimeSpan(DateTime.Now.Ticks);
            wc.DownloadFileAsync(new Uri(selectedServer.downurl + string.Format(strUrls[icnt], ticks)), Application.UserAppDataPath + "\\temp.tmp");
        }

        private void AddToListView(string d, string u, string isp, string server, long bytes, double speed, double diff)
        {
            Speed sp = new Speed(d.Equals("x") ? Direction.Down : u.Equals("x") ? Direction.Up : Direction.UnKnown, speed)
            {
                Bytes = bytes,
                Diff = diff,
                Isp = isp,
                Server = server,
                When = DateTime.Now
            };
            si.Add(sp);
        }


        private string[] GetImageUrls()
        {
            //string[] images = { "random350x350.jpg?x={0}&y=1", "random350x350.jpg?x={0}&y=2", "random750x750.jpg?x={0}&y=1", "random750x750.jpg?x={0}&y=2", "random1000x1000.jpg?x={0}&y=1", "random1000x1000.jpg?x={0}&y=2" };
            string[] images = { "random750x750.jpg?x={0}&y=1", "random750x750.jpg?x={0}&y=2", "random1000x1000.jpg?x={0}&y=1", "random1000x1000.jpg?x={0}&y=2", "random2000x2000.jpg?x={0}&y=1", "random2000x2000.jpg?x={0}&y=2" };
            return images;
        }

        private void UploadTest()
        {
            WebClient wc = new WebClient();
            long ticks= System.DateTime.Now.Ticks;
            Uri uri = new Uri(selectedServer.url + "?x=" + ticks);
            TimeSpan ts = new TimeSpan(System.DateTime.Now.Ticks);
            int icnt = 0;
            int ierror = 0;
            wc.UploadDataCompleted += (a, b) =>
            {
                if (b.Error == null)
                {
                    icnt++;
                    TimeSpan tsdone = new TimeSpan(DateTime.Now.Ticks);
                    TimeSpan tsdiff = new TimeSpan((tsdone.Ticks - ts.Ticks));

                    double speed = Math.Round(int.Parse(b.UserState.ToString()) / tsdiff.TotalMilliseconds);
                    if (tsdiff.TotalSeconds > 0)
                    {
                        AddToListView("", "x", bks.ispname, selectedServer.id + "-" + selectedServer.name, long.Parse(b.UserState.ToString()), speed, tsdiff.TotalSeconds.Round());
                        ierror = 0;
                    }
                }
                else
                {
                    System.Threading.Thread.Sleep(3000);
                    ierror++;
                }
                if (icnt < 6 && ierror<5)
                {
                    ticks = System.DateTime.Now.Ticks;
                    uri = new Uri(selectedServer.url + "?x=" + ticks);
                    byte[] buffer2 = Encoding.Default.GetBytes("content0=" + GetRandomChars(icnt));
                    ts = new TimeSpan(System.DateTime.Now.Ticks);
                    wc.UploadDataAsync(uri, "POST", buffer2, buffer2.Length);
                }
            };
            byte[] buffer1 = Encoding.Default.GetBytes("content0=" + GetRandomChars(icnt));
            ts = new TimeSpan(System.DateTime.Now.Ticks);
            wc.UploadDataAsync(uri, "POST", buffer1, buffer1.Length);
        }

        private string GetRandomChars(int icnt)
        {
            string strRet = "";
            Random rnd = new Random(int.Parse(System.DateTime.Now.Millisecond.ToString()));
            int len = (2000000 / (6 - icnt));
            while (strRet.Length < len)
            {
                strRet += rnd.Next().ToString() + strRet;
            }

            strRet = (strRet.Length >= len) ? strRet.Substring(0, len) : strRet;
            return Convert.ToBase64String(Encoding.Default.GetBytes(strRet));
        }

        private void chkTimed_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = chkTimed.Checked && selectedServer != null;
            if (timer1.Enabled)
            {
                StartSpeedTest();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (cBAlways.Checked || InInterval())
            {
                Random rnd = new Random();
                //comboBox1.SelectedIndex = rnd.Next(0, comboBox1.Items.Count - 1);
                if (comboBox1.SelectedIndex < comboBox1.Items.Count - 1)
                {
                    comboBox1.SelectedIndex++;
                }
                else 
                {
                    comboBox1.SelectedIndex=0;
                }
                StartSpeedTest();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                selectedServer = (comboBox1.SelectedItem as Server);
                refreshMap();
            }
            else
            {
                selectedServer = null;
            }
            btnStart.Enabled = selectedServer != null;
            timer1.Enabled = chkTimed.Checked && selectedServer != null;
            chkTimed.Enabled = selectedServer != null;
        }


        private void UpdateMarkers()
        {
            if (!settingserver.name.Equals("Bredbandskollen") && wBrowse.Document != null && wBrowse.Document.All["hdnmarkers"] != null)
            {
                string nmarkers = getMapMarkers();
                //if (!nmarkers.Equals(markers))
                {
                    markers = nmarkers;
                    wBrowse.Document.All["hdnmarkers"].InnerText = markers;
                    wBrowse.Document.InvokeScript("loadMarkers");
                }
            }
        }

        private bool InInterval()
        {
            DateTime dtFrom = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " " + mTbFrom.Text);
            DateTime dtTo = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " " + mTbTo.Text);
            bool bHoursOk = (mTbFrom.Text.Equals("00:00") && mTbTo.Text.Equals("00:00")) || (dtFrom <= DateTime.Now && dtTo >= DateTime.Now);
            bool bDayOfWeek = false;
            foreach (object o in checkedListBox1.CheckedItems)
            {
                if (DateTime.Today.DayOfWeek.CompareTo(o)==0)
                {
                    bDayOfWeek = true;
                    break;
                }
            }
            return bHoursOk && bDayOfWeek;
        }

        DateTime odtMin; 
        DateTime odtMax;

        private void UpdateGraph()
        {
            if (si.Speeds.Count <= 0)
                return;

            DateTime dtMin = si.Speeds.Min(x => x.When);
            DateTime dtMax = si.Speeds.Max(x => x.When);
            if (odtMin != dtMin || odtMax != dtMax)
            {
                odtMax = dtMax;
                odtMin = dtMin;
                comboDates.Items.Clear();
                comboDates.Items.Add("All");
                DateTime dtDates = DateTime.Parse(dtMin.ToString("yyyy-MM-dd"));
                while (dtMax >= dtDates)
                {
                    if (si.Speeds.Any(x => (x.When.Date == dtDates.Date)))
                    {
                        comboDates.Items.Add(dtDates.ToString("yyyy-MM-dd"));
                    }
                    dtDates = dtDates.AddDays(1);
                }
            }
            chart1.Series.Clear();
            var uniquespeeds=new List<string>();
            if (comboDates.SelectedIndex > 0)
            {
                uniquespeeds = GetUnique(Where(si.Speeds, "When", comboDates.SelectedItem.ToString()).ToList<Speed>(), comboBox2.Text).ToList<string>();
            }
            else
            {
                uniquespeeds = GetUnique(si.Speeds, comboBox2.Text).ToList<string>();
            }
            foreach (string str in uniquespeeds)
            {
                Series ser = chart1.Series.Add(str);
                IEnumerable<Speed> speeds = Where(si.Speeds, comboBox2.Text, str);
                IEnumerable<Speed> sdown = Where(speeds, "Direction", "Down");
                IEnumerable<Speed> sup = Where(speeds, "Direction", "Up");
                if (comboDates.SelectedIndex > 0)
                {
                    sdown = Where(sdown, "When", comboDates.SelectedItem.ToString());
                    sup = Where(sup, "When", comboDates.SelectedItem.ToString());
                }
                double d = 0;
                double u = 0;
                if (cbAverages.Checked)
                {
                    d = sdown != null && sdown.Count() > 0 ? sdown.Average(x => x.Speed8) : 0;
                    u = sup != null && sup.Count() > 0 ? sup.Average(x => x.Speed8) : 0;
                }
                else if (cbMax.Checked)
                {
                    d = sdown != null && sdown.Count() > 0 ? sdown.Max(x => x.Speed8) : 0;
                    u = sup != null && sup.Count() > 0 ? sup.Max(x => x.Speed8) : 0;
                }
                else if (cbMin.Checked)
                {
                    d = sdown != null && sdown.Count() > 0 ? sdown.Min(x => x.Speed8) : 0;
                    u = sup != null && sup.Count() > 0 ? sup.Min(x => x.Speed8) : 0;
                }

                ser.Points.AddXY("Down", d);
                ser.Points.AddXY("Up", u);

                if (!cbAverages.Checked && !cbMax.Checked && !cbMin.Checked)
                {
                    foreach (Speed sp in speeds)
                    {
                        ser.Points.AddXY(sp.When.ToString("yyyy-MM-dd HH:mm"), sp.Speed8);
                    }
                }
                chart1.ChartAreas[0].RecalculateAxesScale();

            }
        }

        private IEnumerable<Speed> Where(IEnumerable<Speed> list, string p, string v)
        {
            List<Speed> l = new List<Speed>();
            switch (p)
            {
                case "When":
                    return list.Where(x => x.When.Date.ToString("yyyy-MM-dd").Equals(v)).OrderBy(x => x.Diff);
                case "Diff":
                    return list.Where(x => x.Diff.ToString().Equals(v)).OrderBy(x=>x.Diff);
                case "Bytes":
                    return list.Where(x => x.Bytes.ToString().Equals(v)).OrderBy(x => x.Bytes);
                case "Isp":
                    return list.Where(x => x.Isp.ToString().Equals(v)).OrderBy(x => x.Isp);
                case "Server":
                    return list.Where(x => x.Server.ToString().Equals(v)).OrderBy(x => x.Server);
                case "Speed1":
                    return list.Where(x => x.Speed1.ToString().Equals(v)).OrderBy(x => x.Speed1);
                case "Speed8":
                    return list.Where(x => x.Speed8.ToString().Equals(v)).OrderBy(x => x.Speed8);
                case "Direction":
                    return list.Where(x => x.Direction.ToString().Equals(v)).OrderBy(x => x.Direction);
            }
            return l;
        }
        private IEnumerable<Speed> Where(List<Speed> list, string p,string v)
        {
            return Where(list.Where(x=>x.GetType()!=null),p,v);
        }

        private IEnumerable<string> GetUnique(List<Speed> list,string prop)
        {
            List<string> rlist = new List<string>();
            foreach(Speed sp in list)
            {
                PropertyInfo pi = sp.GetType().GetProperty(prop);
                if(pi!=null)
                {
                    object o = pi.GetValue(sp, null);
                if(!rlist.Contains(o.ToString()))
                    rlist.Add(o.ToString());
                }
            }
            return rlist;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadSpeeds();

            si.SpeedAdded += (a, b) =>
            {
                if (Loaded)
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = si.Speeds;
                    dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
                    //dataGridView1.AutoResizeColumns();
                    dataGridView1.ResumeLayout();
                    dataGridView1.Refresh();

                    UpdateGraph();
                    UpdateMarkers();
                }
            };

            checkedListBox1.Items.Add(DayOfWeek.Monday);
            checkedListBox1.Items.Add(DayOfWeek.Tuesday);
            checkedListBox1.Items.Add(DayOfWeek.Wednesday);
            checkedListBox1.Items.Add(DayOfWeek.Thursday);
            checkedListBox1.Items.Add(DayOfWeek.Friday);
            checkedListBox1.Items.Add(DayOfWeek.Saturday);
            checkedListBox1.Items.Add(DayOfWeek.Sunday);
            Loaded = true;

            UpdateGraph();
            UpdateMarkers();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSpeeds();
        }

        private void LoadSpeeds()
        {
            string path = Application.StartupPath;
            string file = path + "\\speeds.xml";
            if (File.Exists(file))
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(this.si.GetType());
                FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                try
                {
                    si = xs.Deserialize(fs) as SpeedInfo;
                }catch{}
                fs.Close();
            }
            /*
            if(File.Exists(file))
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(file);

                foreach (XmlNode xn in xd.DocumentElement.SelectNodes("//speedinfo/speeds/speed"))
                { 
                    Speed sp = new Speed();
                    foreach (PropertyInfo pi in sp.GetType().GetProperties().Where(x => x.CanWrite))
                    {
                        var el = xn.Attributes[pi.Name];
                        if (el != null)
                        {
                            object val = el.Value;
                            if (pi.PropertyType == typeof(DateTime))
                            {
                                val = DateTime.Parse(val.ToString());
                            }
                            else if (pi.PropertyType == typeof(double))
                            {
                                val = double.Parse(val.ToString());
                            }
                            else if (pi.PropertyType == typeof(int))
                            {
                                val = int.Parse(val.ToString());
                            }
                            else if (pi.PropertyType == typeof(Int32))
                            {
                                val = Int32.Parse(val.ToString());
                            }
                            else if (pi.PropertyType == typeof(Int64))
                            {
                                val = Int64.Parse(val.ToString());
                            }
                            else if (pi.PropertyType == typeof(Direction))
                            {
                                val = val.Equals("Down") ? Direction.Down : val.Equals("Up") ? Direction.Up : Direction.UnKnown;
                            }
                            else if (pi.PropertyType == typeof(string))
                            {
                                val = val.ToString();
                            }
                            pi.SetValue(sp, val, null);
                        }
                    }
                    si.Add(sp);
                }
            }
            */
        }

        private void SaveSpeeds()
        {
            string path = Application.StartupPath;
            string file = path + "\\speeds.xml";
            try
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(this.si.GetType());
                FileStream fs = new FileStream(file, FileMode.Create, FileAccess.ReadWrite);
                xs.Serialize(fs, this.si);
                fs.Close();
            }
            catch 
            {
            
            }
            /*
            string strxml = "<speedinfo><speeds>";
            foreach (Speed sp in si.Speeds)
            {
                
                strxml+="<speed";
                foreach (PropertyInfo pi in sp.GetType().GetProperties().Where(x => x.CanWrite))
                {                    
                    object val = pi.GetValue(sp,null);
                    
                    if(val!=null)
                        strxml += " " + pi.Name + "='" + val.ToString() + "'";

                }
                strxml+="/>";
            }
            strxml+="</speeds></speedinfo>";
            File.Delete(file);
            File.AppendAllText(file, strxml);
            */
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            si.Speeds.Clear();
        }

        private void cbAverages_Click(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                cbMax.Checked = !(sender as CheckBox).Checked;
                cbMin.Checked = !(sender as CheckBox).Checked;
            }
            UpdateGraph();
        }

        private void cbMax_Click(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                cbAverages.Checked = !(sender as CheckBox).Checked;
                cbMin.Checked = !(sender as CheckBox).Checked;
            }
            UpdateGraph();
        }

        private void cbMin_Click(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                cbAverages.Checked = !(sender as CheckBox).Checked;
                cbMax.Checked = !(sender as CheckBox).Checked;
            }
            UpdateGraph();
        }


        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                int icnt = dataGridView1.SelectedRows.Count;
                if (icnt > 0)
                {
                    int istart = dataGridView1.Rows.Count;
                    foreach (DataGridViewRow dgvr in dataGridView1.SelectedRows)
                    {
                        istart = Math.Min(istart, dgvr.Index);
                    }
                    foreach (DataGridViewRow dgvr in dataGridView1.Rows)
                    {
                        dgvr.Selected = false;
                    }
                    si.Speeds.RemoveRange(istart, icnt);
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = si.Speeds;
                }
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void cBAlways_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = !cBAlways.Checked;
        }

        private void vSbTo_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type == ScrollEventType.SmallIncrement || e.Type == ScrollEventType.SmallDecrement)
            {
                int val = int.Parse(vSbTo.Tag.ToString());
                val = e.Type == ScrollEventType.SmallIncrement ? val + 1 : e.Type == ScrollEventType.SmallDecrement ? val - 1 : val;
                if (val > vSbTo.Minimum && val < vSbTo.Maximum)
                {
                    vSbTo.Tag = val;
                    double h = double.Parse((val * -1).ToString()) / 60;
                    double hl = h.Left();
                    double hr = h.Right();
                    double m = ((h - hl).Round(2) * 60).Round();
                    mTbTo.Text = hl.ToString().PadLeft(2, '0') + ":" + m.ToString().PadLeft(2, '0');
                }
            }
        }

        private void vSbFrom_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type == ScrollEventType.SmallIncrement || e.Type == ScrollEventType.SmallDecrement)
            {
                int val = int.Parse(vSbFrom.Tag.ToString());
                val = e.Type == ScrollEventType.SmallIncrement ? val + 1 : e.Type == ScrollEventType.SmallDecrement ? val - 1 : val;
                if (val > vSbFrom.Minimum && val < vSbFrom.Maximum)
                {
                    vSbFrom.Tag = val;
                    double h = double.Parse((val * -1).ToString()) / 60;
                    double hl = h.Left();
                    double hr = h.Right();
                    double m = ((h - hl).Round(2) * 60).Round();
                    mTbFrom.Text = hl.ToString().PadLeft(2, '0') + ":" + m.ToString().PadLeft(2, '0');
                }
            }
        }

        private void vSbInterval_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type == ScrollEventType.SmallIncrement || e.Type == ScrollEventType.SmallDecrement)
            {
                int val = int.Parse(vSbInterval.Tag.ToString());
                val = e.Type == ScrollEventType.SmallIncrement ? val+1 : e.Type == ScrollEventType.SmallDecrement ? val-1 : val;
                if (val > vSbInterval.Minimum && val < vSbInterval.Maximum)
                {
                    vSbInterval.Tag = val;

                    double h = double.Parse((val * -1).ToString()) / 60;
                    double hl = h.Left();
                    double hr = h.Right();
                    double m = ((h - hl).Round(2) * 60).Round();
                    mTbInterval.Text = hl.ToString().PadLeft(2, '0') + ":" + m.ToString().PadLeft(2, '0');

                    timer1.Interval = 1000 * (hl.Int() + m.Int()) * 60;
                }
            }
        }

        private void mTbInterval_Validated(object sender, EventArgs e)
        {
            string[] s = mTbInterval.Text.Split(':');
            int h = int.Parse(s[0]) * 60;
            int m = int.Parse(s[1]);
            vSbInterval.Value = (h + m) * -1;
            timer1.Interval = 1000 * (h + m) * 60;
        }

        private void mTbFrom_Validated(object sender, EventArgs e)
        {
            string[] s = mTbFrom.Text.Split(':');
            int h = int.Parse(s[0]) * 60;
            int m = int.Parse(s[1]);
            vSbFrom.Value = (h + m) * -1;
        }

        private void mTbTo_Validated(object sender, EventArgs e)
        {
            string[] s = mTbTo.Text.Split(':');
            int h = int.Parse(s[0]) * 60;
            int m = int.Parse(s[1]);
            vSbTo.Value = (h + m) * -1;
        }

        private void mTbTo_KeyUp(object sender, KeyEventArgs e)
        {
            mTbTo_Validated(sender, EventArgs.Empty);
        }

        private void mTbFrom_KeyUp(object sender, KeyEventArgs e)
        {
            mTbFrom_Validated(sender, EventArgs.Empty);
        }

        private void mTbInterval_KeyUp(object sender, KeyEventArgs e)
        {
            mTbInterval_Validated(sender, EventArgs.Empty);
        }

        private void cBSettings_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedItem != null)
            {
                SettingServer ss = (sender as ComboBox).SelectedItem as SettingServer;
                if (ss != null)
                {
                    reloadServers(ss);
                }
            }
        }
        private void reloadServers(SettingServer ss)
        {

            try
            {
                if (ss != null)
                {
                    settingserver = ss;
                    bks.reload(settingserver);
                    comboBox1.DataSource = bks.ClosestServers.Take(50).ToList();
                    comboBox1.DisplayMember = "Name";
                }
            }
            catch { }
        }
        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            refreshMap();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            UpdateGraph();
            UpdateMarkers();
        }

        private void comboDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateGraph();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateGraph();
        }


    }
}
