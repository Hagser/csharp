using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyXCopy
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] myargs =  args;
            foreach (string s in myargs)
            {
                if (!Directory.Exists(s))
                    continue;

                string line;
                Console.WriteLine("Are you sure you want to delete folder '"+s+"'? (Yes/No):");                
                do
                {
                    line = Console.ReadLine();
                    if (line != null)
                    {
                        if (line.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (Directory.Exists(s))
                            {
                                try
                                {
                                    deleteFolder(s);
                                }
                                catch(Exception ex)
                                {
                                    Console.WriteLine(s+":"+ex.Message);
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                }
            } while (line==null); 

            }
            if (myargs != null && myargs.Length > 0)
            {
                Console.Beep(9000, 500);
                Console.WriteLine("Press any key to exit!");
                Console.ReadLine();
            }
        }

        private static void deleteFolder(string folder)
        {
            try
            {
                foreach (string file in Directory.EnumerateFiles(folder, "*", SearchOption.TopDirectoryOnly))
                    if (File.Exists(file))
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(file + ":" + ex.Message);
                        }
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(folder + ":" + ex.Message);
            }

            try
            {
                foreach (string subfolder in Directory.EnumerateDirectories(folder, "*", SearchOption.TopDirectoryOnly))
                    deleteFolder(subfolder);
            }
            catch (Exception ex)
            {
                Console.WriteLine(folder + ":" + ex.Message);
            }
            try {
                Directory.Delete(folder);
            }
            catch (Exception ex)
            {
                Console.WriteLine(folder + ":" + ex.Message);
            }
            Console.WriteLine(folder + " deleted!");

        }
    }
}
