

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;

using static Core.Program;
using static Core.DEF_UI;
using static Core.DEF_Common;
using Core;
using Core.Utility;
using Core.UI;

namespace Core
{
    public partial class MainForm : Form
    {
        public FormTopScreen TitleForm = null;
        public FormSideScreen SideForm = null;
        public FormBottomScreen BottomForm = null;
        public DisplayScreen DisplayScreen = null;
        public FormSettingScreen SettingScreen = null;
        public FormParameterModelScreen ParamerterModelForm = null;
        public FormTeachInspection TeachingInspection = null;
        public FormCalibrationScreen FormCalibration = null;
        public FormCalibrationScreen CalibrationForm = null;
        public FormSettingPlcScreen SettingPlcScreen = null;
        public FormResultHistory ResultHistory = null;
        public FormSimulation Simulation = null;
        public FormInterfaceData InterfaceData = null;
        public FormCurrentHistoryData CurrentHisotryData = null;

        public List<DropMenu> DropDownMenu = new List<DropMenu>();
        public static Form[] MainFormArray = new Form[(int)EFormType.MAX];

        public static EFormType PrevScreen;
        public CDisplayManager DisplayManager = new CDisplayManager();

        public FormIntro intro;
        public Action PLC_HandShake_Start = null;

        private CLogger Logger = null;  //250204 LYK Logger 객체

        public MainForm()
        {
            InitializeComponent();
            InitializeForm();

            this.Hide();


            // Initial Loading Form
            intro = new FormIntro();
            //intro.Show();

            //----------------------------------------------------------------------------------------------
            // Initialize System Core
            //----------------------------------------------------------------------------------------------
            theMainSystem.InitializeCore(this);
            Logger = theMainSystem.GetLogger;

            //----------------------------------------------------------------------------------------------
            // Initialize Form
            //----------------------------------------------------------------------------------------------

            intro.SetStatus("Create Forms", 70);
            CreateForms();

            Thread.Sleep(100);
            intro.SetStatus("Initial Screen", 90);
            InitScreen();

            Thread.Sleep(100);

            intro.Hide();

            this.Show();

            theMainSystem.RefreshCurrentModelName?.Invoke(theRecipe.m_sCurrentModelName);
        }
        
        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.MAIN_POS_X, DEF_UI.MAIN_POS_Y);
            this.Size = new Size(DEF_UI.MAIN_SIZE_WIDTH, DEF_UI.MAIN_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;

        }
        private void InitScreen()
        {
            AttachEventHandlers();

            SelectFormChange(EFormType.AUTO);
            TitleForm.ClickMenuButton(EButtonType.AUTO);
        }

