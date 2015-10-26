using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HexViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        FileInfo fi;
        FileStream fs;
        int icharsperrow = 20;
        int _ipos = 0;
        int ipos {
            get {return _ipos ;}
            set { _ipos = value>0?value:0; textBox1.Text = _ipos.ToString(); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                fi = new FileInfo(openFileDialog1.FileName);
                textBox4.Text = fi.Length.ToString();
                fs = File.OpenRead(fi.FullName);
                readFile();
            }
        }

        private void readFile()
        {
            byte[] buf = new byte[8196];
            string s = "";
            string h = "";
            fs.Position = ipos;
            int iread=fs.Read(buf, 0, buf.Length);
            listView1.Items.Clear();
            int icnt = 0;
            for (int i = 0; i < iread; i++)
            {
                byte b = (byte)buf.GetValue(i);
                s += " " + b.ToString().PadLeft(3,' ').Replace(" ","0");
                h += " " + b.ToString("X").PadLeft(2, ' ').Replace(" ", "0");
                icnt++;
                if (icnt == icharsperrow)
                {
                    ListViewItem lvi = listView1.Items.Add((((ipos + i)-icharsperrow)+1).ToString());
                    lvi.SubItems.Add(s.Substring(1));
                    lvi.SubItems.Add(h.Substring(1));
                    lvi.SubItems.Add(getString(lvi.SubItems[1].Text));
                    s = "";
                    h = "";
                    icnt = 0;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ipos += 8196;
            readFile();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ipos -= 8196;
            readFile();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = "";
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                textBox2.Text += lvi.SubItems[3].Text;
            }
        }

        private string getString(string p)
        {
            string strRet = "";
            foreach (string str in p.Split(' '))
            {
                int ichar = int.Parse(str);
                if (ichar > 20)
                {
                    strRet += (char)ichar;
                }
                else
                {
                    strRet += ".";
                }
            }
            return strRet;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ipos = int.Parse(textBox1.Text);
            readFile();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            icharsperrow = int.Parse(textBox3.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FileStream fs2 = File.OpenWrite(fi.FullName+".clip");
            byte[] buf = new byte[8192];
            int iTotalRead = 0;
            int iRead = 0;
            fs.Position = ipos;

            while ((iRead = fs.Read(buf, 0, buf.Length)) > 0)
            {
                fs2.Write(buf, 0, iRead);
                iTotalRead += iRead;
            }

            fs2.Flush();
            fs2.Close();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            fs.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int ix = 0;
            //for (int ix = 0; ix < 30; ix++)
            {
                FileStream fs2 = File.OpenWrite(fi.FullName.Replace(".avi",".clip" + ix.ToString() + ".avi"));
                byte[] buf = new byte[ipos];
                int iTotalRead = 0;
                int iRead = 0;
                fs.Position = 0;
                iRead = fs.Read(buf, 0, buf.Length);
                fs2.Write(buf, 0, iRead);
                iTotalRead += iRead;
                int ipos2 = int.Parse(textBox4.Text)+ix;
                buf = new byte[fi.Length - ipos2];
                fs.Position = ipos2;
                iRead = fs.Read(buf, 0, buf.Length);
                fs2.Write(buf, 0, iRead);
                iTotalRead += iRead;

                fs2.Flush();
                fs2.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ipos = int.Parse(textBox1.Text);
        }


        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] ss = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (ss != null && ss.Length > 0)
            {
                string s = ss[0];
                if (File.Exists(s))
                {
                    fi = new FileInfo(s);
                    textBox4.Text = fi.Length.ToString();
                    fs = File.OpenRead(fi.FullName);
                    readFile();
                }
            }
        }

        private void Form1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
    }
}
