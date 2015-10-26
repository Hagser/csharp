using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace MultiUploadSL.Web
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class receiver : IHttpHandler
    {
        string strFilename = "";
        public void ProcessRequest(HttpContext context)
        {
            string uha = context.Request.UserHostAddress;
            string filename = context.Request.QueryString["filename"].ToString();
            string name = context.Request.QueryString["name"].ToString();
            string address = context.Request.QueryString["address"].ToString();
            string email = context.Request.QueryString["email"].ToString();
            string golfid = context.Request.QueryString["golfid"].ToString();
            strFilename = context.Server.MapPath("~/App_Data/" + System.DateTime.Now.Ticks.ToString() + "_" + uha + "-" + golfid + "-" + email + "-" + name + "-" + address + "-" + filename);
            try
            {
                //strFilename = context.Server.MapPath("~/App_Data/" + System.DateTime.Now.Ticks.ToString() + "_" + filename);
                using (FileStream fs = File.Create(strFilename))
                {
                    SaveFile(context.Request.InputStream, fs);
                }
            }
            catch{}
        }
        private void SaveFile(Stream stream, FileStream fs)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, bytesRead);
            }
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
