namespace Filesearcher
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkSearchZip = new System.Windows.Forms.CheckBox();
            this.cBoxMusicInfo = new System.Windows.Forms.ComboBox();
            this.txtMusicInfo = new System.Windows.Forms.TextBox();
            this.lblMusicInfo = new System.Windows.Forms.Label();
            this.cBoxExifInfo = new System.Windows.Forms.ComboBox();
            this.txtContains = new System.Windows.Forms.TextBox();
            this.lblContains = new System.Windows.Forms.Label();
            this.txtExifInfo = new System.Windows.Forms.TextBox();
            this.lblExifInfo = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cBoxFolder = new System.Windows.Forms.ComboBox();
            this.btnChooseFolder = new System.Windows.Forms.Button();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkSubfolders = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.txtAParentFolderName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkLB = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDeleteSearch = new System.Windows.Forms.Button();
            this.btnSaveSearch = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.cBoxSavedSearches = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtSearchString = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tSPB = new System.Windows.Forms.ToolStripProgressBar();
            this.tSSLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.tSSLblSearchedFolders = new System.Windows.Forms.ToolStripStatusLabel();
            this.tSSLblInfolder = new System.Windows.Forms.ToolStripStatusLabel();
            this.tSSLblErr = new System.Windows.Forms.ToolStripStatusLabel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.colHName = new System.Windows.Forms.ColumnHeader();
            this.colHParent = new System.Windows.Forms.ColumnHeader();
            this.colHParentName = new System.Windows.Forms.ColumnHeader();
            this.colHSize = new System.Windows.Forms.ColumnHeader();
            this.colHType = new System.Windows.Forms.ColumnHeader();
            this.colHModified = new System.Windows.Forms.ColumnHeader();
            this.colHAccessed = new System.Windows.Forms.ColumnHeader();
            this.colHCreated = new System.Windows.Forms.ColumnHeader();
            this.conMnuFiles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.conMnuFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.statusStrip1);
            this.splitContainer1.Panel2.Controls.Add(this.listView1);
            this.splitContainer1.Size = new System.Drawing.Size(1170, 599);
            this.splitContainer1.SplitterDistance = 245;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkSearchZip);
            this.groupBox3.Controls.Add(this.cBoxMusicInfo);
            this.groupBox3.Controls.Add(this.txtMusicInfo);
            this.groupBox3.Controls.Add(this.lblMusicInfo);
            this.groupBox3.Controls.Add(this.cBoxExifInfo);
            this.groupBox3.Controls.Add(this.txtContains);
            this.groupBox3.Controls.Add(this.lblContains);
            this.groupBox3.Controls.Add(this.txtExifInfo);
            this.groupBox3.Controls.Add(this.lblExifInfo);
            this.groupBox3.Location = new System.Drawing.Point(12, 328);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(223, 161);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Advanced";
            // 
            // chkSearchZip
            // 
            this.chkSearchZip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSearchZip.AutoSize = true;
            this.chkSearchZip.Location = new System.Drawing.Point(6, 19);
            this.chkSearchZip.Name = "chkSearchZip";
            this.chkSearchZip.Size = new System.Drawing.Size(87, 17);
            this.chkSearchZip.TabIndex = 19;
            this.chkSearchZip.Text = "Search in zip";
            this.chkSearchZip.UseVisualStyleBackColor = true;
            this.chkSearchZip.CheckedChanged += new System.EventHandler(this.chkSearchZip_CheckedChanged);
            // 
            // cBoxMusicInfo
            // 
            this.cBoxMusicInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cBoxMusicInfo.Enabled = false;
            this.cBoxMusicInfo.FormattingEnabled = true;
            this.cBoxMusicInfo.Items.AddRange(new object[] {
            "AlbumArtist",
            "AlbumTitle",
            "Author",
            "BeatsPerMinute",
            "Composer",
            "Description",
            "Duration",
            "EncodedBy",
            "EncodingSettings",
            "FileSize",
            "Genre",
            "Is_Protected",
            "IsVBR",
            "Language",
            "Lyrics",
            "Provider",
            "ProviderStyle",
            "Publisher",
            "SourceURL",
            "Title",
            "TrackNumber",
            "Year"});
            this.cBoxMusicInfo.Location = new System.Drawing.Point(6, 96);
            this.cBoxMusicInfo.Name = "cBoxMusicInfo";
            this.cBoxMusicInfo.Size = new System.Drawing.Size(102, 21);
            this.cBoxMusicInfo.TabIndex = 18;
            this.cBoxMusicInfo.SelectedIndexChanged += new System.EventHandler(this.cBoxMusicInfo_SelectedIndexChanged);
            this.cBoxMusicInfo.TextChanged += new System.EventHandler(this.cBoxMusicInfo_TextChanged);
            // 
            // txtMusicInfo
            // 
            this.txtMusicInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtMusicInfo.Enabled = false;
            this.txtMusicInfo.Location = new System.Drawing.Point(114, 96);
            this.txtMusicInfo.Name = "txtMusicInfo";
            this.txtMusicInfo.Size = new System.Drawing.Size(103, 20);
            this.txtMusicInfo.TabIndex = 17;
            this.txtMusicInfo.TextChanged += new System.EventHandler(this.txtMusicInfo_TextChanged);
            // 
            // lblMusicInfo
            // 
            this.lblMusicInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMusicInfo.AutoSize = true;
            this.lblMusicInfo.Enabled = false;
            this.lblMusicInfo.Location = new System.Drawing.Point(3, 80);
            this.lblMusicInfo.Name = "lblMusicInfo";
            this.lblMusicInfo.Size = new System.Drawing.Size(55, 13);
            this.lblMusicInfo.TabIndex = 16;
            this.lblMusicInfo.Text = "Music info";
            // 
            // cBoxExifInfo
            // 
            this.cBoxExifInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cBoxExifInfo.Enabled = false;
            this.cBoxExifInfo.FormattingEnabled = true;
            this.cBoxExifInfo.Items.AddRange(new object[] {
            "Aperture Value",
            "Chrominance Table",
            "Color Space",
            "Components Configuration",
            "Compressed Bits Per Pixel",
            "Custom Rendered",
            "Date and time of digital data generation",
            "Date and time of original data generation",
            "Digital Zoom Ratio",
            "Exif Version",
            "Exposure Bias Value",
            "Exposure Mode",
            "Exposure Program",
            "Exposure Time",
            "F Number",
            "File change date and time",
            "File Source",
            "Flash",
            "Flashpix Version",
            "Focal Length",
            "Focal Plane Resolution Unit",
            "Focal Plane X Resolution",
            "Focal Plane Y Resolution",
            "Image title",
            "Interoperability Index",
            "ISO Speed Ratings",
            "JPEG Interchange Format",
            "JPEG Interchange Format Length",
            "Light Source",
            "Luminance Table",
            "Make",
            "Maker Note",
            "Max Aperture Value",
            "Metering Mode",
            "Model",
            "Orientation",
            "Pixel X Dimension",
            "Pixel Y Dimension",
            "Resolution Unit",
            "Scene Capture Type",
            "Scene Type",
            "Sensing Method",
            "Shutter Speed Value",
            "Thumbnail Compression",
            "Thumbnail Data",
            "Thumbnail Date Time",
            "Thumbnail Equip Make",
            "Thumbnail Equip Model",
            "Thumbnail Orientation",
            "Thumbnail Resolution Unit",
            "Thumbnail Resolution X",
            "Thumbnail Resolution Y",
            "User comments",
            "White Balance",
            "X Resolution",
            "Y Resolution",
            "YCbCr Positioning"});
            this.cBoxExifInfo.Location = new System.Drawing.Point(6, 56);
            this.cBoxExifInfo.Name = "cBoxExifInfo";
            this.cBoxExifInfo.Size = new System.Drawing.Size(102, 21);
            this.cBoxExifInfo.TabIndex = 15;
            this.cBoxExifInfo.TextChanged += new System.EventHandler(this.cBoxExifInfo_TextChanged);
            // 
            // txtContains
            // 
            this.txtContains.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtContains.Enabled = false;
            this.txtContains.Location = new System.Drawing.Point(6, 135);
            this.txtContains.Name = "txtContains";
            this.txtContains.Size = new System.Drawing.Size(211, 20);
            this.txtContains.TabIndex = 14;
            this.txtContains.TextChanged += new System.EventHandler(this.txtContains_TextChanged);
            // 
            // lblContains
            // 
            this.lblContains.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblContains.AutoSize = true;
            this.lblContains.Enabled = false;
            this.lblContains.Location = new System.Drawing.Point(3, 119);
            this.lblContains.Name = "lblContains";
            this.lblContains.Size = new System.Drawing.Size(48, 13);
            this.lblContains.TabIndex = 13;
            this.lblContains.Text = "Contains";
            // 
            // txtExifInfo
            // 
            this.txtExifInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtExifInfo.Enabled = false;
            this.txtExifInfo.Location = new System.Drawing.Point(114, 56);
            this.txtExifInfo.Name = "txtExifInfo";
            this.txtExifInfo.Size = new System.Drawing.Size(103, 20);
            this.txtExifInfo.TabIndex = 12;
            this.txtExifInfo.TextChanged += new System.EventHandler(this.txtExifInfo_TextChanged);
            // 
            // lblExifInfo
            // 
            this.lblExifInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblExifInfo.AutoSize = true;
            this.lblExifInfo.Enabled = false;
            this.lblExifInfo.Location = new System.Drawing.Point(3, 40);
            this.lblExifInfo.Name = "lblExifInfo";
            this.lblExifInfo.Size = new System.Drawing.Size(44, 13);
            this.lblExifInfo.TabIndex = 11;
            this.lblExifInfo.Text = "Exif info";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cBoxFolder);
            this.groupBox2.Controls.Add(this.btnChooseFolder);
            this.groupBox2.Controls.Add(this.txtDate);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.chkSubfolders);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtSize);
            this.groupBox2.Controls.Add(this.txtAParentFolderName);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.chkLB);
            this.groupBox2.Location = new System.Drawing.Point(12, 138);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(223, 184);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings";
            // 
            // cBoxFolder
            // 
            this.cBoxFolder.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cBoxFolder.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.cBoxFolder.FormattingEnabled = true;
            this.cBoxFolder.Location = new System.Drawing.Point(6, 157);
            this.cBoxFolder.Name = "cBoxFolder";
            this.cBoxFolder.Size = new System.Drawing.Size(180, 21);
            this.cBoxFolder.TabIndex = 11;
            this.cBoxFolder.TextChanged += new System.EventHandler(this.cBoxFolder_TextChanged);
            // 
            // btnChooseFolder
            // 
            this.btnChooseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnChooseFolder.Location = new System.Drawing.Point(192, 158);
            this.btnChooseFolder.Name = "btnChooseFolder";
            this.btnChooseFolder.Size = new System.Drawing.Size(22, 20);
            this.btnChooseFolder.TabIndex = 9;
            this.btnChooseFolder.Text = "...";
            this.btnChooseFolder.UseVisualStyleBackColor = true;
            this.btnChooseFolder.Click += new System.EventHandler(this.btnChooseFolder_Click);
            // 
            // txtDate
            // 
            this.txtDate.Location = new System.Drawing.Point(101, 73);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(116, 20);
            this.txtDate.TabIndex = 8;
            this.txtDate.TextChanged += new System.EventHandler(this.txtDate_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(98, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Date";
            // 
            // chkSubfolders
            // 
            this.chkSubfolders.AutoSize = true;
            this.chkSubfolders.Checked = true;
            this.chkSubfolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSubfolders.Location = new System.Drawing.Point(101, 135);
            this.chkSubfolders.Name = "chkSubfolders";
            this.chkSubfolders.Size = new System.Drawing.Size(111, 17);
            this.chkSubfolders.TabIndex = 6;
            this.chkSubfolders.Text = "Search subfolders";
            this.chkSubfolders.UseVisualStyleBackColor = true;
            this.chkSubfolders.CheckedChanged += new System.EventHandler(this.chkSubfolders_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "F&older(s):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(190, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "kB";
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(101, 32);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(83, 20);
            this.txtSize.TabIndex = 3;
            this.txtSize.TextChanged += new System.EventHandler(this.txtSize_TextChanged);
            // 
            // txtAParentFolderName
            // 
            this.txtAParentFolderName.Location = new System.Drawing.Point(101, 112);
            this.txtAParentFolderName.Name = "txtAParentFolderName";
            this.txtAParentFolderName.Size = new System.Drawing.Size(116, 20);
            this.txtAParentFolderName.TabIndex = 10;
            this.txtAParentFolderName.TextChanged += new System.EventHandler(this.txtAParentFolderName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(100, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Size";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(98, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Parentfoldername";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Filetypes";
            // 
            // chkLB
            // 
            this.chkLB.CheckOnClick = true;
            this.chkLB.FormattingEnabled = true;
            this.chkLB.Items.AddRange(new object[] {
            "Folders",
            "Documents",
            "Photos",
            "Music",
            "Movies",
            "Programming",
            "Zipped"});
            this.chkLB.Location = new System.Drawing.Point(6, 32);
            this.chkLB.Name = "chkLB";
            this.chkLB.Size = new System.Drawing.Size(89, 109);
            this.chkLB.TabIndex = 0;
            this.chkLB.SelectedIndexChanged += new System.EventHandler(this.chkLB_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDeleteSearch);
            this.groupBox1.Controls.Add(this.btnSaveSearch);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cBoxSavedSearches);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.txtSearchString);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 120);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search";
            // 
            // btnDeleteSearch
            // 
            this.btnDeleteSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteSearch.Location = new System.Drawing.Point(195, 62);
            this.btnDeleteSearch.Name = "btnDeleteSearch";
            this.btnDeleteSearch.Size = new System.Drawing.Size(22, 20);
            this.btnDeleteSearch.TabIndex = 12;
            this.btnDeleteSearch.Text = "X";
            this.btnDeleteSearch.UseVisualStyleBackColor = true;
            this.btnDeleteSearch.Click += new System.EventHandler(this.btnDeleteSearch_Click);
            // 
            // btnSaveSearch
            // 
            this.btnSaveSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSearch.Location = new System.Drawing.Point(78, 88);
            this.btnSaveSearch.Name = "btnSaveSearch";
            this.btnSaveSearch.Size = new System.Drawing.Size(67, 23);
            this.btnSaveSearch.TabIndex = 13;
            this.btnSaveSearch.Text = "Save";
            this.btnSaveSearch.UseVisualStyleBackColor = true;
            this.btnSaveSearch.Click += new System.EventHandler(this.btnSaveSearch_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Saved searches";
            // 
            // cBoxSavedSearches
            // 
            this.cBoxSavedSearches.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cBoxSavedSearches.FormattingEnabled = true;
            this.cBoxSavedSearches.Location = new System.Drawing.Point(6, 61);
            this.cBoxSavedSearches.Name = "cBoxSavedSearches";
            this.cBoxSavedSearches.Size = new System.Drawing.Size(183, 21);
            this.cBoxSavedSearches.TabIndex = 3;
            this.cBoxSavedSearches.SelectedIndexChanged += new System.EventHandler(this.cBoxSavedSearches_SelectedIndexChanged);
            this.cBoxSavedSearches.TextChanged += new System.EventHandler(this.cBoxSavedSearches_TextChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(150, 88);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(67, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtSearchString
            // 
            this.txtSearchString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchString.Location = new System.Drawing.Point(6, 19);
            this.txtSearchString.Name = "txtSearchString";
            this.txtSearchString.Size = new System.Drawing.Size(211, 20);
            this.txtSearchString.TabIndex = 1;
            this.txtSearchString.TextChanged += new System.EventHandler(this.txtSearchString_TextChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.Location = new System.Drawing.Point(6, 88);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(67, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSPB,
            this.tSSLbl,
            this.tSSLblSearchedFolders,
            this.tSSLblInfolder,
            this.tSSLblErr});
            this.statusStrip1.Location = new System.Drawing.Point(0, 577);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(921, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tSPB
            // 
            this.tSPB.Name = "tSPB";
            this.tSPB.Size = new System.Drawing.Size(100, 16);
            this.tSPB.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.tSPB.Visible = false;
            // 
            // tSSLbl
            // 
            this.tSSLbl.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tSSLbl.Name = "tSSLbl";
            this.tSSLbl.Size = new System.Drawing.Size(0, 17);
            // 
            // tSSLblSearchedFolders
            // 
            this.tSSLblSearchedFolders.Name = "tSSLblSearchedFolders";
            this.tSSLblSearchedFolders.Size = new System.Drawing.Size(0, 17);
            // 
            // tSSLblInfolder
            // 
            this.tSSLblInfolder.Name = "tSSLblInfolder";
            this.tSSLblInfolder.Size = new System.Drawing.Size(0, 17);
            // 
            // tSSLblErr
            // 
            this.tSSLblErr.ForeColor = System.Drawing.Color.Red;
            this.tSSLblErr.Name = "tSSLblErr";
            this.tSSLblErr.Size = new System.Drawing.Size(0, 17);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHName,
            this.colHParent,
            this.colHParentName,
            this.colHSize,
            this.colHType,
            this.colHModified,
            this.colHAccessed,
            this.colHCreated});
            this.listView1.ContextMenuStrip = this.conMnuFiles;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(921, 574);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            // 
            // colHName
            // 
            this.colHName.Tag = System.TypeCode.String;
            this.colHName.Text = "Name";
            this.colHName.Width = 150;
            // 
            // colHParent
            // 
            this.colHParent.Tag = System.TypeCode.String;
            this.colHParent.Text = "Parent";
            this.colHParent.Width = 200;
            // 
            // colHParentName
            // 
            this.colHParentName.Tag = System.TypeCode.String;
            this.colHParentName.Text = "ParentName";
            // 
            // colHSize
            // 
            this.colHSize.Tag = System.TypeCode.Int64;
            this.colHSize.Text = "Size";
            this.colHSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colHSize.Width = 78;
            // 
            // colHType
            // 
            this.colHType.Tag = System.TypeCode.String;
            this.colHType.Text = "Type";
            this.colHType.Width = 37;
            // 
            // colHModified
            // 
            this.colHModified.Tag = System.TypeCode.DateTime;
            this.colHModified.Text = "Modified";
            this.colHModified.Width = 125;
            // 
            // colHAccessed
            // 
            this.colHAccessed.Tag = System.TypeCode.DateTime;
            this.colHAccessed.Text = "Accessed";
            this.colHAccessed.Width = 125;
            // 
            // colHCreated
            // 
            this.colHCreated.Tag = System.TypeCode.DateTime;
            this.colHCreated.Text = "Created";
            this.colHCreated.Width = 125;
            // 
            // conMnuFiles
            // 
            this.conMnuFiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem});
            this.conMnuFiles.Name = "conMnuFiles";
            this.conMnuFiles.Size = new System.Drawing.Size(135, 26);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 599);
            this.Controls.Add(this.splitContainer1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Filesearcher";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.conMnuFiles.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSearchString;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox chkLB;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader colHName;
        private System.Windows.Forms.ColumnHeader colHParent;
        private System.Windows.Forms.ColumnHeader colHSize;
        private System.Windows.Forms.ColumnHeader colHType;
        private System.Windows.Forms.ColumnHeader colHModified;
        private System.Windows.Forms.ColumnHeader colHAccessed;
        private System.Windows.Forms.ColumnHeader colHCreated;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkSubfolders;
        private System.Windows.Forms.Button btnChooseFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtExifInfo;
        private System.Windows.Forms.Label lblExifInfo;
        private System.Windows.Forms.TextBox txtAParentFolderName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tSSLbl;
        private System.Windows.Forms.ColumnHeader colHParentName;
        private System.Windows.Forms.ToolStripStatusLabel tSSLblInfolder;
        private System.Windows.Forms.ContextMenuStrip conMnuFiles;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.TextBox txtContains;
        private System.Windows.Forms.Label lblContains;
        private System.Windows.Forms.ComboBox cBoxExifInfo;
        private System.Windows.Forms.ComboBox cBoxMusicInfo;
        private System.Windows.Forms.TextBox txtMusicInfo;
        private System.Windows.Forms.Label lblMusicInfo;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox chkSearchZip;
        private System.Windows.Forms.ToolStripStatusLabel tSSLblSearchedFolders;
        private System.Windows.Forms.ComboBox cBoxFolder;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cBoxSavedSearches;
        private System.Windows.Forms.Button btnSaveSearch;
        private System.Windows.Forms.ToolStripStatusLabel tSSLblErr;
        private System.Windows.Forms.Button btnDeleteSearch;
        private System.Windows.Forms.ToolStripProgressBar tSPB;
    }
}

