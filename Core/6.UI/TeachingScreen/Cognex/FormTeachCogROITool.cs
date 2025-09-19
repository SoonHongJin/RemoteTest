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
using Core.DataProcess;

namespace Core.UI
{
    public partial class FormTeachCogROITool : Form, ITeachForm
    {
        private MainForm MainForm;
        private FormTeachInspection TeachMainForm;  //parent form
        private int m_nSelectCam = 0;       //Selected cam number
        private int m_nToolBlockSelected;
        private int m_nToolSelected = 0;    //Selected tool list index
        private int m_nInspectSelected = 0; //Selected inspection mode
        private int m_nToolPreIndex = 0;

        private Font font = new Font("Calibri", 14, FontStyle.Bold);
        private Font ComboBoxFont = new Font("Calibri", 9, FontStyle.Bold);

        private Point TeachFormLocation;
        public RadioButton[] RadioButtons = new RadioButton[4];

        private CThread Thread_ApplyAndSave;
        //250122 NWT Parameter Exception
        private List<CExceptionData> exceptionDataList = new List<CExceptionData>();
        private ParameterExceptionHandler parameterExceptionHandler = null;

        private CogToolBlock m_TeachingToolBlock = null;
        private List<ICogTool> m_TeachingToolList = new List<ICogTool>();   //Teaching CogTool List
        private CogImage8Grey m_InspectionImage = null;
        private CLogger Logger = null;

        private int m_nLinePointX = 0;
        private int m_nLinePointY = 0;
        private double m_dSearchLength = 0;
        private double m_dProjectionLength = 0;

        private int m_nCellIndex = 0;

        public FormTeachCogROITool(MainForm _MainForm, FormTeachInspection _TeachMainForm, CLogger _logger)
        {
            InitializeComponent();
            TopLevel = false;
            TeachMainForm = _TeachMainForm;
            MainForm = _MainForm;
            exceptionDataList = theRecipe.m_listExceptionData;
            MainForm.SetEachControlResize(this);    //240801 NIS Conrol Resize

            Set_RadioButtonsList();
            m_nInspectSelected = (int)INSP_MODE.Crack;
            SetData();
            ToolListShow();
            TeachFormLocation = TeachMainForm.Get_TeachFormScreenLocation();  //240721 NIS Get TeachFormScreen Location
            Set_Thead();

            for (int i = 0; i < 4; i++)
            {
                this.Controls.Find($"rdb_Cam{i + 1}", true)[0].Visible = false;
            }

            Disable_RadioButtons();
            Logger = _logger;
        }

        private void Set_Thead()
        {
            Thread_ApplyAndSave = new CThread()
            {
                Work = Run_ApplyAndSave,
                nSleepTime = 1
            };
            Thread_ApplyAndSave.ThreadStart();
        }

        public void ShowTeachingPage()
        {
            ToolListShow();

            //CreatLabeling(m_InspectionImage);
        }
        public void CloseTeachingPage()
        {
            cbo_CellIndex.SelectedIndex = 0;
        }

        private void btn_LogicToolBlock_Click(object sender, EventArgs e)   //240720 NIS Display ToolBlockEditPage
        {

            //_ToolBlock = theRecipe.CrackInspectToolBlock.Tools[m_nSelectCam] as CogToolBlock; 임시 주석

            ToolBlockEdit ToolEdit = new ToolBlockEdit(m_nInspectSelected, m_TeachingToolBlock); //240703 NIS 실행한 TeachMode에 따라서 ToolBlockEdit 타이틀 수정
            ToolEdit.Location = TeachFormLocation;
            ToolEdit.ShowDialog();

        }

