using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;

namespace MyPhotoSlideshow
{
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();

            string rssURL = HtmlPage.Document.GetElementById("rssURL").GetAttribute("value");
            string receiverURL = HtmlPage.Document.GetElementById("receiverURL").GetAttribute("value");
            string screenWidth = HtmlPage.Document.GetElementById("screenWidth").GetAttribute("value");
            string screenHeight = HtmlPage.Document.GetElementById("screenHeight").GetAttribute("value");

            SaveISS("receiverURL", receiverURL);
        }

        private void SaveISS(string in_name,string in_val)
        {
            System.IO.IsolatedStorage.IsolatedStorageSettings iss = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
            if (iss.Contains(in_name))
                iss.Remove(in_name);
            iss.Add(in_name, in_val);
            iss.Save();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new Matrix();
            //this.RootVisual = new Circle();
            //this.RootVisual = new MouseMove();
            //this.RootVisual = new MainPage();
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }
        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
