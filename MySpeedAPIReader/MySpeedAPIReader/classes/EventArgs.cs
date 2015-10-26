using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpeedAPIReader
{
    class ListChangeEventArgs:EventArgs
    {
        public worldLeaderboardEvents wle = new worldLeaderboardEvents();
        public List<worldLeaderboards> wllist = new List<worldLeaderboards>();
        public List<Driver> drivers = new List<Driver>();
        public ListChangeEventArgs(worldLeaderboardEvents _wle, List<worldLeaderboards> _wllist,List<Driver> _drivers)
        {
            wle = _wle;
            wllist = _wllist;
            drivers = _drivers;
        }
    }
}
