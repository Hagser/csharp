using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Collections;

namespace MyTroops
{
    public partial class Form1 : Form
    {
        const string urlbase = "http://{0}.minitroopers.com";
        const string hqbase = urlbase + "/hq";
        const string loginbase = urlbase + "/login";
        const string histbase = urlbase + "/history";
        const string oppbase = urlbase + "/b/opp";
        const string battlebase = urlbase + "/b/battle";
        const string missionbase = urlbase + "/b/mission?chk={1}";
        const string raidbase = urlbase + "/b/raid?chk={1}";
        const string trooperbase = urlbase + "/t/{1}";
        const string levelupbase = urlbase + "/t/{2}?levelup={1}";
        const string skillbase = urlbase + "/levelup/{3}?skill={1}&chk={2}";
        const string strList = "asterisks;rikast;hundar;hundra;hudar;urana;brusar;burar;barrs;burars;tjurarna;narrat;naturas;njurar;lorenjupanu;rikets sal;bulgur;alasd;corpis;toxicsh;sniperys;salmonz;riod;scorned;kalasnikovs;mef1rst;pekines;611an;87an;ambjorns;knasen;qwertyuioplkjhgfds;coolchicken;mirkw;91ank;nilskarlsson;nyaayn;brunulf;korvmojj;tankfort;fastnaienhiss;gronulf;torrvara;islaet;agatons;ddrdrrdrdrdr;zombot;zombotr;retsnom;retsnim;esproc;minionen;minionin;hejduk;kudjeh;realstuff;ffutslaer;tokmus;mustok;toksum;muskot;xarol;motive;leirbag;nahc;toobs;toob;dreddjohn;moderation;housekeeper;spareme;nhojder;suspension;noitaredom;dernojh;repeek;emeraps;noisnepsus;nhojdderds;rtobmoz;tobmoz;larox;sboot;bosot";

        const string strCosts = "1;1,2;5,3;15,4;32,5;55,6;88,7;129,8;181,9;243,10;320,11;401,12;498,13;609,14;733";
        const string strRikets = "rikets sal";


        List<KeyValuePair<int, int>> costs = new List<KeyValuePair<int, int>>();
        List<troop> list = strList.Split(';').Select(x => new troop() { Name = x }).ToList();
        
