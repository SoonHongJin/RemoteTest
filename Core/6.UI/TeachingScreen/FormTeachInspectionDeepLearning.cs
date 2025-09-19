using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.Dimensioning;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Implementation;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.ToolBlock;
using Core.DataProcess;
using Core.Function;
using Core.Utility;
using Insnex.Vision2D.CalibFix;
using Insnex.Vision2D.Common;
using Insnex.Vision2D.Core;
using Insnex.Vision2D.Core;
using Insnex.Vision2D.Finder;
using Insnex.Vision2D.ImageProcessing;
using Insnex.Vision2D.ImageProcessing;
using Insnex.Vision2D.Intersection;
using Insnex.Vision2D.Measurement;
using Insnex.Vision2D.ToolBlock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Core.Program;

namespace Core.UI
{
    public partial class FormTeachInspectionDeepLearning : Form
    {
        private MainForm MainForm = null;
        private FormTeachInspection TeachMainForm;  //parent form
        private int m_nSelectCam = 0;       //Selected cam number
        private int m_nToolSelected = 0;    //Selected tool list index
        private int m_nInspectSelected = 0; //Selected inspection mode

        CogToolBlock _ToolBlock = null;     //CogToolBlock

        private List<ICogTool> m_TeachingToolList = new List<ICogTool>();   //Teaching CogTool List

        private Font font = new Font("Calibri", 18, FontStyle.Bold);
        private Font ComboBoxFont = new Font("Calibri", 9, FontStyle.Bold);

        private Point TeachFormLocation;
        public List<CTeachImageControl> Frames = new List<CTeachImageControl>();  //240209 LYK CImage Control List
        private int SelectedRowIndex = -1;
        private int SelectedColIndex = -1;
        private string strDictionaryName = "";
        List<int> GroupIndex = new List<int>();

        //250122 NWT Parameter Exception
        private List<CExceptionData> exceptionDataList = new List<CExceptionData>();
        private ParameterExceptionHandler parameterExceptionHandler = null;

        private CLogger Logger = null;
        private int LBL_END_POINT = 210;

        private List<System.Windows.Forms.Label> lblDefectNameList = new List<System.Windows.Forms.Label>();
        private List<TextBox> tbDefectScoreList = new List<TextBox>();
        private List<TextBox> tbDefectAreScoreList = new List<TextBox>();

        public FormTeachInspectionDeepLearning(MainForm _MainForm, FormTeachInspection _TeachMainForm, CLogger _logger)
        {
            InitializeComponent();
            TopLevel = false;
            TeachMainForm = _TeachMainForm;
            MainForm = _MainForm;
            exceptionDataList = theRecipe.m_listExceptionData;

            Logger = _logger;

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; ++i)
            {
                // 0~3 : Mono (R,G,B 중 하나) 
                Frames.Add(new CTeachImageControl(MainForm, Logger)
                {
                    Id = i
                });

                // Live Display 와 다른 점 : Panel 하나에 Bound 4개, Live 는 각 패널에 Bound 1개 씩 
                Panel_Display.Controls.Add(Frames[i]); // Mono 

                Frames[i].SetCamera(theMainSystem.Cameras[i], 0);
            }

            DoLoad();

            //MainForm.SetEachControlResize(this);    //240801 NIS Conrol Resize

            this.FormLocate(0);

            m_nInspectSelected = (int)INSP_MODE.DeepLearning;

            TeachFormLocation = TeachMainForm.Get_TeachFormScreenLocation();  //240721 NIS Get TeachFormScreen Location
            
        }

