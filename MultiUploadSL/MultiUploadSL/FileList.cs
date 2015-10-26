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
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MultiUploadSL
{
    public class FileList : INotifyPropertyChanged
    {
        private ObservableCollection<MyFileInfo> _myFileList = new ObservableCollection<MyFileInfo>();
        public ObservableCollection<MyFileInfo> myFileList
        {
            get { return _myFileList; }
            set { _myFileList = value; FirePropertyChanged("myFileList"); }
        }
        public FileList()
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
