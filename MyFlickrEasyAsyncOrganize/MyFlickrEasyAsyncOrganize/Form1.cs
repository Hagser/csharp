using MyFlickrEasyAsyncOrganize.classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MyFlickrEasyAsyncOrganize
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Dictionary<string, string> tokens = new Dictionary<string, string>();
        private const string Consumer_Key = "c4fbdfed59e8a2b0bcd5e700486c3fd3";
        private const string Consumer_Secret = "20d74f4746485bd9";
        private const string Consumer_Callback = "http%3A%2F%2Fwww.hagser.se%2F";
        private void bgetTokens_Click(object sender, EventArgs e)
        {
            tokens.Clear();
            WebClient wc = new WebClient();
            OAuthBase ooo = new OAuthBase();

            string normalizedUrl = "";
            string normalizedRequestParameters = "";
            string adr = "https://www.flickr.com/services/oauth/request_token" +
            "?oauth_signature_method=HMAC-SHA1" +
            "&oauth_version=1.0" +
            "&oauth_callback=" + Consumer_Callback;

            string strSignature = ooo.GenerateSignature(new Uri(adr),
                Consumer_Key, Consumer_Secret,
                "", "", "GET", ooo.GenerateTimeStamp(), ooo.GenerateNonce(),out normalizedUrl,out normalizedRequestParameters);

            string address = normalizedUrl + "?" + normalizedRequestParameters +                        
            "&oauth_signature=" + strSignature;

            label1.Text = strSignature;
            try
            {
                string str = wc.DownloadString(address);
                if (!str.Equals(""))
                {
                    foreach (string s in str.Split('&'))
                    {
                        string[] kv = s.Split('=');
                        tokens.Add(kv[0], kv[1]);
                        bgetAuth.Enabled = true;
                    }
                    bgetAcTok.Enabled = checkTokens();
                }
            }
            catch(Exception ex){
                string sksks = ex.Message;
                label1.Text = sksks;
            }
        }

        private bool checkTokens()
        {
            if(tokens.Count<1)
                return false;

            WebClient wc = new WebClient();
            OAuthBase ooo = new OAuthBase();

            string normalizedUrl = "";
            string normalizedRequestParameters = "";
            string adr = "https://api.flickr.com/services/rest" +
"?nojsoncallback=1" +
"&format=json" +
"&oauth_consumer_key="+Consumer_Key +
"&oauth_signature_method=HMAC-SHA1" +
"&oauth_version=1.0" +
"&method=flickr.auth.oauth.checkToken";

            string strSignature = ooo.GenerateSignature(new Uri(adr),
                Consumer_Key, Consumer_Secret,
                tokens["oauth_token"], tokens["oauth_token_secret"], "GET", ooo.GenerateTimeStamp(), ooo.GenerateNonce(), out normalizedUrl, out normalizedRequestParameters);

            string address = normalizedUrl + "?" + normalizedRequestParameters +
            "&oauth_signature=" + strSignature;

            label1.Text = strSignature;
            try
            {
                string str = wc.DownloadString(address);
                if (!str.Equals(""))
                {
                    foreach (string s in str.Split('&'))
                    {
                        string[] kv = s.Split('=');
                        //tokens.Add(kv[0], kv[1]);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                string sksks = ex.Message;
                label1.Text = sksks;
            }
            return false;
        }

        


        private void button2_Click(object sender, EventArgs e)
        {
            string adr = "https://www.flickr.com/services/oauth/request_token"+
"?oauth_nonce=95613465" +
"&oauth_timestamp=1305586162" +
"&oauth_consumer_key=653e7a6ecc1d528c516cc8f92cf98611"+
"&oauth_signature_method=HMAC-SHA1"+
"&oauth_version=1.0" +
"&oauth_callback=http%3A%2F%2Fwww.example.com";
            OAuthBase ooo = new OAuthBase();

            string normalizedUrl = "";
            string normalizedRequestParameters = "";

            string strSignature = ooo.GenerateSignature(new Uri(adr),
                "653e7a6ecc1d528c516cc8f92cf98611", "",
                "", "", "GET", "1305586162", "95613465", out normalizedUrl, out normalizedRequestParameters);
            label1.Text = strSignature;

        }

        private void bgetAuth_Click(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();
            string address = "https://www.flickr.com/services/oauth/authorize" +
            "?perms=delete&oauth_token=" + tokens["oauth_token"];
            webBrowser1.Navigate(address);
            webBrowser1.Navigated += (a, b) => {
                if (b.Url.AbsoluteUri.StartsWith("http://www.hagser.se"))
                {
                    string[] qs = b.Url.Query.Split('&');
                    foreach (string s in qs)
                    {
                        if (s.StartsWith("?"))
                            s.Remove(0, 1);
                        if (s.StartsWith("oauth_verifier"))
                        {
                            tokens.Add("oauth_verifier", s.Split('=')[1]);
                            bgetAcTok.Enabled = true;
                        }
                    }
                }
            };
            return;

            try
            {
                string str = wc.DownloadString(address);
                if (!str.Equals(""))
                {
                    foreach (string s in str.Split('&'))
                    {
                        string[] kv = s.Split('=');
                        tokens.Add(kv[0], kv[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                string sksks = ex.Message;
                label1.Text = sksks;
            }

        }

        private void bgetAcTok_Click(object sender, EventArgs e)
        {
            if (!tokens.ContainsKey("oauth_verifier") || !tokens.ContainsKey("oauth_token") || !tokens.ContainsKey("oauth_token_secret"))
                return;
            WebClient wc = new WebClient();
            OAuthBase ooo = new OAuthBase();

            string normalizedUrl = "";
            string normalizedRequestParameters = "";
            string adr = "https://www.flickr.com/services/oauth/access_token" +
"?oauth_verifier=" + tokens["oauth_verifier"]+
"&oauth_consumer_key="+Consumer_Key +
"&oauth_signature_method=HMAC-SHA1" +
"&oauth_version=1.0";

            string strSignature = ooo.GenerateSignature(new Uri(adr),
                Consumer_Key, Consumer_Secret,
                tokens["oauth_token"], tokens["oauth_token_secret"], "GET", ooo.GenerateTimeStamp(), ooo.GenerateNonce(), out normalizedUrl, out normalizedRequestParameters);

            string address = normalizedUrl + "?" + normalizedRequestParameters +
            "&oauth_signature=" + strSignature;

            label1.Text = strSignature;
            try
            {
                string str = wc.DownloadString(address);
                if (!str.Equals(""))
                {
                    tokens.Remove("oauth_token");
                    tokens.Remove("oauth_token_secret");
                    foreach (string s in str.Split('&'))
                    {
                        string[] kv = s.Split('=');
                        tokens.Add(kv[0], kv[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                string sksks = ex.Message;
                label1.Text = sksks;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tokens.Add("oauth_token", "72157656492128658-bee10066347e90db");
            tokens.Add("oauth_token_secret", "cb3570efb744a491");
            tokens.Add("user_id", "46372758@N07");
            bgetPhotosets.Enabled = checkTokens();
        }
        photosets phs = new photosets();
        private void bgetPhotosets_Click(object sender, EventArgs e)
        {
            if (!tokens.ContainsKey("oauth_token") || !tokens.ContainsKey("oauth_token_secret")||!tokens.ContainsKey("user_id"))
            {
                bgetPhotosets.Enabled = false;
                return;
            }

            string json = "";
            int nTries1 = 0;
            while (string.IsNullOrEmpty(json) && nTries1 < 5)
            {
                json = getJsonRest("flickr.photosets.getList", "user_id", tokens["user_id"]);
                nTries1++;
            }
            if (!string.IsNullOrEmpty(json))
            {
                XmlDocument xd = new XmlDocument();
                
                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(json.ToString().Replace(".", ","), "result");
                phs = new photosets();
                DataSet ds = new DataSet();
                ds.ReadXml(new XmlNodeReader(xd));
                using (DataTable dt1 = ds.Tables["photosets"])
                {
                    phs = getPhotosets(dt1.Rows[0]);
                }
                using (DataTable dtph = ds.Tables["photoset"])
                {
                    DataTable dtt = ds.Tables["title"];
                    DataTable dtd = ds.Tables["description"];
                    for (int n = 0; n < dtph.Rows.Count; n++)
                    {
                        DataRow dr = dtph.Rows[n];
                        DataRow drt = dtt.Rows[n];
                        DataRow drd = dtd.Rows[n];
                        photoset ph = new photoset();
                        ph = getPhotoset(dr);
                        if (drt != null)
                            ph.title = drt[0].ToString();
                        if (drd != null)
                            ph.description = drd[0].ToString();
                        phs.children.Add(ph);
                    }
                }

                photoset phnone = new photoset();
                phnone.title = "none";
                phnone.description = "none";
                phs.children.Add(phnone);

                photoset phuntagged = new photoset();
                phuntagged.title = "untagged";
                phuntagged.description = "untagged";
                phs.children.Add(phuntagged);

                chkListFrom.Items.Clear();
                comboTo.Items.Clear();
                foreach (photoset ps in phs.children)
                {
                    chkListFrom.Items.Add(ps.title);
                    comboTo.Items.Add(ps.title);
                }
                label2.Text = "Photosets:" + phs.children.Count;
                bgetPhotos.Enabled = (phs.children.Count > 0);
            }
        }


        private photosets getPhotosets(DataRow dr)
        {
            photosets w = new photosets();
            Type source = dr.GetType();
            Type dest = w.GetType();
            foreach (PropertyInfo pi in dest.GetProperties())
            {
                if (dr.Table.Columns.Contains(pi.Name))
                {
                    string str = dr[pi.Name].ToString();
                    if (pi.PropertyType == typeof(bool))
                    {
                        bool bout = false;
                        bool.TryParse(str, out bout);
                        pi.SetValue(w, bout);
                    }
                    else if (pi.PropertyType == typeof(double))
                    {
                        double bout = 0;
                        double.TryParse(str, out bout);
                        pi.SetValue(w, bout);
                    }
                    else if (pi.PropertyType == typeof(DateTime))
                    {
                        DateTime bout = DateTime.MinValue;
                        DateTime.TryParse(str, out bout);
                        pi.SetValue(w, bout);
                    }
                    else if (pi.PropertyType == typeof(Int32))
                    {
                        Int32 bout = Int32.MinValue;
                        Int32.TryParse(str, out bout);
                        pi.SetValue(w, bout);
                    }
                    else
                        pi.SetValue(w, str);
                }

            }
            return w;
        }
        private photoset getPhotoset(DataRow dr)
        {
            photoset w = new photoset();
            Type source = dr.GetType();
            Type dest = w.GetType();
            foreach (PropertyInfo pi in dest.GetProperties())
            {
                if (dr.Table.Columns.Contains(pi.Name))
                {
                    string str = dr[pi.Name].ToString();
                    if (pi.PropertyType == typeof(bool))
                    {
                        bool bout = false;
                        bool.TryParse(str, out bout);
                        pi.SetValue(w, bout);
                    }
                    else if (pi.PropertyType == typeof(double))
                    {
                        double bout = 0;
                        double.TryParse(str, out bout);
                        pi.SetValue(w, bout);
                    }
                    else if (pi.PropertyType == typeof(DateTime))
                    {
                        DateTime bout = DateTime.MinValue;
                        DateTime.TryParse(str, out bout);
                        pi.SetValue(w, bout);
                    }
                    else if (pi.PropertyType == typeof(Int32))
                    {
                        Int32 bout = Int32.MinValue;
                        Int32.TryParse(str, out bout);
                        pi.SetValue(w, bout);
                    }
                    else
                        pi.SetValue(w, str);
                }

            }
            return w;
        }
        private photo getPhoto(DataRow dr)
        {
            photo w = new photo();
            Type source = dr.GetType();
            Type dest = w.GetType();
            foreach (PropertyInfo pi in dest.GetProperties())
            {
                if (dr.Table.Columns.Contains(pi.Name))
                {
                    string str = dr[pi.Name].ToString();
                    if (pi.PropertyType == typeof(bool))
                    {
                        bool bout = false;
                        bool.TryParse(str, out bout);
                        pi.SetValue(w, bout);
                    }
                    else if (pi.PropertyType == typeof(double))
                    {
                        double bout = 0;
                        double.TryParse(str, out bout);
                        pi.SetValue(w, bout);
                    }
                    else if (pi.PropertyType == typeof(DateTime))
                    {
                        DateTime bout = DateTime.MinValue;
                        DateTime.TryParse(str, out bout);
                        pi.SetValue(w, bout);
                    }
                    else if (pi.PropertyType == typeof(Int32))
                    {
                        Int32 bout = Int32.MinValue;
                        Int32.TryParse(str, out bout);
                        pi.SetValue(w, bout);
                    }
                    else
                        pi.SetValue(w, str);
                }

            }
            return w;
        }

        private string postJsonRest(string method, string arguments, string parameters)
        {

            WebClient wc = new WebClient();
            OAuthBase ooo = new OAuthBase();

            string normalizedUrl = "";
            string normalizedRequestParameters = "";
            string adr = "https://api.flickr.com/services/rest" +
            "?nojsoncallback=1" +
            "&format=json" +
            "&oauth_consumer_key=" + Consumer_Key +
            "&oauth_signature_method=HMAC-SHA1" +
            "&oauth_version=1.0" +
            "&method=" + method;


            string postdata = "";
            var args = arguments.Split('|');
            var pars = parameters.Split('|');
            adr += "&" + args[0] + "=" + pars[0];
            for (int i = 1; i < Math.Min(args.Length, pars.Length); i++)
            {
                postdata += args[i] + "=" + pars[i]+"&";
            }

            string strSignature = ooo.GenerateSignature(new Uri(adr),
                Consumer_Key, Consumer_Secret,
                tokens["oauth_token"], tokens["oauth_token_secret"], "POST", ooo.GenerateTimeStamp(), ooo.GenerateNonce(), out normalizedUrl, out normalizedRequestParameters);

            string address = normalizedUrl + "?" + normalizedRequestParameters +
            "&oauth_signature=" + strSignature;

            try
            {
                if (postdata.EndsWith("&"))
                    postdata = postdata.Remove(postdata.Length - 1, 1);

                string str = wc.UploadString(address, "POST", postdata);
                return str;
            }
            catch (Exception ex)
            {
                string sksks = ex.Message;
                label1.Text = sksks;
            }
            return "";
        }

        private string getJsonRest(string method, string arguments, string parameters)
        {

            WebClient wc = new WebClient();
            OAuthBase ooo = new OAuthBase();

            string normalizedUrl = "";
            string normalizedRequestParameters = "";
            string adr = "https://api.flickr.com/services/rest" +
            "?nojsoncallback=1" +
            "&format=json" +
            "&oauth_consumer_key=" + Consumer_Key +
            "&oauth_signature_method=HMAC-SHA1" +
            "&oauth_version=1.0" +
            "&method=" + method;

            var args = arguments.Split('|');
            var pars = parameters.Split('|');
            for (int i=0;i<Math.Min(args.Length,pars.Length);i++)
            {
                adr += "&" + args[i] + "=" + pars[i];
            }

            string strSignature = ooo.GenerateSignature(new Uri(adr),
                Consumer_Key, Consumer_Secret,
                tokens["oauth_token"], tokens["oauth_token_secret"], "GET", ooo.GenerateTimeStamp(), ooo.GenerateNonce(), out normalizedUrl, out normalizedRequestParameters);

            string address = normalizedUrl + "?" + normalizedRequestParameters +
            "&oauth_signature=" + strSignature;

            try
            {
                string str = wc.DownloadString(address);
                return str;
            }
            catch (Exception ex)
            {
                string sksks = ex.Message;
                //label1.Text = sksks;
            }
            return "";
        }

        private void bgetPhotos_Click(object sender, EventArgs e)
        {
            if (!tokens.ContainsKey("oauth_token") || !tokens.ContainsKey("oauth_token_secret") || !tokens.ContainsKey("user_id")||phs.children.Count<1)
            {
                bgetPhotos.Enabled = false;
                return;
            }
            chkListFrom.Enabled = false;
            ThreadPool.QueueUserWorkItem(GetPhotos, null);
            
        }

        private void GetUntaggedPhotos(object state)
        {
            this.Invoke((MethodInvoker)delegate
            {
                pBSet.Maximum = 1;
                pBSet.Value = 0;
            });

            var phsid = phs.children.FirstOrDefault(x => x.title.Equals("untagged"));
            if (phsid == null)
                return;

            string json = "";
            int nTries1 = 0;
            while (string.IsNullOrEmpty(json) && nTries1 < 5)
            {
                json = getJsonRest("flickr.photos.getUntagged", "per_page", "500");
                nTries1++;
            }

            if (!string.IsNullOrEmpty(json))
            {
                XmlDocument xd = new XmlDocument();

                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(json.ToString().Replace(".", ","), "result");
                DataSet ds = new DataSet();
                ds.ReadXml(new XmlNodeReader(xd));
                using (DataTable dtph = ds.Tables["photos"])
                {
                    if (dtph != null)
                    {
                            DataRow dr = dtph.Rows[0];
                            photoset ph = new photoset() { pages = int.Parse(dr["pages"].ToString()), count_photos = int.Parse(dr["total"].ToString()) };

                            this.Invoke((MethodInvoker)delegate
                            {
                                label2.Text = "(" + ph.count_photos + ")";
                                pBMove.Maximum = ph.count_photos;
                                pBMove.Value = 0;
                            });
                            if (ph.pages == 0)
                            {
                                ph.pages = int.Parse(Math.Floor((double)ph.count_photos / 500) + "") + 1;
                            }
                            List<double> dsecavg = new List<double>();

                            DateTime dtStartFirst = DateTime.Now;
                            for (int i = 1; i <= ph.pages; i++)
                            {
                                DateTime dtStart = DateTime.Now;
                                string json2 = "";
                                int nTries2 = 0;
                                while (string.IsNullOrEmpty(json2) && nTries2 < 5)
                                {
                                    json2 = getJsonRest("flickr.photos.getUntagged", "per_page|page|extras","500|" + i + "|geo");
                                    nTries2++;
                                }
                                if (!string.IsNullOrEmpty(json2))
                                {
                                    XmlDocument xd2 = new XmlDocument();

                                    xd2 = (XmlDocument)JsonConvert.DeserializeXmlNode(json2.ToString(), "result");

                                    DataSet ds2 = new DataSet();
                                    ds2.ReadXml(new XmlNodeReader(xd2));
                                    using (DataTable dtph2 = ds2.Tables["photo"])
                                    {
                                        if (dtph2 != null)
                                        {
                                            for (int n2 = 0; n2 < dtph2.Rows.Count; n2++)
                                            {

                                                DateTime dtStart2 = DateTime.Now;

                                                DataRow dr2 = dtph2.Rows[n2];
                                                photo ph2 = new photo();
                                                ph2 = getPhoto(dr2);
                                                //getPhotoInfo(ph2);
                                                phsid.photos.Add(ph2);
                                                this.Invoke((MethodInvoker)delegate
                                                {
                                                    if ((pBMove.Value + 1) > pBMove.Maximum)
                                                        pBMove.Maximum = pBMove.Value + 1;
                                                    pBMove.Value++;
                                                    label3.Text = "Photos:" + phsid.photos.Count;
                                                });
                                                if (chkAutoTag.Checked)
                                                {
                                                    getExif(ph2, phsid.id);
                                                    if (!string.IsNullOrEmpty(ph2.place_id))
                                                        getPlace(ph2);
                                                    tagPhoto(ph2);
                                                }

                                                DateTime dtEnd = DateTime.Now;

                                                TimeSpan ts = new TimeSpan(dtEnd.Ticks - dtStart2.Ticks);
                                                double sec = ts.TotalSeconds * ph.count_photos;
                                                dsecavg.Add(sec);
                                                double davg = dsecavg.Average();
                                                DateTime ddiff = dtStartFirst.AddSeconds(davg);
                                                this.Invoke((MethodInvoker)delegate
                                                {
                                                    label6.Text = "Time left:" + dtStartFirst.ToString("HH:mm:ss") + "+" + getFTime(davg) +"="+ ddiff.ToString("HH:mm:ss");
                                                });
                                            }
                                        }
                                    }
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        label3.Text = "Photos:" + phsid.photos.Count;
                                    });
                                }//if json2 is null


                            }//Pages loog
                    }//if(dtph != null)
                }//Using datatable
            }//if json is null

            this.Invoke((MethodInvoker)delegate
            {
                pBSet.Value++;
            });
        }

        string getFTime(double seconds)
        { 
            TimeSpan ts = new TimeSpan(0,0,int.Parse(Math.Round(Math.Abs(seconds))+""));
            DateTime dt = new DateTime(ts.Ticks);

            return dt.ToString("hh:mm:ss");

        }

        private void GetPhotos(object state)
        {
            this.Invoke((MethodInvoker)delegate
            {
                pBSet.Maximum = chkListFrom.CheckedItems.Count;
                pBSet.Value = 0;
            });
            foreach (string sphotoset in chkListFrom.CheckedItems)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    pBSet.Value++;
                });
                var phsid = phs.children.FirstOrDefault(x => x.title.Equals(sphotoset));
                if (phsid == null)
                    continue;
                if (phsid.photos.Count > 0)
                {
                    phsid.photos.Clear();
                }
                if (phsid.title.Equals("untagged"))
                {
                    GetUntaggedPhotos(null);
                }
                else if (phsid.title.Equals("none"))
                {
                    GetNotInSet(null);
                }
                else
                {
                    string json = "";
                    int nTries1 = 0;
                    while (string.IsNullOrEmpty(json) && nTries1 < 5)
                    {
                        json = getJsonRest("flickr.photosets.getInfo", "user_id|photoset_id", tokens["user_id"] + "|" + phsid.id);
                        nTries1++;
                    }

                    if (!string.IsNullOrEmpty(json))
                    {
                        XmlDocument xd = new XmlDocument();

                        xd = (XmlDocument)JsonConvert.DeserializeXmlNode(json.ToString().Replace(".", ","), "result");
                        DataSet ds = new DataSet();
                        ds.ReadXml(new XmlNodeReader(xd));
                        using (DataTable dtph = ds.Tables["photoset"])
                        {
                            if (dtph != null)
                            {
                                for (int n = 0; n < dtph.Rows.Count; n++)
                                {
                                    DataRow dr = dtph.Rows[n];
                                    photoset ph = new photoset();
                                    ph = getPhotoset(dr);
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        label2.Text = phsid.title + "(" + ph.count_photos + ")";
                                        pBMove.Maximum = ph.count_photos;
                                        pBMove.Value = 0;
                                    });
                                    if (ph.pages == 0)
                                    {
                                        ph.pages = int.Parse(Math.Floor((double)ph.count_photos / 500) + "") + 1;
                                    }
                                    for (int i = 1; i <= ph.pages; i++)
                                    {

                                        string json2 = "";
                                        int nTries2 = 0;
                                        while (string.IsNullOrEmpty(json2) && nTries2 < 5)
                                        {
                                            json2 = getJsonRest("flickr.photosets.getPhotos", "user_id|photoset_id|page|extras", tokens["user_id"] + "|" + phsid.id + "|" + i + "|geo,tags");
                                            nTries2++;
                                        }
                                        if (!string.IsNullOrEmpty(json2))
                                        {
                                            XmlDocument xd2 = new XmlDocument();

                                            xd2 = (XmlDocument)JsonConvert.DeserializeXmlNode(json2.ToString(), "result");

                                            DataSet ds2 = new DataSet();
                                            ds2.ReadXml(new XmlNodeReader(xd2));
                                            using (DataTable dtph2 = ds2.Tables["photo"])
                                            {
                                                if (dtph2 != null)
                                                {
                                                    for (int n2 = 0; n2 < dtph2.Rows.Count; n2++)
                                                    {
                                                        DataRow dr2 = dtph2.Rows[n2];
                                                        photo ph2 = new photo();
                                                        ph2 = getPhoto(dr2);
                                                        phsid.photos.Add(ph2);
                                                        this.Invoke((MethodInvoker)delegate
                                                        {
                                                            if ((pBMove.Value + 1) > pBMove.Maximum)
                                                                pBMove.Maximum = pBMove.Value + 1;
                                                            pBMove.Value++;
                                                            label3.Text = "Photos:" + phsid.photos.Count;
                                                        });
                                                        if (chkAutoTag.Checked)
                                                        {
                                                            getExif(ph2, phsid.id);
                                                            if (!string.IsNullOrEmpty(ph2.place_id))
                                                                getPlace(ph2);
                                                            tagPhoto(ph2);
                                                        }
                                                    }
                                                }
                                            }
                                            this.Invoke((MethodInvoker)delegate
                                            {
                                                label3.Text = "Photos:" + phsid.photos.Count;
                                            });
                                        }//if json2 is null
                                    }//Pages loog
                                }//Photosets loop
                            }//if(dtph != null)
                        }//Using datatable
                    }//if json is null
                }//if phsid.title equals null
            }//for checked photosets loop
            this.Invoke((MethodInvoker)delegate {
                chkListFrom.Enabled = true;
            });
        }
        private void GetNotInSet(object state)
        {
            var phsid = phs.children.FirstOrDefault(x => x.title.Equals("none"));
            if (phsid == null)
                return;

            string jsonn = "";
            int nTries1 = 0;
            while (string.IsNullOrEmpty(jsonn) && nTries1 < 5)
            {
                jsonn = getJsonRest("flickr.photos.getNotInSet", "per_page", "500");
                nTries1++;
            }

            if (!string.IsNullOrEmpty(jsonn))
            {
                XmlDocument xd2 = new XmlDocument();

                xd2 = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonn.ToString(), "result");

                DataSet dsp = new DataSet();
                dsp.ReadXml(new XmlNodeReader(xd2));
                int pages=0;
                int total = 0;
                using (DataTable dtphs = dsp.Tables["photos"])
                {
                    if (dtphs != null)
                    {
                        DataRow drp = dtphs.Rows[0];
                        pages = int.Parse(drp["pages"].ToString());
                        total = int.Parse(drp["total"].ToString());

                        this.Invoke((MethodInvoker)delegate
                        {
                            label2.Text = "Photoset:" + total;
                            pBMove.Maximum = total;
                            pBMove.Value = 0;
                        });
                    }
                }
                for (int p = 1; p <= pages; p++)
                {
                    string jsonnp = "";
                    int nTriesp = 0;
                    while (string.IsNullOrEmpty(jsonnp) && nTriesp < 5)
                    {
                        jsonnp = getJsonRest("flickr.photos.getNotInSet", "per_page|page", "500|" + p);
                        nTriesp++;
                    }
                    if (!string.IsNullOrEmpty(jsonnp))
                    {
                        XmlDocument xd = new XmlDocument();

                        xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonn.ToString(), "result");

                        DataSet ds = new DataSet();
                        ds.ReadXml(new XmlNodeReader(xd2));

                        using (DataTable dtph2 = ds.Tables["photo"])
                        {
                            if (dtph2 != null)
                            {
                                for (int n2 = 0; n2 < dtph2.Rows.Count; n2++)
                                {
                                    DataRow dr2 = dtph2.Rows[n2];
                                    photo ph2 = new photo();
                                    ph2 = getPhoto(dr2);
                                    phsid.photos.Add(ph2);
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        label3.Text = "Photos:" + phsid.photos.Count;
                                        if ((pBMove.Value + 1) > pBMove.Maximum)
                                            pBMove.Maximum = pBMove.Value + 1;
                                        pBMove.Value++;
                                        
                                    });
                                    if (chkAutoTag.Checked)
                                    {
                                        getExif(ph2, phsid.id);
                                        if (!string.IsNullOrEmpty(ph2.place_id))
                                            getPlace(ph2);
                                        tagPhoto(ph2);
                                    }
                                }
                            }
                        }

                        this.Invoke((MethodInvoker)delegate
                        {
                            label3.Text = "Photos:" + phsid.photos.Count;
                        });
                    }
                }
            }        
        }

        List<place> places = new List<place>();
        private void getPlace(photo ph2)
        {
            
            if (!tokens.ContainsKey("oauth_token") || !tokens.ContainsKey("oauth_token_secret") )
            {
                return;
            }
            if (!places.Any(x => x.place_id.Equals(ph2.place_id)))
            {
                place place = new place() { place_id = ph2.place_id };
                string json = getJsonRest("flickr.places.getInfo", "place_id", ph2.place_id);
                if (!string.IsNullOrEmpty(json))
                {
                    XmlDocument xd = new XmlDocument();

                    xd = (XmlDocument)JsonConvert.DeserializeXmlNode(json.ToString().Replace(".", ","), "result");
                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlNodeReader(xd));
                    using (DataTable dt = ds.Tables["locality"])
                    {
                        if (dt != null)
                        {
                            place.locality = dt.Rows[0][0].ToString();
                        }
                    }
                    using (DataTable dt = ds.Tables["region"])
                    {
                        if (dt != null)
                        {
                            place.region = dt.Rows[0][0].ToString();
                        }
                    }
                    using (DataTable dt = ds.Tables["county"])
                    {
                        if (dt != null)
                        {
                            place.county = dt.Rows[0][0].ToString();
                        }
                    }
                    using (DataTable dt = ds.Tables["country"])
                    {
                        if (dt != null)
                        {
                            place.country = dt.Rows[0][0].ToString();
                        }
                    }
                }                
                ph2.place = place;
                places.Add(place);
            }
            else
            {
                var place = places.FirstOrDefault(x => x.place_id.Equals(ph2.place_id));
                if(place!=null)
                {
                    ph2.place = place;
                }
            }
        }
        private void getPhotoInfo(photo ph2)
        {

            if (!tokens.ContainsKey("oauth_token") || !tokens.ContainsKey("oauth_token_secret"))
            {
                return;
            }
            string json = "";
            int nTries = 0;
            while (string.IsNullOrEmpty(json) && nTries < 5)
            {
                json = getJsonRest("flickr.photos.getInfo", "photo_id", ph2.id);
                nTries++;
            }
                
            if (!string.IsNullOrEmpty(json))
            {
                XmlDocument xd = new XmlDocument();

                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(json.ToString().Replace(".", ","), "result");
                DataSet ds = new DataSet();
                ds.ReadXml(new XmlNodeReader(xd));
                using (DataTable dt = ds.Tables["photo"])
                {
                    if (dt != null)
                    {
                        photo p = getPhoto(dt.Rows[0]);
                        ph2 = p;
                    }
                }
            }
        }

        private void chkListFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            listPhotos();
        }
        private void listPhotos() {
            if (chkListFrom.SelectedItem!=null && phs != null && phs.children.Count > 0)
            {
                var phsid = phs.children.FirstOrDefault(x => x.title.Equals(chkListFrom.SelectedItem.ToString()));
                if (phsid != null)
                {
                    dataGridView1.DataSource = phsid.photos.ToList();
                }

            }

        }
        private void bRetry_Click(object sender, EventArgs e)
        {
            bgetPhotosets.Enabled = checkTokens();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;
            //https://farm1.staticflickr.com/378/20273414576_1348d8c9f1_q_d.jpg
            string staticURLBase = "http://farm{0}.staticflickr.com/{1}/{2}_{3}_q_d.jpg";
            var row = dataGridView1.SelectedRows[0];
            string farm = row.Cells[dataGridView1.Columns["farm"].Index].Value.ToString();
            string server = row.Cells[dataGridView1.Columns["server"].Index].Value.ToString();
            string id = row.Cells[dataGridView1.Columns["id"].Index].Value.ToString();
            string secret = row.Cells[dataGridView1.Columns["secret"].Index].Value.ToString();

            string imgUrl = string.Format(staticURLBase, farm, server, id, secret);
            pictureBox1.ImageLocation = imgUrl;

            
        }

        private void getExif(photo ph,string photosetid )
        {
            if (!tokens.ContainsKey("oauth_token") || !tokens.ContainsKey("oauth_token_secret") || !tokens.ContainsKey("user_id"))
            {
                return;
            }
            var phsid = phs.children.FirstOrDefault(x => x.id==photosetid || x.id.Equals(photosetid));
            if (phsid == null)
                return;

            string json = getJsonRest("flickr.photos.getExif", "photo_id|secret", ph.id + "|" + ph.secret);
            if (!string.IsNullOrEmpty(json))
            {
                XmlDocument xd = new XmlDocument();

                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(json.ToString().Replace(".", ","), "result");
                DataSet ds = new DataSet();
                ds.ReadXml(new XmlNodeReader(xd));
                using (DataTable dtex = ds.Tables["exif"])
                {
                    if (dtex != null)
                    {
                        DataTable dtr = ds.Tables["raw"];
                        for (int n = 0; n < dtex.Rows.Count; n++)
                        {
                            //photo ph = phsid.photos.FirstOrDefault(x => x.id.Equals(id));
                            DataRow drex = dtex.Rows[n];
                            DataRow drt = dtr.Rows[n];
                            //Make,Model,DateTimeOriginal

                            if (drex != null)
                            {
                                string label = drex["tag"].ToString();
                                string val = drt[0].ToString();
                                if (label.Equals("make", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    ph.make = val;
                                }
                                else if (label.Equals("model", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    ph.model = val;
                                }
                                else if (label.Equals("DateTimeOriginal", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    try
                                    {
                                        string[] vd = val.Split(' ')[0].Split(':');
                                        string[] vh = val.Split(' ')[1].Split(':');
                                        DateTime dtout = new DateTime(int.Parse(vd[0]), int.Parse(vd[1]), int.Parse(vd[2]), int.Parse(vh[0]), int.Parse(vh[1]), int.Parse(vh[2]));
                                        ph.datetimeoriginal = dtout;
                                    }
                                    catch { }
                                }
                            }

                        }
                    }
                }
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListFrom.Items.Count; i++)
                chkListFrom.SetItemChecked(i, true);
        }

        private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListFrom.Items.Count; i++)
                chkListFrom.SetItemChecked(i, false);
        }

        private void selectInvertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListFrom.Items.Count; i++)
                chkListFrom.SetItemChecked(i, !chkListFrom.GetItemChecked(i));

        }


        private void bMovePhotos_Click(object sender, EventArgs e)
        {
            if (!tokens.ContainsKey("oauth_token") || !tokens.ContainsKey("oauth_token_secret") || !tokens.ContainsKey("user_id")||phs.children.Count<1)
            {                
                return;
            }
            ThreadPool.QueueUserWorkItem(MovePhotos, null);
        }

        private void MovePhotos(object state)
        {
            string strTo = "";
            this.Invoke((MethodInvoker)delegate{
                chkListFrom.Enabled = false;
                strTo = comboTo.Text.ToString();
            });
            var phtid = phs.children.FirstOrDefault(x => x.title.Equals(strTo));
            if (phtid == null)
                return;
            int itotal = 0;
            var chkL = chkListFrom.CheckedItems;
            foreach (string sphotoset in chkL)
            {
                var phsid = phs.children.FirstOrDefault(x => x.title.Equals(sphotoset));
                if (phsid == null || phsid.photos.Count < 1)
                    continue;
                itotal += phsid.photos.Count;
            }
            this.Invoke((MethodInvoker)delegate
            {

                pBMove.Value = 0;
                pBMove.Maximum = itotal;
            });
            foreach (string sphotoset in chkL)
            {
                var phsid = phs.children.FirstOrDefault(x => x.title.Equals(sphotoset));
                if (phsid == null || phsid.photos.Count < 1)
                    continue;
                string photoIds = "";
                this.Invoke((MethodInvoker)delegate
                {
                    label2.Text = "Photoset:" + phsid.title + "(" + phsid.photos.Count + ")";
                    pBSet.Value = 0;
                    pBSet.Maximum = phsid.photos.Count;
                });
                foreach (photo p in phsid.photos)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        label3.Text = "Photo:" + p.title;
                    });
                    string json1 = "";
                    int nTries1 = 0;
                    while (string.IsNullOrEmpty(json1) && nTries1 < 5)
                    {
                        json1 = getJsonRest("flickr.photosets.addPhoto", "photo_id|photoset_id", p.id + "|" + phtid.id);
                        nTries1++;
                    }
                    if (!string.IsNullOrEmpty(json1) && !string.IsNullOrEmpty(phsid.id))
                    {
                        XmlDocument xd1 = new XmlDocument();
                        xd1 = (XmlDocument)JsonConvert.DeserializeXmlNode(json1.ToString().Replace(".", ","), "result");
                        DataSet ds1 = new DataSet();
                        ds1.ReadXml(new XmlNodeReader(xd1));
                        using (DataTable dtph1 = ds1.Tables["result"])
                        {
                            if (dtph1 != null)
                            {
                                photoIds += p.id + ",";
                                string json2 = "";
                                int nTries2 = 0;
                                while (string.IsNullOrEmpty(json2) && nTries2 < 5)
                                {
                                    json2 = getJsonRest("flickr.photosets.removePhoto", "photo_id|photoset_id", p.id + "|" + phsid.id);
                                    nTries2++;
                                }

                            }
                        }
                    }
                    this.Invoke((MethodInvoker)delegate
                    {
                        pBMove.Value++;
                        pBSet.Value++;
                    });
                }
                phsid.photos.Clear();
                this.Invoke((MethodInvoker)delegate
                {
                    label3.Text = "Done";
                });
            }
            this.Invoke((MethodInvoker)delegate
            {
                chkListFrom.Enabled = true;
            });
        }
        private void bremovePhotosets_Click(object sender, EventArgs e)
        {
            if (!tokens.ContainsKey("oauth_token") || !tokens.ContainsKey("oauth_token_secret") || !tokens.ContainsKey("user_id") || phs.children.Count < 1)
            {
                return;
            }
            
            ThreadPool.QueueUserWorkItem(RemovePhotosets, null);

        }
        private void RemovePhotosets(object state)
        {
            CheckedListBox.CheckedItemCollection chkl = null;
            this.Invoke((MethodInvoker)delegate
            {
                chkl = chkListFrom.CheckedItems;
            });
            if (chkl != null)
            {
                foreach (string sphotoset in chkl)
                {
                    foreach (var phsid in phs.children.Where(x => x.title.Equals(sphotoset)))
                    {
                        string json2 = "";
                        int nTries2 = 0;
                        while (string.IsNullOrEmpty(json2) && nTries2 < 5)
                        {
                            json2 = getJsonRest("flickr.photosets.delete", "photoset_id", phsid.id);
                            nTries2++;
                        }
                    }
                }
            }        
        }
        private void bremovePhotos_Click(object sender, EventArgs e)
        {
            if (!tokens.ContainsKey("oauth_token") || !tokens.ContainsKey("oauth_token_secret") || !tokens.ContainsKey("user_id") || phs.children.Count < 1)
            {
                return;
            }
            chkListFrom.Enabled = false;
            ThreadPool.QueueUserWorkItem(RemovePhotos, null);

        }
        private void RemovePhotos(object state)
        {
            CheckedListBox.CheckedItemCollection chks=null;
            this.Invoke((MethodInvoker)delegate
            {
                chks = chkListFrom.CheckedItems;
            });
            if (chks != null)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    pBSet.Maximum = chks.Count;
                    pBSet.Value = 0;
                });
                foreach (string sphotoset in chks)
                {
                    var phsid = phs.children.FirstOrDefault(x => x.title.Equals(sphotoset));
                    if (phsid == null || phsid.photos.Count < 1)
                        continue;
                    this.Invoke((MethodInvoker)delegate
                    {
                        pBMove.Maximum = phsid.photos.Count;
                        pBMove.Value = 0;
                        label2.Text = phsid.title + "(" + phsid.photos.Count + ")";
                    });
                    foreach (photo p in phsid.photos)
                    {
                        string json2 = "";
                        int nTries2 = 0;
                        while (string.IsNullOrEmpty(json2) && nTries2 < 5)
                        {
                            json2 = getJsonRest("flickr.photosets.removePhoto", "photo_id|photoset_id", p.id + "|" + phsid.id);
                            nTries2++;
                        }

                        this.Invoke((MethodInvoker)delegate
                        {
                            pBMove.Maximum = phsid.photos.Count;
                            pBMove.Value++;
                            label3.Text = pBMove.Value+"";
                        });
                    }
                    phsid.photos.Clear();
                    this.Invoke((MethodInvoker)delegate
                    { pBSet.Value++; });
                }
            }
            this.Invoke((MethodInvoker)delegate
            {
                chkListFrom.Enabled = true;
            });
        }
        private void btagPhotos_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(TagPhotos, null);
        }
        private void TagPhotos(object state)
        {
            this.Invoke((MethodInvoker)delegate
            {
                pBSet.Maximum = phs.children.Count;
                pBMove.Value = 0;
                pBSet.Value = 0;
            });
            foreach (photoset ps in phs.children)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    pBSet.Value++;
                    pBMove.Maximum = ps.photos.Count;
                    pBMove.Value = 0;
                });
                foreach (photo p in ps.photos)
                {
                    tagPhoto(p);
                    this.Invoke((MethodInvoker)delegate
                    {
                        pBMove.Value++;
                    });
                }
            }

        }

        private void tagPhoto(photo p)
        {
            if (!tokens.ContainsKey("oauth_token") || !tokens.ContainsKey("oauth_token_secret") || !tokens.ContainsKey("user_id"))
            {
                return;
            }

            string json = "";
            int ntries = 0;
            string tags = "";
            if (!string.IsNullOrEmpty(p.make))
            {
                tags = p.make.Split(' ')[0].Replace(",","");
                addToMake(p);
                if(!string.IsNullOrEmpty(p.model))
                    tags += " "  + (p.model + "").Replace(",", ".");
                if(p.datetimeoriginal>DateTime.MinValue)
                    tags += " "+ p.datetimeoriginal.Year + " " + p.datetimeoriginal.ToString("MMMM") + " " + p.datetimeoriginal.DayOfWeek;
            }
            if (!string.IsNullOrEmpty(p.place_id))
            {
                if (p.place != null)
                {
                    tags += " " + (p.place.country + "").Replace(",", " ");
                    tags += " " + (p.place.county + "").Replace(",", " ");
                    tags += " " + (p.place.locality + "").Replace(",", " ");
                    tags += " " + (p.place.region + "").Replace(",", " ");
                }
            }
            tags += " " + p.tags;
            if (!string.IsNullOrEmpty(tags) && tags.Length>3)
            {
                while (string.IsNullOrEmpty(json) && ntries < 5)
                {
                    json = getJsonRest("flickr.photos.setTags", "photo_id|tags", p.id + "|" + tags);
                    ntries++;
                }
            }
            
        }

        private void addToMake(photo p)
        {
            string make = p.make.Split(' ')[0];
            string json = "";
            int nTries1 = 0;
            while (string.IsNullOrEmpty(json) && nTries1 < 5)
            {
                json = getJsonRest("flickr.photosets.getList", "user_id", tokens["user_id"]);
                nTries1++;
            }
            if (!string.IsNullOrEmpty(json))
            {
                XmlDocument xd = new XmlDocument();

                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(json.ToString().Replace(".", ","), "result");
                photosets photosets = new photosets();
                DataSet ds = new DataSet();
                ds.ReadXml(new XmlNodeReader(xd));
                using (DataTable dt1 = ds.Tables["photosets"])
                {
                    photosets = getPhotosets(dt1.Rows[0]);
                }
                using (DataTable dtph = ds.Tables["photoset"])
                {
                    DataTable dtt = ds.Tables["title"];
                    DataTable dtd = ds.Tables["description"];
                    for (int n = 0; n < dtph.Rows.Count; n++)
                    {
                        DataRow dr = dtph.Rows[n];
                        DataRow drt = dtt.Rows[n];
                        DataRow drd = dtd.Rows[n];
                        photoset ph = new photoset();
                        ph = getPhotoset(dr);
                        if (drt != null)
                            ph.title = drt[0].ToString();
                        if (drd != null)
                            ph.description = drd[0].ToString();
                        photosets.children.Add(ph);
                    }
                }
            }
            var phsid = phs.children.FirstOrDefault(x => x.title.Equals(make,StringComparison.InvariantCultureIgnoreCase));
            string photoset_id = "";
            if (phsid == null)
            {
                string json2 = "";
                int nTries2 = 0;
                while (string.IsNullOrEmpty(json2) && nTries2 < 5)
                {
                    json2 = getJsonRest("flickr.photosets.create", "primary_photo_id|title", p.id + "|" + make);
                    nTries2++;
                }

                if (!string.IsNullOrEmpty(json2))
                {
                    XmlDocument xd = new XmlDocument();

                    xd = (XmlDocument)JsonConvert.DeserializeXmlNode(json2.ToString().Replace(".", ","), "result");
                    
                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlNodeReader(xd));
                    using (DataTable dt1 = ds.Tables["photoset"])
                    {
                        if (dt1 != null)
                        {
                            photoset ps = getPhotoset(dt1.Rows[0]);
                            photoset_id = ps.id;
                            ps.title = make;
                            phs.children.Add(ps);
                        }
                    }
                }
            }
            else
            {
                photoset_id = phsid.id;
            }
            string json1 = "";
            int nTries3 = 0;
            while (string.IsNullOrEmpty(json1) && nTries3 < 5)
            {
                json1 = getJsonRest("flickr.photosets.addPhoto", "photo_id|photoset_id", p.id + "|" + photoset_id);
                nTries3++;
            }
    
            
        }

        private void baddToSet_Click(object sender, EventArgs e)
        {

        }

        private void timerTagCloud_Tick(object sender, EventArgs e)
        {

            if (!tokens.ContainsKey("oauth_token") || !tokens.ContainsKey("oauth_token_secret") || !tokens.ContainsKey("user_id"))
            {
                return;
            }

            ThreadPool.QueueUserWorkItem(GetTagCloud, null);
        }

        private void GetTagCloud(object state)
        {
            string json2 = "";
            int nTries2 = 0;
            while (string.IsNullOrEmpty(json2) && nTries2 < 5)
            {
                json2 = getJsonRest("flickr.tags.getMostFrequentlyUsed", "", "");
                nTries2++;
            }

            if (!string.IsNullOrEmpty(json2))
            {
                XmlDocument xd = new XmlDocument();

                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(json2.ToString().Replace(".", ","), "who");

                DataSet ds = new DataSet();
                ds.ReadXml(new XmlNodeReader(xd));
                using (DataTable dt1 = ds.Tables["tag"])
                {
                    if (dt1 != null)
                    {

                        this.Invoke((MethodInvoker)delegate
                        {
                            flpTagCloud.Controls.Clear();
                        });
                        int sum = 0;
                        foreach (DataRow dr in dt1.Rows)
                        {
                            sum = Math.Max(int.Parse(dr[1].ToString()),sum);
                        }
                        foreach (DataRow dr in dt1.Rows)
                        {
                            Label lbl = new Label();
                            lbl.AutoSize = true;
                            lbl.Text = dr[0].ToString();
                            lbl.TextAlign = ContentAlignment.MiddleCenter;
                            lbl.Cursor = Cursors.Hand;
                            lbl.Click += (a, b) => { 
                            
                            };

                            float emSize = GetSize(sum, int.Parse(dr[1].ToString()));

                            Font font = new Font(SystemFonts.DialogFont.FontFamily, emSize, FontStyle.Bold);
                            lbl.Font = font;
                            this.Invoke((MethodInvoker)delegate
                            {
                                flpTagCloud.Controls.Add(lbl);
                            });
                        }
                    }
                }
            }
        }

        private float GetSize(int sum, int part)
        {
            float fsum = float.Parse(sum.ToString());
            float fpart = float.Parse(part.ToString());
            float fperc = (fpart / fsum);

            return Math.Max(54 * fperc,8);
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage.Text.Equals("tagcloud", StringComparison.InvariantCultureIgnoreCase))
                timerTagCloud_Tick(sender, EventArgs.Empty);
        }

        private void btnDeletePhotos_Click(object sender, EventArgs e)
        {
            if (!tokens.ContainsKey("oauth_token") || !tokens.ContainsKey("oauth_token_secret") || !tokens.ContainsKey("user_id") || phs.children.Count < 1)
            {
                return;
            }
            chkListFrom.Enabled = false;
            ThreadPool.QueueUserWorkItem(DeleteUntagged, null);
            
        }
        private void DeleteUntagged(object state)
        {
            this.Invoke((MethodInvoker)delegate
            {
                pBSet.Maximum = 1;
                pBSet.Value = 0;
            });

            var phsid = phs.children.FirstOrDefault(x => x.title.Equals("untagged"));
            if (phsid == null)
                return;

            string json = "";
            int nTries1 = 0;
            while (string.IsNullOrEmpty(json) && nTries1 < 5)
            {
                json = getJsonRest("flickr.photos.getUntagged", "per_page", "500");
                nTries1++;
            }

            if (!string.IsNullOrEmpty(json))
            {
                XmlDocument xd = new XmlDocument();

                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(json.ToString().Replace(".", ","), "result");
                DataSet ds = new DataSet();
                ds.ReadXml(new XmlNodeReader(xd));
                using (DataTable dtph = ds.Tables["photos"])
                {
                    if (dtph != null)
                    {
                        DataRow dr = dtph.Rows[0];
                        photoset ph = new photoset() { pages = int.Parse(dr["pages"].ToString()), count_photos = int.Parse(dr["total"].ToString()) };

                        this.Invoke((MethodInvoker)delegate
                        {
                            label2.Text = "(" + ph.count_photos + ")";
                            pBMove.Maximum = ph.count_photos;
                            pBMove.Value = 0;
                        });
                        if (ph.pages == 0)
                        {
                            ph.pages = int.Parse(Math.Floor((double)ph.count_photos / 500) + "") + 1;
                        }
                        List<double> dsecavg = new List<double>();

                        DateTime dtStartFirst = DateTime.Now;
                        for (int i = 1; i <= ph.pages; i++)
                        {
                            DateTime dtStart = DateTime.Now;
                            string json2 = "";
                            int nTries2 = 0;
                            while (string.IsNullOrEmpty(json2) && nTries2 < 5)
                            {
                                json2 = getJsonRest("flickr.photos.getUntagged", "per_page|page|extras", "500|" + i + "|geo");
                                nTries2++;
                            }
                            if (!string.IsNullOrEmpty(json2))
                            {
                                XmlDocument xd2 = new XmlDocument();

                                xd2 = (XmlDocument)JsonConvert.DeserializeXmlNode(json2.ToString(), "result");

                                DataSet ds2 = new DataSet();
                                ds2.ReadXml(new XmlNodeReader(xd2));
                                using (DataTable dtph2 = ds2.Tables["photo"])
                                {
                                    if (dtph2 != null)
                                    {
                                        for (int n2 = 0; n2 < dtph2.Rows.Count; n2++)
                                        {

                                            DateTime dtStart2 = DateTime.Now;

                                            DataRow dr2 = dtph2.Rows[n2];
                                            photo ph2 = new photo();
                                            ph2 = getPhoto(dr2);
                                            phsid.photos.Add(ph2);
                                            this.Invoke((MethodInvoker)delegate
                                            {
                                                if ((pBMove.Value + 1) > pBMove.Maximum)
                                                    pBMove.Maximum = pBMove.Value + 1;
                                                pBMove.Value++;
                                                label3.Text = "Photos:" + phsid.photos.Count;
                                            });

                                            getExif(ph2, phsid.id);

                                            if (string.IsNullOrEmpty(ph2.make))
                                            {
                                                string json3 = "";
                                                int nTries3 = 0;
                                                while (string.IsNullOrEmpty(json3) && nTries3 < 5)
                                                {
                                                    json3 = getJsonRest("flickr.photos.delete", "photo_id", ph2.id);
                                                    nTries3++;
                                                }
                                            }

                                            DateTime dtEnd = DateTime.Now;

                                            TimeSpan ts = new TimeSpan(dtEnd.Ticks - dtStart2.Ticks);
                                            double sec = ts.TotalSeconds * ph.count_photos;
                                            dsecavg.Add(sec);
                                            double davg = dsecavg.Average();
                                            DateTime ddiff = dtStartFirst.AddSeconds(davg);
                                            this.Invoke((MethodInvoker)delegate
                                            {
                                                label6.Text = "Time left:" + dtStartFirst.ToString("HH:mm:ss") + "+" + getFTime(davg) + "=" + ddiff.ToString("HH:mm:ss");
                                            });
                                        }
                                    }
                                }
                                this.Invoke((MethodInvoker)delegate
                                {
                                    label3.Text = "Photos:" + phsid.photos.Count;
                                });
                            }//if json2 is null


                        }//Pages loog
                    }//if(dtph != null)
                }//Using datatable
            }//if json is null

            this.Invoke((MethodInvoker)delegate
            {
                pBSet.Value++;
                chkListFrom.Enabled = true;
            });
        
        }
    }
}
