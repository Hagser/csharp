using System;
using System.IO;
using System.ComponentModel;

namespace MultiUploadSL
{

    public class MyFileInfo : INotifyPropertyChanged
    {
        private FileInfo _fi;

        private bool _uploaded = false;
        public bool Uploaded
        {
            get { return _uploaded; }
            set { _uploaded = value; FirePropertyChanged("Uploaded"); }
        }

        private string _progress = "";
        public string Progress
        {
            get { return _progress; }
            set { _progress = value; FirePropertyChanged("Progress"); }
        }

        private double _progressvalue = 0;
        public double ProgressValue
        {
            get { return _progressvalue; }
            set { _progressvalue = value; FirePropertyChanged("ProgressValue"); }
        }

        public FileStream OpenRead()
        {
            return _fi.OpenRead();
        }
        public long Length
        {
            get { return _fi.Length; }
        }
        public string Name
        {
            get { return _fi.Name; }
        }
        public string RealSize
        {
            get { return getRealSize(_fi.Length); }
        }

        public MyFileInfo()
        {

        }
        public MyFileInfo(FileInfo fi)
        {
            _fi = fi;
        }
        private string getRealSize(long in_lng)
        {
            string v_ret = "";

            if (in_lng >= 1000000000000)
            {
                v_ret = Math.Round(((double)in_lng) / 1000000000000, 1).ToString() + " TB";
            }
            else if (in_lng >= 1000000000)
            {
                v_ret = Math.Round(((double)in_lng) / 1000000000, 1).ToString() + " GB";
            }
            else if (in_lng >= 1000000)
            {
                v_ret = Math.Round(((double)in_lng) / 1000000, 1).ToString() + " MB";
            }
            else if (in_lng >= 1000)
            {
                v_ret = Math.Round(((double)in_lng) / 1000, 1).ToString() + " kB";
            }
            else
            {
                v_ret = in_lng.ToString() + " B";
            }

            return v_ret;
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
