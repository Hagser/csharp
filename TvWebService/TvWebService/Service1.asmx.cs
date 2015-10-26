using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.IO.Compression;
using System.Text;
using System.Configuration;
using System.Net;
using System.Xml.Serialization;

namespace TvWebService
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {
        List<Programme> _programmes = new List<Programme>();
        public Service1()
        {
            //_programmes = GetDeSeProgrammes();
        }
        private void GetNewXmlTv(object o)
        {
            WebClient wc = new WebClient();

            
            AppSettingsReader asr = new AppSettingsReader();
            string path = asr.GetValue("path", typeof(string)).ToString();

            byte[] byteschan = wc.DownloadData("http://xmltv.tvsajten.com/xmltv/channels.xml.gz");
            string strchanx = UnPack(byteschan);
            XmlDocument xdchan = new XmlDocument();
            xdchan.LoadXml(strchanx);
            string strNewXml = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>\r\n<programmes>\r\n";
            foreach (XmlNode xnchn in xdchan.DocumentElement.SelectNodes("//channel"))
            {
                for (int d = -2; d < 12; d++)
                {
                    DateTime dt = DateTime.Now.AddDays(d);
                    try
                    {
                        byte[] bytes = wc.DownloadData("http://xmltv.tvsajten.com/xmltv/" + xnchn.Attributes["id"].Value + "_" + dt.ToString("yyyy-MM-dd") + ".xml.gz");
                        string strlistx = UnPack(bytes);
                        int istart = strlistx.IndexOf("<programme ");
                        int iend = strlistx.IndexOf("</tv>") - 1;
                        if (istart < iend && istart != -1 && iend != -1)
                        {
                            strlistx = strlistx.Substring(istart, iend - istart);
                            strlistx = strlistx.Replace(" channel", " channelname='" + xnchn.ChildNodes[0].InnerText + "' channel");
                            strNewXml += strlistx + "\r\n";
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            strNewXml += "</programmes>";


            XmlSerializer xs = new XmlSerializer(typeof(List<Programme>));
            MemoryStream ms = new MemoryStream();

            XmlDocument xall = new XmlDocument();
            xall.LoadXml(strNewXml);
            List<Programme> programmes = new List<Programme>();
            foreach(XmlNode xn in xall.DocumentElement.GetElementsByTagName("programme"))
            {
                programmes.Add(new Programme(xn));
            }
            xs.Serialize(ms, programmes);
            File.WriteAllBytes(path, ms.ToArray());
        }

        private List<Programme> GetDeSeProgrammes()
        {
            if (_programmes != null && _programmes.Count > 0)
                return _programmes;

            XmlSerializer xs = new XmlSerializer(typeof(List<Programme>));
            AppSettingsReader asr = new AppSettingsReader();
            List<Programme> prgs =new List<Programme>();
            string path = asr.GetValue("path", typeof(string)).ToString();
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                TimeSpan tsdiff = new TimeSpan(DateTime.Now.Ticks - fi.LastWriteTime.Ticks);

                MemoryStream ms = new MemoryStream(File.ReadAllBytes(path));
                try
                {
                    prgs = (xs.Deserialize(ms) as List<Programme>);
                }
                catch
                { }
                if (prgs == null || prgs.Count == 0 || fi.Length < 1000000 || tsdiff.TotalDays > 5)
                {
                    System.Threading.ThreadPool.QueueUserWorkItem(GetNewXmlTv);
                    //GetNewXmlTv(null);
                }
            }
            else
            {
                System.Threading.ThreadPool.QueueUserWorkItem(GetNewXmlTv);
            }
            _programmes = prgs;
            return prgs;
        }

        [WebMethod]
        public string GetInfo(string p)
        {
            string strret = "";
            if (p.Equals("1234567890qwertyuiop"))
            {
                AppSettingsReader asr = new AppSettingsReader();
                string path = asr.GetValue("path", typeof(string)).ToString();
                strret = path;
                try
                {
                    FileInfo fi = new FileInfo(path);
                    strret += "\nExists:" + fi.Exists.ToString();
                    strret += "\nLen:" + fi.Length.ToString();
                    strret += "\nDate:" + fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm");
                    //string strXml = File.ReadAllText(path);
                    //strret += "\nCanRead:" + strXml.Length.ToString();
                }
                catch (Exception ex)
                {
                    strret += "\n" + ex.Message + " " + ex.StackTrace;
                }
            }
            return strret;
        }
        [WebMethod]
        public string ForceRefresh(string p)
        {
            string strret = "";
            if (p.Equals("1234567890qwertyuiop"))
            {
                AppSettingsReader asr = new AppSettingsReader();
                string path = asr.GetValue("path", typeof(string)).ToString();
                
                try
                {
                    GetNewXmlTv(null);
                }
                catch (Exception ex)
                {
                    strret += "\n" + ex.Message + " " + ex.StackTrace;
                }
            }
            return strret;
        }
        [WebMethod]
        public List<Channel> GetChannels()
        {
            List<Channel> list = new List<Channel>();

            List<Programme> programmes = GetDeSeProgrammes();

            foreach (Programme prg in programmes.Distinct(new ProgrammeComparer()))
            {
                if (!list.Any(x => x.id.Equals(prg.channel)))
                {
                    list.Add(new Channel() { id = prg.channel, name = prg.channelname });
                }
            }
            return list;

        }
        [WebMethod]
        public GetProgrammesResponse GetProgrammes(GetProgrammesRequest request)
        {
            GetProgrammesResponse resp = new GetProgrammesResponse();
            try
            {
                List<Programme> programmes = GetDeSeProgrammes();
                var rpsta = request.programme.start;
                var rpsto = request.programme.stop;

                Programme oldprg = new Programme();
                var list1 = programmes.Where(x => ((string.IsNullOrEmpty(request.programme.channel) ? true : request.programme.channel.Split(';').Contains(x.channel.ToLower())) &&
                    (string.IsNullOrEmpty(request.programme.title) ? true : x.title.ToLower().Contains(request.programme.title.ToLower())) &&
                            (string.IsNullOrEmpty(request.programme.channel) ? true : request.programme.channel.Split(';').Contains(x.channel.ToLower())) &&
                            (string.IsNullOrEmpty(request.programme.subtitle) ? true : x.subtitle.ToLower().Contains(request.programme.subtitle.ToLower())) &&
                            ((request.programme.credits == null || (request.programme.credits != null && request.programme.credits.Count == 0)) ? true : x.credits.Any(y => request.programme.credits.Contains(y))) &&
                            ((request.programme.categories == null || (request.programme.categories != null && request.programme.categories.Count == 0)) ? true : x.categories.Any(y => request.programme.categories.Contains(y))))
                    ).ToList<Programme>();
                var list2 = list1.Where(x => x.stop >= rpsta && x.stop <= rpsto).ToList<Programme>();
                var list3 = list1.Where(x => x.start >= rpsta && x.stop <= rpsto).ToList<Programme>();
                var list4 = list1.Where(x => x.start >= rpsta && x.stop >= rpsto && x.start <= rpsto).ToList<Programme>();
                var list5 = list1.Where(x => x.start <= rpsta && x.stop >= rpsto).ToList<Programme>();
                list1 = list2.Concat(list3).ToList<Programme>();
                list1 = list1.Concat(list4).ToList<Programme>();
                list1 = list1.Concat(list5).ToList<Programme>();
                    

                foreach (Programme prg in list1.Distinct())
                {
                    try
                    {                        
                        if (prg.channel.Equals(oldprg.channel))
                        {
                            if (!prg.start.Equals(oldprg.stop))
                            {
                                Programme prgdiff = new Programme()
                                {
                                    start=oldprg.stop,
                                    stop=prg.start,
                                    title="Sändningsuppehåll!",
                                    channel=prg.channel,
                                    desc = "Uppehåll i sändning!"
                                };
                                resp.programmes.Add(prgdiff);
                            }
                        }
                        resp.programmes.Add(prg);
                        oldprg = CloneProgramme(prg);
                        
                    }
                    catch (Exception ex)
                    {                        
                        resp.exception = ex.Message + " " + ex.StackTrace;
                    }
                }
            }
            catch (Exception ex)
            {
                resp.exception = ex.Message + " " + ex.StackTrace;
            }
            finally {
            
            }
            return resp;
        }

        private Programme CloneProgramme(Programme prg)
        {
            return new Programme()
            {
                categories = prg.categories,
                channel = prg.channel,
                channelname = prg.channelname,
                credits = prg.credits,
                date = prg.date,
                desc = prg.desc,
                episodenum = prg.episodenum,
                start = prg.start,
                stop = prg.stop,
                subtitle = prg.subtitle,
                title = prg.title
            };
        }


        private string UnPack(byte[] bytes)
        {
            string strret = "";
            GZipStream gz = new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress);
            try
            {
                byte[] buffer = new byte[32768];
                int icnt = 0;
                int iread = 0;
                while ((iread=gz.Read(buffer, 0, buffer.Length)) > 0)
                {
                    strret += Encoding.Default.GetString(buffer,0,iread);
                    icnt++;
                }
            }
            catch (InvalidDataException ex)
            {
                strret = Encoding.Default.GetString(bytes);
            }
            return strret.Trim();
        }
    }
    public class Channel
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class GetProgrammesRequest
    {
        public Programme programme { get; set; }
    }

    public class GetProgrammesResponse
    {
        public GetProgrammesResponse()
        {
            programmes = new List<Programme>();
        }
        public List<Programme> programmes { get; set; }
        public string exception { get; set; }
    }
    public class Programme
    {

        public Programme(Programme prg)
            : base()
        {
            credits = prg.credits;
            categories = prg.categories;
            start = prg.start;
            stop = prg.stop;
            channel = prg.channel;
            title = prg.title;
            subtitle = prg.subtitle;
            desc = prg.desc;
            episodenum = prg.episodenum;
        }
        public Programme(XmlNode xn):base()
        {
            try
            {
                credits = new List<Credit>();
                categories = new List<string>();
                start = xn.Attributes["start"] != null ? ParseDate(xn.Attributes["start"].Value) : DateTime.MinValue;
                stop = xn.Attributes["stop"] != null ? ParseDate(xn.Attributes["stop"].Value) : DateTime.MinValue;
                channel = xn.Attributes["channel"] != null ? xn.Attributes["channel"].Value : "";
                channelname = xn.Attributes["channelname"] != null ? xn.Attributes["channelname"].Value : "";
                title = xn.SelectSingleNode("title") != null ? xn.SelectSingleNode("title").InnerText : "";
                subtitle = xn.SelectSingleNode("sub-title") != null ? xn.SelectSingleNode("sub-title").InnerText : "";
                desc = xn.SelectSingleNode("desc") != null ? xn.SelectSingleNode("desc").InnerText : "";
                episodenum = xn.SelectSingleNode("episode-num") != null ? xn.SelectSingleNode("episode-num").InnerText : "";
                if (xn.SelectSingleNode("credits") != null)
                {
                    foreach (XmlNode xnc in xn.SelectSingleNode("credits").ChildNodes)
                    {
                        credits.Add(new Credit(xnc.Name, xnc.InnerText));
                    }
                }
                if (xn.SelectSingleNode("categories") != null)
                {
                    foreach (XmlNode xnc in xn.SelectSingleNode("categories").ChildNodes)
                    {
                        categories.Add(xnc.InnerText);
                    }
                }
            }
            catch (Exception ex)
            {
                string slskdfjsd = "";
            }
        }

        private DateTime ParseDate(string p)
        {
            string strDate = string.Format("{0}-{1}-{2} {3}:{4}:{5}", p.Substring(0, 4), p.Substring(4, 2), p.Substring(6, 2), p.Substring(8, 2), p.Substring(10, 2), p.Substring(12, 2));
            return DateTime.Parse(strDate);
        }
        public Programme()
        {
            credits = new List<Credit>();
            categories = new List<string>();
        }
        public DateTime start { get; set; }
        public DateTime stop { get; set; }
        public string channel { get; set; }
        public string channelname { get; set; }
        public string title { get; set; }
        public string episodenum {get;set;}
        public string subtitle { get; set; }
        public string desc { get; set; }
        public List<Credit> credits { get; set; }
        public string date { get; set; }
        public List<string> categories { get; set; }
    }
    public class Credit
    {
        public Credit(string t, string v)
        {
            type = t;
            value = v;
        }
        public Credit()
        { 
            
        }
        public string type { get; set; }
        public string value { get; set; }
    }
    public class ProgrammeComparer : IEqualityComparer<Programme>
    {

        public bool Equals(Programme x, Programme y)
        {
            return x.channel == y.channel;
        }
        public int GetHashCode(Programme obj)
        {
            return obj.GetHashCode();

        }

    }


}
