namespace Core.UI
{ 
    partial class FormInspectionData
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel3 = new System.Windows.Forms.Panel();
            this.DefectDataGridView = new System.Windows.Forms.DataGridView();
            this.lbl_Date = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.HistoryPanel = new System.Windows.Forms.Panel();
            this.lbl_PageInfo = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.EndDatePicker = new System.Windows.Forms.DateTimePicker();
            this.DataListBox = new System.Windows.Forms.ListBox();
            this.btSearch = new System.Windows.Forms.Button();
            this.btSetfirstPage = new System.Windows.Forms.Button();
            this.StartDatePicker = new System.Windows.Forms.DateTimePicker();
            this.btSetLastPage = new System.Windows.Forms.Button();
            this.btBefore10page = new System.Windows.Forms.Button();
            this.btNext10Page = new System.Windows.Forms.Button();
            this.lbCurrentPage = new System.Windows.Forms.Label();
            this.btNextPage = new System.Windows.Forms.Button();
            this.btnBeforePage = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.DefectDataGridView)).BeginInit();
            this.panel4.SuspendLayout();
            this.HistoryPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel3.Location = new System.Drawing.Point(358, 7);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(637, 32);
            this.panel3.TabIndex = 615;
            // 
            // DefectDataGridView
            // 
            this.DefectDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DefectDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DefectDataGridView.DefaultCellStyle = dataGridViewCellStyle1;
            this.DefectDataGridView.Location = new System.Drawing.Point(358, 42);
            this.DefectDataGridView.Name = "DefectDataGridView";
            this.DefectDataGridView.ReadOnly = true;
            this.DefectDataGridView.RowTemplate.Height = 23;
            this.DefectDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DefectDataGridView.Size = new System.Drawing.Size(637, 757);
            this.DefectDataGridView.TabIndex = 0;
            this.DefectDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DefectDataGridView_CellClick_1);
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
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel4.Controls.Add(this.lbl_Date);
            this.panel4.Location = new System.Drawing.Point(3, 7);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(349, 32);
            this.panel4.TabIndex = 615;
            // 
            // HistoryPanel
            // 
            this.HistoryPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.HistoryPanel.Controls.Add(this.lbl_PageInfo);
            this.HistoryPanel.Controls.Add(this.label5);
            this.HistoryPanel.Controls.Add(this.EndDatePicker);
            this.HistoryPanel.Controls.Add(this.DataListBox);
            this.HistoryPanel.Controls.Add(this.btSearch);
            this.HistoryPanel.Controls.Add(this.btSetfirstPage);
            this.HistoryPanel.Controls.Add(this.StartDatePicker);
            this.HistoryPanel.Controls.Add(this.btSetLastPage);
            this.HistoryPanel.Controls.Add(this.btBefore10page);
            this.HistoryPanel.Controls.Add(this.btNext10Page);
            this.HistoryPanel.Controls.Add(this.lbCurrentPage);
            this.HistoryPanel.Controls.Add(this.btNextPage);
            this.HistoryPanel.Controls.Add(this.btnBeforePage);
            this.HistoryPanel.Controls.Add(this.panel4);
            this.HistoryPanel.Controls.Add(this.panel3);
            this.HistoryPanel.Controls.Add(this.DefectDataGridView);
            this.HistoryPanel.Location = new System.Drawing.Point(0, 0);
            this.HistoryPanel.Name = "HistoryPanel";
            this.HistoryPanel.Size = new System.Drawing.Size(998, 883);
            this.HistoryPanel.TabIndex = 616;
            // 
            // lbl_PageInfo
            // 
            this.lbl_PageInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_PageInfo.AutoSize = true;
            this.lbl_PageInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_PageInfo.ForeColor = System.Drawing.Color.White;
            this.lbl_PageInfo.Location = new System.Drawing.Point(133, 740);
            this.lbl_PageInfo.Name = "lbl_PageInfo";
            this.lbl_PageInfo.Size = new System.Drawing.Size(0, 25);
            this.lbl_PageInfo.TabIndex = 748;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 18F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(6, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 29);
            this.label5.TabIndex = 675;
            this.label5.Text = "~";
            // 
            // EndDatePicker
            // 
            this.EndDatePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EndDatePicker.Location = new System.Drawing.Point(33, 80);
            this.EndDatePicker.Name = "EndDatePicker";
            this.EndDatePicker.Size = new System.Drawing.Size(198, 29);
            this.EndDatePicker.TabIndex = 674;
            // 
            // DataListBox
            // 
            this.DataListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataListBox.FormattingEnabled = true;
            this.DataListBox.ItemHeight = 25;
            this.DataListBox.Location = new System.Drawing.Point(3, 144);
            this.DataListBox.Name = "DataListBox";
            this.DataListBox.ScrollAlwaysVisible = true;
            this.DataListBox.Size = new System.Drawing.Size(349, 579);
            this.DataListBox.TabIndex = 625;
            this.DataListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DataListBox_MouseClick);
            // 
            // btSearch
            // 
            this.btSearch.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSearch.ForeColor = System.Drawing.Color.White;
            this.btSearch.Location = new System.Drawing.Point(237, 45);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(113, 64);
            this.btSearch.TabIndex = 648;
            this.btSearch.Text = "SEARCH";
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // btSetfirstPage
            // 
            this.btSetfirstPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btSetfirstPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSetfirstPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSetfirstPage.ForeColor = System.Drawing.Color.White;
            this.btSetfirstPage.Location = new System.Drawing.Point(14, 771);
            this.btSetfirstPage.Name = "btSetfirstPage";
            this.btSetfirstPage.Size = new System.Drawing.Size(166, 28);
            this.btSetfirstPage.TabIndex = 646;
            this.btSetfirstPage.Text = "FIRST";
            this.btSetfirstPage.UseVisualStyleBackColor = true;
            this.btSetfirstPage.Click += new System.EventHandler(this.btSetfirstPage_Click);
            // 
            // StartDatePicker
            // 
            this.StartDatePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartDatePicker.Location = new System.Drawing.Point(3, 45);
            this.StartDatePicker.Name = "StartDatePicker";
            this.StartDatePicker.Size = new System.Drawing.Size(201, 29);
            this.StartDatePicker.TabIndex = 673;
            // 
            // btSetLastPage
            // 
            this.btSetLastPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btSetLastPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSetLastPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSetLastPage.ForeColor = System.Drawing.Color.White;
            this.btSetLastPage.Location = new System.Drawing.Point(186, 771);
            this.btSetLastPage.Name = "btSetLastPage";
            this.btSetLastPage.Size = new System.Drawing.Size(165, 28);
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
            this.btBefore10page.Location = new System.Drawing.Point(14, 739);
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
            this.btNext10Page.Location = new System.Drawing.Point(309, 737);
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
            this.lbCurrentPage.Location = new System.Drawing.Point(-810, 742);
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
            this.btNextPage.Location = new System.Drawing.Point(274, 737);
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
            this.btnBeforePage.Location = new System.Drawing.Point(62, 740);
            this.btnBeforePage.Name = "btnBeforePage";
            this.btnBeforePage.Size = new System.Drawing.Size(28, 26);
            this.btnBeforePage.TabIndex = 640;
            this.btnBeforePage.Text = "<";
            this.btnBeforePage.UseVisualStyleBackColor = true;
            this.btnBeforePage.Click += new System.EventHandler(this.btnBeforePage_Click);
            // 
            // FormInspectionData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1720, 883);
            this.Controls.Add(this.HistoryPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormInspectionData";
            this.Text = "FormHistoryData";
            ((System.ComponentModel.ISupportInitialize)(this.DefectDataGridView)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.HistoryPanel.ResumeLayout(false);
            this.HistoryPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView DefectDataGridView;
        private System.Windows.Forms.Label lbl_Date;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel HistoryPanel;
        private System.Windows.Forms.ListBox DataListBox;
        private System.Windows.Forms.Label lbCurrentPage;
        private System.Windows.Forms.Button btNextPage;
        private System.Windows.Forms.Button btnBeforePage;
        private System.Windows.Forms.Button btBefore10page;
        private System.Windows.Forms.Button btNext10Page;
        private System.Windows.Forms.Button btSetfirstPage;
        private System.Windows.Forms.Button btSetLastPage;
        private System.Windows.Forms.Button btSearch;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker EndDatePicker;
        private System.Windows.Forms.DateTimePicker StartDatePicker;
        private System.Windows.Forms.Label lbl_PageInfo;
    }
}