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
using System.Windows.Navigation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using PhotoSlideshow.Classes;

namespace PhotoSlideshow
{
    public partial class Circle : UserControl
    {
        public Circle()
        {
            InitializeComponent();

            int iImageCnt = 0;
            photonav.ImageIsChanged += (a, b) =>
            {
                Image img = tabImages[iImageCnt % tabNames.Length];
                img.Source = b.photoinfo.ImageSource;
                img.SetValue(Canvas.ZIndexProperty, iImageCnt);
                iImageCnt++;
            };
            photonav.PlayPauseChanged += (a, b) =>
            {
                if (b.Status==Statuses.Playing)
                {
                    stb.Begin();
                    dt.Start();
                }
                else
                {
                    stb.Stop();
                }
            };
            photonav.SpeedChanged += (a, b) =>
            {
                TimeSpan ts = new TimeSpan(0, 0, 0, int.Parse((Math.Round(b.SpeedTimeSpan.TotalSeconds*0.5)).ToString()));
                Duration dur = new Duration(ts);
                stbDA.Duration = dur;

            };
        }
        DispatcherTimer dt = new DispatcherTimer();
        string[] tabNames = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        Image[] tabImages;
        private void RotatingSurface_Loaded(object sender, RoutedEventArgs e)
        {
            RotatingSurface.Width = 600;
            RotatingSurface.Height = RotatingSurface.Width;
            rotCanvas.CenterX = RotatingSurface.Height / 2;
            rotCanvas.CenterY = RotatingSurface.Width / 2;
            rail.Width = RotatingSurface.Width;
            rail.Height = RotatingSurface.Height;

            int N = tabNames.Length;                // number of images
            // get radius and position of rail
            double wRail = rail.ActualWidth / 2;    // width of rail
            double hRail = rail.ActualHeight / 2;   // height of rail
            double xRail = (double)rail.GetValue(Canvas.LeftProperty);
            double yRail = (double)rail.GetValue(Canvas.TopProperty);
            double wImage = wRail - 50, hImage = hRail-50;       // same size for all images 
            // create an array of images (N depending on tabNames array)
            tabImages = new Image[N];
            for (int i = 0; i < N; i++)
            {
                tabImages[i] = new Image();
                tabImages[i].Width = wImage;
                tabImages[i].Height = hImage;

                // put image at the right place
                double X = wRail * Math.Cos(i * 2 * 3.14 / N) + xRail + wRail - wImage / 2;
                double Y = wRail * Math.Sin(i * 2 * 3.14 / N) + yRail + hRail - hImage / 2;
                tabImages[i].SetValue(Canvas.LeftProperty, X);
                tabImages[i].SetValue(Canvas.TopProperty, Y);

                RotatingSurface.Children.Add(tabImages[i]);
            }
            
            dt.Interval = new TimeSpan(0, 0, 0, 0, 20);
            dt.Tick += (a, b) =>
            {
                for (int i = 0; i < N; i++)
                {
                    RotateTransform rt = new RotateTransform() { Angle = 360 - rotCanvas.Angle, CenterX = tabImages[i].Height / 2, CenterY = tabImages[i].Width / 2 };
                    tabImages[i].RenderTransform = rt;
                }
            };
        }
    }
}
