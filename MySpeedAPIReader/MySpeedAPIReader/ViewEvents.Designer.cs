namespace MySpeedAPIReader
{
    partial class ViewEvents
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
            this.eventmodes = new System.Windows.Forms.CheckedListBox();
            this.events = new System.Windows.Forms.CheckedListBox();
            this.chkTodays = new System.Windows.Forms.CheckBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.minTiers = new System.Windows.Forms.CheckedListBox();
            this.maxTiers = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // eventmodes
            // 
            this.eventmodes.CheckOnClick = true;
            this.eventmodes.FormattingEnabled = true;
            this.eventmodes.Location = new System.Drawing.Point(12, 12);
            this.eventmodes.Name = "eventmodes";
            this.eventmodes.Size = new System.Drawing.Size(68, 49);
            this.eventmodes.TabIndex = 8;
            this.eventmodes.SelectedIndexChanged += new System.EventHandler(this.eventmodes_SelectedIndexChanged);
            // 
            // events
            // 
            this.events.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.events.CheckOnClick = true;
            this.events.FormattingEnabled = true;
            this.events.Location = new System.Drawing.Point(86, 12);
            this.events.Name = "events";
            this.events.Size = new System.Drawing.Size(308, 49);
            this.events.TabIndex = 7;
            this.events.SelectedIndexChanged += new System.EventHandler(this.events_SelectedIndexChanged);
            this.events.MouseEnter += new System.EventHandler(this.events_MouseEnter);
            this.events.MouseLeave += new System.EventHandler(this.events_MouseLeave);
            // 
            // chkTodays
            // 
            this.chkTodays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkTodays.AutoSize = true;
            this.chkTodays.Location = new System.Drawing.Point(400, 12);
            this.chkTodays.Name = "chkTodays";
            this.chkTodays.Size = new System.Drawing.Size(61, 17);
            this.chkTodays.TabIndex = 9;
            this.chkTodays.Text = "Todays";
            this.chkTodays.UseVisualStyleBackColor = true;
            this.chkTodays.Click += new System.EventHandler(this.chkTodays_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 67);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(662, 302);
            this.dataGridView1.TabIndex = 10;
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            // 
            // minTiers
            // 
            this.minTiers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minTiers.CheckOnClick = true;
            this.minTiers.FormattingEnabled = true;
            this.minTiers.Location = new System.Drawing.Point(467, 12);
            this.minTiers.Name = "minTiers";
            this.minTiers.Size = new System.Drawing.Size(56, 49);
            this.minTiers.TabIndex = 11;
            this.minTiers.SelectedIndexChanged += new System.EventHandler(this.minTiers_SelectedIndexChanged);
            // 
            // maxTiers
            // 
            this.maxTiers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maxTiers.CheckOnClick = true;
            this.maxTiers.FormattingEnabled = true;
            this.maxTiers.Location = new System.Drawing.Point(529, 12);
            this.maxTiers.Name = "maxTiers";
            this.maxTiers.Size = new System.Drawing.Size(58, 49);
            this.maxTiers.TabIndex = 12;
            this.maxTiers.SelectedIndexChanged += new System.EventHandler(this.maxTiers_SelectedIndexChanged);
            // 
            // ViewEvents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 381);
            this.Controls.Add(this.maxTiers);
            this.Controls.Add(this.minTiers);
            this.Controls.Add(this.chkTodays);
            this.Controls.Add(this.eventmodes);
            this.Controls.Add(this.events);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ViewEvents";
            this.Text = "ViewEvents";
            this.Load += new System.EventHandler(this.ViewEvents_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox eventmodes;
        private System.Windows.Forms.CheckedListBox events;
        private System.Windows.Forms.CheckBox chkTodays;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckedListBox minTiers;
        private System.Windows.Forms.CheckedListBox maxTiers;
    }
}