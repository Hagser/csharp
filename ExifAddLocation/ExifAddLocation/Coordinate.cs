using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExifAddLocation
{
    public class Coordinate
    {
        public Coordinate()
        {
        }
        public Coordinate(string lat,string lon)
        {
            double reslat;
            if(double.TryParse(lat, out reslat))
                Latitude = reslat;
            
            double reslon;
            if(double.TryParse(lon, out reslon))
                Longitude = reslon;
        }
        public string Name { get; set; }
        public double Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
