namespace Core.UI
{ 
    partial class FormLog
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
            this.lbl_LogData = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.HistoryPanel = new System.Windows.Forms.Panel();
            this.cbo_SelectDate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LogDataListView = new System.Windows.Forms.ListView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_Date = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbl_LogList = new System.Windows.Forms.Label();
            this.DateListView = new System.Windows.Forms.ListView();
            this.panel1.SuspendLayout();
            this.HistoryPanel.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_LogData
            // 
            this.lbl_LogData.AutoSize = true;
            this.lbl_LogData.Font = new System.Drawing.Font("Calibri", 18F);
            this.lbl_LogData.ForeColor = System.Drawing.Color.White;
            this.lbl_LogData.Location = new System.Drawing.Point(807, 1);
            this.lbl_LogData.Name = "lbl_LogData";
            this.lbl_LogData.Size = new System.Drawing.Size(122, 29);
            this.lbl_LogData.TabIndex = 0;
            this.lbl_LogData.Text = "Log History";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel1.Controls.Add(this.lbl_LogData);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1723, 32);
            this.panel1.TabIndex = 613;
            // 
            // HistoryPanel
            // 
            this.HistoryPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.HistoryPanel.Controls.Add(this.cbo_SelectDate);
            this.HistoryPanel.Controls.Add(this.label2);
            this.HistoryPanel.Controls.Add(this.LogDataListView);
            this.HistoryPanel.Controls.Add(this.panel4);
            this.HistoryPanel.Controls.Add(this.panel2);
            this.HistoryPanel.Controls.Add(this.DateListView);
            this.HistoryPanel.Location = new System.Drawing.Point(0, 31);
            this.HistoryPanel.Name = "HistoryPanel";
            this.HistoryPanel.Size = new System.Drawing.Size(1723, 925);
            this.HistoryPanel.TabIndex = 614;
            // 
            // cbo_SelectDate
            // 
            this.cbo_SelectDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.cbo_SelectDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbo_SelectDate.Font = new System.Drawing.Font("Calibri", 12F);
            this.cbo_SelectDate.ForeColor = System.Drawing.Color.White;
            this.cbo_SelectDate.FormattingEnabled = true;
            this.cbo_SelectDate.Location = new System.Drawing.Point(138, 48);
            this.cbo_SelectDate.Name = "cbo_SelectDate";
            this.cbo_SelectDate.Size = new System.Drawing.Size(239, 27);
            this.cbo_SelectDate.TabIndex = 618;
            this.cbo_SelectDate.SelectedIndexChanged += new System.EventHandler(this.cbo_SelectDate_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 18F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(11, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 29);
            this.label2.TabIndex = 617;
            this.label2.Text = "SelectDate : ";
            // 
            // LogDataListView
            // 
            this.LogDataListView.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogDataListView.HideSelection = false;
            this.LogDataListView.Location = new System.Drawing.Point(383, 41);
            this.LogDataListView.Name = "LogDataListView";
            this.LogDataListView.Size = new System.Drawing.Size(1340, 871);
            this.LogDataListView.TabIndex = 616;
            this.LogDataListView.UseCompatibleStateImageBehavior = false;
            this.LogDataListView.View = System.Windows.Forms.View.List;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel4.Controls.Add(this.panel3);
            this.panel4.Controls.Add(this.lbl_Date);
            this.panel4.Location = new System.Drawing.Point(3, 7);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(374, 32);
            this.panel4.TabIndex = 615;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(8, 41);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(374, 32);
            this.panel3.TabIndex = 616;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 18F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(130, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "DateInfo";
            // 
            // lbl_Date
            // 
            this.lbl_Date.AutoSize = true;
            this.lbl_Date.Font = new System.Drawing.Font("Calibri", 18F);
            this.lbl_Date.ForeColor = System.Drawing.Color.White;
            this.lbl_Date.Location = new System.Drawing.Point(130, 3);
            this.lbl_Date.Name = "lbl_Date";
            this.lbl_Date.Size = new System.Drawing.Size(98, 29);
            this.lbl_Date.TabIndex = 0;
            this.lbl_Date.Text = "DateInfo";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel2.Controls.Add(this.lbl_LogList);
            this.panel2.Location = new System.Drawing.Point(383, 7);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1340, 32);
            this.panel2.TabIndex = 614;
            // 
            // lbl_LogList
            // 
            this.lbl_LogList.AutoSize = true;
            this.lbl_LogList.Font = new System.Drawing.Font("Calibri", 18F);
            this.lbl_LogList.ForeColor = System.Drawing.Color.White;
            this.lbl_LogList.Location = new System.Drawing.Point(625, 3);
            this.lbl_LogList.Name = "lbl_LogList";
            this.lbl_LogList.Size = new System.Drawing.Size(85, 29);
            this.lbl_LogList.TabIndex = 0;
            this.lbl_LogList.Text = "Log List";
            // 
            // DateListView
            // 
            this.DateListView.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DateListView.HideSelection = false;
            this.DateListView.Location = new System.Drawing.Point(3, 81);
            this.DateListView.Name = "DateListView";
            this.DateListView.Size = new System.Drawing.Size(374, 831);
            this.DateListView.TabIndex = 1;
            this.DateListView.UseCompatibleStateImageBehavior = false;
            this.DateListView.View = System.Windows.Forms.View.List;
            this.DateListView.SelectedIndexChanged += new System.EventHandler(this.DateListView_SelectedIndexChanged);
            // 
            // FormLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1723, 955);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.HistoryPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLog";
            this.Text = "FormHistoryData";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.HistoryPanel.ResumeLayout(false);
            this.HistoryPanel.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_LogData;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel HistoryPanel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lbl_Date;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbl_LogList;
        private System.Windows.Forms.ListView DateListView;
        private System.Windows.Forms.ListView LogDataListView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbo_SelectDate;
    }
}