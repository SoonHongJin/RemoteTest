
namespace Core.UI
{
    partial class FormCogToolBlock
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
            this.btn_Apply = new System.Windows.Forms.Button();
            this.cogToolBlockEdit = new Cognex.VisionPro.ToolBlock.CogToolBlockEditV2();
            this.Panel_Title = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cogToolBlockEdit)).BeginInit();
            this.Panel_Title.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Apply
            // 
            this.btn_Apply.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btn_Apply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Apply.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Apply.ForeColor = System.Drawing.Color.White;
            this.btn_Apply.Location = new System.Drawing.Point(1569, 848);
            this.btn_Apply.Name = "btn_Apply";
            this.btn_Apply.Size = new System.Drawing.Size(144, 49);
            this.btn_Apply.TabIndex = 325;
            this.btn_Apply.Text = "Apply";
            this.btn_Apply.UseVisualStyleBackColor = true;
            // 
            // cogToolBlockEdit
            // 
            this.cogToolBlockEdit.AllowDrop = true;
            this.cogToolBlockEdit.ContextMenuCustomizer = null;
            this.cogToolBlockEdit.Location = new System.Drawing.Point(21, 47);
            this.cogToolBlockEdit.MinimumSize = new System.Drawing.Size(570, 0);
            this.cogToolBlockEdit.Name = "cogToolBlockEdit";
            this.cogToolBlockEdit.ShowNodeToolTips = true;
            this.cogToolBlockEdit.Size = new System.Drawing.Size(1692, 795);
            this.cogToolBlockEdit.SuspendElectricRuns = false;
            this.cogToolBlockEdit.TabIndex = 324;
            // 
            // Panel_Title
            // 
            this.Panel_Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Panel_Title.Controls.Add(this.label1);
            this.Panel_Title.Location = new System.Drawing.Point(1, 1);
            this.Panel_Title.Name = "Panel_Title";
            this.Panel_Title.Size = new System.Drawing.Size(1725, 40);
            this.Panel_Title.TabIndex = 323;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(16, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tool Edit Page";
            // 
            // FormCogToolBlockEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.ClientSize = new System.Drawing.Size(1725, 1080);
            this.Controls.Add(this.btn_Apply);
            this.Controls.Add(this.cogToolBlockEdit);
            this.Controls.Add(this.Panel_Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormCogToolBlockEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormCogToolBlockEdit";
            ((System.ComponentModel.ISupportInitialize)(this.cogToolBlockEdit)).EndInit();
            this.Panel_Title.ResumeLayout(false);
            this.Panel_Title.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Apply;
        private Cognex.VisionPro.ToolBlock.CogToolBlockEditV2 cogToolBlockEdit;
        private System.Windows.Forms.Panel Panel_Title;
        private System.Windows.Forms.Label label1;
    }
}