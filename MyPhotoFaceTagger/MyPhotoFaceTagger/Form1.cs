using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Threading;


namespace MyPhotoFaceTagger
{
    public partial class Form1 : Form
    {
        HaarCascade face;
        //Dictionary<string,Image<Gray, byte>> trainingImages = new Dictionary<string,Image<Gray, byte>>();
        //List<string> labels = new List<string>();
        List<FileInfo> foundImages = new List<FileInfo>();
        List<trainedFace> trainedFaces = new List<trainedFace>();
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.8d, 0.8d);

        public Form1()
        {
            InitializeComponent();
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            this.MouseWheel+=(a,b)=>{
                if(textBox1.Focused)
                {
                    double d = double.Parse(textBox1.Text);
                    d += b.Delta>0?0.01:-0.01;
                    textBox1.Text = d.ToString();
                }
                if (textBox2.Focused)
                {
                    int d = int.Parse(textBox2.Text);
                    d += b.Delta > 0 ? 1 : -1;
                    textBox2.Text = d.ToString();
                }
                if (textBox3.Focused)
                {
                    int d = int.Parse(textBox3.Text);
                    d += b.Delta > 0 ? 10 : -10;
                    textBox3.Text = d.ToString();
                }
            };
        }
        bool bFind {
            get { return Cancel.Enabled; }
            set { Cancel.Enabled = value; findfaces.Enabled = !value; }
        }
        private void findfaces_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;

