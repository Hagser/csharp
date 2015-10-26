using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySpeedTest
{
    public partial class MapNavigation : UserControl
    {
        public MapNavigation()
        {
            InitializeComponent();
        }
        public event EventHandler VertChanged;
        public event EventHandler HorzChanged;
        public event EventHandler ZoomChanged;

        public int Vert { get { return NavV.Value; } }
        public int Horz { get { return NavH.Value; } }
        public int Zoom { get { return vZoom.Value; } }

        public int MaxZoom { get { return vZoom.Maximum; } set { vZoom.Maximum = value; } }
        public int MinZoom { get { return vZoom.Minimum; } set { vZoom.Minimum = value; } }

        private void NavV_ValueChanged(object sender, EventArgs e)
        {
            if (VertChanged != null)
                VertChanged.Invoke(this, EventArgs.Empty);
        }

        private void NavH_ValueChanged(object sender, EventArgs e)
        {
            if (HorzChanged != null)
                HorzChanged.Invoke(this, EventArgs.Empty);
        }

        private void vZoom_ValueChanged(object sender, EventArgs e)
        {
            if (ZoomChanged != null)
                ZoomChanged.Invoke(this, EventArgs.Empty);
        }
    }
}
