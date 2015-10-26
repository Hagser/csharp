using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Collections.Specialized;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Xml;
using System.Configuration;

namespace MyDownloadApplication
{
    public partial class Form1 : Form
    {
        WebClient wc = new WebClient();
        //ThreadStart ts;
        Thread thread;
        //string url = "";
        //string txtTitle = "";
        public NameValueCollection nvc = new NameValueCollection();
        public Form1()
        {
            InitializeComponent();
            wc.Encoding = Encoding.UTF8;
            #region Alot
            
            foreach (var a in nvc.AllKeys)
            {
                txtUrl.AutoCompleteCustomSource.Add(a);
            }
            #endregion
        }
        //int icnttim = 0;
        bool bDownloaded = false;
        bool bRunning = false;
        string strMegaVideoFile = "megavideo.com/files";
        string strDownloadPath = "";
        int icntThreadPool = 0;
        int icntMaxThreadPool = 0;
        bool bRestart = false;
        System.Windows.Forms.Timer tim = new System.Windows.Forms.Timer();
        Dictionary<string, Downloadinfo> Downloads = new Dictionary<string, Downloadinfo>();
        bool bNotRunSeries = true;
        DayOfWeek dow;
        List<series> seriess = new List<series>();
        bool fixingUrls = false;
        string strLastClipText = "";
        DownloadedFiles downloadedFiles = new DownloadedFiles();

