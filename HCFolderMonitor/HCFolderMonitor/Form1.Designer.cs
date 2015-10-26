namespace HCFolderMonitor
{
    partial class FolderMonitor
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
            this.fBD = new System.Windows.Forms.FolderBrowserDialog();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.oFD = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MonAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MonNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RunAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.RunNoneToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fBD
            // 
            this.fBD.Description = "Choose a folder";
            this.fBD.ShowNewFolderButton = false;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // flowPanel
            // 
            this.flowPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowPanel.Location = new System.Drawing.Point(0, 24);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new System.Drawing.Size(642, 679);
            this.flowPanel.TabIndex = 0;
            // 
            // oFD
            // 
            this.oFD.Filter = "All Executables|*.exe;*.bat;*.vbs|Program|*.exe|Command file|*.bat|VBscript|*.vbs" +
                "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.actionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(642, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // actionToolStripMenuItem
            // 
            this.actionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.monitorToolStripMenuItem,
            this.runToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.actionToolStripMenuItem.Name = "actionToolStripMenuItem";
            this.actionToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.actionToolStripMenuItem.Text = "Action";
            // 
            // monitorToolStripMenuItem
            // 
            this.monitorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MonAllToolStripMenuItem,
            this.MonNoneToolStripMenuItem});
            this.monitorToolStripMenuItem.Name = "monitorToolStripMenuItem";
            this.monitorToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.monitorToolStripMenuItem.Text = "Monitor";
            // 
            // MonAllToolStripMenuItem
            // 
            this.MonAllToolStripMenuItem.Name = "MonAllToolStripMenuItem";
            this.MonAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.MonAllToolStripMenuItem.Text = "All";
            this.MonAllToolStripMenuItem.Click += new System.EventHandler(this.MonAllToolStripMenuItem_Click);
            // 
            // MonNoneToolStripMenuItem
            // 
            this.MonNoneToolStripMenuItem.Name = "MonNoneToolStripMenuItem";
            this.MonNoneToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.MonNoneToolStripMenuItem.Text = "None";
            this.MonNoneToolStripMenuItem.Click += new System.EventHandler(this.MonNoneToolStripMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RunAllToolStripMenuItem1,
            this.RunNoneToolStripMenuItem1});
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.runToolStripMenuItem.Text = "Run";
            // 
            // RunAllToolStripMenuItem1
            // 
            this.RunAllToolStripMenuItem1.Name = "RunAllToolStripMenuItem1";
            this.RunAllToolStripMenuItem1.Size = new System.Drawing.Size(110, 22);
            this.RunAllToolStripMenuItem1.Text = "All";
            this.RunAllToolStripMenuItem1.Click += new System.EventHandler(this.RunAllToolStripMenuItem1_Click);
            // 
            // RunNoneToolStripMenuItem1
            // 
            this.RunNoneToolStripMenuItem1.Name = "RunNoneToolStripMenuItem1";
            this.RunNoneToolStripMenuItem1.Size = new System.Drawing.Size(110, 22);
            this.RunNoneToolStripMenuItem1.Text = "None";
            this.RunNoneToolStripMenuItem1.Click += new System.EventHandler(this.RunNoneToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // FolderMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 703);
            this.Controls.Add(this.flowPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FolderMonitor";
            this.Text = "FolderMonitor";
            this.Load += new System.EventHandler(this.FolderMonitor_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FolderMonitor_FormClosing);
            this.Resize += new System.EventHandler(this.FolderMonitor_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        void FolderMonitor_Resize(object sender, System.EventArgs e)
        {
            this.Text = this.Width + "x" + this.Height;
            foreach (System.Windows.Forms.FlowLayoutPanel flp in flowPanel.Controls)
            {
                flp.Width = int.Parse(System.Math.Round(((double)this.Width-20) / 2,0).ToString());
            }
        }

        #endregion
        private System.Windows.Forms.OpenFileDialog oFD;
        private System.Windows.Forms.FolderBrowserDialog fBD;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.FlowLayoutPanel flowPanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem actionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monitorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MonAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MonNoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RunAllToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem RunNoneToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;

    }
}

