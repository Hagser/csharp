using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyFordonsInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.V)
                {
                    string str = Clipboard.GetText();
                    ReadText(str);

                }
            }
        }

        private void ReadText(string str)
        {
            Dictionary<string, string> veh = new Dictionary<string, string>();
            foreach (string hdr in Vehicle.headers)
            {
                if (hdr.Contains("{0}"))
                {
                    for (int i = 1; i < 10; i++)
                    {
                        AddVal(veh, String.Format(hdr, i), str);
                    }
                }
                else
                {
                    AddVal(veh, hdr, str);
                }
            }
            if(veh.Count>0)
                dicts.Add(veh);
            AddToListView(dicts);
        }

        private void AddToListView(List<Dictionary<string, string>> dicts)
        {
            listView1.Items.Clear();
            listView1.Columns.Clear();
            flowLayoutPanel1.Controls.Clear();
            foreach (var dict in dicts.OrderByDescending(x=>x.Keys.Count))
            {
                if (listView1.Columns.Count == 0)
                {
                    List<string> ks = new List<string>();
                    foreach (var d in dicts)
                        foreach (var k in d.Keys)
                            if (!ks.Contains(k) && (!selectedColumns.ContainsKey(k) || (selectedColumns.ContainsKey(k) && selectedColumns[k])))
                                ks.Add(k);

                    foreach (var k in ks)
                        listView1.Columns.Add(k);
                }

                var lvi = listView1.Items.Add(dict["Registreringsnummer"]);
                foreach (ColumnHeader k in listView1.Columns)
                    if (!k.Text.Equals("Registreringsnummer"))                        
                        lvi.SubItems.Add(dict.ContainsKey(k.Text)?dict[k.Text]:"");

                Label lbl = new Label();
                lbl.Text = dict["Registreringsnummer"] + " " + dict["Fabrikat"];
                lbl.Margin = new System.Windows.Forms.Padding(0);
                
                lbl.Height = listView1.Items[0].Bounds.Height;
                flowLayoutPanel1.Controls.Add(lbl);
            }
        }
        List<Dictionary<string, string>> dicts = new List<Dictionary<string, string>>();
        private void AddVal(Dictionary<string, string> veh, string hdr, string str)
        {
            int ilen = hdr.Length + 2;
            int istart = str.IndexOf(hdr + ": ");
            if (istart != -1)
            {
                istart += ilen;
                int iend1 = str.IndexOf(" ", istart);
                int iend2 = str.IndexOf("\r", istart);
                int iend3 = str.IndexOf("\n", istart);
                int iend = iend3;// < iend1 ? iend2 : iend1;
                if (istart < iend)
                {
                    string val = str.Substring(istart, iend - istart);
                    foreach (string h in Vehicle.headers)
                    {
                        if (val.Contains(h + ": "))
                            val = val.Substring(0, val.IndexOf(h + ": "));
                    }
                    if(!veh.ContainsKey(hdr))
                        veh.Add(hdr, val.Trim());
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(selectedColumns.Count==0)
                foreach (string hdr in Vehicle.headers)
                    selectedColumns.Add(hdr, true);

            foreach (string fil in System.IO.Directory.EnumerateFiles(@"C:\Users\jh\Desktop\fordon\"))
                ReadText(PdfText.pdfText((fil)));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if ((listView1.GetItemRect(1).Left * -1) > listView1.Columns[0].Width + listView1.Columns[1].Width)
            {
                flowLayoutPanel1.Visible = true;
                flowLayoutPanel1.Width = listView1.Columns[0].Width + listView1.Columns[1].Width;
            }
            else
            {
                flowLayoutPanel1.Visible = false;
            }
        }
        Dictionary<string, bool> selectedColumns = new Dictionary<string, bool>();
        private void selectColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectColumns sc = new SelectColumns(selectedColumns);
            sc.FormClosing += (a, b) => {
                foreach (var k in sc.selectedColumns.Keys)
                    selectedColumns[k] = sc.selectedColumns[k];

                dicts.Clear();
                Form1_Load(this, EventArgs.Empty);
            };
            sc.ShowDialog();
        }

 


    }
}
