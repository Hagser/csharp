namespace Text2Speech
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
            this.btnSpeak = new System.Windows.Forms.Button();
            this.txtText = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setVoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tellMeTheTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sayWhatIWriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.afterSpaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.afterSentenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tim_time = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tSSLblDetect = new System.Windows.Forms.ToolStripStatusLabel();
            this.lvReq = new System.Windows.Forms.ListView();
            this.colHReq = new System.Windows.Forms.ColumnHeader();
            this.colHReqConf = new System.Windows.Forms.ColumnHeader();
            this.colHReqTime = new System.Windows.Forms.ColumnHeader();
            this.lvSpoken = new System.Windows.Forms.ListView();
            this.colHSpValue = new System.Windows.Forms.ColumnHeader();
            this.colHSpTime = new System.Windows.Forms.ColumnHeader();
            this.readFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSpeak
            // 
            this.btnSpeak.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSpeak.Location = new System.Drawing.Point(0, 380);
            this.btnSpeak.Name = "btnSpeak";
            this.btnSpeak.Size = new System.Drawing.Size(757, 24);
            this.btnSpeak.TabIndex = 0;
            this.btnSpeak.Text = "Speak!";
            this.btnSpeak.UseVisualStyleBackColor = true;
            this.btnSpeak.Click += new System.EventHandler(this.btnSpeak_Click);
            // 
            // txtText
            // 
            this.txtText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtText.ContextMenuStrip = this.contextMenuStrip1;
            this.txtText.Location = new System.Drawing.Point(0, -1);
            this.txtText.Multiline = true;
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(757, 180);
            this.txtText.TabIndex = 1;
            this.txtText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtText_KeyUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setVoiceToolStripMenuItem,
            this.tellMeTheTimeToolStripMenuItem,
            this.saveToFileToolStripMenuItem,
            this.sayWhatIWriteToolStripMenuItem,
            this.readFileToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(154, 136);
            // 
            // setVoiceToolStripMenuItem
            // 
            this.setVoiceToolStripMenuItem.Name = "setVoiceToolStripMenuItem";
            this.setVoiceToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.setVoiceToolStripMenuItem.Text = "Set voice";
            // 
            // tellMeTheTimeToolStripMenuItem
            // 
            this.tellMeTheTimeToolStripMenuItem.CheckOnClick = true;
            this.tellMeTheTimeToolStripMenuItem.Name = "tellMeTheTimeToolStripMenuItem";
            this.tellMeTheTimeToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.tellMeTheTimeToolStripMenuItem.Text = "Tell me the time";
            this.tellMeTheTimeToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.tellMeTheTimeToolStripMenuItem_CheckStateChanged);
            // 
            // saveToFileToolStripMenuItem
            // 
            this.saveToFileToolStripMenuItem.CheckOnClick = true;
            this.saveToFileToolStripMenuItem.Name = "saveToFileToolStripMenuItem";
            this.saveToFileToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.saveToFileToolStripMenuItem.Text = "Save to file";
            this.saveToFileToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.saveToFileToolStripMenuItem_CheckStateChanged);
            this.saveToFileToolStripMenuItem.Click += new System.EventHandler(this.saveToFileToolStripMenuItem_Click);
            // 
            // sayWhatIWriteToolStripMenuItem
            // 
            this.sayWhatIWriteToolStripMenuItem.CheckOnClick = true;
            this.sayWhatIWriteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.afterSpaceToolStripMenuItem,
            this.afterSentenceToolStripMenuItem});
            this.sayWhatIWriteToolStripMenuItem.Name = "sayWhatIWriteToolStripMenuItem";
            this.sayWhatIWriteToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.sayWhatIWriteToolStripMenuItem.Text = "Say what I write";
            this.sayWhatIWriteToolStripMenuItem.Click += new System.EventHandler(this.sayWhatIWriteToolStripMenuItem_Click);
            // 
            // afterSpaceToolStripMenuItem
            // 
            this.afterSpaceToolStripMenuItem.CheckOnClick = true;
            this.afterSpaceToolStripMenuItem.Name = "afterSpaceToolStripMenuItem";
            this.afterSpaceToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.afterSpaceToolStripMenuItem.Text = "After space";
            this.afterSpaceToolStripMenuItem.Click += new System.EventHandler(this.afterSpaceToolStripMenuItem_Click);
            // 
            // afterSentenceToolStripMenuItem
            // 
            this.afterSentenceToolStripMenuItem.CheckOnClick = true;
            this.afterSentenceToolStripMenuItem.Name = "afterSentenceToolStripMenuItem";
            this.afterSentenceToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.afterSentenceToolStripMenuItem.Text = "After sentence";
            this.afterSentenceToolStripMenuItem.Click += new System.EventHandler(this.afterSentenceToolStripMenuItem_Click);
            // 
            // tim_time
            // 
            this.tim_time.Interval = 60000;
            this.tim_time.Tick += new System.EventHandler(this.tim_time_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSSLblDetect});
            this.statusStrip1.Location = new System.Drawing.Point(0, 407);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(757, 22);
            this.statusStrip1.TabIndex = 3;
            // 
            // tSSLblDetect
            // 
            this.tSSLblDetect.Name = "tSSLblDetect";
            this.tSSLblDetect.Size = new System.Drawing.Size(0, 17);
            // 
            // lvReq
            // 
            this.lvReq.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHReq,
            this.colHReqConf,
            this.colHReqTime});
            this.lvReq.GridLines = true;
            this.lvReq.Location = new System.Drawing.Point(0, 185);
            this.lvReq.Name = "lvReq";
            this.lvReq.Size = new System.Drawing.Size(396, 189);
            this.lvReq.TabIndex = 4;
            this.lvReq.UseCompatibleStateImageBehavior = false;
            this.lvReq.View = System.Windows.Forms.View.Details;
            // 
            // colHReq
            // 
            this.colHReq.Text = "Value";
            this.colHReq.Width = 210;
            // 
            // colHReqConf
            // 
            this.colHReqConf.Text = "Confidence";
            this.colHReqConf.Width = 80;
            // 
            // colHReqTime
            // 
            this.colHReqTime.Text = "Time";
            // 
            // lvSpoken
            // 
            this.lvSpoken.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHSpValue,
            this.colHSpTime});
            this.lvSpoken.GridLines = true;
            this.lvSpoken.Location = new System.Drawing.Point(402, 185);
            this.lvSpoken.Name = "lvSpoken";
            this.lvSpoken.Size = new System.Drawing.Size(355, 189);
            this.lvSpoken.TabIndex = 5;
            this.lvSpoken.UseCompatibleStateImageBehavior = false;
            this.lvSpoken.View = System.Windows.Forms.View.Details;
            // 
            // colHSpValue
            // 
            this.colHSpValue.Text = "Value";
            this.colHSpValue.Width = 218;
            // 
            // colHSpTime
            // 
            this.colHSpTime.Text = "Time";
            // 
            // readFileToolStripMenuItem
            // 
            this.readFileToolStripMenuItem.Name = "readFileToolStripMenuItem";
            this.readFileToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.readFileToolStripMenuItem.Text = "Read file";
            this.readFileToolStripMenuItem.Click += new System.EventHandler(this.readFileToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 429);
            this.Controls.Add(this.lvSpoken);
            this.Controls.Add(this.lvReq);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.btnSpeak);
            this.Name = "Form1";
            this.Text = "Text2Speech";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSpeak;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setVoiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tellMeTheTimeToolStripMenuItem;
        private System.Windows.Forms.Timer tim_time;
        private System.Windows.Forms.ToolStripMenuItem saveToFileToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tSSLblDetect;
        private System.Windows.Forms.ToolStripMenuItem sayWhatIWriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem afterSpaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem afterSentenceToolStripMenuItem;
        private System.Windows.Forms.ListView lvReq;
        private System.Windows.Forms.ColumnHeader colHReq;
        private System.Windows.Forms.ColumnHeader colHReqTime;
        private System.Windows.Forms.ListView lvSpoken;
        private System.Windows.Forms.ColumnHeader colHSpValue;
        private System.Windows.Forms.ColumnHeader colHSpTime;
        private System.Windows.Forms.ColumnHeader colHReqConf;
        private System.Windows.Forms.ToolStripMenuItem readFileToolStripMenuItem;
    }
}

