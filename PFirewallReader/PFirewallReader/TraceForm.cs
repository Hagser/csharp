using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace PFirewallReader
{
    public partial class TraceForm : Form
    {
        Dictionary<IPAddress, IPHostEntry> iphost = new Dictionary<IPAddress, IPHostEntry>();
        public TraceForm(IPAddress ipaddress)
        {
            InitializeComponent();
            this.Text += " - " + ipaddress.ToString();
        }
        private void fixStuff()
        {
            try
            {
                dataGridView1.Columns.Remove(dataGridView1.Columns["Options"]);
                dataGridView1.Columns.Remove(dataGridView1.Columns["Buffer"]);
            }
            catch { }
            this.Width = dataGridView1.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + 70;
            int thisHeight = ((3+dataGridView1.Rows.Count) * dataGridView1.RowTemplate.Height);
            this.Height = Math.Min(thisHeight, Screen.PrimaryScreen.WorkingArea.Height);

        }
        private void TraceForm_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = this.Tag;
            fixStuff();
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            fixStuff();
        }

        private void TraceForm_Shown(object sender, EventArgs e)
        {
            fixStuff();
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                DataGridViewCell dcsrc = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (string.IsNullOrEmpty(dcsrc.ToolTipText))
                {
                    string Ip = dcsrc.Value.ToString();
                    IPAddress address;
                    if (IPAddress.TryParse(Ip, out address))
                    {
                        if (!iphost.ContainsKey(address))
                        {
                            try
                            {
                                IPHostEntry iph = System.Net.Dns.EndGetHostEntry(System.Net.Dns.BeginGetHostEntry(address, (a) =>
                                {
                                    if (a.IsCompleted)
                                    {
                                        string s = "";
                                    }
                                }, null));
                                iphost.Add(address, iph);
                                AddToolTipTextToIp(address, iph);
                            }
                            catch { }
                        }
                        else
                        {
                            AddToolTipTextToIp(address, iphost[address]);
                        }
                    }
                }
            }
        }

        private void AddToolTipTextToIp(IPAddress address, IPHostEntry iph)
        {
            string strAliases = "";
            foreach (string str in iph.Aliases)
            {
                strAliases += str + ",";
            }
            if (strAliases.EndsWith(","))
            {
                strAliases = strAliases.Substring(0, strAliases.Length - 1);
            }
            if (string.IsNullOrEmpty(strAliases))
            {
                strAliases = iph.HostName;
            }
            if (!address.ToString().Equals(strAliases))
            {
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    DataGridViewCell dcsrc = dr.Cells["Address"];
                    if (string.IsNullOrEmpty(dcsrc.ToolTipText) && address.ToString().Equals(dcsrc.Value.ToString()))
                    {
                        dcsrc.ToolTipText = strAliases;
                    }
                }
            }
        }
    }
}
