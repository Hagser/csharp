using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySpeedClicker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Restart();
            
        }

        private static void Restart()
        {
            try
            {
                Form1 frm = new Form1();
                Application.Run(frm);
            }
            catch(Exception ex)
            {
                Restart();
            }
        }
    }
}
