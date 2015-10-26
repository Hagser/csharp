using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyPhotoInfo
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            listBoxRight.Items.Add("Exif Info");
            listBoxRight.Items.Add("Map");
            listBoxRight.Items.Add("Thumbnails");
            listBoxRight.Items.Add("Big Photo");
            listBoxLeft.Items.Add("Treeview");
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            List<object> RemoveList = new List<object>();
            foreach (object str in listBoxLeft.SelectedItems)
            {
                if (listBoxRight.Items.Count == 4)
                    break;
                listBoxRight.Items.Add(str.ToString());
                RemoveList.Add(str);
            }
            foreach (object str in RemoveList)
            {
                listBoxLeft.Items.Remove(str);
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            List<object> RemoveList = new List<object>();
            foreach (object str in listBoxRight.SelectedItems)
            {
                listBoxLeft.Items.Add(str.ToString());
                RemoveList.Add(str);
            }
            foreach (object str in RemoveList)
            {
                listBoxRight.Items.Remove(str);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Tag = listBoxRight.Items.GetEnumerator();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}