        private void btn_Run_Click(object sender, EventArgs e)
        {
            int nSelect = lib_ToolList.SelectedIndex;
            CogFindLineTool FindLine = m_TeachingToolList[nSelect] as CogFindLineTool;
            double Offset = 0;
            int ImageIndex = 0;
            bool ExcuteMode = false;

            if (cbo_CellIndex.Items.Count > 0)
            {
                // 20250911 SHJ  Find Line Tool 첫번째 위치로 갱신 
                Offset = (theRecipe.MaterialInfo.m_dCellHeight + theRecipe.MaterialInfo.m_dCellPadHeight) * (m_nCellIndex);
                FindLine.RunParams.ExpectedLineSegment.StartY -= Offset;
                FindLine.RunParams.ExpectedLineSegment.EndY -= Offset;

                m_InspectionImage = FindLine.InputImage;

                ImageIndex = cbo_ImageIndex.SelectedIndex;
            }


            if (m_InspectionImage != null)
            {
                FindLineParameterApply(FindLine);

                System.Threading.Thread.Sleep(100);

                // ToolBlock 실행 
                theMainSystem.Cameras[m_nSelectCam].RunTeachingInspect(ImageIndex, false, true, m_nCellIndex);

                FindLineShowTool(FindLine); // LastRun Display 

                CreatLabeling(m_InspectionImage); // ROI 전체 내용 Display
            }

            if (cbo_CellIndex.Items.Count > 0)
            {
                // 20250911 SHJ Cell Index 현재 선택된 위치로 조정
                Offset = (theRecipe.MaterialInfo.m_dCellHeight + theRecipe.MaterialInfo.m_dCellPadHeight) * m_nCellIndex;
                FindLine.RunParams.ExpectedLineSegment.StartY += Offset;
                FindLine.RunParams.ExpectedLineSegment.EndY += Offset;
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
            m_nToolSelected = (int)lib_ToolList.SelectedIndex;

            CogFindLineTool PreFindLine = m_TeachingToolList[m_nToolPreIndex] as CogFindLineTool;

            // 기존 로드 되어 있는 FindLine Tool 원 위치로 원복 
            double Offset = (theRecipe.MaterialInfo.m_dCellHeight + theRecipe.MaterialInfo.m_dCellPadHeight) * m_nCellIndex;

            PreFindLine.RunParams.ExpectedLineSegment.StartY -= Offset;
            PreFindLine.RunParams.ExpectedLineSegment.EndY -= Offset;

            // Cell Index 초기화
            m_nCellIndex = 0;

            if(cbo_CellIndex.Items.Count > 0)
                cbo_CellIndex.SelectedIndex = 0;

            theMainSystem.Wait(100);
                
            ICogTool SelectTool = m_TeachingToolList[m_nToolSelected];

            if (SelectTool.Name.Contains("FindLine"))// Tool 이름으로 구분하여 Parameter 표시
            {
                CogFindLineTool FindLine = SelectTool as CogFindLineTool;

                SetTextFindLineData(FindLine);
                FindLineShowTool(FindLine);
            }

            m_nToolPreIndex = m_nToolSelected;
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
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Crack ROI Teaching Cam1 Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_ONE;
                    }
                    else if (RadioButtons[DEF_SYSTEM.CAM_TWO].Name == rdb.Name)
                    {
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Crack ROI Teaching Cam2 Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_TWO;
                    }
                    else if (RadioButtons[DEF_SYSTEM.CAM_THREE].Name == rdb.Name)
                    {
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Crack ROI Teaching Cam3 Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_THREE;
                    }
                    else if (RadioButtons[DEF_SYSTEM.CAM_FOUR].Name == rdb.Name)
                    {
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Crack ROI Teaching Cam4 Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_FOUR;
                    }

                    //ToolBlockListShow();
                    ToolListShow();
                    //240709 NIS Cam 변경 시 마다 Inspection Display
                    CreatLabeling(m_InspectionImage);
                }
            }
            catch (Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Form Teach Crack Exception Catch : {ex.Message}");
            }
        }
        private void Run_ApplyAndSave()
        {
            ICogTool tool = m_TeachingToolList[m_nToolSelected];

            //int ImageIndex = cbo_ImageIndex.SelectedIndex;

            if (tool.Name.Contains("FindLine")) // Tool 이름으로 구분하여 Parameter Apply
            {
                CogFindLineTool FindLine = tool as CogFindLineTool;
                FindLineParameterApply(FindLine);
            }
            else if (tool.Name.Contains("PMAlign"))
            {
                CogPMAlignTool PMAlign = tool as CogPMAlignTool;
            }

            theRecipe.m_CogInspToolBlock[m_nSelectCam] = m_TeachingToolBlock;

            theRecipe.ToolSave(theRecipe.m_sCurrentModelName);
            theRecipe.DataSave(theRecipe.m_sCurrentModelName);

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                theMainSystem.Cameras[i].ToolBlockRefresh();
        }
        private void ApplyAndSave_Click(object sender, EventArgs e)
        {
            cbo_CellIndex.SelectedIndex = 0;

            Thread_ApplyAndSave.Continue();
        }
        //private void ToolBlockListShow()
        //{
        //    m_TeachingToolBlockList.Clear();
        //    lib_ToolBlockList.Items.Clear();

        //    if (theRecipe.m_CogPreToolBlock[m_nSelectCam].Tools == null)
        //        return;
        //    for (int i = 0; i < theRecipe.m_CogPreToolBlock[m_nSelectCam].Tools.Count; i++)
        //    {
        //        ICogTool tool = theRecipe.m_CogPreToolBlock[m_nSelectCam].Tools[i];
        //        string sTemp = tool.Name;

        //        lib_ToolBlockList.Font = font;
        //        lib_ToolBlockList.Items.Add(sTemp);
        //        m_TeachingToolBlockList.Add(tool);
        //    }

        //    lib_ToolBlockList.SetSelected(0, true);
        //}

        private void ToolListShow()   //240720 NIS Set ToolList
        {
            //250909 SHJ 현재 Display 에서 룰베이스 사용 하기 떄문에 디스플레이 조건에 맞춰서 Tool 정보 표시 
            if (theRecipe.m_sCurrentEquipment.Contains("DISPLAY")) //250804 LYK 임시 처리
            {
                m_TeachingToolList.Clear();
                lib_ToolList.Items.Clear();

                int ImageIndex = cbo_ImageIndex.SelectedIndex;

                // 250514 SHJ ToolBlock 을 현재 Inspect 에서 깊은 복사를 사용해서 실행 하기 때문에 Recipe ToolBlock 을 이용해서 마지막 이미지 기준으로 Run 실행 
                //theMainSystem.Cameras[m_nSelectCam].RunTeachingInspect(ImageIndex);

                // 250514 SHJ ToolBlock 없으면 종료 
                if (theRecipe.m_CogInspToolBlock[m_nSelectCam] == null)
                    return;
                else
                    m_TeachingToolBlock = theMainSystem.Cameras[m_nSelectCam].GetInspToolBlock(ImageIndex);

                for (int i = 0; i < m_TeachingToolBlock.Tools.Count; i++)
                {
                    ICogTool tool = m_TeachingToolBlock.Tools[i];
                    string sTemp = tool.Name;

                    if (sTemp.Contains("FindLine"))
                    {
                        lib_ToolList.Font = font;
                        lib_ToolList.Items.Add(sTemp);
                        m_TeachingToolList.Add(tool);
                    }
                }

                lib_ToolList.SetSelected(0, true);
            }
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

        private void FindLineShowTool(CogFindLineTool _pFindLine)       //240720 NIS Display findline result
        {
            m_InspectionImage = _pFindLine.InputImage;

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

            // 250515 SHJ 화면에서 캘리퍼 조정할 경우 저장된 값과 현재 Line Tool 값 비교해서 변경 된 정보 화면 출력
            m_nLinePointX = (int)_pFindLine.RunParams.ExpectedLineSegment.MidpointX;
            m_nLinePointY = (int)_pFindLine.RunParams.ExpectedLineSegment.MidpointY;
            m_dSearchLength = _pFindLine.RunParams.CaliperSearchLength;
            m_dProjectionLength = _pFindLine.RunParams.CaliperProjectionLength;
        }
        private void SetData()          //240720 NIS Set SearchDirection combobox
        {
            cbo_SearchDirection.Font = ComboBoxFont;
            cbo_SearchDirection.Items.Add("Light to Dark Polarity");
            cbo_SearchDirection.Items.Add("Dark to Light Polarity");
            cbo_SearchDirection.Items.Add("Random Polarity");

            cbo_ImageIndex.Font = ComboBoxFont;
            int ImgCount = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? theRecipe.SliceImageCount : theRecipe.MergeImageCount;
            for (int i = 0; i < ImgCount; i++)
                cbo_ImageIndex.Items.Add($"Image Index: {i}");

            cbo_ImageIndex.SelectedIndex = 0;

            // 20250911 SHJ Display 에 필요한 Teaching 정보임으로 디스플레일 경우만 활성화 
            if(theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY)
            {
                lbl_CellIndex.Visible = true;
                cbo_CellIndex.Visible = true;

                cbo_CellIndex.Font = ComboBoxFont;
                for (int i = 1; i < theRecipe.MaterialInfo.m_nCellVertCount + 1; i++)
                    cbo_CellIndex.Items.Add($"Cell Index : {i}");

                cbo_CellIndex.SelectedIndex = 0;
            }
            else
            {
                lbl_CellIndex.Visible = false;
                cbo_CellIndex.Visible = false;
            }
        }
        private void CreatLabeling(CogImage8Grey _Image)
        {
            // 250909 SHJ 이미지가 있는 경우 진입 
            if (_Image != null)
            {
                //250909 SHJ 최종 검사 화면 출력 
                try
                {
                    // 전체 ToolBlock 실행 
                    int ImageIndex = cbo_ImageIndex.SelectedIndex;
                    theMainSystem.Cameras[m_nSelectCam].RunTeachingInspect(ImageIndex);

                    CogRecord MainRecord = new CogRecord("Main", _Image.GetType(), CogRecordUsageConstants.Result, false, _Image, "Main");
                    CogGraphicCollection GraphicCollection = new CogGraphicCollection();

                    for (int i = 0; i < m_TeachingToolBlock.Tools.Count; i++)
                    {
                        if (m_TeachingToolBlock.Tools[i].GetType().ToString().Contains("FindLineTool"))
                        {
                            CogFindLineTool tool = theRecipe.m_CogInspToolBlock[m_nSelectCam].Tools[i] as CogFindLineTool;
                            MainRecord.SubRecords.Add(tool.CreateLastRunRecord());
                        }

                    }

                    List<CogPolygon> SearchAreaList = new List<CogPolygon>();

                    // 20250909 SHJ Output 중 타입이 ListPolygon 이 있을 경우 입력 받아 출력 
                    for (int i = 0; i < m_TeachingToolBlock.Outputs.Count; i++)
                    {
                        if (m_TeachingToolBlock.Outputs[i].ValueType == typeof(List<CogPolygon>))
                            SearchAreaList = m_TeachingToolBlock.Outputs[i].Value as List<CogPolygon>;
                    }

                    for (int i = 0; i < SearchAreaList.Count; i++)
                    {
                        GraphicCollection.Add(SearchAreaList[i]);
                    }


                    MainRecord.SubRecords.Add(new CogRecord("Graphic", typeof(CogGraphicCollection), CogRecordUsageConstants.Result, false, GraphicCollection, "Graphic"));

                    ImageDisplay.Record = MainRecord;
                    ImageDisplay.Fit();
                }
                catch (Exception ex)
                {
                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Teaching ROI Tool Error {ex.ToString()}");
                }
            }
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
        private void Disable_RadioButtons()
        {
            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                this.Controls.Find($"rdb_Cam{i + 1}", true)[0].Visible = true;
            }
        }

        private void FormTeachInspectionCrack_FormClosed(object sender, FormClosedEventArgs e)
        {
            Thread_ApplyAndSave.ThreadStop();
        }
       
        private void Reset_DisplayScreen()
        {
            ImageDisplay.Record = null;
        }

        private void FormTeachInspectionCrack_VisibleChanged(object sender, EventArgs e)
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
                        m_InspectionImage = new CogImage8Grey(new Bitmap(fileDialog.FileNames[0]));
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

        private void CurrentImageDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            m_nToolSelected = lib_ToolList.SelectedIndex;
            ICogTool tool = m_TeachingToolList[m_nToolSelected];
            if (tool.GetType().ToString().Contains("CogFindLine"))
            {
                CogFindLineTool FindLine = tool as CogFindLineTool;

                int Current_PointX = (int)FindLine.RunParams.ExpectedLineSegment.MidpointX;
                int Current_PointY = (int)FindLine.RunParams.ExpectedLineSegment.MidpointY;
                double Current_SearchLength = FindLine.RunParams.CaliperSearchLength;
                double Current_ProjectionLength = FindLine.RunParams.CaliperProjectionLength;

                // 250515 SHJ 화면 출력할때 정보와 마우스 이동 후 Line Tool 정보가 일치 하지 않으면 Parameter 업데이트 
                if ((m_nLinePointX != Current_PointX) || (m_nLinePointY != Current_PointY) ||
                     (m_dSearchLength != Current_SearchLength) || (m_dProjectionLength != Current_ProjectionLength))
                {

                    SetTextFindLineData(FindLine);
                }
            }
        }

        private void cbo_ImageIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolListShow();
        }

        private void cbo_CellIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            int CellIndex = cbo_CellIndex.SelectedIndex;

            m_nToolSelected = lib_ToolList.SelectedIndex;
            ICogTool tool = m_TeachingToolList[m_nToolSelected];
            if (tool.GetType().ToString().Contains("CogFindLine"))
            {
                CogFindLineTool FindLine = tool as CogFindLineTool;

                if (CellIndex >= 0)
                {
                    // 기존 위치로 원복 
                    double Offset = (theRecipe.MaterialInfo.m_dCellHeight + theRecipe.MaterialInfo.m_dCellPadHeight) * m_nCellIndex;

                    FindLine.RunParams.ExpectedLineSegment.StartY -= Offset;
                    FindLine.RunParams.ExpectedLineSegment.EndY -= Offset;

                    // 선택 된 Cell 위치까지 이동 
                    Offset = (theRecipe.MaterialInfo.m_dCellHeight + theRecipe.MaterialInfo.m_dCellPadHeight) * (CellIndex);
                    FindLine.RunParams.ExpectedLineSegment.StartY += Offset;
                    FindLine.RunParams.ExpectedLineSegment.EndY += Offset;
                }
            }

            m_nCellIndex = CellIndex;
        }
    }
}
