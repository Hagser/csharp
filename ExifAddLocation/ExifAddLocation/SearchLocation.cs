using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ExifAddLocation
{
    public partial class SearchLocation : Form
    {
        public Location location { get; set; }
        public SearchLocation(Hashtable htArgs)
        {
            InitializeComponent();
            foreach (object k in htArgs.Keys)
            {
                addArg(k.ToString(), htArgs[k]);
            }
        }


        private void addArg(string p, object p_2)
        {
            System.Windows.Forms.Label lbl = new System.Windows.Forms.Label();
            lbl.Text=p;
            lbl.Width = 100;
            switch (p_2.GetType().FullName)
            { 
                case "System.Windows.Forms.Button":
                    System.Windows.Forms.Button btn = (System.Windows.Forms.Button)p_2;
                    btn.Text = p;
                    flowLayoutPanel1.Controls.Add(btn);
                    break;
                case "System.Windows.Forms.TextBox":
                    flowLayoutPanel1.Controls.Add(lbl);
                    System.Windows.Forms.TextBox txt1 = (System.Windows.Forms.TextBox)p_2;
                    txt1.Width = 100;
                    txt1.Height = 23;
                    txt1.Name = "txt" + p;
                    flowLayoutPanel1.Controls.Add(txt1);
                    break;
                case "System.Windows.Forms.RadioButton":
                    flowLayoutPanel1.Controls.Add(lbl);
                    flowLayoutPanel1.Controls.Add((System.Windows.Forms.RadioButton)p_2);
                    break;
                case "System.Windows.Forms.CheckBox":
                    flowLayoutPanel1.Controls.Add(lbl);
                    flowLayoutPanel1.Controls.Add((System.Windows.Forms.CheckBox)p_2);
                    break;
                default:
                    flowLayoutPanel1.Controls.Add(lbl);
                    System.Windows.Forms.TextBox txt2 = (System.Windows.Forms.TextBox)p_2;
                    txt2.Width = 100;
                    txt2.Height = 23;
                    txt2.Name = "txt" + p;
                    flowLayoutPanel1.Controls.Add(txt2);
                    break;

            }
        }
        public SearchLocation()
        {
            InitializeComponent();
        }
    }
}
