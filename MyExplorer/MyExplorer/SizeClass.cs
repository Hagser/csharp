using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace MyExplorer
{
    public class SizeClass:INotifyPropertyChanged
    {
        
        private System.Timers.Timer tim = new System.Timers.Timer(1 * 60 * 1000);
        private long _size = -1;
        public SizeClass()
        {
            at = DateTime.Now;
            tim.Elapsed += (a, b) => {
                tim.Stop();
                ThreadPool.QueueUserWorkItem(getSize, this.path);
            };
        }
        public DateTime at { get; set; }
        public long size { 
            get { 
                return _size; 
            } 
            set {
                if (_size != value)
                {
                    _size = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("size"));
                }
                tim.Start(); 
            } 
        }
        public string path { get; set; }

        private void getSize(object dir)
        {
            this.size = getSize(dir + "");
        }
        private long getSize(string dir)
        {
            try
            {
                return Directory.GetFiles(dir, "*", SearchOption.AllDirectories).Sum(f => new FileInfo(f).Length);
            }
            catch { }
            return -1;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
