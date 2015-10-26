using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MyDeveloper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string strFile = string.Empty;
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                strFile = ofd.FileName;
                OpenFile(strFile);
            }
            btnCopy.Enabled = (listView1.Items.Count > 0 && Directory.Exists(lblTo.Text));
        }

        private void OpenFile(string strFile)
        {
            listView1.Items.Clear();
            var list = File.ReadAllLines(strFile);
            foreach (var row in list)
            {
                var lvi = listView1.Items.Add(row);
                lvi.SubItems.Add(File.Exists(row)+"");
                lvi.SubItems.Add("");
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void btnTo_Click(object sender, EventArgs e)
        {
            fbd.ShowNewFolderButton = true;

            fbd.SelectedPath = lblTo.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                lblTo.Text = fbd.SelectedPath;
            }
            btnCopy.Enabled = (listView1.Items.Count > 0 && Directory.Exists(lblTo.Text));
        }
        private void CopyFile(ListViewItem lvi, string from, string to)
        {
            lvi.SubItems[1].Text = from;
            lvi.SubItems[2].Text = to;
            if (IsFromLarger(from, to))
                File.Copy(from, to, true);
        }

        private bool IsFromLarger(string from, string to)
        {
            var fif = new FileInfo(from);
            var fit = new FileInfo(to);
            var ret = !fit.Exists || (fif.Length > fit.Length);
            return ret;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //R:\photos\CanonEos650D\photos\2014\04\27\IMG_7394.JPG
            //R:\photos\Samsung3\Camera\20131028_131846.jpg
            //R:\photos\s5
            //C:\Users\jh\Downloads\Fram\20131213_074301.jpg

            foreach (ListViewItem lvi in listView1.Items)
            {
                Application.DoEvents();
                lvi.EnsureVisible();
                var path = lvi.SubItems[0].Text;
                var fi = new FileInfo(path);
                var fn = fi.Name;
                var fo = fi.DirectoryName;
                var dp = lblTo.Text + "\\" + fn;
                if (fo.Contains("Canon"))
                {
                    fo = fo.Replace(@"\\ubuntu\share$\photos\CanonEos650D\photos", @"\\ubuntu\share$\photos\CanonEos650D\photos_org");
                    var np = fo + "\\" + fn;
                    if (File.Exists(np))
                    {
                        CopyFile(lvi,np, dp);
                    }
                    else
                    {
                        np = fi.DirectoryName + "\\" + fn;
                        CopyFile(lvi, np, dp);
                    }
                }
                else if (fo.Contains("Downloads\\Fram"))
                {
                    fo = fo.Replace(@"C:\Users\jh\Downloads\Fram", @"\\ubuntu\share$\photos\s5");
                    var np = fo + "\\" + fn;
                    if (File.Exists(np))
                    {
                        CopyFile(lvi, np, dp);
                    }
                    else
                    {
                        np = fi.DirectoryName + "\\" + fn;
                        CopyFile(lvi, np, dp);
                    }
                }
                else
                {
                    var np = fo + "\\" + fn;
                    if (File.Exists(np))
                    {
                        CopyFile(lvi, np, dp);
                    }
                }
            }
        }
    }
}
