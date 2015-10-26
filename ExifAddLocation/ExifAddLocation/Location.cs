using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExifAddLocation
{
    public class Location:Object
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Name { get; set; }

        public Location()
        { }
    }
}
