using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyScreenSaver
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            PerformanceCounterCategory[] cats = PerformanceCounterCategory.GetCategories();
            comboBox1.Items.AddRange(cats.OrderBy(x=>x.CategoryName).ToArray());
            comboBox1.ValueMember = "CategoryName";
            comboBox1.TextChanged += (a, b) => {
                PerformanceCounterCategory cat = comboBox1.SelectedItem as PerformanceCounterCategory;
                if (cat != null)
                {
                    comboBox2.ResetText();
                    comboBox3.ResetText();
                    comboBox2.Items.Clear();
                    comboBox2.Items.AddRange(cat.GetInstanceNames().OrderBy(x => x).ToArray());
                }
            };
            comboBox2.TextChanged += (a, b) =>
            {
                PerformanceCounterCategory cat = comboBox1.SelectedItem as PerformanceCounterCategory;
                string strInst = comboBox2.Text;
                PerformanceCounter[] counters = cat.GetCounters(strInst);
                comboBox3.ResetText();
                comboBox3.Items.Clear();
                comboBox3.Items.AddRange(counters.OrderBy(x => x.CounterName).ToArray());
                comboBox3.ValueMember = "CounterName";

            };

            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);

            if (key.OpenSubKey(Application.ProductName, true) != null)
            {
                key = key.OpenSubKey(Application.ProductName, true);
                if (key.OpenSubKey(Application.ProductVersion, true) != null)
                {
                    key = key.OpenSubKey(Application.ProductVersion, true);
                    string cat = key.GetValue("Category").ToString();
                    string ins = key.GetValue("Instance").ToString();
                    string cou = key.GetValue("Counter").ToString();
                    comboBox1.Text = cat;
                    comboBox2.Text = ins;
                    comboBox3.Text = cou;
                }
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);

            if(key.OpenSubKey(Application.ProductName,true)==null)
                key.CreateSubKey(Application.ProductName);
            key = key.OpenSubKey(Application.ProductName, true);

            if (key.OpenSubKey(Application.ProductVersion, true) == null)
                key.CreateSubKey(Application.ProductVersion);
            key = key.OpenSubKey(Application.ProductVersion, true);

            key.SetValue("Category", comboBox1.Text);
            key.SetValue("Instance", comboBox2.Text);
            key.SetValue("Counter", comboBox3.Text);

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
