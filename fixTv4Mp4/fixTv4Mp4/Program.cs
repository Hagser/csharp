using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace fixTv4Mp4
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\jh\Desktop\elem\";
            string filepath = path + DateTime.Today.Ticks.ToString() + ".mp4";
            if (File.Exists(filepath))
                File.Delete(filepath);

            int icnt = 0;
            foreach (string file in Directory.EnumerateFiles(path, "*_s.txt", SearchOption.AllDirectories))
            {
                Console.WriteLine(file);
                string alltext = File.ReadAllText(file,Encoding.Default);
                int istart = alltext.IndexOf("\r\n\r\n") + 4;
                File.AppendAllText(filepath, alltext.Substring(istart),Encoding.Default);
                Thread.Sleep(10);
                //if (icnt > 30)
                //    break;
                icnt++;
            }
            Console.WriteLine("Done");

        }
    }
}
