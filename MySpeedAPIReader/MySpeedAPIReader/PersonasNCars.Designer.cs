namespace MySpeedAPIReader
{
    partial class PersonasNCars
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
            this.personas = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.NewUser = new System.Windows.Forms.ToolStripTextBox();
            this.cars = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.personas)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cars)).BeginInit();
            this.SuspendLayout();
            // 
            // personas
            // 
            this.personas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.personas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.personas.ContextMenuStrip = this.contextMenuStrip1;
            this.personas.Location = new System.Drawing.Point(12, 12);
            this.personas.Name = "personas";
            this.personas.Size = new System.Drawing.Size(664, 188);
            this.personas.TabIndex = 0;
            this.personas.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.personas_CellContentClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewUser});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(161, 29);
            // 
            // NewUser
            // 
            this.NewUser.AcceptsReturn = true;
            this.NewUser.Name = "NewUser";
            this.NewUser.Size = new System.Drawing.Size(100, 23);
            this.NewUser.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NewUser_KeyUp);
            // 
            // cars
            // 
            this.cars.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cars.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cars.Location = new System.Drawing.Point(12, 206);
            this.cars.Name = "cars";
            this.cars.Size = new System.Drawing.Size(664, 187);
            this.cars.TabIndex = 1;
            // 
            // PersonasNCars
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 407);
            this.Controls.Add(this.cars);
            this.Controls.Add(this.personas);
            this.MaximizeBox = false;
            this.Name = "PersonasNCars";
            this.Text = "PersonasNCars";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PersonasNCars_FormClosing);
            this.Load += new System.EventHandler(this.PersonasNCars_Load);
            ((System.ComponentModel.ISupportInitialize)(this.personas)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cars)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView personas;
        private System.Windows.Forms.DataGridView cars;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripTextBox NewUser;

    }
}