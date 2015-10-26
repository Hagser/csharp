using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyDuplicatedFileFinder
{
    class FileExtComparer:IEqualityComparer<file>
    {

        public bool Equals(file x, file y)
        {
            FileInfo fix = new FileInfo(x.path);
            FileInfo fiy = new FileInfo(y.path);
            return fix.Extension.Equals(fiy.Extension, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(file obj)
        {
            FileInfo fix = new FileInfo(obj.path);
            return fix.Extension.GetHashCode();
        }
    }
}
