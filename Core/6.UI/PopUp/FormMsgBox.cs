using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static Core.DEF_Common;

namespace Core.UI
{
    public partial class FormMsgBox : Form
    {

        private CMessageInfo MsgInfo = new CMessageInfo();
        private bool IsUpdated = false;

        public FormMsgBox()
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

        public void SetMessage(string strMsg, EMessageType type = EMessageType.NONE)
        {      
            //MsgInfo.Message[(int)ELanguage.ENGLISH] = strMsg;
            TextEng.Text = strMsg;

            if (type != EMessageType.NONE) MsgInfo.Type = type;
            BtnOK.Text = "OK";
        }


        private void FormUtilMsg_Load(object sender, EventArgs e)
        {
            //TextEng.Text = MsgInfo.GetMessage(ELanguage.ENGLISH);
            //this.Text = $"Message : {MsgInfo.Index}";

            BtnOK.Visible = true;
            BtnNo.Visible = true;

            switch (MsgInfo.Type)
            {
                case EMessageType.OK:
                    BtnNo.Visible = false; BtnOK.Text = "OK"; BtnNo.Text = "No";
                    break;

                case EMessageType.OK_CANCEL:
                    BtnNo.Visible = true; BtnOK.Text = "OK"; BtnNo.Text = "Cancel";
                    break;

                case EMessageType.CONFIRM_CANCEL:
                    BtnNo.Visible = true; BtnOK.Text = "Confirm"; BtnNo.Text = "Canel";
                    break;
            }

            timer1.Enabled = true;
            timer1.Start();
        }


        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            timer1.Enabled = false;
            this.Close();
        }
    }
}
