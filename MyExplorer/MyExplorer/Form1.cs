using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
//using Microsoft.WindowsAPICodePack.Taskbar;

namespace MyExplorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (DriveInfo di in DriveInfo.GetDrives())//.Where(x=>x.IsReady))
            {
                TreeNode tn = treeView1.Nodes.Add(di.Name.Replace("\\",""),di.Name);
                addChildren(tn);
            }
            listView1.Sorting = SortOrder.Ascending;
            listView1_ColumnClick(this, new ColumnClickEventArgs(3));
        }


        private string getFriendlySize(double vsum)
        {
            string strRet = "NaN";

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
            else if (vsum > 0)
            {
                strRet = vsum.ToString() + " b";
            }

            return strRet;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Nodes.Count == 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    addChildren(treeView1.SelectedNode);
                    this.Cursor = Cursors.Default;
                }
                treeView1.SelectedNode.Expand();
                showPath(treeView1.SelectedNode.FullPath,true);
                textBox1.Text = treeView1.SelectedNode.FullPath.Replace("\\\\","\\");
            }
            

        }

        private void showPath(string p,bool bListFiles)
        {
            if (bListFiles)
            {
                listView1.Items.Clear();
                try
                {
                    if (filesToolStripMenuItem.Checked)
                    {
                        foreach (string fil in Directory.GetFiles(p, "*", SearchOption.TopDirectoryOnly).OrderBy(x => x))
                        {
                            FileInfo fi = new FileInfo(fil);
                            if (fi.Exists)
                            {
                                ListViewItem lvi = listView1.Items.Add(fi.Name);
                                lvi.ImageIndex = 1;
                                lvi.SubItems.Add(fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                lvi.SubItems.Add(fi.Extension);
                                lvi.SubItems.Add(getFriendlySize(fi.Length));
                                lvi.SubItems[3].Tag=fi.Length;
                                lvi.SubItems.Add(fi.Directory.FullName);
                            }
                        }
                    }
                    else
                    {

                        foreach (string fil in Directory.GetDirectories(p, "*", SearchOption.TopDirectoryOnly).OrderBy(x => x))
                        {
                            DirectoryInfo fi = new DirectoryInfo(fil);
                            if (fi.Exists)
                            {
                                ListViewItem lvi = listView1.Items.Add(fi.Name);
                                lvi.ImageIndex = 0;
                                lvi.SubItems.Add(fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                lvi.SubItems.Add(fi.Extension);
                                try
                                {
                                    double dsize = GetDirectorySizes(fi.FullName);
                                    lvi.SubItems.Add(getFriendlySize(dsize));
                                    lvi.SubItems[3].Tag = dsize;
                                }
                                catch(Exception ex) { 
                                    lvi.SubItems.Add(""); 
                                }
                                finally {  }
                                lvi.SubItems.Add(fi.FullName);
                            }
                        }
                        listView1.Sort();
                        this.Text = "MyExplorer";
                    }
                }
                catch { }
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            string key = p;
            string[] parts= p.Split('\\');
            for(int ip=parts.Length-1;ip>=0;ip--)
            {
                string part = parts[ip];
                int iend = key.LastIndexOf(part) - 1;
                if (iend > 0)
                {
                    key = key.Substring(0, iend);
                    TreeNode[] tns = treeView1.Nodes.Find(key, true);
                    if (tns.Length > 0)
                    {
                        if (tns[0].Nodes.Count == 0)
                        {
                            addChildren(tns[0]);
                            showPath(p,false);
                        }
                        tns[0].Expand();
                        break;
                    }
                }
            }
        }

        private double GetDirectorySizes(string dir)
        {
            var nodes = treeView1.Nodes.Find(dir, true);
            if (nodes != null && nodes.Count()>0)
            {
                var node = nodes[0];
                if (node.Tag != null && ((SizeClass)node.Tag)!=null)
                {
                    return ((SizeClass)node.Tag).size;
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(getDirectorySizes, dir);
                }
            }
            else
            {
                ThreadPool.QueueUserWorkItem(getDirectorySizes, dir);
            }
            return -1;
        }

        private void getDirectorySizes(object state)
        {
            string strdir = state.ToString();
            long dsize = getSize(strdir);
            if (dsize > 0)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    var nodes = treeView1.Nodes.Find(strdir, true);
                    if (nodes != null && nodes.Count() > 0)
                    {
                        var node = nodes[0];
                        SizeClass sc = new SizeClass() { size = dsize, path = strdir };
                        node.Tag = sc;
                        sc.PropertyChanged += (a, b) => {
                            SizeClass _sc = (SizeClass)a;
                            if(_sc!=null)
                                updateListView(_sc);
                        };

                    }
                    updateListView(strdir, dsize);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                });
            }
        }

        private void updateListView(string strdir,double dsize)
        {
            this.Invoke((MethodInvoker)delegate {
                foreach (ListViewItem itm in listView1.Items)
                {
                    if (strdir.Equals(itm.SubItems[4].Text, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //if (string.IsNullOrEmpty(itm.SubItems[3].Text))
                        {
                            itm.SubItems[3].Text = getFriendlySize(dsize);
                            itm.SubItems[3].Tag = dsize;
                            listView1.Sort();
                            //listView1.Refresh();
                            break;
                        }
                    }
                }
            });
        }
        private void updateListView(SizeClass sc)
        {
            this.Invoke((MethodInvoker)delegate {
                updateListView(sc.path, sc.size);
                var nodes = treeView1.Nodes.Find(sc.path, true);
                if (nodes != null && nodes.Count() > 0)
                {
                    var node = nodes[0];
                    node.Tag = sc;
                }
            });
        }
        private static long getSize(string dir)
        {
            try
            {
                return Directory.GetFiles(dir, "*", SearchOption.AllDirectories).Sum(f => new FileInfo(f).Length);
            }
            catch{}
            return -1;
        }

        private void addChildren(TreeNode treeNode)
        {
            try
            {
                foreach (string dir in Directory.GetDirectories(treeNode.FullPath, "*", SearchOption.TopDirectoryOnly).OrderBy(x => x))
                {
                    DirectoryInfo di = new DirectoryInfo(dir);
                    treeNode.Nodes.Add(di.FullName, di.Name);

                }
            }
            catch { }
        }

        private void copyToSingleFolderAsync(object state)
        {
            this.Invoke((MethodInvoker)delegate {
                Cursor = Cursors.AppStarting;
            });
            dynamic dstate = state as dynamic;
            string[] allfiles;
            iupdatedchannels = 0;
            foreach (string fil in (allfiles = Directory.GetFiles(dstate.SearchPath, "*.jpg", SearchOption.AllDirectories)))
            {
                imaxchannels = allfiles.Count();

                FileInfo fi = new FileInfo(fil);
                if (fi.Exists)
                {
                    string strFile = fi.FullName.Replace(fi.Directory.FullName, dstate.DestinationPath).Replace(fi.Name, "") + "\\" + fi.Directory.Name + "_" + fi.Name;
                    try
                    {
                        if (!File.Exists(strFile))
                            fi.CopyTo(strFile);
                    }
                    catch { }
                }
            }
            this.Invoke((MethodInvoker)delegate
            {
                iupdatedchannels++;
                Cursor = Cursors.Default;
                MessageBox.Show("Done!");
            });
        }
        private void copyToSingleFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                
                if (Directory.Exists(treeView1.SelectedNode.FullPath))
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.SelectedPath = treeView1.SelectedNode.FullPath;
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {

                        ThreadPool.QueueUserWorkItem(copyToSingleFolderAsync, new { SearchPath = treeView1.SelectedNode.FullPath, DestinationPath = fbd.SelectedPath });

                    }
                }
            }
        }

        private float getFontSize(Image img)
        {
            return ((MathX.Avg(img.Height,img.Width) / 100)*2)+2;
        }
        private void addFilenameAsync(object state)
        {
            this.Invoke((MethodInvoker)delegate { 
                Cursor = Cursors.AppStarting;
            });
            dynamic dstate = state as dynamic;
            string[] allfiles;
            iupdatedchannels = 0;
            foreach (string fil in (allfiles=Directory.GetFiles(dstate.SearchPath, "*.jpg", SearchOption.AllDirectories)))
            {
                imaxchannels = allfiles.Count();
                FileInfo fi = new FileInfo(fil);
                string strFile = fi.FullName.Replace(fi.Directory.FullName, dstate.DestinationPath).Replace(fi.Name, "") + "\\" + fi.Directory.Name + "_" + fi.Name;

                if (fi.Exists && !File.Exists(strFile))
                {
                    try
                    {
                        Image img = Bitmap.FromFile(fi.FullName);
                        float fontSize = getFontSize(img);
                        Font font = new Font(DefaultFont.FontFamily, fontSize);
                        ImageFormat format = ImageFormat.Jpeg;
                        PointF pf = new PointF(2, img.Height - (fontSize + 10));
                        Graphics g = Graphics.FromImage(img);

                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.DrawString(fi.Name.Replace(fi.Extension, ""), font, new SolidBrush(Color.OrangeRed), pf);
                        g.Save();

                        img.Save(strFile, format);
                    }
                    catch { }
                    finally { System.GC.Collect(); }
                }
                iupdatedchannels++;
            }
            this.Invoke((MethodInvoker)delegate
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Done!");
            });
        }
        private void addFilenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                
                if (Directory.Exists(treeView1.SelectedNode.FullPath))
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.SelectedPath = treeView1.SelectedNode.FullPath;
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {

                        ThreadPool.QueueUserWorkItem(addFilenameAsync, new { SearchPath = treeView1.SelectedNode.FullPath, DestinationPath = fbd.SelectedPath });
                        
                    }
                }
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (Directory.Exists(textBox1.Text))
            {
                showPath(textBox1.Text, true);
                TreeNode[] tns = treeView1.Nodes.Find(textBox1.Text, true);
                if (tns.Length > 0)
                {
                    treeView1.SelectedNode = tns[0];
                }
            }
        }

        int imaxchannels = 0;
        int _iupdatedchannels = 0;
        int iupdatedchannels
        {
            get
            {
                return _iupdatedchannels;
            }
            set
            {
                _iupdatedchannels = value;
                //TaskbarManager.Instance.SetProgressValue(_iupdatedchannels, imaxchannels);
            }
        }
        private void copyMovToSingleFolderAsync(object state)
        {
            this.Invoke((MethodInvoker)delegate{
                Cursor = Cursors.AppStarting;
            });
            dynamic dstate = state as dynamic;

            string[] allfiles;
            this.Invoke((MethodInvoker)delegate
            {
                toolStripProgressBar1.Visible = true;
                toolStripProgressBar2.Visible = true;

                toolStripProgressBar1.Maximum = 5;
                toolStripProgressBar1.Value = 0;
            });
            foreach (string ext in new string[] { "*.avi", "*.mpg", "*.mov", "*.3gp", "*.mp4" })
            {
                this.Invoke((MethodInvoker)delegate
                {
                    toolStripProgressBar2.Value = 0;
                    toolStripProgressBar1.ToolTipText = "Copying " + ext;
                });
                    iupdatedchannels = 0;
                foreach (string fil in (allfiles = Directory.GetFiles(dstate.SearchPath, ext, SearchOption.AllDirectories)))
                {
                    imaxchannels = allfiles.Count();
                    this.Invoke((MethodInvoker)delegate
                    {
                        toolStripProgressBar2.Maximum = allfiles.Count();
                        toolStripProgressBar2.ToolTipText = "Copying file: " + fil;
                    });
                    FileInfo fi = new FileInfo(fil);
                    if (fi.Exists)
                    {
                        string strFile = fi.FullName.Replace(fi.Directory.FullName, dstate.DestinationPath).Replace(fi.Name, "") + "\\" + fi.DirectoryName + "_" + fi.Name;
                        try
                        {
                            if (!File.Exists(strFile))
                            {
                                fi.CopyTo(strFile);
                            }
                            else
                            {
                                strFile = strFile.Replace(fi.Name, "~" + fi.Name);
                                if (!File.Exists(strFile))
                                {
                                    fi.CopyTo(strFile);
                                }
                            }
                        }
                        catch { }
                    }
                    iupdatedchannels++;
                    this.Invoke((MethodInvoker)delegate
                    {
                        toolStripProgressBar2.Value++;
                    });
                }
                this.Invoke((MethodInvoker)delegate
                {
                    toolStripProgressBar1.Value++;
                });
            }
            this.Invoke((MethodInvoker)delegate
            {
                Cursor = Cursors.Default;
                MessageBox.Show("Done!");
            });
        
        }
        private void copyMovToSingleFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (Directory.Exists(treeView1.SelectedNode.FullPath))
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.SelectedPath = treeView1.SelectedNode.FullPath;
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        ThreadPool.QueueUserWorkItem(copyMovToSingleFolderAsync, new { SearchPath = treeView1.SelectedNode.FullPath, DestinationPath = fbd.SelectedPath });
                    }
                }
            }
        }

        private void filesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foldersToolStripMenuItem.Checked = !filesToolStripMenuItem.Checked;
        }

        private void foldersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filesToolStripMenuItem.Checked = !foldersToolStripMenuItem.Checked;
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            if (listView1.Sorting == SortOrder.Ascending)
                listView1.Sorting = SortOrder.Descending;
            else
                listView1.Sorting = SortOrder.Ascending;

            listView1.ListViewItemSorter = new ListViewItemComparer(e.Column, listView1.Sorting, e.Column == 3 ? TypeCode.Double : TypeCode.String);
            listView1.Sort();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                var it = listView1.SelectedIndices[0];
                var lvi = listView1.Items[it];
                string path = lvi.SubItems[4].Text;
                var nodes =  treeView1.Nodes.Find(path, true);
                if (nodes.Count() > 0)
                {
                    var node = nodes[0];
                    treeView1.SelectedNode = node;
                }
            }
        }
        public string SelectedDir;
        private void showInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(SelectedDir))
                Process.Start(SelectedDir);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                SelectedDir = listView1.SelectedItems[0].SubItems[4].Text;
            }
        }
    }
    public static class MathX
    {
        public static double Avg(params double[] p)
        {
            double dRet = 0;
            foreach (double d in p)
            {
                dRet += d;
            }
            if (p.Length > 0 && dRet > 0)
            {
                dRet = dRet / p.Length;
            }
            return dRet;
        }
        public static float Avg(params float[] p)
        {
            float dRet = 0;
            foreach (float d in p)
            {
                dRet += d;
            }
            if (p.Length > 0 && dRet > 0)
            {
                dRet = dRet / p.Length;
            }
            return dRet;
        }
    }
}
