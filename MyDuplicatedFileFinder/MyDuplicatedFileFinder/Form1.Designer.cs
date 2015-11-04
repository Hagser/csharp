namespace MyDuplicatedFileFinder
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblFilesCount = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideFilenameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideExtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtFindSize = new System.Windows.Forms.TextBox();
            this.lblDirName = new System.Windows.Forms.Label();
            this.txtFindExt = new System.Windows.Forms.TextBox();
            this.lblSelectedSize = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.chkLDrives = new System.Windows.Forms.CheckedListBox();
            this.chkShow = new System.Windows.Forms.CheckBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.chkBrowsed = new System.Windows.Forms.CheckBox();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.selectOneOfAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllButOneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(12, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(12, 41);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Stop";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblFilesCount
            // 
            this.lblFilesCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFilesCount.Location = new System.Drawing.Point(12, 97);
            this.lblFilesCount.Name = "lblFilesCount";
            this.lblFilesCount.Size = new System.Drawing.Size(75, 13);
            this.lblFilesCount.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(93, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(722, 423);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectFolderToolStripMenuItem,
            this.openFileLocationToolStripMenuItem,
            this.toolStripMenuItem1,
            this.deleteToolStripMenuItem,
            this.hideFilesToolStripMenuItem,
            this.hideFilenameToolStripMenuItem,
            this.hideExtToolStripMenuItem,
            this.toolStripMenuItem2,
            this.selectOneOfAllToolStripMenuItem,
            this.selectAllButOneToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(169, 214);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // selectFolderToolStripMenuItem
            // 
            this.selectFolderToolStripMenuItem.Name = "selectFolderToolStripMenuItem";
            this.selectFolderToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.selectFolderToolStripMenuItem.Text = "Select folder";
            // 
            // openFileLocationToolStripMenuItem
            // 
            this.openFileLocationToolStripMenuItem.Name = "openFileLocationToolStripMenuItem";
            this.openFileLocationToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.openFileLocationToolStripMenuItem.Text = "Open file location";
            this.openFileLocationToolStripMenuItem.Click += new System.EventHandler(this.openFileLocationToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(165, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.deleteToolStripMenuItem.Text = "Delete files";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // hideFilesToolStripMenuItem
            // 
            this.hideFilesToolStripMenuItem.Name = "hideFilesToolStripMenuItem";
            this.hideFilesToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.hideFilesToolStripMenuItem.Text = "Hide files";
            this.hideFilesToolStripMenuItem.Click += new System.EventHandler(this.hideFilesToolStripMenuItem_Click);
            // 
            // hideFilenameToolStripMenuItem
            // 
            this.hideFilenameToolStripMenuItem.Name = "hideFilenameToolStripMenuItem";
            this.hideFilenameToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.hideFilenameToolStripMenuItem.Text = "Hide filename";
            this.hideFilenameToolStripMenuItem.Click += new System.EventHandler(this.hideFilenameToolStripMenuItem_Click);
            // 
            // hideExtToolStripMenuItem
            // 
            this.hideExtToolStripMenuItem.Name = "hideExtToolStripMenuItem";
            this.hideExtToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.hideExtToolStripMenuItem.Text = "Show ext";
            // 
            // txtFindSize
            // 
            this.txtFindSize.Location = new System.Drawing.Point(12, 149);
            this.txtFindSize.Name = "txtFindSize";
            this.txtFindSize.Size = new System.Drawing.Size(75, 20);
            this.txtFindSize.TabIndex = 5;
            this.txtFindSize.Text = "1000000";
            this.txtFindSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblDirName
            // 
            this.lblDirName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDirName.Location = new System.Drawing.Point(12, 115);
            this.lblDirName.Name = "lblDirName";
            this.lblDirName.Size = new System.Drawing.Size(75, 13);
            this.lblDirName.TabIndex = 6;
            // 
            // txtFindExt
            // 
            this.txtFindExt.Location = new System.Drawing.Point(12, 175);
            this.txtFindExt.Name = "txtFindExt";
            this.txtFindExt.Size = new System.Drawing.Size(75, 20);
            this.txtFindExt.TabIndex = 7;
            this.txtFindExt.Text = "*.*";
            // 
            // lblSelectedSize
            // 
            this.lblSelectedSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSelectedSize.Location = new System.Drawing.Point(12, 133);
            this.lblSelectedSize.Name = "lblSelectedSize";
            this.lblSelectedSize.Size = new System.Drawing.Size(75, 13);
            this.lblSelectedSize.TabIndex = 8;
            // 
            // progressBar1
            // 
            this.progressBar1.Enabled = false;
            this.progressBar1.Location = new System.Drawing.Point(12, 201);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(75, 16);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 10;
            this.progressBar1.Visible = false;
            // 
            // chkLDrives
            // 
            this.chkLDrives.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.chkLDrives.CheckOnClick = true;
            this.chkLDrives.FormattingEnabled = true;
            this.chkLDrives.Location = new System.Drawing.Point(12, 223);
            this.chkLDrives.Name = "chkLDrives";
            this.chkLDrives.Size = new System.Drawing.Size(75, 184);
            this.chkLDrives.TabIndex = 11;
            // 
            // chkShow
            // 
            this.chkShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShow.AutoSize = true;
            this.chkShow.Location = new System.Drawing.Point(12, 413);
            this.chkShow.Name = "chkShow";
            this.chkShow.Size = new System.Drawing.Size(77, 17);
            this.chkShow.TabIndex = 12;
            this.chkShow.Text = "show dupl.";
            this.chkShow.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(37, 70);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(50, 23);
            this.btnBrowse.TabIndex = 13;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // chkBrowsed
            // 
            this.chkBrowsed.AutoSize = true;
            this.chkBrowsed.Location = new System.Drawing.Point(16, 75);
            this.chkBrowsed.Name = "chkBrowsed";
            this.chkBrowsed.Size = new System.Drawing.Size(15, 14);
            this.chkBrowsed.TabIndex = 14;
            this.chkBrowsed.UseVisualStyleBackColor = true;
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(165, 6);
            // 
            // selectOneOfAllToolStripMenuItem
            // 
            this.selectOneOfAllToolStripMenuItem.Name = "selectOneOfAllToolStripMenuItem";
            this.selectOneOfAllToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.selectOneOfAllToolStripMenuItem.Text = "Select one of all";
            this.selectOneOfAllToolStripMenuItem.Click += new System.EventHandler(this.selectOneOfAllToolStripMenuItem_Click);
            // 
            // selectAllButOneToolStripMenuItem
            // 
            this.selectAllButOneToolStripMenuItem.Name = "selectAllButOneToolStripMenuItem";
            this.selectAllButOneToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.selectAllButOneToolStripMenuItem.Text = "Select all but one";
            this.selectAllButOneToolStripMenuItem.Click += new System.EventHandler(this.selectAllButOneToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 447);
            this.Controls.Add(this.chkBrowsed);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.chkShow);
            this.Controls.Add(this.chkLDrives);
            this.Controls.Add(this.lblSelectedSize);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.txtFindExt);
            this.Controls.Add(this.lblDirName);
            this.Controls.Add(this.txtFindSize);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblFilesCount);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSearch);
            this.Name = "Form1";
            this.Text = "MyDuplicatedFileFinder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblFilesCount;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtFindSize;
        private System.Windows.Forms.Label lblDirName;
        private System.Windows.Forms.TextBox txtFindExt;
        private System.Windows.Forms.Label lblSelectedSize;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selectFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ToolStripMenuItem hideFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileLocationToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox chkLDrives;
        private System.Windows.Forms.ToolStripMenuItem hideFilenameToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkShow;
        private System.Windows.Forms.ToolStripMenuItem hideExtToolStripMenuItem;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.CheckBox chkBrowsed;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem selectOneOfAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllButOneToolStripMenuItem;
    }
}

