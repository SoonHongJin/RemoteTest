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
using Core.ui;

namespace Core.UI
{
    public enum FINDLINE_LOCATION_TYPE { Hor = 0, Ver, PrintHor, PrintVer }
    public enum INSP_MODE { Crop = 0, DeepLearning, Contour, Crack, Focus, EdgeToBusbar, ColorROI = 6 }
    public partial class FormTeachInspection : Form
    {
        private int m_nSelectCam = 0;
        private Font font = new Font("Calibri", 18, FontStyle.Bold);
        private Font ComboBoxFont = new Font("Calibri", 9, FontStyle.Bold);

        private MainForm MainForm = null;
        private bool Initialized = false;
        private int m_nInspectSelected = 0;
        private int m_nToolSelected = 0;

        private FormTeachCogROITool CogROIToolTeachingForm;
        private FormTeachCogImageFilter CogImageFilterTeachingForm;     //250515 SHJ 이미지 필터 페이지 추가 
        private FormTeachCogPM CogPMTeachingForm;
        private FormTeachInspectionDeepLearning DeepLearningTeachingForm;
        private FormTeachCogInspect CogBlobTeachForm;

        private FormTeachInsFindLine InsFindLineTeachingForm;           //241206 NWT INSNEX CRACK Teaching Form
        private FormTeachInsPM InsPMTeachingForm; //241209 KCH Insnex Contour Form
        private FormTeachInsBlob InsBlobTeachForm; //241227 KCH Insnex FingerWidth Form

        private List<Form> TeachingFormList = new List<Form>();             //240721 NIS TeachForm List
        private List<Button> TeachButtonList = new List<Button>();          //240721 NIS TeachFormButton List

        private List<ICogTool> m_TeachingToolList = new List<ICogTool>();           
        private List<ICogImage> m_TeachingImageList = new List<ICogImage>();

        private const int BTN_LINE_THICKNESS = 5;      //240805 NIS Set the button in bold

        private CLogger Logger = null;

        public FormTeachInspection(MainForm _MainForm, CLogger _logger)
        {
            InitializeComponent();
            Logger = _logger;
            TopLevel = false;
            MainForm = _MainForm;
            MainForm.SetEachControlResize(this);    //240801 NIS Control Resize

            CreateForm();       //240720 NIS Create TeachingForms
            
        }

        private void CreateForm()       //240720 NIS Create teaching forms
        {

            if (DEF_SYSTEM.LICENSES_KEY == (int)License.COG)
            {
                CogROIToolTeachingForm = new FormTeachCogROITool(MainForm, this, Logger)
                {
                    Parent = pnl_TeachFormScreen,
                    Size = pnl_TeachFormScreen.Size,
                    Visible = false
                };
                TeachingFormList.Add(CogROIToolTeachingForm);
                TeachButtonList.Add(btnDisplayROIToolForm);

                CogImageFilterTeachingForm = new FormTeachCogImageFilter(MainForm, this, Logger)    //240720 NIS Create PrintContour TeachingForm
                {
                    Parent = pnl_TeachFormScreen,
                    Size = pnl_TeachFormScreen.Size,
                    Visible = false
                };
                TeachingFormList.Add(CogImageFilterTeachingForm);
                TeachButtonList.Add(btnDisplayImageFilterForm);

                //blob 채워 넣어야 함
                CogBlobTeachForm = new FormTeachCogInspect(MainForm, this, Logger)    //240720 NIS Create PrintContour TeachingForm
                {
                    Parent = pnl_TeachFormScreen,
                    Size = pnl_TeachFormScreen.Size,
                    Visible = false
                };
                TeachingFormList.Add(CogBlobTeachForm);
                TeachButtonList.Add(btnDisplayBlobForm);

            }
            else if (DEF_SYSTEM.LICENSES_KEY == (int)License.INS)       //241206 NWT Create INS Crop TeachingForm
            {
                InsFindLineTeachingForm = new FormTeachInsFindLine(MainForm, this, Logger)
                {
                    Parent = pnl_TeachFormScreen,
                    Size = pnl_TeachFormScreen.Size,
                    Visible = false
                };
                TeachingFormList.Add(InsFindLineTeachingForm);
                TeachButtonList.Add(btnDisplayROIToolForm);

                InsPMTeachingForm = new FormTeachInsPM(MainForm, this, Logger)    //240720 NIS Create PrintContour TeachingForm
                {
                    Parent = pnl_TeachFormScreen,
                    Size = pnl_TeachFormScreen.Size,
                    Visible = false
                };
                TeachingFormList.Add(InsPMTeachingForm);
                TeachButtonList.Add(btnDisplayImageFilterForm);

                InsBlobTeachForm = new FormTeachInsBlob(MainForm, this, Logger)    //240720 NIS Create PrintContour TeachingForm
                {
                    Parent = pnl_TeachFormScreen,
                    Size = pnl_TeachFormScreen.Size,
                    Visible = false
                };
                TeachingFormList.Add(InsBlobTeachForm);
                TeachButtonList.Add(btnDisplayBlobForm);

            }

            DeepLearningTeachingForm = new FormTeachInspectionDeepLearning(MainForm, this, Logger)   //240720 NIS Create DeepLearning TeachingForm
            {
                Parent = pnl_TeachFormScreen,
                Size = pnl_TeachFormScreen.Size,
                Visible = false
            };
            TeachingFormList.Add(DeepLearningTeachingForm);
            TeachButtonList.Add(btnDisplayDeepLearningForm);
        }

