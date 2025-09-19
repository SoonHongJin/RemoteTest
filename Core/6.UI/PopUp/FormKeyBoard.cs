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
    public partial class FormKeyBoard : Form
    {
        public string InputString = "";
        bool SecretMode;

        public FormKeyBoard(string title, bool SecretMode = false)
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;

            MaximizeBox = false;
            MinimizeBox = false;

            CUtils.AnimateEffect.AnimateWindow(this.Handle, 300, CUtils.AnimateEffect.AW_ACTIVATE | CUtils.AnimateEffect.AW_BLEND);

            if (String.IsNullOrWhiteSpace(title)) BtnTitle.Visible = false;
            BtnTitle.Text = title;
            this.SecretMode = SecretMode;
        }

        private void BtnNo_Click(object sender, EventArgs e)
        {
            double dTag = 0;
            Button Btn = sender as Button;

            dTag = Convert.ToDouble(Btn.Tag);

            InputString = InputString + Convert.ToDouble(dTag);

            UpdateDisplay(InputString);
        }
        private void btn_Char_Click(object sender, EventArgs e)
        {
            Button Btn = sender as Button;
            InputString = InputString + Btn.Text;
            UpdateDisplay(InputString);

        }

        private void UpdateDisplay(string strNo)
        {
            string str = strNo;
            if (SecretMode)
            {
                str = "";
                for (int i = 0; i < strNo.Length; i++)
                    str += "*";
            }
            PresentNo.Text = str;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            int nNo = 0;

            if (InputString == "")
                return;

            nNo = InputString.Length - 1;
            InputString = InputString.Remove(nNo, 1);
            UpdateDisplay(InputString);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            InputString = "";
            UpdateDisplay(InputString);
        }
    }
}
