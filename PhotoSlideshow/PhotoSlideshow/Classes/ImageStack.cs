using System;
using System.IO;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace PhotoSlideshow.Classes
{
    public class ImageStack : INotifyPropertyChanged
    {
        private ObservableCollection<Image> _MyImages = new ObservableCollection<Image>();
        public ObservableCollection<Image> MyImages
        {
            get { return _MyImages; }
            set { _MyImages = value; FirePropertyChanged("MyImages"); }
        }
        public void Remove(Image img)
        {
            _MyImages.Remove(img);
            FirePropertyChanged("MyImages");
        }
        public void Add(Image img)
        {
            _MyImages.Add(img);
            FirePropertyChanged("MyImages");
        }

        public ImageStack()
        {

        }


        #region INotifyPropertyChanged Members

        void FirePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
