using Cognex.VisionPro;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.Implementation;
using Cognex.VisionPro.ToolBlock;
using Core.DataProcess;
using Core.Utility;
using Insnex.Vision2D.CalibFix;
using Insnex.Vision2D.Common;
using Insnex.Vision2D.Core;
using Insnex.Vision2D.ToolBlock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Core.Program;
using Core.Function.Preprocessing;
using Insnex.Vision2D.Controls;

namespace Core.UI
{
    public partial class FormCalibrationCheckerBoard : Form
    {
        private MainForm MainForm;
        private FormCalibrationScreen CalibMainForm;
        private List<ICogTool> m_TeachingToolList = new List<ICogTool>();   //Teaching CogTool List

        private int m_nSelectCam = 0;       //Selected cam number
        private int m_nToolSelected = 0;    //Selected tool list index
        private int m_nInspectSelected = 0; //Selected inspection mode

        private CogToolBlock _ToolBlock = null;     //CogToolBlock

        private Point TeachFormLocation;
        public RadioButton[] RadioButtons = new RadioButton[5];

        private Font font = new Font("Calibri", 18, FontStyle.Bold);
        private CogRecordDisplay[] CalibImageDisplay = new CogRecordDisplay[5];

        private bool m_bLiveTimerStart = false;
        private string[] m_sArrCalibImagesName;

        //250122 NWT Parameter Exception
        private List<CExceptionData> exceptionDataList = new List<CExceptionData>();
        private ParameterExceptionHandler parameterExceptionHandler = null;

        private CImage cCalibImage;// = new CImage(DEF_SYSTEM.LICENSES_KEY);

        private CLogger Logger = null;
        private bool IsCalibError = true;
        private bool[] IsEachCamCalibError = new bool[5] { true, true, true, true, true };

        public FormCalibrationCheckerBoard(MainForm _MainForm, FormCalibrationScreen _CalibMainForm, CLogger _logger)
        {
            InitializeComponent();
            CalibMainForm = _CalibMainForm;
            MainForm = _MainForm;
            Logger = _logger;
            TopLevel = false;
            exceptionDataList = theRecipe.m_listExceptionData;
            cCalibImage = new CImage(DEF_SYSTEM.LICENSES_KEY, theRecipe.m_sCurrentEquipment);

            FormLocate();
            DoLoad();
            MainForm.SetEachControlResize(this);    //240801 NIS Conrol Resize

            TeachFormLocation = CalibMainForm.Get_TeachFormScreenLocation();  //240721 NIS Get TeachFormScreen Location
            CalibMainForm.ResetScreen += Reset_DisplayScreen;
        }

        private void DoLoad()
        {
            SetRdb();
            SetLiveImage();
            RecipeLoad();
        }

        private void SetLiveImage()
        {
            pb_LiveStatus.Image = CalibStatusImgList.Images[0];
        }

