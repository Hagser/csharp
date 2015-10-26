using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SubNet
{
    public class CoordFiles:List<CoordFile>, INotifyPropertyChanged
    {
        public CoordFiles()
        { }
        public event PropertyChangedEventHandler PropertyChanged;
        public void AddCf(CoordFile cf)
        {
            cf.PropertyChanged += (a, b) => {
                PC(b.PropertyName);
            };
            PC("List");
            base.Add(cf);
        }

        private void PC(string p)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(p));
            }
        }
    }
}
