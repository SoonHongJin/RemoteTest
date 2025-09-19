using Cognex.VisionPro.Implementation.Internal;
using Core.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using static Core.Program;

namespace Core.UI
{
    public enum CalibrationStatus       //240805 NIS Calibration 수행 후 사용할 결과값
    {
        NG = -1,
        None = 0,
        Processing = 1,
        Skip = 2,
        OK = 3
    }

    public enum CalibMode
    {
        None = 0,
        CheckerBoard,
    }

    public partial class FormCalibrationScreen : Form
    {
        #region Fields
        private MainForm MainForm = null;
        private FormCalibrationCheckerBoard CheckerBoardCalibForm;
        private FormInsCalibrationCheckerBoard InsCheckerBoardCalibForm;

        private List<Form> CalibFormList = new List<Form>();
        private List<Button> CalibButtonList = new List<Button>();

        private const int BTN_LINE_THICKNESS = 5;

        public Dictionary<string, List<CImage>> CheckerBoardImages = new Dictionary<string, List<CImage>>();

        private StringBuilder CheckBoardPath = new StringBuilder();

        #endregion
        public Action ResetScreen;  //241116 NIS Screen Record 초기화용 Action
        public Action Refresh_CalibStatus;

        private CLogger Logger = null;

        public FormCalibrationScreen(MainForm _MainForm, CLogger _logger)
        {
            InitializeComponent();
            TopLevel = false;

            MainForm = _MainForm;
            Logger = _logger;

            InitializeForm();
            MainForm.SetEachControlResize(this);    //240805 NIS Conrol Resize

            DoLoad();
            CreateForm();
            
        }

        private void DoLoad()
        {
            SetEvent();
            SetCalibStatus();
        }

        private void SetEvent()
        {
            Refresh_CalibStatus += SetCalibStatus;
        }

        private void SetCalibStatus()
        {
            int OK = 1, NG = 2;
            btn_CalibStep_CheckBoard.BackgroundImage = CalibStatusImgList.Images[theRecipe.calibrationStatus.IsCheckerBoardComplete ? OK : NG];
        }

        private void CreateForm()
        {
            //240308 LYK Ins Cog,조건에 따라 맞는 폼 출력
            if (DEF_SYSTEM.LICENSES_KEY == (int)License.COG)
            {
                CheckerBoardCalibForm = new FormCalibrationCheckerBoard(MainForm, this, Logger)
                {
                    Parent = pnl_CalibFormScreen,
                    Size = pnl_CalibFormScreen.Size,
                    Visible = false
                };
                CalibFormList.Add(CheckerBoardCalibForm);
                CalibButtonList.Add(btnDisplayCheckerBoardForm);
            }
            else
            {
                InsCheckerBoardCalibForm = new FormInsCalibrationCheckerBoard(MainForm, this, Logger)
                {
                    Parent = pnl_CalibFormScreen,
                    Size = pnl_CalibFormScreen.Size,
                    Visible = false
                };
                CalibFormList.Add(InsCheckerBoardCalibForm);
                CalibButtonList.Add(btnDisplayCheckerBoardForm);
            }
        }
        private void btn_DisplayEachCalibForm(object sender, EventArgs e)    //240805 NIS Displays only SelectedForm, Hide the others
        {
            Button btn = sender as Button;

            for (int i = 0; i < CalibFormList.Count; i++)   //240805 NIS 모든 Form과 Button 초기화
            {
                CalibFormList[i].Visible = false;
                CalibButtonList[i].FlatAppearance.BorderSize = 1;
            }

            if (btn.Name.Contains("CheckerBoard"))
            {
                //240308 NWT Ins Cog,조건에 따라 맞는 폼 출력
                if (DEF_SYSTEM.LICENSES_KEY == (int)License.COG)
                {
                    CheckerBoardCalibForm.Visible = true;
                    btnDisplayCheckerBoardForm.FlatAppearance.BorderSize = BTN_LINE_THICKNESS;
                    CheckerBoardCalibForm.ScreenScrollVisibleFalse();
                    CheckerBoardCalibForm.FirstRaioBtnClick();
                }
                else
                {
                    InsCheckerBoardCalibForm.Visible = true;
                    btnDisplayCheckerBoardForm.FlatAppearance.BorderSize = BTN_LINE_THICKNESS;
                    InsCheckerBoardCalibForm.ScreenScrollVisibleFalse();
                    InsCheckerBoardCalibForm.FirstRaioBtnClick();
                }
            }

            ResetScreen.Invoke();   //241116 NIS Teaching Screen Record 초기화
        }

        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.MAIN_POS_X, DEF_UI.MAIN_POS_Y);
            this.Size = new Size(DEF_UI.MAIN_SIZE_WIDTH, DEF_UI.MAIN_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;
        }

        /// <summary>
        /// 24.04.06 LYK Initialize 
        /// 체커 보드 캘리브레이션 원본 이미지 저장 폴더 초기화 진행
        /// </summary>
        private void Initialize()
        {
            STime StartTime;
            StartTime.Year = DateTime.Now.ToString("yyyy");
            StartTime.Month = DateTime.Now.ToString("MM");
            StartTime.Day = DateTime.Now.ToString("dd");
            StartTime.Hour = DateTime.Now.ToString("HH");
            StartTime.Min = DateTime.Now.ToString("mm");
            StartTime.Sec = DateTime.Now.ToString("ss");

            string sFolder = string.Format("{0}\\{1}{2}{3}\\{4}{5}{6}_CheckerBoardImage", DEF_SYSTEM.DEF_FOLDER_PATH_CALIB
                                                                     , StartTime.Year
                                                                     , StartTime.Month
                                                                     , StartTime.Day
                                                                     , StartTime.Year
                                                                     , StartTime.Month
                                                                     , StartTime.Day);

            CheckBoardPath.Clear();
            CheckBoardPath.Append(sFolder);

            if (!Directory.Exists(sFolder))
                Directory.CreateDirectory(sFolder);

            sFolder = string.Format("{0}\\{1}{2}{3}\\{4}{5}{6}_BlackCalib", DEF_SYSTEM.DEF_FOLDER_PATH_CALIB
                                                                     , StartTime.Year
                                                                     , StartTime.Month
                                                                     , StartTime.Day
                                                                     , StartTime.Year
                                                                     , StartTime.Month
                                                                     , StartTime.Day);
        }

        public Point Get_TeachFormScreenLocation()      //240721 NIS Get TeachForm Location
        {
            Point PanelLocation = MainForm.Get_DisplayPanelLocation();
            return new Point(PanelLocation.X + pnl_CalibFormScreen.Location.X, PanelLocation.Y + pnl_CalibFormScreen.Location.Y);
        }
        
        private void IsCalibStatusNG(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string btnName = btn.Name.Split('_')[2];
            DialogResult result = MessageBox.Show($"Do you want to change {btnName} Calib Status?", $"Set {btnName} Calib Status", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                
               if (btnName == "CheckBoard")
                    theRecipe.calibrationStatus.IsCheckerBoardComplete = !theRecipe.calibrationStatus.IsCheckerBoardComplete;
            }

            Refresh_CalibStatus();
        }

        private void FormCalibrationScreen_Load(object sender, EventArgs e)
        {
            btnDisplayCheckerBoardForm.PerformClick();
        }
    }
}
