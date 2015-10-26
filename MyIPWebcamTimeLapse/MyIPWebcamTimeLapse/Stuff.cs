using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MyIPWebcamTimeLapse
{
    public static class Stuff
    {
        public static RotateFlipType getRotateFlipType(float p)
        {
            switch (p.ToString())
            {
                case "90": return RotateFlipType.Rotate90FlipNone;
                case "180": return RotateFlipType.Rotate180FlipNone;
                case "270": return RotateFlipType.Rotate270FlipNone;
            }
            return RotateFlipType.RotateNoneFlipNone;
        }
    }
}
