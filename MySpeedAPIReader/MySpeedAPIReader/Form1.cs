using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace MySpeedAPIReader
{
    public partial class Form1 : Form
    {
        string uribase = "http://world.needforspeed.com/SpeedAPI";
        string driveruribase = "/ws/game/nfsw/driver/";
        Dictionary<string, string> mypers = new Dictionary<string, string>();
        Dictionary<int, List<int>> stattypes = new Dictionary<int, List<int>>();
        List<string> uris = new List<string>();
        List<string> shards = new List<string>();
        worldLeaderboardEvents twle = new worldLeaderboardEvents();
        worldLeaderboardEvents wle = new worldLeaderboardEvents();
        List<worldLeaderboards> wllist = new List<worldLeaderboards>();
        List<Driver> drivers = new List<Driver>();
        bool bdebug = false;
        int oldcntev = 0;
        int oldcntlb = 0;
        int cntdone = 0;
        string strDownloadPath = Application.UserAppDataPath;
        bool bEventsChanged = false;
        bool bForce = false;

        public Form1()
        {
            InitializeComponent();
            //mypers.Add("APEX", "JOHANTH,HOJOH,PATRYK280XD,GOMEZKP2,VETEPASE" );
            //mypers.Add("CHICANE", "NILSLIN,QRCZAK91,RYSYSYKSYS,BASBASS,BUMBO72,LENGDA2");

            mypers.Add("APEX", "JOHANTH,HOJOH");
            mypers.Add("CHICANE", "NILSLIN,IKKEP");
            
            // Overall = 1, Daily = 2, Weekly = 3, Monthly = 4
            List<int> weeks = new List<int>();
            for (int i = 0; i < 53; i++)
            {
                weeks.Add(i);
            }
            stattypes.Add(3, weeks);

            List<int> months = new List<int>();
            for (int i = 1; i < 13; i++)
            {
                months.Add(i);
            }
            stattypes.Add(4, months);

            List<int> daily = new List<int>();
            for (int i = 0; i < 7; i++)
            {
                daily.Add(i);
            }
            stattypes.Add(2, daily);

            List<int> overall = new List<int>();
            overall.Add(0);
            stattypes.Add(1, overall);


            timer1.Interval = 15 * 60 * 1000;
            timer2.Interval = 30 * 60 * 1000;
        }

        private void shardStatus(object state)
        {
            this.Invoke((MethodInvoker)delegate
            {
                label4.Text = "";
            });
            WebClient wc = new WebClient();
            string uri = "/ws/game/nfsw/servers/status";
            try
            {
                string json = wc.DownloadString(uribase + uri + (uri.IndexOf("?") == -1 ? "?" : "&") + "output=json");
                dynamic obj = JObject.Parse(json);
                foreach (var ev in obj.worldServerStatus.worldServerStatusList)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        string sn = (!string.IsNullOrEmpty(label4.Text) ? "," : "") + ev.shard + ":" + ev.status;
                        label4.Text += sn;
                        if (sn.EndsWith("down", StringComparison.InvariantCultureIgnoreCase))
                            setRed(label4.Text);
                        else
                            setGreen();
                    });
                }
            }
            catch { }
            this.Invoke((MethodInvoker)delegate{ cntdone++; });
        }

        private void setGreen()
        {
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
            this.Text = "MySpeedApiReader";
        }

        private void setRed(string sn)
        {
            TaskbarManager.Instance.SetProgressValue(1, 1);
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Error);
            this.Text = sn;
        }
        private void loadShards(object state)
        {
            shards.Clear();
            WebClient wc = new WebClient();
            string uri = "/ws/game/nfsw/shard/list";
            try
            {
                string json = wc.DownloadString(uribase + uri + (uri.IndexOf("?") == -1 ? "?" : "&") + "output=json");
                dynamic obj = JObject.Parse(json);
                foreach (var ev in obj.shardList)
                {
                    string sn = ev.shardName;
                    shards.Add(sn);
                }
            }
            catch { }
            if (shards.Count == 0)
            {
                foreach (string k in mypers.Keys)
                {
                    shards.Add(k);
                }
            }
        }
        public void loadPersonas(object state)
        {
            WebClient wc = new WebClient();
            foreach (string shard in shards)
            {
                foreach (string pers in mypers[shard].Split(','))
                {
                    Driver d = drivers.Where(x => x.DriverProfile.name.Equals(pers)).FirstOrDefault();
                    if (d == null)
                    {
                        d = new Driver() { DriverProfile = new worldDriverProfile() { name = pers } };
                        drivers.Add(d);
                    }
                    string uri = "/ws/game/nfsw/driver/" + pers + "/profile?shard=" + shard;

                    try
                    {
                        string json = wc.DownloadString(uribase + uri + (uri.IndexOf("?") == -1 ? "?" : "&") + "output=json");
                        dynamic obj = JObject.Parse(json);
                        var ev = obj.worldDriverProfile;
                        d.DriverProfile.image= ev.image;
                        d.DriverProfile.level=ev.level;
                        d.DriverProfile.personaId=ev.personaId;
                    }
                    catch (Exception ex) { string s = ex.Message; }
                    
                    uri = "/ws/game/nfsw/driver/"+ pers +"/cars?shard="+shard;
                    try
                    {
                        string json = wc.DownloadString(uribase + uri + (uri.IndexOf("?") == -1 ? "?" : "&") + "output=json");
                    
                        dynamic obj = JObject.Parse(json);
                        d.Cars = new List<worldCars>();
                        foreach (var ev in obj.worldCars)
                        {
                            var c = new worldCars()
                                {
                                    carName = ev.carName,
                                    make = ev.make,
                                    physicsProfile = new physicsProfile()
                                    {
                                        acceleration = ev.physicsProfile.acceleration,
                                        carClass = ev.physicsProfile.carClass,
                                        handling = ev.physicsProfile.handling,
                                        rating = ev.physicsProfile.rating,
                                        topSpeed = ev.physicsProfile.topSpeed
                                    }
                                };
                            d.Cars.Add(c);
                        }
                    }
                    catch (Exception ex) { string s = ex.Message; }
                    
                    uri = "/ws/game/nfsw/driver/" + pers + "/stats?shard=" + shard;

                    try
                    {
                        string json = wc.DownloadString(uribase + uri + (uri.IndexOf("?") == -1 ? "?" : "&") + "output=json");
                        if(d.Stats==null)
                        {
                            d.Stats=new worldStats(){eventStats=new List<eventStats>(),skillStats=new List<skillStats>(),streaksStats=new List<streaksStats>()};
                        }
                        dynamic obj = JObject.Parse(json);
                        d.Stats.eventStats.Clear();
                        d.Stats.skillStats.Clear();
                        d.Stats.streaksStats.Clear();
                        foreach (var ev in obj.worldStats.eventStats)
                        {
                            d.Stats.eventStats.Add(new eventStats()
                            {
                                averageFinishingPlace=ev.averageFinishingPlace,
                                multiPlayerAverageFinishingPlace=ev.multiPlayerAverageFinishingPlace,
                                multiPlayerDnfCount=ev.multiPlayerDnfCount,
                                multiPlayerRacesLost=ev.multiPlayerRacesLost,
                                multiPlayerRacesWon=ev.multiPlayerRacesWon,
                                multiPlayerTotalPlacing=ev.multiPlayerTotalPlacing,
                                singlePlayerAverageFinishingPlace=ev.singlePlayerAverageFinishingPlace,
                                singlePlayerDnfCount=ev.singlePlayerDnfCount,
                                singlePlayerRacesLost=ev.singlePlayerRacesLost,
                                singlePlayerRacesWon=ev.singlePlayerRacesWon,
                                singlePlayerTotalPlacing=ev.singlePlayerTotalPlacing,
                                totalRacesLost=ev.totalRacesLost,
                                totalRacesWon=ev.totalRacesWon
                            });
                        }
                        /*foreach (var ev in obj.worldStats.skillStats)
                        {
                            
                        }*/
                        foreach (var ev in obj.worldStats.streaksStats)
                        {
                            d.Stats.streaksStats.Add(new streaksStats(){
                                bestMultiPlayerWinStreak=ev.bestMultiPlayerWinStreak,
                                bestSinglePlayerWinStreak=ev.bestSinglePlayerWinStreak,
                                currentMultiPlayerWinStreak=ev.currentMultiPlayerWinStreak,
                                currentSinglePlayerWinStreak=ev.currentSinglePlayerWinStreak
                            });

                        }
                    }
                    catch (Exception ex) { string s = ex.Message; }

                    uri = "/ws/game/nfsw/driver/" + pers + "/lastLogin?shard=" + shard;

                    try
                    {
                        string json = wc.DownloadString(uribase + uri + (uri.IndexOf("?") == -1 ? "?" : "&") + "output=json");
                        dynamic obj = JObject.Parse(json);
                        d.LastLogin = new worldLastLogin()
                        {
                            date = obj.worldLastLogin.date,
                            day = obj.worldLastLogin.day,
                            hours = obj.worldLastLogin.hours,
                            minutes = obj.worldLastLogin.minutes,
                            month = obj.worldLastLogin.month,
                            seconds = obj.worldLastLogin.seconds,
                            time = obj.worldLastLogin.time,
                            timezoneOffset = obj.worldLastLogin.timezoneOffset,
                            year = obj.worldLastLogin.year
                        };
                    }
                    catch (Exception ex) { string s = ex.Message; }
                }
            }
            this.Invoke((MethodInvoker)delegate { cntdone++; });
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            bEventsChanged = (oldcntev != wle.Events.Count());
            if (cntdone >= 4)
            {
                cntdone = 0;
                ThreadPool.QueueUserWorkItem(new WaitCallback(shardStatus));
                ThreadPool.QueueUserWorkItem(new WaitCallback(loadEvents));
                ThreadPool.QueueUserWorkItem(new WaitCallback(loadLeaderboards));
                ThreadPool.QueueUserWorkItem(new WaitCallback(loadPersonas));

                wle.LastUpdate = DateTime.Now;
            }
            label1.Text = "worldLeaderboardEvents:" + wle.Events.Count() + "" + (bEventsChanged ? "!+" + (wle.Events.Count() - oldcntev) : "");
            oldcntev = wle.Events.Count();

            bool bLeaderboardChanged = oldcntlb != wllist.Count();
            label2.Text = "worldLeaderboardsList:" + wllist.Count() + "" + (oldcntlb != wllist.Count() ? "!+" + (wllist.Count() - oldcntlb) : "");
            oldcntlb = wllist.Count();

            label3.Text = "lastupdate:" + wle.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss");
            if (bEventsChanged || bLeaderboardChanged)
            {
                if(ListChanged!=null)
                    ListChanged.Invoke(null,new ListChangeEventArgs(wle,wllist,drivers));
            }
        }


        public event EventHandler ListChanged;


        private void fixLeaderboards(object state)
        {
            var list = wllist.Where(x => string.IsNullOrEmpty(x.Event.eventName));
            foreach (worldLeaderboards wl in list)
            {
                Event ev = wle.Events.Where(x => x.eventId == wl.Event.eventId).FirstOrDefault();
                if (ev != null)
                {
                    wl.Event.eventModeId = ev.eventModeId;
                    wl.Event.eventName = ev.eventName;
                    wl.Event.laps = ev.laps;
                    wl.Event.length = ev.length;
                    wl.Event.maxTier = ev.maxTier;
                    wl.Event.minTier = ev.minTier;
                }
            }
        }

        private void loadLeaderboards(object state)
        {
            foreach (string shard in shards)
            {
                try
                {
                    foreach (Event ev in wle.Events)
                    {
                        string personas = mypers[shard];
                        {
                            var list = (bForce || bEventsChanged ? stattypes.Keys.ToList<int>() : new List<int>() { 2 });
                            foreach (int lt in list)
                            {
                                var list2 = (bForce || bEventsChanged ? stattypes[lt] : new List<int>() { 0, 1 });
                                foreach (int rf in list2)
                                {
                                    for (int et = 1; et < 3; et++)
                                    {
                                        string uri = uribase + string.Format("/ws/game/nfsw/leaderboards?et="+ et +"&eid={0}&lt=" + lt + "&rf=" + rf + "&dn={1}&shard={2}&output=json&now=", ev.eventId, personas, shard) + DateTime.Now.Ticks.ToString();
                                        try
                                        {
                                            HttpWebRequest hwrq = WebRequest.Create(uri) as HttpWebRequest;
                                            hwrq.Method = "POST";
                                            HttpWebResponse hwrs = hwrq.GetResponse() as HttpWebResponse;
                                            Stream s = hwrs.GetResponseStream();
                                            StreamReader sr = new StreamReader(s);
                                            string json = sr.ReadToEnd();
                                            dynamic wlbobj = JObject.Parse(json);
                                            if (wlbobj != null && wlbobj.worldLeaderboards != null)
                                            {
                                                foreach (dynamic obj in wlbobj.worldLeaderboards)
                                                {
                                                    if (obj != null)
                                                    {
                                                        worldLeaderboards wlb = new worldLeaderboards()
                                                        {
                                                            car = new worldCars() { carName = obj.carName, make = obj.make },
                                                            createdDate = new worldLastLogin() { date = obj.createdDate.date, day = obj.createdDate.day, hours = obj.createdDate.hours, minutes = obj.createdDate.minutes, month = obj.createdDate.month, seconds = obj.createdDate.seconds, time = obj.createdDate.time, timezoneOffset = obj.createdDate.timezoneOffset, year = obj.createdDate.year },
                                                            eventDurationMilliseconds = obj.eventDurationMilliseconds,
                                                            eventType = obj.eventType,
                                                            persona = new Driver() { DriverProfile = new worldDriverProfile() { name = obj.persona.personaName, level = obj.persona.level, personaId = obj.persona.personaId } },
                                                            Event = new Event() { eventId = obj.eventId }
                                                        };
                                                        if (!wllist.Any(x => x.createdDate.time == wlb.createdDate.time))
                                                            wllist.Add(wlb);
                                                    }
                                                }
                                            }
                                        }
                                        catch
                                        { }
                                    }
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {}
            }
            fixLeaderboards(null);
            this.Invoke((MethodInvoker)delegate { cntdone++; });
        }

        private void loadEvents(object state)
        {
            WebClient wc = new WebClient();
            foreach (string shard in shards)
            {
                string uri = "/ws/game/nfsw/leaderboards/events?shard=" + shard;
                try
                {
                    twle.Events.Clear();
                    string json = wc.DownloadString(uribase + uri + (uri.IndexOf("?") == -1 ? "?" : "&") + "output=json");
                    dynamic obj = JObject.Parse(json);
                    foreach (var ev in obj.worldLeaderboardEvents)
                    {
                        Event evnt = new Event()
                        {
                            eventId = ev.eventId,
                            eventModeId = ev.eventModeId,
                            eventName = ev.eventName,
                            laps = ev.laps,
                            length = ev.length,
                            maxTier = ev.maxTier,
                            minTier = ev.minTier
                        };
                        if (!wle.Events.Any(x => x.eventId == evnt.eventId))
                            wle.Events.Add(evnt);
                        if (!twle.Events.Any(x => x.eventId == evnt.eventId))
                            twle.Events.Add(evnt);
                        
                    }
                }
                catch { }
            }
            this.Invoke((MethodInvoker)delegate { cntdone++; });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadFromXml();
            timer1.Enabled = timer2.Enabled = !bdebug;
            oldcntev = wle.Events.Count(); oldcntlb = wllist.Count();
            ThreadPool.QueueUserWorkItem(new WaitCallback(loadShards));
            ThreadPool.QueueUserWorkItem(new WaitCallback(shardStatus));
            ThreadPool.QueueUserWorkItem(new WaitCallback(loadEvents));
            if (bForce || (!bdebug && (wle.LastUpdate > DateTime.MinValue && isLongAgo(wle.LastUpdate))))
            {
                wle.LastUpdate = DateTime.Now;
                ThreadPool.QueueUserWorkItem(new WaitCallback(loadLeaderboards));
                ThreadPool.QueueUserWorkItem(new WaitCallback(loadPersonas));
            }
            cntdone = 4;
            bEventsChanged=(oldcntev != wle.Events.Count());
            label1.Text = "worldLeaderboardEvents:" + wle.Events.Count() + "" + (bEventsChanged ? "!+" + (wle.Events.Count() - oldcntev) : "");
            oldcntev = wle.Events.Count();

            label2.Text = "worldLeaderboardsList:" + wllist.Count() + "" + (oldcntlb != wllist.Count() ? "!+" + (wllist.Count() - oldcntlb) : "");
            oldcntlb = wllist.Count();
            label3.Text = "lastupdate:" + wle.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private bool isLongAgo(DateTime dateTime)
        {
            TimeSpan tsStart = new TimeSpan(DateTime.Now.Ticks-dateTime.Ticks);
            return tsStart.TotalHours > 1;
        }

        private void loadFromXml()
        {
            if (File.Exists(strDownloadPath + "\\worldleaderboardevents.xml"))
            {
                string xml = File.ReadAllText(strDownloadPath + "\\worldleaderboardevents.xml");
                wle = (SeDes.ToObj(xml, wle) as worldLeaderboardEvents);
            }
            else
            {
                wle.Events = new List<Event>();
            }
            twle.Events = new List<Event>();
            if (File.Exists(strDownloadPath + "\\worldleaderboardlist.xml"))
            {
                string xml = File.ReadAllText(strDownloadPath + "\\worldleaderboardlist.xml");
                wllist = (SeDes.ToObj(xml, wllist) as List<worldLeaderboards>);
            }
            if (File.Exists(strDownloadPath + "\\drivers.xml"))
            {
                string xml = File.ReadAllText(strDownloadPath + "\\drivers.xml");
                drivers = (SeDes.ToObj(xml, drivers) as List<Driver>);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveToXml();
        }

        private void saveToXml()
        {
            string xmlser = SeDes.ToXml(wle);
            File.WriteAllText(strDownloadPath + "\\worldleaderboardevents.xml", xmlser);
            
            xmlser = SeDes.ToXml(wllist);
            File.WriteAllText(strDownloadPath + "\\worldleaderboardlist.xml", xmlser);

            xmlser = SeDes.ToXml(drivers);
            File.WriteAllText(strDownloadPath + "\\drivers.xml", xmlser);

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            saveToXml();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ViewData vd = new ViewData(wle, wllist);
            vd.StartPosition = FormStartPosition.Manual;
            vd.Location = new Point() { X = this.Location.X + 50, Y = this.Location.Y + 50 };

            vd.FormClosed += (a, b) => { this.Focus(); ListChanged = null; };
            ListChanged += (a, b) => { 
                if (b != null) {
                    var llll = b as ListChangeEventArgs;
                    vd.UpdateList(llll.wle, llll.wllist);
                } 
            };
            vd.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PersonasNCars pnc = new PersonasNCars(drivers);
            pnc.StartPosition = FormStartPosition.Manual;
            pnc.Location = new Point() { X = this.Location.X + 50, Y = this.Location.Y + 50 };

            pnc.FormClosed += (a, b) => { this.Focus(); ListChanged = null;
             foreach(Driver dr in pnc.drivers)
                {
                    if (!drivers.Any(x => x.DriverProfile != null && x.DriverProfile.name.ToLower().Equals(dr.DriverProfile.name.ToLower())))
                    {
                        drivers.Add(dr);
                    }
             }
            };
            ListChanged += (a, b) =>
            {
                if (b != null)
                {
                    var llll = b as ListChangeEventArgs;
                    pnc.UpdateList(llll.drivers);
                }
            };
            pnc.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1_Tick(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            ViewEvents ve = new ViewEvents(wle, twle);
            ve.StartPosition = FormStartPosition.Manual;
            ve.Location = new Point() { X = this.Location.X + 50, Y = this.Location.Y + 50 };

            ve.FormClosed += (a, b) =>
            {
                this.Focus(); ListChanged = null;
            };
            ve.Show();
        }
    }
}
