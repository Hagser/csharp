using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Xml;
using System.IO;

namespace MyJsonViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        XmlDocument xd = new XmlDocument();
        DataSet ds = new DataSet();

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(textBox1.Text))
            {
                WebClient wc = new WebClient();
                if (textBox1.Text.Contains("api.arbetsformedlingen.se"))
                {
                    wc.Headers.Add("From", "PBIphone@arbetsformedlingen.se");
                }
                wc.Headers.Add("Accept","application/json");
                wc.Headers.Add("Accept-Language","SE");
                wc.Headers.Add("User-Agent","Apache-HttpClient/UNAVAILABLE (java 1.4)");

                string json = wc.DownloadString(textBox1.Text.TrimEnd());
                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(json.ToString(), "");
                //ds.ReadXml(new XmlNodeReader(xd));

                ds.ReadXml((Stream)new MemoryStream(System.Text.Encoding.Default.GetBytes( xd.InnerXml)));

                dataGridView1.DataSource = ds.Tables[0];

                List<string> tbls = new List<string>();
                tbls.Add("Choose");
                for (int i = 0; i < ds.Tables.Count; i++)
                    tbls.Add(ds.Tables[i].TableName);
                comboBox1.DataSource=tbls;
                //http://api.arbetsformedlingen.se/platsannons/2668601
                //http://api.arbetsformedlingen.se/platsannons/matchning?sida=1&antalrader=100&lanid=5&yrkesgruppid=2512

            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ds.Tables[comboBox1.Text];
        }
    }
}
