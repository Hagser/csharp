using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Xml;
using System.Drawing.Imaging;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

using MySql.Data.MySqlClient;

namespace ExifAddLocation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //0x0002 	GPSLatitude
        //0x0004 	GPSLongitude
        //0x0006 	GPSAltitude
        string strFilename = @"R:\photos\";
        string strTxtFilename = @"R:\photos\geotagging\mypos.txt";
        string strKmlFilename = @"R:\photos\geotagging\mypos.kml";
        int sortColumn = -1;
        int izoom = 14;
        bool bPan = false;
        bool bPath = false;
        bool bPhotos = false;
        bool bRemoving = false;
        bool bContextMenu = false;
        string strMapUrl = "";
        ImageList il = new ImageList();
        string v_key = "ABQIAAAAX7ONIXa6VveCx69Di5MojBTd6NVUoXcjCj77ZWemUzn0rUW7zhRsW-z0QgSl_aFRRzOjPB5dJBIwxQ";
        float fLatPan = (float)0.0;
        float fLonPan = (float)0.0;
        float fPanLat = (float)0.0006437301635742188;
        float fPanLng = (float)0.0002500225485882765;
        int maxzoom = 19;
        int minzoom = 1;
        int iphoto = 0;
        int idate = 1;
        int ilat = 2;
        int ilon = 3;

        private void getExifInfo(string strFilename)
        {
            try
            {
                if (File.Exists(strFilename) && strFilename.ToLower().EndsWith(".jpg"))
                {
                    progressBar2.Maximum = 5;
                    progressBar2.Value = 0;
                    /*
                    Image imadsfg = Bitmap.FromFile(strFilename);
                    int[] ints = imadsfg.PropertyIdList;
                    PropertyItem[] pis = imadsfg.PropertyItems;
                    PropertyItem lat = pis.FirstOrDefault(x => x.Id == 2);
                    PropertyItem lon = pis.FirstOrDefault(x => x.Id == 5);

                    if (lat!=null || lon!=null)
                    {
                        //MessageBox.Show(string.Format("{0},{1}", Encoding.Default.GetString( lat.Value), Encoding.Default.GetString(lon.Value)));
                    }
                    */

                    HybridDictionary hd = ImageInfo.ExifReader.GetAllProperties(strFilename);
                    FileInfo fip = new FileInfo(strFilename);
                    
                    string strTD = "";
                    string strDT = "";
                    string strLat = "";
                    string strLon = "";
                    int iFound = 0;
                    bool bExit = false;
                    foreach (object obj in hd.Keys)
                    {
                        if (obj.ToString().ToLower().Equals("orientation"))
                        {
                            string s = hd[obj].ToString();
                        }
                        if (obj.ToString().ToLower().Equals("thumbnail data"))
                        {
                            strTD = hd[obj].ToString();
                            progressBar2.Value++;
                            iFound++; if (iFound >= 4) { break; }
                        }
                        if (obj.ToString().Contains("0002") || obj.ToString().Equals("Longitude"))
                        {
                            strLat = hd[obj].ToString();//.Replace(",", ".");
                            if (strLat.Split(',').Length > 1)
                                strLat = convertDDD(strLat.Split(','));
                            bExit = checkBox1.Checked;
                            progressBar2.Value++;
                            iFound++; if (iFound >= 4) { break; }
                        }
                        if (obj.ToString().Contains("0004") || obj.ToString().Equals("Latitude"))
                        {
                            strLon = hd[obj].ToString();//.Replace(",", ".");
                            if (strLon.Split(',').Length > 1)
                                strLon = convertDDD(strLon.Split(','));
                            bExit = checkBox1.Checked;
                            progressBar2.Value++;
                            iFound++; if (iFound >= 4) { break; }
                        }
                        if (obj.ToString().ToLower().Equals("date and time of original data generation"))
                        {
                            string p = hd[obj].ToString();
                            string strDate = p.Substring(0, 10).Replace(":", "-");
                            string strHour = p.Substring(11);
                            strDT = strDate + " " + strHour;
                            if (strDate.StartsWith("0000"))
                            {
                                strDT = fip.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            
                            DateTime dtPhoto = DateTime.Parse(strDT);
                            dtPhoto = dtPhoto.AddSeconds(70);
                            //TODO Remove this
                            dtPhoto = dtPhoto.AddHours(-1);
                            strDT = dtPhoto.ToShortDateString() + " " + dtPhoto.ToLongTimeString();
                            progressBar2.Value++;
                            iFound++; if (iFound >= 4) { break; }
                        }
                        //listView1.Items.Add(obj.ToString()).SubItems.Add(hd[obj].ToString());
                    }

                    progressBar2.Value = 5;
                    if (!bExit)
                    {
                        if (strDT == "")
                            strDT = fip.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");

                        if (strDT != "")
                        {
                            ListViewItem lvi = listView1.Items.Add(strFilename);
                            lvi.Tag = strTD;
                            if (strTD != "")
                            {
                                showThumbnails(strFilename, strTD);
                                lvi.ImageKey = strFilename;
                            }

                            lvi.SubItems.Add(strDT);
                            lvi.SubItems.Add(strLon);
                            lvi.SubItems.Add(strLat);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+"\r\n"+ex.StackTrace);
            }
        }
        int iasdifnsdfi = 0;
        private string convertDDD(string[] p)
        {
            try
            {
                var deg = double.Parse(p[0].Trim());
                var min = double.Parse(p[1].Trim());
                var sec = double.Parse(p[2].Trim());

                var dec_min = (min * 1.0 + (sec / 60.0));
                var answer1 = Math.Round(deg * 1.0 + (dec_min / 60.0), 6);

                return answer1.ToString().Replace(",", ".");
            }
            catch
            {
                return p[0]+"."+p[1];
            }
        }

        private void getAllExifInfo(string strFilename)
        {
            if (File.Exists(strFilename))
            {

                HybridDictionary hd = ImageInfo.ExifReader.GetAllProperties(strFilename);
                string strData = "";
                foreach (object obj in hd.Keys)
                {
                    if (!obj.ToString().ToLower().Equals("thumbnail data") && !obj.ToString().ToLower().Equals("maker note"))
                    {
                        strData += obj.ToString() + ":\t" + hd[obj].ToString() + "\n";
                    }
                }
                MessageBox.Show(strData);
            }
        }
        private void findDates(string p)
        {

            DateTime dtPhoto = DateTime.Parse(p);
            DateTime dtLess = DateTime.Parse("2100-01-01");
            DateTime dtMore = DateTime.Parse("2000-01-01");
            int iLess = -1;
            int iMore = -1;
            listView2.SelectedItems.Clear();
            foreach (ListViewItem lvi in listView2.Items)
            {
                DateTime dt = DateTime.Parse(lvi.Text);
                if (dt.CompareTo(dtPhoto) == 1 && iMore == -1)
                {
                    dtMore = new DateTime(dt.Ticks);
                    iMore = lvi.Index;
                    iLess = iMore - 1;
                }
                if (lvi.SubItems.Count == 4)
                {
                    lvi.SubItems.Add(dt.CompareTo(dtPhoto).ToString());
                }
                else if (lvi.SubItems.Count >= 5)
                {
                    lvi.SubItems[4].Text = dt.CompareTo(dtPhoto).ToString();
                }
                long tnew = 0;
                DateTime dtnew = dt;

                if (dt.CompareTo(dtPhoto) == -1)
                {
                    tnew = -1*dt.Ticks;
                    dtnew = dtPhoto.AddTicks(tnew);
                }
                else if (dt.CompareTo(dtPhoto) == 1)
                {
                    tnew = -1*dtPhoto.Ticks;
                    dtnew = dt.AddTicks(tnew);
                }

                if (lvi.SubItems.Count == 5)
                {
                    lvi.SubItems.Add(dtnew.ToShortDateString() + " " + dtnew.ToLongTimeString());
                }
                else if (lvi.SubItems.Count == 6)
                {
                    lvi.SubItems[5].Text = dtnew.ToShortDateString() + " " + dtnew.ToLongTimeString();
                }
            }
            if (iLess != -1)
            {
                listView2.SelectedIndices.Add(iLess);
                listView2.Items[iLess].EnsureVisible();
            }
            if (iMore != -1)
            {
                listView2.SelectedIndices.Add(iMore);
                listView2.Items[iMore].EnsureVisible();
            }
            //listView2.Focus();
        }

        private void showThumbnails(FileInfo fi)
        {
            HybridDictionary hd = ImageInfo.ExifReader.GetAllProperties(fi.FullName);
            foreach (object obj in hd.Keys)
            {
                if (obj.ToString().ToLower().Equals("thumbnail data"))
                {
                    if(hd[obj]!=null && !string.IsNullOrEmpty(hd[obj].ToString()))
                        showThumbnails(fi.FullName,hd[obj].ToString());
                    break;
                }

            }

        }

        private void showThumbnails(string strpath,string p)
        {
            if (!string.IsNullOrEmpty(p))
            {
                byte[] imageBytes = getBytes(p);

                MemoryStream stream = new MemoryStream(imageBytes.Length);
                stream.Write(imageBytes, 0, imageBytes.Length);

                pictureBox1.Image = Image.FromStream(stream);
                il.Images.Add(strpath, pictureBox1.Image);
            }
            else
            {
                pictureBox1.Image = Bitmap.FromFile(strpath);
                il.Images.Add(strpath, pictureBox1.Image);
            }
        }

        private byte[] getBytes(string p)
        {
            byte[] b=new byte[4096];
            byte ib = new byte();
            string strCurr = "";
            try
            {
                ArrayList al = new ArrayList();
                p = p.Replace(" ", "");
                foreach (string s in p.Split(",".ToCharArray()[0]))
                {
                    al.Add(s);
                }
                b = new byte[al.Capacity];
                IEnumerator ien = al.GetEnumerator();
                int i = 0;

                while (ien.MoveNext())
                {
                    ib = new byte();
                    strCurr = ien.Current.ToString();
                    ib = (byte)int.Parse(ien.Current.ToString());
                    b.SetValue(ib, i);
                    i++;
                }
            }
            catch (Exception ex)
            {
                string s = ex.StackTrace;
            }
            return b;
        }

        private void getPosInfo(string strFilename)
        {
            if (strFilename.EndsWith("txt"))
            {
                getPosInfoTxt(strFilename);
            }
            else if (strFilename.EndsWith("kml"))
            {
                getPosInfoKml(strFilename);
            }
            else if (strFilename.EndsWith("gpx"))
            {
                getPosInfoGpx(strFilename);
            }
        }

        private void getPosInfoTxt(string strTxtFilename)
        {
            if (File.Exists(strTxtFilename))
            {
                FileStream fs = new FileStream(strTxtFilename, FileMode.Open, FileAccess.ReadWrite);
                StreamReader sr = new StreamReader(fs);

                while (!sr.EndOfStream)
                {
                    addToListView(sr.ReadLine());
                }
            }
        }
        private void getPosInfoKml(string strTxtFilename)
        {
            if (File.Exists(strTxtFilename))
            {
                FileStream fs = new FileStream(strTxtFilename, FileMode.Open, FileAccess.ReadWrite,FileShare.ReadWrite);
                StreamReader sr = new StreamReader(fs);
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(sr.ReadToEnd());
                XmlElement xe = xd.DocumentElement;


                int widx = 0;
                int cidx = 0;
                Dictionary<int, string> whenDict = new Dictionary<int, string>();
                Dictionary<int, string> coordDict = new Dictionary<int, string>();
                foreach (XmlNode xnd in xe.ChildNodes)
                {
                    foreach (XmlNode xnp in xnd.ChildNodes)
                    {
                        if (xnp.LocalName.ToLower().Equals("placemark"))
                        {
                            foreach (XmlNode xnt in xnp.ChildNodes)
                            {
                                if (xnt.LocalName.ToLower().Equals("track"))
                                {
                                    foreach (XmlNode xnw in xnt.ChildNodes)
                                    {
                                        if (xnw.LocalName.ToLower().Equals("when"))
                                        {
                                            string when = xnw.InnerText;
                                            whenDict.Add(widx, when);
                                            widx++;
                                        }
                                        else if (xnw.LocalName.ToLower().Equals("coord"))
                                        {
                                            string coord = xnw.InnerText;
                                            coordDict.Add(cidx, coord);
                                            cidx++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (int idx in whenDict.Keys)
                {

                    if (whenDict.ContainsKey(idx) && coordDict.ContainsKey(idx))
                    {
                        addToListView(fixKmlData(whenDict[idx], coordDict[idx]));
                    }
                }
            }
        }
        private void getPosInfoGpx(string strTxtFilename)
        {
            if (File.Exists(strTxtFilename))
            {
                FileStream fs = new FileStream(strTxtFilename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamReader sr = new StreamReader(fs);
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(sr.ReadToEnd());
                XmlElement xe = xd.DocumentElement;


                int widx = 0;
                int cidx = 0;
                Dictionary<int, string> whenDict = new Dictionary<int, string>();
                Dictionary<int, string> coordDict = new Dictionary<int, string>();
                foreach (XmlNode xnd in xe.ChildNodes)
                {
                    foreach (XmlNode xnp in xnd.ChildNodes)
                    {
                        if (xnp.LocalName.ToLower().Equals("trkseg"))
                        {
                            foreach (XmlNode xnt in xnp.ChildNodes)
                            {
                                if (xnt.LocalName.ToLower().Equals("trkpt"))
                                {
                                    string strAlt = "";
                                    foreach (XmlNode xnw in xnt.ChildNodes)
                                    {
                                        if (xnw.LocalName.ToLower().Equals("time"))
                                        {
                                            string when = xnw.InnerText;
                                            whenDict.Add(widx, when);
                                            widx++;
                                        }
                                        else if (xnw.LocalName.ToLower().Equals("ele"))
                                        {
                                            strAlt = xnw.InnerText;
                                        }
                                    }
                                    XmlAttribute xalat = xnt.Attributes[1];
                                    XmlAttribute xalon = xnt.Attributes[0];
                                    
                                    if (xalat !=null && xalon!=null)
                                    {
                                        int ip = 7;
                                        string strLat = xalat.Value;
                                        if (strLat.Contains("."))
                                        {
                                            if (strLat.Length >= strLat.IndexOf(".") + ip)
                                                strLat = strLat.Substring(0, strLat.IndexOf(".") + ip);
                                        }
                                        string strLon = xalon.Value;
                                        if (strLon.Contains("."))
                                        {
                                            if (strLon.Length >= strLon.IndexOf(".") + ip)
                                                strLon = strLon.Substring(0, strLon.IndexOf(".") + ip);
                                        }

                                        if (strAlt.Contains("."))
                                        {
                                            if (strAlt.Length >= strAlt.IndexOf(".") + 2)
                                                strAlt = strAlt.Substring(0, strAlt.IndexOf(".") + 2);
                                        }

                                        string coord = strLat + " " + strLon + " " + strAlt;
                                        coordDict.Add(cidx, coord);
                                        cidx++;
                                    }                                    
                                }
                            }
                        }
                    }
                }

                foreach (int idx in whenDict.Keys)
                {

                    if (whenDict.ContainsKey(idx) && coordDict.ContainsKey(idx))
                    {
                        addToListView(fixKmlData(whenDict[idx], coordDict[idx]));
                    }
                }
            }
        }

        private string fixKmlData(string when, string where)
        {
            string[] coords = where.Split(" ".ToCharArray()[0]);
            //2010-04-30 18:59:28	73.0	58.702910	15.768880
            //2011-09-24T09:20:58Z
            //15.557356 58.68145 110.0
            DateTime result;
            if(DateTime.TryParse(when,out result))
            {
                when = result.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return string.Format("{0}	{1}	{2}	{3}", when.Replace("T", " ").Replace("Z", " "), coords[2], coords[1], coords[0]);
        }

        private void addToListView(string p)
        {
            string[] parr = p.Split("\t".ToCharArray()[0]);
            ListViewItem lvi = listView2.Items.Add(parr[0]);
            if (parr.Length > 3)
            {
                lvi.SubItems.Add(parr[1]);
                lvi.SubItems.Add(parr[2]);
                lvi.SubItems.Add(parr[3]);
            }
        }

        private void sortList(ListView lvi, int in_Column)
        {
            // Determine whether the column is the same as the last column clicked.
            if (in_Column != sortColumn)
            {
                // Set the sort column to the new column.
                sortColumn = in_Column;
                // Set the sort order to ascending by default.
                lvi.Sorting = SortOrder.Ascending;
            }
            else
            {
                // Determine what the last sort order was and change it.
                if (lvi.Sorting == SortOrder.Ascending)
                    lvi.Sorting = SortOrder.Descending;
                else
                    lvi.Sorting = SortOrder.Ascending;
            }
            if (sortColumn != -1)
            {
                // Call the sort method to manually sort.
                lvi.Sort();
                // Set the ListViewItemSorter property to a new ListViewItemComparer
                // object.

                lvi.ListViewItemSorter = new ListViewItemComparer(in_Column,
                    lvi.Sorting, System.TypeCode.String);
            }
            lvi.Sorting = SortOrder.None;
        }

        private void showMap(string in_url)
        {
            if (!chkGetMap.Checked)
                return;
            try
            {
                
                WebRequest req = WebRequest.Create(in_url);
                req.Method = "GET";

                WebResponse res = req.GetResponse();
                pictureBox2.Image = Image.FromStream(res.GetResponseStream());
            }
            catch(Exception ex) {
                MessageBox.Show("Error getting map:" + ex.Message);
            }
        }
        private void showMap()
        {
            showMap(getMapUrl());
        }
        private int savePosInfo()
        {
            return savePosInfo(false);
        }
        private int savePosInfo(bool bSaveOwn)
        {
            int i_cnt = 0;
            progressBar2.Maximum = 7;
            progressBar2.Value = 0;
            progressBar1.Maximum = listView1.SelectedItems.Count * 7;
            progressBar1.Value = 0;
            string strAlt = txtAlt.Text;
            string strLat = txtLat.Text;
            string strLon = txtLon.Text;
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                if (bSaveOwn)
                {
                    if (lvi.SubItems.Count >= 3)
                    {
                        strLat = lvi.SubItems[ilat].Text;
                        i_cnt++;
                    }
                    if (lvi.SubItems.Count >= 4)
                    {
                        strLon = lvi.SubItems[ilon].Text;
                        i_cnt++;
                    }
                }
                progressBar2.Value = 0;
                int istart1 = lvi.Text.IndexOf("\\photos");
                int istart2 = lvi.Text.IndexOf(":\\") + 3;
                int istart3 = lvi.Text.IndexOf("\\\\");
                string strFilename = txtOutput.Text + lvi.Text.Substring(Math.Max(istart1, Math.Max(istart3, istart2)));
                bool bSaved = false;

                try
                {
                    Directory.CreateDirectory(new FileInfo(strFilename).DirectoryName);
                }
                catch { }
                if (bMySqlPhotos)
                {
                    saveToMySql(lvi.Text,strLat,strLon);
                }
                if (System.IO.File.Exists(lvi.Text))
                {
                    //FileInfo fi = new FileInfo(strFilename);
                    
                    Image img = Bitmap.FromFile(lvi.Text);

                    //if (!fi.IsReadOnly)
                    {
                        if (strAlt != "")
                        {
                            bSaved = true;
                            byte[] b = Encoding.Default.GetBytes(strAlt.Replace(",", ".")+(char)0);
                            
                            if (img.PropertyIdList.Contains(6))
                            {
                                System.Drawing.Imaging.PropertyItem pi = img.GetPropertyItem(6);
                                pi.Len = b.Length;
                                pi.Type = 2;
                                pi.Value = b;
                                img.SetPropertyItem(pi);
                            }
                            else
                            {
                                img.SetPropertyItem(CreateProperty(img, 6, b.Length, 2, b));
                            }
                        }
                        Application.DoEvents();
                        progressBar1.Value++;
                        progressBar2.Value++;
                        if (strLat != "")
                        {
                            bSaved = true;
                            byte[] b = Encoding.Default.GetBytes(strLat.Replace(",", ".") + (char)0);

                            if (img.PropertyIdList.Contains(2))
                            {
                                System.Drawing.Imaging.PropertyItem pi = img.GetPropertyItem(2);
                            
                                pi.Len = b.Length;
                                pi.Type = 2;
                                pi.Value = b;
                                img.SetPropertyItem(pi);
                            }
                            else
                            {
                                img.SetPropertyItem(CreateProperty(img, 2, b.Length, 2, b));

                            }
                            lvi.SubItems[ilat].Text = strLat;
                        }
                        Application.DoEvents();
                        progressBar1.Value++;
                        progressBar2.Value++;
                        if (strLon != "")
                        {
                            bSaved = true;
                            byte[] b = Encoding.Default.GetBytes(strLon.Replace(",", ".") + (char)0);
                            if (img.PropertyIdList.Contains(4))
                            {
                                System.Drawing.Imaging.PropertyItem pi = img.GetPropertyItem(4);
                                pi.Len = b.Length;
                                pi.Type = 2;
                                pi.Value = b;
                                img.SetPropertyItem(pi);
                            }
                            else
                            {
                                img.SetPropertyItem(CreateProperty(img, 4, b.Length, 2, b));

                            }
                            lvi.SubItems[ilon].Text = strLon;
                        }
                        Application.DoEvents();
                        progressBar1.Value++;
                        progressBar2.Value++;
                        if (bSaved)
                        {
                            byte[] b = new byte[4]; b.SetValue((byte)2, 0); b.SetValue((byte)2, 1); b.SetValue((byte)0, 2); b.SetValue((byte)0, 3);
                            System.Drawing.Imaging.PropertyItem pi;
                            if (img.PropertyIdList.Contains(0))
                            {
                                pi = img.GetPropertyItem(0);
                                pi.Len = b.Length;
                                pi.Type = 1;
                                pi.Value = b;
                                img.SetPropertyItem(pi);
                            }
                            else
                            {
                                img.SetPropertyItem(CreateProperty(img, 0, 4, 1, b));
                            }
                            Application.DoEvents();
                            progressBar1.Value++;
                            progressBar2.Value++;
                            byte[] bn = Encoding.Default.GetBytes("N" + (char)0);
                            if (img.PropertyIdList.Contains(1))
                            {
                                pi = img.GetPropertyItem(1);
                                pi.Len = bn.Length;
                                pi.Type = 2;
                                pi.Value = bn;
                                img.SetPropertyItem(pi);
                            }
                            else
                            {
                                
                                img.SetPropertyItem(CreateProperty(img, 1, 1, 2, bn));

                            }
                            Application.DoEvents();
                            progressBar1.Value++;
                            progressBar2.Value++;
                            byte[] be = Encoding.Default.GetBytes("E" + (char)0);
                            if (img.PropertyIdList.Contains(3))
                            {
                                pi = img.GetPropertyItem(3);
                                pi.Len = be.Length;
                                pi.Type = 2;
                                pi.Value = be;
                                img.SetPropertyItem(pi);
                            }
                            else
                            {
                                img.SetPropertyItem(CreateProperty(img, 3, 1, 2, be));
                            }
                            Application.DoEvents();
                            progressBar1.Value++;
                            progressBar2.Value++;
                            byte[] b0 = Encoding.Default.GetBytes("0" + (char)0);
                            if (img.PropertyIdList.Contains(5))
                            {
                                pi = img.GetPropertyItem(5);
                                pi.Len = b0.Length;
                                pi.Type = 2;
                                pi.Value = b0;
                                img.SetPropertyItem(pi);
                            }
                            else
                            {
                                img.SetPropertyItem(CreateProperty(img, 5, 1, 2, b0));
                            }

                            Application.DoEvents();
                            progressBar1.Value++;
                            progressBar2.Value++;
                        }

                        img.Save(strFilename);
                        img.Dispose();
                        img = null;
                    }
                    /*else
                    {
                        Application.DoEvents();
                        progressBar1.Value = progressBar1.Value+7;
                        progressBar2.Value = 7;
                    }*/
                    //getExifInfo(strFilename);
                }
                else
                {
                    Application.DoEvents();
                    progressBar1.Value = progressBar1.Value + 7;
                    progressBar2.Value = 7;
                }
            }
            return i_cnt;
        }
        private void savePosInfo(string file,string strAlt,string strLat,string strLon)
        {
            progressBar2.Maximum = 7;
            progressBar2.Value = 0;
            int istart1 = file.IndexOf("\\photos");
            int istart2 = file.IndexOf(":\\") + 3;
            int istart3 = file.IndexOf("\\\\");
            string strFilename = txtOutput.Text + file.Substring(Math.Max(istart1, Math.Max(istart3, istart2)));
            bool bSaved = false;

            try
            {
                Directory.CreateDirectory(new FileInfo(strFilename).DirectoryName);
            }
            catch { }
            saveToMySql(file, strLat, strLon);
            if (System.IO.File.Exists(file))
            {
                //FileInfo fi = new FileInfo(strFilename);

                Image img = Bitmap.FromFile(file);

                //if (!fi.IsReadOnly)
                {
                    if (strAlt != "")
                    {
                        bSaved = true;
                        byte[] b = Encoding.Default.GetBytes(strAlt.Replace(",", ".") + (char)0);

                        if (img.PropertyIdList.Contains(6))
                        {
                            System.Drawing.Imaging.PropertyItem pi = img.GetPropertyItem(6);
                            pi.Len = b.Length;
                            pi.Type = 2;
                            pi.Value = b;
                            img.SetPropertyItem(pi);
                        }
                        else
                        {
                            img.SetPropertyItem(CreateProperty(img, 6, b.Length, 2, b));
                        }
                    }
                    Application.DoEvents();
                    progressBar2.Value++;
                    if (strLat != "")
                    {
                        bSaved = true;
                        byte[] b = Encoding.Default.GetBytes(strLat.Replace(",", ".") + (char)0);

                        if (img.PropertyIdList.Contains(2))
                        {
                            System.Drawing.Imaging.PropertyItem pi = img.GetPropertyItem(2);

                            pi.Len = b.Length;
                            pi.Type = 2;
                            pi.Value = b;
                            img.SetPropertyItem(pi);
                        }
                        else
                        {
                            img.SetPropertyItem(CreateProperty(img, 2, b.Length, 2, b));

                        }
                            
                    }
                    Application.DoEvents();
                    progressBar2.Value++;
                    if (strLon != "")
                    {
                        bSaved = true;
                        byte[] b = Encoding.Default.GetBytes(strLon.Replace(",", ".") + (char)0);
                        if (img.PropertyIdList.Contains(4))
                        {
                            System.Drawing.Imaging.PropertyItem pi = img.GetPropertyItem(4);
                            pi.Len = b.Length;
                            pi.Type = 2;
                            pi.Value = b;
                            img.SetPropertyItem(pi);
                        }
                        else
                        {
                            img.SetPropertyItem(CreateProperty(img, 4, b.Length, 2, b));

                        }
                            
                    }
                    Application.DoEvents();
                    progressBar2.Value++;
                    if (bSaved)
                    {
                        byte[] b = new byte[4]; b.SetValue((byte)2, 0); b.SetValue((byte)2, 1); b.SetValue((byte)0, 2); b.SetValue((byte)0, 3);
                        System.Drawing.Imaging.PropertyItem pi;
                        if (img.PropertyIdList.Contains(0))
                        {
                            pi = img.GetPropertyItem(0);
                            pi.Len = b.Length;
                            pi.Type = 1;
                            pi.Value = b;
                            img.SetPropertyItem(pi);
                        }
                        else
                        {
                            img.SetPropertyItem(CreateProperty(img, 0, 4, 1, b));
                        }
                        Application.DoEvents();
                        progressBar2.Value++;
                        byte[] bn = Encoding.Default.GetBytes("N" + (char)0);
                        if (img.PropertyIdList.Contains(1))
                        {
                            pi = img.GetPropertyItem(1);
                            pi.Len = bn.Length;
                            pi.Type = 2;
                            pi.Value = bn;
                            img.SetPropertyItem(pi);
                        }
                        else
                        {

                            img.SetPropertyItem(CreateProperty(img, 1, 1, 2, bn));

                        }
                        Application.DoEvents();
                        progressBar2.Value++;
                        byte[] be = Encoding.Default.GetBytes("E" + (char)0);
                        if (img.PropertyIdList.Contains(3))
                        {
                            pi = img.GetPropertyItem(3);
                            pi.Len = be.Length;
                            pi.Type = 2;
                            pi.Value = be;
                            img.SetPropertyItem(pi);
                        }
                        else
                        {
                            img.SetPropertyItem(CreateProperty(img, 3, 1, 2, be));
                        }
                        Application.DoEvents();
                        progressBar2.Value++;
                        byte[] b0 = Encoding.Default.GetBytes("0" + (char)0);
                        if (img.PropertyIdList.Contains(5))
                        {
                            pi = img.GetPropertyItem(5);
                            pi.Len = b0.Length;
                            pi.Type = 2;
                            pi.Value = b0;
                            img.SetPropertyItem(pi);
                        }
                        else
                        {
                            img.SetPropertyItem(CreateProperty(img, 5, 1, 2, b0));
                        }

                        Application.DoEvents();
                        progressBar2.Value++;
                    }

                    img.Save(strFilename);
                    img.Dispose();
                    img = null;
                }
            }
            else
            {
                Application.DoEvents();
                progressBar2.Value = 7;
            }
        }

        private void saveToMySql(string p,string lat,string lon)
        {
            MySqlCommand com = new MySqlCommand(string.Format("update photodatabase.photos set lat='{0}',lon='{1}' where photopath='{2}'", lat.Replace(",", "."), lon.Replace(",", "."), p.Replace("\\\\ubuntu\\share$\\", "/srv/samba/share/").Replace("\\", "/")), getConnection());
            com.ExecuteNonQuery();
            com.Connection.Close();
        }

        private MySqlConnection getConnection()
        {
            string connectionString = "SERVER=ubuntu;DATABASE=photodatabase;UID=photos;PASSWORD=photos_;";
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            return con;
        }

        private static PropertyItem CreateProperty(Image image, int id, int length, short type, byte[] value)
        {
            
            try
            {
                PropertyItem item = image.GetPropertyItem(image.PropertyIdList[0]);
                item.Id = id;
                item.Len = length;
                item.Type = type;
                item.Value = value;
                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string getMapUrl(float fLat,float fLon)
        {
            string strUrl = "http://maps.google.com/maps/api/staticmap?zoom=" + izoom.ToString() + "&size=" + Math.Min(600, pictureBox2.Width).ToString() + "x" + Math.Min(600, pictureBox2.Height).ToString() + "&maptype=roadmap&mobile=false#markers##path#&sensor=false&key=" + v_key;
            string strMarkers = "";
            int icnt = 1;

            string strMarker = "&markers=label:" + icnt.ToString() + "|" + fLat.ToString().Replace(",", ".") + "," + fLon.ToString().Replace(",", ".") + "|";
            strMarkers += strMarker;
            strUrl = strUrl.Replace("#markers#", strMarkers);
            strUrl = strUrl.Replace("#path#", strPath);
            strMapUrl = strUrl;
            return strUrl;
        }
        private string getMapUrl()
        {
            string strUrl = "http://maps.google.com/maps/api/staticmap?zoom=" + izoom.ToString() + "&size=" + Math.Min(600, pictureBox2.Width).ToString() + "x" + Math.Min(600, pictureBox2.Height).ToString() + "&maptype=roadmap&mobile=false#markers##path#&sensor=false&key=" + v_key;
            string strMarkers = "";
            int icnt = 1;
            foreach (ListViewItem lvi in listView2.SelectedItems)
            {

                string strMarker = "&markers=label:" + icnt.ToString() + "|" + lvi.SubItems[ilat].Text.Replace(",", ".") + "," + lvi.SubItems[ilon].Text.Replace(",", ".") + "|";
                if (strUrl.Length + strMarker.Length + strMarkers.Length > 1900)
                { break; }
                strMarkers += strMarker;
                icnt++;

            }
            if (listView2.SelectedItems.Count == 0)
            {
                strMarkers = "&markers=label:" + icnt.ToString() + "|" + fLatPan.ToString().Replace(",", ".") + "," + fLonPan.ToString().Replace(",", ".");
            }
            strUrl = strUrl.Replace("#markers#", strMarkers);
            strUrl = strUrl.Replace("#path#", strPath);
            strMapUrl = strUrl;
            return strUrl;
        }
        private string getMapPhotosUrl()
        {
            string strUrl = "http://maps.google.com/maps/api/staticmap?zoom=" + izoom.ToString() + "&size=" + Math.Min(600, pictureBox2.Width).ToString() + "x" + Math.Min(600, pictureBox2.Height).ToString() + "&maptype=roadmap&mobile=false#markers#&sensor=false&key=" + v_key;
            string strMarkers = "";
            int icnt = 1;
            ArrayList al = new ArrayList();
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                if (lvi.SubItems.Count >= 4 && lvi.SubItems[ilat].Text != "" && lvi.SubItems[ilon].Text != "")
                {
                    string strLL = lvi.SubItems[ilat].Text+"_"+lvi.SubItems[ilon].Text;
                    if (!al.Contains(strLL))
                    {
                        al.Add(strLL);
                        string strMarker = "&markers=label:" + icnt.ToString() + "|" + lvi.SubItems[ilat].Text.Replace(",", ".") + "," + lvi.SubItems[ilon].Text.Replace(",", ".") + "|";
                        if (strUrl.Length + strMarker.Length + strMarkers.Length > 1900)
                        { break; }
                        strMarkers += strMarker;
                        icnt++;
                    }
                }

            }
            strUrl = strUrl.Replace("#markers#", strMarkers);
            strMapUrl = strUrl;
            return strUrl;
        }
        string strPath = "";
        private string getMapPathUrl()
        {
            string strUrl = "http://maps.google.com/maps/api/staticmap?size=" + Math.Min(600, pictureBox2.Width).ToString() + "x" + Math.Min(600, pictureBox2.Height).ToString() + "&maptype=roadmap&mobile=false#path#&sensor=false&key=" + v_key;
            string strMarkers = "";
            int icnt = 1;
            List<Coordinate> clist = new List<Coordinate>();
            foreach (ListViewItem lvi in listView2.SelectedItems.Count > 0 ? listView2.SelectedItems : listView1.SelectedItems)
            {
                icnt++;
                if(icnt%2==0)
                {
                    clist.Add(new Coordinate(lvi.SubItems[ilat].Text.Replace(".", ","), lvi.SubItems[ilon].Text.Replace(".", ",")));
                }
            }
            strMarkers += "&path=color:0x0000ff|weight:5|enc:" + EncodePolyline.EncodeCoordinates(clist);

            strPath = strMarkers;
            strUrl = strUrl.Replace("#path#", strPath);
            
            strMapUrl = strUrl;
            return strUrl;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            bMySqlPhotos = false;
            listView1.Items.Clear();
            il.Images.Clear();
            if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                if (openFileDialog1.FileNames.Length > 1)
                {
                    progressBar1.Maximum = openFileDialog1.FileNames.Length;
                    progressBar1.Value = 0;
                    foreach (string strFname in openFileDialog1.FileNames)
                    {
                        getExifInfo(strFname);
                        Application.DoEvents();
                        progressBar1.Value++;
                    }
                }
                else
                {
                    strFilename = openFileDialog1.FileName;
                    getExifInfo(strFilename);
                }
            }
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            if (txtLat.Text != "" && txtLon.Text != "")
            {
                int i=savePosInfo();
            }
            else
            {
                
                if (savePosInfo(true) == 0)
                {
                    MessageBox.Show("Enter Lat and Lon!");
                }
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            listView2.Items.Clear();
            openFileDialog2.Multiselect = true;
            if (openFileDialog2.ShowDialog() != DialogResult.Cancel)
            {
                foreach(string strTxtFilename in openFileDialog2.FileNames)
                    getPosInfo(strTxtFilename);
            }
            listView2.Sort();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            bPath = false;
            showMap();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            bPath = true;
            showMap(getMapPathUrl());
            
        }
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            
            sortList(listView1, e.Column);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            listView1.SmallImageList = il;
            listView1.LargeImageList = il;
            Size imsize = new Size(72, 72);
            il.ImageSize=imsize;
            loadLocations();

            this.webBrowser1.DocumentText = (new WebClient()).DownloadString("http://ubuntu/maponly.html");

            //getPosInfo(strTxtFilename);
            //getPosInfo(strKmlFilename);
            //getExifInfo(strFilename);
            //sortList(0);
            
        }
        List<Coordinate> locations = new List<Coordinate>();
        private void loadLocations()
        {
            string strPath = Application.CommonAppDataPath + "\\locations.xml";
            if (!File.Exists(strPath))
            {
                /*
                XmlDocument xd = new XmlDocument();
                xd.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExifAddLocation.Locations.xml"));
                XmlElement xe = xd.DocumentElement;
                foreach (XmlNode xn in xe.SelectNodes("location"))
                {
                    if (xn.Attributes.Count >= 3)
                    {
                        string strName = xn.Attributes["name"].Value;
                        string strLat = xn.Attributes["lat"].Value;
                        string strLng = xn.Attributes["lng"].Value;

                        Coordinate coord = new Coordinate(strLat.Replace(".", ","), strLng.Replace(".", ",")) { Name = strName };
                        locations.Add(coord);
                    }
                }*/
                using(StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExifAddLocation.Locations.xml")))
                locations = SeDes.ToObj(sr.ReadToEnd(), locations) as List<Coordinate>;
                File.WriteAllText(strPath, SeDes.ToXml(locations));
            }
            else
            {
                locations = SeDes.ToObj(File.ReadAllText(strPath), locations) as List<Coordinate> ;
                if (locations.Count == 0)
                {
                    File.Delete(strPath);
                    loadLocations();
                }
            }

            listBox3.DisplayMember = "Name";
            listBox3.Sorted = true;
            listBox3.DataSource = locations;
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (listView2.SelectedItems.Count == 1)
            {
                ListViewItem lvi = listView2.SelectedItems[0];
                txtAlt.Text = lvi.SubItems[1].Text;
                txtLat.Text = lvi.SubItems[ilat].Text;
                txtLon.Text = lvi.SubItems[ilon].Text;
                fLatPan = float.Parse(txtLat.Text.Replace(".",","));
                fLonPan = float.Parse(txtLon.Text.Replace(".", ","));
            }
            
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (!bRemoving && !bContextMenu)
            {
                if (listView1.SelectedItems.Count == 1)
                {
                    //showThumbnails(new FileInfo(listView1.SelectedItems[0].SubItems[0].Text));
                    showThumbnails(listView1.SelectedItems[0].SubItems[0].Text, listView1.SelectedItems[0].Tag.ToString());
                    if (listView1.SelectedItems[0].SubItems.Count > 1 && chkGetCLoc.Checked)
                    {
                        findDates(listView1.SelectedItems[0].SubItems[1].Text);
                        //bPath = false;
                        //showMap();
                    }
                    listView1.SelectedItems[0].ImageKey = listView1.SelectedItems[0].SubItems[0].Text;

                }
                else if (listView1.SelectedItems.Count > 1)
                {
                    //showMap(getMapPhotosUrl());
                }
            }
            
        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            /*
            if (izoom < maxzoom)
            {
                izoom++;
            }
            if (bPath)
            {
                showMap(getMapPathUrl());
            }
            else
            {
                showMap();
            }
            */
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (izoom < maxzoom)
            {
                izoom++;
            }
            if (bPan)
            { 
                showMap(getMapUrl(fLatPan,fLonPan));
            }
            else if (bPath)
            {
                showMap(getMapPathUrl());
            }
            else if (bPhotos)
            {
                showMap(getMapPhotosUrl());
            }
            else
            {
                showMap();
            }
            
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (izoom > minzoom)
            {
                izoom--;
            }
            if (bPan)
            {
                showMap(getMapUrl(fLatPan, fLonPan));
            }
            else if (bPath)
            {
                showMap(getMapPathUrl());
            }
            else if (bPhotos)
            {
                showMap(getMapPhotosUrl());
            }
            else
            {
                showMap();
            }
            
        }


        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            
            //getAllExifInfo(listView1.SelectedItems[0].Text);
            Process.Start(listView1.SelectedItems[0].Text);
            
        }

        private void copyURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (strMapUrl != "")
            {
                Clipboard.SetText(strMapUrl);
            }
        }
        private void Drop(object state)
        {
            DragEventArgs e = state as DragEventArgs;

            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            int i;
            this.Invoke((MethodInvoker)delegate
            {
                progressBar1.Maximum = s.Length;
                progressBar1.Value = 0;
            });
            for (i = 0; i < s.Length; i++)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    progressBar2.Maximum = 5;
                });
                if (Directory.Exists(s[i]))
                {
                    DirectoryInfo di = new DirectoryInfo(s[i]);
                    if (!di.Name.ToLower().Contains("org"))
                    {
                        foreach (FileInfo fi in di.GetFiles())
                        {

                            this.Invoke((MethodInvoker)delegate
                            {
                                progressBar2.Value = 0;
                            
                                getExifInfo(fi.FullName);
                            });
                        }
                    }
                }
                else
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        progressBar2.Value = 0;
                    });
                    FileInfo fi = new FileInfo(s[i]);
                    if (!fi.Directory.Name.ToLower().Equals("org"))
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            getExifInfo(s[i]);
                        });
                        
                    }
                }
                this.Invoke((MethodInvoker)delegate
                {
                    progressBar1.Value++;
                });
            }
        }
        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            bMySqlPhotos = false;
            ThreadPool.QueueUserWorkItem(Drop, e);            
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            
            bool bT = false;
            if (e.Button == MouseButtons.Left)
            {
                int ifrom = 10;
                //this.Text = e.X + "x" + e.Y;
                /*if (e.Y < ifrom && e.X < ifrom)
                {
                    fLatPan = fLatPan + getSubPan(1);
                    fLonPan = fLonPan - getSubPan(2);
                    bT = true;
                }
                else */if (e.Y < ifrom)
                {
                    //fLatPan = fLatPan + getSubPan(1);
                    fLatPan = fLatPan + getSubPan(2);
                    bT = true;
                }
                else if (e.X < ifrom)
                {
                    fLonPan = fLonPan - getSubPan(2);
                    bT = true;
                }
                /*else if (e.X > (pictureBox2.Width - ifrom) && e.Y > (pictureBox2.Height - ifrom))
                {
                    fLatPan = fLatPan - getSubPan(1);
                    fLonPan = fLonPan + getSubPan(2);
                    bT = true;
                }*/
                else if (e.X > (pictureBox2.Width - ifrom))
                {
                    //fLonPan = fLonPan + getSubPan(1);
                    fLonPan = fLonPan + getSubPan(2);
                    bT = true;
                }
                else if (e.Y > (pictureBox2.Height - ifrom))
                {
                    fLatPan = fLatPan - getSubPan(2);
                    bT = true;
                }
                if (bT)
                {
                    txtAlt.Text = "";
                    txtLat.Text = fLatPan.ToString().PadRight(9, "0".ToCharArray()[0]);
                    txtLon.Text = fLonPan.ToString().PadRight(9, "0".ToCharArray()[0]);
                    showMap(getMapUrl(fLatPan, fLonPan));
                }
                bPan = bT;
                bPath = !bT;
                bPhotos = !bT;
            }
            
        }

        private float getSubPan(int ic)
        {
            //double dblPan = (double)((double)(izoom-2) / 4);
            //string strPan = "0," + "".PadLeft(int.Parse(Math.Round(dblPan, 0).ToString()), "0".ToCharArray()[0]) + 5;
            double dzoom = (double)(20 - izoom)-2;
            double dPan = (double)(ic==1?fPanLat:fPanLng);
            double dRes = dPan*(Math.Pow((double)2,dzoom));
            string strPan = dRes.ToString();
            //this.Text = izoom.ToString() + "_" + strPan;
            return float.Parse(strPan) ;
        }


        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.Delete && MessageBox.Show("Do you want to remove selected photos from the list?","Removed",MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                deleteSelected();
            }
            
        }

        private void deleteSelected()
        {
            bRemoving = true;
            if (il.Images.Count > 0)
            {
                for (int inum = listView1.Items.Count - 1; inum > -1; inum--)
                {
                    if (listView1.Items[inum].Selected)
                    {
                        il.Images.RemoveAt(inum);
                        listView1.Items.RemoveAt(inum);
                    }
                }
            }
            bRemoving = false;
        }



        private void zoomToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            populateZoomLevels();
        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            populateZoomLevels();
        }
        private void populateZoomLevels()
        {
            removeZoomLevels();
            for (int iz = 19; iz > 0; iz--)
            {
                ToolStripItem tsi = zoomToolStripMenuItem.DropDownItems.Add(iz.ToString());
                tsi.Click += new EventHandler(tsi_Click);
                if (iz == izoom)
                {
                    tsi.Select();
                }
            }
        }

        void tsi_Click(object sender, EventArgs e)
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            izoom = int.Parse(tsi.Text);
            if (bPan)
            {
                showMap(getMapUrl(fLatPan, fLonPan));
            }
            else if (bPath)
            {
                showMap(getMapPathUrl());
            }
            else if (bPhotos)
            {
                showMap(getMapPhotosUrl());
            }
            else
            {
                showMap();
            }
            removeZoomLevels();
        }
        private void removeZoomLevels()
        {
            zoomToolStripMenuItem.DropDownItems.Clear();
        }

        private void zoomToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            removeZoomLevels();
        }
        private void toggleSelect(ListView in_lv, bool bSelected)
        {
            bRemoving = true;
            listView1.SuspendLayout();
            foreach (ListViewItem lvi in in_lv.Items)
            {
                lvi.Selected = bSelected;
            }
            listView1.ResumeLayout();
            bRemoving = false;        
        }
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleSelect(listView1, true);
        }

        private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleSelect(listView1, false);
        }

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteSelected();
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            il.Images.Clear();
        }

        private void showOnMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bPan = false;
            bPath = false;
            bPhotos = true;
            showMap(getMapPhotosUrl());
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                bContextMenu = true;
            }
            else
            {
                bContextMenu = false;
            }
        }

  

        private void txtLat_TextChanged(object sender, EventArgs e)
        {
            if (txtLat.Text.IndexOf(" ") != -1)
            {
                string[] strLat = txtLat.Text.Split(" ".ToCharArray()[0]);
                txtLat.Text = strLat[0].Replace(".",",");
                txtLon.Text = strLat[1].Replace(".", ",");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (txtLat.Text.Length > 0 && txtLon.Text.Length > 0)
            {
                fLatPan = float.Parse(txtLat.Text.Replace(".", ","));
                fLonPan = float.Parse(txtLon.Text.Replace(".", ","));
                showMap(getMapUrl(fLatPan, fLonPan));
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = listView1.SelectedItems.Count;
            progressBar2.Maximum = 2;
            progressBar1.Value = 0;
            progressBar2.Value = 0;
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                int idx = getClosestDate(lvi.SubItems[1].Text);
                progressBar2.Value=1;
                if (idx != -1)
                {
                    ListViewItem lviC = listView2.Items[idx];
                    string strLat = lviC.SubItems[ilat].Text;
                    string strLon = lviC.SubItems[ilon].Text;
                    if (lvi.SubItems.Count < 3)
                    {
                        lvi.SubItems.Add(strLat);
                    }
                    else
                    {
                        lvi.SubItems[ilat].Text=strLat;
                    }

                    if (lvi.SubItems.Count < 4)
                    {
                        lvi.SubItems.Add(strLon);
                    }
                    else
                    {
                        lvi.SubItems[ilon].Text = strLon;
                    }
                }
                progressBar2.Value=2;
                progressBar1.Value = progressBar1.Value + 1;
            }
        }

        private int getClosestDate(string p)
        {
            int i_ret = -1;
            DateTime dtPhoto = DateTime.Parse(p);
            DateTime dtLess = DateTime.Parse("2100-01-01");
            DateTime dtMore = DateTime.Parse("2000-01-01");
            int iLess = -1;
            int iMore = -1;
            bool bMore = false;
            
            List<DateCoord> list = new List<DateCoord>();
            foreach (ListViewItem lvi in listView2.Items)
            {
                DateTime dt = DateTime.Parse(lvi.Text);
                list.Add(new DateCoord() { date = dt, idx = lvi.Index,coord=new Location(){Latitude=lvi.SubItems[ilat].Text,Longitude=lvi.SubItems[ilon].Text} });


                if (dt.CompareTo(dtPhoto) == 1 && iMore == -1)
                {
                    //dtMore = new DateTime(dt.Ticks);
                    iMore = lvi.Index;
                    iLess = iMore - 1;
                }

                long tnew = 0;
                DateTime dtnew = dt;

                if (dt.CompareTo(dtPhoto) == -1)
                {
                    tnew = -1 * dt.Ticks;
                    dtnew = dtPhoto.AddTicks(tnew);
                    dtLess = dtnew;
                }
                else if (dt.CompareTo(dtPhoto) == 1)
                {
                    tnew = -1 * dtPhoto.Ticks;
                    dtnew = dt.AddTicks(tnew);
                    if (!bMore)
                    {
                        dtMore = dtnew;
                        bMore = true;
                    }
                }
                //dtnew.ToShortDateString() + " " + dtnew.ToLongTimeString();
            }

            var itms = list.Where(x => dtPhoto.AddHours(3) <= x.date && dtPhoto.AddHours(-3) >= x.date);


            if (dtLess.Hour <= 1 && dtLess <= dtMore)
            {
                i_ret = iLess;
            }
            else if (dtMore.Hour <= 1 && dtMore <= dtLess)
            {
                i_ret = iMore;
            }
            else if (dtLess.Hour <= 2 && dtLess <= dtMore)
            {
                i_ret = iLess;
            }
            else if (dtMore.Hour <= 2 && dtMore <= dtLess)
            {
                i_ret = iMore;
            }
            else if (dtLess.Hour <= 3 && dtLess <= dtMore)
            {
                i_ret = iLess;
            }
            else if (dtMore.Hour <= 3 && dtMore <= dtLess)
            {
                i_ret = iMore;
            }
            else if (dtLess.Hour <= 4 && dtLess <= dtMore)
            {
                i_ret = iLess;
            }
            else if (dtMore.Hour <= 4 && dtMore <= dtLess)
            {
                i_ret = iMore;
            }
            return i_ret;

        }

        private void buttonTestEnc_Click(object sender, EventArgs e)
        {
            List<Coordinate> list = new List<Coordinate>();
            list.Add(new Coordinate("38,5", "-120,2"));//_p~iF~ps|U
            list.Add(new Coordinate("40,7", "-120,95"));//_ulLnnqC
            list.Add(new Coordinate("43,252", "-126,453"));//_mqNvxq`@
            string strEnco = EncodePolyline.EncodeCoordinates(list);
            MessageBox.Show(strEnco);
        }

        private void copyPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (strPath != "")
            {
                Clipboard.SetText(strPath);
            }
        }

        private void listView2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.Down)
                {
                    for (int i = listView2.SelectedItems[0].Index; i < listView2.Items.Count; i++)
                    {
                        DateTime dtStart = DateTime.Parse(listView2.SelectedItems[0].Text);
                        DateTime dtEnd = DateTime.Parse(listView2.Items[i].Text);
                        DateTime dtDiff = new DateTime(dtEnd.Ticks - dtStart.Ticks);
                        if (dtDiff.DayOfYear > 1)
                        {
                            listView2.SelectedItems.Clear();
                            ListViewItem lvi = listView2.Items[i];
                            lvi.Selected = true;
                            lvi.Focused = true;
                            lvi.EnsureVisible();
                            break;
                        }
                    }
                }
                else if (e.KeyCode == Keys.Up)
                {

                    for (int i = listView2.SelectedItems[0].Index; i > 0; i--)
                    {
                        DateTime dtStart = DateTime.Parse(listView2.SelectedItems[0].Text);
                        DateTime dtEnd = DateTime.Parse(listView2.Items[i].Text);
                        DateTime dtDiff = new DateTime(dtStart.Ticks - dtEnd.Ticks);
                        if (dtDiff.DayOfYear > 1)
                        {
                            ListViewItem lvi = listView2.Items[i];
                            lvi.Selected = true;
                            lvi.Focused = true;
                            lvi.EnsureVisible();
                            break;
                        }
                    }
                }
            }
        }

        private void deleteTaggedToolStripMenuItem_Click(object sender, EventArgs e)
        {

            bRemoving = true;
            for (int inum = listView1.Items.Count - 1; inum > -1; inum--)
            {
                if (listView1.Items[inum].SubItems[ilat].Text != "" && listView1.Items[inum].SubItems[ilon].Text != "")
                {
                    il.Images.RemoveAt(inum);
                    listView1.Items.RemoveAt(inum);
                }
            }
            bRemoving = false;
        }

        private void refreshThumbsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            il.Images.Clear();
            foreach (ListViewItem lvi in listView1.Items)
            {
                if (lvi.Tag != null)
                {
                    showThumbnails(lvi.SubItems[0].Text, lvi.Tag.ToString());
                }
                else
                {
                    showThumbnails(new FileInfo(lvi.Text));
                }
                lvi.ImageKey = lvi.SubItems[0].Text;
            }
        }
        Location searchLoc = new Location();
        SearchLocation sl = null;
        private void button8_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ht.Clear();
            ht.Add("City", new TextBox());
            ht.Add("Country", new TextBox());
            Button btnSelect = new Button();
            btnSelect.Click += new EventHandler(btnSelect_Click);
            ht.Add("Select", btnSelect);
            Button btnSearch = new Button();
            btnSearch.Click += new EventHandler(btnSearch_Click);
            ht.Add("Search", btnSearch);
            sl = new SearchLocation(ht);
            sl.StartPosition = FormStartPosition.CenterParent;
            sl.Left = this.Left + 200;
            sl.Top = this.Top + 200;
            sl.Width = 310;
            sl.Height = 150;
            DialogResult dr = sl.ShowDialog();

        }
        string strSearchText = "";
        void btnSelect_Click(object sender, EventArgs e)
        {
            txtLat.Text = searchLoc.Latitude.Replace(".", ",");
            txtLon.Text = searchLoc.Longitude.Replace(".", ",");
            txtAlt.Text = "";
            strSearchText = searchLoc.Name;

            fLatPan = float.Parse(txtLat.Text.Replace(".", ","));
            fLonPan = float.Parse(txtLon.Text.Replace(".", ","));

            showMap(getMapUrl(fLatPan, fLonPan));

            sl.Close();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Parent != null)
            {
                FlowLayoutPanel flp = (FlowLayoutPanel)btn.Parent;
                TextBox tbCity = (TextBox)getControl(flp, "txtCity");
                if (tbCity != null)
                {
                    TextBox tbCountry = (TextBox)getControl(flp, "txtCountry");
                    if (tbCountry != null)
                    {
                        if (tbCity.Text != "")
                        {
                            findLocation(tbCity.Text);
                        }
                    }
                }
            }
        }

        private void findLocation(string p)
        {
            try
            {
                Uri uri = new Uri("http://maps.google.com/maps/api/geocode/xml?address=" + p + "&sensor=false");
                WebRequest wreq = WebRequest.Create(uri);
                wreq.Method = "GET";
                WebResponse wres = wreq.GetResponse();
                Stream stream = wres.GetResponseStream();

                string strXml = "";
                StreamReader sr = new StreamReader(stream);
                strXml = sr.ReadToEnd();
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(strXml.Trim());
                XmlNode xnstat = xd.SelectSingleNode("GeocodeResponse/status");
                if (xnstat != null && xnstat.InnerText == "OK")
                {
                    XmlNode xnresname = xd.SelectSingleNode("GeocodeResponse/result/address_component/long_name");
                    if (xnresname != null)
                    {
                        XmlNode xnresloc = xd.SelectSingleNode("GeocodeResponse/result/geometry/location");
                        if (xnresloc != null)
                        {
                            string strCity = xnresname.InnerText;
                            string strLat = xnresloc.ChildNodes[0].InnerText;
                            string strLon = xnresloc.ChildNodes[1].InnerText;
                            searchLoc.Latitude = strLat;
                            searchLoc.Longitude = strLon;
                            searchLoc.Name = strCity;
                            showMap(getMapUrl(float.Parse(strLat.Replace(".", ",")), float.Parse(strLon.Replace(".", ","))));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Control getControl(FlowLayoutPanel flp, string p)
        {
            foreach (Control ctl in flp.Controls)
            {
                if (ctl.Name == p)
                {
                    return ctl;
                }
            }
            return null;
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (listBox3.SelectedItems.Count > 0)
            {
                Coordinate coord = listBox3.SelectedItem as Coordinate;
                txtAlt.Text = "";
                txtLat.Text = coord.Latitude.ToString();
                txtLon.Text = coord.Longitude.ToString();
                fLatPan = float.Parse(txtLat.Text);
                fLonPan = float.Parse(txtLon.Text);
                showMap(getMapUrl(fLatPan, fLonPan));
            }
            
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using(FolderBrowserDialog fbd = new FolderBrowserDialog())
            if (fbd.ShowDialog() == DialogResult.OK)
                txtOutput.Text = fbd.SelectedPath;
        }

        private void txtOutput_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = !string.IsNullOrEmpty(txtOutput.Text);
            btnFromMap.Enabled = !string.IsNullOrEmpty(txtOutput.Text);
        }
        bool bMySqlPhotos = false;
        private void btnMySqlPhotos_Click(object sender, EventArgs e)
        {
            bMySqlPhotos = true;
            MySqlCommand com = new MySqlCommand("select photopath from photos where (lat='' or lon='') and photopath not like '%stopm%' and photopath not like '%pano%' and photopath not like '%_fix%' and photopath not like '%alinestelefon%' order by date limit 100;", getConnection());
            MySqlDataReader mr = com.ExecuteReader();
            int icnt = 0;
            while(mr.Read())
            {
                string strPath = mr.GetString(0).Replace("/srv/samba/share/", "\\\\ubuntu\\share$\\").Replace("/", "\\");
                getExifInfo(strPath);
                icnt++;
            }
            string sdlksdlfkj = iasdifnsdfi.ToString();
            com.Connection.Close();
        }

        private void btnSavePos_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(strSearchText))
            {
                locations.Add(new Coordinate(txtLat.Text, txtLon.Text) { Altitude = double.Parse(string.IsNullOrEmpty(txtAlt.Text) ? "0" : txtAlt.Text), Name = strSearchText });
                object objVal = listBox3.SelectedValue;
                listBox3.DataSource = ReturnMe(locations);
                listBox3.ValueMember = "Name";
                listBox3.SelectedValue = strSearchText;
            }
        }

        private List<Coordinate> ReturnMe(List<Coordinate> locations)
        {
            return locations.ToList();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string strPath = Application.CommonAppDataPath + "\\locations.xml";
            File.WriteAllText(strPath, SeDes.ToXml(locations));
        }

        private void btnIgnore_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Are you sure that you want to ignore the selected photos?", "Ignore", MessageBoxButtons.YesNo))
            {
                MySqlConnection con = getConnection();
                foreach (ListViewItem lvi in listView1.SelectedItems)
                {
                    MySqlCommand com = new MySqlCommand(string.Format("update photodatabase.photos set lat='{0}',lon='{1}' where photopath='{2}'", "ignore", "ignore", lvi.Text.Replace("\\\\ubuntu\\share$\\", "/srv/samba/share/").Replace("\\", "/")), con);
                    com.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        private void btnFromMap_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ht.Clear();
            SearchLocation sl2 = new SearchLocation(ht);
            Button btnSelect = new Button();
            TextBox tbText = new TextBox();
            tbText.MaxLength = 16384;
            btnSelect.Click += (a, b) => {
                sl2.Close();
                foreach (string strall in tbText.Text.Split('|'))
                {
                    if (!string.IsNullOrEmpty(strall))
                    {
                        string[] strs = strall.Split(';');
                        if (strs.Length >= 3)
                        {
                            string strPath = strs[0].Replace("/srv/samba/share/", "\\\\ubuntu\\share$\\").Replace("/", "\\").Replace("%20", " ");
                            savePosInfo(strPath, "", strs[1], strs[2]);
                        }
                    }
                }
            };
            ht.Add("Import", btnSelect);
            ht.Add("Text", tbText);
            sl2 = new SearchLocation(ht);
            sl2.StartPosition = FormStartPosition.CenterParent;
            sl2.Left = this.Left + 200;
            sl2.Top = this.Top + 200;
            sl2.Width = 100;
            sl2.Height = 120;
            DialogResult dr = sl2.ShowDialog();
        }

        private void listView2_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (listView2.Sorting == SortOrder.Ascending)
                listView2.Sorting = SortOrder.Descending;
            else
                listView2.Sorting = SortOrder.Ascending;

            listView2.ListViewItemSorter = new ListViewItemComparer(e.Column, listView2.Sorting);
            listView2.Sort();
        }

    }
}
