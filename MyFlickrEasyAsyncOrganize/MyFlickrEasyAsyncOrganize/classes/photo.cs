using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFlickrEasyAsyncOrganize.classes
{
    public class photo : flickrbase
    {
        public photo(){ }
        public string id { get; set; }
        public string farm { get; set; }
        public string secret{get;set;}
        public string server{get;set;}
        public string title{get;set;}
        public string isprimary{get;set;}
        public string make { get; set; }
        public string model { get; set; }
        public string tags { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string place_id { get; set; }
        public string woeid { get; set; }
        public place place { get; set; }
        
        public DateTime datetimeoriginal { get; set; }
    }
}
