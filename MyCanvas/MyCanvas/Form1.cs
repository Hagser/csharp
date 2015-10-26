using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyCanvas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Ball> balls = new List<Ball>();
        
        int speed = 2;
        Bitmap bmp = new Bitmap(100, 100); Graphics g = null;
        
		private int cosx(double r,double m,double s,double wf)
		{
		    return int.Parse((Math.Floor(Math.Cos(r) * s * m) + wf).ToString());
		}
		private int sinx(double r,double m,double s,double hf)
        {
		    return int.Parse((Math.Floor(Math.Sin(r) * s * m) + hf).ToString());
		}

        private void Form1_Load(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp); g.FillRectangle(Brushes.Black, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            
            
            
            Timer tc = new Timer();
            tc.Interval = 500;
            bool darkstyle=false;
            int w, h = 0;
            tc.Tick += (a, b) =>
            {
                w = pictureBox1.Width;
                h = pictureBox1.Height;
                int hf = int.Parse(Math.Floor(h / 2d).ToString());
                int wf = int.Parse(Math.Floor(w / 2d).ToString());
                int s = int.Parse(Math.Floor(Math.Min(wf, hf) / 2d).ToString());

                DateTime dt = DateTime.Now;
                darkstyle = dt.Hour > 18 || dt.Hour < 5;

                g.FillRectangle(darkstyle ? Brushes.Black : Brushes.Silver, new Rectangle(0, 0, w, h));
                Pen pen = new Pen(darkstyle ? Brushes.Gray : Brushes.Gray) { Width = s / 60 };

                g.DrawEllipse(pen, new Rectangle(wf-s, hf-s, s*2, s*2));

                pen = new Pen(darkstyle ? Brushes.Silver : Brushes.Black) { Width = s / 60 };
                for (double r = 0; r < (2 * Math.PI); r = r + (Math.PI / 30))
                {

                    var mx = cosx(r, .99,s,wf);
                    var my = sinx(r, .99,s,hf);                    

                    var x = cosx(r, 1.01,s,wf);
                    var y = sinx(r, 1.01,s,hf);
                    g.DrawLine(pen, new Point(mx, my), new Point(x, y));
                    
                }
                pen = new Pen(darkstyle ? Brushes.Silver : Brushes.Black) { Width = s / 20 };
                for (double r = 0; r < (2 * Math.PI); r = r + (Math.PI / 6))
                {
                    var mx = cosx(r, .9, s, wf);
                    var my = sinx(r, .9, s, hf);

                    var x = cosx(r, 1.1, s, wf);
                    var y = sinx(r, 1.1, s, hf);
                    g.DrawLine(pen, new Point(mx, my), new Point(x, y));

                }

                var ho = dt.Hour;
                if (ho > 12)
                    ho = ho - 12;
                var mi = dt.Minute;
                var se = dt.Second;
                var mse = dt.Millisecond;

                //Draw hours arm
		        var rho = (((Math.PI / 6) * ho) - ((Math.PI / 6) * 15)) + (mi/120);		    
                var hx = cosx(rho,.7,s,wf);
		        var hy = sinx(rho,.7,s,hf);
		        pen = new Pen(darkstyle ? Brushes.DarkGreen : Brushes.Green) { Width = s / 20 };
                g.DrawLine(pen, new Point(wf, hf), new Point(hx, hy));


                //Draw minutes arm
                var rmi = (((Math.PI / 30) * mi) - ((Math.PI / 30) * 15)) + (se / 720);
                var mix = cosx(rmi, .9, s, wf);
                var miy = sinx(rmi, .9, s, hf);
                pen = new Pen(darkstyle ? Brushes.DarkBlue : Brushes.Blue) { Width = s / 40 };
                g.DrawLine(pen, new Point(wf, hf), new Point(mix, miy));


                //Draw seconds arm
                var rse = ((Math.PI / 30) * se) - ((Math.PI / 30) * 15);//+ (mse/10000);
                var sx = cosx(rse, .95, s, wf);
                var sy = sinx(rse, .95, s, hf);
                var ssx = cosx(rse, -.25,s,wf);
                var ssy = sinx(rse, -.25,s,hf);
                pen = new Pen(darkstyle ? Brushes.Gray : Brushes.Black) { Width = s / 80 };
                g.DrawLine(pen, new Point(ssx, ssy), new Point(sx, sy));


                //Draw larger gray ring
                pen = new Pen(darkstyle ? Brushes.Gray : Brushes.Black) { Width = s / 80 };
                Rectangle rect = new Rectangle(wf - (s / 15), hf - (s / 15), (s / 15) * 2, (s / 15) * 2);
                g.DrawEllipse(pen, rect);
                g.FillEllipse(darkstyle ? Brushes.Silver : Brushes.Gray, rect);

                //Draw smaller gray ring
                pen = new Pen(darkstyle ? Brushes.Gray : Brushes.Black) { Width = s / 80 };
                Rectangle rect2 = new Rectangle(wf - (s / 55), hf - (s / 55), (s / 55) * 2, (s / 55) * 2);
                g.DrawEllipse(pen, rect2);
                g.FillEllipse(Brushes.Black, rect2);
                
                pictureBox1.Image = bmp;
            };
            tc.Start();
            return;
            
            Random rnd = new Random((new Random()).Next(65000));
            for (int i = 0; i < 50; i++)
            {
                int rx = (rnd.Next(speed * -1, speed));
                int ry = (rnd.Next(speed * -1, speed));
                Color color = Color.FromArgb(rnd.Next(50, 255), rnd.Next(50, 255), rnd.Next(50, 255));
                Ball ball = new Ball()
                    {
                        x=rnd.Next(0,pictureBox1.Width),
                        y=rnd.Next(0,pictureBox1.Height),
                        brush = new SolidBrush(color),
                        mx = rx != 0 ? rx : speed,
                        my = ry != 0 ? ry : speed*-1,
                        size=rnd.Next(2,20)
                    };
                balls.Add(ball);
                ball.isx = ball.x;
                ball.isy = ball.y;
                ball.polygon = getPolyGon(ball);
                //g.FillPie(ball.brush, new Rectangle(ball.x, ball.y, ball.size, ball.size), 0, 360);
                //g.FillPolygon(ball.brush, getPolyGon(ball));
            }
            Ball ball1 = new Ball() {
                x = rnd.Next(0, pictureBox1.Width),
                y = rnd.Next(0, pictureBox1.Height),
                brush = new SolidBrush(Color.White),
                size=100
            };
            g.DrawPolygon(Pens.White, getPolyGon(ball1));
            //
            //g.DrawString(lista.Count.ToString(), DefaultFont, Brushes.Blue, new Point(m - int.Parse(Math.Floor(DefaultFont.GetHeight()).ToString()), m - int.Parse(Math.Floor(DefaultFont.GetHeight()).ToString())));

            pictureBox1.Image = bmp;
            return;

            Timer t = new Timer();
            t.Interval = 5;
            t.Tick += (a, b) => {
                g.FillRectangle(Brushes.Black, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));

                if (line.Count > 0)
                {
                    foreach(List<Point> points in line)
                        g.FillPolygon(Brushes.White, points.ToArray());
                }
                if (tmpline.Count > 0)
                    g.DrawPolygon(Pens.Blue, tmpline.ToArray());

                g.Flush();
                List<Ball> remove = new List<Ball>();
                foreach (Ball ball in balls)
                {
                    
                    Point[] ps = ball.polygon;
                    if (ps.Where(p => p.Y < bmp.Height && p.X < bmp.Width && p.X > 0 && p.Y > 0 && (p.X + (ball.isx - ball.x)) > 0 && (p.Y + (ball.isy - ball.y)) > 0 && (p.X + (ball.isx - ball.x)) < bmp.Width && (p.Y + (ball.isy - ball.y)) < bmp.Height)
                        .Any(p => !bmp.GetPixel(p.X + (ball.isx - ball.x), p.Y + (ball.isy - ball.y)).Equals(ball.color))
                        )
                    {
                        g.DrawString("Jupp", DefaultFont, Brushes.YellowGreen, ball.x - 20, ball.y - 20);
                        ball.mx *= -1;
                        //ball.my *= -1;
                    }
                    else
                    {
                        if (ball.x <= 0 || ball.x + (ball.size / 2) >= pictureBox1.Width)
                        {
                            Color c = ball.color;
                            ball.color = Color.FromArgb(Math.Max(c.R - 1, 0), Math.Max(c.G - 1, 0), Math.Max(c.B - 1, 0));
                            ball.mx *= -1;
                        }

                        if (ball.y <= 0 || ball.y + (ball.size / 2) >= pictureBox1.Height)
                        {
                            Color c = ball.color;
                            ball.color = Color.FromArgb(Math.Max(c.R - 1, 0), Math.Max(c.G - 1, 0), Math.Max(c.B - 1, 0));
                            ball.my *= -1;
                        }
                    }
                    ball.x += ball.mx;
                    ball.y += ball.my;

                    g.FillPie(ball.brush, new Rectangle(ball.x, ball.y, ball.size, ball.size), 0, 360);
                    //g.FillPolygon(ball.brush, getPolyGon(ball));

                    Color c2 = (ball.brush as SolidBrush).Color;
                    if (c2.B < 10 || c2.R < 10 || c2.G < 10)
                        remove.Add(ball);

                }
                foreach (Ball ball in remove)
                    balls.Remove(ball);
                remove.Clear();


                this.Text = balls.Count.ToString();
                if (balls.Count == 0)
                    t.Stop();

                pictureBox1.Image = bmp;
            };
            t.Start();
        }

        private Point[] getPolyGon(Ball ball)
        {
            Dictionary<double, Point> dict = new Dictionary<double, Point>();
            
            List<Point> lista = new List<Point>();

            int s = ball.size / 2;
            for (double v = 0; v <= (2 * Math.PI); v = v + (Math.PI / 360))
            {
                double x = Math.Floor(Math.Cos(v) * s) + ball.x+s;
                double y = Math.Floor(Math.Sin(v) * s) + ball.y + s;

                int ix = int.Parse(x.ToString());
                int iy = int.Parse(y.ToString());
                Point p = new Point(ix, iy);
                if (!lista.Contains(p))
                    lista.Add(p);

            }
            return lista.ToArray();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp); 
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            pictureBox1.Image = bmp;
        }
        int lx, ly = -1;
        bool bdraw = false;
        List<List<Point>> line = new List<List<Point>>();
        List<Point> tmpline = new List<Point>();
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && bdraw)
            {
                tmpline.Add(new Point(e.X, e.Y));
                g.DrawLine(Pens.White, e.X, e.Y, lx, ly);
                tmpline.Add(new Point(lx, ly));
                lx = e.X;
                ly = e.Y;
                g.Flush();
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            bdraw = true;
            tmpline = new List<Point>();
            lx = e.X;
            ly = e.Y;

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if(tmpline.Count>0)
                line.Add(tmpline);
            bdraw = false;
        }
    }
    public class Ball
    {
        public int isx { get; set; }
        public int isy { get; set; }
        public Point[] polygon { get; set; }
        public int size { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public Brush brush { get; set; }
        public int mx { get; set; }
        public int my { get; set; }
        public Color color
        {
            get {
                return (this.brush != null && (this.brush as SolidBrush) != null) ? (this.brush as SolidBrush).Color : Color.Black;
            }
            set { 
                if(this.brush != null && (this.brush as SolidBrush) != null)
                    (this.brush as SolidBrush).Color=value;
            }
        }
    }
}
