using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.UI
{
    public partial class FormParameterSystemScreen : Form
    {
        //private CSystemData SystemData;

        public FormParameterSystemScreen()
        {
            InitializeComponent();
        }
        private void FormParameterSystemScreen_Enter(object sender, EventArgs e)
        {
            /*220511 LYK 임시 주석
             
            //SystemData = ObjectExtensions.Copy(CMainFrame.DataManager.SystemData);

            // 이미지 자동삭제 모드 Load ( 0 : Day, 1 : Capacity)
            if (SystemData.DeleteMode == 0)
                rb_Day.Checked = true;
            else if (SystemData.DeleteMode == 1)
                rb_Capacity.Checked = true;

            // 자동삭제 값 Load
            txt_DayValue.Text = Convert.ToString(SystemData.DayValue);
            txt_CapacityValue.Text = Convert.ToString(SystemData.CapacityValue);

            // 판정에 따른 이미지 저장 Load ( 0 : All, 1 : NG)
            if (SystemData.SaveResultMode == 0)
                rb_All.Checked = true;
            else if (SystemData.SaveResultMode == 1)
                rb_NG.Checked = true;

            */
        }

        // Parameter Save
        private void btn_Save_Click(object sender, EventArgs e)
        {
            /*220511 LYK 임시 주석

            // 이미지 자동삭제 모드 설정 ( 0 : Day, 1 : Capacity)
            if (rb_Day.Checked == true)
                SystemData.DeleteMode = 0;
            else if (rb_Capacity.Checked == true)
                SystemData.DeleteMode = 1;

            // 자동삭제 값 저장
            SystemData.DayValue = Convert.ToInt16(txt_DayValue.Text); // Day
            SystemData.CapacityValue = Convert.ToInt16(txt_CapacityValue.Text); // Capacity

            // 판정에 따른 이미지 저장 ( 0 : All, 1 : NG)
            if (rb_All.Checked == true)
                SystemData.SaveResultMode = 0;
            else if (rb_NG.Checked == true)
                SystemData.SaveResultMode = 1;

            // 저장
            //CMainFrame.mCore.SaveSystemData(SystemData);
            */
        }

    }
}
