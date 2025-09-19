﻿using System;
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
    public partial class FormKeyPad : Form
    {
        string strInput = "";
        string strCurrent = "";

        public FormKeyPad()
        {
            SetValue("");

            InitializeComponent();

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.Fixed3D;

            CUtils.AnimateEffect.AnimateWindow(this.Handle, 300, CUtils.AnimateEffect.AW_ACTIVATE | CUtils.AnimateEffect.AW_BLEND);
        }

        public void SetValue(string strValue)
        {
            strCurrent = strValue;
        }

        private void BtnNo_Click(object sender, EventArgs e)
        {
            double dTag = 0;
            Button Btn = sender as Button;

            dTag = Convert.ToDouble(Btn.Tag);

            strInput = strInput + Convert.ToDouble(dTag);

            UpdateDisplay(strInput);
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            int nNo = 0;

            if (strInput == "")
                return;

            nNo = strInput.Length - 1;
            strInput = strInput.Remove(nNo, 1);
            UpdateDisplay(strInput);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            strInput = "";
            UpdateDisplay(strInput);
        }

        private void UpdateDisplay(string strNo)
        {
            ModifyNo.Text = strNo;
        }

        private void BtnComma_Click(object sender, EventArgs e)
        {

            if (strInput.Contains(".")) return;

            if (strInput != "")
            {
                strInput = strInput + ".";
            }

            UpdateDisplay(strInput);
        }

        private void BtnSign_Click(object sender, EventArgs e)
        {
            if (strInput == "") return;
            double nNo = 0;

            nNo = Convert.ToDouble(strInput);

            if (nNo > 0)
            {
                strInput = "-" + strInput;
            }
            else if (nNo < 0)
            {
                strInput = strInput.Replace("-", "");
            }

            UpdateDisplay(strInput);
        }

        private void FormKeyPad_Load(object sender, EventArgs e)
        {
            PresentNo.Text = strCurrent;
            //ModifyNo.Text = strCurrent;
            //strInput = strCurrent;
        }
    }
}
