using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using ImageInfo;
using WMPLib;
using zip=ICSharpCode.SharpZipLib;
using System.Xml;
using System.Diagnostics;



namespace Filesearcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string in_filename = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\savedSearches.xml";
        Thread thSearcher;
        WindowsMediaPlayer mp = new WindowsMediaPlayer();
        static bool bStop = true;
        bool bPBActive = false;

        Hashtable htFileTypes = new Hashtable();
        Hashtable htColumnHeaders = new Hashtable();
        int sortColumn = -1;

        int iLblSearchedFolders = 0;
        string strLblInfolder = "";
        string strLbl = "";

        bool bFileTypeFolder = false;
        string strExifInfoType = "";
        string strExifInfo = "";
        string strMusicInfoType = "";
        string strMusicInfo = "";
        string strFileContains = "";
        string strFolders = "";
        Hashtable htFolders = new Hashtable();
        Hashtable htFoldersSearched = new Hashtable();
        string strSearchString = "";
        long lngSize = 0;
        DateTime dtNlDate = new DateTime();
        DateTime dtDate = new DateTime();
        string strPFolder = "";
        bool bSearchFolders = true;
        bool bSearchZip = false;

        /// <summary>
        /// Returns extension from filetypenames
        /// </summary>
        /// <param name="in_name">Filetypenames</param>
        /// <returns>string</returns>
        private string getExts(string in_name)
        {
            string v_ret = "";
            switch (in_name)
            {
                case "Folders":
                    break;
                case "Documents":
                    v_ret = "*.doc*;*.xls*;*.txt;*.inf;*.xml";
                    break;
                case "Photos":
                    v_ret = "*.jpg;*.png;*.tif;*.gif";
                    break;
                case "Music":
                    v_ret = "*.mp3;*.asf;*.wma;*.wav";
                    break;
                case "Movies":
                    v_ret = "*.avi;*.mpg;*.mov;*.wmv";
                    break;
                case "Programming":
                    v_ret = "*.cs;*.htm*;*.vbs;*.asp;*.php";
                    break;
                case "Zipped":
                    v_ret = "*.zip;*.gz;*.ra*;*.cab;*.iso";
                    break;
            }
            return v_ret;
        }
        /// <summary>
        /// Returns datestring from DateTime
        /// </summary>
        /// <param name="in_dt">DateTime</param>
        /// <returns>string</returns>
        private string getDateToString(DateTime in_dt)
        {
            return in_dt.ToShortDateString() + " " + in_dt.ToLongTimeString();
        }

        private void startSearchThread()
        {
            if (htFolders.Count>0 && (strSearchString != "" || htFileTypes.Count > 0) && bStop)
            {
                checkForm();
                htFoldersSearched.Clear();
                listView1.Items.Clear();
                listView1.Sorting = SortOrder.None;
                iLblSearchedFolders = 0;
                bStop = false;
                btnSearch.Enabled = false;
                bPBActive = true;
                
                strLbl = "";
                strLblInfolder = "";
                tSSLblErr.Text = "";
                
                ThreadStart ts = new ThreadStart(startSearch);
                thSearcher = new Thread(ts);
                thSearcher.Priority = ThreadPriority.BelowNormal;
                thSearcher.Start();
                
            }
        }
        /// <summary>
        /// Starts to search
        /// </summary>
        private void startSearch()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                foreach (object k in htFolders.Keys)
                {
                    string strFolder = htFolders[k].ToString();
                    if (Directory.Exists(strFolder))
                    {
                        ListView n_lv = new ListView();
                        addColumnHeaders(n_lv);
                        searchFiles(strSearchString, strFolder, n_lv);
                        if (bStop) { break; }

                        this.Invoke((MethodInvoker)delegate{
                        foreach (ListViewItem tmplvi in n_lv.Items)
                        {

                            ListViewItem lvi = listView1.Items.Add(tmplvi.Text);
                            for (int i = 1; i < tmplvi.SubItems.Count; i++)
                            {
                                lvi.SubItems.Add(tmplvi.SubItems[i].Text).Tag = tmplvi.SubItems[i].Tag;
                            }
                        }
                        strLbl = listView1.Items.Count + " file(s)/folder(s) found.";
                        n_lv.Items.Clear();
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                this.Invoke((MethodInvoker)delegate{
                    tSSLblErr.Text = ex.Message;
                });
            }
            finally{
                Cursor.Current = Cursors.Default;
                cancelSearchThread();
            }
        }
        private void addColumnHeaders(ListView in_lv)
        {
            for(int k=0;k<htColumnHeaders.Count;k++)
            {
                in_lv.Columns.Add(htColumnHeaders[k].ToString());
            }
        }
        private void searchFiles(string in_ss, string in_folder, ListView in_lv)
        {
            if (!bStop)
            {
                getDirectories(in_ss, in_folder, in_lv);
            }
        }
        private void getDirectories(string in_ss, string in_folder, ListView in_lv)
        {
            strLblInfolder = in_folder;
            iLblSearchedFolders++;

            if (bSearchFolders)
            {
                try
                {
                    foreach (string strdir in Directory.GetDirectories(in_folder))
                    {
                        if (bStop) { break; }
                        getDirectories(in_ss, strdir, in_lv);
                    }

                }
                catch { }
            }
            if (bFileTypeFolder)
            {
                    DirectoryInfo di = new DirectoryInfo(in_folder);
                    if (di.Name.ToLower().Equals(in_ss.ToLower()) || (in_ss.IndexOf("*")!=-1 && di.Name.ToLower().Contains(in_ss.Replace("*","").ToLower())))
                    {
                        if (dtDate == dtNlDate || (dtDate != dtNlDate && (dtDate >= di.CreationTime || dtDate >= di.LastWriteTime || dtDate >= di.LastAccessTime)))
                        {
                            //this.Invoke((MethodInvoker)delegate
                            //{
                                ListViewItem lvi = in_lv.Items.Add(di.Name);
                                lvi.SubItems.Add(di.Parent.FullName).Tag = di.Parent.FullName;
                                lvi.SubItems.Add(di.Parent.Name).Tag = di.Parent.Name;
                                lvi.SubItems.Add("").Tag = "";
                                lvi.SubItems.Add("Folder").Tag = "Folder";
                                lvi.SubItems.Add(getDateToString(di.LastWriteTime)).Tag = di.LastWriteTime;
                                lvi.SubItems.Add(getDateToString(di.LastAccessTime)).Tag = di.LastAccessTime;
                                lvi.SubItems.Add(getDateToString(di.CreationTime)).Tag = di.CreationTime;
                                lvi.Tag = di.Name;
                                //lvi.Tag = di;
                                
                            //});
                        }
                    }
            }
            else
            {
                if (!bStop)
                {
                    try
                    {
                        DirectoryInfo di = new DirectoryInfo(in_folder);
                        bool boverride = (strPFolder == "");
                        bool bpfeq = (strPFolder != "" && di.Name.ToLower().Equals(strPFolder));
                        bool bpfneq = (strPFolder != "" && strPFolder.IndexOf("!") != -1 && !di.Name.ToLower().Equals(strPFolder.Replace("!", "")));

                        if (boverride || bpfeq || bpfneq)
                        {
                            if (htFileTypes.Count>0)
                            {
                                foreach (object k in htFileTypes.Keys)
                                {
                                    getFiles("*" + in_ss + htFileTypes[k].ToString(), in_folder, in_lv);
                                }
                            }
                            else
                            {
                                getFiles(in_ss, in_folder, in_lv);
                            }
                        }
                    }
                    catch { }
                }
            }

            if (!bStop)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    foreach (ListViewItem tmplvi in in_lv.Items)
                    {

                        ListViewItem lvi = listView1.Items.Add(tmplvi.Text);
                        for (int i = 1; i < tmplvi.SubItems.Count; i++)
                        {
                            lvi.SubItems.Add(tmplvi.SubItems[i].Text).Tag = tmplvi.SubItems[i].Tag;
                        }
                    }

                    strLbl = listView1.Items.Count + " file(s)/folder(s) found.";
                    in_lv.Items.Clear();
                });

                if (htFolders.ContainsValue(in_folder.ToLower()))
                {
                    htFoldersSearched.Add(htFoldersSearched.Count, in_folder);
                }
            }
            if (bStop || htFolders.Count == htFoldersSearched.Count)
            {
                cancelSearchThread();
            }

            
        }
        private void removeValueFromHT(Hashtable in_ht, string in_value)
        {
            foreach (object k in in_ht.Keys)
            {
                if (in_ht[k].ToString().ToLower().Equals(in_value.ToLower()))
                {
                    in_ht.Remove(k);
                    break;
                }
            }
        
        }
        private void getFiles(string in_ss, string in_folder, ListView in_lv)
        {
            //GC.Collect();
            //Application.DoEvents();

            try
            {
                foreach (string strfile in Directory.GetFiles(in_folder, in_ss))
                {
                    if (bStop) { break; }
                        strLblInfolder = in_folder;
                        FileInfo fi = new FileInfo(strfile);
                        
                        if (lngSize == 0 || (lngSize != 0 && lngSize >= fi.Length))
                        {
                            if (dtDate == dtNlDate || (dtDate != dtNlDate && (dtDate >= fi.CreationTime || dtDate >= fi.LastWriteTime || dtDate >= fi.LastAccessTime)))
                            {
                                if (strMusicInfo == "" || strMusicInfoType == "" || (strMusicInfo != "" && strMusicInfoType != "" && isMusic(fi.FullName) && getItemInfo(fi.FullName, getRealMusicInfoName(strMusicInfoType)).ToLower().Equals(strMusicInfo.ToLower())))
                                {
                                    if (strExifInfo == "" || strExifInfoType == "" || (strExifInfo != "" && strExifInfoType != "" && isPhoto(fi.FullName) && getPhotoInfo(fi.FullName, strExifInfoType).ToLower().Equals(strExifInfo.ToLower())))
                                    {
                                        if(strFileContains=="" || (strFileContains!="" && fileContains(fi.FullName,strFileContains)) )
                                        {
                                            //this.Invoke((MethodInvoker)delegate
                                            //{

                                                ListViewItem lvi = in_lv.Items.Add(fi.Name);
                                                lvi.SubItems.Add(fi.DirectoryName).Tag = fi.DirectoryName;
                                                lvi.SubItems.Add(fi.Directory.Name).Tag = fi.Directory.Name;
                                                lvi.SubItems.Add(getRealSize(fi.Length)).Tag = fi.Length.ToString();
                                                lvi.SubItems.Add(fi.Extension).Tag = fi.Extension;
                                                lvi.SubItems.Add(getDateToString(fi.LastWriteTime)).Tag = fi.LastWriteTime;
                                                lvi.SubItems.Add(getDateToString(fi.LastAccessTime)).Tag = fi.LastAccessTime;
                                                lvi.SubItems.Add(getDateToString(fi.CreationTime)).Tag = fi.CreationTime;
                                                lvi.Tag = fi.Name;
                                                //strLbl = in_lv.Items.Count + " file(s)/folder(s) found.";

                                            //});
                                        }
                                    }
                                }
                            }
                        }
                }
            }
            catch { }
        }
        
        private void cancelSearchThread()
        {
            try
            {
                bStop = true;
                //thSearcher.Abort();
                btnSearch.Enabled = true;
                btnSearch.IsAccessible = true;
                btnSearch.Enabled = false;
                btnSearch.IsAccessible = false;
                btnSearch.Enabled = true;
                btnSearch.IsAccessible = true;
                tSSLblErr.Text = "";
            }
            catch
            { }
            finally
            {
                bPBActive = false;
                strLblInfolder = "";
            }
        }

        private void sortMedia(int in_Column)
        {
            // Determine whether the column is the same as the last column clicked.
            if (in_Column != sortColumn)
            {
                // Set the sort column to the new column.
                sortColumn = in_Column;
                // Set the sort order to ascending by default.
                listView1.Sorting = SortOrder.Ascending;
            }
            else
            {
                // Determine what the last sort order was and change it.
                if (listView1.Sorting == SortOrder.Ascending)
                    listView1.Sorting = SortOrder.Descending;
                else
                    listView1.Sorting = SortOrder.Ascending;
            }
            if (sortColumn != -1)
            {
                // Call the sort method to manually sort.
                listView1.Sort();
                // Set the ListViewItemSorter property to a new ListViewItemComparer
                // object.

                listView1.ListViewItemSorter = new ListViewItemComparer(in_Column,
                    listView1.Sorting, (System.TypeCode)listView1.Columns[in_Column].Tag);
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            checkForm();
            startSearchThread();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancelSearchThread();
        }

        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = cBoxFolder.Text;
            folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.SelectedPath != "" && Directory.Exists(folderBrowserDialog1.SelectedPath))
            {
                cBoxFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        private void chkLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            bFileTypeFolder = false;

            lblContains.Enabled = false;
            txtContains.Enabled = false;

            lblExifInfo.Enabled = false;
            txtExifInfo.Enabled = false;
            cBoxExifInfo.Enabled = false;

            lblMusicInfo.Enabled = false;
            txtMusicInfo.Enabled = false;
            cBoxMusicInfo.Enabled = false;

            checkForm();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cBoxFolder.Text = Application.CommonAppDataPath;
            foreach (ColumnHeader colh in listView1.Columns)
            {
                htColumnHeaders.Add(htColumnHeaders.Count, colh.Text);
            }
            cBoxFolder.Items.Add("All");
            cBoxFolder.Items.Add("Local");
            foreach (DriveInfo di in DriveInfo.GetDrives())
            {
                try
                {
                    cBoxFolder.Items.Add(di.Name);
                }
                catch { }
            }
            loadSearches();

        }
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            sortMedia(e.Column);
        }

        private bool fileContains(string in_path, string in_con)
        {
            bool b_ret = false;

            FileInfo fi = new FileInfo(in_path);
            using (FileStream fs = fi.OpenRead())
            {
                byte[] b = new byte[1024];
                StringBuilder sb = new StringBuilder();
                while (fs.Read(b, 0, b.Length) != 0 && !b_ret)
                {
                    sb.Append(System.Text.Encoding.Default.GetString(b));
                    //MessageBox.Show(sb.ToString(0, sb.Length) + "\n" + sb.ToString(0, sb.Length).IndexOf(in_con) + "\n" + in_con);
                    b_ret = sb.ToString(0, sb.Length).ToLower().Contains(in_con);
                }
                fs.Close();
                fs.Dispose();
            }

            return b_ret;
        }
        private string getRealMusicInfoName(string in_name)
        { 
            string v_ret="";
            if(in_name.Equals("AlbumArtist") || in_name.Equals("AlbumTitle" ) || in_name.Equals( "BeatsPerMinute" ) || in_name.Equals("Composer" ) || in_name.Equals("EncodedBy" ) || in_name.Equals("EncodingSettings" ) || in_name.Equals("Genre" ) || in_name.Equals("Language" ) || in_name.Equals("Lyrics" ) || in_name.Equals("Provider" ) || in_name.Equals("ProviderStyle" ) || in_name.Equals("Publisher" ) || in_name.Equals("TrackNumber" ) || in_name.Equals("Year"))
            {
                v_ret="WM/" + in_name;
            }
            else
            {
                v_ret=in_name;
            }
            return v_ret;
        }
        private string getPhotoInfo(string in_path,string in_name)
        {
            string v_ret="";
            Hashtable ht = getEXIF(in_path);
            foreach (DictionaryEntry de in ht)
            { 
                if(de.Key.ToString().ToLower().Equals(in_name.ToLower()))
                {
                    v_ret=de.Value.ToString();
                    break;
                }
            }            

            return v_ret;
        }
        private Hashtable getEXIF(string in_path)
        {
            Hashtable ht = new Hashtable();
            IDictionary dict = ExifReader.GetReadableProperties(in_path);

            ICollection icolk = dict.Keys;
            ICollection icolv = dict.Values;
            IEnumerator ienumk = icolk.GetEnumerator();
            IEnumerator ienumv = icolv.GetEnumerator();

            ienumk.MoveNext();
            do
            {
                GC.Collect();
                Application.DoEvents();

                try
                {
                    ienumv.MoveNext();
                    if (!ienumv.Current.ToString().ToLower().Equals("[unreadable data]"))
                    {
                        ht.Add(ienumk.Current.ToString(), ienumv.Current.ToString());
                    }

                }
                catch (Exception ex)
                {
                }
            } while (ienumk.MoveNext());
            return ht;
        }
        private Hashtable getMediaInfo(string in_path)
        {
            Hashtable ht = new Hashtable();
            
            IWMPMedia imed = mp.newMedia(in_path);
            for (int z = 0; z < imed.attributeCount; z++)
            {
                ht.Add(imed.getAttributeName(z), imed.getItemInfo(imed.getAttributeName(z)));
            }
            mp.close();
            return ht;
        }

        private string getItemInfo(string in_path, string in_name)
        {
            string v_ret = "";
            try
            {
                IWMPMedia imed = mp.newMedia(in_path);
                v_ret = imed.getItemInfo(in_name);
            }
            catch { }
            return v_ret;
        }
        private string getRealSize(long in_lng)
        {
            string v_ret = "";

            if (in_lng >= 1000000000000)
            {
                v_ret = Math.Round(((double)in_lng) / 1000000000000, 1).ToString() + " TB";
            }
            else if (in_lng >= 1000000000)
            {
                v_ret = Math.Round(((double)in_lng) / 1000000000, 1).ToString() + " GB";
            }
            else if (in_lng >= 1000000)
            {
                v_ret = Math.Round(((double)in_lng) / 1000000, 1).ToString() + " MB";
            }
            else if (in_lng >= 1000)
            {
                v_ret = Math.Round(((double)in_lng) / 1000, 1).ToString() + " kB";
            }
            else
            {
                v_ret = in_lng.ToString() + " B";
            }

            return v_ret;
        }
        private bool isPhoto(string in_path)
        {
            return (in_path.ToLower().EndsWith(".jpg")||in_path.ToLower().EndsWith(".gif")||in_path.ToLower().EndsWith(".png")||in_path.ToLower().EndsWith(".tif"));
        }
        private bool isMusic(string in_path)
        {
            return (in_path.ToLower().EndsWith(".mp3")||in_path.ToLower().EndsWith(".wav")||in_path.ToLower().EndsWith(".wma")||in_path.ToLower().EndsWith(".asf"));
        }
        private bool isMovie(string in_path)
        {
            return (in_path.ToLower().EndsWith(".mpg") || in_path.ToLower().EndsWith(".avi") || in_path.ToLower().EndsWith(".wmv") || in_path.ToLower().EndsWith(".mov"));
        }
        private bool isMedia(string in_path)
        {
            return isMusic(in_path) || isMovie(in_path);
        }
        private void checkForm()
        {
            htFileTypes.Clear();

            System.Collections.IEnumerator in_enum = chkLB.CheckedItems.GetEnumerator();
            while (in_enum.MoveNext())
            {
                if (in_enum.Current.ToString() == "Folders")
                {
                    bFileTypeFolder = true;
                }
                if (in_enum.Current.ToString() == "Documents" || in_enum.Current.ToString() == "Programming")
                {
                    if (lblContains.Enabled == false)
                    {
                        lblContains.Enabled = true;
                        txtContains.Enabled = true;
                    }
                }
                if (in_enum.Current.ToString() == "Photos")
                {
                    if (lblExifInfo.Enabled == false)
                    {
                        lblExifInfo.Enabled = true;
                        txtExifInfo.Enabled = true;
                        cBoxExifInfo.Enabled = true;
                    }
                }
                if (in_enum.Current.ToString() == "Music")
                {
                    if (lblMusicInfo.Enabled == false)
                    {
                        lblMusicInfo.Enabled = true;
                        txtMusicInfo.Enabled = true;
                        cBoxMusicInfo.Enabled = true;
                    }
                }

                string[] strExts = getExts(in_enum.Current.ToString()).Split(System.Text.Encoding.Default.GetChars(System.Text.Encoding.Default.GetBytes(";")));
                foreach (string strss in strExts)
                {
                    if (!htFileTypes.ContainsValue(strss))
                    {
                        htFileTypes.Add(htFileTypes.Count, strss);
                    }
                }
            }
        }
        private void setCheckedFileTypes(string in_names)
        {
            string[] strNames = in_names.Split(System.Text.Encoding.Default.GetChars(System.Text.Encoding.Default.GetBytes(";")));
            foreach (string strss in strNames)
            {
                for(int i=0;i<chkLB.Items.Count;i++)
                {
                    if (chkLB.Items[i].ToString().Equals(strss))
                    {
                        chkLB.SetItemChecked(i, true);
                    }
                }  
            }
        }
        private string getCheckedFileTypes()
        {
            string v_ret = "";
            System.Collections.IEnumerator in_enum = chkLB.CheckedItems.GetEnumerator();
            while (in_enum.MoveNext())
            {
                v_ret += v_ret == "" ? in_enum.Current.ToString() : ";" + in_enum.Current.ToString();
            }
            return v_ret;
            
        }
        private Hashtable searchInCompressed(FileInfo fi,string in_searchstring)
        {
            Hashtable ht_ret = new Hashtable();
            if (fi.Extension.ToLower().Equals(".zip"))
            {
                zip.Zip.ZipFile zf = new ICSharpCode.SharpZipLib.Zip.ZipFile(fi.FullName);
                IEnumerator ienumk = zf.GetEnumerator();
                ienumk.MoveNext();

                do
                {
                    GC.Collect();
                    Application.DoEvents();

                    try
                    {
                        zip.Zip.ZipEntry ze = (zip.Zip.ZipEntry)ienumk.Current;
                        if (ze.Name.ToLower().IndexOf(in_searchstring) != -1)
                        {
                            ht_ret.Add(ht_ret.Count, ze);
                        }


                    }
                    catch (Exception ex)
                    {
                    }
                } while (ienumk.MoveNext());
            }
            return ht_ret;
        }
        private string getExportValue(ColumnHeader in_colh)
        {
            string v_ret = "";
            if (in_colh.Tag.GetType() == "".GetType())
            {
                v_ret = "\"" + in_colh.Text + "\"";
            }
            else
            {
                v_ret = in_colh.Text;
            }
            return v_ret;
        }
        private Control getControl(string in_ctlname)
        {
            foreach (Control ctlgb in this.splitContainer1.Panel1.Controls)
            {
                if (ctlgb.GetType().ToString().Equals("System.Windows.Forms.GroupBox"))
                {
                    foreach (Control ctl in ctlgb.Controls)
                    {
                        if (ctl.Name.Equals(in_ctlname))
                        {
                            return ctl;
                        }
                    }
                }
            }
            return (Control)null;
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo[] fins = new FileInfo[listView1.SelectedItems.Count];
            for (int i=0;i<listView1.SelectedItems.Count;i++)
            {
                ListViewItem lvi = listView1.SelectedItems[i];

                fins.SetValue(new FileInfo(lvi.SubItems[1].Text + "\\" + lvi.Text), i);
                /*
                string strText = "";
                string strPath = lvi.SubItems[1].Text + "\\" + lvi.Text;
                if (isPhoto(strPath))
                {
                    Hashtable ht = getEXIF(strPath);
                    foreach (DictionaryEntry de in ht)
                    {
                        strText += de.Key + ":" + de.Value + "\n";
                    }
                }
                else if (isMedia(strPath))
                {
                    Hashtable ht = getMediaInfo(strPath);
                    foreach (DictionaryEntry de in ht)
                    {
                        strText += de.Key + ":" + de.Value + "\n";
                    }
                }
                MessageBox.Show(strText);
                */

            }
            PropForm pf = new PropForm(fins);
            pf.Show();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            tSSLblSearchedFolders.Text = iLblSearchedFolders>0?iLblSearchedFolders.ToString() + " folder(s) searched.":"";
            tSSLblInfolder.Text = strLblInfolder;
            tSSLbl.Text = strLbl;
            btnSearch.Enabled = bStop;
            tSPB.Visible = bPBActive;

            if (!bStop)
            {
                bFileTypeFolder = false;
                checkForm();
            }
        }
        private void cBoxExifInfo_TextChanged(object sender, EventArgs e)
        {
            strExifInfoType = cBoxExifInfo.Text;
        }
        private void cBoxMusicInfo_TextChanged(object sender, EventArgs e)
        {
            strMusicInfoType = cBoxMusicInfo.Text;
        }
        private void txtContains_TextChanged(object sender, EventArgs e)
        {
            strFileContains = txtContains.Text.Length > 0 ? txtContains.Text : "";
        }
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (cBoxExifInfo.Enabled)
            {
                timer1.Enabled = false;
                ArrayList al = new ArrayList();

                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    ListViewItem lvi = listView1.Items[i];
                    if (isPhoto(lvi.Text))
                    {
                        FileInfo fi = new FileInfo(lvi.SubItems[1].Text + "\\" + lvi.Text);
                        al.Add(fi);
                    }
                    
                }

                FileInfo[] fins = new FileInfo[al.Count];

                for (int i = 0; i < al.Count; i++)
                {
                    FileInfo tmpfi = (FileInfo)al[i];
                    fins.SetValue(tmpfi, i);
                }

                LargePictureForm lpf = new LargePictureForm(fins, listView1.SelectedItems[0].Index);
                lpf.Show();
            }
            else
            {
                try
                {
                    ListViewItem lvi = listView1.SelectedItems[0];
                    Process.Start(lvi.SubItems[1].Text + "\\" + lvi.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void chkSearchZip_CheckedChanged(object sender, EventArgs e)
        {
            bSearchZip = chkSearchZip.Checked;
        }
        private void txtSearchString_TextChanged(object sender, EventArgs e)
        {
            strSearchString = txtSearchString.Text;
        }
        private void txtSize_TextChanged(object sender, EventArgs e)
        {
            lngSize = txtSize.Text.Length > 0 ? long.Parse(txtSize.Text) : 0;
        }
        private void txtDate_TextChanged(object sender, EventArgs e)
        {
            dtDate = txtDate.Text.Length >= 10 ? DateTime.Parse(txtDate.Text) : dtNlDate;
        }
        private void txtAParentFolderName_TextChanged(object sender, EventArgs e)
        {
            strPFolder=txtAParentFolderName.Text.Length>0?txtAParentFolderName.Text:"";
        }
        private void cBoxFolder_TextChanged(object sender, EventArgs e)
        {
            /*
            
            {
                try
                {
                    cBoxFolder.Items.Add(di.Name);
                }
                catch { }
            }             
             */
            if (cBoxFolder.Text.Equals("All"))
            {
                strFolders = "";
                foreach (string strFolder in cBoxFolder.Items)
                {
                    if (!strFolder.Equals("All") && !strFolder.Equals("Local"))
                    {
                        strFolders += strFolders == "" ? strFolder : ";" + strFolder;
                    }
                }
            }
            else if (cBoxFolder.Text.Equals("Local"))
            {
                strFolders = "";
                foreach (DriveInfo di in DriveInfo.GetDrives())
                {
                    if (di.DriveType == DriveType.Fixed)
                    {
                        strFolders += strFolders == "" ? di.Name : ";" + di.Name;
                    }
                }
            }
            else
            {
                strFolders = cBoxFolder.Text.Length > 0 ? cBoxFolder.Text : "";
            }
            htFolders.Clear();
            if (strFolders.IndexOf(";") != -1)
            {

                foreach (string strFolder in strFolders.Split(System.Text.Encoding.Default.GetChars(System.Text.Encoding.Default.GetBytes(";"))))
                {
                    if (Directory.Exists(strFolder))
                    {
                        htFolders.Add(htFolders.Count, strFolder.ToLower());
                    }
                }
            }
            else
            {
                if (Directory.Exists(strFolders))
                {
                    htFolders.Add(htFolders.Count, strFolders.ToLower());
                }
            }
            
        }
        private void chkSubfolders_CheckedChanged(object sender, EventArgs e)
        {
            bSearchFolders = chkSubfolders.Checked;
        }
        private void txtExifInfo_TextChanged(object sender, EventArgs e)
        {
            strExifInfo = txtExifInfo.Text.Length > 0 && strExifInfoType.Length > 0 ? txtExifInfo.Text : "";
        }
        private void txtMusicInfo_TextChanged(object sender, EventArgs e)
        {

            strMusicInfo = txtMusicInfo.Text.Length > 0 && strMusicInfoType.Length > 0 ? txtMusicInfo.Text : "";
        }
        private void cBoxMusicInfo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (listView1.Focused)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.A:

                            foreach (ListViewItem lvi in listView1.Items)
                            {
                                lvi.Selected = true;
                            }

                            break;
                        case Keys.C:
                            saveList(true);
                            break;
                        case Keys.S:
                            saveList();
                            break;
                        case Keys.Enter:
                            startSearchThread();
                            break;
                        case Keys.Escape:
                            cancelSearchThread();
                            break;
                    }
                }
                else
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            startSearchThread();
                            break;
                        case Keys.Escape:
                            cancelSearchThread();
                            break;
                    }
                }
            }
            else if (e.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.O:
                        cBoxFolder.SelectAll();
                        cBoxFolder.Focus();
                        break;
                }
            }
        }

        private void saveList()
        { 
        
        }
        private void saveList(bool bToClipBoard)
        {
            if (bToClipBoard)
            {
                string strClip = "";
                foreach (ColumnHeader colh in listView1.Columns)
                {
                    strClip += colh.Text + ";";
                }
                strClip = strClip.Substring(0, strClip.Length - 1) + "\n";
                foreach (ListViewItem lvi in listView1.SelectedItems)
                {
                    foreach (ListViewItem.ListViewSubItem lsub in lvi.SubItems)
                    {
                        strClip += lsub.Text + ";";
                    }
                    strClip = strClip.Substring(0, strClip.Length - 1) + "\n";
                }
                Clipboard.SetDataObject(strClip, true);
            }
        }
        private void exitApp()
        {
            if (MessageBox.Show("Exit application?", "Exit", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                cancelSearchThread();
                Application.Exit();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancelSearchThread();
        }
        private void clearForm()
        {
            try
            {
                foreach (Control ctlgb in this.splitContainer1.Panel1.Controls)
                {
                    if (ctlgb.GetType().ToString().Equals("System.Windows.Forms.GroupBox"))
                    {
                        foreach (Control ctl in ctlgb.Controls)
                        {
                            string ctlType = ctl.GetType().ToString();
                            if (!ctl.Name.Equals("cBoxSavedSearches"))
                            {
                                switch (ctlType)
                                {
                                    case "System.Windows.Forms.CheckBox":
                                        CheckBox tCB = (CheckBox)ctl;
                                        tCB.Checked = false;
                                        break;
                                    case "System.Windows.Forms.ComboBox":
                                        ComboBox tCoB = (ComboBox)ctl;
                                        tCoB.Text = "";
                                        break;
                                    case "System.Windows.Forms.TextBox":
                                        TextBox tTB = (TextBox)ctl;
                                        tTB.Text = "";
                                        break;
                                    case "System.Windows.Forms.CheckedListBox":
                                        CheckedListBox cLB = (CheckedListBox)ctl;
                                        for (int i = 0; i < cLB.Items.Count; i++)
                                        {
                                            cLB.SetItemChecked(i, false);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch { }

        }
        private void loadSearch(string in_searchname)
        {
            if (in_searchname != "")
            {
                XmlDocument xd = new XmlDocument();
                if (File.Exists(in_filename))
                {
                    xd.Load(in_filename);
                    XmlElement xe = xd.DocumentElement;

                    XmlNode xn = xd.SelectSingleNode("searches/search[@name='" + in_searchname + "']");
                    if (xn != null)
                    {
                        clearForm();
                        foreach (XmlNode txn in xn.ChildNodes)
                        {
                            string ctlType = txn.Attributes[0].Value;
                            string ctlName = txn.Name;
                            string ctlText = txn.InnerText;
                            switch (ctlType)
                            {
                                case "System.Windows.Forms.CheckBox":
                                    CheckBox tCB = (CheckBox)getControl(ctlName);
                                    tCB.Checked = bool.Parse(ctlText);
                                    break;
                                case "System.Windows.Forms.ComboBox":
                                    ComboBox tCoB = (ComboBox)getControl(ctlName);
                                    tCoB.Text = ctlText;
                                    break;
                                case "System.Windows.Forms.TextBox":
                                    TextBox tTB = (TextBox)getControl(ctlName);
                                    tTB.Text = ctlText;
                                    break;
                                case "System.Windows.Forms.CheckedListBox":
                                    setCheckedFileTypes(ctlText);
                                    break;

                            }
                        }
                    }

                }
            }
            else
            {
                clearForm();
            }
        }
        private void loadSearches()
        {
            XmlDocument xd = new XmlDocument();
            if (File.Exists(in_filename))
            {
                xd.Load(in_filename);

                XmlElement xe = xd.DocumentElement;
                XmlNodeList xnl = xd.SelectNodes("searches/search");
                cBoxSavedSearches.Items.Clear();

                foreach (XmlNode xn in xnl)
                {
                    cBoxSavedSearches.Items.Add(xn.Attributes[0].Value);
                }
            }
        }
        private void btnSaveSearch_Click(object sender, EventArgs e)
        {
            saveSearch();
        }


        private void btnDeleteSearch_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this search?", "Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                deleteSearch(cBoxSavedSearches.Text);
            }
        }
        private void saveSearch()
        {
            XmlDocument xd = new XmlDocument();
            if (File.Exists(in_filename))
            {
                xd.Load(in_filename);
            }
            else
            {
                xd.LoadXml("<searches></searches>");
            }
            XmlElement xe = xd.DocumentElement;
            XmlNodeType xnte = XmlNodeType.Element;

            bool bNew = true;
            string searchName = cBoxSavedSearches.Text == "" ? getDateToString(DateTime.Now) : cBoxSavedSearches.Text;

            XmlNode newxn = xd.SelectSingleNode("searches/search[@name='" + searchName + "']");
            if (newxn == null)
            {
                newxn = xd.CreateNode(xnte, "search", null);
            }
            else
            {
                bNew = false;
                newxn.RemoveAll();
            }

            XmlAttribute newxa = xd.CreateAttribute("name");

            newxa.Value = searchName;
            newxn.Attributes.Append(newxa);

            foreach (Control ctlgb in this.splitContainer1.Panel1.Controls)
            {
                if (ctlgb.GetType().ToString().Equals("System.Windows.Forms.GroupBox"))
                {
                    foreach (Control ctl in ctlgb.Controls)
                    {

                        string ctlType = ctl.GetType().ToString();
                        string ctlName = "";
                        string ctlText = "";
                        switch (ctlType)
                        {
                            case "System.Windows.Forms.CheckBox":
                                CheckBox tCB = (CheckBox)ctl;
                                ctlName = tCB.Name;
                                ctlText = tCB.Checked.ToString();
                                break;
                            case "System.Windows.Forms.ComboBox":
                                ComboBox tCoB = (ComboBox)ctl;
                                ctlName = tCoB.Name;
                                ctlText = tCoB.Text;
                                break;
                            case "System.Windows.Forms.TextBox":
                                TextBox tTB = (TextBox)ctl;
                                ctlName = tTB.Name;
                                ctlText = tTB.Text;
                                break;
                            case "System.Windows.Forms.CheckedListBox":
                                CheckedListBox tCLB = (CheckedListBox)ctl;
                                ctlName = tCLB.Name;
                                ctlText = getCheckedFileTypes();
                                break;

                        }
                        if (ctlName != "" && ctlText != "")
                        {
                            XmlNode tmpxn = xd.CreateNode(xnte, ctlName, null);

                            XmlAttribute tmpxa = xd.CreateAttribute("type");
                            tmpxa.Value = ctlType;
                            tmpxn.Attributes.Append(tmpxa);

                            tmpxn.InnerText = ctlText;
                            newxn.AppendChild(tmpxn);
                        }
                    }
                }
            }
            if (bNew)
            {
                xe.AppendChild(newxn);
            }
            xd.Save(in_filename);
            cBoxSavedSearches.Text = searchName;
            loadSearches();
        }
        private void deleteSearch(string in_searchname)
        {
            XmlDocument xd = new XmlDocument();
            if (File.Exists(in_filename))
            {
                xd.Load(in_filename);
                XmlElement xe = xd.DocumentElement;

                XmlNode newxn = xd.SelectSingleNode("searches/search[@name='" + in_searchname + "']");
                if (newxn != null)
                {
                    newxn.ParentNode.RemoveChild(newxn);
                    xd.Save(in_filename);
                }                
            }
            cBoxSavedSearches.Text = "";
            loadSearches();
        }

        private void cBoxSavedSearches_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadSearch(cBoxSavedSearches.Text);
        }

        private void cBoxSavedSearches_TextChanged(object sender, EventArgs e)
        {
            if (cBoxSavedSearches.Text == "")
            {
                clearForm();
            }
        }

    }

}
