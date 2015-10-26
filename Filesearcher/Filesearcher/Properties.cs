using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ImageInfo;
using WMPLib;
using zip = ICSharpCode.SharpZipLib;


namespace Filesearcher
{

    public class PropForm : System.Windows.Forms.Form
    {
        TabControl tabctl = new TabControl();
        ListView lv1 = new ListView();
        ListView lv2 = new ListView();
        ListView lv3 = new ListView();
        WindowsMediaPlayer mp = new WindowsMediaPlayer();

        public PropForm(FileInfo[] in_fins)
        {
            try
            {
                loadForm();

                TabPage tp = new TabPage("General");

                if (in_fins.Length == 1 && in_fins[0] != null)
                {
                    FileInfo fi = in_fins[0];

                    ListViewItem lvi = lv1.Items.Add("");
                    lvi.Group = lv1.Groups[0];
                    lvi.SubItems.Add(fi.Name);

                    lvi = lv1.Items.Add("Location");
                    lvi.Group = lv1.Groups[0];
                    lvi.SubItems.Add(fi.DirectoryName);

                    lvi = lv1.Items.Add("Size");
                    lvi.Group = lv1.Groups[0];
                    lvi.SubItems.Add(fi.Length.ToString());

                    lvi = lv1.Items.Add("Type");
                    lvi.Group = lv1.Groups[0];
                    lvi.SubItems.Add(fi.Extension);

                    lvi = lv1.Items.Add("Accessed");
                    lvi.Group = lv1.Groups[3];
                    lvi.SubItems.Add(fi.LastAccessTime.ToShortDateString() + " " + fi.LastAccessTime.ToShortTimeString());

                    lvi = lv1.Items.Add("Modified");
                    lvi.Group = lv1.Groups[3];
                    lvi.SubItems.Add(fi.LastWriteTime.ToShortDateString() + " " + fi.LastWriteTime.ToShortTimeString());

                    lvi = lv1.Items.Add("Created");
                    lvi.Group = lv1.Groups[3];
                    lvi.SubItems.Add(fi.CreationTime.ToShortDateString() + " " + fi.CreationTime.ToShortTimeString());

                    tp.Controls.Add(lv1);

                    tabctl.TabPages.Add(tp);
                    if (isPhoto(fi.FullName))
                    {
                        tp = new TabPage("Details");
                        AddEXIFInfo(fi, lv2);

                        tp.Controls.Add(lv2);

                        tabctl.TabPages.Add(tp);
                    }
                    if (isZip(fi.FullName))
                    {
                        tp = new TabPage("Archive");
                        AddZipInfo(fi, lv3);

                        tp.Controls.Add(lv3);

                        tabctl.TabPages.Add(tp);                   
                    }

                }
                else if (in_fins.Length > 1)
                {
                    int icntFiles = 0;
                    long iFileSizes = 0;
                    string strLocation = "";
                    for (int ipb = 0; ipb < in_fins.Length; ipb++)
                    {
                        if (in_fins[ipb] != null)
                        {
                            FileInfo fi = in_fins[ipb];

                            icntFiles++;
                            iFileSizes += fi.Length;
                            strLocation = strLocation != fi.DirectoryName && strLocation != "" ? "Multi" : fi.DirectoryName;
                        }
                    }
                    ListViewItem lvi = lv1.Items.Add("");
                    lvi.Group = lv1.Groups[0];
                    lvi.SubItems.Add(icntFiles.ToString() + " files");

                    lvi = lv1.Items.Add("Location");
                    lvi.Group = lv1.Groups[0];
                    lvi.SubItems.Add(strLocation);

                    lvi = lv1.Items.Add("Size");
                    lvi.Group = lv1.Groups[0];
                    lvi.SubItems.Add(iFileSizes.ToString());

                    tp.Controls.Add(lv1);

                    tabctl.TabPages.Add(tp);

                    tp = new TabPage("Details");

                    lvi = lv2.Items.Add("Keyword");
                    lvi.Group = lv2.Groups[1];
                    lvi.SubItems.Add("");

                    lvi = lv2.Items.Add("Comments");
                    lvi.Group = lv2.Groups[1];
                    lvi.SubItems.Add("");

                    tp.Controls.Add(lv2);
                    tabctl.TabPages.Add(tp);
                }

                this.Controls.Add(tabctl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }
        private void loadForm()
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.Width = 500;
            this.Height = 600;

            tabctl.Dock = DockStyle.Fill;

            lv1.Columns.Add("Key", 100);
            lv1.Columns.Add("Value", 300);
            lv1.Dock = DockStyle.Fill;
            lv1.View = View.Details;
            lv1.FullRowSelect = true;
            lv1.GridLines = true;
            lv1.Groups.Add("fi", "FileInfo");
            lv1.Groups.Add("desc", "Description");
            lv1.Groups.Add("attr", "Attributes");
            lv1.Groups.Add("Dates", "Dates");

            lv2.Columns.Add("Key", 100);
            lv2.Columns.Add("Value", 300);
            lv2.Dock = DockStyle.Fill;
            lv2.View = View.Details;
            lv2.FullRowSelect = true;
            lv2.GridLines = true;
            lv2.Groups.Add("img", "Exif");
            lv2.Groups.Add("desc", "Description");
            lv2.Groups.Add("attr", "Attributes");
            lv2.Groups.Add("ObjectData", "ObjectData");


            lv3.Columns.Add("File", 100);
            lv3.Columns.Add("Parent", 100);
            lv3.Columns.Add("Size", 100).TextAlign = HorizontalAlignment.Right;
            lv3.Columns.Add("Date", 100);
            lv3.Dock = DockStyle.Fill;
            lv3.View = View.Details;
            lv3.FullRowSelect = true;
            lv3.GridLines = true;
            lv3.Groups.Add("Archive", "Archive");
            lv3.DoubleClick += new EventHandler(lv3_DoubleClick);

        }

        void lv3_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem lvi = lv3.SelectedItems[0];
            ArrayList al = (ArrayList)lvi.Tag;
            FileInfo fi=(FileInfo)al[0];
            zip.Zip.ZipFile zf=(zip.Zip.ZipFile)al[1];
            zip.Zip.ZipEntry ze=(zip.Zip.ZipEntry)al[2];

            string strFName = fi.FullName;

            string strName = ze.Name;
            string strParent = "";

            if (strName.IndexOf("/") != -1)
            {
                string[] strNameArr = strName.Split(Encoding.Default.GetChars(Encoding.Default.GetBytes("/")));
                strName = strNameArr[strNameArr.Length - 1];
                for (int i = 0; i < strNameArr.Length - 2; i++)
                {
                    strParent += "\\" + strNameArr[i];
                }
            }

            if (isZip(strFName))
            {
                zip.Zip.FastZip fz = new ICSharpCode.SharpZipLib.Zip.FastZip();
                fz.ExtractZip(strFName, Application.UserAppDataPath + strParent, ze.Name);
                
                FileInfo[] fins = new FileInfo[1];
                fins.SetValue(new FileInfo(Application.UserAppDataPath + "\\" + ze.Name), 0);
                PropForm pf = new PropForm(fins);
                pf.Show();
            }            
        }

