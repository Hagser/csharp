using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Text;

namespace ClipboardCatcher
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.IContainer components;
		private System.Windows.Forms.SaveFileDialog sFD;
		private System.Windows.Forms.OpenFileDialog oFD;
		public Hashtable htTemp = new Hashtable();
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem chooseToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel tSSL;
        private ListView listV;
        private ColumnHeader colHDisplay;
        private ColumnHeader colHData;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem sortToolStripMenuItem;
        private ToolStripMenuItem ascendingToolStripMenuItem;
        private ToolStripMenuItem descendingToolStripMenuItem;
        private ToolStripMenuItem noneToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem cancelToolStripMenuItem;
        private ToolStripMenuItem copytoolStripMenuItemComma;
        private ToolStripMenuItem copytoolStripMenuItemSemiColon;
        private ToolStripMenuItem copyToolStripMenuItemBreak;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem replacerToolStripMenuItem;
        private ToolStripMenuItem insertLangFileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem reloadLangFilesToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5;
		public string v_filename = "";

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.sFD = new System.Windows.Forms.SaveFileDialog();
            this.oFD = new System.Windows.Forms.OpenFileDialog();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.chooseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tSSL = new System.Windows.Forms.ToolStripStatusLabel();
            this.listV = new System.Windows.Forms.ListView();
            this.colHDisplay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.replacerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.insertLangFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadLangFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copytoolStripMenuItemComma = new System.Windows.Forms.ToolStripMenuItem();
            this.copytoolStripMenuItemSemiColon = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItemBreak = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ascendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.descendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // sFD
            // 
            this.sFD.CreatePrompt = true;
            this.sFD.DefaultExt = "txt";
            this.sFD.Filter = "Text files|*.txt";
            this.sFD.Title = "Save file";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "ClipboardCatcher";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chooseToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(115, 26);
            this.contextMenuStrip1.Opened += new System.EventHandler(this.contextMenuStrip1_Opened);
            // 
            // chooseToolStripMenuItem
            // 
            this.chooseToolStripMenuItem.Name = "chooseToolStripMenuItem";
            this.chooseToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.chooseToolStripMenuItem.Text = "Choose";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSSL});
            this.statusStrip1.Location = new System.Drawing.Point(0, 318);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(469, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tSSL
            // 
            this.tSSL.Name = "tSSL";
            this.tSSL.Size = new System.Drawing.Size(0, 17);
            // 
            // listV
            // 
            this.listV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHDisplay,
            this.colHData});
            this.listV.ContextMenuStrip = this.contextMenuStrip2;
            this.listV.FullRowSelect = true;
            this.listV.GridLines = true;
            this.listV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listV.Location = new System.Drawing.Point(0, 1);
            this.listV.Name = "listV";
            this.listV.Size = new System.Drawing.Size(469, 314);
            this.listV.TabIndex = 2;
            this.listV.UseCompatibleStateImageBehavior = false;
            this.listV.View = System.Windows.Forms.View.Details;
            this.listV.DoubleClick += new System.EventHandler(this.listV_DoubleClick);
            this.listV.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listV_KeyDown);
            // 
            // colHDisplay
            // 
            this.colHDisplay.Text = "";
            this.colHDisplay.Width = 447;
            // 
            // colHData
            // 
            this.colHData.Text = "";
            this.colHData.Width = 0;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripSeparator1,
            this.copyToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.selectAllToolStripMenuItem,
            this.sortToolStripMenuItem,
            this.toolStripMenuItem1,
            this.cancelToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(153, 192);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.replacerToolStripMenuItem,
            this.insertLangFileToolStripMenuItem,
            this.reloadLangFilesToolStripMenuItem});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem2.Text = "File";
            // 
            // replacerToolStripMenuItem
            // 
            this.replacerToolStripMenuItem.CheckOnClick = true;
            this.replacerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem5});
            this.replacerToolStripMenuItem.Name = "replacerToolStripMenuItem";
            this.replacerToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.replacerToolStripMenuItem.Text = "Replacer";
            this.replacerToolStripMenuItem.CheckedChanged += new System.EventHandler(this.replacerToolStripMenuItem_CheckedChanged);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.CheckOnClick = true;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem4.Text = "<%=%>";
            this.toolStripMenuItem4.CheckedChanged += new System.EventHandler(this.toolStripMenuItem4_CheckedChanged);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.CheckOnClick = true;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem5.Text = "\"++\"";
            this.toolStripMenuItem5.CheckedChanged += new System.EventHandler(this.toolStripMenuItem5_CheckedChanged);
            // 
            // insertLangFileToolStripMenuItem
            // 
            this.insertLangFileToolStripMenuItem.Name = "insertLangFileToolStripMenuItem";
            this.insertLangFileToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.insertLangFileToolStripMenuItem.Text = "Insert Lang Files";
            this.insertLangFileToolStripMenuItem.Click += new System.EventHandler(this.insertLangFileToolStripMenuItem_Click);
            // 
            // reloadLangFilesToolStripMenuItem
            // 
            this.reloadLangFilesToolStripMenuItem.Name = "reloadLangFilesToolStripMenuItem";
            this.reloadLangFilesToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.reloadLangFilesToolStripMenuItem.Text = "Reload Lang Files";
            this.reloadLangFilesToolStripMenuItem.Click += new System.EventHandler(this.reloadLangFilesToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copytoolStripMenuItemComma,
            this.copytoolStripMenuItemSemiColon,
            this.copyToolStripMenuItemBreak});
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // copytoolStripMenuItemComma
            // 
            this.copytoolStripMenuItemComma.Name = "copytoolStripMenuItemComma";
            this.copytoolStripMenuItemComma.Size = new System.Drawing.Size(86, 22);
            this.copytoolStripMenuItemComma.Text = ",";
            this.copytoolStripMenuItemComma.Click += new System.EventHandler(this.copytoolStripMenuItem_Click);
            // 
            // copytoolStripMenuItemSemiColon
            // 
            this.copytoolStripMenuItemSemiColon.Name = "copytoolStripMenuItemSemiColon";
            this.copytoolStripMenuItemSemiColon.Size = new System.Drawing.Size(86, 22);
            this.copytoolStripMenuItemSemiColon.Text = ";";
            this.copytoolStripMenuItemSemiColon.Click += new System.EventHandler(this.copytoolStripMenuItem_Click);
            // 
            // copyToolStripMenuItemBreak
            // 
            this.copyToolStripMenuItemBreak.Name = "copyToolStripMenuItemBreak";
            this.copyToolStripMenuItemBreak.Size = new System.Drawing.Size(86, 22);
            this.copyToolStripMenuItemBreak.Text = "\\n";
            this.copyToolStripMenuItemBreak.Click += new System.EventHandler(this.copytoolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.selectAllToolStripMenuItem.Text = "Select &all";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ascendingToolStripMenuItem,
            this.descendingToolStripMenuItem,
            this.noneToolStripMenuItem});
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            this.sortToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sortToolStripMenuItem.Text = "So&rt";
            // 
            // ascendingToolStripMenuItem
            // 
            this.ascendingToolStripMenuItem.Name = "ascendingToolStripMenuItem";
            this.ascendingToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.ascendingToolStripMenuItem.Text = "&Ascending";
            this.ascendingToolStripMenuItem.Click += new System.EventHandler(this.ascendingToolStripMenuItem_Click);
            // 
            // descendingToolStripMenuItem
            // 
            this.descendingToolStripMenuItem.Name = "descendingToolStripMenuItem";
            this.descendingToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.descendingToolStripMenuItem.Text = "&Descending";
            this.descendingToolStripMenuItem.Click += new System.EventHandler(this.descendingToolStripMenuItem_Click);
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Checked = true;
            this.noneToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.noneToolStripMenuItem.Text = "&None";
            this.noneToolStripMenuItem.Click += new System.EventHandler(this.noneToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // cancelToolStripMenuItem
            // 
            this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            this.cancelToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.cancelToolStripMenuItem.Text = "Cancel";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(469, 340);
            this.Controls.Add(this.listV);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "ClipboardCatcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}
		public bool isValid(string in_value)
		{
			bool v_ret=true;//(in_value.IndexOf("http://")==0 || in_value.IndexOf("https://")==0);
			return v_ret;
		}
        Hashtable htLang = new Hashtable();
		private void timer1_Tick(object sender, System.EventArgs e)
		{
            try
            {
                IDataObject dObj;
                string v_val = "";
                string v_key = "";
                dObj = Clipboard.GetDataObject();

                if (dObj.GetDataPresent(DataFormats.Text))
                {
                    v_val = dObj.GetData(DataFormats.Text).ToString();//.Replace("\n", " ");//.Replace("\u000A", " ").Replace("\u000D", " ");
                    string o_val = v_val;
                    v_key = v_val.Replace("\n", " ").Replace("\u000A", " ").Replace("\u000D", " ").Replace("\t", " ");
                    while (v_key.IndexOf("  ") != -1){v_key = v_key.Replace("  ", " ");}
                    v_val = Convertion.ToBase64(v_val);
                    if (isValid(v_val) && !htTemp.ContainsValue(v_val) && !htTemp.ContainsKey(v_key))
                    {
                        htTemp.Add(v_key, v_val);
                        //listboxClips.Items.Add(v_val);
                        ListViewItem lvi = listV.Items.Add(v_key);
                        lvi.SubItems.Add(v_val);
                        tSSL.Text = "Added new value. " + System.DateTime.Now.ToLongTimeString();
                    }
                    if (replacerToolStripMenuItem.Checked)
                    {
                        if (htLang.ContainsKey(o_val.ToLower()))
                        {
                            string k = htLang[o_val.ToLower()].ToString();
                            if (toolStripMenuItem5.Checked)
                            {
                                Clipboard.SetText("\"+Lang.getWord(session,\"" + k + "\")+\"");
                            }
                            else
                            {
                                Clipboard.SetText("<%=Lang.getWord(session,\"" + k + "\")%>");
                            }
                        }
                    }
                }
                else if (dObj.GetDataPresent(DataFormats.Bitmap))
                {
                    Bitmap bmap;
                    bmap = (Bitmap)dObj.GetData(DataFormats.Bitmap);
                    if (!Directory.Exists(Application.StartupPath + "\\saved_file"))
                    {
                        Directory.CreateDirectory(Application.StartupPath + "\\saved_file");
                    }
                    string v_filename = Application.StartupPath + "\\saved_file\\" + System.DateTime.Now.ToString().Replace(" ", "").Replace("/", "").Replace("\\", "").Replace(":", "").Replace("-", "") + ".png";
                    v_key = v_filename;
                    v_val = Convertion.ToBase64(v_filename);
                    if (!htTemp.ContainsValue(v_val) && !htTemp.ContainsKey(v_key))
                    {
                        bmap.Save(v_filename);

                        Clipboard.SetDataObject(new object(), false);

                        ListViewItem lvi = listV.Items.Add(v_key);
                        lvi.SubItems.Add(v_val);

                        htTemp.Add(lvi.Text, lvi.SubItems[1].Text);
                        tSSL.Text = "Saved image: " + v_filename;
                    }
                }

                this.Text = listV.Items.Count.ToString();
            }
            catch
            { }

		}
        private void clicktool(object sender, System.EventArgs e)
        {
            ToolStripItem newtsi = (ToolStripItem)sender;
            string strTag = newtsi.Tag.ToString();
                strTag = getFromBase64(strTag);
            Clipboard.SetText(strTag); 
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            chooseToolStripMenuItem.DropDownItems.Clear();
            for (int x = 0; x < listV.Items.Count; x++)
            {
                string strtext = listV.Items[x].SubItems[0].Text;
                strtext = strtext.Replace("\n", " ").Replace("\u000A", " ").Replace("\u000D", " ").Replace("\t", " ");
                while (strtext.IndexOf("  ") != -1)
                {
                    strtext=strtext.Replace("  ", " ");
                }
                ToolStripItem newtsi = chooseToolStripMenuItem.DropDownItems.Add(strtext.Substring(0,Math.Min(strtext.Length,50)));
                try {
                    if (File.Exists(strtext))
                    {
                        using (Bitmap bmp = new Bitmap(Bitmap.FromFile(strtext), new Size(64, 64)))
                        {
                            //newtsi.Image = bmp;
                            //newtsi.ImageScaling = ToolStripItemImageScaling.SizeToFit;
                            bmp.Dispose();
                        }
                    }
                }
                catch { }
                newtsi.Name = x + "_itm";
                newtsi.Tag = listV.Items[x].SubItems[1].Text;
                newtsi.Click += new EventHandler(this.clicktool);

            }
        }

        private void listV_DoubleClick(object sender, EventArgs e)
        {   
            if (listV.SelectedItems.Count==1)
            {
                string strTag = listV.SelectedItems[0].SubItems[1].Text;
                    strTag = getFromBase64(strTag);
                Clipboard.SetDataObject(strTag);
                tSSL.Text = "Copied to Clipboard. " + System.DateTime.Now.ToLongTimeString();
            }
        }
        private string getFromBase64(string in_string)
        {
            string v_ret = in_string;
            if (Convertion.isBase64(v_ret)) //(v_ret.IndexOf(" ") == -1 && v_ret.IndexOf("\t") == -1 && v_ret.IndexOf("\n") == -1 && v_ret.IndexOf(";") == -1 && v_ret.IndexOf(":") == -1 && v_ret.IndexOf(".") == -1 && v_ret.IndexOf("?") == -1 && v_ret.IndexOf("!") == -1 && v_ret.IndexOf("{") == -1 && v_ret.IndexOf("}") == -1 && v_ret.IndexOf("[") == -1 && v_ret.IndexOf("]") == -1 && v_ret.IndexOf("(") == -1 && v_ret.IndexOf(")") == -1 && v_ret.IndexOf("_") == -1 && v_ret.IndexOf("-") == -1)
            {
                try
                {
                    v_ret = Convertion.FromBase64(v_ret);
                }
                catch { }
            }
            return v_ret;
        }
        private void openfile(string in_filename)
        {
            string v_key = "";
            string v_val = "";
            using (StreamReader sr = File.OpenText(in_filename))
            {
                while (sr.Peek() != -1)
                {
                    v_val = sr.ReadLine();
                    v_key = v_val;
                    v_key = getFromBase64(v_key);

                    if (isValid(v_val) && !htTemp.ContainsValue(v_val) && !htTemp.ContainsKey(v_key))
                    {
                        htTemp.Add(v_key, v_val);
                        //listboxClips.Items.Add(v_val);
                        ListViewItem lvi = listV.Items.Add(v_key);
                        lvi.SubItems.Add(v_val);

                    }
                }
                sr.Close();
            }
        }
        private void openfile(string in_filename,bool bbase64)
        {
            string v_key = "";
            string v_val = "";
            using (StreamReader sr = File.OpenText(in_filename))
            {
                while (sr.Peek() != -1)
                {
                    if (bbase64)
                    {
                        v_val = sr.ReadLine();
                        v_key = v_val;
                        v_key = getFromBase64(v_key);
                    }
                    else
                    {
                        v_val = sr.ReadLine();
                        v_key = v_val;
                    }
                    if (isValid(v_val) && !htTemp.ContainsValue(v_val) && !htTemp.ContainsKey(v_key))
                    {
                        htTemp.Add(v_key, v_val);
                        //listboxClips.Items.Add(v_val);
                        ListViewItem lvi = listV.Items.Add(v_key);
                        lvi.SubItems.Add(v_val);

                    }
                }
                sr.Close();
            }
        }
        private void savefile(string in_filename)
        {
            using (StreamWriter sr = File.CreateText(in_filename))
            {
                for (int i = 0; i < listV.Items.Count; i++)
                {
                    sr.WriteLine(listV.Items[i].SubItems[1].Text);
                }
                sr.Close();
            }
        }
        void openFileDialog()
        {
            oFD.ShowDialog();
            v_filename = oFD.FileName;
            if (v_filename != "")
            {
                tSSL.Text = "Opening: " + v_filename;
                openfile(v_filename);
                tSSL.Text = "Opened: " + v_filename;
            }
        }
        void saveFileDialog()
        {
            if (v_filename == "" || v_filename.Equals(""))
            {
                sFD.ShowDialog();
                v_filename = sFD.FileName;
            }
            if (v_filename != "")
            {
                tSSL.Text = "Saving file: " + v_filename;
                savefile(v_filename);
                tSSL.Text = "Saved file: " + v_filename;
            }
        }
        void selectAll()
        {
            for (int i = 0; i < listV.Items.Count; i++)
            {
                listV.Items[i].Selected = true;
            }        
        }

        void copySelected()
        {
            copySelected("\n");
        }
        void copySelected(string delim)
        {
            string v_val = "";
            for (int i = 0; i < listV.SelectedItems.Count; i++)
            {
                string strTag = listV.SelectedItems[i].SubItems[1].Text.ToString();
                strTag = getFromBase64(strTag);

                if (v_val == "")
                {
                    v_val = strTag;
                }
                else
                {
                    v_val += delim + strTag;
                }
            }
            Clipboard.SetDataObject(v_val);
            tSSL.Text = "Copied to Clipboard. " + System.DateTime.Now.ToLongTimeString();
        }
        void deleteSelected()
        {
            for (int i = listV.SelectedItems.Count - 1; i > -1; i--)
            {
                htTemp.Remove(listV.SelectedItems[i].ToString());
                listV.SelectedItems[i].Remove();
            }
            tSSL.Text = "Selected row(s) deleted. " + System.DateTime.Now.ToLongTimeString(); 
        }
        private void listV_KeyDown(object sender, KeyEventArgs e)
        {            
            if (e.Control)
            {
                if (e.KeyCode == Keys.O)
                {
                    openFileDialog();
                }
                else if (e.KeyCode == Keys.S)
                {
                    saveFileDialog();
                }
                else if (e.KeyCode == Keys.A)
                {
                    selectAll();
                }
                else if (e.KeyCode == Keys.C || e.KeyCode==Keys.Enter)
                {
                    copySelected();
				}
				else if (e.KeyCode == Keys.X)
				{
					listV.Items.Clear();
				}
            }
            if (e.KeyCode == Keys.Enter)
            {
                copySelected();
            }
            if (e.KeyCode == Keys.Delete)
            {
                deleteSelected();
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            try
            {
                listV.Columns[0].Width = this.Width - 30;
                listV.Columns[1].Width = 0;
            }
            catch { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string v_filename = Application.StartupPath + "\\CC_" + DateTime.Today.Ticks.ToString() + ".txt";
            savefile(v_filename);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string v_filename = Application.StartupPath + "\\CC_" + DateTime.Today.Ticks.ToString() + ".txt";
            if(File.Exists(v_filename))
            {
                openfile(v_filename);
            }

        }

        private void sortList(SortOrder so)
        {
            listV.Sorting = so;
            listV.Sort();
            listV.ListViewItemSorter = new ListViewItemComparer(0,listV.Sorting, System.TypeCode.String);
            listV.Sorting = SortOrder.None;
        }
        private void ascendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sortList(SortOrder.Ascending);
            ascendingToolStripMenuItem.Checked = true;
            descendingToolStripMenuItem.Checked=false;
            noneToolStripMenuItem.Checked = false;
        }

        private void descendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sortList(SortOrder.Descending);
            ascendingToolStripMenuItem.Checked = false;
            descendingToolStripMenuItem.Checked = true;
            noneToolStripMenuItem.Checked = false;
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sortList(SortOrder.None);
            ascendingToolStripMenuItem.Checked = false;
            descendingToolStripMenuItem.Checked = false;
            noneToolStripMenuItem.Checked = true;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copySelected();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectAll();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void copytoolStripMenuItem_Click(object sender, EventArgs e)
        {
            copySelected((sender as ToolStripMenuItem).Text);
        }
        Hashtable filewatch = new Hashtable();
        ArrayList openedfiles = new ArrayList();
        private void insertLangFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Lang files|*.properties";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    foreach (string file in ofd.FileNames)
                    {
                        if (File.Exists(file))
                        {
                            FileInfo fi = new FileInfo(file);
                            if (!filewatch.ContainsKey(fi.DirectoryName))
                            {
                                FileSystemWatcher fsw = new FileSystemWatcher(fi.DirectoryName);
                                fsw.EnableRaisingEvents = true;
                                fsw.NotifyFilter = NotifyFilters.LastWrite;
                                fsw.Changed += (a, b) =>
                                {
                                    try
                                    {
                                        if (b.ChangeType == WatcherChangeTypes.Changed)
                                        {
                                            if (openedfiles.Contains(b.FullPath))
                                            {
                                                LoadLangFile(b.FullPath);
                                            }
                                        }
                                    }
                                    catch { }
                                };
                                filewatch.Add(fi.DirectoryName, fsw);
                            }
                            LoadLangFile(file);
                        }
                    }
                }
                catch { }
                replacerToolStripMenuItem.Checked = true;
            }
        }

        private void LoadLangFile(string file)
        {
            try
            {
                if (!openedfiles.Contains(file))
                    openedfiles.Add(file);

                foreach (string line in File.ReadAllLines(file,Encoding.Default))
                {
                    if (line.Contains("="))
                    {
                        string[] vk = line.Split('=');
                        if (htLang.ContainsKey(vk[1].ToLower()))
                            htLang.Remove(vk[1].ToLower());

                        htLang.Add(vk[1].ToLower(), vk[0]);
                    }
                }
            }
            catch { }
        }

        private void reloadLangFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (string k in openedfiles)
            {
                LoadLangFile(k);
            }
        }

        private void toolStripMenuItem4_CheckedChanged(object sender, EventArgs e)
        {
            toolStripMenuItem5.Checked = !toolStripMenuItem4.Checked;
        }

        private void toolStripMenuItem5_CheckedChanged(object sender, EventArgs e)
        {
            toolStripMenuItem4.Checked = !toolStripMenuItem5.Checked;
        }

        private void replacerToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            toolStripMenuItem4.Checked = replacerToolStripMenuItem.Checked;
            toolStripMenuItem5.Checked = !replacerToolStripMenuItem.Checked;
        }

	}
}
