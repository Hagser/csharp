using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace MyPhotoInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dGVExif.DragEnter += new DragEventHandler(dGVExif_DragEnter);
            dGVExif.DragDrop += new DragEventHandler(dGVExif_DragDrop);
            dGVExif.DataError += new DataGridViewDataErrorEventHandler(dGVExif_DataError);

            dGVFiles.DragEnter += new DragEventHandler(dGVExif_DragEnter);
            dGVFiles.DragDrop += new DragEventHandler(dGVExif_DragDrop);
            dGVFiles.DataError += new DataGridViewDataErrorEventHandler(dGVExif_DataError);

            pBMap.DragEnter += new DragEventHandler(dGVExif_DragEnter);
            pBMap.DragDrop += new DragEventHandler(dGVExif_DragDrop);

            pBPhoto.DragEnter += new DragEventHandler(dGVExif_DragEnter);
            pBPhoto.DragDrop += new DragEventHandler(dGVExif_DragDrop);

            treeViewPhotos.DragEnter += new DragEventHandler(dGVExif_DragEnter);
            treeViewPhotos.DragDrop += new DragEventHandler(dGVExif_DragDrop);
            treeViewPhotos.AfterSelect += new TreeViewEventHandler(treeViewPhotos_AfterSelect);

            tableLayoutPanel1.SizeChanged += new EventHandler(tableLayoutPanel1_SizeChanged);

            pBMap.ContextMenu = new ContextMenu(new MenuItem[] { new MenuItem("Copy url", new EventHandler((a, b) => { Clipboard.SetText(strMapUrl); })) });

            dict.Add("Exif Info",dGVExif);
            dict.Add("Map",pBMap);
            dict.Add("Thumbnails",dGVFiles);
            dict.Add("Big Photo",pBPhoto);
            dict.Add("Treeview",treeViewPhotos);
        }

        void treeViewPhotos_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {
                LoadFile(e.Node.Tag.ToString());
            }
        }

        void dGVExif_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        void tableLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            ColumnStyle cs = tableLayoutPanel1.ColumnStyles[1];
            pBPhoto.Width = int.Parse(Math.Round(cs.Width,0).ToString());
        }


        void dGVExif_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop.ToString());
            LoadFile(files[0]);
        }

        private void LoadFile(string Filename)
        {

            BackgroundWorker bw1 = new BackgroundWorker();
            bw1.DoWork += (a, b) =>
                {
                    getExifInfo(Filename);
                };
            bw1.RunWorkerAsync();
            
            BackgroundWorker bw2 = new BackgroundWorker();
            bw2.DoWork += (a, b) =>
                {
                    LoadFiles(Filename);
                };
            bw2.RunWorkerAsync();
            
            BackgroundWorker bw3 = new BackgroundWorker();
            bw3.DoWork += (a, b) =>
            {
                Image img = Bitmap.FromFile(Filename);
                double percentw = (pBPhoto.Width.ToDouble() / img.Width.ToDouble()) * 100;
                double percenth = (pBPhoto.Height.ToDouble() / img.Height.ToDouble()) * 100;
                double percent = Math.Min(percenth, percentw);
                pBPhoto.Image = ImageStuff.ScaleByPercent(ImageStuff.RotateImage(img), percent.ToInt());
            };
            bw3.RunWorkerAsync();
        }
        DirectoryInfo olddi;
        DirectoryInfo oldtdi;
        private void LoadFiles(string Filename)
        {
            LoadTree(Filename);
            DirectoryInfo di = (new FileInfo(Filename)).Directory;
            if (olddi==null || (olddi!=null && !di.FullName.Equals(olddi.FullName)))
            {
                olddi = di;
                FileInfoList.Clear();
                List<PhotoFileInfo> _FileInfoList = new List<PhotoFileInfo>();
                foreach (FileInfo fi in di.GetFiles("*.jpg", SearchOption.TopDirectoryOnly))
                {
                    _FileInfoList.Add(new PhotoFileInfo(fi));
                }
                FileInfoList.AddRange(_FileInfoList.OrderBy(x => x.Name));
                this.Invoke((MethodInvoker)delegate
                {
                    dGVFiles.DataSource = null;
                    dGVFiles.DataSource = FileInfoList;
                    dGVFiles.Refresh();
                });
            }
        }

        private void LoadTree(string Filename)
        {

            DirectoryInfo di = ((new FileInfo(Filename)).Directory).Parent;
            if (di!=null && (oldtdi == null || (oldtdi != null && !di.FullName.Equals(oldtdi.FullName))))
            {
                oldtdi = di;

                TreeNode tnroot = new TreeNode() { Tag = di.FullName, Text = di.Name };
                foreach (DirectoryInfo dis in di.GetDirectories().OrderBy(x=>x.Name))
                {
                    TreeNode tn = new TreeNode() { Tag = dis.FullName, Text = dis.Name };
                    tnroot.Nodes.Add(tn);
                    foreach (FileInfo fi in dis.GetFiles("*.jpg", SearchOption.TopDirectoryOnly).OrderBy(x => x.Name))
                    {
                        TreeNode tnf = new TreeNode() { Tag = fi.FullName, Text = fi.Name };
                        tn.Nodes.Add(tnf);
                    }
                }
                
                this.Invoke((MethodInvoker)delegate
                {
                    treeViewPhotos.Nodes.Clear();
                    treeViewPhotos.Nodes.Add(tnroot);
                });
                

            }
        }


        void dGVExif_DragEnter(object sender, DragEventArgs e)
        {
            
            e.Effect = DragDropEffects.Copy;
        }

        private void pBMap_SizeChanged(object sender, EventArgs e)
        {
            RefreshMap();
        }

        private void RefreshMap()
        {
            if (Lat != "" && Lon != "")
            {
                try
                {
                    WebRequest req = WebRequest.Create(getMapUrl());
                    req.Method = "GET";

                    WebResponse res = req.GetResponse();
                    pBMap.Image = Image.FromStream(res.GetResponseStream());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error getting map:" + ex.Message);
                }
            }
        }
        string Lat = "";
        string Lon = "";

        private string getMapUrl()
        {
            return getMapUrl(Lat,Lon);
        }

        private string getMapUrl(string lat,string lon)
        {
            string strUrl = "http://maps.google.com/maps/api/staticmap?zoom=12&size=" + Math.Min(600, pBMap.Width).ToString() + "x" + Math.Min(600, pBMap.Height).ToString() + "&maptype=roadmap&mobile=false#markers#&sensor=false";
            string strMarkers = "&markers=label:1|" + lat.Replace(",", ".") + "," + lon.Replace(",", "."); ;

            strUrl = strUrl.Replace("#markers#", strMarkers);
            strMapUrl = strUrl;
            return strUrl;
        }
        string strMapUrl = "";
        List<PhotoInfo> PhotoInfoList = new List<PhotoInfo>();
        List<PhotoFileInfo> FileInfoList = new List<PhotoFileInfo>();
        private void getExifInfo(string strFilename)
        {
            try
            {
                if (File.Exists(strFilename) && strFilename.ToLower().EndsWith(".jpg"))
                {
                    HybridDictionary hd = ImageInfo.ExifReader.GetAllProperties(strFilename);
                    string strTD = "";
                    PhotoInfoList.Clear();
                    Lat = ""; Lon = "";
                    List<PhotoInfo> _PhotoInfoList = new List<PhotoInfo>();
                    foreach (object obj in hd.Keys)
                    {
                        if (obj.ToString().ToLower().Equals("thumbnail data"))
                        {
                            strTD = hd[obj].ToString();
                        }
                        if (obj.ToString().Equals("Latitude"))
                        {
                            Lat = hd[obj].ToString();//.Replace(",", ".");
                        }
                        if (obj.ToString().Equals("Longitude"))
                        {
                            Lon = hd[obj].ToString();//.Replace(",", ".");
                        }
                        PhotoInfo pi = new PhotoInfo() { InfoName = obj.ToString(), InfoValue = getRealValue(obj, hd) };
                        _PhotoInfoList.Add(pi);
                    }

                    if (Lat != "" && Lon != "")
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            RefreshMap();
                        });
                    }
                    this.Invoke((MethodInvoker)delegate
                    {
                        PhotoInfoList.AddRange(_PhotoInfoList.OrderBy(x => x.InfoName));
                        dGVExif.DataSource = null;
                        dGVExif.DataSource = PhotoInfoList;
                        dGVExif.Refresh();
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private string getRealValue(object obj, HybridDictionary hd)
        {
            string result = "";
            string p = hd[obj].ToString();
            if (obj.ToString().ToLower().Contains("date"))
            {
                string strDate = p.Substring(0, 10).Replace(":", "-");
                string strHour = p.Substring(11);
                string strDT = strDate + " " + strHour;
                DateTime dtPhoto = DateTime.Parse(strDT);
                dtPhoto = dtPhoto.AddSeconds(70);
                result = dtPhoto.ToShortDateString() + " " + dtPhoto.ToLongTimeString();
            }
            else
            {
                result = p;
            }
            return result;
        }
        private void dGVFiles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (olddi!=null && dGVFiles.SelectedCells.Count >= 1)
            {
                LoadFile(olddi.FullName + "\\" + (dGVFiles.Rows[dGVFiles.SelectedCells[0].RowIndex].DataBoundItem as PhotoFileInfo).Name);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings set = new Settings();
            if (set.ShowDialog() == DialogResult.OK)
            {
                if (set.Tag != null)
                {
                    foreach (Control ct in tableLayoutPanel1.Controls)
                    {
                        ct.Visible = false;
                    }
                    tableLayoutPanel1.Controls.Clear();

                    string strGrid = "";
                    int icol = 0;
                    int irow = 0;

                    System.Collections.IEnumerator ienum = (System.Collections.IEnumerator)set.Tag;
                    while (ienum.MoveNext())
                    {
                        string strName = ienum.Current.ToString();
                        Control ctrl = GetControl(strName);
                        ctrl.Visible = true;
                        strGrid += strName + ":" + icol + "," + irow + "\r\n";
                        tableLayoutPanel1.Controls.Add(ctrl, icol, irow);
                        /*tableLayoutPanel1.SetColumn(ctrl, icol);
                        tableLayoutPanel1.SetRow(ctrl, irow);*/
                        icol++;
                        if (icol == 2)
                        {
                            icol = 0;
                            irow++;
                        }
                    }
                }
            }
        }
        Dictionary<string, Control> dict = new Dictionary<string, Control>();
        private Control GetControl(string p)
        {
            return dict.ContainsKey(p) ? dict[p] : null;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.O)
                {
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string filename in openFileDialog1.FileNames)
                        {
                            LoadFile(filename);
                            break;
                        }
                    }
                }
            }
        }

    }
}
