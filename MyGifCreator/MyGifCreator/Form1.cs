using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGifCreator
{
    public partial class Form1 : Form
    {
        public Form1(string[] args)
        {
            InitializeComponent();
            if (args.Count() > 0)
            {
                string folder = args[0];
                if (Directory.Exists(folder))
                {
                    SelectedPath = folder;
                    button1_Click(this, EventArgs.Empty);
                }
                    
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem((a) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    button1.Enabled = false;
                    webBrowser1.Navigate("about:blank");
                });
                try
                {
                    string[] tmpfiles = Directory.EnumerateFiles(Application.CommonAppDataPath).ToArray();
                    foreach (string file in tmpfiles)
                        File.Delete(file);
                }
                catch { }
                string strPath = SelectedPath;
                var files = Directory.EnumerateFiles(strPath, "*.jpg",SearchOption.AllDirectories).Select(x => new FileInfo(x)).OrderBy(x => x.CreationTime).Select(x => x.FullName).ToArray();
                //Variable declaration
                StringCollection stringCollection = new StringCollection();
                MemoryStream memoryStream;
                BinaryWriter binaryWriter;
                Image image;
                Byte[] buf1;
                Byte[] buf2;
                Byte[] buf3;
                //Variable declaration
                string strNewFile = Application.CommonAppDataPath + "\\" + DateTime.Now.Ticks.ToString() + ".gif";
                FileStream fs = new FileStream(strNewFile, FileMode.CreateNew);

                stringCollection.AddRange(files);
                double dir = files.Count();
                dir = dir / 300;


                this.Invoke((MethodInvoker)delegate
                {
                    TimeSpan tsDiff = new TimeSpan(DateTime.Now.Ticks - (textBox1.Tag != null ? ((DateTime)textBox1.Tag).Ticks : DateTime.MinValue.Ticks));
                    if (dir > 1 && tsDiff.TotalSeconds>10)
                    {
                        textBox1.Text = int.Parse(Math.Floor(dir).ToString()).ToString();
                    }
                    progressBar1.Maximum = stringCollection.Count * 2;
                });

                memoryStream = new MemoryStream();
                buf2 = new Byte[19];
                buf3 = new Byte[8];
                buf2[0] = 33;  //extension introducer
                buf2[1] = 255; //application extension
                buf2[2] = 11;  //size of block
                buf2[3] = 78;  //N
                buf2[4] = 69;  //E
                buf2[5] = 84;  //T
                buf2[6] = 83;  //S
                buf2[7] = 67;  //C
                buf2[8] = 65;  //A
                buf2[9] = 80;  //P
                buf2[10] = 69; //E
                buf2[11] = 50; //2
                buf2[12] = 46; //.
                buf2[13] = 48; //0
                buf2[14] = 3;  //Size of block
                buf2[15] = 1;  //
                buf2[16] = 0;  //
                buf2[17] = 0;  //
                buf2[18] = 0;  //Block terminator

                buf3[0] = 33;  //Extension introducer
                buf3[1] = 249; //Graphic control extension
                buf3[2] = 4;   //Size of block
                buf3[3] = 9;   //Flags: reserved, disposal method, user input, transparent color
                buf3[4] = 3;  //Delay time low byte 10
                buf3[5] = 0;   //Delay time high byte 3
                buf3[6] = 255; //Transparent color index
                buf3[7] = 0;   //Block terminator

                binaryWriter = new BinaryWriter(fs);
                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(() => { return false; });
                int ir = 1;
                int.TryParse(textBox1.Text, out ir);
                double dall = stringCollection.Count;
                for (int picCount = 0; picCount < stringCollection.Count; picCount++)
                {
                    if (ir > 0 && picCount % ir == 0)
                    {

                        image = null;
                        System.GC.Collect();
                        image = Bitmap.FromFile(stringCollection[picCount]);


                        double dw = image.Width;
                        double dh = image.Height;

                        double dratio = (dh / dw);
                        dw = Math.Min(640, image.Width);
                        dh = dw * dratio;

                        int width = int.Parse(Math.Round(dw, 0).ToString());
                        int height = int.Parse(Math.Round(dh, 0).ToString());

                        Size size = new System.Drawing.Size(width, height);
                        image = new Bitmap(image, size);
                        //image = image.GetThumbnailImage(image.Width / 2, image.Height / 2, myCallback, IntPtr.Zero);
                        Graphics g = Graphics.FromImage(image);

                        int sweepAngle = int.Parse(Math.Floor(360d * ((picCount + 1) / dall)).ToString());
                        g.DrawArc(Pens.Orange, 10, 20, 50, 50, 0, sweepAngle);
                        g.Save();
                        
                        image.Save(memoryStream, ImageFormat.Gif);
                        buf1 = memoryStream.ToArray();

                        if (picCount == 0)
                        {
                            //only write these the first time....
                            binaryWriter.Write(buf1, 0, 781); //Header & global color table
                            binaryWriter.Write(buf2, 0, 19); //Application extension
                        }

                        binaryWriter.Write(buf3, 0, 8); //Graphic extension
                        binaryWriter.Write(buf1, 789, buf1.Length - 790); //Image data
                        /*
                        if (picCount == stringCollection.Count - 1)
                        {
                            //only write this one the last time....
                            binaryWriter.Write(";"); //Image terminator
                        }
                        */
                        memoryStream.SetLength(0);
                        System.GC.Collect();
                    }
                    this.Invoke((MethodInvoker)delegate
                    {
                        progressBar1.Value = picCount;
                    });
                }
                for (int picCount = stringCollection.Count-1; picCount >0 ; picCount--)
                {
                    if (ir > 0 && picCount % ir == 0)
                    {

                        image = null;
                        System.GC.Collect();
                        image = Bitmap.FromFile(stringCollection[picCount]);

                        double dw = image.Width;
                        double dh = image.Height;

                        double dratio = (dh / dw);
                        dw = Math.Min(1024, image.Width);
                        dh = dw * dratio;

                        int width = int.Parse(Math.Round(dw, 0).ToString());
                        int height = int.Parse(Math.Round(dh, 0).ToString());

                        Size size = new System.Drawing.Size(width, height); image = new Bitmap(image, size);
                        //image = image.GetThumbnailImage(image.Width / 2, image.Height / 2, myCallback, IntPtr.Zero);
                        Graphics g = Graphics.FromImage(image);
                        int sweepAngle = int.Parse(Math.Floor(360d * ((picCount + 1) / dall)).ToString());
                        g.DrawArc(Pens.Orange, 10, 20, 50, 50, 0, sweepAngle);
                        g.Save();
                        image.Save(memoryStream, ImageFormat.Gif);
                        buf1 = memoryStream.ToArray();

                        if (picCount == 0)
                        {
                            //only write these the first time....
                            binaryWriter.Write(buf1, 0, 781); //Header & global color table
                            binaryWriter.Write(buf2, 0, 19); //Application extension
                        }

                        binaryWriter.Write(buf3, 0, 8); //Graphic extension
                        binaryWriter.Write(buf1, 789, buf1.Length - 790); //Image data
                        /*
                        if (picCount == stringCollection.Count - 1)
                        {
                            //only write this one the last time....
                            binaryWriter.Write(";"); //Image terminator
                        }
                        */
                        memoryStream.SetLength(0);
                        System.GC.Collect();
                    }
                    this.Invoke((MethodInvoker)delegate
                    {
                        progressBar1.Value++;
                    });
                }
                this.Invoke((MethodInvoker)delegate
                {
                    progressBar1.Value = progressBar1.Maximum;
                    button1.Enabled = true;
                });
                binaryWriter.Write(";"); //Image terminator
                binaryWriter.Close();
                fs.Close();

                string strNewHtmlFile = strNewFile.Replace(".gif", ".htm");
                File.AppendAllText(strNewHtmlFile, "<html><img src=\"" + strNewFile + "\"/></html>");
                webBrowser1.Navigate(strNewHtmlFile);
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = SelectedPath;
            if (DialogResult.OK == folderBrowserDialog1.ShowDialog())
                SelectedPath = folderBrowserDialog1.SelectedPath;
        }
        string SelectedPath = @"C:\Users\jh\Dropbox\galaxy\";

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Tag = DateTime.Now;
        }
    }
}