        private void CreateForms()
        {
            InitMainDisplayScreen();

            TitleForm = new FormTopScreen(this, Logger)
            {
                Parent = TitlePanel,
                Size = TitlePanel.Size
            };

            SideForm = new FormSideScreen(this, Logger, theMainSystem.InterfacePLC)
            {
                Parent = SidePanel,
                Size = SidePanel.Size
            };

            BottomForm = new FormBottomScreen(this)
            {
                Parent = BottomPanel,
                Size = BottomPanel.Size
            };

            for (int i = 0; i < DROP_MENU_MAXCOUNT; ++i)
            {
                DropDownMenu.Add(new DropMenu(this, TitleForm)
                {
                    m_nSelectDropMenuNum = i
                });

                DropDownMenu[i].Initialized();
            }

            InitSettingScreen();

            CalibrationForm = new FormCalibrationScreen(this, Logger)
            {
                Parent = DisplayPanel,
                Size = DisplayPanel.Size
            };

            MainFormArray[(int)EFormType.SETTING_CALIBRATION] = CalibrationForm;

            SettingPlcScreen = new FormSettingPlcScreen(this, theMainSystem.InterfacePLC, Logger)
            {
                Parent = DisplayPanel,
                Size = DisplayPanel.Size
            };

            MainFormArray[(int)EFormType.SETTING_PLC] = SettingPlcScreen;

            ParamerterModelForm = new FormParameterModelScreen(this, Logger)
            {
                Parent = DisplayPanel,
                Size = DisplayPanel.Size
            };

            MainFormArray[(int)EFormType.PARAMETER_MODEL] = ParamerterModelForm;

            TeachingInspection = new FormTeachInspection(this, Logger)
            {
                Parent = DisplayPanel,
                Size = DisplayPanel.Size
            };

            MainFormArray[(int)EFormType.TEACHING] = TeachingInspection;

            ResultHistory = new FormResultHistory(this, Logger)
            {
                Parent = DisplayPanel,
                Size = DisplayPanel.Size
            };
            MainFormArray[(int)EFormType.HISTORY_DATA] = ResultHistory;

            Simulation = new FormSimulation(this, Logger)
            {
                Parent = DisplayPanel,
                Size = DisplayPanel.Size
            };

            MainFormArray[(int)EFormType.HISTORY_SIMUL] = Simulation;

            InterfaceData = new FormInterfaceData(this, Logger)
            {
                Parent = DisplayPanel,
                Size = DisplayPanel.Size
            };

            MainFormArray[(int)EFormType.HISTORY_INTERFACE_DATA] = InterfaceData;

            CurrentHisotryData = new FormCurrentHistoryData(this, Logger)
            {
                Parent = DisplayPanel,
                Size = DisplayPanel.Size
            };

            MainFormArray[(int)EFormType.HISOTRY_CURRENT_DATA] = InterfaceData;

        }

        private void InitMainDisplayScreen()
        {
            DisplayScreen = new DisplayScreen(this, Logger)
            {
                Parent = DisplayPanel,
                Size = DisplayPanel.Size
            };

            MainFormArray[(int)EFormType.AUTO] = DisplayScreen;

        }

        private void InitSettingScreen()
        {
            SettingScreen = new FormSettingScreen(this, theMainSystem.m_PageController,  Logger, theMainSystem.GetLiveStatus())
            {
                Parent = DisplayPanel,
                Size = DisplayPanel.Size,

            };
            MainFormArray[(int)EFormType.SETTING_CAMERA] = SettingScreen;
        }

        private void AttachEventHandlers()
        {
            DisplayManager.FormHandler += new FormSelectEventHandler(SelectFormChange);
        }

        private void DetachEventHandlers()
        {
            DisplayManager.FormHandler -= new FormSelectEventHandler(SelectFormChange);
        }

        private void SelectFormChange(EFormType type)
        {
            if (PrevScreen == EFormType.TEACHING)
                MainFormArray[(int)PrevScreen].Hide();

            MainFormArray[(int)PrevScreen].Hide();
            MainFormArray[(int)type].Top = TitleForm.Location.Y; // + 2;
            MainFormArray[(int)type].Show();
            PrevScreen = type;
        }


        //==================================================================================
        #region [PopUp Display]

        static public bool DisplayMsg(string strMsg)
        {
            //var dlg = new FormMessageBox();
            //dlg.SetMessage(strMsg, EMessageType.OK);
            //dlg.TopMost = true;
            //dlg.ShowDialog();

            //if (dlg.DialogResult == DialogResult.OK)
            //    return true;
            //else return false;

            return true;
        }

        static public bool InquireMsg(string strMsg)
        {
            var dlg = new FormMsgBox();
            dlg.SetMessage(strMsg, EMessageType.OK_CANCEL);
            dlg.TopMost = true;
            dlg.ShowDialog();

            if (dlg.DialogResult == DialogResult.OK)
                return true;
            else return false;

  
        }

        static public bool GetKeyPad(string strCurrent, out string strModify)
        {
            var dlg = new FormKeyPad();
            dlg.SetValue(strCurrent);
            dlg.TopMost = true;
            dlg.ShowDialog();

            if (dlg.DialogResult == DialogResult.OK)
            {
                if (dlg.ModifyNo.Text == "")
                {
                    strModify = "0";
                }
                else
                {
                    strModify = dlg.ModifyNo.Text;
                }
            }
            else
            {
                strModify = strCurrent;
                dlg.Dispose();
                return false;
            }
            dlg.Dispose();
            return true;

        }

