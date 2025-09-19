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
using Core.Process;

using static Core.Program;

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
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.Implementation;
using Cognex.VisionPro;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Net;
using Cognex.VisionPro.CalibFix;
using static Insnex.Vision2D.Common.InsFactory;

namespace Core.UI
{
    public partial class FormTeachInsFindLine : Form, ITeachForm
    {
        private MainForm MainForm;
        private CCameraManager cCameraManager = null;
        private FormTeachInspection TeachMainForm;  //parent form
        private int m_nSelectCam = 0;       //Selected cam number
        private int m_nToolBlockSelected = 0;    //Selected tool list index
        private int m_nToolSelected = 0;    //Selected tool list index
        private int m_nInspectSelected = 0; //Selected inspection mode

        private Font font = new Font("Calibri", (int)(18 * theRecipe.m_dUIResizeRatio), FontStyle.Bold);
        private Font ComboBoxFont = new Font("Calibri", (int)(9 * theRecipe.m_dUIResizeRatio), FontStyle.Bold);

        private Point TeachFormLocation;
        public RadioButton[] RadioButtons = new RadioButton[4];

        private double Pixel_Resolution = 0.0235;   //240926 NIS 임시로 하드 코딩. 추후 Calibration 도입 후 수정 필요

        private InsToolBlock _InsToolBlock = null;
        private List<IInsTool> m_InsTeachingToolBlockList = new List<IInsTool>();   //Teaching CogTool List

        private List<IInsTool> m_InsTeachingToolList = new List<IInsTool>();   //Teaching CogTool List
        private InsImage8Grey InspectionImage = null;
        private CLogger Logger = null;

        public FormTeachInsFindLine(MainForm _MainForm, FormTeachInspection _TeachMainForm, CLogger _logger)
        {
            InitializeComponent();
            TopLevel = false;
            TeachMainForm = _TeachMainForm;
            MainForm = _MainForm;
            MainForm.SetEachControlResize(this);    //240801 NIS Conrol Resize

            Set_RadioButtonsList();
            m_nInspectSelected = (int)INSP_MODE.Crack;
            SetData();
            ToolBlockListShow();

            TeachFormLocation = TeachMainForm.Get_TeachFormScreenLocation();  //240721 NIS Get TeachFormScreen Location

            Set_RadioButtonsList();
           
            for(int i = 0; i < 4; i++)
            {
                this.Controls.Find($"rdb_Cam{i + 1}", true)[0].Visible = false;
            }

            Disable_RadioButtons();

            Logger = _logger;
        }

       
        private void btn_LogicToolBlock_Click(object sender, EventArgs e)
        {
            _InsToolBlock = theRecipe.m_InsPreToolBlock[m_nSelectCam] as InsToolBlock;

            ToolBlockEdit ToolEdit = new ToolBlockEdit(m_nInspectSelected, null, _InsToolBlock); //240703 NIS 실행한 TeachMode에 따라서 ToolBlockEdit 타이틀 수정
            ToolEdit.Location = TeachFormLocation;
            ToolEdit.Show();
        }

        private void btn_Run_Click(object sender, EventArgs e)
        {
            if (InspectionImage != null)    //250405 NIS Test용 이미지 로드 없이 실행 시 이미지 로드창 띄우기
            {
                int nSelect = lib_ToolList.SelectedIndex;
                InsFindLineTool FindLine = m_InsTeachingToolList[nSelect] as InsFindLineTool;

                FindLineParameterApply(FindLine);

                System.Threading.Thread.Sleep(100);

                for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                {
                    for (int j = 0; j < theRecipe.m_InsPreToolBlock[i].Tools.Count; j++)
                    {
                        ToolBlockRun(i, theRecipe.m_InsPreToolBlock[i].Tools[j] as InsToolBlock, m_nInspectSelected);
                    }
                }

                FindLineShowTool(FindLine);

                TeachInspectionRun();
            }
        }

        private void btn_Reverse_Click(object sender, EventArgs e)
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
            m_nToolSelected = (int)lib_ToolList.SelectedIndex;

            IInsTool SelectTool = m_InsTeachingToolList[m_nToolSelected];

