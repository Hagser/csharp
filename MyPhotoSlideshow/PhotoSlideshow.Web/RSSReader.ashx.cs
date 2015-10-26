using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Net;
using System.IO;

namespace MyPhotoSlideshow.Web
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class RSSReader : IHttpHandler
    {


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (context.Request.QueryString["rssurl"]!=null)
            {
                string strUri = context.Request.QueryString["rssurl"].ToString();
                Uri uri = new Uri(strUri);

                WebRequest wreq = WebRequest.Create(uri);
                wreq.Method = "GET";
                WebResponse wres = wreq.GetResponse();
                Stream stream = wres.GetResponseStream();

                string strXml = "";
                StreamReader sr = new StreamReader(stream);
                strXml = sr.ReadToEnd();
                strXml = loadXml(strXml);
                //strXml = GoodStuff.Convertion.ToBase64(strXml);
                context.Response.Write(strXml);

            }
            else
            {
                context.Response.Write("What?");
            }
        }

        private string loadXml(string strXml)
        {
            string strRet = "";
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml(strXml);
                XmlNode xnstat = xd.SelectSingleNode("rsp");
                if (xnstat != null && getAttribute(xnstat, "stat").ToLower().Equals("ok"))
                {
                    foreach (XmlNode xn in xd.SelectNodes("rsp/photos/photo"))
                    {
                        if (xn != null)
                        {
                            string id = getAttribute(xn, "id");
                            string owner = getAttribute(xn, "owner");
                            string secret = getAttribute(xn, "secret");
                            string server = getAttribute(xn, "server");
                            string farm = getAttribute(xn, "farm");
                            string title = getAttribute(xn, "title");
                            strRet += String.Format("{0}#{1}#{2}#{3}#{4}#{5};", id, owner, secret, server, farm, title);
                        }
                    }

                    strRet = strRet.Substring(0, strRet.Length - 1);
                }
                else
                { 
                
                }
            }
            catch (Exception ex)
            { 
                
            }
            return strRet;
        }

        private string getAttribute(XmlNode xn, string p)
        {
            foreach (XmlAttribute att in xn.Attributes)
            {
                if (att.Name.ToLower().Equals(p))
                {
                    return att.Value;
                }
            }
            return "";
        }

        private string fixInstructions(string p)
        {
            int istart = 0;
            int iend = 0;
            while (p.IndexOf("<") != -1)
            {
                istart = p.IndexOf("<");
                iend = p.IndexOf(">", istart + 1);
                p = p.Remove(istart, (iend - istart) + 1);
            }
            return p.Replace("&nbsp;", " ");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
