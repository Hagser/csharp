
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPhotoInfo
{
    public static class ext
    {
        public static int ToInt(this double dbl)
        {
            return int.Parse(Math.Round(dbl, 0).ToString());
        }
        public static double ToDouble(this int val)
        {
            return double.Parse(val.ToString());
        }
    }
}
