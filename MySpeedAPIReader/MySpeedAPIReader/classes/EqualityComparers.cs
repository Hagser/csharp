using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpeedAPIReader
{
    class worldDriverProfileComparer : IEqualityComparer<worldDriverProfile>
    {
        #region IEqualityComparer<Contact> Members

        public bool Equals(worldDriverProfile x, worldDriverProfile y)
        {
            return x.name.Equals(y.name);
        }

        public int GetHashCode(worldDriverProfile obj)
        {
            return obj.name.GetHashCode();
        }

        #endregion
    }
    class worldCarsComparer : IEqualityComparer<worldCars>
    {
        #region IEqualityComparer<Contact> Members

        public bool Equals(worldCars x, worldCars y)
        {
            return (x.make + " " + x.carName).Equals(y.make + " " + y.carName);
        }

        public int GetHashCode(worldCars obj)
        {
            return (obj.make+" " +obj.carName).GetHashCode();
        }

        #endregion
    }
    class EventMaxTierComparer : IEqualityComparer<Event>
    {
        #region IEqualityComparer<Event> Members

        public bool Equals(Event x, Event y)
        {
            return (x.maxTier).Equals(y.maxTier);
        }

        public int GetHashCode(Event obj)
        {
            return (obj.maxTier).GetHashCode();
        }

        #endregion
    }
    class EventMinTierComparer : IEqualityComparer<Event>
    {
        #region IEqualityComparer<Event> Members

        public bool Equals(Event x, Event y)
        {
            return (x.minTier).Equals(y.minTier);
        }

        public int GetHashCode(Event obj)
        {
            return (obj.minTier).GetHashCode();
        }

        #endregion
    }
    class EventComparer : IEqualityComparer<Event>
    {
        #region IEqualityComparer<Contact> Members

        public bool Equals(Event x, Event y)
        {
            return (x.eventName).Equals(y.eventName);
        }

        public int GetHashCode(Event obj)
        {
            return (obj.eventName).GetHashCode();
        }

        #endregion
    }
    class BestRacesComparer : IEqualityComparer<BestRaces>
    {
        #region IEqualityComparer<Contact> Members

        public bool Equals(BestRaces x, BestRaces y)
        {
            return (x.eventName).Equals(y.eventName);
        }

        public int GetHashCode(BestRaces obj)
        {
            return (obj.eventName).GetHashCode();
        }

        #endregion
    }
    
}
