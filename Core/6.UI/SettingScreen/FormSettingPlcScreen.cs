using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
//using System.Globalization;

using Core.Utility;
using Core.Process;
using static Core.Program;
using Core.DataProcess;

namespace Core.UI
{
    public partial class FormSettingPlcScreen : Form
    {
        private MainForm MainForm;
        private CInterfacePLC InterPLC = null;      //240412 LYK InterfacePLC Instance
        private int[] iLabelCnt = new int[] { 20, 9, 14 };

        //250122 NWT Parameter Exception
        private List<CExceptionData> exceptionDataList = new List<CExceptionData>();
        private ParameterExceptionHandler parameterExceptionHandler = null;
        private CLogger Logger = null;
        public FormSettingPlcScreen(MainForm _MainForm, CInterfacePLC _InterPLC, CLogger _logger)
        {
            InitializeComponent();
            TopLevel = false;
            MainForm = _MainForm;
            Logger = _logger;
            exceptionDataList = theRecipe.m_listExceptionData;
            InitializeForm();
            MainForm.SetEachControlResize(this);    //240801 NIS Conrol Resize

            InterPLC = _InterPLC;
            InterPLC.RefreshPLCReadBitData = RfreshPLCBitData;

            SetUIData();

            RefreshDeviceStatus();
        }

        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.MAIN_POS_X, DEF_UI.MAIN_POS_Y);
            this.Size = new Size(DEF_UI.MAIN_SIZE_WIDTH, DEF_UI.MAIN_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void SetUIData()
        {
            for (int i = 1; i <= iLabelCnt[0]; i++)  //lbl_Read_1 ~ lbl_Read_20까지 주소 출력
            {
                Label lbl = Controls.Find($"lbl_Read_{i}", true)[0] as Label;
                lbl.Text = lbl.Text + '\n' + theRecipe.PLCAddress.ReadAddress[i-1];
                lbl.TextAlign = ContentAlignment.MiddleRight;
            }

            PLCLabelLocationX_Align();

        }

        private void FormSettingPlcScreen_Load(object sender, EventArgs e)
        {
            RefreshDeviceStatus();
        }

        private void RefreshDeviceStatus()
        {
            btn_PLCStatus.BackColor = InterPLC.m_PLC.GetPLCConnectedCheck() == true ? Color.Lime : Color.Red;
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            theMainSystem.InterfacePLC.m_PLC.m_bDisconnect = false;
            theMainSystem.InterfacePLC.Reconnect();
            if(MainForm.PLC_HandShake_Start != null)
                MainForm.PLC_HandShake_Start.Invoke();
            RefreshDeviceStatus();
        }

        private void btn_Disconnect_Click(object sender, EventArgs e)
        {
            theMainSystem.InterfacePLC.m_PLC.m_bDisconnect = true;
            theMainSystem.InterfacePLC.Disconnect();
            RefreshDeviceStatus();
        }
        
        //240902 NIS PLC UI 갱신 적용
        private void RfreshPLCBitData(string Data, string sType)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    string name = sType.Split('_')[1];
                    if (name.Contains("ProcCarrier"))
                        name = name.Replace("Proc", "");

                    this.Controls.Find($"txt_{name}", true)[0].Text = Data;
                }));
            }
            catch(Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"RefreshPLCBitData Error : {e.Message}");
            }
        }

        //240902 NIS PLC UI 실시간 갱신
        private void checkBox_PLCLiveActivation_CheckedChanged(object sender, EventArgs e)
        {
            DEF_SYSTEM.REALTIME_COMMUNICATION_ACTIVATION = checkBox_PLCLiveActivation.Checked;
        }

        private void btn_PLCWrite_SetW(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string str = btn.Name.Split('_')[2];

            short nValue = short.Parse(this.Controls.Find($"txt_Write{str}", true)[0].Text);
            InterPLC.SetW($"WD_{str}", nValue);
        }

        private void btn_PLCWrite_SetQuality(object sender, EventArgs e)    //MONO COLOR JUDGE
        {
            Button btn = (Button)sender;
            string str = btn.Name.Split('_')[2];

            short nValue = short.Parse(this.Controls.Find($"txt_Write{str}", true)[0].Text);
            InterPLC.SetW($"WD_{str}", nValue);
        }
        private void btn_PLCWrite_SetValue(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string str = btn.Name.Split('_')[2];

            short nValue = (short)(double.Parse(this.Controls.Find($"txt_Write{str}", true)[0].Text) * 1000);
            InterPLC.SetW($"WD_{str}", nValue);
        }
        private void btn_PLCWrite_SetPos(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string str = btn.Name.Split('_')[2];

            short nValue = (short)(double.Parse(this.Controls.Find($"txt_Write{str}", true)[0].Text) * 1000);
            InterPLC.SetDW($"WD_{str}", nValue);
        }

        private void btn_WriteWaferID_Click(object sender, EventArgs e)
        {
            string WaferID = txt_WriteWaferID.Text;
            InterPLC.m_PLC.SetWaferID("WD_WaferID", 18, WaferID);
        }

        private void btn_FlagON_Click(object sender, EventArgs e)
        {
            string tempWriteDataName = cbb_WriDataList.SelectedItem.ToString();

            InterPLC.SetB(tempWriteDataName, 1);
        }

        private void btn_FlagOFF_Click(object sender, EventArgs e)
        {
            string tempWriteDataName = cbb_WriDataList.SelectedItem.ToString();

            InterPLC.SetB(tempWriteDataName, 255);
        }
        
        private void btn_WriteFlag_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                DomainUpDown domain;
                string address = btn.Name.Split('_')[2];

                domain = Controls.Find($"dm_Write_{address}", true)[0] as DomainUpDown;         //Flag domain 찾기

                if(btn.Name.Contains("BACK"))
                    InterPLC.SetB($"WB_{address}_BACK", short.Parse(domain.SelectedItem.ToString()));
                else
                    InterPLC.SetB($"WB_{address}", short.Parse(domain.SelectedItem.ToString()));

            }
            catch (Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"PLC WriteFlag Error : {ex.Message}");
                MessageBox.Show($"WriteFlag Error : {ex.Message}");
            }
        }
        private void PLCLabelLocationX_Align()  //241017 NIS 맨 위측의 Label 기준으로 정렬
        {
            Control[] controls;
            int ReadPLCAlignPointX = ReadPLCAlignPointX = lbl_Read_1.Location.X + lbl_Read_1.Width;
            int WriteFlagPLCAlignPointX = WriteFlagPLCAlignPointX = lbl_WriteFlag_1.Location.X + lbl_WriteFlag_1.Width;
            int WriteDataPLCAlignPointX = WriteDataPLCAlignPointX = lbl_WriteData_1.Location.X + lbl_WriteData_1.Width;

            //241017 NIS PLC Read Label Align
            for (int i = 1; i <= iLabelCnt[0]; i++)
            {
                Label lbl = Controls.Find($"lbl_Read_{i}", true)[0] as Label;
                lbl.Location = new Point(ReadPLCAlignPointX - lbl.Width, lbl.Location.Y);
            }

            //241017 NIS PLC Write Flag Label Align
            for (int i = 1; i <= iLabelCnt[1]; i++)
            {
                Label lbl = Controls.Find($"lbl_WriteFlag_{i}", true)[0] as Label;
                lbl.Location = new Point(WriteFlagPLCAlignPointX - lbl.Width, lbl.Location.Y);
            }

            //241017 NIS PLC Write Data Label Align
            for (int i = 1; i <= iLabelCnt[2]; i++)
            {
                Label lbl = Controls.Find($"lbl_WriteData_{i}", true)[0] as Label;
                lbl.Location = new Point(WriteDataPLCAlignPointX - lbl.Width, lbl.Location.Y);
            }

        }

        /// <summary>
        /// 25.01.22 NWT Parameter Click Event
        /// Parameter 클릭 시 keypad 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Click_Event(object sender, EventArgs e)
        {
            TextBox tempTxtBox = (TextBox)sender;
            string strCurrent = "", strModify = "";
            strCurrent = tempTxtBox.Text;

            if (!MainForm.GetKeyPad(strCurrent, out strModify))
                return;

            for (int i = 0; i < exceptionDataList.Count; i++)
            {
                if (exceptionDataList[i].Tag.Contains(tempTxtBox.Name))
                {
                    using (parameterExceptionHandler = new ParameterExceptionHandler(exceptionDataList[i].Page, exceptionDataList[i].Name, exceptionDataList[i].Tag, exceptionDataList[i].Min, exceptionDataList[i].Max, exceptionDataList[i].DataType, strModify))
                    {
                        tempTxtBox.Text = parameterExceptionHandler.CheckData() ? strModify : strCurrent;
                        break;
                    }
                }
            }
        }

        private void Click_Event_Keyboard(object sender, EventArgs e)
        {
            TextBox tempTxtBox = (TextBox)sender;
            string strCurrent = "", strModify = "";
            strCurrent = tempTxtBox.Text;

            if (!MainForm.GetKeyboard(out strModify, "Input Wafer ID", false))
                return;

            for (int i = 0; i < exceptionDataList.Count; i++)
            {
                if (exceptionDataList[i].Tag.Contains(tempTxtBox.Name))
                {
                    using (parameterExceptionHandler = new ParameterExceptionHandler(exceptionDataList[i].Page, exceptionDataList[i].Name, exceptionDataList[i].Tag, exceptionDataList[i].Min, exceptionDataList[i].Max, exceptionDataList[i].DataType, strModify))
                    {
                        tempTxtBox.Text = parameterExceptionHandler.CheckData() ? strModify : strCurrent;
                        break;
                    }
                }
            }
        }

        private string previousText = string.Empty; //250123 NWT 이전 파라미터 저장용 변수
        /// <summary>
        /// 25.01.23 NWT Textbox의 Text가 변경되기 전 Data 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void text_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                TextBox textBox = (TextBox)sender;
                previousText = textBox.Text;
            }
        }
        /// <summary>
        /// 25.01.23 NWT Parameter 값을 확인하여 기준값보다 크거나 작을경우 경고창 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textChanged(object sender, EventArgs e)    //250123 NWT 파라미터 값 확인
        {
            TextBox GetTextBox = (TextBox)sender;
            string tempText = GetTextBox.Text;
            for (int i = 0; i < exceptionDataList.Count; i++)
            {
                if (exceptionDataList[i].Tag.Contains(GetTextBox.Name))
                {
                    if (string.IsNullOrEmpty(tempText) == false)
                    {
                        using (parameterExceptionHandler = new ParameterExceptionHandler(exceptionDataList[i].Page, exceptionDataList[i].Name, exceptionDataList[i].Tag, exceptionDataList[i].Min, exceptionDataList[i].Max, exceptionDataList[i].DataType, tempText))
                        {
                            GetTextBox.Text = parameterExceptionHandler.CheckData() ? tempText : previousText;
                            break;
                        }
                    }
                }
            }
        }
    }

    
}