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
using static Core.DEF_Common;
using static Core.DEF_UI;
using Core.UI;
using Core;

namespace Core.UI
{
    public partial class DropMenu : Form
    {
        private FormTopScreen TopScreen = null;
        private MainForm MainForm = null;
        private List<string> listBtnName = new List<string>();
        private int locationX = 0;
        private int locationY = 0;
        private int formWidth = 0;

        public int m_nSelectDropMenuNum = 0;

        public DropMenu(MainForm _MainForm, FormTopScreen _topScreen)
        {
            InitializeComponent();
            TopScreen = _topScreen;
            MainForm = _MainForm;
            MainForm.SetEachControlResize(this);    //240801 NIS Conrol Resize
            // 출력 위치

        }

        public void Initialized()
        {
            BtnLocationSetting(m_nSelectDropMenuNum);
            SelectListBtnName(m_nSelectDropMenuNum);
        }

        private void BtnLocationSetting(int _nSelectBtn)
        {
            switch (_nSelectBtn)
            {
                case SETTING_MENU:
                    this.locationX = TopScreen.BtnSettingPage.Location.X + 12;
                    this.locationY = (int)(TopScreen.Height / 1.2);
                    this.formWidth = TopScreen.BtnSettingPage.Width;

                    break;
                case PARAM_MENU:
                    this.locationX = TopScreen.BtnParamPage.Location.X + 12;
                    this.locationY = (int)(TopScreen.Height / 1.2);
                    this.formWidth = TopScreen.BtnParamPage.Width;

                    break;

                case HISTROY_MENU:
                    this.locationX = TopScreen.BtnHistoryPage.Location.X + 12;
                    this.locationY = (int)(TopScreen.Height / 1.2);
                    this.formWidth = TopScreen.BtnHistoryPage.Width;

                    break;

                case TEACH_MENU:
                    this.locationX = TopScreen.BtnTeachPage.Location.X + 12;
                    this.locationY = (int)(TopScreen.Height / 1.2);
                    this.formWidth = TopScreen.BtnTeachPage.Width;

                    break;
            }

            this.Height = listBtnName.Count * DropMenuBtn1.Height; // Btn Height(45)
        }

        private void SelectListBtnName(int _nSelectName)
        {
            switch (_nSelectName)
            {
                case SETTING_MENU:
                    listBtnName.Add(CAM_SETTING);
                    listBtnName.Add(CALIBRATION);
                    listBtnName.Add(PLC_SETTING);

                    break;
                case PARAM_MENU:
                    listBtnName.Add(MODEL_PARAM);
                    //listBtnName.Add(CALIB_PARAM);

                    break;
                case HISTROY_MENU:
                    listBtnName.Add(HISTORY_DATA);
                    listBtnName.Add(HISTORY_SIMUL);
                    listBtnName.Add(HISTORY_INTERFACE_DATA);

                    break;

                case TEACH_MENU:
                    listBtnName.Add(TEACHING);
                    //listBtnName.Add(COLOR_TEACH);

                    break;
            }
        }
        // 폼을 불러올때
        private void FormHistoryDropMenu_Load(object sender, EventArgs e)
        {
            this.DesktopLocation = new Point(locationX, locationY);
            this.Width = formWidth;

            BtnLocationSetting(m_nSelectDropMenuNum);
            SelectListBtnName(m_nSelectDropMenuNum);

            // DropMenu에 있는 Btn 인덱스, 이름 지정
            ButtonNaming();
        }

        // DropMenu에 있는 Btn 인덱스, 이름 지정
        private void ButtonNaming()
        {
            foreach (Control con in this.Controls)
            {
                if (con.TabIndex < listBtnName.Count)
                    con.Text = listBtnName[con.TabIndex];
            }
        }

        #region History DropBar Btn Click Event

        // Log Page Index 0


        // Simulation Page Index 1


        #endregion

        private void DropMenuBtn1_Click(object sender, EventArgs e)
        {
            string EventMessageStringData = (string)sender.ToString();
            string[] EvnetMessageSplitData = EventMessageStringData.Split(':');

            switch (m_nSelectDropMenuNum)
            {
                case SETTING_MENU:

                    if (CAM_SETTING == EvnetMessageSplitData[1].Trim() /*&& theRecipe.UserAccess["Mono Cam"] 임시 주석*/)
                    {
                        TopScreen.SelectPage(EFormType.SETTING_CAMERA);
                        TopScreen.IsDropFormAlreadyOpen();
                        TopScreen.RefresuClickButton();
                    }
                    else
                    {
                        MessageBox.Show("Not allowed access.");
                    }

                    break;
                case PARAM_MENU:

                    if (MODEL_PARAM == EvnetMessageSplitData[1].Trim() /*&& theRecipe.UserAccess["Model"] 임시 주석*/)
                    {
                        TopScreen.SelectPage(EFormType.PARAMETER_MODEL);
                        TopScreen.IsDropFormAlreadyOpen();
                        //FormParameterModelScreen form = (FormParameterModelScreen)MainForm.MainFormArray[(int)DEF_UI.EFormType.PARAMETER_MODEL];
                        //form.SetDataGrid();   //임시 주석
                        TopScreen.RefresuClickButton();
                    }
                    else
                    {
                        MessageBox.Show("Not allowed access.");
                    }

                    break;
                case HISTROY_MENU:

                    if (HISTORY_DATA == EvnetMessageSplitData[1].Trim() /*&& theRecipe.UserAccess["DataView"] 임시 주석*/)
                    {
                        TopScreen.SelectPage(EFormType.HISTORY_DATA);
                        TopScreen.IsDropFormAlreadyOpen();

                        TopScreen.RefresuClickButton();
                    }
                    else
                    {
                        MessageBox.Show("Not allowed access.");
                    }

                    break;

                case TEACH_MENU:
                    if (TEACHING == EvnetMessageSplitData[1].Trim() /*&& theRecipe.UserAccess["Mono"] 임시 주석*/)
                    {
                        TopScreen.SelectPage(EFormType.TEACHING);

                        TopScreen.IsDropFormAlreadyOpen();

                        TopScreen.RefresuClickButton();
                        //MainForm.TeachingInspection.ResetScreen.Invoke();
                    }
                    else
                    {
                        MessageBox.Show("Not allowed access.");
                    }

                    break;
            }
        }