            bFind = true;
            foundImages.Clear();
            foreach (string fil in Directory.GetFiles(curDir.Text, "*.jpg",SearchOption.AllDirectories))
            {
                if (!bFind)
                    break;
                FileInfo fi = new FileInfo(fil);
                if (!fi.Directory.Name.ToLower().Equals("org") && (!hideTrained.Checked || (hideTrained.Checked && !trainedFaces.Any(x => x.path.Equals(fil)))))
                    foundImages.Add(fi);

            }
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = foundImages;
            Action a = new Action(()=>{
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    try
                    {
                        if (!bFind)
                            break;
                        string sfil = row.Cells[6].Value.ToString();
                        Image<Bgr, byte> img = new Image<Bgr, byte>(sfil).Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC); ;
                        if (!hasFaces(img))
                        {
                            if (hideNoFacedImages.Checked)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    try
                                    {
                                        row.Visible = false;
                                    }
                                    catch (Exception ex) { errorLabel.Text = ex.Message + "\n" + ex.StackTrace; }
                                });
                            }
                            else
                            {
                                row.Cells[0].ErrorText = "No faces";
                            }
                        }
                    }
                    catch (Exception ex) { 
                        
                        this.Invoke((MethodInvoker)delegate
                        {
                            errorLabel.Text = ex.Message + "\n" + ex.StackTrace;
                        });
                    }
                    GC.Collect();
                }
                this.Invoke((MethodInvoker)delegate
                {
                    progressBar1.Visible = false; bFind = false;
                });
            });
            ThreadPool.QueueUserWorkItem(new WaitCallback(checkFace), a);
        }
        private void checkFace(object o)
        {
            (o as Action).Invoke();
        }
        private void browse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = curDir.Text;
            if (DialogResult.OK == folderBrowserDialog1.ShowDialog())
            {
                curDir.Text = folderBrowserDialog1.SelectedPath;   
            }
        }

        private void curDir_TextChanged(object sender, EventArgs e)
        {
            findfaces.Enabled = !string.IsNullOrEmpty(curDir.Text) && Directory.Exists(curDir.Text);
        }
        string fil = "",name="";
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (bFind)
                return;
            if (dataGridView1.SelectedCells.Count <= 0)
                return;

            fil = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[6].Value != null ? dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[6].Value.ToString() : "";
            if (!File.Exists(fil))
                return;
            try
            {
                progressBar1.Visible = true;

                Image<Bgr, byte> img = new Image<Bgr, byte>(fil).Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC); ;
                
                if (!trainedFaces.Any(x => x.path.Equals(fil)))
                {
                    img=findFaces(img);
                    img.Draw("not trained", ref font, new Point(20, 20), new Bgr(Color.Purple));
                }
                else
                {
                    foreach (trainedFace tf in trainedFaces.Where(x => x.path.Equals(fil)))
                    {
                        img.Draw(tf.rect, new Bgr(Color.Red), 2);
                        img.Draw(tf.name, ref font, new Point(tf.rect.X - 2, tf.rect.Y - 2), new Bgr(Color.LightGreen));
                    }
                }
                GC.Collect();
                pictureBox1.Image = img.Bitmap;
                pictureBox1.Refresh();
            }
            catch (Exception ex) { errorLabel.Text = ex.Message + "\n" + ex.StackTrace; }
            finally
            {
                progressBar1.Visible = false;
            }
        }
        private bool hasFaces(Image<Bgr, byte> img)
        {
            //Convert it to Grayscale
            Image<Gray, byte> gray = img.Convert<Gray, Byte>();

            //Equalization step
            gray._EqualizeHist();
            try
            {
                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                        face,
                        scaleFactor, minNeighbors,
                    //Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.FIND_BIGGEST_OBJECT,
                        HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                        new Size(minsize,minsize));
                GC.Collect();
                return facesDetected.Length > 0 && facesDetected[0].Length>0;
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message + "\n" + ex.StackTrace;
                GC.Collect();
            }
            return false;
        }

        private Image<Bgr, byte> findFaces(Image<Bgr, byte> img)
        {
            name = "";
            //Convert it to Grayscale
            Image<Gray, byte> gray = img.Convert<Gray, Byte>();

            //Equalization step
            gray._EqualizeHist();
            try
            {
                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                        face,
                        scaleFactor,minNeighbors,
                        //Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.FIND_BIGGEST_OBJECT,
                        HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                        new Size(minsize, minsize));

                Image<Gray, byte> result;

                foreach (Control ct in flowLayoutPanel1.Controls)
                {
                    (ct as PictureBox).Image = null;
                    ct.Dispose();
                }
                flowLayoutPanel1.Controls.Clear();

                int ContTrain = 0;
                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    //result = gray.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    result = gray.Convert<Gray, byte>().Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);


                    //draw the face detected in the 0th (gray) channel with blue color
                    img.Draw(f.rect, new Bgr(Color.Red), 2);

                    if (trainedFaces.Count != 0 && !skipname.Checked)
                    {
                        ContTrain = trainedFaces.Count;
                        //TermCriteria for face recognition with numbers of trained images like maxIteration
                        MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain*2, 0.001);
                        try
                        {
                            //Eigen face recognizer
                            EigenObjectRecognizer recognizer = new EigenObjectRecognizer(trainedFaces.OrderBy(x => x.path).Select(x => x.img).ToArray(), trainedFaces.OrderBy(x => x.path).Select(x => x.name).ToList<string>().ToArray(), 4000, ref termCrit);

                            name = recognizer.Recognize(result);

                            //Draw the label for each face detected and recognized
                            img.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));
                        }
                        catch (Exception ex) { errorLabel.Text = ex.Message + "\n" + ex.StackTrace; }

                    }
                    addToFlow(img, f, name);
                }
            }
            catch (Exception ex) { errorLabel.Text = ex.Message + "\n" + ex.StackTrace; }
            GC.Collect();
            return img;
        }

        private Image<Bgr, byte> findFacesDebug(Image<Bgr, byte> img)
        {
            name = "";
            //Convert it to Grayscale
            Image<Gray, byte> gray = img.Convert<Gray, Byte>();

            //Equalization step
            gray._EqualizeHist();
            try
            {
                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                        face,scaleFactor,minNeighbors,
                    //Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.FIND_BIGGEST_OBJECT,
                        HAAR_DETECTION_TYPE.DEFAULT,
                        new Size(minsize, minsize));

                Image<Gray, byte> result;

                foreach (Control ct in flowLayoutPanel1.Controls)
                {
                    (ct as PictureBox).Image = null;
                    ct.Dispose();
                }
                flowLayoutPanel1.Controls.Clear();

                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    //result = gray.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    result = gray.Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    //draw the face detected in the 0th (gray) channel with blue color
                    img.Draw(f.rect, new Bgr(Color.Red), 2);
                    addToFlow(img, f, "");
                }

            }
            catch (Exception ex) { errorLabel.Text = ex.Message + "\n" + ex.StackTrace; }
            GC.Collect();
            return img;
        }
        private void addToFlow(Image<Bgr, byte> result, MCvAvgComp f, string name)
        {
            PictureBox pbox = new PictureBox();
            pbox.Tag = new object[] { new int[] { f.rect.X, f.rect.Y,f.rect.Width, f.rect.Height}, name };
            pbox.Click += new EventHandler(pbox_Click);
            pbox.ContextMenuStrip = contextMenuStrip1;
            pbox.SizeMode = PictureBoxSizeMode.StretchImage;
            pbox.Height = 50;
            pbox.Width = 50;
            pbox.Cursor = Cursors.Hand;
            pbox.Image = result.Copy(f.rect).Bitmap;
            flowLayoutPanel1.Controls.Add(pbox);
        }

        void pbox_Click(object sender, EventArgs e)
        {
            
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            PictureBox pb = (tsmi.Owner as  ContextMenuStrip).SourceControl as PictureBox;
            if (pb != null && pb.Tag != null)
            {
                object[] o = pb.Tag as object[];
                var rect = o[0] as int[];
                Rectangle r = new Rectangle(rect[0], rect[1], rect[2], rect[3]);
                var name = o[1] as string;
                addTrainedFace(fil, (sender as ToolStripMenuItem).Text, r);
            }
            else
            {
                addTrainedFace((sender as ToolStripMenuItem).Text);
                selectNextRow();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(name))
            {
                addTrainedFace(name);
                selectNextRow();
            }
        }

        private void selectNextRow()
        {
            if (dataGridView1.Rows.Count > dataGridView1.SelectedCells[0].RowIndex)
            {
                int ip = dataGridView1.SelectedCells[0].RowIndex + 1;
                if (dataGridView1.Rows.Count > ip+1)
                {
                    while (!dataGridView1.Rows[ip].Visible)
                    {
                        ip++;
                        if (dataGridView1.Rows.Count < ip + 1)
                            break;
                    }
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        row.Selected = false;
                    }
                    dataGridView1.Rows[ip].Selected = true;
                }
            }
        }

        private void addTrainedFace(string fil,string name,Rectangle rect)
        {
            if (pictureBox1.Image != null)
            {
                Image<Gray, byte> img = new Image<Gray, byte>(fil);
                img = img.Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                
                if (!trainedFaces.Any(x => x.path.Equals(fil,StringComparison.InvariantCultureIgnoreCase) && x.rect.X==rect.X&&x.rect.Y==rect.Y))
                {
                    trainedFaces.Add(new trainedFace() { img = img, path = fil, name = name, rect = rect });

                    trySave();
                }
                GC.Collect();
            }
        } 

        private void addTrainedFace(string name)
        {
            if (pictureBox1.Image != null)
            {
                Image<Gray, byte> img = new Image<Gray, byte>(fil);
                img = img.Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                if (!trainedFaces.Any(x => x.path.Equals(fil)))
                {
                    trainedFaces.Add(new trainedFace() { path = fil, img = img, name = name });

                    trySave();
                }
                GC.Collect();
            }
        }

        string[] names = new[] { "Aline", "Unn", "Yrsa","Ulv", "Johan", "Barbro", "Göran", "Martti", "Marita", "Pamina", "Mathias", "Frida", "Hampus", "Kasper", "Thea" };

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string name in names)
            {
                ToolStripMenuItem tsi = new ToolStripMenuItem(name);
                tsi.Click += ToolStripMenuItem_Click;
                contextMenuStrip1.Items.Add(tsi);
            }

            if (!File.Exists(strDownloadPath + "\\trainedImages.xml"))
                return;
            string xml = File.ReadAllText(strDownloadPath + "\\trainedImages.xml");
            trainedFaces = (SeDes.ToObj(xml, trainedFaces) as List<trainedFace>);
            ThreadStart ts = new ThreadStart(fixFaces);
            progressBar1.Visible = true;

            IAsyncResult iar = ts.BeginInvoke((AsyncCallback) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    progressBar1.Visible = false;
                });
            }, null);
            

        }
        private void fixFaces()
        {
            foreach(var tf in  trainedFaces)
                try { tf.img = new Image<Gray, byte>(tf.path).Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC); }
                catch { }
           // trainedFaces.ForEach(x => x.img = new Image<Gray, byte>(x.path).Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC));
            GC.Collect();
        }
        string strDownloadPath = Application.UserAppDataPath;

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            trainedFaces.ForEach(x => x.img = null);
            GC.Collect();
            string xmlser = SeDes.ToXml(trainedFaces);
            File.WriteAllText(strDownloadPath + "\\trainedImages.xml", xmlser);
        }


        public double scaleFactor { get { return string.IsNullOrEmpty(textBox1.Text) ? 1.35 : double.Parse(textBox1.Text); } set { textBox1.Text = value.ToString(); } }

        public int minNeighbors { get { return string.IsNullOrEmpty(textBox2.Text) ? 3 : int.Parse(textBox2.Text); } set { textBox2.Text = value.ToString(); } }

        public int minsize { get { return string.IsNullOrEmpty(textBox3.Text) ? 50 : int.Parse(textBox3.Text); } set { textBox3.Text = value.ToString(); } }

        private void debugHaar()
        {
            if (!File.Exists(fil))
                return;
            progressBar1.Visible = true;
            Image<Bgr, byte> img = new Image<Bgr, byte>(fil).Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC); ;

            img = findFacesDebug(img);

            GC.Collect();
            pictureBox1.Image = img.Bitmap;
            pictureBox1.Refresh();
            progressBar1.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            double ddlkjsdn=double.Parse(textBox1.Text);
            if (ddlkjsdn<=1)
            {
                textBox1.Text = "1,1";
            }
            debugHaar();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            debugHaar();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            debugHaar();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            trySave();
        }

        private void trySave()
        {
            try
            {
                List<trainedFace> temptf = new List<trainedFace>();
                foreach (trainedFace tf in trainedFaces)
                {
                    temptf.Add(new trainedFace() { name = tf.name, path = tf.path, rect = tf.rect });
                }

                string xmlser = SeDes.ToXml(temptf);
                File.WriteAllText(strDownloadPath + "\\trainedImages.xml", xmlser);
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message + "\n" + ex.StackTrace;
            }
        }

        private void hideNoFacedImages_CheckedChanged(object sender, EventArgs e)
        {
            if (hideNoFacedImages.Checked)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    try
                    {
                        if (!bFind)
                            break;
                        string sfil = row.Cells[6].Value.ToString();
                        Image<Bgr, byte> img = new Image<Bgr, byte>(sfil).Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC); ;
                        if (!hasFaces(img))
                        {
                            if (hideNoFacedImages.Checked)
                            {
                                row.Visible = false;
                            }
                            else
                            {
                                row.Cells[0].ErrorText = "No faces";
                            }
                        }
                    }
                    catch (Exception ex) { errorLabel.Text = ex.Message + "\n" + ex.StackTrace; }
                    GC.Collect();
                }
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.Visible)
                        row.Visible = true;
                }
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            bFind = false;
        }
    }
}
