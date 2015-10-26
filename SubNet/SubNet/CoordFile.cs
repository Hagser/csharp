using System.IO;
using System;
using System.ComponentModel;
namespace SubNet
{
    public class CoordFile:INotifyPropertyChanged
    {
//        private string strUrlBase = "http://www.submachineworld.com/subnet_data/";
        private string strUrlBase = "http://www.mateuszskutnik.com/submachine/subnet_data/";

        private DateTime _LastModified;
        private DateTime _LastRefreshed;
        private string _Name;
        private string _FullName;

        public DateTime LastRefreshed
        {
            get
            {
                return _LastRefreshed;
            }
            set { _LastRefreshed = value; PC("LastRefreshed"); }
        }

        public DateTime LastModified
        {
            get
            {
                return _LastModified;
            }
            set { _LastModified = value; PC("LastModified"); UpdateLastWriteDate(value); }
        }

        public string DateDiff
        {
            get
            {
                try{
                    TimeSpan tsDiff = new TimeSpan(_LastRefreshed.Ticks - _LastModified.Ticks);
                    return string.Format("{0} {1}:{2}:{3}", tsDiff.Days.ToString().PadLeft(3, '0'), tsDiff.Hours.ToString().PadLeft(2, '0'), tsDiff.Minutes.ToString().PadLeft(2, '0'), tsDiff.Seconds.ToString().PadLeft(2, '0'));
                }
                catch{}
                return "";
            }
            set {}
        }

        private void UpdateLastWriteDate(DateTime value)
        {
            if (FullName == null)
                return;
            FileInfo fi = new FileInfo(FullName);
            if (fi.Exists)
            {
                fi.LastWriteTime = value;
            }
        }
        public string Name
        {
            get
            {
                return _Name;
            }
            set { _Name = value; PC("Name"); }
        }
        public string FullName
        {
            get
            {
                return _FullName;
            }
            set { _FullName = value; PC("FullName"); }
        }
        public string Url
        {
            get { return (Name.Contains("subnet_start") ? strUrlBase.Replace("subnet_data/", "") : strUrlBase) + Name; }
            set { }
        }
        public CoordFile()
        { }
        public CoordFile(FileInfo fi)
        {
            _LastModified = fi.LastWriteTime;
            _FullName = fi.FullName;
            _Name = fi.Name;
        }
        private void PC(string p)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(p));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}