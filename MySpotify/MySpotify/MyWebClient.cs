using System.IO;
using System.Net;

namespace MySpotify
{

    public class MyWebClient
    {
        public static string UploadString(HttpWebRequest request, string Data, CookieCollection Cookies)
        {
            string ret = "";
            request.CookieContainer = new CookieContainer();
            foreach (Cookie cookie in Cookies)
                request.CookieContainer.Add(cookie);

            //request.Headers = Headers;
            HttpWebResponse res;
            request.Method = "POST";

            //request.ContentLength = Data.Length;
            using (Stream s = request.GetRequestStream())
            {
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(Data);
                    //s.Close();
                    sw.Close();
                }
            }
            ret = GetString(request, out res);

            return ret;
        }

        public static string DownloadString(string Url, out HttpWebResponse Response)
        {
            return GetString(Url, out Response);
        }
        private static string GetString(string Url, out HttpWebResponse Response)
        {
            return GetString(WebRequest.Create(Url) as HttpWebRequest, out Response);
        }
        private static string GetString(HttpWebRequest rq, out HttpWebResponse Response)
        {
            string ret = "";
            Response = GetUrl(rq);
            Response.Cookies = new CookieCollection();
            if (Response.Headers["Set-Cookie"] != null)
            {
                foreach (string str in Response.Headers["Set-Cookie"].Split('/'))
                {
                    if (str != "")
                    {
                        string[] cookie = str.Trim().Split('=');
                        if (cookie.Length > 1)
                        {
                            string name = cookie[0].Replace(",", "").Trim();
                            string value = cookie[1].Trim().Split(';')[0];
                            Response.Cookies.Add(new Cookie(name, value, "/", rq.RequestUri.Host));
                        }
                    }
                }
            }

            using (Stream s = Response.GetResponseStream())
            using (StreamReader sr = new StreamReader(s))
                ret = sr.ReadToEnd();
            return ret;
        }
        private static HttpWebResponse GetUrl(HttpWebRequest rq)
        {
            return rq.GetResponse() as HttpWebResponse;
        }

    }
}