            if (SelectTool.Name.Contains("FindLine"))
            {
                InsFindLineTool FindLine = SelectTool as InsFindLineTool;

                SetTextFindLineData(FindLine);
                FindLineShowTool(FindLine);
            }
            //else if (SelectTool.Name.Contains("PMAlign"))
            //{
            //    CogPMAlignTool PMAlign = SelectTool as CogPMAlignTool;
            //    PMAlignShowTool(PMAlign);
            //}
        }

        private void rdb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdb = sender as RadioButton;
            try
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

                ToolBlockListShow();

                //240709 NIS Cam 변경 시 마다 Inspection Display
                TeachInspectionRun();
            }
            catch (Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Form Teach Crack Exception Catch : {ex.Message}");
            }
        }

        private void Run_ApplyAndSave()
        {
            IInsTool tool = m_InsTeachingToolList[m_nToolSelected];

            if (tool.Name.Contains("FindLine")) 
            {
                InsFindLineTool FindLine = tool as InsFindLineTool;
                FindLineParameterApply(FindLine);
            }

            theRecipe.ToolSave(theRecipe.m_sCurrentModelName);
            theRecipe.DataSave(theRecipe.m_sCurrentModelName);
        }

        private void ApplyAndSave_Click(object sender, EventArgs e)
        {
            Run_ApplyAndSave(); //241210 Apply Save Thread 삭제
        }

        private void ToolBlockListShow()    //250405 NIS Set ToolBlockList
        {
            m_InsTeachingToolBlockList.Clear();
            lib_ToolBlockList.Items.Clear();

            if (theRecipe.m_InsPreToolBlock[m_nSelectCam].Tools == null)
                return;
            for (int i = 0; i < theRecipe.m_InsPreToolBlock[m_nSelectCam].Tools.Count; i++)
            {
                IInsTool tool = theRecipe.m_InsPreToolBlock[m_nSelectCam].Tools[i];
                string sTemp = tool.Name;

                lib_ToolBlockList.Font = font;
                lib_ToolBlockList.Items.Add(sTemp);
                m_InsTeachingToolBlockList.Add(tool);
            }

            lib_ToolBlockList.SetSelected(0, true);
        }

        private void ToolListShow()   //240720 NIS Set ToolList
        {
            m_InsTeachingToolList.Clear();
            lib_ToolList.Items.Clear();

            if (theRecipe.m_InsPreToolBlock[m_nSelectCam].Tools[m_nToolBlockSelected] == null)
                return;
            for (int i = 0; i < ((InsToolBlock)theRecipe.m_InsPreToolBlock[m_nSelectCam].Tools[m_nToolBlockSelected]).Tools.Count; i++)
            {
                IInsTool tool = ((InsToolBlock)theRecipe.m_InsPreToolBlock[m_nSelectCam].Tools[m_nToolBlockSelected]).Tools[i];
                string sTemp = tool.Name;

                if (sTemp.Contains("FindLine")) 
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
                InsCurrentImageDisplay.Record = CurrRecord;
                InsCurrentImageDisplay.FitImage();
            }
            catch (Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"FindLine Tool Save Error Catch : {e.Message}");
            }
        }
       
        private void ToolBlockRun(int _nCamNum, InsToolBlock _toolBlock, int _nMode)    //240720 NIS ToolBlock Run
        {
            _toolBlock.Inputs[0].Value = InspectionImage;//new InsImage8Grey(theMainSystem.Cameras[_nCamNum].ListImages[0][DEF_SYSTEM.BACK].pData, theMainSystem.Cameras[_nCamNum].ListImages[0][DEF_SYSTEM.BACK].m_nWidth, theMainSystem.Cameras[_nCamNum].ListImages[0][DEF_SYSTEM.BACK].m_nHeight, theMainSystem.Cameras[_nCamNum].ListImages[0][DEF_SYSTEM.BACK].m_nStride);
            _toolBlock.Run();
        }

        private void FindLineShowTool(InsFindLineTool _pFindLine)       //240720 NIS Display findline result
        {
            InsRecord CurrRecord = new InsRecord();
            CurrRecord.ContentType = typeof(IInsImage);
            CurrRecord.Content = _pFindLine.InputImage;
            CurrRecord.Annotation = "InputImage";
            CurrRecord.RecordKey = "InputImage";
            CurrRecord.RecordUsage = InsRecordUsageConstants.Diagnostic;

            CurrRecord.SubRecords.Add(_pFindLine.CreateCurrentRecord());
            InsCurrentImageDisplay.Record = CurrRecord;
            InsCurrentImageDisplay.FitImage();

            InsRecord LastRecord = new InsRecord();
            LastRecord.ContentType = typeof(IInsImage);
            LastRecord.Content = _pFindLine.InputImage;
            LastRecord.Annotation = "InputImage";
            LastRecord.RecordKey = "InputImage";
            LastRecord.RecordUsage = InsRecordUsageConstants.Diagnostic;

            LastRecord.SubRecords.Add(_pFindLine.CreateLastRunRecord());
            InsLastRunImageDisplay.Record = LastRecord;
            InsLastRunImageDisplay.FitImage();
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
        private void TeachInspectionRun()   //240709 NIS Cam 변경 시 마다 Inspection Display
        {
            CreateLabeling(InspectionImage);
            
            //Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Controller Reset");
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

        private void CreateLabeling(InsImage8Grey _Image)
        {
            InsRecord MainRecord = new InsRecord("Main", _Image.GetType(), InsRecordUsageConstants.Result, true, _Image, "Main");
            InsRecord SubRecord = new InsRecord("GraphicCollection", _Image.GetType(), InsRecordUsageConstants.Result, true, _Image, "GraphicCollection");
            bool IsError = false;

            for (int k = 0; k < theRecipe.m_InsPreToolBlock.Count; k++)
            {
                for (int i = 0; i < theRecipe.m_InsPreToolBlock[m_nSelectCam].Tools.Count; i++)
                {
                    for (int j = 0; j < ((InsToolBlock)theRecipe.m_InsPreToolBlock[m_nSelectCam].Tools[i]).Tools.Count; j++)
                    {
                        if (((InsToolBlock)theRecipe.m_InsPreToolBlock[m_nSelectCam].Tools[i]).Tools[j].Name.Contains("FindLineTool"))
                        {
                            InsFindLineTool tool = ((InsToolBlock)theRecipe.m_InsPreToolBlock[m_nSelectCam].Tools[i]).Tools[j] as InsFindLineTool;
                            InsLineSegment LineSeg = tool.Results.FoundLineSegment;
                            LineSeg.LineWidthInScreenPixels = 5;
                            SubRecord.AddGraphicToRecord(LineSeg, "GraphicCollection", "");
                        }
                    }
                }
            }
            
            MainRecord.SubRecords.Add(SubRecord);
            InsInspImageDisplay.Record = MainRecord;

            InsInspImageDisplay.FitImage();
        }
        

        private InsGraphicLabel CreatInsLabel(double _PosX, double _PosY, string _txt, Color _Color, Font _font)
        {
            InsGraphicLabel CreatLabel = new InsGraphicLabel();
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

        private void Reset_DisplayScreen()
        {
            InsInspImageDisplay.Record = null;
        }

        //241207 NWT Fitting Mode에 따라 Txtbox 비활성화
        private void cbo_FittingMode_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbo_FittingMode.SelectedIndex == 0 || cbo_FittingMode.SelectedIndex == 1)
            {
                txt_NumofIgnore.Enabled = true;
                txt_Sigma.Enabled = false;
                txt_IterrateTimes.Enabled = false;
                lbl_IterateTimes.ForeColor = Color.Gray;
                lbl_Sigma.ForeColor = Color.Gray;
                lbl_Ignore.ForeColor = Color.White;
            }
            else
            {
                txt_NumofIgnore.Enabled = false;
                txt_Sigma.Enabled = true;
                txt_IterrateTimes.Enabled = true;
                lbl_IterateTimes.ForeColor = Color.White;
                lbl_Sigma.ForeColor = Color.White;
                lbl_Ignore.ForeColor = Color.Gray;
            }

        }

        private void FormInsTeachInspectionCrack_VisibleChanged(object sender, EventArgs e)
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

        private void lib_ToolBlockList_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_nToolBlockSelected = (int)lib_ToolBlockList.SelectedIndex;

            ToolListShow();
        }

        private void lib_ToolList_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_nToolSelected = (int)lib_ToolList.SelectedIndex;

            IInsTool SelectTool = m_InsTeachingToolList[m_nToolSelected];

            if (SelectTool.Name.Contains("FindLine"))// 선택된 Tool 이름으로 구분하여 Parameter 표시 - SHJ
            {
                InsFindLineTool FindLine = SelectTool as InsFindLineTool;

                SetTextFindLineData(FindLine);
                FindLineShowTool(FindLine);
            }
        }
    }
}
