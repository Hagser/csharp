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
    public class PlayPauseEventArgs:EventArgs
    {
        public Statuses Status { get; set; }
    }
    public enum Statuses
    { 
        Paused,
        Playing
    }
    public enum ImagePlacements
    {
        Random,
        ZeroToEnd,
        Split
    }
}