        static public bool GetKeyboard(out string strModify, string title = "Input", bool SecretMode = false)
        {
            var dlg = new FormKeyBoard(title, SecretMode);
            dlg.TopMost = true;
            dlg.ShowDialog();

            if (dlg.DialogResult == DialogResult.OK)
            {
                strModify = dlg.InputString;
                return true;
            }
            else
            {
                strModify = "";
                return false;
            }

        }


        #endregion
        public Point Get_DisplayPanelLocation() //240721 NIS Get DisplayPanel Location
        {
            return DisplayPanel.Location;
        }

        /// <summary>
        /// 24.07.30 NIS CalculateFontRatio
        /// UI Resize에 관련된 Ratio를 구하는 함수
        /// Ratio는 MainForm과 모든 controls의 Resize에 활용됨
        /// </summary>
        private void CalculateFontRatio()       //240730 NIS FormResize-Calculate resize ratio
        {
            theRecipe.m_dUIResizeRatio_Width = (double)Screen.PrimaryScreen.Bounds.Width / DEF_UI.MAIN_SIZE_WIDTH;
            theRecipe.m_dUIResizeRatio_Height = (double)Screen.PrimaryScreen.Bounds.Height / DEF_UI.MAIN_SIZE_HEIGHT;
            theRecipe.m_dUIResizeRatio = Math.Min(theRecipe.m_dUIResizeRatio_Width, theRecipe.m_dUIResizeRatio_Height);
        }

        /// <summary>
        /// 24.07.30 NIS SetFormResize
        /// Form 전체사이즈 resize
        /// </summary>
        /// <param name="mainControl"></param>
        public void SetFormResize(Control mainControl)
        {
            mainControl.Font = new Font(mainControl.Font.FontFamily, mainControl.Font.Size * (float)theRecipe.m_dUIResizeRatio);
            mainControl.Location = new Point((int)(DEF_UI.MAIN_POS_X * theRecipe.m_dUIResizeRatio_Width), (int)(DEF_UI.MAIN_POS_Y * theRecipe.m_dUIResizeRatio_Height));
            mainControl.Size = new Size((int)(DEF_UI.MAIN_SIZE_WIDTH * theRecipe.m_dUIResizeRatio_Width), (int)(DEF_UI.MAIN_SIZE_HEIGHT * theRecipe.m_dUIResizeRatio_Height));
        }

        /// <summary>
        /// 24.07.30 NIS SetEachControlResize(Control mainControl)
        /// mainControls 하위의 control들을 resize
        /// </summary>
        /// <param name="mainControl"></param>
        public void SetEachControlResize(Control mainControl)  //240730 NIS FormResize-ControlResize
        {
            foreach (Control control in mainControl.Controls)
            {
                control.Font = new Font(control.Font.FontFamily, control.Font.Size * (float)theRecipe.m_dUIResizeRatio);
                control.Location = new Point((int)(control.Location.X * theRecipe.m_dUIResizeRatio_Width), (int)(control.Location.Y * theRecipe.m_dUIResizeRatio_Height));
                control.Size = new Size((int)(control.Bounds.Width * theRecipe.m_dUIResizeRatio_Width), (int)(control.Bounds.Height * theRecipe.m_dUIResizeRatio_Height));
                control.BackgroundImageLayout = ImageLayout.Stretch;

                if (control.HasChildren)        //240801 NIS 하위에 컨트롤이 있으면 재귀실행
                    SetEachControlResize(control);
            }
        }
    }


    public delegate void FormSelectEventHandler(EFormType type);

    public class CDisplayManager
    {
        public event FormSelectEventHandler FormHandler = null;

        public void FormSelectChange(EFormType type)
        {
            if (FormHandler != null)
            {
                FormHandler(type);
            }
        }
    }

    
}
