namespace Core.UI
{
    partial class FormInterfaceData
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_DataHistory = new System.Windows.Forms.Label();
            this.HistoryPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtElapsedTime = new System.Windows.Forms.TextBox();
            this.btBefore10page = new System.Windows.Forms.Button();
            this.lbCurrentPage = new System.Windows.Forms.Label();
            this.btnBeforePage = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtEndTime = new System.Windows.Forms.TextBox();
            this.txtStartTime = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.DataListBox = new System.Windows.Forms.ListBox();
            this.btSetfirstPage = new System.Windows.Forms.Button();
            this.btSetLastPage = new System.Windows.Forms.Button();
            this.btSearch = new System.Windows.Forms.Button();
            this.tbSearchText = new System.Windows.Forms.TextBox();
            this.CBO_SELECT_DATE = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CBO_SELECT_MONTH = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btNext10Page = new System.Windows.Forms.Button();
            this.btNextPage = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.InterfaceChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1.SuspendLayout();
            this.HistoryPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InterfaceChart)).BeginInit();
            this.SuspendLayout();
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
            this.lbl_DataHistory.Size = new System.Drawing.Size(135, 29);
            this.lbl_DataHistory.TabIndex = 0;
            this.lbl_DataHistory.Text = "Data History";
            // 
            // HistoryPanel
            // 
            this.HistoryPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.HistoryPanel.Controls.Add(this.label1);
            this.HistoryPanel.Controls.Add(this.txtElapsedTime);
            this.HistoryPanel.Controls.Add(this.btBefore10page);
            this.HistoryPanel.Controls.Add(this.lbCurrentPage);
            this.HistoryPanel.Controls.Add(this.btnBeforePage);
            this.HistoryPanel.Controls.Add(this.label6);
            this.HistoryPanel.Controls.Add(this.label5);
            this.HistoryPanel.Controls.Add(this.txtEndTime);
            this.HistoryPanel.Controls.Add(this.txtStartTime);
            this.HistoryPanel.Controls.Add(this.panel3);
            this.HistoryPanel.Controls.Add(this.DataListBox);
            this.HistoryPanel.Controls.Add(this.btSetfirstPage);
            this.HistoryPanel.Controls.Add(this.btSetLastPage);
            this.HistoryPanel.Controls.Add(this.btSearch);
            this.HistoryPanel.Controls.Add(this.tbSearchText);
            this.HistoryPanel.Controls.Add(this.CBO_SELECT_DATE);
            this.HistoryPanel.Controls.Add(this.label3);
            this.HistoryPanel.Controls.Add(this.CBO_SELECT_MONTH);
            this.HistoryPanel.Controls.Add(this.label2);
            this.HistoryPanel.Controls.Add(this.btNext10Page);
            this.HistoryPanel.Controls.Add(this.btNextPage);
            this.HistoryPanel.Controls.Add(this.panel2);
            this.HistoryPanel.Controls.Add(this.InterfaceChart);
            this.HistoryPanel.Location = new System.Drawing.Point(0, 33);
            this.HistoryPanel.Name = "HistoryPanel";
            this.HistoryPanel.Size = new System.Drawing.Size(1920, 1048);
            this.HistoryPanel.TabIndex = 616;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(1447, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 841;
            this.label1.Text = "Elapsed Time :";
            // 
            // txtElapsedTime
            // 
            this.txtElapsedTime.Location = new System.Drawing.Point(1445, 226);
            this.txtElapsedTime.Name = "txtElapsedTime";
            this.txtElapsedTime.Size = new System.Drawing.Size(82, 20);
            this.txtElapsedTime.TabIndex = 840;
            // 
            // btBefore10page
            // 
            this.btBefore10page.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btBefore10page.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btBefore10page.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btBefore10page.ForeColor = System.Drawing.Color.White;
            this.btBefore10page.Location = new System.Drawing.Point(13, 609);
            this.btBefore10page.Name = "btBefore10page";
            this.btBefore10page.Size = new System.Drawing.Size(42, 33);
            this.btBefore10page.TabIndex = 839;
            this.btBefore10page.Text = "<<";
            this.btBefore10page.UseVisualStyleBackColor = true;
            this.btBefore10page.Click += new System.EventHandler(this.btBefore10page_Click_1);
            // 
            // lbCurrentPage
            // 
            this.lbCurrentPage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCurrentPage.AutoSize = true;
            this.lbCurrentPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCurrentPage.ForeColor = System.Drawing.Color.White;
            this.lbCurrentPage.Location = new System.Drawing.Point(118, 607);
            this.lbCurrentPage.Name = "lbCurrentPage";
            this.lbCurrentPage.Size = new System.Drawing.Size(0, 31);
            this.lbCurrentPage.TabIndex = 838;
            // 
            // btnBeforePage
            // 
            this.btnBeforePage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btnBeforePage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBeforePage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBeforePage.ForeColor = System.Drawing.Color.White;
            this.btnBeforePage.Location = new System.Drawing.Point(61, 610);
            this.btnBeforePage.Name = "btnBeforePage";
            this.btnBeforePage.Size = new System.Drawing.Size(28, 31);
            this.btnBeforePage.TabIndex = 837;
            this.btnBeforePage.Text = "<";
            this.btnBeforePage.UseVisualStyleBackColor = true;
            this.btnBeforePage.Click += new System.EventHandler(this.btnBeforePage_Click_1);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(1447, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 836;
            this.label6.Text = "End Tme :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(1447, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 835;
            this.label5.Text = "Start Time : ";
            // 
            // txtEndTime
            // 
            this.txtEndTime.Location = new System.Drawing.Point(1445, 174);
            this.txtEndTime.Name = "txtEndTime";
            this.txtEndTime.Size = new System.Drawing.Size(82, 20);
            this.txtEndTime.TabIndex = 834;
            // 
            // txtStartTime
            // 
            this.txtStartTime.Location = new System.Drawing.Point(1445, 128);
            this.txtStartTime.Name = "txtStartTime";
            this.txtStartTime.Size = new System.Drawing.Size(82, 20);
            this.txtStartTime.TabIndex = 833;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel3.Controls.Add(this.label4);
            this.panel3.Location = new System.Drawing.Point(353, 17);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1216, 32);
            this.panel3.TabIndex = 821;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 18F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(533, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 29);
            this.label4.TabIndex = 0;
            this.label4.Text = "Process Chart";
            // 
            // DataListBox
            // 
            this.DataListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.DataListBox.FormattingEnabled = true;
            this.DataListBox.ItemHeight = 25;
            this.DataListBox.Location = new System.Drawing.Point(11, 126);
            this.DataListBox.Name = "DataListBox";
            this.DataListBox.ScrollAlwaysVisible = true;
            this.DataListBox.Size = new System.Drawing.Size(325, 479);
            this.DataListBox.TabIndex = 832;
            this.DataListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DataListBox_MouseClick_1);
            // 
            // btSetfirstPage
            // 
            this.btSetfirstPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btSetfirstPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSetfirstPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSetfirstPage.ForeColor = System.Drawing.Color.White;
            this.btSetfirstPage.Location = new System.Drawing.Point(13, 652);
            this.btSetfirstPage.Name = "btSetfirstPage";
            this.btSetfirstPage.Size = new System.Drawing.Size(163, 28);
            this.btSetfirstPage.TabIndex = 831;
            this.btSetfirstPage.Text = "FIRST";
            this.btSetfirstPage.UseVisualStyleBackColor = true;
            this.btSetfirstPage.Click += new System.EventHandler(this.btSetfirstPage_Click_1);
            // 
            // btSetLastPage
            // 
            this.btSetLastPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btSetLastPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSetLastPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSetLastPage.ForeColor = System.Drawing.Color.White;
            this.btSetLastPage.Location = new System.Drawing.Point(182, 652);
            this.btSetLastPage.Name = "btSetLastPage";
            this.btSetLastPage.Size = new System.Drawing.Size(148, 28);
            this.btSetLastPage.TabIndex = 830;
            this.btSetLastPage.Text = "LAST";
            this.btSetLastPage.UseVisualStyleBackColor = true;
            this.btSetLastPage.Click += new System.EventHandler(this.btSetLastPage_Click_1);
            // 
            // btSearch
            // 
            this.btSearch.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSearch.ForeColor = System.Drawing.Color.White;
            this.btSearch.Location = new System.Drawing.Point(241, 93);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(97, 29);
            this.btSearch.TabIndex = 829;
            this.btSearch.Text = "SEARCH";
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click_1);
            // 
            // tbSearchText
            // 
            this.tbSearchText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSearchText.Location = new System.Drawing.Point(13, 93);
            this.tbSearchText.Name = "tbSearchText";
            this.tbSearchText.Size = new System.Drawing.Size(228, 29);
            this.tbSearchText.TabIndex = 828;
            // 
            // CBO_SELECT_DATE
            // 
            this.CBO_SELECT_DATE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_SELECT_DATE.Font = new System.Drawing.Font("Calibri", 15F);
            this.CBO_SELECT_DATE.FormattingEnabled = true;
            this.CBO_SELECT_DATE.Location = new System.Drawing.Point(260, 55);
            this.CBO_SELECT_DATE.Name = "CBO_SELECT_DATE";
            this.CBO_SELECT_DATE.Size = new System.Drawing.Size(78, 32);
            this.CBO_SELECT_DATE.TabIndex = 827;
            this.CBO_SELECT_DATE.SelectedIndexChanged += new System.EventHandler(this.CBO_SELECT_DATE_SelectedIndexChanged_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 18F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(189, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 29);
            this.label3.TabIndex = 826;
            this.label3.Text = "Date : ";
            // 
            // CBO_SELECT_MONTH
            // 
            this.CBO_SELECT_MONTH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_SELECT_MONTH.Font = new System.Drawing.Font("Calibri", 15F);
            this.CBO_SELECT_MONTH.FormattingEnabled = true;
            this.CBO_SELECT_MONTH.Location = new System.Drawing.Point(87, 55);
            this.CBO_SELECT_MONTH.Name = "CBO_SELECT_MONTH";
            this.CBO_SELECT_MONTH.Size = new System.Drawing.Size(96, 32);
            this.CBO_SELECT_MONTH.TabIndex = 825;
            this.CBO_SELECT_MONTH.SelectedIndexChanged += new System.EventHandler(this.CBO_SELECT_MONTH_SelectedIndexChanged_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(8, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 26);
            this.label2.TabIndex = 824;
            this.label2.Text = "Month : ";
            // 
            // btNext10Page
            // 
            this.btNext10Page.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btNext10Page.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btNext10Page.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btNext10Page.ForeColor = System.Drawing.Color.White;
            this.btNext10Page.Location = new System.Drawing.Point(288, 609);
            this.btNext10Page.Name = "btNext10Page";
            this.btNext10Page.Size = new System.Drawing.Size(42, 33);
            this.btNext10Page.TabIndex = 823;
            this.btNext10Page.Text = ">>";
            this.btNext10Page.UseVisualStyleBackColor = true;
            this.btNext10Page.Click += new System.EventHandler(this.btNext10Page_Click_1);
            // 
            // btNextPage
            // 
            this.btNextPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btNextPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btNextPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btNextPage.ForeColor = System.Drawing.Color.White;
            this.btNextPage.Location = new System.Drawing.Point(253, 609);
            this.btNextPage.Name = "btNextPage";
            this.btNextPage.Size = new System.Drawing.Size(29, 33);
            this.btNextPage.TabIndex = 822;
            this.btNextPage.Text = ">";
            this.btNextPage.UseVisualStyleBackColor = true;
            this.btNextPage.Click += new System.EventHandler(this.btNextPage_Click_1);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel2.Controls.Add(this.label7);
            this.panel2.Location = new System.Drawing.Point(11, 17);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(327, 32);
            this.panel2.TabIndex = 820;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 18F);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(117, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 29);
            this.label7.TabIndex = 0;
            this.label7.Text = "Wafer ID";
            // 
            // InterfaceChart
            // 
            chartArea2.Name = "ChartArea1";
            this.InterfaceChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.InterfaceChart.Legends.Add(legend2);
            this.InterfaceChart.Location = new System.Drawing.Point(353, 55);
            this.InterfaceChart.Name = "InterfaceChart";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.RangeBar;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            series2.YValuesPerPoint = 2;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            this.InterfaceChart.Series.Add(series2);
            this.InterfaceChart.Size = new System.Drawing.Size(1216, 628);
            this.InterfaceChart.TabIndex = 819;
            this.InterfaceChart.Text = "chart1";
            this.InterfaceChart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.InterfaceChart_MouseClick);
            // 
            // FormInterfaceData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1920, 1048);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.HistoryPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormInterfaceData";
            this.Text = "FormHistoryData";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.HistoryPanel.ResumeLayout(false);
            this.HistoryPanel.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InterfaceChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_DataHistory;
        private System.Windows.Forms.Panel HistoryPanel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtEndTime;
        private System.Windows.Forms.TextBox txtStartTime;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox DataListBox;
        private System.Windows.Forms.Button btSetfirstPage;
        private System.Windows.Forms.Button btSetLastPage;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.TextBox tbSearchText;
        private System.Windows.Forms.ComboBox CBO_SELECT_DATE;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CBO_SELECT_MONTH;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btNext10Page;
        private System.Windows.Forms.Button btNextPage;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataVisualization.Charting.Chart InterfaceChart;
        private System.Windows.Forms.Button btBefore10page;
        private System.Windows.Forms.Label lbCurrentPage;
        private System.Windows.Forms.Button btnBeforePage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtElapsedTime;
    }
}