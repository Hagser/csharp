using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace MyPhotoFaceTagger
{
    public class trainedFace
    {
        public trainedFace()
        { }
        public string path { get; set; }
        public Rectangle rect { get; set; }
        public string name { get; set; }
        [NonSerialized]
        public Image<Gray, byte> img;
    }
}
