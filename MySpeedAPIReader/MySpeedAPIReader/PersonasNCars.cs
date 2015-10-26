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
    public partial class PersonasNCars : Form
    {
        public List<Driver> drivers = new List<Driver>();
        public PersonasNCars(List<Driver> _drivers)
        {
            InitializeComponent();
            drivers = _drivers;
        }
        public void UpdateList(List<Driver> _drivers)
        {
            drivers = _drivers;
            personas.DataSource = (drivers.Where(x => x.LastLogin != null).OrderByDescending(x => x.LastLogin.dateTime.Ticks).Select(x => new
                {
                    x.DriverProfile.name,
                    x.DriverProfile.level,
                    Avg = x.Stats.eventStats.Count > 0 ? x.Stats.eventStats[0].multiPlayerAverageFinishingPlace : 0,
                    Won = x.Stats.eventStats.Count > 0 ? x.Stats.eventStats[0].multiPlayerRacesWon : 0,
                    Lost = x.Stats.eventStats.Count > 0 ? x.Stats.eventStats[0].multiPlayerRacesLost : 0,
                    Total = x.Stats.eventStats.Count > 0 ? x.Stats.eventStats[0].multiPlayerTotalPlacing : 0,
                    LastLogon = x.LastLogin.dateTime.ToString("yyyy-MM-dd HH:mm")
                }).ToList());
        }
        private void PersonasNCars_Load(object sender, EventArgs e)
        {
            personas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            cars.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            personas.DataSource = (drivers.Where(x => x.LastLogin != null).OrderByDescending(x => x.LastLogin.dateTime.Ticks).Select(x => new
            {
                x.DriverProfile.name,
                x.DriverProfile.level,
                Avg = x.Stats.eventStats.Count > 0 ? x.Stats.eventStats[0].multiPlayerAverageFinishingPlace : 0,
                Won = x.Stats.eventStats.Count > 0 ? x.Stats.eventStats[0].multiPlayerRacesWon : 0,
                Lost = x.Stats.eventStats.Count > 0 ? x.Stats.eventStats[0].multiPlayerRacesLost : 0,
                Total = x.Stats.eventStats.Count > 0 ? x.Stats.eventStats[0].multiPlayerTotalPlacing : 0,
                LastLogon = x.LastLogin.dateTime.ToString("yyyy-MM-dd HH:mm")
            }).ToList());
        }
        private void personas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            cars.DataSource = (drivers.Where(d => d.DriverProfile.name.Equals(personas.Rows[personas.SelectedCells[0].RowIndex].Cells[0].Value.ToString())).First().Cars.Select(c => new { c.make, c.carName,carClass= c.physicsProfile.carClass.Replace("carclass_",""), c.physicsProfile.rating, c.physicsProfile.topSpeed, c.physicsProfile.acceleration, c.physicsProfile.handling }).OrderByDescending(c=>c.rating).ToList());
        }


        private void NewUser_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                drivers.Add(new Driver() { DriverProfile = new worldDriverProfile() { name = NewUser.Text }, LastLogin = new worldLastLogin() { dateTime = DateTime.Now } });
                NewUser.Text = "";
            }
        }

        private void PersonasNCars_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
