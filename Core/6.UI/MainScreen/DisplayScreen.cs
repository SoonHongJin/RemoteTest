using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core;
using Core.DataProcess;
using Core.Function;
using Core.Utility;
using ScottPlot;
using static Core.Program;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Core.UI
{
    public partial class DisplayScreen : Form
    {
        public enum WAFER_CRACK { Crack = 0, Chipping, Broken };

        private MainForm MainForm = null;

        private bool Maximized = true;
        private bool Initialized = false;
        public List<CImageControl> Frames = new List<CImageControl>();
        public List<CImageControl> DefectMap = new List<CImageControl>();
        public List<CImageControl> DefectFrames = new List<CImageControl>();

        //private CProgressBar ProgressBar = new CProgressBar();

        //private CProgressBar[] ProgressBar = new CProgressBar[10];

        private readonly Point[] ProgressPoint = new Point[] { new Point { X = 12 , Y = 28  },
                                                               new Point { X = 12, Y = 77  },
                                                               new Point { X = 12, Y = 123 },
                                                               new Point { X = 12, Y = 170 },
                                                               new Point { X = 12, Y = 218 },
                                                               new Point { X = 12, Y = 263 },
                                                               new Point { X = 12, Y = 312 },
                                                               new Point { X = 12, Y = 361 },
                                                               new Point { X = 12, Y = 410 }
                                                               };

        //Ticks 간격 설정
        double[] xTicks = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
        double[] yTicks = { 0, 1000, 2000 };

        string[] XTicksLabels, YTicksLabels;
        List<double> xs = new List<double>();
        List<double> ys1 = new List<double>();
        List<double> ys2 = new List<double>();
        List<double> ys3 = new List<double>();
        int yTicksMax = 2000;   //디스플레이되는 y값 최대치

        //240724 NWT DisplayFilter Form
        bool DisplayFormchk = false;

        private List<Label> lblDefectNames = new List<Label>();
        private List<Label> lblDefectCount = new List<Label>();
        private List<Panel> pnlDefectStatus = new List<Panel>();
        private List<Button> btnDefectColors = new List<Button>();
        private List<Label> lblDefectTypes = new List<Label>();

        private CLogger Logger = null;

        //241010 NWT IMG List 용 열거형 추가      
        public enum ResultBar
        {
            OK,
            NG
        }

        public enum ResultPnl
        {
            UnSelectedOK,
            SelectedOK,
            UnSelectedNG,
            SelectedNG
        }

        public enum DisplayBtn
        {
            Unselected,
            Selected
        }
        public DisplayScreen(MainForm _MainForm, CLogger _logger)
        {
            InitializeComponent();

            MainForm = _MainForm;
            Logger = _logger;
            TopLevel = false;
            MainForm.SetEachControlResize(this);    //240801 NIS Conrol Resize

            theMainSystem.RefreshResult = DisplayResult;
            theMainSystem.RefreshDefectLabel = DefectClassListShow;
            theMainSystem.CycleTime = DisplayCycleTime;
            theMainSystem.RefreshScottPlot = ScottPlot_Refresh;
            theMainSystem.RefreshCropDefecType = CropDefectType;

            int Camcount = 0;
            
            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; ++i) //240621 LYK 수정
            {
                Frames.Add(new CImageControl(MainForm, Logger)
                {
                    Id = i,
                    Mode = DEF_SYSTEM.DISPLAY_IMAGE
                });

                // Panel 에 Frame 배치 
                MainDisplayPanel.Controls.Add(Frames[i]);
                Frames[i].SetCamera(theMainSystem.Cameras[i], DEF_SYSTEM.DISPLAY_IMAGE);

                DefectMap.Add(new CImageControl(MainForm, Logger)
                {
                    Id = i,
                    Mode = DEF_SYSTEM.DISPLAY_MAP
                });

                // Panel 에 Frame 배치 
                MainDisplayTable.Controls.Add(DefectMap[i]);
                DefectMap[i].SetCamera(theMainSystem.Cameras[i], DEF_SYSTEM.DISPLAY_MAP);

            }

            for (int i = 0; i < 10; i++)
            {
                DefectFrames.Add(new CImageControl(MainForm, Logger)
                {
                    Id = 0,
                    Idx = i,
                    Mode = DEF_SYSTEM.DISPLAY_DEFECT
                });
                Panel DefectPnl = this.Controls.Find($"DefectPanel{i}", true)[0] as Panel;
                DefectPnl.Controls.Add(DefectFrames[i]);
                DefectFrames[i].SetCamera(theMainSystem.Cameras[0], DEF_SYSTEM.DISPLAY_DEFECT);
            }

            this.FormLocate(0);

            

            SetPanelLabel();
            SetDisplayPanel();
            ScottPlot_Setting();

            Initialized = true;
        }

        private void FormLocate(int _type)
        {
            //Maximized = !Maximized;
            Rectangle fullSize = new Rectangle(0, 0, MainDisplayPanel.Width, MainDisplayPanel.Height);//FullPanel.Bounds;

            //int width = MainDisplayPanel.Width;
            int width = MainDisplayPanel.Width / 2;
            int height = MainDisplayPanel.Height / 2;
            Rectangle[] bounds = new Rectangle[DEF_SYSTEM.CAM_MAX_COUNT];

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; ++i)
            {
                bounds[i].X = 0;
                bounds[i].Y = 0;

                bounds[i].Width = MainDisplayPanel.Width;
                bounds[i].Height = MainDisplayPanel.Height;

                // White Display 분할
                Frames[i].Bounds = bounds[i];
                Frames[i].Show();

                DefectMap[i].Bounds = bounds[i];
                DefectMap[i].Show();
            }

            for (int i = 0; i < 10; i++)
            {
                Rectangle rec = new Rectangle();

                Panel DefectPnl = this.Controls.Find($"DefectPanel{i}", true)[0] as Panel;

                rec.X = 0;
                rec.Y = 0;
                rec.Width = DefectPnl.Width;
                rec.Height = DefectPnl.Height;

                DefectFrames[i].Bounds = rec;
                DefectFrames[i].Show();
            }
        }

        /// <summary>
        /// 24.03.22 LYK SetPanelLabel 
        /// Defect와 관련된 Panel, Label등에 색상 변경(OK - 초록, NG - 빨강), 데이터 등을 Set 하는 함수
        /// 간단히 처리 할 수 있도록 반복문으로 변경
        /// </summary>
        private void SetPanelLabel()
        {
            //try
            {
                lbl_WaferID.Text = "";
                lbl_DeepClass.Text = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? "Classify" : "Segmentation";

                // 20250915 SHJ 메인 화면 프로젝트에 맞게 딥러닝 클래스 표시 
                int nClassCnt = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? theRecipe.ClassifyName.Count : theRecipe.SegClassName.Count;

                for (int i = 0; i < nClassCnt; i++)
                {
                    //this.Controls.Find($"lbl_DefectName{i + 1}", true)[0].Text = $"Label_{theRecipe.ClassifyName[i].ClassifyName}";
                    //this.Controls.Find($"lbl_Mono{i + 1}", true)[0].Text = "0";
                    //this.Controls.Find($"btn_DefectColor{i + 1}", true)[0].BackColor = theRecipe.ClassifyName[i].m_sClassColor;//Color.Gray;

                    string ClassName = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? theRecipe.ClassifyName[i].ClassifyName : theRecipe.SegClassName[i].SegClassName;

                    Label lblName = this.Controls.Find($"lbl_DefectName{i + 1}", true)[0] as Label;
                    lblName.Text = $"Label_{ClassName}";
                    lblDefectNames.Add(lblName);

                    Label lblCount = this.Controls.Find($"lbl_Mono{i + 1}", true)[0] as Label;
                    lblCount.Text = "0";
                    lblDefectCount.Add(lblCount);

                    Panel pnlStatus = this.Controls.Find($"pnl_Mono{i + 1}", true)[0] as Panel;
                    pnlStatus.BackgroundImage = ResultBarImgList.Images[0];
                    pnlDefectStatus.Add(pnlStatus);

                    Color classColor = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? theRecipe.ClassifyName[i].m_sClassColor : theRecipe.SegClassName[i].m_sClassColor;
                    Button btnClassColor = this.Controls.Find($"btn_DefectColor{i + 1}", true)[0] as Button;
                    btnClassColor.BackColor = classColor;
                    btnDefectColors.Add(btnClassColor);
                }

                for (int i = nClassCnt; i < DEF_UI.CLSCOUNT; i++)
                {
                    this.Controls.Find($"lbl_DefectName{i + 1}", true)[0].Visible = false;
                    this.Controls.Find($"lbl_Mono{i + 1}", true)[0].Visible = false;
                    this.Controls.Find($"pnl_Mono{i + 1}", true)[0].Visible = false;
                    this.Controls.Find($"btn_DefectColor{i + 1}", true)[0].Visible = false;
                }


                for (int i = 0; i < 10; i ++)
                {
                    Label lblType = this.Controls.Find($"lblDefectType{i}", true)[0] as Label;
                    lblType.Text = $"-";
                    lblDefectTypes.Add(lblType);
                }

                //ProgressBar = new CProgressBar();
                //ProgressBar.Width = 360;
                //ProgressBar.Height = 28;
                //ProgressBar.Location = new Point(10, 55);
                //pnl_Yield.Controls.Add(ProgressBar);

                double Ratio = 0;
                if (theRecipe.m_nTotalRunCount != 0)
                    Ratio = theRecipe.m_nOKRunCount / theRecipe.m_nTotalRunCount;

                //ProgressBar.Value = (int)Ratio;
                //ProgressBar.sValue = Ratio.ToString("F2");
                //ProgressBar.ProgressColor = Color.AliceBlue;

                //for (int i = 0; i < theRecipe.SegClassName.Count; i++)
                //{
                //    this.Controls.Find($"lbl_DefectName{i + 1}", true)[0].Text = $"Label_{theRecipe.SegClassName[i].SegClassName}";
                //    this.Controls.Find($"lbl_Mono{i + 1}", true)[0].Text = "0";
                //    this.Controls.Find($"btn_DefectColor{i + 1}", true)[0].BackColor = theRecipe.SegClassName[i].m_sClassColor;
                //}

                //for (int i = theRecipe.SegClassName.Count; i < DEF_UI.SEGCOUNT; i++)
                //{
                //    this.Controls.Find($"lbl_DefectName{i + 1}", true)[0].Visible = false;
                //    this.Controls.Find($"lbl_Mono{i + 1}", true)[0].Visible = false;
                //    this.Controls.Find($"pnl_Mono{i + 1}", true)[0].Visible = false;
                //    this.Controls.Find($"btn_DefectColor{i + 1}", true)[0].Visible = false;
                //}

                //for (int i = 0; i < theRecipe.ClassifyName.Count; i++)
                //{
                //    this.Controls.Find($"lbl_Color{i + 1}", true)[0].Text = theRecipe.ClassifyName[i].ClassifyName;
                //    this.Controls.Find($"lbl_Color{i + 1}", true)[0].Visible = true;
                //    this.Controls.Find($"pnl_Color{i + 1}", true)[0].BackColor = Color.Gray;
                //    this.Controls.Find($"pnl_Color{i + 1}", true)[0].Visible = true;
                //}
                ////240922 NWT Color Defect
                //for (int i = theRecipe.ClassifyName.Count; i < DEF_UI.CLSCOUNT; i++)
                //{
                //    this.Controls.Find($"lbl_Color{i + 1}", true)[0].Visible = false;
                //    this.Controls.Find($"pnl_Color{i + 1}", true)[0].Visible = false;
                //}
            }
            //catch(Exception e)
            //{

            //} 
        }

        /// <summary>
        /// 24.03.22 LYK DefectListShow 함수 수정
        /// 복잡했던 내용 정리
        /// </summary>
        /// <param name="_Manager"></param>
        /// <param name="_Infos"></param>

        private void DefectClassListShow(InspectionInfo _Infos, int _CurIdx)
        {
            try
            {
                // Main System 에서 Result Update 할때 한번에 호출 되도록 수정 
                this.Invoke(new MethodInvoker(delegate ()
                {
                    for (int i = 0; i < theRecipe.ClassifyName.Count; i++)
                    {
                        //Label lblCount = pnl_MonoDisplay.Controls.Find($"lbl_Mono{i + 1}", true)[0] as Label;
                        //Panel pnlStatus = pnl_MonoDisplay.Controls.Find($"pnl_Mono{i + 1}", true)[0] as Panel;
                        //Button btnClassColor = pnl_MonoDisplay.Controls.Find($"btn_DefectColor{i + 1}", true)[0] as Button;

                        string sJudge = theRecipe.ClassifyName[i].sJudge;

                        if (sJudge == "OK")
                        {
                            pnlDefectStatus[i].BackgroundImage = ResultBarImgList.Images[1];   // OK Image
                        }
                        else if (sJudge == "SEMI_OK")
                        {
                            pnlDefectStatus[i].BackgroundImage = ResultBarImgList.Images[2];   // Semi OK Image
                        }
                        else if (sJudge == "NG")
                        {
                            pnlDefectStatus[i].BackgroundImage = ResultBarImgList.Images[3];   // NG Image
                        }

                        //else
                        //    MonoDefectImage.BackgroundImage = ResultBarImgList.Images[0];   //None Image, 미사용 또는 Test 8

                        // 클래스 색상이 표시가 되지 않아 Judge 와 색상 같도록 임시 수정 
                        btnDefectColors[i].BackColor = theRecipe.ClassifyName[i].m_sClassColor;
                        lblDefectCount[i].Text = theRecipe.ClassifyName[i].nTotalCnt.ToString();
                    }

                }));
            }
            catch (Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"DefectListShow Catch Error : {e.Message}");
            }
        }

        private void DisplayCycleTime(string _sTime)
        {
            int offset = 10;
            try
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    lbl_GrabCycleTime.ForeColor = Color.White;
                    lbl_GrabCycleTime.Text = _sTime + " ms";
                    lbl_GrabCycleTime.Location = new Point(pnl_TimeDisplay.Width - lbl_GrabCycleTime.Width - offset, lbl_GrabCycleTime.Location.Y);

                }));
            }
            catch (Exception e)
            {

            }

        }

        // <summary>
        ///25.05.20 LYK
        /// 검사 결과에 따른 불량 타입을 화면 라벨에 표시하는 함수
        /// (UI 스레드에서 안전하게 실행되도록 보장)
        /// </summary>
        /// <param name="_Infos">현재 검사 결과 정보</param>
        /// <param name="_CurIdx">표시할 인덱스</param>
        private void CropDefectType(InspectionInfo _Infos, int _CurIdx)
        {
            try
            {
                //250520 LYK 1. 입력 값 유효성 검사
                if (_Infos == null || _Infos.InspResults == null)
                    return;

                int count = _Infos.InspResults.Count > 10 ? 10 : _Infos.InspResults.Count;

                //250520 LYK 2. 라벨을 업데이트할 로직 정의
                void UpdateAllLabels()
                {
                    //for (int i = 0; i < _Infos.InspResults.Count; i++)
                    //{
                    //    var result = _Infos.InspResults[i];

                    //    if(i > _Infos.InspResults.Count)
                    //        lblDefectTypes[i].Text = result != null ? result.m_sDefectType : "-";
                    //}

                    // 250530 SHJ 위 내용 임시 주석 Insp Defect Type 과 UI 라벨 갯수로 매칭 
                    for(int i = 0; i < lblDefectTypes.Count; i ++)
                    {

                        if (_Infos.InspResults.Count >= lblDefectTypes.Count)
                        {
                            var result = _Infos.InspResults[i];
                            lblDefectTypes[i].Text = result != null ? result.m_sDefectType : "-";
                        }
                            
                    }
                }

                //250520 LYK 3. UI 스레드 여부에 따라 처리 분기
                if (this.InvokeRequired)
                    this.Invoke(new MethodInvoker(UpdateAllLabels));
                else
                    UpdateAllLabels();
            }
            catch (Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog],
                    "CropDefectType Exception: " + e.ToString());
            }
        }

        private void DisplayResult(InspectionInfo _Infos, int _CurIdx)
        {

            int offset = 10;
            try
            {

                this.Invoke(new MethodInvoker(delegate ()
                {

                    lbl_InspectionTotalTime.ForeColor = Color.White;
                    lbl_InspectionTotalTime.Text = _Infos.totalGrabInspectionTime.ToString() + " ms";
                    lbl_InspectionTotalTime.Location = new Point(pnl_TimeDisplay.Width - lbl_InspectionTotalTime.Width - offset, lbl_InspectionTotalTime.Location.Y);

                    lbl_DeepLearningTime.ForeColor = Color.White;
                    lbl_DeepLearningTime.Text = _Infos.DeepLearningTime.ToString() + " ms";
                    lbl_DeepLearningTime.Location = new Point(pnl_TimeDisplay.Width - lbl_DeepLearningTime.Width - offset, lbl_DeepLearningTime.Location.Y);

                    lbl_GrabCycleTime.ForeColor = Color.White;
                    lbl_GrabCycleTime.Text = _Infos.InspTime + " ms";
                    lbl_GrabCycleTime.Location = new Point(pnl_TimeDisplay.Width - lbl_GrabCycleTime.Width - offset, lbl_GrabCycleTime.Location.Y);

                    lbl_WaferID.Text = _Infos.ID;
                    lbl_DefectCount.Text = _Infos.nDefectCnt.ToString();

                    //250517 LYK 임시 주석
                    //lbl_TotalCount.Text = theRecipe.m_nTotalRunCount.ToString();
                    //lbl_NGCount.Text = theRecipe.m_nNGRunCount.ToString();
                    //
                    //double Ratio = (theRecipe.m_nOKRunCount / theRecipe.m_nTotalRunCount) * 100;

                    //250403 LYK 딥러닝 사용 유무에 따라 OK, NG 판정  임시 처리

                    if (_Infos.sJudge == "NG" || _Infos.sJudge != "OK")
                    {
                        pnl_RuleOK.BackgroundImage = ResultpnlImgList.Images[(int)ResultPnl.UnSelectedOK];
                        pnl_RuleNG.BackgroundImage = ResultpnlImgList.Images[(int)ResultPnl.SelectedNG];
                    }
                    else
                    {
                        pnl_RuleOK.BackgroundImage = ResultpnlImgList.Images[(int)ResultPnl.SelectedOK];
                        pnl_RuleNG.BackgroundImage = ResultpnlImgList.Images[(int)ResultPnl.UnSelectedNG];
                    }

                    if (_Infos.sClassifyJudge == "NG" || _Infos.sClassifyJudge != "OK")
                    {
                        pnl_DeepOK.BackgroundImage = ResultpnlImgList.Images[(int)ResultPnl.UnSelectedOK];
                        pnl_DeepNG.BackgroundImage = ResultpnlImgList.Images[(int)ResultPnl.SelectedNG];
                    }
                    else
                    {
                        pnl_DeepOK.BackgroundImage = ResultpnlImgList.Images[(int)ResultPnl.SelectedOK];
                        pnl_DeepNG.BackgroundImage = ResultpnlImgList.Images[(int)ResultPnl.UnSelectedNG];
                    }


                    //this.Update();
                }));
            }
            catch (Exception e)
            {

            }

        }

        private void ScottPlot_Setting()
        {
            //XTick 설정
            XTicksLabels = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25" };
            formsPlot.Plot.XTicks(xTicks, XTicksLabels);
            formsPlot.Plot.SetAxisLimitsX(0, XTicksLabels.Length + 1);

            //YTick 설정
            YTicksLabels = new string[] { "0", "1000", "2000" };
            formsPlot.Plot.YTicks(yTicks, YTicksLabels);
            formsPlot.Plot.SetAxisLimitsY(0, yTicksMax + 1);
            formsPlot.Plot.YLabel("[ms]");

            formsPlot.Plot.Style(figureBackground: Color.Transparent, dataBackground: Color.Transparent, grid: Color.White, tick: Color.White, axisLabel: Color.White);

            //범례 설정
            //pictureBox_Legend.Image = formsPlot.Plot.RenderLegend();

            formsPlot.Refresh();
        }

        private void btn_Trigger_Click(object sender, EventArgs e)
        {
            //theRecipe.m_nTestNumber++;
            //DisableTriggerBtn();
            string[] LastNGData = { "", ""};// MainForm.BottomForm.GetNGDataInfo();   임시 주석
            theRecipe.LastRecipeSave(theRecipe.m_sCurrentModelName, LastNGData);


            string msg = $"Inspection Start.";
            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], msg);

            theMainSystem.DoInspectionStart();
        }

        private void btn_DisplayFilter_Click(object sender, EventArgs e)        //240708 NWT MainDisplay Defect UI Popup창 추가
        {
            int width = 0;
            int height = 0;
            GetDisplayFilterLocation(ref width, ref height);
            //MainForm.ShowDisplayFilter(width, height);        //임시 주석
        }

        private void ScottPlot_Refresh(InspectionInfo _Info, int _GrabCycleTime)    //검사 데이터 들어올 때마다 해당 메서드 실행하는 것으로 수정 필요
        {
            try
            {
                this.Invoke(new Action(delegate ()
                {
                    //Plot 초기화
                    formsPlot.Plot.Clear();

                    //FowardToBack Refresh
                    //ys1.Insert(0, GetRandomInt());
                    //ys2.Insert(0, GetRandomInt());
                    //ys3.Insert(0, GetRandomInt());

                    //CSV 데이터 추가하기
                    ys1.Insert(0, _Info.totalGrabInspectionTime);
                    ys2.Insert(0, _Info.DeepLearningTime);
                    ys3.Insert(0, _Info.InspTime);

                    if (xs.Count < 25)
                    {
                        xs.Add(xs.Count + 1);
                    }
                    if (ys1.Count > 25)
                    {
                        ys1.RemoveAt(25);
                        ys2.RemoveAt(25);
                        ys3.RemoveAt(25);
                    }

                    //플랏 추가
                    formsPlot.Plot.AddScatter(xs.ToArray(), ys1.ToArray(), Color.Red, label: "Inspection Time");
                    formsPlot.Plot.AddScatter(xs.ToArray(), ys2.ToArray(), Color.Green, label: "Deeplearning Time");
                    formsPlot.Plot.AddScatter(xs.ToArray(), ys3.ToArray(), Color.Blue, label: "Rulebase Time");

                    //설정
                    ScottPlot_Setting();

                    //Plot 갱신
                    formsPlot.Refresh();
                }));
            }
            catch (Exception e)
            {

            }
        }

        private void ActivateForm(Form form)
        {
            if (DisplayFormchk == false)
            {
                DisplayFormchk = true;
                form.Show();
                form.FormClosing += formclosing_Event;
            }
        }

        private void lbl_WaferID_Click(object sender, EventArgs e)
        {

        }
        //240922 NWT Display Panel 버튼 추가
        //241007 NWT 이미지 리스트 추가
        private void btn_MonoDisplay_Click(object sender, EventArgs e)
        {
            tab_Display.SelectedIndex = 0;
            btn_MonoDisplay.BackgroundImage = DisplaybtnImgList.Images[(int)DisplayBtn.Selected];
            btn_ColorDisplay.BackgroundImage = DisplaybtnImgList.Images[(int)DisplayBtn.Unselected];
        }

        //241007 NWT 이미지 리스트 추가
        private void btn_ColorDisplay_Click(object sender, EventArgs e)
        {
            tab_Display.SelectedIndex = 1;
            btn_ColorDisplay.BackgroundImage = DisplaybtnImgList.Images[(int)DisplayBtn.Selected];
            btn_MonoDisplay.BackgroundImage = DisplaybtnImgList.Images[(int)DisplayBtn.Unselected];
        }

        //240922 NWT 처음 실행 시 Mono Display
        //241007 NWT 이미지 리스트 추가
        private void SetDisplayPanel()
        {
            tab_Display.SelectedIndex = 0;
            btn_MonoDisplay.BackgroundImage = DisplaybtnImgList.Images[(int)DisplayBtn.Selected];
            btn_ColorDisplay.BackgroundImage = DisplaybtnImgList.Images[(int)DisplayBtn.Unselected];
        }

        private void formclosing_Event(object sender, EventArgs e)
        {
            DisplayFormchk = false;
        }

        public void GetDisplayFilterLocation(ref int _width, ref int _height)
        {
            _width = panel4.Size.Width + btn_DisplayFilter.Location.X;
            _height = DEF_UI.TOP_SIZE_HEIGHT + btn_DisplayFilter.Location.Y + btn_DisplayFilter.Height;
        }

        /// <summary>
        /// 2025.02.07 NWT Trigger Click시 모든 검사가 완료될 때 까지 Trigger button 숨김처리
        /// </summary>
        public void DisableTriggerBtn()
        {
            this.Invoke(new MethodInvoker(delegate ()
            {
                btn_Trigger.Enabled = false;
            }));

        }

        public void EnableTriggerBtn()
        {
            this.Invoke(new MethodInvoker(delegate ()
            {
                btn_Trigger.Enabled = true;
            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            theMainSystem.DoInspectionStop();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        public void GetMousePoint(float _X, float _Y)
        {
            lbl_MouseLocation.Text = $"X: {_X:f3} Y: {_Y:f3}";
        }
    }
}
