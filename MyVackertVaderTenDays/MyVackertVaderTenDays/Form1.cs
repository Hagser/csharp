using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyVackertVaderTenDays
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void stuffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = @"http://www.vackertvader.se/nyk%C3%B6ping/10d/yr-smhi";

            WebClient wc = new WebClient();
            string html = wc.DownloadString(url);
            int istart = 0;
            cls clss = new cls() { Id = "ten-day-cell"};
            clss.children.Add(new cls() { Id = "ten-day-date" });
            clss.children.Add(new cls() { Id = "ten-day-temperature" });
            clss.children.Add(new cls() { Id = "ten-day-min-temperature" });
            clss.children.Add(new cls() { Id = "ten-day-wind" });
            clss.children.Add(new cls() { Id = "ten-day-rain" });
            clss.children.Add(new cls() { Id = "ten-day-windarrow-img" });
            List<TenDays> tds = new List<TenDays>();
            while (true)
            {
                istart = html.IndexOf("<table class=\"ten-day-table\">",istart+1);
                if (istart != -1)
                {
                    int iend = html.IndexOf("</table>", istart + 1);
                    if (iend > istart)
                    {
                        string tbl = html.Substring(istart, iend - istart);
                        string strtd = "";
                        int istarttd = tbl.IndexOf("class=\"" + clss.Id);
                        int iendtd = tbl.IndexOf("</td>", istarttd + 1);
                        strtd = tbl.Substring(istarttd, iendtd - istarttd);
                        while (!string.IsNullOrEmpty(strtd))
                        {
                            TenDays td = new TenDays();
                            foreach (cls c in clss.children)
                            {
                                string strdiv = "";
                                int istartdiv = strtd.IndexOf("class=\"" + c.Id);
                                if (istartdiv != -1)
                                {
                                    int istartenddiv = strtd.IndexOf(">", istartdiv) + 1;
                                    int ienddiv = strtd.IndexOf("</div>", istartenddiv);

                                    strdiv = strtd.Substring(istartenddiv, ienddiv - istartenddiv);
                                    if(strdiv.Contains("<img alt=\"vindpil\" class=\"ten-day-windarrow-img\""))
                                    {
                                        var apa = strdiv.Split('<');
                                        if (apa.Length > 1)
                                        {
                                            strdiv = apa[0];
                                            string wp = apa[1].Replace("img alt=\"vindpil\" class=\"ten-day-windarrow-img\" src=\"", "");
                                            var sll = wp.Split('"');
                                            td.windarrowimg = sll[0].Replace("http://static.vackertvader.se/images/arrows/blue/15/", "").Replace(".png", "");
                                        }
                                        // alt=\"vindpil\" class=\"ten-day-windarrow-img\" src=\"http://static.vackertvader.se/images/arrows/blue/15/315.png\" />", "");
                                    }

                                    strdiv = strdiv.Trim();
                                    strdiv = strdiv.Replace("&nbsp;", "");
                                    switch (c.Property)
                                    {
                                        case "date": td.date = strdiv; break;
                                        case "mintemperature": td.mintemperature = strdiv; break;
                                        case "rain": td.rain = strdiv; break;
                                        case "wind": td.wind = strdiv; break;
                                        case "temperature": td.temperature = strdiv; break;
                                    }
                                }
                            }
                            tds.Add(td);
                            istarttd = tbl.IndexOf(clss.Id, iendtd);
                            if (istarttd != -1)
                            {
                                iendtd = tbl.IndexOf("</td>", istarttd + 1);
                                strtd = tbl.Substring(istarttd, iendtd - istarttd);
                            }
                            else
                            {
                                strtd = "";
                            }

                        }
                    }
                }
                else
                {
                    break;
                }
            }
            dataGridView1.DataSource = tds;
        }
    }
    public class cls
    {
        private string _Id { get; set; }
        public string Id
        {
            get { return _Id; }
            set {
            Property = value.Replace("ten-day-", "").Replace("-", "");
            _Id = value;
        } }
        public string Property { get; set; }
        public List<cls> children { get; set; }
        public cls()
        {
            children = new List<cls>();
        }
    }
}
