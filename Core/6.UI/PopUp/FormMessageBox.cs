using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Core.Layers;

using static Core.Layers.DEF_Common;

using static Core.Layers.MDataManager;

using static Core.Layers.DEF_DataManager;

using static Core.Layers.DEF_Error;

namespace Core.UI
{
    public partial class FormMessageBox : Form
    {
        private CMessageInfo MsgInfo = new CMessageInfo();
        private bool IsUpdated = false;

        public FormMessageBox()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.Fixed3D;
        }

        /// <summary>
        /// Message Index Number를 이용해서 MessageInfo 를 Loading
        /// index를 이용해서 호출하면, 실제 프로그램 코드에선 알아보기가 힘들것 같아서
        /// 일부러 주석처리하고 사용하지 않기로 함
        /// </summary>
        /// <param name="index"></param>
        public void SetMessage(int index)
        {
            MsgInfo.Index = index;
            int iResult = CMainFrame.DataManager.LoadMessageInfo(index, out MsgInfo);
            if (iResult != SUCCESS) BtnSave.Text = "Add New";
            else BtnSave.Text = "Update";
        }


        public void SetMessage(string strMsg, EMessageType type = EMessageType.NONE)
        {
            int iResult = CMainFrame.DataManager.LoadMessageInfo(strMsg, out MsgInfo);
            if (iResult != SUCCESS)
            {
                MsgInfo.Message[(int)ELanguage.ENGLISH] = strMsg;
                MsgInfo.Message[(int)MeCore.Language] = strMsg;
                if(type != EMessageType.NONE) MsgInfo.Type = type;
                BtnSave.Text = "Add New";
            } else
            {
                if(type != EMessageType.NONE) MsgInfo.Type = type;
                BtnSave.Text = "Save";
            }
        }

        private void FormUtilMsg_Load(object sender, EventArgs e)
        {
            TextEng.Text = MsgInfo.GetMessage(ELanguage.ENGLISH);
            TextSystem.Text = MsgInfo.GetMessage(MeCore.Language);
            this.Text = $"Message : {MsgInfo.Index}";

            if(MeCore.Language == ELanguage.ENGLISH)
            {
                Label_System.Visible = false;
                TextSystem.Visible = false;
            }

            BtnConfirm.Visible = true;
            BtnCancel.Text = "Cancel";
            switch (MsgInfo.Type)
            {
                case EMessageType.OK:
                    BtnCancel.Visible = false; BtnConfirm.Text = "OK";
                    break;

                case EMessageType.OK_CANCEL:
                    BtnCancel.Visible = true; BtnConfirm.Text = "OK";
                    break;

                case EMessageType.CONFIRM_CANCEL:
                    BtnCancel.Visible = true; BtnConfirm.Text = "Confirm";
                    break;
            }

            timer1.Start();
        }
       
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if(IsUpdated == true)
            {
                if (MessageBox.Show("Message is updated, but not updated. is it ok?", "Confirm", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            MsgInfo.Message[(int)ELanguage.ENGLISH] = TextEng.Text;
            MsgInfo.Message[(int)MeCore.Language] = TextSystem.Text;
            CMainFrame.DataManager.UpdateMessageInfo(MsgInfo);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (IsUpdated == true)
            {
                if (MessageBox.Show("Message is updated, but not updated. is it ok?", "Confirm", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    return;
            }

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(TextEng.Text != MsgInfo.GetMessage() 
                || TextSystem.Text != MsgInfo.GetMessage(MeCore.Language))
            {
                BtnSave.Visible = true;
                IsUpdated = true;
            } else
            {
                BtnSave.Visible = false;
                IsUpdated = false;
            }
        }
    }
}