        private MegaVideoLink GetMegaVideoLink(string strurl, string strTitle)
        {
            return GetMegaVideoLink(strurl, true, strTitle);
        }
        private MegaVideoLink GetMegaVideoLink(string strurl, bool downloadrmtp, string strTitle)
        {
            MegaVideoLink mvl = new MegaVideoLink();
            if (strurl.Contains("gorillavid"))
            {
                HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(strurl);
                hwr.Method = "GET";
                hwr.Referer = "http://www.tv-links.eu/gateway.php?data=";
                hwr.KeepAlive = true;
                hwr.UserAgent="Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11";
                hwr.Accept="text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                //hwr.Headers.Add("Accept-Encoding:gzip,deflate,sdch");
                hwr.Headers.Add("Accept-Language:sv-SE,sv;q=0.8,en-US;q=0.6,en;q=0.4");
                hwr.Headers.Add("Accept-Charset:ISO-8859-1,utf-8;q=0.7,*;q=0.3");
            
                WebResponse wr = hwr.GetResponse();

                Stream sres = wr.GetResponseStream();
                StreamReader sr = new StreamReader(sres);
                string strHTML = sr.ReadToEnd();

                string strFile1 = "<input type=\"hidden\" name=\"id\" value=\"";
                int istart1 = strHTML.IndexOf(strFile1) + strFile1.Length;
                int iend1 = strHTML.IndexOf("\"", istart1 + 1);
                string id = strHTML.Substring(istart1, iend1 - istart1).Replace("\"", "");

                string strFile3 = "<input type=\"hidden\" name=\"fname\" value=\"";
                int istart3 = strHTML.IndexOf(strFile3) + strFile3.Length;
                int iend3 = strHTML.IndexOf("\"", istart3 + 1);
                string fname = strHTML.Substring(istart3, iend3 - istart3).Replace("\"", "");

                Wait(7);

                HttpWebRequest hwr2 = (HttpWebRequest)HttpWebRequest.Create(strurl);
                hwr2.Method = "POST";
                hwr2.Referer = strurl;

            
                Uri uri = new Uri(strurl);

                hwr2.KeepAlive = true;
                hwr2.UserAgent="Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11";
                hwr2.Accept="text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                //hwr2.Headers.Add("Accept-Encoding:gzip,deflate,sdch");
                hwr2.Headers.Add("Accept-Language:sv-SE,sv;q=0.8,en-US;q=0.6,en;q=0.4");
                hwr2.Headers.Add("Accept-Charset:ISO-8859-1,utf-8;q=0.7,*;q=0.3");
                hwr2.Headers.Add("Origin: " + uri.Host);
                hwr2.ContentType="application/x-www-form-urlencoded";


                string cookies = "__utma=80043521.449880553.1330493477.1330493477.1330493477.1;__utmb=80043521.1.10.1330493477;__utmc=80043521;__utmz=80043521.1330493477.1.1.utmcsr=tv-links.eu|utmccn=(referral)|utmcmd=referral|utmcct=/_gate_way.html";
                ArrayList al = new ArrayList();
                foreach (string str in cookies.Split(';'))
                {
                    string[] cookie = str.Split('=');
                    string name = cookie[0];
                    string value = cookie[1];
                    AddCookie(al, new Cookie(name, value, "/", uri.Host));
                }
                hwr2.AllowAutoRedirect = false;
                hwr2.CookieContainer = new CookieContainer();
                foreach (Cookie c in al)
                {
                    hwr2.CookieContainer.Add(c);
                }
            
                Stream s = hwr2.GetRequestStream();

                string strPost = "op=download1&usr_login=&id=" + id + "&fname=" + fname + "&referer=" + System.Web.HttpUtility.UrlEncode("http://www.tv-links.eu/linkview.html?data=") + "&method_free=Free+Download";
                s.Write(Encoding.Default.GetBytes(strPost), 0, strPost.Length);
                WebResponse wr2 = hwr2.GetResponse();

                Stream sres2 = wr2.GetResponseStream();
                StreamReader sr2 = new StreamReader(sres2);
                string strHTML2 = sr2.ReadToEnd();


                string findmp = "var flashvars = {";
                int istartmp = strHTML2.IndexOf(findmp);
                int iendmp = strHTML2.IndexOf("}", istartmp + 1);
                string strVideoTagmp = strHTML2.Substring(istartmp + findmp.Length, iendmp - (istartmp + findmp.Length));
                strVideoTagmp = strVideoTagmp.Replace("\t", "").Replace("\n", "").Replace("\r", "").Trim();
                string strFile = "";

                foreach (string str in strVideoTagmp.Split(','))
                {
                    if (str.StartsWith("file:"))
                    {
                        strFile = str.Replace("file:", "");
                        mvl.Title = fname;
                        mvl.Url = strFile.Replace("\"", "");

                    }
                }
            
            }
            else if (strurl.Contains("urplay.se"))
            {
                try
                {
                    bool bRmtpDown = false;
                    string strHTML = wc.DownloadString(new Uri(strurl));

                    strTitle = !string.IsNullOrEmpty(strTitle)?strTitle:getTitle(strHTML, strurl);


                    string find1 = "urPlayer.init({";
                    int istart1 = strHTML.IndexOf(find1);
                    int iend1 = strHTML.IndexOf("});", istart1 + 1);
                    string strVideoTag1 = strHTML.Substring(istart1 + find1.Length, iend1 - (istart1 + find1.Length));

                    string strFile = "";
                    string strPage = "";
                    string[] strTag = new string[] { "file:", "file_hd:", "file_mobile:", "file_html5_hd:", "file_html5:", "file_flash:" };

                    foreach (string strr in strVideoTag1.Split(','))
                    {
                        string str = strr.Replace("\"", "");

                        if (strTag.Any(x=> str.StartsWith(x)))
                        {
                            strFile = str.Split(':')[1].ReplaceAll("", " ", "\"", "\\");
                            if(strFile!="")
                                break;
                        }
                        else if (str.StartsWith("sharing.link="))
                        {
                            strPage = str.Split('=')[1];
                        }
                    }
                    strPage = "rtmp://130.242.59.75/ondemand/";
                    bRmtpDown = GetWithRmtpDump(strPage+strFile,strFile,strPage,true,strTitle);
                    mvl.RtmpDump = bRmtpDown;
                    return mvl;
                    
                }
                catch (Exception ex) {  }
            }
            else if (strurl.Contains("putlocker"))
            {
                HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(strurl);
                hwr.Method = "GET";
                hwr.Referer = "http://www.tv-links.eu/gateway.php?data=";
                hwr.KeepAlive = true;
                hwr.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11";
                hwr.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                //hwr.Headers.Add("Accept-Encoding:gzip,deflate,sdch");
                hwr.Headers.Add("Accept-Language:sv-SE,sv;q=0.8,en-US;q=0.6,en;q=0.4");
                hwr.Headers.Add("Accept-Charset:ISO-8859-1,utf-8;q=0.7,*;q=0.3");

                ArrayList al = new ArrayList();

                Uri uri = new Uri(strurl);


                string cookies = "__utma=80043521.449880553.1330493477.1330493477.1330493477.1;__utmb=80043521.1.10.1330493477;__utmc=80043521;__utmz=80043521.1330493477.1.1.utmcsr=tv-links.eu|utmccn=(referral)|utmcmd=referral|utmcct=/_gate_way.html";
                foreach (string str in cookies.Split(';'))
                {
                    string[] cookie = str.Split('=');
                    string name = cookie[0];
                    string value = cookie[1];
                    AddCookie(al, new Cookie(name, value, "/", uri.Host));
                }

                hwr.CookieContainer = new CookieContainer();
                foreach (Cookie c in al)
                {
                    hwr.CookieContainer.Add(c);
                }



                WebResponse wr = hwr.GetResponse();

                Stream sres = wr.GetResponseStream();
                StreamReader sr = new StreamReader(sres);
                string strHTML = sr.ReadToEnd();
                if (!string.IsNullOrEmpty(strHTML))
                {
                    string strFile1 = "<input type=\"hidden\" value=\"";
                    int istart1 = strHTML.IndexOf(strFile1) + strFile1.Length;
                    int iend1 = strHTML.IndexOf("\" name=\"hash\">", istart1 + 1);
                    if (istart1 != -1 && iend1 != -1)
                    {
                        string id = strHTML.Substring(istart1, iend1 - istart1).Replace("\"", "");

                        if (wr.Headers["Set-Cookie"] != null)
                        {
                            foreach (string str in wr.Headers["Set-Cookie"].Split('/'))
                            {
                                if (str != "")
                                {
                                    string[] cookie = str.Trim().Split('=');
                                    string name = cookie[0].Replace(",", "").Trim();
                                    string value = cookie[1].Trim().Split(';')[0];
                                    AddCookie(al, new Cookie(name, value, "/", uri.Host));
                                }
                            }
                        }



                        Wait(7);

                        HttpWebRequest hwr2 = (HttpWebRequest)HttpWebRequest.Create(strurl);
                        hwr2.Method = "POST";
                        hwr2.Referer = strurl;

                        hwr2.KeepAlive = true;
                        hwr2.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11";
                        hwr2.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                        //hwr2.Headers.Add("Accept-Encoding:gzip,deflate,sdch");
                        hwr2.Headers.Add("Accept-Language:sv-SE,sv;q=0.8,en-US;q=0.6,en;q=0.4");
                        hwr2.Headers.Add("Accept-Charset:ISO-8859-1,utf-8;q=0.7,*;q=0.3");
                        hwr2.Headers.Add("Origin: " + uri.Host);
                        hwr2.ContentType = "application/x-www-form-urlencoded";


                        //hwr2.AllowAutoRedirect = false;
                        hwr2.CookieContainer = new CookieContainer();
                        foreach (Cookie c in al)
                        {
                            hwr2.CookieContainer.Add(c);
                        }

                        Stream s = hwr2.GetRequestStream();

                        string strPost = "hash=" + id + "&confirm=Continue+as+Free+User";
                        s.Write(Encoding.Default.GetBytes(strPost), 0, strPost.Length);
                        WebResponse wr2 = hwr2.GetResponse();

                        Stream sres2 = wr2.GetResponseStream();
                        StreamReader sr2 = new StreamReader(sres2);
                        string strHTML2 = sr2.ReadToEnd();

                        string findmp = "/get_file.php?stream=";
                        int istartmp = strHTML2.IndexOf(findmp);
                        int iendmp = strHTML2.IndexOf("'", istartmp + 1);
                        if (istartmp != -1 && istartmp < iendmp)
                        {
                            string strVideoTagmp = "http://" + uri.Host + strHTML2.Substring(istartmp, iendmp - (istartmp));

                            HttpWebRequest hwr3 = (HttpWebRequest)HttpWebRequest.Create(strVideoTagmp);
                            hwr3.Method = "GET";
                            hwr3.Referer = "http://static.putlocker.com/video_player.swf?0.7";

                            hwr3.KeepAlive = true;
                            hwr3.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11";
                            hwr3.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

                            hwr3.Headers.Add("Accept-Language:sv-SE,sv;q=0.8,en-US;q=0.6,en;q=0.4");
                            hwr3.Headers.Add("Accept-Charset:ISO-8859-1,utf-8;q=0.7,*;q=0.3");

                            hwr3.CookieContainer = new CookieContainer();
                            foreach (Cookie c in al)
                            {
                                hwr3.CookieContainer.Add(c);
                            }

                            WebResponse wr3 = hwr3.GetResponse();
                            Stream sres3 = wr3.GetResponseStream();
                            StreamReader sr3 = new StreamReader(sres3);
                            string strHTML3 = sr3.ReadToEnd();


                            XmlDocument xd = new XmlDocument();
                            xd.LoadXml(strHTML3);
                            XmlNode xn = xd.GetElementsByTagName("item")[0];

                            mvl.Url = xn.LastChild.Attributes["url"].Value;
                        }
                        else
                        {

                            findmp = "/get_file.php?download=";
                            istartmp = strHTML2.IndexOf(findmp);
                            iendmp = strHTML2.IndexOf("\"", istartmp + 1);
                            if (istartmp != -1 && istartmp < iendmp)
                            {
                                string strVideoTagmp = "http://" + uri.Host + strHTML2.Substring(istartmp, iendmp - (istartmp));
                                mvl.Url = strVideoTagmp;
                            }

                        }
                    }
                }
                return mvl;            
            }
            else if (strurl.Contains("svtplay.se"))
            {
                try
                {
                    bool bRmtpDown = false;
                    string strHTML = wc.DownloadString(new Uri(strurl));

                    /*
<a id="player" class="svtplayer svtVideoPlayer" data-id="123164"
		   data-start="click"
		   data-title="V&auml;rldens st&ouml;rsta regnskog, Del 1 - V&auml;rldens st&ouml;rsta regnskog"
		   		   data-length="3003"
		   data-available-on-mobile="true"
		   data-json-href="/video/123164"
		   data-popout-href="/video/123164?type=embed"
		   href="/video/123164?type=embed">
                     
                     */


                    //strTitle = !string.IsNullOrEmpty(strTitle) ? strTitle : getTitle(strHTML, strurl);
                    
                    //string find1 = "<param name=\"flashvars\" value=\"";
                    string find1 = "<a id=\"player\"";
                    int istart1 = strHTML.IndexOf(find1);
                    int iend1 = strHTML.IndexOf(">", istart1 + 1);
                    string strVideoTag1 = strHTML.Substring(istart1 + find1.Length, iend1 - (istart1 + find1.Length));
                    strVideoTag1 = strVideoTag1.ReplaceAll("", "\n", "\r").ReplaceAll(" ", "\t");
                    foreach (string strData in strVideoTag1.Split(' '))
                    {
                        string[] attr = strData.Split('=');
                        if (attr[0].Equals("data-id"))
                        {
                            string dataid = attr[1].Replace("\"", "");
                            string jsonurl = string.Format("http://www.svtplay.se/video/{0}?output=json",dataid);
                            HttpWebRequest hwr = HttpWebRequest.Create(jsonurl) as HttpWebRequest;
                            hwr.Accept = "application/json";
                            HttpWebResponse hws = hwr.GetResponse() as HttpWebResponse;
                            StreamReader sr = new StreamReader(hws.GetResponseStream());
                            string strJSON = sr.ReadToEnd();
                            dynamic vals = Newtonsoft.Json.JsonConvert.DeserializeObject(strJSON);
                            int br = -1;
                            string rtmpurl = "";
                            strTitle=vals.context.title;
                            foreach (dynamic vidref in vals.video.videoReferences)
                            {
                                if (vidref.bitrate > br)
                                {
                                    br = vidref.bitrate;
                                    rtmpurl = vidref.url;
                                }
                            }
                            if (rtmpurl.StartsWith("rmtpe") || rtmpurl.StartsWith("rtmp"))
                            {
                                bRmtpDown = GetWithRmtpDump(rtmpurl, strTitle);
                            }
                            else
                            {

                                mvl.Url = rtmpurl;
                                bRmtpDown = false;
                            }
                            mvl.RtmpDump = bRmtpDown;
                            return mvl;
                        }
                    }
                    /*
                    string strVal = strVideoTag1.Replace("dynamicStreams=", "");
                    foreach (string strUrl in strVal.Split('|'))
                    {
                        string strOnlyUrl = strUrl.Replace("url:", "").Split(',')[0];
                        if (strOnlyUrl.StartsWith("rmtpe") || strOnlyUrl.StartsWith("rtmp"))
                        {
                            if (downloadrmtp)
                            {
                                bRmtpDown = GetWithRmtpDump(strOnlyUrl,strTitle);

                                mvl.RtmpDump = bRmtpDown;
                                break;
                            }
                            else
                            {
                                mvl.Url = strOnlyUrl;
                                return mvl;
                            }
                        }
                        else if (strOnlyUrl.StartsWith("pathflv="))
                        {
                            strOnlyUrl = strOnlyUrl.Replace("pathflv=", "");
                            strOnlyUrl = strOnlyUrl.Substring(0,strOnlyUrl.IndexOf("&"));
                            if (strOnlyUrl.StartsWith("rmtpe") || strOnlyUrl.StartsWith("rtmp"))
                            {
                                if (downloadrmtp)
                                {
                                    bRmtpDown = GetWithRmtpDump(strOnlyUrl,strTitle);

                                    mvl.RtmpDump = bRmtpDown;
                                    break;
                                }
                                else
                                {
                                    mvl.Url = strOnlyUrl;
                                    return mvl;

                                }
                            }
                            else if(strOnlyUrl.StartsWith("http"))
                            {
                                mvl.Url = strOnlyUrl;
                                return mvl;
                            }
                        }
                    }
                    */
                    mvl.RtmpDump = bRmtpDown;
                    return mvl;
                    

                }
                catch (Exception ex) {  }
            }
            else if (strurl.StartsWith("http://www.megavideo.com/"))
            {
                try
                {
                    string strHTML = wc.DownloadString(new Uri(strurl));
                    string strSub1;
                    int intPos1;
                    //Obtaining K1 var
                    strSub1 = strHTML.Substring(strHTML.IndexOf("flashvars.title = ") + 19);
                    intPos1 = strSub1.IndexOf(";");
                    if (strTitle == "")
                    {
                        strTitle = strSub1.Substring(0, intPos1 - 1);
                    }
                    //Obtaining K1 var
                    strSub1 = strHTML.Substring(strHTML.IndexOf("flashvars.k1 = ") + 16);
                    intPos1 = strSub1.IndexOf(";");
                    string txtK1 = strSub1.Substring(0, intPos1 - 1);
                    //Obtaining K2 var
                    strSub1 = strHTML.Substring(strHTML.IndexOf("flashvars.k2 = ") + 16);
                    intPos1 = strSub1.IndexOf(";");
                    string txtK2 = strSub1.Substring(0, intPos1 - 1);
                    //Obtaining S var
                    strSub1 = strHTML.Substring(strHTML.IndexOf("flashvars.s = ") + 15);
                    intPos1 = strSub1.IndexOf(";");
                    string txtS = strSub1.Substring(0, intPos1 - 1);
                    //Obtaining UN var
                    strSub1 = strHTML.Substring(strHTML.IndexOf("flashvars.un = ") + 16);
                    intPos1 = strSub1.IndexOf(";");
                    string txtUN = strSub1.Substring(0, intPos1 - 1);
                    //Obtainig FLV flash video
                    string link = "http://www" + txtS + "."+ strMegaVideoFile +"/" + MegaVideoDecrypter.decrypt(txtUN, txtK1, txtK2) + "/";
                    mvl.Url = link;
                    return mvl;

                }
                catch (Exception ex) {  }
            }
            else if (strurl.StartsWith("http://www.blinkx.com/watch-video/"))
            {
                bool bRedirect = true;
                ArrayList al = new ArrayList();
                while (bRedirect)
                {
                    try
                    {
                        Uri uri = new Uri(strurl);
                        HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(strurl);
                        hwr.Method = "GET";
                        hwr.Referer = "http://www.blinkx.com/watch-video/";

                        string cookies = "bsid=7079221757de082518ceed5c58d1955c;bsidextra=86fb29c3e14d45a52814630b506a2e1d;bsidextrastart=tvshow";
                        foreach (string str in cookies.Split(';'))
                        {
                            string[] cookie = str.Split('=');
                            string name = cookie[0];
                            string value = cookie[1];
                            AddCookie(al, new Cookie(name, value, "/", uri.Host));
                        }
                        hwr.AllowAutoRedirect = false;
                        hwr.CookieContainer = new CookieContainer();
                        foreach (Cookie c in al)
                        {
                            hwr.CookieContainer.Add(c);
                        }
                        WebResponse wr = hwr.GetResponse();
                        if (wr.Headers["Set-Cookie"] != null)
                        {
                            foreach (string str in wr.Headers["Set-Cookie"].Split(';'))
                            {
                                string[] cookie = str.Trim().Split('=');
                                string name = cookie[0].Trim();
                                string value = cookie[1].Trim();
                                AddCookie(al, new Cookie(name, value, "/", uri.Host));
                            }
                        }
                        if (wr.Headers["Location"] != null)
                        {
                            strurl = wr.Headers["Location"];
                            bRedirect = !(strurl.StartsWith("http://www.megavideo.com/"));
                        }
                        else
                        {
                            bRedirect = false;
                        }
                    }
                    catch (Exception ex) {  bRedirect = false; }
                }
                return GetMegaVideoLink(strurl,strTitle);
            }
            else if (strurl.StartsWith("http://www.videobb.com/video/"))
            {

                string strHTML = wc.DownloadString(new Uri(strurl.Replace("http://www.videobb.com/video/", "http://www.videobb.com/player_control/settings.php?v=")));
                string strFile = "\"token1\":";
                int istart2 = strHTML.IndexOf(strFile) + strFile.Length + 1;
                int iend2 = strHTML.IndexOf(",", istart2 + 1) - 1;
                string strToken = strHTML.Substring(istart2, iend2 - istart2).Replace("\"", "");
                mvl.Url = Encoding.Default.GetString(Convert.FromBase64String(strToken));
                return mvl;
            }
            else if (strurl.StartsWith("http://www.videobb.com/watch_video.php"))
            {

                string strHTML = wc.DownloadString(new Uri(strurl.Replace("http://www.videobb.com/watch_video.php?v=", "http://www.videobb.com/player_control/settings.php?v=")));
                string strFile = "\"token1\":";
                int istart2 = strHTML.IndexOf(strFile) + strFile.Length + 1;
                int iend2 = strHTML.IndexOf(",", istart2 + 1) - 1;
                string strToken = strHTML.Substring(istart2, iend2 - istart2).Replace("\"", "");
                mvl.Url = Encoding.Default.GetString(Convert.FromBase64String(strToken));
                return mvl;

            }

            else if (strurl.StartsWith("http://www.videozer.com/video/"))
            {

                string strHTML = wc.DownloadString(new Uri(strurl.Replace("http://www.videozer.com/video/", "http://www.videozer.com/player_control/settings.php?v=")));
                string strFile = "\"token1\":";
                int istart2 = strHTML.IndexOf(strFile) + strFile.Length + 1;
                int iend2 = strHTML.IndexOf(",", istart2 + 1) - 1;
                string strToken = strHTML.Substring(istart2, iend2 - istart2).Replace("\"", "");
                mvl.Url = Encoding.Default.GetString(Convert.FromBase64String(strToken));
                return mvl;

            }
            else if (strurl.StartsWith("http://www.videozer.com/watch_video.php"))
            {

                string strHTML = wc.DownloadString(new Uri(strurl.Replace("http://www.videozer.com/watch_video.php?v=", "http://www.videozer.com/player_control/settings.php?v=")));
                string strFile = "\"token1\":";
                int istart2 = strHTML.IndexOf(strFile) + strFile.Length + 1;
                int iend2 = strHTML.IndexOf(",", istart2 + 1) - 1;
                string strToken = strHTML.Substring(istart2, iend2 - istart2).Replace("\"", "");
                mvl.Url = Encoding.Default.GetString(Convert.FromBase64String(strToken));
                return mvl;

            }
            else if (strurl.StartsWith("http://sharesix.com"))
            {
                Uri uri = new Uri(strurl);
                ArrayList al = new ArrayList();

                string strHTML = wc.DownloadString(uri);

                string strFile1 = "<input type=\"hidden\" name=\"id\" value=\"";
                int istart1 = strHTML.IndexOf(strFile1) + strFile1.Length;
                int iend1 = strHTML.IndexOf("\"", istart1 + 1);
                string id = strHTML.Substring(istart1, iend1 - istart1).Replace("\"", "");

                string strFile2 = "<input type=\"hidden\" name=\"rand\" value=\"";
                int istart2 = strHTML.IndexOf(strFile2) + strFile2.Length;
                int iend2 = strHTML.IndexOf("\"", istart2 + 1);
                string rand = strHTML.Substring(istart2, iend2 - istart2).Replace("\"", "");

                string strFile3 = "<input type=\"hidden\" name=\"fname\" value=\"";
                int istart3 = strHTML.IndexOf(strFile3) + strFile3.Length;
                int iend3 = strHTML.IndexOf("\"", istart3 + 1);
                string fname = strHTML.Substring(istart3, iend3 - istart3).Replace("\"", "");

                if (!string.IsNullOrEmpty(fname))
                {
                    strTitle = fname;
                }
                Wait(7);

                if (wc.ResponseHeaders["Set-Cookie"] != null)
                {
                    foreach (string str in wc.ResponseHeaders["Set-Cookie"].Split(';'))
                    {
                        if (!str.Contains("domain=") && !str.Contains("path=") && !str.Contains("expires="))
                        {
                            string[] cookie = str.Trim().Split('=');
                            if (cookie.Length > 1)
                            {
                                string name = cookie[0].Trim();
                                string value = cookie[1].Trim();
                                AddCookie(al, new Cookie(name, value, "/", uri.Host));
                            }
                        }
                    }
                }

                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(strurl);
                string cookies = strurl.Contains("movpod.net") ? "aff=1" : "view_count=1";
                foreach (string str in cookies.Split(';'))
                {
                    string[] cookie = str.Split('=');
                    if (cookie.Length > 1)
                    {
                        string name = cookie[0].Trim();
                        string value = cookie[1].Trim();
                        AddCookie(al, new Cookie(name, value, "/", uri.Host));
                    }
                }

                hwr.AllowAutoRedirect = false;
                hwr.CookieContainer = new CookieContainer();
                foreach (Cookie c in al)
                {
                    hwr.CookieContainer.Add(c);
                }

                if (strurl.Contains("movpod.net"))
                {
                    try
                    {
                        hwr.ContentType = "application/x-www-form-urlencoded";
                        hwr.KeepAlive = true;
                        hwr.UserAgent = "Mozilla/5.0 (Windows NT 5.1; AppleWebKit/535.2 (KHTML]= like Gecko; Chrome/15.0.874.120 Safari/535.2";
                        hwr.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                        hwr.Expect = "";
                    }
                    catch (Exception ex)
                    {
                        string s1 = ex.Message;
                    }

                    hwr.Referer = "http://www.movpod.net/cnb/" + id;


                }

                hwr.Method = "POST";
                Stream s = hwr.GetRequestStream();

                string strPost = strurl.Contains("movpod.net") ? 
                    "op=download1&usr_login=&id=" + id + "&fname=" + fname + "&referer=" + System.Web.HttpUtility.UrlEncode("http://www.tv-links.eu/linkview.html?data=") + "&method_free=Free+Download" : 
                    "op=download2&id=" + id + "&rand=" + rand + "&referer=" + System.Web.HttpUtility.UrlEncode("http://www.tv-links.eu/linkview.html?data=") + "&method_free=&method_premium=&down_direct=1";
                s.Write(Encoding.Default.GetBytes(strPost), 0, strPost.Length);
                WebResponse wr = hwr.GetResponse();

                Stream sres = wr.GetResponseStream();
                StreamReader sr = new StreamReader(sres);
                string strHTML2 = sr.ReadToEnd();

                //s1.addVariable('file','http://82.199.140.2:182/d/knq3ds43ylsah6zg6ncpfliwy7ohxwxlm74b4fi53k2q2uu67dclrsiw/111.flv');
                if (!string.IsNullOrEmpty(strHTML2))
                {
                    if (strurl.Contains("http://sharesix.com/"))
                    {
                        try
                        {
                            string findmp = "var flashvars = {";
                            int istartmp = strHTML2.IndexOf(findmp);
                            int iendmp = strHTML2.IndexOf("}", istartmp + 1);
                            string strVideoTagmp = strHTML2.Substring(istartmp + findmp.Length, iendmp - (istartmp + findmp.Length));
                            strVideoTagmp = strVideoTagmp.Replace("\t", "").Replace("\n", "").Replace("\r", "").Trim();
                            string strFile = "";

                            foreach (string str in strVideoTagmp.Split(','))
                            {
                                if (str.StartsWith("file:"))
                                {
                                    strFile = str.Replace("file:", "");
                                    mvl.Url = strFile.Replace("\"", "");
                                    return mvl;

                                }
                            }
                        }
                        catch (Exception ex) { }
                    }
                    else
                    {
                        string strFile4 = "s1.addVariable('file','";

                        int istart4 = strHTML2.IndexOf(strFile4) + strFile4.Length + 1;
                        int iend4 = strHTML2.IndexOf("'", istart4 + 1) - 1;
                        string LoomboUrl = strHTML2.Substring(istart4, iend4 - istart4);
                        mvl.Url = LoomboUrl;
                        return mvl;


                    }
                }

                mvl.Url = strurl;
                return mvl;
            }
            else if (strurl.StartsWith("http://loombo.com/") || strurl.Contains("movpod.net"))
            {
                Uri uri = new Uri(strurl);
                ArrayList al = new ArrayList();
                
                string strHTML = wc.DownloadString(uri);

                string strFile1 = "<input type=\"hidden\" name=\"id\" value=\"";
                int istart1 = strHTML.IndexOf(strFile1) + strFile1.Length;
                int iend1 = strHTML.IndexOf("\"", istart1 + 1);
                string id = strHTML.Substring(istart1, iend1 - istart1).Replace("\"", "");

                string strFile2 = "<input type=\"hidden\" name=\"rand\" value=\"";
                int istart2 = strHTML.IndexOf(strFile2) + strFile2.Length;
                int iend2 = strHTML.IndexOf("\"", istart2 + 1);
                string rand = strHTML.Substring(istart2, iend2 - istart2).Replace("\"", "");

                string strFile3 = "<input type=\"hidden\" name=\"fname\" value=\"";
                int istart3 = strHTML.IndexOf(strFile3) + strFile3.Length;
                int iend3 = strHTML.IndexOf("\"", istart3 + 1);
                string fname = strHTML.Substring(istart3, iend3 - istart3).Replace("\"", "");
                
                if (!string.IsNullOrEmpty(fname))
                {
                    strTitle = fname;
                }
                Wait(7);

                if (wc.ResponseHeaders["Set-Cookie"] != null)
                {
                    foreach (string str in wc.ResponseHeaders["Set-Cookie"].Split(';'))
                    {
                        if (!str.Contains("domain=") && !str.Contains("path=") && !str.Contains("expires="))
                        {
                            string[] cookie = str.Trim().Split('=');
                            string name = cookie[0].Trim();
                            string value = cookie[1].Trim();
                            AddCookie(al, new Cookie(name, value, "/", uri.Host));
                        }
                    }
                }

                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(strurl);
                string cookies = strurl.Contains("movpod.net")?"aff=1":"view_count=1";
                foreach (string str in cookies.Split(';'))
                {
                    string[] cookie = str.Split('=');
                    string name = cookie[0];
                    string value = cookie[1];
                    AddCookie(al, new Cookie(name, value, "/", uri.Host));
                }

                hwr.AllowAutoRedirect = false;
                hwr.CookieContainer = new CookieContainer();
                foreach (Cookie c in al)
                {
                    hwr.CookieContainer.Add(c);
                }

                if (strurl.Contains("movpod.net"))
                {
                    try
                    {                        
                        hwr.ContentType = "application/x-www-form-urlencoded";
                        hwr.KeepAlive = true;
                        hwr.UserAgent = "Mozilla/5.0 (Windows NT 5.1; AppleWebKit/535.2 (KHTML]= like Gecko; Chrome/15.0.874.120 Safari/535.2";
                        hwr.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                        hwr.Expect = "";
                    }
                    catch (Exception ex)
                    {
                        string s1 = ex.Message;
                    }
                    
                    hwr.Referer = "http://www.movpod.net/cnb/" + id;


                }

                hwr.Method = "POST";
                Stream s = hwr.GetRequestStream();

                string strPost = strurl.Contains("movpod.net") ? "op=download1&usr_login=&id=" + id + "&fname=" + fname + "&referer=" + System.Web.HttpUtility.UrlEncode("http://www.tv-links.eu/linkview.html?data=") + "&method_free=Free+Download" : "op=download2&id=" + id + "&rand=" + rand + "&referer=" + System.Web.HttpUtility.UrlEncode("http://www.tv-links.eu/linkview.html?data=") + "&method_free=&method_premium=&down_direct=1";
                s.Write(Encoding.Default.GetBytes(strPost), 0, strPost.Length);
                WebResponse wr = hwr.GetResponse();

                Stream sres = wr.GetResponseStream();
                StreamReader sr = new StreamReader(sres);
                string strHTML2 = sr.ReadToEnd();

                //s1.addVariable('file','http://82.199.140.2:182/d/knq3ds43ylsah6zg6ncpfliwy7ohxwxlm74b4fi53k2q2uu67dclrsiw/111.flv');
                if (!string.IsNullOrEmpty(strHTML2))
                {
                    if (strurl.Contains("movpod.net"))
                    {
                        try
                        {
                            string findmp = "var flashvars = {";
                            int istartmp = strHTML2.IndexOf(findmp);
                            int iendmp = strHTML2.IndexOf("}", istartmp + 1);
                            string strVideoTagmp = strHTML2.Substring(istartmp + findmp.Length, iendmp - (istartmp + findmp.Length));
                            strVideoTagmp = strVideoTagmp.Replace("\t", "").Replace("\n", "").Replace("\r", "").Trim();
                            string strFile = "";

                            foreach (string str in strVideoTagmp.Split(','))
                            {
                                if (str.StartsWith("file:"))
                                {
                                    strFile = str.Replace("file:","");
                                    mvl.Url = strFile.Replace("\"", "");
                                    return mvl;

                                }
                            }
                        }
                        catch (Exception ex) {  }
                    }
                    else
                    {
                        string strFile4 = "s1.addVariable('file','";

                        int istart4 = strHTML2.IndexOf(strFile4) + strFile4.Length + 1;
                        int iend4 = strHTML2.IndexOf("'", istart4 + 1) - 1;
                        string LoomboUrl = strHTML2.Substring(istart4, iend4 - istart4);
                        mvl.Url = LoomboUrl;
                        return mvl;


                    }
                }

                mvl.Url = strurl;
                return mvl;
            }
            else if (strurl.StartsWith("http://www.novamov.com/video/") || strurl.StartsWith("http://www.novamov.com/embed.php?v=") || strurl.StartsWith("http://embed.novamov.com/embed.php?v="))
            {
                string strHTML = wc.DownloadString(new Uri(strurl));
                string strFile = "flashvars.file=";
                int istart2 = strHTML.IndexOf(strFile) + strFile.Length + 1;
                int iend2 = strHTML.IndexOf(";", istart2 + 1) - 1;
                mvl.Url = strHTML.Substring(istart2, iend2 - istart2);
                return mvl;

            }
            else if (strurl.StartsWith("http://inturpo.com") || strurl.Contains("http://www.free-tv-video-online.me/player/novamov.php?id="))
            {
                string strHTML = wc.DownloadString(new Uri(strurl));

                string[] strNovaMovs = "http://www.novamov.com/embed.php?v=;http://embed.novamov.com/embed.php?v=".Split(';');
                foreach (string strNovaMov in strNovaMovs)
                {
                    if (strHTML.Contains(strNovaMov))
                    {
                        int istart = strHTML.IndexOf(strNovaMov);
                        int iend = strHTML.IndexOf('"', istart + 1);
                        int iend1 = strHTML.IndexOf("'", istart + 1);
                        iend = Math.Min(iend, iend1);
                        string strNovaUrl = strHTML.Substring(istart, iend - istart);
                        string strHTMLNova = wc.DownloadString(new Uri(strNovaUrl));
                        string strFile = "flashvars.file=";
                        int istart2 = strHTMLNova.IndexOf(strFile) + strFile.Length + 1;
                        int iend2 = strHTMLNova.IndexOf(";", istart2 + 1) - 1;

                        mvl.Url = strHTMLNova.Substring(istart2, iend2 - istart2);
                        return mvl;


                    }
                }
                string[] strMegaMovs = "http://www.megavideo.com/v/;http://www.megavideo.com/?v=;http://www.megavideo.com/d/;http://www.megavideo.com/?d=".Split(';');
                foreach (string strMegaMov in strMegaMovs)
                {
                    if (strHTML.Contains(strMegaMov))
                    {
                        int istart = strHTML.IndexOf(strMegaMov);
                        int iend = strHTML.IndexOf('"', istart + 1);
                        int iend1 = strHTML.IndexOf("'", istart + 1);
                        iend = Math.Min(iend, iend1);
                        string strMegaUrl = strHTML.Substring(istart, iend - istart);
                        
                        mvl.Url = strMegaUrl;
                        return mvl;

                    }
                }

            }
            else if (strurl.Contains("http://www.free-tv-video-online.me/player/zshare.php?id="))
            {
                string strHTML = "";
                bool bRunZ = true;
                ArrayList al = new ArrayList();
                while (bRunZ)
                {
                    string strzShare = "http://www.zshare.net/videoplayer/player.php";
                    if (strHTML.Contains(strzShare))
                    {
                        #region zshare2
                        Uri uri = new Uri(strurl);
                        HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(strurl);
                        hwr.Method = "GET";
                        hwr.Referer = strzShare;

                        string cookies = "sid=9a4ed3e6b39857073d0126ec11fd9fa1;_pk_id.1.9e69=18186ba2cc2a77a1.1300869689.2.1300875993.1300869703;PHPSESSID=c6950e619cc09c42bd5cc88a9cab47bd;4406a86d0ea84214c04cc353c10a2353=1";
                        foreach (string str in cookies.Split(';'))
                        {
                            string[] cookie = str.Split('=');
                            string name = cookie[0];
                            string value = cookie[1];
                            AddCookie(al, new Cookie(name, value, "/", uri.Host));
                        }
                        hwr.AllowAutoRedirect = false;
                        hwr.CookieContainer = new CookieContainer();
                        foreach (Cookie c in al)
                        {
                            hwr.CookieContainer.Add(c);
                        }
                        WebResponse wr = hwr.GetResponse();
                        if (wr.Headers["Set-Cookie"] != null)
                        {
                            foreach (string str in wr.Headers["Set-Cookie"].Split(';'))
                            {
                                string[] cookie = str.Trim().Split('=');
                                string name = cookie[0].Trim();
                                string value = cookie[1].Trim();
                                AddCookie(al, new Cookie(name, value, "/", uri.Host));
                            }
                        }

                        Stream s = wr.GetResponseStream();
                        StreamReader sr = new StreamReader(s);

                        string strHTMLzShare = sr.ReadToEnd();
                        string strFile = "file: ";
                        int istart2 = strHTMLzShare.IndexOf(strFile) + strFile.Length + 1;
                        int iend2 = strHTMLzShare.IndexOf(",", istart2 + 1) - 1;
                        bRunZ = false;
                        mvl.Url= strHTMLzShare.Substring(istart2, iend2 - istart2);
                        return mvl;
                        #endregion

                    }
                    else
                    {
                        #region zshare1
                        Uri uri = new Uri(strurl);
                        HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(strurl);
                        hwr.Method = "GET";
                        hwr.Referer = strzShare;

                        string cookies = "sid=9a4ed3e6b39857073d0126ec11fd9fa1;_pk_id.1.9e69=18186ba2cc2a77a1.1300869689.2.1300875993.1300869703;PHPSESSID=c6950e619cc09c42bd5cc88a9cab47bd;4406a86d0ea84214c04cc353c10a2353=1";
                        foreach (string str in cookies.Split(';'))
                        {
                            string[] cookie = str.Split('=');
                            string name = cookie[0];
                            string value = cookie[1];
                            AddCookie(al, new Cookie(name, value, "/", uri.Host));
                        }
                        hwr.AllowAutoRedirect = false;
                        hwr.CookieContainer = new CookieContainer();
                        foreach (Cookie c in al)
                        {
                            hwr.CookieContainer.Add(c);
                        }
                        WebResponse wr = hwr.GetResponse();
                        if (wr.Headers["Set-Cookie"] != null)
                        {
                            foreach (string str in wr.Headers["Set-Cookie"].Split(';'))
                            {
                                string[] cookie = str.Trim().Split('=');
                                string name = cookie[0].Trim();
                                string value = cookie[1].Trim();
                                AddCookie(al, new Cookie(name, value, "/", uri.Host));
                            }
                        }

                        Stream s = wr.GetResponseStream();
                        StreamReader sr = new StreamReader(s);

                        strHTML = sr.ReadToEnd();
                        int istart = strHTML.IndexOf(strzShare);
                        int iend = strHTML.IndexOf('"', istart + 1);
                        int iend1 = strHTML.IndexOf("'", istart + 1);
                        iend = Math.Min(iend, iend1);
                        strurl = strHTML.Substring(istart, iend - istart);

                        #endregion


                    }
                }
            }
            mvl.Url = strurl;
            return mvl;

        }
        private string getTitle(string strHTML, string strurl)
        {
            string ret = "";

            if (strurl.ToLower().Contains("urplay.se"))
            {
                string strTitle = "<title";
                int istart = strHTML.IndexOf(strTitle) + strTitle.Length;
                istart = strHTML.IndexOf(">", istart)+1;
                int iend = strHTML.IndexOf("</title", istart + 1);
                ret = strHTML.Substring(istart, iend - istart);
                ret = ret.Replace(" ", "_");
            }
            else if (strurl.ToLower().Contains("svtplay.se"))
            {
                string strTitle = "<meta property=\"og:title\" content=\"";
                int istart = strHTML.IndexOf(strTitle) + strTitle.Length;
                int iend = strHTML.IndexOf("\"", istart + 1);
                ret = strHTML.Substring(istart, iend - istart);
                ret = ret.Replace(" ", "_");
            }

            return ret.ReplaceRx("[^a-zA-Z0-9åäöÅÄÖ\\.-_ ]", "").Replace(":", "");
        }
        private void Wait(int p)
        {
            /*
            bool bTimeOut = false;

            System.Windows.Forms.Timer tim = new System.Windows.Forms.Timer();
            tim.Interval = p*1000;
            tim.Tick += (a, b) => { bTimeOut = true; tim.Stop(); };
            tim.Start();

            while (!bTimeOut) { Application.DoEvents(); };
            */

            Thread.Sleep(p * 1000);
        }
        private string GetLocation(string url)
        {
            WebRequest wr = WebRequest.Create(url);
            wr.Method = "get";
            try
            {
                using (WebResponse wp = wr.GetResponse())
                {
                    if (wp.Headers["location"] != null)
                    {
                        return wp.Headers["location"];
                    }
                    else if (wp.ResponseUri != null)
                    {
                        return wp.ResponseUri.AbsoluteUri;
                    }
                }
            }
            catch { }
            return url;
        }
        private string GetAllSvtVideos(string p)
        {
            string ret = "";
            string turl = "http://svtplay.se/t/";
            if (p.Contains(turl))
            {
                string s = p.Replace(turl, "");
                string[] ss = s.Split('/');
                string n = ss[0];
                int i = int.Parse(n);
                int c = 1;
                string u = "";
                bool r = true;
                while (r)
                {
                    for (int iplus = 1; iplus < 5; iplus++)
                    {
                        u = string.Format(turl + "{0}/?ajax,sb/sb,p{1},{2},f,-1", n, i + iplus, c);
                        using (WebClient wc = new WebClient())
                        {
                            wc.Encoding = Encoding.UTF8;
                            try
                            {
                                string strHtml = wc.DownloadString(u);
                                r = !strHtml.Contains("<li class=\"error\">");
                                if (r)
                                {
                                    string f = "<a href=\"/v/";
                                    string e = "\"";
                                    int istart = strHtml.IndexOf(f);
                                    int iend = strHtml.IndexOf(e, istart + f.Length + 2);
                                    while (istart != -1 && iend != -1 && iend > istart)
                                    {
                                        string found = strHtml.Substring(istart, iend - istart);
                                        ret += "http://svtplay.se/v/" + found.Replace(f, "").Split('/')[0] + ";";

                                        istart = strHtml.IndexOf(f, iend);
                                        iend = strHtml.IndexOf(e, istart + f.Length + 2);
                                    }
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                r = false;
                                break;
                            }
                        }
                    }
                    c++;
                }
            }
            else
            {
                ret = p;
            }
            return ret;
        }
        private bool GetWithRmtpDump(string strOnlyUrl, string mp4File, string strPageUrl, bool IsUr,string strTitle)
        {
            return GetWithRmtpDump(strOnlyUrl, mp4File, strPageUrl, IsUr, false, strTitle);
        }
        private bool GetWithRmtpDump(string strOnlyUrl, string mp4File, string strPageUrl, bool IsUr, bool resume, string strTitle)
        {
            if (IsUr)
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = Application.StartupPath + @"\externals\rtmpdump";

                string strFileName = (string.IsNullOrEmpty(strTitle) ? GetFileNameFromUrl(strOnlyUrl) : strTitle + GetExtFromUrl(strOnlyUrl));
                strFileName = strFileName.Replace(":", "");
                string strFilePath = strDownloadPath + "\\" + strFileName ;
                strFilePath = strFilePath.Replace(@"\\", "\\");

                if (File.Exists(strFilePath) && !resume)
                    return true;

                DownloadedFile df = downloadedFiles.Files.Where(x => x.OnlyUrl == strOnlyUrl).FirstOrDefault();
                if (df == null)
                {
                    df = new DownloadedFile() { OnlyUrl = strOnlyUrl, Size = 0, Progress = 0, FilePath = strFilePath, ContentLength = 0, Info = "" };
                    downloadedFiles.Add(df);
                }

                int icntNotChanged = 0;
                long oldLength = 0;
                System.Windows.Forms.Timer tim = new System.Windows.Forms.Timer();
                tim.Interval = 1000;
                tim.Tick += (a, b) =>
                {
                    long newLength = File.Exists(strFilePath)?(new FileInfo(strFilePath)).Length:0;
                    df.Size = newLength;
                    if (oldLength > 0 && oldLength == newLength)
                    {
                        icntNotChanged++;
                    }
                    else
                    {
                        icntNotChanged = 0;
                    }
                    FixTitle(strOnlyUrl, new Downloadinfo() { Speed = (newLength - oldLength) });
                    oldLength = newLength;
                    if (icntNotChanged > 100)
                    {
                        FixTitle(strOnlyUrl, new Downloadinfo() { Speed = 0, ProgressArgs = new DownloadInfoChangedEventArgs() { BytesReceived = newLength, TotalBytesToReceive = newLength } });
                        df.Size = newLength;
                        df.Progress = 100;
                        df.ContentLength = newLength;
                        tim.Stop();
                    }
                };
                tim.Start();
                //-r rtmp://130.242.59.75/ -W http://www.urplay.se/design/ur/javascript/jwplayer-5.10.swf -t rtmp://130.242.59.75/ondemand/ -a ondemand -p  -y mp4: 173000-173999/173268-11.mp4 -o "F:\media\SVT\blandat\Geografens_testamente__Norden__Tre_huvudstäder__UR_Play.mp4"
                string s = @"-r rtmp://130.242.59.75/ -W http://www.urplay.se/design/ur/javascript/jwplayer-5.10.swf -t rtmp://130.242.59.75/ondemand/ -a ondemand -p " + strPageUrl + " -y mp4:" + mp4File + " -o \"" + strFilePath + "\"";

                psi.Arguments = s + (resume ? " -e" : ""); ;
                psi.CreateNoWindow = false;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.UseShellExecute = true;

                Process proc = Process.Start(psi);

                proc.PriorityClass = ProcessPriorityClass.Normal;
                System.Windows.Forms.Timer timproc = new System.Windows.Forms.Timer();
                timproc.Interval = 1000 * 60 * 60 * 4;
                timproc.Tick += (a, b) => { timproc.Stop(); try { if (!proc.HasExited) { proc.Kill(); } } catch { } };
                timproc.Start();

                return true;
            }
            else
            {
                return GetWithRmtpDump(strOnlyUrl,strTitle);
            }
        }
        private bool GetWithRmtpDump(string strOnlyUrl,string strTitle)
        {
            return GetWithRmtpDump(strOnlyUrl, false,strTitle);
        }
        private bool GetWithRmtpDump(string strOnlyUrl,bool resume,string strTitle)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = psi.FileName = Application.StartupPath + @"\externals\rtmpdump";

