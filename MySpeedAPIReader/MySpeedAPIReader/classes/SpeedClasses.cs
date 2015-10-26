using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MySpeedAPIReader
{
    public class BestRaces
    {
        public string car { get; set; }
        public string driver { get; set; }
        public string eventName { get; set; }
        public double? length { get; set; }
    }
    public class worldLeaderboardEvents
    {
        public List<Event> Events { get; set; }
        public DateTime LastUpdate { get; set; }
    }
    public class worldLeaderboardsResult
    {
        public string eventName { get; set; }
        public string len { get; set; }
        public string avgSpeed { get {
            double dblspeed = double.Parse(((length / idur) * 3600000).ToString());
            return Math.Round(dblspeed,2).ToString() + " km/h";
        } }
        public string duration { get { TimeSpan ts = new TimeSpan(0, 0, 0, 0, idur);
        return string.Format("{0}:{1}.{2}", ts.Minutes.ToString().PadLeft(2, '0'), ts.Seconds.ToString().PadLeft(2, '0'), ts.Milliseconds.ToString().PadLeft(3, '0'));
        }
        }
        public string type { get; set; }
        public string mode { get; set; }
        public string driver { get; set; }
        public string car { get; set; }
        public string carclass { get; set; }
        public DateTime date { get; set; }
        public double? length { get; set; }
        public int idur { get; set; }
    }
    public class worldLeaderboards
    { 
        public Event Event {get;set;}
        public int eventType { get; set; }
        public int eventDurationMilliseconds { get; set; }
        public worldCars car {get;set; }
        public Driver persona { get; set; }
        public worldLastLogin createdDate { get; set; }
    }

    public class EventWithImage
    {
        public int? eventId { get; set; }
        public string eventName { get; set; }
        public string mode { get; set; }
        public string length { get; set; }
        public int? laps { get; set; }
        public Image imagemin { get; set; }
        public Image imagemax { get; set; }
    }
    public class Event
    {
        public int? eventId{get;set;}
        public string eventName{get;set;}
        public double? length{get;set;}
        public int? laps{get;set;}
        public int? minTier{get;set;}
        public int? maxTier{get;set;}
        public int? eventModeId { get; set; }
    }
    public class Driver
    {
        public List<worldCars> Cars { get; set; }
        public worldStats Stats { get; set; }
        public worldDriverProfile DriverProfile { get; set; }
        public worldLastLogin LastLogin { get; set; }
    }
    public class worldLastLogin
    {
        public int date { get; set; }
        public int day { get; set; }
        public int hours { get; set; }
        public int minutes { get; set; }
        public int month { get; set; }
        public int seconds { get; set; }
        public long time { get; set; }
        public int timezoneOffset { get; set; }
        public int year { get; set; }
        public DateTime dateTime
        {
            get
            {
                DateTime dt = new DateTime(year + 1900, month + 1, date, hours+timezoneOffset, minutes, seconds);
                return dt;
            }
            set {
                year = value.Year - 1900;
                month = value.Month - 1;
                date = value.Day;
                hours = value.Hour;
                minutes = value.Minute;
                seconds = value.Second;
                timezoneOffset = 0;
            }
        }
    }
    public class worldDriverProfile
    {
        public int image { get; set; }
        public int level { get; set; }
        public string name { get; set; }
        public long personaId { get; set; }
    }
    public class worldCars
    {
        public string carName { get; set; }
        public string make { get; set; }
        public physicsProfile physicsProfile { get; set; }

    }
    public class physicsProfile
    {
        public int acceleration { get; set; }
        public int handling { get; set; }
        public int rating { get; set; }
        public int topSpeed { get; set; }
        public string carClass { get; set; }
    }
    public class worldStats
    {
        public List<eventStats> eventStats { get; set; }
        public List<skillStats> skillStats { get; set; }
        public List<streaksStats> streaksStats { get; set; }
    }
    public class eventStats
    {
        public double averageFinishingPlace { get; set; }
        public double multiPlayerAverageFinishingPlace { get; set; }
        public double multiPlayerDnfCount { get; set; }
        public double multiPlayerRacesLost { get; set; }
        public double multiPlayerRacesWon { get; set; }
        public double multiPlayerTotalPlacing { get; set; }
        public double singlePlayerAverageFinishingPlace { get; set; }
        public double singlePlayerDnfCount { get; set; }
        public double singlePlayerRacesLost { get; set; }
        public double singlePlayerRacesWon { get; set; }
        public double singlePlayerTotalPlacing { get; set; }
        public double totalRacesLost { get; set; }
        public double totalRacesWon { get; set; }
    }
    public class skillStats
    { }
    public class streaksStats
    {
        public double bestMultiPlayerWinStreak { get; set; }
        public double bestSinglePlayerWinStreak { get; set; }
        public double currentMultiPlayerWinStreak { get; set; }
        public double currentSinglePlayerWinStreak { get; set; }
    }
}