        private void btn_DisplayEachTeachingForm(object sender, EventArgs e)    //240720 NIS Displays only SelectedForm, Hide the others
        {
            Button btn = sender as Button;

            // 20250911 Display 같은 경우 FindLine Tool 위치를 초기화 해야해서 작업 추가 
            if (DEF_SYSTEM.LICENSES_KEY == (int)License.COG && CogROIToolTeachingForm.Visible && theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY)
            {
                CogROIToolTeachingForm.CloseTeachingPage();
            }

            for (int i = 0; i < TeachingFormList.Count; i++)
            {
                if (TeachingFormList[i].Visible && !TeachingFormList[i].Name.Contains("Deep"))
                {
                    //240723 NIS Check visibled form
                    m_nSelectCam = ((ITeachForm)TeachingFormList[i]).Get_m_nSelectCam() == DEF_SYSTEM.CAM_FIVE ? DEF_SYSTEM.CAM_ONE : ((ITeachForm)TeachingFormList[i]).Get_m_nSelectCam();                //240723 NIS Check selected camera number
                }
                //m_nSelectCam = 0;
                TeachingFormList[i].Visible = false;
                TeachButtonList[i].FlatAppearance.BorderSize = 1;
            }

            if (btn.Name.Contains("Deep"))
            {
                DeepLearningTeachingForm.Visible = true;
                btnDisplayDeepLearningForm.FlatAppearance.BorderSize = BTN_LINE_THICKNESS;
            }
            else if (btn.Name.Contains("ROI"))
            {
                if (DEF_SYSTEM.LICENSES_KEY == (int)License.COG)
                {
                    CogROIToolTeachingForm.Visible = true;
                    btnDisplayROIToolForm.FlatAppearance.BorderSize = BTN_LINE_THICKNESS;
                    CogROIToolTeachingForm.RadioButtons[m_nSelectCam].Checked = true;
                    CogROIToolTeachingForm.ShowTeachingPage();
                }
                else if(DEF_SYSTEM.LICENSES_KEY == (int)License.INS)
                {
                    InsFindLineTeachingForm.Visible = true;
                    btnDisplayROIToolForm.FlatAppearance.BorderSize = BTN_LINE_THICKNESS;
                }
            }
            else if (btn.Name.Contains("Image"))
            {
                if (DEF_SYSTEM.LICENSES_KEY == (int)License.COG)
                {
                    CogImageFilterTeachingForm.Visible = true;
                    btnDisplayImageFilterForm.FlatAppearance.BorderSize = BTN_LINE_THICKNESS;
                    CogImageFilterTeachingForm.RadioButtons[m_nSelectCam].Checked = true;
                    CogImageFilterTeachingForm.ShowTeachingPage();
                }
                else
                {
                    InsPMTeachingForm.Visible = true;
                    btnDisplayImageFilterForm.FlatAppearance.BorderSize = BTN_LINE_THICKNESS;
                    //InsPMTeachingForm.RadioButtons[m_nSelectCam].Checked = true;  임시 주석
                }
            }
            else if (btn.Name.Contains("Blob"))
            {
                if (DEF_SYSTEM.LICENSES_KEY == (int)License.COG)
                {
                    CogBlobTeachForm.Visible = true;
                    btnDisplayBlobForm.FlatAppearance.BorderSize = BTN_LINE_THICKNESS;
                    CogBlobTeachForm.RadioButtons[m_nSelectCam].Checked= true;
                    CogBlobTeachForm.ShowTeachingPage();
                }
                else
                {
                    InsBlobTeachForm.Visible = true;
                    btnDisplayBlobForm.FlatAppearance.BorderSize = BTN_LINE_THICKNESS;
                    InsBlobTeachForm.RadioButtons[m_nSelectCam].Checked = true;
                }
            }
        }

        public Point Get_TeachFormScreenLocation()      //240721 NIS Get TeachForm Location
        {
            Point PanelLocation = MainForm.Get_DisplayPanelLocation();
            return new Point(PanelLocation.X + pnl_TeachFormScreen.Location.X, PanelLocation.Y + pnl_TeachFormScreen.Location.Y);
        }

        public FormTeachInspectionDeepLearning Get_DeepLearningTeachingForm()
        {
            return DeepLearningTeachingForm;
        }

        private void FormTeachInspection_Load(object sender, EventArgs e)   //240724 NIS Form이 Load될 때 Crack 클릭하기
        {
            btnDisplayROIToolForm.PerformClick();     //240724 NIS Display Crack Form
        }

    }
}
