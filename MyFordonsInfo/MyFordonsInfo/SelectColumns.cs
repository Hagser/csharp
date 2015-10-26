using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyFordonsInfo
{
    public partial class SelectColumns : Form
    {
        public Dictionary<string, bool> selectedColumns = new Dictionary<string, bool>();
        public SelectColumns(Dictionary<string,bool> allColumns)
        {
            InitializeComponent();
            checkedListBox1.Items.AddRange(allColumns.Keys.ToArray());

            for (int i = 0; i < checkedListBox1.Items.Count; i++)            
                checkedListBox1.SetItemChecked(i, allColumns[checkedListBox1.Items[i] + ""]);
            
        }

        private void SelectColumns_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                selectedColumns.Add(checkedListBox1.Items[i]+"", checkedListBox1.GetItemChecked(i));
        }
    }
}