            string strFileName = (string.IsNullOrEmpty(strTitle) ? GetFileNameFromUrl(strOnlyUrl) : strTitle + GetExtFromUrl(strOnlyUrl));
            strFileName = strFileName.Replace(":", "");
            string strFilePath = strDownloadPath + "\\" + strFileName;
            strFilePath = strFilePath.Replace(@"\\", "\\");
            

            if (File.Exists(strFilePath) && !resume)
                return true;

            DownloadedFile df = downloadedFiles.Files.Where(x => x.OnlyUrl == strOnlyUrl).FirstOrDefault();
            if (df == null)
            {
                df = new DownloadedFile() { OnlyUrl = strOnlyUrl, Size = 0, Progress = 0, FilePath = strFilePath, ContentLength = 0, Info = "" };
                downloadedFiles.Add(df);
            }

            int icntNotChanged = 0;
            long oldLength = 0;

            psi.Arguments = "-r \"" + strOnlyUrl + "\" -o \"" + strFilePath + "\"" + (resume ? " -e" : "");
            psi.CreateNoWindow = false;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = true;

            Process proc = Process.Start(psi);
            try { proc.PriorityClass = ProcessPriorityClass.Idle; proc.BeginOutputReadLine(); }
            catch { }
            System.Windows.Forms.Timer tim = new System.Windows.Forms.Timer();
            tim.Interval = 1000;
            tim.Tick+=(a,b)=>{
                try { proc.PriorityClass = ProcessPriorityClass.Idle; proc.BeginOutputReadLine(); }
                catch { }
                long newLength = File.Exists(strFilePath) ? (new FileInfo(strFilePath)).Length : 0;
                df.Size = newLength;
                if (oldLength > 0 && oldLength == newLength)
                {
                    icntNotChanged++;
                }
                else
                {
                    icntNotChanged = 0;
                }
                FixTitle(strOnlyUrl, new Downloadinfo() { Speed = (newLength - oldLength) });
                oldLength = newLength;
                try
                {
                    //lvi.SubItems[6].Text = proc.StandardOutput.ReadToEnd();
                }
                catch { }
                if (icntNotChanged > 100)
                {
                    FixTitle(strOnlyUrl, new Downloadinfo() { Speed = 0, ProgressArgs = new DownloadInfoChangedEventArgs() { BytesReceived = newLength, TotalBytesToReceive = newLength } });
                    df.Size = newLength;
                    df.Progress = 100;
                    df.ContentLength = newLength; 
                    tim.Stop();
                    
                }
            };
            tim.Start();
            
