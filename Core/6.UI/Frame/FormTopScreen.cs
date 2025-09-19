using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

using Cognex.VisionPro;
using Cognex.VisionPro.Implementation;

using static Core.Program;
using static Core.DEF_UI;
using static Core.DEF_Common;
using Core;
using Core.Utility;

namespace Core.UI
{
    public partial class FormTopScreen : Form
    {
        public enum BUTTON_NAME { Setting = 0, Param, Teach, History};

        private MainForm MainForm = null;
        private DisplayScreen displayScreen = null;
        private Button[] BtnPage = new Button[6];
        private EButtonType CurrentButton = EButtonType.NONE;
        private EFormType CurrentPage = EFormType.AUTO;

        private CHardWareUsage HardWareUsage = new CHardWareUsage();
        private SOpenCheck[] OpenCheck = new SOpenCheck[4];

        private CLogger Logger = null;

        enum EBtnOption
        {
            Normal,
            Clicked,
            Over,
            Disable,
            Max,
        }

        public FormTopScreen(MainForm _mainForm, CLogger _logger)
        {
            InitializeComponent();
            InitializeForm();

            this.TopLevel = false;
            MainForm = _mainForm;
            Logger = _logger;
            
            ResourceMapping();

            TimerUI.Tick += OnClock;
            TimerUI.Start();
            OnClock(null, null);
            MainForm.SetEachControlResize(this);    //240731 NIS Conrol Resize
            this.Show();
        }

        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            //this.DesktopLocation = new Point(DEF_UI.TOP_POS_X, DEF_UI.TOP_POS_Y);
            this.Size = new Size(DEF_UI.TOP_SIZE_WIDTH, DEF_UI.TOP_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void OnClock(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            //theMainSystem.CheckDateLine(date);

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en");
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
            lblDate.Text = DateTime.Now.ToString("yyyy-MM-dd (ddd)", culture);

            //NWT 240623 Auto Delete Date   임시 주석
            //if(theRecipe.m_nAutoDeletechk == 0 && lblTime.Text == "12:00:00") 임시 주석
            //    theMainSystem.m_ThreadDateDelete.Continue();  임시 주석

        }
        public void OnExit()
        {
            theMainSystem.UnInitialzieCore();
            GC.Collect();
            Application.ExitThread();
            Environment.Exit(0);
        }

        private void OnClose(object sender, EventArgs e)
        {

            if (MainForm.InquireMsg("Exit System?"))
            {
                OnExit();
            }

        }

        private void ResourceMapping()
        {
            BtnPage[0] = BtnMainPage;
            BtnPage[0] = BtnMainPage;
            BtnPage[1] = BtnSettingPage;
            BtnPage[2] = BtnParamPage;
            BtnPage[3] = BtnTeachPage;
            BtnPage[4] = BtnHistoryPage;
            BtnPage[5] = btnExit;

            SelectPage(EFormType.AUTO);
        }

        public void SelectPage(EFormType index)
        {
            CurrentPage = index;
            
            MainForm?.DisplayManager.FormSelectChange(index);

        }

        /// NOTE : 220216 by Noh 
        /// ClickMenuButton, SelectPage 분리 된 이유
        /// Menu Bar 에 버튼은 6개, 
        /// 메인 화면에 출력 될 form 의 개수는 6개 이상 
        /// enum count 매칭에 문제가 발생하기 때문 

        public void ClickMenuButton(EButtonType index)
        {
            if (CurrentButton == index) return;

            CurrentButton = index;

            for (int i = 0; i < (int)EButtonType.MAX; i++)
            {
                if (i == (int)CurrentButton) continue;
                ButtonDisplay(i, EBtnOption.Normal);

                BtnPage[i].FlatStyle = FlatStyle.Flat;
            }

            ButtonDisplay((int)index, EBtnOption.Clicked);
        }

        /// NOTE : 20216 by Noh
        /// 버튼 마다 DropMenu Form 을 생성하게 되며
        /// 다른 버튼을 누를 때, open 되어 있는 모든 dropMenu Form 을
        /// close 하기 위함 
        public void IsDropFormAlreadyOpen()
        {
            for(int i = 0; i < DROP_MENU_MAXCOUNT; ++i)
            {
                MainForm.DropDownMenu[i].Hide();
            }
        }

        private void ButtonDisplay(int BtnNo, EBtnOption Option)
        {
            BtnPage[BtnNo].BackgroundImage = ImgList.Images[(int)Option + (BtnNo * 3)];
        }

        private void BtnPageEnter(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button.Name == "BtnMainPage")
            {
                if (CurrentButton != EButtonType.AUTO)
                    ButtonDisplay(0, EBtnOption.Over);
            }
            else if (button.Name == "BtnSettingPage")
            {
                if (CurrentButton != EButtonType.SETTING)
                    ButtonDisplay(1, EBtnOption.Over);
            }
            else if (button.Name == "BtnParamPage")
            {
                if (CurrentButton != EButtonType.PARAM)
                    ButtonDisplay(2, EBtnOption.Over);
            }
            else if (button.Name == "BtnTeachPage")
            {
                if (CurrentButton != EButtonType.TEACH)
                    ButtonDisplay(3, EBtnOption.Over);
            }
            else if (button.Name == "BtnHistoryPage")
            {
                if (CurrentButton != EButtonType.HISTORY)
                    ButtonDisplay(4, EBtnOption.Over);
            }
            else
            {
                if (CurrentButton != EButtonType.EXIT)
                    ButtonDisplay(5, EBtnOption.Over);
            }
        }

