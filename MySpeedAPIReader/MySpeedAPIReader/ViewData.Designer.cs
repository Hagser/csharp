namespace MySpeedAPIReader
{
    partial class ViewData
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
            this.personas = new System.Windows.Forms.CheckedListBox();
            this.events = new System.Windows.Forms.CheckedListBox();
            this.fromDate = new System.Windows.Forms.DateTimePicker();
            this.toDate = new System.Windows.Forms.DateTimePicker();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.eventmodes = new System.Windows.Forms.CheckedListBox();
            this.chkBestRaces = new System.Windows.Forms.CheckBox();
            this.cars = new System.Windows.Forms.CheckedListBox();
            this.chkMultiCars = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.eventTypes = new System.Windows.Forms.CheckedListBox();
            this.carclasses = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // personas
            // 
            this.personas.CheckOnClick = true;
            this.personas.FormattingEnabled = true;
            this.personas.Location = new System.Drawing.Point(12, 12);
            this.personas.Name = "personas";
            this.personas.Size = new System.Drawing.Size(119, 49);
            this.personas.TabIndex = 0;
            this.personas.SelectedIndexChanged += new System.EventHandler(this.personas_SelectedIndexChanged);
            this.personas.MouseEnter += new System.EventHandler(this.personas_MouseEnter);
            this.personas.MouseLeave += new System.EventHandler(this.personas_MouseLeave);
            // 
            // events
            // 
            this.events.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.events.CheckOnClick = true;
            this.events.FormattingEnabled = true;
            this.events.Location = new System.Drawing.Point(585, 12);
            this.events.Name = "events";
            this.events.Size = new System.Drawing.Size(198, 49);
            this.events.TabIndex = 2;
            this.events.SelectedIndexChanged += new System.EventHandler(this.events_SelectedIndexChanged);
            this.events.MouseEnter += new System.EventHandler(this.events_MouseEnter);
            this.events.MouseLeave += new System.EventHandler(this.events_MouseLeave);
            // 
            // fromDate
            // 
            this.fromDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.fromDate.Location = new System.Drawing.Point(789, 12);
            this.fromDate.Name = "fromDate";
            this.fromDate.ShowCheckBox = true;
            this.fromDate.Size = new System.Drawing.Size(112, 20);
            this.fromDate.TabIndex = 3;
            this.fromDate.ValueChanged += new System.EventHandler(this.fromDate_ValueChanged);
            // 
            // toDate
            // 
            this.toDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.toDate.Location = new System.Drawing.Point(789, 38);
            this.toDate.Name = "toDate";
            this.toDate.ShowCheckBox = true;
            this.toDate.Size = new System.Drawing.Size(112, 20);
            this.toDate.TabIndex = 4;
            this.toDate.ValueChanged += new System.EventHandler(this.toDate_ValueChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 67);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(889, 316);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.DataSourceChanged += new System.EventHandler(this.dataGridView1_DataSourceChanged);
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            // 
            // eventmodes
            // 
            this.eventmodes.CheckOnClick = true;
            this.eventmodes.FormattingEnabled = true;
            this.eventmodes.Location = new System.Drawing.Point(137, 12);
            this.eventmodes.Name = "eventmodes";
            this.eventmodes.Size = new System.Drawing.Size(68, 49);
            this.eventmodes.TabIndex = 6;
            this.eventmodes.SelectedIndexChanged += new System.EventHandler(this.eventmodes_SelectedIndexChanged);
            // 
            // chkBestRaces
            // 
            this.chkBestRaces.AutoSize = true;
            this.chkBestRaces.Location = new System.Drawing.Point(211, 12);
            this.chkBestRaces.Name = "chkBestRaces";
            this.chkBestRaces.Size = new System.Drawing.Size(76, 17);
            this.chkBestRaces.TabIndex = 7;
            this.chkBestRaces.Text = "Best races";
            this.chkBestRaces.UseVisualStyleBackColor = true;
            this.chkBestRaces.CheckedChanged += new System.EventHandler(this.chkBestRaces_CheckedChanged);
            // 
            // cars
            // 
            this.cars.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cars.CheckOnClick = true;
            this.cars.FormattingEnabled = true;
            this.cars.Location = new System.Drawing.Point(400, 12);
            this.cars.Name = "cars";
            this.cars.Size = new System.Drawing.Size(179, 49);
            this.cars.TabIndex = 8;
            this.cars.SelectedIndexChanged += new System.EventHandler(this.cars_SelectedIndexChanged);
            this.cars.MouseEnter += new System.EventHandler(this.cars_MouseEnter);
            this.cars.MouseLeave += new System.EventHandler(this.cars_MouseLeave);
            // 
            // chkMultiCars
            // 
            this.chkMultiCars.AutoSize = true;
            this.chkMultiCars.Enabled = false;
            this.chkMultiCars.Location = new System.Drawing.Point(211, 27);
            this.chkMultiCars.Name = "chkMultiCars";
            this.chkMultiCars.Size = new System.Drawing.Size(71, 17);
            this.chkMultiCars.TabIndex = 9;
            this.chkMultiCars.Text = "Multi cars";
            this.chkMultiCars.UseVisualStyleBackColor = true;
            this.chkMultiCars.CheckedChanged += new System.EventHandler(this.chkMultiCars_CheckedChanged);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(211, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 17);
            this.label1.TabIndex = 10;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // eventTypes
            // 
            this.eventTypes.CheckOnClick = true;
            this.eventTypes.FormattingEnabled = true;
            this.eventTypes.Items.AddRange(new object[] {
            "MP",
            "SP"});
            this.eventTypes.Location = new System.Drawing.Point(293, 12);
            this.eventTypes.Name = "eventTypes";
            this.eventTypes.Size = new System.Drawing.Size(45, 49);
            this.eventTypes.TabIndex = 11;
            this.eventTypes.SelectedIndexChanged += new System.EventHandler(this.eventTypes_SelectedIndexChanged);
            // 
            // carclasses
            // 
            this.carclasses.CheckOnClick = true;
            this.carclasses.FormattingEnabled = true;
            this.carclasses.Items.AddRange(new object[] {
            "S",
            "A",
            "B",
            "C",
            "D",
            "E"});
            this.carclasses.Location = new System.Drawing.Point(344, 12);
            this.carclasses.Name = "carclasses";
            this.carclasses.Size = new System.Drawing.Size(50, 49);
            this.carclasses.TabIndex = 12;
            this.carclasses.SelectedIndexChanged += new System.EventHandler(this.carclasses_SelectedIndexChanged);
            // 
            // ViewData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 395);
            this.Controls.Add(this.carclasses);
            this.Controls.Add(this.eventTypes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkMultiCars);
            this.Controls.Add(this.cars);
            this.Controls.Add(this.chkBestRaces);
            this.Controls.Add(this.eventmodes);
            this.Controls.Add(this.toDate);
            this.Controls.Add(this.fromDate);
            this.Controls.Add(this.events);
            this.Controls.Add(this.personas);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ViewData";
            this.Text = "ViewData";
            this.Load += new System.EventHandler(this.ViewData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox personas;
        private System.Windows.Forms.CheckedListBox events;
        private System.Windows.Forms.DateTimePicker fromDate;
        private System.Windows.Forms.DateTimePicker toDate;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckedListBox eventmodes;
        private System.Windows.Forms.CheckBox chkBestRaces;
        private System.Windows.Forms.CheckedListBox cars;
        private System.Windows.Forms.CheckBox chkMultiCars;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox eventTypes;
        private System.Windows.Forms.CheckedListBox carclasses;
    }
}