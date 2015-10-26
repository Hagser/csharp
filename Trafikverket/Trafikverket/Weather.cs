using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trafikverket
{
    public class SlimWeather
    {
        public double AirTemp { get; set; }
        public double RoadTemp { get; set; }
        public string MeasurePoint { get; set; }
        public string Id { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public double WindForce { get; set; }
        public double AverageWindForce { get; set; }
        public double MaxWindForce { get; set; }
        public int CountyNo { get; set; }
        public int PrecipitationIconId { get; set; }

    }
    public class Weather:INotifyPropertyChanged
    {
        public bool Active { get { return _Active; } set { if (value != _Active) { _Active = value; InvokePropertyChanged("Active"); } } }

        public double AirTemp { get { return _AirTemp; } set { if (value != _AirTemp) { _AirTemp = value; InvokePropertyChanged("AirTemp"); } } }
        public int AirTempIconId { get { return _AirTempIconId; } set { if (value != _AirTempIconId) { _AirTempIconId = value; InvokePropertyChanged("AirTempIconId"); } } }
        public int AverageWindIconId { get { return _AverageWindIconId; } set { if (value != _AverageWindIconId) { _AverageWindIconId = value; InvokePropertyChanged("AverageWindIconId"); } } }
        public double AmountOfPrecipitation { get { return _AmountOfPrecipitation; } set { if (value != _AmountOfPrecipitation) { _AmountOfPrecipitation = value; InvokePropertyChanged("AmountOfPrecipitation"); } } }
        public int CountyNo { get { return _CountyNo; } set { if (value != _CountyNo) { _CountyNo = value; InvokePropertyChanged("CountyNo"); } } }
        public double EW { get { return _EW; } set { if (value != _EW) { _EW = value; InvokePropertyChanged("EW"); } } }
        public string Id { get { return _Id; } set { if (value != _Id) { _Id = value; InvokePropertyChanged("Id"); } } }
        public int MaxWindIconId { get { return _MaxWindIconId; } set { if (value != _MaxWindIconId) { _MaxWindIconId = value; InvokePropertyChanged("MaxWindIconId"); } } }
        public string MeasurePoint { get { return _MeasurePoint; } set { if (value != _MeasurePoint) { _MeasurePoint = value; InvokePropertyChanged("MeasurePoint"); } } }
        public DateTime MeasureTime { get { return _MeasureTime; } set { if (value != _MeasureTime) { _MeasureTime = value; InvokePropertyChanged("MeasureTime"); } } }
        public double NS { get { return _NS; } set { if (value != _NS) { _NS = value; InvokePropertyChanged("NS"); } } }
        public int PrecipitationIconId { get { return _PrecipitationIconId; } set { if (value != _PrecipitationIconId) { _PrecipitationIconId = value; InvokePropertyChanged("PrecipitationIconId"); } } }
        public double RoadTemp { get { return _RoadTemp; } set { if (value != _RoadTemp) { _RoadTemp = value; InvokePropertyChanged("RoadTemp"); } } }
        public int RoadTempIconId { get { return _RoadTempIconId; } set { if (value != _RoadTempIconId) { _RoadTempIconId = value; InvokePropertyChanged("RoadTempIconId"); } } }
        public int StationIconId { get { return _StationIconId; } set { if (value != _StationIconId) { _StationIconId = value; InvokePropertyChanged("StationIconId"); } } }
        public int WindIconId { get { return _WindIconId; } set { if (value != _WindIconId) { _WindIconId = value; InvokePropertyChanged("WindIconId"); } } }
        public int ZoomLevel { get { return _ZoomLevel; } set { if (value != _ZoomLevel) { _ZoomLevel = value; InvokePropertyChanged("ZoomLevel"); } } }
        public double Lat { get { return _Lat; } set { if (value != _Lat) { _Lat = value; InvokePropertyChanged("Lat"); } } }
        public double Lng { get { return _Lng; } set { if (value != _Lng) { _Lng = value; InvokePropertyChanged("Lng"); } } }
        public double AverageWindForce { get { return _AverageWindForce; } set { if (value != _AverageWindForce) { _AverageWindForce = value; InvokePropertyChanged("AverageWindForce"); } } }
        public double MaxWindForce { get { return _MaxWindForce; } set { if (value != _MaxWindForce) { _MaxWindForce = value; InvokePropertyChanged("MaxWindForce"); } } }
        public double WindForce { get { return _WindForce; } set { if (value != _WindForce) { _WindForce = value; InvokePropertyChanged("WindForce"); } } }
        public double Moisture { get { return _Moisture; } set { if (value != _Moisture) { _Moisture = value; InvokePropertyChanged("Moisture"); } } }

        private bool _Active { get; set; }
        private double _AirTemp { get; set; }
        private int _AirTempIconId { get; set; }
        private int _AverageWindIconId { get; set; }
        private double _AmountOfPrecipitation { get; set; }
        private int _CountyNo { get; set; }
        private double _EW { get; set; }
        private string _Id { get; set; }
        private int _MaxWindIconId { get; set; }
        private string _MeasurePoint { get; set; }
        private DateTime _MeasureTime { get; set; }
        private double _NS { get; set; }
        private int _PrecipitationIconId { get; set; }
        private double _RoadTemp { get; set; }
        private int _RoadTempIconId { get; set; }
        private int _StationIconId { get; set; }
        private int _WindIconId { get; set; }
        private int _ZoomLevel { get; set; }
        private double _Lat { get; set; }
        private double _Lng { get; set; }
        private double _AverageWindForce { get; set; }
        private double _MaxWindForce { get; set; }
        private double _WindForce { get; set; }
        private double _Moisture { get; set; }
        

        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<string, object> oldvalues { get; private set; }
        private void InvokePropertyChanged(string p)
        {
            if (oldvalues == null)
                oldvalues = new Dictionary<string, object>();

            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(p));

            if (oldvalues.ContainsKey(p))
                oldvalues[p] = this.GetType().GetProperty(p).GetValue(this);
            else
                oldvalues.Add(p, this.GetType().GetProperty(p).GetValue(this));

        }



    }
}
