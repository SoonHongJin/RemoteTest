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

using Core.Utility;
using Core.Function;

using static Core.Program;


using System.Drawing.Drawing2D;

using Insnex.Vision2D.Core;
using Insnex.Vision2D.Common;
using Insnex.Vision2D.ToolBlock;
using Insnex.Vision2D.ImageProcessing;
using Insnex.Vision2D.Finder;
using Insnex.Vision2D.Intersection;
using Insnex.Vision2D.CalibFix;
using Insnex.Vision2D.Measurement;
using Insnex.Vision2D;
using Insnex.Vision2D.Display;
using Insnex.Vision2D.Controls;
using Insnex.Vision2D.Finder;
using Core.DataProcess;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro;
using static Insnex.Vision2D.Common.InsFactory;
using Insnex.Vision2D.PatternMatch;

namespace Core.UI
{
    public partial class FormTeachInsPM : Form, ITeachForm
    {
        private MainForm MainForm;
        private FormTeachInspection TeachMainForm;  //parent form
        private int m_nSelectCam = 0;       //Selected cam number
        private int m_nToolSelected = 0;    //Selected tool list index
        private int m_nInspectSelected = 0; //Selected inspection mode

        private Font font = new Font("Calibri", 14, FontStyle.Bold);
        private Font ComboBoxFont = new Font("Calibri", 9, FontStyle.Bold);

        private Point TeachFormLocation;
        public RadioButton[] RadioButtons = new RadioButton[4];

        private InsToolBlock _InsToolBlock = null;
        private List<IInsTool> m_InsTeachingToolList = new List<IInsTool>();   //Teaching CogTool List
        private IInsRecordDisplay[] m_DisplayScreen = new IInsRecordDisplay[5];

        //250308 NIS Graphic Variable
        private string[] _key = { "Wafer", "Print" };
        private int[] _labeLPosX = { 100, 2800 };
        private int[] _labelPosY = { 150, 450, 150, 450, 4500, 4800, 4500, 4800 };
        private Color[] LabelColor = { Color.Blue, Color.Orange, Color.Blue, Color.Red };
        private string[] TwoAreaPos = { "", "TOP", "LEFT", "BTM", "RIGHT" };

        //250307 NIS Printcontour Labeling
        private double WaferCenterX = 0, WaferCenterY = 0, PrintCenterX = 0, PrintCenterY = 0;
        private PointF[,] Cam_WaferPrint_PixXY = new PointF[4, 4]; //Cam, Wafer1/Wafer2/Print1/Print2, X/Y
        private double[,] Cam_WaferPrint_PMScore = new double[4, 4]; //Cam, Wafer1/Wafer2/Print1/Print2 Score
        private string[] DataKeys = new string[] { "dWaferPoint", "dWaferPoint2", "dPrintPoint", "dPrintPoint2", "WaferScore", "WaferScore2", "PrintScore", "PrintScore2" };


        //250307 NIS Save Pattern
        private string pattern_path = $"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{theRecipe.m_sCurrentModelName}\\PrintContour_Pattern";    //250114 NIS Pattern save and load
        private System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
        private System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

        private InsImage8Grey InspectionImage;
        private CLogger Logger = null;


        public FormTeachInsPM(MainForm _MainForm, FormTeachInspection _TeachMainForm, CLogger _logger)
        {
            InitializeComponent();
            TopLevel = false;
            TeachMainForm = _TeachMainForm;

            MainForm = _MainForm;
            MainForm.SetEachControlResize(this);    //240801 NIS Conrol Resize]
            this.FormLocate(0);                     //240819 NIS Resize ImageDisplayCam0~4

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; ++i)
            {
                m_DisplayScreen[i] = this.Controls.Find($"ImageDisplayCam{i}", true)[0] as IInsRecordDisplay;
            }

            Set_RadioButtonsList();
            m_nInspectSelected = (int)INSP_MODE.Contour;
            SetData();
            ToolListShow();

            TeachFormLocation = TeachMainForm.Get_TeachFormScreenLocation();  //240721 NIS Get TeachFormScreen Location

            Logger = _logger;
        }

        private void btn_LogicToolBlock_Click(object sender, EventArgs e)   //240720 NIS Display ToolBlockEditPage
        {
            _InsToolBlock = theRecipe.m_InsPreToolBlock[m_nSelectCam];

            ToolBlockEdit ToolEdit = new ToolBlockEdit(m_nInspectSelected, null, _InsToolBlock); //240703 NIS 실행한 TeachMode에 따라서 ToolBlockEdit 타이틀 수정
            ToolEdit.Location = TeachFormLocation;
            ToolEdit.ShowDialog();
            
        }


