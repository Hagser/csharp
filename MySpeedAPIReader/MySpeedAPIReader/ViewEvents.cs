using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Reflection;

namespace MySpeedAPIReader
{
    public partial class ViewEvents : Form
    {
        public ViewEvents(worldLeaderboardEvents _wle,worldLeaderboardEvents _twle)
        {
            InitializeComponent();
            wle = _wle;
            twle = _twle;
        }

        worldLeaderboardEvents wle = new worldLeaderboardEvents();
        worldLeaderboardEvents twle = new worldLeaderboardEvents();
        string classimgbase = "MySpeedAPIReader.images.car_class_";
        private void ViewEvents_Load(object sender, EventArgs e)
        {
            events.Items.AddRange(wle.Events.OrderBy(x => x.eventName).Select(x => x.eventName).ToArray<string>());
            eventmodes.Items.AddRange(wle.Events.Select(x => getEventModeName(x.eventModeId)).Distinct().ToArray<string>());
            
            IEqualityComparer<Event> emxt = new EventMaxTierComparer();
            maxTiers.Items.AddRange(wle.Events.Distinct(emxt).OrderBy(x => x.maxTier.Value).Select(x => getTierToClass(x.maxTier.Value)).ToArray<string>());
            minTiers.Items.AddRange(maxTiers.Items);

        }
        public string getTierToClass(int tier) {
            var num = tier;
            if (num > 749)
                return "S";
            if (num > 599)
                return "A";
            if (num > 499)
                return "B";
            if (num > 399)
                return "C";
            if (num > 249)
                return "D";
            return "E";
        }
        private void events_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getFilteredList();
        }

        private void eventmodes_SelectedIndexChanged(object sender, EventArgs e)
        {
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
        private List<EventWithImage> getFilteredList()
        {
            try
            {
                var list = wle.Events.Where(x =>
                    (events.CheckedItems.Count == 0 || events.CheckedItems.Contains(x.eventName)) &&
                    (maxTiers.CheckedItems.Count == 0 || maxTiers.CheckedItems.Contains(getTierToClass(x.maxTier.Value))) &&
                    (minTiers.CheckedItems.Count == 0 || minTiers.CheckedItems.Contains(getTierToClass(x.minTier.Value))) &&
                    (eventmodes.CheckedItems.Count == 0 || eventmodes.CheckedItems.Contains(getEventModeName(x.eventModeId)))
                    ).Select(x => new EventWithImage()
                    {
                        eventId = x.eventId,
                        eventName = x.eventName,
                        mode = getEventModeName(x.eventModeId),
                        length = x.length.HasValue ? Math.Round(x.length.Value, 2).ToString() + " km" : "?",
                        laps = x.laps,
                        imagemin = Bitmap.FromStream(getImage(this.classimgbase + getTierToClass(x.minTier.Value).ToLower() + ".png")),
                        imagemax = Bitmap.FromStream(getImage(this.classimgbase + getTierToClass(x.maxTier.Value).ToLower() + ".png")),
                    });
                    
                    #region ordering
                    
                    if (bAsc)
                    {
                        switch (ColumnIndex)
                        {
                            case 0:
                                list = list.OrderBy(x => x.eventId);
                                break;
                            case 1:
                                list = list.OrderBy(x => x.eventName);
                                break;
                            case 2:
                                list = list.OrderBy(x => x.mode);
                                break;
                            case 3:
                                list = list.OrderBy(x => double.Parse(x.length.Replace(" km", "")));
                                break;
                            case 4:
                                list = list.OrderBy(x => x.laps);
                                break;
                        }
                    }
                    else
                    {
                        switch (ColumnIndex)
                        {
                            case 0:
                                list = list.OrderByDescending(x => x.eventId);
                                break;
                            case 1:
                                list = list.OrderByDescending(x => x.eventName);
                                break;
                            case 2:
                                list = list.OrderByDescending(x => x.mode);
                                break;
                            case 3:
                                list = list.OrderByDescending(x => double.Parse(x.length.Replace(" km", "")));
                                break;
                            case 4:
                                list = list.OrderByDescending(x => x.laps);
                                break;
                        }
                    }
                    #endregion

                return list != null ? list.ToList() : new List<EventWithImage>();
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            return new List<EventWithImage>();
        }

        private System.IO.Stream getImage(string p)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            return _assembly.GetManifestResourceStream(p);
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

        private void chkTodays_Click(object sender, EventArgs e)
        {
            if (chkTodays.Checked)
            {
                for (int i = 0; i < events.Items.Count; i++)
                {
                    events.SetItemChecked(i, twle.Events != null && twle.Events.Any(x => x.eventName.Equals(events.Items[i].ToString(), StringComparison.InvariantCultureIgnoreCase)));
                }
            }
            else 
            {
                for (int i = 0; i < events.Items.Count; i++)
                {
                    events.SetItemChecked(i, false);
                }
            }
            dataGridView1.DataSource = getFilteredList();
        }

        private void minTiers_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getFilteredList();
        }

        private void maxTiers_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getFilteredList();
        }


    }
}
