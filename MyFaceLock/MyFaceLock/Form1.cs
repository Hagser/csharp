using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using MySeDes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyFaceLock
{
    public partial class Form1 : Form
    {
        HaarCascade face;
        Capture _capture;
        List<trainedFace> trainedFaces = new List<trainedFace>();
        Bitmap trainedimg = null;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.8d, 0.8d);
        public Form1()
        {
            InitializeComponent();
            face = new HaarCascade("haarcascade_frontalface_default.xml");
        }
        DateTime dtWhenFound = DateTime.MinValue;
        private void timer1_Tick(object sender, EventArgs e)
        {
            Image<Bgr, Byte> frame = _capture.QueryFrame();
            bool bface = hasFaces(frame);
            if (bface)
            {
                this.Opacity = 0;
                dtWhenFound = DateTime.MinValue;
                //findFaces(frame);
            }
            else
            {
                if (dtWhenFound == DateTime.MinValue)
                    dtWhenFound = DateTime.Now;
                this.Opacity = 1;
                this.WindowState = FormWindowState.Maximized;
                TimeSpan tsDiff = new TimeSpan(DateTime.Now.Ticks - dtWhenFound.Ticks);
                if (tsDiff.TotalSeconds > 3)
                {
                    //closeFire();
                }
            }
            this.Text = "Hasface:" + bface;
            if (trainedimg != null)
            {
                EyeOpen.Imaging.ComparableImage ci = new EyeOpen.Imaging.ComparableImage(trainedimg);
                double dblSim = ci.CalculateSimilarity(new EyeOpen.Imaging.ComparableImage(frame.Bitmap));
                this.Text += ", Similarity:" + dblSim;
            }

            //pictureBox1.Image = frame.Bitmap;
        }
        MyDllImport.user32.CallBack cb;
        bool cbb(IntPtr val1, IntPtr val2)
        {
            var info = MyDllImport.user32.getWindowInfo(val1);
            
            return true;
        }
        private void closeFire()
        {
            this.TopMost = false;
            this.WindowState = FormWindowState.Normal;
            cb = new MyDllImport.user32.CallBack(cbb);
            MyDllImport.user32.getWindows(cb, IntPtr.Zero);
            //
                //MyDllImport.user32.setWindowPos(id, IntPtr.Zero, 0, 0, 0, 0, MyDllImport.user32.SetWindowPosFlags.HideWindow);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            _capture = new Capture(1);

            _capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FPS, 30);
            _capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 1024);
            _capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 768);

            timer1.Enabled = true;
            if (File.Exists(strDownloadPath + "\\trainedImages.xml"))
                trainedFaces = SeDes.LoadFromXml(strDownloadPath + "\\trainedImages.xml", trainedFaces) as List<trainedFace>;

            ThreadPool.QueueUserWorkItem(fixFaces);
        }

        private void fixFaces(object state)
        {
            trainedFaces.Where(x=>x.path!=null).ToList().ForEach(x => x.img = new Image<Bgr, byte>(x.path));
            GC.Collect();
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
                        new Size(minsize, minsize));
                GC.Collect();
                return facesDetected.Length > 0 && facesDetected[0].Length > 0;
            }
            catch (Exception ex)
            {
                this.Text = ex.Message + "\n" + ex.StackTrace;
                GC.Collect();
            }
            return false;
        }

        public double scaleFactor = 1.35; 

        public int minNeighbors = 3; 

        public int minsize= 100; 

        private Image<Bgr, byte> findFaces(Image<Bgr, byte> img)
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
                        new Size(minsize, minsize));

                Image<Bgr, byte> result;


                int ContTrain = 0;
                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    result = img.Copy(f.rect);
                        //gray.Copy(f.rect).Convert<Gray, byte>().Resize(f.rect.Width, f.rect.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    //result = gray.Convert<Gray, byte>().Resize(f.rect.Width, f.rect.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                    //draw the face detected in the 0th (gray) channel with blue color
                    img.Draw(f.rect, new Bgr(Color.Red), 2);
                    if(addFacesToolStripMenuItem.Checked)
                        addTrainedFace(result, f.rect);

                    if (trainedFaces.Count != 0 && matchFaceToolStripMenuItem.Checked)
                    {
                        ContTrain = trainedFaces.Count;
                        //TermCriteria for face recognition with numbers of trained images like maxIteration
                        MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain * 2, 0.001);
                        try
                        {
                            //Eigen face recognizer
                            EigenObjectRecognizer recognizer = new EigenObjectRecognizer(trainedFaces.OrderBy(x => x.path).Select(x => x.img.Convert<Gray, Byte>().Resize(200, 200, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC)).ToArray(), trainedFaces.OrderBy(x => x.path).Select(x => x.name).ToList<string>().ToArray(), 4000, ref termCrit);

                            this.Text = recognizer.Recognize(result.Convert<Gray, Byte>().Resize(200, 200, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC));

                            //Draw the label for each face detected and recognized
                            img.Draw(this.Text, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));
                        }
                        catch (Exception ex) { this.Text = ex.Message + "\n" + ex.StackTrace; }

                    }
                }
            }
            catch (Exception ex) { this.Text = ex.Message + "\n" + ex.StackTrace; }
            GC.Collect();
            return img;
        }
        bool? bTrainedFace;

        private void addTrainedFace(Image<Bgr, byte> img, Rectangle rect)
        {
            if (pictureBox1.Image != null)
            {
                //img = img.Convert<Gray, byte>().Resize(200, 200, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                string path = strDownloadPath + string.Format("\\{0}.jpg", System.DateTime.Now.Ticks);

                if (!trainedFaces.Any(x => x.rect.X == rect.X && x.rect.Y == rect.Y))
                {
                    img.Save(path);
                    trainedFaces.Add(new trainedFace() { path = path, img = img, rect = rect, name = trainedFaces.Count + "" });
                    //this.Text = "Added:" + trainedFaces.Count;
                    trySave();
                }
                GC.Collect();
            }
        }

        string strDownloadPath = Application.UserAppDataPath;
        private void trySave()
        {
            try
            {
                SeDes.SaveToXml(strDownloadPath + "\\trainedImages.xml", trainedFaces);
            }
            catch (Exception ex)
            {
                this.Text = ex.Message + "\n" + ex.StackTrace;
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            trainedFaces.ForEach(x => x.img = null);
            trySave();
        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(strDownloadPath);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Application.Exit();
        }
    }
}
