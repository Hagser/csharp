using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace MyPhotoSlideshow.Web
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DownloadPhoto : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["path"] != null)
            {
                string strPath = context.Request.QueryString["path"].ToString();
                string strSize = context.Request.QueryString["size"].ToString();
                if (strPath.StartsWith("file-"))
                {
                    strPath = strPath.Replace("file-", "");
                    strPath = GoodStuff.Convertion.FromBase64(strPath);
                    if (File.Exists(strPath))
                    {
                        
                        if (strSize.Equals("small"))
                        {
                            if (!File.Exists(strPath.Replace(".jpg", "_s.jpg").Replace(".JPG", "_s.JPG")))
                            {
                                Image imag = Bitmap.FromFile(strPath);
                                Image img = ScaleByPercentThumb(RotateImage(imag), 5);
                                foreach (PropertyItem pi in imag.PropertyItems)
                                {
                                    img.SetPropertyItem(pi);
                                }
                                strPath = strPath.Replace(".jpg", "_s.jpg").Replace(".JPG", "_s.JPG");
                                img.Save(strPath, ImageFormat.Jpeg);
                                img.Dispose();
                            }
                            else
                            {
                                strPath = strPath.Replace(".jpg", "_s.jpg").Replace(".JPG", "_s.JPG");
                            }
                        }
                        else if (strSize.Equals("med"))
                        {
                            if (!File.Exists(strPath.Replace(".jpg", "_m.jpg").Replace(".JPG", "_m.JPG")))
                            {
                                Image imag = Bitmap.FromFile(strPath);
                                Image img = ScaleByPercent(RotateImage(imag), 25);
                                foreach (PropertyItem pi in imag.PropertyItems)
                                {
                                    img.SetPropertyItem(pi);
                                }
                                strPath = strPath.Replace(".jpg", "_m.jpg").Replace(".JPG", "_m.JPG");
                                img.Save(strPath, ImageFormat.Jpeg);
                                img.Dispose();
                            }
                            else
                            {
                                strPath = strPath.Replace(".jpg", "_m.jpg").Replace(".JPG", "_m.JPG");
                            }
                        }
                        context.Response.ContentType = "image/jpeg";
                        context.Response.WriteFile(strPath);
                        if (strSize.Equals("small") || strSize.Equals("med"))
                        {
                            //File.Delete(strPath);
                        }
                    }
                }
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("Error");
            }
        }

        private Image RotateImage(Image img)
        {
            foreach (PropertyItem pi in img.PropertyItems)
            {
                //0x0112
                int ix = 0x112;
                if (pi.Id == ix)
                {
                    string s = pi.Value.Length > 0 ? pi.Value[0].ToString() : "";
                    /*
 2) transform="-flip horizontal";;
 3) transform="-rotate 180";;
 4) transform="-flip vertical";;
 5) transform="-transpose";;
 6) transform="-rotate 90";;
 7) transform="-transverse";;
 8) transform="-rotate 270";;                     
                     
                     */
                    if (s.Equals("1"))
                        //img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    if (s.Equals("2"))
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    if (s.Equals("3"))
                        img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    if (s.Equals("4"))
                        img.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    if (s.Equals("5"))
                        img.RotateFlip(RotateFlipType.Rotate90FlipX);
                    if (s.Equals("6"))
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    if (s.Equals("7"))
                        img.RotateFlip(RotateFlipType.Rotate90FlipY);
                    if (s.Equals("8"))
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                }
            }
            return img;
        }
        static Image ScaleByPercent(Image imgPhoto, int Percent)
        {
            float nPercent = ((float)Percent / 100);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight,
                                     PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                                    imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
        static Image ScaleByPercentThumb(Image imgPhoto, int Percent)
        {
            float nPercent = ((float)Percent / 100);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(75, 75,
                                     PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                                    imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            //grPhoto.SetClip(new Rectangle(5, 5, 75, 75));
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, 75, 75),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
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