        private void btn_Run_Click(object sender, EventArgs e)
        {
            if (InspectionImage != null)
            {
                int i = 0;

                IInsTool tool = m_InsTeachingToolList[m_nToolSelected];


                if (tool.Name.Contains("FindLine") || tool.Name.Contains("Hor") || tool.Name.Contains("Ver"))
                {
                    InsFindLineTool FindLine = tool as InsFindLineTool;

                    FindLineParameterApply(FindLine);
                }
                else if (tool.Name.Contains("PM"))
                {

                }

                for (i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                {
                    ToolBlockRun(i, theRecipe.m_InsPreToolBlock[m_nSelectCam], m_nInspectSelected);
                }

                if (tool.Name.Contains("FindLine") || tool.Name.Contains("Hor") || tool.Name.Contains("Ver"))
                {
                    InsFindLineTool FindLine = tool as InsFindLineTool;
                    FindLineShowTool(FindLine);
                }
                else if (tool.Name.Contains("PM"))
                {
                    InsPMAlignTool PMAlign = tool as InsPMAlignTool;
                    PMAlignShowTool(PMAlign);
                }

                TeachInspectionRun();
            }
        }

        private void Reverse_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            InsFindLineTool FindLine = m_InsTeachingToolList[m_nToolSelected] as InsFindLineTool;
            FindLine_Reverse(FindLine);
        }

        private void btn_FitCaliper_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            InsFindLineTool FindLine = m_InsTeachingToolList[m_nToolSelected] as InsFindLineTool;

