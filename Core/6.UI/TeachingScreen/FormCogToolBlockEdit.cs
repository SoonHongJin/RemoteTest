using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Core.Program;

using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;

namespace Core.UI
{
    public partial class FormCogToolBlock : Form
    {
        private int m_nSelectCam = 0;
        private CogToolBlock m_ToolBlock = null;

        public FormCogToolBlock()
        {
            InitializeComponent();
        }

        public void OnShow(CogToolBlock _ToolBlock, int _SelectCam)
        {

            m_ToolBlock = (CogToolBlock)CogSerializer.DeepCopyObject(_ToolBlock);
            m_nSelectCam = _SelectCam;

            cogToolBlockEdit.Subject = m_ToolBlock;

            this.Show();
        }

        private void btn_Apply_Click(object sender, EventArgs e)
        {
            theRecipe.m_CogInspToolBlock[m_nSelectCam] = (CogToolBlock)CogSerializer.DeepCopyObject(m_ToolBlock);
        }
    }
}
