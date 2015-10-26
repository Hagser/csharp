using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyDownloadApplication
{

    public class MegaVideoDecrypter
    {

        private static Array aRedimension(Array orgArray, Int32 tamaño)
        {
            Type t = orgArray.GetType().GetElementType();
            Array nArray = Array.CreateInstance(t, tamaño);
            Array.Copy(orgArray, 0, nArray, 0, Math.Min(orgArray.Length, tamaño));
            return nArray;
        }

        private static string Concat(Array items, string delimiter)
        {
            bool first = true;

            StringBuilder sb = new StringBuilder();
            foreach (object item in items)
            {
                if (item == null)
                    continue;

                if (!first)
                {
                    sb.Append(delimiter);
                }
                else
                {
                    first = false;
                }
                sb.Append(item);
            }
            return sb.ToString();
        }

        private static string[] DeConcat(string strphrase)
        {
            string[] arrTemp = { };
            for (int i = 0; i < strphrase.Length; i++)
            {
                arrTemp = (string[])aRedimension(arrTemp, arrTemp.Length + 1);
                arrTemp[arrTemp.Length - 1] = strphrase[i].ToString();
            }

            return arrTemp;
        }

        public static string decrypt(string str, string key1, string key2)
        {
            string[] __reg1 = { };
            int __reg3 = 0;
            string __reg0;
            while (__reg3 < str.Length)
            {
                __reg0 = str[__reg3].ToString();
                if (__reg0 == "0")
                {
                    __reg1 = (string[])aRedimension(__reg1, __reg1.Length + 1);
                    __reg1[__reg1.Length - 1] = "0000";
                }
                else
                {
                    if (__reg0 == "1")
                    {
                        __reg1 = (string[])aRedimension(__reg1, __reg1.Length + 1);
                        __reg1[__reg1.Length - 1] = "0001";
                    }
                    else
                    {
                        if (__reg0 == "2")
                        {
                            __reg1 = (string[])aRedimension(__reg1, __reg1.Length + 1);
                            __reg1[__reg1.Length - 1] = "0010";
                        }
                        else
                        {
                            if (__reg0 == "3")
                            {
                                __reg1 = (string[])aRedimension(__reg1, __reg1.Length + 1);
                                __reg1[__reg1.Length - 1] = "0011";
                            }
                            else
                            {
                                if (__reg0 == "4")
                                {
                                    __reg1 = (string[])aRedimension(__reg1, __reg1.Length + 1);
                                    __reg1[__reg1.Length - 1] = "0100";
                                }
                                else
                                {
                                    if (__reg0 == "5")
                                    {
                                        __reg1 = (string[])aRedimension(__reg1, __reg1.Length + 1);
                                        __reg1[__reg1.Length - 1] = "0101";
                                    }
                                    else
                                    {
                                        if (__reg0 == "6")
                                        {
                                            __reg1 = (string[])aRedimension(__reg1, __reg1.Length + 1);
                                            __reg1[__reg1.Length - 1] = "0110";
                                        }
                                        else
                                        {
                                            if (__reg0 == "7")
                                            {
                                                __reg1 = (string[])aRedimension(__reg1, __reg1.Length + 1);
                                                __reg1[__reg1.Length - 1] = "0111";
                                            }
                                            else
                                            {
                                                if (__reg0 == "8")
                                                {
                                                    __reg1 = (string[])aRedimension(__reg1, __reg1.Length + 1);
                                                    __reg1[__reg1.Length - 1] = "1000";
                                                }
                                                else
                                                {
                                                    if (__reg0 == "9")
                                                    {
                                                        __reg1 = (string[])aRedimension(__reg1, __reg1.Length + 1);
                                                        __reg1[__reg1.Length - 1] = "1001";
                                                    }
                                                    else
                                                    {
                                                        if (__reg0 == "a")
                                                        {
                                                            __reg1 = (string[])aRedimension(__reg1, __reg1.Length + 1);
                                                            __reg1[__reg1.Length - 1] = "1010";
                                                        }
                                                        else
                                                        {
                                                            if (__reg0 == "b")
                                                            {
                                                                __reg1 = (string[])aRedimension(__reg1, __reg1.Length + 1);
                                                                __reg1[__reg1.Length - 1] = "1011";
                                                            }
                                                            else
                                                            {
                                                                if (__reg0 == "c")
                                                                {
                                                                    __reg1 = (string[])aRedimension(__reg1, __reg1.Length + 1);
                                                                    __reg1[__reg1.Length - 1] = "1100";
                                                                }
                                                                else
                                                                {
                                                                    if (__reg0 == "d")
                                                                    {
                                                                        __reg1 = (string[])aRedimension(__reg1, __reg1.Length + 1);
                                                                        __reg1[__reg1.Length - 1] = "1101";
                                                                    }
                                                                    else
                                                                    {
                                                                        if (__reg0 == "e")
                                                                        {
                                                                            __reg1 = (string[])aRedimension(__reg1, __reg1.Length + 1);
                                                                            __reg1[__reg1.Length - 1] = "1110";
                                                                        }
                                                                        else
                                                                        {
                                                                            if (__reg0 == "f")
                                                                            {
                                                                                __reg1 = (string[])aRedimension(__reg1, __reg1.Length + 1);
                                                                                __reg1[__reg1.Length - 1] = "1111";
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ++__reg3;
            }
            string strTemp1 = Concat(__reg1, "");
            __reg1 = DeConcat(strTemp1);

            int[] __reg6;
            __reg6 = new int[384];
            __reg3 = 0;
            while (__reg3 < 384)
            {
                key1 = ((int.Parse(key1) * 11 + 77213) % 81371).ToString();
                key2 = ((int.Parse(key2) * 17 + 92717) % 192811).ToString();
                __reg6[__reg3] = (int.Parse(key1) + int.Parse(key2)) % 128;
                ++__reg3;
            }

            __reg3 = 256;
            int __reg5, __reg4, __reg8;
            while (__reg3 >= 0)
            {
                __reg5 = __reg6[__reg3];
                __reg4 = __reg3 % 128;
                __reg8 = int.Parse(__reg1[__reg5]);
                __reg1[__reg5] = __reg1[__reg4];
                __reg1[__reg4] = __reg8.ToString();
                --__reg3;
            }

            __reg3 = 0;
            while (__reg3 < 128)
            {
                __reg1[__reg3] = (int.Parse(__reg1[__reg3]) ^ (__reg6[__reg3 + 256] & 1)).ToString();
                ++__reg3;
            }

            string __reg12;
            __reg12 = Concat(__reg1, "");

            string[] __reg7 = { };
            __reg3 = 0;
            int intTmp1 = 0;
            while (__reg3 < __reg12.Length)
            {
                string __reg9;
                //if (intTmp1 < 4) __reg9 = __reg12.Substring(__reg3, 4 - intTmp1);
                //else __reg9 = "";
                __reg9 = __reg12.Substring(__reg3, 4);
                __reg7 = (string[])aRedimension(__reg7, __reg7.Length + 1);
                __reg7[__reg7.Length - 1] = __reg9;
                __reg3 = __reg3 + 4;
                intTmp1++;
            }

            string[] __reg2 = { };
            __reg3 = 0;
            while (__reg3 < __reg7.Length)
            {
                __reg0 = __reg7[__reg3];
                if (__reg0 == "0000")
                {
                    __reg2 = (string[])aRedimension(__reg2, __reg2.Length + 1);
                    __reg2[__reg2.Length - 1] = "0";
                }
                else
                {
                    if (__reg0 == "0001")
                    {
                        __reg2 = (string[])aRedimension(__reg2, __reg2.Length + 1);
                        __reg2[__reg2.Length - 1] = "1";
                    }
                    else
                    {
                        if (__reg0 == "0010")
                        {
                            __reg2 = (string[])aRedimension(__reg2, __reg2.Length + 1);
                            __reg2[__reg2.Length - 1] = "2";
                        }
                        else
                        {
                            if (__reg0 == "0011")
                            {
                                __reg2 = (string[])aRedimension(__reg2, __reg2.Length + 1);
                                __reg2[__reg2.Length - 1] = "3";
                            }
                            else
                            {
                                if (__reg0 == "0100")
                                {
                                    __reg2 = (string[])aRedimension(__reg2, __reg2.Length + 1);
                                    __reg2[__reg2.Length - 1] = "4";
                                }
                                else
                                {
                                    if (__reg0 == "0101")
                                    {
                                        __reg2 = (string[])aRedimension(__reg2, __reg2.Length + 1);
                                        __reg2[__reg2.Length - 1] = "5";
                                    }
                                    else
                                    {
                                        if (__reg0 == "0110")
                                        {
                                            __reg2 = (string[])aRedimension(__reg2, __reg2.Length + 1);
                                            __reg2[__reg2.Length - 1] = "6";
                                        }
                                        else
                                        {
                                            if (__reg0 == "0111")
                                            {
                                                __reg2 = (string[])aRedimension(__reg2, __reg2.Length + 1);
                                                __reg2[__reg2.Length - 1] = "7";
                                            }
                                            else
                                            {
                                                if (__reg0 == "1000")
                                                {
                                                    __reg2 = (string[])aRedimension(__reg2, __reg2.Length + 1);
                                                    __reg2[__reg2.Length - 1] = "8";
                                                }
                                                else
                                                {
                                                    if (__reg0 == "1001")
                                                    {
                                                        __reg2 = (string[])aRedimension(__reg2, __reg2.Length + 1);
                                                        __reg2[__reg2.Length - 1] = "9";
                                                    }
                                                    else
                                                    {
                                                        if (__reg0 == "1010")
                                                        {
                                                            __reg2 = (string[])aRedimension(__reg2, __reg2.Length + 1);
                                                            __reg2[__reg2.Length - 1] = "a";
                                                        }
                                                        else
                                                        {
                                                            if (__reg0 == "1011")
                                                            {
                                                                __reg2 = (string[])aRedimension(__reg2, __reg2.Length + 1);
                                                                __reg2[__reg2.Length - 1] = "b";
                                                            }
                                                            else
                                                            {
                                                                if (__reg0 == "1100")
                                                                {
                                                                    __reg2 = (string[])aRedimension(__reg2, __reg2.Length + 1);
                                                                    __reg2[__reg2.Length - 1] = "c";
                                                                }
                                                                else
                                                                {
                                                                    if (__reg0 == "1101")
                                                                    {
                                                                        __reg2 = (string[])aRedimension(__reg2, __reg2.Length + 1);
                                                                        __reg2[__reg2.Length - 1] = "d";
                                                                    }
                                                                    else
                                                                    {
                                                                        if (__reg0 == "1110")
                                                                        {
                                                                            __reg2 = (string[])aRedimension(__reg2, __reg2.Length + 1);
                                                                            __reg2[__reg2.Length - 1] = "e";
                                                                        }
                                                                        else
                                                                        {
                                                                            if (__reg0 == "1111")
                                                                            {
                                                                                __reg2 = (string[])aRedimension(__reg2, __reg2.Length + 1);
                                                                                __reg2[__reg2.Length - 1] = "f";
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ++__reg3;
            }
            return Concat(__reg2, "");
        }

    }
}
