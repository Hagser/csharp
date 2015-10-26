namespace MyPhotoInfo
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pBPhoto = new System.Windows.Forms.PictureBox();
            this.dGVExif = new System.Windows.Forms.DataGridView();
            this.dGVFiles = new System.Windows.Forms.DataGridView();
            this.pBMap = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeViewPhotos = new System.Windows.Forms.TreeView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBPhoto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dGVExif)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dGVFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBMap)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AllowDrop = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.5F));
            this.tableLayoutPanel1.Controls.Add(this.pBPhoto, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.dGVExif, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dGVFiles, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pBMap, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.78439F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 68.21561F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(818, 514);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pBPhoto
            // 
            this.pBPhoto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pBPhoto.Location = new System.Drawing.Point(309, 166);
            this.pBPhoto.Name = "pBPhoto";
            this.pBPhoto.Size = new System.Drawing.Size(506, 345);
            this.pBPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pBPhoto.TabIndex = 3;
            this.pBPhoto.TabStop = false;
            this.pBPhoto.Tag = "Big Photo";
            // 
            // dGVExif
            // 
            this.dGVExif.AllowDrop = true;
            this.dGVExif.AllowUserToAddRows = false;
            this.dGVExif.AllowUserToDeleteRows = false;
            this.dGVExif.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dGVExif.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVExif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGVExif.Location = new System.Drawing.Point(3, 3);
            this.dGVExif.Name = "dGVExif";
            this.dGVExif.Size = new System.Drawing.Size(300, 157);
            this.dGVExif.TabIndex = 0;
            this.dGVExif.Tag = "Exif Info";
            // 
            // dGVFiles
            // 
            this.dGVFiles.AllowDrop = true;
            this.dGVFiles.AllowUserToAddRows = false;
            this.dGVFiles.AllowUserToDeleteRows = false;
            this.dGVFiles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dGVFiles.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dGVFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGVFiles.Location = new System.Drawing.Point(3, 166);
            this.dGVFiles.Name = "dGVFiles";
            this.dGVFiles.Size = new System.Drawing.Size(300, 345);
            this.dGVFiles.TabIndex = 1;
            this.dGVFiles.Tag = "Thumbnails";
            this.dGVFiles.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGVFiles_CellClick);
            // 
            // pBMap
            // 
            this.pBMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pBMap.Location = new System.Drawing.Point(309, 3);
            this.pBMap.Name = "pBMap";
            this.pBMap.Size = new System.Drawing.Size(506, 157);
            this.pBMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pBMap.TabIndex = 2;
            this.pBMap.TabStop = false;
            this.pBMap.Tag = "Map";
            this.pBMap.SizeChanged += new System.EventHandler(this.pBMap_SizeChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(818, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // treeViewPhotos
            // 
            this.treeViewPhotos.AllowDrop = true;
            this.treeViewPhotos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewPhotos.Location = new System.Drawing.Point(0, 24);
            this.treeViewPhotos.Name = "treeViewPhotos";
            this.treeViewPhotos.Size = new System.Drawing.Size(818, 514);
            this.treeViewPhotos.TabIndex = 2;
            this.treeViewPhotos.Tag = "Treeview";
            this.treeViewPhotos.Visible = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "*.jpg";
            this.openFileDialog1.Filter = "Images|*.jpg";
            this.openFileDialog1.InitialDirectory = "r:\\photos\\";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "Choose images.";
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 538);
            this.Controls.Add(this.treeViewPhotos);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "MyPhotoInfo";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pBPhoto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dGVExif)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dGVFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBMap)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dGVExif;
        private System.Windows.Forms.DataGridView dGVFiles;
        private System.Windows.Forms.PictureBox pBPhoto;
        private System.Windows.Forms.PictureBox pBMap;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.TreeView treeViewPhotos;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

