using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace MyDuplicatedFileFinder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int ColumnIndex = 0;
        bool bBreak = false;
        List<file> foundFiles = new List<file>();
        List<file> duplicatedFiles = new List<file>();
        Dictionary<int, int> ColWidth = new Dictionary<int, int>();
        int findSize {
            get
            {
                return string.IsNullOrEmpty(txtFindSize.Text) ? 0 : int.Parse(txtFindSize.Text);
            }
            set {
                txtFindSize.Text = value.ToString();
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
        string findExt
        {
            get
            {
                return string.IsNullOrEmpty(txtFindExt.Text) ? "*" : txtFindExt.Text;
            }
            set
            {
                txtFindExt.Text = value;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            foundFiles = new List<file>();
            dataGridView1.DataSource = null;
            lblFilesCount.Text = foundFiles.Count.ToString();

            btnSearch.Enabled = false;
            btnCancel.Enabled = true;
            IsProcessing = true;
            bBreak = false;

            if (chkBrowsed.Checked && !string.IsNullOrEmpty(lblDirName.Text))
            {
                AppDoEv();
                getDirDirs(new DirectoryInfo(lblDirName.Text));
            }
            else
            {
                foreach (DriveInfo drive in chkLDrives.CheckedItems)//.Where(drive=>drive.DriveType==System.IO.DriveType.Fixed))
                {
                    if (bBreak)
                        break;
                    AppDoEv();
                    lblDirName.Text = drive.RootDirectory.Name;
                    getDirDirs(drive.RootDirectory);
                }

            }
            saveColWidth();
            dataGridView1.DataSource = null;
            var rmf = new List<file>();
            if (bBreak)
                bBreak = false;
            if (chkShow.Checked)
            {
                foreach (file fil in foundFiles)
                {
                    if (bBreak)
                        break;
                    AppDoEv();
                    if (foundFiles.Count(x => x.name == fil.name && x.len == fil.len) == 1)
                        rmf.Add(fil);
                }
                foreach (file fil in rmf)
                {
                    foundFiles.Remove(fil);
                }
            }

            dataGridView1.DataSource = foundFiles.OrderBy(fil => fil.name).ToList<file>();

            setColWidth();

            btnSearch.Enabled = true;
            btnCancel.Enabled = false;
            lblFilesCount.Text = foundFiles.Count.ToString();
            IsProcessing = false;
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
                    addItem(fil.Name.Replace(fil.Extension,"").Replace(" ","_"),fil.Length.ToString(),fil.FullName);
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
            foundFiles.Add(new file() { name = name, len = long.Parse(len), path = path });

            if (System.DateTime.Now.Second % 10 == 0)
            {
                saveColWidth();
                
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = foundFiles;//.OrderBy(fil => fil.name).ToList<file>();

                setColWidth();
  
                lblFilesCount.Text = foundFiles.Count.ToString();

                if (foundFiles.Count > 200000)
                {
                    bBreak = true;
                }
            }
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
        private void button2_Click(object sender, EventArgs e)
        {
            bBreak = true;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            long vsum = 0;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                AppDoEv();
                vsum += long.Parse(row.Cells[1].Value.ToString());
            }
            lblSelectedSize.Text = getFriendlySize(vsum);
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

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            selectFolderToolStripMenuItem.DropDownItems.Clear();
            if(dataGridView1.SelectedCells.Count>0)
            {
                int row = dataGridView1.SelectedCells[0].RowIndex;
                string path = dataGridView1.Rows[row].Cells[2].Value.ToString();
                string fullpath = "";
                foreach (string folder in path.Split('\\'))
                {
                    if (path.Equals(fullpath + folder))
                        break;
                    fullpath = fullpath + folder + "\\";
                    ToolStripItem tsi = selectFolderToolStripMenuItem.DropDownItems.Add(fullpath);
                    tsi.Click += (a, b) => {
                        ToolStripItem tsic = a as ToolStripItem;
                        if (tsic != null)
                        {
                            string strfullpath = tsic.Text;
                            IsProcessing = true;
                            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
                            {
                                dgvr.Selected = dgvr.Cells[2].Value.ToString().StartsWith(strfullpath);
                            }
                            IsProcessing = false;
                        }
                    };
                }
            }
            hideExtToolStripMenuItem.DropDownItems.Clear();
            IEqualityComparer<file> fec = new FileExtComparer();
            var fileext = foundFiles.Distinct(fec).Select(x=>new FileInfo(x.path).Extension).OrderBy(x=>x).ToList<string>();
            ToolStripItem tsiall = hideExtToolStripMenuItem.DropDownItems.Add("All");
            tsiall.Click += (a, b) =>
            {
                saveColWidth();
                IsProcessing = true;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = foundFiles;
                IsProcessing = false;
            };
            foreach (string fext in fileext.Where(x=>x.Length<6))
            {
                ToolStripItem tsi = hideExtToolStripMenuItem.DropDownItems.Add(fext);
                tsi.Click += (a, b) =>
                {
                    ToolStripItem tsic = a as ToolStripItem;
                    if (tsic != null)
                    {
                        saveColWidth();
                        string strext = tsic.Text;
                        IsProcessing = true;
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = foundFiles.Where(x=>new FileInfo(x.path).Extension.Equals(strext,StringComparison.InvariantCultureIgnoreCase)).ToList<file>();

                        IsProcessing = false;
                    }
                };
            }


        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (DialogResult.Yes == MessageBox.Show("Are you sure?", "Delete", MessageBoxButtons.YesNo))
                {
                    int icnt = dataGridView1.SelectedRows.Count;
                    if (DialogResult.Yes == MessageBox.Show("Are you absolutely certain that you want to delete " + icnt.ToString() + " files from your computer!?", "Delete", MessageBoxButtons.YesNo))
                    {
                        IsProcessing = true;
                        foreach (DataGridViewRow dgvr in dataGridView1.SelectedRows)
                        {
                            AppDoEv();
                            string strfullpath = dgvr.Cells[2].Value.ToString();
                            try {
                                File.Delete(strfullpath);
                                removeRow(dgvr);
                            }
                            catch (Exception ex) { dgvr.Cells[0].ErrorText = ex.Message; }
                        }
                        IsProcessing = false;
                        refreshDataGridView();
                    }
                }
            }
            else
            {
                MessageBox.Show("No files selected!");
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

        private void removeRow(DataGridViewRow row)
        {
            var fil = foundFiles.Where(x => x.path.Equals(row.Cells[2].Value.ToString())).FirstOrDefault();
            if(fil!=null)
                foundFiles.Remove(fil);
        }

        private void hideFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsProcessing = true;
            foreach (DataGridViewRow dgvr in dataGridView1.SelectedRows)
            {
                AppDoEv();
                removeRow(dgvr);
            }
            refreshDataGridView();
            IsProcessing = false;
        }

        private void openFileLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int row = dataGridView1.SelectedCells[0].RowIndex;
                string path = dataGridView1.Rows[row].Cells[2].Value.ToString();
                FileInfo fi = new FileInfo(path);
                Process.Start(fi.Directory.FullName);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            bBreak = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chkLDrives.Items.AddRange(DriveInfo.GetDrives());
        }

        private void hideFilenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsProcessing = true;
            var cellname = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value as string;
            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                AppDoEv();
                if(dgvr.Cells[0].Value.ToString().Equals(cellname))
                    removeRow(dgvr);
            }
            refreshDataGridView();
            IsProcessing = false;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                lblDirName.Text = fbd.SelectedPath;
                chkBrowsed.Checked = true;
            }
        }

        private void selectOneOfAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grps = foundFiles.GroupBy(f => f.hash);
            foreach (var grp in grps)
            {
                bool bFound = false;
                foreach (var fil in grp)
                {
                    if (bFound)
                        break;
                    string cellname = fil.path;
                    foreach (DataGridViewRow dgvr in dataGridView1.Rows)
                    {
                        if (bFound)
                            break;
                        AppDoEv();
                        if (dgvr.Cells[2].Value.ToString().Equals(cellname))
                        {
                            bFound = true;
                            dgvr.Selected = true;
                        }
                    }
                    break;
                }
            }
        }

        private void selectAllButOneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grps = foundFiles.GroupBy(f => f.hash);
            foreach (var grp in grps)
            {
                int ntot = grp.Count()-1;
                int ncnt = 0;
                foreach (var fil in grp.Take(ntot))
                {
                    string cellname = fil.path;
                    foreach (DataGridViewRow dgvr in dataGridView1.Rows)
                    {
                        if (ncnt==ntot)
                            break;
                        AppDoEv();
                        if (dgvr.Cells[2].Value.ToString().Equals(cellname))
                        {
                            dgvr.Selected = true;
                            ncnt++;
                        }
                    }
                }
            }
        }
    }
}
