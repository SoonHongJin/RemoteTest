
namespace Core.Utility
{
    partial class CHistoryImageControl
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.View = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.View)).BeginInit();
            this.SuspendLayout();
            // 
            // View
            // 
            this.View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.View.Location = new System.Drawing.Point(0, 0);
            this.View.Name = "View";
            this.View.Size = new System.Drawing.Size(550, 508);
            this.View.TabIndex = 4;
            this.View.TabStop = false;
            this.View.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            this.View.DoubleClick += new System.EventHandler(this.View_MouseDoubleClick);
            this.View.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.View.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            this.View.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            this.View.Resize += new System.EventHandler(this.OnSize);
            // 
            // CHistoryImageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.View);
            this.Name = "CHistoryImageControl";
            this.Size = new System.Drawing.Size(550, 508);
            ((System.ComponentModel.ISupportInitialize)(this.View)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox View;
    }
}
