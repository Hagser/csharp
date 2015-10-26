using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace downloadFunds
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://seb.dk/pow/fmk/2100/fonder_{0}.TXT";
            string file = @"C:\Users\johan\Documents\GitHub\fonder\fonder\{0}.txt";
            string filecsv = @"C:\Users\johan\Documents\GitHub\fonder\fonder\{0}.csv";
            string year = args.Length > 0 ? args[0] : "2015";
            for (int m = 1; m < 13; m++)
            {
                for (int d = 1; d < 32; d++)
                {
                    string date = year + "-" + (m + "").PadLeft(2, '0') + "-" + (d + "").PadLeft(2, '0');
                    DateTime dt;
                    if (DateTime.TryParse(date, out dt) && dt<DateTime.Today)
                    {
                        Uri uri = new Uri(string.Format(url, date));
                        try
                        {
                            using (WebClient wc = new WebClient())
                            {
                                string filepath = string.Format(file, date);
                                if (!File.Exists(filepath))
                                {
                                    Console.WriteLine(uri + "-" + filepath);
                                    //Console.Beep(500, 50);

                                    wc.DownloadFile(uri, filepath);
                                }
                                else
                                {
                                    //Console.Beep(5000, 50);

                                }
                            }
                        }
                        catch
                        {
                            //Console.Beep(10000, 50);
                        }
                    }
                }
            
            }
            string todayUrl = "http://www.pensionsmyndigheten.se/fundfact/kurser.csv";

            try
            {
                using (WebClient wc = new WebClient())
                {
                    string filepath = string.Format(filecsv, DateTime.Today.ToString("yyyy-MM-dd"));
                    if (File.Exists(filepath))
                        File.Delete(filepath);
                    Console.WriteLine(todayUrl + "-" + filepath);
                    //Console.Beep(500, 50);

                    wc.DownloadFile(todayUrl, filepath);
                }
            }
            catch
            {
                //Console.Beep(10000, 50);
            }
        }
    }
}
