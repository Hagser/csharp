<%@ WebHandler Language="C#" Class="sender" %>

using System;
using System.IO;
using System.Web;
using System.Xml;

public class sender : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        if (context.Request.QueryString["filename"] == null)
        {
            context.Response.StatusCode = 404;
            return;
        }
        string filename = context.Request.QueryString["filename"].ToString();
        if (context.Request.QueryString["mime"] == null)
        {
            context.Response.ContentType = "application/octet-stream";
        }
        else
        {
            string strMime = "application/octet-stream";
            if (filename.ToLower().EndsWith("pdf"))
            {
                strMime = "application/pdf";
            }
            else if (filename.ToLower().EndsWith("xlsx"))
            {
                strMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            else if (filename.ToLower().EndsWith("xml"))
            {
                strMime = "text/xml";
            }
            else if (filename.ToLower().EndsWith("jpg"))
            {
                strMime = "image/jpeg";
            }
            else if (filename.ToLower().EndsWith("png"))
            {
                strMime = "image/png";
            }

            context.Response.ContentType = strMime;
        }

        string path = getPath();
        string strFilename = path + "\\" + filename;
        
        string meth = context.Request.HttpMethod.ToLower();
        string url = context.Request.RawUrl;
        
        if (!filename.StartsWith("v_"))
        {
            if (meth.Equals("options"))
            {
                context.Response.StatusCode = 207;
                context.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<D:multistatus xmlns:D=\"DAV:\"><D:response><D:href>" + url + "</D:href></D:response></D:multistatus>");
                return;
            }
            if (meth.Equals("propfind"))
            {
                context.Response.StatusCode = 207;
                Stream s = context.Request.InputStream;
                if (s.Length > 0)
                {
                    XmlDocument xd = new XmlDocument();
                    xd.Load(s);
                    XmlNode xn = xd.GetElementsByTagName("prop")[0];
                    XmlNode xna = xd.GetElementsByTagName("allprop")[0];
                    XmlNode xnp = xd.GetElementsByTagName("propname")[0];
                    string strXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<D:multistatus xmlns:D=\"DAV:\"><D:response><D:href>" + url + "</D:href>";
                    if (xna != null || xnp != null)
                    {
                        strXml += "<D:propstat>\n" +
                       "<D:prop xmlns:R=\"" + url + "\">\n" +
                            "<R:bigbox/>\n" +
                            "<R:author/>\n" +
                            "<D:creationdate/>\n" +
                            "<D:displayname/>\n" +
                            "<D:resourcetype/>\n" +
                            "<D:supportedlock/>\n" +
                       "</D:prop>\n" +
                       "</D:propstat>\n<D:status>HTTP/1.1 200 OK</D:status>";
                    }
                    else if (xn != null)
                    {
                        strXml += "<D:propstat>\n" +
                       "<D:prop xmlns:R=\"" + url + "\">\n";
                        foreach (XmlNode xcn in xn.ChildNodes)
                        {
                            strXml += "<D:" + xcn.Name + "/>\n";
                        }
                        strXml += "</D:prop>\n" +
                       "</D:propstat>\n<D:status>HTTP/1.1 200 OK</D:status>";
                    }

                    context.Response.Write(strXml + "</D:response></D:multistatus>");
                }
                return;
            }
            if (meth.Equals("lock"))
            {
                context.Response.StatusCode = 200;

                Stream s = context.Request.InputStream;
                if (s.Length > 0)
                {
                    XmlDocument xd = new XmlDocument();
                    xd.Load(s);
                    XmlNodeList xnlsl = xd.GetElementsByTagName("lockscope");
                    XmlNodeList xnltl = xd.GetElementsByTagName("locktype");
                    XmlNodeList xnlol = xd.GetElementsByTagName("owner");
                    string locktype = "";
                    string lockscope = "";
                    string owner = "";
                    string timeout = "Second-604800";
                    string depth = "Infinity";
                    if (context.Request.Headers["Timeout"] != null)
                    {
                        timeout = context.Request.Headers["Timeout"];
                    }
                    if (context.Request.Headers["Depth"] != null)
                    {
                        depth = context.Request.Headers["Depth"];
                    }
                    if (xnlsl != null && xnlsl.Count > 0)
                    {
                        if (xnlsl[0].FirstChild != null)
                        {
                            lockscope = xnlsl[0].FirstChild.Name;
                        }
                    }

                    if (xnltl != null && xnltl.Count > 0)
                    {
                        if (xnltl[0].FirstChild != null)
                        {
                            locktype = xnltl[0].FirstChild.Name;
                        }
                    }

                    if (xnlol != null && xnlol.Count > 0)
                    {
                        if (xnlol[0].FirstChild != null)
                        {
                            if (xnlol[0].FirstChild.FirstChild != null)
                            {
                                owner = "<D:" + xnlol[0].FirstChild.FirstChild.Name + ">" + xnlol[0].FirstChild.FirstChild.Value + "</D:" + xnlol[0].FirstChild.FirstChild.Name + ">";
                            }
                            else
                            {
                                owner = xnlol[0].FirstChild.Value;
                            }
                        }
                    }
                    string strXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<D:prop xmlns:D=\"DAV:\">\n" +
                    "     <D:lockdiscovery>\n" +
                    "          <D:activelock>\n" +
                    "               <D:locktype><D:" + locktype + "/></D:locktype>\n" +
                    "               <D:lockscope><D:" + lockscope + "/></D:lockscope>\n" +
                    "               <D:depth>" + depth + "</D:depth>\n" +
                    "               <D:owner>" + owner + "</D:owner>\n" +
                    "               <D:timeout>" + timeout + "</D:timeout>\n" +
                        /*"               <D:locktoken>\n" +
                        "                    <D:href>\n" +
                        "               opaquelocktoken:e71d4fae-5dec-22d6-fea5-00a0c91e6be4\n" +
                        "                    </D:href>\n" +
                        "               </D:locktoken>\n" +*/
                    "          </D:activelock>\n" +
                    "     </D:lockdiscovery>\n" +
                    "</D:prop>";

                    context.Response.Write(strXml);
                }
                return;
            }
            if (meth.Equals("put"))
            {
                if (File.Exists(strFilename))
                {
                    int icnt = 0;
                    foreach (string strfile in Directory.GetFiles(getPath(), "v_*" + filename, SearchOption.TopDirectoryOnly))
                    {
                        string name = strfile.Replace(path, "").Replace(filename, "").Replace("v_", "");
                        icnt = Math.Max(int.Parse(name), icnt);
                    }
                    strFilename = getPath() + "\\" + "v_" + (icnt + 1) + filename;
                }
                using (FileStream fs = File.Create(strFilename))
                {
                    SaveFile(context.Request.InputStream, fs);
                }
                context.Response.StatusCode = 200;
                return;
            }
        }
        if (meth.Equals("head") || meth.Equals("get"))
        {
            try
            {
                if (File.Exists(strFilename))
                {
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);

                    int icnt = 0;
                    foreach (string strfile in Directory.GetFiles(getPath(), "v_*" + filename, SearchOption.TopDirectoryOnly))
                    {
                        string name = strfile.Replace(path, "").Replace(filename, "").Replace("v_", "");
                        icnt = Math.Max(int.Parse(name), icnt);
                    }
                    if (icnt > 0)
                    {
                        strFilename = getPath() + "\\" + "v_" + icnt + filename;
                    }
                    context.Response.AppendHeader("Content-Length", (new FileInfo(strFilename)).Length.ToString());
                    if (meth.Equals("get"))
                    {
                        context.Response.WriteFile(strFilename);
                    }
                }
                else
                {
                    context.Response.StatusCode = 404;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message + "\n" + ex.StackTrace);
                context.Response.StatusCode = 500;
            }
            return;
        }
    }

    private string getPath()
    {
        System.Configuration.AppSettingsReader asr = new System.Configuration.AppSettingsReader();
        string path = asr.GetValue("path", typeof(string)) as string;
        return path;
    }

    private void SaveFile(Stream stream, FileStream fs)
    {
        byte[] buffer = new byte[32768];
        int bytesRead;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            fs.Write(buffer, 0, bytesRead);
        }
    }
    
    private void WriteFile(Stream stream, FileStream fs)
    {
        byte[] buffer = new byte[32768];
        int bytesRead;
        while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) != 0)
        {
            stream.Write(buffer, 0, bytesRead);
        }
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}