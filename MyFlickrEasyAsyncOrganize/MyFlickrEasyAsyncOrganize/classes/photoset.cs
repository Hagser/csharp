using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlickrEasyAsyncOrganize.classes
{
    public class photoset : flickrbase
    {
        public photoset()
        { }
        public string id{get;set;}
        public string primary{get;set;}
        public string secret{get;set;}
        public string server{get;set;}
        public string farm{get;set;}
        public string videos{get;set;}
        public string needs_interstitial{get;set;}
        public string visibility_can_see_set{get;set;}
        public int count_views{get;set;}
        public int count_comments { get; set; }
        public int count_photos { get; set; }
        public string can_comment{get;set;}
        public string date_create{get;set;}
        public string date_update{get;set;}
        public string title { get; set; }
        public string description { get; set; }
        public List<photo> photos = new List<photo>();
    }
}
