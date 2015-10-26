using System;
using System.Drawing;
using System.Threading;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.IO;
using System.Diagnostics;

namespace MathsChallenge
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{

		#region Variable declaration
		public static string[] strCmdLine = new string[10];
		public string v_folder="";
		public string v_file="";
		public string v_folder55="d:\\blandat\\script\\mathchallenge\\vbs";
		public string v_file55="55_out.txt";
		public string v_folderAllPrimes="";
		public string v_fileAllPrimes="";
		public int v_start1212=1;
		public int v_end1212=10000000;
		public int v_start12=0;
		public int v_end12=100000000;
		public int v_startPrime=0;
		public int v_endPrime=1000000;
		public double v_start34=0;
		public double v_end34=1;
		public int v_cnt=0;
		Hashtable httemp;
		Hashtable htctrl = new Hashtable();
		
		public bool b_running1212=false;
		public bool b_running12=false;
		public bool b_running15=false;
		
		public bool b_running16=false;
		public bool b_running18=false;
		public bool b_running20=false;
		public bool b_running25=false;
		public bool b_running34=false;
		public bool b_running35=false;
		public bool b_running36=false;
		public bool b_running48=false;
		public bool b_running55=false;
		public bool b_running56=false;
		public bool b_running102=false;
		public bool b_running104=false;
		public bool b_runningAllPrimes=false;

		public bool b_PrimesLoaded=false;
		public bool b_PrimesdbAltered=false;
		#endregion

		#region Declare Threads
		Thread t1212;
		Thread t12;
		Thread t15;
		Thread t16;
		Thread t18;
		Thread t20;
		Thread t25;
		Thread t29;
		Thread t34;
		Thread t35;
		Thread t36;
		Thread t48;
		Thread t55;
		Thread t56;
		Thread t102;
		Thread t104;
		Thread tAllPrimes;
		Thread th = null;
		#endregion

		#region ControlDef

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage12;
		private System.Windows.Forms.Button btn12Start;
		private System.Windows.Forms.TextBox txt12File;
		private System.Windows.Forms.Button btn12Browse;
		private System.Windows.Forms.TextBox txt12Log;
		private System.Windows.Forms.OpenFileDialog oFD;
		private System.Windows.Forms.SaveFileDialog sFD;
		private System.Windows.Forms.ProgressBar pB1;
		private System.Windows.Forms.Label lblProg;
		private System.Windows.Forms.ListBox listBox;
		private System.Windows.Forms.TextBox txt12End;
		private System.Windows.Forms.Button btnExport12;
		private System.Windows.Forms.CheckBox chkRealtime;
		private System.Windows.Forms.TabPage tabPage20;
		private System.Windows.Forms.Button btn20Start;
		private System.Windows.Forms.TextBox txtNum20;
		private System.Windows.Forms.Label lbl20;
		private System.Windows.Forms.TabPage tabPage25;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btn25Start;
		private System.Windows.Forms.TextBox txt25Start;
		private System.Windows.Forms.TextBox txt25Length;
		private System.Windows.Forms.Label lbl25Info;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TabPage tabPage36;
		private System.Windows.Forms.ListBox listBox36;
		private System.Windows.Forms.Button btn36start;
		private System.Windows.Forms.TabPage tabPage34;
		private System.Windows.Forms.Button btn34Start;
		private System.Windows.Forms.ListBox listBox34;
		private System.Windows.Forms.TextBox txt34End;
		private System.Windows.Forms.ProgressBar pB34;
		private System.Windows.Forms.Button btn34Export;
		private System.Windows.Forms.Button btn12Stop;
		private System.Windows.Forms.TextBox txt34Start;
		private System.Windows.Forms.TabPage tabPage18;
		private System.Windows.Forms.ListBox listBox18;
		private System.Windows.Forms.Button btn18Start;
		private System.Windows.Forms.Label lbl18;
		private System.Windows.Forms.Button btn18Stop;
		private System.Windows.Forms.TextBox txt20Ans;
		private System.Windows.Forms.TabPage tabPage16;
		private System.Windows.Forms.Button btn16Start;
		private System.Windows.Forms.TextBox txt16Sum;
		private System.Windows.Forms.Label lbl16;
		private System.Windows.Forms.TabPage tabPage48;
		private System.Windows.Forms.TextBox txt48Last10;
		private System.Windows.Forms.Label lbl48;
		private System.Windows.Forms.Button btn48Start;
		private System.Windows.Forms.TabPage tabPageAllPrimes;
		private System.Windows.Forms.ListBox listBoxAllPrimes;
		private System.Windows.Forms.Button btnAllPrimesStart;
		private System.Windows.Forms.Label lblAllPrimes;
		private System.Windows.Forms.TextBox txtAllPrimesStart;
		private System.Windows.Forms.TextBox txtAllPrimesEnd;
		private System.Windows.Forms.Button btnAllPrimesExport;
		private System.Windows.Forms.Button btnAllPrimesStop;
		private System.Windows.Forms.TabPage tabPage55;
		private System.Windows.Forms.ListBox listBox55;
		private System.Windows.Forms.Button btn55Start;
		private System.Windows.Forms.TabPage tabPageTest;
		private System.Windows.Forms.ListBox listBoxTest;
		private System.Windows.Forms.Button btnTestStart;
		private System.Windows.Forms.TextBox txtTestLength;
		private System.Windows.Forms.TextBox txtAllPrimesSaveAfter;
		private System.Windows.Forms.TextBox txtAns36;
		private System.Windows.Forms.TabPage tabPage56;
		private System.Windows.Forms.TextBox txtAns56;
		private System.Windows.Forms.Button btnStart56;
		private System.Windows.Forms.ListBox listBox56;
		private System.Windows.Forms.TabPage tabPage29;
		private System.Windows.Forms.TextBox txtAns29;
		private System.Windows.Forms.Button btnStart29;
		private System.Windows.Forms.TabPage tabPage35;
		private System.Windows.Forms.Button btnStop35;
		private System.Windows.Forms.Button btnExport35;
		private System.Windows.Forms.TextBox txtEnd35;
		private System.Windows.Forms.TextBox txtStart35;
		private System.Windows.Forms.ListBox listBox35;
		private System.Windows.Forms.Button btnStart35;
		private System.Windows.Forms.Label lbl35;
		private System.Windows.Forms.TabPage tabPage1212;
		private System.Windows.Forms.Button btnStart1212;
		private System.Windows.Forms.Button btnEnd1212;
		private System.Windows.Forms.ListView lV1212;
		private System.Windows.Forms.TextBox txtStart1212;
		private System.Windows.Forms.ColumnHeader lV1212colNum;
		private System.Windows.Forms.ColumnHeader lV1212colFac;
		private System.Windows.Forms.ColumnHeader lV1212colSum;
		private System.Windows.Forms.TextBox txtEnd1212;
		private System.Windows.Forms.ColumnHeader lV1212colDivs;
		private System.Windows.Forms.ColumnHeader lV1212colDivCount;
		private System.Windows.Forms.ListView lV29;
		private System.Windows.Forms.ColumnHeader lV29colA;
		private System.Windows.Forms.ColumnHeader lV29colB;
		private System.Windows.Forms.ColumnHeader lV29colSum;
		private System.Windows.Forms.Button btn25Stop;

		private System.Windows.Forms.TabPage tabPage104;
		private System.Windows.Forms.Button btnStop104;
		private System.Windows.Forms.Button btnStart104;
		private System.Windows.Forms.TabPage tabPage15;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.TextBox txt15Width;
		private System.Windows.Forms.ListView lV15;
		private System.Windows.Forms.Button btn15End;
		private System.Windows.Forms.Button btn15Start;
		private System.Windows.Forms.ColumnHeader columnHeader11;
		private System.Windows.Forms.ColumnHeader columnHeader12;
		private System.Windows.Forms.ColumnHeader columnHeader13;

		private System.Windows.Forms.Label lbl15Count;
		private System.Windows.Forms.TabPage tabPageThreadPrio;
		private System.Windows.Forms.ListView lVPrio;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem mnuItemH;
		private System.Windows.Forms.MenuItem mnuItemAN;
		private System.Windows.Forms.MenuItem mnuItemN;
		private System.Windows.Forms.MenuItem mnuItemBN;
		private System.Windows.Forms.MenuItem mnuItemL;
		private System.Windows.Forms.MenuItem mnuItemR;
		private System.Windows.Forms.MenuItem mnuItemS;
		private System.Windows.Forms.MenuItem mnuItemNR;
		private System.Windows.Forms.Button btnAllPrimesLastNum;

		private System.Windows.Forms.ListView lV104;
		private System.Windows.Forms.ColumnHeader columnHeader17;
		private System.Windows.Forms.ColumnHeader columnHeader18;
		private System.Windows.Forms.ColumnHeader columnHeader19;
		private System.Windows.Forms.ColumnHeader columnHeader20;
		private System.Windows.Forms.ColumnHeader columnHeader21;
		private System.Windows.Forms.ColumnHeader columnHeader9;
		private System.Windows.Forms.ColumnHeader columnHeader10;
		private System.Windows.Forms.Label lbl104Start;
		private System.Windows.Forms.TabPage tabPage102;
		private System.Windows.Forms.PictureBox pic102;
		private System.Windows.Forms.Button btnStart102;
		private System.Windows.Forms.Button btnStop102;
		private System.Windows.Forms.ProgressBar pB102;

        #endregion

        private System.ComponentModel.IContainer components;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}


	
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.txt12Log = new System.Windows.Forms.TextBox();
            this.chkRealtime = new System.Windows.Forms.CheckBox();
            this.btnExport12 = new System.Windows.Forms.Button();
            this.txt12End = new System.Windows.Forms.TextBox();
            this.listBox = new System.Windows.Forms.ListBox();
            this.btn12Stop = new System.Windows.Forms.Button();
            this.lblProg = new System.Windows.Forms.Label();
            this.pB1 = new System.Windows.Forms.ProgressBar();
            this.btn12Browse = new System.Windows.Forms.Button();
            this.txt12File = new System.Windows.Forms.TextBox();
            this.btn12Start = new System.Windows.Forms.Button();
            this.tabPage1212 = new System.Windows.Forms.TabPage();
            this.txtEnd1212 = new System.Windows.Forms.TextBox();
            this.txtStart1212 = new System.Windows.Forms.TextBox();
            this.lV1212 = new System.Windows.Forms.ListView();
            this.lV1212colFac = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lV1212colSum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lV1212colNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lV1212colDivs = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lV1212colDivCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnEnd1212 = new System.Windows.Forms.Button();
            this.btnStart1212 = new System.Windows.Forms.Button();
            this.tabPage15 = new System.Windows.Forms.TabPage();
            this.lbl15Count = new System.Windows.Forms.Label();
            this.txt15Width = new System.Windows.Forms.TextBox();
            this.lV15 = new System.Windows.Forms.ListView();
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn15End = new System.Windows.Forms.Button();
            this.btn15Start = new System.Windows.Forms.Button();
            this.tabPage16 = new System.Windows.Forms.TabPage();
            this.txt16Sum = new System.Windows.Forms.TextBox();
            this.lbl16 = new System.Windows.Forms.Label();
            this.btn16Start = new System.Windows.Forms.Button();
            this.tabPage18 = new System.Windows.Forms.TabPage();
            this.btn18Stop = new System.Windows.Forms.Button();
            this.lbl18 = new System.Windows.Forms.Label();
            this.listBox18 = new System.Windows.Forms.ListBox();
            this.btn18Start = new System.Windows.Forms.Button();
            this.tabPage20 = new System.Windows.Forms.TabPage();
            this.txt20Ans = new System.Windows.Forms.TextBox();
            this.lbl20 = new System.Windows.Forms.Label();
            this.txtNum20 = new System.Windows.Forms.TextBox();
            this.btn20Start = new System.Windows.Forms.Button();
            this.tabPage25 = new System.Windows.Forms.TabPage();
            this.btn25Stop = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl25Info = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt25Length = new System.Windows.Forms.TextBox();
            this.txt25Start = new System.Windows.Forms.TextBox();
            this.btn25Start = new System.Windows.Forms.Button();
            this.tabPage29 = new System.Windows.Forms.TabPage();
            this.lV29 = new System.Windows.Forms.ListView();
            this.lV29colA = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lV29colB = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lV29colSum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtAns29 = new System.Windows.Forms.TextBox();
            this.btnStart29 = new System.Windows.Forms.Button();
            this.tabPage34 = new System.Windows.Forms.TabPage();
            this.txt34Start = new System.Windows.Forms.TextBox();
            this.btn34Export = new System.Windows.Forms.Button();
            this.pB34 = new System.Windows.Forms.ProgressBar();
            this.txt34End = new System.Windows.Forms.TextBox();
            this.listBox34 = new System.Windows.Forms.ListBox();
            this.btn34Start = new System.Windows.Forms.Button();
            this.tabPage35 = new System.Windows.Forms.TabPage();
            this.lbl35 = new System.Windows.Forms.Label();
            this.btnStop35 = new System.Windows.Forms.Button();
            this.btnExport35 = new System.Windows.Forms.Button();
            this.txtEnd35 = new System.Windows.Forms.TextBox();
            this.txtStart35 = new System.Windows.Forms.TextBox();
            this.listBox35 = new System.Windows.Forms.ListBox();
            this.btnStart35 = new System.Windows.Forms.Button();
            this.tabPage36 = new System.Windows.Forms.TabPage();
            this.txtAns36 = new System.Windows.Forms.TextBox();
            this.listBox36 = new System.Windows.Forms.ListBox();
            this.btn36start = new System.Windows.Forms.Button();
            this.tabPage48 = new System.Windows.Forms.TabPage();
            this.txt48Last10 = new System.Windows.Forms.TextBox();
            this.lbl48 = new System.Windows.Forms.Label();
            this.btn48Start = new System.Windows.Forms.Button();
            this.tabPage55 = new System.Windows.Forms.TabPage();
            this.listBox55 = new System.Windows.Forms.ListBox();
            this.btn55Start = new System.Windows.Forms.Button();
            this.tabPage56 = new System.Windows.Forms.TabPage();
            this.listBox56 = new System.Windows.Forms.ListBox();
            this.txtAns56 = new System.Windows.Forms.TextBox();
            this.btnStart56 = new System.Windows.Forms.Button();
            this.tabPage102 = new System.Windows.Forms.TabPage();
            this.pB102 = new System.Windows.Forms.ProgressBar();
            this.btnStop102 = new System.Windows.Forms.Button();
            this.btnStart102 = new System.Windows.Forms.Button();
            this.pic102 = new System.Windows.Forms.PictureBox();
            this.tabPage104 = new System.Windows.Forms.TabPage();
            this.lbl104Start = new System.Windows.Forms.Label();
            this.lV104 = new System.Windows.Forms.ListView();
            this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader20 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader21 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnStop104 = new System.Windows.Forms.Button();
            this.btnStart104 = new System.Windows.Forms.Button();
            this.tabPageTest = new System.Windows.Forms.TabPage();
            this.txtTestLength = new System.Windows.Forms.TextBox();
            this.listBoxTest = new System.Windows.Forms.ListBox();
            this.btnTestStart = new System.Windows.Forms.Button();
            this.tabPageAllPrimes = new System.Windows.Forms.TabPage();
            this.btnAllPrimesLastNum = new System.Windows.Forms.Button();
            this.txtAllPrimesSaveAfter = new System.Windows.Forms.TextBox();
            this.btnAllPrimesStop = new System.Windows.Forms.Button();
            this.btnAllPrimesExport = new System.Windows.Forms.Button();
            this.txtAllPrimesEnd = new System.Windows.Forms.TextBox();
            this.txtAllPrimesStart = new System.Windows.Forms.TextBox();
            this.lblAllPrimes = new System.Windows.Forms.Label();
            this.listBoxAllPrimes = new System.Windows.Forms.ListBox();
            this.btnAllPrimesStart = new System.Windows.Forms.Button();
            this.tabPageThreadPrio = new System.Windows.Forms.TabPage();
            this.lVPrio = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuItemR = new System.Windows.Forms.MenuItem();
            this.mnuItemS = new System.Windows.Forms.MenuItem();
            this.mnuItemNR = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.mnuItemH = new System.Windows.Forms.MenuItem();
            this.mnuItemAN = new System.Windows.Forms.MenuItem();
            this.mnuItemN = new System.Windows.Forms.MenuItem();
            this.mnuItemBN = new System.Windows.Forms.MenuItem();
            this.mnuItemL = new System.Windows.Forms.MenuItem();
            this.oFD = new System.Windows.Forms.OpenFileDialog();
            this.sFD = new System.Windows.Forms.SaveFileDialog();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1.SuspendLayout();
            this.tabPage12.SuspendLayout();
            this.tabPage1212.SuspendLayout();
            this.tabPage15.SuspendLayout();
            this.tabPage16.SuspendLayout();
            this.tabPage18.SuspendLayout();
            this.tabPage20.SuspendLayout();
            this.tabPage25.SuspendLayout();
            this.tabPage29.SuspendLayout();
            this.tabPage34.SuspendLayout();
            this.tabPage35.SuspendLayout();
            this.tabPage36.SuspendLayout();
            this.tabPage48.SuspendLayout();
            this.tabPage55.SuspendLayout();
            this.tabPage56.SuspendLayout();
            this.tabPage102.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic102)).BeginInit();
            this.tabPage104.SuspendLayout();
            this.tabPageTest.SuspendLayout();
            this.tabPageAllPrimes.SuspendLayout();
            this.tabPageThreadPrio.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage12);
            this.tabControl1.Controls.Add(this.tabPage1212);
            this.tabControl1.Controls.Add(this.tabPage15);
            this.tabControl1.Controls.Add(this.tabPage16);
            this.tabControl1.Controls.Add(this.tabPage18);
            this.tabControl1.Controls.Add(this.tabPage20);
            this.tabControl1.Controls.Add(this.tabPage25);
            this.tabControl1.Controls.Add(this.tabPage29);
            this.tabControl1.Controls.Add(this.tabPage34);
            this.tabControl1.Controls.Add(this.tabPage35);
            this.tabControl1.Controls.Add(this.tabPage36);
            this.tabControl1.Controls.Add(this.tabPage48);
            this.tabControl1.Controls.Add(this.tabPage55);
            this.tabControl1.Controls.Add(this.tabPage56);
            this.tabControl1.Controls.Add(this.tabPage102);
            this.tabControl1.Controls.Add(this.tabPage104);
            this.tabControl1.Controls.Add(this.tabPageTest);
            this.tabControl1.Controls.Add(this.tabPageAllPrimes);
            this.tabControl1.Controls.Add(this.tabPageThreadPrio);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(560, 421);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage12
            // 
            this.tabPage12.Controls.Add(this.txt12Log);
            this.tabPage12.Controls.Add(this.chkRealtime);
            this.tabPage12.Controls.Add(this.btnExport12);
            this.tabPage12.Controls.Add(this.txt12End);
            this.tabPage12.Controls.Add(this.listBox);
            this.tabPage12.Controls.Add(this.btn12Stop);
            this.tabPage12.Controls.Add(this.lblProg);
            this.tabPage12.Controls.Add(this.pB1);
            this.tabPage12.Controls.Add(this.btn12Browse);
            this.tabPage12.Controls.Add(this.txt12File);
            this.tabPage12.Controls.Add(this.btn12Start);
            this.tabPage12.Location = new System.Drawing.Point(4, 40);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Size = new System.Drawing.Size(552, 377);
            this.tabPage12.TabIndex = 0;
            this.tabPage12.Text = "12";
            // 
            // txt12Log
            // 
            this.txt12Log.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt12Log.Location = new System.Drawing.Point(480, 160);
            this.txt12Log.Name = "txt12Log";
            this.txt12Log.Size = new System.Drawing.Size(64, 20);
            this.txt12Log.TabIndex = 11;
            this.txt12Log.Text = "75";
            // 
            // chkRealtime
            // 
            this.chkRealtime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRealtime.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRealtime.Location = new System.Drawing.Point(480, 112);
            this.chkRealtime.Name = "chkRealtime";
            this.chkRealtime.Size = new System.Drawing.Size(64, 16);
            this.chkRealtime.TabIndex = 10;
            this.chkRealtime.Text = "Realtime";
            // 
            // btnExport12
            // 
            this.btnExport12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport12.Location = new System.Drawing.Point(480, 80);
            this.btnExport12.Name = "btnExport12";
            this.btnExport12.Size = new System.Drawing.Size(64, 23);
            this.btnExport12.TabIndex = 9;
            this.btnExport12.Text = "Export";
            this.btnExport12.Click += new System.EventHandler(this.btnExport12_Click);
            // 
            // txt12End
            // 
            this.txt12End.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt12End.Location = new System.Drawing.Point(480, 136);
            this.txt12End.Name = "txt12End";
            this.txt12End.Size = new System.Drawing.Size(64, 20);
            this.txt12End.TabIndex = 8;
            this.txt12End.Text = "10000000";
            this.txt12End.TextChanged += new System.EventHandler(this.txt12End_TextChanged);
            // 
            // listBox
            // 
            this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox.Location = new System.Drawing.Point(0, 48);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(472, 290);
            this.listBox.TabIndex = 7;
            // 
            // btn12Stop
            // 
            this.btn12Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn12Stop.Location = new System.Drawing.Point(480, 56);
            this.btn12Stop.Name = "btn12Stop";
            this.btn12Stop.Size = new System.Drawing.Size(64, 23);
            this.btn12Stop.TabIndex = 6;
            this.btn12Stop.Text = "Stop";
            this.btn12Stop.Click += new System.EventHandler(this.btn12Stop_Click);
            // 
            // lblProg
            // 
            this.lblProg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProg.Location = new System.Drawing.Point(0, 24);
            this.lblProg.Name = "lblProg";
            this.lblProg.Size = new System.Drawing.Size(472, 23);
            this.lblProg.TabIndex = 5;
            this.lblProg.Text = "Progress";
            // 
            // pB1
            // 
            this.pB1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pB1.Location = new System.Drawing.Point(0, 361);
            this.pB1.Maximum = 100000000;
            this.pB1.Name = "pB1";
            this.pB1.Size = new System.Drawing.Size(552, 16);
            this.pB1.TabIndex = 4;
            // 
            // btn12Browse
            // 
            this.btn12Browse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn12Browse.Location = new System.Drawing.Point(480, 8);
            this.btn12Browse.Name = "btn12Browse";
            this.btn12Browse.Size = new System.Drawing.Size(64, 23);
            this.btn12Browse.TabIndex = 2;
            this.btn12Browse.Text = "Last num";
            this.btn12Browse.Click += new System.EventHandler(this.btn12Browse_Click);
            // 
            // txt12File
            // 
            this.txt12File.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt12File.Location = new System.Drawing.Point(0, 0);
            this.txt12File.Name = "txt12File";
            this.txt12File.Size = new System.Drawing.Size(472, 20);
            this.txt12File.TabIndex = 1;
            // 
            // btn12Start
            // 
            this.btn12Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn12Start.Location = new System.Drawing.Point(480, 32);
            this.btn12Start.Name = "btn12Start";
            this.btn12Start.Size = new System.Drawing.Size(64, 23);
            this.btn12Start.TabIndex = 0;
            this.btn12Start.Text = "Start";
            this.btn12Start.Click += new System.EventHandler(this.btn12Start_Click);
            // 
            // tabPage1212
            // 
            this.tabPage1212.Controls.Add(this.txtEnd1212);
            this.tabPage1212.Controls.Add(this.txtStart1212);
            this.tabPage1212.Controls.Add(this.lV1212);
            this.tabPage1212.Controls.Add(this.btnEnd1212);
            this.tabPage1212.Controls.Add(this.btnStart1212);
            this.tabPage1212.Location = new System.Drawing.Point(4, 40);
            this.tabPage1212.Name = "tabPage1212";
            this.tabPage1212.Size = new System.Drawing.Size(552, 377);
            this.tabPage1212.TabIndex = 14;
            this.tabPage1212.Text = "12?";
            // 
            // txtEnd1212
            // 
            this.txtEnd1212.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtEnd1212.Location = new System.Drawing.Point(272, 8);
            this.txtEnd1212.Name = "txtEnd1212";
            this.txtEnd1212.Size = new System.Drawing.Size(64, 20);
            this.txtEnd1212.TabIndex = 4;
            this.txtEnd1212.Text = "200000000";
            this.txtEnd1212.TextChanged += new System.EventHandler(this.txtEnd1212_TextChanged);
            // 
            // txtStart1212
            // 
            this.txtStart1212.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtStart1212.Location = new System.Drawing.Point(224, 8);
            this.txtStart1212.Name = "txtStart1212";
            this.txtStart1212.Size = new System.Drawing.Size(40, 20);
            this.txtStart1212.TabIndex = 3;
            this.txtStart1212.Text = "1";
            this.txtStart1212.TextChanged += new System.EventHandler(this.txtStart1212_TextChanged);
            // 
            // lV1212
            // 
            this.lV1212.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lV1212.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lV1212colFac,
            this.lV1212colSum,
            this.lV1212colNum,
            this.lV1212colDivs,
            this.lV1212colDivCount});
            this.lV1212.Location = new System.Drawing.Point(8, 40);
            this.lV1212.Name = "lV1212";
            this.lV1212.Size = new System.Drawing.Size(536, 330);
            this.lV1212.TabIndex = 2;
            this.lV1212.UseCompatibleStateImageBehavior = false;
            this.lV1212.View = System.Windows.Forms.View.Details;
            // 
            // lV1212colFac
            // 
            this.lV1212colFac.Text = "Factors";
            this.lV1212colFac.Width = 85;
            // 
            // lV1212colSum
            // 
            this.lV1212colSum.Text = "Sum";
            this.lV1212colSum.Width = 81;
            // 
            // lV1212colNum
            // 
            this.lV1212colNum.Text = "Number";
            this.lV1212colNum.Width = 97;
            // 
            // lV1212colDivs
            // 
            this.lV1212colDivs.Text = "Divs";
            // 
            // lV1212colDivCount
            // 
            this.lV1212colDivCount.Text = "DivCount";
            // 
            // btnEnd1212
            // 
            this.btnEnd1212.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnd1212.Location = new System.Drawing.Point(472, 8);
            this.btnEnd1212.Name = "btnEnd1212";
            this.btnEnd1212.Size = new System.Drawing.Size(75, 23);
            this.btnEnd1212.TabIndex = 1;
            this.btnEnd1212.Text = "End";
            this.btnEnd1212.Click += new System.EventHandler(this.btnEnd1212_Click);
            // 
            // btnStart1212
            // 
            this.btnStart1212.Location = new System.Drawing.Point(8, 8);
            this.btnStart1212.Name = "btnStart1212";
            this.btnStart1212.Size = new System.Drawing.Size(75, 23);
            this.btnStart1212.TabIndex = 0;
            this.btnStart1212.Text = "Start";
            this.btnStart1212.Click += new System.EventHandler(this.btnStart1212_Click);
            // 
            // tabPage15
            // 
            this.tabPage15.Controls.Add(this.lbl15Count);
            this.tabPage15.Controls.Add(this.txt15Width);
            this.tabPage15.Controls.Add(this.lV15);
            this.tabPage15.Controls.Add(this.btn15End);
            this.tabPage15.Controls.Add(this.btn15Start);
            this.tabPage15.Location = new System.Drawing.Point(4, 40);
            this.tabPage15.Name = "tabPage15";
            this.tabPage15.Size = new System.Drawing.Size(552, 377);
            this.tabPage15.TabIndex = 16;
            this.tabPage15.Text = "15";
            // 
            // lbl15Count
            // 
            this.lbl15Count.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl15Count.Location = new System.Drawing.Point(121, 12);
            this.lbl15Count.Name = "lbl15Count";
            this.lbl15Count.Size = new System.Drawing.Size(344, 16);
            this.lbl15Count.TabIndex = 9;
            // 
            // txt15Width
            // 
            this.txt15Width.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt15Width.Location = new System.Drawing.Point(88, 9);
            this.txt15Width.Name = "txt15Width";
            this.txt15Width.Size = new System.Drawing.Size(24, 20);
            this.txt15Width.TabIndex = 8;
            this.txt15Width.Text = "2";
            // 
            // lV15
            // 
            this.lV15.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lV15.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13});
            this.lV15.Location = new System.Drawing.Point(7, 39);
            this.lV15.Name = "lV15";
            this.lV15.Size = new System.Drawing.Size(536, 330);
            this.lV15.TabIndex = 7;
            this.lV15.UseCompatibleStateImageBehavior = false;
            this.lV15.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Step";
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "X";
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Y";
            // 
            // btn15End
            // 
            this.btn15End.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn15End.Location = new System.Drawing.Point(471, 7);
            this.btn15End.Name = "btn15End";
            this.btn15End.Size = new System.Drawing.Size(75, 23);
            this.btn15End.TabIndex = 6;
            this.btn15End.Text = "End";
            this.btn15End.Click += new System.EventHandler(this.btn15End_Click);
            // 
            // btn15Start
            // 
            this.btn15Start.Location = new System.Drawing.Point(7, 7);
            this.btn15Start.Name = "btn15Start";
            this.btn15Start.Size = new System.Drawing.Size(75, 23);
            this.btn15Start.TabIndex = 5;
            this.btn15Start.Text = "Start";
            this.btn15Start.Click += new System.EventHandler(this.btn15Start_Click);
            // 
            // tabPage16
            // 
            this.tabPage16.Controls.Add(this.txt16Sum);
            this.tabPage16.Controls.Add(this.lbl16);
            this.tabPage16.Controls.Add(this.btn16Start);
            this.tabPage16.Location = new System.Drawing.Point(4, 40);
            this.tabPage16.Name = "tabPage16";
            this.tabPage16.Size = new System.Drawing.Size(552, 377);
            this.tabPage16.TabIndex = 6;
            this.tabPage16.Text = "16s";
            // 
            // txt16Sum
            // 
            this.txt16Sum.Location = new System.Drawing.Point(96, 176);
            this.txt16Sum.Name = "txt16Sum";
            this.txt16Sum.Size = new System.Drawing.Size(96, 20);
            this.txt16Sum.TabIndex = 7;
            // 
            // lbl16
            // 
            this.lbl16.Location = new System.Drawing.Point(40, 48);
            this.lbl16.Name = "lbl16";
            this.lbl16.Size = new System.Drawing.Size(224, 120);
            this.lbl16.TabIndex = 6;
            // 
            // btn16Start
            // 
            this.btn16Start.Location = new System.Drawing.Point(96, 8);
            this.btn16Start.Name = "btn16Start";
            this.btn16Start.Size = new System.Drawing.Size(96, 23);
            this.btn16Start.TabIndex = 4;
            this.btn16Start.Text = "Start";
            this.btn16Start.Click += new System.EventHandler(this.btn16Start_Click);
            // 
            // tabPage18
            // 
            this.tabPage18.Controls.Add(this.btn18Stop);
            this.tabPage18.Controls.Add(this.lbl18);
            this.tabPage18.Controls.Add(this.listBox18);
            this.tabPage18.Controls.Add(this.btn18Start);
            this.tabPage18.Location = new System.Drawing.Point(4, 40);
            this.tabPage18.Name = "tabPage18";
            this.tabPage18.Size = new System.Drawing.Size(552, 377);
            this.tabPage18.TabIndex = 5;
            this.tabPage18.Text = "18";
            // 
            // btn18Stop
            // 
            this.btn18Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn18Stop.Location = new System.Drawing.Point(496, 32);
            this.btn18Stop.Name = "btn18Stop";
            this.btn18Stop.Size = new System.Drawing.Size(48, 23);
            this.btn18Stop.TabIndex = 13;
            this.btn18Stop.Text = "Stop";
            this.btn18Stop.Click += new System.EventHandler(this.btn18Stop_Click);
            // 
            // lbl18
            // 
            this.lbl18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl18.Location = new System.Drawing.Point(496, 64);
            this.lbl18.Name = "lbl18";
            this.lbl18.Size = new System.Drawing.Size(48, 16);
            this.lbl18.TabIndex = 12;
            // 
            // listBox18
            // 
            this.listBox18.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox18.HorizontalScrollbar = true;
            this.listBox18.Location = new System.Drawing.Point(0, 8);
            this.listBox18.Name = "listBox18";
            this.listBox18.Size = new System.Drawing.Size(488, 264);
            this.listBox18.TabIndex = 11;
            // 
            // btn18Start
            // 
            this.btn18Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn18Start.Location = new System.Drawing.Point(496, 8);
            this.btn18Start.Name = "btn18Start";
            this.btn18Start.Size = new System.Drawing.Size(48, 23);
            this.btn18Start.TabIndex = 10;
            this.btn18Start.Text = "Start";
            this.btn18Start.Click += new System.EventHandler(this.btn18Start_Click);
            // 
            // tabPage20
            // 
            this.tabPage20.Controls.Add(this.txt20Ans);
            this.tabPage20.Controls.Add(this.lbl20);
            this.tabPage20.Controls.Add(this.txtNum20);
            this.tabPage20.Controls.Add(this.btn20Start);
            this.tabPage20.Location = new System.Drawing.Point(4, 40);
            this.tabPage20.Name = "tabPage20";
            this.tabPage20.Size = new System.Drawing.Size(552, 377);
            this.tabPage20.TabIndex = 1;
            this.tabPage20.Text = "20s";
            // 
            // txt20Ans
            // 
            this.txt20Ans.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt20Ans.Location = new System.Drawing.Point(448, 8);
            this.txt20Ans.Name = "txt20Ans";
            this.txt20Ans.Size = new System.Drawing.Size(96, 20);
            this.txt20Ans.TabIndex = 3;
            // 
            // lbl20
            // 
            this.lbl20.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl20.Location = new System.Drawing.Point(40, 80);
            this.lbl20.Name = "lbl20";
            this.lbl20.Size = new System.Drawing.Size(480, 120);
            this.lbl20.TabIndex = 2;
            // 
            // txtNum20
            // 
            this.txtNum20.Location = new System.Drawing.Point(8, 8);
            this.txtNum20.Name = "txtNum20";
            this.txtNum20.Size = new System.Drawing.Size(96, 20);
            this.txtNum20.TabIndex = 1;
            // 
            // btn20Start
            // 
            this.btn20Start.Location = new System.Drawing.Point(96, 40);
            this.btn20Start.Name = "btn20Start";
            this.btn20Start.Size = new System.Drawing.Size(96, 23);
            this.btn20Start.TabIndex = 0;
            this.btn20Start.Text = "Start";
            this.btn20Start.Click += new System.EventHandler(this.btn20Start_Click);
            // 
            // tabPage25
            // 
            this.tabPage25.Controls.Add(this.btn25Stop);
            this.tabPage25.Controls.Add(this.label2);
            this.tabPage25.Controls.Add(this.lbl25Info);
            this.tabPage25.Controls.Add(this.label1);
            this.tabPage25.Controls.Add(this.txt25Length);
            this.tabPage25.Controls.Add(this.txt25Start);
            this.tabPage25.Controls.Add(this.btn25Start);
            this.tabPage25.Location = new System.Drawing.Point(4, 40);
            this.tabPage25.Name = "tabPage25";
            this.tabPage25.Size = new System.Drawing.Size(552, 377);
            this.tabPage25.TabIndex = 2;
            this.tabPage25.Text = "25s";
            // 
            // btn25Stop
            // 
            this.btn25Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn25Stop.Location = new System.Drawing.Point(472, 32);
            this.btn25Stop.Name = "btn25Stop";
            this.btn25Stop.Size = new System.Drawing.Size(75, 23);
            this.btn25Stop.TabIndex = 7;
            this.btn25Stop.Text = "Stop";
            this.btn25Stop.Click += new System.EventHandler(this.btn25Stop_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Length";
            // 
            // lbl25Info
            // 
            this.lbl25Info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl25Info.Location = new System.Drawing.Point(16, 64);
            this.lbl25Info.Name = "lbl25Info";
            this.lbl25Info.Size = new System.Drawing.Size(528, 304);
            this.lbl25Info.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Start";
            // 
            // txt25Length
            // 
            this.txt25Length.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt25Length.Location = new System.Drawing.Point(56, 32);
            this.txt25Length.Name = "txt25Length";
            this.txt25Length.Size = new System.Drawing.Size(408, 20);
            this.txt25Length.TabIndex = 2;
            this.txt25Length.Text = "1000";
            // 
            // txt25Start
            // 
            this.txt25Start.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt25Start.Location = new System.Drawing.Point(56, 8);
            this.txt25Start.Name = "txt25Start";
            this.txt25Start.Size = new System.Drawing.Size(408, 20);
            this.txt25Start.TabIndex = 1;
            this.txt25Start.Text = "1";
            // 
            // btn25Start
            // 
            this.btn25Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn25Start.Location = new System.Drawing.Point(472, 8);
            this.btn25Start.Name = "btn25Start";
            this.btn25Start.Size = new System.Drawing.Size(75, 23);
            this.btn25Start.TabIndex = 0;
            this.btn25Start.Text = "Start";
            this.btn25Start.Click += new System.EventHandler(this.btn25Start_Click);
            // 
            // tabPage29
            // 
            this.tabPage29.Controls.Add(this.lV29);
            this.tabPage29.Controls.Add(this.txtAns29);
            this.tabPage29.Controls.Add(this.btnStart29);
            this.tabPage29.Location = new System.Drawing.Point(4, 40);
            this.tabPage29.Name = "tabPage29";
            this.tabPage29.Size = new System.Drawing.Size(552, 377);
            this.tabPage29.TabIndex = 12;
            this.tabPage29.Text = "29s";
            // 
            // lV29
            // 
            this.lV29.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lV29.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lV29colA,
            this.lV29colB,
            this.lV29colSum});
            this.lV29.Location = new System.Drawing.Point(8, 64);
            this.lV29.Name = "lV29";
            this.lV29.Size = new System.Drawing.Size(536, 304);
            this.lV29.TabIndex = 17;
            this.lV29.UseCompatibleStateImageBehavior = false;
            this.lV29.View = System.Windows.Forms.View.Details;
            // 
            // lV29colA
            // 
            this.lV29colA.Text = "A";
            // 
            // lV29colB
            // 
            this.lV29colB.Text = "B";
            // 
            // lV29colSum
            // 
            this.lV29colSum.Text = "Sum";
            this.lV29colSum.Width = 267;
            // 
            // txtAns29
            // 
            this.txtAns29.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAns29.Location = new System.Drawing.Point(8, 40);
            this.txtAns29.Name = "txtAns29";
            this.txtAns29.Size = new System.Drawing.Size(536, 20);
            this.txtAns29.TabIndex = 16;
            // 
            // btnStart29
            // 
            this.btnStart29.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart29.Location = new System.Drawing.Point(480, 8);
            this.btnStart29.Name = "btnStart29";
            this.btnStart29.Size = new System.Drawing.Size(64, 23);
            this.btnStart29.TabIndex = 15;
            this.btnStart29.Text = "Start";
            this.btnStart29.Click += new System.EventHandler(this.btnStart29_Click);
            // 
            // tabPage34
            // 
            this.tabPage34.Controls.Add(this.txt34Start);
            this.tabPage34.Controls.Add(this.btn34Export);
            this.tabPage34.Controls.Add(this.pB34);
            this.tabPage34.Controls.Add(this.txt34End);
            this.tabPage34.Controls.Add(this.listBox34);
            this.tabPage34.Controls.Add(this.btn34Start);
            this.tabPage34.Location = new System.Drawing.Point(4, 40);
            this.tabPage34.Name = "tabPage34";
            this.tabPage34.Size = new System.Drawing.Size(552, 377);
            this.tabPage34.TabIndex = 4;
            this.tabPage34.Text = "34s";
            // 
            // txt34Start
            // 
            this.txt34Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt34Start.Location = new System.Drawing.Point(416, 8);
            this.txt34Start.Name = "txt34Start";
            this.txt34Start.Size = new System.Drawing.Size(64, 20);
            this.txt34Start.TabIndex = 13;
            this.txt34Start.Text = "33333333";
            this.txt34Start.TextChanged += new System.EventHandler(this.txt34Start_TextChanged);
            // 
            // btn34Export
            // 
            this.btn34Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn34Export.Location = new System.Drawing.Point(344, 8);
            this.btn34Export.Name = "btn34Export";
            this.btn34Export.Size = new System.Drawing.Size(64, 23);
            this.btn34Export.TabIndex = 12;
            this.btn34Export.Text = "Export";
            this.btn34Export.Click += new System.EventHandler(this.btn34Export_Click);
            // 
            // pB34
            // 
            this.pB34.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pB34.Location = new System.Drawing.Point(0, 361);
            this.pB34.Maximum = 0;
            this.pB34.Name = "pB34";
            this.pB34.Size = new System.Drawing.Size(552, 16);
            this.pB34.TabIndex = 11;
            // 
            // txt34End
            // 
            this.txt34End.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt34End.Location = new System.Drawing.Point(480, 8);
            this.txt34End.Name = "txt34End";
            this.txt34End.Size = new System.Drawing.Size(64, 20);
            this.txt34End.TabIndex = 9;
            this.txt34End.Text = "100000000";
            this.txt34End.TextChanged += new System.EventHandler(this.txt34End_TextChanged);
            // 
            // listBox34
            // 
            this.listBox34.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox34.Location = new System.Drawing.Point(8, 32);
            this.listBox34.Name = "listBox34";
            this.listBox34.Size = new System.Drawing.Size(536, 251);
            this.listBox34.TabIndex = 8;
            // 
            // btn34Start
            // 
            this.btn34Start.Location = new System.Drawing.Point(8, 8);
            this.btn34Start.Name = "btn34Start";
            this.btn34Start.Size = new System.Drawing.Size(75, 23);
            this.btn34Start.TabIndex = 3;
            this.btn34Start.Text = "Start";
            this.btn34Start.Click += new System.EventHandler(this.btn34Start_Click);
            // 
            // tabPage35
            // 
            this.tabPage35.Controls.Add(this.lbl35);
            this.tabPage35.Controls.Add(this.btnStop35);
            this.tabPage35.Controls.Add(this.btnExport35);
            this.tabPage35.Controls.Add(this.txtEnd35);
            this.tabPage35.Controls.Add(this.txtStart35);
            this.tabPage35.Controls.Add(this.listBox35);
            this.tabPage35.Controls.Add(this.btnStart35);
            this.tabPage35.Location = new System.Drawing.Point(4, 40);
            this.tabPage35.Name = "tabPage35";
            this.tabPage35.Size = new System.Drawing.Size(552, 377);
            this.tabPage35.TabIndex = 13;
            this.tabPage35.Text = "35";
            // 
            // lbl35
            // 
            this.lbl35.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbl35.Location = new System.Drawing.Point(0, 353);
            this.lbl35.Name = "lbl35";
            this.lbl35.Size = new System.Drawing.Size(552, 24);
            this.lbl35.TabIndex = 23;
            // 
            // btnStop35
            // 
            this.btnStop35.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop35.Location = new System.Drawing.Point(476, 32);
            this.btnStop35.Name = "btnStop35";
            this.btnStop35.Size = new System.Drawing.Size(72, 23);
            this.btnStop35.TabIndex = 22;
            this.btnStop35.Text = "Stop";
            this.btnStop35.Click += new System.EventHandler(this.btnStop35_Click);
            // 
            // btnExport35
            // 
            this.btnExport35.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport35.Location = new System.Drawing.Point(476, 104);
            this.btnExport35.Name = "btnExport35";
            this.btnExport35.Size = new System.Drawing.Size(72, 23);
            this.btnExport35.TabIndex = 21;
            this.btnExport35.Text = "Export";
            // 
            // txtEnd35
            // 
            this.txtEnd35.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEnd35.Location = new System.Drawing.Point(476, 80);
            this.txtEnd35.Name = "txtEnd35";
            this.txtEnd35.Size = new System.Drawing.Size(72, 20);
            this.txtEnd35.TabIndex = 20;
            this.txtEnd35.Text = "1000000";
            // 
            // txtStart35
            // 
            this.txtStart35.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStart35.Location = new System.Drawing.Point(476, 56);
            this.txtStart35.Name = "txtStart35";
            this.txtStart35.Size = new System.Drawing.Size(72, 20);
            this.txtStart35.TabIndex = 19;
            this.txtStart35.Text = "1";
            // 
            // listBox35
            // 
            this.listBox35.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox35.Location = new System.Drawing.Point(4, 8);
            this.listBox35.Name = "listBox35";
            this.listBox35.Size = new System.Drawing.Size(464, 303);
            this.listBox35.TabIndex = 18;
            // 
            // btnStart35
            // 
            this.btnStart35.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart35.Location = new System.Drawing.Point(476, 8);
            this.btnStart35.Name = "btnStart35";
            this.btnStart35.Size = new System.Drawing.Size(72, 23);
            this.btnStart35.TabIndex = 17;
            this.btnStart35.Text = "Start";
            this.btnStart35.Click += new System.EventHandler(this.btnStart35_Click);
            // 
            // tabPage36
            // 
            this.tabPage36.Controls.Add(this.txtAns36);
            this.tabPage36.Controls.Add(this.listBox36);
            this.tabPage36.Controls.Add(this.btn36start);
            this.tabPage36.Location = new System.Drawing.Point(4, 40);
            this.tabPage36.Name = "tabPage36";
            this.tabPage36.Size = new System.Drawing.Size(552, 377);
            this.tabPage36.TabIndex = 3;
            this.tabPage36.Text = "36s";
            this.tabPage36.Click += new System.EventHandler(this.tabPage36_Click);
            // 
            // txtAns36
            // 
            this.txtAns36.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAns36.Location = new System.Drawing.Point(480, 32);
            this.txtAns36.Name = "txtAns36";
            this.txtAns36.Size = new System.Drawing.Size(64, 20);
            this.txtAns36.TabIndex = 10;
            // 
            // listBox36
            // 
            this.listBox36.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox36.Location = new System.Drawing.Point(0, 8);
            this.listBox36.Name = "listBox36";
            this.listBox36.Size = new System.Drawing.Size(472, 329);
            this.listBox36.TabIndex = 9;
            // 
            // btn36start
            // 
            this.btn36start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn36start.Location = new System.Drawing.Point(480, 8);
            this.btn36start.Name = "btn36start";
            this.btn36start.Size = new System.Drawing.Size(64, 23);
            this.btn36start.TabIndex = 8;
            this.btn36start.Text = "Start";
            this.btn36start.Click += new System.EventHandler(this.btn36start_Click);
            // 
            // tabPage48
            // 
            this.tabPage48.Controls.Add(this.txt48Last10);
            this.tabPage48.Controls.Add(this.lbl48);
            this.tabPage48.Controls.Add(this.btn48Start);
            this.tabPage48.Location = new System.Drawing.Point(4, 40);
            this.tabPage48.Name = "tabPage48";
            this.tabPage48.Size = new System.Drawing.Size(552, 377);
            this.tabPage48.TabIndex = 7;
            this.tabPage48.Text = "48s";
            // 
            // txt48Last10
            // 
            this.txt48Last10.Location = new System.Drawing.Point(40, 200);
            this.txt48Last10.Name = "txt48Last10";
            this.txt48Last10.Size = new System.Drawing.Size(224, 20);
            this.txt48Last10.TabIndex = 10;
            // 
            // lbl48
            // 
            this.lbl48.Location = new System.Drawing.Point(36, 69);
            this.lbl48.Name = "lbl48";
            this.lbl48.Size = new System.Drawing.Size(224, 120);
            this.lbl48.TabIndex = 9;
            // 
            // btn48Start
            // 
            this.btn48Start.Location = new System.Drawing.Point(92, 29);
            this.btn48Start.Name = "btn48Start";
            this.btn48Start.Size = new System.Drawing.Size(96, 23);
            this.btn48Start.TabIndex = 8;
            this.btn48Start.Text = "Start";
            this.btn48Start.Click += new System.EventHandler(this.btn48Start_Click);
            // 
            // tabPage55
            // 
            this.tabPage55.Controls.Add(this.listBox55);
            this.tabPage55.Controls.Add(this.btn55Start);
            this.tabPage55.Location = new System.Drawing.Point(4, 40);
            this.tabPage55.Name = "tabPage55";
            this.tabPage55.Size = new System.Drawing.Size(552, 377);
            this.tabPage55.TabIndex = 9;
            this.tabPage55.Text = "55s";
            // 
            // listBox55
            // 
            this.listBox55.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox55.HorizontalScrollbar = true;
            this.listBox55.Location = new System.Drawing.Point(0, 8);
            this.listBox55.Name = "listBox55";
            this.listBox55.Size = new System.Drawing.Size(472, 329);
            this.listBox55.TabIndex = 11;
            // 
            // btn55Start
            // 
            this.btn55Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn55Start.Location = new System.Drawing.Point(480, 8);
            this.btn55Start.Name = "btn55Start";
            this.btn55Start.Size = new System.Drawing.Size(64, 23);
            this.btn55Start.TabIndex = 10;
            this.btn55Start.Text = "Start";
            this.btn55Start.Click += new System.EventHandler(this.btn55Start_Click);
            // 
            // tabPage56
            // 
            this.tabPage56.Controls.Add(this.listBox56);
            this.tabPage56.Controls.Add(this.txtAns56);
            this.tabPage56.Controls.Add(this.btnStart56);
            this.tabPage56.Location = new System.Drawing.Point(4, 40);
            this.tabPage56.Name = "tabPage56";
            this.tabPage56.Size = new System.Drawing.Size(552, 377);
            this.tabPage56.TabIndex = 11;
            this.tabPage56.Text = "56";
            // 
            // listBox56
            // 
            this.listBox56.Location = new System.Drawing.Point(56, 64);
            this.listBox56.Name = "listBox56";
            this.listBox56.Size = new System.Drawing.Size(200, 160);
            this.listBox56.TabIndex = 14;
            // 
            // txtAns56
            // 
            this.txtAns56.Location = new System.Drawing.Point(88, 40);
            this.txtAns56.Name = "txtAns56";
            this.txtAns56.Size = new System.Drawing.Size(128, 20);
            this.txtAns56.TabIndex = 13;
            // 
            // btnStart56
            // 
            this.btnStart56.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart56.Location = new System.Drawing.Point(376, 8);
            this.btnStart56.Name = "btnStart56";
            this.btnStart56.Size = new System.Drawing.Size(64, 23);
            this.btnStart56.TabIndex = 11;
            this.btnStart56.Text = "Start";
            this.btnStart56.Click += new System.EventHandler(this.btnStart56_Click);
            // 
            // tabPage102
            // 
            this.tabPage102.Controls.Add(this.pB102);
            this.tabPage102.Controls.Add(this.btnStop102);
            this.tabPage102.Controls.Add(this.btnStart102);
            this.tabPage102.Controls.Add(this.pic102);
            this.tabPage102.Location = new System.Drawing.Point(4, 40);
            this.tabPage102.Name = "tabPage102";
            this.tabPage102.Size = new System.Drawing.Size(552, 377);
            this.tabPage102.TabIndex = 18;
            this.tabPage102.Text = "102";
            // 
            // pB102
            // 
            this.pB102.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pB102.Location = new System.Drawing.Point(64, 11);
            this.pB102.Name = "pB102";
            this.pB102.Size = new System.Drawing.Size(424, 16);
            this.pB102.TabIndex = 18;
            // 
            // btnStop102
            // 
            this.btnStop102.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop102.Enabled = false;
            this.btnStop102.Location = new System.Drawing.Point(496, 8);
            this.btnStop102.Name = "btnStop102";
            this.btnStop102.Size = new System.Drawing.Size(48, 23);
            this.btnStop102.TabIndex = 17;
            this.btnStop102.Text = "Stop";
            this.btnStop102.Click += new System.EventHandler(this.btnStop102_Click);
            // 
            // btnStart102
            // 
            this.btnStart102.Location = new System.Drawing.Point(8, 8);
            this.btnStart102.Name = "btnStart102";
            this.btnStart102.Size = new System.Drawing.Size(48, 23);
            this.btnStart102.TabIndex = 15;
            this.btnStart102.Text = "Start";
            this.btnStart102.Click += new System.EventHandler(this.btnStart102_Click);
            // 
            // pic102
            // 
            this.pic102.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pic102.Location = new System.Drawing.Point(0, 40);
            this.pic102.Name = "pic102";
            this.pic102.Size = new System.Drawing.Size(1000, 1000);
            this.pic102.TabIndex = 0;
            this.pic102.TabStop = false;
            // 
            // tabPage104
            // 
            this.tabPage104.Controls.Add(this.lbl104Start);
            this.tabPage104.Controls.Add(this.lV104);
            this.tabPage104.Controls.Add(this.btnStop104);
            this.tabPage104.Controls.Add(this.btnStart104);
            this.tabPage104.Location = new System.Drawing.Point(4, 40);
            this.tabPage104.Name = "tabPage104";
            this.tabPage104.Size = new System.Drawing.Size(552, 377);
            this.tabPage104.TabIndex = 15;
            this.tabPage104.Text = "104";
            // 
            // lbl104Start
            // 
            this.lbl104Start.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl104Start.Location = new System.Drawing.Point(64, 16);
            this.lbl104Start.Name = "lbl104Start";
            this.lbl104Start.Size = new System.Drawing.Size(424, 136);
            this.lbl104Start.TabIndex = 18;
            // 
            // lV104
            // 
            this.lV104.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lV104.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader17,
            this.columnHeader18,
            this.columnHeader19,
            this.columnHeader9,
            this.columnHeader20,
            this.columnHeader10,
            this.columnHeader21});
            this.lV104.Location = new System.Drawing.Point(8, 40);
            this.lV104.Name = "lV104";
            this.lV104.Size = new System.Drawing.Size(536, 328);
            this.lV104.TabIndex = 17;
            this.lV104.UseCompatibleStateImageBehavior = false;
            this.lV104.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "Number";
            this.columnHeader17.Width = 37;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "Length";
            this.columnHeader18.Width = 42;
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "Start";
            this.columnHeader19.Width = 66;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "PB";
            this.columnHeader9.Width = 37;
            // 
            // columnHeader20
            // 
            this.columnHeader20.Text = "End";
            this.columnHeader20.Width = 66;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "PB";
            this.columnHeader10.Width = 37;
            // 
            // columnHeader21
            // 
            this.columnHeader21.Text = "Date";
            this.columnHeader21.Width = 111;
            // 
            // btnStop104
            // 
            this.btnStop104.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop104.Enabled = false;
            this.btnStop104.Location = new System.Drawing.Point(496, 8);
            this.btnStop104.Name = "btnStop104";
            this.btnStop104.Size = new System.Drawing.Size(48, 23);
            this.btnStop104.TabIndex = 16;
            this.btnStop104.Text = "Stop";
            this.btnStop104.Click += new System.EventHandler(this.btnStop104_Click);
            // 
            // btnStart104
            // 
            this.btnStart104.Location = new System.Drawing.Point(8, 8);
            this.btnStart104.Name = "btnStart104";
            this.btnStart104.Size = new System.Drawing.Size(48, 23);
            this.btnStart104.TabIndex = 14;
            this.btnStart104.Text = "Start";
            this.btnStart104.Click += new System.EventHandler(this.btnStart104_Click);
            // 
            // tabPageTest
            // 
            this.tabPageTest.Controls.Add(this.txtTestLength);
            this.tabPageTest.Controls.Add(this.listBoxTest);
            this.tabPageTest.Controls.Add(this.btnTestStart);
            this.tabPageTest.Location = new System.Drawing.Point(4, 40);
            this.tabPageTest.Name = "tabPageTest";
            this.tabPageTest.Size = new System.Drawing.Size(552, 377);
            this.tabPageTest.TabIndex = 10;
            this.tabPageTest.Text = "Test";
            // 
            // txtTestLength
            // 
            this.txtTestLength.Location = new System.Drawing.Point(56, 0);
            this.txtTestLength.Name = "txtTestLength";
            this.txtTestLength.Size = new System.Drawing.Size(52, 20);
            this.txtTestLength.TabIndex = 11;
            this.txtTestLength.Text = "50";
            // 
            // listBoxTest
            // 
            this.listBoxTest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxTest.HorizontalScrollbar = true;
            this.listBoxTest.Location = new System.Drawing.Point(0, 24);
            this.listBoxTest.Name = "listBoxTest";
            this.listBoxTest.Size = new System.Drawing.Size(544, 316);
            this.listBoxTest.TabIndex = 9;
            // 
            // btnTestStart
            // 
            this.btnTestStart.Location = new System.Drawing.Point(0, 0);
            this.btnTestStart.Name = "btnTestStart";
            this.btnTestStart.Size = new System.Drawing.Size(56, 23);
            this.btnTestStart.TabIndex = 8;
            this.btnTestStart.Text = "Start";
            this.btnTestStart.Click += new System.EventHandler(this.btnTestStart_Click);
            // 
            // tabPageAllPrimes
            // 
            this.tabPageAllPrimes.Controls.Add(this.btnAllPrimesLastNum);
            this.tabPageAllPrimes.Controls.Add(this.txtAllPrimesSaveAfter);
            this.tabPageAllPrimes.Controls.Add(this.btnAllPrimesStop);
            this.tabPageAllPrimes.Controls.Add(this.btnAllPrimesExport);
            this.tabPageAllPrimes.Controls.Add(this.txtAllPrimesEnd);
            this.tabPageAllPrimes.Controls.Add(this.txtAllPrimesStart);
            this.tabPageAllPrimes.Controls.Add(this.lblAllPrimes);
            this.tabPageAllPrimes.Controls.Add(this.listBoxAllPrimes);
            this.tabPageAllPrimes.Controls.Add(this.btnAllPrimesStart);
            this.tabPageAllPrimes.Location = new System.Drawing.Point(4, 40);
            this.tabPageAllPrimes.Name = "tabPageAllPrimes";
            this.tabPageAllPrimes.Size = new System.Drawing.Size(552, 377);
            this.tabPageAllPrimes.TabIndex = 8;
            this.tabPageAllPrimes.Text = "AllPrimes";
            this.tabPageAllPrimes.Click += new System.EventHandler(this.tabPageAllPrimes_Click);
            // 
            // btnAllPrimesLastNum
            // 
            this.btnAllPrimesLastNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAllPrimesLastNum.Location = new System.Drawing.Point(480, 8);
            this.btnAllPrimesLastNum.Name = "btnAllPrimesLastNum";
            this.btnAllPrimesLastNum.Size = new System.Drawing.Size(64, 23);
            this.btnAllPrimesLastNum.TabIndex = 18;
            this.btnAllPrimesLastNum.Text = "Last num";
            this.btnAllPrimesLastNum.Click += new System.EventHandler(this.btnAllPrimesBrowse_Click);
            // 
            // txtAllPrimesSaveAfter
            // 
            this.txtAllPrimesSaveAfter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAllPrimesSaveAfter.Location = new System.Drawing.Point(480, 152);
            this.txtAllPrimesSaveAfter.Name = "txtAllPrimesSaveAfter";
            this.txtAllPrimesSaveAfter.Size = new System.Drawing.Size(64, 20);
            this.txtAllPrimesSaveAfter.TabIndex = 17;
            this.txtAllPrimesSaveAfter.Text = "1000";
            // 
            // btnAllPrimesStop
            // 
            this.btnAllPrimesStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAllPrimesStop.Location = new System.Drawing.Point(480, 56);
            this.btnAllPrimesStop.Name = "btnAllPrimesStop";
            this.btnAllPrimesStop.Size = new System.Drawing.Size(64, 23);
            this.btnAllPrimesStop.TabIndex = 16;
            this.btnAllPrimesStop.Text = "Stop";
            this.btnAllPrimesStop.Click += new System.EventHandler(this.btnAllPrimesStop_Click);
            // 
            // btnAllPrimesExport
            // 
            this.btnAllPrimesExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAllPrimesExport.Location = new System.Drawing.Point(480, 128);
            this.btnAllPrimesExport.Name = "btnAllPrimesExport";
            this.btnAllPrimesExport.Size = new System.Drawing.Size(64, 23);
            this.btnAllPrimesExport.TabIndex = 15;
            this.btnAllPrimesExport.Text = "Export";
            this.btnAllPrimesExport.Click += new System.EventHandler(this.btnAllPrimesExport_Click);
            // 
            // txtAllPrimesEnd
            // 
            this.txtAllPrimesEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAllPrimesEnd.Location = new System.Drawing.Point(480, 104);
            this.txtAllPrimesEnd.Name = "txtAllPrimesEnd";
            this.txtAllPrimesEnd.Size = new System.Drawing.Size(64, 20);
            this.txtAllPrimesEnd.TabIndex = 14;
            this.txtAllPrimesEnd.Text = "100000000";
            this.txtAllPrimesEnd.TextChanged += new System.EventHandler(this.txtAllPrimesEnd_TextChanged);
            // 
            // txtAllPrimesStart
            // 
            this.txtAllPrimesStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAllPrimesStart.Location = new System.Drawing.Point(480, 80);
            this.txtAllPrimesStart.Name = "txtAllPrimesStart";
            this.txtAllPrimesStart.Size = new System.Drawing.Size(64, 20);
            this.txtAllPrimesStart.TabIndex = 13;
            this.txtAllPrimesStart.Text = "1";
            // 
            // lblAllPrimes
            // 
            this.lblAllPrimes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblAllPrimes.Location = new System.Drawing.Point(0, 329);
            this.lblAllPrimes.Name = "lblAllPrimes";
            this.lblAllPrimes.Size = new System.Drawing.Size(552, 48);
            this.lblAllPrimes.TabIndex = 12;
            // 
            // listBoxAllPrimes
            // 
            this.listBoxAllPrimes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxAllPrimes.Location = new System.Drawing.Point(0, 8);
            this.listBoxAllPrimes.Name = "listBoxAllPrimes";
            this.listBoxAllPrimes.Size = new System.Drawing.Size(472, 316);
            this.listBoxAllPrimes.TabIndex = 11;
            // 
            // btnAllPrimesStart
            // 
            this.btnAllPrimesStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAllPrimesStart.Location = new System.Drawing.Point(480, 32);
            this.btnAllPrimesStart.Name = "btnAllPrimesStart";
            this.btnAllPrimesStart.Size = new System.Drawing.Size(64, 23);
            this.btnAllPrimesStart.TabIndex = 10;
            this.btnAllPrimesStart.Text = "Start";
            this.btnAllPrimesStart.Click += new System.EventHandler(this.btnAllPrimesStart_Click);
            // 
            // tabPageThreadPrio
            // 
            this.tabPageThreadPrio.Controls.Add(this.lVPrio);
            this.tabPageThreadPrio.Location = new System.Drawing.Point(4, 40);
            this.tabPageThreadPrio.Name = "tabPageThreadPrio";
            this.tabPageThreadPrio.Size = new System.Drawing.Size(552, 377);
            this.tabPageThreadPrio.TabIndex = 17;
            this.tabPageThreadPrio.Text = "Prio";
            // 
            // lVPrio
            // 
            this.lVPrio.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lVPrio.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.lVPrio.ContextMenu = this.contextMenu1;
            this.lVPrio.Location = new System.Drawing.Point(8, 8);
            this.lVPrio.Name = "lVPrio";
            this.lVPrio.Size = new System.Drawing.Size(536, 360);
            this.lVPrio.TabIndex = 0;
            this.lVPrio.UseCompatibleStateImageBehavior = false;
            this.lVPrio.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Thread";
            this.columnHeader6.Width = 92;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Prio";
            this.columnHeader7.Width = 87;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "State";
            this.columnHeader8.Width = 76;
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItem3});
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuItemR,
            this.mnuItemS,
            this.mnuItemNR});
            this.menuItem1.Text = "State";
            // 
            // mnuItemR
            // 
            this.mnuItemR.Index = 0;
            this.mnuItemR.Text = "Running";
            this.mnuItemR.Click += new System.EventHandler(this.mnuItemR_Click);
            // 
            // mnuItemS
            // 
            this.mnuItemS.Index = 1;
            this.mnuItemS.Text = "Suspended";
            this.mnuItemS.Click += new System.EventHandler(this.mnuItemS_Click);
            // 
            // mnuItemNR
            // 
            this.mnuItemNR.Index = 2;
            this.mnuItemNR.Text = "Not running";
            this.mnuItemNR.Click += new System.EventHandler(this.mnuItemNR_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "-";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuItemH,
            this.mnuItemAN,
            this.mnuItemN,
            this.mnuItemBN,
            this.mnuItemL});
            this.menuItem3.Text = "Priority";
            // 
            // mnuItemH
            // 
            this.mnuItemH.Index = 0;
            this.mnuItemH.Text = "Highest";
            this.mnuItemH.Click += new System.EventHandler(this.mnuItemH_Click);
            // 
            // mnuItemAN
            // 
            this.mnuItemAN.Index = 1;
            this.mnuItemAN.Text = "AboveNormal";
            this.mnuItemAN.Click += new System.EventHandler(this.mnuItemAN_Click);
            // 
            // mnuItemN
            // 
            this.mnuItemN.Index = 2;
            this.mnuItemN.Text = "Normal";
            this.mnuItemN.Click += new System.EventHandler(this.mnuItemN_Click);
            // 
            // mnuItemBN
            // 
            this.mnuItemBN.Index = 3;
            this.mnuItemBN.Text = "BelowNormal";
            this.mnuItemBN.Click += new System.EventHandler(this.mnuItemBN_Click);
            // 
            // mnuItemL
            // 
            this.mnuItemL.Index = 4;
            this.mnuItemL.Text = "Lowest";
            this.mnuItemL.Click += new System.EventHandler(this.mnuItemL_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Factors";
            this.columnHeader1.Width = 85;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Sum";
            this.columnHeader2.Width = 81;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Number";
            this.columnHeader3.Width = 97;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Divs";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "DivCount";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(560, 421);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "MathsChallenge";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage12.ResumeLayout(false);
            this.tabPage12.PerformLayout();
            this.tabPage1212.ResumeLayout(false);
            this.tabPage1212.PerformLayout();
            this.tabPage15.ResumeLayout(false);
            this.tabPage15.PerformLayout();
            this.tabPage16.ResumeLayout(false);
            this.tabPage16.PerformLayout();
            this.tabPage18.ResumeLayout(false);
            this.tabPage20.ResumeLayout(false);
            this.tabPage20.PerformLayout();
            this.tabPage25.ResumeLayout(false);
            this.tabPage25.PerformLayout();
            this.tabPage29.ResumeLayout(false);
            this.tabPage29.PerformLayout();
            this.tabPage34.ResumeLayout(false);
            this.tabPage34.PerformLayout();
            this.tabPage35.ResumeLayout(false);
            this.tabPage35.PerformLayout();
            this.tabPage36.ResumeLayout(false);
            this.tabPage36.PerformLayout();
            this.tabPage48.ResumeLayout(false);
            this.tabPage48.PerformLayout();
            this.tabPage55.ResumeLayout(false);
            this.tabPage56.ResumeLayout(false);
            this.tabPage56.PerformLayout();
            this.tabPage102.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic102)).EndInit();
            this.tabPage104.ResumeLayout(false);
            this.tabPageTest.ResumeLayout(false);
            this.tabPageTest.PerformLayout();
            this.tabPageAllPrimes.ResumeLayout(false);
            this.tabPageAllPrimes.PerformLayout();
            this.tabPageThreadPrio.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/*
		static void Main() 
		{
			Application.Run(new Form1());
		}
		*/
		public static bool IsOneInstance() 
		{ 
			Process pcur = Process.GetCurrentProcess(); 
			Process[] ps = Process.GetProcesses(); 
			
			foreach( Process p in ps ) 
			{ 
				if ( pcur.Id  != p.Id ) 
					if ( pcur.ProcessName == p.ProcessName ) 
						return false; 
			} 
			return true; 
		} 


		static void Main(string[] sCmdLine) 
		{
			Process pcur = Process.GetCurrentProcess();
			pcur.PriorityClass=ProcessPriorityClass.Idle;

			if(IsOneInstance()) 
			{
				strCmdLine = sCmdLine;
				Application.Run(new Form1());
			}
		}



		void run1212()
		{
			Cursor.Current=Cursors.WaitCursor;
			btnStart1212.Enabled=false;
			
			string s_divs="";
			string strFacts = "";
			
			int v_cnt = int.Parse(getStartTri());
			txtStart1212.Text=v_cnt.ToString();

			int v_sum =(((v_cnt*v_cnt) / 2) + (v_cnt/2));

			for(int xx=1;xx<v_cnt;xx++)
			{
				strFacts += xx.ToString() + ",";
			}

			//MessageBox.Show("v_cnt:" + v_cnt.ToString() + "\nv_sum:" + v_sum.ToString());

			lV1212.Items.Clear();
			b_running1212=true;

			
			
			while(b_running1212)
			{
				v_cnt++;
				if(!b_running1212)
				{
					break;
				}

				b_running1212=(v_cnt<v_end1212);


				strFacts += v_cnt.ToString() + ",";
				v_sum=v_sum + v_cnt;

				s_divs = getDivisors((double)v_sum);
				string[] s_divsarr = s_divs.Split((char)44);
				if(s_divsarr.Length>100)
				{
			
					addToTriangles(v_sum.ToString(),strFacts.Substring(0,strFacts.Length-1),v_cnt.ToString(),s_divs);
			
					ListViewItem new_item = lV1212.Items.Add(strFacts.Substring(0,strFacts.Length-1));
					new_item.SubItems.Add(v_sum.ToString());
					new_item.SubItems.Add(v_cnt.ToString());
					new_item.SubItems.Add(s_divs);
					new_item.SubItems.Add(s_divsarr.Length.ToString());

					if (s_divsarr.Length>=490)
					{
						//MessageBox.Show(v_cnt + " has "+ s_divsarr.Length.ToString() +" divisors!");
						//Clipboard.SetDataObject(v_cnt,true);
						//break;
					}

					Application.DoEvents();
					GC.Collect();
				}

				if(!b_running1212)
				{
					break;
				}

				b_running1212=(v_cnt<v_end1212);
			}
			
			btnStart1212.Enabled=true;

			Cursor.Current=Cursors.Default;
			t1212.Abort();
		}
		

		void run12()
		{
			Cursor.Current=Cursors.WaitCursor;
			btn12Browse.Enabled=false;
			btn12Start.Enabled=false;
			btnExport12.Enabled=false;
			string new_Divisors="";
			string new_Divisors_2="";
			char chsplit = (char)44;
			string[] divArr;
			string[] divArr_2;
			int v_cnt=0;
			int i_to=0;
			listBox.Items.Clear();
			b_running12=true;
			v_start12++;
			if(!isEven(v_start12))
			{
				v_start12++;
			}
			//alterPrimeDB("MEMORY");
			//loadPrimes();
			loadDivisors();
			//alterPrimeDB("MYISAM");
			for(int i = v_start12;i<=v_end12;i=i+2)
			{
				i_to=v_end12-i;
				//if(isEven(i))
				//{
					
					if(!htctrl.ContainsKey(i) && !htctrl.ContainsKey(i_to))//!isDbPrime(i))
					{
						new_Divisors = getDivisors(i);
						divArr = new_Divisors.Split(chsplit);
						new_Divisors_2 = getDivisors(i_to);
						divArr_2 = new_Divisors_2.Split(chsplit);
						if(!b_running12)
						{
							break;
						}
						if (divArr.Length>=int.Parse(txt12Log.Text))
						{
							addToDivisors(i.ToString(),new_Divisors,divArr.Length.ToString());
								
							if (divArr.Length>500)
							{
								MessageBox.Show(i + " är det första!");
								Clipboard.SetDataObject(i,true);
								break;
							}
							if(!chkRealtime.Checked)
							{
								pB1.Value=int.Parse(i.ToString());
								listBox.Items.Add(i+ "	"+ divArr.Length + "	" + DateTime.Now.ToString());
								listBox.SelectedIndex=listBox.Items.Count-1;

								Application.DoEvents();
							}
						}
						if (divArr_2.Length>=int.Parse(txt12Log.Text))
						{
							addToDivisors(i_to.ToString(),new_Divisors_2,divArr_2.Length.ToString());

							if(!chkRealtime.Checked)
							{
								listBox.Items.Add(i_to+ "	"+ divArr_2.Length + "	" + DateTime.Now.ToString());
								listBox.SelectedIndex=listBox.Items.Count-1;
								Application.DoEvents();
							}
						}

						GC.Collect();
						if(!b_running12)
						{
							break;
						}
						v_cnt++;
					}
				//}

			}
			btn12Browse.Enabled=true;
			btn12Start.Enabled=true;
			btnExport12.Enabled=true;
			Cursor.Current=Cursors.Default;
			t12.Abort();
		}
		void run15()
		{
			
			int v_len = int.Parse(txt15Width.Text);
			
			b_running15=true;
			lV15.Items.Clear();

			int v_steps = 0;
			int v_x = 0;
			int v_y = 0;
			string strRoute = "";
			bool bDone = false;
			while(!bDone)
			{
				string[] strRouteArr = strRoute.Split((char)124);
				if(strRouteArr.Length>v_len)
				{
					strRoute="0,0|";
					v_x=0;
					v_y=0;
				}
				strRoute += "";
				if(countThisRoute(strRoute)<v_len-1)
				{
				
				}

				//tblSqRoutes

			}
			
			
			
			t15.Abort();
		}
		void run20()
		{
			BigInteger bi = new BigInteger(1);
			BigInteger startbi = new BigInteger(int.Parse(txtNum20.Text));
			b_running20=true;
			for(BigInteger i = startbi;i>=1;i--)
			{
				if(!b_running20)
				{
					break;
				}
				bi=bi*i;
			}
			
			lbl20.Text=bi.ToString();
			double num_sum=getNumSum(bi.ToString());
			txt20Ans.Text=num_sum.ToString();
			
			t20.Abort();
		}


		void run25()
		{
			//Fn = Fn–1 + Fn–2, where F1 = 1 and F2 = 1
			
			BigInteger F1= new BigInteger(1);
			BigInteger F2= new BigInteger(1);
			BigInteger Fn= new BigInteger(1);
			int i=int.Parse(txt25Start.Text)+3;
			b_running25=true;
			while(b_running25 && !Fn.ToString().Equals("INF"))
			{
				F2 = F1;
				F1 = Fn;
				Fn = F1 + F2;
				if(i%10==0)
				{
					Application.DoEvents();
					lbl25Info.Text = i + "\n" + (Fn + "\n" + Fn.ToString().Length);
				}
				if (Fn.ToString().Length>=int.Parse(txt25Length.Text))
				{
					lbl25Info.Text ="The first term in the Fibonacci sequence to contain "+ txt25Length.Text +"-digits is:" + (i-1) + " Fn.ToString().Length: " + Fn.ToString().Length;
					break;
				}
				GC.Collect();
				i++;
				
			}
			t25.Abort();
		}

		/*
		void run25()
		{
			//Fn = Fn–1 + Fn–2, where F1 = 1 and F2 = 1
			
			double F1=1;
			double F2=1;
			double Fn=1;
			int i=int.Parse(txt25Start.Text)+3;
			b_running25=true;
			while(b_running25 && !Fn.Equals("INF"))
			{
				F2 = F1;
				F1 = Fn;
				Fn = double.Parse(getRealVal(F1.ToString())) + double.Parse(getRealVal(F2.ToString())) + 0;
				Application.DoEvents();
				lbl25Info.Text = i + "\n" + (Fn + "\n" + getRelLen(Fn.ToString()));
				if (double.Parse(getRelLen(Fn.ToString()))>=double.Parse(txt25Length.Text))
				{
					lbl25Info.Text ="The first term in the Fibonacci sequence to contain "+ txt25Length.Text +"-digits is:" + i + " getRelLen(Fn): " + getRelLen(Fn.ToString());
					break;
				}
				GC.Collect();
				i++;
				
			}
			t25.Abort();
		}
		*/
		void run34()
		{
			double v_sum=0;
			
			string s_num="";
			string strfs="";
			double dcurrf_num=0;

			pB34.Maximum=int.Parse(txt34End.Text);
			v_start34=double.Parse(txt34Start.Text);
			v_end34=double.Parse(txt34End.Text);

			listBox34.Items.Clear();
			b_running34=true;

			for(double v_num=v_start34;v_num<v_end34;v_num++)
			{

				s_num=v_num.ToString();
				strfs="";
				v_sum=0;
				if(!b_running34)
				{
					break;
				}
				for(int i=0;i<s_num.Length;i++)
				{
					if(!b_running34)
					{
						break;
					}
					dcurrf_num=getFactorials(double.Parse(s_num.Substring(i,1)));
					strfs+=dcurrf_num +",";
					v_sum+=dcurrf_num;
				}
				if(!b_running34)
				{
					break;
				}
				if((s_num.Length>1 && v_num.Equals(v_sum)) || (v_num%100000)==0)
				{
					listBox34.Items.Add(v_num + "	" + strfs + "	" + v_sum);
				}
				if((v_num%10000)==0)
				{
					pB34.Value=int.Parse(v_num.ToString());
					Application.DoEvents();
				}
				GC.Collect();
			}
			pB34.Value=int.Parse(txt34End.Text);
			MessageBox.Show("34-Done");
			t34.Abort();
		}


		void run35()
		{
						
			if(!b_PrimesLoaded)
			{
				lbl35.Text="Altering database...";
				alterPrimeDB("MEMORY");

				lbl35.Text="Loading primes...";
				loadPrimes(1,1000000);

				lbl35.Text="Altering database back...";
				alterPrimeDB("MYISAM");
			}

			lbl35.Text="Searching for primes...";

			int v_start=int.Parse(txtStart35.Text);
			int v_end=int.Parse(txtEnd35.Text);

			int v_num=0;
			int v_cnt=0;
			string[] v_numAnagram;

			int v_i=0;
			
			foreach(object k in htctrl.Keys)
			{
				if(!b_running35 || v_num > v_end)
				{
					goto exitrun;
				}

				v_num=int.Parse(k.ToString());

				if(v_num>=v_start && v_num < v_end)
				{
					v_numAnagram = getAnagrams(v_num);

					for(v_i=0;v_i<v_numAnagram.Length;v_i++)
					{
						if(!b_running35 || v_num > v_end)
						{
							goto exitrun;
						}
						if(!htctrl.ContainsKey((object)v_numAnagram[v_i].ToString()))
						{
							break;
						}
						if(!b_running35 || v_num< v_end)
						{
							goto exitrun;
						}
					}
					if(v_i==v_numAnagram.Length-1)
					{
						listBox35.Items.Add(v_num + " true");
						v_cnt++;
					}
					else
					{
						listBox35.Items.Add(v_num + " false");
					}
				}

				if(!b_running35 || v_num > v_end)
				{
					goto exitrun;
				}
			}

			exitrun:

			lbl35.Text="Suspending thread...";
			t35.Abort();
			lbl35.Text="Exiting run35()...";
		}



		void run36()
		{
			string strBin="";
			int v_sum = 0;
			for(int i = 1;i<1000000;i++)
			{
				if(ispalindrome(i.ToString()))
				{
					strBin=Convert.ToString(i,2);
					if(ispalindrome(strBin))
					{
						listBox36.Items.Add(i + " " + strBin);
						v_sum=v_sum+i;
					}
				}
			}
			txtAns36.Text=v_sum.ToString();
		}
		void run56()
		{
			double v_sum = 0;
			double o_sum = 0;
			double biSum=0;
			for(double a = 1;a<100;a++)
			{
				for(double b = 1;b<100;b++)
				{
					biSum=Math.Pow(a,b);
					v_sum = getNumSum(DoubleConverter.ToExactString(biSum));
					if(v_sum>o_sum)
					{
						o_sum=v_sum;
						listBox56.Items.Add(a + " " + b + " " + biSum);
					}
				}
			}
			txtAns56.Text=o_sum.ToString();
		}
		void run29()
		{
			double biSum=0;
			Hashtable htUnique=new Hashtable();
			for(double a = 2;a<=100;a++)
			{
				for(double b = 2;b<=100;b++)
				{
					biSum=Math.Pow(a,b);
					if(!htUnique.ContainsKey(DoubleConverter.ToExactString(biSum)))
					{
						htUnique.Add(DoubleConverter.ToExactString(biSum),DoubleConverter.ToExactString(biSum));
						ListViewItem new_itm = lV29.Items.Add(a.ToString());
						new_itm.SubItems.Add(b.ToString());
						new_itm.SubItems.Add(biSum.ToString());
					}

				}
			}
			txtAns29.Text=htUnique.Count.ToString();
		}
		Bitmap drawGraph(Bitmap in_bit)
		{
		
			
			Graphics g = Graphics.FromImage(in_bit);
			
			Point aPoint = new Point(1,pic102.Height/2);
			Point bPoint = new Point(pic102.Width,pic102.Height/2);
			
			Point cPoint = new Point(pic102.Width/2,1);
			Point dPoint = new Point(pic102.Width/2,pic102.Width);
			
			Pen p = new Pen(Color.Black,1);

			p.DashStyle=System.Drawing.Drawing2D.DashStyle.Dash;

			g.DrawLine(p,aPoint,bPoint);
			g.DrawLine(p,cPoint,dPoint);
			return in_bit;

		}
		void run102()
		{
			b_running102=true;
			btnStart102.Enabled=false;
			btnStop102.Enabled=true;
			Bitmap bit = new Bitmap(pic102.Width, pic102.Height);

			//bit=drawGraph(bit);

			int v_cnt=0;
			int v_row_cnt=0;
			string v_filename = @"C:\blandat\SCRIPT\mathchallenge\vbs\html\project\triangles.txt";
			
			if(v_filename!="")
			{
				FileInfo fi = new FileInfo(v_filename);
				StreamReader sr = (StreamReader)File.OpenText(v_filename);
				string v_line="";
				while(sr.Peek()!=-1 && v_row_cnt<10000 && b_running102)
				{
					v_line = sr.ReadLine();
					if (v_line!="")
					{
						if(bOriginWithinTriangle(v_line))
						{
							//bit = createTriBit(v_line,bit);
							v_cnt++;
						}
					}
					v_row_cnt++;
				}
			}
			MessageBox.Show(v_cnt.ToString() +" of " +v_row_cnt.ToString() + " triangle(s) contains origo.");

			//pic102.Image = bit;
			//pic102.Refresh();

			b_running102=false;
			btnStart102.Enabled=true;
			btnStop102.Enabled=false;

		}
		bool bOriginWithinTriangle(string in_tri_text)
		{
			int v_cnt=0;
			string[] s_p = in_tri_text.Split((char)44);

			Point aPoint = new Point(getRealX(int.Parse(s_p[0])),getRealY(int.Parse(s_p[1])));
			Point bPoint = new Point(getRealX(int.Parse(s_p[2])),getRealY(int.Parse(s_p[3])));
			Point cPoint = new Point(getRealX(int.Parse(s_p[4])),getRealY(int.Parse(s_p[5])));
			
			//a->b
			//b->c
			//c->a

			Brush nb = Brushes.White;
			Rectangle rect = new Rectangle(0,0,1000,1000);
			Region r = new Region(rect);

			
			Point oPoint = new Point(pic102.Width/2,pic102.Height/2); //origo
			Bitmap bit = new Bitmap(pic102.Width, pic102.Height);

			Graphics g = Graphics.FromImage(bit);
			g.FillRegion(nb,r);
			g.DrawLine(Pens.Black,aPoint,bPoint);
			g.DrawLine(Pens.Black,cPoint,bPoint);
			g.DrawLine(Pens.Black,cPoint,aPoint);
			
			//g.DrawEllipse (Pens.Blue,oPoint.X,oPoint.Y,3,3);

			//pic102.Image = bit;
			//pic102.Refresh();
			

			int v_min_x=Math.Min(Math.Min(aPoint.X,bPoint.X),cPoint.X);
			int v_min_y=Math.Min(Math.Min(aPoint.Y,bPoint.Y),cPoint.Y);
			int v_max_x=Math.Max(Math.Max(aPoint.X,bPoint.X),cPoint.X);
			int v_max_y=Math.Max(Math.Max(aPoint.Y,bPoint.Y),cPoint.Y);

			for(int i_x=oPoint.X;i_x<=v_max_x+2;i_x++)
			{
				try
				{
					Color v_color=bit.GetPixel(i_x,oPoint.Y);

					//g.DrawLine(Pens.Black,new Point(v_min_x,oPoint.Y),new Point(i_x,oPoint.Y));

					if(v_color.R==Color.Black.R && v_color.G==Color.Black.G && v_color.B==Color.Black.B)
					{
						v_cnt++;
						Console.WriteLine(i_x.ToString() + "| " + v_cnt.ToString() + "| " + v_color.R.ToString() + "," + v_color.G.ToString() + "," + v_color.B.ToString() + "|"  + Color.Black.R.ToString() + "," + Color.Black.G.ToString() + "," + Color.Black.B.ToString());
					}
				}
				catch(Exception ex)
				{
				Console.WriteLine (ex.Message);
				
				}
			}

			pic102.Image = bit;
			pic102.Refresh();


			return (v_cnt>1);
		}
		Bitmap createTriBit(string in_tri_text,Bitmap in_bit)
		{

			Graphics g = Graphics.FromImage(in_bit);
			
			string[] s_p = in_tri_text.Split((char)44);

			Point aPoint = new Point(getRealX(int.Parse(s_p[0])),getRealY(int.Parse(s_p[1])));
			Point bPoint = new Point(getRealX(int.Parse(s_p[2])),getRealY(int.Parse(s_p[3])));
			Point cPoint = new Point(getRealX(int.Parse(s_p[4])),getRealY(int.Parse(s_p[5])));

			g.DrawLine(Pens.Black,aPoint,bPoint);
			g.DrawLine(Pens.Black,cPoint,bPoint);
			g.DrawLine(Pens.Black,cPoint,aPoint);

			return in_bit;
		}

		int getRealX(int in_x)
		{
			in_x=in_x/2;
			if(in_x<0)
			{
				return (pic102.Width/2)+(in_x);
			}
			else
			{
				return (pic102.Width/2)+(in_x);
			}
		}

		int getRealY(int in_y)
		{
			in_y=in_y/2;
			if(in_y<0)
			{
				return (pic102.Height/2)+(in_y*-1);
			}
			else
			{
				return (pic102.Height/2)-(in_y);
			}
		}

		void run104()
		{
			
			b_running104=true;
			btnStart104.Enabled=false;
			btnStop104.Enabled=true;

			lbl104Start.Text="Getting start fib";
			int i=getStartFib();//

			lbl104Start.Text="Getting start fibvals";
			BigInteger[] FS= getStartFibVal();

			BigInteger F1= FS[2];

			BigInteger F2= FS[1];
			
			BigInteger Fn= FS[0];

			string v_start = "";
			string v_end = "";
			string strFn = "";
			int FnLen = 0;
			bool bStartPB=false;
			bool bEndPB=false;
			int v_maxCnt=12;
			int v_lV104Height=lV104.Height;
			lbl104Start.Text="Start fibnum: " + i.ToString();

			//addToFibonacci("1","1",1,false,false);
			//addToFibonacci("2","1",1,false,false);
			//try
			//{
				strFn = Fn.ToString();
				FnLen = strFn.Length;
				ListViewItem nItm;
				
				while(b_running104 && !strFn.Equals("INF"))
				{
					bStartPB=false;
					bEndPB=false;

					F2 = F1;
					F1 = Fn;
					Fn = F1 + F2;
					strFn = Fn.ToString();
					FnLen = strFn.Length;

					//if((i+2)%10==0)
					//{
						Application.DoEvents();

					//}
					if(lV104.Height>0)
					{
						v_lV104Height=lV104.Height;
					}
					double lvH = (double)v_lV104Height;
					
					lvH = (lvH-17) / 15;

					v_maxCnt = int.Parse(Math.Round(lvH,0).ToString());
					int v_countTrue = 0;

					while(lV104.Items.Count>0 && lV104.Items.Count>v_maxCnt && v_countTrue<v_maxCnt)
					{
						if(lV104.Items[0].SubItems[3].Text.Equals("False") && lV104.Items[0].SubItems[5].Text.Equals("False"))
						{
							lV104.Items[0].Remove();
						}
						else
						{
							try
							{
								ListViewItem lvtemp1 = lV104.Items[0];
								ListViewItem lvtemp2 = lV104.Items[lV104.Items.Count-1];

								lV104.Items[0]=null;
								lV104.Items[lV104.Items.Count-1]=null;

								lV104.Items[0]=lvtemp2;
								lV104.Items[lV104.Items.Count-1]=lvtemp1;
							}
							catch
							{}
							v_countTrue++;
						}
					}
					if(FnLen>=18)
					{
						v_start = strFn.Substring(0,9);
						v_end = strFn.Substring(FnLen-9,9);

						bStartPB=isPandigital(v_start);
						bEndPB=isPandigital(v_end);

						nItm = lV104.Items.Add((i+2).ToString());
						nItm.SubItems.Add(FnLen.ToString());
						nItm.SubItems.Add(v_start);
						nItm.SubItems.Add(bStartPB.ToString());
						nItm.SubItems.Add(v_end);
						nItm.SubItems.Add(bEndPB.ToString());
						nItm.SubItems.Add(DateTime.Now.ToString());
			
					}
					else
					{

						nItm = lV104.Items.Add((i+2).ToString());
						nItm.SubItems.Add(FnLen.ToString());
						nItm.SubItems.Add("");
						nItm.SubItems.Add("");
						nItm.SubItems.Add("");
						nItm.SubItems.Add("");
						nItm.SubItems.Add(DateTime.Now.ToString());
					}

					addToFibonacci((i+2).ToString(),strFn,FnLen,bStartPB,bEndPB);

					if(bStartPB && bEndPB)
					{
						nItm = lV104.Items.Add((i+2).ToString());
						nItm.SubItems.Add(FnLen.ToString());
						nItm.SubItems.Add(v_start + " true");
						nItm.SubItems.Add(v_end + " true");
						nItm.SubItems.Add(DateTime.Now.ToString());
						b_running104=false;
						break;
					}
					lV104.Items[lV104.Items.Count-1].Focused=true;
			
					GC.Collect();
					i++;
				}
			/*}
			catch
			{
			
			}*/
			b_running104=false;
			btnStart104.Enabled=true;
			btnStop104.Enabled=false;

			t104.Abort();
		}



		bool isPandigital(string in_val)
		{
			bool v_ret=true;
			for(int xx = 1;xx<=9;xx++)
			{
				if(in_val.IndexOf(xx.ToString())==-1)
				{
					v_ret=false;
					break;
				}
			}
			return v_ret;
				
		}

		bool isPandigital(string in_val,int in_start,int in_end)
		{
			bool v_ret=true;
			for(int xx = in_start;xx<=in_end;xx++)
			{
				if(in_val.IndexOf(xx.ToString())==-1)
				{
					v_ret=false;
					break;
				}
			}
			return v_ret;
				
		}
		void run18()
		{
			listBox18.Items.Clear();
			b_running18=true;
			countRoutes(-1,0,0);
		}
		void run16()
		{
			double dStart = Math.Pow(2,1000);
			lbl16.Text=DoubleConverter.ToExactString(dStart);
			txt16Sum.Text=getNumSum(DoubleConverter.ToExactString(dStart)).ToString();
		}

		void run48()
		{
			BigInteger dSum = new BigInteger(0);
			BigInteger dTempSum = new BigInteger(0);
			
			for(BigInteger d= new BigInteger(1);d<=1000;d++)
			{
				dTempSum=d;
				for(BigInteger xx = new BigInteger(1);xx<d;xx++)
				{
					dTempSum = dTempSum*d;
				}

				dSum+=dTempSum;
				
				if(d%10==0)
				{
					lbl48.Text=d.ToString() + ":" + dSum;
				}
			}

			string dNums = dSum.ToString();
			//lbl48.Text=dNums;
			if (dNums.Length>10)
			{
				txt48Last10.Text="Last 10:" + dNums.Substring(dNums.Length-10,10);
			}
			
		}

		void run55()
		{
			double v_cnt=0;
			double v_numf = 0;
			double v_numb = 0;
			for(double i = 10;i<=10000;i++)
			{
				v_cnt=0;
				v_numf = i;
				do
				{
					v_numb = getBackwards(DoubleConverter.ToExactString(v_numf));
					v_numf = v_numf+v_numb;
					v_cnt++;
					//listBox55.Items.Add(i + "\t" + v_numf + "\t" + v_numb + "\t" + ispalindrome(DoubleConverter.ToExactString(v_numf)) + "\t" + v_cnt);
				}
				while(!((ispalindrome(DoubleConverter.ToExactString(v_numf)) && !v_numf.Equals(i)) || v_cnt>1000 || DoubleConverter.ToExactString(v_numf).Length>308));
				listBox55.Items.Add(i + "\t" + v_numf + "\t" + v_numb + "\t" + ispalindrome(DoubleConverter.ToExactString(v_numf)) + "\t" + v_cnt);
				listBox55.SelectedIndex=listBox55.Items.Count-1;
				writetofile55(i + "\t" + v_numf + "\t" + v_numb + "\t" + ispalindrome(DoubleConverter.ToExactString(v_numf)) + "\t" + v_cnt);
			}
			MessageBox.Show("55-done");
		}
		void alterPrimeDB(string db_type)
		{
			b_PrimesdbAltered=true;
			string MyConString = "DRIVER={MySQL ODBC 3.51 Driver};" +
				"SERVER=localhost;" +
				"DATABASE=Primes;" +
				"UID=primes;" +
				"OPTION=3";

			OdbcConnection odbcconn = new OdbcConnection(MyConString);
			odbcconn.Open();

			OdbcCommand odbccom=new OdbcCommand("ALTER TABLE primes.primes ENGINE = " + db_type + ";",odbcconn);
			odbccom.ExecuteNonQuery();
			odbccom.Dispose();

			odbcconn.Close();
			odbcconn.Dispose();
		}
		void runAllPrimes()
		{
			b_runningAllPrimes=true;

			Hashtable ht = new Hashtable();

			if(!b_PrimesLoaded)
			{
				loadPrimes();
			}
            this.Invoke((MethodInvoker)delegate{
                lblAllPrimes.Text = "Searching for primes...";
            });

			v_startPrime=int.Parse(txtAllPrimesStart.Text);
			v_endPrime = int.Parse(txtAllPrimesEnd.Text);

			int v_num1=0;
			int v_bnum1=0;
			int v_num2=0;
			int v_cnt=0;

			if(!isOdd(v_startPrime))
			{
				v_startPrime++;
			}
			if(isOdd(v_endPrime))
			{
				v_endPrime++;
			}
			for(int i = v_startPrime;i<=v_endPrime;i=i+2)
			{
				if(!b_runningAllPrimes)
				{
					break;
				}
				v_num1=i;
				v_num2=v_endPrime-i;
                double v_num3 = Math.Floor(((double)v_num2) / 2);
                double v_num4 = Math.Floor(((double)v_num3) / 2);
                double v_num5 = Math.Floor(((double)v_num4) / 2);
                if (!isOdd(v_num3))
                {
                    v_num3++;
                }
                if (!isOdd(v_num4))
                {
                    v_num4++;
                }
                if (!isOdd(v_num5))
                {
                    v_num5++;
                }

                this.Invoke((MethodInvoker)delegate
                {
                    txtAllPrimesStart.Text = v_startPrime.ToString();
                    txtAllPrimesEnd.Text = v_endPrime.ToString();
                });
				if((v_cnt%50)==0)
				{
                    this.Invoke((MethodInvoker)delegate
                    {
                        lblAllPrimes.Text = v_num1.ToString() + " - " + v_num2.ToString() + " - " + v_num3.ToString() + " - " + v_num4.ToString() + " - " + v_num5.ToString() + "\n" + htctrl.Count.ToString() + " - " + ht.Count.ToString() + "\n" + listBoxAllPrimes.Items.Count.ToString();
                    });
				}
				BigInteger bi1 = new BigInteger(v_num1);
				BigInteger bi2 = new BigInteger(v_num2);
					
				if (isOdd(v_num1) && !htctrl.ContainsKey(v_num1) && !ht.ContainsKey(v_num1) && bi1.isProbablePrime(10))// isPrime(v_num1))
				{
					ht.Add(v_num1,v_num1);
					htctrl.Add(v_num1,v_num1);
				}
				if (isOdd(v_num2) && !htctrl.ContainsKey(v_num2) && !ht.ContainsKey(v_num2) && bi2.isProbablePrime(10))// isPrime(v_num2))
				{
					ht.Add(v_num2,v_num2);
					htctrl.Add(v_num2,v_num2);
				}
                if (isOdd(v_num3) && !htctrl.ContainsKey(v_num3) && !ht.ContainsKey(v_num3) && bi2.isProbablePrime(10))// isPrime(v_num3))
                {
                    ht.Add(v_num3, v_num3);
                    htctrl.Add(v_num3, v_num3);
                }
                if (isOdd(v_num4) && !htctrl.ContainsKey(v_num4) && !ht.ContainsKey(v_num4) && bi2.isProbablePrime(10))// isPrime(v_num4))
                {
                    ht.Add(v_num4, v_num4);
                    htctrl.Add(v_num4, v_num4);
                }
                if (isOdd(v_num5) && !htctrl.ContainsKey(v_num5) && !ht.ContainsKey(v_num5) && bi2.isProbablePrime(10))// isPrime(v_num5))
                {
                    ht.Add(v_num5, v_num5);
                    htctrl.Add(v_num5, v_num5);
                }
                string strtxtAllPrimesSaveAfter = "";
                this.Invoke((MethodInvoker)delegate
                {
                    strtxtAllPrimesSaveAfter=txtAllPrimesSaveAfter.Text;
                });
                if (ht.Count >= int.Parse(strtxtAllPrimesSaveAfter))
				{
                    this.Invoke((MethodInvoker)delegate
                    {
                        lblAllPrimes.Text = v_num1.ToString() + " - " + v_num2.ToString() + " - " + v_num3.ToString() + " - " + v_num4.ToString() + " - " + v_num5.ToString() + "\n" + htctrl.Count.ToString() + " - " + ht.Count.ToString() + "\n" + listBoxAllPrimes.Items.Count.ToString();
                    });
					v_bnum1=v_num1;
					
					foreach(object k in ht.Keys)
					{
                        this.Invoke((MethodInvoker)delegate
                        {
                            listBoxAllPrimes.Items.Add(k + "\t" + DateTime.Now.ToString());
                            listBoxAllPrimes.SelectedIndex = listBoxAllPrimes.Items.Count - 1;
                        });
						//addToPrimeDb(k.ToString());
					}
					
					ht.Clear();
				}
                this.Invoke((MethodInvoker)delegate
                {
			        if (listBoxAllPrimes.Items.Count >= int.Parse(txtAllPrimesSaveAfter.Text)*10)
			        {
                        listBoxAllPrimes.Items.Clear();
			        }
                });
                GC.Collect();
				if ((v_cnt % 100)==0 )
				{
					Application.DoEvents();
				}
				if(!b_runningAllPrimes)
				{
					break;
				}

				v_cnt++;
			}

			tAllPrimes.Abort();
            this.Invoke((MethodInvoker)delegate
            {
                lblAllPrimes.Text = "Exiting runAllPrimes()...";
            });
		}
		

		void saveFile()
		{
			string v_filename = "";
			sFD.ShowDialog();
			v_filename=sFD.FileName;
			if(v_filename!="")
			{
				StreamWriter sr = File.CreateText(v_filename);
				sr.Close();
				FileInfo fi = new FileInfo(v_filename);
				v_file = fi.Name;
				v_folder = fi.DirectoryName;
				txt12File.Text=v_folder + "\\" + v_file;
			}
		}

		void saveLastPrime(int in_num)
		{
			string in_txt = in_num.ToString();
			if(v_folderAllPrimes=="" || v_fileAllPrimes=="")
			{
				string v_filename = "";
				sFD.ShowDialog();
				v_filename=sFD.FileName;
				if(v_filename!="")
				{
					StreamWriter sr = File.CreateText(v_filename);
					sr.Close();
					FileInfo fi = new FileInfo(v_filename);
					v_fileAllPrimes = fi.Name;
					v_folderAllPrimes = fi.DirectoryName;
				}
			}
			if(v_folderAllPrimes!="" && v_fileAllPrimes!="")
			{
				StreamWriter sw;
				if(File.Exists(v_folderAllPrimes + "\\" + v_fileAllPrimes))
				{
					sw=File.AppendText(v_folderAllPrimes + "\\" + v_fileAllPrimes);
				}
				else
				{
					sw=File.CreateText(v_folderAllPrimes + "\\" + v_fileAllPrimes);
				}
				sw.WriteLine(in_txt);
				sw.Close();
			}
		}

		void browsePrimeFile()
		{
			string v_filename = "";
			oFD.ShowDialog();
			v_filename=oFD.FileName;
			if(v_filename!="")
			{
				FileInfo fi = new FileInfo(v_filename);
				v_fileAllPrimes = fi.Name;
				v_folderAllPrimes = fi.DirectoryName;
				StreamReader sr = (StreamReader)File.OpenText(v_folderAllPrimes + "\\" + v_fileAllPrimes);
				string v_line="";
				string v_oldline="";
				while(sr.Peek()!=-1)
				{
					v_line = sr.ReadLine();
					if (v_line!="")
					{
						v_oldline=v_line;
					}
				}
				sr.Close();
				if(v_oldline!="")
				{
					txtAllPrimesStart.Text=v_oldline;
				}
			}
		}

		void browseFile()
		{
			string v_filename = "";
			oFD.ShowDialog();
			v_filename=oFD.FileName;
			if(v_filename!="")
			{
				FileInfo fi = new FileInfo(v_filename);
				v_file = fi.Name;
				v_folder = fi.DirectoryName;
				txt12File.Text=v_folder + "\\" + v_file;
				StreamReader sr = (StreamReader)File.OpenText(v_folder + "\\" + v_file);
				string v_line="";
				string v_oldline="";
				while(sr.Peek()!=-1)
				{
					v_line = sr.ReadLine();
					if (v_line!="")
					{
						v_oldline=v_line;
					}
				}
				sr.Close();
				if(v_oldline!="" && v_oldline.IndexOf("\t")!=-1)
				{
					v_start12=int.Parse(v_oldline.Substring(0,v_oldline.IndexOf("\t")));
					pB1.Value=int.Parse(v_start12.ToString());
				}
			}
		}
		void browseFile(string v_filename)
		{
			if(v_filename!="")
			{
				FileInfo fi = new FileInfo(v_filename);
				v_file = fi.Name;
				v_folder = fi.DirectoryName;
				txt12File.Text=v_folder + "\\" + v_file;
				StreamReader sr = (StreamReader)File.OpenText(v_folder + "\\" + v_file);
				string v_line="";
				string v_oldline="";
				while(sr.Peek()!=-1)
				{
					v_line = sr.ReadLine();
					if (v_line!="")
					{
						v_oldline=v_line;
					}
				}
				sr.Close();
				if(v_oldline!="" && v_oldline.IndexOf("\t")!=-1)
				{
					v_start12=int.Parse(v_oldline.Substring(0,v_oldline.IndexOf("\t")));
					pB1.Value=int.Parse(v_start12.ToString());
				}
			}
		}	
		void countRoutes(int in_row,int in_col,int in_lvl)
		{
			lbl18.Text=listBox18.Items.Count.ToString();
			if (b_running18)
			{
				string strMdbFile="G:\\mathchallenge\\vbs\\18\\18.mdb";
				v_cnt++;
				OleDbConnection oleconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strMdbFile);
				OleDbConnection oleconn2 = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strMdbFile);
				OleDbCommand olecom;
				OleDbCommand olecom2;
				OleDbDataReader oledr;
				OleDbDataReader oledr2;
				string strsql="";
				string strsql2="";
				strsql = "select NumID,Num,NumRow,NumCol from Triangle where NumRow=" + (in_row+1);
				//writetofile12 strsql
				oleconn.Open();
				olecom=new OleDbCommand(strsql,oleconn);
				oledr = olecom.ExecuteReader();
				//listBox18.Items.Add(strsql);
				while(oledr.Read())
				{
					int v_num=oledr.GetInt32(3);
					strsql2 = "select NumID,Num,NumRow,NumCol from Triangle where NumRow=" + oledr.GetInt32(2) + " and (NumCol=" + (v_num-1) + " or NumCol=" + v_num + " or NumCol=" + (v_num+1) + ") order by NumCol";
					if (v_cnt==1)
					{
						//	writetofile12 rs(0).Name & vbtab & rs(1).Name & vbtab & rs(2).Name & vbtab & rs(3).Name & vbtab & "v_cnt"
						//listBox18.Items.Add(oledr.GetName(0) + "	" + oledr.GetName(1) + "	" + oledr.GetName(2) + "	" + oledr.GetName(3) + "	v_cnt");

					}
					//writetofile12 
					//listBox18.Items.Add(strsql2);
					//listBox18.Items.Add(oledr.GetInt32(0) + "	" + oledr.GetInt32(1) + "	" + oledr.GetInt32(2) + "	" + oledr.GetInt32(3) + "	" + v_cnt);
					//writetofile12 strsql2
					oleconn2.Open();
					olecom2=new OleDbCommand(strsql2,oleconn2);
					oledr2 = olecom2.ExecuteReader();
					while(oledr2.Read())
					{
						listBox18.Items.Add("countRoutes("+ oledr2.GetInt32(2) +" ,"+ oledr2.GetInt32(3) +","+ (in_lvl+1) +");");
						if(listBox18.Items.Count>17000)//oledr2.GetInt32(2)>7 && oledr2.GetInt32(3)>7)
						{
							b_running18=false;
							break;
						}
						countRoutes(oledr2.GetInt32(2),oledr2.GetInt32(3),in_lvl+1);
					}
					oleconn2.Close();
				}
				oleconn.Close();
			}
		}
		
		void writetofile12(string in_txt)
		{
			if(v_folder!="" && v_file!="")
			{
				StreamWriter sw;
				if(File.Exists(v_folder + "\\" + v_file))
				{
					sw=File.AppendText(v_folder + "\\" + v_file);
				}
				else
				{
					sw=File.CreateText(v_folder + "\\" + v_file);
				}
				sw.WriteLine(in_txt);
				sw.Close();
			}
		}
		void writetofile55(string in_txt)
		{
			if(v_folder55!="" && v_file55!="")
			{
				StreamWriter sw;
				if(File.Exists(v_folder55 + "\\" + v_file55))
				{
					sw=File.AppendText(v_folder55 + "\\" + v_file55);
				}
				else
				{
					sw=File.CreateText(v_folder55 + "\\" + v_file55);
				}
				sw.WriteLine(in_txt);
				sw.Close();
			}
		}
		void writetofileAllPrimes(string in_txt)
		{
			if(v_folderAllPrimes!="" && v_fileAllPrimes!="")
			{
				StreamWriter sw;
				if(File.Exists(v_folderAllPrimes + "\\" + v_fileAllPrimes))
				{
					sw=File.AppendText(v_folderAllPrimes + "\\" + v_fileAllPrimes);
				}
				else
				{
					sw=File.CreateText(v_folderAllPrimes + "\\" + v_fileAllPrimes);
				}
				sw.WriteLine(in_txt);
				sw.Close();
			}
		}
		void addToFibonacci(string in_fnum,string in_num,int in_len,bool bStartPD,bool bEndPD)
		{

			string MyConString = "dsn=sqlBulk;uid=bulkuser;";
			string strStartPD="";
			string strEndPD="";
			if(bStartPD)
			{
				strStartPD="1";
			}
			else
			{
				strStartPD="0";
			}

			if(bEndPD)
			{
				strEndPD="1";
			}
			else
			{
				strEndPD="0";
			}

			OdbcConnection odbcconn = new OdbcConnection(MyConString);	

			string strsql = "select FibNum from tblFibonacci where FibNum = '" + in_fnum + "'";
			odbcconn.Open();
			OdbcCommand odbccom=new OdbcCommand(strsql,odbcconn);
			OdbcDataReader odbcdr = odbccom.ExecuteReader();
			if(!odbcdr.Read())
			{
				odbcconn.Close();
				strsql = "insert into tblFibonacci (FibNum,FibVal,FibLen,bStartPD,bEndPD) values('" + in_fnum + "','"+ in_num +"',"+ in_len +","+ strStartPD +","+ strEndPD +")";
				odbcconn.Open();
				odbccom=new OdbcCommand(strsql,odbcconn);
				odbccom.ExecuteNonQuery();
			}
			
			odbccom.Dispose();
			odbcconn.Close();
			odbcconn.Dispose();
			//}
		}
		void addToDivisors(string in_num,string in_divs,string in_numdivs)
		{
			//if(!isDbPrime(double.Parse(in_num)))
			//{
			//string strMdbFile="G:\\mathchallenge\\vbs\\Primes.mde";
			//OleDbConnection oleconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strMdbFile);
			//OleDbConnection oleconn = new OleDbConnection("dsn=Primes;uid=primes;");
			string MyConString = "dsn=sqlBulk;uid=bulkuser;";

			OdbcConnection odbcconn = new OdbcConnection(MyConString);	
			
			string strsql = "insert into tblDivisors (Number,divs,numdivs) values(" + in_num + ",'"+ in_divs +"',"+ in_numdivs +")";
			odbcconn.Open();
			OdbcCommand odbccom=new OdbcCommand(strsql,odbcconn);
			//Clipboard.SetDataObject(strsql,true);
			odbccom.ExecuteNonQuery();
			
			odbccom.Dispose();
			odbcconn.Close();
			odbcconn.Dispose();
			//}
		}
		void addToTriangles(string in_sum,string in_facts,string in_num,string in_divs)
		{
			
			string MyConString = "dsn=sqlBulk;uid=bulkuser;";

			OdbcConnection odbcconn = new OdbcConnection(MyConString);	
			string[] s_divs = in_divs.Split((char)44);
			string strsql = "insert into tblTriangleNums (num,Facts,NumSum,Divs,Num_Divs) values(" + in_num + ",'"+ in_facts +"',"+ in_sum +",'"+ in_divs +"',"+ s_divs.Length +")";
			odbcconn.Open();
			OdbcCommand odbccom=new OdbcCommand(strsql,odbcconn);
			//Clipboard.SetDataObject(strsql,true);
			odbccom.ExecuteNonQuery();
			
			odbccom.Dispose();
			odbcconn.Close();
			odbcconn.Dispose();
			//}
		}
		
		//SELECT MAX(PrimeNum) FROM tblPrimes
		public string getStartTri()
		{
			string v_ret="0";
			string MyConString = "dsn=sqlBulk;uid=bulkuser;";

			OdbcConnection odbcconn = new OdbcConnection(MyConString);	
			
			string strsql = "select max(num) from tblTriangleNums";
			odbcconn.Open();
			OdbcCommand odbccom=new OdbcCommand(strsql,odbcconn);
			
			try
			{
				OdbcDataReader odbcdr = odbccom.ExecuteReader();
				if(odbcdr.Read() && !odbcdr.IsDBNull(0))
				{
					v_ret = odbcdr.GetValue(0).ToString();
				}

			}
			catch
			{
				
			}
			odbccom.Dispose();
			odbcconn.Close();
			odbcconn.Dispose();

			return v_ret;
		}

		public int getStartPrime()
		{
			int v_ret = 0;

			string MyConString = "dsn=sqlBulk;uid=bulkuser;";

			OdbcConnection odbcconn = new OdbcConnection(MyConString);	
			
			string strsql = "SELECT MAX(PrimeNum) FROM tblPrimes";
			odbcconn.Open();

			OdbcCommand odbccom=new OdbcCommand(strsql,odbcconn);
			
			try
			{
				OdbcDataReader odbcdr = odbccom.ExecuteReader();
				if(odbcdr.Read() && !odbcdr.IsDBNull(0))
				{
					v_ret = odbcdr.GetInt32(0);
				}

			}
			catch
			{
				
			}
			odbccom.Dispose();
			odbcconn.Close();
			odbcconn.Dispose();

			return v_ret;
		}
		public int getStartPrime(int in_lower_than)
		{
			int v_ret = 0;

			string MyConString = "dsn=sqlBulk;uid=bulkuser;";

			OdbcConnection odbcconn = new OdbcConnection(MyConString);	
			
			string strsql = "SELECT MAX(PrimeNum) FROM tblPrimes where PrimeNum<" + in_lower_than;
			odbcconn.Open();

			OdbcCommand odbccom=new OdbcCommand(strsql,odbcconn);
			
			try
			{
				OdbcDataReader odbcdr = odbccom.ExecuteReader();
				if(odbcdr.Read() && !odbcdr.IsDBNull(0))
				{
					v_ret = odbcdr.GetInt32(0);
				}

			}
			catch
			{
				
			}
			odbccom.Dispose();
			odbcconn.Close();
			odbcconn.Dispose();

			return v_ret;
		}

		public int getStartPrime(bool bView)
		{
			int v_ret = 0;

			string MyConString = "dsn=sqlBulk;uid=bulkuser;";

			OdbcConnection odbcconn = new OdbcConnection(MyConString);	
			
			string strsql = "SELECT PrimeNum FROM getStartPrime";
			odbcconn.Open();

			OdbcCommand odbccom=new OdbcCommand(strsql,odbcconn);
			
			try
			{
				OdbcDataReader odbcdr = odbccom.ExecuteReader();
				if(odbcdr.Read() && !odbcdr.IsDBNull(0))
				{
					v_ret = odbcdr.GetInt32(0);
				}

			}
			catch
			{
				
			}
			odbccom.Dispose();
			odbcconn.Close();
			odbcconn.Dispose();

			return v_ret;
		}

		public int getStartPrime(int in_lower_than,int in_larger_than)
		{
			int v_ret = 0;

			string MyConString = "dsn=sqlBulk;uid=bulkuser;";

			OdbcConnection odbcconn = new OdbcConnection(MyConString);	
			
			string strsql = "SELECT MAX(PrimeNum) FROM tblPrimes where PrimeNum > " + in_larger_than + " and PrimeNum<" + in_lower_than;
			odbcconn.Open();

			OdbcCommand odbccom=new OdbcCommand(strsql,odbcconn);
			
			try
			{
				OdbcDataReader odbcdr = odbccom.ExecuteReader();
				if(odbcdr.Read() && !odbcdr.IsDBNull(0))
				{
					v_ret = odbcdr.GetInt32(0);
				}

			}
			catch
			{
				
			}
			odbccom.Dispose();
			odbcconn.Close();
			odbcconn.Dispose();

			return v_ret;
		}

		public int countThisRoute(string strRoute)
		{
			int v_ret=0;
			string MyConString = "dsn=sqlBulk;uid=bulkuser;";

			OdbcConnection odbcconn = new OdbcConnection(MyConString);	
				
			string strsql = "select count(*) from tblSqRoutes where Route like '"+ strRoute +"%'";
			odbcconn.Open();
			OdbcCommand odbccom=new OdbcCommand(strsql,odbcconn);
				
			try
			{
				OdbcDataReader odbcdr = odbccom.ExecuteReader();
				if(odbcdr.Read() && !odbcdr.IsDBNull(0))
				{
					v_ret = odbcdr.GetInt32(0);
				}

			}
			catch
			{
					
			}
			odbccom.Dispose();
			odbcconn.Close();
			odbcconn.Dispose();

			return v_ret;

		}
		public int getStartFib()
		{
			int v_ret=0;
			string MyConString = "dsn=sqlBulk;uid=bulkuser;";

			OdbcConnection odbcconn = new OdbcConnection(MyConString);	
			
			string strsql = "select top 3 FibNum from tblFibonacci order by FibNum desc";
			odbcconn.Open();
			OdbcCommand odbccom=new OdbcCommand(strsql,odbcconn);
			
			try
			{
				OdbcDataReader odbcdr = odbccom.ExecuteReader();
				if(odbcdr.Read() && !odbcdr.IsDBNull(0))
				{
					v_ret = odbcdr.GetInt32(0);
				}

			}
			catch
			{
				
			}
			odbccom.Dispose();
			odbcconn.Close();
			odbcconn.Dispose();

			return v_ret;

		}

		public BigInteger[] getStartFibVal()
		{
			BigInteger[] v_ret= {new BigInteger(0),new BigInteger(0),new BigInteger(0)};
			string MyConString = "dsn=sqlBulk;uid=bulkuser;";
			int v_cnt=0;

			OdbcConnection odbcconn = new OdbcConnection(MyConString);	
			
			string strsql = "select top 3 FibVal from tblFibonacci order by FibNum desc";
			odbcconn.Open();
			OdbcCommand odbccom=new OdbcCommand(strsql,odbcconn);
			
			//try
			//{
				OdbcDataReader odbcdr = odbccom.ExecuteReader();
				while(odbcdr.Read() && !odbcdr.IsDBNull(0))
				{
					v_ret[v_cnt] = new BigInteger(odbcdr.GetString(0),10);
					v_cnt++;
				}

			/*}
			catch
			{
				
			}*/
			odbccom.Dispose();
			odbcconn.Close();
			odbcconn.Dispose();

			return v_ret;

		}

		public string getStartDiv()
		{
			string v_ret="0";
			string MyConString = "dsn=sqlBulk;uid=bulkuser;";

			OdbcConnection odbcconn = new OdbcConnection(MyConString);	
			
			string strsql = "select max(Number) from tblDivisors where number<5000000";
			odbcconn.Open();
			OdbcCommand odbccom=new OdbcCommand(strsql,odbcconn);
			
			try
			{
				OdbcDataReader odbcdr = odbccom.ExecuteReader();
				if(odbcdr.Read() && !odbcdr.IsDBNull(0))
				{
					v_ret = odbcdr.GetValue(0).ToString();
				}

			}
			catch
			{
				
			}
			odbccom.Dispose();
			odbcconn.Close();
			odbcconn.Dispose();

			return v_ret;
		}
		

		void addToPrimeDb(string in_num)
		{
			//if(!isDbPrime(double.Parse(in_num)))
			//{
				//string strMdbFile="G:\\mathchallenge\\vbs\\Primes.mde";
				//OleDbConnection oleconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strMdbFile);
				//OleDbConnection oleconn = new OleDbConnection("dsn=Primes;uid=primes;");
			/*string MyConString = "DRIVER={MySQL ODBC 3.51 Driver};" +
				"SERVER=localhost;" +
				"DATABASE=Primes;" +
				"UID=primes;" +
				"OPTION=3";
			*/
			
			string MyConString = "dsn=sqlBulk;uid=bulkuser;";
			OdbcConnection odbcconn = new OdbcConnection(MyConString);	
			
			string strsql = "insert into tblPrimes (PrimeNum) values(" + in_num + ")";
				odbcconn.Open();
				OdbcCommand odbccom=new OdbcCommand(strsql,odbcconn);
				odbccom.ExecuteNonQuery();
				/*try
				{
					odbccom.ExecuteNonQuery();
				}
				catch
				{
					//writetofileAllPrimes(in_num);
				}*/
				odbccom.Dispose();
				odbcconn.Close();
				odbcconn.Dispose();
			//}
		}

		void addToPrimesDb()
		{			
			//if(!isDbPrime(double.Parse(in_num)))
			//{

			if(httemp.Count>0)
			{
				string strMdbFile="G:\\mathchallenge\\vbs\\Primes.mde";
				OleDbConnection oleconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strMdbFile);
				string strsql = "";
				OleDbCommand olecom;
				foreach(object k in httemp.Keys)
				{
					oleconn.Open();
					strsql = "insert into Primes (PrimeNum) values(" + k.ToString() + ")";
					olecom=new OleDbCommand(strsql,oleconn);
					//try
					//{
						olecom.ExecuteNonQuery();
						listBoxAllPrimes.Items.Add(k + "\t" + DateTime.Now.ToString());
						listBoxAllPrimes.SelectedIndex=listBoxAllPrimes.Items.Count-1;
					/*}
					catch
					{
					//	writetofileAllPrimes(strsql.Replace("insert into Primes (PrimeNum) values(","").Replace(");",""));
					}*/

					olecom.Dispose();
					oleconn.Close();
					
				}
				oleconn.Dispose();
			}
			httemp.Clear();
			//}
		}
		
		void loadPrimes()
		{
			b_PrimesLoaded=true;
            try
            {
                //Hashtable v_ret=new Hashtable();
                //string strMdbFile="G:\\mathchallenge\\vbs\\Primes.mde";
                //OleDbConnection oleconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strMdbFile);
                /*
                string MyConString = "DRIVER={MySQL ODBC 3.51 Driver};" +
                    "SERVER=localhost;" +
                    "DATABASE=Primes;" +
                    "UID=primes;" +
                    "OPTION=3";
                */

                string MyConString = "dsn=sqlBulk;uid=bulkuser;";

                OdbcConnection odbcconn = new OdbcConnection(MyConString);
                OdbcCommand odbccom;
                string strsql = "select distinct PrimeNum from tblPrimes order by PrimeNum";
                odbcconn.Open();
                odbccom = new OdbcCommand(strsql, odbcconn);
                OdbcDataReader odbcdr = odbccom.ExecuteReader();
                int v_val = 0;
                while (odbcdr.Read())
                {
                    v_val = (int)odbcdr.GetInt32(0);
                    if (!htctrl.ContainsKey(v_val))
                    {
                        htctrl.Add(v_val, v_val);
                    }
                }
                odbcdr.Close();
                odbccom.Dispose();
                odbcconn.Close();
                odbcconn.Dispose();
            }
            catch { }
			//return v_ret;
		}

		void loadPrimes(int v_min,int v_max)
		{
			b_PrimesLoaded=true;
			//Hashtable v_ret=new Hashtable();
			//string strMdbFile="G:\\mathchallenge\\vbs\\Primes.mde";
			//OleDbConnection oleconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strMdbFile);
			string MyConString = "DRIVER={MySQL ODBC 3.51 Driver};" +
				"SERVER=localhost;" +
				"DATABASE=Primes;" +
				"UID=primes;" +
				"OPTION=3";

			OdbcConnection odbcconn = new OdbcConnection(MyConString);
			OdbcCommand odbccom;
			string strsql = "select distinct PrimeNum from Primes where PrimeNum between " + v_min.ToString() + " and " + v_max.ToString() + " order by PrimeNum";
			odbcconn.Open();
			odbccom=new OdbcCommand(strsql,odbcconn);
			OdbcDataReader odbcdr= odbccom.ExecuteReader();
			int v_val=0;
			while(odbcdr.Read())
			{
				v_val=(int)odbcdr.GetInt32(0);
				if(!htctrl.ContainsKey(v_val))
				{
					htctrl.Add(v_val,v_val);
				}
			}
			odbcdr.Close();
			odbccom.Dispose();
			odbcconn.Close();
			odbcconn.Dispose();
			//return v_ret;
		}


		void loadDivisors()
		{
			string MyConString = "dsn=sqlBulk;uid=bulkuser;";

			OdbcConnection odbcconn = new OdbcConnection(MyConString);
			OdbcCommand odbccom;
			string strsql = "select distinct Number from tblDivisors";
			odbcconn.Open();
			odbccom=new OdbcCommand(strsql,odbcconn);
			OdbcDataReader odbcdr= odbccom.ExecuteReader();
			int v_val=0;
			while(odbcdr.Read())
			{
				v_val=(int)odbcdr.GetInt32(0);
				if(!htctrl.ContainsKey(v_val))
				{
					htctrl.Add(v_val,v_val);
				}
			}
			odbcdr.Close();
			odbccom.Dispose();
			odbcconn.Close();
			odbcconn.Dispose();
		}

        bool isOdd(double in_num)
        {
            return isOdd(int.Parse(Math.Floor(in_num).ToString()));
        }
		bool isOdd(int in_num)
		{
			bool v_ret=false;
			string temp_num="";
			temp_num=in_num.ToString();
			v_ret=(temp_num.Substring(temp_num.Length-1,1)=="1" ||
				temp_num.Substring(temp_num.Length-1,1)=="3" ||
				temp_num.Substring(temp_num.Length-1,1)=="5" ||
				temp_num.Substring(temp_num.Length-1,1)=="7" ||
				temp_num.Substring(temp_num.Length-1,1)=="9");
			return v_ret;
		}

		bool isEven(int in_num)
		{
			bool v_ret=false;
			string temp_num="";
			temp_num=in_num.ToString();
			v_ret=(temp_num.Substring(temp_num.Length-1,1)=="0" ||
				temp_num.Substring(temp_num.Length-1,1)=="2" ||
				temp_num.Substring(temp_num.Length-1,1)=="4" ||
				temp_num.Substring(temp_num.Length-1,1)=="6" ||
				temp_num.Substring(temp_num.Length-1,1)=="8");
			return v_ret;
		}

		bool isPrime(int in_num)
		{
			bool v_ret;
			v_ret=true;
			double v_halfNum = ((in_num-2)/2);
			double v_end = double.Parse(Math.Round(v_halfNum,0).ToString());
			for(double o = 2;o<=v_end;o++)
			{
				if(!b_runningAllPrimes)
				{
					goto exitrun;
				}
				double v_inum1=(in_num/o);
				double v_inum2=(in_num/(in_num-o));
				string v_num1=v_inum1.ToString();
				string v_num2=v_inum2.ToString();
				bool v_1notprime=(v_num1.IndexOf(",")<=0);
				bool v_2notprime=(v_num2.IndexOf(",")<=0);
				if (v_1notprime || v_2notprime)
				{
					v_ret=false;
					break;
				}
			}
			exitrun:
			return v_ret;
		}

		bool isDbPrime(double in_num)
		{
			bool v_ret=false;

			//string strMdbFile="G:\\mathchallenge\\vbs\\Primes.mdb";
			//OleDbConnection oleconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strMdbFile);
			string MyConString = "DRIVER={MySQL ODBC 3.51 Driver};" +
				"SERVER=localhost;" +
				"DATABASE=Primes;" +
				"UID=primes;" +
				"OPTION=3";

			OdbcConnection odbcconn = new OdbcConnection(MyConString);
			OdbcCommand odbccom;
			OdbcDataReader odbcdr;
			string strsql="";
			strsql = "select PrimeNum from Primes where PrimeNum=" + in_num.ToString();
			odbcconn.Open();
			odbccom=new OdbcCommand(strsql,odbcconn);
			odbcdr = odbccom.ExecuteReader();

			v_ret=odbcdr.Read();

			odbcdr.Close();
			odbccom.Dispose();
			odbcconn.Close();
			odbcconn.Dispose();

			return v_ret;
		}

		bool ispalindrome(string in_num)
		{
			bool v_ret=true;
			int v_end = in_num.Length-1;
			int v_num1=0;
			int v_num2=0;
			for(int o=0;o<=v_end;o++)
			{
				v_num1=int.Parse(in_num.Substring(o,1));
				if(v_end.Equals(o))
				{
					v_num2=v_num1;
				}
				else
				{
					v_num2=int.Parse(in_num.Substring((v_end-o),1));
				}
				if(v_num1!=v_num2)
				{
					v_ret=false;
					break;
				}
			}
			return v_ret;
		}
		
		
		string[] getAnagrams(int in_num)
		{
			string[] v_ret=null;
			string strNum = in_num.ToString();
			for(int xx=1;xx<strNum.Length;xx++)
			{
				
			}

			return v_ret;
		}

		int[] SortIntList(int[] in_list)
		{
			int[] v_ret=null;
			int tempnum=0;
			for(int i=0;i<=in_list.Length;i++)
			{
				for(int n=0;n<=in_list.Length;n++)
				{
					if (in_list[n]>in_list[i])
					{
						tempnum = in_list[i];
						in_list[i] = in_list[n];
						in_list[n] = tempnum;
					}
				}
			}
			return v_ret;
		}
		

		int getLowest(int in_num)
		{
			int v_ret=0;
			string sin_num = in_num.ToString();
			int[] numlist=null;
			
			for(int i = 1;i<=sin_num.Length;i++)
			{
				numlist.SetValue(int.Parse(sin_num.Substring(i,1)),i);
			}
			numlist=SortIntList(numlist);
			v_ret=numlist[0];
			return v_ret;
		}
		int getHighest(int in_num)
		{
			int v_ret=0;
			string sin_num = in_num.ToString();
			int[] numlist=null;
			
			for(int i = 1;i<=sin_num.Length;i++)
			{
				numlist.SetValue(int.Parse(sin_num.Substring(i,1)),i);
			}
			numlist=SortIntList(numlist);
			v_ret=numlist[numlist.Length];
			return v_ret;
		}
		

		double getNumSum(string in_num)
		{
			double v_ret=0;
			for(int i =0;i<in_num.Length;i++)
			{
				v_ret+=double.Parse(in_num.Substring(i,1))+0;
			}
			return v_ret;
		}
		double getNumSum(double in_num)
		{
			double v_ret=0;
			for(int i =0;i<in_num.ToString().Length;i++)
			{
				v_ret+=double.Parse(in_num.ToString().Substring(i,1))+0;
			}
			return v_ret;
		}
		double getNumSum(int in_num)
		{
			double v_ret=0;
			for(int i =0;i<in_num.ToString().Length;i++)
			{
				v_ret+=double.Parse(in_num.ToString().Substring(i,1))+0;
			}
			return v_ret;
		}


		double getFactorials(double v_num)
		{
			double dou=0;
			/*for(double i = v_num;i>=1;i--)
			{
				dou=dou*i;
			}*/
			switch(int.Parse(v_num.ToString()))
			{
				case 0:
					dou=0;
					break;
				case 1:
					dou=1;
					break;
				case 2:
					dou=2;
					break;
				case 3:
					dou=6;
					break;
				case 4:
					dou=24;
					break;
				case 5:
					dou=120;
					break;
				case 6:
					dou=720;
					break;
				case 7:
					dou=5040;
					break;
				case 8:
					dou=40320;
					break;
				case 9:
					dou=362880;
					break;
			}
			return dou;
		}

		double getBackwards(string in_num)
		{
			string v_ret="";
			for(int o = in_num.Length-1;o>=0;o--)
			{
				v_ret+= in_num.Substring(o,1);
			}
			return double.Parse(v_ret);
		}

		
		string getRealVal(string in_val)
		{
			string v_ret="";
			int v_start = in_val.IndexOf("E");
			if (v_start>0)
			{
				double val1=double.Parse(in_val.Substring(0,v_start));
				double val2=double.Parse(in_val.Substring(v_start+1,in_val.Length-(v_start+1)));
				double val= (val1 * (Math.Pow(10,val2)));
				v_ret=val.ToString();
			}
			else
			{
				v_ret= in_val;
			}
			return v_ret;
		}

		string getRelLen(string in_val)
		{
			string v_ret="";
			int v_start = in_val.IndexOf("E");
			int v_start2 = in_val.IndexOf(",")-1;
			if (v_start>0)
			{
				int val2=int.Parse(in_val.Substring(v_start+1,in_val.Length-(v_start+1)));
				int val = v_start2 + val2;
				v_ret=val.ToString(); 
			}				 
			else
			{
				v_ret= in_val.Length.ToString();
			}
			return v_ret;
		}

		string getDivisors(double in_num)
		{
			string v_ret="";
			double v_dbl=0;
			string s_dbl ="";
			for(int o = 1;o<=in_num;o++)
			{
				v_dbl = (in_num/o);
				s_dbl = v_dbl.ToString();
				if (s_dbl.IndexOf(",")==-1 && s_dbl.IndexOf(".")==-1)
				{
					if (v_ret.Equals(""))
					{
						v_ret = o.ToString();
					}
					else
					{
						v_ret += "," + o.ToString();
					}
				}
				if(!b_running12 && !b_running1212)
				{
					break;
				}

			}
			return v_ret;
		}
		

		#region Control Voids
		private void btn12Start_Click(object sender, System.EventArgs e)
		{
			//ThreadStart ts = new ThreadStart(run12);
			/*
			if(v_folder!="" && v_file!="")
			{
				t12 = new Thread(ts);
				t12.Priority=ThreadPriority.Lowest;
				t12.Start();
			}
			else
			{
				saveFile();
				if(txt12File.Text!="")
				{
					t12 = new Thread(ts);
					t12.Priority=ThreadPriority.Lowest;
					t12.Start();
				}
			}
			*/
			ThreadStart ts = new ThreadStart(run12);
			
			if(v_start12<=0)
			{
				v_start12=int.Parse(getStartDiv());
				pB1.Value=int.Parse(v_start12.ToString());
				txt12File.Text=v_start12.ToString();

				t12 = new Thread(ts);
				t12.Priority=ThreadPriority.Lowest;
				t12.Start();
			}
			else
			{
				t12 = new Thread(ts);
				t12.Priority=ThreadPriority.Lowest;
				t12.Start();
			}

		}

		private void btn12Browse_Click(object sender, System.EventArgs e)
		{
			//browseFile();
			v_start12=int.Parse(getStartDiv());
			pB1.Value=int.Parse(v_start12.ToString());
			txt12File.Text=v_start12.ToString();
		}

		private void btn12Stop_Click(object sender, System.EventArgs e)
		{
			b_running12=false;
			btn12Stop.Enabled=true;
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{

			v_end12=int.Parse(txt12End.Text);
			v_end1212=int.Parse(txtEnd1212.Text);
			pB1.Maximum=v_end12;

			ThreadStart ts = null;

			foreach(string sCmd in strCmdLine)
			{
				switch(sCmd.ToLower())
				{
					case "-run12":
						tabPage1212.Show();

						ts = new ThreadStart(run1212);
						t1212 = new Thread(ts);
						t1212.Priority=ThreadPriority.Lowest;
						t1212.Start();
					break;
					case "-run104":

						tabPage104.Show();


						ts = new ThreadStart(run104);
						t104 = new Thread(ts);
						t104.Priority=ThreadPriority.Lowest;
						t104.Start();
					break;
					case "-runallprimes":

						tabPageAllPrimes.Show();

						v_startPrime = getStartPrime(true);
						txtAllPrimesStart.Text = v_startPrime.ToString();

						ts = new ThreadStart(runAllPrimes);
						tAllPrimes = new Thread(ts);
						tAllPrimes.Priority=ThreadPriority.Lowest;
						tAllPrimes.Start();
						break;
				}
			}

		}

		private void txt12End_TextChanged(object sender, System.EventArgs e)
		{
			v_end12=int.Parse(txt12End.Text);
			pB1.Maximum=v_end12;
		}
		void endAllThreads()
		{

			b_running1212=false;
			b_running12=false;
			b_running15=false;
			
			b_running16=false;
			b_running18=false;
			b_running20=false;
			b_running25=false;
			b_running34=false;
			b_running35=false;
			b_running36=false;
			b_running48=false;
			b_running55=false;
			b_running56=false;
			b_running104=false;
			b_runningAllPrimes=false;

			try{ t1212.Abort();}
			catch{}
			try{ t12.Abort();}
			catch{}
			try{ t15.Abort();}
			catch{}
			try{ t16.Abort();}
			catch{}
			try{ t18.Abort();}
			catch{}
			try{ t20.Abort();}
			catch{}
			try{ t25.Abort();}
			catch{}
			try{ t29.Abort();}
			catch{}
			try{ t34.Abort();}
			catch{}
			try{ t35.Abort();}
			catch{}
			try{ t36.Abort();}
			catch{}
			try{ t48.Abort();}
			catch{}
			try{ t55.Abort();}
			catch{}
			try{ t56.Abort();}
			catch{}
			try{ t104.Abort();}
			catch{}
			try{ tAllPrimes.Abort();}
			catch{}
		
		}
		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{


endAllThreads();
	
			


		}

		private void btnExport12_Click(object sender, System.EventArgs e)
		{
			string v_filename = "";
			sFD.ShowDialog();
			v_filename=sFD.FileName;
			if(v_filename!="")
			{
				StreamWriter sr = File.CreateText(v_filename);
				for(int i = 0;i<listBox.Items.Count;i++)
				{
					sr.WriteLine(listBox.Items[i].ToString());
				}
				sr.Close();

			}
		}

		private void btn20Start_Click(object sender, System.EventArgs e)
		{
			ThreadStart ts = new ThreadStart(run20);
			t20 = new Thread(ts);
			t20.Priority=ThreadPriority.Lowest;
			t20.Start();
		}

		private void btn36start_Click(object sender, System.EventArgs e)
		{
			ThreadStart ts = new ThreadStart(run36);
			t36 = new Thread(ts);
			t36.Priority=ThreadPriority.Lowest;
			t36.Start();
		}

		private void btn34Start_Click(object sender, System.EventArgs e)
		{
			ThreadStart ts = new ThreadStart(run34);
			t34 = new Thread(ts);
			t34.Priority=ThreadPriority.Lowest;
			t34.Start();
		}
		private void btn25Start_Click(object sender, System.EventArgs e)
		{
			ThreadStart ts = new ThreadStart(run25);
			t25 = new Thread(ts);
			t25.Priority=ThreadPriority.Lowest;
			t25.Start();
		}
		private void txt34End_TextChanged(object sender, System.EventArgs e)
		{
			v_end34 = double.Parse(txt34End.Text);
			pB34.Maximum =int.Parse(v_end34.ToString());
			
		}

		private void btn34Export_Click(object sender, System.EventArgs e)
		{
			string v_filename = "";
			sFD.ShowDialog();
			v_filename=sFD.FileName;
			if(v_filename!="")
			{
				StreamWriter sr = File.CreateText(v_filename);
				for(int i = 0;i<listBox34.Items.Count;i++)
				{
					sr.WriteLine(listBox34.Items[i].ToString());
				}
				sr.Close();

			}
		}

		private void tabPage36_Click(object sender, System.EventArgs e)
		{
			/*
			There is a simple underlying principle to all positional based notation 
			systems, and once you get that, it is fairly straightforward to decode 
			and convert numbers in any base.  Starting with the units position, 
			each position to the left is valued base times greater.  For example, 
			in base 10, the number 1234 is actually understood like this: 

			  1234 = 1 * 10*10*10  = 1*1000 
				   + 2 * 10+10     = 2* 100 
				   + 3 * 10        = 3*  10 
				   + 4 * 1         = 4*   1 


			Looking at it as powers of the base, the pattern is clearer: 


			  1234 = 1 * 10^3  = 1*1000 
				   + 2 * 10^2  = 2* 100 
				   + 3 * 10^1  = 3*  10 
				   + 4 * 10^0  = 4*   1 


			Any non zero real number 'n' to the 'zeroth' power (n^0) = 1, so the 
			units position in any base is always valued at 1.  The pattern is the 
			same for every base.  Consider 1234 base 8, octal: 


			  1234 = 1 * 8^3  = 1*512  = 512 
				   + 2 * 8^2  = 2* 64  = 128 
				   + 3 * 8^1  = 3*  8  =  24 
				   + 4 * 8^0  = 4*  1  =   4 
										---- 
										 668 decimal 


			Now consider 1111 base 2, binary: 


			  1111 = 1 * 2^3  = 1*8  = 8 
				   + 1 * 2^2  = 1*4  = 4 
				   + 1 * 2^1  = 1*2  = 2 
				   + 1 * 2^0  = 1*1  = 1 
									  -- 
									  15 decimal 


			Because each position to the left is exactly base times as large as 
			the position to the right, we do not need a symbol = base.  That 
			value is represented by a 0 with a 1 to the left.  For example, in 
			base 10 we don't need a symbol with a value of 10, because we use a 
			0 with 1 in the position to the left: 10.  The same is true for 
			every base.  In base 2 there is no symbol for 2, we use 10, in base 
			8 there is no symbol for 8, we use 10, in base 16 we have symbols 
			for all the values 0 through 15, then we use 10 for the base size. 
			In any base system, 10 is the way we write the 'base' number.  Each 
			position increases from 0 up to the value of base -1, then we carry. 


			  Base  2: 0 1 10 
			  Base  3: 0 1 2 10 
			  Base  4: 0 1 2 3 10 
			  Base  5: 0 1 2 3 4 10 
			  Base  8: 0 1 2 3 4 5 6 7 10 
			  Base 10: 0 1 2 3 4 5 6 7 8 9 10 
			  Base 16: 0 1 2 3 4 5 6 7 8 9 a b c d e f 10 
			  Base 20: 0 1 2 3 4 5 6 7 8 9 a b c d e f g h i j 10 



			*/
		}

		private void txt34Start_TextChanged(object sender, System.EventArgs e)
		{
			v_start34= double.Parse(txt34Start.Text);
		}

		private void Form1_Deactivate(object sender, System.EventArgs e)
		{
			endAllThreads();
		}
		private void btn18Start_Click(object sender, System.EventArgs e)
		{
			ThreadStart ts = new ThreadStart(run18);
			t18 = new Thread(ts);
			t18.Priority=ThreadPriority.Lowest;
			t18.Start();
		}

		private void btn18Stop_Click(object sender, System.EventArgs e)
		{
			b_running18=false;
		}
		
		private void btn16Start_Click(object sender, System.EventArgs e)
		{
			ThreadStart ts = new ThreadStart(run16);
			t16 = new Thread(ts);
			t16.Priority=ThreadPriority.Lowest;
			t16.Start();
		}
		private void btn48Start_Click(object sender, System.EventArgs e)
		{
			ThreadStart ts = new ThreadStart(run48);
			t48 = new Thread(ts);
			t48.Priority=ThreadPriority.Lowest;
			t48.Start();
		}
		private void btnAllPrimesStart_Click(object sender, System.EventArgs e)
		{
			ThreadStart ts = new ThreadStart(runAllPrimes);
			tAllPrimes = new Thread(ts);
			tAllPrimes.Priority=ThreadPriority.Lowest;
			tAllPrimes.Start();
			
		}

		private void btnAllPrimesExport_Click(object sender, System.EventArgs e)
		{
			string v_filename = "";
			sFD.ShowDialog();
			v_filename=sFD.FileName;
			if(v_filename!="")
			{
				StreamWriter sr = File.CreateText(v_filename);
				for(int i = 0;i<listBoxAllPrimes.Items.Count;i++)
				{
					sr.WriteLine(listBoxAllPrimes.Items[i].ToString());
				}
				sr.Close();

			}
		}

		private void btnAllPrimesStop_Click(object sender, System.EventArgs e)
		{
			b_runningAllPrimes=false;
		}

		private void btn55Start_Click(object sender, System.EventArgs e)
		{
			
			ThreadStart ts = new ThreadStart(run55);
			t55 = new Thread(ts);
			t55.Priority=ThreadPriority.Lowest;
			t55.Start();
			/*
			MessageBox.Show("121-" + ispalindrome(DoubleConverter.ToExactString(double.Parse("121"))));
			MessageBox.Show("123-" + ispalindrome(DoubleConverter.ToExactString(double.Parse("123"))));
			*/
		}

		private void btnTestStart_Click(object sender, System.EventArgs e)
		{
			try
			{
				listBoxTest.Items.Clear();
				BigInteger bi = new BigInteger(1);
				BigInteger oldbi = new BigInteger(0);
				int biLen = bi.ToString().Length;
				int maxLen = int.Parse(txtTestLength.Text);
				while(biLen<maxLen && bi!=oldbi)
				{
					oldbi=bi;
					bi=bi*2;
					biLen = bi.ToString().Length;
					listBoxTest.Items.Add(bi.ToString() + "\t" + biLen.ToString());
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
		}

		private void tabPageAllPrimes_Click(object sender, System.EventArgs e)
		{
		
		}

		private void btnAllPrimesBrowse_Click(object sender, System.EventArgs e)
		{
			//browsePrimeFile();
			v_startPrime = getStartPrime(true);
			txtAllPrimesStart.Text = v_startPrime.ToString();
		}


		private void btnStart56_Click(object sender, System.EventArgs e)
		{
			ThreadStart ts = new ThreadStart(run56);
			t56 = new Thread(ts);
			t56.Priority=ThreadPriority.Lowest;
			t56.Start();
		}
		private void btnStart29_Click(object sender, System.EventArgs e)
		{
			ThreadStart ts = new ThreadStart(run29);
			t29 = new Thread(ts);
			t29.Priority=ThreadPriority.Lowest;
			t29.Start();
		}

		private void btnStart35_Click(object sender, System.EventArgs e)
		{
			b_running35=true;
			/*ThreadStart ts = new ThreadStart(run35);
			t35 = new Thread(ts);
			t35.Priority=ThreadPriority.Lowest;
			t35.Start();*/
			run35();
		}

		private void btnStop35_Click(object sender, System.EventArgs e)
		{
			b_running35=false;
		}

		#endregion

		private void btnStart1212_Click(object sender, System.EventArgs e)
		{
			ThreadStart ts = new ThreadStart(run1212);
			
			//v_start1212=int.Parse(txtStart1212.Text);
			v_start1212 = int.Parse(getStartTri());

			v_end1212=int.Parse(txtEnd1212.Text);			
			
			t1212 = new Thread(ts);
			t1212.Priority=ThreadPriority.Lowest;
			t1212.Start();
			
		}

		private void btnEnd1212_Click(object sender, System.EventArgs e)
		{
			b_running1212=false;
			btnStart1212.Enabled=true;
		}

		private void txtEnd1212_TextChanged(object sender, System.EventArgs e)
		{
			v_end1212=int.Parse(txtEnd1212.Text);
		}

		private void txtStart1212_TextChanged(object sender, System.EventArgs e)
		{
			v_start1212=int.Parse(txtStart1212.Text);
		}

		private void btn25Stop_Click(object sender, System.EventArgs e)
		{
			b_running25=false;
			btn25Start.Enabled=true;
		}

		private void btnStart104_Click(object sender, System.EventArgs e)
		{
			btnStop104.Enabled=true;
			btnStart104.Enabled=false;

			ThreadStart ts = new ThreadStart(run104);
			
			t104 = new Thread(ts);
			t104.Priority=ThreadPriority.Lowest;
			t104.Start();
		}

		private void btnStop104_Click(object sender, System.EventArgs e)
		{
			b_running104=false;
			btnStart104.Enabled=true;
			btnStop104.Enabled=false;
		}

		private void btn15Start_Click(object sender, System.EventArgs e)
		{
			ThreadStart ts = new ThreadStart(run15);
			
			t15 = new Thread(ts);
			t15.Priority=ThreadPriority.Lowest;
			t15.Start();
		}

		private void btn15End_Click(object sender, System.EventArgs e)
		{
			b_running15=false;
			btn15Start.Enabled=true;
		}

		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			loadThreads();
		}
		void loadThreads()
		{
			lVPrio.Items.Clear();
			addPrioItem(t1212,"t1212");
			addPrioItem(t12,"t12");
			addPrioItem(t15,"t15");
			addPrioItem(t16,"t16");
			addPrioItem(t18,"t18");
			addPrioItem(t20,"t20");
			addPrioItem(t25,"t25");
			addPrioItem(t29,"t29");
			addPrioItem(t34,"t34");
			addPrioItem(t35,"t35");
			addPrioItem(t36,"t36");
			addPrioItem(t48,"t48");
			addPrioItem(t55,"t55");
			addPrioItem(t56,"t56");
			addPrioItem(t102,"t102");
			addPrioItem(t104,"t104");
			addPrioItem(tAllPrimes,"tAllPrimes");		
		}
		void addPrioItem(Thread th,string thName)
		{
			ListViewItem nitm;
			try
			{
				nitm = lVPrio.Items.Add(thName);
				nitm.SubItems.Add(th.Priority.ToString());
				nitm.SubItems.Add(th.ThreadState.ToString());
			}
			catch
			{
				lVPrio.Items[lVPrio.Items.Count-1].Remove();
				nitm = lVPrio.Items.Add(thName);
				nitm.SubItems.Add("");
				nitm.SubItems.Add("Not running");
			}
		}
		Thread getThread(string thname)
		{

			Thread v_ret = null;
			switch(thname)
			{
				case "t1212":
					v_ret=t1212;
				break;
				case "t12":
					v_ret=t12;
				break;
				case "t15":
					v_ret=t15;
				break;
				case "t16":
					v_ret=t16;
				break;
				case "t18":
					v_ret=t18;
				break;
				case "t20":
					v_ret=t20;
				break;
				case "t25":
					v_ret=t25;
				break;
				case "t29":
					v_ret=t29;
				break;
				case "t34":
					v_ret=t34;
				break;
				case "t35":
					v_ret=t35;
				break;
				case "t36":
					v_ret=t36;
				break;
				case "t48":
					v_ret=t48;
				break;
				case "t55":
					v_ret=t55;
				break;
				case "t56":
					v_ret=t56;
				break;
				case "t102":
					v_ret=t102;
					break;
				case "t104":
					v_ret=t104;
					break;
				case "tAllPrimes":
					v_ret=tAllPrimes;
				break;
				default:
				break;
			}
			return v_ret;
		}
		private void mnuItemR_Click(object sender, System.EventArgs e)
		{
			try
			{
				th = getThread(lVPrio.SelectedItems[0].Text);
				if(th.ThreadState==System.Threading.ThreadState.Suspended)
				{
					th.Resume();
				}
				else
				{
					th.Start();
				}
			}
			catch
			{
			
			}
			finally
			{
				loadThreads();
			}

		}

		private void mnuItemS_Click(object sender, System.EventArgs e)
		{
			try
			{
				th = getThread(lVPrio.SelectedItems[0].Text);
				if(th.ThreadState==System.Threading.ThreadState.Running)
				{
					th.Suspend();
				}

			}
			catch
			{
			
			}
			finally
			{
				loadThreads();
			}
		
		}

		private void mnuItemNR_Click(object sender, System.EventArgs e)
		{
			try
			{
				th = getThread(lVPrio.SelectedItems[0].Text);
				if(th.ThreadState==System.Threading.ThreadState.Running)
				{
					th.Abort();
				}
			}
			catch
			{
			
			}
			finally
			{
				loadThreads();
			}

		}

		private void mnuItemH_Click(object sender, System.EventArgs e)
		{
			try
			{
				th = getThread(lVPrio.SelectedItems[0].Text);
				th.Priority = ThreadPriority.Highest;
			}
			catch
			{
			
			}
			finally
			{
				loadThreads();
			}
		
		}

		private void mnuItemAN_Click(object sender, System.EventArgs e)
		{
			try
			{
				th = getThread(lVPrio.SelectedItems[0].Text);
				th.Priority = ThreadPriority.AboveNormal;
			}
			catch
			{
			
			}
			finally
			{
				loadThreads();
			}
		
		}

		private void mnuItemN_Click(object sender, System.EventArgs e)
		{
			try
			{
				th = getThread(lVPrio.SelectedItems[0].Text);
				th.Priority = ThreadPriority.Normal;
			}
			catch
			{
			
			}
			finally
			{
				loadThreads();
			}

		}

		private void mnuItemBN_Click(object sender, System.EventArgs e)
		{
			try
			{
				th = getThread(lVPrio.SelectedItems[0].Text);
				th.Priority = ThreadPriority.BelowNormal;
			}
			catch
			{
			
			}
			finally
			{
				loadThreads();
			}

		}

		private void mnuItemL_Click(object sender, System.EventArgs e)
		{
			try
			{
				th = getThread(lVPrio.SelectedItems[0].Text);
				th.Priority = ThreadPriority.Lowest;
			}
			catch
			{
			
			}
			finally
			{
				loadThreads();
			}

		}

		private void contextMenu1_Popup(object sender, System.EventArgs e)
		{
			try
			{
				th = getThread(lVPrio.SelectedItems[0].Text);
				switch(th.Priority.ToString())
				{
					case "Highest":
						mnuItemH.Checked=true;
						mnuItemAN.Checked=false;
						mnuItemN.Checked=false;
						mnuItemBN.Checked=false;
						mnuItemL.Checked=false;
						break;
					case "AboveNormal":
						mnuItemH.Checked=false;
						mnuItemAN.Checked=true;
						mnuItemN.Checked=false;
						mnuItemBN.Checked=false;
						mnuItemL.Checked=false;
						break;
					case "Normal":
						mnuItemH.Checked=false;
						mnuItemAN.Checked=false;
						mnuItemN.Checked=true;
						mnuItemBN.Checked=false;
						mnuItemL.Checked=false;
						break;
					case "BeloNormal":
						mnuItemH.Checked=false;
						mnuItemAN.Checked=false;
						mnuItemN.Checked=false;
						mnuItemBN.Checked=true;
						mnuItemL.Checked=false;
						break;
					case "Lowest":
						mnuItemH.Checked=false;
						mnuItemAN.Checked=false;
						mnuItemN.Checked=false;
						mnuItemBN.Checked=false;
						mnuItemL.Checked=true;
						break;
				}
				switch(th.ThreadState.ToString())
				{
					case "Running":
						mnuItemR.Checked=true;
						mnuItemS.Checked=false;
						mnuItemN.Checked=false;
						break;
					case "Suspended":
						mnuItemR.Checked=false;
						mnuItemS.Checked=true;
						mnuItemN.Checked=false;
						break;
					case "Not running":
						mnuItemR.Checked=false;
						mnuItemS.Checked=false;
						mnuItemN.Checked=true;
						break;
				}
			}
			catch
			{}
		}

		private void txtAllPrimesEnd_TextChanged(object sender, System.EventArgs e)
		{
			v_endPrime=int.Parse(txtAllPrimesEnd.Text);
		}

		private void btnStart102_Click(object sender, System.EventArgs e)
		{
			btnStop104.Enabled=true;
			btnStart104.Enabled=false;

			ThreadStart ts = new ThreadStart(run102);
			
			t102 = new Thread(ts);
			t102.Priority=ThreadPriority.Lowest;
			t102.Start();
		}

		private void btnStop102_Click(object sender, System.EventArgs e)
		{
			b_running102=false;
			btnStart102.Enabled=true;
			btnStop102.Enabled=false;
		}


		




	}
}
