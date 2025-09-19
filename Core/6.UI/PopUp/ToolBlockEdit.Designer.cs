namespace Core.UI
{
    partial class ToolBlockEdit
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
            this.Panel_Title = new System.Windows.Forms.Panel();
            this.lbl_SetToolPageName = new System.Windows.Forms.Label();
            this.btn_ToolBlock_Save = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pnl_ToolBlockEditor = new System.Windows.Forms.Panel();
            this.cogToolBlockEdit = new Cognex.VisionPro.ToolBlock.CogToolBlockEditV2();
            this.Panel_Title.SuspendLayout();
            this.pnl_ToolBlockEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogToolBlockEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel_Title
            // 
            this.Panel_Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Panel_Title.Controls.Add(this.lbl_SetToolPageName);
            this.Panel_Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel_Title.Location = new System.Drawing.Point(0, 0);
            this.Panel_Title.Name = "Panel_Title";
            this.Panel_Title.Size = new System.Drawing.Size(1720, 40);
            this.Panel_Title.TabIndex = 0;
            // 
            // lbl_SetToolPageName
            // 
            this.lbl_SetToolPageName.AutoSize = true;
            this.lbl_SetToolPageName.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_SetToolPageName.ForeColor = System.Drawing.Color.White;
            this.lbl_SetToolPageName.Location = new System.Drawing.Point(16, 8);
            this.lbl_SetToolPageName.Name = "lbl_SetToolPageName";
            this.lbl_SetToolPageName.Size = new System.Drawing.Size(115, 23);
            this.lbl_SetToolPageName.TabIndex = 0;
            this.lbl_SetToolPageName.Text = "Tool Edit Page";
            // 
            // btn_ToolBlock_Save
            // 
            this.btn_ToolBlock_Save.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_ToolBlock_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ToolBlock_Save.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ToolBlock_Save.ForeColor = System.Drawing.Color.White;
            this.btn_ToolBlock_Save.Location = new System.Drawing.Point(1289, 782);
            this.btn_ToolBlock_Save.Name = "btn_ToolBlock_Save";
            this.btn_ToolBlock_Save.Size = new System.Drawing.Size(149, 70);
            this.btn_ToolBlock_Save.TabIndex = 321;
            this.btn_ToolBlock_Save.Text = "Save";
            this.btn_ToolBlock_Save.UseVisualStyleBackColor = true;
            this.btn_ToolBlock_Save.Click += new System.EventHandler(this.btn_ToolBlock_Save_Click);
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(1451, 782);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 70);
            this.button1.TabIndex = 322;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pnl_ToolBlockEditor
            // 
            this.pnl_ToolBlockEditor.Controls.Add(this.cogToolBlockEdit);
            this.pnl_ToolBlockEditor.Location = new System.Drawing.Point(3, 46);
            this.pnl_ToolBlockEditor.Name = "pnl_ToolBlockEditor";
            this.pnl_ToolBlockEditor.Size = new System.Drawing.Size(1600, 700);
            this.pnl_ToolBlockEditor.TabIndex = 323;
            // 
            // cogToolBlockEdit
            // 
            this.cogToolBlockEdit.AllowDrop = true;
            this.cogToolBlockEdit.ContextMenuCustomizer = null;
            this.cogToolBlockEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogToolBlockEdit.Location = new System.Drawing.Point(0, 0);
            this.cogToolBlockEdit.MinimumSize = new System.Drawing.Size(905, 0);
            this.cogToolBlockEdit.Name = "cogToolBlockEdit";
            this.cogToolBlockEdit.ShowNodeToolTips = true;
            this.cogToolBlockEdit.Size = new System.Drawing.Size(1600, 700);
            this.cogToolBlockEdit.SuspendElectricRuns = false;
            this.cogToolBlockEdit.TabIndex = 2;
            // 
            // ToolBlockEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.ClientSize = new System.Drawing.Size(1720, 883);
            this.Controls.Add(this.pnl_ToolBlockEditor);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_ToolBlock_Save);
            this.Controls.Add(this.Panel_Title);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ToolBlockEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ToolBlockEdit";
            this.Panel_Title.ResumeLayout(false);
            this.Panel_Title.PerformLayout();
            this.pnl_ToolBlockEditor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogToolBlockEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel_Title;
        private System.Windows.Forms.Label lbl_SetToolPageName;
        private System.Windows.Forms.Button btn_ToolBlock_Save;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel pnl_ToolBlockEditor;
        private Cognex.VisionPro.ToolBlock.CogToolBlockEditV2 cogToolBlockEdit;
    }
}