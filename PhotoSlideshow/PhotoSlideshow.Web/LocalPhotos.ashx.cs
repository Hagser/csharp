using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.IO;

namespace PhotoSlideshow.Web
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LocalPhotos : IHttpHandler
    {
        AppSettingsReader asr = new AppSettingsReader();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string strPath = asr.GetValue("photoRoot", typeof(string)).ToString();

            if (context.Request.QueryString["month"] != null)
            {
                string strMonth = context.Request.QueryString["month"].ToString();
                DateTime dtMonth = DateTime.Parse(strMonth);
                strPath += dtMonth.Year + "\\" + dtMonth.Month.ToString().PadLeft(2, (char)48);
            }
            string strRet = "";
            if (Directory.Exists(strPath))
            {
                foreach (FileInfo fi in new DirectoryInfo(strPath).GetFiles("*.jpg", SearchOption.AllDirectories))
                {
                    if (!fi.DirectoryName.ToLower().Contains("org") && !fi.Name.ToLower().EndsWith("_m.jpg") && !fi.Name.ToLower().EndsWith("_s.jpg"))
                    {
                        strRet += "file-" + GoodStuff.Convertion.ToBase64(fi.FullName) + ";";// +fi.LastAccessTime.ToString("yyyy-MM-dd") + "|";
                    }
                }
                if (!string.IsNullOrEmpty(strRet))
                {
                    strRet = strRet.Substring(0, strRet.Length - 1);
                }
            }
            context.Response.Write(strRet);
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
