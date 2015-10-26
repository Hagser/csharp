namespace MyAniApp
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.animToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.everyNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.everyNPicture = new System.Windows.Forms.ToolStripTextBox();
            this.subfoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.confToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.watchForChangesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.animPerHourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripHourText = new System.Windows.Forms.ToolStripTextBox();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Location = new System.Drawing.Point(0, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(639, 393);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.trackBar1.Location = new System.Drawing.Point(0, 446);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(639, 45);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.Value = 1;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            this.trackBar1.Enter += new System.EventHandler(this.trackBar1_Enter);
            this.trackBar1.Leave += new System.EventHandler(this.trackBar1_Leave);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(0, 422);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(639, 20);
            this.textBox1.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.animToolStripMenuItem,
            this.confToolStripMenuItem,
            this.animPerHourToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(639, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // animToolStripMenuItem
            // 
            this.animToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.everyNToolStripMenuItem,
            this.subfoldersToolStripMenuItem});
            this.animToolStripMenuItem.Name = "animToolStripMenuItem";
            this.animToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.animToolStripMenuItem.Text = "Anim";
            // 
            // everyNToolStripMenuItem
            // 
            this.everyNToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.everyNPicture});
            this.everyNToolStripMenuItem.Name = "everyNToolStripMenuItem";
            this.everyNToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.everyNToolStripMenuItem.Text = "Every n";
            // 
            // everyNPicture
            // 
            this.everyNPicture.Name = "everyNPicture";
            this.everyNPicture.Size = new System.Drawing.Size(100, 23);
            this.everyNPicture.Text = "1";
            // 
            // subfoldersToolStripMenuItem
            // 
            this.subfoldersToolStripMenuItem.CheckOnClick = true;
            this.subfoldersToolStripMenuItem.Name = "subfoldersToolStripMenuItem";
            this.subfoldersToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.subfoldersToolStripMenuItem.Text = "Subfolders";
            this.subfoldersToolStripMenuItem.CheckedChanged += new System.EventHandler(this.subfoldersToolStripMenuItem_CheckedChanged);
            // 
            // confToolStripMenuItem
            // 
            this.confToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.watchForChangesToolStripMenuItem});
            this.confToolStripMenuItem.Name = "confToolStripMenuItem";
            this.confToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.confToolStripMenuItem.Text = "Conf";
            // 
            // watchForChangesToolStripMenuItem
            // 
            this.watchForChangesToolStripMenuItem.CheckOnClick = true;
            this.watchForChangesToolStripMenuItem.Name = "watchForChangesToolStripMenuItem";
            this.watchForChangesToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.watchForChangesToolStripMenuItem.Text = "Watch for changes";
            this.watchForChangesToolStripMenuItem.CheckedChanged += new System.EventHandler(this.watchForChangesToolStripMenuItem_CheckedChanged);
            // 
            // animPerHourToolStripMenuItem
            // 
            this.animPerHourToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripHourText,
            this.startToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.animPerHourToolStripMenuItem.Name = "animPerHourToolStripMenuItem";
            this.animPerHourToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.animPerHourToolStripMenuItem.Text = "Anim per hour";
            // 
            // toolStripHourText
            // 
            this.toolStripHourText.Name = "toolStripHourText";
            this.toolStripHourText.Size = new System.Drawing.Size(100, 23);
            this.toolStripHourText.Text = "12:00";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.NotifyFilter = System.IO.NotifyFilters.FileName;
            this.fileSystemWatcher1.SynchronizingObject = this;
            this.fileSystemWatcher1.Created += new System.IO.FileSystemEventHandler(this.fileSystemWatcher1_Created);
            this.fileSystemWatcher1.Deleted += new System.IO.FileSystemEventHandler(this.fileSystemWatcher1_Deleted);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 491);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "MyAniApp";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem animToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem everyNToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox everyNPicture;
        private System.Windows.Forms.ToolStripMenuItem confToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem watchForChangesToolStripMenuItem;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.ToolStripMenuItem subfoldersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem animPerHourToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripHourText;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
    }
}

