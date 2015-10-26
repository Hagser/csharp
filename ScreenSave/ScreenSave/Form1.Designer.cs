namespace SS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.trackLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topmostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.captureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAVIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tempStoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.memoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripPath = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDropDownInterval = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripStatusInterval = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.intervalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(562, 391);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trackLocationToolStripMenuItem,
            this.topmostToolStripMenuItem,
            this.showToolStripMenuItem,
            this.toolStripSeparator1,
            this.captureToolStripMenuItem,
            this.saveAVIToolStripMenuItem,
            this.tempStoreToolStripMenuItem,
            this.intervalToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 186);
            // 
            // trackLocationToolStripMenuItem
            // 
            this.trackLocationToolStripMenuItem.Checked = true;
            this.trackLocationToolStripMenuItem.CheckOnClick = true;
            this.trackLocationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trackLocationToolStripMenuItem.Name = "trackLocationToolStripMenuItem";
            this.trackLocationToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.trackLocationToolStripMenuItem.Text = "TrackLocation";
            this.trackLocationToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.trackLocationToolStripMenuItem_CheckStateChanged);
            // 
            // topmostToolStripMenuItem
            // 
            this.topmostToolStripMenuItem.Checked = true;
            this.topmostToolStripMenuItem.CheckOnClick = true;
            this.topmostToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.topmostToolStripMenuItem.Name = "topmostToolStripMenuItem";
            this.topmostToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.topmostToolStripMenuItem.Text = "Topmost";
            this.topmostToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.topmostToolStripMenuItem_CheckStateChanged);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.CheckOnClick = true;
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showToolStripMenuItem.Text = "Show";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // captureToolStripMenuItem
            // 
            this.captureToolStripMenuItem.CheckOnClick = true;
            this.captureToolStripMenuItem.Enabled = false;
            this.captureToolStripMenuItem.Name = "captureToolStripMenuItem";
            this.captureToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.captureToolStripMenuItem.Text = "Capture";
            this.captureToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.captureToolStripMenuItem_CheckStateChanged);
            // 
            // saveAVIToolStripMenuItem
            // 
            this.saveAVIToolStripMenuItem.Enabled = false;
            this.saveAVIToolStripMenuItem.Name = "saveAVIToolStripMenuItem";
            this.saveAVIToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAVIToolStripMenuItem.Text = "Save AVI";
            this.saveAVIToolStripMenuItem.Click += new System.EventHandler(this.saveAVIToolStripMenuItem_Click);
            // 
            // tempStoreToolStripMenuItem
            // 
            this.tempStoreToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.memoryToolStripMenuItem,
            this.fileToolStripMenuItem});
            this.tempStoreToolStripMenuItem.Name = "tempStoreToolStripMenuItem";
            this.tempStoreToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.tempStoreToolStripMenuItem.Text = "Temp Store";
            // 
            // memoryToolStripMenuItem
            // 
            this.memoryToolStripMenuItem.Name = "memoryToolStripMenuItem";
            this.memoryToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.memoryToolStripMenuItem.Text = "Memory";
            this.memoryToolStripMenuItem.Click += new System.EventHandler(this.memoryToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Checked = true;
            this.fileToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPath,
            this.toolStripDropDownInterval,
            this.toolStripStatusInterval,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 391);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(562, 22);
            this.statusStrip1.TabIndex = 2;
            // 
            // toolStripPath
            // 
            this.toolStripPath.Name = "toolStripPath";
            this.toolStripPath.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripDropDownInterval
            // 
            this.toolStripDropDownInterval.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownInterval.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownInterval.Image")));
            this.toolStripDropDownInterval.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownInterval.Name = "toolStripDropDownInterval";
            this.toolStripDropDownInterval.Size = new System.Drawing.Size(29, 20);
            this.toolStripDropDownInterval.Text = "toolStripDropDownButton1";
            // 
            // toolStripStatusInterval
            // 
            this.toolStripStatusInterval.Name = "toolStripStatusInterval";
            this.toolStripStatusInterval.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(109, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(109, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // intervalToolStripMenuItem
            // 
            this.intervalToolStripMenuItem.Name = "intervalToolStripMenuItem";
            this.intervalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.intervalToolStripMenuItem.Text = "Interval";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 413);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.Text = "SS";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.LocationChanged += new System.EventHandler(this.Form1_LocationChanged);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem trackLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem topmostToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripPath;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownInterval;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusInterval;
        private System.Windows.Forms.ToolStripMenuItem captureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAVIToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tempStoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem memoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem intervalToolStripMenuItem;
    }
}

