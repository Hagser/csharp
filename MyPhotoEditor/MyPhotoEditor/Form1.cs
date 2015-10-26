using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Drawing.Imaging;

namespace MyPhotoEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images|*.jpg";
            ofd.Multiselect = true;
            if (DialogResult.OK == ofd.ShowDialog())
            {
                LoadFiles(ofd.FileNames);
            }
        }

        private void LoadFiles(string[] files)
        {
            foreach (string file in files)
            {
                if (Directory.Exists(file))
                {
                    LoadFiles(Directory.EnumerateFiles(file,"*.jpg").ToArray());
                }
                else
                {
                    FileInfo fi = new FileInfo(file);
                    TabPage tp = new TabPage(fi.Name);

                    tp.GotFocus += (a, b) => {
                        PictureBox pb1 = (a as TabPage).PB();
                        pb1.Image = Bitmap.FromFile((pb1.Tag as PBTAG).FI.FullName);
                    };
                    tp.LostFocus += (a, b) =>
                        {
                            PictureBox pb1 = (a as TabPage).PB();
                            //pb1.Image = null;
                            System.GC.Collect();
                        };
                    PictureBox pb = new PictureBox();
                    pb.Dock = DockStyle.Fill;
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    //
                    pb.Tag = new PBTAG() { FI = fi, clipRect = new Rectangle() };
                    pb.ContextMenuStrip = contextMenuStrip1;
                    tp.Controls.Add(pb);
                    tabControl1.TabPages.Add(tp);
                }
            }
        }

        private void cWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage tp = SelectedTab;
            PictureBox pb = tp.PB();
            pb.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pb.Refresh();
            markChanged(tp);
        }

        private void cCWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage tp = SelectedTab;
            PictureBox pb = tp.PB();
            pb.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            pb.Refresh();
            markChanged(tp);
        }

        private void toolStripWidth_KeyUp(object sender, KeyEventArgs e)
        {
            bool bPercent = toolStripWidth.Text.Contains("%");
            toolStripWidth.Text = Regex.Replace(toolStripWidth.Text, "\\D", "");
            if (!string.IsNullOrEmpty(toolStripWidth.Text) && int.Parse(toolStripWidth.Text)>0)
            {
                TabPage tp = SelectedTab;
                PictureBox pb = tp.PB();
                FileInfo fi = tp.FI();
                if(uniformToolStripMenuItem.Checked)
                    toolStripHeight.Text = Math.Floor(pb.Image.Height * (float.Parse(toolStripWidth.Text) / pb.Image.Width)).ToString();
                if (int.Parse(toolStripHeight.Text) > 0)
                {
                    pb.Image = new Bitmap(Bitmap.FromFile(fi.FullName) as Image, new Size(int.Parse(toolStripWidth.Text), int.Parse(toolStripHeight.Text)));
                    markChanged(tp);
                }
            }
        }

        private void toolStripHeight_KeyUp(object sender, KeyEventArgs e)
        {
            bool bPercent = toolStripHeight.Text.Contains("%");
            toolStripHeight.Text = Regex.Replace(toolStripHeight.Text, "\\D", "");
            if (!string.IsNullOrEmpty(toolStripHeight.Text) && int.Parse(toolStripHeight.Text) > 0)
            {
                TabPage tp = SelectedTab;
                PictureBox pb = tp.PB();
                FileInfo fi = tp.FI();
                if (uniformToolStripMenuItem.Checked)
                    toolStripWidth.Text = Math.Floor(pb.Image.Width * (float.Parse(toolStripHeight.Text) / pb.Image.Height)).ToString();
                if (int.Parse(toolStripWidth.Text) > 0)
                {
                    pb.Image = new Bitmap(Bitmap.FromFile(fi.FullName) as Image, new Size(int.Parse(toolStripWidth.Text), int.Parse(toolStripHeight.Text)));
                    markChanged(tp);
                }
            }
        }

        private void resizeToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            TabPage tp = SelectedTab;
            PictureBox pb = tp.PB();
            if (pb == null)
                return;
            if(pb.Image==null)
                pb.Image = Bitmap.FromFile((pb.Tag as PBTAG).FI.FullName);
            toolStripWidth.Text = pb.Image.Width.ToString();
            toolStripHeight.Text = pb.Image.Height.ToString();
            markChanged(tp);
        }

        private void toolStripRotate_KeyUp(object sender, KeyEventArgs e)
        {
            toolStripRotate.Text = Regex.Replace(toolStripRotate.Text, "\\D", "");
            if (!string.IsNullOrEmpty(toolStripRotate.Text) && int.Parse(toolStripRotate.Text) > 0)
            {
                TabPage tp = SelectedTab;
                PictureBox pb = tp.PB();
                FileInfo fi = tp.FI();
                Image img = pb.Image;
                using (Graphics g = Graphics.FromImage(img))
                {
                    // Set world transform of graphics object to translate.
                    Point p = new Point(int.Parse(Math.Floor(float.Parse(img.Width.ToString()) / 2).ToString()), int.Parse(Math.Floor(float.Parse(img.Height.ToString()) / 2).ToString()));
                    //g.RenderingOrigin = p;
                    //g.TranslateTransform(p.X,p.Y);
                    g.TranslateTransform(p.X * -1, p.Y);
                    //g.FillRectangle(Brushes.White, 0, 0, 9000, 9000);
                    g.RotateTransform(float.Parse(toolStripRotate.Text));
                    g.DrawImage(img,0,0);
                    //g.Save();
                }
                pb.Image = img;
                pb.Refresh();
                markChanged(tp);
            }
        }

        private void toolStripWidthAll_KeyUp(object sender, KeyEventArgs e)
        {
            bool bPercent = toolStripWidthAll.Text.Contains("%");
            toolStripWidthAll.Text = Regex.Replace(toolStripWidthAll.Text, "\\D", "");
            if (!string.IsNullOrEmpty(toolStripWidthAll.Text) && int.Parse(toolStripWidthAll.Text) > 0)
            {
                foreach (TabPage tp in tabControl1.TabPages)
                {
                    PictureBox pb = tp.PB();
                    FileInfo fi = tp.FI();
                    if(uniformAllToolStripMenuItem.Checked)
                        toolStripHeightAll.Text = Math.Floor(pb.Image.Height * (float.Parse(toolStripWidthAll.Text) / pb.Image.Width)).ToString();
                    if (int.Parse(toolStripHeightAll.Text) > 0)
                    {
                        pb.Image = new Bitmap(Bitmap.FromFile(fi.FullName) as Image, new Size(int.Parse(toolStripWidthAll.Text), int.Parse(toolStripHeightAll.Text)));
                        markChanged(tp);
                    }
                }
            }
        }

        private void toolStripHeightAll_KeyUp(object sender, KeyEventArgs e)
        {
            bool bPercent = toolStripHeightAll.Text.Contains("%");
            toolStripHeightAll.Text = Regex.Replace(toolStripHeightAll.Text, "\\D", "");
            if (!string.IsNullOrEmpty(toolStripHeightAll.Text) && int.Parse(toolStripHeightAll.Text) > 0)
            {
                foreach (TabPage tp in tabControl1.TabPages)
                {
                    markChanged(tp);
                    PictureBox pb = tp.PB();
                    FileInfo fi = tp.FI();
                    if (uniformAllToolStripMenuItem.Checked)
                        toolStripWidthAll.Text = Math.Floor(pb.Image.Width * (float.Parse(toolStripHeightAll.Text) / pb.Image.Height)).ToString();
                    if (int.Parse(toolStripWidthAll.Text) > 0)
                    {
                        pb.Image = new Bitmap(Bitmap.FromFile(fi.FullName) as Image, new Size(int.Parse(toolStripWidthAll.Text), int.Parse(toolStripHeightAll.Text)));
                        markChanged(tp);
                    }
                }
            }
        }

        private void cWAlltoolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TabPage tp in tabControl1.TabPages)
            {
                PictureBox pb = tp.PB();
                FileInfo fi = tp.FI();
                Image img = Bitmap.FromFile(fi.FullName);
                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pb.Image = img;
                pb.Refresh();
                markChanged(tp);
            }
        }

        private void ccWAlltoolStripMenuItem4_Click(object sender, EventArgs e)
        {
            foreach (TabPage tp in tabControl1.TabPages)
            {
                PictureBox pb = tp.PB();
                FileInfo fi = tp.FI();
                Image img = Bitmap.FromFile(fi.FullName);
                img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                pb.Image = img;
                pb.Refresh();
                markChanged(tp);
            }
        }

        private void unmarkChanged(TabPage tp)
        {
            tp.Text = tp.Text.EndsWith("*") ? tp.Text.Substring(0, tp.Text.Length - 1) : tp.Text;
        }
        private void markChanged(TabPage tp)
        {
            tp.Text = tp.Text.EndsWith("*") ? tp.Text : tp.Text + "*";
        }

        private void toolStripRotateAll_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Save in new destination?", "Save", MessageBoxButtons.YesNoCancel);
            if (dr == DialogResult.Cancel)
                return;
            string strFolder = "";
            if (dr == DialogResult.Yes)
            {
                if (DialogResult.OK != FBD.Browse(out strFolder, true))
                    strFolder = "";
            }
            using (TabPage tp = SelectedTab)
            {
                PictureBox pb = tp.PB();
                FileInfo fi = tp.FI();
                string strFilename = strFolder != "" ? strFolder + "\\" + fi.Name : fi.FullName;
                pb.Image.Save(strFilename, ImageFormat.Jpeg);
                unmarkChanged(tp);
            }
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Save in new destination?","Save all",MessageBoxButtons.YesNoCancel);
            if (dr == DialogResult.Cancel)
                return;
            string strFolder = "";
            if (dr == DialogResult.Yes)
            {
                if (DialogResult.OK != FBD.Browse(out strFolder, true))
                    strFolder = "";
            }
            foreach (TabPage tp in tabControl1.TabPages)
            {
                PictureBox pb = tp.PB();
                FileInfo fi = tp.FI();
                string strFilename = strFolder != "" ? strFolder + "\\" + fi.Name : fi.FullName;
                pb.Image.Save(strFilename,ImageFormat.Jpeg);
                unmarkChanged(tp);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedTab.isChanged())
            {
                DialogResult dr = MessageBox.Show("Image is changed but not saved!\nDo you want to save before closing?", "Changed", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Cancel)
                    return;
                if (dr == DialogResult.Yes)
                {
                    PictureBox pb = SelectedTab.PB();
                    FileInfo fi = SelectedTab.FI();
                    pb.Image.Save(fi.FullName);
                }
            }
            tabControl1.TabPages.Remove(SelectedTab);
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(tabControl1.Any(t=>t.isChanged()))
            {
                DialogResult dr = MessageBox.Show("One or more image(s) are changed but not saved!\nDo you want to save all before closing?", "Changed", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Cancel)
                    return;
                if (dr == DialogResult.Yes)
                    saveAllToolStripMenuItem_Click(sender, e);
            }
            tabControl1.TabPages.Clear();
        }
        public TabPage SelectedTab {
            get { return tabControl1.SelectedTab; }
            set { tabControl1.SelectedTab = value; }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            //50%
            foreach (TabPage tp in tabControl1.TabPages)
            {
                ThreadPool.QueueUserWorkItem(ResizePicture, tp);
            }
        }
        private void ResizePicture(object state)
        {
            TabPage tp = state as TabPage;
                tp.Select();
            PictureBox pb = tp.PB();
            FileInfo fi = tp.FI();
            System.GC.Collect();
            if (pb.Image == null)
                pb.Image = new Bitmap(Bitmap.FromFile(fi.FullName) as Image);
            this.Invoke((MethodInvoker)delegate
            {
                System.GC.Collect();
                pb.Image = new Bitmap(Bitmap.FromFile(fi.FullName) as Image, int.Parse(Math.Floor(pb.Image.Width * .5).ToString()), int.Parse(Math.Floor(pb.Image.Height * .5).ToString()));
                pb.Refresh();
                markChanged(tp);
                System.GC.Collect();
            });

        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            //25%
            foreach (TabPage tp in tabControl1.TabPages)
            {
                PictureBox pb = tp.PB();
                FileInfo fi = tp.FI();
                System.GC.Collect();
                pb.Image = new Bitmap(Bitmap.FromFile(fi.FullName) as Image, int.Parse(Math.Floor(pb.Image.Width * .25).ToString()), int.Parse(Math.Floor(pb.Image.Height * .25).ToString()));

                pb.Refresh();
                markChanged(tp);
            }
        }

        private void tabControl1_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            LoadFiles(s);
        }

        private void tabControl1_DragOver(object sender, DragEventArgs e)
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
//            PictureBox pb1 = tabControl1.SelectedTab.PB();
//            pb1.Image = Bitmap.FromFile((pb1.Tag as FileInfo).FullName);
        }
        string SelectedPath = "";
        private void halfSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = getClipboardOrSelected();
            if (DialogResult.OK == fbd.ShowDialog())
            {
                icnt = 0;
                ThreadPool.SetMinThreads(1, 1);
                ThreadPool.SetMaxThreads(4, 4);
                SelectedPath = fbd.SelectedPath;
                var list = Directory.EnumerateFiles(SelectedPath, "*.jpg", SearchOption.AllDirectories);
                icntlist = list.Count();
                foreach (string file in list.OrderBy(x=>x))
                    ThreadPool.QueueUserWorkItem(ResizeImage,file);
            }

        }

        private string getClipboardOrSelected()
        {
            string temp = Clipboard.GetText();
            if (!string.IsNullOrEmpty(temp) && System.IO.Directory.Exists(temp))
            {
                return temp;
            }
            return SelectedPath;
        }
        int icntlist = 0;
        int icnt = 0;
        private void ResizeImage(object state)
        {
            FileInfo fi = new FileInfo(state as string);

            System.GC.Collect();
            Image img = Bitmap.FromFile(fi.FullName);
            if (img.Width > 2690)
            {
                PropertyItem[] itms = img.PropertyItems;
                System.GC.Collect();
                img = new Bitmap(Bitmap.FromFile(fi.FullName),
                int.Parse(Math.Floor(img.Width * .5).ToString()),
                int.Parse(Math.Floor(img.Height * .5).ToString()));

                foreach (PropertyItem pi in itms)
                {
                    img.SetPropertyItem(pi);
                }

                System.GC.Collect();
                string newfilename = fi.DirectoryName + "\\" + fi.Name.Replace(".JPG", "~.JPG").Replace(".jpg", "~.jpg");
                img.Save(newfilename, ImageFormat.Jpeg);
                System.GC.Collect();
                img = null;
                System.GC.Collect();
                try
                {

                    var arg = new { nf = newfilename, fi = fi };
                    fi = null;
                    Action act = new Action(delegate
                    {
                        System.IO.File.SetCreationTime(arg.nf, arg.fi.CreationTime);
                        System.IO.File.SetLastAccessTime(arg.nf, arg.fi.LastAccessTime);
                        System.IO.File.SetLastWriteTime(arg.nf, arg.fi.LastWriteTime);
                        System.IO.File.Delete(arg.fi.FullName);
                        System.IO.File.Move(arg.nf, arg.fi.FullName);
                    });
                    MoveFile(act, 0);
                }
                catch
                {

                }
            }
            else
            {
                img = null;
            }
            System.GC.Collect();
            icnt++;
            this.Invoke((MethodInvoker)delegate { this.Text = "MyPhotoEditor " + icnt + "/" + icntlist; });
            if (icntlist <= icnt)
            {
                MessageBox.Show("Jupp");
            }            
        }

        private void MoveFile(Action act,int cnt)
        {
            if (cnt > 8)
                return;
            try
            {
                act.Invoke();
            }
            catch(Exception ex)
            {                
                Thread.Sleep(5000);
                cnt++;
                MoveFile(act,cnt);
            }
        }
        private void cropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bcropAll = false;
            TabPage tp = SelectedTab;
            PictureBox pb = tp.PB();
            pb.ContextMenuStrip = null;
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            if (pb.Image == null)
                pb.Image = Bitmap.FromFile((pb.Tag as PBTAG).FI.FullName);


            pb.MouseDown += (a, b) => {
                if (b.Button == MouseButtons.Right)
                {
                    (pb.Tag as PBTAG).clipRect = new Rectangle();
                    pb.Image = Bitmap.FromFile((pb.Tag as PBTAG).FI.FullName);
                }
            };
            pb.MouseUp += (a, b) =>
            {
                Rectangle clipRect = (pb.Tag as PBTAG).clipRect;
                if (b.Button == MouseButtons.Left)
                {
                    pb.Image = Bitmap.FromFile((pb.Tag as PBTAG).FI.FullName);
                    float ratio = (float.Parse(pb.Image.Width.ToString()) / float.Parse(pb.Width.ToString()));

                    Graphics g = Graphics.FromImage(pb.Image);
                    g.ExcludeClip(clipRect);
                    Bitmap imgtr = new Bitmap(2, 2);
                    imgtr.SetPixel(0, 0, Color.Transparent);
                    imgtr.SetPixel(0, 1, Color.Silver);
                    imgtr.SetPixel(1, 0, Color.Silver);
                    imgtr.SetPixel(1, 1, Color.Transparent);
                    TextureBrush tb = new TextureBrush(imgtr);
                    g.FillRectangle(tb, new Rectangle(0, 0, pb.Image.Width, pb.Image.Height));
                    g.Save();
                    //pb.Refresh();
                }
            };
            pb.MouseMove += (a, b) => {
                Rectangle clipRect = (pb.Tag as PBTAG).clipRect;
                if (b.Button == MouseButtons.Left)
                {
                    pb.Image = Bitmap.FromFile((pb.Tag as PBTAG).FI.FullName);
                    float ratiow = (float.Parse(pb.Image.Width.ToString()) / float.Parse(pb.Width.ToString()));
                    float ratioh = (float.Parse(pb.Image.Height.ToString()) / float.Parse(pb.Height.ToString()));
                    PictureBox pbi = a as PictureBox;
                    if (clipRect.Location.IsEmpty)
                    {
                        clipRect.Location = new Point(
                            int.Parse(Math.Floor(((b.X - pbi.Left) * ratiow)).ToString()),
                            int.Parse(Math.Floor(((b.Y - pbi.Top) * ratioh)).ToString())
                            );
                    }
                    clipRect.Size = new Size(
                        int.Parse(Math.Floor(((b.X-pbi.Left) * ratiow - clipRect.Location.X)).ToString()),
                        int.Parse(Math.Floor(((b.Y-pbi.Top) * ratioh - clipRect.Location.Y)).ToString())
                        );
                    
                    Graphics g = Graphics.FromImage(pb.Image);

                    g.ExcludeClip(clipRect);
                    Bitmap imgtr = new Bitmap(2,2);
                    imgtr.SetPixel(0,0,Color.Transparent);
                    imgtr.SetPixel(0,1,Color.Silver);
                    imgtr.SetPixel(1,0,Color.Silver);
                    imgtr.SetPixel(1,1,Color.Transparent);
                    TextureBrush tb = new TextureBrush(imgtr);
                    g.FillRectangle(tb, new Rectangle(0, 0, pb.Image.Width, pb.Image.Height));
                    g.Save();
                   
                    pb.Refresh();

                    (pb.Tag as PBTAG).clipRect = clipRect;
                }
            };
        }
        Rectangle cropAllRect = new Rectangle();
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (bcropAll)
                {
                    TabPage tp = SelectedTab;
                    PictureBox pb = tp.PB();
                    Bitmap src = new Bitmap(pb.Image);
                    cropAllRect = pb.CR();
                    tabControl1.TabPages.Remove(SelectedTab);
                    icntlist = cropAllFiles.Count;
                    icnt = 0;
                    foreach (var file in cropAllFiles)
                        ThreadPool.QueueUserWorkItem(CropAll,file);
                }
                else
                {
                    TabPage tp = SelectedTab;
                    PictureBox pb = tp.PB();
                    Bitmap src = new Bitmap(pb.Image);
                    Rectangle cropRect = pb.CR();
                    Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                    using (Graphics g = Graphics.FromImage(target))
                    {
                        g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
                        g.Save();
                    }
                    pb.Image = target;
                    pb.Refresh();
                }
            }
        }

        private void CropAll(object state)
        {
            string file = state.ToString();
            if (File.Exists(file))
            {
                FileInfo fi = new FileInfo(file);
                Bitmap src = Bitmap.FromFile(file) as Bitmap;
                Rectangle cropRect = cropAllRect;
                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
                    g.Save();
                }
                System.GC.Collect();
                string newfilename = fi.DirectoryName + "\\" + fi.Name.Replace(".JPG", "~.JPG").Replace(".jpg", "~.jpg");
                target.Save(newfilename, ImageFormat.Jpeg);
                System.GC.Collect();
                target = null;
                src = null;
                System.GC.Collect();
                try
                {

                    var arg = new { nf = newfilename, fi = fi };
                    fi = null;
                    Action act = new Action(delegate
                    {
                        System.IO.File.SetCreationTime(arg.nf, arg.fi.CreationTime);
                        System.IO.File.SetLastAccessTime(arg.nf, arg.fi.LastAccessTime);
                        System.IO.File.SetLastWriteTime(arg.nf, arg.fi.LastWriteTime);
                        System.IO.File.Delete(arg.fi.FullName);
                        System.IO.File.Move(arg.nf, arg.fi.FullName);
                    });
                    MoveFile(act, 0);
                }
                catch
                {

                }
                icnt++;
                this.Invoke((MethodInvoker)delegate { this.Text = "MyPhotoEditor " + icnt + "/" + icntlist; });
                if (icntlist <= icnt)
                {
                    MessageBox.Show("Jupp");
                }            

            }
        }

        bool bcropAll = false;
        List<string> cropAllFiles = new List<string>();
        private void cropAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = getClipboardOrSelected();
            if (DialogResult.OK == fbd.ShowDialog())
            {
                icnt = 0;
                ThreadPool.SetMinThreads(1, 1);
                ThreadPool.SetMaxThreads(4, 4);
                SelectedPath = fbd.SelectedPath;
                cropAllFiles = Directory.EnumerateFiles(SelectedPath, "*.jpg", SearchOption.AllDirectories).ToList();
                icntlist = cropAllFiles.Count();
                LoadFiles(cropAllFiles.Take(1).ToArray());

                bcropAll = true;

                TabPage tp = SelectedTab;
                PictureBox pb = tp.PB();
                pb.ContextMenuStrip = null;
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                if (pb.Image == null)
                    pb.Image = Bitmap.FromFile((pb.Tag as PBTAG).FI.FullName);


                pb.MouseDown += (a, b) =>
                {
                    if (b.Button == MouseButtons.Right)
                    {
                        (pb.Tag as PBTAG).clipRect = new Rectangle();
                        pb.Image = Bitmap.FromFile((pb.Tag as PBTAG).FI.FullName);
                    }
                };
                pb.MouseUp += (a, b) =>
                {
                    Rectangle clipRect = (pb.Tag as PBTAG).clipRect;
                    if (b.Button == MouseButtons.Left)
                    {
                        pb.Image = Bitmap.FromFile((pb.Tag as PBTAG).FI.FullName);
                        float ratio = (float.Parse(pb.Image.Width.ToString()) / float.Parse(pb.Width.ToString()));

                        Graphics g = Graphics.FromImage(pb.Image);
                        g.ExcludeClip(clipRect);
                        Bitmap imgtr = new Bitmap(2, 2);
                        imgtr.SetPixel(0, 0, Color.Transparent);
                        imgtr.SetPixel(0, 1, Color.Silver);
                        imgtr.SetPixel(1, 0, Color.Silver);
                        imgtr.SetPixel(1, 1, Color.Transparent);
                        TextureBrush tb = new TextureBrush(imgtr);
                        g.FillRectangle(tb, new Rectangle(0, 0, pb.Image.Width, pb.Image.Height));
                        g.Save();

                        //foreach (string file in list.OrderBy(x => x))
                        //ThreadPool.QueueUserWorkItem(ResizeImage, file);
                    }
                };
                pb.MouseMove += (a, b) =>
                {
                    Rectangle clipRect = (pb.Tag as PBTAG).clipRect;
                    if (b.Button == MouseButtons.Left)
                    {
                        pb.Image = Bitmap.FromFile((pb.Tag as PBTAG).FI.FullName);
                        float ratiow = (float.Parse(pb.Image.Width.ToString()) / float.Parse(pb.Width.ToString()));
                        float ratioh = (float.Parse(pb.Image.Height.ToString()) / float.Parse(pb.Height.ToString()));
                        PictureBox pbi = a as PictureBox;
                        if (clipRect.Location.IsEmpty)
                        {
                            clipRect.Location = new Point(
                                int.Parse(Math.Floor(((b.X - pbi.Left) * ratiow)).ToString()),
                                int.Parse(Math.Floor(((b.Y - pbi.Top) * ratioh)).ToString())
                                );
                        }
                        clipRect.Size = new Size(
                            int.Parse(Math.Floor(((b.X - pbi.Left) * ratiow - clipRect.Location.X)).ToString()),
                            int.Parse(Math.Floor(((b.Y - pbi.Top) * ratioh - clipRect.Location.Y)).ToString())
                            );

                        Graphics g = Graphics.FromImage(pb.Image);

                        g.ExcludeClip(clipRect);
                        Bitmap imgtr = new Bitmap(2, 2);
                        imgtr.SetPixel(0, 0, Color.Transparent);
                        imgtr.SetPixel(0, 1, Color.Silver);
                        imgtr.SetPixel(1, 0, Color.Silver);
                        imgtr.SetPixel(1, 1, Color.Transparent);
                        TextureBrush tb = new TextureBrush(imgtr);
                        g.FillRectangle(tb, new Rectangle(0, 0, pb.Image.Width, pb.Image.Height));
                        g.Save();

                        pb.Refresh();

                        (pb.Tag as PBTAG).clipRect = clipRect;
                    }
                };
            }
        }

    }
    public class PBTAG
    {
        public FileInfo FI { get; set; }
        public Rectangle clipRect { get; set; }
    }
    public static class TabControlExt
    {
        public static bool Any(this TabControl t, Func<TabPage, bool> func)
        {
            return t.Where(func).Count > 0;
        }
        public static List<TabPage> Where(this TabControl t, Func<TabPage,bool> func)
        {
            List<TabPage> list = new List<TabPage>();
            foreach (TabPage tp in t.TabPages)
            {
                list.Add(tp);
            }
            return list.Where(func).ToList();
        }
    }
    public static class PictureBoxExt
    {
        public static FileInfo FI(this PictureBox pb)
        {
            return (pb.Tag as PBTAG).FI;
        }
        public static Rectangle CR(this PictureBox pb)
        {
            return (pb.Tag as PBTAG).clipRect;
        }
    }
    public static class TabPageExt
    {
        public static bool isChanged(this TabPage tp)
        {
            return tp.Text.EndsWith("*");
        }
        public static PictureBox PB(this TabPage tp)
        {
            return tp!=null && tp.Controls.Count > 0 ? tp.Controls[0] as PictureBox : null;
        }
        public static FileInfo FI(this TabPage tp)
        {
            return (tp.PB().Tag as PBTAG).FI;
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
