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

namespace MyHalfSize
{
    public partial class Form1 : Form
    {
        public Form1(string[] args)
        {
            InitializeComponent();
            if (args != null && args.Length > 0)
            {
                DialogResult dr = MessageBox.Show("Save in new destination?", "Save all", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Cancel)
                    return;

                if (dr == DialogResult.Yes)
                {
                    if (DialogResult.OK != FBD.Browse(out strFolder, true))
                        strFolder = "";
                }
                ThreadPool.QueueUserWorkItem(LoadFiles, args);
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            DialogResult dr = MessageBox.Show("Save in new destination?", "Save all", MessageBoxButtons.YesNoCancel);
            if (dr == DialogResult.Cancel)
                return;

            if (dr == DialogResult.Yes)
            {
                if (DialogResult.OK != FBD.Browse(out strFolder, true))
                    strFolder = "";
            }
            ThreadPool.QueueUserWorkItem(LoadFiles, s);
        }
        private void LoadFiles(object state)
        {
            UseWaitCursor = true;
            LoadFiles(state as string[]);
            UseWaitCursor = false;
        }
        string strFolder = "";
        int idone = 0;
        private void LoadFiles(string[] files)
        {
            foreach (string file in files)
            {
                if (Directory.Exists(file))
                {
                    LoadFiles(Directory.EnumerateFiles(file).ToArray());
                }
                else
                {
                    try
                    {
                        FileInfo fi = new FileInfo(file);
                        if (fi.Extension.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase))
                        {
                            DateTime dtlw = fi.LastWriteTime;
                            DateTime dtct = fi.CreationTime;
                            using (Image img = Bitmap.FromFile(file))
                            {
                                System.GC.Collect();
                                using (Image Image = new Bitmap(Bitmap.FromFile(fi.FullName) as Image, int.Parse(Math.Floor(img.Width * .5).ToString()), int.Parse(Math.Floor(img.Height * .5).ToString())))
                                {
                                    string strFilename = strFolder != "" ? strFolder + "\\" + fi.Name : fi.FullName;
                                    foreach (PropertyItem pi in img.PropertyItems)
                                    {
                                        Image.SetPropertyItem(pi);
                                    }
                                    Image.Save(strFilename, ImageFormat.Jpeg);
                                    FileInfo finew = new FileInfo(strFilename);
                                    finew.LastWriteTime = dtlw;
                                    finew.CreationTime = dtct;
                                    idone++;
                                    this.Invoke((MethodInvoker)delegate { label1.Text = idone.ToString(); });
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
        }
        private void Form1_DragEnter(object sender, DragEventArgs e)
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

        private void label1_DragDrop(object sender, DragEventArgs e)
        {
            Form1_DragDrop(sender, e);
        }

        private void label1_DragEnter(object sender, DragEventArgs e)
        {
            Form1_DragEnter(sender, e);
        }
    }
    public static class FBD
    {
        public static DialogResult Browse(out string SelectedPath)
        {
            return Browse(out SelectedPath, false);
        }
        public static DialogResult Browse(out string SelectedPath, bool ShowNewFolderButton)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = ShowNewFolderButton;
            DialogResult dr = fbd.ShowDialog();
            SelectedPath = fbd.SelectedPath;
            return dr;
        }
    }
}
