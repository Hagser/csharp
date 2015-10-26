using System;

namespace MySpeedTest
{
    public class Speed
    {
        public DateTime When { get; set; }
        public Direction Direction { get; set; }
        public string Isp { get; set; }
        public string Server { get; set; }
        public long Bytes { get; set; }
        public double Speed1 { get; set; }
        public double Speed8 { get { return Speed1 * 8; } }
        public double Diff { get; set; }
        public Speed(Direction dir, double spe) { Direction = dir; Speed1 = spe; }
        public Speed(){}
    }
}
