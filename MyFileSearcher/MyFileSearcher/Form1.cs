using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MyFileSearcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int findSize
        {
            get
            {
                return string.IsNullOrEmpty(textBox1.Text) ? 0 : int.Parse(textBox1.Text);
            }
            set
            {
                textBox1.Text = value.ToString();
            }
        }
        bool _isprocessing;
        bool IsProcessing
        {
            get
            {
                return _isprocessing;
            }
            set
            {
                _isprocessing = value; progressBar1.Enabled = _isprocessing; progressBar1.Visible = _isprocessing;
            }
        }
        int ColumnIndex = 0;
        bool bBreak = false;
        string strDownloadPath = Application.UserAppDataPath;
        List<file> foundFiles = new List<file>();
        List<file> allFiles = new List<file>();
        Dictionary<int, int> ColWidth = new Dictionary<int, int>();
        string findExt
        {
            get
            {
                return string.IsNullOrEmpty(textBox2.Text) ? "*" : textBox2.Text;
            }
            set
            {
                textBox2.Text = value;
            }
        }

        private void refreshDataGridView()
        {
            saveColWidth();
            dataGridView1.DataSource = null;
            switch (ColumnIndex)
            {
                case 0:
                    dataGridView1.DataSource = foundFiles.OrderBy(fil => fil.name).ToList<file>();
                    break;
                case 1:
                    dataGridView1.DataSource = foundFiles.OrderBy(fil => fil.len).ToList<file>();
                    break;
                case 2:
                    dataGridView1.DataSource = foundFiles.OrderBy(fil => fil.path).ToList<file>();
                    break;

            }
            setColWidth();
        }
        private void AppDoEv()
        {
            if (System.DateTime.Now.Second % 2 == 0)
            {
                Application.DoEvents();
            }
        }
        private void setColWidth()
        {
            foreach (var k in ColWidth.Keys)
                dataGridView1.Columns[k].Width = ColWidth[k];
        }
        private void saveColWidth()
        {
            ColWidth.Clear();
            int icnt = 0;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                ColWidth.Add(icnt, col.Width); icnt++;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            loadFromXml();
            checkedListBox1.Items.AddRange(DriveInfo.GetDrives());
            foreach (DriveInfo d in DriveInfo.GetDrives().Where(d=>d.IsReady))
            {
                FileSystemWatcher fsw = new FileSystemWatcher(d.Name);
                fsw.EnableRaisingEvents = true;
                fsw.SynchronizingObject = this;
                fsw.IncludeSubdirectories = true;
                fsw.Changed += fileSystemWatcher1_Changed;
                fsw.Created += fileSystemWatcher1_Created;
                fsw.Deleted += fileSystemWatcher1_Deleted;
                fsw.Renamed += fileSystemWatcher1_Renamed;
            }
            
        }
        private void loadFromXml()
        {
            if (File.Exists(strDownloadPath + "\\allFiles.xml"))
            {
                string xml = File.ReadAllText(strDownloadPath + "\\allFiles.xml");
                allFiles = (SeDes.ToObj(xml, allFiles) as List<file>);
            }
        }
        private void saveToXml()
        {
            string xmlser = SeDes.ToXml(allFiles);
            File.WriteAllText(strDownloadPath + "\\allFiles.xml", xmlser);
        }
        private string getFriendlySize(double vsum)
        {
            string strRet = "";

            if (vsum >= 1000000000)
            {
                strRet = Math.Round((vsum / 1000000000), 2).ToString() + " Gb";
            }
            else if (vsum >= 1000000)
            {
                strRet = Math.Round((vsum / 1000000), 2).ToString() + " Mb";
            }
            else if (vsum >= 1000)
            {
                strRet = Math.Round((vsum / 1000), 2).ToString() + " kb";
            }
            else
            {
                strRet = vsum.ToString() + " b";
            }

            return strRet;
        }
        SortOrder sortorder = SortOrder.Ascending;

        private void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
        {
            addFile(e.FullPath);
        }

        private void addFile(string p)
        {
            try
            {
                FileInfo fi = new FileInfo(p);
                if (!allFiles.Any(x => x.path.Equals(p)))
                    allFiles.Add(new file() { path = p, name = fi.Name, len = fi.Length });
                if (System.DateTime.Now.Second % 10 == 0)
                {
                    label1.Text = allFiles.Count.ToString();
                }
            }
            catch { }
            System.GC.Collect();
        }

        private void fileSystemWatcher1_Deleted(object sender, FileSystemEventArgs e)
        {
            try
            {
                file fil = allFiles.FirstOrDefault(x => x.path.Equals(e.FullPath));
                if (fil != null)
                    allFiles.Remove(fil);
                label1.Text = allFiles.Count.ToString();
            }
            catch { }
            System.GC.Collect();
        }

        private void fileSystemWatcher1_Renamed(object sender, RenamedEventArgs e)
        {
            try
            {
                FileInfo fi = new FileInfo(e.FullPath);
                file fil = allFiles.FirstOrDefault(x => x.path.Equals(e.FullPath));
                if (fil != null)
                {
                    fil.path = fi.FullName;
                    fil.name = fi.Name;
                    fil.len = fi.Length;
                }
                else
                {
                    addFile(e.FullPath);
                }
                label1.Text = allFiles.Count.ToString();
            }
            catch { }
            System.GC.Collect();
        }

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                FileInfo fi = new FileInfo(e.FullPath);
                file fil = allFiles.FirstOrDefault(x => x.path.Equals(e.FullPath));
                if (fil != null)
                {
                    fil.path = fi.FullName;
                    fil.name = fi.Name;
                    fil.len = fi.Length;
                }
                else
                {
                    addFile(e.FullPath);
                }
                label1.Text = allFiles.Count.ToString();
            }
            catch { }
            System.GC.Collect();
        }

        void getDirDirs(DirectoryInfo di)
        {
            try
            {
                foreach (var dir in di.EnumerateDirectories("*", System.IO.SearchOption.TopDirectoryOnly))
                {
                    if (bBreak)
                        break;
                    AppDoEv();
                    getDirFiles(dir);
                    getDirDirs(dir);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                //addItem("", "", ex.Message);
            }
        }
        void getDirFiles(DirectoryInfo di)
        {
            try
            {
                foreach (var fil in di.EnumerateFiles(findExt, System.IO.SearchOption.TopDirectoryOnly).Where(f => f.Length > findSize))
                {
                    if (bBreak)
                        break;
                    AppDoEv();
                    addItem(fil.Name.Replace(fil.Extension, "").Replace(" ", "_"), fil.Length.ToString(), fil.FullName);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                //addItem("","", ex.Message);
            }
        }

        void addItem(string name, string len, string path)
        {
            addFile(path);
            if (foundFiles.Count < 200000)
            {
                foundFiles.Add(new file() { name = name, len = long.Parse(len), path = path });

                if (System.DateTime.Now.Second % 10 == 0)
                {
                    saveColWidth();

                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = foundFiles;//.OrderBy(fil => fil.name).ToList<file>();

                    setColWidth();

                    if (foundFiles.Count > 200000)
                    {
                        //bBreak = true;
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            foundFiles = new List<file>();
            dataGridView1.DataSource = null;

            button1.Enabled = false;
            button2.Enabled = true;
            IsProcessing = true;
            bBreak = false;

            foreach (DriveInfo drive in checkedListBox1.CheckedItems)
            {
                if (bBreak)
                    break;
                AppDoEv();
                getDirDirs(drive.RootDirectory);
            }
            dataGridView1.DataSource = foundFiles.OrderBy(fil => fil.name).ToList<file>();

            setColWidth();

            button1.Enabled = true;
            button2.Enabled = false;
            IsProcessing = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bBreak = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveToXml();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            button1.Enabled = checkBox1.Checked;
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (!checkBox1.Checked)
            {
                if (textBox2.Text.Length > 3)
                {                    
                    getList();
                    refreshDataGridView();
                }
            }
        }

        private List<file> getList()
        {
            foundFiles.Clear();
            foundFiles.AddRange(allFiles.Where(x => x.name.Contains(textBox2.Text) || x.path.Contains(textBox2.Text)));
            return foundFiles;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            saveColWidth();
            dataGridView1.DataSource = null;
            ColumnIndex = e.ColumnIndex;
            if (sortorder == SortOrder.Ascending)
            {
                sortorder = SortOrder.Descending;
                switch (e.ColumnIndex)
                {
                    case 0:
                        dataGridView1.DataSource = foundFiles.OrderBy(fil => fil.name).ToList<file>();
                        break;
                    case 1:
                        dataGridView1.DataSource = foundFiles.OrderBy(fil => fil.len).ToList<file>();
                        break;
                    case 2:
                        dataGridView1.DataSource = foundFiles.OrderBy(fil => fil.path).ToList<file>();
                        break;

                }
            }
            else if (sortorder == SortOrder.Descending)
            {
                sortorder = SortOrder.Ascending;
                switch (e.ColumnIndex)
                {
                    case 0:
                        dataGridView1.DataSource = foundFiles.OrderByDescending(fil => fil.name).ToList<file>();
                        break;
                    case 1:
                        dataGridView1.DataSource = foundFiles.OrderByDescending(fil => fil.len).ToList<file>();
                        break;
                    case 2:
                        dataGridView1.DataSource = foundFiles.OrderByDescending(fil => fil.path).ToList<file>();
                        break;

                }
            }
            setColWidth();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            saveToXml();
        }
    }
}
