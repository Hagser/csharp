using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HCFolderMonitor
{
    public class FSW : FileSystemWatcher
    {
        public object Tag = null;
        public FSW()
        {

        }
    }

}
