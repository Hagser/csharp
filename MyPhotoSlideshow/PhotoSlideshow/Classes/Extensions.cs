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
    public static class Extensions
    {
        public static int GetIndex(this PresentationFrameworkCollection<UIElement> collection, Image img)
        {
            int iret = 0;
            foreach (UIElement el in collection)
            {
                if (el.GetType() == typeof(Image))
                {
                    Image imgel = (Image)el;
                    if (imgel.Equals(img))
                    {
                        return iret;
                    }
                }
                iret++;
            }
            return -1;
        }
    }
}
