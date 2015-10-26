using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Drawing.Imaging;

namespace MyPhotoInfo
{
    class PhotoFileInfo
    {
        public PhotoFileInfo(FileInfo fileInfo)
        { 
        //20507
            Name = fileInfo.Name;
            Size = fileInfo.Length;
            CreationTime = fileInfo.CreationTime;
            Thumbnail = FixThumbnail(fileInfo.FullName);
        }

        private Image FixThumbnail(string strfilename)
        {
            Image img = Bitmap.FromFile(strfilename);
            if (img.PropertyItems.Any(x => x.Id == 20507))
            {
                PropertyItem pi = img.PropertyItems.Where(x => x.Id == 20507).FirstOrDefault();
                byte[] bytes = pi.Value;

                MemoryStream stream = new MemoryStream(bytes.Length);
                stream.Write(bytes, 0, bytes.Length);

                return ImageStuff.ScaleByPercent(ImageStuff.RotateThumbImage(Image.FromStream(stream)), 50);
            }
            return null;
        }
        public Image Thumbnail { get; private set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime CreationTime { get; set; }


        
    }
}
