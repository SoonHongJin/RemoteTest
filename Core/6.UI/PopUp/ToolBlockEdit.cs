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

using Insnex.Vision2D.Core;
using Insnex.Vision2D.Common;
using Insnex.Vision2D.ToolBlock;
using Insnex.Vision2D.ImageProcessing;
using Insnex.Vision2D.Finder;
using Insnex.Vision2D.Intersection;
using Insnex.Vision2D.CalibFix;
using Insnex.Vision2D.Measurement;
using Insnex.ToolEditor.Controls;

namespace Core.UI
{
    public partial class ToolBlockEdit : Form
    {

        // 타이틀바를 이용 하여 Form 움직이기 
        private bool _IsMoving;
        Point fPt;

        public ToolBlockEdit(int inspectNum, CogToolBlock _ToolBlock = null, InsToolBlock _InsToolBlock = null)
        {
            InitializeComponent();
            if (DEF_SYSTEM.LICENSES_KEY == (int)License.COG)
            {
                cogToolBlockEdit.Subject = _ToolBlock;

            }
            else if (DEF_SYSTEM.LICENSES_KEY == (int)License.INS)
            {
                InsToolEditorControl insToolEditorControl = new InsToolEditorControl();
                pnl_ToolBlockEditor.Controls.Add(insToolEditorControl);
                insToolEditorControl.Dock = DockStyle.Fill;
                insToolEditorControl.Subject = _InsToolBlock;

                cogToolBlockEdit.Visible = false;
            }
            SetToolBlockEditPageName(inspectNum);
        }

        private void SetToolBlockEditPageName(int inspectNum)       //240703 NIS TeachMode에 따라서 타이틀 수정
        {

            switch(inspectNum)
            {
                case 0: //INSP_MODE.Crop
                    lbl_SetToolPageName.Text = "Crop Tool Edit Page";
                    break;
                case 1: //INSP_MODE.DeepLearning
                    //lbl_SetToolPageName.Text = "DeepLearning Tool Edit Page";
                    break;
                case 2: //INSP_MODE.Contour
                    lbl_SetToolPageName.Text = "Contour Tool Edit Page";
                    break;
                case 3: //INSP_MODE.Crack
                    lbl_SetToolPageName.Text = "Crack Tool Edit Page";
                    break;
                case 4: //INSP_MODE.EdgeToBusbar
                    lbl_SetToolPageName.Text = "EdgeToBusbar Tool Edit Page";
                    break;
                case 5: //Calib_MODE.Belt
                    lbl_SetToolPageName.Text = "Belt Calib Tool Edit Page";
                    break;
                case 6: //INSP_MODE.ColorROI
                    lbl_SetToolPageName.Text = "Color ROI Tool Edit Page";
                    break;
            }
        }

        //240703 NIS 마우스 드래그로 Tool창 이동 못하도록 이벤트 해제
        private void Panel_Title_MouseDown(object sender, MouseEventArgs e)
        {
            // Title Bar 눌렀을떄 움직이도록 
            _IsMoving = true;

            // 눌렀을때 위치 저장 
            fPt = new Point(e.X, e.Y);
        }

        private void Panel_Title_MouseMove(object sender, MouseEventArgs e)
        {
            // 왼쪽 버튼으로 눌렀을 경우에만 이동
            if(_IsMoving && (e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Point NewPoint = new Point(this.Left - (fPt.X - e.X), this.Top - (fPt.Y - e.Y));
                this.Location = NewPoint;
            }
        }

        private void Panel_Title_MouseUp(object sender, MouseEventArgs e)
        {
            // 마우스 버튼 땠을때 종료 
            _IsMoving = false;
        }

        private void btn_ToolBlock_Save_Click(object sender, EventArgs e)
        {
            theRecipe.ToolSave(theRecipe.m_sCurrentModelName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
