using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Reflection;

namespace MySpeedTest
{
    public class bksettings
    {
        public bksettings()
        { }
        public bksettings(SettingServer ss)
        {
            reload(ss);
        }
        public void reload(SettingServer ss)
        {
            XmlDocument xd = new XmlDocument();
            string[] urls = { ss.serverurl, ss.configurl };
            foreach (string url in urls)
            {
                try
                {
                    WebClient wc = new WebClient();
                    //wc.Headers.Add("Connection: keep-alive");
                    wc.Headers.Add("User-Agent: Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.63 Safari/535.7");
                    wc.Headers.Add("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                    //wc.Headers.Add("Accept-Encoding: gzip,deflate,sdch");
                    wc.Headers.Add("Accept-Language: sv-SE,sv;q=0.8,en-US;q=0.6,en;q=0.4");
                    wc.Headers.Add("Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.3");
                    wc.Headers.Add("Cookie: stnetsid=143fbb57965bf491cb54cd3fe99c579b; ip_addresses=1607562521%2C1334492615%2C1607581380%2C1832561699%2C1607553702%2C1306196242%2C1832578173%2C1832567539%2C1334501197%2C1607547521%2C1607581091%2C1334497589%2C1832528682%2C1607545688%2C1832574465%2C1519204042%2C1607559989%2C1519203981%2C1832519925%2C1832580514%2C1832539683%2C1519203750%2C1519203198%2C1832545907%2C1519202764%2C1607538350%2C1832520568%2C1334489887%2C1832579905%2C1519200207%2C1832528448%2C1607559974%2C1832520096%2C1607555046%2C1832579579%2C1519200619%2C3566735255");

                    wc.Encoding = Encoding.UTF8;
                    string strXml = wc.DownloadString(url);
                    xd.LoadXml(strXml);
                    XmlElement xe = xd.DocumentElement;
                    foreach (PropertyInfo pi in this.GetType().GetProperties().Where(x => x.PropertyType == typeof(string)))
                    {

                        XmlNode xn = xd.SelectSingleNode("//settings/" + pi.Name);
                        if (xn != null)
                        {
                            pi.SetValue(this, xn.InnerText, null);
                        }
                    }
                    XmlNode xnc = xd.SelectSingleNode("//settings/client");
                    if (xnc != null)
                    {
                        this.client = new Client();
                        foreach (PropertyInfo pi in this.client.GetType().GetProperties().Where(x => x.PropertyType == typeof(string)))
                        {
                            XmlAttribute xatt = xnc.Attributes[pi.Name];
                            if (xatt != null)
                            {
                                pi.SetValue(this.client, xatt.Value, null);
                            }
                        }
                    }
                    if (xd.SelectNodes("//settings/servers/server").Count > 0)
                    {
                        servers = new List<Server>();
                        foreach (XmlNode xn in xd.SelectNodes("//settings/servers/server"))
                        {
                            Server server = new Server(xn);
                            if (!servers.Any(x => x.name.Equals(server.name)))
                                servers.Add(server);
                        }
                    }
                }
                catch { }
            }
            if (SettingsRefreshed != null)
                SettingsRefreshed.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler SettingsRefreshed;
        public string licensekey { get; set; }
        public string customer { get; set; }
        public string hashkey { get; set; }
        public string previsp { get; set; }
        public string ispname { get { return string.IsNullOrEmpty(_ispname)?client!=null?client.isp:"":_ispname;} set { _ispname=value;} }
        private string _ispname { get; set; }
        public List<Server> servers { get; set; }
        public Client client { get; set; }
        public List<Server> ClosestServers {
            get {
                return servers.OrderBy(x => ((Math.Max(x.dlat, client.dlat) - Math.Min(x.dlat, client.dlat)) + (Math.Max(x.dlon, client.dlon) - Math.Min(x.dlon, client.dlon)))).ToList();
            }
        }
    }
    public class Client
    {
        public string ip { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public double dlat { get { return !string.IsNullOrEmpty(lat) ? double.Parse(lat.Replace(".", ",")) : 0; } }
        public double dlon { get { return !string.IsNullOrEmpty(lon) ? double.Parse(lon.Replace(".", ",")) : 0; } }
        public string isp { get; set; }
        public string isprating { get; set; }

    }
    public class Server
    {
        public Server(XmlNode xn)
        {
            foreach (PropertyInfo pi in this.GetType().GetProperties().Where(x => x.PropertyType == typeof(string)))
            {
                XmlAttribute xa = xn.Attributes[pi.Name];
                if (xa != null)
                {
                    string strval = xa.Value;
                    if (pi.Name.Equals("lat") || pi.Name.Equals("lon"))
                    {
                        if (strval.Equals("1065.5839"))
                        {
                            strval = "65.5839";
                        }
                        else if (strval.Equals("1022.1632"))
                        {
                            strval = "22.1632";
                        }
                        pi.SetValue(this, strval, null);
                    }
                    else
                    {
                        pi.SetValue(this, xa.Value, null);
                    }
                }
            }
        }
        public string downurl { get{return url.Replace("upload.php","");} }
        public string url { get; set; }
        public string url2 { get; set; }
        public string url3 { get; set; }
        public string url4 { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public double dlat { get { return !string.IsNullOrEmpty(lat) ? double.Parse(lat.Replace(".", ",")) : 0; } }
        public double dlon { get { return !string.IsNullOrEmpty(lon) ? double.Parse(lon.Replace(".", ",")) : 0; } }
        public string name { get; set; }
        public string visibility { get; set; }
        public string id { get; set; }
    }
    public class SettingServer
    {
        public string configurl { get { return _configurl + (new Random().Next()).ToString(); } set { _configurl = value; } }
        public string serverurl { get { return _serverurl + (new Random().Next()).ToString(); } set { _serverurl = value; } }
        public string name { get; set; }
        private string _configurl;
        private string _serverurl;
    }
}
