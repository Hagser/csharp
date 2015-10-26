using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ClipboardCatcher
{
    class Convertion
    {
        /// <summary>
        /// Returns char array from byte array.
        /// </summary>
        /// <param name="sBytes">byte array</param>
        /// <returns>char array</returns>
        public static char[] ToCharArray(byte[] sBytes)
        {
            return Encoding.Default.GetChars(sBytes);
        }
        /// <summary>
        /// /// Returns char array from string.
        /// </summary>
        /// <param name="sString">string</param>
        /// <returns>char array</returns>
        public static char[] ToCharArray(string sString)
        {
            return Encoding.Default.GetChars(ToByteArray(sString));
        }

        /// <summary>
        /// Returns byte array from char array.
        /// </summary>
        /// <param name="sChars">char array</param>
        /// <returns>byte array</returns>
        public static byte[] ToByteArray(char[] sChars)
        {
            return Encoding.Default.GetBytes(sChars);
        }
        /// <summary>
        /// Returns byte array from string.
        /// </summary>
        /// <param name="sString">string</param>
        /// <returns>byte array</returns>
        public static byte[] ToByteArray(string sString)
        {
            return Encoding.Default.GetBytes(sString);
        }

        /// <summary>
        /// Returns string from char array.
        /// </summary>
        /// <param name="sChars">char array</param>
        /// <returns>string</returns>
        public static string ToString(char[] sChars)
        {
            return Encoding.Default.GetString(ToByteArray(sChars));
        }
        /// <summary>
        /// Returns string from byte array.
        /// </summary>
        /// <param name="sBytes">byte array</param>
        /// <returns>string</returns>
        public static string ToString(byte[] sBytes)
        {
            return Encoding.Default.GetString(sBytes);
        }

        /// <summary>
        /// Returns string from Base 64 string.
        /// </summary>
        /// <param name="sString">string</param>
        /// <returns>Base 64 string</returns>
        public static string FromBase64(string sString)
        {
            byte[] sBytes = Convert.FromBase64String(sString);
            return ToString(sBytes);
        }
        /// <summary>
        /// Returns string from Base 64 byte array.
        /// </summary>
        /// <param name="sString_bytes">byte array</param>
        /// <returns>Base 64 string</returns>
        public static string FromBase64(byte[] sString_bytes)
        {
            byte[] sBytes = Convert.FromBase64String(ToString(sString_bytes));
            return ToString(sBytes);
        }
        /// <summary>
        /// Returns string from Base 64 char array.
        /// </summary>
        /// <param name="sString_chars">char array</param>
        /// <returns>Base 64 string</returns>
        public static string FromBase64(char[] sString_chars)
        {
            byte[] sBytes = Convert.FromBase64String(ToString(sString_chars));
            return ToString(sBytes);
        }

        /// <summary>
        /// Returns byte array from Base 64 string.
        /// </summary>
        /// <param name="sString">Base 64 string</param>
        /// <returns>byte array</returns>
        public static byte[] FromBase64ToBytes(string sString)
        {
            byte[] sBytes = Convert.FromBase64String(sString);
            return sBytes;
        }
        /// <summary>
        /// Returns byte array from Base 64 byte array.
        /// </summary>
        /// <param name="sString_bytes">Base 64 byte array</param>
        /// <returns>byte array</returns>
        public static byte[] FromBase64ToBytes(byte[] sString_bytes)
        {
            string sString = ToString(sString_bytes);
            return FromBase64ToBytes(sString);
        }
        /// <summary>
        /// Returns byte array from Base 64 char array.
        /// </summary>
        /// <param name="sString_chars">Base 64 char array</param>
        /// <returns>byte array</returns>
        public static byte[] FromBase64ToBytes(char[] sString_chars)
        {
            string sString = ToString(sString_chars);
            return FromBase64ToBytes(sString);
        }

        /// <summary>
        /// Returns Base 64 string from string.
        /// </summary>
        /// <param name="sString">string</param>
        /// <returns>Base 64 string</returns>
        public static string ToBase64(string sString)
        {
            byte[] sString_bytes = Encoding.Default.GetBytes(sString);
            return ToBase64(sString_bytes);
        }
        /// <summary>
        /// Returns Base 64 string from char array.
        /// </summary>
        /// <param name="sString_chars">char array</param>
        /// <returns>Base 64 string</returns>
        public static string ToBase64(char[] sString_chars)
        {
            byte[] sString_bytes = Encoding.Default.GetBytes(sString_chars);
            return ToBase64(sString_bytes);
        }
        /// <summary>
        /// Returns Base 64 string from byte array.
        /// </summary>
        /// <param name="sString_bytes">byte array</param>
        /// <returns>Base 64 string</returns>
        public static string ToBase64(byte[] sString_bytes)
        {
            return Convert.ToBase64String(sString_bytes);
        }

        /// <summary>
        /// Determins if string is Base 64 string.
        /// </summary>
        /// <param name="sString">string</param>
        /// <returns>bool</returns>
        public static bool isBase64(string sString)
        {
            double dblLen = ((double)sString.Length / 4);
            string sStringLenOK = dblLen.ToString();
            sString = Regex.Replace(sString, @"[A-Za-z0-9\+/=]", "");
            return (sString.Length==0) && (sStringLenOK.IndexOf(",") == -1) && (sStringLenOK.IndexOf(".") == -1);
        }
        /// <summary>
        /// Determins if byte array is Base 64 string.
        /// </summary>
        /// <param name="sString_bytes">byte array</param>
        /// <returns>bool</returns>
        public static bool isBase64(byte[] sString_bytes)
        {
            string sString = ToString(sString_bytes);
            return isBase64(sString);
        }
        /// <summary>
        /// Determins if char array is Base 64 string.
        /// </summary>
        /// <param name="sString_chars">char array</param>
        /// <returns>bool</returns>
        public static bool isBase64(char[] sString_chars)
        {
            string sString = ToString(sString_chars);
            return isBase64(sString);
        }

    }
}
 
