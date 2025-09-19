
namespace Core
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.DisplayPanel = new System.Windows.Forms.Panel();
            this.TitlePanel = new System.Windows.Forms.Panel();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.SidePanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // DisplayPanel
            // 
            this.DisplayPanel.Location = new System.Drawing.Point(0, 63);
            this.DisplayPanel.Name = "DisplayPanel";
            this.DisplayPanel.Size = new System.Drawing.Size(1725, 917);
            this.DisplayPanel.TabIndex = 12;
            // 
            // TitlePanel
            // 
            this.TitlePanel.BackColor = System.Drawing.Color.White;
            this.TitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitlePanel.Location = new System.Drawing.Point(0, 0);
            this.TitlePanel.Name = "TitlePanel";
            this.TitlePanel.Size = new System.Drawing.Size(1920, 63);
            this.TitlePanel.TabIndex = 11;
            // 
            // BottomPanel
            // 
            this.BottomPanel.Location = new System.Drawing.Point(0, 980);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(1920, 100);
            this.BottomPanel.TabIndex = 14;
            // 
            // SidePanel
            // 
            this.SidePanel.Location = new System.Drawing.Point(1725, 63);
            this.SidePanel.Name = "SidePanel";
            this.SidePanel.Size = new System.Drawing.Size(195, 917);
            this.SidePanel.TabIndex = 13;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.BottomPanel);
            this.Controls.Add(this.SidePanel);
            this.Controls.Add(this.DisplayPanel);
            this.Controls.Add(this.TitlePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel DisplayPanel;
        private System.Windows.Forms.Panel TitlePanel;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Panel SidePanel;
    }
}