            System.Windows.Forms.Timer timproc = new System.Windows.Forms.Timer();
            timproc.Interval = 1000*60*60*4;
            timproc.Tick += (a, b) => { timproc.Stop(); try { if (!proc.HasExited) { proc.Kill(); } } catch { } };
            timproc.Start();

            return true;
        }
        private void DownQueue(object state)
        {
            if (state.GetType() == typeof(Uri))
            {
                Uri uri = (Uri)state;
                WebClient svtwc = new WebClient();
                svtwc.Encoding = Encoding.UTF8;
                this.Invoke((MethodInvoker)delegate
                {
                    
                });
                svtwc.DownloadFile(uri.AbsoluteUri, CreateStructure(strDownloadPath + "\\" + uri.LocalPath.Replace("/", "\\")));
                this.Invoke((MethodInvoker)delegate{
                    
                    icntThreadPool++;
                    if (icntThreadPool >= icntMaxThreadPool)
                    {
                        
                        this.Text = "SVT Done!";
                    }
                });
            }
        }
        private string CreateStructure(string p)
        {
            string[] parts = p.Split('\\');
            string strPath = "";
            for(int i=0;i<parts.Length-1;i++)
            {
                string part = parts[i];
                strPath += part + "\\";
                if(!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
            }
            return strPath +"\\"+ parts[parts.Length-1];
        }
        
        private void AddCookie(ArrayList cookieContainer, Cookie cookie)
        {
            if (cookieContainer.Count > 0)
            {
                foreach (Cookie c in cookieContainer)
                {
                    if (c.Name.Equals(cookie.Name))
                    {
                        c.Value = cookie.Value;
                        return;
                    }
                }
            }
            cookieContainer.Add(cookie);
        }
        private string GetVureelLink(string strurl)
        {
            string strHTML = wc.DownloadString(new Uri(strurl));
            string strfind = "jwplayer(\"mediaspace\").setup(";

            int start = strHTML.IndexOf(strfind) + strfind.Length;
            int end = strHTML.IndexOf("});", start);
            string strSetup = strHTML.Substring(start, end - start);
            string[] vars = strSetup.Split(',');
            string link = "";

            foreach (string str in vars)
            {
                string str2 = str.TrimStart('\n').TrimEnd('\n').Trim();
                if (str2.StartsWith("file"))
                {
                    string[] file = str2.Split(' ');
                    if (file.Length > 1)
                    {
                        link = file[1].Replace("\"", "").TrimStart(' ').Trim();
                    }
                }
            }

            return link;
        }
        private void StartDownload(string _url,string strTitle)
        {
            if (_url != null && _url.StartsWith("http"))
            {
                DownloadedFile file = new DownloadedFile() { OnlyUrl = _url, Info = strTitle };
                ThreadPool.SetMaxThreads(5, 5);
                ThreadPool.QueueUserWorkItem(new WaitCallback(DownloadFile2), file);
            }
        }
        private void DownloadFile2(object _file)
        {
            DownloadedFile file = _file as DownloadedFile;
            string url = file.OnlyUrl;
            string strFileName = (string.IsNullOrEmpty(file.Info) ? GetFileNameFromUrl(url) : file.Info) + ".tmp";
            string strFilePath = strDownloadPath + "\\" + strFileName;
            this.Invoke((MethodInvoker)delegate {  });

            try
            {

                this.Invoke((MethodInvoker)delegate
                {
                    while (File.Exists(strFilePath))
                    {
                        strFilePath = strFilePath + ".tmp";
                        strFileName = strFileName + ".tmp";
                    }
                    if (!File.Exists(strFilePath))
                    {
                        DownloadedFile df = new DownloadedFile() { OnlyUrl = url, Size = 0, Progress = 0, FilePath = strFilePath, ContentLength = 0, Info = "" };
                        downloadedFiles.Add(df);
                    }
                });
                bRunning = true;
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                DateTime dtOld = DateTime.Now;
                long BytesReceivedOld = 0;
                string oldspeed = "";
                long lSpeedOld = 0;
                DateTime dtLastUpdate = DateTime.Now.AddHours(-1);
                TimeSpan tsLastUpdate = new TimeSpan(DateTime.Now.Ticks - dtLastUpdate.Ticks);
                wc.DownloadProgressChanged += (a, b) =>
                {
                    if (tsLastUpdate.TotalSeconds >= 1 || tsLastUpdate.TotalMilliseconds == 0)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            DownloadedFile df = downloadedFiles.Files.Where(x => x.FilePath.ToLower().Equals(strFilePath.Replace(".part", "").ToLower())).FirstOrDefault();
                            if (df!=null)
                            {
                                string speed = oldspeed;
                                long BytesReceived = b.BytesReceived;
                                long lSpeed = lSpeedOld;

                                if (BytesReceived > BytesReceivedOld)
                                {
                                    long BytesReceivedDiff = BytesReceived - BytesReceivedOld;

                                    DateTime dtNow = DateTime.Now;
                                    long longDiff = dtNow.Ticks - dtOld.Ticks;
                                    TimeSpan tsDiff = (new TimeSpan(longDiff));
                                    if (longDiff > 0 && (tsDiff.TotalSeconds >= 2 && BytesReceivedDiff > 10000))
                                    {
                                        BytesReceivedOld = BytesReceived;
                                        TimeSpan ts = new TimeSpan(longDiff);
                                        long sDiff = long.Parse(Math.Round(ts.TotalMilliseconds, 0).ToString());
                                        lSpeed = ((BytesReceivedDiff / sDiff)) * 1000;
                                        dtOld = dtNow;
                                        speed = " " + FriendlySpeed(lSpeed);
                                        if (speed.Length > 1)
                                        {
                                            oldspeed = speed;
                                        }
                                        lSpeedOld = lSpeed;
                                    }
                                    else
                                    {
                                        speed = oldspeed;
                                        lSpeed = lSpeedOld;
                                    }
                                }
                                df.Size = BytesReceived;
                                df.Progress = b.ProgressPercentage;
                                df.ContentLength = b.TotalBytesToReceive;
                            
                                FixTitle(file.OnlyUrl, new Downloadinfo(b) { Speed = lSpeed });
                                //this.Text = txtTitle + " " + b.ProgressPercentage.ToString() + "%" + speed;
                            }
                        });
                        dtLastUpdate = DateTime.Now;
                        tsLastUpdate = new TimeSpan(DateTime.Now.Ticks - dtLastUpdate.Ticks);
                    }
                    
                };
                wc.DownloadFileCompleted += (a, b) =>
                {
                    this.Invoke((MethodInvoker)delegate { 
                        
                        if (Downloads.ContainsKey(file.OnlyUrl))
                        {
                            Downloads[file.OnlyUrl].Speed = 0; 
                        } 
                        if (strFilePath.EndsWith(".tmp")) { 
                            //File.Move(strFilePath, strFilePath.Replace(".tmp", "")); 
                        }
                        
                    }
                    );
                };
                wc.DownloadFileAsync(new Uri(url), strFilePath);

            }
            catch (Exception ex)
            { this.Invoke((MethodInvoker)delegate {  bRunning = false; }); }

        }
        private void FixTitle(string strUrl, Downloadinfo di)
        {
            if (!string.IsNullOrEmpty(strUrl))
            {
                if (Downloads.ContainsKey(strUrl))
                {
                    Downloads.Remove(strUrl);
                }
                Downloads.Add(strUrl, di);

                double progress = 0;
                long speeds = 0;
                int cntDone = Downloads.Values.Count(x => x.ProgressPercentage == 100);
                progress = Downloads.Values.Sum(x => x.ProgressPercentage);
                speeds = Downloads.Values.Sum(x => x.Speed);
                progress = (progress / Downloads.Values.Count);

                this.Text = cntDone.ToString() + "/" + Downloads.Values.Count.ToString() + " " + Math.Round(progress, 0).ToString() + "%" + FriendlySpeed(speeds);
            }
            else
            {
                string stop = "";
            }
        }
        private string FriendlySpeed(long lSpeed)
        {
            return " " + (lSpeed > 1000000 ? (lSpeed / 1000000).ToString() + " MB/s" : lSpeed > 1024 ? (lSpeed / 1024).ToString() + " kB/s" : lSpeed.ToString() + " B/s");
        }
        private long GetSize(string url, bool bTestRange)
        {
            long iret = -1;
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(url);
                hwr.Timeout = 10000;
                if (bTestRange)
                {
                    hwr.Method = "GET";
                    hwr.AddRange("bytes", 0, 100000);
                }
                else
                {
                    hwr.Method = "HEAD";
                }
                WebResponse wr = hwr.GetResponse();
                iret = wr.ContentLength;
            }
            catch (Exception ex)
            {
                iret = -2;
            }
            return iret;
        }
        private string GetFileNameFromUrl(string url)
        {
            string[] strUrl = url.Split('/');
            string[] strQUrl = strUrl[strUrl.Length - 1].Split('?');
            return strQUrl[0];
        }
        private string GetExtFromUrl(string url)
        {
            string[] strUrl = url.Split('.');
            string[] strQUrl = strUrl[strUrl.Length - 1].Split('?');
            return "." + strQUrl[0];
        }
        private string GetExt(string p)
        {
            foreach (string s in "pdf;htm;txt;mp3;avi;flv;doc;xml".Split(';'))
            {
                if (p.ToLower().Contains(s))
                    return s;
            }
            if (p.ToLower().Contains("audio/mpeg"))
                return "mp3";
            if (p.ToLower().Contains("plain"))
                return "txt";

            return "tmp";
        }
        private void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string strTitle="";
                tim.Tick += (a, b) =>
                    {
                        tim.Stop();

                        bRestart = false;
                        if (txtUrl.Text.Contains(";"))
                        {
                            Downloads.Clear();
                            ArrayList alUrls = new ArrayList();
                            ArrayList alMvlUrls = new ArrayList();
                            foreach (string strUrl in txtUrl.Text.Split(';'))
                            {
                                try
                                {
                                    if (!alUrls.Contains(strUrl))
                                    {
                                        alUrls.Add(strUrl);
                                        MegaVideoLink mvl = GetMegaVideoLink(strUrl,strTitle);

                                        if (!Downloads.ContainsKey(mvl.Url) && !mvl.RtmpDump)
                                        {
                                            //Downloads.Add(mvl.Url, new Downloadinfo());
                                            //StartDownload(mvl.Url,strTitle);
                                            alMvlUrls.Add(mvl);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                { }
                            }
                            foreach (MegaVideoLink mvl in alMvlUrls)
                            {
                                StartDownload(mvl.Url, mvl.Title);
                            }
                            txtUrl.Text = "";
                        }
                        else if (txtUrl.Text.StartsWith("rtmp://"))
                        {
                            GetWithRmtpDump(txtUrl.Text, strTitle);
                            txtUrl.Text = "";
                        }
                        else if (txtUrl.Text.StartsWith("rtmpe://"))
                        {
                            GetWithRmtpDump(txtUrl.Text, strTitle);
                            txtUrl.Text = "";
                        }
                        else if (txtUrl.Text.Contains("urplay.se"))
                        {
                            MegaVideoLink mvl = GetMegaVideoLink(txtUrl.Text, strTitle);
                            txtUrl.Text = mvl.Url;
                            if (!mvl.RtmpDump)
                                StartDownload(txtUrl.Text, strTitle);
                        }
                        else if (txtUrl.Text.Contains("svtplay.se"))
                        {
                            MegaVideoLink mvl = GetMegaVideoLink(txtUrl.Text, strTitle);
                            txtUrl.Text = mvl.Url;
                            if (!mvl.RtmpDump)
                                StartDownload(txtUrl.Text, strTitle);
                        }
                        else if (txtUrl.Text.Contains("sharesix.com"))
                        {
                            MegaVideoLink mvl = GetMegaVideoLink(txtUrl.Text, strTitle);
                            txtUrl.Text = mvl.Url;
                            if (!mvl.RtmpDump)
                                StartDownload(txtUrl.Text, strTitle);
                        }
                        else if (txtUrl.Text.Contains("movpod.net"))
                        {
                            MegaVideoLink mvl = GetMegaVideoLink(txtUrl.Text, strTitle);
                            txtUrl.Text = mvl.Url;
                            if (!mvl.RtmpDump)
                                StartDownload(txtUrl.Text, strTitle);
                        }
                        else if (txtUrl.Text.Contains("http://www.megavideo.com/"))
                        {
                            MegaVideoLink mvl = GetMegaVideoLink(txtUrl.Text, strTitle);
                            txtUrl.Text = mvl.Url;
                            if (!mvl.RtmpDump)
                                StartDownload(txtUrl.Text, strTitle);
                        }
                        else if (txtUrl.Text.Contains("http://loombo.com/"))
                        {
                            MegaVideoLink mvl = GetMegaVideoLink(txtUrl.Text, strTitle);
                            txtUrl.Text = mvl.Url;
                            if (!mvl.RtmpDump)
                                StartDownload(txtUrl.Text, strTitle);
                        }
                        else if (txtUrl.Text.StartsWith("http://inturpo.com") || txtUrl.Text.Contains("http://www.free-tv-video-online.me/player/novamov.php?id"))
                        {
                            MegaVideoLink mvl = GetMegaVideoLink(txtUrl.Text, strTitle);
                            txtUrl.Text = mvl.Url;
                            if (!mvl.RtmpDump)
                                StartDownload(txtUrl.Text, strTitle);
                        }
                        else if (txtUrl.Text.Contains("http://www.free-tv-video-online.me/player/zshare.php?id"))
                        {
                            MegaVideoLink mvl = GetMegaVideoLink(txtUrl.Text, strTitle);
                            txtUrl.Text = mvl.Url;
                            if (!mvl.RtmpDump)
                                StartDownload(txtUrl.Text, strTitle);
                        }
                        else if (txtUrl.Text.StartsWith("http://www.videobb.com/video/") || txtUrl.Text.StartsWith("http://www.videobb.com/watch_video.php"))
                        {
                            MegaVideoLink mvl = GetMegaVideoLink(txtUrl.Text, strTitle);
                            txtUrl.Text = mvl.Url;
                            if (!mvl.RtmpDump)
                                StartDownload(txtUrl.Text, strTitle);
                        }
                        else if (txtUrl.Text.StartsWith("http://www.videozer.com/video/") || txtUrl.Text.StartsWith("http://www.videozer.com/watch_video.php"))
                        {
                            MegaVideoLink mvl = GetMegaVideoLink(txtUrl.Text, strTitle);
                            txtUrl.Text = mvl.Url;
                            if (!mvl.RtmpDump)
                                StartDownload(txtUrl.Text, strTitle);
                        }
                        else if (txtUrl.Text.StartsWith("http://www.novamov.com/video/") || txtUrl.Text.StartsWith("http://www.novamov.com/embed.php?v=") || txtUrl.Text.StartsWith("http://embed.novamov.com/embed.php?v="))
                        {
                            MegaVideoLink mvl = GetMegaVideoLink(txtUrl.Text, strTitle);
                            txtUrl.Text = mvl.Url;
                            if (!mvl.RtmpDump)
                                StartDownload(txtUrl.Text, strTitle);
                        }
                        else if (txtUrl.Text.Contains("vureel.com/video"))
                        {
                            txtUrl.Text = GetVureelLink(txtUrl.Text);
                            StartDownload(txtUrl.Text, strTitle);
                        }
                        else if (txtUrl.Text.Contains("gorillavid"))
                        {
                            MegaVideoLink mvl = GetMegaVideoLink(txtUrl.Text, "");
                            txtUrl.Text = mvl.Url;
                            StartDownload(mvl.Url, mvl.Title);
                        }
                        else if (txtUrl.Text.Contains("putlocker"))
                        {
                            MegaVideoLink mvl = GetMegaVideoLink(txtUrl.Text, "");
                            txtUrl.Text = mvl.Url;
                            StartDownload(mvl.Url, mvl.Title);
                        }
                        else if (!string.IsNullOrEmpty(txtUrl.Text) && txtUrl.Text.StartsWith("http://"))
                        {
                            bRestart = true;
                            StartDownload(txtUrl.Text, strTitle);
                        }
                        else
                        {
                            strTitle = txtUrl.Text;
                            txtUrl.Text = nvc[txtUrl.Text];
                        }
                        btnDownload.Enabled = true;
                    };
                tim.Interval = comboBox1.Text.Equals("Now") ? 1000 : int.Parse(comboBox1.Text) * 3600000;
                tim.Start();
                btnDownload.Enabled = false;
            }
            catch (Exception ex)
            {  }
        }
        private void StartSeriesMonitor()
        {
            if (DateTime.Today.DayOfWeek != dow || bNotRunSeries)
            {
                dow = DateTime.Today.DayOfWeek;
                bNotRunSeries = false;

                ArrayList al = new ArrayList();
                foreach (series s in seriess.Where(x=>x.enabled && x.day.ToLower().Equals(DateTime.Today.DayOfWeek.ToString().ToLower())))
                {
                    string strLast = GetAllSvtVideos(s.url).Split(';')[0];
                    al.Add(GetMegaVideoLink((!string.IsNullOrEmpty(strLast) ? strLast : s.url), true, (!string.IsNullOrEmpty(strLast) ? "" : s.title)));
                }
                dataGridViewSeries.DataSource = seriess;
                dataGridViewSeries.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                foreach (MegaVideoLink mvl in al)
                {
                    if (!mvl.RtmpDump && mvl.Url != null && mvl.Url.StartsWith("http"))
                    {
                        StartDownload(mvl.Url,mvl.Title);
                    }
                }
            }
            System.Windows.Forms.Timer tim = new System.Windows.Forms.Timer();
            tim.Interval = 1000 * 60 * 60 * 8;
            tim.Tick += (a, b) => { StartSeriesMonitor(); tim.Stop(); };
            tim.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            strDownloadPath = GetSettings("DownloadPath");
            if (strDownloadPath == "")
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.ShowNewFolderButton = false;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    strDownloadPath = fbd.SelectedPath;
                    SaveSettings("DownloadPath", strDownloadPath);
                }
            }

            if (File.Exists(strDownloadPath + "\\downloadedfiles.xml"))
            {
                string xml = File.ReadAllText(strDownloadPath + "\\downloadedfiles.xml");

                downloadedFiles = (SeDes.ToObj(xml,downloadedFiles) as DownloadedFiles);
                downloadedFiles.Files.Where(x => !File.Exists(x.FilePath)).ForAll((x) => { x.Info = "Non existing"; });
            }

            if (File.Exists(strDownloadPath + "\\downloadseries.xml"))
            {
                string xml = File.ReadAllText(strDownloadPath + "\\downloadseries.xml");

                seriess = (SeDes.ToObj(xml,seriess) as List<series>);
            }
            BindingSource bs = new BindingSource();
            bs.DataSource=downloadedFiles;
            bs.ResumeBinding();
            bs.DataMember = "Files";
            bs.AllowNew = true;
            //Binding bind = new Binding("DataSource", downloadedFiles, "Files");
            //bind.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            //bind.ControlUpdateMode = ControlUpdateMode.OnPropertyChanged;
            //dataGridDownload.DataBindings.Add(bind);
            dataGridDownload.DataSource = bs;
            
            downloadedFiles.FileAdded += (a, b) =>
            {
                dataGridDownload.Refresh();
                dataGridDownload.DataSource = null; dataGridDownload.DataSource = bs;

            };
            downloadedFiles.FileRemoved += (a, b) =>
            {
                dataGridDownload.Refresh();
                dataGridDownload.DataSource = null; dataGridDownload.DataSource = bs;

            };
            DateTime dtLastUpdate = DateTime.Now.AddHours(-1);
            TimeSpan tsLastUpdate = new TimeSpan(DateTime.Now.Ticks - dtLastUpdate.Ticks);
            downloadedFiles.FilesChanged += (a, b) =>
            {
                if (tsLastUpdate.TotalSeconds >= 1 || tsLastUpdate.TotalMilliseconds==0)
                {
                    dataGridDownload.Refresh();
                    dtLastUpdate = DateTime.Now;
                }
                tsLastUpdate = new TimeSpan(DateTime.Now.Ticks - dtLastUpdate.Ticks);
            };

            StartSeriesMonitor();

        }

        private void SaveSettings(string name, string value)
        {
            string file = Application.StartupPath + "\\settings.xml";
            if (!File.Exists(file))
            {
                File.AppendAllText(file,"<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<settings/>");
            }
            XmlDocument xd = new XmlDocument();
            xd.Load(file);

            XmlAttribute xatt = xd.DocumentElement.Attributes[name];
            if (xatt != null)
            {
                xatt.Value = value;
            }
            else
            {
                XmlAttribute att = xd.CreateAttribute(name);
                att.Value = value;
                xd.DocumentElement.Attributes.Append(att);
            }
            
            xd.Save(file); 
        }

        private string GetSettings(string name)
        {
            string file = Application.StartupPath + "\\settings.xml";
            if(File.Exists(file))
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(file);
                XmlAttribute xatt = xd.DocumentElement.Attributes[name];
                if (xatt != null)
                {
                    return xatt.Value;
                }       
            }
            return "";
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (thread!=null&&thread.IsAlive)
                {
                    thread.Abort();
                }
            }
            finally
            {
            }

            string xmlser = SeDes.ToXml(seriess);
            File.WriteAllText(strDownloadPath + "\\downloadseries.xml", xmlser);

            string xml = SeDes.ToXml(downloadedFiles);
            File.WriteAllText(strDownloadPath + "\\downloadedfiles.xml",xml);
            //Process.Start(Application.StartupPath);
        }
        private void button1_Click(object sender, EventArgs e)
        {
        }
        private void timerClipboard_Tick(object sender, EventArgs e)
        {
            string strText = Clipboard.GetText();
            if((strText.StartsWith("http://") || strText.StartsWith("rtmpe://")) && !strText.Equals(strLastClipText))
            {
                strLastClipText = strText;
                if (txtUrl.Text.Length == 0)
                {
                    txtUrl.Text = strLastClipText;
                }
                else if (txtUrl.Text.EndsWith(";"))
                {
                    txtUrl.Text += strLastClipText;
                }
                else
                {
                    txtUrl.Text += ";" + strLastClipText;
                }

            }
        }
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            btnDownload.Enabled = true;
            tim.Stop();
        }
        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridDownload.SelectedRows.Count>=1)
            {
                foreach (DataGridViewRow lvi in dataGridDownload.SelectedRows)
                {
                    if (!string.IsNullOrEmpty(lvi.Cells[1].Value.ToString()))
                    {
                        string strUrl = lvi.Cells[1].Value.ToString();
                        string strTitle = lvi.Cells[0].Value.ToString();
                        if (strUrl.ToLower().StartsWith("r") && strUrl.Contains("://"))
                        {
                            GetWithRmtpDump(strUrl, strTitle);
                        }
                        else if (strUrl.ToLower().StartsWith("h") && strUrl.Contains("://"))
                        {
                            StartDownload(strUrl, strTitle);
                        }
                    }
                }
            }
        }
        private void resumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridDownload.SelectedRows.Count >= 1)
            {
                foreach (DataGridViewRow lvi in dataGridDownload.SelectedRows)
                {
                    string strUrl = lvi.Cells[1].Value.ToString();
                    string strTitle = lvi.Cells[0].Value.ToString();
                    bool IsUr = strUrl.ToLower().Contains("urplay");
                    if ((strUrl.ToLower().StartsWith("r") || IsUr) && strUrl.Contains("://"))
                    {
                        if (IsUr)
                        {
                            string[] strUrlSplit = strUrl.Replace("http://","").Split('/');
                            string strPageUrl = "http://" + strUrlSplit[0] + "/" + strUrlSplit[1] + "/";
                            string mp4File = strUrl.Replace(strPageUrl,"");
                            GetWithRmtpDump(strUrl, mp4File, strPageUrl, true, true, strTitle);
                        }
                        else
                        {
                            GetWithRmtpDump(strUrl, true, strTitle);
                        }
                    }
                    else if (strUrl.ToLower().StartsWith("h") && strUrl.Contains("://"))
                    {
                        StartDownload(strUrl, strTitle);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(txtUrl.Text))
                        {
                            MegaVideoLink mvl = GetMegaVideoLink(txtUrl.Text, false, strTitle);
                            IsUr = mvl.Url.ToLower().Contains("urplay");
                            if ((mvl.Url.ToLower().StartsWith("r") || IsUr) && mvl.Url.Contains("://"))
                            {
                                if (IsUr)
                                {
                                    string[] strUrlSplit = strUrl.Replace("http://", "").Split('/');
                                    string strPageUrl = "http://" + strUrlSplit[0] + "/" + strUrlSplit[1] + "/";
                                    string mp4File = strUrl.Replace(strPageUrl, "");

                                    GetWithRmtpDump(mvl.Url, mp4File, strPageUrl, true, true, strTitle);
                                }
                                else
                                {
                                    GetWithRmtpDump(mvl.Url, true, strTitle);
                                }

                            }
                        }
                    }
                }
            }
        }
        private void btnDownloadChecked_Click(object sender, EventArgs e)
        {
            if (!seriess.Any(x=>x.download))
                return;
            txtUrl.Text = "";
            foreach (series o in seriess.Where(x=>x.download))
            {
                string strLast = GetAllSvtVideos(o.url).Split(';')[0];
                txtUrl.Text += o.url + ";";
            }
            btnDownload_Click(sender, EventArgs.Empty);
        }
        private void txtUrl_TextChanged(object sender, EventArgs e)
        {
            if (!fixingUrls)
            {
                fixingUrls = true;
                string strText = txtUrl.Text;
                string[] urls = strText.Split(';');

                foreach (string url in urls)
                {
                    if (url.Contains("tv-links.eu/gateway.php"))
                    {
                        strText = strText.Replace(url, GetLocation(url));
                    }
                }
                txtUrl.Text = strText;
                fixingUrls = false;
            }
        }
        private void btnAllSvt_Click(object sender, EventArgs e)
        {
            txtUrl.TextChanged -= txtUrl_TextChanged;

            string strText = txtUrl.Text;
            string[] urls = strText.Split(';');

            foreach (string url in urls)
            {
                if (url.Contains("http://svtplay.se/t/"))
                {
                    strText = strText.Replace(url,GetAllSvtVideos(url))+";";
                }
            }
            txtUrl.Text = strText.Length > 0 ? strText.Substring(0, strText.Length - 1) : strText;
            txtUrl.TextChanged += txtUrl_TextChanged;
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(strDownloadPath);
        }

        private void dataGridViewSeries_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.V)
                {
                    string strtext = Clipboard.GetText();
                    if (strtext.StartsWith("http"))
                    {
                        seriess.Add(new series() { day = "", download = false, enabled = false, url = strtext });
                        dataGridViewSeries.DataSource = null;
                        dataGridViewSeries.DataSource = seriess;
                    }
                }

            }
            else
            {
                if (e.KeyCode == Keys.Delete)
                {
                    dataGridViewSeries.SuspendLayout();
                    foreach (DataGridViewRow row in dataGridViewSeries.SelectedRows)
                    {
                        series s = seriess.Where(x => x.url == row.Cells[3].Value.ToString()).FirstOrDefault();
                        if (s != null)
                            seriess.Remove(s);
                    }
                    dataGridViewSeries.DataSource = null;
                    dataGridViewSeries.DataSource = seriess;
                    dataGridViewSeries.ResumeLayout();
                    dataGridViewSeries.ClearSelection();
                    dataGridViewSeries.Refresh();
                }
            }
        }

        private void dataGridDownload_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                dataGridDownload.SuspendLayout();
                foreach (DataGridViewRow row in dataGridDownload.SelectedRows)
                {
                    if (row != null && row.Cells!=null && row.Cells[0]!=null &&row.Cells[0].Value!=null)
                    {
                        DownloadedFile df = downloadedFiles.Files.Where(x => x.FilePath == row.Cells[6].Value.ToString() && x.OnlyUrl == row.Cells[1].Value.ToString()).FirstOrDefault();
                        if (df != null)
                            downloadedFiles.Remove(df);
                    }
                }
                dataGridDownload.ResumeLayout();
                dataGridDownload.ClearSelection();
                dataGridDownload.Refresh();
            }
        }

        private void dataGridDownload_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btnGetTitle_Click(object sender, EventArgs e)
        {
            foreach (series s in seriess.Where(x => string.IsNullOrEmpty(x.title)))
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    string strHtml = wc.DownloadString(s.url);
                    s.title = getTitle(strHtml, s.url);
                }
            }

            dataGridViewSeries.DataSource = null;
            dataGridViewSeries.DataSource = seriess;
        }

        private void runTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 1000;
            DownloadedFile df =new DownloadedFile();
            df.OnlyUrl="http://nisse.tst";
            df.FilePath="nisse.tst";
            df.Size=0;
            df.Progress=0;
            t.Tick += (a, b) =>
            {
                df.Progress++;
                df.Size=df.Size+10000;
                Downloadinfo di =new Downloadinfo() { Speed = (new Random().Next(1000,500000))  };
                di.ProgressArgs.BytesReceived = long.Parse(df.Size.ToString());
                di.ProgressArgs.TotalBytesToReceive = 1000000;
                di.ProgressArgs.UserState = df;

                FixTitle(df.OnlyUrl,di);
            };
            t.Start();

            downloadedFiles.Add(df);
            
        }

        private void dataGridDownload_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            { 
                DataGridViewCell dgvcname = dataGridDownload[e.ColumnIndex,e.RowIndex];
                DownloadedFile df = downloadedFiles.Files[e.RowIndex];
                
                FileInfo fiOld =new FileInfo(df.FilePath);
                string newFileName = fiOld.FullName.Replace(fiOld.Name,dgvcname.Value.ToString());
                if (!File.Exists(newFileName))
                {
                    File.Move(fiOld.FullName, newFileName);
                    df.FilePath = newFileName;
                }
            }
        }

        private void dataGridDownload_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }
    }
    public static class ieext {
        public static IEnumerable<T> ForAll<T>(this IEnumerable<T> list, Action<T> act)
        {
            foreach (T t in list)
            {
                act.Invoke(t);
            }
            return list;
        }
    }

}
