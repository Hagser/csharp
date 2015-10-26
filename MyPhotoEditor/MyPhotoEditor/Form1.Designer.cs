namespace MyPhotoEditor
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cWToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cCWToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripRotate = new System.Windows.Forms.ToolStripTextBox();
            this.resizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripWidth = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripHeight = new System.Windows.Forms.ToolStripTextBox();
            this.uniformToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cropToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.cWAlltoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ccWAlltoolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripRotateAll = new System.Windows.Forms.ToolStripTextBox();
            this.ResizeAlltoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripWidthAll = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripHeightAll = new System.Windows.Forms.ToolStripTextBox();
            this.uniformAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.halfSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.rotateToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cWToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cCWToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cropAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDrop = true;
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(938, 590);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabControl1.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabControl1_DragDrop);
            this.tabControl1.DragOver += new System.Windows.Forms.DragEventHandler(this.tabControl1_DragOver);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolStripMenuItem1,
            this.halfSizeToolStripMenuItem,
            this.cropAllToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(938, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAllToolStripMenuItem,
            this.toolStripMenuItem3,
            this.closeToolStripMenuItem,
            this.closeAllToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.saveAllToolStripMenuItem.Text = "Save All";
            this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(117, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.closeAllToolStripMenuItem.Text = "Close All";
            this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.closeAllToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rotateToolStripMenuItem,
            this.resizeToolStripMenuItem,
            this.cropToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // rotateToolStripMenuItem
            // 
            this.rotateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cWToolStripMenuItem,
            this.cCWToolStripMenuItem,
            this.toolStripRotate});
            this.rotateToolStripMenuItem.Name = "rotateToolStripMenuItem";
            this.rotateToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.rotateToolStripMenuItem.Text = "Rotate";
            // 
            // cWToolStripMenuItem
            // 
            this.cWToolStripMenuItem.Name = "cWToolStripMenuItem";
            this.cWToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.cWToolStripMenuItem.Text = "90 CW";
            this.cWToolStripMenuItem.Click += new System.EventHandler(this.cWToolStripMenuItem_Click);
            // 
            // cCWToolStripMenuItem
            // 
            this.cCWToolStripMenuItem.Name = "cCWToolStripMenuItem";
            this.cCWToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.cCWToolStripMenuItem.Text = "90 CCW";
            this.cCWToolStripMenuItem.Click += new System.EventHandler(this.cCWToolStripMenuItem_Click);
            // 
            // toolStripRotate
            // 
            this.toolStripRotate.Name = "toolStripRotate";
            this.toolStripRotate.Size = new System.Drawing.Size(100, 23);
            this.toolStripRotate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripRotate_KeyUp);
            // 
            // resizeToolStripMenuItem
            // 
            this.resizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripWidth,
            this.toolStripHeight,
            this.uniformToolStripMenuItem});
            this.resizeToolStripMenuItem.Name = "resizeToolStripMenuItem";
            this.resizeToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.resizeToolStripMenuItem.Text = "Resize";
            this.resizeToolStripMenuItem.DropDownOpening += new System.EventHandler(this.resizeToolStripMenuItem_DropDownOpening);
            // 
            // toolStripWidth
            // 
            this.toolStripWidth.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripWidth.AutoToolTip = true;
            this.toolStripWidth.Name = "toolStripWidth";
            this.toolStripWidth.Size = new System.Drawing.Size(100, 23);
            this.toolStripWidth.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolStripWidth.ToolTipText = "Width";
            this.toolStripWidth.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripWidth_KeyUp);
            // 
            // toolStripHeight
            // 
            this.toolStripHeight.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripHeight.AutoToolTip = true;
            this.toolStripHeight.Name = "toolStripHeight";
            this.toolStripHeight.Size = new System.Drawing.Size(100, 23);
            this.toolStripHeight.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolStripHeight.ToolTipText = "Height";
            this.toolStripHeight.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripHeight_KeyUp);
            // 
            // uniformToolStripMenuItem
            // 
            this.uniformToolStripMenuItem.Checked = true;
            this.uniformToolStripMenuItem.CheckOnClick = true;
            this.uniformToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uniformToolStripMenuItem.Name = "uniformToolStripMenuItem";
            this.uniformToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.uniformToolStripMenuItem.Text = "Uniform";
            // 
            // cropToolStripMenuItem
            // 
            this.cropToolStripMenuItem.Name = "cropToolStripMenuItem";
            this.cropToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.cropToolStripMenuItem.Text = "Crop";
            this.cropToolStripMenuItem.Click += new System.EventHandler(this.cropToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.ResizeAlltoolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(56, 20);
            this.toolStripMenuItem1.Text = "Edit All";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cWAlltoolStripMenuItem,
            this.ccWAlltoolStripMenuItem4,
            this.toolStripRotateAll});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(108, 22);
            this.toolStripMenuItem2.Text = "Rotate";
            // 
            // cWAlltoolStripMenuItem
            // 
            this.cWAlltoolStripMenuItem.Name = "cWAlltoolStripMenuItem";
            this.cWAlltoolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.cWAlltoolStripMenuItem.Text = "90 CW";
            this.cWAlltoolStripMenuItem.Click += new System.EventHandler(this.cWAlltoolStripMenuItem_Click);
            // 
            // ccWAlltoolStripMenuItem4
            // 
            this.ccWAlltoolStripMenuItem4.Name = "ccWAlltoolStripMenuItem4";
            this.ccWAlltoolStripMenuItem4.Size = new System.Drawing.Size(160, 22);
            this.ccWAlltoolStripMenuItem4.Text = "90 CCW";
            this.ccWAlltoolStripMenuItem4.Click += new System.EventHandler(this.ccWAlltoolStripMenuItem4_Click);
            // 
            // toolStripRotateAll
            // 
            this.toolStripRotateAll.Name = "toolStripRotateAll";
            this.toolStripRotateAll.Size = new System.Drawing.Size(100, 23);
            this.toolStripRotateAll.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripRotateAll_KeyUp);
            // 
            // ResizeAlltoolStripMenuItem
            // 
            this.ResizeAlltoolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripWidthAll,
            this.toolStripHeightAll,
            this.uniformAllToolStripMenuItem,
            this.toolStripSeparator1,
            this.selectToolStripMenuItem});
            this.ResizeAlltoolStripMenuItem.Name = "ResizeAlltoolStripMenuItem";
            this.ResizeAlltoolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.ResizeAlltoolStripMenuItem.Text = "Resize";
            // 
            // toolStripWidthAll
            // 
            this.toolStripWidthAll.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripWidthAll.AutoToolTip = true;
            this.toolStripWidthAll.Name = "toolStripWidthAll";
            this.toolStripWidthAll.Size = new System.Drawing.Size(100, 23);
            this.toolStripWidthAll.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolStripWidthAll.ToolTipText = "Width";
            this.toolStripWidthAll.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripWidthAll_KeyUp);
            // 
            // toolStripHeightAll
            // 
            this.toolStripHeightAll.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripHeightAll.AutoToolTip = true;
            this.toolStripHeightAll.Name = "toolStripHeightAll";
            this.toolStripHeightAll.Size = new System.Drawing.Size(100, 23);
            this.toolStripHeightAll.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolStripHeightAll.ToolTipText = "Height";
            this.toolStripHeightAll.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripHeightAll_KeyUp);
            // 
            // uniformAllToolStripMenuItem
            // 
            this.uniformAllToolStripMenuItem.Checked = true;
            this.uniformAllToolStripMenuItem.CheckOnClick = true;
            this.uniformAllToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uniformAllToolStripMenuItem.Name = "uniformAllToolStripMenuItem";
            this.uniformAllToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.uniformAllToolStripMenuItem.Text = "Uniform";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // selectToolStripMenuItem
            // 
            this.selectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5,
            this.toolStripMenuItem6});
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            this.selectToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.selectToolStripMenuItem.Text = "Select";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(96, 22);
            this.toolStripMenuItem5.Text = "50%";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(96, 22);
            this.toolStripMenuItem6.Text = "25%";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // halfSizeToolStripMenuItem
            // 
            this.halfSizeToolStripMenuItem.Name = "halfSizeToolStripMenuItem";
            this.halfSizeToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.halfSizeToolStripMenuItem.Text = "HalfSize";
            this.halfSizeToolStripMenuItem.Click += new System.EventHandler(this.halfSizeToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rotateToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(109, 26);
            // 
            // rotateToolStripMenuItem1
            // 
            this.rotateToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cWToolStripMenuItem1,
            this.cCWToolStripMenuItem1});
            this.rotateToolStripMenuItem1.Name = "rotateToolStripMenuItem1";
            this.rotateToolStripMenuItem1.Size = new System.Drawing.Size(108, 22);
            this.rotateToolStripMenuItem1.Text = "Rotate";
            // 
            // cWToolStripMenuItem1
            // 
            this.cWToolStripMenuItem1.Name = "cWToolStripMenuItem1";
            this.cWToolStripMenuItem1.Size = new System.Drawing.Size(116, 22);
            this.cWToolStripMenuItem1.Text = "90 CW";
            this.cWToolStripMenuItem1.Click += new System.EventHandler(this.cWToolStripMenuItem_Click);
            // 
            // cCWToolStripMenuItem1
            // 
            this.cCWToolStripMenuItem1.Name = "cCWToolStripMenuItem1";
            this.cCWToolStripMenuItem1.Size = new System.Drawing.Size(116, 22);
            this.cCWToolStripMenuItem1.Text = "90 CCW";
            this.cCWToolStripMenuItem1.Click += new System.EventHandler(this.cCWToolStripMenuItem_Click);
            // 
            // cropAllToolStripMenuItem
            // 
            this.cropAllToolStripMenuItem.Name = "cropAllToolStripMenuItem";
            this.cropAllToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.cropAllToolStripMenuItem.Text = "CropAll";
            this.cropAllToolStripMenuItem.Click += new System.EventHandler(this.cropAllToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 614);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "MyPhotoEditor";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cWToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cCWToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripWidth;
        private System.Windows.Forms.ToolStripTextBox toolStripHeight;
        private System.Windows.Forms.ToolStripMenuItem uniformToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripRotate;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem cWAlltoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ccWAlltoolStripMenuItem4;
        private System.Windows.Forms.ToolStripTextBox toolStripRotateAll;
        private System.Windows.Forms.ToolStripMenuItem ResizeAlltoolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripWidthAll;
        private System.Windows.Forms.ToolStripTextBox toolStripHeightAll;
        private System.Windows.Forms.ToolStripMenuItem uniformAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem rotateToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cWToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cCWToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem halfSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cropToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cropAllToolStripMenuItem;
    }
}

