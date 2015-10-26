using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PhotoSlideshow.Web.Classes
{
    public class Photos
    {
        [DataMember]
        public string PhotoPath { get; set; }
        [DataMember]
        public DateTime LastModified { get; set; }
    }
}
