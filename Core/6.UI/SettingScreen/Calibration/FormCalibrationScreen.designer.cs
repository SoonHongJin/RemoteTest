namespace Core.UI
{
    partial class FormCalibrationScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCalibrationScreen));
            this.lbl_CalibrationSetting = new System.Windows.Forms.Label();
            this.pnl_CalibrationSetting = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_CalibStep_CheckBoard = new System.Windows.Forms.Button();
            this.pnl_CalibrationButton = new System.Windows.Forms.Panel();
            this.btnDisplayCheckerBoardForm = new System.Windows.Forms.Button();
            this.pnl_CalibFormScreen = new System.Windows.Forms.Panel();
            this.CalibStatusImgList = new System.Windows.Forms.ImageList(this.components);
            this.pnl_CalibrationSetting.SuspendLayout();
            this.pnl_CalibrationButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_CalibrationSetting
            // 
            this.lbl_CalibrationSetting.AutoSize = true;
            this.lbl_CalibrationSetting.Font = new System.Drawing.Font("Calibri", 18F);
            this.lbl_CalibrationSetting.ForeColor = System.Drawing.Color.White;
            this.lbl_CalibrationSetting.Location = new System.Drawing.Point(784, 1);
            this.lbl_CalibrationSetting.Name = "lbl_CalibrationSetting";
            this.lbl_CalibrationSetting.Size = new System.Drawing.Size(122, 29);
            this.lbl_CalibrationSetting.TabIndex = 0;
            this.lbl_CalibrationSetting.Text = "Calibration";
            // 
            // pnl_CalibrationSetting
            // 
            this.pnl_CalibrationSetting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.pnl_CalibrationSetting.Controls.Add(this.label5);
            this.pnl_CalibrationSetting.Controls.Add(this.btn_CalibStep_CheckBoard);
            this.pnl_CalibrationSetting.Controls.Add(this.lbl_CalibrationSetting);
            this.pnl_CalibrationSetting.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_CalibrationSetting.Location = new System.Drawing.Point(0, 0);
            this.pnl_CalibrationSetting.Name = "pnl_CalibrationSetting";
            this.pnl_CalibrationSetting.Size = new System.Drawing.Size(1720, 32);
            this.pnl_CalibrationSetting.TabIndex = 64;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(1535, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(173, 19);
            this.label5.TabIndex = 804;
            this.label5.Text = "CheckerBoard Calibration";
            // 
            // btn_CalibStep_CheckBoard
            // 
            this.btn_CalibStep_CheckBoard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_CalibStep_CheckBoard.FlatAppearance.BorderSize = 0;
            this.btn_CalibStep_CheckBoard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_CalibStep_CheckBoard.Location = new System.Drawing.Point(1499, 2);
            this.btn_CalibStep_CheckBoard.Name = "btn_CalibStep_CheckBoard";
            this.btn_CalibStep_CheckBoard.Size = new System.Drawing.Size(30, 30);
            this.btn_CalibStep_CheckBoard.TabIndex = 803;
            this.btn_CalibStep_CheckBoard.TabStop = false;
            this.btn_CalibStep_CheckBoard.UseVisualStyleBackColor = true;
            this.btn_CalibStep_CheckBoard.Click += new System.EventHandler(this.IsCalibStatusNG);
            // 
            // pnl_CalibrationButton
            // 
            this.pnl_CalibrationButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.pnl_CalibrationButton.Controls.Add(this.btnDisplayCheckerBoardForm);
            this.pnl_CalibrationButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_CalibrationButton.Location = new System.Drawing.Point(0, 32);
            this.pnl_CalibrationButton.Name = "pnl_CalibrationButton";
            this.pnl_CalibrationButton.Size = new System.Drawing.Size(1720, 35);
            this.pnl_CalibrationButton.TabIndex = 637;
            // 
            // btnDisplayCheckerBoardForm
            // 
            this.btnDisplayCheckerBoardForm.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDisplayCheckerBoardForm.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btnDisplayCheckerBoardForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplayCheckerBoardForm.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnDisplayCheckerBoardForm.ForeColor = System.Drawing.Color.White;
            this.btnDisplayCheckerBoardForm.Location = new System.Drawing.Point(0, 0);
            this.btnDisplayCheckerBoardForm.Name = "btnDisplayCheckerBoardForm";
            this.btnDisplayCheckerBoardForm.Size = new System.Drawing.Size(150, 35);
            this.btnDisplayCheckerBoardForm.TabIndex = 2;
            this.btnDisplayCheckerBoardForm.Text = "CheckerBoard";
            this.btnDisplayCheckerBoardForm.UseVisualStyleBackColor = true;
            this.btnDisplayCheckerBoardForm.Click += new System.EventHandler(this.btn_DisplayEachCalibForm);
            // 
            // pnl_CalibFormScreen
            // 
            this.pnl_CalibFormScreen.Location = new System.Drawing.Point(0, 67);
            this.pnl_CalibFormScreen.Name = "pnl_CalibFormScreen";
            this.pnl_CalibFormScreen.Size = new System.Drawing.Size(1720, 883);
            this.pnl_CalibFormScreen.TabIndex = 638;
            // 
            // CalibStatusImgList
            // 
            this.CalibStatusImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("CalibStatusImgList.ImageStream")));
            this.CalibStatusImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.CalibStatusImgList.Images.SetKeyName(0, "LED_GRAY.gif");
            this.CalibStatusImgList.Images.SetKeyName(1, "LED_GREEN.gif");
            this.CalibStatusImgList.Images.SetKeyName(2, "LED_RED.gif");
            // 
            // FormCalibrationScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.ClientSize = new System.Drawing.Size(1720, 950);
            this.Controls.Add(this.pnl_CalibFormScreen);
            this.Controls.Add(this.pnl_CalibrationButton);
            this.Controls.Add(this.pnl_CalibrationSetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormCalibrationScreen";
            this.Text = "FormCalibrationScreen";
            this.Load += new System.EventHandler(this.FormCalibrationScreen_Load);
            this.pnl_CalibrationSetting.ResumeLayout(false);
            this.pnl_CalibrationSetting.PerformLayout();
            this.pnl_CalibrationButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_CalibrationSetting;
        private System.Windows.Forms.Panel pnl_CalibrationSetting;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_CalibStep_CheckBoard;
        private System.Windows.Forms.Panel pnl_CalibrationButton;
        private System.Windows.Forms.Button btnDisplayCheckerBoardForm;
        public System.Windows.Forms.Panel pnl_CalibFormScreen;
        private System.Windows.Forms.ImageList CalibStatusImgList;
    }
}