        private void SetRdb()
        {
            Set_RadioButtonsList();

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                Controls.Find($"rdb_Cam{i+1}", true)[0].Visible = true;
            }
        }
        private void RecipeLoad()   //241014 NIS 
        {
            tb_TileSizeX.Text = theRecipe.m_dCheckerBoardTitleSize.X.ToString("F3");
            tb_TileSizeY.Text = theRecipe.m_dCheckerBoardTitleSize.Y.ToString("F3");
        }
        private void RecipeSave()
        {
            try
            {
                theRecipe.m_dCheckerBoardTitleSize.X = (float)Math.Round(double.Parse(tb_TileSizeX.Text), 3);
                theRecipe.m_dCheckerBoardTitleSize.Y = (float)Math.Round(double.Parse(tb_TileSizeY.Text), 3);
            }
            catch (Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"CheckerBoard Calib RecipeSave() Error : {ex.Message}");
                
            }
        }

        private void btn_LogicToolBlock_Click(object sender, EventArgs e)   //241010 NIS Open vpp ToolBlock
        {
            _ToolBlock = ((CogToolBlock)theRecipe.CalibrationToolBlock.Tools[m_nSelectCam]);
                                                        //임시 주석
            ToolBlockEdit ToolEdit = new ToolBlockEdit(/*ToolBlockEditTitle_Offset + */m_nInspectSelected, _ToolBlock); //240703 NIS 실행한 TeachMode에 따라서 ToolBlockEdit 타이틀 수정
            ToolEdit.Location = TeachFormLocation;
            ToolEdit.Size = CalibMainForm.pnl_CalibFormScreen.Size;
            ToolEdit.ShowDialog();
        }
        private void btn_Run_Click(object sender, EventArgs e)
        {
            m_bLiveTimerStart = false;
            LiveTimer.Stop();
            pb_LiveStatus.Image = CalibStatusImgList.Images[0]; //250115 NIS BeltCalib Live Off Image

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                ToolBlockRun(i, theRecipe.CalibrationToolBlock.Tools[i] as CogToolBlock, m_nInspectSelected);
            }
            m_nToolSelected = (int)lib_ToolList.SelectedIndex;
            ICogTool SelectTool = m_TeachingToolList[m_nToolSelected];

            if (SelectTool.Name.Contains("CogCalibCheckerboardTool"))// Tool 이름으로 구분하여 Parameter 표시
            {
                CogCalibCheckerboardTool CalibTool = SelectTool as CogCalibCheckerboardTool;

                SetTextCalibToolData(CalibTool);
                CalibCheckerBoardToolShow(CalibTool);
            }
            TeachInspectionRun();

            DisplayPixelResolution();
        }

        private void ToolList_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_nToolSelected = (int)lib_ToolList.SelectedIndex;

            ICogTool SelectTool = m_TeachingToolList[m_nToolSelected];

            if (SelectTool.Name.Contains("CogCalibCheckerboardTool"))// Tool 이름으로 구분하여 Parameter 표시
            {
                CogCalibCheckerboardTool CalibTool = SelectTool as CogCalibCheckerboardTool;

                SetTextCalibToolData(CalibTool);
                CalibCheckerBoardToolShow(CalibTool);
            }
        }

        private void rdb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdb = sender as RadioButton;
            try
            {
                if (rdb.Checked)
                {
                    if (RadioButtons[DEF_SYSTEM.CAM_ONE].Name == rdb.Name)
                    {
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"CheckerBoard Calib {rdb_Cam1.Text} Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_ONE;
                    }
                    else if (RadioButtons[DEF_SYSTEM.CAM_TWO].Name == rdb.Name)
                    {
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"CheckerBoard Calib {rdb_Cam2.Text} Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_TWO;
                    }
                    else if (RadioButtons[DEF_SYSTEM.CAM_THREE].Name == rdb.Name)
                    {
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"CheckerBoard Calib {rdb_Cam3.Text} Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_THREE;
                    }
                    else if (RadioButtons[DEF_SYSTEM.CAM_FOUR].Name == rdb.Name)
                    {
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"CheckerBoard Calib {rdb_Cam4.Text} Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_FOUR;
                    }
                    else if (RadioButtons[DEF_SYSTEM.CAM_FIVE].Name == rdb.Name)
                    {
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"CheckerBoard Calib {rdb_Cam5.Text} Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_FIVE;
                    }

                    ToolListShow();
                    CalibDisplayScreenVisbleTF();

                    TeachInspectionRun();
                }
            }
            catch (Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"CheckerBoard Calib rdb_CheckedChanged() Error : {ex.Message}");
            }
        }

        private void CalibDisplayScreenVisbleTF()
        {
            if (m_nSelectCam == DEF_SYSTEM.CAM_FIVE)
            {
                for (int i = 0; i <= DEF_SYSTEM.CAM_FOUR; i++)
                    CalibImageDisplay[i].Visible = false;
                CalibImageDisplay[DEF_SYSTEM.CAM_FIVE].Visible = true;
            }
            else
            {
                for (int i = 0; i <= DEF_SYSTEM.CAM_FOUR; i++)
                    CalibImageDisplay[i].Visible = true;
                CalibImageDisplay[DEF_SYSTEM.CAM_FIVE].Visible = false;
            }
        }

        private void ApplyAndSave_Click(object sender, EventArgs e)
        {
            Run_ApplyAndSave();
        }

        private void Run_ApplyAndSave()
        {
            try
            {
                ICogTool tool = m_TeachingToolList[m_nToolSelected];

                
                if (tool.Name.Contains("CogCalibCheckerboardTool")) // Tool 이름으로 구분하여 Parameter Apply
                {
                    CogCalibCheckerboardTool CalibTool = tool as CogCalibCheckerboardTool;
                    CalibToolParameterApply(CalibTool);
                }
                RecipeSave();

                theRecipe.ToolSave(theRecipe.m_sCurrentModelName);
                theRecipe.DataSave(theRecipe.m_sCurrentModelName);
            }
            catch (Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"CheckerBoard Calib Run_ApplyAndSave() Error : {ex.Message}");
            }
        }
        private void ToolListShow()   //240720 NIS Set ToolList
        {
            if (DEF_SYSTEM.LICENSES_KEY == (int)License.COG)
            {
                m_TeachingToolList.Clear();
                lib_ToolList.Items.Clear();

                if (theRecipe.CalibrationToolBlock == null)
                    return;

                for (int i = 0; i < ((CogToolBlock)theRecipe.CalibrationToolBlock.Tools[m_nSelectCam]).Tools.Count; i++)
                {
                    ICogTool tool = ((CogToolBlock)theRecipe.CalibrationToolBlock.Tools[m_nSelectCam]).Tools[i];
                    string sTemp = tool.Name;

                    if (sTemp.Contains("CogFindLine"))
                    {
                        lib_ToolList.Font = font;
                        lib_ToolList.Items.Add(sTemp);
                        m_TeachingToolList.Add(tool);
                    }
                    else if(sTemp.Contains("CogCalibCheckerboardTool"))
                    {
                        lib_ToolList.Font = font;
                        lib_ToolList.Items.Add(sTemp);
                        m_TeachingToolList.Add(tool);
                    }
                }
                lib_ToolList.SetSelected(0, true);
            }
        }

        private void CalibToolParameterApply(CogCalibCheckerboardTool CalibTool)
        {
            try
            {
                CalibTool.Calibration.PhysicalTileSizeX = double.Parse(tb_TileSizeX.Text);
                CalibTool.Calibration.PhysicalTileSizeY = double.Parse(tb_TileSizeY.Text);

                CogRecord CurrRecord = new CogRecord();
                CurrRecord.ContentType = typeof(ICogImage);
                CurrRecord.Content = CalibTool.InputImage;
                CurrRecord.Annotation = "InputImage";
                CurrRecord.RecordKey = "InputImage";
                CurrRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;

                CurrRecord.SubRecords.Add(CalibTool.CreateCurrentRecord());
                CurrentImageDisplay.Record = CurrRecord;
                CurrentImageDisplay.Fit();
            }
            catch (Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"CalibCheckerboard Tool Save Error Catch : {e.Message}");
            }
        }

        private void ToolBlockRun(int _nCamNum, CogToolBlock _toolBlock, int _nMode)    //240720 NIS ToolBlock Run
        {
            //_toolBlock.Inputs[0].Value = theMainSystem.Cameras[_nCamNum].BeltImages[0][m_nImageType].CogGrayImage;    임시 주석
            _toolBlock.Run();
        }

        private void SetTextCalibToolData(CogCalibCheckerboardTool CalibTool)
        {
            tb_TileSizeX.Text = CalibTool.Calibration.PhysicalTileSizeX.ToString();
            tb_TileSizeY.Text = CalibTool.Calibration.PhysicalTileSizeY.ToString();
        }

        private void CalibCheckerBoardToolShow(CogCalibCheckerboardTool CalibTool)
        {
            CogRecord CurrRecord = new CogRecord();
            CurrRecord.ContentType = typeof(ICogImage);
            CurrRecord.Content = CalibTool.InputImage;
            CurrRecord.Annotation = "InputImage";
            CurrRecord.RecordKey = "InputImage";
            CurrRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;

            CurrRecord.SubRecords.Add(CalibTool.CreateCurrentRecord());
            CurrentImageDisplay.Record = CurrRecord;
            CurrentImageDisplay.Fit();

            CogRecord LastRecord = new CogRecord();
            LastRecord.ContentType = typeof(ICogImage);
            LastRecord.Content = CalibTool.InputImage;
            LastRecord.Annotation = "InputImage";
            LastRecord.RecordKey = "InputImage";
            LastRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;

            LastRecord.SubRecords.Add(CalibTool.CreateLastRunRecord());
            LastRunImageDisplay.Record = LastRecord;
            LastRunImageDisplay.Fit();
        }

        private void TeachInspectionRun()   //240709 NIS Cam 변경 시 마다 Inspection Display
        {
            for(int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                //임시주석
                //CogRecord MainRecord = new CogRecord("Main", theMainSystem.Cameras[i].BeltImages[0][m_nImageType].GetType(), CogRecordUsageConstants.Result, false, theMainSystem.Cameras[i].BeltImages[0][m_nImageType].CogImage, "Main");
                CogGraphicCollection GraphicCollection = new CogGraphicCollection();

                //임시 주석
                //MainRecord.SubRecords.Add(new CogRecord("Graphic", typeof(CogGraphicCollection), CogRecordUsageConstants.Result, false, GraphicCollection, "Graphic"));

                //임시 주석
                //CalibImageDisplay[i].Image = theMainSystem.Cameras[i].BeltImages[0][m_nImageType].CogImage;   
                //CalibImageDisplay[i].Record = MainRecord;
                CalibImageDisplay[i].Fit();
            }
        }

        private void Set_RadioButtonsList()
        {
            RadioButtons[0] = rdb_Cam1;
            RadioButtons[1] = rdb_Cam2;
            RadioButtons[2] = rdb_Cam3;
            RadioButtons[3] = rdb_Cam4;
            RadioButtons[4] = rdb_Cam5;
        }

        private void FormLocate()
        {
            int width = ImageDisplayPanel.Width;
            int height = ImageDisplayPanel.Height;
            int length = width < height ? width : height;

            //240815 NIS 분할된 Frame 위치와 비율 조정
            // 240815 NIS 분할된 Frame 위치와 비율 조정
            Point[] locations = new Point[5]
            {
                new Point(0, 0),
                new Point(length, 0),
                new Point(0, length),
                new Point(length, length),
                new Point(0, 0)
            };

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                CalibImageDisplay[i] = new CogRecordDisplay
                {
                    Location = i < locations.Length ? locations[i] : new Point(0, 0),
                    Width = length * (1 + i / 4),
                    Height = length * (1 + i / 4)
                };

                ImageDisplayPanel.Controls.Add(CalibImageDisplay[i]);
            }


            pnl_ImageDisplay_Title.Width = length * 2;
            lbl_ImageDisplay_Title.Location = new Point((pnl_ImageDisplay_Title.Width - lbl_ImageDisplay_Title.Width) / 2, lbl_ImageDisplay_Title.Location.Y);
        }
        public void ScreenScrollVisibleFalse() //241011 NIS Screen Scroll visible=false
        {
            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++) //241015 NIS Screen이 만들어진 후 수행가능한 동작
            {
                CalibImageDisplay[i].HorizontalScrollBar = false;
                CalibImageDisplay[i].VerticalScrollBar = false;
            }
        }
        private void btn_LiveOn_Click(object sender, EventArgs e)
        {
            if (m_bLiveTimerStart == false)
            {
                m_bLiveTimerStart = true;
                LiveTimer.Start();
                pb_LiveStatus.Image = CalibStatusImgList.Images[1]; //250115 NIS BeltCalib Live On Image

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Belt Timer Start");
            }
        }

        private void btn_LiveOff_Click(object sender, EventArgs e)
        {
            m_bLiveTimerStart = false;
            LiveTimer.Stop();
            pb_LiveStatus.Image = CalibStatusImgList.Images[0]; //250115 NIS BeltCalib Live Off Image
        }

        private void LiveTimer_Tick(object sender, EventArgs e)
        {
            //theMainSystem.DoBeltCalibGrabStart(DEF_SYSTEM.BELT_CALIB);    임시 주석

            CogCalibCheckerboardTool CalibTool = m_TeachingToolList[lib_ToolList.SelectedIndex] as CogCalibCheckerboardTool;

            CalibToolParameterApply(CalibTool);

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                ToolBlockRun(i, theRecipe.CalibrationToolBlock.Tools[i] as CogToolBlock, m_nInspectSelected);
            }

            CalibCheckerBoardToolShow(CalibTool);

            TeachInspectionRun();

            DisplayPixelResolution();
        }

        public void FirstRaioBtnClick()
        {
            rdb_Cam1.Checked = true;
        }
        private void Reset_DisplayScreen()
        {
            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                CalibImageDisplay[i].Record = null;
        }
        private string CreateDirectory()
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
            if (!Directory.Exists(sFolder))
                Directory.CreateDirectory(sFolder);

            return sFolder;
        }

        private void Load_CalibImage(object sender, EventArgs e)
        {
            string dirPath = CreateDirectory();

            m_bLiveTimerStart = false;
            LiveTimer.Stop();
            pb_LiveStatus.Image = CalibStatusImgList.Images[0]; //250115 NIS BeltCalib Live Off Image

            Button btn = sender as Button;
            if (btn.Name.Contains("One"))
            {
                using (OpenFileDialog fileDialog = new OpenFileDialog())
                {
                    fileDialog.Filter = "Bitmap files (*.bmp)|*.bmp";
                    fileDialog.Multiselect = false;
                    fileDialog.InitialDirectory = dirPath;

                    DialogResult dr = fileDialog.ShowDialog();

                    if (dr == DialogResult.OK)
                    {
                        try
                        {
                            m_sArrCalibImagesName = fileDialog.FileNames;
                            CogCalibCheckerboardTool CalibTool = ((CogToolBlock)theRecipe.CalibrationToolBlock.Tools[m_nSelectCam]).Tools[0] as CogCalibCheckerboardTool;
                            CalibTool.InputImage = new CogImage8Grey(new Bitmap(m_sArrCalibImagesName[0]));
                            CalibTool.Calibration.CalibrationImage = CalibTool.InputImage;
                            CalibTool.Calibration.Calibrate();

                            CalibCheckerBoardToolShow(CalibTool);
                            //250224 NIS Check each CheckerBoard calibration
                            IsEachCamCalibError[m_nSelectCam] = false;
                            int ErrorCount = 0;
                            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                            {
                                if (IsEachCamCalibError[i])
                                    ErrorCount++;
                            }
                            IsCalibError = ErrorCount == 0 ? false : true;
                        }
                        catch (Exception ex)
                        {
                            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Calibration Failed : {ex.Message}");
                            MessageBox.Show("CheckerBoard Calibration Failed. Please check CheckerBoard", "Calib Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            IsCalibError = true;
                        }
                    }
                    else if (dr == DialogResult.Cancel)
                        ;
                }
            }

            DisplayPixelResolution();
        }

        private void Grab_CalibImage(object sender, EventArgs e)
        {
            m_bLiveTimerStart = false;
            LiveTimer.Stop();
            pb_LiveStatus.Image = CalibStatusImgList.Images[0]; //250115 NIS BeltCalib Live Off Image

            Button btn = sender as Button;
            if (btn.Name.Contains("One"))
            {
                try
                {
                    CogCalibCheckerboardTool CalibTool = ((CogToolBlock)theRecipe.CalibrationToolBlock.Tools[m_nSelectCam]).Tools[0] as CogCalibCheckerboardTool;
                    CalibTool.Calibration.CalibrationImage = CalibTool.InputImage;
                    CalibTool.Calibration.Calibrate();
                    CalibCheckerBoardToolShow(CalibTool);
                    //250224 NIS Check each CheckerBoard calibration
                    IsEachCamCalibError[m_nSelectCam] = false;
                    int ErrorCount = 0;
                    for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                    {
                        if (IsEachCamCalibError[i])
                            ErrorCount++;
                    }
                    IsCalibError = ErrorCount == 0 ? false : true;
                }
                catch (Exception ex)
                {
                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Calibration Failed : {ex.Message}");
                    MessageBox.Show("CheckerBoard Calibration Failed. Please check CheckerBoard", "Calib Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    IsEachCamCalibError[m_nSelectCam] = true;
                }
            }
            else
            {
                try
                {
                    for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                    {
                        CogCalibCheckerboardTool CalibTool = ((CogToolBlock)theRecipe.CalibrationToolBlock.Tools[i]).Tools[0] as CogCalibCheckerboardTool;
                        CalibTool.Calibration.CalibrationImage = CalibTool.InputImage;
                        CalibTool.Calibration.Calibrate();
                        CalibTool.Run();
                        if (i == m_nSelectCam)
                            CalibCheckerBoardToolShow(CalibTool);
                    }
                    IsCalibError = false;
                    CheckCalibComplete();   //250224 NIS Check CheckerBoard calibration
                }
                catch (Exception ex)
                {
                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Calibration Failed : {ex.Message}");
                    MessageBox.Show("CheckerBoard Calibration Failed. Please check CheckerBoard", "Calib Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    IsCalibError = true;
                }
            }

            DisplayPixelResolution();
        }

        private void Save_CalibImage(object sender, EventArgs e)
        {
            string dirPath = CreateDirectory();
            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                CogCalibCheckerboardTool CalibTool = ((CogToolBlock)theRecipe.CalibrationToolBlock.Tools[i]).Tools[0] as CogCalibCheckerboardTool;
                
                CogImage8Grey calibImage = CalibTool.Calibration.CalibrationImage as CogImage8Grey;
                //cCalibImage.CogImage = calibImage;
                cCalibImage.SaveBmpFromICogImage($"{dirPath}\\{i}.bmp");
            }
            //theMainSystem.Cameras[i].BeltImages[0][m_nImageType].SaveBmp($"{dirPath}\\{i}.bmp", theMainSystem.Cameras[i].BeltImages[0][m_nImageType]);
        }

        private void DisplayPixelResolution()
        {
            /*  임시 주석
            string pix1 = theMainSystem.m_darrPixelResolution[0] != 0 ? (theMainSystem.m_darrPixelResolution[0] * 1000).ToString("F3") : "";
            string pix2 = theMainSystem.m_darrPixelResolution[1] != 0 ? (theMainSystem.m_darrPixelResolution[1] * 1000).ToString("F3") : "";
            string pix3 = theMainSystem.m_darrPixelResolution[2] != 0 ? (theMainSystem.m_darrPixelResolution[2] * 1000).ToString("F3") : "";
            string pix4 = theMainSystem.m_darrPixelResolution[3] != 0 ? (theMainSystem.m_darrPixelResolution[3] * 1000).ToString("F3") : "";
            string pix5 = theMainSystem.m_darrPixelResolution[4] != 0 ? (theMainSystem.m_darrPixelResolution[4] * 1000).ToString("F3") : "";
            lbl_PixelResolution.Text = $"{pix1} {pix2} {pix3} {pix4} {pix5}";
            */
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

        private void btn_Calibration_Complete_Click(object sender, EventArgs e)
        {
            CheckCalibComplete();
        }
        private void CheckCalibComplete()
        {
            theRecipe.calibrationStatus.IsCheckerBoardComplete = !IsCalibError;
            string result = theRecipe.calibrationStatus.IsCheckerBoardComplete ? "Succeed" : "Failed";
            MessageBox.Show($"CheckerBoard Calibration {result}", "CheckerBoard Calibration", MessageBoxButtons.OK);

            MainForm.CalibrationForm.Refresh_CalibStatus.Invoke();
        }
    }
}
