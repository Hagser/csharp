using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySpeedAPIReader
{
    public partial class ViewData : Form
    {
        worldLeaderboardEvents wle = new worldLeaderboardEvents();
        List<worldLeaderboards> wllist = new List<worldLeaderboards>();
        string mypers = "JOHANTH,HOJOH,NILSLIN,ICBS4EVER";
        public ViewData(worldLeaderboardEvents _wle, List<worldLeaderboards> _wllist)
        {
            InitializeComponent();
            wle = _wle;
            wllist = _wllist;
        }
        public void UpdateList(worldLeaderboardEvents _wle, List<worldLeaderboards> _wllist)
        {
            wle = _wle;
            wllist = _wllist;
            IEqualityComparer<worldDriverProfile> wdpc = new worldDriverProfileComparer();
            IEqualityComparer<worldCars> wcc = new worldCarsComparer();

            var plist = new List<string>();
            foreach (string str in personas.CheckedItems)
            {
                plist.Add(str);
            }
            personas.Tag = plist;
            var elist = new List<string>();
            foreach (string str in events.CheckedItems)
            {
                elist.Add(str);
            }
            events.Tag = elist;
            var emlist = new List<string>();
            foreach (string str in eventmodes.CheckedItems)
            {
                emlist.Add(str);
            }
            eventmodes.Tag = emlist;
            var clist = new List<string>();
            foreach (string str in cars.CheckedItems)
            {
                clist.Add(str);
            }
            cars.Tag = clist;

            personas.Items.Clear();
            events.Items.Clear();
            eventmodes.Items.Clear();
            cars.Items.Clear();

            personas.Items.AddRange(wllist.Select(x => x.persona.DriverProfile).Distinct(wdpc).OrderBy(x => x.name).Select(x => x.name).ToArray<string>());
            events.Items.AddRange(wle.Events.OrderBy(x => x.eventName).Select(x => x.eventName).ToArray<string>());
            eventmodes.Items.AddRange(wle.Events.Select(x => getEventModeName(x.eventModeId)).Distinct().ToArray<string>());
            cars.Items.AddRange(wllist.Select(x => x.car).Distinct(wcc).OrderBy(x => (x.make + " " + x.carName)).Select(x => (x.make + " " + x.carName)).ToArray<string>());

            var arr = new[] { personas, events, eventmodes, cars };
            foreach (CheckedListBox o in arr)
            {
                for (int i = 0; i < o.Items.Count; i++)
                {
                    o.SetItemChecked(i, (o.Tag as List<string>).Contains(o.Items[i]));
                }
            }
            
            
            //fromDate.Value = wllist.Min(x => x.createdDate.dateTime).Date;
            //toDate.Value = wllist.Max(x => x.createdDate.dateTime).Date;

            dataGridView1.DataSource = getFilteredList();
        }
        private void personas_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getFilteredList();
        }

        private void events_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getFilteredList();
        }

        private void fromDate_ValueChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getFilteredList();
        }

        private void toDate_ValueChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getFilteredList();
        }

        private void eventmodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getFilteredList();
        }

        private void ViewData_Load(object sender, EventArgs e)
        {
            IEqualityComparer<worldDriverProfile> wdpc = new worldDriverProfileComparer();
            personas.Items.AddRange(wllist.Select(x => x.persona.DriverProfile).Distinct(wdpc).OrderBy(x=>x.name).Select(x=>x.name).ToArray<string>());

            events.Items.AddRange(wle.Events.OrderBy(x=>x.eventName).Select(x => x.eventName).ToArray<string>());

            eventmodes.Items.AddRange(wle.Events.Select(x => getEventModeName(x.eventModeId)).Distinct().ToArray<string>());

            IEqualityComparer<worldCars> wcc = new worldCarsComparer();
            cars.Items.AddRange(wllist.Select(x => x.car).Distinct(wcc).OrderBy(x => (x.make + " " + x.carName)).Select(x => (x.make + " " + x.carName)).ToArray<string>());

            fromDate.Value = wllist.Min(x => x.createdDate.dateTime).Date;
            toDate.Value = wllist.Max(x => x.createdDate.dateTime).Date;

            dataGridView1.DataSource = getFilteredList();
        }
        public string getEventModeName(int? id)
        {
            switch (id)
            {
                case 4: return "CIRCUIT";
                case 9: return "SPRINT";
                case 19: return "DRAG";
            }
            return "";
        }
        public string getEventTypeName(int? et)
        {
            switch (et)
            {
                case 1: return "MP";
                case 2: return "SP";
            }
            return "";
        }
        private List<worldLeaderboardsResult> getFilteredList()
        {
            try{
                var list = wllist.Where(x => x.createdDate.dateTime.Date >= fromDate.Value && x.createdDate.dateTime.Date <= toDate.Value &&
                    (eventTypes.CheckedItems.Count == 0 || eventTypes.CheckedItems.Contains(getEventTypeName(x.eventType))) &&
                    (events.CheckedItems.Count == 0 || events.CheckedItems.Contains(x.Event.eventName)) &&
                    (cars.CheckedItems.Count == 0 || cars.CheckedItems.Contains(x.car.make + " " + x.car.carName)) &&
                    (personas.CheckedItems.Count == 0 || personas.CheckedItems.Contains(x.persona.DriverProfile.name)) &&
                    (carclasses.CheckedItems.Count == 0 || (x.car!=null&&x.car.physicsProfile!=null&&x.car.physicsProfile.carClass!=null&& carclasses.CheckedItems.Contains(x.car.physicsProfile.carClass.Replace("carclass_", "")))) &&
                    (eventmodes.CheckedItems.Count == 0 || eventmodes.CheckedItems.Contains(getEventModeName(x.Event.eventModeId)))
                    //&& x.Event.length.HasValue && x.Event.laps.HasValue && x.Event.maxTier.HasValue && x.Event.minTier.HasValue && !string.IsNullOrEmpty(x.Event.eventName)
                    ).ToList()
                    .Select(x => new worldLeaderboardsResult()
                    {
                        len = x.Event.length.HasValue?Math.Round(x.Event.length.Value, 2).ToString() + " km":"?",
                        eventName = string.IsNullOrEmpty(x.Event.eventName) ? x.Event.eventId.ToString() : x.Event.eventName,
                        type = getEventTypeName(x.eventType),
                        length = x.Event.length.HasValue?x.Event.length:10,
                        idur = x.eventDurationMilliseconds,
                        driver = x.persona.DriverProfile.name,
                        car = x.car.make + " " + x.car.carName,
                        carclass = (x.car!=null&&x.car.physicsProfile!=null)?x.car.physicsProfile.carClass:"",
                        date = x.createdDate.dateTime,
                        mode = getEventModeName(x.Event.eventModeId)
                    });

                if (chkBestRaces.Checked)
                {
                    if (!chkMultiCars.Checked)
                    {
                        var oga = list.GroupBy(x => new { driver = x.driver, eventName = x.eventName, length = x.length });
                        var oga2 = oga.Select(x => new worldLeaderboardsResult() { len = Math.Round(x.Key.length.Value, 2).ToString() + " km", driver = x.Key.driver, eventName = x.Key.eventName, idur = x.Min(y => y.idur), length = x.Key.length });
                        var oga3 = new List<worldLeaderboardsResult>();
                        foreach (var w in oga2)
                        {
                            var wl = list.First(x => x.driver == w.driver && x.idur == w.idur && x.eventName == w.eventName);
                            w.car = wl.car;
                            w.carclass = wl.carclass;
                            w.date = wl.date;
                            w.mode = wl.mode;
                            oga3.Add(w);
                        }

                        list = oga3.OrderBy(x => x.idur).OrderBy(x => x.eventName);
                    }
                    else
                    {
                        var oga = list.GroupBy(x => new { driver = x.driver, eventName = x.eventName, length = x.length, car = x.car });
                        var oga2 = oga.Select(x => new worldLeaderboardsResult() { len = Math.Round(x.Key.length.Value, 2).ToString() + " km", driver = x.Key.driver, eventName = x.Key.eventName, idur = x.Min(y => y.idur), length = x.Key.length });
                        var oga3 = new List<worldLeaderboardsResult>();
                        foreach (var w in oga2)
                        {
                            var wl = list.First(x => x.driver == w.driver && x.idur == w.idur && x.eventName == w.eventName);
                            w.car = wl.car;
                            w.carclass = wl.carclass;
                            w.date = wl.date;
                            w.mode = wl.mode;
                            oga3.Add(w);
                        }

                        list = oga3.OrderBy(x => x.idur).OrderBy(x => x.eventName);
                    }
                }
                else
                {
                    #region ordering
                    if (bAsc)
                    {
                        switch (ColumnIndex)
                        {
                            case 0:
                                list = list.OrderBy(x => x.eventName);
                                break;
                            case 1:
                                list = list.OrderBy(x => x.length);
                                break;
                            case 2:
                                list = list.OrderBy(x => double.Parse(x.avgSpeed.Replace(" km/h", "")));
                                break;
                            case 3:
                                list = list.OrderBy(x => x.idur);
                                break;
                            case 4:
                                list = list.OrderBy(x => x.mode);
                                break;
                            case 5:
                                list = list.OrderBy(x => x.driver);
                                break;
                            case 6:
                                list = list.OrderBy(x => x.car);
                                break;
                            case 7:
                                list = list.OrderBy(x => x.date);
                                break;
                        }
                    }
                    else
                    {
                        switch (ColumnIndex)
                        {
                            case 0:
                                list = list.OrderByDescending(x => x.eventName);
                                break;
                            case 1:
                                list = list.OrderByDescending(x => x.length);
                                break;
                            case 2:
                                list = list.OrderByDescending(x => double.Parse(x.avgSpeed.Replace(" km/h", "")));
                                break;
                            case 3:
                                list = list.OrderByDescending(x => x.idur);
                                break;
                            case 4:
                                list = list.OrderByDescending(x => x.mode);
                                break;
                            case 5:
                                list = list.OrderByDescending(x => x.driver);
                                break;
                            case 6:
                                list = list.OrderByDescending(x => x.car);
                                break;
                            case 7:
                                list = list.OrderByDescending(x => x.date);
                                break;
                        }
                    }
                    #endregion
                }
                if (list != null)
                {
                    label1.Text = list.Count().ToString();
                }
                return list != null ? list.ToList() : new List<worldLeaderboardsResult>();
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            label1.Text = "0";
            return new List<worldLeaderboardsResult>();
        }

        int ColumnIndex = 0;
        bool bAsc = true;
        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (ColumnIndex == e.ColumnIndex)
            {
                bAsc = !bAsc;
            }
            else
            {
                bAsc = true;
            }
            ColumnIndex = e.ColumnIndex;

            dataGridView1.DataSource = getFilteredList();

        }

        private void events_MouseEnter(object sender, EventArgs e)
        {
            events.Height = 449;
            events.Focus();
        }

        private void events_MouseLeave(object sender, EventArgs e)
        {
            events.Height = 49;
        }

        private void personas_MouseEnter(object sender, EventArgs e)
        {
            personas.Height = 196;
            personas.Focus();
        }

        private void personas_MouseLeave(object sender, EventArgs e)
        {
            personas.Height = 49;
        }

        private void chkBestRaces_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getFilteredList();
            chkMultiCars.Enabled = chkBestRaces.Checked;
            if (!chkBestRaces.Checked)
                chkMultiCars.Checked = false;
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            return;
            foreach (DataGridViewRow r in dataGridView1.Rows)
            { 
                r.DefaultCellStyle=new DataGridViewCellStyle(){BackColor=(mypers.Contains(r.Cells["driver"].Value.ToString())?Color.DarkGreen:Color.White)};
            }
        }

        private void cars_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getFilteredList();
        }

        private void cars_MouseEnter(object sender, EventArgs e)
        {
            cars.Height = 249;
            cars.Focus();
        }

        private void cars_MouseLeave(object sender, EventArgs e)
        {
            cars.Height = 49;
        }

        private void chkMultiCars_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getFilteredList();
        }

        private void eventTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getFilteredList();
        }

        private void carclasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getFilteredList();
        }

    }
}
