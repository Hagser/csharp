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
    public partial class MouseMove : UserControl
    {
        public MouseMove()
        {
            InitializeComponent();
            photonav.MouseEnterImage += new PhotoNavigator.MouseEnterImageEventHandler(photonav_MouseEnterImage);
            photonav.PlayPauseChanged += new PhotoNavigator.PlayPauseEventHandler(photonav_PlayPauseChanged);
            photonav.ImageIsChanged += new PhotoNavigator.ImageIsChangedEventHandler(photonav_ImageIsChanged);
        }

        void photonav_PlayPauseChanged(object sender, MyPhotoSlideshow.Classes.PlayPauseEventArgs e)
        {
            if (e.Status == Statuses.Playing)
            {
                photonav.MouseEnterImage -= photonav_MouseEnterImage;
            }
            else
            {
                photonav.MouseEnterImage += photonav_MouseEnterImage;
            }
        }

        void photonav_ImageIsChanged(object sender, ImageIsChangedEventArgs e)
        {
            PhotoInfo[] pis = { e.prevPhotoInfo, e.photoinfo, e.nextPhotoInfo };
            AddImage(pis);
        }

        void photonav_MouseEnterImage(object sender, MyPhotoSlideshow.Classes.MouseEnterImageEventArgs e)
        {

            PhotoInfo[] pis = { e.prevPhotoInfo, e.photoinfo, e.nextPhotoInfo };
            AddImage(pis);
        }

        private void AddImage(PhotoInfo photoInfo)
        {

            int imax = LayoutRoot.Children.Count;
            int icnt = 0;
            if (imax > 50)
            {
                LayoutRoot.Children.RemoveAt(1);
            }
            foreach (UIElement el in LayoutRoot.Children)
            {
                if (el.GetType() == typeof(Image))
                {
                    icnt++;
                    Image elimg = (el as Image);
                    elimg.RenderTransform = new SkewTransform() { AngleX = 360 - (imax - icnt), AngleY = 360 - (imax - icnt) };
                    double dop = double.Parse(icnt.ToString()) / double.Parse(imax.ToString());
                    elimg.Opacity = dop;

                }
            }
            Image img = new Image() { Source = photoInfo.ImageSource, Height = LayoutRoot.RowDefinitions[0].Height.Value,Margin=new Thickness(30), VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch, Stretch = Stretch.Uniform };
            img.SetValue(Grid.RowProperty, 0);
            LayoutRoot.Children.Add(img);
        }
        private void AddImage(PhotoInfo[] pis)
        {
            int imax = LayoutRoot.Children.Count;
            int icnt = 0;
            if (imax > 50)
            {
                LayoutRoot.Children.RemoveAt(1);
                LayoutRoot.Children.RemoveAt(1);
                LayoutRoot.Children.RemoveAt(1);
            }
            foreach (UIElement el in LayoutRoot.Children)
            {
                if (el.GetType() == typeof(Image))
                {
                    icnt++;
                    Image elimg = (el as Image);
                    elimg.RenderTransform = new SkewTransform() { AngleX = 360 - (imax - icnt), AngleY = 360 - (imax - icnt) };
                    double dop = double.Parse(icnt.ToString()) / double.Parse(imax.ToString());
                    elimg.Opacity = dop;

                }
            }
            int ipcnt = 0;
            foreach (PhotoInfo photoInfo in pis)
            {
                if (photoInfo != null)
                {
                    Image img = new Image() { Source = photoInfo.ImageSource, Height = LayoutRoot.RowDefinitions[0].Height.Value - 150, Margin = new Thickness(30), VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = ipcnt == 0 ? HorizontalAlignment.Left : ipcnt == 1 ? HorizontalAlignment.Center : ipcnt == 2 ? HorizontalAlignment.Right : HorizontalAlignment.Stretch, Stretch = Stretch.Uniform };
                    img.SetValue(Grid.RowProperty, 0);
                    LayoutRoot.Children.Add(img);
                    ipcnt++;
                }
            }
        }
    }
}