            FindLine_CaliperFit(FindLine);
        }

        private void ToolList_SelectedIndexChanged(object sender, EventArgs e)
        {
            TeachingPopupAllClose();        //240722 NIS Hide FindLinePanel and PMAlignPanel

            m_nToolSelected = (int)lib_ToolList.SelectedIndex;

            IInsTool SelectTool = m_InsTeachingToolList[m_nToolSelected];

            if (SelectTool.Name.Contains("FindLine") || SelectTool.Name.Contains("Hor") || SelectTool.Name.Contains("Ver"))
            {
                InsFindLineTool FindLine = SelectTool as InsFindLineTool;

                SetTextFindLineData(FindLine);
                FindLineShowTool(FindLine);
            }
            else if (SelectTool.Name.Contains("PM"))
            {
                InsPMAlignTool PMAlign = SelectTool as InsPMAlignTool;

                PMAlignShowTool(PMAlign);

                m_InsTeachingToolList[m_nToolSelected] = PMAlign;
            }
        }

        private void rdb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdb = sender as RadioButton;

            if(rdb.Checked)
            {
                try
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
                    //TeachInspectionRun();
                }
                catch (Exception ex)
                {
                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Form Print Contour Exception Catch : {ex.Message}");
                }
            }
        }

        private void ApplyAndSave_Click(object sender, EventArgs e)
        {
            Run_ApplyAndSave(); //241210 Apply Save Thread 삭제
        }

        private void Run_ApplyAndSave()
        {
            IInsTool tool = m_InsTeachingToolList[m_nToolSelected];

            if (tool.Name.Contains("FindLine") || tool.Name.Contains("Hor") || tool.Name.Contains("Ver")) 
            {
                InsFindLineTool FindLine = tool as InsFindLineTool;
                FindLineParameterApply(FindLine);
            }
            else if (tool.Name.Contains("PM"))
            {
                InsPMAlignTool PMAlign = tool as InsPMAlignTool;
            }

            theRecipe.ToolSave(theRecipe.m_sCurrentModelName);
        }

        private void ToolListShow()   //250318 NIS Set ToolList
        {
            m_InsTeachingToolList.Clear();
            lib_ToolList.Items.Clear();

            if (theRecipe.m_InsPreToolBlock[m_nSelectCam] == null)
                return;

            for (int i = 0; i < theRecipe.m_InsPreToolBlock[m_nSelectCam].Tools.Count; i++)
            {
                IInsTool tool = theRecipe.m_InsPreToolBlock[m_nSelectCam].Tools[i];
                string sTemp = tool.Name;
                if (sTemp.Contains("PMAlignTool"))
                {
                    lib_ToolList.Font = font;
                    lib_ToolList.Items.Add(sTemp);
                    m_InsTeachingToolList.Add(tool);
                }
            }

            lib_ToolList.SetSelected(0, true);
        }

        private void FindLineParameterApply(InsFindLineTool _pFindLine) //240720 NIS Apply parmas
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

                _pFindLine.RunParams.Terations = int.Parse(txt_IterrateTimes.Text); //241207 NWT Ransac Iterate Times

                _pFindLine.RunParams.Sigma = int.Parse(txt_Sigma.Text); //241207 NWT Ransac Sigma

                int nDirection = cbo_SearchDirection.SelectedIndex;
                if (nDirection == 0)             //231007 LYK 밝은 곳에서 어두운 곳
                    _pFindLine.RunParams.CaliperRunParams.Edge0Polarity = InsCaliperPolarityConstants.LightToDark;
                else if (nDirection == 1)        //231007 LYK 어두운곳에서 밝은 곳
                    _pFindLine.RunParams.CaliperRunParams.Edge0Polarity = InsCaliperPolarityConstants.DarkToLight;
                else if (nDirection == 2)           //231007 LYK 임의의 극성
                    _pFindLine.RunParams.CaliperRunParams.Edge0Polarity = InsCaliperPolarityConstants.DontCare;

                int nFittingMode = cbo_FittingMode.SelectedIndex;
                if (nFittingMode == 0)
                    _pFindLine.RunParams.FittingMethod = CppInternal.FittingType.IgnorePoints;
                else if (nFittingMode == 1)
                    _pFindLine.RunParams.FittingMethod = CppInternal.FittingType.IgnorePointsPlus;
                else if (nFittingMode == 2)
                    _pFindLine.RunParams.FittingMethod = CppInternal.FittingType.Ransac;

                double CaliperScore = 0.0;
                CaliperScore = _pFindLine.RunParams.CaliperRunParams.SingleEdgeScorers[0].X0; //241210 KCH Caliper Score 적용

                int nSetScore = int.Parse(txt_Score.Text);
                int nGetScore = (int)CaliperScore;

                if (nSetScore != nGetScore)
                {
                    _pFindLine.RunParams.CaliperRunParams.SingleEdgeScorers[0].X0 = (float)nSetScore;
                }

                _pFindLine.RunParams.CaliperRunParams.FilterHalfSizeInPixels = int.Parse(txt_CaliperFilter.Text);   //231007 LYK 필터 절반 픽셀

                _pFindLine.RunParams.CaliperRunParams.ContrastThreshold = double.Parse(txt_CaliperContrast.Text);   //231007 LYK 대비 임계값

                InsRecord CurrRecord = new InsRecord();
                CurrRecord.ContentType = typeof(IInsImage);
                CurrRecord.Content = _pFindLine.InputImage;
                CurrRecord.Annotation = "InputImage";
                CurrRecord.RecordKey = "InputImage";
                CurrRecord.RecordUsage = InsRecordUsageConstants.Diagnostic;

                CurrRecord.SubRecords.Add(_pFindLine.CreateCurrentRecord());
                CurrentImageDisplay.Record = CurrRecord;
                CurrentImageDisplay.FitImage();
            }
            catch (Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"FindLine Tool Save Error Catch : {e.Message}");
            }
        }
        private void ToolBlockRun(int _nCamNum, InsToolBlock _toolBlock, int _nMode)    //240720 NIS ToolBlock Run
        {
            _toolBlock.Inputs[0].Value = InspectionImage;

            _toolBlock.Run();
        }

        private void FindLine_Reverse(InsFindLineTool _pFindLine)
        {
            double dDirectionValue = 0.0;
            dDirectionValue = _pFindLine.RunParams.CaliperSearchDirection;
            _pFindLine.RunParams.CaliperSearchDirection = dDirectionValue *= -1;
        }

        private void FindLine_CaliperFit(InsFindLineTool _pFindLine)
        {
            int nImgWidth = _pFindLine.InputImage.Width;
            int nImgHeight = _pFindLine.InputImage.Height;
            double dChanged_StartX = 0.0, dChanged_StartY = 0.0, dChanged_EndX = 0.0, dChanged_EndY = 0.0;

            InsTransform2DLinear Linear = _pFindLine.InputImage.GetTransform(_pFindLine.RunParams.ExpectedLineSegment.SelectedSpaceName, "#") as InsTransform2DLinear;
            Linear.MapPoint(nImgWidth / 4, nImgHeight / 2, out dChanged_StartX, out dChanged_StartY);
            Linear.MapPoint(nImgWidth / 4 * 3, nImgHeight / 2, out dChanged_EndX, out dChanged_EndY);

            _pFindLine.RunParams.ExpectedLineSegment.StartX = dChanged_StartX;
            _pFindLine.RunParams.ExpectedLineSegment.StartY = dChanged_StartY;
            _pFindLine.RunParams.ExpectedLineSegment.EndX = dChanged_EndX;
            _pFindLine.RunParams.ExpectedLineSegment.EndY = dChanged_EndY;
        }
        private void SetTextFindLineData(InsFindLineTool _pFindLine)
        {
            txt_NumofCaliper.Text = _pFindLine.RunParams.NumCalipers.ToString();                        //231007 LYK 캘리퍼 수

            txt_SearchLength.Text = _pFindLine.RunParams.CaliperSearchLength.ToString();                //231007 LYK 캘리퍼 검색 길이

            txt_ProjectionLength.Text = _pFindLine.RunParams.CaliperProjectionLength.ToString();        //231007 LYK 캘리퍼 프로젝션 길이

            txt_NumofIgnore.Text = _pFindLine.RunParams.NumToIgnore.ToString();                         //231007 LYK 무시할 숫자

            txt_IterrateTimes.Text = _pFindLine.RunParams.Terations.ToString();                         //241207 NWT Ransac iterate Times

            txt_Sigma.Text = _pFindLine.RunParams.Sigma.ToString();                                     //241207 NWT Ransac Sigma

            int nDirection = 0;

            if (_pFindLine.RunParams.CaliperRunParams.Edge0Polarity == InsCaliperPolarityConstants.LightToDark)             //231007 LYK 밝은 곳에서 어두운 곳
                nDirection = 0;
            else if (_pFindLine.RunParams.CaliperRunParams.Edge0Polarity == InsCaliperPolarityConstants.DarkToLight)        //231007 LYK 어두운곳에서 밝은 곳
                nDirection = 1;
            else if (_pFindLine.RunParams.CaliperRunParams.Edge0Polarity == InsCaliperPolarityConstants.DontCare)           //231007 LYK 임의의 극성
                nDirection = 2;

            cbo_SearchDirection.SelectedIndex = nDirection;

            int nFittingMode = 0;

            if (_pFindLine.RunParams.FittingMethod == CppInternal.FittingType.IgnorePoints)
                nFittingMode = 0;
            else if (_pFindLine.RunParams.FittingMethod == CppInternal.FittingType.IgnorePointsPlus)
                nFittingMode = 1;
            else if (_pFindLine.RunParams.FittingMethod == CppInternal.FittingType.Ransac)
                nFittingMode = 2;

            cbo_FittingMode.SelectedIndex = nFittingMode;

            txt_Score.Text = _pFindLine.RunParams.CaliperRunParams.SingleEdgeScorers[0].X0.ToString(); //241210 KCH UI에 Score값 띄우기

            txt_CaliperFilter.Text = _pFindLine.RunParams.CaliperRunParams.FilterHalfSizeInPixels.ToString();   //231007 LYK 필터 절반 픽셀

            txt_CaliperContrast.Text = _pFindLine.RunParams.CaliperRunParams.ContrastThreshold.ToString();      //231007 LYK 대비 임계값
        }
        private void FindLineShowTool(InsFindLineTool _pFindLine)
        {
            InsRecord CurrRecord = new InsRecord();
            CurrRecord.ContentType = typeof(IInsImage);
            CurrRecord.Content = _pFindLine.InputImage;
            CurrRecord.Annotation = "InputImage";
            CurrRecord.RecordKey = "InputImage";
            CurrRecord.RecordUsage = InsRecordUsageConstants.Diagnostic;

            CurrRecord.SubRecords.Add(_pFindLine.CreateCurrentRecord());
            CurrentImageDisplay.Record = CurrRecord;
            CurrentImageDisplay.FitImage();

            InsRecord LastRecord = new InsRecord();
            LastRecord.ContentType = typeof(IInsImage);
            LastRecord.Content = _pFindLine.InputImage;
            LastRecord.Annotation = "InputImage";
            LastRecord.RecordKey = "InputImage";
            LastRecord.RecordUsage = InsRecordUsageConstants.Diagnostic;

            LastRecord.SubRecords.Add(_pFindLine.CreateLastRunRecord());
            LastRunImageDisplay.Record = LastRecord;
            LastRunImageDisplay.FitImage();

            Panel_FindLine.Visible = true;
        }
        private void TeachInspectionRun()   //240709 NIS Cam 변경 시 마다 Inspection Display
        {
            CreateLabeling(InspectionImage); // Print Contour 전용 Teaching Page Graphic 출력 

            //Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Controller Reset");
        }
        private void CreateLabeling(InsImage8Grey _Image)
        {
            InsRecord MainRecord = new InsRecord("Main", _Image.GetType(), InsRecordUsageConstants.Result, true, _Image, "Main");
            InsRecord SubRecord = new InsRecord("GraphicCollection", _Image.GetType(), InsRecordUsageConstants.Result, true, _Image, "GraphicCollection");
            bool IsError = false;

            for (int i = 0; i < theRecipe.m_InsPreToolBlock[m_nSelectCam].Tools.Count; i++)
            {
                if (theRecipe.m_InsPreToolBlock[m_nSelectCam].Tools[i].Name.Contains("PMAlignTool"))
                {
                    InsPMAlignTool tool = theRecipe.m_InsPreToolBlock[m_nSelectCam].Tools[i] as InsPMAlignTool;

                    SubRecord.AddGraphicToRecord(CreatInsMarker(tool.Results[0].GetPose().TranslationX, tool.Results[0].GetPose().TranslationY, Color.Orange, 10), "GraphicCollection", "");
                }
            }
            MainRecord.SubRecords.Add(SubRecord);
            m_DisplayScreen[m_nSelectCam].Record = MainRecord;

            m_DisplayScreen[m_nSelectCam].FitImage();
        }
        private void SetData()          //240720 NIS Set SearchDirection combobox
        {
            cbo_SearchDirection.Font = ComboBoxFont;
            cbo_SearchDirection.Items.Add("Light to Dark Polarity");
            cbo_SearchDirection.Items.Add("Dark to Light Polarity");
            cbo_SearchDirection.Items.Add("Random Polarity");
            cbo_FittingMode.Font = ComboBoxFont;
            cbo_FittingMode.Items.Add("Ignored Points");
            cbo_FittingMode.Items.Add("Ignored Points Plus");
            cbo_FittingMode.Items.Add("Ransac");
        }
        private void TeachingPopupAllClose()
        {
            Panel_FindLine.Visible = false;
            Panel_PMAlign.Visible = false;
            Panel_Blob.Visible = false;
        }
        private void PMAlignShowTool(InsPMAlignTool _pPMAlign)
        {
            if (_pPMAlign.Region != null) 
                cb_RegionUse.Checked = true;

            rb_Pattern.Checked = true; //PM Align Tool 기본 표시는 Pattern 으로 표시 
            InsRecord CurrRecord = new InsRecord();
            CurrRecord.ContentType = typeof(IInsImage);
            CurrRecord.Content = _pPMAlign.Pattern.TrainImage;
            CurrRecord.Annotation = "InputImage";
            CurrRecord.RecordKey = "InputImage";
            CurrRecord.RecordUsage = InsRecordUsageConstants.Diagnostic;

            CurrRecord.SubRecords.Add(_pPMAlign.CreateCurrentRecord().SubRecords[1]);
            CurrentImageDisplay.Record = CurrRecord;
            CurrentImageDisplay.FitImage();

            InsRecord LastRecord = new InsRecord();
            LastRecord.ContentType = typeof(IInsImage);
            LastRecord.Content = _pPMAlign.InputImage;
            LastRecord.Annotation = "InputImage";
            LastRecord.RecordKey = "InputImage";
            LastRecord.RecordUsage = InsRecordUsageConstants.Diagnostic;

            LastRecord.SubRecords.Add(_pPMAlign.CreateLastRunRecord());
            LastRunImageDisplay.Record = LastRecord;
            LastRunImageDisplay.FitImage();

            if(_pPMAlign.Pattern.Trained)
            {
                if (_pPMAlign.Results != null && _pPMAlign.RunStatus.Result != InsToolResultConstants.Error && _pPMAlign.Results.Count == 1)
                    txt_GetScore_Pattern.Text = (_pPMAlign.Results[0].Score * 100).ToString("F1");

                TrainImageDisplay.Record = new InsRecord("", typeof(InsImage8Grey), true, _pPMAlign.Pattern.GetTrainedPatternImage(), "");
                TrainImageDisplay.FitImage();
            }

            Panel_PMAlign.Visible = true;
        }

        private void btn_FitPatternRegion_Click(object sender, EventArgs e)
        {
            IInsImage DisplayImage = InspectionImage;

            CurrentImageDisplay.FitImage();

            InsPMAlignTool _PMAlign = m_InsTeachingToolList[m_nToolSelected] as InsPMAlignTool;
            InsTransform2DLinear PixelFromScreen= new InsTransform2DLinear();

            InsRectangle TrainRec = _PMAlign.Pattern.TrainRegion as InsRectangle;

            InsRectangle SearchRec = new InsRectangle();


            if (rb_Pattern.Checked)
            {
                DisplayImage.SelectedSpaceName = _PMAlign.InputImage.SelectedSpaceName;
                PixelFromScreen = DisplayImage.GetTransform(_PMAlign.Pattern.TrainRegion.SelectedSpaceName, "#") as InsTransform2DLinear;
                TrainRec.SelectedSpaceName = _PMAlign.InputImage.SelectedSpaceName;
            }
            else if (rb_Region.Checked)
            {
                //DisplayImage.SelectedSpaceName = "@//Fixture";
                //PixelFromScreen = DisplayImage.GetTransform(_PMAlign.InputImage.SelectedSpaceName, "@//Fixture") as InsTransform2DLinear;
                //SearchRec.SelectedSpaceName = "@//Fixture";
            }

            int Image_CenX = DisplayImage.Width / 2;
            int Image_CenY = DisplayImage.Height / 2;

            double ConvertWidth = DisplayImage.Width * PixelFromScreen.Scaling;
            double ConvertHeight = DisplayImage.Height * PixelFromScreen.Scaling;
            double ConvertCenX = 0;
            double ConvertCenY = 0;

            PixelFromScreen.MapPoint((double)Image_CenX, (double)Image_CenY, out ConvertCenX, out ConvertCenY);

            if (rb_Pattern.Checked)
            {
                {
                    if (DisplayImage.Width >= DisplayImage.Height) // 이미지 정방향 
                    {
                        TrainRec.CenterX = ConvertCenX;
                        TrainRec.CenterY = ConvertCenY;
                        TrainRec.Width = ConvertWidth / 2;
                        TrainRec.Height = ConvertHeight / 2;


                        _PMAlign.Pattern.Origin.TranslationX = ConvertCenX;
                        _PMAlign.Pattern.Origin.TranslationY = ConvertCenY;
                        //_PMAlign.Pattern.TrainRegion = TrainRec;
                    }
                    else
                    {
                        TrainRec.CenterX = ConvertCenY;
                        TrainRec.CenterY = ConvertCenX;
                        TrainRec.Width = ConvertHeight / 2;
                        TrainRec.Height = ConvertWidth / 2;

                        _PMAlign.Pattern.Origin.TranslationX = ConvertCenY;
                        _PMAlign.Pattern.Origin.TranslationY = ConvertCenX;
                        _PMAlign.Pattern.TrainRegion = TrainRec;
                    }
                }  
            }
            else if (rb_Region.Checked)
            {
                if (DisplayImage.Width >= DisplayImage.Height) // 이미지 정방향 
                {
                    SearchRec.X = ConvertCenX - ConvertWidth / 4;
                    SearchRec.Y = ConvertCenY - ConvertHeight / 4;
                    SearchRec.Width = ConvertWidth / 2;
                    SearchRec.Height = ConvertHeight / 2;

                    _PMAlign.Region = SearchRec;
                }
                else
                {
                    SearchRec.X = ConvertCenY - ConvertHeight / 4;
                    SearchRec.Y = ConvertCenX - ConvertWidth / 4;
                    SearchRec.Width = ConvertHeight / 2;
                    SearchRec.Height = ConvertWidth / 2;

                    _PMAlign.Region = TrainRec;

                }
            }

        }
        private void btn_FitPatternPoint_Click(object sender, EventArgs e)
        {
            InsPMAlignTool _PMAlign = m_InsTeachingToolList[m_nToolSelected] as InsPMAlignTool;
            //250327 NIS Pattern InsCircle 추가
            if (_PMAlign.Pattern.TrainRegion.GetType() == typeof(InsCircle))
            {
                InsCircle rec = new InsCircle();
                rec.SelectedSpaceName = _PMAlign.Pattern.TrainRegion.SelectedSpaceName;

                rec = (InsCircle)_PMAlign.Pattern.TrainRegion;

                _PMAlign.Pattern.Origin.TranslationX = rec.CenterX; 
                _PMAlign.Pattern.Origin.TranslationY = rec.CenterY;
            }
            else
            {
                InsRectangle rec = new InsRectangle();
                rec.SelectedSpaceName = _PMAlign.Pattern.TrainRegion.SelectedSpaceName;

                rec = (InsRectangle)_PMAlign.Pattern.TrainRegion;

                _PMAlign.Pattern.Origin.TranslationX = rec.CenterX;
                _PMAlign.Pattern.Origin.TranslationY = rec.CenterY;
            }
        }
        private void btn_TrainImageUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                InsPMAlignTool _PMAlign = m_InsTeachingToolList[m_nToolSelected] as InsPMAlignTool;
                _PMAlign.Pattern.TrainImage = _PMAlign.InputImage as InsImage8Grey;

                _PMAlign.Pattern.TrainImage = InsSerializer.DeepCopyObject(_PMAlign.InputImage) as InsImage8Grey;

                //_PMAlign.Pattern.TrainImage = new CogImage8Grey(_PMAlign.InputImage as CogImage8Grey);
                _PMAlign.Pattern.UnTrain();

                //_PMAlign.Pattern.TrainImage = ((CogToolBlock)theRecipe.ContourInspectToolBlock[m_nSelectCam].Tools["CorrectImage"]).Outputs[0].Value as CogImage8Grey;

                PatternShowTrainRegion(_PMAlign); 

                TrainImageDisplay.Record = null;
                TrainImageDisplay.FitImage();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"TrainImageUpdate Error : {ex.Message}");
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"TrainImageUpdate Error : {ex.Message}");
            }
        }

        private void btn_TrainPattern_Click(object sender, EventArgs e)
        {
            InsPMAlignTool _PMAlign = m_InsTeachingToolList[m_nToolSelected] as InsPMAlignTool;

            try
            {
                _PMAlign.Pattern.Train();

                if (_PMAlign.Pattern.Trained)
                {
                    TrainImageDisplay.Image = _PMAlign.Pattern.GetTrainedPatternImage();
                    TrainImageDisplay.FitImage();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Pattern Train Error : {ex.Message}");
            }
        }

        private void PMAlignOriginMove(int _moveX, int _moveY)
        {
            // Train 된 Origin 좌표를 이동시켜주는 내용 
            InsPMAlignTool _PMAlign = m_InsTeachingToolList[m_nToolSelected] as InsPMAlignTool;

            double OriginX = _PMAlign.Pattern.Origin.TranslationX;
            double OriginY = _PMAlign.Pattern.Origin.TranslationY;
            double PixelX, PixelY;

            InsTransform2DLinear PixelLinear = _PMAlign.Pattern.TrainImage.GetTransform("#", _PMAlign.Pattern.TrainImage.SelectedSpaceName) as InsTransform2DLinear;
            PixelLinear.MapPoint(OriginX, OriginY, out PixelX, out PixelY);

            PixelX += _moveX;
            PixelY += _moveY;

            double ConvertX, ConvertY;
            InsTransform2DLinear TrainLinear = _PMAlign.Pattern.TrainImage.GetTransform(_PMAlign.Pattern.TrainImage.SelectedSpaceName, "#") as InsTransform2DLinear;
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
            InsPMAlignTool _PMAlign = m_InsTeachingToolList[m_nToolSelected] as InsPMAlignTool;

            PatternShowTrainRegion(_PMAlign); 

        }

        private void rb_SelectRegion_Click(object sender, EventArgs e)
        {
            InsPMAlignTool _PMAlign = m_InsTeachingToolList[m_nToolSelected] as InsPMAlignTool;

            PatternShowSearchRegion(_PMAlign);  

            if (_PMAlign.Region != null)
                cb_RegionUse.Checked = true;
        }

        private void cb_RegionUse_CheckedChange(object sender, EventArgs e)
        {
            InsPMAlignTool _PMAlign = m_InsTeachingToolList[m_nToolSelected] as InsPMAlignTool;

            if (cb_RegionUse.Checked)
            {
                if (_PMAlign.Region == null)
                    PatternSearchRegionFit(_PMAlign); 
            }
            else
            {
                _PMAlign.Region = null;
            }

            PatternShowSearchRegion(_PMAlign);

        }

        private void btn_FitSearchRegion_Click(object sender, EventArgs e)
        {
            InsPMAlignTool _PMAlign = m_InsTeachingToolList[m_nToolSelected] as InsPMAlignTool;

            if (_PMAlign.Region == null)
                PatternSearchRegionFit(_PMAlign);

            PatternShowSearchRegion(_PMAlign);

        }

        private void PatternSearchRegionFit(InsPMAlignTool _PMAlign)
        {
            //PatternShowSearchRegion(_PMAlign);
            IInsImage DisplayImage = CurrentImageDisplay.Image;

            //PMAlign SearchRegion에 InsRectangle 넣기
            InsRectangle SearchRec = new InsRectangle();
            SearchRec.GraphicDOFEnable = InsRectangleDOFConstants.All;
            SearchRec.GraphicDOFEnableBase = InsGraphicDOFConstants.All;
            SearchRec.Interactive = true;

            SearchRec.SelectedSpaceName = "@/Fixture";
            DisplayImage.SelectedSpaceName = _PMAlign.InputImage.SelectedSpaceName;

            int Image_CenX = DisplayImage.Width / 2;
            int Image_CenY = DisplayImage.Height / 2;

            InsTransform2DLinear PixelFromScreen = DisplayImage.GetTransform(_PMAlign.Pattern.TrainRegion.SelectedSpaceName, "#") as InsTransform2DLinear;

            double ConvertWidth = DisplayImage.Width * PixelFromScreen.Scaling;
            double ConvertHeight = DisplayImage.Height * PixelFromScreen.Scaling;
            double ConvertCenX = 0;
            double ConvertCenY = 0;

            PixelFromScreen.MapPoint((double)Image_CenX, (double)Image_CenY, out ConvertCenX, out ConvertCenY);


            if (DisplayImage.Width >= DisplayImage.Height) // 이미지 정방향 
            {
                SearchRec.X = ConvertCenX - ConvertWidth / 4;
                SearchRec.Y = ConvertCenY - ConvertHeight / 4;
                SearchRec.Width = ConvertWidth / 2;
                SearchRec.Height = ConvertHeight / 2;
                _PMAlign.Region = SearchRec;
            }
            else
            {
                SearchRec.X = ConvertCenY - ConvertHeight / 4;
                SearchRec.Y = ConvertCenX - ConvertWidth / 4;
                SearchRec.Width = ConvertHeight / 2;
                SearchRec.Height = ConvertWidth / 2;

                _PMAlign.Region = SearchRec;
            }
        }

        private void cbo_FittingMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbo_FittingMode.SelectedIndex == 2) //Ransac이면
            {
                txt_NumofIgnore.Enabled = false;    //NumOfIngnore Disable
                txt_IterrateTimes.Enabled = true;
                txt_Sigma.Enabled = true;
            }
            else
            {
                txt_NumofIgnore.Enabled = true;
                txt_IterrateTimes.Enabled = false;
                txt_Sigma.Enabled = false;
            }
        }

        private void PatternShowSearchRegion(InsPMAlignTool _PMAlign)
        {
            CurrentImageDisplay.FitImage();

            InsRecord CurrRecord = new InsRecord();
            CurrRecord.ContentType = typeof(IInsImage);
            CurrRecord.Content = _PMAlign.Pattern.TrainImage;
            CurrRecord.Annotation = "InputImage";
            CurrRecord.RecordKey = "InputImage";
            CurrRecord.RecordUsage = InsRecordUsageConstants.Diagnostic;

            CurrRecord.SubRecords.Add(_PMAlign.CreateCurrentRecord().SubRecords[0]);
            CurrentImageDisplay.Image = _PMAlign.InputImage;
            CurrentImageDisplay.Record = CurrRecord;
            CurrentImageDisplay.FitImage();
        }

        private void PatternShowTrainRegion(InsPMAlignTool _PMAlign)
        {
            InsRecord CurrRecord = new InsRecord();
            CurrRecord.ContentType = typeof(IInsImage);
            CurrRecord.Content = _PMAlign.Pattern.TrainImage;
            CurrRecord.Annotation = "InputImage";
            CurrRecord.RecordKey = "InputImage";
            CurrRecord.RecordUsage = InsRecordUsageConstants.Diagnostic;

            CurrRecord.SubRecords.Add(_PMAlign.CreateCurrentRecord().SubRecords[1]);
            CurrentImageDisplay.Record = CurrRecord;
            CurrentImageDisplay.FitImage();
        }


        private InsLineSegment CreatInsSegment(double _StartX, double _StartY, double _EndX, double _EndY, InsGraphicLineStyleConstants _LineStyle, Color _Color)
        {
            InsLineSegment CreatSegment = new InsLineSegment();
            CreatSegment.SetStartEnd(_StartX, _StartY, _EndX, _EndY);
            CreatSegment.LineStyle = _LineStyle;
            CreatSegment.Color = _Color;
            CreatSegment.LineWidthInScreenPixels = 2;

            return CreatSegment;
        }

        private InsPointMarker CreatInsMarker(double _PosX, double _PosY, Color _Color, int _Size)
        {
            InsPointMarker CreatMarker = new InsPointMarker();
            CreatMarker.X = _PosX;
            CreatMarker.Y = _PosY;
            CreatMarker.Color = _Color;
            CreatMarker.SizeInScreenPixels = _Size;

            return CreatMarker;
        }

        private InsGraphicLabel CreatInsLabel(double _PosX, double _PosY, string _txt, Color _Color, Font _font)
        {
            InsGraphicLabel CreatLabel = new InsGraphicLabel();
            CreatLabel.X = _PosX;
            CreatLabel.Y = _PosY;
            CreatLabel.Text = _txt;
            CreatLabel.Color = _Color;
            CreatLabel.Font = _font;
            CreatLabel.FontSize = 200;

            return CreatLabel;
        }

        private InsGraphicLabel CreatInsLabel(double _PosX, double _PosY, string _txt, Color _Color, Font _font, float _fontSize)
        {
            InsGraphicLabel CreatLabel = new InsGraphicLabel();
            CreatLabel.X = _PosX;
            CreatLabel.Y = _PosY;
            CreatLabel.Text = _txt;
            CreatLabel.Color = _Color;
            CreatLabel.Font = _font;
            CreatLabel.FontSize = _fontSize;

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

            //250327 NIS KOR PRI용 가운데 카메라 Contour Bound 추가
            InsRecordDisplayControl[] bounds = new InsRecordDisplayControl[5]
            {
                ImageDisplayCam0, ImageDisplayCam1, ImageDisplayCam2, ImageDisplayCam3, ImageDisplayCam4
            };

            //(theRecipe.m_sCurrentEquipment.Contains("SORTER") || (theRecipe.m_sCurrentCountry == "KOREA" && theRecipe.m_sCurrentEquipment == "PRINTER")) == true ? theRecipe.m_nMaxCamCount - 1 : theRecipe.m_nMaxCamCount;
            //250327 NIS KOR PRI 용 PrintContour 스크린 생성

            for (int i = 0; i < 4; ++i)
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
            /*  임시 주석
            for (int i = 0; i < 4; i++)
                m_DisplayScreen[i].Record = null;
            */
        }


        private void FormInsTeachInspectionPrintContour_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                Reset_DisplayScreen();
        }

        private void btm_ImgLoad_Click(object sender, EventArgs e)
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
                        InspectionImage = new InsImage8Grey(fileDialog.FileNames[0]);
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

        private void btn_PM_Save_Click(object sender, EventArgs e)
        {
            try
            {
                m_nToolSelected = (int)lib_ToolList.SelectedIndex;  //현재 선택된 툴 인덱스 가져오기

                IInsTool SelectTool = m_InsTeachingToolList[m_nToolSelected];

                if (SelectTool.Name.Contains("PM"))     //PM이면 저장 실행
                {
                    if (!Directory.Exists(pattern_path))
                        Directory.CreateDirectory(pattern_path);

                    InsPMAlignTool PMAlign = SelectTool as InsPMAlignTool;

                    saveFileDialog.Filter = "Pattern Files (*.ins)|*.ins";

                    saveFileDialog.Title = "Save a Pattern File";
                    saveFileDialog.InitialDirectory = pattern_path;
                    DialogResult userInput = saveFileDialog.ShowDialog();

                    if (userInput == DialogResult.OK && saveFileDialog.FileName != "")
                    {
                        InsSerializer.SaveObjectToFile(PMAlign.Pattern, saveFileDialog.FileName);
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

                IInsTool SelectTool = m_InsTeachingToolList[m_nToolSelected];

                if (SelectTool.Name.Contains("PM")) //PM이면 로드 실행
                {
                    if (!Directory.Exists(pattern_path))
                        Directory.CreateDirectory(pattern_path);

                    InsPMAlignTool PMAlign = SelectTool as InsPMAlignTool;

                    openFileDialog.Filter = "Pattern Files (*.ins)|*.ins";

                    openFileDialog.Title = "Load a Pattern File";
                    openFileDialog.InitialDirectory = pattern_path;
                    DialogResult userInput = openFileDialog.ShowDialog();

                    if (userInput == DialogResult.OK && openFileDialog.FileName != "")
                    {
                        PMAlign.Pattern = InsSerializer.LoadObjectFromFile(openFileDialog.FileName) as InsPMAlignPattern;
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
    }
}