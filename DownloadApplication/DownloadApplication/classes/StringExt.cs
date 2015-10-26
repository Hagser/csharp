using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyDownloadApplication
{
    public static class StringExt
    {

        public static string ReplaceRx(this string s, string pattern, string replacement)
        {
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(pattern);
            return r.Replace(s, replacement);
        }


        /// <summary>
        /// Replace all params with same value
        /// </summary>
        /// <param name="str">this string</param>
        /// <param name="replacewith">what to replace with</param>
        /// <param name="p">things to find</param>
        /// <returns></returns>
        public static string ReplaceAll(this string str, string replacewith, params string[] p)
        {
            string ret = str;
            foreach (string s in p)
            {
                ret = ret.Replace(s, replacewith);
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="find1"></param>
        /// <param name="find2"></param>
        /// <param name="removepresuf"></param>
        /// <returns></returns>
        public static string Substring(this string str, string find1, string find2, bool removebetween)
        {
            string strRet = "";
            int istart = str.IndexOf(find1);
            int iend = str.IndexOf(find2, istart + 1, StringComparison.InvariantCultureIgnoreCase);
            if (istart > -1 && istart < iend)
            {
                strRet = str.Substring(istart, iend - istart);

                if (removebetween)
                {
                    strRet = str.Remove(istart, (iend - istart) + find2.Length);
                }
            }
            return strRet;
        }
        public static string Substring(this string str, string find1, string find2)
        {
            return str.Substring(find1, find2, false);
        }
    }

}
