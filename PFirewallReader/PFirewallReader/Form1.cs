using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Net.NetworkInformation;
using System.Net;

namespace PFirewallReader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Dictionary<IPAddress, TraceForm> iptrace = new Dictionary<IPAddress, TraceForm>();
        Dictionary<IPAddress, IPHostEntry> iphost = new Dictionary<IPAddress, IPHostEntry>();
        Dictionary<string, ArrayList> dictFilter = new Dictionary<string, ArrayList>();
        bool bFilterApplied = false;
        FileInfo fi;
        DataSet ds = new DataSet();

        string strRealPath = Environment.SystemDirectory+@"\LogFiles\Firewall\pfirewall.log";
        string strPath = Environment.SystemDirectory+@"\LogFiles\Firewall\pfirewall.log";
        string strTitle = "PFirewall reader";
        DateTime dtOld = DateTime.MaxValue;
        DateTime dtNew = DateTime.Now;
        private void l(string p)
        {
            
            dtOld = dtNew;
            dtNew = DateTime.Now;
            Console.WriteLine(dtNew.Subtract(dtOld).TotalMilliseconds.ToString().PadLeft(10," ".ToCharArray()[0]) + "\t\t" + p);
        }
        private void RefreshLog()
        {
            try
            {
                progressBar1.Visible = true;
                Application.DoEvents(); l("RL1");
                fi = new FileInfo(strPath);
                using (FileStream fs = File.Open(strPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string[] lines = sr.ReadToEnd().Split("\n".ToCharArray());
                        DataTable dt = ds.Tables[0];
                        for (int r = dt.Rows.Count - 1; r < lines.Length - 1; r++)
                        {
                            if (r > 5)
                            {
                                if (r % 1000 == 0)
                                {
                                    Application.DoEvents(); l("RL2-" + r);
                                }
                                DataRow dr = dt.Rows.Add(lines[r].Replace("\r", "").Split(" ".ToCharArray()));
                                if (dr.ItemArray[2].ToString().StartsWith("DROP"))
                                {
                                    dr.SetColumnError(2, "Prohibited connection");
                                }
                                if (!dr.ItemArray[4].ToString().StartsWith("192.168"))
                                {
                                    dr.SetColumnError(4, "Someone's trying to connect");
                                }
                                if (dr.ItemArray[6].ToString().StartsWith("3389") && !string.IsNullOrEmpty(dr.GetColumnError(4)))
                                {
                                    dr.SetColumnError(6, "Remote desktop connection");
                                }
                                if (dr.ItemArray[7].ToString().StartsWith("3389"))
                                {
                                    dr.SetColumnError(7, "Remote desktop connection");
                                }

                            }
                        }
                    }

                    DataTable dt2 = ds.Tables[0];
                    this.Text = strTitle + " - " + dt2.Rows.Count;
                    dataGridView1.DataSource = dt2; l("RL3");
                    if (bFilterApplied)
                    {
                        ApplyFilter(); l("RL3-1");
                    }
                    if (goToEndToolStripMenuItem.Checked && dataGridView1.Rows.Count > 0)
                    {
                        dataGridView1.FirstDisplayedCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0];
                    }
                    int oldwidth = 0; l("RL4");
                    int oldleft = dataGridView1.RowHeadersWidth - dataGridView1.HorizontalScrollingOffset;
                    foreach (ComboBox cb in panelFilter.Controls)
                    {
                        cb.Width = dataGridView1.Columns[cb.Name].Width;
                        cb.Left = oldwidth + oldleft;
                        oldwidth = cb.Width;
                        oldleft = cb.Left;

                        ArrayList al = new ArrayList();
                        al.Add("");
                        if (cb.DataSource == null)
                        {
                            if (cb.Name != "time")
                            {
                                foreach (DataRow dr in dt2.Rows)
                                {
                                    if (al.Count > 200)
                                        break;
                                    if (!al.Contains(dr[cb.Name].ToString()))
                                    {
                                        al.Add(dr[cb.Name].ToString());
                                    }
                                }
                            }
                            l("RL4-1");
                            al.Sort(); l("RL4-2");
                            cb.DataSource = al;
                        }
                    }
                    l("RL4-3");
                    dataGridView1.Refresh(); l("RL4-4");
                    dataGridView1.Update(); l("RL4-5");
                }
            }
            catch { }
            progressBar1.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string[] strcolumns = "date time action protocol src-ip dst-ip src-port dst-port size tcpflags tcpsyn tcpack tcpwin icmptype icmpcode info path".Split(" ".ToCharArray());
            foreach (string strcol in strcolumns)
            {
                dt.Columns.Add(strcol,typeof(string));
                ComboBox cb = new ComboBox() { Name = strcol };
                cb.TextChanged += new EventHandler(cb_TextChanged);
                panelFilter.Controls.Add(cb);
            }
            ds.Tables.Add(dt);
            dataGridView1.AutoGenerateColumns = true;

            RefreshLog();
        }

        void cb_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string strFilter = "";
            foreach (ComboBox cb in panelFilter.Controls)
            {
                if (cb.Text.Length >= 2)
                {
                    strFilter += "[" + cb.Name + "]='" + cb.Text + "' and ";
                }
            }
            if (strFilter.EndsWith(" and "))
            {
                strFilter = strFilter.Substring(0, strFilter.Length - 4);
            }
            if (strFilter.Length > 2)
            {
                DataRow[] dr = ds.Tables[0].Select(strFilter);
                DataSet dsfilter = new DataSet();
                DataTable dtfilter = dsfilter.Tables.Add();
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    dtfilter.Columns.Add(dc.ColumnName);
                }
                foreach (DataRow row in dr)
                {
                    dtfilter.ImportRow(row);
                }
                dataGridView1.DataSource = dtfilter;
                int cnt = dr.Length;
                bFilterApplied = true;
            }
            else
            {
                dataGridView1.DataSource = ds.Tables[0];
                bFilterApplied = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            System.Net.NetworkInformation.IPGlobalProperties ipgp = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties();
            System.Net.NetworkInformation.TcpStatistics ts = ipgp.GetTcpIPv4Statistics();
            System.Net.NetworkInformation.UdpStatistics us = ipgp.GetUdpIPv4Statistics();
            ArrayList al = new ArrayList();
            ArrayList alus = new ArrayList();
            al.Add(ts);
            alus.Add(us);
            if (dataGridView6.DataSource == null || (dataGridView6.DataSource != null && !((ArrayList)dataGridView6.DataSource).Equals(alus)))
            {
                try
                {
                    dataGridView6.DataSource = alus;
                }
                catch { }
            }

            if (dataGridView5.DataSource == null || (dataGridView5.DataSource != null && !((ArrayList)dataGridView5.DataSource).Equals(al)))
            {
                try
                {
                    dataGridView5.DataSource = al;
                }
                catch { }
            }

            System.Net.IPEndPoint[] udplist = ipgp.GetActiveUdpListeners();
            if (dataGridView4.DataSource == null || (dataGridView4.DataSource != null && !((System.Net.IPEndPoint[])dataGridView4.DataSource).Equals(udplist)))
            {
                try
                {
                    dataGridView4.DataSource = udplist;
                }
                catch { }
            }
            System.Net.IPEndPoint[] tcplist = ipgp.GetActiveTcpListeners();
            if (dataGridView3.DataSource == null || (dataGridView3.DataSource != null && !((System.Net.IPEndPoint[])dataGridView3.DataSource).Equals(tcplist)))
            {
                try
                {
                    dataGridView3.DataSource = tcplist;
                }
                catch { }
            }
            System.Net.NetworkInformation.TcpConnectionInformation[] tci = ipgp.GetActiveTcpConnections();

            if (dataGridView2.DataSource == null || (dataGridView2.DataSource != null && !((System.Net.NetworkInformation.TcpConnectionInformation[])dataGridView2.DataSource).Equals(tci)))
            {
                try
                {
                    dataGridView2.DataSource = tci;
                }
                catch { }
            }
            

            FileInfo nfi = new FileInfo(strPath);
            
            if (nfi.LastWriteTime > fi.LastWriteTime)
            {
                RefreshLog();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (!bFilterApplied)
            {
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    DataGridViewRow dr = dataGridView1.SelectedRows[0];
                    if (dr.Cells["action"].Value.ToString().Equals("OPEN"))
                    {
                        for (int r = dr.Index + 1; r < dataGridView1.Rows.Count; r++)
                        {
                            DataGridViewRow drf = dataGridView1.Rows[r];
                            if (dr.Cells["protocol"].Value.Equals(drf.Cells["protocol"].Value) &&
                                dr.Cells["src-ip"].Value.Equals(drf.Cells["src-ip"].Value) &&
                                dr.Cells["dst-ip"].Value.Equals(drf.Cells["dst-ip"].Value) &&
                                dr.Cells["src-port"].Value.Equals(drf.Cells["src-port"].Value) &&
                                dr.Cells["dst-port"].Value.Equals(drf.Cells["dst-port"].Value)
                                )
                            {
                                drf.Selected = true;
                                break;
                            }
                        }
                    }
                    else if (dr.Cells["action"].Value.ToString().Equals("CLOSE"))
                    {

                        for (int r = dr.Index - 1; r > 0; r--)
                        {
                            DataGridViewRow drf = dataGridView1.Rows[r];
                            if (dr.Cells["protocol"].Value.Equals(drf.Cells["protocol"].Value) &&
                                dr.Cells["src-ip"].Value.Equals(drf.Cells["src-ip"].Value) &&
                                dr.Cells["dst-ip"].Value.Equals(drf.Cells["dst-ip"].Value) &&
                                dr.Cells["src-port"].Value.Equals(drf.Cells["src-port"].Value) &&
                                dr.Cells["dst-port"].Value.Equals(drf.Cells["dst-port"].Value)
                                )
                            {
                                drf.Selected = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                Clipboard.SetText(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            }
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            int oldwidth = 0;
            int oldleft = dataGridView1.RowHeadersWidth - dataGridView1.HorizontalScrollingOffset;
            foreach (ComboBox cb in panelFilter.Controls)
            {
                cb.Width = dataGridView1.Columns[cb.Name].Width;
                cb.Left = oldwidth + oldleft;
                oldwidth = cb.Width;
                oldleft = cb.Left;
            }
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PFirewall|PFirewall.*";
            openFileToolStripMenuItem.Checked = true;
            closeFileToolStripMenuItem.Checked = false;
            if (ofd.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(ofd.FileName) && File.Exists(ofd.FileName))
            {
                strPath = ofd.FileName;
                ds.Tables[0].Rows.Clear();
                foreach (ComboBox cb in panelFilter.Controls)
                {
                    cb.DataSource = null;
                }
                RefreshLog();
                timer1.Enabled = false;
            }
        }

        private void closeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileToolStripMenuItem.Checked = false;
            closeFileToolStripMenuItem.Checked = true;
            strPath = strRealPath;
            ds.Tables[0].Rows.Clear();
            foreach (ComboBox cb in panelFilter.Controls)
            {
                cb.DataSource = null;
            }
            RefreshLog();
            timer1.Enabled = true;

        }
        private void lookupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 1)
            {
                DataGridViewCell dc = dataGridView1.SelectedCells[0];
                DataGridViewColumn dgc = dataGridView1.Columns[dc.ColumnIndex];
                if (dgc.Name.Equals("src-ip") || dgc.Name.Equals("dst-ip"))
                {
                    string Ip = dc.Value.ToString();
                    if (!Ip.StartsWith("192.168") && !Ip.StartsWith("255.") && !Ip.Equals("239.255.255.250"))
                    {
                        
                            System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                            ping.PingCompleted += (a, b) =>
                            {
                                MessageBox.Show(Ip + "\n" + b.Reply.Status.ToString());
                            };
                            IPAddress address;
                            if (IPAddress.TryParse(Ip, out address))
                            {
                                if (!iptrace.ContainsKey(address))
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
                                    BackgroundWorker bw = new BackgroundWorker();
                                    bw.DoWork += (a, b) =>
                                        {
                                            IPAddress ipaddress = (IPAddress)b.Argument;
                                            TraceForm tf = new TraceForm(ipaddress);
                                            tf.Tag = Diag.PerformPathping(ipaddress, 50, 5000);
                                            iptrace.Add(address,tf);
                                            tf.ShowDialog();
                                        };
                                    bw.RunWorkerAsync(address);
                                }
                                else
                                {
                                    iptrace[address].ShowDialog();
                                }
                            }
                            //ping.SendAsync(dc.Value.ToString(),null);

                        
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
                    try
                    {
                        DataGridViewCell dcsrc = dr.Cells["src-ip"];
                        DataGridViewCell dcdst = dr.Cells["dst-ip"];
                        if (string.IsNullOrEmpty(dcsrc.ToolTipText) && address.ToString().Equals(dcsrc.Value.ToString()))
                        {
                            dcsrc.ToolTipText = strAliases;
                        }
                        if (string.IsNullOrEmpty(dcdst.ToolTipText) && address.ToString().Equals(dcdst.Value.ToString()))
                        {
                            dcdst.ToolTipText = strAliases;
                        }
                    }
                    catch (Exception ex)
                    { }
                }
            }
        }


        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex>-1 && e.ColumnIndex>-1 && dataGridView1.Columns[e.ColumnIndex].Name.ToLower().EndsWith("-ip"))
            {
                DataGridViewCell dcsrc = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (string.IsNullOrEmpty(dcsrc.ToolTipText))
                {
                    string Ip = dcsrc.Value.ToString();
                    if (!Ip.StartsWith("192.168") && !Ip.StartsWith("255.") && !Ip.Equals("239.255.255.250"))
                    {
                        IPAddress address;
                        if (IPAddress.TryParse(Ip, out address))
                        {
                            if (!iphost.ContainsKey(address))
                            {
                                try
                                {
                                    BackgroundWorker bw = new BackgroundWorker();
                                    bw.DoWork += (a1, b1) =>
                                    {
                                        try
                                        {
                                            IPHostEntry iph = System.Net.Dns.EndGetHostEntry(System.Net.Dns.BeginGetHostEntry(address, (a) =>
                                            {
                                                if (a.IsCompleted)
                                                {
                                                }
                                            }, null));
                                            if (!iphost.ContainsKey(address))
                                            {
                                                iphost.Add(address, iph);
                                            }
                                            AddToolTipTextToIp(address, iph);
                                        }
                                        catch { }
                                    };
                                    bw.RunWorkerAsync();
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
        }


    }
}
