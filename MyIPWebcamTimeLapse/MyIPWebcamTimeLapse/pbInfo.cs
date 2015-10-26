using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MyIPWebcamTimeLapse
{
    public class pbInfo
    {
        public delegate void pbInfoChangedEventHandler(object sender, PropertyChangedEventArgs e);
        public string FilePath { get { return _FilePath; } set { if (string.IsNullOrEmpty(value)) { return; } if (_FilePath != value) { OnpbInfoChanged(new PropertyChangedEventArgs("FilePath")); } _FilePath = value; } }
        public string Server { get { return _Server; } set {  { OnpbInfoChanged(new PropertyChangedEventArgs("Server")); } _Server = value; } }
        public float Rotate { get { return _Rotate; } set {  { OnpbInfoChanged(new PropertyChangedEventArgs("Rotate")); } _Rotate = value; } }
        public Exception Error { get { return _Error; } set { { OnpbInfoChanged(new PropertyChangedEventArgs("Error")); } _Error = value; } }

        private string _FilePath { get; set; }
        private string _Server { get; set; }
        private float _Rotate { get; set; }
        private Exception _Error { get; set; }

        public event pbInfoChangedEventHandler pbInfoChanged;
        protected virtual void OnpbInfoChanged(PropertyChangedEventArgs e)
        {
            if (pbInfoChanged != null)
                pbInfoChanged(this, e);
        }
    }
}
