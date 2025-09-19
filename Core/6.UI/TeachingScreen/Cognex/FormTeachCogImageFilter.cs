using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Implementation;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.ToolBlock;
using Core.DataProcess;
using Core.DataProcess;
using Core.Function;
using Core.UI;
using Core.Utility;
using Core.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Core.Program;

namespace Core.ui
{
    public partial class FormTeachCogImageFilter : Form, ITeachForm
    {
        private MainForm MainForm;
        private FormTeachInspection TeachMainForm;  //parent form
        private int m_nSelectCam = 0;       //Selected cam number
        private int m_nToolBlockSelected;
        private int m_nToolSelected = 0;    //Selected tool list index
        private int m_nInspectSelected = 0; //Selected inspection mode

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

        private List<CogMaskCreatorRegion> m_ListMaskRegion = new List<CogMaskCreatorRegion>();
        private CogBlobTool m_BlobShowMaskTool = new CogBlobTool();

        public FormTeachCogImageFilter(MainForm _MainForm, FormTeachInspection _TeachMainForm, CLogger _logger)
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

            CreatLabeling(m_InspectionImage);
        }

        private void Run_ApplyAndSave()
        {
            theRecipe.ToolSave(theRecipe.m_sCurrentModelName);
            theRecipe.DataSave(theRecipe.m_sCurrentModelName);

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                theMainSystem.Cameras[i].ToolBlockRefresh();
        }

        private void SetData()
        {
            cbo_MaskBackground.Items.Clear();
            cbo_MaskBackground.Font = ComboBoxFont;
            cbo_MaskBackground.Items.Add("DontCare");
            cbo_MaskBackground.Items.Add("Care");

            cbo_MaskInside.Items.Clear();
            cbo_MaskInside.Font = ComboBoxFont;
            cbo_MaskInside.Items.Add("DontCare");
            cbo_MaskInside.Items.Add("Care");

            cbo_ImageIndex.Items.Clear();
            cbo_ImageIndex.Font = ComboBoxFont;
            for (int i = 0; i < theRecipe.MergeImageCount; i++)
                cbo_ImageIndex.Items.Add($"Image Index: {i}");

            cbo_ImageIndex.SelectedIndex = 0;
        }