        private void btn_Run_Click(object sender, EventArgs e)
        {
            //for(int j = 0; j < DEF_SYSTEM.CAM_MAX_COUNT; j ++)
            //{
            //            for (int i = 0; i < theMainSystem.ProductInfo.DefectManager.Count; i++)
            //                theMainSystem.ProductInfo.DefectManager[j][i].Dispose();
            //}

            //theMainSystem.ProductInfo.DefectManager[theMainSystem.m_CurrrentIdx].Clear();        

            //250127 NIS FingerKnotAllow ROI
            //for (int i = 0; i <= DEF_SYSTEM.CAM_FOUR; i++)
            //    InnerFingerAllowIdx[i] = i % 2 == 0 ? 0 : FingerKnotAllowPointList[i - 1].Count - 2;

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)    
            {
                //딥러닝 검사 실행 -> 순차 실행을 위해 0번 카메라에서 루프로 실행 

                theMainSystem.Cameras[i].m_bIsInspection = true; // Inspection Mode 

                theMainSystem.Cameras[i].RunTeachingInspect(0, true); // -> Full Mode 검사 Merge 카운트 전체를 검사 한다 

                // 200515 SHJ  ToolBlock Run 결과는 theMainSystem.ProductInfo.DefectManager 보유하고 있다 
                //theMainSystem.Cameras[i].InspDeepLearning[0].DefectCopy(theMainSystem.ProductInfo.DefectManager[i][j]);
                //theMainSystem.Cameras[i].InspDeepLearning[0].WorkComplete(theMainSystem.ProductInfo.DefectManager[i], null);

                // Teaching Page Show
                this.Frames[i].Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                {
                    theMainSystem.Cameras[i].ShowTeachingDisplay();
                }));
            }

        }
        private void ApplyAndSave_Click(object sender, EventArgs e)
        {
            Run_ApplyAndSave();
        }
        private void Run_ApplyAndSave()
        {
            foreach (Control tabPageCon in tabDeepLearning.Controls)
            {
                try
                {
                    if (tabPageCon.Name == "tabPageDeepParam")
                    {
                        //for (int i = 0; i < theRecipe.SegClassName.Count - 3; i++)
                        for(int i = 0; i < theRecipe.ClassifyName.Count; i ++)
                        {
                            //theRecipe.ClassifyName[i].nSetScore = Convert.ToInt32(tabPageCon.Controls.Find($"txtScoreThre{i + 1}", true)[0].Text);
                            //theRecipe.m_nDefectScoreThreshHold[i] = theRecipe.ClassifyName[i].nSetScore;// Convert.ToInt32(tabPageCon.Controls.Find($"txtScoreThre{i + 1}", true)[0].Text);
                            //theRecipe.m_nDefectAreaThreshHold[i] = Convert.ToInt32(tabPageCon.Controls.Find($"txtAreScoreThre{i + 1}", true)[0].Text);

                            theRecipe.SegClassName[i].nSetScore = Convert.ToInt32(tabPageCon.Controls.Find($"txtScoreThre{i + 1}", true)[0].Text);
                            theRecipe.m_nDefectScoreThreshHold[i] = theRecipe.SegClassName[i].nSetScore;// Convert.ToInt32(tabPageCon.Controls.Find($"txtScoreThre{i + 1}", true)[0].Text);
                            theRecipe.m_nDefectAreaThreshHold[i] = Convert.ToInt32(tabPageCon.Controls.Find($"txtAreScoreThre{i + 1}", true)[0].Text);//250821 SJH
                        }

                        theMainSystem.Cameras[DEF_SYSTEM.CAM_ONE].InspDeepLearning.InspectionOptionSet(theRecipe.m_nDefectScoreThreshHold, theRecipe.m_nDefectAreaThreshHold);    //임시 주석

                    }
                    else
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "DeepLearning tabPage is out of Index");
                }
                catch (Exception ex)
                {
                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Class : {this.Name}, Method : Run_ApplyAndSave() - {tabPageCon.Name} page save Error: {ex.Message}");
                }
            }
            theRecipe.DataSave(theRecipe.m_sCurrentModelName);
            theRecipe.ToolSave(theRecipe.m_sCurrentModelName);

            //for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            //    theMainSystem.Cameras[i].ToolBlockRefresh();
        }

        private void FormLocate(int _type)
        {
            Rectangle fullSize = new Rectangle(0, 0, Panel_Display.Width, Panel_Display.Height);  //FullPanel.Bounds;

            int width;
            int height;
            int length;

            width = Panel_Display.Width;
            height = Panel_Display.Height;

            length = width < height ? width : height;

            Rectangle[] bounds = new Rectangle[4];

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; ++i)
            {
                bounds[i].X = 0;// fullSize.X;// width - length;
                bounds[i].Y = 0;// fullSize.Y;// height - length;
                bounds[i].Width = fullSize.Width;//length;
                bounds[i].Height = fullSize.Height;//length;

                // Mono Display 분할 
                Frames[i].Bounds = bounds[i];
                Frames[i].Show();
            }
        }
        private void DoLoad()
        {
            try
            {
                foreach (Control ct in this.Controls)
                {
                    if (ct.GetType().Name.ToLower() == "tabcontrol")
                    {
                        TabControl tab = ct as TabControl;
                       
                        foreach (Control tabPageCon in tab.Controls)
                        {
                            if (tabPageCon.Name == "tabPageDeepParam")
                            {
                                //LBL_END_POINT = txtScoreThre1.Location.X - 5;
                                //for (int i = 0; i < theRecipe.SegClassName.Count; i++)
                                //{
                                //    if (theRecipe.SegClassName[i].SegClassName == "Crack" || theRecipe.SegClassName[i].SegClassName == "Chipping" || theRecipe.SegClassName[i].SegClassName == "Broken")
                                //        continue;

                                //    Label label = tabPageCon.Controls.Find($"lbl_DefectName{i + 1}", true)[0] as Label;
                                //    label.Text = theRecipe.SegClassName[i].SegClassName;
                                //    //tabPageCon.Controls.Find($"lbl_DefectName{i + 1}", true)[0].Text = theRecipe.SegClassName[i].SegClassName;
                                //    tabPageCon.Controls.Find($"txtScoreThre{i + 1}", true)[0].Text = theRecipe.m_nDefectScoreThreshHold[i].ToString();
                                //    tabPageCon.Controls.Find($"txtAreScoreThre{i + 1}", true)[0].Text = theRecipe.m_nDefectAreaThreshHold[i].ToString();
                                //    label.SetBounds(LBL_END_POINT - label.Width, label.Location.Y, label.Width, label.Height);
                                //}

                                //if (theRecipe.SegClassName.Count < 18)
                                //{
                                //    for (int i = theRecipe.SegClassName.Count - 3; i < 18; i++)
                                //    {
                                //        tabPageCon.Controls.Find($"lbl_DefectName{i + 1}", true)[0].Visible = false;
                                //        tabPageCon.Controls.Find($"txtScoreThre{i + 1}", true)[0].Visible = false;
                                //        tabPageCon.Controls.Find($"txtAreScoreThre{i + 1}", true)[0].Visible = false;
                                //    }
                                //}

                                // 250515 SHJ 탭 컨트롤 중 Label , Textbox 는 전부 화면에 안보이게 처리 -> 타입 형태로 Disable 시킬려고 하였으나 표시해야 하는 항목도 있어서 이름으로 처리

                                if (theRecipe.m_sCurrentEquipment.Contains("ELECTRONIC"))//YJS 250910
                                {
                                    for (int i = 0; i < tabPageCon.Controls.Count; i++)
                                    {
                                        if (tabPageCon.Controls[i].Name.Contains("Defect") || tabPageCon.Controls[i].Name.Contains("Score"))
                                        {
                                            if (tabPageCon.Controls[i].Name.Contains("txtAreScoreThreLabel"))
                                                continue;
                                            else
                                                tabPageCon.Controls[i].Visible = false;
                                        }
                                    }

                                    for (int i = 0; i < theRecipe.SegClassName.Count; i++)
                                    {
                                        //this.Controls.Find($"lbl_DefectName{i + 1}", true)[0].Text = $"Label_{theRecipe.ClassifyName[i].ClassifyName}";
                                        //this.Controls.Find($"lbl_Mono{i + 1}", true)[0].Text = "0";
                                        //this.Controls.Find($"btn_DefectColor{i + 1}", true)[0].BackColor = theRecipe.ClassifyName[i].m_sClassColor;//Color.Gray;

                                        System.Windows.Forms.Label lblName = tabPageCon.Controls.Find($"lbl_DefectName{i + 1}", true)[0] as System.Windows.Forms.Label;
                                        lblName.Text = $"{theRecipe.SegClassName[i].SegClassName}";
                                        lblName.Visible = true;
                                        lblName.SetBounds(LBL_END_POINT - lblName.Width, lblName.Location.Y, lblName.Width, lblName.Height);
                                        lblDefectNameList.Add(lblName);

                                        TextBox tbScore = tabPageCon.Controls.Find($"txtScoreThre{i + 1}", true)[0] as TextBox;
                                        //tbScore.Text = theRecipe.SegClassName[i].nSetScore.ToString();
                                        tbScore.Text = theRecipe.SegClassName[i].nSetScore.ToString();
                                        tbScore.Visible = true;
                                        tbScore.TextChanged += txtScoreTextChange;
                                        tbDefectScoreList.Add(tbScore);

                                        TextBox tbAreScore = this.Controls.Find($"txtAreScoreThre{i + 1}", true)[0] as TextBox;
                                        tbAreScore.Text = theRecipe.m_nDefectAreaThreshHold[i].ToString();
                                        tbAreScore.Visible = true;
                                        tbAreScore.TextChanged += txtScoreTextChange;
                                        tbDefectAreScoreList.Add(tbAreScore);
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < tabPageCon.Controls.Count; i++)
                                    {
                                        if (tabPageCon.Controls[i].Name.Contains("Defect") || tabPageCon.Controls[i].Name.Contains("Score"))
                                            tabPageCon.Controls[i].Visible = false;
                                    }
                                    // 250515 SHJ 클래시피 갯수 만큼 화면 표시 
                                    for (int i = 0; i < theRecipe.ClassifyName.Count; i++)
                                    {
                                        //this.Controls.Find($"lbl_DefectName{i + 1}", true)[0].Text = $"Label_{theRecipe.ClassifyName[i].ClassifyName}";
                                        //this.Controls.Find($"lbl_Mono{i + 1}", true)[0].Text = "0";
                                        //this.Controls.Find($"btn_DefectColor{i + 1}", true)[0].BackColor = theRecipe.ClassifyName[i].m_sClassColor;//Color.Gray;

                                        System.Windows.Forms.Label lblName = tabPageCon.Controls.Find($"lbl_DefectName{i + 1}", true)[0] as System.Windows.Forms.Label;
                                        lblName.Text = $"{theRecipe.ClassifyName[i].ClassifyName}";
                                        lblName.Visible = true;
                                        lblName.SetBounds(LBL_END_POINT - lblName.Width, lblName.Location.Y, lblName.Width, lblName.Height);
                                        lblDefectNameList.Add(lblName);

                                        TextBox tbScore = tabPageCon.Controls.Find($"txtScoreThre{i + 1}", true)[0] as TextBox;
                                        tbScore.Text = theRecipe.ClassifyName[i].nSetScore.ToString();
                                        tbScore.Visible = true;
                                        tbScore.TextChanged += txtScoreTextChange;
                                        tbDefectScoreList.Add(tbScore);

                                        TextBox tbAreScore = this.Controls.Find($"txtAreScoreThre{i + 1}", true)[0] as TextBox;
                                        tbAreScore.Text = theRecipe.m_nDefectAreaThreshHold[i].ToString();
                                        tbDefectAreScoreList.Add(tbAreScore);
                                    }
                                }
                                    LBL_END_POINT = txtScoreThre1.Location.X - 5;
                            }
  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Class : {this.Name}, Method : DoLoad() - Error: {ex.Message}");
            }
        }

        private void SetRadioButtonText(RadioButton rdb, string[] variableArray, int index)
        {
            
        }

        private void ToolBlockRun(int _nCamNum, InsToolBlock _toolBlock, int _nMode)    //240720 NIS ToolBlock Run
        {
            
        }

        private void Reset_DisplayScreen()
        {
            for (int i = 0; i < Frames.Count; i++)
                Frames[i].SetImage(null);
        }

        private void FormTeachInspectionDeepLearning_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                Reset_DisplayScreen();
            }
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
                if (tempTxtBox.Name.Contains(exceptionDataList[i].Tag))
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

        private void txtScoreTextChange(object sender, EventArgs e)
        {
            // 250515 SHJ Text 박스에 0 -100 값만 입력 되도록 처리
            TextBox tb = (TextBox)sender;
            /*
            int ChangeValue = int.Parse(tb.Text);

            if (ChangeValue > 100)
                ChangeValue = 100;

            if (ChangeValue < 0)
                ChangeValue = 0;
           
            tb.Text = ChangeValue.ToString();
            */
        }
    }
}
