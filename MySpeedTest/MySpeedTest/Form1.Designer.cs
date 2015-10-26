namespace MySpeedTest
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cBSettings = new System.Windows.Forms.ComboBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.chkTimed = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.comboDates = new System.Windows.Forms.ComboBox();
            this.cbMin = new System.Windows.Forms.CheckBox();
            this.cbMax = new System.Windows.Forms.CheckBox();
            this.cbAverages = new System.Windows.Forms.CheckBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxUpdateGraph = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxUpdateMap = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.vSbInterval = new System.Windows.Forms.VScrollBar();
            this.mTbInterval = new System.Windows.Forms.MaskedTextBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.mTbFrom = new System.Windows.Forms.MaskedTextBox();
            this.vSbTo = new System.Windows.Forms.VScrollBar();
            this.vSbFrom = new System.Windows.Forms.VScrollBar();
            this.mTbTo = new System.Windows.Forms.MaskedTextBox();
            this.cBAlways = new System.Windows.Forms.CheckBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.wBrowse = new System.Windows.Forms.WebBrowser();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cBSettings);
            this.splitContainer1.Panel1.Controls.Add(this.btnReset);
            this.splitContainer1.Panel1.Controls.Add(this.btnStart);
            this.splitContainer1.Panel1.Controls.Add(this.chkTimed);
            this.splitContainer1.Panel1.Controls.Add(this.comboBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(847, 407);
            this.splitContainer1.SplitterDistance = 27;
            this.splitContainer1.TabIndex = 1;
            // 
            // cBSettings
            // 
            this.cBSettings.FormattingEnabled = true;
            this.cBSettings.Location = new System.Drawing.Point(351, 5);
            this.cBSettings.Name = "cBSettings";
            this.cBSettings.Size = new System.Drawing.Size(169, 21);
            this.cBSettings.TabIndex = 0;
            this.cBSettings.SelectedValueChanged += new System.EventHandler(this.cBSettings_SelectedValueChanged);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(270, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 14;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(3, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // chkTimed
            // 
            this.chkTimed.AutoSize = true;
            this.chkTimed.Enabled = false;
            this.chkTimed.Location = new System.Drawing.Point(84, 7);
            this.chkTimed.Name = "chkTimed";
            this.chkTimed.Size = new System.Drawing.Size(55, 17);
            this.chkTimed.TabIndex = 9;
            this.chkTimed.Text = "Timed";
            this.chkTimed.UseVisualStyleBackColor = true;
            this.chkTimed.Click += new System.EventHandler(this.chkTimed_CheckedChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(145, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(119, 21);
            this.comboBox1.TabIndex = 12;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(847, 376);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(839, 350);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "List";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ShowCellErrors = false;
            this.dataGridView1.ShowRowErrors = false;
            this.dataGridView1.Size = new System.Drawing.Size(833, 344);
            this.dataGridView1.TabIndex = 13;
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.comboDates);
            this.tabPage2.Controls.Add(this.cbMin);
            this.tabPage2.Controls.Add(this.cbMax);
            this.tabPage2.Controls.Add(this.cbAverages);
            this.tabPage2.Controls.Add(this.comboBox2);
            this.tabPage2.Controls.Add(this.chart1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(839, 350);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Graph";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // comboDates
            // 
            this.comboDates.FormattingEnabled = true;
            this.comboDates.Location = new System.Drawing.Point(3, 6);
            this.comboDates.Name = "comboDates";
            this.comboDates.Size = new System.Drawing.Size(113, 21);
            this.comboDates.TabIndex = 17;
            this.comboDates.SelectedIndexChanged += new System.EventHandler(this.comboDates_SelectedIndexChanged);
            // 
            // cbMin
            // 
            this.cbMin.AutoSize = true;
            this.cbMin.Location = new System.Drawing.Point(405, 8);
            this.cbMin.Name = "cbMin";
            this.cbMin.Size = new System.Drawing.Size(67, 17);
            this.cbMin.TabIndex = 16;
            this.cbMin.Text = "Minimum";
            this.cbMin.UseVisualStyleBackColor = true;
            this.cbMin.Click += new System.EventHandler(this.cbMin_Click);
            // 
            // cbMax
            // 
            this.cbMax.AutoSize = true;
            this.cbMax.Location = new System.Drawing.Point(329, 8);
            this.cbMax.Name = "cbMax";
            this.cbMax.Size = new System.Drawing.Size(70, 17);
            this.cbMax.TabIndex = 15;
            this.cbMax.Text = "Maximum";
            this.cbMax.UseVisualStyleBackColor = true;
            this.cbMax.Click += new System.EventHandler(this.cbMax_Click);
            // 
            // cbAverages
            // 
            this.cbAverages.AutoSize = true;
            this.cbAverages.Location = new System.Drawing.Point(252, 8);
            this.cbAverages.Name = "cbAverages";
            this.cbAverages.Size = new System.Drawing.Size(71, 17);
            this.cbAverages.TabIndex = 14;
            this.cbAverages.Text = "Averages";
            this.cbAverages.UseVisualStyleBackColor = true;
            this.cbAverages.Click += new System.EventHandler(this.cbAverages_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(122, 6);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(124, 21);
            this.comboBox2.TabIndex = 13;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(8, 33);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(823, 309);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Controls.Add(this.cBAlways);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(839, 350);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Settings";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxUpdateGraph);
            this.groupBox3.Location = new System.Drawing.Point(362, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(171, 185);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Graph";
            // 
            // checkBoxUpdateGraph
            // 
            this.checkBoxUpdateGraph.AutoSize = true;
            this.checkBoxUpdateGraph.Location = new System.Drawing.Point(6, 21);
            this.checkBoxUpdateGraph.Name = "checkBoxUpdateGraph";
            this.checkBoxUpdateGraph.Size = new System.Drawing.Size(114, 17);
            this.checkBoxUpdateGraph.TabIndex = 10;
            this.checkBoxUpdateGraph.Text = "Update all the time";
            this.checkBoxUpdateGraph.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxUpdateMap);
            this.groupBox2.Location = new System.Drawing.Point(185, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(171, 185);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Map";
            // 
            // checkBoxUpdateMap
            // 
            this.checkBoxUpdateMap.AutoSize = true;
            this.checkBoxUpdateMap.Location = new System.Drawing.Point(6, 22);
            this.checkBoxUpdateMap.Name = "checkBoxUpdateMap";
            this.checkBoxUpdateMap.Size = new System.Drawing.Size(114, 17);
            this.checkBoxUpdateMap.TabIndex = 9;
            this.checkBoxUpdateMap.Text = "Update all the time";
            this.checkBoxUpdateMap.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.vSbInterval);
            this.groupBox1.Controls.Add(this.mTbInterval);
            this.groupBox1.Controls.Add(this.checkedListBox1);
            this.groupBox1.Controls.Add(this.mTbFrom);
            this.groupBox1.Controls.Add(this.vSbTo);
            this.groupBox1.Controls.Add(this.vSbFrom);
            this.groupBox1.Controls.Add(this.mTbTo);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(8, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(171, 185);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Interval";
            // 
            // vSbInterval
            // 
            this.vSbInterval.Location = new System.Drawing.Point(44, 19);
            this.vSbInterval.Maximum = 0;
            this.vSbInterval.Minimum = -1439;
            this.vSbInterval.Name = "vSbInterval";
            this.vSbInterval.Size = new System.Drawing.Size(14, 20);
            this.vSbInterval.TabIndex = 6;
            this.vSbInterval.Tag = "-5";
            this.vSbInterval.Value = -5;
            this.vSbInterval.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vSbInterval_Scroll);
            // 
            // mTbInterval
            // 
            this.mTbInterval.Location = new System.Drawing.Point(7, 19);
            this.mTbInterval.Mask = "00:00";
            this.mTbInterval.Name = "mTbInterval";
            this.mTbInterval.Size = new System.Drawing.Size(34, 20);
            this.mTbInterval.TabIndex = 5;
            this.mTbInterval.Text = "0005";
            this.mTbInterval.ValidatingType = typeof(System.DateTime);
            this.mTbInterval.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mTbInterval_KeyUp);
            this.mTbInterval.Validated += new System.EventHandler(this.mTbInterval_Validated);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(6, 71);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(159, 109);
            this.checkedListBox1.TabIndex = 0;
            // 
            // mTbFrom
            // 
            this.mTbFrom.Location = new System.Drawing.Point(7, 45);
            this.mTbFrom.Mask = "00:00";
            this.mTbFrom.Name = "mTbFrom";
            this.mTbFrom.Size = new System.Drawing.Size(34, 20);
            this.mTbFrom.TabIndex = 1;
            this.mTbFrom.Text = "0000";
            this.mTbFrom.ValidatingType = typeof(System.DateTime);
            this.mTbFrom.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mTbFrom_KeyUp);
            this.mTbFrom.Validated += new System.EventHandler(this.mTbFrom_Validated);
            // 
            // vSbTo
            // 
            this.vSbTo.Location = new System.Drawing.Point(152, 45);
            this.vSbTo.Maximum = 0;
            this.vSbTo.Minimum = -1439;
            this.vSbTo.Name = "vSbTo";
            this.vSbTo.Size = new System.Drawing.Size(14, 20);
            this.vSbTo.TabIndex = 4;
            this.vSbTo.Tag = "0";
            this.vSbTo.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vSbTo_Scroll);
            // 
            // vSbFrom
            // 
            this.vSbFrom.Location = new System.Drawing.Point(44, 45);
            this.vSbFrom.Maximum = 0;
            this.vSbFrom.Minimum = -1439;
            this.vSbFrom.Name = "vSbFrom";
            this.vSbFrom.Size = new System.Drawing.Size(14, 20);
            this.vSbFrom.TabIndex = 2;
            this.vSbFrom.Tag = "0";
            this.vSbFrom.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vSbFrom_Scroll);
            // 
            // mTbTo
            // 
            this.mTbTo.Location = new System.Drawing.Point(115, 45);
            this.mTbTo.Mask = "00:00";
            this.mTbTo.Name = "mTbTo";
            this.mTbTo.Size = new System.Drawing.Size(34, 20);
            this.mTbTo.TabIndex = 3;
            this.mTbTo.Text = "0000";
            this.mTbTo.ValidatingType = typeof(System.DateTime);
            this.mTbTo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mTbTo_KeyUp);
            this.mTbTo.Validated += new System.EventHandler(this.mTbTo_Validated);
            // 
            // cBAlways
            // 
            this.cBAlways.AutoSize = true;
            this.cBAlways.Checked = true;
            this.cBAlways.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cBAlways.Location = new System.Drawing.Point(8, 204);
            this.cBAlways.Name = "cBAlways";
            this.cBAlways.Size = new System.Drawing.Size(99, 17);
            this.cBAlways.TabIndex = 5;
            this.cBAlways.Text = "Run all the time";
            this.cBAlways.UseVisualStyleBackColor = true;
            this.cBAlways.CheckedChanged += new System.EventHandler(this.cBAlways_CheckedChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.wBrowse);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(839, 350);
            this.tabPage4.TabIndex = 5;
            this.tabPage4.Text = "Map";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // wBrowse
            // 
            this.wBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wBrowse.Location = new System.Drawing.Point(3, 3);
            this.wBrowse.MinimumSize = new System.Drawing.Size(20, 20);
            this.wBrowse.Name = "wBrowse";
            this.wBrowse.ScriptErrorsSuppressed = true;
            this.wBrowse.Size = new System.Drawing.Size(833, 344);
            this.wBrowse.TabIndex = 0;
            this.wBrowse.SizeChanged += new System.EventHandler(this.pictureBox1_SizeChanged);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 407);
            this.Controls.Add(this.splitContainer1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "MySpeedTest";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox chkTimed;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ComboBox cBSettings;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox cbMin;
        private System.Windows.Forms.CheckBox cbMax;
        private System.Windows.Forms.CheckBox cbAverages;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.VScrollBar vSbInterval;
        private System.Windows.Forms.MaskedTextBox mTbInterval;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.MaskedTextBox mTbFrom;
        private System.Windows.Forms.VScrollBar vSbTo;
        private System.Windows.Forms.VScrollBar vSbFrom;
        private System.Windows.Forms.MaskedTextBox mTbTo;
        private System.Windows.Forms.CheckBox cBAlways;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.WebBrowser wBrowse;
        private System.Windows.Forms.ComboBox comboDates;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxUpdateMap;
        private System.Windows.Forms.CheckBox checkBoxUpdateGraph;

    }
}

