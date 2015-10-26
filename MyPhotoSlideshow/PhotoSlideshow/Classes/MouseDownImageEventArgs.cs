using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MyPhotoSlideshow.Classes
{
    public class MouseDownImageEventArgs:EventArgs
    {
        public Image image { get; set; }
        public PhotoInfo photoinfo { get; set; }
        public Point point { get; set; }
        public PhotoInfo prevPhotoInfo { get; set; }
        public PhotoInfo nextPhotoInfo { get; set; }
    }
}
