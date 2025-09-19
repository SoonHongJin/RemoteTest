namespace Core.UI
{
    partial class FormParameterModelScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormParameterModelScreen));
            this.lbl_ModelParameter = new System.Windows.Forms.Label();
            this.panel15 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbl_ModellList = new System.Windows.Forms.Label();
            this.lst_ModelList = new System.Windows.Forms.ListBox();
            this.btn_NewModel = new System.Windows.Forms.Button();
            this.btn_LoadModel = new System.Windows.Forms.Button();
            this.btn_CopyModel = new System.Windows.Forms.Button();
            this.btn_RenameModel = new System.Windows.Forms.Button();
            this.btn_DeleteModel = new System.Windows.Forms.Button();
            this.btn_RefreshModel = new System.Windows.Forms.Button();
            this.pnl_RecipeList = new System.Windows.Forms.Panel();
            this.btn_CamSetApplySave = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.btn_DeleteConditionRow = new System.Windows.Forms.Button();
            this.btn_AddConditionRow = new System.Windows.Forms.Button();
            this.dg_ConditionSet = new System.Windows.Forms.DataGridView();
            this.chb_Activate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_cb_ImageSaveCondition_Condition = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dg_cb_ImageSaveCondition_Logic = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chb_RedImage = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.chb_GreenImage = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.chb_BlueImage = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.chb_BackImage = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.chb_ColorImage = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dg_ImagePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnl_ConditionSave = new System.Windows.Forms.Panel();
            this.pnl_AutoDelete2 = new System.Windows.Forms.Panel();
            this.label22 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.rb_DriveLimit = new System.Windows.Forms.RadioButton();
            this.tb_DriveLimit = new System.Windows.Forms.TextBox();
            this.tb_DateLimit = new System.Windows.Forms.TextBox();
            this.rb_DateLimit = new System.Windows.Forms.RadioButton();
            this.pnl_AutoDelete = new System.Windows.Forms.Panel();
            this.label21 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.AlignTarget1ImgList = new System.Windows.Forms.ImageList(this.components);
            this.AlignTarget2ImgList = new System.Windows.Forms.ImageList(this.components);
            this.panel15.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnl_RecipeList.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_ConditionSet)).BeginInit();
            this.pnl_ConditionSave.SuspendLayout();
            this.pnl_AutoDelete2.SuspendLayout();
            this.pnl_AutoDelete.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_ModelParameter
            // 
            this.lbl_ModelParameter.AutoSize = true;
            this.lbl_ModelParameter.Font = new System.Drawing.Font("Calibri", 18F);
            this.lbl_ModelParameter.ForeColor = System.Drawing.Color.White;
            this.lbl_ModelParameter.Location = new System.Drawing.Point(772, 1);
            this.lbl_ModelParameter.Name = "lbl_ModelParameter";
            this.lbl_ModelParameter.Size = new System.Drawing.Size(185, 29);
            this.lbl_ModelParameter.TabIndex = 0;
            this.lbl_ModelParameter.Text = "Model Parameter";
            // 
            // panel15
            // 
            this.panel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel15.Controls.Add(this.lbl_ModelParameter);
            this.panel15.Location = new System.Drawing.Point(0, 0);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(1720, 32);
            this.panel15.TabIndex = 41;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel3.Controls.Add(this.lbl_ModellList);
            this.panel3.Location = new System.Drawing.Point(27, 55);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(377, 32);
            this.panel3.TabIndex = 43;
            // 
            // lbl_ModellList
            // 
            this.lbl_ModellList.AutoSize = true;
            this.lbl_ModellList.Font = new System.Drawing.Font("Calibri", 15F);
            this.lbl_ModellList.ForeColor = System.Drawing.Color.White;
            this.lbl_ModellList.Location = new System.Drawing.Point(140, 3);
            this.lbl_ModellList.Name = "lbl_ModellList";
            this.lbl_ModellList.Size = new System.Drawing.Size(97, 24);
            this.lbl_ModellList.TabIndex = 0;
            this.lbl_ModellList.Text = "Model List";
            // 
            // lst_ModelList
            // 
            this.lst_ModelList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.lst_ModelList.Font = new System.Drawing.Font("Calibri", 12F);
            this.lst_ModelList.ForeColor = System.Drawing.Color.White;
            this.lst_ModelList.FormattingEnabled = true;
            this.lst_ModelList.ItemHeight = 19;
            this.lst_ModelList.Items.AddRange(new object[] {
            "Model_1",
            "Model_2"});
            this.lst_ModelList.Location = new System.Drawing.Point(23, 17);
            this.lst_ModelList.Name = "lst_ModelList";
            this.lst_ModelList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lst_ModelList.Size = new System.Drawing.Size(193, 270);
            this.lst_ModelList.TabIndex = 104;
            this.lst_ModelList.SelectedIndexChanged += new System.EventHandler(this.lst_ModelList_SelectedIndexChanged);
            // 
            // btn_NewModel
            // 
            this.btn_NewModel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_NewModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_NewModel.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_NewModel.ForeColor = System.Drawing.Color.White;
            this.btn_NewModel.Location = new System.Drawing.Point(237, 17);
            this.btn_NewModel.Name = "btn_NewModel";
            this.btn_NewModel.Size = new System.Drawing.Size(117, 33);
            this.btn_NewModel.TabIndex = 105;
            this.btn_NewModel.Text = "New Model";
            this.btn_NewModel.UseVisualStyleBackColor = true;
            this.btn_NewModel.Click += new System.EventHandler(this.btn_NewModel_Click);
            // 
            // btn_LoadModel
            // 
            this.btn_LoadModel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_LoadModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_LoadModel.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_LoadModel.ForeColor = System.Drawing.Color.White;
            this.btn_LoadModel.Location = new System.Drawing.Point(237, 72);
            this.btn_LoadModel.Name = "btn_LoadModel";
            this.btn_LoadModel.Size = new System.Drawing.Size(117, 33);
            this.btn_LoadModel.TabIndex = 106;
            this.btn_LoadModel.Text = "Load Model";
            this.btn_LoadModel.UseVisualStyleBackColor = true;
            this.btn_LoadModel.Click += new System.EventHandler(this.btn_LoadModel_Click);
            // 
            // btn_CopyModel
            // 
            this.btn_CopyModel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_CopyModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_CopyModel.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_CopyModel.ForeColor = System.Drawing.Color.White;
            this.btn_CopyModel.Location = new System.Drawing.Point(237, 127);
            this.btn_CopyModel.Name = "btn_CopyModel";
            this.btn_CopyModel.Size = new System.Drawing.Size(117, 33);
            this.btn_CopyModel.TabIndex = 108;
            this.btn_CopyModel.Text = "Copy Model";
            this.btn_CopyModel.UseVisualStyleBackColor = true;
            this.btn_CopyModel.Click += new System.EventHandler(this.btn_CopyModel_Click);
            // 
            // btn_RenameModel
            // 
            this.btn_RenameModel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_RenameModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_RenameModel.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_RenameModel.ForeColor = System.Drawing.Color.White;
            this.btn_RenameModel.Location = new System.Drawing.Point(237, 182);
            this.btn_RenameModel.Name = "btn_RenameModel";
            this.btn_RenameModel.Size = new System.Drawing.Size(117, 33);
            this.btn_RenameModel.TabIndex = 109;
            this.btn_RenameModel.Text = "Rename Model";
            this.btn_RenameModel.UseVisualStyleBackColor = true;
            this.btn_RenameModel.Click += new System.EventHandler(this.btn_RenameModel_Click);
            // 
            // btn_DeleteModel
            // 
            this.btn_DeleteModel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_DeleteModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DeleteModel.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_DeleteModel.ForeColor = System.Drawing.Color.White;
            this.btn_DeleteModel.Location = new System.Drawing.Point(237, 237);
            this.btn_DeleteModel.Name = "btn_DeleteModel";
            this.btn_DeleteModel.Size = new System.Drawing.Size(117, 33);
            this.btn_DeleteModel.TabIndex = 110;
            this.btn_DeleteModel.Text = "Delete Model";
            this.btn_DeleteModel.UseVisualStyleBackColor = true;
            this.btn_DeleteModel.Click += new System.EventHandler(this.btn_DeleteModel_Click);
            // 
            // btn_RefreshModel
            // 
            this.btn_RefreshModel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_RefreshModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_RefreshModel.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_RefreshModel.ForeColor = System.Drawing.Color.White;
            this.btn_RefreshModel.Location = new System.Drawing.Point(237, 292);
            this.btn_RefreshModel.Name = "btn_RefreshModel";
            this.btn_RefreshModel.Size = new System.Drawing.Size(117, 33);
            this.btn_RefreshModel.TabIndex = 111;
            this.btn_RefreshModel.Text = "Refresh Model";
            this.btn_RefreshModel.UseVisualStyleBackColor = true;
            this.btn_RefreshModel.Click += new System.EventHandler(this.btn_RefreshModel_Click);
            // 
            // pnl_RecipeList
            // 
            this.pnl_RecipeList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_RecipeList.Controls.Add(this.lst_ModelList);
            this.pnl_RecipeList.Controls.Add(this.btn_RefreshModel);
            this.pnl_RecipeList.Controls.Add(this.btn_NewModel);
            this.pnl_RecipeList.Controls.Add(this.btn_DeleteModel);
            this.pnl_RecipeList.Controls.Add(this.btn_LoadModel);
            this.pnl_RecipeList.Controls.Add(this.btn_RenameModel);
            this.pnl_RecipeList.Controls.Add(this.btn_CopyModel);
            this.pnl_RecipeList.Location = new System.Drawing.Point(27, 87);
            this.pnl_RecipeList.Name = "pnl_RecipeList";
            this.pnl_RecipeList.Size = new System.Drawing.Size(377, 345);
            this.pnl_RecipeList.TabIndex = 113;
            // 
            // btn_CamSetApplySave
            // 
            this.btn_CamSetApplySave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_CamSetApplySave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_CamSetApplySave.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_CamSetApplySave.ForeColor = System.Drawing.Color.White;
            this.btn_CamSetApplySave.Location = new System.Drawing.Point(1408, 860);
            this.btn_CamSetApplySave.Name = "btn_CamSetApplySave";
            this.btn_CamSetApplySave.Size = new System.Drawing.Size(143, 33);
            this.btn_CamSetApplySave.TabIndex = 152;
            this.btn_CamSetApplySave.Text = "Apply && Save";
            this.btn_CamSetApplySave.UseVisualStyleBackColor = true;
            this.btn_CamSetApplySave.Click += new System.EventHandler(this.btn_CamSetApplySave_Click);
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel8.Controls.Add(this.label5);
            this.panel8.Location = new System.Drawing.Point(410, 55);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(675, 32);
            this.panel8.TabIndex = 118;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 15F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(226, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(171, 24);
            this.label5.TabIndex = 0;
            this.label5.Text = "Image Save Control";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Calibri", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(13, 11);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(132, 24);
            this.label20.TabIndex = 369;
            this.label20.Text = "Conditions Set";
            // 
            // btn_DeleteConditionRow
            // 
            this.btn_DeleteConditionRow.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_DeleteConditionRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DeleteConditionRow.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_DeleteConditionRow.ForeColor = System.Drawing.Color.White;
            this.btn_DeleteConditionRow.Location = new System.Drawing.Point(616, 6);
            this.btn_DeleteConditionRow.Name = "btn_DeleteConditionRow";
            this.btn_DeleteConditionRow.Size = new System.Drawing.Size(50, 29);
            this.btn_DeleteConditionRow.TabIndex = 368;
            this.btn_DeleteConditionRow.Text = "-";
            this.btn_DeleteConditionRow.UseVisualStyleBackColor = true;
            this.btn_DeleteConditionRow.Click += new System.EventHandler(this.btn_DeleteConditionRow_Click);
            // 
            // btn_AddConditionRow
            // 
            this.btn_AddConditionRow.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_AddConditionRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_AddConditionRow.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_AddConditionRow.ForeColor = System.Drawing.Color.White;
            this.btn_AddConditionRow.Location = new System.Drawing.Point(560, 6);
            this.btn_AddConditionRow.Name = "btn_AddConditionRow";
            this.btn_AddConditionRow.Size = new System.Drawing.Size(50, 29);
            this.btn_AddConditionRow.TabIndex = 112;
            this.btn_AddConditionRow.Text = "+";
            this.btn_AddConditionRow.UseVisualStyleBackColor = true;
            this.btn_AddConditionRow.Click += new System.EventHandler(this.btn_AddConditionRow_Click);
            // 
            // dg_ConditionSet
            // 
            this.dg_ConditionSet.AllowUserToAddRows = false;
            this.dg_ConditionSet.AllowUserToDeleteRows = false;
            this.dg_ConditionSet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_ConditionSet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chb_Activate,
            this.dataGridViewTextBoxColumn1,
            this.dg_cb_ImageSaveCondition_Condition,
            this.dg_cb_ImageSaveCondition_Logic,
            this.dataGridViewTextBoxColumn3,
            this.chb_RedImage,
            this.chb_GreenImage,
            this.chb_BlueImage,
            this.chb_BackImage,
            this.chb_ColorImage,
            this.dg_ImagePath});
            this.dg_ConditionSet.Location = new System.Drawing.Point(9, 38);
            this.dg_ConditionSet.Name = "dg_ConditionSet";
            this.dg_ConditionSet.Size = new System.Drawing.Size(660, 198);
            this.dg_ConditionSet.TabIndex = 366;
            this.dg_ConditionSet.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_ConditionSet_CellClick);
            this.dg_ConditionSet.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellDoubleClick);
            // 
            // chb_Activate
            // 
            this.chb_Activate.HeaderText = "Activate";
            this.chb_Activate.Name = "chb_Activate";
            this.chb_Activate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chb_Activate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chb_Activate.Width = 70;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dg_cb_ImageSaveCondition_Condition
            // 
            this.dg_cb_ImageSaveCondition_Condition.HeaderText = "Defect";
            this.dg_cb_ImageSaveCondition_Condition.Name = "dg_cb_ImageSaveCondition_Condition";
            this.dg_cb_ImageSaveCondition_Condition.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_cb_ImageSaveCondition_Condition.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dg_cb_ImageSaveCondition_Condition.Width = 180;
            // 
            // dg_cb_ImageSaveCondition_Logic
            // 
            this.dg_cb_ImageSaveCondition_Logic.HeaderText = "Condition";
            this.dg_cb_ImageSaveCondition_Logic.Name = "dg_cb_ImageSaveCondition_Logic";
            this.dg_cb_ImageSaveCondition_Logic.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_cb_ImageSaveCondition_Logic.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dg_cb_ImageSaveCondition_Logic.Width = 70;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Reference";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // chb_RedImage
            // 
            this.chb_RedImage.HeaderText = "Red";
            this.chb_RedImage.Name = "chb_RedImage";
            this.chb_RedImage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chb_RedImage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chb_RedImage.Width = 40;
            // 
            // chb_GreenImage
            // 
            this.chb_GreenImage.HeaderText = "Green";
            this.chb_GreenImage.Name = "chb_GreenImage";
            this.chb_GreenImage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chb_GreenImage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chb_GreenImage.Width = 40;
            // 
            // chb_BlueImage
            // 
            this.chb_BlueImage.HeaderText = "Blue";
            this.chb_BlueImage.Name = "chb_BlueImage";
            this.chb_BlueImage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chb_BlueImage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chb_BlueImage.Width = 40;
            // 
            // chb_BackImage
            // 
            this.chb_BackImage.HeaderText = "Back";
            this.chb_BackImage.Name = "chb_BackImage";
            this.chb_BackImage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chb_BackImage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chb_BackImage.Width = 40;
            // 
            // chb_ColorImage
            // 
            this.chb_ColorImage.FalseValue = "";
            this.chb_ColorImage.HeaderText = "Color";
            this.chb_ColorImage.Name = "chb_ColorImage";
            this.chb_ColorImage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chb_ColorImage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chb_ColorImage.Width = 40;
            // 
            // dg_ImagePath
            // 
            this.dg_ImagePath.HeaderText = "Image Path";
            this.dg_ImagePath.Name = "dg_ImagePath";
            this.dg_ImagePath.Width = 300;
            // 
            // pnl_ConditionSave
            // 
            this.pnl_ConditionSave.Controls.Add(this.dg_ConditionSet);
            this.pnl_ConditionSave.Controls.Add(this.label20);
            this.pnl_ConditionSave.Controls.Add(this.btn_DeleteConditionRow);
            this.pnl_ConditionSave.Controls.Add(this.btn_AddConditionRow);
            this.pnl_ConditionSave.Location = new System.Drawing.Point(408, 88);
            this.pnl_ConditionSave.Name = "pnl_ConditionSave";
            this.pnl_ConditionSave.Size = new System.Drawing.Size(677, 252);
            this.pnl_ConditionSave.TabIndex = 365;
            // 
            // pnl_AutoDelete2
            // 
            this.pnl_AutoDelete2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_AutoDelete2.Controls.Add(this.label22);
            this.pnl_AutoDelete2.Controls.Add(this.label19);
            this.pnl_AutoDelete2.Controls.Add(this.rb_DriveLimit);
            this.pnl_AutoDelete2.Controls.Add(this.tb_DriveLimit);
            this.pnl_AutoDelete2.Controls.Add(this.tb_DateLimit);
            this.pnl_AutoDelete2.Controls.Add(this.rb_DateLimit);
            this.pnl_AutoDelete2.Location = new System.Drawing.Point(408, 376);
            this.pnl_AutoDelete2.Name = "pnl_AutoDelete2";
            this.pnl_AutoDelete2.Size = new System.Drawing.Size(240, 73);
            this.pnl_AutoDelete2.TabIndex = 355;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Calibri", 12F);
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(199, 44);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(20, 19);
            this.label22.TabIndex = 0;
            this.label22.Text = "%";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Calibri", 12F);
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(198, 12);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(41, 19);
            this.label19.TabIndex = 0;
            this.label19.Text = "Days";
            // 
            // rb_DriveLimit
            // 
            this.rb_DriveLimit.AutoSize = true;
            this.rb_DriveLimit.Font = new System.Drawing.Font("Calibri", 12F);
            this.rb_DriveLimit.ForeColor = System.Drawing.Color.White;
            this.rb_DriveLimit.Location = new System.Drawing.Point(7, 41);
            this.rb_DriveLimit.Name = "rb_DriveLimit";
            this.rb_DriveLimit.Size = new System.Drawing.Size(97, 23);
            this.rb_DriveLimit.TabIndex = 345;
            this.rb_DriveLimit.TabStop = true;
            this.rb_DriveLimit.Text = "Drive Limit";
            this.rb_DriveLimit.UseVisualStyleBackColor = true;
            // 
            // tb_DriveLimit
            // 
            this.tb_DriveLimit.BackColor = System.Drawing.Color.Black;
            this.tb_DriveLimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_DriveLimit.Font = new System.Drawing.Font("Calibri", 12F);
            this.tb_DriveLimit.ForeColor = System.Drawing.Color.White;
            this.tb_DriveLimit.Location = new System.Drawing.Point(103, 41);
            this.tb_DriveLimit.Name = "tb_DriveLimit";
            this.tb_DriveLimit.Size = new System.Drawing.Size(95, 27);
            this.tb_DriveLimit.TabIndex = 263;
            this.tb_DriveLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_DriveLimit.TextChanged += new System.EventHandler(this.textChanged);
            this.tb_DriveLimit.DoubleClick += new System.EventHandler(this.Click_Event);
            this.tb_DriveLimit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.text_MouseDown);
            // 
            // tb_DateLimit
            // 
            this.tb_DateLimit.BackColor = System.Drawing.Color.Black;
            this.tb_DateLimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_DateLimit.Font = new System.Drawing.Font("Calibri", 12F);
            this.tb_DateLimit.ForeColor = System.Drawing.Color.White;
            this.tb_DateLimit.Location = new System.Drawing.Point(103, 9);
            this.tb_DateLimit.Name = "tb_DateLimit";
            this.tb_DateLimit.Size = new System.Drawing.Size(95, 27);
            this.tb_DateLimit.TabIndex = 263;
            this.tb_DateLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_DateLimit.TextChanged += new System.EventHandler(this.textChanged);
            this.tb_DateLimit.DoubleClick += new System.EventHandler(this.Click_Event);
            this.tb_DateLimit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.text_MouseDown);
            // 
            // rb_DateLimit
            // 
            this.rb_DateLimit.AutoSize = true;
            this.rb_DateLimit.Font = new System.Drawing.Font("Calibri", 12F);
            this.rb_DateLimit.ForeColor = System.Drawing.Color.White;
            this.rb_DateLimit.Location = new System.Drawing.Point(7, 11);
            this.rb_DateLimit.Name = "rb_DateLimit";
            this.rb_DateLimit.Size = new System.Drawing.Size(94, 23);
            this.rb_DateLimit.TabIndex = 345;
            this.rb_DateLimit.TabStop = true;
            this.rb_DateLimit.Text = "Date Limit";
            this.rb_DateLimit.UseVisualStyleBackColor = true;
            // 
            // pnl_AutoDelete
            // 
            this.pnl_AutoDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.pnl_AutoDelete.Controls.Add(this.label21);
            this.pnl_AutoDelete.Location = new System.Drawing.Point(408, 346);
            this.pnl_AutoDelete.Name = "pnl_AutoDelete";
            this.pnl_AutoDelete.Size = new System.Drawing.Size(239, 32);
            this.pnl_AutoDelete.TabIndex = 120;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Calibri", 15F);
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(55, 4);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(136, 24);
            this.label21.TabIndex = 0;
            this.label21.Text = "Set AutoDelete";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Calibri", 15F);
            this.label23.ForeColor = System.Drawing.Color.White;
            this.label23.Location = new System.Drawing.Point(114, 1040);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(487, 24);
            this.label23.TabIndex = 366;
            this.label23.Text = "미국 현장에 PM Align이 없기 때문에 PM Align Mode 잠시 막아둠";
            // 
            // AlignTarget1ImgList
            // 
            this.AlignTarget1ImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("AlignTarget1ImgList.ImageStream")));
            this.AlignTarget1ImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.AlignTarget1ImgList.Images.SetKeyName(0, "Wafer Edge.png");
            this.AlignTarget1ImgList.Images.SetKeyName(1, "Print Line.png");
            this.AlignTarget1ImgList.Images.SetKeyName(2, "Bifacial Big Circle.png");
            this.AlignTarget1ImgList.Images.SetKeyName(3, "Bifacial Small Circle.png");
            this.AlignTarget1ImgList.Images.SetKeyName(4, "Laser Fiducial.png");
            this.AlignTarget1ImgList.Images.SetKeyName(5, "Laser Fiducial.png");
            // 
            // AlignTarget2ImgList
            // 
            this.AlignTarget2ImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("AlignTarget2ImgList.ImageStream")));
            this.AlignTarget2ImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.AlignTarget2ImgList.Images.SetKeyName(0, "SearchArea_TwoArea.png");
            this.AlignTarget2ImgList.Images.SetKeyName(1, "Print Line.png");
            this.AlignTarget2ImgList.Images.SetKeyName(2, "Bifacial Big Circle.png");
            this.AlignTarget2ImgList.Images.SetKeyName(3, "Bifacial Small Circle.png");
            this.AlignTarget2ImgList.Images.SetKeyName(4, "Laser Fiducial.png");
            this.AlignTarget2ImgList.Images.SetKeyName(5, "Laser Fiducial.png");
            // 
            // FormParameterModelScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1615, 1100);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.pnl_ConditionSave);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.pnl_AutoDelete2);
            this.Controls.Add(this.pnl_AutoDelete);
            this.Controls.Add(this.btn_CamSetApplySave);
            this.Controls.Add(this.pnl_RecipeList);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel15);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormParameterModelScreen";
            this.Text = "1";
            this.Enter += new System.EventHandler(this.FormParameterModelScreen_Enter);
            this.panel15.ResumeLayout(false);
            this.panel15.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.pnl_RecipeList.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_ConditionSet)).EndInit();
            this.pnl_ConditionSave.ResumeLayout(false);
            this.pnl_ConditionSave.PerformLayout();
            this.pnl_AutoDelete2.ResumeLayout(false);
            this.pnl_AutoDelete2.PerformLayout();
            this.pnl_AutoDelete.ResumeLayout(false);
            this.pnl_AutoDelete.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbl_ModelParameter;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lbl_ModellList;
        private System.Windows.Forms.ListBox lst_ModelList;
        private System.Windows.Forms.Button btn_NewModel;
        private System.Windows.Forms.Button btn_LoadModel;
        private System.Windows.Forms.Button btn_CopyModel;
        private System.Windows.Forms.Button btn_RenameModel;
        private System.Windows.Forms.Button btn_DeleteModel;
        private System.Windows.Forms.Button btn_RefreshModel;
        private System.Windows.Forms.Panel pnl_RecipeList;
        private System.Windows.Forms.Button btn_CamSetApplySave;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btn_DeleteConditionRow;
        private System.Windows.Forms.Button btn_AddConditionRow;
        private System.Windows.Forms.DataGridView dg_ConditionSet;
        private System.Windows.Forms.Panel pnl_ConditionSave;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chb_Activate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn dg_cb_ImageSaveCondition_Condition;
        private System.Windows.Forms.DataGridViewComboBoxColumn dg_cb_ImageSaveCondition_Logic;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chb_RedImage;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chb_GreenImage;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chb_BlueImage;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chb_BackImage;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chb_ColorImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_ImagePath;
        private System.Windows.Forms.Panel pnl_AutoDelete2;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.RadioButton rb_DriveLimit;
        public System.Windows.Forms.TextBox tb_DriveLimit;
        public System.Windows.Forms.TextBox tb_DateLimit;
        private System.Windows.Forms.RadioButton rb_DateLimit;
        private System.Windows.Forms.Panel pnl_AutoDelete;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ImageList AlignTarget1ImgList;
        private System.Windows.Forms.ImageList AlignTarget2ImgList;
    }
}