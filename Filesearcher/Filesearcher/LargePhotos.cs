using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ImageInfo;
using System.Drawing;

namespace Filesearcher
{

    public class LargePictureForm : System.Windows.Forms.Form
    {
        PictureBox pb = new PictureBox();
        PictureBox[] pbs = new PictureBox[10];
        FileInfo[] fins = new FileInfo[10];
        int ipborder = 0;


        public LargePictureForm(FileInfo[] in_fins, int in_order)
        {
            ipborder = in_order;
            pb.MouseUp += new MouseEventHandler(pb_MouseUp);
            this.KeyUp += new KeyEventHandler(LargePictureForm_KeyUp);
            fins = in_fins;
            pbs = new PictureBox[in_fins.Length];
            if (in_fins.Length > in_order && in_fins[in_order] != null && in_fins[in_order].Length>0)
            {
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Dock = DockStyle.Fill;
                this.Text = ipborder + "/" + in_fins.Length + " " + in_fins[in_order].FullName;
                Image img = Bitmap.FromFile(in_fins[in_order].FullName);
                pb.Image = img;

                this.WindowState = FormWindowState.Maximized;
                this.Controls.Add(pb);
            }
        }

        void LargePictureForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                nextPb();
            }
            else if (e.KeyCode == Keys.Left)
            {
                prevPb();
            }
        }
        public void loadPb(int in_order)
        {
            if (fins[ipborder] != null && fins[ipborder].Length > 0)
            {
                if (pbs[ipborder] == null || pbs[ipborder].Image == null)
                {
                    FileInfo fi = fins[ipborder];
                    this.Text = ipborder + "/" + fins.Length + " " + fi.FullName;
                    Image img = Bitmap.FromFile(fi.FullName);
                    pb.Image = img;
                    pbs.SetValue(pb, ipborder);
                    pbs[ipborder].Image = img;
                }
                else
                {
                    pb.Image = pbs[ipborder].Image;
                }
            }
        }
        public void nextPb()
        {
            if (pbs.Length > (ipborder + 1))
            {
                ipborder++;
                loadPb(ipborder);
            }
            else
            {
                if (ipborder != -1)
                {
                    ipborder = -1;
                    nextPb();
                }
            }
        }
        public void prevPb()
        {
            if (0 <= (ipborder - 1))
            {
                ipborder--;
                loadPb(ipborder);
            }
            else
            {
                if (ipborder != pbs.Length - 1)
                {
                    ipborder = pbs.Length - 1;
                    prevPb();
                }
            }
        }
        void pb_MouseUp(object sender, MouseEventArgs e)
        {
            double dblw = double.Parse(this.Width.ToString());
            double dblwtp = dblw * .1;
            if (e.X <= int.Parse(Math.Round(dblwtp, 0).ToString()))
            {
                prevPb();
            }
            else if (e.X >= int.Parse(Math.Round((dblw - dblwtp), 0).ToString()))
            {
                nextPb();
            }
        }
    }

}
