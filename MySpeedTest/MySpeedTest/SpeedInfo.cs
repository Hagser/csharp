using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MySpeedTest
{
    public class SpeedInfo
    {
        public SpeedInfo()
        {
            Speeds = new List<Speed>();
        }
        public event EventHandler SpeedAdded;
        public void Add(double spe)
        {
            Add(Direction.UnKnown, spe);
        }

        public void Add(Direction dir,double spe)
        {
            Add(new Speed(dir,spe));
        }
        public void Add(Speed Speed)
        {
            Speeds.Add(Speed);
            if (SpeedAdded != null)
                SpeedAdded.Invoke(this, EventArgs.Empty);
        }
        public List<Speed> Speeds { get; set; }
        public double AverageSpeed
        {
            get { return Math.Round((Speeds.Sum(x=>x.Speed1) / Speeds.Count),2); }
        }
        public double AverageUpSpeed
        {
            get { return Math.Round((Speeds.Where(x=>x.Direction==Direction.Up).Sum(x => x.Speed1) / Speeds.Count), 2); }
        }
        public double AverageDownSpeed
        {
            get { return Math.Round((Speeds.Where(x => x.Direction == Direction.Down).Sum(x => x.Speed1) / Speeds.Count), 2); }
        }
    }
}
