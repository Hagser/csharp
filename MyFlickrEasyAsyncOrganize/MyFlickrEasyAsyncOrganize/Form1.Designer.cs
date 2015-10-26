namespace MyFlickrEasyAsyncOrganize
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
            this.bgetTokens = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.bgetAuth = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.bgetAcTok = new System.Windows.Forms.Button();
            this.bgetPhotosets = new System.Windows.Forms.Button();
            this.bgetPhotos = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.bRetry = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.baddToSet = new System.Windows.Forms.Button();
            this.btagPhotos = new System.Windows.Forms.Button();
            this.pBSet = new System.Windows.Forms.ProgressBar();
            this.pBMove = new System.Windows.Forms.ProgressBar();
            this.bremovePhotos = new System.Windows.Forms.Button();
            this.bremovePhotosets = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.comboTo = new System.Windows.Forms.ComboBox();
            this.chkListFrom = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectInvertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bMovePhotos = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.flpTagCloud = new System.Windows.Forms.FlowLayoutPanel();
            this.timerTagCloud = new System.Windows.Forms.Timer(this.components);
            this.chkAutoTag = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnDeletePhotos = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // bgetTokens
            // 
            this.bgetTokens.Location = new System.Drawing.Point(6, 19);
            this.bgetTokens.Name = "bgetTokens";
            this.bgetTokens.Size = new System.Drawing.Size(75, 23);
            this.bgetTokens.TabIndex = 0;
            this.bgetTokens.Text = "getTokens";
            this.bgetTokens.UseVisualStyleBackColor = true;
            this.bgetTokens.Click += new System.EventHandler(this.bgetTokens_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // bgetAuth
            // 
            this.bgetAuth.Enabled = false;
            this.bgetAuth.Location = new System.Drawing.Point(6, 6);
            this.bgetAuth.Name = "bgetAuth";
            this.bgetAuth.Size = new System.Drawing.Size(75, 23);
            this.bgetAuth.TabIndex = 3;
            this.bgetAuth.Text = "getAuth";
            this.bgetAuth.UseVisualStyleBackColor = true;
            this.bgetAuth.Click += new System.EventHandler(this.bgetAuth_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(8, 35);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(716, 365);
            this.webBrowser1.TabIndex = 4;
            // 
            // bgetAcTok
            // 
            this.bgetAcTok.Enabled = false;
            this.bgetAcTok.Location = new System.Drawing.Point(87, 6);
            this.bgetAcTok.Name = "bgetAcTok";
            this.bgetAcTok.Size = new System.Drawing.Size(75, 23);
            this.bgetAcTok.TabIndex = 5;
            this.bgetAcTok.Text = "getAcTok";
            this.bgetAcTok.UseVisualStyleBackColor = true;
            this.bgetAcTok.Click += new System.EventHandler(this.bgetAcTok_Click);
            // 
            // bgetPhotosets
            // 
            this.bgetPhotosets.Enabled = false;
            this.bgetPhotosets.Location = new System.Drawing.Point(6, 6);
            this.bgetPhotosets.Name = "bgetPhotosets";
            this.bgetPhotosets.Size = new System.Drawing.Size(82, 23);
            this.bgetPhotosets.TabIndex = 6;
            this.bgetPhotosets.Text = "getPhotosets";
            this.bgetPhotosets.UseVisualStyleBackColor = true;
            this.bgetPhotosets.Click += new System.EventHandler(this.bgetPhotosets_Click);
            // 
            // bgetPhotos
            // 
            this.bgetPhotos.Enabled = false;
            this.bgetPhotos.Location = new System.Drawing.Point(94, 6);
            this.bgetPhotos.Name = "bgetPhotos";
            this.bgetPhotos.Size = new System.Drawing.Size(75, 23);
            this.bgetPhotos.TabIndex = 7;
            this.bgetPhotos.Text = "getPhotos";
            this.bgetPhotos.UseVisualStyleBackColor = true;
            this.bgetPhotos.Click += new System.EventHandler(this.bgetPhotos_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(740, 434);
            this.tabControl1.TabIndex = 8;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.bRetry);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.bgetTokens);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(732, 408);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Token";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // bRetry
            // 
            this.bRetry.Location = new System.Drawing.Point(87, 19);
            this.bRetry.Name = "bRetry";
            this.bRetry.Size = new System.Drawing.Size(75, 23);
            this.bRetry.TabIndex = 2;
            this.bRetry.Text = "retry";
            this.bRetry.UseVisualStyleBackColor = true;
            this.bRetry.Click += new System.EventHandler(this.bRetry_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.bgetAuth);
            this.tabPage2.Controls.Add(this.webBrowser1);
            this.tabPage2.Controls.Add(this.bgetAcTok);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(732, 408);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Auth";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnDeletePhotos);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.chkAutoTag);
            this.tabPage3.Controls.Add(this.baddToSet);
            this.tabPage3.Controls.Add(this.btagPhotos);
            this.tabPage3.Controls.Add(this.pBSet);
            this.tabPage3.Controls.Add(this.pBMove);
            this.tabPage3.Controls.Add(this.bremovePhotos);
            this.tabPage3.Controls.Add(this.bremovePhotosets);
            this.tabPage3.Controls.Add(this.pictureBox1);
            this.tabPage3.Controls.Add(this.dataGridView1);
            this.tabPage3.Controls.Add(this.comboTo);
            this.tabPage3.Controls.Add(this.chkListFrom);
            this.tabPage3.Controls.Add(this.bMovePhotos);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.bgetPhotosets);
            this.tabPage3.Controls.Add(this.bgetPhotos);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(732, 408);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Sets & Photos";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // baddToSet
            // 
            this.baddToSet.Location = new System.Drawing.Point(407, 47);
            this.baddToSet.Name = "baddToSet";
            this.baddToSet.Size = new System.Drawing.Size(75, 23);
            this.baddToSet.TabIndex = 22;
            this.baddToSet.Text = "addToSet";
            this.baddToSet.UseVisualStyleBackColor = true;
            this.baddToSet.Click += new System.EventHandler(this.baddToSet_Click);
            // 
            // btagPhotos
            // 
            this.btagPhotos.Location = new System.Drawing.Point(407, 76);
            this.btagPhotos.Name = "btagPhotos";
            this.btagPhotos.Size = new System.Drawing.Size(75, 23);
            this.btagPhotos.TabIndex = 21;
            this.btagPhotos.Text = "tagPhotos";
            this.btagPhotos.UseVisualStyleBackColor = true;
            this.btagPhotos.Click += new System.EventHandler(this.btagPhotos_Click);
            // 
            // pBSet
            // 
            this.pBSet.Location = new System.Drawing.Point(175, 6);
            this.pBSet.Name = "pBSet";
            this.pBSet.Size = new System.Drawing.Size(203, 23);
            this.pBSet.TabIndex = 20;
            // 
            // pBMove
            // 
            this.pBMove.Location = new System.Drawing.Point(384, 6);
            this.pBMove.Name = "pBMove";
            this.pBMove.Size = new System.Drawing.Size(203, 23);
            this.pBMove.TabIndex = 19;
            // 
            // bremovePhotos
            // 
            this.bremovePhotos.Location = new System.Drawing.Point(488, 76);
            this.bremovePhotos.Name = "bremovePhotos";
            this.bremovePhotos.Size = new System.Drawing.Size(101, 23);
            this.bremovePhotos.TabIndex = 18;
            this.bremovePhotos.Text = "removePhotos";
            this.bremovePhotos.UseVisualStyleBackColor = true;
            this.bremovePhotos.Click += new System.EventHandler(this.bremovePhotos_Click);
            // 
            // bremovePhotosets
            // 
            this.bremovePhotosets.Location = new System.Drawing.Point(488, 105);
            this.bremovePhotosets.Name = "bremovePhotosets";
            this.bremovePhotosets.Size = new System.Drawing.Size(101, 23);
            this.bremovePhotosets.TabIndex = 17;
            this.bremovePhotosets.Text = "removePhotosets";
            this.bremovePhotosets.UseVisualStyleBackColor = true;
            this.bremovePhotosets.Click += new System.EventHandler(this.bremovePhotosets_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(595, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(129, 117);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(218, 134);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(506, 260);
            this.dataGridView1.TabIndex = 15;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // comboTo
            // 
            this.comboTo.FormattingEnabled = true;
            this.comboTo.Location = new System.Drawing.Point(241, 105);
            this.comboTo.Name = "comboTo";
            this.comboTo.Size = new System.Drawing.Size(160, 21);
            this.comboTo.TabIndex = 14;
            // 
            // chkListFrom
            // 
            this.chkListFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.chkListFrom.CheckOnClick = true;
            this.chkListFrom.ContextMenuStrip = this.contextMenuStrip1;
            this.chkListFrom.FormattingEnabled = true;
            this.chkListFrom.Location = new System.Drawing.Point(44, 105);
            this.chkListFrom.Name = "chkListFrom";
            this.chkListFrom.Size = new System.Drawing.Size(165, 289);
            this.chkListFrom.TabIndex = 13;
            this.chkListFrom.SelectedIndexChanged += new System.EventHandler(this.chkListFrom_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem,
            this.selectInvertToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(139, 70);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.selectAllToolStripMenuItem.Text = "Select &All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            this.selectNoneToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.selectNoneToolStripMenuItem.Text = "Select &None";
            this.selectNoneToolStripMenuItem.Click += new System.EventHandler(this.selectNoneToolStripMenuItem_Click);
            // 
            // selectInvertToolStripMenuItem
            // 
            this.selectInvertToolStripMenuItem.Name = "selectInvertToolStripMenuItem";
            this.selectInvertToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.selectInvertToolStripMenuItem.Text = "Select &Invert";
            this.selectInvertToolStripMenuItem.Click += new System.EventHandler(this.selectInvertToolStripMenuItem_Click);
            // 
            // bMovePhotos
            // 
            this.bMovePhotos.Location = new System.Drawing.Point(407, 105);
            this.bMovePhotos.Name = "bMovePhotos";
            this.bMovePhotos.Size = new System.Drawing.Size(75, 23);
            this.bMovePhotos.TabIndex = 12;
            this.bMovePhotos.Text = "movePhotos";
            this.bMovePhotos.UseVisualStyleBackColor = true;
            this.bMovePhotos.Click += new System.EventHandler(this.bMovePhotos_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(215, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "To";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "From";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Photo:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Photoset:";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.flpTagCloud);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(732, 408);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Tagcloud";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // flpTagCloud
            // 
            this.flpTagCloud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTagCloud.Location = new System.Drawing.Point(3, 3);
            this.flpTagCloud.Name = "flpTagCloud";
            this.flpTagCloud.Size = new System.Drawing.Size(726, 402);
            this.flpTagCloud.TabIndex = 0;
            // 
            // timerTagCloud
            // 
            this.timerTagCloud.Enabled = true;
            this.timerTagCloud.Interval = 600000;
            this.timerTagCloud.Tick += new System.EventHandler(this.timerTagCloud_Tick);
            // 
            // chkAutoTag
            // 
            this.chkAutoTag.AutoSize = true;
            this.chkAutoTag.Location = new System.Drawing.Point(6, 82);
            this.chkAutoTag.Name = "chkAutoTag";
            this.chkAutoTag.Size = new System.Drawing.Size(63, 17);
            this.chkAutoTag.TabIndex = 23;
            this.chkAutoTag.Text = "Autotag";
            this.chkAutoTag.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Time left:";
            // 
            // btnDeletePhotos
            // 
            this.btnDeletePhotos.Location = new System.Drawing.Point(488, 47);
            this.btnDeletePhotos.Name = "btnDeletePhotos";
            this.btnDeletePhotos.Size = new System.Drawing.Size(101, 23);
            this.btnDeletePhotos.TabIndex = 25;
            this.btnDeletePhotos.Text = "deletePhotos";
            this.btnDeletePhotos.UseVisualStyleBackColor = true;
            this.btnDeletePhotos.Click += new System.EventHandler(this.btnDeletePhotos_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 434);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bgetTokens;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bgetAuth;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button bgetAcTok;
        private System.Windows.Forms.Button bgetPhotosets;
        private System.Windows.Forms.Button bgetPhotos;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboTo;
        private System.Windows.Forms.CheckedListBox chkListFrom;
        private System.Windows.Forms.Button bMovePhotos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button bRetry;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectInvertToolStripMenuItem;
        private System.Windows.Forms.Button bremovePhotosets;
        private System.Windows.Forms.Button bremovePhotos;
        private System.Windows.Forms.ProgressBar pBMove;
        private System.Windows.Forms.ProgressBar pBSet;
        private System.Windows.Forms.Button btagPhotos;
        private System.Windows.Forms.Button baddToSet;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.FlowLayoutPanel flpTagCloud;
        private System.Windows.Forms.Timer timerTagCloud;
        private System.Windows.Forms.CheckBox chkAutoTag;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnDeletePhotos;
    }
}

