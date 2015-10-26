using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyAppRunner
{
    class MyApps
    {
        public string Path { get; set; }
        public IntPtr Handle { get; set; }
        public FileInfo Fileinfo { get { return new FileInfo(Path); } set { } }
    }
}
