<%@ WebHandler Language="C#" Class="receiver" %>

using System;
using System.IO;
using System.Web;

public class receiver : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        string filename = context.Request.QueryString["filename"] != null ? context.Request.QueryString["filename"].ToString() : "";
        Stream stream = context.Request.InputStream;
        if (context.Request.Files != null && context.Request.Files.Count > 0)
        {
            filename = context.Request.Files[0].FileName;
            string[] filenames=filename.Split('\\');
            filename = filenames[filenames.Length - 1];
            stream = context.Request.Files[0].InputStream;
        }
        context.Response.Buffer = false;
        if (!string.IsNullOrEmpty(filename))
        {
            string strFilename = getPath() + "\\" + filename;
            try
            {
                if (File.Exists(strFilename))
                {
                    int icnt = 0;
                    foreach (string strfile in Directory.GetFiles(getPath(), "v_*" + filename, SearchOption.TopDirectoryOnly))
                    {
                        string name = strfile.Replace(getPath(), "").Replace(filename, "").Replace("v_", "");
                        icnt = Math.Max(int.Parse(name), icnt);
                    }
                    strFilename = getPath() + "\\" + "v_" + (icnt + 1) + filename;
                }
                context.Response.Write("<script type='text/javascript'>\n");
                using (FileStream fs = File.Create(strFilename))
                {
                    SaveFile(stream, fs, context);
                }
                //context.Response.Write("parent.document.location.reload();\n");
                context.Response.Write("</script>");

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Status = ex.Message + "\n" + ex.StackTrace;
            }
        }
        
    }

    private string getPath()
    {
        System.Configuration.AppSettingsReader asr = new System.Configuration.AppSettingsReader();
        string path = asr.GetValue("path", typeof(string)) as string;
        return path;
    }
    
    private void SaveFile(Stream stream, FileStream fs,HttpContext context)
    {
        byte[] buffer = new byte[32768];
        int bytesRead;
        int totbytesread = 0;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            fs.Write(buffer, 0, bytesRead);
            totbytesread += bytesRead;
            context.Response.Write("parent.setBytes('" + totbytesread + "','"+ stream.Length +"');\n");
            context.Response.Flush();
        }
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}