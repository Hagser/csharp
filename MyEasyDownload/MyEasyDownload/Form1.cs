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
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections.Specialized;
using System.Diagnostics;

namespace MyEasyDownload
{
    public partial class Form1 : Form
    {
        DateTime dtLastProgressPart = DateTime.Now;
        long downProgressPart = 0;
        long lastBytesPart = 0;
        public Form1()
        {
            InitializeComponent();

        }

        private string getFriendlySize(double vsum)
        {
            string strRet = "";

            if (vsum >= 1000000000)
            {
                strRet = Math.Round((vsum / 1000000000), 2).ToString() + " Gb";
            }
            else if (vsum >= 1000000)
            {
                strRet = Math.Round((vsum / 1000000), 2).ToString() + " Mb";
            }
            else if (vsum >= 1000)
            {
                strRet = Math.Round((vsum / 1000), 2).ToString() + " kb";
            }
            else
            {
                strRet = vsum.ToString() + " b";
            }

            return strRet;
        }
        int icntAll { get { return downs != null ? downs.Count : 0; } set { } }
        int icntDone = 0;
        #region oldbtn
        /*

        private void button1_Click(object sender, EventArgs e)
        {

            if (button1.Text.Equals("Down") && strUrl != null && strUrl.StartsWith("http"))
            {
                myProgressBar1.Visible = true;
                myProgressBar1.Progresses = new Dictionary<string, int>();
                bCancel = false;
                while (chart1.Series.Count > 1)
                {
                    chart1.Series.RemoveAt(1);
                }
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                TaskbarManager.Instance.SetProgressValue(0, 100);
                icntDone = 0;
                toolStripFileName.Text = "";
                toolStripTextBox1.Text = "";
                toolStripTextBox1.Tag = false;
                foreach (string str in strUrl.Split(';'))
                {
                    if (string.IsNullOrEmpty(str) || str.Length < 5)
                        continue;
                    icntAll++;
                    DateTime dtLastProgress = DateTime.Now;
                    long downProgress = 0;
                    long lastBytes = 0;
                    WebClient wc = new WebClient();
                    wc.Proxy = new WebProxy("http://localhost:8080", true, "http://*.*.*".Split(','));

                    List<long> lavg = new List<long>();
                    wc.DownloadFileCompleted += (a, b) =>
                    {
                        icntDone++;
                        if (b.Error == null)
                        {
                            Uri uric = b.UserState as Uri;

                            string filename = uric.Segments[uric.Segments.Length - 1];
                            //using (SaveFileDialog sfd = new SaveFileDialog())
                            {
                                //sfd.FileName = filename1;
                                //if (DialogResult.OK == sfd.ShowDialog(this))
                                {

                                    bool changed = false;

                                    bool.TryParse(toolStripTextBox1.Tag!=null?toolStripTextBox1.Tag.ToString():"false",out changed);

                                    if(icntAll==1 && !toolStripTextBox1.Text.Equals(filename) && !changed)
                                    {
                                        toolStripFileName.Text = filename;
                                        toolStripTextBox1.Text = filename;
                                    }
                                    else if(icntAll==1 && changed)
                                    {
                                        filename = toolStripTextBox1.Text;
                                    }

                                    string filename1 = FolderPath + "\\" + filename;

                                    if(!File.Exists(filename1))
                                        File.Move(Application.CommonAppDataPath + "\\tmp" + getB64Url(uric) + ".tmp", filename1);
                                }
                                strUrl = strUrl.Replace(";" + uric.ToString(), "").Replace(uric.ToString() + ";", "");
                            }
                        }
                        else
                        {
                            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
                            TaskbarManager.Instance.SetProgressValue(100, 100);
                            this.Text = b.Error.Message;
                        }
                        if (icntAll <= icntDone)
                        {
                            myProgressBar1.Visible = false;
                            button1.Text = "Down";
                            this.Text = "Done";
                            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                            TaskbarManager.Instance.SetProgressValue(100, 100);
                        }
                    };
                    wc.DownloadProgressChanged += (a, b) =>
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            if (bCancel)
                            {
                                myProgressBar1.Visible = false;
                                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                                TaskbarManager.Instance.SetProgressValue(100, 100);
                                wc.CancelAsync();
                                button1.Text = "Down";
                            }
                            Uri urip = b.UserState as Uri;

                            
                            string filename = urip.Segments[urip.Segments.Length - 1];

                            bool changed = false;

                            bool.TryParse(toolStripTextBox1.Tag != null ? toolStripTextBox1.Tag.ToString() : "false", out changed);

                            if (icntAll == 1 && !toolStripTextBox1.Text.Equals(filename) && !changed)
                            {
                                toolStripFileName.Text = filename;
                                toolStripTextBox1.Text = filename;
                            }



                            myProgressBar1.ChangeValue(filename,b.ProgressPercentage);

                            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - dtLastProgress.Ticks);
                            double tms = ts.TotalSeconds;
                            downProgress += b.BytesReceived - lastBytes;
                            lastBytes = b.BytesReceived;
                            if (lavg.Count > 20)
                                lavg.RemoveAt(0);
                            lavg.Add(downProgress);
                            double davg = lavg.Count < 4 ? (lavg.Sum() / lavg.Count) : ((lavg.Sum() - (lavg.Max() + lavg.Min())) / (lavg.Count - 2));
                            double timeRem = Math.Floor((b.TotalBytesToReceive - lastBytes) / davg);
                            //progress.Add(new DownProgress() { percent = b.ProgressPercentage, avg = davg, secrem = timeRem });
                            double exactPerc = Math.Round((double.Parse(b.BytesReceived.ToString()) / double.Parse(b.TotalBytesToReceive.ToString())) * 100, 2);
                            if (tms >= 1)
                            {
                                int irem = int.Parse(timeRem.ToString());
                                DateTime dtrem = new DateTime(new TimeSpan(0, 0, irem).Ticks);
                                string strRem = dtrem.ToString("HH:mm:ss");

                                chart1.Series[filename.Replace(".", " ")].Points.AddXY(exactPerc, davg);
                                this.Text = string.Format("{0}/s {1}/{2}-{3}%, {4}", getFriendlySize(davg), getFriendlySize(lastBytes), getFriendlySize(b.TotalBytesToReceive), b.ProgressPercentage, strRem);
                                downProgress = 0;
                                dtLastProgress = DateTime.Now;
                            }
                            try
                            {
                                int tp = myProgressBar1.TotalProgress;
                                TaskbarManager.Instance.SetProgressValue(tp, 100);
                            }
                            catch { }
                        });
                    };
                    Uri uri = new Uri(str.Replace("\r\n",""));

                    if (checkBox1.Checked)
                        DownloadParts(uri);
                    else
                    {
                        string filenameser = uri.Segments[uri.Segments.Length - 1];
                        myProgressBar1.Progresses.Add(filenameser, 0);
                        Series ser = chart1.Series.Add(filenameser.Replace(".", " "));
                        ser.ChartType = SeriesChartType.FastLine;
                        //TopGearStuff****
                        // http://2fc85e898c366a47f229f5fce4f32400.wvedge0.00607-wvedge0.dna.qbrick.com/00607-down0/20140214/20140214124720845-cdwa1ed4e7hv2madv9du0zjby-973_all.wvm
                        wc.Headers.Add("Referer", "http://www.kanal9play.se/play/program/32626741/video/539660306");
                        wc.Headers.Add("Cookie", "session=29f47777-9d74-4276-a343-5653afa11fd8; FirefoxDisabledCheck=; s_fid=610CE8B75D3F905C-1A8A40A70CD699FE");
                        //****
                        wc.DownloadFileAsync(uri, Application.CommonAppDataPath + "\\tmp" + getB64Url(uri) + ".tmp", uri);
                    }
                    Thread.Sleep(100);
                }
                button1.Text = "Cancel";
            }
            else if (button1.Text.Equals("Cancel"))
            {
                bCancel = true;
                myProgressBar1.Visible = false;
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                TaskbarManager.Instance.SetProgressValue(100, 100);
                button1.Text = "Down";
            } 
        }         
         */
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {

            if (button1.Text.Equals("Down") && strUrl != null && strUrl.StartsWith("http"))
            {
                myProgressBar1.Visible = true;
                myProgressBar1.Progresses = new Dictionary<string, int>();
                bCancel = false;
                while (chart1.Series.Count > 1)
                {
                    chart1.Series.RemoveAt(1);
                }
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                TaskbarManager.Instance.SetProgressValue(0, 100);
                icntDone = 0;
                toolStripFileName.Text = "";
                toolStripTextBox1.Text = "";
                toolStripTextBox1.Tag = false;
                foreach (WebClient wc in downs)
                {
                    string str = wc.BaseAddress;
                    if (string.IsNullOrEmpty(str) || str.Length < 5)
                        continue;

                    Uri uri = new Uri(str.Replace("\r\n",""));

                    string filenameser = uri.Segments[uri.Segments.Length - 2]+uri.Segments[uri.Segments.Length - 1]+uri.Query;
                    myProgressBar1.Progresses.Add(filenameser, 0);
                    Series ser = chart1.Series.Add(filenameser.Replace(".", " "));
                    ser.ChartType = SeriesChartType.FastLine;

                    wc.DownloadFileAsync(uri, Application.CommonAppDataPath + "\\tmp" + getB64Url(uri) + ".tmp", uri);
                    
                    Thread.Sleep(100);
                }
                button1.Text = "Cancel";
            }
            else if (button1.Text.Equals("Cancel"))
            {
                bCancel = true;
                myProgressBar1.Visible = false;
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                TaskbarManager.Instance.SetProgressValue(100, 100);
                button1.Text = "Down";
            } 
        }
        List<WebClient> downs = new List<WebClient>();
        private WebClient CreateWebClient(string url)
        {
            return CreateWebClient(url, new NameValueCollection());
        }
        private WebClient CreateWebClient(string url,NameValueCollection nvc)
        {

            WebClient wc = new WebClient();
            wc.BaseAddress = url;
            foreach(var k in nvc.AllKeys)
                wc.Headers.Add(k, nvc[k]);

            if (string.IsNullOrEmpty(url) || url.Length < 5)
                return wc;
            DateTime dtLastProgress = DateTime.Now;
            long downProgress = 0;
            long lastBytes = 0;

            wc.Proxy = new WebProxy("http://localhost:8080", true, "http://*.*.*".Split(','));

            List<long> lavg = new List<long>();
            wc.DownloadFileCompleted += (a, b) =>
            {
                icntDone++;
                if (b.Error == null)
                {
                    Uri uric = b.UserState as Uri;

                    string filename = uric.Segments[uric.Segments.Length - 1];
                    //using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        //sfd.FileName = filename1;
                        //if (DialogResult.OK == sfd.ShowDialog(this))
                        {

                            bool changed = false;

                            bool.TryParse(toolStripTextBox1.Tag != null ? toolStripTextBox1.Tag.ToString() : "false", out changed);

                            if (icntAll == 1 && !toolStripTextBox1.Text.Equals(filename) && !changed)
                            {
                                toolStripFileName.Text = filename;
                                toolStripTextBox1.Text = filename;
                            }
                            else if (icntAll == 1 && changed)
                            {
                                filename = toolStripTextBox1.Text;
                            }

                            string filename1 = FolderPath + "\\" + filename;

                            if (!File.Exists(filename1))
                                File.Move(Application.CommonAppDataPath + "\\tmp" + getB64Url(uric) + ".tmp", filename1);
                        }
                        strUrl = strUrl.Replace(";" + uric.ToString(), "").Replace(uric.ToString() + ";", "");
                        downs.Remove(wc);
                    }
                }
                else
                {
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
                    TaskbarManager.Instance.SetProgressValue(100, 100);
                    this.Text = b.Error.Message;
                }
                if (myProgressBar1.TotalProgress==100)
                {
                    myProgressBar1.Visible = false;
                    button1.Text = "Down";
                    this.Text = "Done";
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                    TaskbarManager.Instance.SetProgressValue(100, 100);
                }
            };
            wc.DownloadProgressChanged += (a, b) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Uri urip = b.UserState as Uri;
                    string filename = urip.Segments[urip.Segments.Length - 2] + urip.Segments[urip.Segments.Length - 1] + urip.Query;
                    if (bCancel || b.TotalBytesToReceive<0)
                    {
                        myProgressBar1.Progresses.Remove(filename);
                        myProgressBar1.Visible = false;
                        TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                        TaskbarManager.Instance.SetProgressValue(100, 100);
                        wc.CancelAsync();
                        downs.Remove(wc);
                        button1.Text = "Down";
                        return;
                    }



