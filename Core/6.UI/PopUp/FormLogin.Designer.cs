namespace Core.UI
{
    partial class FormLogin
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
            this.tb_PassWord = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_ID = new System.Windows.Forms.TextBox();
            this.LBL_ModelNum = new System.Windows.Forms.Label();
            this.btn_Login = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.rb_Operator = new System.Windows.Forms.RadioButton();
            this.rb_Master = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel3.Controls.Add(this.lbl_CreateModel);
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(281, 31);
            this.panel3.TabIndex = 45;
            // 
            // lbl_CreateModel
            // 
            this.lbl_CreateModel.AutoSize = true;
            this.lbl_CreateModel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CreateModel.ForeColor = System.Drawing.Color.White;
            this.lbl_CreateModel.Location = new System.Drawing.Point(3, 0);
            this.lbl_CreateModel.Name = "lbl_CreateModel";
            this.lbl_CreateModel.Size = new System.Drawing.Size(63, 26);
            this.lbl_CreateModel.TabIndex = 0;
            this.lbl_CreateModel.Text = "Log In";
            // 
            // tb_PassWord
            // 
            this.tb_PassWord.BackColor = System.Drawing.Color.Black;
            this.tb_PassWord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_PassWord.Font = new System.Drawing.Font("Calibri", 12F);
            this.tb_PassWord.ForeColor = System.Drawing.Color.White;
            this.tb_PassWord.Location = new System.Drawing.Point(110, 114);
            this.tb_PassWord.Name = "tb_PassWord";
            this.tb_PassWord.PasswordChar = '*';
            this.tb_PassWord.Size = new System.Drawing.Size(163, 27);
            this.tb_PassWord.TabIndex = 91;
            this.tb_PassWord.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(12, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 19);
            this.label1.TabIndex = 90;
            this.label1.Text = "PassWord : ";
            // 
            // tb_ID
            // 
            this.tb_ID.BackColor = System.Drawing.Color.Black;
            this.tb_ID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_ID.Font = new System.Drawing.Font("Calibri", 12F);
            this.tb_ID.ForeColor = System.Drawing.Color.White;
            this.tb_ID.Location = new System.Drawing.Point(110, 70);
            this.tb_ID.Name = "tb_ID";
            this.tb_ID.Size = new System.Drawing.Size(163, 27);
            this.tb_ID.TabIndex = 89;
            this.tb_ID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LBL_ModelNum
            // 
            this.LBL_ModelNum.AutoSize = true;
            this.LBL_ModelNum.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_ModelNum.ForeColor = System.Drawing.SystemColors.Control;
            this.LBL_ModelNum.Location = new System.Drawing.Point(12, 72);
            this.LBL_ModelNum.Name = "LBL_ModelNum";
            this.LBL_ModelNum.Size = new System.Drawing.Size(35, 19);
            this.LBL_ModelNum.TabIndex = 88;
            this.LBL_ModelNum.Text = "ID : ";
            // 
            // btn_Login
            // 
            this.btn_Login.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_Login.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Login.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_Login.ForeColor = System.Drawing.Color.White;
            this.btn_Login.Location = new System.Drawing.Point(12, 181);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(117, 33);
            this.btn_Login.TabIndex = 94;
            this.btn_Login.Text = "Login";
            this.btn_Login.UseVisualStyleBackColor = true;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Font = new System.Drawing.Font("Calibri", 12F);
            this.btn_Cancel.ForeColor = System.Drawing.Color.White;
            this.btn_Cancel.Location = new System.Drawing.Point(156, 181);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(117, 33);
            this.btn_Cancel.TabIndex = 95;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // rb_Operator
            // 
            this.rb_Operator.AutoSize = true;
            this.rb_Operator.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_Operator.ForeColor = System.Drawing.Color.White;
            this.rb_Operator.Location = new System.Drawing.Point(12, 40);
            this.rb_Operator.Name = "rb_Operator";
            this.rb_Operator.Size = new System.Drawing.Size(98, 24);
            this.rb_Operator.TabIndex = 96;
            this.rb_Operator.TabStop = true;
            this.rb_Operator.Text = "Operator";
            this.rb_Operator.UseVisualStyleBackColor = true;
            // 
            // rb_Master
            // 
            this.rb_Master.AutoSize = true;
            this.rb_Master.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_Master.ForeColor = System.Drawing.Color.White;
            this.rb_Master.Location = new System.Drawing.Point(133, 40);
            this.rb_Master.Name = "rb_Master";
            this.rb_Master.Size = new System.Drawing.Size(82, 24);
            this.rb_Master.TabIndex = 97;
            this.rb_Master.TabStop = true;
            this.rb_Master.Text = "Master";
            this.rb_Master.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.ForeColor = System.Drawing.Color.White;
            this.checkBox1.Location = new System.Drawing.Point(193, 151);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(70, 17);
            this.checkBox1.TabIndex = 98;
            this.checkBox1.Text = "ViewP.W";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.ClientSize = new System.Drawing.Size(285, 244);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.rb_Master);
            this.Controls.Add(this.rb_Operator);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Login);
            this.Controls.Add(this.tb_PassWord);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_ID);
            this.Controls.Add(this.LBL_ModelNum);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLogin";
            this.Text = "FormLogin";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lbl_CreateModel;
        public System.Windows.Forms.TextBox tb_PassWord;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox tb_ID;
        private System.Windows.Forms.Label LBL_ModelNum;
        private System.Windows.Forms.Button btn_Login;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.RadioButton rb_Operator;
        private System.Windows.Forms.RadioButton rb_Master;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}