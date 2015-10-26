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
using MyPhotoSlideshow.Classes;

namespace MyPhotoSlideshow
{
    public partial class Matrix : UserControl
    {
        public Matrix()
        {
            InitializeComponent();
            photonav.MouseDownImage += new PhotoNavigator.MouseDownImageEventHandler(photonav_MouseDownImage);
            photonav.ImageIsChanged += new PhotoNavigator.ImageIsChangedEventHandler(photonav_ImageIsChanged);
            photonav.PlayPauseChanged += new PhotoNavigator.PlayPauseEventHandler(photonav_PlayPauseChanged);
            photonav.MouseEnterImage += new PhotoNavigator.MouseEnterImageEventHandler(photonav_MouseEnterImage);
            photonav.MouseMove += new MouseEventHandler(photonav_MouseMove);
            photonav.ThumbnailSize = new Size(150, 150);
            photonav.ImagePlacements = ImagePlacements.Random;
        }
        double mpx, mpy;
        void photonav_MouseMove(object sender, MouseEventArgs e)
        {
            mpx = e.GetPosition(photonav).X;
            mpy = e.GetPosition(photonav).Y;
        }

        void photonav_MouseEnterImage(object sender, MouseEnterImageEventArgs e)
        {
            string title = e.photoinfo.title;
            string owner = "";
            InfoText.Visibility = Visibility.Visible;
            InfoText.Text = title + "\n" + owner;
            InfoBorder.SetValue(Canvas.LeftProperty, mpx);
            InfoBorder.SetValue(Canvas.TopProperty, mpy);
            
            //InfoCanvas.Margin = new Thickness(e.point.X, e.point.Y, 0, 0);
        }

        void photonav_MouseDownImage(object sender, MouseDownImageEventArgs e)
        {
            BigImage.Source = e.photoinfo.ImageSource;
            BigCanvas.MaxHeight = this.ActualHeight - 30;
            BigCanvas.Height = BigCanvas.MaxHeight;
            BigCanvas.Visibility = Visibility.Visible;
            BigImage.Visibility = Visibility.Visible;
        }

        void photonav_PlayPauseChanged(object sender, MyPhotoSlideshow.Classes.PlayPauseEventArgs e)
        {
            if (e.Status == Statuses.Paused)
            {
                BigCanvas.Visibility = Visibility.Collapsed;
                BigImage.Visibility = Visibility.Collapsed;
            }
            else if (e.Status == Statuses.Playing)
            {
                BigCanvas.MaxHeight = this.ActualHeight - 30;
                BigCanvas.Height = BigCanvas.MaxHeight;
                BigCanvas.Visibility = Visibility.Visible;
                BigImage.Visibility = Visibility.Visible;
                photonav.AutoGotoNextDate = true;
            }
        }

        void photonav_ImageIsChanged(object sender, MyPhotoSlideshow.Classes.ImageIsChangedEventArgs e)
        {
            BigImage.Source = e.photoinfo.ImageSource;
        }

        private void BigCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            photonav.PlayPauseClick(Statuses.Paused);
            photonav.AutoGotoNextDate = false;
        }


    }
}
