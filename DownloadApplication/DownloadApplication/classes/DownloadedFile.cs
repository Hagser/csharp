using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MyDownloadApplication
{
    public class DownloadedFile:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string FileName { get { return (new System.IO.FileInfo(FilePath)).Name; } }
        public string OnlyUrl { get { return _OnlyUrl; } set { if (_OnlyUrl != value) { _OnlyUrl = value; InvokePropertyChanged("OnlyUrl"); } } }
        public string SizeText { get { return Math.Round((Size/ 1024),2).ToString() + " kB"; } }
        public double Size { get { return _Size; } set { if (_Size != value) { _Size = value; InvokePropertyChanged("Size"); } } }
        public string ProgressText { get { return Progress.ToString() + "%"; } }
        public double Progress { get { return _Progress; } set { if (_Progress != value) { _Progress = value; InvokePropertyChanged("Progress"); } } }
        public string FilePath { get { return _FilePath; } set { if (_FilePath != value) { _FilePath = value; InvokePropertyChanged("FilePath"); } } }
        public double ContentLength { get { return _ContentLength; } set { if (_ContentLength != value) { _ContentLength = value; InvokePropertyChanged("ContentLength"); } } }
        public string Info { get { return _Info; } set { if (_Info != value) { _Info = value; InvokePropertyChanged("Info"); } } }
        
        private string _OnlyUrl = "";
        private double _Size = 0;
        private double _Progress = 0;
        private string _FilePath = "";
        private double _ContentLength = 0;
        private string _Info = "";

        private void InvokePropertyChanged(string parameter)
        {
            if(PropertyChanged!=null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(parameter));
            }
        }

    }
}
