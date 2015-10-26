using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading;
using MySeDes;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MyWeatherCams
{
    public partial class Form1 : Form
    {
        string strDownloadPath = Application.UserAppDataPath;//+@"\debug\";
        public Form1()
        {
            InitializeComponent();
        }
        List<webcam> list = new List<webcam>();
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            button1.BackColor = timer1.Enabled ? Color.DarkGreen : Color.FromKnownColor(KnownColor.Control);
            button1.ForeColor = timer1.Enabled ? Color.White : Color.Black;

            if (timer1.Enabled)
                timer1_Tick(sender, e);
        }


        private void DownTrCams()
        {
            //http://trafikinfo.trafikverket.se/lit/orion/orionproxy.ashx
            string url = "http://trafikinfo.trafikverket.se/lit/orion/orionproxy.ashx";
            string data = "<ORIONML version='1.0'><REQUEST plugin='CameraInfo' version='' locale='SE_sv' authenticationkey='7fd72d2a-4746-482c-b856-15a64f85a205'><PLUGINML  table=\"Cameras\" filter=\"TypeValue='RoadConditionCamera' or TypeValue = 'atk' or TypeValue = 'TrafficCamera'\"  /></REQUEST></ORIONML>";
            
            HttpWebResponse resp;
            string s = MyWebClient.DownloadString("http://trafikinfo.trafikverket.se/lit/", out resp);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.ContentType = "text/xml";
            request.Referer = "http://trafikinfo.trafikverket.se/LIT/";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.KeepAlive = true;
            string json = MyWebClient.UploadString(request, data, resp.Cookies);
            //string json = wc.UploadString(url, data).Replace("\\ufeff", "");
            dynamic obj = JObject.Parse(json);
            int icnt = 0;
            List<webcam> wc = new List<webcam>();
            foreach (var ev in obj.Cameras.Camera)
            {
                string desc = ev.Description;
                string x = ev.X;
                string id = ev.Id;
                string name = ev.Name;
                string y = ev.Y;
                string pu = ev.PhotoUrl;
                string ty = ev.Type;
                string st = ev.PhotoTime;
                DateTime pt = string.IsNullOrEmpty(st) ? DateTime.MinValue : DateTime.Parse(st);
                wc.Add(new webcam() { id = id, lat = x, lng = y, name = name, url = pu, LastModified = pt });
            }
            foreach (var w in wc)
            {
                var ww = ConvertXYZToLatLngAlt(w);
                /*
                if (ww.name.Equals("Motorvägsbron S"))
                {
                    string sasd = "asldkjfl";
                    if (sasd != null)
                    { }
                }
                */
                if (!list.Any(c => c.id.Equals(ww.id)))
                    list.Add(ww);
            }

        }
        private webcam ConvertXYZToLatLngAlt(webcam wc)
        {
            webcam wcout = new webcam() { id = wc.id, lat = wc.lat, lng = wc.lng, LastModified = wc.LastModified, name = wc.name, url = wc.url };
            string lat = "";
            string lng = "";
            double dlat = double.Parse(wc.lat.Replace(".",","));
            double dlng = double.Parse(wc.lng.Replace(".", ","));
            GeoUTMConverter conv = new GeoUTMConverter();
            conv.ToLatLon(dlat,dlng,33,GeoUTMConverter.Hemisphere.Northern);
            wcout.lat = Math.Round(conv.Latitude, 6).ToString().Replace(",", ".");
            wcout.lng = Math.Round(conv.Longitude, 6).ToString().Replace(",", ".");

            return wcout;
        }

        private void downloadCams()
        {
            if (list.Count > 0) {
                foreach (webcam r in list)
                {
                    r.lat = r.lat.Replace(",", ".");
                    r.lng = r.lng.Replace(",", ".");
                    ThreadPool.QueueUserWorkItem(downIfChanged, r);
                }
            }
            else
            {
                //string url = "http://webcam-api-eu.herokuapp.com/webcams/get_webcams_boundary?callback=jQuery164034308020158312846_1367187674405&limit=300&swpointy=58.71837604123238&swpointx=16.681749389062475&nepointy=59.64698212185716&nepointx=18.604356810937475&_=1367187675105";
                string url = "http://webcam-api-eu.herokuapp.com/webcams/get_webcams_boundary?callback=jQuery164034308020158312846_1367187674405&limit=1000&swpointy=55&swpointx=11&nepointy=69&nepointx=25&_=1367187675105";
                WebClient wc = new WebClient();
                try
                {
                    string json = wc.DownloadString(url).Replace("jQuery164034308020158312846_1367187674405(", "").Replace("\\ufeff","");
                    json = json.Substring(0, json.Length - 1);
                    dynamic obj = JObject.Parse(json);
                    int icnt = 0;
                    foreach (var ev in obj.webcams)
                    {
                        if (("" + ev.image_url).Contains("stp2.gofore.com"))
                            continue;
                        
                        var r = new webcam { id = ev.id, url = ev.image_url, lat = ev.lat, lng = ev.lng, name = ev.name };
                        r.url = r.url.Replace("http://proxy.vackertvader.se/redirect?url=", "");
                        ThreadPool.QueueUserWorkItem(downIfChanged, r);
                        list.Add(r);
                        icnt++;
                        if (icnt > 1000)
                            break;
                    }
                }
                catch { }
            }

            dataGridView1.DataSource = list.ToList();
            labelCnt.Text = list.Count().ToString();
        }

        private void downIfChanged(object state)
        {
            webcam r = state as webcam;
            using (WebClient wc = new WebClient())
            {
                try
                {

                    string strfolder = strDownloadPath + "\\webcams\\" + r.id + "\\" + DateTime.Today.ToString("yyyyMMdd") + "\\";
                    if (!Directory.Exists(strfolder))
                        Directory.CreateDirectory(strfolder);
                    string strFileName = "tmp.jpg";
                    string strFilePath = strfolder + strFileName;

                    if (File.Exists(strFilePath))
                    {
                        File.Move(strFilePath, strFilePath.Replace(strFileName, (new FileInfo(strFilePath)).LastWriteTime.ToString("yyyyMMdd_HHmmss") + ".jpg"));
                    }

                    wc.DownloadFile(r.url, strFilePath);
                    Thread.Sleep(1000);
                    string lastmodified = wc.ResponseHeaders.AllKeys.Contains("Last-Modified") ? wc.ResponseHeaders["Last-Modified"] : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime LastModified = DateTime.Parse(lastmodified);
                    string destFileName = strFilePath.Replace(strFileName, LastModified.ToString("yyyyMMdd_HHmmss") + ".jpg");
                    if (File.Exists(strFilePath) && !File.Exists(destFileName))
                    {
                        File.Move(strFilePath, destFileName);
                    }
                    if (File.Exists(strFilePath))
                        File.Delete(strFilePath);

                    r.setFile(new FileInfo(destFileName));
					r.cnt = Directory.EnumerateFiles(strfolder).Count();
                    r.LastModified = LastModified;
                }
                catch(Exception ex) {
                    string s = ex.Message;
                }
            }               
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(strDownloadPath + "\\list.xml"))
                return;
            string xml = File.ReadAllText(strDownloadPath + "\\list.xml");
            list = (SeDes.ToObj(xml, list) as List<webcam>);
            DownTrCams();
            foreach (webcam r in list)
            {
                r.lat = r.lat.Replace(",", ".");
                r.lng = r.lng.Replace(",", ".");
            }
            dataGridView1.DataSource = list.ToList();
            labelCnt.Text = list.Count().ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            downloadCams();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start(strDownloadPath);
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
                textBox1.Text = Regex.Replace(textBox1.Text, "\\D", "");
            if (!string.IsNullOrEmpty(textBox1.Text))
                timer1.Interval = int.Parse(textBox1.Text) * 1000;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string xml = SeDes.ToXml(list);
            string tempfile = strDownloadPath + "\\list_"+ DateTime.Now.Ticks.ToString() +".xml";
            string origfile = strDownloadPath + "\\list.xml";
            File.WriteAllText(tempfile, xml);
            if (File.Exists(origfile))
                File.Delete(origfile);
            File.Move(tempfile, origfile);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
        }

        PictureForm pf;
        bool bMapOpen = false;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0)
                return;
            var img = dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value;
            if (bMapOpen)
            {
                if (map != null)
                {
                    webcam wc = list.FirstOrDefault(x => x.id.Equals(img));
                    if (wc != null)
                        map.panTo(wc);
                }
            }
            else
            {
                ShowPic(img);
                //ThreadPool.QueueUserWorkItem(ShowPic, img);
            }
        }
        private void ShowPic(object state)
        {
            var img = state.ToString();
            this.UseWaitCursor = true;
            try
            {
                var files = System.IO.Directory.EnumerateFiles(strDownloadPath + "\\webcams\\" + img + "\\" + DateTime.Today.ToString("yyyyMMdd") + "\\").Select(x => new FileInfo(x)).Where(file => file.Extension.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase) && file.Length > 0).OrderByDescending(x => x.LastWriteTime).Take(300).OrderBy(x => x.LastWriteTime).ToArray();
                if (pf != null)
                    pf.Close();
                pf = new PictureForm(files);
                pf.Show();
                pf.Left = this.Right - 50;
                pf.Top = this.Top;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            finally
            {
                this.UseWaitCursor = false;
            }

        }
        Map map;
        private void btnMap_Click(object sender, EventArgs e)
        {
            if (bMapOpen)
                return;
            map = new Map(list);
            map.scrollTo += (a, b) =>
            {
                var id = b.id;
                var lat = b.lat;
                var lng = b.lng;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value.ToString().Equals(id))
                    {
                        row.Selected = true;
                        dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;

                        break;
                    }
                }
            };
            map.FormClosed += (a, b) => { bMapOpen = false; };
            map.Show();
            bMapOpen = true;
        }

        private void btnPicture_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0)
                return;
            var img = dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value;
            ShowPic(img);
        }
    }
    public class webcam
    {
        public webcam()
        { }
        public string id { get; set; }
        public string url { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string name { get; set; }
		public int cnt { get; set; }
        private FileInfo file { get;set;}
        public DateTime LastModified { get { return (file != null) ? file.LastWriteTime : DateTime.MinValue; } set { if (file != null) { file.LastWriteTime = value; } } }
        public void setFile(FileInfo fi)
        {
            file = fi;
        }
    }
}
