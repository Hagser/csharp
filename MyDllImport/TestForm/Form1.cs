using MyDllImport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForm
{
    public partial class Form1 : Form
    {
        globalMouseHook gmh = new globalMouseHook();
        globalKeyboardHook ghk = new globalKeyboardHook();
        public Form1()
        {
            InitializeComponent();
            gmh.MouseEvent += gmh_MouseEvent;
            ghk.KeyDown += ghk_KeyDown;
        }

        void ghk_KeyDown(object sender, KeyEventArgs e)
        {
            label1.Text = String.Format("c:{0}", e.KeyValue);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        int iclicks = 0;
        void gmh_MouseEvent(object sender, MouseEventArgs e)
        {
            label1.Text = String.Format("x:{0},y:{1},d:{2},h:{3},d:{4}", e.X, e.Y, e.Clicks, e.Clicks - iclicks, e.Delta);
            iclicks = e.Clicks;
        }
    }
}
