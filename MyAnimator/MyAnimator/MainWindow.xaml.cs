using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace MyAnimator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        Dictionary<string, ImageSource> liss = new Dictionary<string, ImageSource>();
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try{
            ImageSourceConverter isc = new ImageSourceConverter();
            string strDir = @"C:\ProgramData\MyIPWebcamTimeLapse\MyIPWebcamTimeLapse\1.0.0.4\192.168.1.13\20130311\";
            Action act = new Action(() =>
            {
                try
                {
                    foreach (string fil in System.IO.Directory.EnumerateFiles(strDir))
                    {
                        if (!liss.ContainsKey(fil))
                        {
                            ImageSource iss = isc.ConvertFromString(fil) as ImageSource;
                            liss.Add(fil, iss);
                        }
                    }
                }
                catch (Exception ec)
                {
                    string sdsldkfjsldkjf = ec.Message;
                }
            });
            act.Invoke();
            int i = 0; 
            TimerCallback tc = new TimerCallback((a) => {

                act.Invoke();
                try
                {
                    SetImageCallback d = new SetImageCallback((ims,_i)=>{                            
                        image1.Source = ims;
                        this.Title =_i+"."+ System.DateTime.Now.ToString("HH:mm:ss");
                    });

                    this.Dispatcher.Invoke(d,liss.Select(x => x.Value).ToArray()[i],i);
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
                i++;

                if(i == liss.Count)
                    i=1;
                    
            });
            System.Threading.Timer tim = new Timer(tc,null,0,50);
            }
            catch (Exception ec)
            {
                string sdsldkfjsldkjf = ec.Message;
            }
        }
        delegate void SetImageCallback(ImageSource ims,int incnt);

    }
}
