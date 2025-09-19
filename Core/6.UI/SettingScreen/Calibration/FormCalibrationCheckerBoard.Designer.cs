namespace Core.UI
{
    partial class FormCalibrationCheckerBoard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCalibrationCheckerBoard));
            this.ImageDisplayPanel = new System.Windows.Forms.Panel();
            this.btn_LogicToolBlock = new System.Windows.Forms.Button();
            this.Panel_FindLine = new System.Windows.Forms.Panel();
            this.btn_SaveCalibImg = new System.Windows.Forms.Button();
            this.btn_AllImgGrab = new System.Windows.Forms.Button();
            this.btn_AllImgLoad = new System.Windows.Forms.Button();
            this.btn_OneImgGrab = new System.Windows.Forms.Button();
            this.btn_OneImgLoad = new System.Windows.Forms.Button();
            this.tb_TileSizeY = new System.Windows.Forms.TextBox();
            this.tb_TileSizeX = new System.Windows.Forms.TextBox();
            this.lbl_TileSizeY = new System.Windows.Forms.Label();
            this.lbl_TileSizeX = new System.Windows.Forms.Label();
            this.btn_Run = new System.Windows.Forms.Button();
            this.btn_ApplyAndSave = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label35 = new System.Windows.Forms.Label();
            this.lib_ToolList = new System.Windows.Forms.ListBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label59 = new System.Windows.Forms.Label();
            this.rdb_Cam4 = new System.Windows.Forms.RadioButton();
            this.rdb_Cam3 = new System.Windows.Forms.RadioButton();
            this.rdb_Cam2 = new System.Windows.Forms.RadioButton();
            this.rdb_Cam1 = new System.Windows.Forms.RadioButton();
            this.LastRunImageDisplay = new Cognex.VisionPro.CogRecordDisplay();
            this.CurrentImageDisplay = new Cognex.VisionPro.CogRecordDisplay();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label61 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label62 = new System.Windows.Forms.Label();
            this.Panel_PMAlign = new System.Windows.Forms.Panel();
            this.button8 = new System.Windows.Forms.Button();
            this.btn_TrainPattern_Pattern = new System.Windows.Forms.Button();
            this.btn_TrainImage_Pattern = new System.Windows.Forms.Button();
            this.btn_OriginRight_Pattern = new System.Windows.Forms.Button();
            this.btn_OriginLeft_Pattern = new System.Windows.Forms.Button();
            this.btn_OriginUnder_Pattern = new System.Windows.Forms.Button();
            this.btn_OriginUp_Pattern = new System.Windows.Forms.Button();
            this.btn_FitPoint_Pattern = new System.Windows.Forms.Button();
            this.btn_FitRegion_Pattern = new System.Windows.Forms.Button();
            this.cb_RegionUse = new System.Windows.Forms.CheckBox();
            this.rb_Region = new System.Windows.Forms.RadioButton();
            this.rb_Pattern = new System.Windows.Forms.RadioButton();
            this.TrainImageDisplay = new Cognex.VisionPro.CogRecordDisplay();
            this.label70 = new System.Windows.Forms.Label();
            this.btn_Run_ContourPattern = new System.Windows.Forms.Button();
            this.btn_ApplyAndSave_ContourPattern = new System.Windows.Forms.Button();
            this.panel12 = new System.Windows.Forms.Panel();
            this.label63 = new System.Windows.Forms.Label();
            this.label83 = new System.Windows.Forms.Label();
            this.txt_GetScore_Pattern = new System.Windows.Forms.TextBox();
            this.Panel_Blob = new System.Windows.Forms.Panel();
            this.lib_BlobRunFilterList = new System.Windows.Forms.ListBox();
            this.label107 = new System.Windows.Forms.Label();
            this.label106 = new System.Windows.Forms.Label();
            this.cbo_Polarity_Blob = new System.Windows.Forms.ComboBox();
            this.txt_MinSize_Blob = new System.Windows.Forms.TextBox();
            this.label102 = new System.Windows.Forms.Label();
            this.txt_Threshold_Blob = new System.Windows.Forms.TextBox();
            this.label96 = new System.Windows.Forms.Label();
            this.label94 = new System.Windows.Forms.Label();
            this.btn_FilterDel_Blob = new System.Windows.Forms.Button();
            this.label73 = new System.Windows.Forms.Label();
            this.lib_BlobFilterList = new System.Windows.Forms.ListBox();
            this.btn_FilterAdd_Blob = new System.Windows.Forms.Button();
            this.label72 = new System.Windows.Forms.Label();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label64 = new System.Windows.Forms.Label();
            this.pb_LiveStatus = new System.Windows.Forms.PictureBox();
            this.lbl_LiveStatus = new System.Windows.Forms.Label();
            this.btn_LiveOff = new System.Windows.Forms.Button();
            this.btn_LiveOn = new System.Windows.Forms.Button();
            this.LiveTimer = new System.Windows.Forms.Timer(this.components);
            this.rdb_Cam5 = new System.Windows.Forms.RadioButton();
            this.pnl_ImageDisplay_Title = new System.Windows.Forms.Panel();
            this.lbl_ImageDisplay_Title = new System.Windows.Forms.Label();
            this.btn_Calibration_Complete = new System.Windows.Forms.Button();
            this.lbl_PixelResolution = new System.Windows.Forms.Label();
            this.lbl_PixelResolutionTitle = new System.Windows.Forms.Label();
            this.CalibStatusImgList = new System.Windows.Forms.ImageList(this.components);
            this.Panel_FindLine.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LastRunImageDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentImageDisplay)).BeginInit();
            this.panel9.SuspendLayout();
            this.panel10.SuspendLayout();
            this.Panel_PMAlign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrainImageDisplay)).BeginInit();
            this.panel12.SuspendLayout();
            this.Panel_Blob.SuspendLayout();
            this.panel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_LiveStatus)).BeginInit();
            this.pnl_ImageDisplay_Title.SuspendLayout();
            this.SuspendLayout();
            // 
            // ImageDisplayPanel
            // 
            this.ImageDisplayPanel.Location = new System.Drawing.Point(1052, 44);
            this.ImageDisplayPanel.Name = "ImageDisplayPanel";
            this.ImageDisplayPanel.Size = new System.Drawing.Size(643, 749);
            this.ImageDisplayPanel.TabIndex = 374;
            // 
            // btn_LogicToolBlock
            // 
            this.btn_LogicToolBlock.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_LogicToolBlock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_LogicToolBlock.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_LogicToolBlock.ForeColor = System.Drawing.Color.White;
            this.btn_LogicToolBlock.Location = new System.Drawing.Point(277, 796);
            this.btn_LogicToolBlock.Name = "btn_LogicToolBlock";
            this.btn_LogicToolBlock.Size = new System.Drawing.Size(136, 52);
            this.btn_LogicToolBlock.TabIndex = 373;
            this.btn_LogicToolBlock.Text = "Logic ToolBlock";
            this.btn_LogicToolBlock.UseVisualStyleBackColor = true;
            this.btn_LogicToolBlock.Click += new System.EventHandler(this.btn_LogicToolBlock_Click);
            // 
            // Panel_FindLine
            // 
            this.Panel_FindLine.Controls.Add(this.btn_SaveCalibImg);
            this.Panel_FindLine.Controls.Add(this.btn_AllImgGrab);
            this.Panel_FindLine.Controls.Add(this.btn_AllImgLoad);
            this.Panel_FindLine.Controls.Add(this.btn_OneImgGrab);
            this.Panel_FindLine.Controls.Add(this.btn_OneImgLoad);
            this.Panel_FindLine.Controls.Add(this.tb_TileSizeY);
            this.Panel_FindLine.Controls.Add(this.tb_TileSizeX);
            this.Panel_FindLine.Controls.Add(this.lbl_TileSizeY);
            this.Panel_FindLine.Controls.Add(this.lbl_TileSizeX);
            this.Panel_FindLine.Controls.Add(this.btn_Run);
            this.Panel_FindLine.Controls.Add(this.btn_ApplyAndSave);
            this.Panel_FindLine.Controls.Add(this.panel7);
            this.Panel_FindLine.Location = new System.Drawing.Point(278, 580);
            this.Panel_FindLine.Name = "Panel_FindLine";
            this.Panel_FindLine.Size = new System.Drawing.Size(770, 213);
            this.Panel_FindLine.TabIndex = 370;
            // 
            // btn_SaveCalibImg
            // 
            this.btn_SaveCalibImg.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_SaveCalibImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SaveCalibImg.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_SaveCalibImg.ForeColor = System.Drawing.Color.White;
            this.btn_SaveCalibImg.Location = new System.Drawing.Point(512, 123);
            this.btn_SaveCalibImg.Name = "btn_SaveCalibImg";
            this.btn_SaveCalibImg.Size = new System.Drawing.Size(241, 33);
            this.btn_SaveCalibImg.TabIndex = 381;
            this.btn_SaveCalibImg.Text = "Save Calibration Image";
            this.btn_SaveCalibImg.UseVisualStyleBackColor = true;
            this.btn_SaveCalibImg.Click += new System.EventHandler(this.Save_CalibImage);
            // 
            // btn_AllImgGrab
            // 
            this.btn_AllImgGrab.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_AllImgGrab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_AllImgGrab.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_AllImgGrab.ForeColor = System.Drawing.Color.White;
            this.btn_AllImgGrab.Location = new System.Drawing.Point(512, 83);
            this.btn_AllImgGrab.Name = "btn_AllImgGrab";
            this.btn_AllImgGrab.Size = new System.Drawing.Size(241, 33);
            this.btn_AllImgGrab.TabIndex = 380;
            this.btn_AllImgGrab.Text = "All Image Grab and Calibration";
            this.btn_AllImgGrab.UseVisualStyleBackColor = true;
            this.btn_AllImgGrab.Click += new System.EventHandler(this.Grab_CalibImage);
            // 
            // btn_AllImgLoad
            // 
            this.btn_AllImgLoad.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_AllImgLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_AllImgLoad.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_AllImgLoad.ForeColor = System.Drawing.Color.White;
            this.btn_AllImgLoad.Location = new System.Drawing.Point(512, 42);
            this.btn_AllImgLoad.Name = "btn_AllImgLoad";
            this.btn_AllImgLoad.Size = new System.Drawing.Size(241, 33);
            this.btn_AllImgLoad.TabIndex = 380;
            this.btn_AllImgLoad.Text = "All Image Load and Calibration";
            this.btn_AllImgLoad.UseVisualStyleBackColor = true;
            this.btn_AllImgLoad.Click += new System.EventHandler(this.Load_CalibImage);
            // 
            // btn_OneImgGrab
            // 
            this.btn_OneImgGrab.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_OneImgGrab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OneImgGrab.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_OneImgGrab.ForeColor = System.Drawing.Color.White;
            this.btn_OneImgGrab.Location = new System.Drawing.Point(257, 83);
            this.btn_OneImgGrab.Name = "btn_OneImgGrab";
            this.btn_OneImgGrab.Size = new System.Drawing.Size(241, 33);
            this.btn_OneImgGrab.TabIndex = 380;
            this.btn_OneImgGrab.Text = "One Image Grab and Calibration";
            this.btn_OneImgGrab.UseVisualStyleBackColor = true;
            this.btn_OneImgGrab.Click += new System.EventHandler(this.Grab_CalibImage);
            // 
            // btn_OneImgLoad
            // 
            this.btn_OneImgLoad.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_OneImgLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OneImgLoad.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_OneImgLoad.ForeColor = System.Drawing.Color.White;
            this.btn_OneImgLoad.Location = new System.Drawing.Point(257, 42);
            this.btn_OneImgLoad.Name = "btn_OneImgLoad";
            this.btn_OneImgLoad.Size = new System.Drawing.Size(241, 33);
            this.btn_OneImgLoad.TabIndex = 380;
            this.btn_OneImgLoad.Text = "One Image Load and Calibration";
            this.btn_OneImgLoad.UseVisualStyleBackColor = true;
            this.btn_OneImgLoad.Click += new System.EventHandler(this.Load_CalibImage);
            // 
            // tb_TileSizeY
            // 
            this.tb_TileSizeY.BackColor = System.Drawing.Color.Black;
            this.tb_TileSizeY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_TileSizeY.Font = new System.Drawing.Font("Calibri", 12F);
            this.tb_TileSizeY.ForeColor = System.Drawing.Color.White;
            this.tb_TileSizeY.Location = new System.Drawing.Point(167, 91);
            this.tb_TileSizeY.Name = "tb_TileSizeY";
            this.tb_TileSizeY.Size = new System.Drawing.Size(73, 27);
            this.tb_TileSizeY.TabIndex = 379;
            this.tb_TileSizeY.Text = "4";
            this.tb_TileSizeY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_TileSizeY.Click += new System.EventHandler(this.Click_Event);
            this.tb_TileSizeY.TextChanged += new System.EventHandler(this.textChanged);
            this.tb_TileSizeY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.text_MouseDown);
            // 
            // tb_TileSizeX
            // 
            this.tb_TileSizeX.BackColor = System.Drawing.Color.Black;
            this.tb_TileSizeX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_TileSizeX.Font = new System.Drawing.Font("Calibri", 12F);
            this.tb_TileSizeX.ForeColor = System.Drawing.Color.White;
            this.tb_TileSizeX.Location = new System.Drawing.Point(167, 58);
            this.tb_TileSizeX.Name = "tb_TileSizeX";
            this.tb_TileSizeX.Size = new System.Drawing.Size(73, 27);
            this.tb_TileSizeX.TabIndex = 379;
            this.tb_TileSizeX.Text = "4";
            this.tb_TileSizeX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_TileSizeX.Click += new System.EventHandler(this.Click_Event);
            this.tb_TileSizeX.TextChanged += new System.EventHandler(this.textChanged);
            this.tb_TileSizeX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.text_MouseDown);
            // 
            // lbl_TileSizeY
            // 
            this.lbl_TileSizeY.AutoSize = true;
            this.lbl_TileSizeY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_TileSizeY.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_TileSizeY.Location = new System.Drawing.Point(19, 91);
            this.lbl_TileSizeY.Name = "lbl_TileSizeY";
            this.lbl_TileSizeY.Size = new System.Drawing.Size(142, 20);
            this.lbl_TileSizeY.TabIndex = 378;
            this.lbl_TileSizeY.Text = "Tile Size Y[mm] :";
            // 
            // lbl_TileSizeX
            // 
            this.lbl_TileSizeX.AutoSize = true;
            this.lbl_TileSizeX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_TileSizeX.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_TileSizeX.Location = new System.Drawing.Point(19, 59);
            this.lbl_TileSizeX.Name = "lbl_TileSizeX";
            this.lbl_TileSizeX.Size = new System.Drawing.Size(142, 20);
            this.lbl_TileSizeX.TabIndex = 378;
            this.lbl_TileSizeX.Text = "Tile Size X[mm] :";
            // 
            // btn_Run
            // 
            this.btn_Run.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_Run.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Run.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_Run.ForeColor = System.Drawing.Color.White;
            this.btn_Run.Location = new System.Drawing.Point(514, 165);
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
            this.btn_ApplyAndSave.Location = new System.Drawing.Point(640, 166);
            this.btn_ApplyAndSave.Name = "btn_ApplyAndSave";
            this.btn_ApplyAndSave.Size = new System.Drawing.Size(120, 33);
            this.btn_ApplyAndSave.TabIndex = 288;
            this.btn_ApplyAndSave.Text = "Apply && Save";
            this.btn_ApplyAndSave.UseVisualStyleBackColor = true;
            this.btn_ApplyAndSave.Click += new System.EventHandler(this.ApplyAndSave_Click);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel7.Controls.Add(this.label35);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(770, 32);
            this.panel7.TabIndex = 287;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.White;
            this.label35.Location = new System.Drawing.Point(281, 2);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(139, 24);
            this.label35.TabIndex = 0;
            this.label35.Text = "Tool Parameter";
            // 
            // lib_ToolList
            // 
            this.lib_ToolList.FormattingEnabled = true;
            this.lib_ToolList.Location = new System.Drawing.Point(19, 613);
            this.lib_ToolList.Name = "lib_ToolList";
            this.lib_ToolList.Size = new System.Drawing.Size(253, 238);
            this.lib_ToolList.TabIndex = 368;
            this.lib_ToolList.SelectedIndexChanged += new System.EventHandler(this.ToolList_SelectedIndexChanged);
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel8.Controls.Add(this.label59);
            this.panel8.Location = new System.Drawing.Point(19, 579);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(253, 32);
            this.panel8.TabIndex = 361;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label59.ForeColor = System.Drawing.Color.White;
            this.label59.Location = new System.Drawing.Point(82, 2);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(80, 24);
            this.label59.TabIndex = 0;
            this.label59.Text = "Tool List";
            // 
            // rdb_Cam4
            // 
            this.rdb_Cam4.AutoSize = true;
            this.rdb_Cam4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_Cam4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.rdb_Cam4.Location = new System.Drawing.Point(347, 546);
            this.rdb_Cam4.Name = "rdb_Cam4";
            this.rdb_Cam4.Size = new System.Drawing.Size(81, 28);
            this.rdb_Cam4.TabIndex = 367;
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
            this.rdb_Cam3.Location = new System.Drawing.Point(238, 546);
            this.rdb_Cam3.Name = "rdb_Cam3";
            this.rdb_Cam3.Size = new System.Drawing.Size(81, 28);
            this.rdb_Cam3.TabIndex = 366;
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
            this.rdb_Cam2.Location = new System.Drawing.Point(128, 546);
            this.rdb_Cam2.Name = "rdb_Cam2";
            this.rdb_Cam2.Size = new System.Drawing.Size(81, 28);
            this.rdb_Cam2.TabIndex = 365;
            this.rdb_Cam2.TabStop = true;
            this.rdb_Cam2.Text = "Cam2";
            this.rdb_Cam2.UseVisualStyleBackColor = true;
            this.rdb_Cam2.Visible = false;
            this.rdb_Cam2.CheckedChanged += new System.EventHandler(this.rdb_CheckedChanged);
            // 
            // rdb_Cam1
            // 
            this.rdb_Cam1.AutoSize = true;
            this.rdb_Cam1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_Cam1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.rdb_Cam1.Location = new System.Drawing.Point(18, 546);
            this.rdb_Cam1.Name = "rdb_Cam1";
            this.rdb_Cam1.Size = new System.Drawing.Size(81, 28);
            this.rdb_Cam1.TabIndex = 364;
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
            this.LastRunImageDisplay.Location = new System.Drawing.Point(526, 44);
            this.LastRunImageDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.LastRunImageDisplay.MouseWheelSensitivity = 1D;
            this.LastRunImageDisplay.Name = "LastRunImageDisplay";
            this.LastRunImageDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("LastRunImageDisplay.OcxState")));
            this.LastRunImageDisplay.Size = new System.Drawing.Size(507, 489);
            this.LastRunImageDisplay.TabIndex = 363;
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
            this.CurrentImageDisplay.Location = new System.Drawing.Point(12, 44);
            this.CurrentImageDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.CurrentImageDisplay.MouseWheelSensitivity = 1D;
            this.CurrentImageDisplay.Name = "CurrentImageDisplay";
            this.CurrentImageDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("CurrentImageDisplay.OcxState")));
            this.CurrentImageDisplay.Size = new System.Drawing.Size(508, 489);
            this.CurrentImageDisplay.TabIndex = 362;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel9.Controls.Add(this.label61);
            this.panel9.Location = new System.Drawing.Point(526, 12);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(506, 32);
            this.panel9.TabIndex = 360;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label61.ForeColor = System.Drawing.Color.White;
            this.label61.Location = new System.Drawing.Point(183, -3);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(135, 24);
            this.label61.TabIndex = 0;
            this.label61.Text = "LastRun Image";
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel10.Controls.Add(this.label62);
            this.panel10.Location = new System.Drawing.Point(12, 12);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(508, 32);
            this.panel10.TabIndex = 359;
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label62.ForeColor = System.Drawing.Color.White;
            this.label62.Location = new System.Drawing.Point(192, -3);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(129, 24);
            this.label62.TabIndex = 0;
            this.label62.Text = "Current Image";
            // 
            // Panel_PMAlign
            // 
            this.Panel_PMAlign.Controls.Add(this.button8);
            this.Panel_PMAlign.Controls.Add(this.btn_TrainPattern_Pattern);
            this.Panel_PMAlign.Controls.Add(this.btn_TrainImage_Pattern);
            this.Panel_PMAlign.Controls.Add(this.btn_OriginRight_Pattern);
            this.Panel_PMAlign.Controls.Add(this.btn_OriginLeft_Pattern);
            this.Panel_PMAlign.Controls.Add(this.btn_OriginUnder_Pattern);
            this.Panel_PMAlign.Controls.Add(this.btn_OriginUp_Pattern);
            this.Panel_PMAlign.Controls.Add(this.btn_FitPoint_Pattern);
            this.Panel_PMAlign.Controls.Add(this.btn_FitRegion_Pattern);
            this.Panel_PMAlign.Controls.Add(this.cb_RegionUse);
            this.Panel_PMAlign.Controls.Add(this.rb_Region);
            this.Panel_PMAlign.Controls.Add(this.rb_Pattern);
            this.Panel_PMAlign.Controls.Add(this.TrainImageDisplay);
            this.Panel_PMAlign.Controls.Add(this.label70);
            this.Panel_PMAlign.Controls.Add(this.btn_Run_ContourPattern);
            this.Panel_PMAlign.Controls.Add(this.btn_ApplyAndSave_ContourPattern);
            this.Panel_PMAlign.Controls.Add(this.panel12);
            this.Panel_PMAlign.Controls.Add(this.label83);
            this.Panel_PMAlign.Controls.Add(this.txt_GetScore_Pattern);
            this.Panel_PMAlign.Location = new System.Drawing.Point(278, 580);
            this.Panel_PMAlign.Name = "Panel_PMAlign";
            this.Panel_PMAlign.Size = new System.Drawing.Size(770, 213);
            this.Panel_PMAlign.TabIndex = 371;
            // 
            // button8
            // 
            this.button8.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.ForeColor = System.Drawing.Color.White;
            this.button8.Location = new System.Drawing.Point(369, 72);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(119, 27);
            this.button8.TabIndex = 328;
            this.button8.Text = "Fit Search Region";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // btn_TrainPattern_Pattern
            // 
            this.btn_TrainPattern_Pattern.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_TrainPattern_Pattern.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_TrainPattern_Pattern.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_TrainPattern_Pattern.ForeColor = System.Drawing.Color.White;
            this.btn_TrainPattern_Pattern.Location = new System.Drawing.Point(530, 125);
            this.btn_TrainPattern_Pattern.Name = "btn_TrainPattern_Pattern";
            this.btn_TrainPattern_Pattern.Size = new System.Drawing.Size(223, 35);
            this.btn_TrainPattern_Pattern.TabIndex = 327;
            this.btn_TrainPattern_Pattern.Text = "Train Pattern";
            this.btn_TrainPattern_Pattern.UseVisualStyleBackColor = true;
            // 
            // btn_TrainImage_Pattern
            // 
            this.btn_TrainImage_Pattern.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_TrainImage_Pattern.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_TrainImage_Pattern.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_TrainImage_Pattern.ForeColor = System.Drawing.Color.White;
            this.btn_TrainImage_Pattern.Location = new System.Drawing.Point(530, 84);
            this.btn_TrainImage_Pattern.Name = "btn_TrainImage_Pattern";
            this.btn_TrainImage_Pattern.Size = new System.Drawing.Size(223, 35);
            this.btn_TrainImage_Pattern.TabIndex = 326;
            this.btn_TrainImage_Pattern.Text = "Train Image Update";
            this.btn_TrainImage_Pattern.UseVisualStyleBackColor = true;
            // 
            // btn_OriginRight_Pattern
            // 
            this.btn_OriginRight_Pattern.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_OriginRight_Pattern.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OriginRight_Pattern.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_OriginRight_Pattern.ForeColor = System.Drawing.Color.White;
            this.btn_OriginRight_Pattern.Location = new System.Drawing.Point(392, 156);
            this.btn_OriginRight_Pattern.Name = "btn_OriginRight_Pattern";
            this.btn_OriginRight_Pattern.Size = new System.Drawing.Size(53, 30);
            this.btn_OriginRight_Pattern.TabIndex = 325;
            this.btn_OriginRight_Pattern.Text = "→";
            this.btn_OriginRight_Pattern.UseVisualStyleBackColor = true;
            // 
            // btn_OriginLeft_Pattern
            // 
            this.btn_OriginLeft_Pattern.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_OriginLeft_Pattern.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OriginLeft_Pattern.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_OriginLeft_Pattern.ForeColor = System.Drawing.Color.White;
            this.btn_OriginLeft_Pattern.Location = new System.Drawing.Point(284, 156);
            this.btn_OriginLeft_Pattern.Name = "btn_OriginLeft_Pattern";
            this.btn_OriginLeft_Pattern.Size = new System.Drawing.Size(53, 30);
            this.btn_OriginLeft_Pattern.TabIndex = 324;
            this.btn_OriginLeft_Pattern.Text = "←";
            this.btn_OriginLeft_Pattern.UseVisualStyleBackColor = true;
            // 
            // btn_OriginUnder_Pattern
            // 
            this.btn_OriginUnder_Pattern.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_OriginUnder_Pattern.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OriginUnder_Pattern.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_OriginUnder_Pattern.ForeColor = System.Drawing.Color.White;
            this.btn_OriginUnder_Pattern.Location = new System.Drawing.Point(338, 174);
            this.btn_OriginUnder_Pattern.Name = "btn_OriginUnder_Pattern";
            this.btn_OriginUnder_Pattern.Size = new System.Drawing.Size(53, 30);
            this.btn_OriginUnder_Pattern.TabIndex = 323;
            this.btn_OriginUnder_Pattern.Text = "↓";
            this.btn_OriginUnder_Pattern.UseVisualStyleBackColor = true;
            // 
            // btn_OriginUp_Pattern
            // 
            this.btn_OriginUp_Pattern.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_OriginUp_Pattern.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OriginUp_Pattern.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_OriginUp_Pattern.ForeColor = System.Drawing.Color.White;
            this.btn_OriginUp_Pattern.Location = new System.Drawing.Point(338, 142);
            this.btn_OriginUp_Pattern.Name = "btn_OriginUp_Pattern";
            this.btn_OriginUp_Pattern.Size = new System.Drawing.Size(53, 30);
            this.btn_OriginUp_Pattern.TabIndex = 322;
            this.btn_OriginUp_Pattern.Text = "↑";
            this.btn_OriginUp_Pattern.UseVisualStyleBackColor = true;
            // 
            // btn_FitPoint_Pattern
            // 
            this.btn_FitPoint_Pattern.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_FitPoint_Pattern.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_FitPoint_Pattern.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_FitPoint_Pattern.ForeColor = System.Drawing.Color.White;
            this.btn_FitPoint_Pattern.Location = new System.Drawing.Point(369, 105);
            this.btn_FitPoint_Pattern.Name = "btn_FitPoint_Pattern";
            this.btn_FitPoint_Pattern.Size = new System.Drawing.Size(119, 33);
            this.btn_FitPoint_Pattern.TabIndex = 321;
            this.btn_FitPoint_Pattern.Text = "Fit Point";
            this.btn_FitPoint_Pattern.UseVisualStyleBackColor = true;
            // 
            // btn_FitRegion_Pattern
            // 
            this.btn_FitRegion_Pattern.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_FitRegion_Pattern.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_FitRegion_Pattern.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_FitRegion_Pattern.ForeColor = System.Drawing.Color.White;
            this.btn_FitRegion_Pattern.Location = new System.Drawing.Point(241, 105);
            this.btn_FitRegion_Pattern.Name = "btn_FitRegion_Pattern";
            this.btn_FitRegion_Pattern.Size = new System.Drawing.Size(119, 33);
            this.btn_FitRegion_Pattern.TabIndex = 320;
            this.btn_FitRegion_Pattern.Text = "Fit Region";
            this.btn_FitRegion_Pattern.UseVisualStyleBackColor = true;
            // 
            // cb_RegionUse
            // 
            this.cb_RegionUse.AutoSize = true;
            this.cb_RegionUse.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_RegionUse.ForeColor = System.Drawing.Color.White;
            this.cb_RegionUse.Location = new System.Drawing.Point(369, 44);
            this.cb_RegionUse.Name = "cb_RegionUse";
            this.cb_RegionUse.Size = new System.Drawing.Size(102, 23);
            this.cb_RegionUse.TabIndex = 318;
            this.cb_RegionUse.Text = "Region Use";
            this.cb_RegionUse.UseVisualStyleBackColor = true;
            // 
            // rb_Region
            // 
            this.rb_Region.AutoSize = true;
            this.rb_Region.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_Region.ForeColor = System.Drawing.Color.White;
            this.rb_Region.Location = new System.Drawing.Point(252, 72);
            this.rb_Region.Name = "rb_Region";
            this.rb_Region.Size = new System.Drawing.Size(111, 23);
            this.rb_Region.TabIndex = 317;
            this.rb_Region.TabStop = true;
            this.rb_Region.Text = "Serch Region";
            this.rb_Region.UseVisualStyleBackColor = true;
            // 
            // rb_Pattern
            // 
            this.rb_Pattern.AutoSize = true;
            this.rb_Pattern.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_Pattern.ForeColor = System.Drawing.Color.White;
            this.rb_Pattern.Location = new System.Drawing.Point(252, 44);
            this.rb_Pattern.Name = "rb_Pattern";
            this.rb_Pattern.Size = new System.Drawing.Size(74, 23);
            this.rb_Pattern.TabIndex = 316;
            this.rb_Pattern.TabStop = true;
            this.rb_Pattern.Text = "Pattern";
            this.rb_Pattern.UseVisualStyleBackColor = true;
            // 
            // TrainImageDisplay
            // 
            this.TrainImageDisplay.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.TrainImageDisplay.ColorMapLowerRoiLimit = 0D;
            this.TrainImageDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.TrainImageDisplay.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.TrainImageDisplay.ColorMapUpperRoiLimit = 1D;
            this.TrainImageDisplay.DoubleTapZoomCycleLength = 2;
            this.TrainImageDisplay.DoubleTapZoomSensitivity = 2.5D;
            this.TrainImageDisplay.Location = new System.Drawing.Point(26, 73);
            this.TrainImageDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.TrainImageDisplay.MouseWheelSensitivity = 1D;
            this.TrainImageDisplay.Name = "TrainImageDisplay";
            this.TrainImageDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("TrainImageDisplay.OcxState")));
            this.TrainImageDisplay.Size = new System.Drawing.Size(177, 130);
            this.TrainImageDisplay.TabIndex = 315;
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label70.ForeColor = System.Drawing.Color.White;
            this.label70.Location = new System.Drawing.Point(22, 42);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(187, 23);
            this.label70.TabIndex = 314;
            this.label70.Text = "< Pattern Train Image >";
            // 
            // btn_Run_ContourPattern
            // 
            this.btn_Run_ContourPattern.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_Run_ContourPattern.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Run_ContourPattern.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_Run_ContourPattern.ForeColor = System.Drawing.Color.White;
            this.btn_Run_ContourPattern.Location = new System.Drawing.Point(530, 166);
            this.btn_Run_ContourPattern.Name = "btn_Run_ContourPattern";
            this.btn_Run_ContourPattern.Size = new System.Drawing.Size(109, 33);
            this.btn_Run_ContourPattern.TabIndex = 313;
            this.btn_Run_ContourPattern.Text = "Trigger && Run";
            this.btn_Run_ContourPattern.UseVisualStyleBackColor = true;
            // 
            // btn_ApplyAndSave_ContourPattern
            // 
            this.btn_ApplyAndSave_ContourPattern.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_ApplyAndSave_ContourPattern.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ApplyAndSave_ContourPattern.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_ApplyAndSave_ContourPattern.ForeColor = System.Drawing.Color.White;
            this.btn_ApplyAndSave_ContourPattern.Location = new System.Drawing.Point(645, 166);
            this.btn_ApplyAndSave_ContourPattern.Name = "btn_ApplyAndSave_ContourPattern";
            this.btn_ApplyAndSave_ContourPattern.Size = new System.Drawing.Size(109, 33);
            this.btn_ApplyAndSave_ContourPattern.TabIndex = 288;
            this.btn_ApplyAndSave_ContourPattern.Text = "Apply && Save";
            this.btn_ApplyAndSave_ContourPattern.UseVisualStyleBackColor = true;
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel12.Controls.Add(this.label63);
            this.panel12.Location = new System.Drawing.Point(-1, 3);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(756, 32);
            this.panel12.TabIndex = 287;
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label63.ForeColor = System.Drawing.Color.White;
            this.label63.Location = new System.Drawing.Point(281, -2);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(139, 24);
            this.label63.TabIndex = 0;
            this.label63.Text = "Tool Parameter";
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.Font = new System.Drawing.Font("Calibri", 12F);
            this.label83.ForeColor = System.Drawing.Color.White;
            this.label83.Location = new System.Drawing.Point(528, 46);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(79, 19);
            this.label83.TabIndex = 307;
            this.label83.Text = "GetScore : ";
            // 
            // txt_GetScore_Pattern
            // 
            this.txt_GetScore_Pattern.BackColor = System.Drawing.Color.Black;
            this.txt_GetScore_Pattern.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_GetScore_Pattern.Font = new System.Drawing.Font("Calibri", 12F);
            this.txt_GetScore_Pattern.ForeColor = System.Drawing.Color.White;
            this.txt_GetScore_Pattern.Location = new System.Drawing.Point(613, 43);
            this.txt_GetScore_Pattern.Name = "txt_GetScore_Pattern";
            this.txt_GetScore_Pattern.Size = new System.Drawing.Size(65, 27);
            this.txt_GetScore_Pattern.TabIndex = 308;
            this.txt_GetScore_Pattern.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Panel_Blob
            // 
            this.Panel_Blob.Controls.Add(this.lib_BlobRunFilterList);
            this.Panel_Blob.Controls.Add(this.label107);
            this.Panel_Blob.Controls.Add(this.label106);
            this.Panel_Blob.Controls.Add(this.cbo_Polarity_Blob);
            this.Panel_Blob.Controls.Add(this.txt_MinSize_Blob);
            this.Panel_Blob.Controls.Add(this.label102);
            this.Panel_Blob.Controls.Add(this.txt_Threshold_Blob);
            this.Panel_Blob.Controls.Add(this.label96);
            this.Panel_Blob.Controls.Add(this.label94);
            this.Panel_Blob.Controls.Add(this.btn_FilterDel_Blob);
            this.Panel_Blob.Controls.Add(this.label73);
            this.Panel_Blob.Controls.Add(this.lib_BlobFilterList);
            this.Panel_Blob.Controls.Add(this.btn_FilterAdd_Blob);
            this.Panel_Blob.Controls.Add(this.label72);
            this.Panel_Blob.Controls.Add(this.button17);
            this.Panel_Blob.Controls.Add(this.button18);
            this.Panel_Blob.Controls.Add(this.panel11);
            this.Panel_Blob.Location = new System.Drawing.Point(278, 580);
            this.Panel_Blob.Name = "Panel_Blob";
            this.Panel_Blob.Size = new System.Drawing.Size(770, 213);
            this.Panel_Blob.TabIndex = 372;
            // 
            // lib_BlobRunFilterList
            // 
            this.lib_BlobRunFilterList.FormattingEnabled = true;
            this.lib_BlobRunFilterList.Location = new System.Drawing.Point(303, 68);
            this.lib_BlobRunFilterList.Name = "lib_BlobRunFilterList";
            this.lib_BlobRunFilterList.Size = new System.Drawing.Size(177, 108);
            this.lib_BlobRunFilterList.TabIndex = 338;
            // 
            // label107
            // 
            this.label107.AutoSize = true;
            this.label107.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label107.ForeColor = System.Drawing.Color.White;
            this.label107.Location = new System.Drawing.Point(330, 46);
            this.label107.Name = "label107";
            this.label107.Size = new System.Drawing.Size(123, 19);
            this.label107.TabIndex = 337;
            this.label107.Text = "< Running Filter >";
            // 
            // label106
            // 
            this.label106.AutoSize = true;
            this.label106.Font = new System.Drawing.Font("Calibri", 12F);
            this.label106.ForeColor = System.Drawing.Color.White;
            this.label106.Location = new System.Drawing.Point(572, 60);
            this.label106.Name = "label106";
            this.label106.Size = new System.Drawing.Size(70, 19);
            this.label106.TabIndex = 336;
            this.label106.Text = "Polarity : ";
            // 
            // cbo_Polarity_Blob
            // 
            this.cbo_Polarity_Blob.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.cbo_Polarity_Blob.ForeColor = System.Drawing.Color.White;
            this.cbo_Polarity_Blob.FormattingEnabled = true;
            this.cbo_Polarity_Blob.Location = new System.Drawing.Point(647, 59);
            this.cbo_Polarity_Blob.Name = "cbo_Polarity_Blob";
            this.cbo_Polarity_Blob.Size = new System.Drawing.Size(108, 21);
            this.cbo_Polarity_Blob.TabIndex = 335;
            // 
            // txt_MinSize_Blob
            // 
            this.txt_MinSize_Blob.BackColor = System.Drawing.Color.Black;
            this.txt_MinSize_Blob.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_MinSize_Blob.Font = new System.Drawing.Font("Calibri", 12F);
            this.txt_MinSize_Blob.ForeColor = System.Drawing.Color.White;
            this.txt_MinSize_Blob.Location = new System.Drawing.Point(647, 91);
            this.txt_MinSize_Blob.Name = "txt_MinSize_Blob";
            this.txt_MinSize_Blob.Size = new System.Drawing.Size(108, 27);
            this.txt_MinSize_Blob.TabIndex = 334;
            this.txt_MinSize_Blob.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label102
            // 
            this.label102.AutoSize = true;
            this.label102.Font = new System.Drawing.Font("Calibri", 12F);
            this.label102.ForeColor = System.Drawing.Color.White;
            this.label102.Location = new System.Drawing.Point(536, 96);
            this.label102.Name = "label102";
            this.label102.Size = new System.Drawing.Size(106, 19);
            this.label102.TabIndex = 333;
            this.label102.Text = "Insp Min Size : ";
            // 
            // txt_Threshold_Blob
            // 
            this.txt_Threshold_Blob.BackColor = System.Drawing.Color.Black;
            this.txt_Threshold_Blob.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_Threshold_Blob.Font = new System.Drawing.Font("Calibri", 12F);
            this.txt_Threshold_Blob.ForeColor = System.Drawing.Color.White;
            this.txt_Threshold_Blob.Location = new System.Drawing.Point(647, 128);
            this.txt_Threshold_Blob.Name = "txt_Threshold_Blob";
            this.txt_Threshold_Blob.Size = new System.Drawing.Size(108, 27);
            this.txt_Threshold_Blob.TabIndex = 332;
            this.txt_Threshold_Blob.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label96
            // 
            this.label96.AutoSize = true;
            this.label96.Font = new System.Drawing.Font("Calibri", 12F);
            this.label96.ForeColor = System.Drawing.Color.White;
            this.label96.Location = new System.Drawing.Point(528, 132);
            this.label96.Name = "label96";
            this.label96.Size = new System.Drawing.Size(114, 19);
            this.label96.TabIndex = 331;
            this.label96.Text = "Run Threshold : ";
            // 
            // label94
            // 
            this.label94.AutoSize = true;
            this.label94.Font = new System.Drawing.Font("Calibri", 12F);
            this.label94.ForeColor = System.Drawing.Color.White;
            this.label94.Location = new System.Drawing.Point(208, 136);
            this.label94.Name = "label94";
            this.label94.Size = new System.Drawing.Size(89, 19);
            this.label94.TabIndex = 330;
            this.label94.Text = "Filter Delete";
            // 
            // btn_FilterDel_Blob
            // 
            this.btn_FilterDel_Blob.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_FilterDel_Blob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_FilterDel_Blob.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_FilterDel_Blob.ForeColor = System.Drawing.Color.White;
            this.btn_FilterDel_Blob.Location = new System.Drawing.Point(219, 158);
            this.btn_FilterDel_Blob.Name = "btn_FilterDel_Blob";
            this.btn_FilterDel_Blob.Size = new System.Drawing.Size(67, 41);
            this.btn_FilterDel_Blob.TabIndex = 329;
            this.btn_FilterDel_Blob.Text = "◀";
            this.btn_FilterDel_Blob.UseVisualStyleBackColor = true;
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.Font = new System.Drawing.Font("Calibri", 12F);
            this.label73.ForeColor = System.Drawing.Color.White;
            this.label73.Location = new System.Drawing.Point(217, 65);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(71, 19);
            this.label73.TabIndex = 328;
            this.label73.Text = "Filter Add";
            // 
            // lib_BlobFilterList
            // 
            this.lib_BlobFilterList.FormattingEnabled = true;
            this.lib_BlobFilterList.Location = new System.Drawing.Point(23, 68);
            this.lib_BlobFilterList.Name = "lib_BlobFilterList";
            this.lib_BlobFilterList.Size = new System.Drawing.Size(177, 108);
            this.lib_BlobFilterList.TabIndex = 326;
            // 
            // btn_FilterAdd_Blob
            // 
            this.btn_FilterAdd_Blob.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_FilterAdd_Blob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_FilterAdd_Blob.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_FilterAdd_Blob.ForeColor = System.Drawing.Color.White;
            this.btn_FilterAdd_Blob.Location = new System.Drawing.Point(219, 87);
            this.btn_FilterAdd_Blob.Name = "btn_FilterAdd_Blob";
            this.btn_FilterAdd_Blob.Size = new System.Drawing.Size(67, 41);
            this.btn_FilterAdd_Blob.TabIndex = 324;
            this.btn_FilterAdd_Blob.Text = "▶";
            this.btn_FilterAdd_Blob.UseVisualStyleBackColor = true;
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label72.ForeColor = System.Drawing.Color.White;
            this.label72.Location = new System.Drawing.Point(65, 46);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(93, 19);
            this.label72.TabIndex = 314;
            this.label72.Text = "< Filter List >";
            // 
            // button17
            // 
            this.button17.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.button17.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button17.Font = new System.Drawing.Font("Calibri", 12F);
            this.button17.ForeColor = System.Drawing.Color.White;
            this.button17.Location = new System.Drawing.Point(530, 166);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(109, 33);
            this.button17.TabIndex = 313;
            this.button17.Text = "Trigger && Run";
            this.button17.UseVisualStyleBackColor = true;
            // 
            // button18
            // 
            this.button18.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.button18.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button18.Font = new System.Drawing.Font("Calibri", 12F);
            this.button18.ForeColor = System.Drawing.Color.White;
            this.button18.Location = new System.Drawing.Point(645, 166);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(109, 33);
            this.button18.TabIndex = 288;
            this.button18.Text = "Apply && Save";
            this.button18.UseVisualStyleBackColor = true;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel11.Controls.Add(this.label64);
            this.panel11.Location = new System.Drawing.Point(-1, 3);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(756, 32);
            this.panel11.TabIndex = 287;
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label64.ForeColor = System.Drawing.Color.White;
            this.label64.Location = new System.Drawing.Point(281, -2);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(139, 24);
            this.label64.TabIndex = 0;
            this.label64.Text = "Tool Parameter";
            // 
            // pb_LiveStatus
            // 
            this.pb_LiveStatus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_LiveStatus.ErrorImage = null;
            this.pb_LiveStatus.InitialImage = null;
            this.pb_LiveStatus.Location = new System.Drawing.Point(1168, 801);
            this.pb_LiveStatus.Name = "pb_LiveStatus";
            this.pb_LiveStatus.Size = new System.Drawing.Size(30, 30);
            this.pb_LiveStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_LiveStatus.TabIndex = 378;
            this.pb_LiveStatus.TabStop = false;
            // 
            // lbl_LiveStatus
            // 
            this.lbl_LiveStatus.AutoSize = true;
            this.lbl_LiveStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_LiveStatus.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_LiveStatus.Location = new System.Drawing.Point(1053, 799);
            this.lbl_LiveStatus.Name = "lbl_LiveStatus";
            this.lbl_LiveStatus.Size = new System.Drawing.Size(109, 20);
            this.lbl_LiveStatus.TabIndex = 377;
            this.lbl_LiveStatus.Text = "Live Status :";
            // 
            // btn_LiveOff
            // 
            this.btn_LiveOff.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_LiveOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_LiveOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_LiveOff.ForeColor = System.Drawing.Color.White;
            this.btn_LiveOff.Location = new System.Drawing.Point(918, 799);
            this.btn_LiveOff.Margin = new System.Windows.Forms.Padding(2);
            this.btn_LiveOff.Name = "btn_LiveOff";
            this.btn_LiveOff.Size = new System.Drawing.Size(120, 30);
            this.btn_LiveOff.TabIndex = 376;
            this.btn_LiveOff.Text = "Live Off";
            this.btn_LiveOff.UseVisualStyleBackColor = true;
            this.btn_LiveOff.Click += new System.EventHandler(this.btn_LiveOff_Click);
            // 
            // btn_LiveOn
            // 
            this.btn_LiveOn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_LiveOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_LiveOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_LiveOn.ForeColor = System.Drawing.Color.White;
            this.btn_LiveOn.Location = new System.Drawing.Point(792, 799);
            this.btn_LiveOn.Margin = new System.Windows.Forms.Padding(2);
            this.btn_LiveOn.Name = "btn_LiveOn";
            this.btn_LiveOn.Size = new System.Drawing.Size(120, 30);
            this.btn_LiveOn.TabIndex = 375;
            this.btn_LiveOn.Text = "Live On";
            this.btn_LiveOn.UseVisualStyleBackColor = true;
            this.btn_LiveOn.Click += new System.EventHandler(this.btn_LiveOn_Click);
            // 
            // LiveTimer
            // 
            this.LiveTimer.Interval = 500;
            this.LiveTimer.Tick += new System.EventHandler(this.LiveTimer_Tick);
            // 
            // rdb_Cam5
            // 
            this.rdb_Cam5.AutoSize = true;
            this.rdb_Cam5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_Cam5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.rdb_Cam5.Location = new System.Drawing.Point(445, 546);
            this.rdb_Cam5.Name = "rdb_Cam5";
            this.rdb_Cam5.Size = new System.Drawing.Size(81, 28);
            this.rdb_Cam5.TabIndex = 367;
            this.rdb_Cam5.TabStop = true;
            this.rdb_Cam5.Text = "Cam5";
            this.rdb_Cam5.UseVisualStyleBackColor = true;
            this.rdb_Cam5.Visible = false;
            this.rdb_Cam5.CheckedChanged += new System.EventHandler(this.rdb_CheckedChanged);
            // 
            // pnl_ImageDisplay_Title
            // 
            this.pnl_ImageDisplay_Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.pnl_ImageDisplay_Title.Controls.Add(this.lbl_ImageDisplay_Title);
            this.pnl_ImageDisplay_Title.Location = new System.Drawing.Point(1052, 12);
            this.pnl_ImageDisplay_Title.Name = "pnl_ImageDisplay_Title";
            this.pnl_ImageDisplay_Title.Size = new System.Drawing.Size(643, 32);
            this.pnl_ImageDisplay_Title.TabIndex = 379;
            // 
            // lbl_ImageDisplay_Title
            // 
            this.lbl_ImageDisplay_Title.AutoSize = true;
            this.lbl_ImageDisplay_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ImageDisplay_Title.ForeColor = System.Drawing.Color.White;
            this.lbl_ImageDisplay_Title.Location = new System.Drawing.Point(251, -3);
            this.lbl_ImageDisplay_Title.Name = "lbl_ImageDisplay_Title";
            this.lbl_ImageDisplay_Title.Size = new System.Drawing.Size(127, 24);
            this.lbl_ImageDisplay_Title.TabIndex = 0;
            this.lbl_ImageDisplay_Title.Text = "Display Image";
            // 
            // btn_Calibration_Complete
            // 
            this.btn_Calibration_Complete.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_Calibration_Complete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Calibration_Complete.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Calibration_Complete.ForeColor = System.Drawing.Color.White;
            this.btn_Calibration_Complete.Location = new System.Drawing.Point(1211, 799);
            this.btn_Calibration_Complete.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Calibration_Complete.Name = "btn_Calibration_Complete";
            this.btn_Calibration_Complete.Size = new System.Drawing.Size(157, 30);
            this.btn_Calibration_Complete.TabIndex = 380;
            this.btn_Calibration_Complete.Text = "Check Calib";
            this.btn_Calibration_Complete.UseVisualStyleBackColor = true;
            this.btn_Calibration_Complete.Click += new System.EventHandler(this.btn_Calibration_Complete_Click);
            // 
            // lbl_PixelResolution
            // 
            this.lbl_PixelResolution.AutoSize = true;
            this.lbl_PixelResolution.Font = new System.Drawing.Font("Calibri", 12F);
            this.lbl_PixelResolution.ForeColor = System.Drawing.Color.White;
            this.lbl_PixelResolution.Location = new System.Drawing.Point(576, 830);
            this.lbl_PixelResolution.Name = "lbl_PixelResolution";
            this.lbl_PixelResolution.Size = new System.Drawing.Size(367, 19);
            this.lbl_PixelResolution.TabIndex = 381;
            this.lbl_PixelResolution.Text = "This value will be updated when you click Trigger&&Run.";
            // 
            // lbl_PixelResolutionTitle
            // 
            this.lbl_PixelResolutionTitle.AutoSize = true;
            this.lbl_PixelResolutionTitle.Font = new System.Drawing.Font("Calibri", 12F);
            this.lbl_PixelResolutionTitle.ForeColor = System.Drawing.Color.White;
            this.lbl_PixelResolutionTitle.Location = new System.Drawing.Point(419, 830);
            this.lbl_PixelResolutionTitle.Name = "lbl_PixelResolutionTitle";
            this.lbl_PixelResolutionTitle.Size = new System.Drawing.Size(151, 19);
            this.lbl_PixelResolutionTitle.TabIndex = 382;
            this.lbl_PixelResolutionTitle.Text = "Pixel Resolution[um] :";
            // 
            // CalibStatusImgList
            // 
            this.CalibStatusImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("CalibStatusImgList.ImageStream")));
            this.CalibStatusImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.CalibStatusImgList.Images.SetKeyName(0, "LED_GRAY.gif");
            this.CalibStatusImgList.Images.SetKeyName(1, "LED_GREEN.gif");
            this.CalibStatusImgList.Images.SetKeyName(2, "LED_RED.gif");
            // 
            // FormCalibrationCheckerBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.ClientSize = new System.Drawing.Size(1720, 950);
            this.Controls.Add(this.lbl_PixelResolution);
            this.Controls.Add(this.lbl_PixelResolutionTitle);
            this.Controls.Add(this.btn_Calibration_Complete);
            this.Controls.Add(this.pnl_ImageDisplay_Title);
            this.Controls.Add(this.pb_LiveStatus);
            this.Controls.Add(this.lbl_LiveStatus);
            this.Controls.Add(this.btn_LiveOff);
            this.Controls.Add(this.btn_LiveOn);
            this.Controls.Add(this.ImageDisplayPanel);
            this.Controls.Add(this.btn_LogicToolBlock);
            this.Controls.Add(this.Panel_FindLine);
            this.Controls.Add(this.lib_ToolList);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.rdb_Cam5);
            this.Controls.Add(this.rdb_Cam4);
            this.Controls.Add(this.rdb_Cam3);
            this.Controls.Add(this.rdb_Cam2);
            this.Controls.Add(this.rdb_Cam1);
            this.Controls.Add(this.LastRunImageDisplay);
            this.Controls.Add(this.CurrentImageDisplay);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.Panel_PMAlign);
            this.Controls.Add(this.Panel_Blob);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormCalibrationCheckerBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormCalibrationCheckerBoard";
            this.Panel_FindLine.ResumeLayout(false);
            this.Panel_FindLine.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LastRunImageDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentImageDisplay)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.Panel_PMAlign.ResumeLayout(false);
            this.Panel_PMAlign.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrainImageDisplay)).EndInit();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.Panel_Blob.ResumeLayout(false);
            this.Panel_Blob.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_LiveStatus)).EndInit();
            this.pnl_ImageDisplay_Title.ResumeLayout(false);
            this.pnl_ImageDisplay_Title.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel ImageDisplayPanel;
        private System.Windows.Forms.Button btn_LogicToolBlock;
        private System.Windows.Forms.Panel Panel_FindLine;
        private System.Windows.Forms.Button btn_Run;
        private System.Windows.Forms.Button btn_ApplyAndSave;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.ListBox lib_ToolList;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.RadioButton rdb_Cam4;
        private System.Windows.Forms.RadioButton rdb_Cam3;
        private System.Windows.Forms.RadioButton rdb_Cam2;
        private System.Windows.Forms.RadioButton rdb_Cam1;
        public Cognex.VisionPro.CogRecordDisplay LastRunImageDisplay;
        public Cognex.VisionPro.CogRecordDisplay CurrentImageDisplay;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Panel Panel_PMAlign;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button btn_TrainPattern_Pattern;
        private System.Windows.Forms.Button btn_TrainImage_Pattern;
        private System.Windows.Forms.Button btn_OriginRight_Pattern;
        private System.Windows.Forms.Button btn_OriginLeft_Pattern;
        private System.Windows.Forms.Button btn_OriginUnder_Pattern;
        private System.Windows.Forms.Button btn_OriginUp_Pattern;
        private System.Windows.Forms.Button btn_FitPoint_Pattern;
        private System.Windows.Forms.Button btn_FitRegion_Pattern;
        private System.Windows.Forms.CheckBox cb_RegionUse;
        private System.Windows.Forms.RadioButton rb_Region;
        private System.Windows.Forms.RadioButton rb_Pattern;
        private Cognex.VisionPro.CogRecordDisplay TrainImageDisplay;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.Button btn_Run_ContourPattern;
        private System.Windows.Forms.Button btn_ApplyAndSave_ContourPattern;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Label label83;
        public System.Windows.Forms.TextBox txt_GetScore_Pattern;
        private System.Windows.Forms.Panel Panel_Blob;
        private System.Windows.Forms.ListBox lib_BlobRunFilterList;
        private System.Windows.Forms.Label label107;
        private System.Windows.Forms.Label label106;
        private System.Windows.Forms.ComboBox cbo_Polarity_Blob;
        public System.Windows.Forms.TextBox txt_MinSize_Blob;
        private System.Windows.Forms.Label label102;
        public System.Windows.Forms.TextBox txt_Threshold_Blob;
        private System.Windows.Forms.Label label96;
        private System.Windows.Forms.Label label94;
        private System.Windows.Forms.Button btn_FilterDel_Blob;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.ListBox lib_BlobFilterList;
        private System.Windows.Forms.Button btn_FilterAdd_Blob;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.PictureBox pb_LiveStatus;
        private System.Windows.Forms.Label lbl_LiveStatus;
        private System.Windows.Forms.Button btn_LiveOff;
        private System.Windows.Forms.Button btn_LiveOn;
        private System.Windows.Forms.Label lbl_TileSizeY;
        private System.Windows.Forms.Label lbl_TileSizeX;
        private System.Windows.Forms.Timer LiveTimer;
        public System.Windows.Forms.TextBox tb_TileSizeY;
        public System.Windows.Forms.TextBox tb_TileSizeX;
        private System.Windows.Forms.RadioButton rdb_Cam5;
        private System.Windows.Forms.Panel pnl_ImageDisplay_Title;
        private System.Windows.Forms.Label lbl_ImageDisplay_Title;
        private System.Windows.Forms.Button btn_OneImgLoad;
        private System.Windows.Forms.Button btn_AllImgGrab;
        private System.Windows.Forms.Button btn_AllImgLoad;
        private System.Windows.Forms.Button btn_OneImgGrab;
        private System.Windows.Forms.Button btn_SaveCalibImg;
        private System.Windows.Forms.Button btn_Calibration_Complete;
        private System.Windows.Forms.Label lbl_PixelResolution;
        private System.Windows.Forms.Label lbl_PixelResolutionTitle;
        private System.Windows.Forms.ImageList CalibStatusImgList;
    }
}