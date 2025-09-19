namespace Core.ui
{
    partial class FormTeachCogImageFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTeachCogImageFilter));
            this.panel13 = new System.Windows.Forms.Panel();
            this.label138 = new System.Windows.Forms.Label();
            this.panel16 = new System.Windows.Forms.Panel();
            this.label136 = new System.Windows.Forms.Label();
            this.panel17 = new System.Windows.Forms.Panel();
            this.label137 = new System.Windows.Forms.Label();
            this.ImageDisplay = new Cognex.VisionPro.CogRecordDisplay();
            this.LastRunImageDisplay = new Cognex.VisionPro.CogRecordDisplay();
            this.CurrentImageDisplay = new Cognex.VisionPro.CogRecordDisplay();
            this.rdb_Cam4 = new System.Windows.Forms.RadioButton();
            this.rdb_Cam3 = new System.Windows.Forms.RadioButton();
            this.rdb_Cam2 = new System.Windows.Forms.RadioButton();
            this.rdb_Cam1 = new System.Windows.Forms.RadioButton();
            this.lib_ToolList = new System.Windows.Forms.ListBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label59 = new System.Windows.Forms.Label();
            this.btn_ImgLoad = new System.Windows.Forms.Button();
            this.btn_LogicToolBlock = new System.Windows.Forms.Button();
            this.Panel_FindLine = new System.Windows.Forms.Panel();
            this.btn_MaskListSetData = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.cbo_MaskInside = new System.Windows.Forms.ComboBox();
            this.txt_MaskName = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.cbo_MaskBackground = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btn_MaskingDelete = new System.Windows.Forms.Button();
            this.btn_MaskingAdd = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.lib_MaskingList = new System.Windows.Forms.ListBox();
            this.btn_Run = new System.Windows.Forms.Button();
            this.btn_ApplyAndSave = new System.Windows.Forms.Button();
            this.panel14 = new System.Windows.Forms.Panel();
            this.label124 = new System.Windows.Forms.Label();
            this.cbo_ImageIndex = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel13.SuspendLayout();
            this.panel16.SuspendLayout();
            this.panel17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastRunImageDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentImageDisplay)).BeginInit();
            this.panel8.SuspendLayout();
            this.Panel_FindLine.SuspendLayout();
            this.panel14.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel13
            // 
            this.panel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel13.Controls.Add(this.label138);
            this.panel13.Location = new System.Drawing.Point(1034, 0);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(682, 32);
            this.panel13.TabIndex = 361;
            // 
            // label138
            // 
            this.label138.AutoSize = true;
            this.label138.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label138.ForeColor = System.Drawing.Color.White;
            this.label138.Location = new System.Drawing.Point(287, 2);
            this.label138.Name = "label138";
            this.label138.Size = new System.Drawing.Size(119, 24);
            this.label138.TabIndex = 0;
            this.label138.Text = "Result Image";
            // 
            // panel16
            // 
            this.panel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel16.Controls.Add(this.label136);
            this.panel16.Location = new System.Drawing.Point(514, 0);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(506, 32);
            this.panel16.TabIndex = 360;
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
            this.panel17.TabIndex = 359;
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
            this.ImageDisplay.TabIndex = 364;
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
            this.CurrentImageDisplay.Location = new System.Drawing.Point(0, 32);
            this.CurrentImageDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.CurrentImageDisplay.MouseWheelSensitivity = 1D;
            this.CurrentImageDisplay.Name = "CurrentImageDisplay";
            this.CurrentImageDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("CurrentImageDisplay.OcxState")));
            this.CurrentImageDisplay.Size = new System.Drawing.Size(508, 489);
            this.CurrentImageDisplay.TabIndex = 362;
            // 
            // rdb_Cam4
            // 
            this.rdb_Cam4.AutoSize = true;
            this.rdb_Cam4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_Cam4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.rdb_Cam4.Location = new System.Drawing.Point(335, 534);
            this.rdb_Cam4.Name = "rdb_Cam4";
            this.rdb_Cam4.Size = new System.Drawing.Size(81, 28);
            this.rdb_Cam4.TabIndex = 368;
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
            this.rdb_Cam3.TabIndex = 367;
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
            this.rdb_Cam2.TabIndex = 366;
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
            this.rdb_Cam1.TabIndex = 365;
            this.rdb_Cam1.TabStop = true;
            this.rdb_Cam1.Text = "Cam1";
            this.rdb_Cam1.UseVisualStyleBackColor = true;
            this.rdb_Cam1.Visible = false;
            this.rdb_Cam1.CheckedChanged += new System.EventHandler(this.rdb_CheckedChanged);
            // 
            // lib_ToolList
            // 
            this.lib_ToolList.FormattingEnabled = true;
            this.lib_ToolList.ItemHeight = 12;
            this.lib_ToolList.Location = new System.Drawing.Point(7, 601);
            this.lib_ToolList.Name = "lib_ToolList";
            this.lib_ToolList.Size = new System.Drawing.Size(253, 172);
            this.lib_ToolList.TabIndex = 370;
            this.lib_ToolList.SelectedIndexChanged += new System.EventHandler(this.ToolList_SelectedIndexChanged);
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel8.Controls.Add(this.label59);
            this.panel8.Location = new System.Drawing.Point(7, 567);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(253, 32);
            this.panel8.TabIndex = 369;
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
            // btn_ImgLoad
            // 
            this.btn_ImgLoad.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_ImgLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ImgLoad.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_ImgLoad.ForeColor = System.Drawing.Color.White;
            this.btn_ImgLoad.Location = new System.Drawing.Point(7, 815);
            this.btn_ImgLoad.Name = "btn_ImgLoad";
            this.btn_ImgLoad.Size = new System.Drawing.Size(120, 33);
            this.btn_ImgLoad.TabIndex = 372;
            this.btn_ImgLoad.Text = "Image Load";
            this.btn_ImgLoad.UseVisualStyleBackColor = true;
            this.btn_ImgLoad.Click += new System.EventHandler(this.btn_ImgLoad_Click);
            // 
            // btn_LogicToolBlock
            // 
            this.btn_LogicToolBlock.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_LogicToolBlock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_LogicToolBlock.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_LogicToolBlock.ForeColor = System.Drawing.Color.White;
            this.btn_LogicToolBlock.Location = new System.Drawing.Point(264, 782);
            this.btn_LogicToolBlock.Name = "btn_LogicToolBlock";
            this.btn_LogicToolBlock.Size = new System.Drawing.Size(136, 51);
            this.btn_LogicToolBlock.TabIndex = 371;
            this.btn_LogicToolBlock.Text = "Logic ToolBlock";
            this.btn_LogicToolBlock.UseVisualStyleBackColor = true;
            this.btn_LogicToolBlock.Click += new System.EventHandler(this.btn_LogicToolBlock_Click);
            // 
            // Panel_FindLine
            // 
            this.Panel_FindLine.Controls.Add(this.btn_MaskListSetData);
            this.Panel_FindLine.Controls.Add(this.label20);
            this.Panel_FindLine.Controls.Add(this.cbo_MaskInside);
            this.Panel_FindLine.Controls.Add(this.txt_MaskName);
            this.Panel_FindLine.Controls.Add(this.label17);
            this.Panel_FindLine.Controls.Add(this.cbo_MaskBackground);
            this.Panel_FindLine.Controls.Add(this.label15);
            this.Panel_FindLine.Controls.Add(this.btn_MaskingDelete);
            this.Panel_FindLine.Controls.Add(this.btn_MaskingAdd);
            this.Panel_FindLine.Controls.Add(this.label12);
            this.Panel_FindLine.Controls.Add(this.lib_MaskingList);
            this.Panel_FindLine.Controls.Add(this.panel14);
            this.Panel_FindLine.Location = new System.Drawing.Point(262, 563);
            this.Panel_FindLine.Name = "Panel_FindLine";
            this.Panel_FindLine.Size = new System.Drawing.Size(770, 213);
            this.Panel_FindLine.TabIndex = 373;
            // 
            // btn_MaskListSetData
            // 
            this.btn_MaskListSetData.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_MaskListSetData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_MaskListSetData.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_MaskListSetData.ForeColor = System.Drawing.Color.White;
            this.btn_MaskListSetData.Location = new System.Drawing.Point(46, 128);
            this.btn_MaskListSetData.Name = "btn_MaskListSetData";
            this.btn_MaskListSetData.Size = new System.Drawing.Size(108, 27);
            this.btn_MaskListSetData.TabIndex = 326;
            this.btn_MaskListSetData.Text = "ReName";
            this.btn_MaskListSetData.UseVisualStyleBackColor = true;
            this.btn_MaskListSetData.Click += new System.EventHandler(this.btn_MaskListSetData_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(43, 65);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(119, 18);
            this.label20.TabIndex = 314;
            this.label20.Text = "< Masking Name >";
            // 
            // cbo_MaskInside
            // 
            this.cbo_MaskInside.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.cbo_MaskInside.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_MaskInside.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbo_MaskInside.Font = new System.Drawing.Font("Calibri", 12F);
            this.cbo_MaskInside.ForeColor = System.Drawing.Color.White;
            this.cbo_MaskInside.FormattingEnabled = true;
            this.cbo_MaskInside.Location = new System.Drawing.Point(550, 166);
            this.cbo_MaskInside.Name = "cbo_MaskInside";
            this.cbo_MaskInside.Size = new System.Drawing.Size(130, 27);
            this.cbo_MaskInside.TabIndex = 325;
            this.cbo_MaskInside.SelectedIndexChanged += new System.EventHandler(this.cbo_MaskInside_SelectedIndexChanged);
            // 
            // txt_MaskName
            // 
            this.txt_MaskName.BackColor = System.Drawing.Color.Black;
            this.txt_MaskName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_MaskName.Font = new System.Drawing.Font("Calibri", 12F);
            this.txt_MaskName.ForeColor = System.Drawing.Color.White;
            this.txt_MaskName.Location = new System.Drawing.Point(46, 93);
            this.txt_MaskName.Name = "txt_MaskName";
            this.txt_MaskName.Size = new System.Drawing.Size(108, 27);
            this.txt_MaskName.TabIndex = 315;
            this.txt_MaskName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(554, 136);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(118, 18);
            this.label17.TabIndex = 324;
            this.label17.Text = "Inside Pixel Value";
            // 
            // cbo_MaskBackground
            // 
            this.cbo_MaskBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.cbo_MaskBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_MaskBackground.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbo_MaskBackground.Font = new System.Drawing.Font("Calibri", 12F);
            this.cbo_MaskBackground.ForeColor = System.Drawing.Color.White;
            this.cbo_MaskBackground.FormattingEnabled = true;
            this.cbo_MaskBackground.Location = new System.Drawing.Point(550, 93);
            this.cbo_MaskBackground.Name = "cbo_MaskBackground";
            this.cbo_MaskBackground.Size = new System.Drawing.Size(130, 27);
            this.cbo_MaskBackground.TabIndex = 323;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(537, 63);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(152, 18);
            this.label15.TabIndex = 322;
            this.label15.Text = "Background Pixel Value";
            // 
            // btn_MaskingDelete
            // 
            this.btn_MaskingDelete.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_MaskingDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_MaskingDelete.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_MaskingDelete.ForeColor = System.Drawing.Color.White;
            this.btn_MaskingDelete.Location = new System.Drawing.Point(202, 136);
            this.btn_MaskingDelete.Name = "btn_MaskingDelete";
            this.btn_MaskingDelete.Size = new System.Drawing.Size(77, 65);
            this.btn_MaskingDelete.TabIndex = 321;
            this.btn_MaskingDelete.Text = "Delete Masking";
            this.btn_MaskingDelete.UseVisualStyleBackColor = true;
            this.btn_MaskingDelete.Click += new System.EventHandler(this.btn_MaskingDelete_Click);
            // 
            // btn_MaskingAdd
            // 
            this.btn_MaskingAdd.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_MaskingAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_MaskingAdd.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_MaskingAdd.ForeColor = System.Drawing.Color.White;
            this.btn_MaskingAdd.Location = new System.Drawing.Point(202, 65);
            this.btn_MaskingAdd.Name = "btn_MaskingAdd";
            this.btn_MaskingAdd.Size = new System.Drawing.Size(77, 65);
            this.btn_MaskingAdd.TabIndex = 320;
            this.btn_MaskingAdd.Text = "Add Masking";
            this.btn_MaskingAdd.UseVisualStyleBackColor = true;
            this.btn_MaskingAdd.Click += new System.EventHandler(this.btn_MaskingAdd_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Calibri", 12F);
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(331, 43);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(115, 19);
            this.label12.TabIndex = 317;
            this.label12.Text = "< Masking List >";
            // 
            // lib_MaskingList
            // 
            this.lib_MaskingList.FormattingEnabled = true;
            this.lib_MaskingList.ItemHeight = 12;
            this.lib_MaskingList.Location = new System.Drawing.Point(289, 65);
            this.lib_MaskingList.Name = "lib_MaskingList";
            this.lib_MaskingList.Size = new System.Drawing.Size(196, 136);
            this.lib_MaskingList.TabIndex = 316;
            this.lib_MaskingList.SelectedIndexChanged += new System.EventHandler(this.lib_MaskingList_SelectedIndexChanged);
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
            this.btn_ApplyAndSave.Click += new System.EventHandler(this.btn_ApplyAndSave_Click);
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
            this.cbo_ImageIndex.TabIndex = 328;
            this.cbo_ImageIndex.SelectedIndexChanged += new System.EventHandler(this.cbo_ImageIndex_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(9, 782);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 23);
            this.label1.TabIndex = 327;
            this.label1.Text = "Image List : ";
            // 
            // FormTeachCogImageFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.ClientSize = new System.Drawing.Size(1720, 883);
            this.Controls.Add(this.cbo_ImageIndex);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Panel_FindLine);
            this.Controls.Add(this.btn_ImgLoad);
            this.Controls.Add(this.btn_LogicToolBlock);
            this.Controls.Add(this.lib_ToolList);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.rdb_Cam4);
            this.Controls.Add(this.rdb_Cam3);
            this.Controls.Add(this.rdb_Cam2);
            this.Controls.Add(this.rdb_Cam1);
            this.Controls.Add(this.ImageDisplay);
            this.Controls.Add(this.LastRunImageDisplay);
            this.Controls.Add(this.btn_ApplyAndSave);
            this.Controls.Add(this.btn_Run);
            this.Controls.Add(this.CurrentImageDisplay);
            this.Controls.Add(this.panel13);
            this.Controls.Add(this.panel16);
            this.Controls.Add(this.panel17);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormTeachCogImageFilter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormTeachCogImageFilter";
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.panel16.ResumeLayout(false);
            this.panel16.PerformLayout();
            this.panel17.ResumeLayout(false);
            this.panel17.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastRunImageDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentImageDisplay)).EndInit();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.Panel_FindLine.ResumeLayout(false);
            this.Panel_FindLine.PerformLayout();
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Label label138;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Label label136;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Label label137;
        public Cognex.VisionPro.CogRecordDisplay ImageDisplay;
        public Cognex.VisionPro.CogRecordDisplay LastRunImageDisplay;
        public Cognex.VisionPro.CogRecordDisplay CurrentImageDisplay;
        private System.Windows.Forms.RadioButton rdb_Cam4;
        private System.Windows.Forms.RadioButton rdb_Cam3;
        private System.Windows.Forms.RadioButton rdb_Cam2;
        private System.Windows.Forms.RadioButton rdb_Cam1;
        private System.Windows.Forms.ListBox lib_ToolList;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Button btn_ImgLoad;
        private System.Windows.Forms.Button btn_LogicToolBlock;
        private System.Windows.Forms.Panel Panel_FindLine;
        private System.Windows.Forms.Button btn_Run;
        private System.Windows.Forms.Button btn_ApplyAndSave;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Label label124;
        private System.Windows.Forms.Button btn_MaskListSetData;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox cbo_MaskInside;
        public System.Windows.Forms.TextBox txt_MaskName;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cbo_MaskBackground;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btn_MaskingDelete;
        private System.Windows.Forms.Button btn_MaskingAdd;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ListBox lib_MaskingList;
        private System.Windows.Forms.ComboBox cbo_ImageIndex;
        private System.Windows.Forms.Label label1;
    }
}