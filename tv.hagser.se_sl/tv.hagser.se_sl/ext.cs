using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace tv.hagser.se_sl
{
    public static class ienumext
    {
        public static string Flatten<T>(this List<T> list)
        {
            string strRet = "";
            foreach (T t in list)
            {
                strRet += t.ToString() + ";";
            }
            strRet = strRet.EndsWith(";") ? strRet.Substring(0, strRet.Length - 1) : strRet;
            return strRet;
        }
    }
}
