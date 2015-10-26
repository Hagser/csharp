using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlickrEasyAsyncOrganize.classes
{
    public class photosets : flickrbase
    {
        public photosets()
        { }
        public string cancreate{get;set;}
        public List<photoset> children = new List<photoset>();
    }
}
