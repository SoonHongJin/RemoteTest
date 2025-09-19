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
using Core.Function;

using static Core.Program;

using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.Implementation;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Dimensioning;
using System.Drawing.Drawing2D;

using Insnex.Vision2D.Core;
using Insnex.Vision2D.Common;
using Insnex.Vision2D.ToolBlock;
using Insnex.Vision2D.ImageProcessing;
using Insnex.Vision2D.Finder;
using Insnex.Vision2D.Intersection;
using Insnex.Vision2D.CalibFix;
using Insnex.Vision2D.Measurement;
using System.IO;
using Insnex.Vision2D;
using Microsoft.Win32;
using Core.DataProcess;
using ScottPlot.Drawing.Colormaps;

namespace Core.UI
{
    public partial class FormTeachCogPM : Form, ITeachForm
    {
        private MainForm MainForm;
        private FormTeachInspection TeachMainForm;  //parent form
        private int m_nSelectCam = 0;       //Selected cam number
        private int m_nToolSelected = 0;    //Selected tool list index
        private int m_nInspectSelected = 0; //Selected inspection mode

        CogToolBlock _ToolBlock = null;     //CogToolBlock
        CogRecordDisplay[] m_DisplayScreen = new CogRecordDisplay[4];

        private List<ICogTool> m_TeachingToolList = new List<ICogTool>();   //Teaching CogTool List

        private Font font = new Font("Calibri", 14, FontStyle.Bold);
        private Font ComboBoxFont = new Font("Calibri", 9, FontStyle.Bold);

        private Point TeachFormLocation;
        public RadioButton[] RadioButtons = new RadioButton[4];

        private CogTransform2DLinear[] CalibPointFromPix = new CogTransform2DLinear[4];
        private CogLine[] CogWaferLine_TopLeftBtmRight = new CogLine[4] { new CogLine(), new CogLine(), new CogLine(), new CogLine() };

        private CogImage8Grey InspectionImage = null;

        private string pattern_path = $"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{theRecipe.m_sCurrentModelName}\\PrintContour_Pattern";    //250114 NIS Pattern save and load
        private System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
        private System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
        //250122 NWT Parameter Exception
        private List<CExceptionData> exceptionDataList = new List<CExceptionData>();
        private ParameterExceptionHandler parameterExceptionHandler = null;

        private CLogger Logger = null;

        public FormTeachCogPM(MainForm _MainForm, FormTeachInspection _TeachMainForm, CLogger _logger)
        {
            InitializeComponent();
            TopLevel = false;
            TeachMainForm = _TeachMainForm;


            MainForm = _MainForm;
            exceptionDataList = theRecipe.m_listExceptionData;
            MainForm.SetEachControlResize(this);    //240801 NIS Conrol Resize]
            this.FormLocate(0);                     //240819 NIS Resize ImageDisplayCam0~4

            for (int i = 0; i < 4; ++i)
            {
                m_DisplayScreen[i] = this.Controls.Find($"ImageDisplayCam{i}", true)[0] as CogRecordDisplay;
            }

            Set_RadioButtonsList();
            m_nInspectSelected = (int)INSP_MODE.Contour;
            SetData();
            ToolListShow();

            TeachFormLocation = TeachMainForm.Get_TeachFormScreenLocation();  //240721 NIS Get TeachFormScreen Location
            for (int i = 0; i < 4; i++)
            {
                //CalibPointFromPix[i] = ((CogCalibCheckerboardTool)((CogToolBlock)theRecipe.CalibrationToolBlock.Tools[i]).Tools[0]).Calibration.GetComputedUncalibratedFromCalibratedTransform() as CogTransform2DLinear; //임시 주석
            }

            Logger = _logger;
        }

        private void btn_LogicToolBlock_Click(object sender, EventArgs e)   //240720 NIS Display ToolBlockEditPage
        {
            _ToolBlock = theRecipe.m_CogInspToolBlock[m_nSelectCam];

            ToolBlockEdit ToolEdit = new ToolBlockEdit(m_nInspectSelected, _ToolBlock); //240703 NIS 실행한 TeachMode에 따라서 ToolBlockEdit 타이틀 수정
            ToolEdit.Location = TeachFormLocation;
            ToolEdit.ShowDialog();
            
        }

        private void btn_Run_Click(object sender, EventArgs e)
        {
            if (InspectionImage != null)
            {

                ICogTool tool = m_TeachingToolList[m_nToolSelected];

                if (tool.Name.Contains("FindLine"))
                {
                    CogFindLineTool FindLine = tool as CogFindLineTool;

                    FindLineParameterApply(FindLine);
                }

                for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                {
                    ToolBlockRun(i, theRecipe.m_CogInspToolBlock[i], m_nInspectSelected);
                }

                if (tool.Name.Contains("FindLine"))
                {
                    CogFindLineTool FindLine = tool as CogFindLineTool;
                    FindLineShowTool(FindLine);
                }
                else if (tool.Name.Contains("PM"))
                {
                    CogPMAlignTool PMAlign = tool as CogPMAlignTool;
                    PMAlignShowTool(PMAlign);
                }

                TeachInspectionRun();
            }
        }

        private void Reverse_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            CogFindLineTool FindLine = m_TeachingToolList[m_nToolSelected] as CogFindLineTool;
            FindLine_Reverse(FindLine);
        }

        private void btn_FitCaliper_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            CogFindLineTool FindLine = m_TeachingToolList[m_nToolSelected] as CogFindLineTool;

