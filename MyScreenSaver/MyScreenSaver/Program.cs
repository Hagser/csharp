using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyScreenSaver
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);
            string cat = "Processor";
            string ins = "_Total";
            string cou = "% Processor Time";

            if (key.OpenSubKey(Application.ProductName, true) != null)
            {
                key = key.OpenSubKey(Application.ProductName, true);
                if (key.OpenSubKey(Application.ProductVersion, true) != null)
                {
                    key = key.OpenSubKey(Application.ProductVersion, true);
                    string scat = key.GetValue("Category").ToString();
                    string sins = key.GetValue("Instance").ToString();
                    string scou = key.GetValue("Counter").ToString();
                    if (!string.IsNullOrEmpty(scat) && !string.IsNullOrEmpty(scat) && !string.IsNullOrEmpty(scat))
                    {
                        cat=scat;
                        ins=sins;
                        cou=scou;
                    }
                    else if (!string.IsNullOrEmpty(scat) && string.IsNullOrEmpty(scat) && !string.IsNullOrEmpty(scat))
                    {
                        cat = scat;
                        cou = scou;
                    }
                }
            }

            System.Diagnostics.PerformanceCounter counter = !string.IsNullOrEmpty(cat) && !string.IsNullOrEmpty(ins) && !string.IsNullOrEmpty(cou)?
                new System.Diagnostics.PerformanceCounter(cat, cou, ins, true):
                !string.IsNullOrEmpty(cat) && string.IsNullOrEmpty(ins) && !string.IsNullOrEmpty(cou)?
                new System.Diagnostics.PerformanceCounter(cat, cou, true):
                new System.Diagnostics.PerformanceCounter();
            Application.ApplicationExit += (a, b) => { if (counter != null && !string.IsNullOrEmpty(counter.CategoryName)) { System.Diagnostics.PerformanceCounter.CloseSharedResources(); counter.EndInit(); } };
            if (args.Length > 0)
            {
                if (args[0].ToLower().Trim().Substring(0, 2) == "/s") //show
                {
                    //run the screen saver
                    //Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    ShowScreensaver(counter);
                    Application.Run();
                }
                else if (args[0].ToLower().Trim().Substring(0, 2) == "/p") //preview
                {
                    //show the screen saver preview
                    //Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    //args[1] is the handle to the preview window
                    Application.Run(new Form1(new IntPtr(long.Parse(args[1]))));
                }
                else if (args[0].ToLower().Trim().Substring(0, 2) == "/c") //configure
                {
                    //nothing to configure
                    Application.EnableVisualStyles();
                    ConfigForm cf = new ConfigForm();
                    cf.ShowDialog();
                }
                else
                // an argument was passed, but it wasn't /s, /p,
                // or /c, so we don't care wtf it was
                {
                    //show the screen saver anyway
                    //Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    ShowScreensaver(counter);
                    Application.Run();
                }
            }
            else //no arguments were passed
            {
                //run the screen saver
                Application.SetCompatibleTextRenderingDefault(false);
                ShowScreensaver(counter);
                Application.Run();
            }

        }

        //will show the screen saver
        static void ShowScreensaver(System.Diagnostics.PerformanceCounter counter)
        {
            //loops through all the computer's screens (monitors)
            counter.BeginInit();
            foreach (Screen screen in Screen.AllScreens)
            {
                //creates a form just for that screen 
                //and passes it the bounds of that screen
                Form1 screensaver = new Form1(screen.Bounds,counter);
                screensaver.Show();
            }
        }
    }
}
