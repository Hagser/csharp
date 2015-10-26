using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyOPMLReader
{
    class CallbackClass
    {
        public CallbackClass(string _url)
        {
            url = _url;
        }
        public string url { get; set; }
    }
}