            FindLine_CaliperFit(FindLine);
        }

        private void ToolList_SelectedIndexChanged(object sender, EventArgs e)
        {
            TeachingPopupAllClose();        //240722 NIS Hide FindLinePanel and PMAlignPanel

            m_nToolSelected = (int)lib_ToolList.SelectedIndex;

            ICogTool SelectTool = m_TeachingToolList[m_nToolSelected];

            if (SelectTool.Name.Contains("FindLine"))//  Tool 이름으로 구분하여 Parameter 표시
            {
                CogFindLineTool FindLine = SelectTool as CogFindLineTool;

                SetTextFindLineData(FindLine);
                FindLineShowTool(FindLine);
            }
            else if (SelectTool.Name.Contains("PM"))// Tool 이름으로 구분하여 Parameter 표시
            {
                CogPMAlignTool PMAlign = SelectTool as CogPMAlignTool;

                PMAlignShowTool(PMAlign);

                m_TeachingToolList[m_nToolSelected] = PMAlign;
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
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Contour ROI Teaching Cam1 Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_ONE;
                    }
                    else if (RadioButtons[DEF_SYSTEM.CAM_TWO].Name == rdb.Name)
                    {
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Contour ROI Teaching Cam2 Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_TWO;
                    }
                    else if (RadioButtons[DEF_SYSTEM.CAM_THREE].Name == rdb.Name)
                    {
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Contour ROI Teaching Cam3 Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_THREE;
                    }
                    else if (RadioButtons[DEF_SYSTEM.CAM_FOUR].Name == rdb.Name)
                    {
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Contour ROI Teaching Cam4 Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_FOUR;
                    }

                    ToolListShow();

                    //240709 NIS Cam 변경 시 마다 Inspection Display
                    TeachInspectionRun();
                }
            }
            catch (Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Form Print Contour Exception Catch : {ex.Message}");
            }
        }

        private void ApplyAndSave_Click(object sender, EventArgs e)
        {
            Run_ApplyAndSave();
        }

        private void Run_ApplyAndSave()
        {
            ICogTool tool = m_TeachingToolList[m_nToolSelected];

            if (tool.Name.Contains("FindLine")) // Tool 이름으로 구분하여 Parameter Apply
            {
                CogFindLineTool FindLine = tool as CogFindLineTool;
                FindLineParameterApply(FindLine);
            }
            else if (tool.Name.Contains("PM"))
            {
                CogPMAlignTool PMAlign = tool as CogPMAlignTool;
            }

            theRecipe.ToolSave(theRecipe.m_sCurrentModelName);
        }

        private void ToolListShow()   //240720 NIS Set ToolList
        {
            m_TeachingToolList.Clear();
            lib_ToolList.Items.Clear();

            if (theRecipe.m_CogInspToolBlock[m_nSelectCam] == null)
                return;

            for (int i = 0; i < theRecipe.m_CogInspToolBlock[m_nSelectCam].Tools.Count; i++)
            {
                ICogTool tool = theRecipe.m_CogInspToolBlock[m_nSelectCam].Tools[i];
                string sTemp = tool.Name;

                lib_ToolList.Font = font;
                lib_ToolList.Items.Add(sTemp);
                m_TeachingToolList.Add(tool);
            }
            lib_ToolList.SetSelected(0, true);
        }

        private void FindLineParameterApply(CogFindLineTool _pFindLine) //240720 NIS Apply parmas
        {
            try
            {
                if (int.Parse(txt_NumofCaliper.Text) < 2)
                {
                    MessageBox.Show("Num of Caliper must be greater than 2");

                    return;
                }
                else
                {
                    _pFindLine.RunParams.NumCalipers = int.Parse(txt_NumofCaliper.Text);    //231007 LYK 캘리퍼 수
                }

                _pFindLine.RunParams.CaliperSearchLength = double.Parse(txt_SearchLength.Text); //231007 LYK 캘리퍼 검색 길이

                _pFindLine.RunParams.CaliperProjectionLength = double.Parse(txt_ProjectionLength.Text); //231007 LYK 프로젝션 길이

                _pFindLine.RunParams.NumToIgnore = int.Parse(txt_NumofIgnore.Text); //231007 LYK 무시할 숫자

                int nDirection = cbo_SearchDirection.SelectedIndex;
                if (nDirection == 0)             //231007 LYK 밝은 곳에서 어두운 곳
                    _pFindLine.RunParams.CaliperRunParams.Edge0Polarity = CogCaliperPolarityConstants.LightToDark;
                else if (nDirection == 1)        //231007 LYK 어두운곳에서 밝은 곳
                    _pFindLine.RunParams.CaliperRunParams.Edge0Polarity = CogCaliperPolarityConstants.DarkToLight;
                else if (nDirection == 2)           //231007 LYK 임의의 극성
                    _pFindLine.RunParams.CaliperRunParams.Edge0Polarity = CogCaliperPolarityConstants.DontCare;

                CogCaliperScorers cogCaliperScorers = _pFindLine.RunParams.CaliperRunParams.SingleEdgeScorers;
                CogCaliperScorerContrast cogCaliperScorerContrast = cogCaliperScorers[0] as CogCaliperScorerContrast;

                double CaliperScore = 0.0, NonUse1 = 0.0, NonUse2 = 0.0, NonUse3 = 0.0, NonUse4 = 0.0;
                cogCaliperScorerContrast.GetXYParameters(out CaliperScore, out NonUse1, out NonUse2, out NonUse3, out NonUse4);

                int nSetScore = int.Parse(txt_Score.Text);
                int nGetScore = (int)CaliperScore;

                if (nSetScore != nGetScore)
                {
                    cogCaliperScorerContrast.SetXYParameters(nSetScore, NonUse1, NonUse2, NonUse3, NonUse4);    //231007 LYK 캘리퍼 스코어
                }

                _pFindLine.RunParams.CaliperRunParams.FilterHalfSizeInPixels = int.Parse(txt_CaliperFilter.Text);   //231007 LYK 필터 절반 픽셀

                _pFindLine.RunParams.CaliperRunParams.ContrastThreshold = double.Parse(txt_CaliperContrast.Text);   //231007 LYK 대비 임계값

                CogRecord CurrRecord = new CogRecord();
                CurrRecord.ContentType = typeof(ICogImage);
                CurrRecord.Content = _pFindLine.InputImage;
                CurrRecord.Annotation = "InputImage";
                CurrRecord.RecordKey = "InputImage";
                CurrRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;

                CurrRecord.SubRecords.Add(_pFindLine.CreateCurrentRecord());
                CurrentImageDisplay.Record = CurrRecord;
                CurrentImageDisplay.Fit();
            }
            catch (Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"FindLine Tool Save Error Catch : {e.Message}");
            }
        }
        private void ToolBlockRun(int _nCamNum, CogToolBlock _toolBlock, int _nMode)    //240720 NIS ToolBlock Run
        {
            _toolBlock.Inputs[0].Value = InspectionImage;
            _toolBlock.Run();
        }

        private void FindLine_Reverse(CogFindLineTool _pFindLine)
        {
            double dDirectionValue = 0.0;
            dDirectionValue = _pFindLine.RunParams.CaliperSearchDirection;
            _pFindLine.RunParams.CaliperSearchDirection = dDirectionValue *= -1;
        }

        private void FindLine_CaliperFit(CogFindLineTool _pFindLine)
        {
            int nImgWidth = _pFindLine.InputImage.Width;
            int nImgHeight = _pFindLine.InputImage.Height;
            double dChanged_StartX = 0.0, dChanged_StartY = 0.0, dChanged_EndX = 0.0, dChanged_EndY = 0.0;

            CogTransform2DLinear Linear = _pFindLine.InputImage.GetTransform(_pFindLine.RunParams.ExpectedLineSegment.SelectedSpaceName, "#") as CogTransform2DLinear;
            Linear.MapPoint(nImgWidth / 4, nImgHeight / 2, out dChanged_StartX, out dChanged_StartY);
            Linear.MapPoint(nImgWidth / 4 * 3, nImgHeight / 2, out dChanged_EndX, out dChanged_EndY);

            _pFindLine.RunParams.ExpectedLineSegment.StartX = dChanged_StartX;
            _pFindLine.RunParams.ExpectedLineSegment.StartY = dChanged_StartY;
            _pFindLine.RunParams.ExpectedLineSegment.EndX = dChanged_EndX;
            _pFindLine.RunParams.ExpectedLineSegment.EndY = dChanged_EndY;
        }
        private void SetTextFindLineData(CogFindLineTool _pFindLine)
        {
            txt_NumofCaliper.Text = _pFindLine.RunParams.NumCalipers.ToString();                        //231007 LYK 캘리퍼 수

            txt_SearchLength.Text = _pFindLine.RunParams.CaliperSearchLength.ToString();                //231007 LYK 캘리퍼 검색 길이

            txt_ProjectionLength.Text = _pFindLine.RunParams.CaliperProjectionLength.ToString();        //231007 LYK 캘리퍼 프로젝션 길이

            txt_NumofIgnore.Text = _pFindLine.RunParams.NumToIgnore.ToString();                         //231007 LYK 무시할 숫자

            int nDirection = 0;

            if (_pFindLine.RunParams.CaliperRunParams.Edge0Polarity == CogCaliperPolarityConstants.LightToDark)             //231007 LYK 밝은 곳에서 어두운 곳
                nDirection = 0;
            else if (_pFindLine.RunParams.CaliperRunParams.Edge0Polarity == CogCaliperPolarityConstants.DarkToLight)        //231007 LYK 어두운곳에서 밝은 곳
                nDirection = 1;
            else if (_pFindLine.RunParams.CaliperRunParams.Edge0Polarity == CogCaliperPolarityConstants.DontCare)           //231007 LYK 임의의 극성
                nDirection = 2;

            cbo_SearchDirection.SelectedIndex = nDirection;

            CogCaliperScorers cogCaliperScorers = _pFindLine.RunParams.CaliperRunParams.SingleEdgeScorers;
            CogCaliperScorerContrast cogCaliperScorerContrast = cogCaliperScorers[0] as CogCaliperScorerContrast;

            double CaliperScore = 0.0, NonUse1 = 0.0, NonUse2 = 0.0, NonUse3 = 0.0, NonUse4 = 0.0;
            cogCaliperScorerContrast.GetXYParameters(out CaliperScore, out NonUse1, out NonUse2, out NonUse3, out NonUse4); //231007 LYK 캘리퍼 스코어

            txt_Score.Text = CaliperScore.ToString();

            txt_CaliperFilter.Text = _pFindLine.RunParams.CaliperRunParams.FilterHalfSizeInPixels.ToString();   //231007 LYK 필터 절반 픽셀

            txt_CaliperContrast.Text = _pFindLine.RunParams.CaliperRunParams.ContrastThreshold.ToString();      //231007 LYK 대비 임계값
        }
        private void FindLineShowTool(CogFindLineTool _pFindLine)
        {
            CogRecord CurrRecord = new CogRecord();
            CurrRecord.ContentType = typeof(ICogImage);
            CurrRecord.Content = _pFindLine.InputImage;
            CurrRecord.Annotation = "InputImage";
            CurrRecord.RecordKey = "InputImage";
            CurrRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;

            CurrRecord.SubRecords.Add(_pFindLine.CreateCurrentRecord());
            CurrentImageDisplay.Record = CurrRecord;
            CurrentImageDisplay.Fit();

            CogRecord LastRecord = new CogRecord();
            LastRecord.ContentType = typeof(ICogImage);
            LastRecord.Content = _pFindLine.InputImage;
            LastRecord.Annotation = "InputImage";
            LastRecord.RecordKey = "InputImage";
            LastRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;

            LastRecord.SubRecords.Add(_pFindLine.CreateLastRunRecord());
            LastRunImageDisplay.Record = LastRecord;
            LastRunImageDisplay.Fit();

            Panel_FindLine.Visible = true;
        }
        private void TeachInspectionRun()   //240709 NIS Cam 변경 시 마다 Inspection Display
        {
            PrintCreatLabeling(InspectionImage); // Print Contour 전용 Teaching Page Graphic 출력

            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Controller Reset");
        }
        private void SetData()          //240720 NIS Set SearchDirection combobox
        {
            cbo_SearchDirection.Font = ComboBoxFont;
            cbo_SearchDirection.Items.Add("Light to Dark Polarity");
            cbo_SearchDirection.Items.Add("Dark to Light Polarity");
            cbo_SearchDirection.Items.Add("Random Polarity");
        }
        private void TeachingPopupAllClose()
        {
            Panel_FindLine.Visible = false;
            Panel_PMAlign.Visible = false;
            Panel_Blob.Visible = false;
        }
        private void PMAlignShowTool(CogPMAlignTool _pPMAlign)
        {
            if (_pPMAlign.SearchRegion != null) // 검사 영역이 존재 하면 체크박스 체크
                cb_RegionUse.Checked = true;

            rb_Pattern.Checked = true; //PM Align Tool 기본 표시는 Pattern 으로 표시 

            CogRecord CurrRecord = new CogRecord();
            CurrRecord.ContentType = typeof(ICogImage);
            CurrRecord.Content = _pPMAlign.Pattern.TrainImage;
            CurrRecord.Annotation = "InputImage";
            CurrRecord.RecordKey = "InputImage";
            CurrRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;

            CurrRecord.SubRecords.Add(_pPMAlign.CreateCurrentRecord().SubRecords[1]);
            CurrentImageDisplay.Record = CurrRecord;
            CurrentImageDisplay.Fit();

            CogRecord LastRecord = new CogRecord();
            LastRecord.ContentType = typeof(ICogImage);
            LastRecord.Content = _pPMAlign.InputImage;
            LastRecord.Annotation = "InputImage";
            LastRecord.RecordKey = "InputImage";
            LastRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;

            LastRecord.SubRecords.Add(_pPMAlign.CreateLastRunRecord());
            LastRunImageDisplay.Record = LastRecord;
            LastRunImageDisplay.Fit();

            CogRecord TrainRecord = new CogRecord();

            if (_pPMAlign.Pattern.Trained) 
            {
                TrainRecord.ContentType = typeof(ICogImage);
                TrainRecord.Content = _pPMAlign.Pattern.GetTrainedPatternImage();
                TrainRecord.Annotation = "TrainImage";
                TrainRecord.RecordKey = "TrainImage";
                TrainRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;
                TrainRecord.SubRecords.Add(_pPMAlign.CreateCurrentRecord().SubRecords[1]);

                try
                {
                    if (_pPMAlign.RunStatus.Result != CogToolResultConstants.Error && _pPMAlign.Results.Count == 1)
                        txt_GetScore_Pattern.Text = (_pPMAlign.Results[0].Score * 100).ToString("F1");
                }
                catch { }
            }
            TrainImageDisplay.Record = TrainRecord;
            TrainImageDisplay.Fit();

            Panel_PMAlign.Visible = true;
        }


        private void MultiPMAlignShowTool(CogPMAlignMultiTool _MultiPMTool) //241115 NIS PMlistshow in the MultiPMTool
        {

        }
        private void btn_FitPatternRegion_Click(object sender, EventArgs e)
        {
            ICogImage DisplayImage = CurrentImageDisplay.Image;

            CurrentImageDisplay.Fit();

            CogPMAlignTool _PMAlign = m_TeachingToolList[m_nToolSelected] as CogPMAlignTool;

            CogTransform2DLinear PixelFromScreen = new CogTransform2DLinear();
            CogRectangle TrainRec = new CogRectangle();
            CogRectangle SearchRec = new CogRectangle();
            DisplayImage.SelectedSpaceName = _PMAlign.InputImage.SelectedSpaceName;

            if (rb_Pattern.Checked)
            {
                PixelFromScreen = DisplayImage.GetTransform(_PMAlign.Pattern.TrainRegion.SelectedSpaceName, "#") as CogTransform2DLinear;
                TrainRec.SelectedSpaceName = _PMAlign.InputImage.SelectedSpaceName;

            }
            //else if (rb_Region.Checked)
            //{
            //    PixelFromScreen = DisplayImage.GetTransform(_PMAlign.InputImage.SelectedSpaceName, "@\\Fixture") as CogTransform2DLinear;
            //    SearchRec.SelectedSpaceName = "@\\Fixture";
            //}

            int Image_CenX = DisplayImage.Width / 2;
            int Image_CenY = DisplayImage.Height / 2;

            double ConvertWidth = DisplayImage.Width * PixelFromScreen.Scaling;
            double ConvertHeight = DisplayImage.Height * PixelFromScreen.Scaling;
            double ConvertCenX = 0;
            double ConvertCenY = 0;

            PixelFromScreen.MapPoint((double)Image_CenX, (double)Image_CenY, out ConvertCenX, out ConvertCenY);

            if (rb_Pattern.Checked)
            {
                TrainRec.GraphicDOFEnable = CogRectangleDOFConstants.All;
                TrainRec.GraphicDOFEnableBase = CogGraphicDOFConstants.All;
                TrainRec.Interactive = true;

                if (DisplayImage.Width >= DisplayImage.Height) // 이미지 정방향 
                {
                    TrainRec.X = ConvertCenX - ConvertWidth / 4;
                    TrainRec.Y = ConvertCenY - ConvertHeight / 4;
                    TrainRec.Width = ConvertWidth / 2;
                    TrainRec.Height = ConvertHeight / 2;

                    _PMAlign.Pattern.TrainRegion.FitToBoundingBox(TrainRec);
                    _PMAlign.Pattern.Origin.TranslationX = ConvertCenX;
                    _PMAlign.Pattern.Origin.TranslationY = ConvertCenY;
                }
                else
                {
                    TrainRec.X = ConvertCenY - ConvertHeight / 4;
                    TrainRec.Y = ConvertCenX - ConvertWidth / 4;
                    TrainRec.Width = ConvertHeight / 2;
                    TrainRec.Height = ConvertWidth / 2;

                    _PMAlign.Pattern.TrainRegion.FitToBoundingBox(TrainRec);
                    _PMAlign.Pattern.Origin.TranslationX = ConvertCenY;
                    _PMAlign.Pattern.Origin.TranslationY = ConvertCenX;
                }
            }
            else if (rb_Region.Checked)
            {
                SearchRec.GraphicDOFEnable = CogRectangleDOFConstants.All;
                SearchRec.GraphicDOFEnableBase = CogGraphicDOFConstants.All;
                SearchRec.Interactive = true;

                if (DisplayImage.Width >= DisplayImage.Height) // 이미지 정방향 
                {
                    SearchRec.X = ConvertCenX - ConvertWidth / 4;
                    SearchRec.Y = ConvertCenY - ConvertHeight / 4;
                    SearchRec.Width = ConvertWidth / 2;
                    SearchRec.Height = ConvertHeight / 2;

                    _PMAlign.SearchRegion = SearchRec;
                    _PMAlign.SearchRegion.FitToBoundingBox(SearchRec);
                }
                else
                {
                    SearchRec.X = ConvertCenY - ConvertHeight / 4;
                    SearchRec.Y = ConvertCenX - ConvertWidth / 4;
                    SearchRec.Width = ConvertHeight / 2;
                    SearchRec.Height = ConvertWidth / 2;

                    _PMAlign.SearchRegion = SearchRec;
                    _PMAlign.SearchRegion.FitToBoundingBox(SearchRec);
                }
            }
            CurrentImageDisplay.Fit();
        }
        private void btn_FitPatternPoint_Click(object sender, EventArgs e)
        {
            CogPMAlignTool _PMAlign = m_TeachingToolList[m_nToolSelected] as CogPMAlignTool;
            CogRectangleAffine rec = new CogRectangleAffine();
            rec.SelectedSpaceName = _PMAlign.Pattern.TrainRegion.SelectedSpaceName;

            rec = (CogRectangleAffine)_PMAlign.Pattern.TrainRegion;

            _PMAlign.Pattern.Origin.TranslationX = rec.CenterX; 
            _PMAlign.Pattern.Origin.TranslationY = rec.CenterY;
        }

        private void btn_TrainImageUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                CogPMAlignTool _PMAlign = m_TeachingToolList[m_nToolSelected] as CogPMAlignTool;

                //_PMAlign.Pattern.TrainImage = _PMAlign.InputImage;

                _PMAlign.Pattern.TrainImage = CogSerializer.DeepCopyObject(_PMAlign.InputImage) as CogImage8Grey;

                //_PMAlign.Pattern.TrainImage = new CogImage8Grey(_PMAlign.InputImage as CogImage8Grey);
                _PMAlign.Pattern.Untrain();

                //_PMAlign.Pattern.TrainImage = ((CogToolBlock)theRecipe.ContourInspectToolBlock[m_nSelectCam].Tools["CorrectImage"]).Outputs[0].Value as CogImage8Grey;

                PatternShowTrainRegion(_PMAlign); 

                TrainImageDisplay.Record = null;
                TrainImageDisplay.Fit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"TrainImageUpdate Error : {ex.Message}");
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"TrainImageUpdate Error : {ex.Message}");
            }
        }

        private void btn_TrainPattern_Click(object sender, EventArgs e)
        {
            CogPMAlignTool _PMAlign = m_TeachingToolList[m_nToolSelected] as CogPMAlignTool;

            try
            {
                _PMAlign.Pattern.Train();

                CogRecord TrainRecord = new CogRecord();

                if (_PMAlign.Pattern.Trained)
                {
                    TrainRecord.ContentType = typeof(ICogImage);
                    TrainRecord.Content = _PMAlign.Pattern.GetTrainedPatternImage();
                    TrainRecord.Annotation = "TrainImage";
                    TrainRecord.RecordKey = "TrainImage";
                    TrainRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;
                    TrainRecord.SubRecords.Add(_PMAlign.CreateCurrentRecord().SubRecords[1]);
                }
                TrainImageDisplay.Record = TrainRecord;
                TrainImageDisplay.Fit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Pattern Train Error : {ex.Message}");
            }
        }

        private void PMAlignOriginMove(int _moveX, int _moveY)
        {
            // Train 된 Origin 좌표를 이동시켜주는 내용 
            CogPMAlignTool _PMAlign = m_TeachingToolList[m_nToolSelected] as CogPMAlignTool;

            double OriginX = _PMAlign.Pattern.Origin.TranslationX;
            double OriginY = _PMAlign.Pattern.Origin.TranslationY;
            double PixelX, PixelY;

            CogTransform2DLinear PixelLinear = _PMAlign.Pattern.TrainImage.GetTransform("#", _PMAlign.Pattern.TrainImage.SelectedSpaceName) as CogTransform2DLinear;
            PixelLinear.MapPoint(OriginX, OriginY, out PixelX, out PixelY);

            PixelX += _moveX;
            PixelY += _moveY;

            double ConvertX, ConvertY;
            CogTransform2DLinear TrainLinear = _PMAlign.Pattern.TrainImage.GetTransform(_PMAlign.Pattern.TrainImage.SelectedSpaceName, "#") as CogTransform2DLinear;
            TrainLinear.MapPoint(PixelX, PixelY, out ConvertX, out ConvertY);

            _PMAlign.Pattern.Origin.TranslationX = ConvertX;
            _PMAlign.Pattern.Origin.TranslationY = ConvertY;
        }

        private void btn_PatternOriginUp_Click(object sender, EventArgs e)
        {
            PMAlignOriginMove(0, -1);
        }

        private void btn_PatternOriginDown_Click(object sender, EventArgs e)
        {
            PMAlignOriginMove(0, +1);
        }

        private void btn_PatternOriginLeft_Click(object sender, EventArgs e)
        {
            PMAlignOriginMove(-1, 0);
        }
        private void btn_PatternOriginRight_Click(object sender, EventArgs e)
        {
            PMAlignOriginMove(+1, 0);
        }

        private void rb_SelectPattern_Click(object sender, EventArgs e)
        {
            CogPMAlignTool _PMAlign = m_TeachingToolList[m_nToolSelected] as CogPMAlignTool;

            PatternShowTrainRegion(_PMAlign); 

        }

        private void rb_SelectRegion_Click(object sender, EventArgs e)
        {
            CogPMAlignTool _PMAlign = m_TeachingToolList[m_nToolSelected] as CogPMAlignTool;

            PatternShowSearchRegion(_PMAlign);  

            if (_PMAlign.SearchRegion != null)
                cb_RegionUse.Checked = true;
        }

        private void cb_RegionUse_CheckedChange(object sender, EventArgs e)
        {
            CogPMAlignTool _PMAlign = m_TeachingToolList[m_nToolSelected] as CogPMAlignTool;

            if (cb_RegionUse.Checked)
            {
                if (_PMAlign.SearchRegion == null)
                    PatternSearchRegionFit(_PMAlign); 
            }
            else
            {
                _PMAlign.SearchRegion = null;
            }

            PatternShowSearchRegion(_PMAlign);

        }

        private void btn_FitSearchRegion_Click(object sender, EventArgs e)
        {
            CogPMAlignTool _PMAlign = m_TeachingToolList[m_nToolSelected] as CogPMAlignTool;

            if (_PMAlign.SearchRegion == null)
                PatternSearchRegionFit(_PMAlign);

            PatternShowSearchRegion(_PMAlign);

        }

        private void PatternSearchRegionFit(CogPMAlignTool _PMAlign)
        {
            ICogImage DisplayImage = CurrentImageDisplay.Image;
            CogRectangle SearchRec = new CogRectangle();

            SearchRec.SelectedSpaceName = _PMAlign.InputImage.SelectedSpaceName;
            DisplayImage.SelectedSpaceName = _PMAlign.InputImage.SelectedSpaceName;

            int Image_CenX = DisplayImage.Width / 2;
            int Image_CenY = DisplayImage.Height / 2;

            CogTransform2DLinear PixelFromScreen = DisplayImage.GetTransform(_PMAlign.Pattern.TrainRegion.SelectedSpaceName, "#") as CogTransform2DLinear;

            double ConvertWidth = DisplayImage.Width * PixelFromScreen.Scaling;
            double ConvertHeight = DisplayImage.Height * PixelFromScreen.Scaling;
            double ConvertCenX = 0;
            double ConvertCenY = 0;

            PixelFromScreen.MapPoint((double)Image_CenX, (double)Image_CenY, out ConvertCenX, out ConvertCenY);

            SearchRec.SelectedSpaceName = _PMAlign.Pattern.TrainRegion.SelectedSpaceName;

            if (DisplayImage.Width >= DisplayImage.Height) // 이미지 정방향 
            {
                SearchRec.X = ConvertCenX - ConvertWidth / 4;
                SearchRec.Y = ConvertCenY - ConvertHeight / 4;
                SearchRec.Width = ConvertWidth / 2;
                SearchRec.Height = ConvertHeight / 2;

                _PMAlign.SearchRegion.FitToBoundingBox(SearchRec);
            }
            else
            {
                SearchRec.X = ConvertCenY - ConvertHeight / 4;
                SearchRec.Y = ConvertCenX - ConvertWidth / 4;
                SearchRec.Width = ConvertHeight / 2;
                SearchRec.Height = ConvertWidth / 2;

                _PMAlign.SearchRegion.FitToBoundingBox(SearchRec);
            }
        }

        private void PatternShowSearchRegion(CogPMAlignTool _PMAlign)
        {
            CurrentImageDisplay.Fit();

            CogRecord CurrRecord = new CogRecord();
            CurrRecord.ContentType = typeof(ICogImage);
            CurrRecord.Content = _PMAlign.InputImage;
            CurrRecord.Annotation = "InputImage";
            CurrRecord.RecordKey = "InputImage";
            CurrRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;

            CurrRecord.SubRecords.Add(_PMAlign.CreateCurrentRecord().SubRecords[0]);
            CurrentImageDisplay.Record = CurrRecord;
            CurrentImageDisplay.Fit();
        }

        private void PatternShowTrainRegion(CogPMAlignTool _PMAlign)
        {
            CogRecord CurrRecord = new CogRecord();
            CurrRecord.ContentType = typeof(ICogImage);
            CurrRecord.Content = _PMAlign.Pattern.TrainImage;
            CurrRecord.Annotation = "InputImage";
            CurrRecord.RecordKey = "InputImage";
            CurrRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;

            CurrRecord.SubRecords.Add(_PMAlign.CreateCurrentRecord().SubRecords[1]);
            CurrentImageDisplay.Record = CurrRecord;
            CurrentImageDisplay.Fit();
        }
        
        private void PrintCreatLabeling(CogImage8Grey _Image)
        {
            Font Labelfont = new Font("Calibri", 10, FontStyle.Regular);

            List<CogRecord> _RecordList = new List<CogRecord>(); // Vision Pro Record
            List<CogGraphicCollection> _GraphicList = new List<CogGraphicCollection>(); // Graphic

            try
            {
                CogRecord MainRecord = new CogRecord("Main", _Image.GetType(), CogRecordUsageConstants.Result, true, _Image, "Main");
                bool IsError = false;

                for (int i = 0; i < theRecipe.m_CogInspToolBlock[m_nSelectCam].Tools.Count; i++)
                {
                    if (theRecipe.m_CogInspToolBlock[m_nSelectCam].Tools[i].Name.Contains("PMAlignTool"))
                    {
                        CogPMAlignTool tool = theRecipe.m_CogInspToolBlock[m_nSelectCam].Tools[i] as CogPMAlignTool;
                        MainRecord.SubRecords.Add(tool.CreateLastRunRecord());
                    }
                }
                m_DisplayScreen[m_nSelectCam].Record = MainRecord;

                m_DisplayScreen[m_nSelectCam].Fit();


                /* 임시 주석
                //250301 NIS 데이터 집계
                for (int i = 0; i <= DEF_SYSTEM.CAM_FOUR; i++)
                {
                    for (int j = 0; j < Cam_WaferPrint_PixXY.GetLength(1); j++)
                    {
                        Cam_WaferPrint_PixXY[i, j] = (PointF)((CogToolBlock)theRecipe.ContourInspectToolBlock.Tools[i]).Outputs[DataKeys[j]].Value;
                        Cam_WaferPrint_PMScore[i, j] = (double)((CogToolBlock)theRecipe.ContourInspectToolBlock.Tools[i]).Outputs[DataKeys[j + 4]].Value;
                    }
                }

                //250301 NIS WaferPos at Corner
                if (theRecipe.m_nWaferPos == 0)
                {
                    for (int i = 0; i < WaferPoints.Length; i++)    //Convert Calib
                    {
                        theMainSystem.CogCalibLinear_Invert[i].MapPoint(Cam_WaferPrint_PixXY[i, WAFER].X, Cam_WaferPrint_PixXY[i, WAFER].Y, out double calibX, out double calibY);
                        WaferPoints[i].Pos = new PointF((float)calibX, (float)calibY);
                    }
                }
                else //250301 NIS WaferPos at Center
                {
                    for (int i = 0; i <= DEF_SYSTEM.CAM_FOUR; i++)
                    {
                        //TOP
                        if (i == DEF_SYSTEM.CAM_ONE || i == DEF_SYSTEM.CAM_TWO)
                        {
                            for (int j = WAFER; j < 2; j++)
                                SetPointPosition(ref WaferPoints[TOP], Cam_WaferPrint_PixXY[i, j], Cam_WaferPrint_PMScore[i, j], nCenterPixel, TOP, i);
                        }
                        //LEFT
                        if (i == DEF_SYSTEM.CAM_ONE || i == DEF_SYSTEM.CAM_THREE)
                        {
                            for (int j = WAFER; j < 2; j++)
                                SetPointPosition(ref WaferPoints[LEFT], Cam_WaferPrint_PixXY[i, j], Cam_WaferPrint_PMScore[i, j], nCenterPixel, LEFT, i);
                        }
                        //BTM
                        if (i == DEF_SYSTEM.CAM_THREE || i == DEF_SYSTEM.CAM_FOUR)
                        {
                            for (int j = WAFER; j < 2; j++)
                                SetPointPosition(ref WaferPoints[BTM], Cam_WaferPrint_PixXY[i, j], Cam_WaferPrint_PMScore[i, j], nCenterPixel, BTM, i);
                        }
                        //RIGHT
                        if (i == DEF_SYSTEM.CAM_TWO || i == DEF_SYSTEM.CAM_FOUR)
                        {
                            for (int j = WAFER; j < 2; j++)
                                SetPointPosition(ref WaferPoints[RIGHT], Cam_WaferPrint_PixXY[i, j], Cam_WaferPrint_PMScore[i, j], nCenterPixel, RIGHT, i);
                        }
                    }
                }
                //250301 NIS PrintPos at Corner
                if (theRecipe.m_nPrintPos == 0)
                {
                    for (int i = 0; i < PrintPoints.Length; i++)    //Convert Calib
                    {
                        theMainSystem.CogCalibLinear_Invert[i].MapPoint(Cam_WaferPrint_PixXY[i, PRINT].X, Cam_WaferPrint_PixXY[i, PRINT].Y, out double calibX, out double calibY);
                        PrintPoints[i].Pos = new PointF((float)calibX, (float)calibY);
                    }
                }
                else //250301 NIS PrintPos at Center
                {
                    for (int i = 0; i <= DEF_SYSTEM.CAM_FOUR; i++)
                    {
                        //TOP
                        if (i == DEF_SYSTEM.CAM_ONE || i == DEF_SYSTEM.CAM_TWO)
                        {
                            for (int j = PRINT; j < 4; j++)
                                SetPointPosition(ref PrintPoints[TOP], Cam_WaferPrint_PixXY[i, j], Cam_WaferPrint_PMScore[i, j], nCenterPixel, TOP, i);
                        }
                        //LEFT
                        if (i == DEF_SYSTEM.CAM_ONE || i == DEF_SYSTEM.CAM_THREE)
                        {
                            for (int j = PRINT; j < 4; j++)
                                SetPointPosition(ref PrintPoints[LEFT], Cam_WaferPrint_PixXY[i, j], Cam_WaferPrint_PMScore[i, j], nCenterPixel, LEFT, i);
                        }
                        //BTM
                        if (i == DEF_SYSTEM.CAM_THREE || i == DEF_SYSTEM.CAM_FOUR)
                        {
                            for (int j = PRINT; j < 4; j++)
                                SetPointPosition(ref PrintPoints[BTM], Cam_WaferPrint_PixXY[i, j], Cam_WaferPrint_PMScore[i, j], nCenterPixel, BTM, i);
                        }
                        //RIGHT
                        if (i == DEF_SYSTEM.CAM_TWO || i == DEF_SYSTEM.CAM_FOUR)
                        {
                            for (int j = PRINT; j < 4; j++)
                                SetPointPosition(ref PrintPoints[RIGHT], Cam_WaferPrint_PixXY[i, j], Cam_WaferPrint_PMScore[i, j], nCenterPixel, RIGHT, i);
                        }
                    }
                }
                if (theRecipe.m_nWaferPos == 0)
                {
                    ContourCrossLine[0].SetFromStartXYEndXY(WaferPoints[0].Pos.X, WaferPoints[0].Pos.Y, WaferPoints[3].Pos.X, WaferPoints[3].Pos.Y);
                    ContourCrossLine[1].SetFromStartXYEndXY(WaferPoints[1].Pos.X, WaferPoints[1].Pos.Y, WaferPoints[2].Pos.X, WaferPoints[2].Pos.Y);
                }
                else
                {
                    ContourCrossLine[0].SetFromStartXYEndXY(WaferPoints[TOP].Pos.X, WaferPoints[TOP].Pos.Y, WaferPoints[BTM].Pos.X, WaferPoints[BTM].Pos.Y);
                    ContourCrossLine[1].SetFromStartXYEndXY(WaferPoints[LEFT].Pos.X, WaferPoints[LEFT].Pos.Y, WaferPoints[RIGHT].Pos.X, WaferPoints[RIGHT].Pos.Y);
                }
                if (theRecipe.m_nPrintPos == 0)
                {
                    ContourCrossLine[2].SetFromStartXYEndXY(PrintPoints[0].Pos.X, PrintPoints[0].Pos.Y, PrintPoints[3].Pos.X, PrintPoints[3].Pos.Y);
                    ContourCrossLine[3].SetFromStartXYEndXY(PrintPoints[1].Pos.X, PrintPoints[1].Pos.Y, PrintPoints[2].Pos.X, PrintPoints[2].Pos.Y);
                }
                else
                {
                    ContourCrossLine[2].SetFromStartXYEndXY(PrintPoints[TOP].Pos.X, PrintPoints[TOP].Pos.Y, PrintPoints[BTM].Pos.X, PrintPoints[BTM].Pos.Y);
                    ContourCrossLine[3].SetFromStartXYEndXY(PrintPoints[LEFT].Pos.X, PrintPoints[LEFT].Pos.Y, PrintPoints[RIGHT].Pos.X, PrintPoints[RIGHT].Pos.Y);
                }
                cMath.GetIntersectLineLine(ContourCrossLine[0], ContourCrossLine[1], out double WaferInterX, out double WaferInterY, out double WaferAngle);
                cMath.GetIntersectLineLine(ContourCrossLine[2], ContourCrossLine[3], out double PrintInterX, out double PrintInterY, out double PrintAngle);

                for (int i = 0; i <= DEF_SYSTEM.CAM_FOUR; i++)
                {
                    WaferCenterX += WaferPoints[i].Pos.X / 4;
                    WaferCenterY += WaferPoints[i].Pos.Y / 4;
                    PrintCenterX += PrintPoints[i].Pos.X / 4;
                    PrintCenterY += PrintPoints[i].Pos.Y / 4;
                }
                WaferAngle = WaferAngle > 0 ? WaferAngle : -WaferAngle;
                PrintAngle = PrintAngle > 0 ? PrintAngle : -PrintAngle;
                double AlignX = Math.Round(WaferCenterX - PrintCenterX, 3) * theRecipe.m_dAlignDirection_XYT[0];
                double AlignY = Math.Round(WaferCenterY - PrintCenterY, 3) * theRecipe.m_dAlignDirection_XYT[1];
                double AlignT = Math.Round(WaferAngle - PrintAngle, 3) * theRecipe.m_dAlignDirection_XYT[2];

                for (int i = 0; i <= DEF_SYSTEM.CAM_FOUR; i++)
                {
                    _GraphicList.Add(new CogGraphicCollection());

                    for (int j = 0; j < 2; j++)
                    {
                        string _labeltextX = j == 0 ? $"{_key[j]} Point X : {WaferPoints[i].Pos.X.ToString("F3")}" : $"{_key[j]} Point X : {PrintPoints[i].Pos.X.ToString("F3")}";
                        string _labeltextY = j == 0 ? $"{_key[j]} Point Y : {WaferPoints[i].Pos.Y.ToString("F3")}" : $"{_key[j]} Point Y : {PrintPoints[i].Pos.Y.ToString("F3")}";

                        _GraphicList[i].Add(CreatCogLabel(_labeLPosX[j], _labelPosY[i * 2 + 0], _labeltextX, LabelColor[j], Labelfont));   //X좌표
                        _GraphicList[i].Add(CreatCogLabel(_labeLPosX[j], _labelPosY[i * 2 + 1], _labeltextY, LabelColor[j], Labelfont));   //Y좌표
                    }

                    theMainSystem.CogCalibLinear[i].MapPoint(WaferCenterX, WaferCenterY, out double WaferCenterPixX, out double WaferCenterPixY);
                    theMainSystem.CogCalibLinear[i].MapPoint(PrintCenterX, PrintCenterY, out double PrintCenterPixX, out double PrintCenterPixY);

                    //250304 NIS 교차점 그래픽 표기
                    if (theRecipe.m_nWaferPos == 0)
                    {
                        _GraphicList[i].Add(CreatCogMarker(Cam_WaferPrint_PixXY[i, WAFER].X, Cam_WaferPrint_PixXY[i, WAFER].Y, CogColorConstants.Blue, 50));
                        _GraphicList[i].Add(CreatCogSegment(Cam_WaferPrint_PixXY[i, WAFER].X, Cam_WaferPrint_PixXY[i, WAFER].Y, WaferCenterPixX, WaferCenterPixY, CogGraphicLineStyleConstants.Dot, CogColorConstants.Blue));
                        _GraphicList[i].Add(CreatCogMarker(WaferCenterPixX, WaferCenterPixY, CogColorConstants.Blue, 50));
                    }
                    else
                    {
                        theMainSystem.CogCalibLinear[i].MapPoint(WaferPoints[n_arrTopBtm[i]].Pos.X, WaferPoints[n_arrTopBtm[i]].Pos.Y, out double PixX1, out double PixY1);
                        theMainSystem.CogCalibLinear[i].MapPoint(WaferPoints[n_arrLeftRight[i]].Pos.X, WaferPoints[n_arrLeftRight[i]].Pos.Y, out double PixX2, out double PixY2);
                        theMainSystem.CogCalibLinear[i].MapPoint(WaferCenterX, WaferCenterY, out double PixX_Center, out double PixY_Center);
                        _GraphicList[i].Add(CreatCogMarker(PixX1, PixY1, CogColorConstants.Blue, 50));
                        _GraphicList[i].Add(CreatCogSegment(PixX1, PixY1, PixX_Center, PixY_Center, CogGraphicLineStyleConstants.Dot, CogColorConstants.Blue));
                        _GraphicList[i].Add(CreatCogMarker(PixX2, PixY2, CogColorConstants.Blue, 50));
                        _GraphicList[i].Add(CreatCogSegment(PixX2, PixY2, PixX_Center, PixY_Center, CogGraphicLineStyleConstants.Dot, CogColorConstants.Blue));
                        _GraphicList[i].Add(CreatCogMarker(PixX_Center, PixY_Center, CogColorConstants.Blue, 50));
                    }

                    //250304 NIS 교차점 그래픽 표기
                    if (theRecipe.m_nPrintPos == 0)
                    {
                        _GraphicList[i].Add(CreatCogMarker(Cam_WaferPrint_PixXY[i, PRINT].X, Cam_WaferPrint_PixXY[i, PRINT].Y, CogColorConstants.Orange, 50));
                        _GraphicList[i].Add(CreatCogSegment(Cam_WaferPrint_PixXY[i, PRINT].X, Cam_WaferPrint_PixXY[i, PRINT].Y, PrintCenterPixX, PrintCenterPixY, CogGraphicLineStyleConstants.Dot, CogColorConstants.Orange));
                        _GraphicList[i].Add(CreatCogMarker(PrintCenterPixX, PrintCenterPixY, CogColorConstants.Orange, 50));
                    }
                    else
                    {
                        theMainSystem.CogCalibLinear[i].MapPoint(PrintPoints[n_arrTopBtm[i]].Pos.X, PrintPoints[n_arrTopBtm[i]].Pos.Y, out double PixX1, out double PixY1);
                        theMainSystem.CogCalibLinear[i].MapPoint(PrintPoints[n_arrLeftRight[i]].Pos.X, PrintPoints[n_arrLeftRight[i]].Pos.Y, out double PixX2, out double PixY2);
                        theMainSystem.CogCalibLinear[i].MapPoint(PrintCenterX, PrintCenterY, out double PixX_Center, out double PixY_Center);
                        _GraphicList[i].Add(CreatCogMarker(PixX1, PixY1, CogColorConstants.Orange, 50));
                        _GraphicList[i].Add(CreatCogSegment(PixX1, PixY1, PixX_Center, PixY_Center, CogGraphicLineStyleConstants.Dot, CogColorConstants.Orange));
                        _GraphicList[i].Add(CreatCogMarker(PixX2, PixY2, CogColorConstants.Orange, 50));
                        _GraphicList[i].Add(CreatCogSegment(PixX2, PixY2, PixX_Center, PixY_Center, CogGraphicLineStyleConstants.Dot, CogColorConstants.Orange));
                        _GraphicList[i].Add(CreatCogMarker(PixX_Center, PixY_Center, CogColorConstants.Orange, 50));
                    }

                    CogRecord MainRecord = new CogRecord("Main", theMainSystem.Cameras[i].GetTeachingCogImage(m_nInspectSelected).GetType(), CogRecordUsageConstants.Result, false, theMainSystem.Cameras[i].GetTeachingCogImage(m_nInspectSelected) as CogImage8Grey, "Main");

                    if (i == 0) // 첫번째 카메라일 경우만 Align 값 표시 
                    {
                        Font Alignfont = new Font("Calibri", 10, FontStyle.Bold);

                        _GraphicList[i].Add(CreatCogLabel(800, 800, $"AlignX : {AlignX}", CogColorConstants.Green, Alignfont));
                        _GraphicList[i].Add(CreatCogLabel(2300, 800, $"AlignY : {AlignY}", CogColorConstants.Green, Alignfont));
                        _GraphicList[i].Add(CreatCogLabel(3800, 800, $"AlignT : {AlignT}", CogColorConstants.Green, Alignfont));
                    }

                    MainRecord.SubRecords.Add(new CogRecord("Graphic", typeof(CogGraphicCollection), CogRecordUsageConstants.Result, false, _GraphicList[i], "Graphic"));
                    _RecordList.Add(((CogToolBlock)theRecipe.ContourInspectToolBlock.Tools[i]).Outputs["Record"].Value as CogRecord);
                    MainRecord.SubRecords.Add(_RecordList[i]);

                    m_DisplayScreen[i].Image = theMainSystem.Cameras[i].GetTeachingCogImage(m_nInspectSelected);

                    // Contour Result Record
                    m_DisplayScreen[i].Record = MainRecord;
                    m_DisplayScreen[i].Fit();
                    MainRecord.SubRecords.Clear();
                }
                _GraphicList.Clear();
                */


            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 25.03.04 NIS Score을 비교해서 Contour 좌표 할당
        /// </summary>
        /// <param name="pointF"></param>
        /// <param name="score"></param>
        /// <param name="nCenterPixel"></param>
        /// <param name="POS"></param>
        /// <param name="CAM"></param>
        private void SetPointPosition( PointF pointF, double score, int nCenterPixel, int POS, int CAM)
        {
            /*  임시 주석
            if (POS == TOP)//TOP
            {
                if (pointF.Y != 0 && pointF.Y < nCenterPixel)
                {
                    if (contourPoint.Score < score)
                    {
                        theMainSystem.CogCalibLinear_Invert[CAM].MapPoint(pointF.X, pointF.Y, out double calibX, out double calibY);
                        contourPoint.Pos = new PointF((float)calibX, (float)calibY);
                        contourPoint.Score = score;
                    }
                }
            }
            else if (POS == LEFT)  //LEFT
            {
                if (pointF.X != 0 && pointF.X < nCenterPixel)
                {
                    if (contourPoint.Score < score)
                    {
                        theMainSystem.CogCalibLinear_Invert[CAM].MapPoint(pointF.X, pointF.Y, out double calibX, out double calibY);
                        contourPoint.Pos = new PointF((float)calibX, (float)calibY);
                        contourPoint.Score = score;
                    }
                }
            }
            else if (POS == BTM)  //BTM
            {
                if (pointF.Y != 0 && pointF.Y > nCenterPixel)
                {
                    if (contourPoint.Score < score)
                    {
                        theMainSystem.CogCalibLinear_Invert[CAM].MapPoint(pointF.X, pointF.Y, out double calibX, out double calibY);
                        contourPoint.Pos = new PointF((float)calibX, (float)calibY);
                        contourPoint.Score = score;
                    }
                }
            }
            else if (POS == RIGHT)  //RIGHT
            {
                if (pointF.X != 0 && pointF.X > nCenterPixel)
                {
                    if (contourPoint.Score < score)
                    {
                        theMainSystem.CogCalibLinear_Invert[CAM].MapPoint(pointF.X, pointF.Y, out double calibX, out double calibY);
                        contourPoint.Pos = new PointF((float)calibX, (float)calibY);
                        contourPoint.Score = score;
                    }
                }
            }
            */
        }

        private CogLineSegment CreatCogSegment(double _StartX, double _StartY, double _EndX, double _EndY, CogGraphicLineStyleConstants _LineStyle, CogColorConstants _Color)
        {
            CogLineSegment CreatSegment = new CogLineSegment();
            CreatSegment.SetStartEnd(_StartX, _StartY, _EndX, _EndY);
            CreatSegment.LineStyle = _LineStyle;
            CreatSegment.Color = _Color;
            CreatSegment.LineWidthInScreenPixels = 2;

            return CreatSegment;
        }

        private CogPointMarker CreatCogMarker(double _PosX, double _PosY, CogColorConstants _Color, int _Size)
        {
            CogPointMarker CreatMarker = new CogPointMarker();
            CreatMarker.X = _PosX;
            CreatMarker.Y = _PosY;
            CreatMarker.Color = _Color;
            CreatMarker.SizeInScreenPixels = _Size;

            return CreatMarker;
        }

        private CogGraphicLabel CreatCogLabel(double _PosX, double _PosY, string _txt, CogColorConstants _Color, Font _font)
        {
            CogGraphicLabel CreatLabel = new CogGraphicLabel();
            CreatLabel.X = _PosX;
            CreatLabel.Y = _PosY;
            CreatLabel.Text = _txt;
            CreatLabel.Color = _Color;
            CreatLabel.Font = _font;

            return CreatLabel;
        }
        public int Get_m_nSelectCam()
        {
            return m_nSelectCam;
        }
        private void Set_RadioButtonsList()
        {
            RadioButtons[0] = rdb_Cam1;
            RadioButtons[1] = rdb_Cam2;
            RadioButtons[2] = rdb_Cam3;
            RadioButtons[3] = rdb_Cam4;

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                RadioButtons[i].Visible = true;
            }
        }

        public void PrintCounterToolListShow()
        {
            ToolListShow();
        }

        private void FormLocate(int _type)
        {
            int width = ImageDisplayPanel.Width / 2;
            int height = ImageDisplayPanel.Height / 2;
            int length = width < height ? width : height;

            CogRecordDisplay[] bounds = new CogRecordDisplay[4]
            {
                ImageDisplayCam0, ImageDisplayCam1, ImageDisplayCam2, ImageDisplayCam3
            };

            
            //240815 NIS 분할된 Frame 위치와 비율 조정
            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; ++i)
            {
                if (i == 0)
                {
                    bounds[i].Location = new Point(width - length, 0);
                }
                else if (i == 1)
                {
                    bounds[i].Location = new Point(bounds[0].Location.X + bounds[0].Width, bounds[0].Location.Y);
                }
                else if (i == 2)
                {
                    bounds[i].Location = new Point(bounds[0].Location.X, bounds[0].Location.Y + bounds[0].Height);
                }
                else if (i == 3)
                {
                    bounds[i].Location = new Point(bounds[0].Location.X + bounds[0].Width, bounds[0].Location.Y + bounds[0].Height);
                }

                bounds[i].Width = DEF_SYSTEM.CAM_MAX_COUNT == 1 || i == DEF_SYSTEM.CAM_FIVE ? length * 2 : length;
                bounds[i].Height = DEF_SYSTEM.CAM_MAX_COUNT == 1 || i == DEF_SYSTEM.CAM_FIVE ? length * 2 : length;
                bounds[i].Visible = true;
            }
        }

        private void Reset_DisplayScreen()
        {
            for (int i = 0; i < m_DisplayScreen.Length; i++)
                m_DisplayScreen[i].Record = null;
        }


        //250114 NIS 패턴 저장
        private void btn_PM_Save_Click(object sender, EventArgs e)
        {
            try
            {
                m_nToolSelected = (int)lib_ToolList.SelectedIndex;  //현재 선택된 툴 인덱스 가져오기

                ICogTool SelectTool = m_TeachingToolList[m_nToolSelected];

                if (SelectTool.Name.Contains("PM"))     //PM이면 저장 실행
                {
                    if(!Directory.Exists(pattern_path))
                        Directory.CreateDirectory(pattern_path);

                    CogPMAlignTool PMAlign = SelectTool as CogPMAlignTool;

                    saveFileDialog.Filter = "Pattern Files (*.vpp)|*.vpp";

                    saveFileDialog.Title = "Save a Pattern File";
                    saveFileDialog.InitialDirectory = pattern_path;
                    DialogResult userInput = saveFileDialog.ShowDialog();

                    if (userInput == DialogResult.OK && saveFileDialog.FileName != "")
                    {
                        CogSerializer.SaveObjectToFile(PMAlign.Pattern, saveFileDialog.FileName);
                        MessageBox.Show("Pattern Save Successful", "Pattern Save", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"PrintContour Pattern Save Error : {ex.Message}");
                MessageBox.Show("Pattern Save Failed", "Pattern Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //250114 NIS 패턴 로드
        private void btn_PM_Load_Click(object sender, EventArgs e)
        {
            try
            {
                m_nToolSelected = (int)lib_ToolList.SelectedIndex;  //현재 선택된 툴 인덱스 가져오기

                ICogTool SelectTool = m_TeachingToolList[m_nToolSelected];

                if (SelectTool.Name.Contains("PM")) //PM이면 로드 실행
                {
                    if (!Directory.Exists(pattern_path))
                        Directory.CreateDirectory(pattern_path);

                    CogPMAlignTool PMAlign = SelectTool as CogPMAlignTool;

                    openFileDialog.Filter = "Pattern Files (*.vpp)|*.vpp";

                    openFileDialog.Title = "Load a Pattern File";
                    openFileDialog.InitialDirectory = pattern_path;
                    DialogResult userInput = openFileDialog.ShowDialog();

                    if (userInput == DialogResult.OK && openFileDialog.FileName != "")
                    {
                        PMAlign.Pattern = CogSerializer.LoadObjectFromFile(openFileDialog.FileName) as CogPMAlignPattern;
                        PMAlignShowTool(PMAlign);   //250114 NIS 패턴 로드 후 그래픽 갱신
                        MessageBox.Show("Pattern Load Successful", "Pattern Load", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"PrintContour Pattern Load Error : {ex.Message}");
                MessageBox.Show("Pattern Load Failed", "Pattern Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormTeachInspectionPrintContour_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                Reset_DisplayScreen();
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

        private void btn_ImgLoad_Click(object sender, EventArgs e)
        {
            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Image Load Run");

            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Bitmap files (*.bmp)|*.bmp";
                fileDialog.Multiselect = false;

                DialogResult dr = fileDialog.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    try
                    {
                        InspectionImage = new CogImage8Grey(new Bitmap(fileDialog.FileNames[0]));
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Image Load Completed");
                    }
                    catch (Exception ex)
                    {
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Image Load Failed : {ex.Message}");
                    }
                }
                else if (dr == DialogResult.Cancel)
                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Image Load Cancel");
            }
        }
    }
}