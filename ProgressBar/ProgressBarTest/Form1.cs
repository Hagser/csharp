using MyProgressBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgressBarTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mpb.Dock = DockStyle.Fill;
            for (int i = 0; i < 5; i++)
            {
                mpb.Progresses.Add("nils_" + i.ToString(), GetRan() * GetRan());
            }
            mpb.Progresses.Add("nils_6",100);
            mpb.Width = 100;
            mpb.Location = new Point(0, 0);
            mpb.BorderStyle = BorderStyle.FixedSingle;
            mpb.BackColor = Color.Blue;
            this.Controls.Add(mpb);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                var list = mpb.Progresses.Keys.ToList();
                foreach (var s in list)
                {
                    mpb.ChangeValue(s, (mpb.Progresses[s] + GetRan()));
                    mpb.ChangeColor(s, GetColor(mpb.Progresses[s]%7));
                }
            }
            catch(Exception ex)
            {
                this.Text = ex.Message;
            
            }
        }
        private Color GetColor(int mmm)
        {
            switch (mmm)
            {
                case 0: return Color.Blue;
                case 1: return Color.Yellow;
                case 2: return Color.Red;
                case 3: return Color.Orange;
                case 4: return Color.Cyan;
                case 5: return Color.Magenta;
                case 6: return Color.Black;
                case 7: return Color.Violet;

            }
            return Color.Green;
        }
        private int GetRan()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            return rnd.Next(1, 3);
        }
    }
}
