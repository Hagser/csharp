using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyQRGen;
using System.Text.RegularExpressions;
namespace MyQRGen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Random rnds = new Random(System.DateTime.Now.Millisecond);
        private Bitmap GenerateQRCode(string URL, System.Drawing.Color DarkColor, System.Drawing.Color LightColor)
        {
            Form1.ActiveForm.Text = URL;
            var Encoder= new Gma.QrCodeNet.Encoding.QrEncoder(Gma.QrCodeNet.Encoding.ErrorCorrectionLevel.L);
            var Code = Encoder.Encode(URL);
            double hd = getDiff(256, Code.Matrix.Height);
            double wd = getDiff(256, Code.Matrix.Width);
            double fd = Math.Floor(Math.Min(hd, wd));
            int ifd = int.Parse(fd.ToString());
            var TempBMP = new Bitmap(256, 256);
            Graphics g = Graphics.FromImage(TempBMP);
            
            for (int X = 0; X < Code.Matrix.Width; X++)
            {
                for (int Y = 0; Y < Code.Matrix.Height;Y++ )
                {
                    if (Code.Matrix.InternalArray[X, Y])
                    {
                        g.FillRectangle(new SolidBrush(DarkColor), X * ifd.ToFloat(), Y * ifd.ToFloat(), ifd.ToFloat(), ifd.ToFloat());
                    }
                    else
                    {
                        g.FillRectangle(new SolidBrush(LightColor), X * ifd.ToFloat(), Y * ifd.ToFloat(), ifd.ToFloat(), ifd.ToFloat());
                    }
                }
            }
            
            return TempBMP;
        }

        private double getDiff(int p1, int p2)
        {
            double d1 = double.Parse(p1 + "");
            double d2 = double.Parse(p2 + "");

            double dret = d1 / d2;

            return dret;


        }
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 20; i++)
            {
                string ran = getRan(20);
                Bitmap image = GenerateQRCode("http://proxy.hagser.se/ul?q=" + ran, Color.Black, Color.White);
                imageList1.Images.Add(ran, image);
                listView1.Items.Add("http://proxy.hagser.se/ul?q=" + ran, ran);
            }
            

        }

        private string getRan(int imax)
        {
            string strret = "";
            int icnt = 0;
            Random rnd = new Random(rnds.Next(int.MinValue,int.MaxValue));
            Regex rx = new Regex("[a-zA-Z0-9]");
            while (strret.Length < imax || icnt>10000)
            {
                int ir = rnd.Next(33, 255);
                string s = (char)ir+"";
                if (rx.IsMatch(s))
                    strret += s;
                icnt++;
            }
            return strret;
        }
    }
}
