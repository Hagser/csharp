using System.Windows.Controls;
using System.Collections.ObjectModel;
using PhotoSlideshow.Classes;
using System.Windows.Media;
using System;
using System.Windows.Browser;
using System.Net;
using System.IO;
using System.Linq;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Threading;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PhotoSlideshow
{
    public partial class MainPage : UserControl
    {
        
        public MainPage()
        {
            InitializeComponent();


            storyboard1.Completed += (a, b) => {
                storyboard1b.Begin();
            };
            storyboard2.Completed += (a, b) =>
            {
                storyboard2b.Begin();
            };
            storyboard3.Completed += (a, b) =>
            {
                storyboard3b.Begin();
            };
            photonav.PlayPauseChanged += (a, b) =>
            {
                if (b.Status == Statuses.Paused)
                {
                    foreach (var uiel in this.Resources)
                    {
                        if (uiel.GetType().BaseType == typeof(Storyboard))
                        {
                            (uiel as Storyboard).Stop();
                        }
                    }
                }
            };
            int iImageCnt = 0;
            photonav.ImageIsChanged += (a, b) =>
                {
                    switch(iImageCnt%3)
                    {
                        case 0: BigImage1.Source = null; BigImage1.Source = b.photoinfo.ImageSource; BigImage1.Height = 0; storyboard1.Stop(); storyboard1.Begin(); storyboardOp1.Stop(); storyboardOpR1.Stop(); storyboardOpR2.Begin(); storyboardOp2.Begin(); storyboardOpR3.Stop(); storyboardOp3.Stop();
                            break;
                        case 1: BigImage2.Source = null; BigImage2.Source = b.photoinfo.ImageSource; BigImage2.Height = 0; storyboard2.Stop(); storyboard2.Begin(); storyboardOp1.Stop(); storyboardOpR1.Stop(); storyboardOp2.Stop(); storyboardOpR2.Stop(); storyboardOpR3.Begin(); storyboardOp3.Begin();
                            break;
                        case 2: BigImage3.Source = null; BigImage3.Source = b.photoinfo.ImageSource; BigImage3.Height = 0; storyboard3.Stop(); storyboard3.Begin(); storyboardOpR1.Begin(); storyboardOp1.Begin(); storyboardOpR2.Stop(); storyboardOpR3.Stop(); storyboardOp2.Stop(); storyboardOp3.Stop();
                            break;
                    }

                    iImageCnt++;
                };
        }

        private void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BigImageStackPanel.Width = e.NewSize.Width - 4;

            BigImage1.Width = Math.Round(BigImageStackPanel.Width / 3, 0) - 4;
            BigImage2.Width = BigImage1.Width;
            BigImage3.Width = BigImage2.Width;
        }



    }
}
