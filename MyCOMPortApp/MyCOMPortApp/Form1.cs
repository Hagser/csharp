using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyCOMPortApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                textBox1.Text += serialPort1.ReadExisting();
                textBox1.Select(textBox1.Text.Length - 1,0);
                textBox1.ScrollToCaret();
                });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            serialPort1.Open();
        }
    }
}
