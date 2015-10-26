using System;
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
using System.Drawing;
using System.Net;
using System.IO;
namespace MyOPMLReader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ArrayList oldlinks = new ArrayList();
        bool bRunning = false;

        private void GetOPML(string p)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.Load(p);
                if (xd.SelectNodes("//body/outline").Count > 0)
                {
                    AddChildNodes(xd.SelectNodes("//body/outline"), treeView1);
                }
                else if (xd.SelectNodes("//channel/item").Count > 0)
                {
                    listView1.Items.Clear();
                    GetRSS(p, true);
                    //http://www.podiobooks.com/title/ancestor/feed
                    //http://www.podiobooks.com/title/earthcore/feed
                }
            }
            catch { }
        }
        private TreeNode AddTreeNode(string text, string url, TreeView p)
        {
            TreeNode tn = p.Nodes.Add(text);
            if (url != null)
            {
                tn.Tag = url;
                GetRSS(url, false);
            }
            return tn;
        }
        private TreeNode AddTreeNode(string text, string url, TreeNode p)
        {
            TreeNode tn = p.Nodes.Add(text);
            if (url != null)
            {
                tn.Tag = url;
                /*CallbackClass obj = new CallbackClass(url);
                ThreadPool.QueueUserWorkItem((WaitCallback)GetRSS, obj);
                bRunning = true;*/
            }
            return tn;
        }
        private void AddChildNodes(XmlNodeList xmlNodeList, TreeNode tn)
        {
            foreach (XmlNode xn in xmlNodeList)
            {
                AddChildNodes(xn, AddTreeNode((xn.Attributes.GetNamedItem("text") != null ? xn.Attributes.GetNamedItem("text").Value : ""), (xn.Attributes.GetNamedItem("url") != null ? xn.Attributes.GetNamedItem("url").Value : null),tn));
            }
        }
        private void AddChildNodes(XmlNodeList xmlNodeList, TreeView treeView)
        {
            foreach (XmlNode xn in xmlNodeList)
            {
                AddChildNodes(xn, AddTreeNode((xn.Attributes.GetNamedItem("text") != null ? xn.Attributes.GetNamedItem("text").Value : ""), (xn.Attributes.GetNamedItem("url") != null ? xn.Attributes.GetNamedItem("url").Value : null), treeView));
            }
        }
        private void AddChildNodes(XmlDocument xd, TreeView treeView)
        {
            foreach (XmlNode xn in xd.ChildNodes)
            {
                AddChildNodes(xn, AddTreeNode((xn.Attributes.GetNamedItem("text") != null ? xn.Attributes.GetNamedItem("text").Value : ""), (xn.Attributes.GetNamedItem("url") != null ? xn.Attributes.GetNamedItem("url").Value : null), treeView));
            }
        }
        private void AddChildNodes(XmlNode xp, TreeNode tn)
        {
            foreach (XmlNode xn in xp.ChildNodes)
            {
                AddChildNodes(xn, AddTreeNode((xn.Attributes.GetNamedItem("text") != null ? xn.Attributes.GetNamedItem("text").Value : ""), (xn.Attributes.GetNamedItem("url") != null ? xn.Attributes.GetNamedItem("url").Value : null), tn));
            }
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null && e.Node.Tag != null)
            {
                listView1.Items.Clear();
                GetRSS(e.Node.Tag.ToString(),true);
            }
        }

        private void GetRSS(object userstate)
        {
            CallbackClass obj = (CallbackClass)userstate;
            string p = obj.url;
            XmlDocument xd = new XmlDocument();
            xd.Load(p);
            foreach (XmlNode xn in xd.SelectNodes("//channel/item"))
            {
                bool bAdded = AddToArrayList(xn);
                if (bAdded)
                {
                    NewItemsFlag(p);
                }
            }
        }
        private void GetRSS(string p,bool bAddToListView)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.Load(p);
                foreach (XmlNode xn in xd.SelectNodes("//channel/item"))
                {
                    if (bAddToListView)
                    {
                        AddNodeToListView(xn);
                    }
                    else
                    {
                        bool bAdded = AddToArrayList(xn);
                        if (bAdded)
                        {
                            NewItemsFlag(p);
                        }
                    }
                }
            }
            catch { }
        }

        private void NewItemsFlag(string p)
        {
            foreach (TreeNode tn in treeView1.Nodes)
            {
                if (tn.Tag != null && tn.Tag.ToString().ToLower().Equals(p.ToLower()))
                {
                    if(!tn.Text.EndsWith("*"))
                        Invoke((MethodInvoker)delegate { tn.Text += "*"; });
                    break;
                }
                NewItemsFlag(p, tn);
            }
        }
        private void NewItemsFlag(string p, TreeNode ptn)
        {
            foreach (TreeNode tn in ptn.Nodes)
            {
                if (tn.Tag != null && tn.Tag.ToString().ToLower().Equals(p.ToLower()))
                {
                    if (!tn.Text.EndsWith("*"))
                        Invoke((MethodInvoker)delegate { tn.Text += "*"; });
                    break;
                }
                NewItemsFlag(p, tn);
            }
        }
        private bool AddToArrayList(XmlNode xn)
        { 
            string link = GetNodeValue(xn,"link");
            string enclosure = GetNodeAttributeValue(xn,"enclosure","url");
            string url = !string.IsNullOrEmpty(link) ? link : !string.IsNullOrEmpty(enclosure) ? enclosure : "";
            if (!oldlinks.Contains(url))
            {
                oldlinks.Add(url);
                return true;
            }
            return false;
        }
        private void AddNodeToListView(XmlNode xn)
        {
            string title = GetNodeValue(xn, "title").Replace("<![CDATA[", "").Replace("]]>", "");
            string description = GetNodeValue(xn, "description").Replace("<![CDATA[", "").Replace("]]>", "");
            string link = GetNodeValue(xn,"link");
            string guid = GetNodeValue(xn,"guid");
            string pubdate = GetNodeValue(xn,"pubdate");
            string enclosure = GetNodeAttributeValue(xn,"enclosure","url");

            ListViewItem lvi = listView1.Items.Add(title);
            lvi.SubItems.Add(pubdate);
            lvi.SubItems.Add(description);
            lvi.SubItems.Add(!string.IsNullOrEmpty(link) ? link : !string.IsNullOrEmpty(enclosure) ? enclosure : "");
            lvi.SubItems.Add("");
            lvi.SubItems.Add("");
            lvi.SubItems.Add("");
        }
        private XmlAttribute GetNodeAttribute(XmlNode xn, string p, string p_2)
        {
            XmlNode xn1 = GetNode(xn, p);
            if (xn1 != null)
            {
                foreach (XmlAttribute xatt in xn1.Attributes)
                { 
                    if(xatt.Name.ToLower().Equals(p_2.ToLower()))
                    {
                        return xatt;
                    }
                }
            }
            return null;
        }
        private string GetNodeAttributeValue(XmlNode xn, string p, string p_2)
        {
            XmlAttribute xatt = GetNodeAttribute(xn, p,p_2);
            if (xatt != null && xatt.Value!=null)
            {
                return xatt.Value;
            }
            return "";
        }
        private XmlNode GetNode(XmlNode xn, string p)
        {
            foreach (XmlNode cn in xn.ChildNodes)
            {
                if (cn.Name.ToLower().Equals(p.ToLower()))
                {
                    return cn;
                }
            }
            return null;
        }
        private string GetNodeValue(XmlNode xn, string p)
        {
            XmlNode xn1 = GetNode(xn, p);
            if (xn1 != null && xn1.InnerText!=null)
                return xn1.InnerText;
            return "";
        }

        private void tStxtUrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetOPML(tStxtUrl.Text);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (bRunning)
            {
                if (tSBlink.BackColor.Equals(SystemColors.ButtonFace))
                {
                    tSBlink.BackColor = SystemColors.ActiveCaption;
                }
                else
                {
                    tSBlink.BackColor = SystemColors.ButtonFace;
                }
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem lvi = listView1.SelectedItems[0];
                if (lvi.SubItems.Count >= 3 && lvi.SubItems[3].Text.StartsWith("http://"))
                {
                    string url = lvi.SubItems[3].Text;
                    axWindowsMediaPlayer1.currentMedia = axWindowsMediaPlayer1.newMedia(url);

                }
            }
        }

        private void playAllToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void playNewToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                string url = lvi.SubItems[3].Text;
                CallbackClass obj = new CallbackClass(url);
                ThreadPool.QueueUserWorkItem((WaitCallback)Download, obj);
            }
        }
        private void Download(object userstate)
        {
            CallbackClass cc = (CallbackClass)userstate;
            Uri uri = new Uri(cc.url);
            string strFilePath = CreateStructure(Application.CommonAppDataPath + "\\" + uri.LocalPath.Replace("/", "\\"));
            WebClient wc = new WebClient();
            ListViewItem lvi2;
            this.Invoke((MethodInvoker)delegate
                {
                    lvi2 = FindListViewItem(cc.url);
                    if (lvi2 != null)
                    {
                        lvi2.SubItems[6].Text = strFilePath;
                    }
                });
            wc.DownloadProgressChanged += (a, b) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    lvi2 = FindListViewItem(cc.url);
                    if (lvi2 != null)
                    {
                        //lvi2.SubItems[2].Text = b.BytesReceived.ToString();
                        lvi2.SubItems[5].Text = b.ProgressPercentage.ToString() + "%";
                        lvi2.SubItems[4].Text = (b.TotalBytesToReceive/1024).ToString()+"kb";
                    }
                });
            };
            wc.DownloadFileCompleted += (a, b) =>
            {
                this.Invoke((MethodInvoker)delegate { });
            };
            wc.DownloadFileAsync(uri, strFilePath);
        }
        private void addToPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private ListViewItem FindListViewItem(string p)
        {
            foreach (ListViewItem lvi in listView1.Items)
            {
                if (lvi.SubItems[3].Text.Equals(p))
                    return lvi;
            }
            return null;
        }

        private string CreateStructure(string p)
        {
            string[] parts = p.Split('\\');
            string strPath = "";
            for (int i = 0; i < parts.Length - 1; i++)
            {
                string part = parts[i];
                strPath += part + "\\";
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
            }
            return strPath + "\\" + parts[parts.Length - 1];
        }

    }
}
