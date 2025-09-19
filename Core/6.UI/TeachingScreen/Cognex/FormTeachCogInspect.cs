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
using Insnex.Vision2D;
using Cognex.VisionPro.ToolBlock;

namespace Core.UI
{
    public partial class FormTeachCogInspect : Form, ITeachForm
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

        private CogToolBlock m_TeachingToolBlock = null;
        private List<ICogTool> m_TeachingToolList = new List<ICogTool>();   //Teaching CogTool List
        private CogImage8Grey m_InspectionImage = null;

        private CLogger Logger = null;

        private CogImage8Grey InspectionImage = null;
        public FormTeachCogInspect(MainForm _MainForm, FormTeachInspection _TeachMainForm, CLogger _logger)
        {
            InitializeComponent();
            TopLevel = false;
            TeachMainForm = _TeachMainForm;
            MainForm = _MainForm;
            MainForm.SetEachControlResize(this);    

            Set_RadioButtonsList();
            Hide_RadioButton();
            m_nInspectSelected = (int)INSP_MODE.Focus;
            SetData();
            ToolListShow();

            TeachFormLocation = TeachMainForm.Get_TeachFormScreenLocation();  //240721 NIS Get TeachFormScreen Location
            Logger = _logger;
        }

        public void ShowTeachingPage()
        {
            ToolListShow();
            int nSelect = lib_ToolList.SelectedIndex;
            CogBlobTool BlobTool = m_TeachingToolList[nSelect] as CogBlobTool;

            //CreatLabeling(m_InspectionImage, BlobTool);
        }

        private void btn_LogicToolBlock_Click(object sender, EventArgs e)   //240720 NIS Display ToolBlockEditPage
        {
            
            //m_TeachingToolBlock = ((CogToolBlock)theRecipe.m_CogInspToolBlock[m_nSelectCam]);

            ToolBlockEdit ToolEdit = new ToolBlockEdit(m_nInspectSelected, m_TeachingToolBlock, null); //240703 NIS 실행한 TeachMode에 따라서 ToolBlockEdit 타이틀 수정
            ToolEdit.Location = TeachFormLocation;
            ToolEdit.ShowDialog();
            
            
        }
        private void btn_Run_Click(object sender, EventArgs e)
        {
            int nSelect = lib_ToolList.SelectedIndex;
            CogBlobTool BlobTool = m_TeachingToolList[nSelect] as CogBlobTool;

            m_InspectionImage = BlobTool.InputImage as CogImage8Grey;

            int ImageIndex = cbo_ImageIndex.SelectedIndex;

            int CellIndex = 0;
            bool ExcuteMode = false;

            if (cbo_CellIndex.Items.Count > 0)
            {
                CellIndex = cbo_CellIndex.SelectedIndex;
            }

            if (m_InspectionImage != null)
            {
                BlobParameterApply(BlobTool);

                System.Threading.Thread.Sleep(100);

                // ToolBlock 실행 
                theMainSystem.Cameras[m_nSelectCam].RunTeachingInspect(ImageIndex, false, true, CellIndex);

                BlobShowTool(BlobTool);

                CreatLabeling(m_InspectionImage, BlobTool);
            }
        }

        private void ToolList_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_nToolSelected = (int)lib_ToolList.SelectedIndex;

            ICogTool SelectTool = m_TeachingToolList[m_nToolSelected];

