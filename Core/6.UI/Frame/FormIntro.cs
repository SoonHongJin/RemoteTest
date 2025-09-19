using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core.Utility;

namespace Core.UI
{
    public partial class FormIntro : Form
    {
        private CProgressBar ProcBar = new CProgressBar();
        public FormIntro()
        {
            InitializeComponent();
            //CUtils.AnimateEffect.AnimateWindow(this.Handle, 3000, CUtils.AnimateEffect.AW_ACTIVATE | CUtils.AnimateEffect.AW_BLEND);
            ProcBar = new CProgressBar();
            ProcBar.Width = DEF_UI.ProgressWidth - 5;
            ProcBar.Height = DEF_UI.ProgressHeight;
            ProcBar.Location = new Point(DEF_UI.ProgressPoint[0].X + 5, DEF_UI.ProgressPoint[0].Y);
            //ProcBar.Style = ProgressBarStyle.Continuous;
            Controls.Add(ProcBar);

        }

        public void SetStatus(string strText, int nProgress)
        {
            ProcBar.Value = nProgress;
            ProcBar.sValue = $"{strText} + {nProgress.ToString()}";
            ProcBar.ProgressColor = Color.FromArgb(69, 90, 230);
        }
    }
}
