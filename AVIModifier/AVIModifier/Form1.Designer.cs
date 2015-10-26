namespace AVIModifier
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnCreate = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblFrameRate = new System.Windows.Forms.Label();
            this.txtFrameRate = new System.Windows.Forms.TextBox();
            //this.mpVideo = new AxWMPLib.AxWindowsMediaPlayer();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.lblSize = new System.Windows.Forms.Label();
            this.btnExtract = new System.Windows.Forms.Button();
            this.btnOpenAvi = new System.Windows.Forms.Button();
            this.chkCompress = new System.Windows.Forms.CheckBox();
            this.pBox = new System.Windows.Forms.PictureBox();
            this.btnPlay = new System.Windows.Forms.Button();
            this.hSB = new System.Windows.Forms.HScrollBar();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.btnFolder = new System.Windows.Forms.Button();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSF = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSkip = new System.Windows.Forms.TextBox();
            //((System.ComponentModel.ISupportInitialize)(this.mpVideo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(12, 210);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "Create AVI";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 405);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(487, 18);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 1;
            // 
            // lblFrameRate
            // 
            this.lblFrameRate.AutoSize = true;
            this.lblFrameRate.Location = new System.Drawing.Point(9, 10);
            this.lblFrameRate.Name = "lblFrameRate";
            this.lblFrameRate.Size = new System.Drawing.Size(54, 13);
            this.lblFrameRate.TabIndex = 2;
            this.lblFrameRate.Text = "Framerate";
            // 
            // txtFrameRate
            // 
            this.txtFrameRate.Location = new System.Drawing.Point(12, 26);
            this.txtFrameRate.Name = "txtFrameRate";
            this.txtFrameRate.Size = new System.Drawing.Size(51, 20);
            this.txtFrameRate.TabIndex = 3;
            this.txtFrameRate.Text = "30";
            // 
            // mpVideo
            // 
			/*
            this.mpVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mpVideo.Enabled = true;
            this.mpVideo.Location = new System.Drawing.Point(93, 12);
            this.mpVideo.Name = "mpVideo";
            this.mpVideo.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("mpVideo.OcxState")));
            this.mpVideo.Size = new System.Drawing.Size(406, 345);
            this.mpVideo.TabIndex = 4;
			*/
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(12, 66);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(51, 20);
            this.txtSize.TabIndex = 6;
            this.txtSize.Text = "20";
            this.txtSize.TextChanged += new System.EventHandler(this.txtSize_TextChanged);
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(9, 50);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(44, 13);
            this.lblSize.TabIndex = 5;
            this.lblSize.Text = "Size (%)";
            // 
            // btnExtract
            // 
            this.btnExtract.Location = new System.Drawing.Point(12, 268);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(75, 23);
            this.btnExtract.TabIndex = 7;
            this.btnExtract.Text = "Extract AVI";
            this.btnExtract.UseVisualStyleBackColor = true;
            this.btnExtract.Click += new System.EventHandler(this.btnExtract_Click);
            // 
            // btnOpenAvi
            // 
            this.btnOpenAvi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenAvi.Location = new System.Drawing.Point(12, 355);
            this.btnOpenAvi.Name = "btnOpenAvi";
            this.btnOpenAvi.Size = new System.Drawing.Size(75, 23);
            this.btnOpenAvi.TabIndex = 8;
            this.btnOpenAvi.Text = "Open AVI";
            this.btnOpenAvi.UseVisualStyleBackColor = true;
            this.btnOpenAvi.Click += new System.EventHandler(this.btnOpenAvi_Click);
            // 
            // chkCompress
            // 
            this.chkCompress.AutoSize = true;
            this.chkCompress.Location = new System.Drawing.Point(12, 132);
            this.chkCompress.Name = "chkCompress";
            this.chkCompress.Size = new System.Drawing.Size(72, 17);
            this.chkCompress.TabIndex = 9;
            this.chkCompress.Text = "Compress";
            this.chkCompress.UseVisualStyleBackColor = true;
            // 
            // pBox
            // 
            this.pBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pBox.Location = new System.Drawing.Point(93, 12);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(406, 345);
            this.pBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pBox.TabIndex = 10;
            this.pBox.TabStop = false;
            this.pBox.Visible = false;
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(12, 297);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(75, 23);
            this.btnPlay.TabIndex = 11;
            this.btnPlay.Text = "Play AVI";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // hSB
            // 
            this.hSB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hSB.Location = new System.Drawing.Point(93, 360);
            this.hSB.Name = "hSB";
            this.hSB.Size = new System.Drawing.Size(406, 18);
            this.hSB.TabIndex = 12;
            this.hSB.ValueChanged += new System.EventHandler(this.hSB_ValueChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(12, 326);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close AVI";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblError
            // 
            this.lblError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblError.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblError.Location = new System.Drawing.Point(12, 381);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(487, 21);
            this.lblError.TabIndex = 14;
            // 
            // btnFolder
            // 
            this.btnFolder.Location = new System.Drawing.Point(12, 239);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(75, 23);
            this.btnFolder.TabIndex = 15;
            this.btnFolder.Text = "Folder AVI";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(12, 106);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(51, 20);
            this.txtWidth.TabIndex = 17;
            this.txtWidth.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Width";
            // 
            // chkSF
            // 
            this.chkSF.AutoSize = true;
            this.chkSF.Location = new System.Drawing.Point(12, 155);
            this.chkSF.Name = "chkSF";
            this.chkSF.Size = new System.Drawing.Size(76, 17);
            this.chkSF.TabIndex = 18;
            this.chkSF.Text = "Subfolders";
            this.chkSF.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Skip";
            // 
            // txtSkip
            // 
            this.txtSkip.Location = new System.Drawing.Point(12, 189);
            this.txtSkip.Name = "txtSkip";
            this.txtSkip.Size = new System.Drawing.Size(51, 20);
            this.txtSkip.TabIndex = 20;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 435);
            this.Controls.Add(this.txtSkip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkSF);
            this.Controls.Add(this.txtWidth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFolder);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.hSB);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.pBox);
            this.Controls.Add(this.chkCompress);
            this.Controls.Add(this.btnOpenAvi);
            this.Controls.Add(this.btnExtract);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.lblSize);
            //this.Controls.Add(this.mpVideo);
            this.Controls.Add(this.txtFrameRate);
            this.Controls.Add(this.lblFrameRate);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnCreate);
            this.Name = "Form1";
            this.Text = "AVIModifier";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            //((System.ComponentModel.ISupportInitialize)(this.mpVideo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblFrameRate;
        private System.Windows.Forms.TextBox txtFrameRate;
        //private AxWMPLib.AxWindowsMediaPlayer mpVideo;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.Button btnOpenAvi;
        private System.Windows.Forms.CheckBox chkCompress;
        private System.Windows.Forms.PictureBox pBox;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.HScrollBar hSB;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSF;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSkip;
    }
}