            if (SelectTool.Name.Contains("Blob"))// Tool 이름으로 구분하여 Parameter 표시
            {
                CogBlobTool BlobTool = SelectTool as CogBlobTool;

                SetTextBlobData(BlobTool);
                BlobShowTool(BlobTool);
            }
        }

        private void BlobParameterApply(CogBlobTool _BlobTool)
        {
            try
            {
                if (int.Parse(txt_BlobSize.Text) < 1)
                {
                    MessageBox.Show("Min Pixel Size 1");

                    return;
                }
                else
                {
                    _BlobTool.RunParams.ConnectivityMinPixels = int.Parse(txt_BlobSize.Text);    // 최소 픽셀 사이즈 
                }

                _BlobTool.RunParams.SegmentationParams.HardFixedThreshold = int.Parse(txt_BlobScale.Text);

                /*
                CogBlobMorphologyConstants.CloseHorizontal;
                CogBlobMorphologyConstants.CloseSquare;
                CogBlobMorphologyConstants.CloseVertical;
                CogBlobMorphologyConstants.DilateHorizontal;
                CogBlobMorphologyConstants.DilateSquare;
                CogBlobMorphologyConstants.DilateVertical;
                CogBlobMorphologyConstants.ErodeHorizontal;
                CogBlobMorphologyConstants.ErodeSquare;
                CogBlobMorphologyConstants.ErodeVertical;
                CogBlobMorphologyConstants.OpenHorizontal;
                CogBlobMorphologyConstants.OpenSquare;
                CogBlobMorphologyConstants.OpenVertical
                    */

                _BlobTool.RunParams.MorphologyOperations.Clear();

                for (int i = 0; i < lib_BlobRunFilterList.Items.Count; i++)
                {
                    string sitem = (string)lib_BlobRunFilterList.Items[i];

                    if (sitem == "CloseHorizontal")
                        _BlobTool.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.CloseHorizontal);
                    else if (sitem == "CloseSquare")
                        _BlobTool.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.CloseSquare);
                    else if (sitem == "CloseVertical")
                        _BlobTool.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.CloseVertical);
                    else if (sitem == "DilateHorizontal")
                        _BlobTool.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.DilateHorizontal);
                    else if (sitem == "DilateSquare")
                        _BlobTool.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.DilateSquare);
                    else if (sitem == "DilateVertical")
                        _BlobTool.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.DilateVertical);
                    else if (sitem == "ErodeHorizontal")
                        _BlobTool.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.ErodeHorizontal);
                    else if (sitem == "ErodeSquare")
                        _BlobTool.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.ErodeSquare);
                    else if (sitem == "ErodeVertical")
                        _BlobTool.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.ErodeVertical);
                    else if (sitem == "OpenHorizontal")
                        _BlobTool.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.OpenHorizontal);
                    else if (sitem == "OpenSquare")
                        _BlobTool.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.OpenSquare);
                    else if (sitem == "OpenVertical")
                        _BlobTool.RunParams.MorphologyOperations.Add(CogBlobMorphologyConstants.OpenVertical);

                }

                if (cbo_BlobPolarity.SelectedIndex == 0)
                    _BlobTool.RunParams.SegmentationParams.Polarity = CogBlobSegmentationPolarityConstants.LightBlobs;
                else
                    _BlobTool.RunParams.SegmentationParams.Polarity = CogBlobSegmentationPolarityConstants.DarkBlobs;

            }
            catch (Exception e)
            {
            }

        }

        private void BlobShowTool(CogBlobTool _BlobTool)
        {
            m_InspectionImage = _BlobTool.InputImage as CogImage8Grey;

            CogRecord CurrRecord = new CogRecord();
            CurrRecord.ContentType = typeof(ICogImage);
            CurrRecord.Content = _BlobTool.InputImage;
            CurrRecord.Annotation = "InputImage";
            CurrRecord.RecordKey = "InputImage";
            CurrRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;

            CurrRecord.SubRecords.Add(_BlobTool.CreateCurrentRecord().SubRecords[0]);
            CurrentImageDisplay.Record = CurrRecord;
            CurrentImageDisplay.Fit();

            CogRecord LastRecord = new CogRecord();
            LastRecord.ContentType = typeof(ICogImage);
            LastRecord.Content = _BlobTool.InputImage;
            LastRecord.Annotation = "InputImage";
            LastRecord.RecordKey = "InputImage";
            LastRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;

            LastRecord.SubRecords.Add(_BlobTool.CreateLastRunRecord());
            LastRunImageDisplay.Record = LastRecord;
            LastRunImageDisplay.Fit();
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
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Inspect Teaching Cam1 Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_ONE;
                    }
                    else if (RadioButtons[DEF_SYSTEM.CAM_TWO].Name == rdb.Name)
                    {
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Inspect Teaching Cam2 Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_TWO;
                    }
                    else if (RadioButtons[DEF_SYSTEM.CAM_THREE].Name == rdb.Name)
                    {
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Inspect Teaching Cam3 Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_THREE;
                    }
                    else if (RadioButtons[DEF_SYSTEM.CAM_FOUR].Name == rdb.Name)
                    {
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Inspect Teaching Cam4 Radio Button Click.");
                        m_nSelectCam = DEF_SYSTEM.CAM_FOUR;
                    }

                    //ToolBlockListShow();
                    ToolListShow();

                    int nSelect = lib_ToolList.SelectedIndex;
                    CogBlobTool BlobTool = m_TeachingToolList[nSelect] as CogBlobTool;

                    //240709 NIS Cam 변경 시 마다 Inspection Display
                    CreatLabeling(m_InspectionImage, BlobTool);
                }
            }
            catch (Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Form Teach Crack Exception Catch : {ex.Message}");
            }
            
        }

        private void SetTextBlobData(CogBlobTool _BlobTool)
        {
            txt_BlobSize.Text = _BlobTool.RunParams.ConnectivityMinPixels.ToString();

            txt_BlobScale.Text = _BlobTool.RunParams.SegmentationParams.HardFixedThreshold.ToString();

            /*
            CogBlobMorphologyConstants.CloseHorizontal;
            CogBlobMorphologyConstants.CloseSquare;
            CogBlobMorphologyConstants.CloseVertical;
            CogBlobMorphologyConstants.DilateHorizontal;
            CogBlobMorphologyConstants.DilateSquare;
            CogBlobMorphologyConstants.DilateVertical;
            CogBlobMorphologyConstants.ErodeHorizontal;
            CogBlobMorphologyConstants.ErodeSquare;
            CogBlobMorphologyConstants.ErodeVertical;
            CogBlobMorphologyConstants.OpenHorizontal;
            CogBlobMorphologyConstants.OpenSquare;
            CogBlobMorphologyConstants.OpenVertical
            */

            lib_BlobFilterList.Items.Clear();
            lib_BlobFilterList.Font = ComboBoxFont;

            lib_BlobFilterList.Items.Add("CloseSquare");
            lib_BlobFilterList.Items.Add("CloseHorizontal");
            lib_BlobFilterList.Items.Add("CloseVertical");
            lib_BlobFilterList.Items.Add("DilateSquare");
            lib_BlobFilterList.Items.Add("DilateHorizontal");
            lib_BlobFilterList.Items.Add("DilateVertical");
            lib_BlobFilterList.Items.Add("ErodeSquare");
            lib_BlobFilterList.Items.Add("ErodeHorizontal");
            lib_BlobFilterList.Items.Add("ErodeVertical");
            lib_BlobFilterList.Items.Add("OpenSquare");
            lib_BlobFilterList.Items.Add("OpenHorizontal");
            lib_BlobFilterList.Items.Add("OpenVertical");

            lib_BlobRunFilterList.Items.Clear();
            lib_BlobRunFilterList.Font = ComboBoxFont;

            for (int i = 0; i < _BlobTool.RunParams.MorphologyOperations.Count; i++)
            {
                if (CogBlobMorphologyConstants.CloseSquare == _BlobTool.RunParams.MorphologyOperations[i])
                    lib_BlobRunFilterList.Items.Add("CloseSquare");
                else if (CogBlobMorphologyConstants.CloseHorizontal == _BlobTool.RunParams.MorphologyOperations[i])
                    lib_BlobRunFilterList.Items.Add("CloseHorizontal");
                else if (CogBlobMorphologyConstants.CloseVertical == _BlobTool.RunParams.MorphologyOperations[i])
                    lib_BlobRunFilterList.Items.Add("CloseVertical");
                else if (CogBlobMorphologyConstants.DilateSquare == _BlobTool.RunParams.MorphologyOperations[i])
                    lib_BlobRunFilterList.Items.Add("DilateSquare");
                else if (CogBlobMorphologyConstants.DilateHorizontal == _BlobTool.RunParams.MorphologyOperations[i])
                    lib_BlobRunFilterList.Items.Add("DilateHorizontal");
                else if (CogBlobMorphologyConstants.DilateVertical == _BlobTool.RunParams.MorphologyOperations[i])
                    lib_BlobRunFilterList.Items.Add("DilateVertical");
                else if (CogBlobMorphologyConstants.ErodeSquare == _BlobTool.RunParams.MorphologyOperations[i])
                    lib_BlobRunFilterList.Items.Add("ErodeSquare");
                else if (CogBlobMorphologyConstants.ErodeHorizontal == _BlobTool.RunParams.MorphologyOperations[i])
                    lib_BlobRunFilterList.Items.Add("ErodeHorizontal");
                else if (CogBlobMorphologyConstants.ErodeVertical == _BlobTool.RunParams.MorphologyOperations[i])
                    lib_BlobRunFilterList.Items.Add("ErodeVertical");
                else if (CogBlobMorphologyConstants.OpenSquare == _BlobTool.RunParams.MorphologyOperations[i])
                    lib_BlobRunFilterList.Items.Add("OpenSquare");
                else if (CogBlobMorphologyConstants.OpenHorizontal == _BlobTool.RunParams.MorphologyOperations[i])
                    lib_BlobRunFilterList.Items.Add("OpenHorizontal");
                else if (CogBlobMorphologyConstants.OpenVertical == _BlobTool.RunParams.MorphologyOperations[i])
                    lib_BlobRunFilterList.Items.Add("OpenVertical");

            }

            if (_BlobTool.RunParams.SegmentationParams.Polarity == CogBlobSegmentationPolarityConstants.DarkBlobs)
                cbo_BlobPolarity.SelectedIndex = 1;
            else
                cbo_BlobPolarity.SelectedIndex = 0;
        }


        private void ApplyAndSave_Click(object sender, EventArgs e)
        {
            Run_ApplyAndSave(); //241210 Apply Save Thread 삭제
        }

        private void Run_ApplyAndSave()
        {

            ICogTool tool = m_TeachingToolList[m_nToolSelected];

            if (tool.Name.Contains("Blob")) // Tool 이름으로 구분하여 Parameter Apply
            {
                CogBlobTool BlobTool = tool as CogBlobTool;
                BlobParameterApply(BlobTool);
            }
            theRecipe.ToolSave(theRecipe.m_sCurrentModelName);
            theRecipe.DataSave(theRecipe.m_sCurrentModelName);

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                theMainSystem.Cameras[i].ToolBlockRefresh();
        }
    
        private void ToolListShow()   //240720 NIS Set ToolList
        {
            if (!theRecipe.m_sCurrentEquipment.Contains("DISPLAY")) //250804 LYK 임시 처리
                return;

            m_TeachingToolList.Clear();
            lib_ToolList.Items.Clear();

            int ImageIndex = cbo_ImageIndex.SelectedIndex;

            // 250514 SHJ ToolBlock 을 현재 Inspect 에서 깊은 복사를 사용해서 실행 하기 때문에 Recipe ToolBlock 을 이용해서 마지막 이미지 기준으로 Run 실행 
            //theMainSystem.Cameras[m_nSelectCam].RunTeachingInspect(ImageIndex);

            if ((CogToolBlock)theRecipe.m_CogInspToolBlock[m_nSelectCam] == null)
                return;

            m_TeachingToolBlock = theMainSystem.Cameras[m_nSelectCam].GetInspToolBlock(ImageIndex);//theRecipe.m_CogInspToolBlock[m_nSelectCam];

            for (int i = 0; i < m_TeachingToolBlock.Tools.Count; i++)
            {
                ICogTool tool = m_TeachingToolBlock.Tools[i];
                string sTemp = tool.Name;

                if (sTemp.Contains("Blob"))
                {
                    lib_ToolList.Font = font;
                    lib_ToolList.Items.Add(sTemp);
                    m_TeachingToolList.Add(tool);
                }
            }

            lib_ToolList.SetSelected(0, true);

        }

        private void SetData()          //240720 NIS Set SearchDirection combobox
        {
            cbo_BlobPolarity.Items.Clear();
            cbo_BlobPolarity.Font = ComboBoxFont;
            cbo_BlobPolarity.Items.Add("LightBlobs");
            cbo_BlobPolarity.Items.Add("DarkBlobs");

            cbo_ImageIndex.Items.Clear();
            cbo_ImageIndex.Font = ComboBoxFont;
            for (int i = 0; i < theRecipe.MergeImageCount; i++)
                cbo_ImageIndex.Items.Add($"Image Index: {i}");

            cbo_ImageIndex.SelectedIndex = 0;

            // 20250911 SHJ Display 에 필요한 Teaching 정보임으로 디스플레일 경우만 활성화 
            if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY)
            {
                lbl_CellIndex.Visible = true;
                cbo_CellIndex.Visible = true;

                cbo_CellIndex.Font = ComboBoxFont;
                int CellVerCnt = theRecipe.MaterialInfo.m_nCellColumn;
                for (int i = 1; i < CellVerCnt + 1; i++)
                    cbo_CellIndex.Items.Add($"Cell Index : {i}");

                cbo_CellIndex.SelectedIndex = 0;
            }
            else
            {
                lbl_CellIndex.Visible = false;
                cbo_CellIndex.Visible = false;
            }
        }
        private void FindLineResult(int SelectCamNum)       //240702 NIS TeachPage EdgeToBusbar Trigger Run시 우측에 결과 디스플레이
        {

            CogRecord MainRecord = new CogRecord("Main", InspectionImage.GetType(), CogRecordUsageConstants.Result, false, InspectionImage as CogImage8Grey, "Main");
            
            foreach (ICogTool Cogtool in ((CogToolBlock)theRecipe.m_CogInspToolBlock[SelectCamNum]).Tools) //241210 KCH 결과에 IntersectLine Paint
            {
                if (Cogtool.Name.Contains("Find"))
                {
                    CogFindLineTool FindLine = Cogtool as CogFindLineTool;
                    MainRecord.SubRecords.Add(FindLine.CreateLastRunRecord());
                }
            }
           
            ImageDisplay.Record = MainRecord;
            ImageDisplay.Fit();
            
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

        private void CreatLabeling(CogImage8Grey _Image, CogBlobTool _BlobTool)
        {

            if (_Image != null)
            {
                try
                {
                    // 전체 ToolBlcok Show
                    int ImageIndex = cbo_ImageIndex.SelectedIndex;
                    // ToolBlock 실행 
                    theMainSystem.Cameras[m_nSelectCam].RunTeachingInspect(ImageIndex);

                    //// Pointer Image -> CogImage 변환 
                    //using (CogImage8Root root8 = new CogImage8Root())
                    //{
                    //    CImage img = theMainSystem.Cameras[m_nSelectCam].ListImages[InpIdx][0];
                    //    root8.Initialize(img.m_nSliceWidth, img.m_nSliceHeight , img.pSliceData[ImageIndex], img.m_nSliceStride, null);
                    //    InspImage.SetRoot(root8);
                    //}

                    CogRecord MainRecord = new CogRecord("Main", _Image.GetType(), CogRecordUsageConstants.Result, false, _Image, "Main");
                    CogGraphicCollection GraphicCollection = new CogGraphicCollection();

                    CogToolBlock InspToolBlock = theMainSystem.Cameras[m_nSelectCam].GetInspToolBlock(ImageIndex);

                    // 20250918 SHJ TeachDefectManager 는 List 순서 카메라 인덱스 -> 이미지 인덱스 순서 (Insp Index 사용 X)
                    int Count = theMainSystem.ProductInfo.TeachDefectManager[0][ImageIndex].Count;

                    // 250909 SHJ 검사 완료된 위치 이미지 항목에 표시 
                    for (int i = 0; i < Count; i++)
                    {
                        double CenX = (float)theMainSystem.ProductInfo.TeachDefectManager[0][ImageIndex][i].InspResult.DefectPos.X; // Math.Round(OutputResult[i].CenterOfMassX, 3);
                        double CenY = (float)theMainSystem.ProductInfo.TeachDefectManager[0][ImageIndex][i].InspResult.DefectPos.Y; //Math.Round(OutputResult[i].CenterOfMassY, 3);
                        double Width = theMainSystem.ProductInfo.TeachDefectManager[0][ImageIndex][i].InspResult.Width;//Math.Round(OutputResult[i].GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeWidth), 3);
                        double Height = theMainSystem.ProductInfo.TeachDefectManager[0][ImageIndex][i].InspResult.Height;//Math.Round(OutputResult[i].GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeHeight), 3);

                        int InnderWidth = (int)Width + 30;
                        int InnderHeight = (int)Height + 30;

                        CogRectangle rect = new CogRectangle();
                        rect.Color = CogColorConstants.Red;
                        rect.SetCenterWidthHeight(CenX, CenY, InnderWidth, InnderHeight);
                        rect.LineWidthInScreenPixels = 2;
                        GraphicCollection.Add(rect);
                    }

                    List<CogPolygon> SearchAreaList = new List<CogPolygon>();

                    // 250909 SHJ 검사 영역 Record 추가 
                    for (int i = 0; i < InspToolBlock.Outputs.Count; i++)
                    {
                        if (InspToolBlock.Outputs[i].ValueType == typeof(List<CogPolygon>))
                            SearchAreaList = InspToolBlock.Outputs[i].Value as List<CogPolygon>;
                    }

                    for (int i = 0; i < SearchAreaList.Count; i++)
                    {
                        GraphicCollection.Add(SearchAreaList[i]);
                    }

                    MainRecord.SubRecords.Add(new CogRecord("Graphic", typeof(CogGraphicCollection), CogRecordUsageConstants.Result, false, GraphicCollection, "Graphic"));

                    ImageDisplay.Record = MainRecord;
                    ImageDisplay.Fit();


                    // Current Display 에 검사 영역 추가 
                    int CellIndex = cbo_CellIndex.SelectedIndex;

                    CogRecord CurrentRecord = new CogRecord("Main", _Image.GetType(), CogRecordUsageConstants.Result, false, _Image, "Main");
                    CogGraphicCollection AreGraphic = new CogGraphicCollection();
                    AreGraphic.Add(SearchAreaList[CellIndex]);
                    CurrentRecord.SubRecords.Add(new CogRecord("Graphic", typeof(CogGraphicCollection), CogRecordUsageConstants.Result, false, AreGraphic, "Graphic"));
                    CurrentImageDisplay.Record = CurrentRecord;
                    CurrentImageDisplay.Fit();

                }
                catch (Exception ex)
                {
                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Teaching Inspect Tool Error {ex.ToString()}");
                }
            }

        }

        private void cbo_ImageIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolListShow();
        }
    }
}