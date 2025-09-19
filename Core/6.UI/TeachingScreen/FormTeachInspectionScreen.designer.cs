namespace Core.UI
{
    partial class FormTeachInspection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTeachInspection));
            this.pnl_CameraSetting = new System.Windows.Forms.Panel();
            this.lbl_InspectionTeaching = new System.Windows.Forms.Label();
            this.panel25 = new System.Windows.Forms.Panel();
            this.btnDisplayDeepLearningForm = new System.Windows.Forms.Button();
            this.btnDisplayBlobForm = new System.Windows.Forms.Button();
            this.btnDisplayImageFilterForm = new System.Windows.Forms.Button();
            this.btnDisplayROIToolForm = new System.Windows.Forms.Button();
            this.pnl_TeachFormScreen = new System.Windows.Forms.Panel();
            this.pnl_CameraSetting.SuspendLayout();
            this.panel25.SuspendLayout();
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
            this.lbl_InspectionTeaching.Size = new System.Drawing.Size(230, 29);
            this.lbl_InspectionTeaching.TabIndex = 0;
            this.lbl_InspectionTeaching.Text = "Inspection Teaching";
            // 
            // panel25
            // 
            this.panel25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panel25.Controls.Add(this.btnDisplayDeepLearningForm);
            this.panel25.Controls.Add(this.btnDisplayBlobForm);
            this.panel25.Controls.Add(this.btnDisplayImageFilterForm);
            this.panel25.Controls.Add(this.btnDisplayROIToolForm);
            this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel25.Location = new System.Drawing.Point(0, 32);
            this.panel25.Name = "panel25";
            this.panel25.Size = new System.Drawing.Size(1720, 35);
            this.panel25.TabIndex = 344;
            // 
            // btnDisplayDeepLearningForm
            // 
            this.btnDisplayDeepLearningForm.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDisplayDeepLearningForm.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btnDisplayDeepLearningForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplayDeepLearningForm.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnDisplayDeepLearningForm.ForeColor = System.Drawing.Color.White;
            this.btnDisplayDeepLearningForm.Location = new System.Drawing.Point(450, 0);
            this.btnDisplayDeepLearningForm.Name = "btnDisplayDeepLearningForm";
            this.btnDisplayDeepLearningForm.Size = new System.Drawing.Size(150, 35);
            this.btnDisplayDeepLearningForm.TabIndex = 8;
            this.btnDisplayDeepLearningForm.Text = "DeepLearning";
            this.btnDisplayDeepLearningForm.UseVisualStyleBackColor = true;
            this.btnDisplayDeepLearningForm.Click += new System.EventHandler(this.btn_DisplayEachTeachingForm);
            // 
            // btnDisplayBlobForm
            // 
            this.btnDisplayBlobForm.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDisplayBlobForm.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btnDisplayBlobForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplayBlobForm.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnDisplayBlobForm.ForeColor = System.Drawing.Color.White;
            this.btnDisplayBlobForm.Location = new System.Drawing.Point(300, 0);
            this.btnDisplayBlobForm.Name = "btnDisplayBlobForm";
            this.btnDisplayBlobForm.Size = new System.Drawing.Size(150, 35);
            this.btnDisplayBlobForm.TabIndex = 7;
            this.btnDisplayBlobForm.Text = "Inspect Tool";
            this.btnDisplayBlobForm.UseVisualStyleBackColor = true;
            this.btnDisplayBlobForm.Click += new System.EventHandler(this.btn_DisplayEachTeachingForm);
            // 
            // btnDisplayImageFilterForm
            // 
            this.btnDisplayImageFilterForm.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDisplayImageFilterForm.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btnDisplayImageFilterForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplayImageFilterForm.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnDisplayImageFilterForm.ForeColor = System.Drawing.Color.White;
            this.btnDisplayImageFilterForm.Location = new System.Drawing.Point(150, 0);
            this.btnDisplayImageFilterForm.Name = "btnDisplayImageFilterForm";
            this.btnDisplayImageFilterForm.Size = new System.Drawing.Size(150, 35);
            this.btnDisplayImageFilterForm.TabIndex = 3;
            this.btnDisplayImageFilterForm.Text = "Image Filter Tool";
            this.btnDisplayImageFilterForm.UseVisualStyleBackColor = true;
            this.btnDisplayImageFilterForm.Click += new System.EventHandler(this.btn_DisplayEachTeachingForm);
            // 
            // btnDisplayROIToolForm
            // 
            this.btnDisplayROIToolForm.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDisplayROIToolForm.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btnDisplayROIToolForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplayROIToolForm.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnDisplayROIToolForm.ForeColor = System.Drawing.Color.White;
            this.btnDisplayROIToolForm.Location = new System.Drawing.Point(0, 0);
            this.btnDisplayROIToolForm.Name = "btnDisplayROIToolForm";
            this.btnDisplayROIToolForm.Size = new System.Drawing.Size(150, 35);
            this.btnDisplayROIToolForm.TabIndex = 0;
            this.btnDisplayROIToolForm.Text = "ROI Tool";
            this.btnDisplayROIToolForm.UseVisualStyleBackColor = true;
            this.btnDisplayROIToolForm.Click += new System.EventHandler(this.btn_DisplayEachTeachingForm);
            // 
            // pnl_TeachFormScreen
            // 
            this.pnl_TeachFormScreen.Location = new System.Drawing.Point(0, 67);
            this.pnl_TeachFormScreen.Name = "pnl_TeachFormScreen";
            this.pnl_TeachFormScreen.Size = new System.Drawing.Size(1720, 883);
            this.pnl_TeachFormScreen.TabIndex = 345;
            // 
            // FormTeachInspection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1720, 950);
            this.Controls.Add(this.pnl_TeachFormScreen);
            this.Controls.Add(this.panel25);
            this.Controls.Add(this.pnl_CameraSetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormTeachInspection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormTeachInspection";
            this.Load += new System.EventHandler(this.FormTeachInspection_Load);
            this.pnl_CameraSetting.ResumeLayout(false);
            this.pnl_CameraSetting.PerformLayout();
            this.panel25.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_CameraSetting;
        private System.Windows.Forms.Label lbl_InspectionTeaching;
        private System.Windows.Forms.Panel panel25;
        private System.Windows.Forms.Button btnDisplayROIToolForm;
        private System.Windows.Forms.Button btnDisplayImageFilterForm;
        private System.Windows.Forms.Panel pnl_TeachFormScreen;
        private System.Windows.Forms.Button btnDisplayBlobForm;
        private System.Windows.Forms.Button btnDisplayDeepLearningForm;
    }
}