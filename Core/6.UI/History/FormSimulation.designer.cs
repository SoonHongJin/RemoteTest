namespace Core.UI
{
    partial class FormSimulation
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
            this.CBO_SELECT_DATE = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_Date = new System.Windows.Forms.Label();
            this.MainDisplayPanel = new System.Windows.Forms.Panel();
            this.CBO_SELECT_MONTH = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_DataHistory = new System.Windows.Forms.Label();
            this.HistoryPanel = new System.Windows.Forms.Panel();
            this.tbSearchText = new System.Windows.Forms.TextBox();
            this.btSearch = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_RunSimul = new System.Windows.Forms.Button();
            this.btSetfirstPage = new System.Windows.Forms.Button();
            this.btSetLastPage = new System.Windows.Forms.Button();
            this.btBefore10page = new System.Windows.Forms.Button();
            this.btNext10Page = new System.Windows.Forms.Button();
            this.lbCurrentPage = new System.Windows.Forms.Label();
            this.btNextPage = new System.Windows.Forms.Button();
            this.btnBeforePage = new System.Windows.Forms.Button();
            this.DataListBox = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbl_DataList = new System.Windows.Forms.Label();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.HistoryPanel.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // CBO_SELECT_DATE
            // 
            this.CBO_SELECT_DATE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_SELECT_DATE.Font = new System.Drawing.Font("Calibri", 15F);
            this.CBO_SELECT_DATE.FormattingEnabled = true;
            this.CBO_SELECT_DATE.Location = new System.Drawing.Point(232, 47);
            this.CBO_SELECT_DATE.Name = "CBO_SELECT_DATE";
            this.CBO_SELECT_DATE.Size = new System.Drawing.Size(59, 32);
            this.CBO_SELECT_DATE.TabIndex = 624;
            this.CBO_SELECT_DATE.SelectedIndexChanged += new System.EventHandler(this.CBO_SELECT_DATE_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 18F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(167, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 29);
            this.label3.TabIndex = 623;
            this.label3.Text = "Date : ";
            // 
            // lbl_Date
            // 
            this.lbl_Date.AutoSize = true;
            this.lbl_Date.Font = new System.Drawing.Font("Calibri", 18F);
            this.lbl_Date.ForeColor = System.Drawing.Color.White;
            this.lbl_Date.Location = new System.Drawing.Point(95, 2);
            this.lbl_Date.Name = "lbl_Date";
            this.lbl_Date.Size = new System.Drawing.Size(98, 29);
            this.lbl_Date.TabIndex = 0;
            this.lbl_Date.Text = "DateInfo";
            // 
            // MainDisplayPanel
            // 
            this.MainDisplayPanel.Location = new System.Drawing.Point(297, 84);
            this.MainDisplayPanel.Name = "MainDisplayPanel";
            this.MainDisplayPanel.Size = new System.Drawing.Size(1428, 696);
            this.MainDisplayPanel.TabIndex = 620;
            // 
            // CBO_SELECT_MONTH
            // 
            this.CBO_SELECT_MONTH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_SELECT_MONTH.Font = new System.Drawing.Font("Calibri", 15F);
            this.CBO_SELECT_MONTH.FormattingEnabled = true;
            this.CBO_SELECT_MONTH.Location = new System.Drawing.Point(78, 47);
            this.CBO_SELECT_MONTH.Name = "CBO_SELECT_MONTH";
            this.CBO_SELECT_MONTH.Size = new System.Drawing.Size(89, 32);
            this.CBO_SELECT_MONTH.TabIndex = 618;
            this.CBO_SELECT_MONTH.SelectedIndexChanged += new System.EventHandler(this.CBO_SELECT_MONTH_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Month : ";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel4.Controls.Add(this.lbl_Date);
            this.panel4.Location = new System.Drawing.Point(3, 7);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(288, 32);
            this.panel4.TabIndex = 615;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel1.Controls.Add(this.lbl_DataHistory);
            this.panel1.Location = new System.Drawing.Point(0, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1725, 34);
            this.panel1.TabIndex = 615;
            // 
            // lbl_DataHistory
            // 
            this.lbl_DataHistory.AutoSize = true;
            this.lbl_DataHistory.Font = new System.Drawing.Font("Calibri", 18F);
            this.lbl_DataHistory.ForeColor = System.Drawing.Color.White;
            this.lbl_DataHistory.Location = new System.Drawing.Point(800, 1);
            this.lbl_DataHistory.Name = "lbl_DataHistory";
            this.lbl_DataHistory.Size = new System.Drawing.Size(119, 29);
            this.lbl_DataHistory.TabIndex = 0;
            this.lbl_DataHistory.Text = "Simulation";
            // 
            // HistoryPanel
            // 
            this.HistoryPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.HistoryPanel.Controls.Add(this.tbSearchText);
            this.HistoryPanel.Controls.Add(this.btSearch);
            this.HistoryPanel.Controls.Add(this.panel5);
            this.HistoryPanel.Controls.Add(this.bt_RunSimul);
            this.HistoryPanel.Controls.Add(this.btSetfirstPage);
            this.HistoryPanel.Controls.Add(this.btSetLastPage);
            this.HistoryPanel.Controls.Add(this.btBefore10page);
            this.HistoryPanel.Controls.Add(this.btNext10Page);
            this.HistoryPanel.Controls.Add(this.lbCurrentPage);
            this.HistoryPanel.Controls.Add(this.btNextPage);
            this.HistoryPanel.Controls.Add(this.btnBeforePage);
            this.HistoryPanel.Controls.Add(this.DataListBox);
            this.HistoryPanel.Controls.Add(this.CBO_SELECT_DATE);
            this.HistoryPanel.Controls.Add(this.label3);
            this.HistoryPanel.Controls.Add(this.MainDisplayPanel);
            this.HistoryPanel.Controls.Add(this.CBO_SELECT_MONTH);
            this.HistoryPanel.Controls.Add(this.label2);
            this.HistoryPanel.Controls.Add(this.panel4);
            this.HistoryPanel.Controls.Add(this.panel2);
            this.HistoryPanel.Location = new System.Drawing.Point(0, 33);
            this.HistoryPanel.Name = "HistoryPanel";
            this.HistoryPanel.Size = new System.Drawing.Size(1920, 1048);
            this.HistoryPanel.TabIndex = 616;
            // 
            // tbSearchText
            // 
            this.tbSearchText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSearchText.Location = new System.Drawing.Point(9, 116);
            this.tbSearchText.Name = "tbSearchText";
            this.tbSearchText.Size = new System.Drawing.Size(176, 29);
            this.tbSearchText.TabIndex = 647;
            // 
            // btSearch
            // 
            this.btSearch.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSearch.ForeColor = System.Drawing.Color.White;
            this.btSearch.Location = new System.Drawing.Point(189, 117);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(101, 29);
            this.btSearch.TabIndex = 648;
            this.btSearch.Text = "SEARCH";
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel5.Controls.Add(this.label1);
            this.panel5.Location = new System.Drawing.Point(297, 45);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1426, 30);
            this.panel5.TabIndex = 615;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(624, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "MonoImage";
            // 
            // bt_RunSimul
            // 
            this.bt_RunSimul.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.bt_RunSimul.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_RunSimul.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_RunSimul.ForeColor = System.Drawing.Color.White;
            this.bt_RunSimul.Location = new System.Drawing.Point(9, 84);
            this.bt_RunSimul.Name = "bt_RunSimul";
            this.bt_RunSimul.Size = new System.Drawing.Size(282, 29);
            this.bt_RunSimul.TabIndex = 651;
            this.bt_RunSimul.Text = "RUN";
            this.bt_RunSimul.UseVisualStyleBackColor = true;
            this.bt_RunSimul.Click += new System.EventHandler(this.bt_RunSimul_Click);
            // 
            // btSetfirstPage
            // 
            this.btSetfirstPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btSetfirstPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSetfirstPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSetfirstPage.ForeColor = System.Drawing.Color.White;
            this.btSetfirstPage.Location = new System.Drawing.Point(14, 821);
            this.btSetfirstPage.Name = "btSetfirstPage";
            this.btSetfirstPage.Size = new System.Drawing.Size(130, 28);
            this.btSetfirstPage.TabIndex = 646;
            this.btSetfirstPage.Text = "FIRST";
            this.btSetfirstPage.UseVisualStyleBackColor = true;
            this.btSetfirstPage.Click += new System.EventHandler(this.btSetfirstPage_Click);
            // 
            // btSetLastPage
            // 
            this.btSetLastPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btSetLastPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSetLastPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSetLastPage.ForeColor = System.Drawing.Color.White;
            this.btSetLastPage.Location = new System.Drawing.Point(155, 821);
            this.btSetLastPage.Name = "btSetLastPage";
            this.btSetLastPage.Size = new System.Drawing.Size(134, 28);
            this.btSetLastPage.TabIndex = 645;
            this.btSetLastPage.Text = "LAST";
            this.btSetLastPage.UseVisualStyleBackColor = true;
            this.btSetLastPage.Click += new System.EventHandler(this.btSetLastPage_Click);
            // 
            // btBefore10page
            // 
            this.btBefore10page.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btBefore10page.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btBefore10page.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btBefore10page.ForeColor = System.Drawing.Color.White;
            this.btBefore10page.Location = new System.Drawing.Point(13, 789);
            this.btBefore10page.Name = "btBefore10page";
            this.btBefore10page.Size = new System.Drawing.Size(42, 28);
            this.btBefore10page.TabIndex = 644;
            this.btBefore10page.Text = "<<";
            this.btBefore10page.UseVisualStyleBackColor = true;
            this.btBefore10page.Click += new System.EventHandler(this.btBefore10page_Click);
            // 
            // btNext10Page
            // 
            this.btNext10Page.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btNext10Page.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btNext10Page.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btNext10Page.ForeColor = System.Drawing.Color.White;
            this.btNext10Page.Location = new System.Drawing.Point(249, 787);
            this.btNext10Page.Name = "btNext10Page";
            this.btNext10Page.Size = new System.Drawing.Size(42, 28);
            this.btNext10Page.TabIndex = 643;
            this.btNext10Page.Text = ">>";
            this.btNext10Page.UseVisualStyleBackColor = true;
            this.btNext10Page.Click += new System.EventHandler(this.btNext10Page_Click);
            // 
            // lbCurrentPage
            // 
            this.lbCurrentPage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCurrentPage.AutoSize = true;
            this.lbCurrentPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCurrentPage.ForeColor = System.Drawing.Color.White;
            this.lbCurrentPage.Location = new System.Drawing.Point(111, 792);
            this.lbCurrentPage.Name = "lbCurrentPage";
            this.lbCurrentPage.Size = new System.Drawing.Size(0, 25);
            this.lbCurrentPage.TabIndex = 642;
            // 
            // btNextPage
            // 
            this.btNextPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btNextPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btNextPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btNextPage.ForeColor = System.Drawing.Color.White;
            this.btNextPage.Location = new System.Drawing.Point(213, 787);
            this.btNextPage.Name = "btNextPage";
            this.btNextPage.Size = new System.Drawing.Size(29, 28);
            this.btNextPage.TabIndex = 641;
            this.btNextPage.Text = ">";
            this.btNextPage.UseVisualStyleBackColor = true;
            this.btNextPage.Click += new System.EventHandler(this.btNextPage_Click);
            // 
            // btnBeforePage
            // 
            this.btnBeforePage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btnBeforePage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBeforePage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBeforePage.ForeColor = System.Drawing.Color.White;
            this.btnBeforePage.Location = new System.Drawing.Point(61, 790);
            this.btnBeforePage.Name = "btnBeforePage";
            this.btnBeforePage.Size = new System.Drawing.Size(28, 26);
            this.btnBeforePage.TabIndex = 640;
            this.btnBeforePage.Text = "<";
            this.btnBeforePage.UseVisualStyleBackColor = true;
            this.btnBeforePage.Click += new System.EventHandler(this.btnBeforePage_Click);
            // 
            // DataListBox
            // 
            this.DataListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataListBox.FormattingEnabled = true;
            this.DataListBox.ItemHeight = 25;
            this.DataListBox.Location = new System.Drawing.Point(8, 151);
            this.DataListBox.Name = "DataListBox";
            this.DataListBox.ScrollAlwaysVisible = true;
            this.DataListBox.Size = new System.Drawing.Size(283, 629);
            this.DataListBox.TabIndex = 625;
            this.DataListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DataListBox_MouseClick);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel2.Controls.Add(this.lbl_DataList);
            this.panel2.Location = new System.Drawing.Point(297, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1426, 32);
            this.panel2.TabIndex = 614;
            // 
            // lbl_DataList
            // 
            this.lbl_DataList.AutoSize = true;
            this.lbl_DataList.Font = new System.Drawing.Font("Calibri", 18F);
            this.lbl_DataList.ForeColor = System.Drawing.Color.White;
            this.lbl_DataList.Location = new System.Drawing.Point(619, 2);
            this.lbl_DataList.Name = "lbl_DataList";
            this.lbl_DataList.Size = new System.Drawing.Size(150, 29);
            this.lbl_DataList.TabIndex = 0;
            this.lbl_DataList.Text = "Image Display";
            // 
            // FormSimulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1920, 1048);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.HistoryPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSimulation";
            this.Text = "FormHistoryData";
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.HistoryPanel.ResumeLayout(false);
            this.HistoryPanel.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox CBO_SELECT_DATE;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_Date;
        private System.Windows.Forms.Panel MainDisplayPanel;
        private System.Windows.Forms.ComboBox CBO_SELECT_MONTH;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_DataHistory;
        private System.Windows.Forms.Panel HistoryPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox DataListBox;
        private System.Windows.Forms.Label lbCurrentPage;
        private System.Windows.Forms.Button btNextPage;
        private System.Windows.Forms.Button btnBeforePage;
        private System.Windows.Forms.Button btBefore10page;
        private System.Windows.Forms.Button btNext10Page;
        private System.Windows.Forms.Button btSetfirstPage;
        private System.Windows.Forms.Button btSetLastPage;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.TextBox tbSearchText;
        private System.Windows.Forms.Button bt_RunSimul;
        private System.Windows.Forms.Label lbl_DataList;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label1;
    }
}