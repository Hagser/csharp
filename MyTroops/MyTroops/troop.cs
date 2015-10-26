using System;
using System.Collections.Generic;
namespace MyTroops
{
    public class troop
    {
        public troop()
        {
            Troops = new List<troop>();
        }
        public DateTime LastUpdate { get; set; }
        public double HoursAgo { get { return new TimeSpan(DateTime.Now.Ticks - LastUpdate.Ticks).TotalHours; } set { } }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Money { get; set; }
        public int Power { get; set; }
        public List<troop> Troops { get; set; }
        public string Data { get; set; }
        public int NumTroops { get { return Troops != null ? Troops.Count : 0; } set { } }
        public int lastraid { get; set; }
        public int Cost { get; set; }
    }
}