        bool bauto = false;
        public Form1(string[] args)
        {
            InitializeComponent();

            costs = strCosts.Split(',').Select(x => new KeyValuePair<int, int>(int.Parse(x.Split(';')[0]), int.Parse(x.Split(';')[1]))).ToList();

            if (args!=null && args.Length > 0)
            {
                if (args.Contains("auto"))
                {
                    bauto = true;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<troop> troops = new List<troop>();
            string filename = Application.CommonAppDataPath + "\\troops.xml";
            if (System.IO.File.Exists(filename))
            {
                troops = MySeDes.SeDes.LoadFromXml(filename, troops) as List<troop>;

                if (troops != null && troops.Count >= list.Count)
                {
                    list = troops;
                }
                else if (troops != null && troops.Count < list.Count)
                {
                    List<troop> temptroops = new List<troop>();
                    foreach (troop t in list)
                        if(!troops.Any(x=>x.Name.Equals(t.Name)))
                            temptroops.Add(t);
                    foreach (troop t in temptroops)
                        troops.Add(t);

                    list = troops;
                }
            }
            foreach (var t in list.OrderBy(x => x.Name).OrderByDescending(x=>x.Power))
            {
                ListViewItem lvi = listView1.Items.Add(t.Name);
                lvi.Tag=t;
                lvi.SubItems.Add("");
                lvi.SubItems.Add(t.Money);
                lvi.SubItems.Add(t.Power);
                lvi.SubItems.Add(t.NumTroops);

                int sumpower = t.Troops.Count > 0 ? t.Name.Equals(strRikets) ? t.Troops.Min(x => x.Power) : t.Troops.Max(x => x.Power) : 1;
                int cost = t.Troops.Count > 0 ? t.Name.Equals(strRikets) ? t.Troops.Min(x => x.Cost) : t.Troops.Max(x => x.Cost) : 0;
                lvi.SubItems.Add(sumpower);
                int remain = cost > 0 ? cost - t.Money : GetRemainder(sumpower, t.Money);
                lvi.SubItems.Add(remain);
                int iraid = t.lastraid;
                lvi.SubItems.Add(iraid);
                FixColor(remain, lvi, iraid);
            }
            GetMoneyAndPower();
            if (bauto)
            {
                icntarmies = listView1.Items.Count;
                foreach (ListViewItem i in listView1.Items)
                {
                    if (!i.Text.Equals(strRikets,StringComparison.InvariantCultureIgnoreCase))
                    {
                        ThreadPool.QueueUserWorkItem(Battle, i.Text);
                    }
                }            
            }
        }
        int icntarmies = 0;
        int icntdone = 0;
        private void GetMoneyAndPower()
        {
            GetMoneyAndPower(false);
        }
        private void GetMoneyAndPower(bool bforce)
        {
            if(listView1.SelectedItems.Count>0)
            {
                foreach (ListViewItem i in listView1.SelectedItems)
                {
                    var t = (i.Tag as troop);
                    if (bforce)
                    {       
                        t.LastUpdate = DateTime.MinValue;
                        foreach (var tr in t.Troops)
                        {
                            tr.LastUpdate = DateTime.MinValue;
                        }
                    }
                    if (t.HoursAgo > 5)
                        ThreadPool.QueueUserWorkItem(GetMoneyAndPower, i.Text);
                }
            }
            else
            {
                foreach (ListViewItem i in listView1.Items)
                {
                    var t = (i.Tag as troop);
                    if (t.HoursAgo > 5)
                        ThreadPool.QueueUserWorkItem(GetMoneyAndPower, i.Text);
                }
            }
        }
        private void GetMoneyAndPower(object state)
        {
            string army = state.ToString();
            string money = "";
            string power = "";
            string raid = "";
            List<string> troops = new List<string>();
            string swfp = "";
            using (WebClient wc = new WebClient())
            {
                try
                {

                    wc.DownloadString(string.Format(histbase, army.Replace(" ", "-")));

                    string strHtml = wc.DownloadString(string.Format(hqbase, army.Replace(" ", "-")));
                    if (strHtml.Contains("<div class=\"money\""))
                    {
                        string find1 = "<div class=\"money\"";
                        string find2 = ">";
                        string find3 = "</div>";

                        int istart1 = strHtml.IndexOf(find1);
                        int istart2 = strHtml.IndexOf(find2, istart1);
                        int istart = istart2 + 1;
                        int iend = strHtml.IndexOf(find3, istart);
                        if (iend > istart)
                        {
                            money = strHtml.Substring(istart, iend - istart);
                            money = money != null ? money.Trim() : "";
                        }
                    }
                    if (strHtml.Contains("<div class=\"power\""))
                    {
                        string find1 = "<div class=\"power\"";
                        string find2 = ">";
                        string find3 = "</div>";
                        int istart1 = strHtml.IndexOf(find1);
                        int istart2 = strHtml.IndexOf(find2, istart1);
                        int istart = istart2 + 1;
                        int iend = strHtml.IndexOf(find3, istart);
                        if (iend > istart)
                        {
                            power = strHtml.Substring(istart, iend - istart);
                            power = power != null ? power.Trim() : "";
                        }
                    }
                    if (strHtml.Contains("<h1 class=\"hbox\">Raid"))
                    {
                        string find1 = "<h1 class=\"hbox\">Raid";
                        string find3 = "</h1>";
                        int istart = strHtml.IndexOf(find1) + find1.Length;
                        int iend = strHtml.IndexOf(find3, istart);
                        if (iend > istart)
                        {
                            raid = strHtml.Substring(istart, iend - istart);
                            raid = raid != null ? raid.Replace("Phase","").Trim() : "";
                        }
                        if (string.IsNullOrEmpty(raid))
                            raid = "0";
                    }
                    if (strHtml.Contains("var ttips = ["))
                    {
                        string find1 = "var ttips = ";
                        string find2 = "[";
                        string find3 = "];";
                        int istart1 = strHtml.IndexOf(find1);
                        int istart2 = strHtml.IndexOf(find2, istart1);
                        int istart = istart2 + 1;
                        int iend = strHtml.IndexOf(find3, istart);
                        if (iend > istart)
                        {
                            string tt = strHtml.Substring(istart, iend - istart);
                            var rs = new string[] { "\r", "\n", "\t", "Show <strong>", "</strong>&rsquo;s stats sheet", "'", "  " };
                            foreach (string ssss in rs)
                            {
                                while (tt.Contains(ssss))
                                {
                                    tt = tt.Replace(ssss, " ");
                                }
                            }
                            troops = new List<string>(tt.Split(','));
                        }
                    }
                    if (strHtml.Contains("var swfp = { menu : false, flashvars :"))
                    {
                        string find1 = "var swfp = { menu : false, flashvars :";
                        string find2 = "'";
                        string find3 = "'";
                        int istart1 = strHtml.IndexOf(find1);
                        int istart2 = strHtml.IndexOf(find2, istart1);
                        int istart = istart2 + 1;
                        int iend = strHtml.IndexOf(find3, istart);
                        if (iend > istart)
                        {
                            string tt = strHtml.Substring(istart, iend - istart);
                            int rcnt = 0;

                            while (tt.Contains("%25"))
                            {
                                tt = tt.Replace("%25", "%");
                            }
                            foreach (string str in tt.Split('%'))
                            {
                                if (rcnt == 0||str.Length<2)
                                {
                                    rcnt++;
                                    continue;
                                }
                                int value = 0;
                                string stringValue = "";
                                try
                                {
                                    value = Convert.ToInt32(str.Substring(0, 2), 16);
                                    
                                    stringValue = Char.ConvertFromUtf32(value);
                                    if(value<32 || value>200)
                                        tt = tt.Replace("%" + str.Substring(0, 2), "");
                                    else
                                        tt = tt.Replace("%" + str.Substring(0, 2), stringValue);
                                }
                                catch (Exception ex)
                                {
                                    string sösdlfk = ex.Message;
                                }
                                rcnt++;
                            }
                            var rs = new string[] { 
                                "data=oy10:;", 
                                "Zjy10:ClientMode:",
                                "y8:VE|ny6:", 
                                "y6:An%6jy11:", 
                                "hy5:", 
                                "$PSy59:http://data.minitroopers.com/swf/mini.swf?v=1g&chk", 
                                "'", 
                                "  " };
                            foreach (string ssss in rs)
                            {
                                while (tt.Contains(ssss))
                                {
                                    tt = tt.Replace(ssss, " ");
                                }
                            }
                            swfp = tt;
                        }
                    }
                }
                catch (Exception ex)
                { }
            }

             UpdateMP(army, money, power, troops, swfp,raid); 
            
        }

        private void UpdateMP(object army, string money, string power, List<string> troops, string swfp, string raid)
        {
            var t = list.FirstOrDefault(x => x.Name.Equals(army));
            t.LastUpdate = DateTime.Now;
            if(!string.IsNullOrEmpty(money))
                t.Money = int.Parse(money);
            if (!string.IsNullOrEmpty(power))
                t.Power = int.Parse(power);
            if (!string.IsNullOrEmpty(swfp))
                t.Data = swfp.TrimStart().TrimEnd().Trim();
            if (troops != null && troops.Count() > 0 && t.Troops.Count < troops.Count(x => !x.Contains("Add a new")))
            {
                t.Troops = troops.Where(x => !x.Contains("Add a new")).Select(x => new troop() { Name = x.Trim() }).ToList();
                int icnt=0;
                foreach(troop tt in t.Troops)
                {
                    tt.Id = icnt;
                    icnt++;
                }
            }
            if (t.Troops.Count>0 && t.Troops.Min(x => x.HoursAgo) > 5)
            {
                int i = UpdateTrooperPower(army.ToString(), t.Troops);
            }
            int sumpower = t.Troops.Count > 0 ? t.Name.Equals(strRikets) ? t.Troops.Min(x => x.Power) : t.Troops.Max(x => x.Power) : 1;
            int cost = t.Troops.Count > 0 ? t.Name.Equals(strRikets) ? t.Troops.Min(x => x.Cost) : t.Troops.Max(x => x.Cost) : 0;
            //int imoney = string.IsNullOrEmpty(money) ? 0 : int.Parse(money);
            int iraid = string.IsNullOrEmpty(raid) ? -1 : int.Parse(raid);
            t.lastraid = iraid;
            int remain = cost > 0 ? cost - t.Money : GetRemainder(sumpower, t.Money);

            this.Invoke((MethodInvoker)delegate
            {
                ListViewItem lvi = listView1.Items.FirstOrDefault<ListViewItem>(x => x.Text.Equals(army));
                lvi.SubItems[2].Text = money;
                FixColor(remain, lvi, iraid);
                lvi.SubItems[3].Text = power;
                lvi.SubItems[4].Text = t.NumTroops.ToString();
                lvi.SubItems[5].Text = sumpower.ToString();
                lvi.SubItems[6].Text = remain.ToString();
                lvi.SubItems[7].Text = iraid.ToString();
                lvi.Tag = t;
            });
                
            /*
            var tr = list.FirstOrDefault(x => x.Name.Equals(t.Name));
            if (tr != null)
                tr = t;
            */

            GC.Collect();
        }

        private int GetRemainder(int sumpower, int imoney)
        {
            var kvp = costs.FirstOrDefault(x => x.Key == sumpower);
            return (kvp.Value - imoney);
        }

        private void FixColor(int remain, ListViewItem lvi, int iraid)
        {
            int irem = remain - (iraid > 0 ? (iraid - 1) * 4 : 0);

            if (irem < 16 && irem>4)
            {
                lvi.BackColor = Color.DarkOrange;
                lvi.ForeColor = Color.White;
            }
            else if (irem <= 4)
            {
                lvi.BackColor = Color.DarkGreen;
                lvi.ForeColor = Color.White;
            }
        }

        private int UpdateTrooperPower(string army, List<troop> troops)
        {
            int ip = 1;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    string strHtml = "";
                    if (army.Equals(strRikets))
                    {
                        strHtml = wc.DownloadString(string.Format(hqbase, army.Replace(" ", "-")));
                        if (strHtml.Contains("<form method=\"POST\" action=\"/login\">"))
                        {
                            if (wc.ResponseHeaders["Set-Cookie"] != null)
                            {
                                var c = wc.ResponseHeaders["Set-Cookie"];
                                wc.Headers.Add("Cookie", c.ToString());
                            }
                            HttpWebRequest wrq = WebRequest.Create(string.Format(loginbase, army.Replace(" ", "-"))) as HttpWebRequest;
                            wrq.Headers.Add(wc.Headers);
                            wrq.AllowAutoRedirect = false;
                            wrq.Method = "POST";
                            byte[] b = Encoding.Default.GetBytes(string.Format("login={0}&pass={1}", army.Replace(" ", "+"), army.Replace(" ", "")));
                            wrq.GetRequestStream().Write(b, 0, b.Length);
                            HttpWebResponse wrs = wrq.GetResponse() as HttpWebResponse;
                            if (wrs.GetResponseHeader("Set-Cookie") != null)
                            {
                                var c = wrs.GetResponseHeader("Set-Cookie");
                                wc.Headers.Clear();
                                wc.Headers.Add("Cookie", c.ToString());
                            }

                        }
                    }
                    int id = 0;
                    foreach (troop t in troops.Where(x=>x.HoursAgo>5))
                    {
                        string strUrl = string.Format(trooperbase, army.Replace(" ", "-"), id);

                        strHtml = wc.DownloadString(strUrl);

                        if (strHtml.Contains(">Upgrade\nfor\n"))
                        {
                            string find1 = ">Upgrade\nfor\n";
                            string find3 = "\n<";
                            int istart = strHtml.IndexOf(find1) + find1.Length;
                            int iend = strHtml.IndexOf(find3, istart);
                            if (iend > istart)
                            {
                                string cost = strHtml.Substring(istart, iend - istart);
                                int icost = 0;
                                if(int.TryParse(cost,out icost))
                                    t.Cost=icost;
                            }
                        }

                        int icnt = -1;
                        string strFind = "<li class=\"on\"";
                        while (strHtml.Contains(strFind))
                        {
                            int istart = strHtml.IndexOf(strFind);
                            if (istart > 0)
                            {
                                icnt++;
                                strHtml = strHtml.Substring(istart + 1, strHtml.Length - (istart + 1));
                            }
                            else
                            {
                                break;
                            }
                        }
                        t.Power = icnt;
                        t.LastUpdate = DateTime.Now;
                        id++;
                    }
                }
                catch(Exception ex) {
                    string s = ex.Message;
                    ip = -1;
                }
            }
            return ip;
        }
        private void battle_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listView1.SelectedItems)
            {
                ThreadPool.QueueUserWorkItem(Battle,i.Text);
            }
        }
        private void Battle(object state)
        {
            string strChk = "";
            string army = state.ToString();
            troop t = list.FirstOrDefault(x => x.Name.Equals(army));

            using (WebClient wc = new WebClient())
            {
                string strHtml = "";
                try
                {
                    strHtml = wc.DownloadString(string.Format(hqbase, army.Replace(" ", "-")));
                    if (strHtml.Contains("<form method=\"POST\" action=\"/login\">"))
                    {
                        if (wc.ResponseHeaders["Set-Cookie"] != null)
                        {
                            var c = wc.ResponseHeaders["Set-Cookie"];
                            wc.Headers.Add("Cookie", c.ToString());
                        }
                        HttpWebRequest wrq = WebRequest.Create(string.Format(loginbase, army.Replace(" ", "-"))) as HttpWebRequest;
                        wrq.Headers.Add(wc.Headers);
                        wrq.AllowAutoRedirect = false;
                        wrq.Method = "POST";
                        byte[] b = Encoding.Default.GetBytes(string.Format("login={0}&pass={1}", army.Replace(" ", "+"), army.Replace(" ", "")));
                        wrq.GetRequestStream().Write(b, 0, b.Length);
                        HttpWebResponse wrs = wrq.GetResponse() as HttpWebResponse;
                        if (wrs.GetResponseHeader("Set-Cookie") != null)
                        {
                            var c = wrs.GetResponseHeader("Set-Cookie");
                            wc.Headers.Clear();
                            wc.Headers.Add("Cookie", c.ToString());
                        }

                    }

                    strHtml = wc.DownloadString(string.Format(oppbase, army.Replace(" ", "-")));
                    if (wc.ResponseHeaders["Set-Cookie"] != null)
                    {
                        var c = wc.ResponseHeaders["Set-Cookie"];
                        wc.Headers.Clear();
                        wc.Headers.Add("Cookie", c.ToString());
                    }
                    if (strHtml.Contains("<input name=\"chk\""))
                    {
                        string find1 = "<input name=\"chk\" type=\"hidden\" value=\"";
                        string find2 = "\"/>";
                        int istart = strHtml.IndexOf(find1) + find1.Length;
                        int iend = strHtml.IndexOf(find2, istart);
                        if (iend > istart)
                        {
                            string chk = strHtml.Substring(istart, iend - istart);
                            strChk = chk;
                        }
                    }
                }
                catch (Exception ex)
                { }
                this.Invoke((MethodInvoker)delegate { UpdateDone(army, "M"); });
                try
                {
                    if (!string.IsNullOrEmpty(strChk))
                    {
                        foreach (var friend in list.OrderBy(x=>x.Power).Take(4))
                        {
                            if (!friend.Name.Equals(army, StringComparison.InvariantCultureIgnoreCase))
                            {
                                string ssdf = wc.UploadString(string.Format(battlebase, army.Replace(" ", "-")), "post", string.Format("chk={0}&friend={1}", strChk, friend.Name));
                            }
                        }
                        for (int n = 0; n < 3; n++)
                            wc.DownloadString(string.Format(missionbase, army.Replace(" ", "-"), strChk));
                    }
                }
                catch (WebException te)
                {}
                if (!string.IsNullOrEmpty(strChk))
                {
                    this.Invoke((MethodInvoker)delegate { UpdateDone(army, "R"); });
                    bool braid = true;
                    int iretry = 0;

                    while (braid && iretry < 3)
                    {
                        HttpWebRequest wrq = WebRequest.Create(string.Format(raidbase, army.Replace(" ", "-"), strChk)) as HttpWebRequest;
                        wrq.Headers.Add(wc.Headers);
                        wrq.Timeout = 60000;
                        wrq.ReadWriteTimeout = 60000;
                        wrq.AllowAutoRedirect = false;
                        try
                        {
                            HttpWebResponse wrs = wrq.GetResponse() as HttpWebResponse;

                            if (wrs.GetResponseHeader("Set-Cookie") != null)
                            {
                                var c = wrs.GetResponseHeader("Set-Cookie");
                                if (c.ToString().Contains("failed"))
                                    braid = false;
                            }
                            else
                            {
                                braid = false;
                            }
                            iretry = 0;
                        }
                        catch (WebException te)
                        {
                            Thread.Sleep(5000);
                            iretry++;
                            this.Invoke((MethodInvoker)delegate { UpdateDone(army, iretry.ToString()); });
                        }
                    }
                    
                    //if (!army.Equals(strRikets))
                    {
                        this.Invoke((MethodInvoker)delegate { UpdateDone(army, "L"); });
                        try
                        {
                            string strTrooper = t.Troops.Count > 0 ? (t.Troops.OrderBy(x => x.Power).First() as troop).Id.ToString() : "0";
                            string strT = (army.Equals(strRikets) || army.Equals("lorenjupanu") ? strTrooper : "0");
                            strHtml = wc.DownloadString(string.Format(levelupbase, army.Replace(" ", "-"), strChk, strT));
                            if (strHtml.Contains("document.location = '/levelup/"+ strT +"?skill="))
                            {
                                string find1 = "document.location = '/levelup/" + strT + "?skill=";
                                string find2 = "&chk=" + strChk;
                                int istart = strHtml.IndexOf(find1) + find1.Length;
                                int iend = strHtml.IndexOf(find2, istart);
                                if (iend > istart)
                                {
                                    string skill = strHtml.Substring(istart, iend - istart);
                                    wc.DownloadString(string.Format(skillbase, army.Replace(" ", "-"), skill, strChk,strT));
                                }
                            }
                        }
                        catch (WebException te)
                        { }
                    }
                }
            }
            GetMoneyAndPower(army);
            this.Invoke((MethodInvoker)delegate { UpdateDone(army,"X"); });
            icntdone++;
            if (icntarmies == icntdone + 1)
            {
                Battle(strRikets);
            }
            else if (icntarmies == icntdone)
            {
                this.Invoke((MethodInvoker)delegate { Application.Exit(); });
            }
        }

        private void UpdateDone(object army,string text)
        {
            foreach (ListViewItem lvi in listView1.Items)
                if (lvi.Text.Equals(army))
                {
                    lvi.SubItems[1].Text = text;
                    break;
                }
        }

        private void GetMP_Click(object sender, EventArgs e)
        {
            GetMoneyAndPower(true);
        }

        private void Goto_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listView1.SelectedItems)
            {
                Process.Start(string.Format(hqbase, i.Text.Replace(" ","-")));
            }
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {

        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.A)
                    foreach (ListViewItem lvi in listView1.Items)
                        lvi.Selected = true;
            }
        }

        private void btnCopyData_Click(object sender, EventArgs e)
        {
            string strAll = "";
            foreach(troop t in list.OrderByDescending(x=>x.Power))
            {
                strAll += t.Name + "\t\t\t" + t.Data + "\r\n";
            }
            Clipboard.SetText(strAll);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string filename = Application.CommonAppDataPath + "\\troops.xml";
            MySeDes.SeDes.SaveToXml(filename, list);
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                ListViewItem lvi = listView1.SelectedItems[0];
                var t = lvi.Tag as troop;
                dataGridView1.DataSource = t.Troops.Select(x => new { Id = x.Id, Name = x.Name, Power = x.Power }).ToList();
            }
        }

        private void btnRecruit_Click(object sender, EventArgs e)
        {
            var data = Microsoft.Win32.Registry.GetValue("HKEY_CLASSES_ROOT\\.htm", "", "");
            var data2 = Microsoft.Win32.Registry.GetValue(string.Format(@"HKEY_CLASSES_ROOT\{0}\shell\open\command", data), "", "");
            string prg = (data2 + "");
            prg = prg.Split('"')[prg.StartsWith("\"")?1:0];
            
            foreach (ListViewItem i in listView1.SelectedItems)
            {
                Process.Start(prg," --incognito \""+ string.Format(urlbase, i.Text.Replace(" ", "-"))+"\"");
            }
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            Process.Start(Application.CommonAppDataPath);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.V)
                {
                    if (Clipboard.ContainsText())
                    {
                        string tr = Clipboard.GetText();
                        tr = tr.Trim();
                        troop s = new troop() { Name = tr };
                        if (!list.Any(x => x.Name.Equals(tr)))
                        {
                            list.Add(s);
                            ListViewItem lvi = listView1.Items.Add(s.Name);
                            lvi.Tag = s;
                            lvi.SubItems.Add("");
                            lvi.SubItems.Add(s.Money);
                            lvi.SubItems.Add(s.Power);
                            lvi.SubItems.Add(0);
                            lvi.SubItems.Add(1);
                            lvi.SubItems.Add(0);
                            lvi.SubItems.Add("-1");//Raid
                        }
                    }
                }
            }

        }

    }
    public static class SubItemsExt
    {
        public static ListViewItem.ListViewSubItem Add(this ListViewItem.ListViewSubItemCollection e, int i)
        {
            return e.Add(i.ToString());
        }
        public static IEnumerable<ListViewItem> Where<ListViewItem>(this ListView.ListViewItemCollection e, Func<ListViewItem,bool> func)
        {
            return e.Cast<ListViewItem>().Where(func);
        }
        public static ListViewItem FirstOrDefault<ListViewItem>(this ListView.ListViewItemCollection e, Func<ListViewItem, bool> func)
        {
            return e.Cast<ListViewItem>().FirstOrDefault(func);
        }
    }
}
