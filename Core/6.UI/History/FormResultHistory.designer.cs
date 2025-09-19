namespace Core.UI
{
    partial class FormResultHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormResultHistory));
            this.pnl_CameraSetting = new System.Windows.Forms.Panel();
            this.lbl_InspectionTeaching = new System.Windows.Forms.Label();
            this.panel25 = new System.Windows.Forms.Panel();
            this.btnDisplayInspectionForm = new System.Windows.Forms.Button();
            this.pnl_HistoryFormScreen = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lbl_Date = new System.Windows.Forms.Label();
            this.DefectDisplayCam = new System.Windows.Forms.Panel();
            this.pnl_CameraSetting.SuspendLayout();
            this.panel25.SuspendLayout();
            this.pnl_HistoryFormScreen.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_CameraSetting
            // 
            this.pnl_CameraSetting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.pnl_CameraSetting.Controls.Add(this.lbl_InspectionTeaching);
            this.pnl_CameraSetting.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_CameraSetting.Location = new System.Drawing.Point(0, 0);
            this.pnl_CameraSetting.Name = "pnl_CameraSetting";
            this.pnl_CameraSetting.Size = new System.Drawing.Size(1720, 32);
            this.pnl_CameraSetting.TabIndex = 65;
            // 
            // lbl_InspectionTeaching
            // 
            this.lbl_InspectionTeaching.AutoSize = true;
            this.lbl_InspectionTeaching.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_InspectionTeaching.ForeColor = System.Drawing.Color.White;
            this.lbl_InspectionTeaching.Location = new System.Drawing.Point(745, 1);
            this.lbl_InspectionTeaching.Name = "lbl_InspectionTeaching";
            this.lbl_InspectionTeaching.Size = new System.Drawing.Size(161, 29);
            this.lbl_InspectionTeaching.TabIndex = 0;
            this.lbl_InspectionTeaching.Text = "Result History";
            // 
            // panel25
            // 
            this.panel25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panel25.Controls.Add(this.btnDisplayInspectionForm);
            this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel25.Location = new System.Drawing.Point(0, 32);
            this.panel25.Name = "panel25";
            this.panel25.Size = new System.Drawing.Size(1720, 35);
            this.panel25.TabIndex = 344;
            // 
            // btnDisplayInspectionForm
            // 
            this.btnDisplayInspectionForm.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDisplayInspectionForm.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btnDisplayInspectionForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplayInspectionForm.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnDisplayInspectionForm.ForeColor = System.Drawing.Color.White;
            this.btnDisplayInspectionForm.Location = new System.Drawing.Point(0, 0);
            this.btnDisplayInspectionForm.Name = "btnDisplayInspectionForm";
            this.btnDisplayInspectionForm.Size = new System.Drawing.Size(150, 35);
            this.btnDisplayInspectionForm.TabIndex = 0;
            this.btnDisplayInspectionForm.Text = "Inspection";
            this.btnDisplayInspectionForm.UseVisualStyleBackColor = true;
            this.btnDisplayInspectionForm.Click += new System.EventHandler(this.btn_DisplayEachHistoryForm);
            // 
            // pnl_HistoryFormScreen
            // 
            this.pnl_HistoryFormScreen.Controls.Add(this.panel4);
            this.pnl_HistoryFormScreen.Controls.Add(this.DefectDisplayCam);
            this.pnl_HistoryFormScreen.Location = new System.Drawing.Point(0, 67);
            this.pnl_HistoryFormScreen.Name = "pnl_HistoryFormScreen";
            this.pnl_HistoryFormScreen.Size = new System.Drawing.Size(1720, 883);
            this.pnl_HistoryFormScreen.TabIndex = 345;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel4.Controls.Add(this.lbl_Date);
            this.panel4.Location = new System.Drawing.Point(1007, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(710, 32);
            this.panel4.TabIndex = 622;
            // 
            // lbl_Date
            // 
            this.lbl_Date.AutoSize = true;
            this.lbl_Date.Font = new System.Drawing.Font("Calibri", 18F);
            this.lbl_Date.ForeColor = System.Drawing.Color.White;
            this.lbl_Date.Location = new System.Drawing.Point(310, 0);
            this.lbl_Date.Name = "lbl_Date";
            this.lbl_Date.Size = new System.Drawing.Size(139, 29);
            this.lbl_Date.TabIndex = 0;
            this.lbl_Date.Text = "Result Image";
            // 
            // DefectDisplayCam
            // 
            this.DefectDisplayCam.Location = new System.Drawing.Point(1007, 42);
            this.DefectDisplayCam.Name = "DefectDisplayCam";
            this.DefectDisplayCam.Size = new System.Drawing.Size(710, 710);
            this.DefectDisplayCam.TabIndex = 621;
            // 
            // FormResultHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1720, 950);
            this.Controls.Add(this.pnl_HistoryFormScreen);
            this.Controls.Add(this.panel25);
            this.Controls.Add(this.pnl_CameraSetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormResultHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormTeachInspection";
            this.Load += new System.EventHandler(this.FormTeachInspection_Load);
            this.pnl_CameraSetting.ResumeLayout(false);
            this.pnl_CameraSetting.PerformLayout();
            this.panel25.ResumeLayout(false);
            this.pnl_HistoryFormScreen.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_CameraSetting;
        private System.Windows.Forms.Label lbl_InspectionTeaching;
        private System.Windows.Forms.Panel panel25;
        private System.Windows.Forms.Button btnDisplayInspectionForm;
        private System.Windows.Forms.Panel pnl_HistoryFormScreen;
        private System.Windows.Forms.Panel DefectDisplayCam;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lbl_Date;
    }
}