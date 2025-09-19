
namespace Core.UI
{
    partial class FormTopScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTopScreen));
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnHistoryPage = new System.Windows.Forms.Button();
            this.BtnTeachPage = new System.Windows.Forms.Button();
            this.btn_LightReset = new System.Windows.Forms.Button();
            this.pnl_DriveD = new System.Windows.Forms.Panel();
            this.lbl_DriveD = new System.Windows.Forms.Label();
            this.BtnParamPage = new System.Windows.Forms.Button();
            this.pnl_CPU = new System.Windows.Forms.Panel();
            this.lbl_CPU = new System.Windows.Forms.Label();
            this.BtnSettingPage = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.BtnMainPage = new System.Windows.Forms.Button();
            this.pnl_DriveC = new System.Windows.Forms.Panel();
            this.lbl_DriveC = new System.Windows.Forms.Label();
            this.pnl_Memory = new System.Windows.Forms.Panel();
            this.lbl_Memory = new System.Windows.Forms.Label();
            this.button11 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblSwVersion = new System.Windows.Forms.Label();
            this.TimerUI = new System.Windows.Forms.Timer(this.components);
            this.ImgList = new System.Windows.Forms.ImageList(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.pnl_DriveD.SuspendLayout();
            this.pnl_CPU.SuspendLayout();
            this.pnl_DriveC.SuspendLayout();
            this.pnl_Memory.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.BtnHistoryPage);
            this.panel1.Controls.Add(this.BtnTeachPage);
            this.panel1.Controls.Add(this.btn_LightReset);
            this.panel1.Controls.Add(this.pnl_DriveD);
            this.panel1.Controls.Add(this.BtnParamPage);
            this.panel1.Controls.Add(this.pnl_CPU);
            this.panel1.Controls.Add(this.BtnSettingPage);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.BtnMainPage);
            this.panel1.Controls.Add(this.pnl_DriveC);
            this.panel1.Controls.Add(this.pnl_Memory);
            this.panel1.Controls.Add(this.button11);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblDate);
            this.panel1.Controls.Add(this.lblTime);
            this.panel1.Location = new System.Drawing.Point(12, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1080, 48);
            this.panel1.TabIndex = 782;
            // 
            // BtnHistoryPage
            // 
            this.BtnHistoryPage.BackColor = System.Drawing.Color.Transparent;
            this.BtnHistoryPage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnHistoryPage.BackgroundImage")));
            this.BtnHistoryPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnHistoryPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.BtnHistoryPage.FlatAppearance.BorderSize = 0;
            this.BtnHistoryPage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.BtnHistoryPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.BtnHistoryPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnHistoryPage.ForeColor = System.Drawing.Color.Transparent;
            this.BtnHistoryPage.Location = new System.Drawing.Point(740, 4);
            this.BtnHistoryPage.Name = "BtnHistoryPage";
            this.BtnHistoryPage.Size = new System.Drawing.Size(167, 40);
            this.BtnHistoryPage.TabIndex = 787;
            this.BtnHistoryPage.UseVisualStyleBackColor = false;
            this.BtnHistoryPage.Click += new System.EventHandler(this.BtnPageClick);
            this.BtnHistoryPage.MouseEnter += new System.EventHandler(this.BtnPageEnter);
            this.BtnHistoryPage.MouseLeave += new System.EventHandler(this.BtnPageLeave);
            // 
            // BtnTeachPage
            // 
            this.BtnTeachPage.BackColor = System.Drawing.Color.Transparent;
            this.BtnTeachPage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnTeachPage.BackgroundImage")));
            this.BtnTeachPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnTeachPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.BtnTeachPage.FlatAppearance.BorderSize = 0;
            this.BtnTeachPage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.BtnTeachPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.BtnTeachPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTeachPage.ForeColor = System.Drawing.Color.Transparent;
            this.BtnTeachPage.Location = new System.Drawing.Point(560, 4);
            this.BtnTeachPage.Name = "BtnTeachPage";
            this.BtnTeachPage.Size = new System.Drawing.Size(167, 40);
            this.BtnTeachPage.TabIndex = 786;
            this.BtnTeachPage.UseVisualStyleBackColor = false;
            this.BtnTeachPage.Click += new System.EventHandler(this.BtnPageClick);
            this.BtnTeachPage.MouseEnter += new System.EventHandler(this.BtnPageEnter);
            this.BtnTeachPage.MouseLeave += new System.EventHandler(this.BtnPageLeave);
            // 
            // btn_LightReset
            // 
            this.btn_LightReset.Font = new System.Drawing.Font("GalanoGrotesque-SemiBold", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_LightReset.Location = new System.Drawing.Point(1361, 7);
            this.btn_LightReset.Name = "btn_LightReset";
            this.btn_LightReset.Size = new System.Drawing.Size(89, 23);
            this.btn_LightReset.TabIndex = 811;
            this.btn_LightReset.Text = "Light Reset";
            this.btn_LightReset.UseVisualStyleBackColor = true;
            this.btn_LightReset.Visible = false;
            this.btn_LightReset.Click += new System.EventHandler(this.btn_LightReset_Click);
            // 
            // pnl_DriveD
            // 
            this.pnl_DriveD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.pnl_DriveD.Controls.Add(this.lbl_DriveD);
            this.pnl_DriveD.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnl_DriveD.Location = new System.Drawing.Point(1577, 32);
            this.pnl_DriveD.Name = "pnl_DriveD";
            this.pnl_DriveD.Size = new System.Drawing.Size(56, 27);
            this.pnl_DriveD.TabIndex = 810;
            this.pnl_DriveD.Visible = false;
            // 
            // lbl_DriveD
            // 
            this.lbl_DriveD.AutoSize = true;
            this.lbl_DriveD.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_DriveD.ForeColor = System.Drawing.Color.White;
            this.lbl_DriveD.Location = new System.Drawing.Point(19, 5);
            this.lbl_DriveD.Name = "lbl_DriveD";
            this.lbl_DriveD.Size = new System.Drawing.Size(17, 16);
            this.lbl_DriveD.TabIndex = 1;
            this.lbl_DriveD.Text = "D";
            this.lbl_DriveD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnParamPage
            // 
            this.BtnParamPage.BackColor = System.Drawing.Color.Transparent;
            this.BtnParamPage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnParamPage.BackgroundImage")));
            this.BtnParamPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnParamPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.BtnParamPage.FlatAppearance.BorderSize = 0;
            this.BtnParamPage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.BtnParamPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.BtnParamPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnParamPage.ForeColor = System.Drawing.Color.Transparent;
            this.BtnParamPage.Location = new System.Drawing.Point(380, 4);
            this.BtnParamPage.Name = "BtnParamPage";
            this.BtnParamPage.Size = new System.Drawing.Size(167, 40);
            this.BtnParamPage.TabIndex = 785;
            this.BtnParamPage.UseVisualStyleBackColor = false;
            this.BtnParamPage.Click += new System.EventHandler(this.BtnPageClick);
            this.BtnParamPage.MouseEnter += new System.EventHandler(this.BtnPageEnter);
            this.BtnParamPage.MouseLeave += new System.EventHandler(this.BtnPageLeave);
            // 
            // pnl_CPU
            // 
            this.pnl_CPU.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.pnl_CPU.Controls.Add(this.lbl_CPU);
            this.pnl_CPU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.pnl_CPU.Location = new System.Drawing.Point(1456, 5);
            this.pnl_CPU.Name = "pnl_CPU";
            this.pnl_CPU.Size = new System.Drawing.Size(56, 27);
            this.pnl_CPU.TabIndex = 807;
            this.pnl_CPU.Visible = false;
            // 
            // lbl_CPU
            // 
            this.lbl_CPU.AutoSize = true;
            this.lbl_CPU.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CPU.ForeColor = System.Drawing.Color.White;
            this.lbl_CPU.Location = new System.Drawing.Point(2, 6);
            this.lbl_CPU.Name = "lbl_CPU";
            this.lbl_CPU.Size = new System.Drawing.Size(44, 16);
            this.lbl_CPU.TabIndex = 1;
            this.lbl_CPU.Text = "   CPU";
            this.lbl_CPU.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnSettingPage
            // 
            this.BtnSettingPage.BackColor = System.Drawing.Color.Transparent;
            this.BtnSettingPage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnSettingPage.BackgroundImage")));
            this.BtnSettingPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.BtnSettingPage.FlatAppearance.BorderSize = 0;
            this.BtnSettingPage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.BtnSettingPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.BtnSettingPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSettingPage.ForeColor = System.Drawing.Color.Transparent;
            this.BtnSettingPage.Location = new System.Drawing.Point(200, 4);
            this.BtnSettingPage.Name = "BtnSettingPage";
            this.BtnSettingPage.Size = new System.Drawing.Size(167, 40);
            this.BtnSettingPage.TabIndex = 793;
            this.BtnSettingPage.UseVisualStyleBackColor = false;
            this.BtnSettingPage.Click += new System.EventHandler(this.BtnPageClick);
            this.BtnSettingPage.MouseEnter += new System.EventHandler(this.BtnPageEnter);
            this.BtnSettingPage.MouseLeave += new System.EventHandler(this.BtnPageLeave);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("GalanoGrotesque-SemiBold", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(1280, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 789;
            this.button1.Text = "Light TRG";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnMainPage
            // 
            this.BtnMainPage.BackColor = System.Drawing.Color.Transparent;
            this.BtnMainPage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnMainPage.BackgroundImage")));
            this.BtnMainPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.BtnMainPage.FlatAppearance.BorderSize = 0;
            this.BtnMainPage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.BtnMainPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.BtnMainPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMainPage.ForeColor = System.Drawing.Color.Transparent;
            this.BtnMainPage.Location = new System.Drawing.Point(20, 4);
            this.BtnMainPage.Name = "BtnMainPage";
            this.BtnMainPage.Size = new System.Drawing.Size(167, 40);
            this.BtnMainPage.TabIndex = 792;
            this.BtnMainPage.UseVisualStyleBackColor = false;
            this.BtnMainPage.Click += new System.EventHandler(this.BtnPageClick);
            this.BtnMainPage.MouseEnter += new System.EventHandler(this.BtnPageEnter);
            this.BtnMainPage.MouseLeave += new System.EventHandler(this.BtnPageLeave);
            // 
            // pnl_DriveC
            // 
            this.pnl_DriveC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.pnl_DriveC.Controls.Add(this.lbl_DriveC);
            this.pnl_DriveC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.pnl_DriveC.Location = new System.Drawing.Point(1577, 5);
            this.pnl_DriveC.Name = "pnl_DriveC";
            this.pnl_DriveC.Size = new System.Drawing.Size(56, 27);
            this.pnl_DriveC.TabIndex = 809;
            this.pnl_DriveC.Visible = false;
            // 
            // lbl_DriveC
            // 
            this.lbl_DriveC.AutoSize = true;
            this.lbl_DriveC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_DriveC.ForeColor = System.Drawing.Color.White;
            this.lbl_DriveC.Location = new System.Drawing.Point(19, 5);
            this.lbl_DriveC.Name = "lbl_DriveC";
            this.lbl_DriveC.Size = new System.Drawing.Size(16, 16);
            this.lbl_DriveC.TabIndex = 1;
            this.lbl_DriveC.Text = "C";
            this.lbl_DriveC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnl_Memory
            // 
            this.pnl_Memory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.pnl_Memory.Controls.Add(this.lbl_Memory);
            this.pnl_Memory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.pnl_Memory.Location = new System.Drawing.Point(1456, 32);
            this.pnl_Memory.Name = "pnl_Memory";
            this.pnl_Memory.Size = new System.Drawing.Size(56, 27);
            this.pnl_Memory.TabIndex = 808;
            this.pnl_Memory.Visible = false;
            // 
            // lbl_Memory
            // 
            this.lbl_Memory.AutoSize = true;
            this.lbl_Memory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Memory.ForeColor = System.Drawing.Color.White;
            this.lbl_Memory.Location = new System.Drawing.Point(2, 5);
            this.lbl_Memory.Name = "lbl_Memory";
            this.lbl_Memory.Size = new System.Drawing.Size(46, 16);
            this.lbl_Memory.TabIndex = 1;
            this.lbl_Memory.Text = "   Mem";
            this.lbl_Memory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.Color.Black;
            this.button11.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button11.BackgroundImage")));
            this.button11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Font = new System.Drawing.Font("GalanoGrotesque-SemiBold", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button11.Location = new System.Drawing.Point(1298, 23);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(152, 42);
            this.button11.TabIndex = 802;
            this.button11.Text = "Firmware : v1.1.63";
            this.button11.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("GalanoGrotesque-SemiBold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(1210, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(362, 37);
            this.label1.TabIndex = 774;
            this.label1.Text = "Connected Insight Inspection";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Font = new System.Drawing.Font("GalanoGrotesque-Medium", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.Color.White;
            this.lblDate.Location = new System.Drawing.Point(914, -3);
            this.lblDate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(145, 25);
            this.lblDate.TabIndex = 772;
            this.lblDate.Text = "2022-01-01 (Mon)";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.Font = new System.Drawing.Font("GalanoGrotesque-Medium", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Location = new System.Drawing.Point(915, 17);
            this.lblTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(85, 29);
            this.lblTime.TabIndex = 771;
            this.lblTime.Text = "12:30:59";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExit.BackgroundImage")));
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExit.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.ForeColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(1106, 6);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(48, 48);
            this.btnExit.TabIndex = 781;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.OnClose);
            this.btnExit.MouseEnter += new System.EventHandler(this.BtnPageEnter);
            this.btnExit.MouseLeave += new System.EventHandler(this.BtnPageLeave);
            // 
            // lblSwVersion
            // 
            this.lblSwVersion.AutoSize = true;
            this.lblSwVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblSwVersion.Font = new System.Drawing.Font("Calibri", 12F);
            this.lblSwVersion.ForeColor = System.Drawing.Color.White;
            this.lblSwVersion.Location = new System.Drawing.Point(529, 247);
            this.lblSwVersion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSwVersion.Name = "lblSwVersion";
            this.lblSwVersion.Size = new System.Drawing.Size(82, 19);
            this.lblSwVersion.TabIndex = 772;
            this.lblSwVersion.Text = "SW-Version";
            // 
            // TimerUI
            // 
            this.TimerUI.Tick += new System.EventHandler(this.OnClock);
            // 
            // ImgList
            // 
            this.ImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImgList.ImageStream")));
            this.ImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImgList.Images.SetKeyName(0, "UnSelectedMain.png");
            this.ImgList.Images.SetKeyName(1, "SelectedMain.png");
            this.ImgList.Images.SetKeyName(2, "RolloverMain.png");
            this.ImgList.Images.SetKeyName(3, "UnSelectedSetting.png");
            this.ImgList.Images.SetKeyName(4, "SelectedSetting.png");
            this.ImgList.Images.SetKeyName(5, "RolloverSetting.png");
            this.ImgList.Images.SetKeyName(6, "UnSelectedParamater.png");
            this.ImgList.Images.SetKeyName(7, "SelectedParamater.png");
            this.ImgList.Images.SetKeyName(8, "RolloverParamater.png");
            this.ImgList.Images.SetKeyName(9, "UnSelectedTeach.png");
            this.ImgList.Images.SetKeyName(10, "SelectedTeach.png");
            this.ImgList.Images.SetKeyName(11, "RolloverTeach.png");
            this.ImgList.Images.SetKeyName(12, "UnSelectedHistory.png");
            this.ImgList.Images.SetKeyName(13, "SelectedHistory.png");
            this.ImgList.Images.SetKeyName(14, "RolloverHistory.png");
            this.ImgList.Images.SetKeyName(15, "Unselected.png");
            this.ImgList.Images.SetKeyName(16, "Rollover.png");
            this.ImgList.Images.SetKeyName(17, "Rollover.png");
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1920, 71);
            this.panel2.TabIndex = 783;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1292, 23);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 784;
            this.button2.Text = "Simul";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(1739, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(169, 56);
            this.pictureBox1.TabIndex = 783;
            this.pictureBox1.TabStop = false;
            // 
            // FormTopScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1920, 268);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblSwVersion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormTopScreen";
            this.Text = "FormTopScreen";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnl_DriveD.ResumeLayout(false);
            this.pnl_DriveD.PerformLayout();
            this.pnl_CPU.ResumeLayout(false);
            this.pnl_CPU.PerformLayout();
            this.pnl_DriveC.ResumeLayout(false);
            this.pnl_DriveC.PerformLayout();
            this.pnl_Memory.ResumeLayout(false);
            this.pnl_Memory.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblSwVersion;
        private System.Windows.Forms.Timer TimerUI;
        private System.Windows.Forms.ImageList ImgList;
        public System.Windows.Forms.Button BtnHistoryPage;
        public System.Windows.Forms.Button BtnTeachPage;
        public System.Windows.Forms.Button BtnParamPage;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button BtnMainPage;
        public System.Windows.Forms.Button BtnSettingPage;
        private System.Windows.Forms.Panel pnl_CPU;
        private System.Windows.Forms.Label lbl_CPU;
        private System.Windows.Forms.Panel pnl_Memory;
        private System.Windows.Forms.Label lbl_Memory;
        private System.Windows.Forms.Panel pnl_DriveD;
        private System.Windows.Forms.Label lbl_DriveD;
        private System.Windows.Forms.Panel pnl_DriveC;
        private System.Windows.Forms.Label lbl_DriveC;
        private System.Windows.Forms.Button btn_LightReset;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button2;
    }
}