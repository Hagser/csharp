using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace MyPhotoInfo
{
    static class ImageStuff
    {

        public static Image ScaleByPercent(Image imgPhoto, int Percent)
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
        public static Image RotateImage(Image img)
        {
            int ix = 0x112;
            foreach (PropertyItem pi in img.PropertyItems.Where(x=>x.Id==ix))
            {
                string s = pi.Value.Length > 0 ? pi.Value[0].ToString() : "";

                //if (s.Equals("1"))
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
            return img;
        }
        public static Image RotateThumbImage(Image img)
        {
            int ix = 20521;
            foreach (PropertyItem pi in img.PropertyItems.Where(x => x.Id == ix))
            {
                string s = pi.Value.Length > 0 ? pi.Value[0].ToString() : "";

                //if (s.Equals("1"))
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
            return img;
        }
    }
}
