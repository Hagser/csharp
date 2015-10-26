namespace MyScreenCapture
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.videoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.recDevsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(528, 430);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem,
            this.saveFilesToolStripMenuItem,
            this.showImageToolStripMenuItem,
            this.setLocationToolStripMenuItem,
            this.recDevsToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 136);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.CheckOnClick = true;
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.runToolStripMenuItem.Text = "Run";
            this.runToolStripMenuItem.CheckedChanged += new System.EventHandler(this.runToolStripMenuItem_CheckedChanged);
            // 
            // saveFilesToolStripMenuItem
            // 
            this.saveFilesToolStripMenuItem.CheckOnClick = true;
            this.saveFilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.audioToolStripMenuItem,
            this.videoToolStripMenuItem});
            this.saveFilesToolStripMenuItem.Name = "saveFilesToolStripMenuItem";
            this.saveFilesToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.saveFilesToolStripMenuItem.Text = "Save files";
            this.saveFilesToolStripMenuItem.CheckedChanged += new System.EventHandler(this.saveFilesToolStripMenuItem_CheckedChanged);
            // 
            // audioToolStripMenuItem
            // 
            this.audioToolStripMenuItem.CheckOnClick = true;
            this.audioToolStripMenuItem.Name = "audioToolStripMenuItem";
            this.audioToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.audioToolStripMenuItem.Text = "Audio";
            // 
            // videoToolStripMenuItem
            // 
            this.videoToolStripMenuItem.CheckOnClick = true;
            this.videoToolStripMenuItem.Name = "videoToolStripMenuItem";
            this.videoToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.videoToolStripMenuItem.Text = "Video";
            // 
            // showImageToolStripMenuItem
            // 
            this.showImageToolStripMenuItem.CheckOnClick = true;
            this.showImageToolStripMenuItem.Name = "showImageToolStripMenuItem";
            this.showImageToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.showImageToolStripMenuItem.Text = "Show image";
            // 
            // setLocationToolStripMenuItem
            // 
            this.setLocationToolStripMenuItem.CheckOnClick = true;
            this.setLocationToolStripMenuItem.Name = "setLocationToolStripMenuItem";
            this.setLocationToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.setLocationToolStripMenuItem.Text = "Set location";
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // recDevsToolStripMenuItem
            // 
            this.recDevsToolStripMenuItem.Name = "recDevsToolStripMenuItem";
            this.recDevsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.recDevsToolStripMenuItem.Text = "Rec Devs";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 430);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Opacity = 0.5D;
            this.Text = "MyScreenCapture";
            this.Move += new System.EventHandler(this.Form1_Move);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem audioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem videoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recDevsToolStripMenuItem;
    }
}

