namespace Core.UI
{
    partial class CreateModel
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbl_CreateModel = new System.Windows.Forms.Label();
            this.RD_NewRecipe = new System.Windows.Forms.RadioButton();
            this.RD_SaveAs = new System.Windows.Forms.RadioButton();
            this.RD_RenameAs = new System.Windows.Forms.RadioButton();
            this.LBL_ModelList = new System.Windows.Forms.Label();
            this.LBL_ModelNum = new System.Windows.Forms.Label();
            this.LBL_ModelID = new System.Windows.Forms.Label();
            this.CBO_ModelList = new System.Windows.Forms.ComboBox();
            this.TXT_ModelNum = new System.Windows.Forms.TextBox();
            this.TXT_ModelID = new System.Windows.Forms.TextBox();
            this.BTN_ModelSave = new System.Windows.Forms.Button();
            this.BTN_ModelCancel = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel3.Controls.Add(this.lbl_CreateModel);
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(499, 32);
            this.panel3.TabIndex = 44;
            // 
            // lbl_CreateModel
            // 
            this.lbl_CreateModel.AutoSize = true;
            this.lbl_CreateModel.Font = new System.Drawing.Font("Calibri", 15F);
            this.lbl_CreateModel.ForeColor = System.Drawing.Color.White;
            this.lbl_CreateModel.Location = new System.Drawing.Point(5, 3);
            this.lbl_CreateModel.Name = "lbl_CreateModel";
            this.lbl_CreateModel.Size = new System.Drawing.Size(124, 24);
            this.lbl_CreateModel.TabIndex = 0;
            this.lbl_CreateModel.Text = "Create Model";
            // 
            // RD_NewRecipe
            // 
            this.RD_NewRecipe.AutoSize = true;
            this.RD_NewRecipe.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RD_NewRecipe.ForeColor = System.Drawing.SystemColors.Control;
            this.RD_NewRecipe.Location = new System.Drawing.Point(51, 51);
            this.RD_NewRecipe.Name = "RD_NewRecipe";
            this.RD_NewRecipe.Size = new System.Drawing.Size(104, 23);
            this.RD_NewRecipe.TabIndex = 45;
            this.RD_NewRecipe.Text = "New Recipe";
            this.RD_NewRecipe.UseVisualStyleBackColor = true;
            this.RD_NewRecipe.CheckedChanged += new System.EventHandler(this.RD_NewRecipe_CheckedChanged);
            // 
            // RD_SaveAs
            // 
            this.RD_SaveAs.AutoSize = true;
            this.RD_SaveAs.Font = new System.Drawing.Font("Calibri", 12F);
            this.RD_SaveAs.ForeColor = System.Drawing.SystemColors.Control;
            this.RD_SaveAs.Location = new System.Drawing.Point(212, 51);
            this.RD_SaveAs.Name = "RD_SaveAs";
            this.RD_SaveAs.Size = new System.Drawing.Size(76, 23);
            this.RD_SaveAs.TabIndex = 46;
            this.RD_SaveAs.Text = "Save as";
            this.RD_SaveAs.UseVisualStyleBackColor = true;
            this.RD_SaveAs.CheckedChanged += new System.EventHandler(this.RD_SaveAs_CheckedChanged);
            // 
            // RD_RenameAs
            // 
            this.RD_RenameAs.AutoSize = true;
            this.RD_RenameAs.Font = new System.Drawing.Font("Calibri", 12F);
            this.RD_RenameAs.ForeColor = System.Drawing.SystemColors.Control;
            this.RD_RenameAs.Location = new System.Drawing.Point(347, 51);
            this.RD_RenameAs.Name = "RD_RenameAs";
            this.RD_RenameAs.Size = new System.Drawing.Size(99, 23);
            this.RD_RenameAs.TabIndex = 47;
            this.RD_RenameAs.Text = "Rename as";
            this.RD_RenameAs.UseVisualStyleBackColor = true;
            this.RD_RenameAs.CheckedChanged += new System.EventHandler(this.RD_RenameAs_CheckedChanged);
            // 
            // LBL_ModelList
            // 
            this.LBL_ModelList.AutoSize = true;
            this.LBL_ModelList.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_ModelList.ForeColor = System.Drawing.SystemColors.Control;
            this.LBL_ModelList.Location = new System.Drawing.Point(51, 93);
            this.LBL_ModelList.Name = "LBL_ModelList";
            this.LBL_ModelList.Size = new System.Drawing.Size(85, 19);
            this.LBL_ModelList.TabIndex = 48;
            this.LBL_ModelList.Text = "Model List :";
            // 
            // LBL_ModelNum
            // 
            this.LBL_ModelNum.AutoSize = true;
            this.LBL_ModelNum.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_ModelNum.ForeColor = System.Drawing.SystemColors.Control;
            this.LBL_ModelNum.Location = new System.Drawing.Point(44, 126);
            this.LBL_ModelNum.Name = "LBL_ModelNum";
            this.LBL_ModelNum.Size = new System.Drawing.Size(92, 19);
            this.LBL_ModelNum.TabIndex = 49;
            this.LBL_ModelNum.Text = "Model Num :";
            // 
            // LBL_ModelID
            // 
            this.LBL_ModelID.AutoSize = true;
            this.LBL_ModelID.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_ModelID.ForeColor = System.Drawing.SystemColors.Control;
            this.LBL_ModelID.Location = new System.Drawing.Point(60, 155);
            this.LBL_ModelID.Name = "LBL_ModelID";
            this.LBL_ModelID.Size = new System.Drawing.Size(76, 19);
            this.LBL_ModelID.TabIndex = 50;
            this.LBL_ModelID.Text = "Model ID :";
            // 
            // CBO_ModelList
            // 
            this.CBO_ModelList.FormattingEnabled = true;
            this.CBO_ModelList.Location = new System.Drawing.Point(142, 91);
            this.CBO_ModelList.Name = "CBO_ModelList";
            this.CBO_ModelList.Size = new System.Drawing.Size(163, 21);
            this.CBO_ModelList.TabIndex = 51;
            this.CBO_ModelList.SelectedIndexChanged += new System.EventHandler(this.CBO_ModelList_SelectedIndexChanged);
            // 
            // TXT_ModelNum
            // 
            this.TXT_ModelNum.BackColor = System.Drawing.Color.Black;
            this.TXT_ModelNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TXT_ModelNum.Font = new System.Drawing.Font("Calibri", 12F);
            this.TXT_ModelNum.ForeColor = System.Drawing.Color.White;
            this.TXT_ModelNum.Location = new System.Drawing.Point(142, 118);
            this.TXT_ModelNum.Name = "TXT_ModelNum";
            this.TXT_ModelNum.Size = new System.Drawing.Size(163, 27);
            this.TXT_ModelNum.TabIndex = 83;
            this.TXT_ModelNum.Text = "80";
            this.TXT_ModelNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TXT_ModelNum.TextChanged += new System.EventHandler(this.textChanged);
            this.TXT_ModelNum.DoubleClick += new System.EventHandler(this.Click_Event);
            this.TXT_ModelNum.MouseDown += new System.Windows.Forms.MouseEventHandler(this.text_MouseDown);
            // 
            // TXT_ModelID
            // 
            this.TXT_ModelID.BackColor = System.Drawing.Color.Black;
            this.TXT_ModelID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TXT_ModelID.Font = new System.Drawing.Font("Calibri", 12F);
            this.TXT_ModelID.ForeColor = System.Drawing.Color.White;
            this.TXT_ModelID.Location = new System.Drawing.Point(142, 151);
            this.TXT_ModelID.Name = "TXT_ModelID";
            this.TXT_ModelID.Size = new System.Drawing.Size(163, 27);
            this.TXT_ModelID.TabIndex = 84;
            this.TXT_ModelID.Text = "80";
            this.TXT_ModelID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TXT_ModelID.TextChanged += new System.EventHandler(this.textChanged);
            this.TXT_ModelID.DoubleClick += new System.EventHandler(this.Click_Event_Keyboard);
            this.TXT_ModelID.MouseDown += new System.Windows.Forms.MouseEventHandler(this.text_MouseDown);
            // 
            // BTN_ModelSave
            // 
            this.BTN_ModelSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.BTN_ModelSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_ModelSave.Font = new System.Drawing.Font("Calibri", 12F);
            this.BTN_ModelSave.ForeColor = System.Drawing.Color.White;
            this.BTN_ModelSave.Location = new System.Drawing.Point(119, 196);
            this.BTN_ModelSave.Name = "BTN_ModelSave";
            this.BTN_ModelSave.Size = new System.Drawing.Size(117, 33);
            this.BTN_ModelSave.TabIndex = 85;
            this.BTN_ModelSave.Text = "Save";
            this.BTN_ModelSave.UseVisualStyleBackColor = true;
            this.BTN_ModelSave.Click += new System.EventHandler(this.BTN_ModelSave_Click);
            // 
            // BTN_ModelCancel
            // 
            this.BTN_ModelCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.BTN_ModelCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_ModelCancel.Font = new System.Drawing.Font("Calibri", 12F);
            this.BTN_ModelCancel.ForeColor = System.Drawing.Color.White;
            this.BTN_ModelCancel.Location = new System.Drawing.Point(281, 196);
            this.BTN_ModelCancel.Name = "BTN_ModelCancel";
            this.BTN_ModelCancel.Size = new System.Drawing.Size(117, 33);
            this.BTN_ModelCancel.TabIndex = 86;
            this.BTN_ModelCancel.Text = "Cancel";
            this.BTN_ModelCancel.UseVisualStyleBackColor = true;
            this.BTN_ModelCancel.Click += new System.EventHandler(this.BTN_ModelCancel_Click);
            // 
            // CreateModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.ClientSize = new System.Drawing.Size(500, 250);
            this.Controls.Add(this.BTN_ModelCancel);
            this.Controls.Add(this.BTN_ModelSave);
            this.Controls.Add(this.TXT_ModelID);
            this.Controls.Add(this.TXT_ModelNum);
            this.Controls.Add(this.CBO_ModelList);
            this.Controls.Add(this.LBL_ModelID);
            this.Controls.Add(this.LBL_ModelNum);
            this.Controls.Add(this.LBL_ModelList);
            this.Controls.Add(this.RD_RenameAs);
            this.Controls.Add(this.RD_SaveAs);
            this.Controls.Add(this.RD_NewRecipe);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CreateModel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CreateModel";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lbl_CreateModel;
        private System.Windows.Forms.RadioButton RD_NewRecipe;
        private System.Windows.Forms.RadioButton RD_SaveAs;
        private System.Windows.Forms.RadioButton RD_RenameAs;
        private System.Windows.Forms.Label LBL_ModelList;
        private System.Windows.Forms.Label LBL_ModelNum;
        private System.Windows.Forms.Label LBL_ModelID;
        private System.Windows.Forms.ComboBox CBO_ModelList;
        public System.Windows.Forms.TextBox TXT_ModelNum;
        public System.Windows.Forms.TextBox TXT_ModelID;
        private System.Windows.Forms.Button BTN_ModelSave;
        private System.Windows.Forms.Button BTN_ModelCancel;
    }
}