using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace SpotifyTrackToAudacity
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\jh\AppData\Roaming\Audacity\";
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml(textBox1.Text);
                int icnt = 0;
                double leng = 0;
                string labels = "";
                foreach (XmlNode xntrack in xd.DocumentElement.GetElementsByTagName("track"))
                {
                    string name = getChildByName(xntrack, "name");
                    string track = getChildByName(xntrack, "track-number");
                    string length = getChildByName(xntrack, "length").Replace(".",",");
                    labels += string.Format("{0}\t{0}\t{1}\r\n",System.Math.Round(leng,6).ToString().Replace(",", "."), name);
                    leng += double.Parse(length);
                    
                    string filename = track.PadLeft(2).Replace(" ", "0") + "_" + name + ".xml";
                    string body = string.Format("<tags>\r\n"+
                                  "  <tag name=\"TRACKNUMBER\" value=\"{0}\"/>\r\n" +
                                  "  <tag name=\"ALBUM\" value=\"{1}\"/>\r\n" +
                                  "  <tag name=\"TITLE\" value=\"{2}\"/>\r\n" +
                                  "  <tag name=\"ARTIST\" value=\"{3}\"/>\r\n" +
                                  "</tags>",track,"The Road",name,"Nick n Warren");

                    System.IO.File.WriteAllText(path + filename, body);
                    icnt++;
                }
                labels += string.Format("{0}\t{0}\t{1}\r\n", System.Math.Round(leng, 6).ToString().Replace(",", "."), "");
                System.IO.File.WriteAllText(path + "theroad.txt", labels);
                MessageBox.Show(icnt + " file(s) saved!");
            }
            catch (Exception ex)
            {
                this.Text = ex.Message;
            }
        }

        private string getChildByName(XmlNode xntrack, string p)
        {
            foreach (XmlNode xn in xntrack.ChildNodes)
            {
                if (p.Equals(xn.Name, StringComparison.InvariantCultureIgnoreCase))
                    return xn.InnerText;
            }
            return "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml(textBox1.Text);
                this.Text = "";
                button1.Enabled = true;
            }
            catch(Exception ex) {
                this.Text = ex.Message;
                button1.Enabled = false;
            }
        }
    }
}
