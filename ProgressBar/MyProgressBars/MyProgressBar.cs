using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace MyProgressBars
{
    public partial class MyProgressBar : UserControl
    {
        public MyProgressBar()
        {
            InitializeComponent();
        }

        public void ChangeValue(string key, int value)
        {
            if (_progresses.ContainsKey(key))
            {
                _progresses[key] = value;
                UpdateFlow();
            }
        }
        public void ChangeColor(string key, Color color)
        {
            if (_progresses.ContainsKey(key))
            {
                if (flowLayoutPanel1.Controls.ContainsKey(key))
                {
                    ProgressBar pb = flowLayoutPanel1.Controls[key] as ProgressBar;
                    pb.ForeColor = color;
                    UpdateFlow();
                }
            }
        }
        public Dictionary<string, int> Progresses {
            get { return _progresses; }
            set { _progresses = value; try { UpdateFlow(); } catch { } }
        }
        private int totalProgress = 0;
        public int TotalProgress
        {
            get
            {
                try
                {
                    return _progresses != null
                        && _progresses.Values != null
                        && _progresses.Values.Count > 0 ?
                        int.Parse(Math.Floor(_progresses.Values.Average()).ToString()) : 0;
                }
                catch { }
                return 0;
            }
            private set { }
        }
        private void UpdateFlow()
        {
            if (_progresses.Keys.Count == 0)
                flowLayoutPanel1.Controls.Clear();
            foreach (var k in _progresses.Keys)
            {
                if (flowLayoutPanel1.Controls.ContainsKey(k))
                {
                    ProgressBar pb = flowLayoutPanel1.Controls[k] as ProgressBar;
                    //pb.ForeColor = Color.Red;
                    if(pb.Maximum>=_progresses[k])
                        pb.Value=_progresses[k];
                    pb.Width = GetWidth();
                    if(this.BackColor!=Color.Transparent)
                        pb.BackColor = this.BackColor;
                    pb.Height = flowLayoutPanel1.Height;
                }
                else
                {
                    ProgressBar pb = new ProgressBar() { Maximum = 100, Minimum = 0, Width = GetWidth() ,Height=flowLayoutPanel1.Height,Margin=new Padding(0) };
                    pb.Name = k;
                    pb.Style = ProgressBarStyle.Continuous;
                    if (this.BackColor != Color.Transparent)
                        pb.BackColor = this.BackColor;
                    flowLayoutPanel1.Controls.Add(pb);
                }
            }
        }

        private int GetWidth()
        {
            double dblMax = flowLayoutPanel1.Width;
            double dblCnt = _progresses.Count > 0 ? _progresses.Count : 1;
            double dblSum = dblMax / dblCnt;
            return int.Parse(Math.Floor(dblSum).ToString());
        }
        
        private Dictionary<string, int> _progresses = new Dictionary<string, int>();
    }
}