        private void Disable_RadioButtons()
        {
            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                this.Controls.Find($"rdb_Cam{i + 1}", true)[0].Visible = true;
            }
        }


        private void ToolListShow()   //240720 NIS Set ToolList
        {
            if (!theRecipe.m_sCurrentEquipment.Contains("DISPLAY")) //250804 LYK 임시 처리
                return;

            m_TeachingToolList.Clear();
            lib_ToolList.Items.Clear();

            int ImageIndex = cbo_ImageIndex.SelectedIndex;

            // 250514 SHJ ToolBlock 을 현재 Inspect 에서 깊은 복사를 사용해서 실행 하기 때문에 Recipe ToolBlock 을 이용해서 마지막 이미지 기준으로 Run 실행 
            theMainSystem.Cameras[m_nSelectCam].RunTeachingInspect(ImageIndex);

            // 250514 SHJ ToolBlock 없으면 종료 
            if (theRecipe.m_CogInspToolBlock[m_nSelectCam] == null)
                return;
            else
                m_TeachingToolBlock = theMainSystem.Cameras[m_nSelectCam].GetInspToolBlock(ImageIndex);//theRecipe.m_CogPreToolBlock[m_nSelectCam];

            for (int i = 0; i < m_TeachingToolBlock.Tools.Count; i++)
            {
                ICogTool tool = m_TeachingToolBlock.Tools[i];
                string sTemp = tool.Name;

                if (sTemp.Contains("MaskCreator"))
                {
                    lib_ToolList.Font = font;
                    lib_ToolList.Items.Add(sTemp);
                    m_TeachingToolList.Add(tool);
                }
            }

            //lib_ToolList.SetSelected(0, true);
        }

        private void ToolList_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_nToolSelected = (int)lib_ToolList.SelectedIndex;

            ICogTool SelectTool = m_TeachingToolList[m_nToolSelected];

            // 250515 SHJ 선택 된 Tool Masking Tool 일 경우 List Refresh
            if (SelectTool.Name.Contains("MaskCreator"))// Tool 이름으로 구분하여 Parameter 표시
            {
                CogMaskCreatorTool MaskTool = SelectTool as CogMaskCreatorTool;

                SetMaskCreatorData(MaskTool);
                MaskCreatorShowTool(MaskTool);
            }

        }

        private void SetMaskCreatorData(CogMaskCreatorTool _MaskTool)
        {
            lib_MaskingList.Items.Clear();
            m_ListMaskRegion.Clear();

            // 250515 SHJ Mask Tool 에 있는 모든 정보를 화면에 출력  
            for (int i = 0; i < _MaskTool.RunParams.MaskAreas.Count; i++)
            {
                lib_MaskingList.Items.Add(_MaskTool.RunParams.MaskAreas[i].Name);
                CogMaskCreatorRegion _region = _MaskTool.RunParams.MaskAreas[i] as CogMaskCreatorRegion;
                m_ListMaskRegion.Add(_region);
            }

            if (_MaskTool.RunParams.BackgroundMaskValue == CogMaskValueConstants.DontCare)
                cbo_MaskBackground.SelectedIndex = 0;
            else
                cbo_MaskBackground.SelectedIndex = 1;

            if (_MaskTool.RunParams.MaskAreas.Count > 0)
                lib_MaskingList.SetSelected(0, true);
        }

        private void MaskCreatorShowTool(CogMaskCreatorTool _MaskTool)
        {
            m_InspectionImage = _MaskTool.InputImage as CogImage8Grey;

            _MaskTool.RunParams.MaskAreas.Clear();
            for (int i = 0; i < m_ListMaskRegion.Count; i++)
            {
                _MaskTool.RunParams.MaskAreas.Add(m_ListMaskRegion[i]);
            }

            CogRecord CurrRecord = new CogRecord();
            CurrRecord.ContentType = typeof(ICogImage);
            CurrRecord.Content = _MaskTool.InputImage;
            CurrRecord.Annotation = "InputImage";
            CurrRecord.RecordKey = "InputImage";
            CurrRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;

            CurrRecord.SubRecords.Add(_MaskTool.CreateCurrentRecord());
            CurrentImageDisplay.Record = CurrRecord;
            CurrentImageDisplay.Fit();

            CogRecord LastRecord = new CogRecord();
            LastRecord.ContentType = typeof(ICogImage);
            LastRecord.Content = _MaskTool.InputImage;
            LastRecord.Annotation = "InputImage";
            LastRecord.RecordKey = "InputImage";
            LastRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;

            m_BlobShowMaskTool.InputImage = _MaskTool.InputImage as CogImage8Grey;

            if(_MaskTool.Result != null)
                m_BlobShowMaskTool.RunParams.InputImageMask = _MaskTool.Result.Mask as CogImage8Grey;

            LastRecord.SubRecords.Add(m_BlobShowMaskTool.CreateCurrentRecord().SubRecords[0]);
            LastRunImageDisplay.Record = LastRecord;
            LastRunImageDisplay.Fit();

        }

        private void btn_MaskingAdd_Click(object sender, EventArgs e)
        {
            ICogTool SelectTool = m_TeachingToolList[m_nToolSelected];

            if (SelectTool.Name.Contains("MaskCreator"))// Tool 이름으로 구분하여 Parameter 표시
            {
                CogMaskCreatorTool MaskTool = SelectTool as CogMaskCreatorTool;
                CogImage8Grey InputImage = MaskTool.InputImage as CogImage8Grey;

                CogMaskCreatorRegion _MaskRegion = new CogMaskCreatorRegion();

                _MaskRegion.Name = "Rectangle";
                _MaskRegion.InsideMaskValue = CogMaskValueConstants.DontCare;

                CogRectangle Rect = new CogRectangle();

                Rect.SetCenterWidthHeight(InputImage.Width / 2, InputImage.Height / 2, InputImage.Width / 4, InputImage.Height / 4);

                Rect.Interactive = true;
                Rect.Visible = true;
                Rect.GraphicDOFEnable = CogRectangleDOFConstants.All;

                _MaskRegion.Region = Rect as CogRectangle;

                m_ListMaskRegion.Add(_MaskRegion);

                lib_MaskingList.Items.Add(_MaskRegion.Name);

                if (m_ListMaskRegion.Count - 1 > 0)
                    lib_MaskingList.SetSelected(m_ListMaskRegion.Count - 1, true);
                else
                    lib_MaskingList.SetSelected(0, true);

                MaskCreatorShowTool(MaskTool);
            }
        }

        private void lib_MaskingList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nSelectIndex = lib_MaskingList.SelectedIndex;

            CogMaskCreatorRegion _MaskRegion = m_ListMaskRegion[nSelectIndex] as CogMaskCreatorRegion;

            txt_MaskName.Text = _MaskRegion.Name;

            CogRectangle Rect = _MaskRegion.Region as CogRectangle;

            Rect.Selected = true;

            if (_MaskRegion.InsideMaskValue == CogMaskValueConstants.DontCare)
                cbo_MaskInside.SelectedIndex = 0;
            else
                cbo_MaskInside.SelectedIndex = 1;

        }

        private void btn_MaskListSetData_Click(object sender, EventArgs e)
        {
            int nSelectIndex = lib_MaskingList.SelectedIndex;
            string sMaskingName = txt_MaskName.Text;

            m_ListMaskRegion[nSelectIndex].Name = sMaskingName;

            ICogTool SelectTool = m_TeachingToolList[m_nToolSelected];

            if (SelectTool.Name.Contains("MaskCreator"))// Tool 이름으로 구분하여 Parameter 표시
            {
                CogMaskCreatorTool MaskTool = SelectTool as CogMaskCreatorTool;

                SetMaskCreatorData(MaskTool);
            }
        }

        private void cbo_MaskInside_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sInsideMaskValue = (string)cbo_MaskInside.SelectedItem;

            int nSelectIndex = lib_MaskingList.SelectedIndex;
            CogMaskCreatorRegion _MaskRegion = m_ListMaskRegion[nSelectIndex] as CogMaskCreatorRegion;
            if (sInsideMaskValue == "DontCare")
                _MaskRegion.InsideMaskValue = CogMaskValueConstants.DontCare;
            else
                _MaskRegion.InsideMaskValue = CogMaskValueConstants.Care;

            //m_ListMaskRegion.RemoveAt(nSelectIndex);

            //m_ListMaskRegion.Add(_MaskRegion);
        }

        private void MaskParameterApply(CogMaskCreatorTool _MaskTool)
        {
            string sInsideMaskValue = (string)cbo_MaskBackground.SelectedItem;

            _MaskTool.RunParams.MaskAreas.Clear();

            // 250515 SHJ Mask List 에 있는 Region 항목들 MaskTool 에 업데이트
            for (int i = 0; i < m_ListMaskRegion.Count; i++)
            {
                _MaskTool.RunParams.MaskAreas.Add(m_ListMaskRegion[i]);
            }

            if (sInsideMaskValue == "DontCare")
                _MaskTool.RunParams.BackgroundMaskValue = CogMaskValueConstants.DontCare;
            else
                _MaskTool.RunParams.BackgroundMaskValue = CogMaskValueConstants.Care;
        }

        private void CreatLabeling(CogImage8Grey _Image)
        {
            if (_Image != null)
            {
                try
                {
                    //250909 SHJ 최종 검사 화면 표시 
                    CogRecord MainRecord = new CogRecord("Main", _Image.GetType(), CogRecordUsageConstants.Result, false, _Image, "Main");
                    CogGraphicCollection GraphicCollection = new CogGraphicCollection();

                    CogImage8Grey ProcImage = new CogImage8Grey();
                    CogImage8Grey MaskImage = new CogImage8Grey();

                    for (int i = 0; i < m_TeachingToolBlock.Outputs.Count; i++)
                    {
                        if (m_TeachingToolBlock.Outputs[i].ValueType == typeof(ICogImage))
                            ProcImage = m_TeachingToolBlock.Outputs[i].Value as CogImage8Grey;
                        else if (m_TeachingToolBlock.Outputs[i].ValueType == typeof(CogImage8Grey))
                            MaskImage = m_TeachingToolBlock.Outputs[i].Value as CogImage8Grey;

                    }

                    MainRecord.ContentType = typeof(ICogImage);
                    MainRecord.Content = ProcImage;
                    MainRecord.Annotation = "InputImage";
                    MainRecord.RecordKey = "InputImage";
                    MainRecord.RecordUsage = CogRecordUsageConstants.Diagnostic;

                    m_BlobShowMaskTool.InputImage = ProcImage as CogImage8Grey;
                    m_BlobShowMaskTool.RunParams.InputImageMask = MaskImage as CogImage8Grey;

                    MainRecord.SubRecords.Add(m_BlobShowMaskTool.CreateCurrentRecord().SubRecords[0]);

                    MainRecord.SubRecords.Add(new CogRecord("Graphic", typeof(CogGraphicCollection), CogRecordUsageConstants.Result, false, GraphicCollection, "Graphic"));

                    ImageDisplay.Record = MainRecord;
                    ImageDisplay.Fit();
                }
                catch (Exception ex)
                {
                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Teaching Image Fiter Tool Error {ex.ToString()}");
                }
            }
        }

        private void btn_Run_Click(object sender, EventArgs e)
        {
            int nSelect = lib_ToolList.SelectedIndex;
            CogMaskCreatorTool MaskTool = m_TeachingToolList[nSelect] as CogMaskCreatorTool;

            m_InspectionImage = MaskTool.InputImage as CogImage8Grey;

            int ImageIndex = cbo_ImageIndex.SelectedIndex;

            if (m_InspectionImage != null)
            {
                MaskParameterApply(MaskTool);

                System.Threading.Thread.Sleep(100);

                // ToolBlock 실행 
                theMainSystem.Cameras[m_nSelectCam].RunTeachingInspect(ImageIndex);

                //for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                //{

                //    ToolBlockRun(i, theRecipe.m_CogPreToolBlock[i] as CogToolBlock, m_nInspectSelected);

                //}

                MaskCreatorShowTool(MaskTool);

                CreatLabeling(m_InspectionImage);
            }
        }

        private void btn_MaskingDelete_Click(object sender, EventArgs e)
        {
            int nSelectIndex = lib_MaskingList.SelectedIndex;
            //m_TeachingMaskCreator.RunParams.MaskAreas.RemoveAt(nSelectIndex);
            m_ListMaskRegion.RemoveAt(nSelectIndex);

            ICogTool SelectTool = m_TeachingToolList[m_nToolSelected];

            if (SelectTool.Name.Contains("MaskCreator"))// Tool 이름으로 구분하여 Parameter 표시
            {
                CogMaskCreatorTool MaskTool = SelectTool as CogMaskCreatorTool;

                SetMaskCreatorData(MaskTool);
                MaskCreatorShowTool(MaskTool);
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

        private void btn_LogicToolBlock_Click(object sender, EventArgs e)
        {
            ToolBlockEdit ToolEdit = new ToolBlockEdit(m_nInspectSelected, m_TeachingToolBlock); //240703 NIS 실행한 TeachMode에 따라서 ToolBlockEdit 타이틀 수정
            ToolEdit.Location = TeachFormLocation;
            ToolEdit.ShowDialog();
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

        private void btn_ApplyAndSave_Click(object sender, EventArgs e)
        {
            ICogTool tool = m_TeachingToolList[m_nToolSelected];

            if (tool.Name.Contains("Mask")) // Tool 이름으로 구분하여 Parameter Apply
            {
                CogMaskCreatorTool MaskTool = tool as CogMaskCreatorTool;
                SetMaskCreatorData(MaskTool);
            }

            Thread_ApplyAndSave.Continue();
        }

        private void cbo_ImageIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolListShow();
        }
    }
}
