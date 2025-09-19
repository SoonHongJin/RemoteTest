
namespace Core.UI
{
    partial class FormMsgBox
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.BtnOK = new System.Windows.Forms.Button();
            this.panel10 = new System.Windows.Forms.Panel();
            this.lb_EngMsg = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.TextEng = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.TextSystem = new System.Windows.Forms.Label();
            this.BtnNo = new System.Windows.Forms.Button();
            this.panel10.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // BtnOK
            // 
            this.BtnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.BtnOK.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.BtnOK.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnOK.Location = new System.Drawing.Point(439, 224);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(130, 61);
            this.BtnOK.TabIndex = 170;
            this.BtnOK.Tag = "3";
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = false;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel10.Controls.Add(this.lb_EngMsg);
            this.panel10.Location = new System.Drawing.Point(4, 31);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(118, 88);
            this.panel10.TabIndex = 173;
            // 
            // lb_EngMsg
            // 
            this.lb_EngMsg.AutoSize = true;
            this.lb_EngMsg.Font = new System.Drawing.Font("Calibri", 15F);
            this.lb_EngMsg.ForeColor = System.Drawing.Color.White;
            this.lb_EngMsg.Location = new System.Drawing.Point(17, 17);
            this.lb_EngMsg.Name = "lb_EngMsg";
            this.lb_EngMsg.Size = new System.Drawing.Size(81, 24);
            this.lb_EngMsg.TabIndex = 0;
            this.lb_EngMsg.Text = "[English]";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(4, 125);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(118, 93);
            this.panel1.TabIndex = 174;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 15F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "[Language]";
            // 
            // TextEng
            // 
            this.TextEng.AutoSize = true;
            this.TextEng.Font = new System.Drawing.Font("Calibri", 15F);
            this.TextEng.ForeColor = System.Drawing.Color.White;
            this.TextEng.Location = new System.Drawing.Point(5, 4);
            this.TextEng.Name = "TextEng";
            this.TextEng.Size = new System.Drawing.Size(81, 24);
            this.TextEng.TabIndex = 0;
            this.TextEng.Text = "[English]";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel2.Controls.Add(this.TextEng);
            this.panel2.Location = new System.Drawing.Point(128, 31);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(586, 88);
            this.panel2.TabIndex = 174;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel3.Controls.Add(this.TextSystem);
            this.panel3.Location = new System.Drawing.Point(128, 125);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(586, 93);
            this.panel3.TabIndex = 175;
            // 
            // TextSystem
            // 
            this.TextSystem.AutoSize = true;
            this.TextSystem.Font = new System.Drawing.Font("Calibri", 15F);
            this.TextSystem.ForeColor = System.Drawing.Color.White;
            this.TextSystem.Location = new System.Drawing.Point(5, 4);
            this.TextSystem.Name = "TextSystem";
            this.TextSystem.Size = new System.Drawing.Size(81, 24);
            this.TextSystem.TabIndex = 0;
            this.TextSystem.Text = "[English]";
            // 
            // BtnNo
            // 
            this.BtnNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.BtnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.BtnNo.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnNo.Location = new System.Drawing.Point(575, 224);
            this.BtnNo.Name = "BtnNo";
            this.BtnNo.Size = new System.Drawing.Size(130, 61);
            this.BtnNo.TabIndex = 176;
            this.BtnNo.Tag = "3";
            this.BtnNo.Text = "No";
            this.BtnNo.UseVisualStyleBackColor = false;
            this.BtnNo.Click += new System.EventHandler(this.BtnNo_Click);
            // 
            // FormMsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(726, 297);
            this.Controls.Add(this.BtnNo);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.BtnOK);
            this.Name = "FormMsgBox";
            this.Text = "FormMsgBox";
            this.Load += new System.EventHandler(this.FormUtilMsg_Load);
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label lb_EngMsg;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label TextEng;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label TextSystem;
        private System.Windows.Forms.Button BtnNo;
    }
}