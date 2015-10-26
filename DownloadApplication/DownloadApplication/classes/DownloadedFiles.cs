using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace MyDownloadApplication
{
    public class DownloadedFiles:INotifyPropertyChanged
    {
        public event EventHandler FileAdded;
        public event EventHandler FileRemoved;
        public event EventHandler FilesChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        public void Add(DownloadedFile df)
        {
            df.PropertyChanged += (a, b) => {
                if (FilesChanged != null)
                {
                    FilesChanged.Invoke(this, EventArgs.Empty);
                }

                InvokePropertyChanged(b.PropertyName);
            };
            files.Add(df);
            if (FileAdded != null)
            {
                FileAdded.Invoke(this, EventArgs.Empty);
            }

            InvokePropertyChanged("Files");
        }

        public void Remove(DownloadedFile df)
        {
            
            if (files.Remove(df) && FileRemoved != null)
            {
                FileRemoved.Invoke(this, EventArgs.Empty);
            }
            InvokePropertyChanged("Files");
        }

        public ObservableCollection<DownloadedFile> Files
        {
            get { return files; }
            set
            {
                files = value;
                InvokePropertyChanged("Files");
            }
        }
        private ObservableCollection<DownloadedFile> files = new ObservableCollection<DownloadedFile>();
        private void InvokePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
