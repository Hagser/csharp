namespace MySpeedTest
{
    partial class MapNavigation
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NavV = new System.Windows.Forms.VScrollBar();
            this.NavH = new System.Windows.Forms.HScrollBar();
            this.label1 = new System.Windows.Forms.Label();
            this.vZoom = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.vZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // NavV
            // 
            this.NavV.Location = new System.Drawing.Point(17, 0);
            this.NavV.Maximum = 64000;
            this.NavV.Name = "NavV";
            this.NavV.Size = new System.Drawing.Size(19, 54);
            this.NavV.TabIndex = 1;
            this.NavV.Value = 32000;
            this.NavV.ValueChanged += new System.EventHandler(this.NavV_ValueChanged);
            // 
            // NavH
            // 
            this.NavH.Location = new System.Drawing.Point(0, 18);
            this.NavH.Maximum = 64000;
            this.NavH.Name = "NavH";
            this.NavH.Size = new System.Drawing.Size(54, 19);
            this.NavH.TabIndex = 2;
            this.NavH.Value = 32000;
            this.NavH.ValueChanged += new System.EventHandler(this.NavH_ValueChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(17, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 19);
            this.label1.TabIndex = 3;
            // 
            // vZoom
            // 
            this.vZoom.AutoSize = false;
            this.vZoom.BackColor = System.Drawing.SystemColors.Window;
            this.vZoom.Location = new System.Drawing.Point(15, 57);
            this.vZoom.Maximum = 13;
            this.vZoom.Minimum = 1;
            this.vZoom.Name = "vZoom";
            this.vZoom.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.vZoom.Size = new System.Drawing.Size(30, 93);
            this.vZoom.TabIndex = 4;
            this.vZoom.TickStyle = System.Windows.Forms.TickStyle.None;
            this.vZoom.Value = 1;
            this.vZoom.ValueChanged += new System.EventHandler(this.vZoom_ValueChanged);
            // 
            // MapNavigation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.vZoom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NavH);
            this.Controls.Add(this.NavV);
            this.Name = "MapNavigation";
            this.Size = new System.Drawing.Size(55, 150);
            ((System.ComponentModel.ISupportInitialize)(this.vZoom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar NavV;
        private System.Windows.Forms.HScrollBar NavH;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar vZoom;
    }
}