        private void BtnPageLeave(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button.Name == "BtnMainPage")
            {
                if (CurrentButton != EButtonType.AUTO)
                    ButtonDisplay(0, EBtnOption.Normal);
            }
            else if (button.Name == "BtnSettingPage")
            {
                if (CurrentButton != EButtonType.SETTING)
                    ButtonDisplay(1, EBtnOption.Normal);
            }
            else if (button.Name == "BtnParamPage")
            {
                if (CurrentButton != EButtonType.PARAM)
                    ButtonDisplay(2, EBtnOption.Normal);
            }
            else if (button.Name == "BtnTeachPage")
            {
                if (CurrentButton != EButtonType.TEACH)
                    ButtonDisplay(3, EBtnOption.Normal);
            }
            else if (button.Name == "BtnHistoryPage")
            {
                if (CurrentButton != EButtonType.HISTORY)
                    ButtonDisplay(4, EBtnOption.Normal);
            }
            else
            {
                if (CurrentButton != EButtonType.EXIT)
                    ButtonDisplay(5, EBtnOption.Normal);
            }
        }


        private void BtnPageClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button.Name == "BtnMainPage")
            {
                ClickMenuButton(EButtonType.AUTO); // Main button 클릭 이미지 변경 
                SelectPage(EFormType.AUTO); // MainScreen Page Open

                OpenCheck[(int)BUTTON_NAME.Setting].OpenCheck = false;
                OpenCheck[(int)BUTTON_NAME.Param].OpenCheck = false;
                OpenCheck[(int)BUTTON_NAME.Teach].OpenCheck = false;
                OpenCheck[(int)BUTTON_NAME.History].OpenCheck = false;

                IsDropFormAlreadyOpen(); // 모든 DropPage 종료
            }
            else if (button.Name == "BtnSettingPage")
            {
                
                ClickMenuButton(EButtonType.SETTING); // Setting button 클릭 이미지 변경 
                IsDropFormAlreadyOpen(); // 모든 DropPage 종료

                if (OpenCheck[(int)BUTTON_NAME.Setting].OpenCheck == false)
                {
                    OpenCheck[(int)BUTTON_NAME.Setting].OpenCheck = true;
                    OpenCheck[(int)BUTTON_NAME.Param].OpenCheck = false;
                    OpenCheck[(int)BUTTON_NAME.Teach].OpenCheck = false;
                    OpenCheck[(int)BUTTON_NAME.History].OpenCheck = false;

                    MainForm.DropDownMenu[DEF_UI.SETTING_MENU].Show();
                }
                else
                {
                    OpenCheck[(int)BUTTON_NAME.Setting].OpenCheck = false;

                    MainForm.DropDownMenu[DEF_UI.SETTING_MENU].Hide();
                }
                
                
            }
            else if (button.Name == "BtnParamPage")
            {
                ClickMenuButton(EButtonType.PARAM); // Parameter button 클릭 이미지 변경 
                IsDropFormAlreadyOpen(); // 모든 DropPage 종료
                FormParameterModelScreen form = (FormParameterModelScreen)MainForm.MainFormArray[(int)DEF_UI.EFormType.PARAMETER_MODEL];
                //form.SetAutoDelete(); //임시 주석

                if (OpenCheck[(int)BUTTON_NAME.Param].OpenCheck == false)
                {
                    OpenCheck[(int)BUTTON_NAME.Param].OpenCheck = true;
                    OpenCheck[(int)BUTTON_NAME.Setting].OpenCheck = false;
                    OpenCheck[(int)BUTTON_NAME.Teach].OpenCheck = false;
                    OpenCheck[(int)BUTTON_NAME.History].OpenCheck = false;

                    MainForm.DropDownMenu[DEF_UI.PARAM_MENU].Show();
                }
                else
                {
                    OpenCheck[(int)BUTTON_NAME.Param].OpenCheck = false;
                    MainForm.DropDownMenu[DEF_UI.PARAM_MENU].Hide();
                }
                
            }
            else if (button.Name == "BtnTeachPage")
            {
                ClickMenuButton(EButtonType.TEACH); // Teach button 클릭 이미지 변경 
                                                    //SelectPage(EFormType.MONOTEACH); // Teach Page Open
                IsDropFormAlreadyOpen(); // 모든 DropPage 종료

                if (OpenCheck[(int)BUTTON_NAME.Teach].OpenCheck == false)
                {
                    OpenCheck[(int)BUTTON_NAME.Teach].OpenCheck = true;
                    OpenCheck[(int)BUTTON_NAME.Param].OpenCheck = false;
                    OpenCheck[(int)BUTTON_NAME.Setting].OpenCheck = false;
                    OpenCheck[(int)BUTTON_NAME.History].OpenCheck = false;

                    MainForm.DropDownMenu[DEF_UI.TEACH_MENU].Show();
                }
                else
                {
                    OpenCheck[(int)BUTTON_NAME.Teach].OpenCheck = false;
                    MainForm.DropDownMenu[DEF_UI.TEACH_MENU].Hide();
                }
                
            }
            else if (button.Name == "BtnHistoryPage")
            {
                ClickMenuButton(EButtonType.HISTORY); // History button 클릭 이미지 변경 
                IsDropFormAlreadyOpen(); // 모든 DropPage 종료
                

                if (OpenCheck[(int)BUTTON_NAME.History].OpenCheck == false)
                {
                    OpenCheck[(int)BUTTON_NAME.History].OpenCheck = true;
                    OpenCheck[(int)BUTTON_NAME.Teach].OpenCheck = false;
                    OpenCheck[(int)BUTTON_NAME.Param].OpenCheck = false;
                    OpenCheck[(int)BUTTON_NAME.Setting].OpenCheck = false;

                    MainForm.DropDownMenu[DEF_UI.HISTROY_MENU].Show();
                }
                else
                {
                    OpenCheck[(int)BUTTON_NAME.History].OpenCheck = false;
                    MainForm.DropDownMenu[DEF_UI.HISTROY_MENU].Hide();
                }
                
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //theMainSystem.m_PageController.TestTrigger();
            //theMainSystem.DoGrabLiveStart();
            //theMainSystem.InterfaceADLink.WriteData(DEF_SYSTEM.IO_VISIONCOMPLETE, 0);
            //theMainSystem.InterfaceADLink.WriteData(DEF_SYSTEM.IO_VISIONREDY, 1);
            //theMainSystem.m_bRunningGrabDeep = false;

            for(int i = 0; i < 4; i++)
            {
                //theMainSystem.Cameras[i].ColorInspectionComplete();
            }
        }

        private void btn_LightReset_Click(object sender, EventArgs e)
        {
            //theMainSystem.Cameras[0].m_Camera.ResetOI();
            //theMainSystem.m_PageController.TriggerReset();
            //theMainSystem.m_PageController.MaxPageSet();
            InspectionInfo info = new InspectionInfo();
            //theMainSystem.mainForm.BottomForm.ErrorImageRefresh(info);    임시 주석
        }

        public void RefresuClickButton()
        {
            OpenCheck[(int)BUTTON_NAME.Setting].OpenCheck = false;
            OpenCheck[(int)BUTTON_NAME.Param].OpenCheck = false;
            OpenCheck[(int)BUTTON_NAME.Teach].OpenCheck = false;
            OpenCheck[(int)BUTTON_NAME.History].OpenCheck = false;
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            theMainSystem.SimulTest();
        }
    }
}
