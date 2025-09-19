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
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro;

namespace Core.UI
{
    public partial class FormTeachInsBlob : Form, ITeachForm
    {
        private MainForm MainForm;
        private FormTeachInspection TeachMainForm;  //parent form
        private int m_nSelectCam = 0;       //Selected cam number
        private int m_nToolSelected = 0;    //Selected tool list index
        private int m_nInspectSelected = 0; //Selected inspection mode

        private string sBlobToolName = string.Empty; //250329 KCH BlobTool 분류용 변수 

        private Font font = new Font("Calibri", 18, FontStyle.Bold);
        private Font ComboBoxFont = new Font("Calibri", 9, FontStyle.Bold);

        private Point TeachFormLocation;
        public RadioButton[] RadioButtons = new RadioButton[4];

        private InsToolBlock _InsToolBlock = null;
        private List<IInsTool> m_InsTeachingToolList = new List<IInsTool>();   //Teaching CogTool List
        private InsImage8Grey InspectionImage = null;

        private CLogger Logger = null;

        public FormTeachInsBlob(MainForm _MainForm, FormTeachInspection _TeachMainForm, CLogger _logger)
        {
            InitializeComponent();
            TopLevel = false;
            TeachMainForm = _TeachMainForm;
            MainForm = _MainForm;
            MainForm.SetEachControlResize(this);    

            Set_RadioButtonsList();
            Hide_RadioButton();
            m_nInspectSelected = (int)INSP_MODE.Focus;
            DoLoad();
            ToolListShow();

            TeachFormLocation = TeachMainForm.Get_TeachFormScreenLocation();  //240721 NIS Get TeachFormScreen Location
            Logger = _logger;
        }
        private void DoLoad()
        {
            SetData();
            DisplayRecipeData();
        }
        private void DisplayRecipeData()
        {
            
            tb_FingerWidthMax.Text = theRecipe.MonoLimit.m_dFingerWidth_Max.ToString("F3");
            tb_FingerWidthMin.Text = theRecipe.MonoLimit.m_dFingerWidth_Min.ToString("F3");
            tb_QLogoLimit.Text = theRecipe.MonoLimit.m_dQLogoAreaLimit.ToString("F3");
            
        }
        private void btn_LogicToolBlock_Click(object sender, EventArgs e)   //240720 NIS Display ToolBlockEditPage
        {
            
            _InsToolBlock = ((InsToolBlock)theRecipe.m_InsInspToolBlock[m_nSelectCam]);

            ToolBlockEdit ToolEdit = new ToolBlockEdit(m_nInspectSelected, null, _InsToolBlock); //240703 NIS 실행한 TeachMode에 따라서 ToolBlockEdit 타이틀 수정
            ToolEdit.Location = TeachFormLocation;
            ToolEdit.ShowDialog();
        }
        private void btn_Run_Click(object sender, EventArgs e)
        {
            if (InspectionImage != null)
            {
                InsFindLineTool FindLine = m_InsTeachingToolList[lib_ToolList.SelectedIndex] as InsFindLineTool;
                IInsTool SelectTool = m_InsTeachingToolList[m_nToolSelected];

                InsImageSharpnessTool sharpnessTool = ((InsToolBlock)theRecipe.m_InsInspToolBlock[m_nSelectCam]).Tools["ImageSharpnessTool_1"] as InsImageSharpnessTool;

                sBlobToolName = SelectTool.Name.Contains("FingerWidth") ? "BlobTool_FingerWidth" : "BlobTool_QLogo";
                InsBlobTool blobTool = ((InsToolBlock)theRecipe.m_InsInspToolBlock[m_nSelectCam]).Tools[$"{sBlobToolName}"] as InsBlobTool;
                
                System.Threading.Thread.Sleep(50);

                int nCamCount = 1;//theRecipe.m_sCurrentCountry == "KOREA" && !theRecipe.m_sCurrentEquipment.Contains("CVD") ? theRecipe.m_nMaxCamCount - 1 : theRecipe.m_nMaxCamCount;

                for (int i = 0; i < nCamCount; i++)
                {
                    ToolBlockRun(i, ((InsToolBlock)theRecipe.m_InsInspToolBlock[i]), m_nInspectSelected);
                }

                if (SelectTool.Name.Contains("Blob"))
                {
                    BlobParameterApply(blobTool, sBlobToolName);
                    BlobShowTool(blobTool);
                    if (sBlobToolName == "BlobTool_FingerWidth")
                    {
                        FingWidthResult(m_nSelectCam);
                    }
                    else
                    {
                        QLogoResult(m_nSelectCam);
                    }
                }
                else if (SelectTool.Name.Contains("FindLine"))
                {
                    FindLineParameterApply(FindLine);
                    FindLineShowTool(FindLine);
                    FindLineResult(m_nSelectCam);
                }
                else
                {
                    SharpnessToolApply(sharpnessTool);
                    SharpnessShowTool(sharpnessTool);
                    SharpnessResult(m_nSelectCam);
                }
                double d_FingerWidthAvg = (double)((InsToolBlock)theRecipe.m_InsInspToolBlock[m_nSelectCam]).Outputs[$"FingerWidthAvg"].Value;
                tb_FingerWidthAvg.Text = d_FingerWidthAvg.ToString("F3");
                double d_Sharpness = (double)((InsToolBlock)theRecipe.m_InsInspToolBlock[m_nSelectCam]).Outputs[$"Focus"].Value;
                tb_SharpnessScore.Text = d_Sharpness.ToString("F3");
                double d_QLogoArea = (double)((InsToolBlock)theRecipe.m_InsInspToolBlock[m_nSelectCam]).Outputs[$"QLogoArea"].Value;// * theMainSystem.m_darrPixelResolution[m_nSelectCam] * theMainSystem.m_darrPixelResolution[m_nSelectCam];
                tb_QLogoArea.Text = d_QLogoArea.ToString("F3");
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
            m_nToolSelected = lib_ToolList.SelectedIndex;
            IInsTool selectedTool = m_InsTeachingToolList[m_nToolSelected];

            //250329 KCH 모든 패널을 먼저 숨김
            Panel_FingerWidth_FindLine.Visible = false;
            Panel_FingerWidth_Blob.Visible = false;
            Panel_Qlogo_Blob.Visible = false;
            Panel_Sharpness.Visible = false;

            string toolName = selectedTool.Name;
            if (toolName.Contains("FindLine") && selectedTool is InsFindLineTool findLineTool)
            {
                Panel_FingerWidth_FindLine.Visible = true;
                SetTextFindLineData(findLineTool);
                FindLineShowTool(findLineTool);
                FindLineResult(m_nSelectCam);
            }
            else if (toolName.Contains("FingerWidth") && selectedTool is InsBlobTool fingerWidthTool)
            {
                Panel_FingerWidth_Blob.Visible = true;
                SetTextBLobData(fingerWidthTool, "BlobTool_FingerWidth");
                BlobShowTool(fingerWidthTool);
                FingWidthResult(m_nSelectCam);
            }
            else if (toolName.Contains("QLogo") && selectedTool is InsBlobTool qLogoTool)
            {
                Panel_Qlogo_Blob.Visible = true;
                SetTextBLobData(qLogoTool, "BlobTool_QLogo");
                BlobShowTool(qLogoTool);
                QLogoResult(m_nSelectCam);
            }
            else if (toolName.Contains("Sharp") && selectedTool is InsImageSharpnessTool sharpnessTool)
            {
                Panel_Sharpness.Visible = true;
                SetSharpnessToolData(sharpnessTool);
                SharpnessShowTool(sharpnessTool);
                SharpnessResult(m_nSelectCam);
            }

        }

        private void SetTextBLobData(InsBlobTool _pBlobTool, string _sBlobToolName)
        {
            if (_sBlobToolName == "BlobTool_FingerWidth")
            {
                tb_FingerWidth_MinPixels.Text = _pBlobTool.RunParams.ConnectivityMinPixels.ToString();
                tb_FingerWidth_Threshold.Text = _pBlobTool.RunParams.SegmentationParams.HardFixedThreshold.ToString();

                int nDirection = 0;

                if (_pBlobTool.RunParams.SegmentationParams.Polarity == InsBlobSegmentationPolarityConstants.LightBlobs)
                    nDirection = 0;
                else if (_pBlobTool.RunParams.SegmentationParams.Polarity == InsBlobSegmentationPolarityConstants.DarkBlobs)
                    nDirection = 1;

                cb_FingerWidth_Polarity.SelectedIndex = nDirection;
            }
            else
            {
                tb_Qlogo_MinPixels.Text = _pBlobTool.RunParams.ConnectivityMinPixels.ToString();
                tb_Qlogo_Threshold.Text = _pBlobTool.RunParams.SegmentationParams.HardFixedThreshold.ToString();

                int nDirection = 0;

                if (_pBlobTool.RunParams.SegmentationParams.Polarity == InsBlobSegmentationPolarityConstants.LightBlobs)
                    nDirection = 0;
                else if (_pBlobTool.RunParams.SegmentationParams.Polarity == InsBlobSegmentationPolarityConstants.DarkBlobs)
                    nDirection = 1;

                cb_QLogo_Polarity.SelectedIndex = nDirection;
            }
            
        }

        private void BlobParameterApply(InsBlobTool _pBlobTool, string _sBlobToolName)
        {
            if (_sBlobToolName == "BlobTool_FingerWidth")
            {
                _pBlobTool.RunParams.ConnectivityMinPixels = int.Parse(tb_FingerWidth_MinPixels.Text);
                _pBlobTool.RunParams.SegmentationParams.HardFixedThreshold = int.Parse(tb_FingerWidth_Threshold.Text);

                int nDirection = cb_FingerWidth_Polarity.SelectedIndex;
                if (nDirection == 0)
                    _pBlobTool.RunParams.SegmentationParams.Polarity = InsBlobSegmentationPolarityConstants.LightBlobs;
                else if (nDirection == 1)
                    _pBlobTool.RunParams.SegmentationParams.Polarity = InsBlobSegmentationPolarityConstants.LightBlobs;
            }
            else
            {
                _pBlobTool.RunParams.ConnectivityMinPixels = int.Parse(tb_Qlogo_MinPixels.Text);
                _pBlobTool.RunParams.SegmentationParams.HardFixedThreshold = int.Parse(tb_Qlogo_Threshold.Text);

                int nDirection = cb_QLogo_Polarity.SelectedIndex;
                if (nDirection == 0)
                    _pBlobTool.RunParams.SegmentationParams.Polarity = InsBlobSegmentationPolarityConstants.LightBlobs;
                else if (nDirection == 1)
                    _pBlobTool.RunParams.SegmentationParams.Polarity = InsBlobSegmentationPolarityConstants.LightBlobs;
            }

        }

        private void SharpnessShowTool(InsImageSharpnessTool _pSharpnessTool)
        {
            InsRecord CurrRecord = new InsRecord();
            CurrRecord.ContentType = typeof(IInsImage);
            CurrRecord.Content = _pSharpnessTool.InputImage;
            CurrRecord.Annotation = "InputImage";
            CurrRecord.RecordKey = "InputImage";
            CurrRecord.RecordUsage = InsRecordUsageConstants.Diagnostic;

            CurrRecord.SubRecords.Add(_pSharpnessTool.CreateCurrentRecord());
            CurrentImageDisplay.Record = CurrRecord;
            CurrentImageDisplay.FitImage();

            InsRecord LastRecord = new InsRecord();
            LastRecord.ContentType = typeof(IInsImage);
            LastRecord.Content = _pSharpnessTool.InputImage;
            LastRecord.Annotation = "InputImage";
            LastRecord.RecordKey = "InputImage";
            LastRecord.RecordUsage = InsRecordUsageConstants.Diagnostic;

            LastRecord.SubRecords.Add(_pSharpnessTool.CreateLastRunRecord());
            LastRunImageDisplay.Record = LastRecord;
            LastRunImageDisplay.FitImage();
        }

        private void BlobShowTool(InsBlobTool _pBlob)
        {
            InsRecord CurrRecord = new InsRecord();
            CurrRecord.ContentType = typeof(IInsImage);
            CurrRecord.Content = _pBlob.InputImage;
            CurrRecord.Annotation = "InputImage";
            CurrRecord.RecordKey = "InputImage";
            CurrRecord.RecordUsage = InsRecordUsageConstants.Diagnostic;

            CurrRecord.SubRecords.Add(_pBlob.CreateCurrentRecord());
            CurrentImageDisplay.Record = CurrRecord;
            CurrentImageDisplay.FitImage();

            InsRecord LastRecord = new InsRecord();
            LastRecord.ContentType = typeof(IInsImage);
            LastRecord.Content = _pBlob.InputImage;
            LastRecord.Annotation = "InputImage";
            LastRecord.RecordKey = "InputImage";
            LastRecord.RecordUsage = InsRecordUsageConstants.Diagnostic;

            LastRecord.SubRecords.Add(_pBlob.CreateLastRunRecord());
            LastRunImageDisplay.Record = LastRecord;
            LastRunImageDisplay.FitImage();
        }



        private void rdb_CheckedChanged(object sender, EventArgs e)
        {
            
            RadioButton rdb = sender as RadioButton;
            try
            {
                if (RadioButtons[DEF_SYSTEM.CAM_ONE].Name == rdb.Name)
                {
                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"FingerWidth ROI Teaching Cam1 Radio Button Click.");
                    m_nSelectCam = DEF_SYSTEM.CAM_ONE;
                }
                else if (RadioButtons[DEF_SYSTEM.CAM_TWO].Name == rdb.Name)
                {
                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"FingerWidth ROI Teaching Cam2 Radio Button Click.");
                    m_nSelectCam = DEF_SYSTEM.CAM_TWO;
                }
                else if (RadioButtons[DEF_SYSTEM.CAM_THREE].Name == rdb.Name)
                {
                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"FingerWidth ROI Teaching Cam3 Radio Button Click.");
                    m_nSelectCam = DEF_SYSTEM.CAM_THREE;
                }
                else if (RadioButtons[DEF_SYSTEM.CAM_FOUR].Name == rdb.Name)
                {
                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"FingerWidth ROI Teaching Cam4 Radio Button Click.");
                    m_nSelectCam = DEF_SYSTEM.CAM_FOUR;
                }
                InsImageSharpnessTool sharpnessTool = ((InsToolBlock)theRecipe.m_InsInspToolBlock[m_nSelectCam]).Tools["ImageSharpnessTool_1"] as InsImageSharpnessTool;
                SetSharpnessToolData(sharpnessTool);

