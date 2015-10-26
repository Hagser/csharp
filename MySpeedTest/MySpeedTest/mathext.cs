using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpeedTest
{
    public static class mathext
    {
        /// <summary>
        /// Rounds a double with zero decimals.
        /// </summary>
        /// <param name="d"></param>
        /// <returns>Rounded double</returns>
        public static int Int(this double d)
        {
            return int.Parse(d.Round().ToString());
        }

        /// <summary>
        /// Rounds a double with zero decimals.
        /// </summary>
        /// <param name="d"></param>
        /// <returns>Rounded double</returns>
        public static double Round(this double d)
        {
            return Math.Round(d, 0);
        }

        /// <summary>
        /// Rounds a double with p decimals.
        /// </summary>
        /// <param name="d"></param>
        /// <returns>Rounded double</returns>
        public static double Round(this double d,int p)
        {
            return Math.Round(d, p);
        }
        /// <summary>
        /// Removes number right of decimal.
        /// </summary>
        /// <param name="d"></param>
        /// <returns>Cut double</returns>
        public static double Left(this double d)
        {
            string cds = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            if (d.ToString().Contains(cds))
            { 
                string[] sd = d.ToString().Split(cds.ToCharArray());
                //double dl = double.Parse("0" + cds + sd[1]);
                double dl = double.Parse(sd[0]);
                return dl;
            }
            return d;

        }

        /// <summary>
        /// Removes number left of decimal.
        /// </summary>
        /// <param name="d"></param>
        /// <returns>Cut double</returns>
        public static double Right(this double d)
        {
            string cds = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            if (d.ToString().Contains(cds))
            {
                string[] sd = d.ToString().Split(cds.ToCharArray());
                double dl = double.Parse("0" + cds + sd[1]);
                //double dl = double.Parse(sd[0]);
                return dl;
            }
            return d;

        }
        /// <summary>
        /// Floors a double.
        /// </summary>
        /// <param name="d"></param>
        /// <returns>Floored double</returns>
        public static double Floor(this double d)
        {
            return Math.Floor(d);
        }

        /// <summary>
        /// Ceilings a double.
        /// </summary>
        /// <param name="d"></param>
        /// <returns>Ceilinged double</returns>
        public static double Ceiling(this double d)
        {
            return Math.Ceiling(d);
        }
    }
}