        private bool isZip(string in_path)
        {
            return (in_path.ToLower().EndsWith(".zip"));
        }
        private bool isPhoto(string in_path)
        {
            return (in_path.ToLower().EndsWith(".jpg") || in_path.ToLower().EndsWith(".gif") || in_path.ToLower().EndsWith(".png") || in_path.ToLower().EndsWith(".tif"));
        }
        private bool isMusic(string in_path)
        {
            return (in_path.ToLower().EndsWith(".mp3") || in_path.ToLower().EndsWith(".wav") || in_path.ToLower().EndsWith(".wma") || in_path.ToLower().EndsWith(".asf"));
        }
        private bool isMovie(string in_path)
        {
            return (in_path.ToLower().EndsWith(".mpg") || in_path.ToLower().EndsWith(".avi") || in_path.ToLower().EndsWith(".wmv") || in_path.ToLower().EndsWith(".mov"));
        }
        private bool isMedia(string in_path)
        {
            return isMusic(in_path) || isMovie(in_path);
        }

        private void AddZipInfo(FileInfo in_fi, ListView in_lv)
        {
            zip.Zip.ZipFile zf = new ICSharpCode.SharpZipLib.Zip.ZipFile(in_fi.FullName);
            IEnumerator ienumk = zf.GetEnumerator();
            ienumk.MoveNext();

            do
            {
                GC.Collect();
                Application.DoEvents();

                try
                {
                    zip.Zip.ZipEntry ze = (zip.Zip.ZipEntry)ienumk.Current;
                    if (ze.IsFile)
                    {
                        string strName = ze.Name;
                        string strParent = in_fi.Name;

                        if (strName.IndexOf("/") != -1)
                        {
                            string[] strNameArr = strName.Split(Encoding.Default.GetChars(Encoding.Default.GetBytes("/")));
                            strName = strNameArr[strNameArr.Length-1];
                            strParent += "\\" + strNameArr[strNameArr.Length - 2];
                        }
                        ListViewItem lvi = in_lv.Items.Add(strName);
                        lvi.SubItems.Add(strParent);
                        lvi.SubItems.Add(getRealSize(ze.Size));
                        lvi.SubItems.Add(ze.DateTime.ToShortDateString() + " " + ze.DateTime.ToLongTimeString());

                        ArrayList al = new ArrayList();
                        al.Add(in_fi);
                        al.Add(zf);
                        al.Add(ze);
                        lvi.Tag = al;

                    }


                }
                catch (Exception ex)
                {
                }
            } while (ienumk.MoveNext());
        }
        private void AddEXIFInfo(FileInfo fi, ListView lv)
        {
            Hashtable ht = getEXIF(fi.FullName);
            foreach (object k in ht.Keys)
            {
                ListViewItem lvi = lv.Items.Add(k.ToString());
                lvi.Group = lv.Groups[0];
                lvi.SubItems.Add(ht[k].ToString());
            }
        }
        private void AddMediaInfo(FileInfo fi, ListView lv)
        {
            Hashtable ht = getMediaInfo(fi.FullName);
            foreach (object k in ht.Keys)
            {
                ListViewItem lvi = lv.Items.Add(k.ToString());
                lvi.Group = lv.Groups[1];
                lvi.SubItems.Add(ht[k].ToString());
            }
        }

        private Hashtable getEXIF(string in_path)
        {
            Hashtable ht = new Hashtable();
            int i_ret = 0;
            IDictionary dict = ExifReader.GetAllProperties(in_path);

            ICollection icolk = dict.Keys;
            ICollection icolv = dict.Values;
            IEnumerator ienumk = icolk.GetEnumerator();
            IEnumerator ienumv = icolv.GetEnumerator();

            ienumk.MoveNext();
            do
            {
                GC.Collect();
                Application.DoEvents();

                i_ret++;
                try
                {
                    ienumv.MoveNext();
                    ht.Add(ienumk.Current.ToString(), ienumv.Current.ToString());

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
    }
}