                InsBlobTool FingerWidthblobTool = ((InsToolBlock)theRecipe.m_InsInspToolBlock[m_nSelectCam]).Tools["BlobTool_FingerWidth"] as InsBlobTool;
                SetTextBLobData(FingerWidthblobTool, "BlobTool_FingerWidth");

                InsBlobTool QLogoblobTool = ((InsToolBlock)theRecipe.m_InsInspToolBlock[m_nSelectCam]).Tools["BlobTool_QLogo"] as InsBlobTool;
                SetTextBLobData(QLogoblobTool, "BlobTool_QLogo");
                ToolListShow();

                //240709 NIS Cam 변경 시 마다 Inspection Display
                TeachInspectionRun();
            }
            catch(Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Form Teach FingerWidth Exception Catch : {ex.Message}");
            }
            
        }

        private void SetSharpnessToolData(InsImageSharpnessTool _sharpnessTool)
        {

            int nDirection = 0;

            if (_sharpnessTool.RunParams.EvaluationMode == InsImageSharpnessModeConstants.Brenner)
                nDirection = 0;
            else if (_sharpnessTool.RunParams.EvaluationMode == InsImageSharpnessModeConstants.EAV)
                nDirection = 1;
            else if (_sharpnessTool.RunParams.EvaluationMode == InsImageSharpnessModeConstants.EnergyGradient)
                nDirection = 2;
            else if (_sharpnessTool.RunParams.EvaluationMode == InsImageSharpnessModeConstants.Laplacian)
                nDirection = 3;
            else if (_sharpnessTool.RunParams.EvaluationMode == InsImageSharpnessModeConstants.NoReferenceImageQualityAssessment)
                nDirection = 4;
            else if (_sharpnessTool.RunParams.EvaluationMode == InsImageSharpnessModeConstants.NRSS)
                nDirection = 5;
            else if (_sharpnessTool.RunParams.EvaluationMode == InsImageSharpnessModeConstants.SMD)
                nDirection = 6;
            else if (_sharpnessTool.RunParams.EvaluationMode == InsImageSharpnessModeConstants.SMD2)
                nDirection = 7;
            else if (_sharpnessTool.RunParams.EvaluationMode == InsImageSharpnessModeConstants.Tenengrad)
                nDirection = 8;


            cbo_EvaluationMode.SelectedIndex = nDirection;
        }

        private void SharpnessToolApply(InsImageSharpnessTool _sharpnessTool)
        {

            int nDirection = cbo_EvaluationMode.SelectedIndex;
            if (nDirection == 0)
                _sharpnessTool.RunParams.EvaluationMode = InsImageSharpnessModeConstants.Brenner;
            else if (nDirection == 1)
                _sharpnessTool.RunParams.EvaluationMode = InsImageSharpnessModeConstants.EAV;
            else if (nDirection == 2)
                _sharpnessTool.RunParams.EvaluationMode = InsImageSharpnessModeConstants.EnergyGradient;
            else if (nDirection == 3)
                _sharpnessTool.RunParams.EvaluationMode = InsImageSharpnessModeConstants.Laplacian;
            else if (nDirection == 4)
                _sharpnessTool.RunParams.EvaluationMode = InsImageSharpnessModeConstants.NoReferenceImageQualityAssessment;
            else if (nDirection == 5)
                _sharpnessTool.RunParams.EvaluationMode = InsImageSharpnessModeConstants.NRSS;
            else if (nDirection == 6)
                _sharpnessTool.RunParams.EvaluationMode = InsImageSharpnessModeConstants.SMD;
            else if (nDirection == 7)
                _sharpnessTool.RunParams.EvaluationMode = InsImageSharpnessModeConstants.SMD2;
            else if (nDirection == 8)
                _sharpnessTool.RunParams.EvaluationMode = InsImageSharpnessModeConstants.Tenengrad;
        }

        private void ApplyAndSave_Click(object sender, EventArgs e)
        {
            Run_ApplyAndSave(); //241210 Apply Save Thread 삭제
        }

        private void Run_ApplyAndSave()
        {
            
            IInsTool tool = m_InsTeachingToolList[m_nToolSelected];
            InsImageSharpnessTool sharpnessTool = ((InsToolBlock)theRecipe.m_InsInspToolBlock[m_nSelectCam]).Tools["ImageSharpnessTool_1"] as InsImageSharpnessTool;
            InsBlobTool FingerWidthblobTool = ((InsToolBlock)theRecipe.m_InsInspToolBlock[m_nSelectCam]).Tools["BlobTool_FingerWidth"] as InsBlobTool;
            InsBlobTool QLogoblobTool = ((InsToolBlock)theRecipe.m_InsInspToolBlock[m_nSelectCam]).Tools["BlobTool_QLogo"] as InsBlobTool;

            SharpnessToolApply(sharpnessTool);
            BlobParameterApply(FingerWidthblobTool, "BlobTool_FingerWidth");
            BlobParameterApply(QLogoblobTool, "BlobTool_QLogo");

            if (tool.Name.Contains("FindLine")) 
            {
                InsFindLineTool FindLine = tool as InsFindLineTool;
                FindLineParameterApply(FindLine);
            }
            
            SetRecipeData();    //250324 NIS QLogo Area limit 설정

            theRecipe.ToolSave(theRecipe.m_sCurrentModelName);
            theRecipe.DataSave(theRecipe.m_sCurrentModelName);
        }
        private void SetRecipeData()    //250324 NIS Recipe 값 할당
        {
            double QLogoLimit = 0, FingerWidthMin = 0, FingerWidthMax = 0;
            bool IsConvert = double.TryParse(tb_QLogoLimit.Text, out QLogoLimit);
            IsConvert &= double.TryParse(tb_FingerWidthMin.Text, out FingerWidthMin) && double.TryParse(tb_FingerWidthMax.Text, out FingerWidthMax);
            
            
            if (IsConvert)
            {
                theRecipe.MonoLimit.m_dQLogoAreaLimit = QLogoLimit;
                theRecipe.MonoLimit.m_dFingerWidth_Min = FingerWidthMin;
                theRecipe.MonoLimit.m_dFingerWidth_Max = FingerWidthMax;
            }
            else
            {
                MessageBox.Show("Data Convert Failed", "", MessageBoxButtons.OK);

                theRecipe.MonoLimit.m_dQLogoAreaLimit = QLogoLimit;
                theRecipe.MonoLimit.m_dFingerWidth_Min = FingerWidthMin;
                theRecipe.MonoLimit.m_dFingerWidth_Max = FingerWidthMax;
            }
            
        }

        
        private void ToolListShow()   //240720 NIS Set ToolList
        {
           
            m_InsTeachingToolList.Clear();
            lib_ToolList.Items.Clear();
            
            if (DEF_SYSTEM.LICENSES_KEY == (int)License.INS)
            {
                if ((InsToolBlock)theRecipe.m_InsInspToolBlock[m_nSelectCam] == null)
                    return;

                for (int i = 0; i < ((InsToolBlock)theRecipe.m_InsInspToolBlock[m_nSelectCam]).Tools.Count; i++)
                {
                    IInsTool tool = ((InsToolBlock)theRecipe.m_InsInspToolBlock[m_nSelectCam]).Tools[i];
                    string sTemp = tool.Name;

                    //if (sTemp.Contains("Hor") || sTemp.Contains("Ver"))
                    if (sTemp.Contains("FindLine") || sTemp.Contains("Blob") || sTemp.Contains("Sharp"))
                    {
                        lib_ToolList.Font = font;

                        lib_ToolList.Items.Add(sTemp);
                        m_InsTeachingToolList.Add(tool);
                    }

                }
                lib_ToolList.SetSelected(0, true);
            }
            
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

                int nDirection = cbo_SearchDirection.SelectedIndex;
                if (nDirection == 0)             //231007 LYK 밝은 곳에서 어두운 곳
                    _pFindLine.RunParams.CaliperRunParams.Edge0Polarity = InsCaliperPolarityConstants.LightToDark;
                else if (nDirection == 1)        //231007 LYK 어두운곳에서 밝은 곳
                    _pFindLine.RunParams.CaliperRunParams.Edge0Polarity = InsCaliperPolarityConstants.DarkToLight;
                else if (nDirection == 2)           //231007 LYK 임의의 극성
                    _pFindLine.RunParams.CaliperRunParams.Edge0Polarity = InsCaliperPolarityConstants.DontCare;

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
        private void ToolBlockRun(int _nCamNum, InsToolBlock _toolBlock,int _nMode)    //240720 NIS ToolBlock Run
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

            int nDirection = 0;

            if (_pFindLine.RunParams.CaliperRunParams.Edge0Polarity == InsCaliperPolarityConstants.LightToDark)             //231007 LYK 밝은 곳에서 어두운 곳
                nDirection = 0;
            else if (_pFindLine.RunParams.CaliperRunParams.Edge0Polarity == InsCaliperPolarityConstants.DarkToLight)        //231007 LYK 어두운곳에서 밝은 곳
                nDirection = 1;
            else if (_pFindLine.RunParams.CaliperRunParams.Edge0Polarity == InsCaliperPolarityConstants.DontCare)           //231007 LYK 임의의 극성
                nDirection = 2;

            cbo_SearchDirection.SelectedIndex = nDirection;

            txt_Score.Text = _pFindLine.RunParams.CaliperRunParams.SingleEdgeScorers[0].X0.ToString();
            
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

           
        }
        private void TeachInspectionRun()   //240709 NIS Cam 변경 시 마다 Inspection Display
        {
            if (InspectionImage != null)
            {
                FindLineResult(m_nSelectCam);

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Controller Reset");
            }
        }
        private void SetData()          //240720 NIS Set SearchDirection combobox
        {
            cbo_SearchDirection.Font = ComboBoxFont;
            cbo_SearchDirection.Items.Add("Light to Dark Polarity");
            cbo_SearchDirection.Items.Add("Dark to Light Polarity");
            cbo_SearchDirection.Items.Add("Random Polarity");
        }
        private void FindLineResult(int SelectCamNum)       //240702 NIS TeachPage EdgeToBusbar Trigger Run시 우측에 결과 디스플레이
        {
            if (InspectionImage != null)
            {
                InsRecord MainRecord = new InsRecord("Main", InspectionImage.GetType(), InsRecordUsageConstants.Result, false, InspectionImage as InsImage8Grey, "Main");
                InsRecord SubRecord = new InsRecord("FingerWidth", typeof(InsImage8Grey), InsRecordUsageConstants.Input, false, InspectionImage, "FingerWidth");

                foreach (IInsTool Instool in ((InsToolBlock)theRecipe.m_InsInspToolBlock[SelectCamNum]).Tools) //241210 KCH 결과에 IntersectLine Paint
                {
                    if (Instool.Name.Contains("Find"))
                    {
                        InsFindLineTool FindLine = Instool as InsFindLineTool;
                        SubRecord.AddGraphicToRecord(FindLine.Results.GetLine(), "FingerWidth", "");
                    }
                }
                MainRecord.SubRecords.Add(SubRecord);


                ImageDisplay.Record = MainRecord;
                ImageDisplay.FitImage();
            }
        }

        private void FingWidthResult(int SelectCamNum)       
        {
            InsBlobTool FingerWidthblobtool = ((InsToolBlock)theRecipe.m_InsInspToolBlock[SelectCamNum]).Tools["BlobTool_FingerWidth"] as InsBlobTool;

            ImageDisplay.Record = FingerWidthblobtool.CreateLastRunRecord();
            ImageDisplay.ShowRecordID = 1;
            ImageDisplay.FitImage();
            
        }

        private void QLogoResult(int SelectCamNum)       
        {
            InsBlobTool QLogoblobtool = ((InsToolBlock)theRecipe.m_InsInspToolBlock[SelectCamNum]).Tools["BlobTool_QLogo"] as InsBlobTool;

            ImageDisplay.Record = QLogoblobtool.CreateLastRunRecord();
            ImageDisplay.ShowRecordID = 1;
            ImageDisplay.FitImage();
        }

        private void SharpnessResult(int SelectCamNum)       
        {
            InsImageSharpnessTool sharpnessTool = ((InsToolBlock)theRecipe.m_InsInspToolBlock[m_nSelectCam]).Tools["ImageSharpnessTool_1"] as InsImageSharpnessTool;

            ImageDisplay.Record = sharpnessTool.CreateLastRunRecord();
            ImageDisplay.FitImage();
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
        }
        private void Hide_RadioButton()
        {
            RadioButtons[1].Visible = false;
            RadioButtons[2].Visible = false;
            RadioButtons[3].Visible = false;
        }

       
        private void Reset_DisplayScreen()
        {
            ImageDisplay.Record = null;
        }

        private void FormInsTeachInspectionFingerWidth_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                Reset_DisplayScreen();
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
    }
}