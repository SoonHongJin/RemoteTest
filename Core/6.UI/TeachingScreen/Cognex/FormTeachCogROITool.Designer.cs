namespace Core.UI
{
    partial class FormTeachCogROITool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTeachCogROITool));
            this.ImageDisplay = new Cognex.VisionPro.CogRecordDisplay();
            this.panel13 = new System.Windows.Forms.Panel();
            this.label138 = new System.Windows.Forms.Label();
            this.Panel_FindLine = new System.Windows.Forms.Panel();
            this.txt_CaliperContrast = new System.Windows.Forms.TextBox();
            this.txt_CaliperFilter = new System.Windows.Forms.TextBox();
            this.txt_Score = new System.Windows.Forms.TextBox();
            this.txt_NumofIgnore = new System.Windows.Forms.TextBox();
            this.txt_ProjectionLength = new System.Windows.Forms.TextBox();
            this.txt_SearchLength = new System.Windows.Forms.TextBox();
            this.txt_NumofCaliper = new System.Windows.Forms.TextBox();
            this.panel14 = new System.Windows.Forms.Panel();
            this.label124 = new System.Windows.Forms.Label();
            this.btn_FitCaliper = new System.Windows.Forms.Button();
            this.btn_Reverse = new System.Windows.Forms.Button();
            this.label127 = new System.Windows.Forms.Label();
            this.label128 = new System.Windows.Forms.Label();
            this.cbo_SearchDirection = new System.Windows.Forms.ComboBox();
            this.label129 = new System.Windows.Forms.Label();
            this.label130 = new System.Windows.Forms.Label();
            this.label131 = new System.Windows.Forms.Label();
            this.label132 = new System.Windows.Forms.Label();
            this.label133 = new System.Windows.Forms.Label();
            this.label134 = new System.Windows.Forms.Label();
            this.btn_Run = new System.Windows.Forms.Button();
            this.btn_ApplyAndSave = new System.Windows.Forms.Button();
            this.rdb_Cam4 = new System.Windows.Forms.RadioButton();
            this.rdb_Cam3 = new System.Windows.Forms.RadioButton();
            this.rdb_Cam2 = new System.Windows.Forms.RadioButton();
            this.rdb_Cam1 = new System.Windows.Forms.RadioButton();
            this.LastRunImageDisplay = new Cognex.VisionPro.CogRecordDisplay();
            this.CurrentImageDisplay = new Cognex.VisionPro.CogRecordDisplay();
            this.panel16 = new System.Windows.Forms.Panel();
            this.label136 = new System.Windows.Forms.Label();
            this.panel17 = new System.Windows.Forms.Panel();
            this.label137 = new System.Windows.Forms.Label();
            this.btn_LogicToolBlock = new System.Windows.Forms.Button();
            this.btn_ImgLoad = new System.Windows.Forms.Button();
            this.lib_ToolList = new System.Windows.Forms.ListBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label59 = new System.Windows.Forms.Label();
            this.cbo_ImageIndex = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbo_CellIndex = new System.Windows.Forms.ComboBox();
            this.lbl_CellIndex = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ImageDisplay)).BeginInit();
            this.panel13.SuspendLayout();
            this.Panel_FindLine.SuspendLayout();
            this.panel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LastRunImageDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentImageDisplay)).BeginInit();
            this.panel16.SuspendLayout();
            this.panel17.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // ImageDisplay
            // 
            this.ImageDisplay.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.ImageDisplay.ColorMapLowerRoiLimit = 0D;
            this.ImageDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.ImageDisplay.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.ImageDisplay.ColorMapUpperRoiLimit = 1D;
            this.ImageDisplay.DoubleTapZoomCycleLength = 2;
            this.ImageDisplay.DoubleTapZoomSensitivity = 2.5D;
            this.ImageDisplay.Location = new System.Drawing.Point(1034, 32);
            this.ImageDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.ImageDisplay.MouseWheelSensitivity = 1D;
            this.ImageDisplay.Name = "ImageDisplay";
            this.ImageDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("ImageDisplay.OcxState")));
            this.ImageDisplay.Size = new System.Drawing.Size(682, 749);
            this.ImageDisplay.TabIndex = 359;
            // 
            // panel13
            // 
            this.panel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel13.Controls.Add(this.label138);
            this.panel13.Location = new System.Drawing.Point(1034, 0);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(682, 32);
            this.panel13.TabIndex = 358;
            // 
            // label138
            // 
            this.label138.AutoSize = true;
            this.label138.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label138.ForeColor = System.Drawing.Color.White;
            this.label138.Location = new System.Drawing.Point(286, 2);
            this.label138.Name = "label138";
            this.label138.Size = new System.Drawing.Size(144, 24);
            this.label138.TabIndex = 0;
            this.label138.Text = "ROI Area Image";
            // 
            // Panel_FindLine
            // 
            this.Panel_FindLine.Controls.Add(this.txt_CaliperContrast);
            this.Panel_FindLine.Controls.Add(this.txt_CaliperFilter);
            this.Panel_FindLine.Controls.Add(this.txt_Score);
            this.Panel_FindLine.Controls.Add(this.txt_NumofIgnore);
            this.Panel_FindLine.Controls.Add(this.txt_ProjectionLength);
            this.Panel_FindLine.Controls.Add(this.txt_SearchLength);
            this.Panel_FindLine.Controls.Add(this.txt_NumofCaliper);
            this.Panel_FindLine.Controls.Add(this.panel14);
            this.Panel_FindLine.Controls.Add(this.btn_FitCaliper);
            this.Panel_FindLine.Controls.Add(this.btn_Reverse);
            this.Panel_FindLine.Controls.Add(this.label127);
            this.Panel_FindLine.Controls.Add(this.label128);
            this.Panel_FindLine.Controls.Add(this.cbo_SearchDirection);
            this.Panel_FindLine.Controls.Add(this.label129);
            this.Panel_FindLine.Controls.Add(this.label130);
            this.Panel_FindLine.Controls.Add(this.label131);
            this.Panel_FindLine.Controls.Add(this.label132);
            this.Panel_FindLine.Controls.Add(this.label133);
            this.Panel_FindLine.Controls.Add(this.label134);
            this.Panel_FindLine.Location = new System.Drawing.Point(262, 563);
            this.Panel_FindLine.Name = "Panel_FindLine";
            this.Panel_FindLine.Size = new System.Drawing.Size(770, 213);
            this.Panel_FindLine.TabIndex = 357;
            // 
            // txt_CaliperContrast
            // 
            this.txt_CaliperContrast.BackColor = System.Drawing.Color.Black;
            this.txt_CaliperContrast.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_CaliperContrast.Font = new System.Drawing.Font("Calibri", 12F);
            this.txt_CaliperContrast.ForeColor = System.Drawing.Color.White;
            this.txt_CaliperContrast.Location = new System.Drawing.Point(647, 123);
            this.txt_CaliperContrast.Name = "txt_CaliperContrast";
            this.txt_CaliperContrast.Size = new System.Drawing.Size(108, 27);
            this.txt_CaliperContrast.TabIndex = 312;
            this.txt_CaliperContrast.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_CaliperContrast.TextChanged += new System.EventHandler(this.textChanged);
            this.txt_CaliperContrast.DoubleClick += new System.EventHandler(this.Click_Event);
            this.txt_CaliperContrast.MouseDown += new System.Windows.Forms.MouseEventHandler(this.text_MouseDown);
            // 
            // txt_CaliperFilter
            // 
            this.txt_CaliperFilter.BackColor = System.Drawing.Color.Black;
            this.txt_CaliperFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_CaliperFilter.Font = new System.Drawing.Font("Calibri", 12F);
            this.txt_CaliperFilter.ForeColor = System.Drawing.Color.White;
            this.txt_CaliperFilter.Location = new System.Drawing.Point(647, 80);
            this.txt_CaliperFilter.Name = "txt_CaliperFilter";
            this.txt_CaliperFilter.Size = new System.Drawing.Size(108, 27);
            this.txt_CaliperFilter.TabIndex = 310;
            this.txt_CaliperFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_CaliperFilter.TextChanged += new System.EventHandler(this.textChanged);
            this.txt_CaliperFilter.DoubleClick += new System.EventHandler(this.Click_Event);
            this.txt_CaliperFilter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.text_MouseDown);
            // 
            // txt_Score
            // 
            this.txt_Score.BackColor = System.Drawing.Color.Black;
            this.txt_Score.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Score.Font = new System.Drawing.Font("Calibri", 12F);
            this.txt_Score.ForeColor = System.Drawing.Color.White;
            this.txt_Score.Location = new System.Drawing.Point(647, 42);
            this.txt_Score.Name = "txt_Score";
            this.txt_Score.Size = new System.Drawing.Size(108, 27);
            this.txt_Score.TabIndex = 308;
            this.txt_Score.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_Score.TextChanged += new System.EventHandler(this.textChanged);
            this.txt_Score.DoubleClick += new System.EventHandler(this.Click_Event);
            this.txt_Score.MouseDown += new System.Windows.Forms.MouseEventHandler(this.text_MouseDown);
            // 
            // txt_NumofIgnore
            // 
            this.txt_NumofIgnore.BackColor = System.Drawing.Color.Black;
            this.txt_NumofIgnore.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_NumofIgnore.Font = new System.Drawing.Font("Calibri", 12F);
            this.txt_NumofIgnore.ForeColor = System.Drawing.Color.White;
            this.txt_NumofIgnore.Location = new System.Drawing.Point(352, 172);
            this.txt_NumofIgnore.Name = "txt_NumofIgnore";
            this.txt_NumofIgnore.Size = new System.Drawing.Size(108, 27);
            this.txt_NumofIgnore.TabIndex = 306;
            this.txt_NumofIgnore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_NumofIgnore.TextChanged += new System.EventHandler(this.textChanged);
            this.txt_NumofIgnore.DoubleClick += new System.EventHandler(this.Click_Event);
            this.txt_NumofIgnore.MouseDown += new System.Windows.Forms.MouseEventHandler(this.text_MouseDown);
            // 
            // txt_ProjectionLength
            // 
            this.txt_ProjectionLength.BackColor = System.Drawing.Color.Black;
            this.txt_ProjectionLength.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_ProjectionLength.Font = new System.Drawing.Font("Calibri", 12F);
            this.txt_ProjectionLength.ForeColor = System.Drawing.Color.White;
            this.txt_ProjectionLength.Location = new System.Drawing.Point(352, 129);
            this.txt_ProjectionLength.Name = "txt_ProjectionLength";
            this.txt_ProjectionLength.Size = new System.Drawing.Size(108, 27);
            this.txt_ProjectionLength.TabIndex = 304;
            this.txt_ProjectionLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_ProjectionLength.TextChanged += new System.EventHandler(this.textChanged);
            this.txt_ProjectionLength.DoubleClick += new System.EventHandler(this.Click_Event);
            this.txt_ProjectionLength.MouseDown += new System.Windows.Forms.MouseEventHandler(this.text_MouseDown);
            // 
            // txt_SearchLength
            // 
            this.txt_SearchLength.BackColor = System.Drawing.Color.Black;
            this.txt_SearchLength.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_SearchLength.Font = new System.Drawing.Font("Calibri", 12F);
            this.txt_SearchLength.ForeColor = System.Drawing.Color.White;
            this.txt_SearchLength.Location = new System.Drawing.Point(352, 86);
            this.txt_SearchLength.Name = "txt_SearchLength";
            this.txt_SearchLength.Size = new System.Drawing.Size(108, 27);
            this.txt_SearchLength.TabIndex = 302;
            this.txt_SearchLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_SearchLength.TextChanged += new System.EventHandler(this.textChanged);
            this.txt_SearchLength.DoubleClick += new System.EventHandler(this.Click_Event);
            this.txt_SearchLength.MouseDown += new System.Windows.Forms.MouseEventHandler(this.text_MouseDown);
            // 
            // txt_NumofCaliper
            // 
            this.txt_NumofCaliper.BackColor = System.Drawing.Color.Black;
            this.txt_NumofCaliper.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_NumofCaliper.Font = new System.Drawing.Font("Calibri", 12F);
            this.txt_NumofCaliper.ForeColor = System.Drawing.Color.White;
            this.txt_NumofCaliper.Location = new System.Drawing.Point(352, 45);
            this.txt_NumofCaliper.Name = "txt_NumofCaliper";
            this.txt_NumofCaliper.Size = new System.Drawing.Size(108, 27);
            this.txt_NumofCaliper.TabIndex = 300;
            this.txt_NumofCaliper.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_NumofCaliper.TextChanged += new System.EventHandler(this.textChanged);
            this.txt_NumofCaliper.DoubleClick += new System.EventHandler(this.Click_Event);
            this.txt_NumofCaliper.MouseDown += new System.Windows.Forms.MouseEventHandler(this.text_MouseDown);
            // 
            // panel14
            // 
            this.panel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel14.Controls.Add(this.label124);
            this.panel14.Location = new System.Drawing.Point(1, 4);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(766, 32);
            this.panel14.TabIndex = 287;
            // 
            // label124
            // 
            this.label124.AutoSize = true;
            this.label124.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label124.ForeColor = System.Drawing.Color.White;
            this.label124.Location = new System.Drawing.Point(283, 0);
            this.label124.Name = "label124";
            this.label124.Size = new System.Drawing.Size(139, 24);
            this.label124.TabIndex = 0;
            this.label124.Text = "Tool Parameter";
            // 
            // btn_FitCaliper
            // 
            this.btn_FitCaliper.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_FitCaliper.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_FitCaliper.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_FitCaliper.ForeColor = System.Drawing.Color.White;
            this.btn_FitCaliper.Location = new System.Drawing.Point(1, 45);
            this.btn_FitCaliper.Name = "btn_FitCaliper";
            this.btn_FitCaliper.Size = new System.Drawing.Size(169, 33);
            this.btn_FitCaliper.TabIndex = 296;
            this.btn_FitCaliper.Text = "Fit Caliper";
            this.btn_FitCaliper.UseVisualStyleBackColor = true;
            this.btn_FitCaliper.Click += new System.EventHandler(this.btn_FitCaliper_Click);
            // 
            // btn_Reverse
            // 
            this.btn_Reverse.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_Reverse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Reverse.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_Reverse.ForeColor = System.Drawing.Color.White;
            this.btn_Reverse.Location = new System.Drawing.Point(1, 84);
            this.btn_Reverse.Name = "btn_Reverse";
            this.btn_Reverse.Size = new System.Drawing.Size(169, 33);
            this.btn_Reverse.TabIndex = 297;
            this.btn_Reverse.Text = "Reverse";
            this.btn_Reverse.UseVisualStyleBackColor = true;
            this.btn_Reverse.Click += new System.EventHandler(this.Reverse_Click);
            // 
            // label127
            // 
            this.label127.AutoSize = true;
            this.label127.Font = new System.Drawing.Font("Calibri", 12F);
            this.label127.ForeColor = System.Drawing.Color.White;
            this.label127.Location = new System.Drawing.Point(521, 128);
            this.label127.Name = "label127";
            this.label127.Size = new System.Drawing.Size(126, 19);
            this.label127.TabIndex = 311;
            this.label127.Text = "Caliper Contrast : ";
            // 
            // label128
            // 
            this.label128.AutoSize = true;
            this.label128.Font = new System.Drawing.Font("Calibri", 18F);
            this.label128.ForeColor = System.Drawing.Color.White;
            this.label128.Location = new System.Drawing.Point(1, 129);
            this.label128.Name = "label128";
            this.label128.Size = new System.Drawing.Size(174, 29);
            this.label128.TabIndex = 283;
            this.label128.Text = "Search Direction";
            // 
            // cbo_SearchDirection
            // 
            this.cbo_SearchDirection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.cbo_SearchDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_SearchDirection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbo_SearchDirection.Font = new System.Drawing.Font("Calibri", 12F);
            this.cbo_SearchDirection.ForeColor = System.Drawing.Color.White;
            this.cbo_SearchDirection.FormattingEnabled = true;
            this.cbo_SearchDirection.Location = new System.Drawing.Point(1, 161);
            this.cbo_SearchDirection.Name = "cbo_SearchDirection";
            this.cbo_SearchDirection.Size = new System.Drawing.Size(169, 27);
            this.cbo_SearchDirection.TabIndex = 298;
            // 
            // label129
            // 
            this.label129.AutoSize = true;
            this.label129.Font = new System.Drawing.Font("Calibri", 12F);
            this.label129.ForeColor = System.Drawing.Color.White;
            this.label129.Location = new System.Drawing.Point(542, 85);
            this.label129.Name = "label129";
            this.label129.Size = new System.Drawing.Size(104, 19);
            this.label129.TabIndex = 309;
            this.label129.Text = "Caliper Filter : ";
            // 
            // label130
            // 
            this.label130.AutoSize = true;
            this.label130.Font = new System.Drawing.Font("Calibri", 12F);
            this.label130.ForeColor = System.Drawing.Color.White;
            this.label130.Location = new System.Drawing.Point(233, 50);
            this.label130.Name = "label130";
            this.label130.Size = new System.Drawing.Size(118, 19);
            this.label130.TabIndex = 299;
            this.label130.Text = "Num of Caliper : ";
            // 
            // label131
            // 
            this.label131.AutoSize = true;
            this.label131.Font = new System.Drawing.Font("Calibri", 12F);
            this.label131.ForeColor = System.Drawing.Color.White;
            this.label131.Location = new System.Drawing.Point(591, 47);
            this.label131.Name = "label131";
            this.label131.Size = new System.Drawing.Size(56, 19);
            this.label131.TabIndex = 307;
            this.label131.Text = "Score : ";
            // 
            // label132
            // 
            this.label132.AutoSize = true;
            this.label132.Font = new System.Drawing.Font("Calibri", 12F);
            this.label132.ForeColor = System.Drawing.Color.White;
            this.label132.Location = new System.Drawing.Point(239, 91);
            this.label132.Name = "label132";
            this.label132.Size = new System.Drawing.Size(112, 19);
            this.label132.TabIndex = 301;
            this.label132.Text = "Search Length : ";
            // 
            // label133
            // 
            this.label133.AutoSize = true;
            this.label133.Font = new System.Drawing.Font("Calibri", 12F);
            this.label133.ForeColor = System.Drawing.Color.White;
            this.label133.Location = new System.Drawing.Point(237, 177);
            this.label133.Name = "label133";
            this.label133.Size = new System.Drawing.Size(113, 19);
            this.label133.TabIndex = 305;
            this.label133.Text = "Num of Ignore : ";
            // 
            // label134
            // 
            this.label134.AutoSize = true;
            this.label134.Font = new System.Drawing.Font("Calibri", 12F);
            this.label134.ForeColor = System.Drawing.Color.White;
            this.label134.Location = new System.Drawing.Point(217, 134);
            this.label134.Name = "label134";
            this.label134.Size = new System.Drawing.Size(134, 19);
            this.label134.TabIndex = 303;
            this.label134.Text = "Projection Length : ";
            // 
            // btn_Run
            // 
            this.btn_Run.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_Run.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Run.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_Run.ForeColor = System.Drawing.Color.White;
            this.btn_Run.Location = new System.Drawing.Point(777, 783);
            this.btn_Run.Name = "btn_Run";
            this.btn_Run.Size = new System.Drawing.Size(120, 33);
            this.btn_Run.TabIndex = 313;
            this.btn_Run.Text = "Trigger && Run";
            this.btn_Run.UseVisualStyleBackColor = true;
            this.btn_Run.Click += new System.EventHandler(this.btn_Run_Click);
            // 
            // btn_ApplyAndSave
            // 
            this.btn_ApplyAndSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_ApplyAndSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ApplyAndSave.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_ApplyAndSave.ForeColor = System.Drawing.Color.White;
            this.btn_ApplyAndSave.Location = new System.Drawing.Point(912, 783);
            this.btn_ApplyAndSave.Name = "btn_ApplyAndSave";
            this.btn_ApplyAndSave.Size = new System.Drawing.Size(120, 33);
            this.btn_ApplyAndSave.TabIndex = 288;
            this.btn_ApplyAndSave.Text = "Apply && Save";
            this.btn_ApplyAndSave.UseVisualStyleBackColor = true;
            this.btn_ApplyAndSave.Click += new System.EventHandler(this.ApplyAndSave_Click);
            // 
            // rdb_Cam4
            // 
            this.rdb_Cam4.AutoSize = true;
            this.rdb_Cam4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_Cam4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.rdb_Cam4.Location = new System.Drawing.Point(335, 534);
            this.rdb_Cam4.Name = "rdb_Cam4";
            this.rdb_Cam4.Size = new System.Drawing.Size(81, 28);
            this.rdb_Cam4.TabIndex = 355;
            this.rdb_Cam4.TabStop = true;
            this.rdb_Cam4.Text = "Cam4";
            this.rdb_Cam4.UseVisualStyleBackColor = true;
            this.rdb_Cam4.Visible = false;
            this.rdb_Cam4.CheckedChanged += new System.EventHandler(this.rdb_CheckedChanged);
            // 
            // rdb_Cam3
            // 
            this.rdb_Cam3.AutoSize = true;
            this.rdb_Cam3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_Cam3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.rdb_Cam3.Location = new System.Drawing.Point(226, 534);
            this.rdb_Cam3.Name = "rdb_Cam3";
            this.rdb_Cam3.Size = new System.Drawing.Size(81, 28);
            this.rdb_Cam3.TabIndex = 354;
            this.rdb_Cam3.TabStop = true;
            this.rdb_Cam3.Text = "Cam3";
            this.rdb_Cam3.UseVisualStyleBackColor = true;
            this.rdb_Cam3.Visible = false;
            this.rdb_Cam3.CheckedChanged += new System.EventHandler(this.rdb_CheckedChanged);
            // 
            // rdb_Cam2
            // 
            this.rdb_Cam2.AutoSize = true;
            this.rdb_Cam2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_Cam2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.rdb_Cam2.Location = new System.Drawing.Point(116, 534);
            this.rdb_Cam2.Name = "rdb_Cam2";
            this.rdb_Cam2.Size = new System.Drawing.Size(81, 28);
            this.rdb_Cam2.TabIndex = 353;
            this.rdb_Cam2.TabStop = true;
            this.rdb_Cam2.Text = "Cam2";
            this.rdb_Cam2.UseVisualStyleBackColor = true;
            this.rdb_Cam2.Visible = false;
            this.rdb_Cam2.CheckedChanged += new System.EventHandler(this.rdb_CheckedChanged);
            // 
            // rdb_Cam1
            // 
            this.rdb_Cam1.AutoSize = true;
            this.rdb_Cam1.Checked = true;
            this.rdb_Cam1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_Cam1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.rdb_Cam1.Location = new System.Drawing.Point(6, 534);
            this.rdb_Cam1.Name = "rdb_Cam1";
            this.rdb_Cam1.Size = new System.Drawing.Size(81, 28);
            this.rdb_Cam1.TabIndex = 352;
            this.rdb_Cam1.TabStop = true;
            this.rdb_Cam1.Text = "Cam1";
            this.rdb_Cam1.UseVisualStyleBackColor = true;
            this.rdb_Cam1.Visible = false;
            this.rdb_Cam1.CheckedChanged += new System.EventHandler(this.rdb_CheckedChanged);
            // 
            // LastRunImageDisplay
            // 
            this.LastRunImageDisplay.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.LastRunImageDisplay.ColorMapLowerRoiLimit = 0D;
            this.LastRunImageDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.LastRunImageDisplay.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.LastRunImageDisplay.ColorMapUpperRoiLimit = 1D;
            this.LastRunImageDisplay.DoubleTapZoomCycleLength = 2;
            this.LastRunImageDisplay.DoubleTapZoomSensitivity = 2.5D;
            this.LastRunImageDisplay.Location = new System.Drawing.Point(514, 32);
            this.LastRunImageDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.LastRunImageDisplay.MouseWheelSensitivity = 1D;
            this.LastRunImageDisplay.Name = "LastRunImageDisplay";
            this.LastRunImageDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("LastRunImageDisplay.OcxState")));
            this.LastRunImageDisplay.Size = new System.Drawing.Size(507, 489);
            this.LastRunImageDisplay.TabIndex = 351;
            // 
            // CurrentImageDisplay
            // 
            this.CurrentImageDisplay.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.CurrentImageDisplay.ColorMapLowerRoiLimit = 0D;
            this.CurrentImageDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.CurrentImageDisplay.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.CurrentImageDisplay.ColorMapUpperRoiLimit = 1D;
            this.CurrentImageDisplay.DoubleTapZoomCycleLength = 2;
            this.CurrentImageDisplay.DoubleTapZoomSensitivity = 2.5D;
            this.CurrentImageDisplay.Location = new System.Drawing.Point(0, 32);
            this.CurrentImageDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.CurrentImageDisplay.MouseWheelSensitivity = 1D;
            this.CurrentImageDisplay.Name = "CurrentImageDisplay";
            this.CurrentImageDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("CurrentImageDisplay.OcxState")));
            this.CurrentImageDisplay.Size = new System.Drawing.Size(508, 489);
            this.CurrentImageDisplay.TabIndex = 350;
            this.CurrentImageDisplay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CurrentImageDisplay_MouseUp);
            // 
            // panel16
            // 
            this.panel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel16.Controls.Add(this.label136);
            this.panel16.Location = new System.Drawing.Point(514, 0);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(506, 32);
            this.panel16.TabIndex = 348;
            // 
            // label136
            // 
            this.label136.AutoSize = true;
            this.label136.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label136.ForeColor = System.Drawing.Color.White;
            this.label136.Location = new System.Drawing.Point(185, 1);
            this.label136.Name = "label136";
            this.label136.Size = new System.Drawing.Size(135, 24);
            this.label136.TabIndex = 0;
            this.label136.Text = "LastRun Image";
            // 
            // panel17
            // 
            this.panel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel17.Controls.Add(this.label137);
            this.panel17.Location = new System.Drawing.Point(0, 0);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(508, 32);
            this.panel17.TabIndex = 347;
            // 
            // label137
            // 
            this.label137.AutoSize = true;
            this.label137.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label137.ForeColor = System.Drawing.Color.White;
            this.label137.Location = new System.Drawing.Point(194, 1);
            this.label137.Name = "label137";
            this.label137.Size = new System.Drawing.Size(129, 24);
            this.label137.TabIndex = 0;
            this.label137.Text = "Current Image";
            // 
            // btn_LogicToolBlock
            // 
            this.btn_LogicToolBlock.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_LogicToolBlock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_LogicToolBlock.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_LogicToolBlock.ForeColor = System.Drawing.Color.White;
            this.btn_LogicToolBlock.Location = new System.Drawing.Point(264, 782);
            this.btn_LogicToolBlock.Name = "btn_LogicToolBlock";
            this.btn_LogicToolBlock.Size = new System.Drawing.Size(136, 60);
            this.btn_LogicToolBlock.TabIndex = 360;
            this.btn_LogicToolBlock.Text = "Logic ToolBlock";
            this.btn_LogicToolBlock.UseVisualStyleBackColor = true;
            this.btn_LogicToolBlock.Click += new System.EventHandler(this.btn_LogicToolBlock_Click);
            // 
            // btn_ImgLoad
            // 
            this.btn_ImgLoad.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_ImgLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ImgLoad.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_ImgLoad.ForeColor = System.Drawing.Color.White;
            this.btn_ImgLoad.Location = new System.Drawing.Point(406, 783);
            this.btn_ImgLoad.Name = "btn_ImgLoad";
            this.btn_ImgLoad.Size = new System.Drawing.Size(120, 59);
            this.btn_ImgLoad.TabIndex = 361;
            this.btn_ImgLoad.Text = "Image Load";
            this.btn_ImgLoad.UseVisualStyleBackColor = true;
            this.btn_ImgLoad.Click += new System.EventHandler(this.btm_ImgLoad_Click);
            // 
            // lib_ToolList
            // 
            this.lib_ToolList.FormattingEnabled = true;
            this.lib_ToolList.ItemHeight = 12;
            this.lib_ToolList.Location = new System.Drawing.Point(7, 601);
            this.lib_ToolList.Name = "lib_ToolList";
            this.lib_ToolList.Size = new System.Drawing.Size(253, 172);
            this.lib_ToolList.TabIndex = 363;
            this.lib_ToolList.SelectedIndexChanged += new System.EventHandler(this.ToolList_SelectedIndexChanged);
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel8.Controls.Add(this.label59);
            this.panel8.Location = new System.Drawing.Point(7, 567);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(253, 32);
            this.panel8.TabIndex = 362;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label59.ForeColor = System.Drawing.Color.White;
            this.label59.Location = new System.Drawing.Point(82, -2);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(80, 24);
            this.label59.TabIndex = 0;
            this.label59.Text = "Tool List";
            // 
            // cbo_ImageIndex
            // 
            this.cbo_ImageIndex.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.cbo_ImageIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_ImageIndex.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbo_ImageIndex.Font = new System.Drawing.Font("Calibri", 12F);
            this.cbo_ImageIndex.ForeColor = System.Drawing.Color.White;
            this.cbo_ImageIndex.FormattingEnabled = true;
            this.cbo_ImageIndex.Location = new System.Drawing.Point(110, 782);
            this.cbo_ImageIndex.Name = "cbo_ImageIndex";
            this.cbo_ImageIndex.Size = new System.Drawing.Size(130, 27);
            this.cbo_ImageIndex.TabIndex = 419;
            this.cbo_ImageIndex.SelectedIndexChanged += new System.EventHandler(this.cbo_ImageIndex_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(9, 782);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 23);
            this.label3.TabIndex = 418;
            this.label3.Text = "Image List : ";
            // 
            // cbo_CellIndex
            // 
            this.cbo_CellIndex.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.cbo_CellIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_CellIndex.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbo_CellIndex.Font = new System.Drawing.Font("Calibri", 12F);
            this.cbo_CellIndex.ForeColor = System.Drawing.Color.White;
            this.cbo_CellIndex.FormattingEnabled = true;
            this.cbo_CellIndex.Location = new System.Drawing.Point(110, 815);
            this.cbo_CellIndex.Name = "cbo_CellIndex";
            this.cbo_CellIndex.Size = new System.Drawing.Size(130, 27);
            this.cbo_CellIndex.TabIndex = 421;
            this.cbo_CellIndex.SelectedIndexChanged += new System.EventHandler(this.cbo_CellIndex_SelectedIndexChanged);
            // 
            // lbl_CellIndex
            // 
            this.lbl_CellIndex.AutoSize = true;
            this.lbl_CellIndex.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CellIndex.ForeColor = System.Drawing.Color.White;
            this.lbl_CellIndex.Location = new System.Drawing.Point(9, 815);
            this.lbl_CellIndex.Name = "lbl_CellIndex";
            this.lbl_CellIndex.Size = new System.Drawing.Size(97, 23);
            this.lbl_CellIndex.TabIndex = 420;
            this.lbl_CellIndex.Text = "Cell Index : ";
            // 
            // FormTeachCogROITool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.ClientSize = new System.Drawing.Size(1720, 883);
            this.Controls.Add(this.cbo_CellIndex);
            this.Controls.Add(this.lbl_CellIndex);
            this.Controls.Add(this.cbo_ImageIndex);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lib_ToolList);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.btn_ImgLoad);
            this.Controls.Add(this.btn_LogicToolBlock);
            this.Controls.Add(this.ImageDisplay);
            this.Controls.Add(this.btn_Run);
            this.Controls.Add(this.panel13);
            this.Controls.Add(this.btn_ApplyAndSave);
            this.Controls.Add(this.Panel_FindLine);
            this.Controls.Add(this.rdb_Cam4);
            this.Controls.Add(this.rdb_Cam3);
            this.Controls.Add(this.rdb_Cam2);
            this.Controls.Add(this.rdb_Cam1);
            this.Controls.Add(this.LastRunImageDisplay);
            this.Controls.Add(this.CurrentImageDisplay);
            this.Controls.Add(this.panel16);
            this.Controls.Add(this.panel17);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormTeachCogROITool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "0";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormTeachInspectionCrack_FormClosed);
            this.VisibleChanged += new System.EventHandler(this.FormTeachInspectionCrack_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.ImageDisplay)).EndInit();
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.Panel_FindLine.ResumeLayout(false);
            this.Panel_FindLine.PerformLayout();
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LastRunImageDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentImageDisplay)).EndInit();
            this.panel16.ResumeLayout(false);
            this.panel16.PerformLayout();
            this.panel17.ResumeLayout(false);
            this.panel17.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Cognex.VisionPro.CogRecordDisplay ImageDisplay;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Label label138;
        private System.Windows.Forms.Panel Panel_FindLine;
        private System.Windows.Forms.Button btn_Run;
        private System.Windows.Forms.Button btn_ApplyAndSave;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Label label124;
        private System.Windows.Forms.Button btn_FitCaliper;
        private System.Windows.Forms.Button btn_Reverse;
        private System.Windows.Forms.Label label127;
        private System.Windows.Forms.Label label128;
        public System.Windows.Forms.TextBox txt_CaliperContrast;
        private System.Windows.Forms.ComboBox cbo_SearchDirection;
        private System.Windows.Forms.Label label129;
        public System.Windows.Forms.TextBox txt_NumofCaliper;
        public System.Windows.Forms.TextBox txt_CaliperFilter;
        private System.Windows.Forms.Label label130;
        private System.Windows.Forms.Label label131;
        public System.Windows.Forms.TextBox txt_SearchLength;
        public System.Windows.Forms.TextBox txt_Score;
        private System.Windows.Forms.Label label132;
        private System.Windows.Forms.Label label133;
        public System.Windows.Forms.TextBox txt_ProjectionLength;
        public System.Windows.Forms.TextBox txt_NumofIgnore;
        private System.Windows.Forms.Label label134;
        private System.Windows.Forms.RadioButton rdb_Cam4;
        private System.Windows.Forms.RadioButton rdb_Cam3;
        private System.Windows.Forms.RadioButton rdb_Cam2;
        private System.Windows.Forms.RadioButton rdb_Cam1;
        public Cognex.VisionPro.CogRecordDisplay LastRunImageDisplay;
        public Cognex.VisionPro.CogRecordDisplay CurrentImageDisplay;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Label label136;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Label label137;
        private System.Windows.Forms.Button btn_LogicToolBlock;
        private System.Windows.Forms.Button btn_ImgLoad;
        private System.Windows.Forms.ListBox lib_ToolList;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.ComboBox cbo_ImageIndex;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbo_CellIndex;
        private System.Windows.Forms.Label lbl_CellIndex;
    }
}