        private void DropMenuBtn2_Click(object sender, EventArgs e)
        {
            string EventMessageStringData = (string)sender.ToString();
            string[] EvnetMessageSplitData = EventMessageStringData.Split(':');

            switch (m_nSelectDropMenuNum)
            {
                case SETTING_MENU:

                    if (CALIBRATION == EvnetMessageSplitData[1].Trim() /*&& theRecipe.UserAccess["Calibration"] 임시 주석*/)
                    {
                        TopScreen.SelectPage(EFormType.SETTING_CALIBRATION);
                        TopScreen.IsDropFormAlreadyOpen();

                        TopScreen.RefresuClickButton();
                        //MainForm.CalibrationForm.ResetScreen.Invoke();    //임시 주석
                    }
                    else
                    {
                        MessageBox.Show("Not allowed access.");
                    }

                    break;

                case HISTROY_MENU:
                    if (HISTORY_SIMUL == EvnetMessageSplitData[1].Trim()) /*&& theRecipe.UserAccess["Simulation"] 임시 주석*/ //250213 KCH TrackingData 버튼 제거로 인해 수정
                    {
                        TopScreen.SelectPage(EFormType.HISTORY_SIMUL);
                        TopScreen.IsDropFormAlreadyOpen();
                        FormSimulation formSimul = (FormSimulation)MainForm.MainFormArray[(int)DEF_UI.EFormType.HISTORY_SIMUL];
                        formSimul.bSimulFirstCheck = true; //240710 KCH Image Refresh를 위한 변수 설정
                        formSimul.GridViewClear();
                        formSimul.LoadDateList();
                        formSimul.SetDataList();
                        TopScreen.RefresuClickButton();
                    }
                    else
                    {
                        MessageBox.Show("Not allowed access.");
                    }


                    break;
            }

        }

        private void DropMenuBtn3_Click(object sender, EventArgs e)
        {
            string EventMessageStringData = (string)sender.ToString();
            string[] EvnetMessageSplitData = EventMessageStringData.Split(':');

            switch (m_nSelectDropMenuNum)
            {
                case SETTING_MENU:

                    if (PLC_SETTING == EvnetMessageSplitData[1].Trim() /*&& theRecipe.UserAccess["PLC"] 임시 주석 */)
                    {
                        TopScreen.SelectPage(EFormType.SETTING_PLC);
                        TopScreen.IsDropFormAlreadyOpen();

                        TopScreen.RefresuClickButton();
                    }
                    else
                    {
                        MessageBox.Show("Not allowed access.");
                    }

                    break;
                case PARAM_MENU:

                    break;
                case HISTROY_MENU:
                    if (HISTORY_INTERFACE_DATA == EvnetMessageSplitData[1].Trim() /*&& theRecipe.UserAccess["Interface"] 임시 주석*/)//250213 KCH TrackingData 버튼 제거로 인해 수정
                    {
                        TopScreen.SelectPage(EFormType.HISTORY_INTERFACE_DATA);
                        TopScreen.IsDropFormAlreadyOpen();

                        TopScreen.RefresuClickButton();
                    }
                    else
                    {
                        MessageBox.Show("Not allowed access.");
                    }


                    break;
            }
        }

        private void DropMenuBtn4_Click(object sender, EventArgs e)
        {
            string EventMessageStringData = (string)sender.ToString();
            string[] EvnetMessageSplitData = EventMessageStringData.Split(':');

            switch (m_nSelectDropMenuNum)
            {
                case SETTING_MENU:
                    

                    break;

                case PARAM_MENU:

                    break;
            }
        }

        private void DropMenuBtn5_Click(object sender, EventArgs e)
        {
            string EventMessageStringData = (string)sender.ToString();
            string[] EvnetMessageSplitData = EventMessageStringData.Split(':');

            switch (m_nSelectDropMenuNum)
            {
                case SETTING_MENU:
                    break;
                case PARAM_MENU:

                    break;
                case HISTROY_MENU:

                    break;
            }
        }
    }
}
