namespace Core.UI
{
    partial class FormMessageBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMessageBox));
            this.BtnCancel = new Syncfusion.Windows.Forms.ButtonAdv();
            this.gradientLabel1 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.BtnConfirm = new Syncfusion.Windows.Forms.ButtonAdv();
            this.TextEng = new Syncfusion.Windows.Forms.Tools.TextBoxExt();
            this.TextSystem = new Syncfusion.Windows.Forms.Tools.TextBoxExt();
            this.BtnSave = new Syncfusion.Windows.Forms.ButtonAdv();
            this.gradientLabel2 = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.Label_System = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.TextEng)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextSystem)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnCancel.ForeColor = System.Drawing.Color.Navy;
            this.BtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("BtnCancel.Image")));
            this.BtnCancel.Location = new System.Drawing.Point(538, 154);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(116, 62);
            this.BtnCancel.TabIndex = 1;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.Horizontal, System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255))))));
            this.gradientLabel1.BorderAppearance = System.Windows.Forms.BorderStyle.None;
            this.gradientLabel1.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel1.Location = new System.Drawing.Point(3, 2);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.Size = new System.Drawing.Size(660, 228);
            this.gradientLabel1.TabIndex = 9;
            this.gradientLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnConfirm
            // 
            this.BtnConfirm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnConfirm.CustomManagedColor = System.Drawing.Color.White;
            this.BtnConfirm.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnConfirm.ForeColor = System.Drawing.Color.Navy;
            this.BtnConfirm.Image = ((System.Drawing.Image)(resources.GetObject("BtnConfirm.Image")));
            this.BtnConfirm.Location = new System.Drawing.Point(416, 154);
            this.BtnConfirm.Name = "BtnConfirm";
            this.BtnConfirm.Size = new System.Drawing.Size(116, 62);
            this.BtnConfirm.TabIndex = 0;
            this.BtnConfirm.Text = "Confirm";
            this.BtnConfirm.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnConfirm.UseVisualStyleBackColor = true;
            this.BtnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // TextEng
            // 
            this.TextEng.Border3DStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.TextEng.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextEng.Location = new System.Drawing.Point(98, 12);
            this.TextEng.Multiline = true;
            this.TextEng.Name = "TextEng";
            this.TextEng.Size = new System.Drawing.Size(556, 57);
            this.TextEng.TabIndex = 30;
            this.TextEng.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextSystem
            // 
            this.TextSystem.Border3DStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.TextSystem.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextSystem.Location = new System.Drawing.Point(98, 74);
            this.TextSystem.Multiline = true;
            this.TextSystem.Name = "TextSystem";
            this.TextSystem.Size = new System.Drawing.Size(556, 57);
            this.TextSystem.TabIndex = 31;
            this.TextSystem.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.Color.LightGray;
            this.BtnSave.BorderStyleAdv = Syncfusion.Windows.Forms.ButtonAdvBorderStyle.Solid;
            this.BtnSave.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSave.Location = new System.Drawing.Point(26, 154);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(116, 62);
            this.BtnSave.TabIndex = 32;
            this.BtnSave.Text = "Save";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // gradientLabel2
            // 
            this.gradientLabel2.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192))))));
            this.gradientLabel2.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.gradientLabel2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gradientLabel2.Location = new System.Drawing.Point(12, 12);
            this.gradientLabel2.Name = "gradientLabel2";
            this.gradientLabel2.Size = new System.Drawing.Size(80, 55);
            this.gradientLabel2.TabIndex = 34;
            this.gradientLabel2.Text = "[English]";
            this.gradientLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label_System
            // 
            this.Label_System.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192))))));
            this.Label_System.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.Label_System.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Label_System.Location = new System.Drawing.Point(12, 74);
            this.Label_System.Name = "Label_System";
            this.Label_System.Size = new System.Drawing.Size(80, 56);
            this.Label_System.TabIndex = 35;
            this.Label_System.Text = "[System Language]";
            this.Label_System.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(659, 222);
            this.Controls.Add(this.Label_System);
            this.Controls.Add(this.gradientLabel2);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.TextSystem);
            this.Controls.Add(this.TextEng);
            this.Controls.Add(this.BtnConfirm);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.gradientLabel1);
            this.Name = "FormMessageBox";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Messgae Box";
            this.Load += new System.EventHandler(this.FormUtilMsg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TextEng)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextSystem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Syncfusion.Windows.Forms.ButtonAdv BtnCancel;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel1;
        private Syncfusion.Windows.Forms.ButtonAdv BtnConfirm;
        private Syncfusion.Windows.Forms.Tools.TextBoxExt TextEng;
        private Syncfusion.Windows.Forms.Tools.TextBoxExt TextSystem;
        private Syncfusion.Windows.Forms.ButtonAdv BtnSave;
        private Syncfusion.Windows.Forms.Tools.GradientLabel gradientLabel2;
        private Syncfusion.Windows.Forms.Tools.GradientLabel Label_System;
        private System.Windows.Forms.Timer timer1;
    }
}