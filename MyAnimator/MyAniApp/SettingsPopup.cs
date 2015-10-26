using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyAniApp
{
    public partial class SettingsPopup : Form
    {
        public bool SubFolders { get;set; }
        public int EveryN { get; set; }
        public List<string> Files { get; set; }
        private string strDir { get; set; }
        private int imax { get; set; }
        public SettingsPopup(string _strDir, int _imax)
        {
            InitializeComponent();
            strDir = _strDir;
            imax = _imax;
            int i = 1;
            int.TryParse(textBox1.Text, out i);
            EveryN = i;
            this.Files = System.IO.Directory.EnumerateFiles(strDir, "*", (checkBox1.Checked ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly)).Take(imax).OrderBy(x => new FileInfo(x).LastWriteTime).ToList();

            double dir = double.Parse(this.Files.Count.ToString());
            this.Text = "SetPop " + dir;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 1;
            int.TryParse(textBox1.Text, out i);
            EveryN = i;
            SubFolders = checkBox1.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.Files = System.IO.Directory.EnumerateFiles(strDir, "*", (checkBox1.Checked ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly)).Take(imax).OrderBy(x => new FileInfo(x).LastWriteTime).ToList();

            double dir = double.Parse(this.Files.Count.ToString());
            this.Text = "SetPop " + dir;
        }
    }
}