                    bool changed = false;

                    bool.TryParse(toolStripTextBox1.Tag != null ? toolStripTextBox1.Tag.ToString() : "false", out changed);

                    if (icntAll == 1 && !toolStripTextBox1.Text.Equals(filename) && !changed)
                    {
                        toolStripFileName.Text = filename;
                        toolStripTextBox1.Text = filename;
                    }

                    myProgressBar1.ChangeValue(filename, b.ProgressPercentage);

                    TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - dtLastProgress.Ticks);
                    double tms = ts.TotalSeconds;
                    downProgress += b.BytesReceived - lastBytes;
                    lastBytes = b.BytesReceived;
                    if (lavg.Count > 20)
                        lavg.RemoveAt(0);
                    lavg.Add(downProgress);
                    double davg = lavg.Count < 4 ? (lavg.Sum() / lavg.Count) : ((lavg.Sum() - (lavg.Max() + lavg.Min())) / (lavg.Count - 2));
                    double timeRem = Math.Floor((b.TotalBytesToReceive - lastBytes) / davg);
                    //progress.Add(new DownProgress() { percent = b.ProgressPercentage, avg = davg, secrem = timeRem });
                    double exactPerc = Math.Round((double.Parse(b.BytesReceived.ToString()) / double.Parse(b.TotalBytesToReceive.ToString())) * 100, 2);
                    if (tms >= 1)
                    {
                        int irem = int.Parse(timeRem.ToString());
                        DateTime dtrem = new DateTime(new TimeSpan(0, 0, irem).Ticks);
                        string strRem = dtrem.ToString("HH:mm:ss");

                        chart1.Series[filename.Replace(".", " ")].Points.AddXY(exactPerc, davg);
                        this.Text = string.Format("{0}/s {1}/{2}-{3}%, {4}", getFriendlySize(davg), getFriendlySize(lastBytes), getFriendlySize(b.TotalBytesToReceive), b.ProgressPercentage, strRem);
                        downProgress = 0;
                        dtLastProgress = DateTime.Now;
                    }
                    try
                    {
                        int tp = myProgressBar1.TotalProgress;
                        TaskbarManager.Instance.SetProgressValue(tp, 100);
                    }
                    catch { }
                });
            };
            return wc;
        }
        private string getB64Url(Uri uric)
        {
            //string su = System.Convert.ToBase64String(Encoding.Default.GetBytes(uric.ToString()));
            string su = System.Convert.ToBase64String(Encoding.Default.GetBytes(uric.Host.ToString() + "/" + uric.Segments[uric.Segments.Length - 1]));
            
            string sdf1 = string.Format("{0:X}", su.GetHashCode());
            string sdf2 = string.Format("{0:X8}", sdf1.GetHashCode());
            Console.WriteLine(sdf1);
            Console.WriteLine(sdf2);

            while (su.Contains("\\") || su.Contains("/"))
            {
                su = System.Convert.ToBase64String(Encoding.Default.GetBytes(su));
            }
            return su;
        }
        private Uri getUrlB64(string url)
        {
            Uri uri = new Uri("http://www.sr.se");
                string surl = Encoding.Default.GetString(System.Convert.FromBase64String(url));
                while (!surl.Contains("http://"))
                {
                    try
                    {
                        surl = Encoding.Default.GetString(System.Convert.FromBase64String(url));
                        if (!string.IsNullOrEmpty(surl))
                        {

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                uri = new Uri(surl);

            return uri;
        }
        Dictionary<int, DownPartClass> partsready = new Dictionary<int, DownPartClass>();
        bool bCancel = false;
        double parts = 18;
        private void DownloadParts(Uri uri)
        {

            myProgressBar1.Visible = true;
            myProgressBar1.Progresses = new Dictionary<string, int>();

            HttpWebRequest wr = WebRequest.Create(uri) as HttpWebRequest;
            wr.Method = "HEAD";

            wr.AddRange(0);
            WebResponse wrs = wr.GetResponse();

            string filename = wrs.ResponseUri.Segments.Last();
            long l = wrs.ContentLength;

            int part = int.Parse(Math.Floor(l / parts).ToString());
            partsready = new Dictionary<int, DownPartClass>();
            for (int i = 0; i < parts; i++)
            {
                myProgressBar1.Progresses.Add("part_"+i, 0);
                DownPartClass dpc = new DownPartClass { uri = uri, i = i, filename = filename, part = part };
                partsready.Add(i, dpc);
                ThreadPool.QueueUserWorkItem(DownPart, dpc);
            }
        }

        private void DownPart(object state)
        {
            DownPartClass dpc = state as DownPartClass;
            try
            {

                this.Invoke((MethodInvoker)delegate
                {
                    myProgressBar1.ChangeColor("part_" + dpc.i, Color.Green);
                });
                string file = Application.CommonAppDataPath + "\\" + dpc.filename + "_" + dpc.i;
                long istart = 0;
                if (File.Exists(file))
                    //File.Delete(file);
                    istart = (new FileInfo(file)).Length;

                if (istart < dpc.part)
                {
                    HttpWebRequest wrl = WebRequest.Create(dpc.uri) as HttpWebRequest;
                    wrl.Method = "GET";
                    wrl.AddRange((dpc.i * dpc.part) + istart, (dpc.i + 1) * dpc.part);
                    wrl.ReadWriteTimeout = int.Parse(Math.Floor((new TimeSpan(1, 0, 0)).TotalMilliseconds).ToString());
                    wrl.Timeout = wrl.ReadWriteTimeout;
                    WebResponse wrsl = wrl.GetResponse();

                    Stream s = wrsl.GetResponseStream();

                    using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
                    {
                        byte[] buffer = new byte[650000];
                        int icnt = 0;
                        while ((icnt = s.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fs.Write(buffer, 0, icnt);
                            partsready[dpc.i].progress += icnt;
                            if (bCancel)
                                break;
                        }
                    }
                }
                else
                {
                    partsready[dpc.i].progress = int.Parse(istart.ToString());
                }
                partsready[dpc.i].done = true;
            }
            catch(Exception ex) {
                dpc.retries++;
                partsready[dpc.i].retries = dpc.retries;
                partsready[dpc.i].error = ex;
                partsready[dpc.i].paused = true;

                this.Invoke((MethodInvoker)delegate
                {
                    myProgressBar1.ChangeColor("part_" + dpc.i, Color.Yellow);
                });
                if (dpc.retries >= 5)
                {
                    dpc.done = true;
                    this.Invoke((MethodInvoker)delegate
                    {
                        myProgressBar1.ChangeColor("part_" + dpc.i, Color.Red);
                    });
                }
            }
            if (dpc.part > dpc.progress && dpc.error==null && dpc.retries<5)
            {
                dpc.paused = true;
                dpc.done = false;
            }
            var pr = partsready.FirstOrDefault(x => x.Value.paused && !x.Value.done);
            if(pr.Value!=null)
                ThreadPool.QueueUserWorkItem(DownPart, pr.Value);

            if (!bCancel && partsready.All(x => x.Value.done))
            {
                AssembleFiles(dpc.filename);
            }
        }

        private void AssembleFiles(string filename)
        {
            partsready = new Dictionary<int, DownPartClass>();
            using (FileStream fs = new FileStream(Application.CommonAppDataPath + "\\" + filename, FileMode.OpenOrCreate))
            {
                for (int i = 0; i < partsready.Count; i++)
                {
                    using (FileStream fsp = new FileStream(Application.CommonAppDataPath + "\\" + filename + "_" + i, FileMode.OpenOrCreate))
                    {
                        byte[] buffer = new byte[6500000];
                        int icnt = 0;
                        while ((icnt = fsp.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fs.Write(buffer, 0, icnt);
                        }
                    }
                    //File.Delete(Application.CommonAppDataPath + "\\part_" + i);
                }
            }

            this.Invoke((MethodInvoker)delegate
            {
                myProgressBar1.Visible = false;
                button1.Text = "Down";
            });
            bCancel = true;

            try
            {
                File.Move(Application.CommonAppDataPath + "\\" + filename, FolderPath + "\\" + filename);
            }
            catch { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            bCancel = true;
        }
        List<string> oldurls = new List<string>();
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                string url = Clipboard.GetText();
                if (url.StartsWith("http"))
                {
                    foreach (string stru in url.Split(';'))
                        if (!oldurls.Contains(stru) && ((strUrl != null && !strUrl.Split(';').Contains(stru)) || strUrl == null) && stru.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                        {
                            oldurls.Add(stru);
                            strUrl += string.IsNullOrEmpty(strUrl) || (!string.IsNullOrEmpty(strUrl) && strUrl.EndsWith(";")) ? stru + ";" : ";" + stru + ";";
                            WebClient wc = CreateWebClient(stru);
                            downs.Add(wc);
                        }
                }
                else if(url.StartsWith("GET "))
                {
                    string getUrl = "";
                    int idx = 0;
                    NameValueCollection nvc = new NameValueCollection();
                    foreach (string sss in url.Split(new string[] { "------------------------------------------------------------------\r\n" }, StringSplitOptions.None))
                    foreach (string stru in sss.Split(new string[] { "\r\n" }, StringSplitOptions.None))
                    {
                        if (string.IsNullOrEmpty(stru))
                        {
                            if (!oldurls.Contains(getUrl))
                            {
                                oldurls.Add(getUrl);

                                WebClient wc = CreateWebClient(getUrl, nvc);
                                downs.Add(wc);
                                string strur = getUrl;
                                if (((strUrl != null && !strUrl.Split(';').Contains(strur)) || strUrl == null) && strur.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    strUrl += string.IsNullOrEmpty(strUrl) || (!string.IsNullOrEmpty(strUrl) && strUrl.EndsWith(";")) ? strur + ";" : ";" + strur + ";";
                                }

                            }
                            idx = 0;
                            break;
                        }

                        if (idx == 0)
                        {
                            idx++;
                            getUrl = stru.Replace("GET ", "").Replace(" HTTP/1.1", "");
                            continue;
                        }
                        var header = stru.Split(':');

                        switch (header[0])
                        {
                            case "Host": var hst = header[1].TrimStart(); getUrl = getUrl.StartsWith("http://")?getUrl: (hst.StartsWith("http://") ? "" : "http://") + hst + getUrl; break;
                            case "Cookie": nvc.Add(header[0].Trim(), header[1].TrimStart()); break;
                            case "User-Agent": nvc.Add(header[0].Trim(), header[1].TrimStart()); break;
                            case "Referer": nvc.Add(header[0].Trim(), header[1].TrimStart() + ":" + (header.Length>2?header[2]:"")); break;
                            //case "Connection": nvc.Add(header[0].Trim(), header[1].TrimStart()); break;

                            case "Accept-Encoding": nvc.Add(header[0].Trim(), header[1].TrimStart()); break;
                            case "Accept": nvc.Add(header[0].Trim(), header[1].TrimStart()); break;
                            case "Accept-Language": nvc.Add(header[0].Trim(), header[1].TrimStart()); break;
                                
                            default: break;
                        }
                        idx++;
                    }
                    string sdöflksödfl = "";
                    if (sdöflksödfl != "alsdaskd")
                    { 
                    
                    }
                }
            }
            if (partsready.Count > 0)
            {
                int progress = partsready.Values.Sum(x => x.progress)+1;
                double TotalBytesToReceive = partsready.Values.Sum(x => x.part)+1;
                int ProgressPercentage = int.Parse(Math.Floor((progress / TotalBytesToReceive) * 100d).ToString());

                try
                {
                    foreach (var dp in partsready.Values)
                    {

                        myProgressBar1.ChangeValue("part_" + dp.i, dp.percent);
                    }
                }
                catch { }

                TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - dtLastProgressPart.Ticks);
                double tms = ts.TotalSeconds;
                downProgressPart += progress - lastBytesPart;
                lastBytesPart = progress;
                if (tms >= 1)
                {
                    this.Text = string.Format("{0}/s {1}/{2}-{3}%", getFriendlySize(downProgressPart), getFriendlySize(lastBytesPart), getFriendlySize(TotalBytesToReceive), ProgressPercentage);
                    downProgressPart = 0;
                    dtLastProgressPart = DateTime.Now;
                }

                TaskbarManager.Instance.SetProgressValue(Math.Min(100, ProgressPercentage), 100);
            }
            dataGridView1.DataSource = downs.Select(x => new { Perc = GetPercent(x.BaseAddress), Url = x.BaseAddress }).ToList();
        }

        private int GetPercent(string p)
        {
            Uri urip = new Uri(p);
            string k = urip.Segments[urip.Segments.Length - 2] + urip.Segments[urip.Segments.Length - 1] + urip.Query;
            return myProgressBar1.Progresses.ContainsKey(k) ? myProgressBar1.Progresses[k] : 0;
        }

        string strUrl { 
            get { return textBox1.Text; }
            set { if (this.InvokeRequired) { this.Invoke((MethodInvoker)delegate { textBox1.Text = value; }); } else { textBox1.Text = value; } }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.Height > 400)
            {
                this.Height = 170;
                chart1.Visible = false;
            }
            else
            {
                this.Height = 401;
                chart1.Visible = true;
            }
        }

        string FolderPath = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            FolderPath = @"c:\users\jh\Documents\";
            foreach (string file in Directory.EnumerateFiles(Application.CommonAppDataPath))
            { 
                FileInfo fi = new FileInfo(file);
                string urlb64 = fi.Name.Replace(".tmp", "").Replace("tmp", "");
                try
                {
                    string url = Encoding.Default.GetString(System.Convert.FromBase64String(urlb64));
                    if (!string.IsNullOrEmpty(url))
                    {
                        ResumeDownload(url);
                    }
                }
                catch (Exception ex)
                { }
            }

        }

        private void ResumeDownload(string url)
        {

        }



        private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            toolStripFileName.Text = toolStripTextBox1.Text;
            toolStripTextBox1.Tag = true;
            
        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Application.CommonAppDataPath);
        }
    }
    public class DownPartClass
    {
        public bool paused { get; set; }
        public Uri uri {get;set;}
        public int i{get;set;}
        public int part{get;set;}
        public string filename{get;set;}
        public int progress { get; set; }
        public int percent { get {
            double TotalBytesToReceive = part + 1;
            return int.Parse(Math.Floor(((progress+1) / TotalBytesToReceive) * 100d).ToString());
        }
            set { }
        }
        public bool done { get; set; }
        public Exception error { get; set; }
        public int retries { get; set; }
    }
}
