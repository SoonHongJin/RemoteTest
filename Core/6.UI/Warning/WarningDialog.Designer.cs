namespace Core.UI
{
    partial class WarningDialog
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ctrLabelContent = new System.Windows.Forms.Label();
            this.ctrBtnOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.ctrLabelContent, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ctrBtnOK, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(290, 78);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ctrLabelContent
            // 
            this.ctrLabelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrLabelContent.Location = new System.Drawing.Point(3, 0);
            this.ctrLabelContent.Name = "ctrLabelContent";
            this.ctrLabelContent.Size = new System.Drawing.Size(284, 49);
            this.ctrLabelContent.TabIndex = 0;
            this.ctrLabelContent.Text = "Content";
            this.ctrLabelContent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ctrBtnOK
            // 
            this.ctrBtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ctrBtnOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.ctrBtnOK.Location = new System.Drawing.Point(212, 52);
            this.ctrBtnOK.Name = "ctrBtnOK";
            this.ctrBtnOK.Size = new System.Drawing.Size(75, 23);
            this.ctrBtnOK.TabIndex = 1;
            this.ctrBtnOK.Text = "OK";
            this.ctrBtnOK.UseVisualStyleBackColor = true;
            // 
            // WarningDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 78);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "WarningDialog";
            this.Text = "Warning";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label ctrLabelContent;
        private System.Windows.Forms.Button ctrBtnOK;
    }
}