using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Core;
using Core.Utility;
using Core.Process;
using Core.HardWare;
using static Core.Program;
using Core.DataProcess;
using InsCHVSControl;
using ScottPlot;

namespace Core.UI
{
    public partial class FormSettingScreen : Form
    {
        private MainForm MainForm = null;

        private int m_cntTest;
        private CCameraManager CameraManager = null;
        private List<byte> SendDataList = new List<byte>();
        private byte[] arrSendData = new byte[11];
        private bool m_bFirstCheck = false;
        private int IndexCameraNum = 0;
        private Rectangle LightArearet = new Rectangle();

        private bool m_bLiveStop = false;
        private int m_nSelectedPage = 0;            //240311 LYK SelectedPage Controller Count
        private CPageController m_PageController;   //240312 LYK Page Controller Instance

        public List<CLiveImageControl> Frames = new List<CLiveImageControl>();
        //250122 NWT Parameter Exception
        private List<CExceptionData> exceptionDataList = new List<CExceptionData>();
        private ParameterExceptionHandler parameterExceptionHandler = null;
        private bool m_bLiveEnable = false;

        private CLogger Logger = null;

        double[] PlotXaxis = null;
        
        public FormSettingScreen(MainForm _MainForm, CPageController _PageController, CLogger _logger, bool _LiveEnable = false)
        {
            MainForm = _MainForm;
            Logger = _logger;
            m_PageController = _PageController;
            m_bLiveEnable = _LiveEnable;
            exceptionDataList = theRecipe.m_listExceptionData;
            InitializeComponent();
            TopLevel = false;

            InitializeForm();

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; ++i)
            {

                Frames.Add(new CLiveImageControl(MainForm)
                {
                    Id = i,
                });

                //240205 LYK Main Display
                LiveDisplayPanel.Controls.Add(Frames[i]);

                Frames[i].SetCamera(theMainSystem.Cameras[i], 0);

            }

            for (int i = 0; i < DEF_SYSTEM.LIGHT_MAX_PAGE; i++)  //240311 LYK TextChanged Event Ãß°¡
            {
                this.Controls.Find($"txtCh{i + 1}", true)[0].TextChanged += Txt_Changed_Event;
            }

            txtMaxPage.TextChanged += Txt_Changed_Event;    //240311 LYK TextChanged Event Ãß°¡

            this.FormLocation(0);


            DoLoad();

            MainForm.SetEachControlResize(this);    //240801 NIS Conrol Resize
            InitializeUI();

            InitializePlot();

            theMainSystem.RefreshHistogramPlot = HistogramPlotRefresh;

            this.FormLocation(0);
        }

        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.MAIN_POS_X, DEF_UI.MAIN_POS_Y);
            this.Size = new Size(DEF_UI.MAIN_SIZE_WIDTH, DEF_UI.MAIN_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void FormSettingScreen_Load(object sender, EventArgs e)
        {
            rdbPage1.Checked = true;    //240311 LYK rdbPage1 ¶óµð¿À ¹öÆ° Check
            rdbPage1.PerformClick();    //240311 LYK °­Á¦ Å¬¸¯
        }

        private void InitializeUI()
        {
            txtImgHeight.Text = theRecipe.CameraRecipe.ImageHeight.ToString();
            tb_Expos_Time.Text = theRecipe.CameraRecipe.Exposure1.ToString();

            tb_FilteringTime.Text = theRecipe.CameraRecipe.Trigger2_Encoder_FilteringTimeWidth.ToString();
            tb_PulseIgnoreCount.Text = theRecipe.CameraRecipe.Trigger2_Encoder_IgnoreCount.ToString();
            tb_FrequencyDivision.Text = theRecipe.CameraRecipe.Trigger2_Divide.ToString();
            tb_InputMultiPlier.Text = theRecipe.CameraRecipe.Trigger2_InputMultiple.ToString();

            cb_LedMode.SelectedItem = theRecipe.CameraRecipe.LedMode.ToString();
            cb_Trig1_Mode.SelectedItem = theRecipe.CameraRecipe.Trigger1Source.ToString();
            cb_Trig2Mode.SelectedItem = theRecipe.CameraRecipe.Trigger2Source.ToString();
            cb_Trig2CountMode.SelectedItem = theRecipe.CameraRecipe.EncoderCountMode.ToString();
            cb_Trig2Direction.SelectedItem = theRecipe.CameraRecipe.EncoderTravelMode.ToString();

            chb_Trig2_Enable.Checked = theRecipe.CameraRecipe.Trigger2Enabled == InsCHVS_FuncEnable.Ins_Enable;
            chb_InputMultiplier_Enable.Checked = theRecipe.CameraRecipe.Trigger2_Enable_InputMultiple == InsCHVS_FuncEnable.Ins_Enable;

            tb_Trig_Frequency.Text = theRecipe.CameraRecipe.TrigPeriod.ToString();
        }

        private void InitializePlot()
        {
            int Width = (int)theMainSystem.Cameras[0].m_Camera.m_nImgWidth;

            PlotXaxis = new double[Width + 1];

            for (int i = 0; i < Width + 1; i++)
                PlotXaxis[i] = i;

            formsPlot.Plot.SetAxisLimitsY(0, 255);
            formsPlot.Plot.SetAxisLimitsX(0, Width + 1);

            formsPlot.Plot.Style(figureBackground: Color.Transparent, dataBackground: Color.White, grid: Color.Transparent, tick: Color.Transparent, axisLabel: Color.Transparent);

            formsPlot.Refresh();
        }

        private void HistogramPlotRefresh(double[] _Values)
        {
            this.Invoke(new Action(delegate ()
            {

                try
                {
                    //Plot 초기화
                    formsPlot.Plot.Clear();

                    formsPlot.Plot.AddScatter(PlotXaxis, _Values, Color.Red, 1, 0, MarkerShape.none, LineStyle.Solid);

                    formsPlot.Plot.SetAxisLimitsY(0, 255);
                    formsPlot.Plot.SetAxisLimitsX(0, _Values.Length);
                    formsPlot.Plot.Style(figureBackground: Color.Transparent, dataBackground: Color.White, grid: Color.Transparent, tick: Color.Transparent, axisLabel: Color.Transparent);
                    //Plot 갱신
                    formsPlot.Refresh();
                }
                catch
                {

                }

            }));
        }

        private void chb_Trig2_Enable_CheckedChanged(object sender, EventArgs e)
        {
            cb_Trig2Mode                .Enabled = chb_Trig2_Enable.Checked;
            cb_Trig2CountMode           .Enabled = chb_Trig2_Enable.Checked;
            cb_Trig2Direction           .Enabled = chb_Trig2_Enable.Checked;
            tb_FilteringTime            .Enabled = chb_Trig2_Enable.Checked;
            tb_PulseIgnoreCount         .Enabled = chb_Trig2_Enable.Checked;
            tb_FrequencyDivision        .Enabled = chb_Trig2_Enable.Checked;
            tb_InputMultiPlier          .Enabled = chb_Trig2_Enable.Checked;
            chb_InputMultiplier_Enable  .Enabled = chb_Trig2_Enable.Checked;
        }
        #region button Event

        // °èÃø ÀÌ¹ÌÁö Å¬¸¯
        private void btn_AcquireImage_Click(object sender, EventArgs e)
        {

        }

        // Live On
        private void btn_LiveOn_Click(object sender, EventArgs e)
        {
            if (m_bLiveStop == false)
            {
                m_bLiveStop = true;
                btn_LiveOn.Enabled = false;
                btn_CamSetApplySave.Enabled = false;
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "CamScreen Live Timer Start");
                theMainSystem.DoGrabLiveStart();
            }
        }

        //240830 LYK Sequence Camera Live Timer Event
        private void LiveTimer_Tick(object sender, EventArgs e)
        {
            
            theMainSystem.DoGrabLiveStart();
        }

        // Live Off
        private void btn_LiveOff_Click(object sender, EventArgs e)
        {
            // Á¶¸í ÃÊ±âÈ­ 
            theMainSystem.DoInspectionStop();
            m_bLiveStop = false;
            btn_LiveOn.Enabled = true;
            btn_CamSetApplySave.Enabled = true;
        }

        // Ä«¸Þ¶ó ¼ÂÆÃ°ª Àû¿ë
        private void btn_CamSetApplySave_Click(object sender, EventArgs e)
        {
                
            DoCameraSetSave();
            //theMainSystem.SetCameraData();

            //m_PageController.SetPageParameter();

            //for (int i = 0; i < DEF_SYSTEM.LIGHT_MAX_PAGE; i++)
            //    theRecipe.LightValue[i].MaxPage = int.Parse(txtMaxPage.Text);   //240311 LYK MaxPageSetting
            theRecipe.DataSave(theRecipe.m_sCurrentModelName);
                
        }

        #endregion

        private void btn_PageController_Click(object sender, EventArgs e)
        {
            //CMainFrame.MainFrame.PageControllerScreen.Show(); //220502 LYK ÀÓ½Ã ÁÖ¼®
        }

        private void FormLocation(int _type)
        {
            //Rectangle fullSize = new Rectangle(0, 0, GrayImageDisplay.Width, GrayImageDisplay.Height);//FullPanel.Bounds;
            Rectangle fullSize = new Rectangle(0, 0, LiveDisplayPanel.Width, LiveDisplayPanel.Height);
            Rectangle[] bounds = new Rectangle[DEF_SYSTEM.CAM_MAX_COUNT]; //241211 NWT Mono 5번 카메라 출력을 위한 bounds 개수 추가

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; ++i)
            {
                bounds[i].X = 0;
                bounds[i].Y = 0;

                bounds[i].Width = fullSize.Width;
                bounds[i].Height = fullSize.Height;


                //241211 NWT 프로그램 실행 시 Cam 1~4 Display
                Frames[i].Bounds = bounds[i];
                Frames[i].Show();
            }
        }

        private void DoLoad()
        {
            //24.02.19 LYK 복잡 했던 내용 수정
            //for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            //{
            //    this.Controls.Find($"txt_Exposure_Cam{i + 1}", true)[0].Text = theRecipe.m_dExposureTime[i].ToString();
            //    this.Controls.Find($"txt_Gain_Cam{i + 1}", true)[0].Text = theRecipe.m_dGain[i].ToString();
            //}

            //241016 NIS MaxPage 초과하는 Page는 숨김처리
            //250103 NWT Page Test
            //for (int i = 1; i <= DEF_SYSTEM.LIGHT_MAX_PAGE; i++)
            //{
            //    int currentMaxPage = int.Parse(txtMaxPage.Text);
            //    if (i > currentMaxPage)
            //        this.Controls.Find($"rdbPage{i}", true)[0].Visible = false;
            //}

        }
        private void DoCameraSetSave()
        {
            theRecipe.CameraRecipe.ImageHeight = uint.Parse(txtImgHeight.Text);
            theRecipe.CameraRecipe.LedMode = (InsCHVS_LED_TriggerMode)Enum.Parse(typeof(InsCHVS_LED_TriggerMode), cb_LedMode.SelectedItem.ToString());
            theRecipe.CameraRecipe.Exposure1 = float.Parse(tb_Expos_Time.Text);

            theRecipe.CameraRecipe.TrigPeriod = float.Parse(tb_Trig_Frequency.Text);

            theRecipe.CameraRecipe.Trigger2Source = (InsCHVS_LineTriggerSource)Enum.Parse(typeof(InsCHVS_LineTriggerSource), cb_Trig1_Mode.SelectedItem.ToString());
            theRecipe.CameraRecipe.Trigger2Enabled = chb_Trig2_Enable.Checked ? InsCHVS_FuncEnable.Ins_Enable : InsCHVS_FuncEnable.Ins_Disable;
            theRecipe.CameraRecipe.Trigger2Source = (InsCHVS_LineTriggerSource)Enum.Parse(typeof(InsCHVS_LineTriggerSource), cb_Trig2Mode.SelectedItem.ToString());
            theRecipe.CameraRecipe.EncoderCountMode = (InsCHVS_Enocder_CountMode)Enum.Parse(typeof(InsCHVS_Enocder_CountMode), cb_Trig2CountMode.SelectedItem.ToString());
            theRecipe.CameraRecipe.EncoderTravelMode = (InsCHVS_Encoder_TriggerMode)Enum.Parse(typeof(InsCHVS_Encoder_TriggerMode), cb_Trig2Direction.SelectedItem.ToString());

            theRecipe.CameraRecipe.Trigger2_Encoder_FilteringTimeWidth = float.Parse(tb_FilteringTime.Text);
            theRecipe.CameraRecipe.Trigger2_Encoder_IgnoreCount = uint.Parse(tb_PulseIgnoreCount.Text);
            theRecipe.CameraRecipe.Trigger2_Divide = uint.Parse(tb_FrequencyDivision.Text);
            theRecipe.CameraRecipe.Trigger2_InputMultiple = uint.Parse(tb_InputMultiPlier.Text);
            theRecipe.CameraRecipe.Trigger2_Enable_InputMultiple = chb_InputMultiplier_Enable.Checked ? InsCHVS_FuncEnable.Ins_Enable : InsCHVS_FuncEnable.Ins_Disable;

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                theMainSystem.Cameras[i].m_Camera.CameraSetting(theRecipe.CameraRecipe);
            }

        }
        
        /// <summary>
        /// 24.03.11 LYK PageDataLoad
        /// ÆäÀÌÁö ÄÁÆ®·Ñ·¯ÀÇ Ã¤³Î µ¥ÀÌÅÍ¸¦ ÇÒ´çÇÑ´Ù.
        /// </summary>
        //theRecipe.LightValue[_nPage].m_nLightValue[i]
        private void PageDataLoad(int _nPage)
        {
            for(int i = 0; i < DEF_SYSTEM.LIGHT_MAX_PAGE; i++)
            {
                this.Controls.Find($"txtCh{i + 1}", true)[0].Text = (theRecipe.LightValue[_nPage].m_nLightValue[i] * DEF_SYSTEM.LIGHT_UNIT).ToString();
            }

        }

        private void radioButtonClick(object sender, EventArgs e)
        {
            RadioButton radio = (RadioButton)sender;

            /*
            switch (radio.Name)
            {
                case "radio0_R":
                    theMainSystem.m_nLiveIndexSection = 1; // R
                    break;

                case "radio0_G":
                    theMainSystem.m_nLiveIndexSection = 2; // G
                    break;

                case "radio0_B":
                    theMainSystem.m_nLiveIndexSection = 3; // B
                    break;

            }
            //241209 NWT Setting Cam screen Cam5 Display용 조건 추가
            switch (radio.Name)
            {
                case "radio_Cam1_4":
                    theMainSystem.m_nLiveCamIndex = 0; // Cam 1 - 4
                    FormshowChanged(theMainSystem.m_nLiveCamIndex);
                    break;

                case "radio_Cam5":
                    theMainSystem.m_nLiveCamIndex = 1; // Cam 5
                    FormshowChanged(theMainSystem.m_nLiveCamIndex);
                    break;
            }
            */

            for (int i = 0; i < DEF_SYSTEM.LIGHT_MAX_PAGE; i++)
            {
                if (radio.Name.Contains($"{i + 1}"))
                {
                    m_nSelectedPage = i;
                    PageDataLoad(i);

                    break;
                }
            }

        }

        private void btn_LightModeOff_Click(object sender, EventArgs e)
        {
        }

        private void btn_LightTargetApply_Click(object sender, EventArgs e)
        {
            //double _tar = Convert.ToDouble(txt_LightCorrectionTarget.Text);
            //
            //for (int i = 0; i < theRecipe.m_nMaxCamCount; ++i)
            //{
            //    theMainSystem.Cameras[i].m_LightCorrect.m_dTargetContrast = _tar;
            //
            //}
        }

        private void FormSettingScreen_Activated(object sender, EventArgs e)
        {
            DoLoad();
        }

        private void btnDataSend_Click(object sender, EventArgs e)
        {

            m_PageController.SetPageParameter();

            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Page Controller Data Send Button Click");

        }

        /// <summary>
        /// 24.03.11 LYK Txt_Changed_Event
        /// ÅØ½ºÆ®ÀÇ °ªÀÌ º¯°æµÈ °æ¿ì ·¹½ÃÇÇ ÆÄ¶ó¹ÌÅÍ¿¡ ÇØ´ç°ªÀ» ÀúÀåÇÑ´Ù.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Txt_Changed_Event(object sender, EventArgs e)
        {
            TextBox GetTextBox = (TextBox)sender;
            string tempText = GetTextBox.Text;
            string controlname = Regex.Replace(GetTextBox.Name, @"\d", "");
            for (int i = 0; i < exceptionDataList.Count; i++)
            {
                if (exceptionDataList[i].Tag.Contains(controlname))
                {
                    if (string.IsNullOrEmpty(tempText) == false)
                    {
                        using (parameterExceptionHandler = new ParameterExceptionHandler(exceptionDataList[i].Page, exceptionDataList[i].Name, exceptionDataList[i].Tag, exceptionDataList[i].Min, exceptionDataList[i].Max, exceptionDataList[i].DataType, tempText))
                        {
                            GetTextBox.Text = (parameterExceptionHandler.CheckData()) ? tempText : previousText;
                            int temp = int.Parse(Regex.Replace(GetTextBox.Name, @"\D", ""));
                            theRecipe.LightValue[m_nSelectedPage].m_nLightValue[temp - 1] = (int)(double.Parse(GetTextBox.Text) / DEF_SYSTEM.LIGHT_UNIT);
                            break;
                            
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 24.12.11 NWT Light Page parameter Cilck Event
        /// 조명 On-time 파라미터 클릭 시 Keypad 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_Click_Event(object sender, EventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            string strCurrent = "", strModify = "";

            strCurrent = txtBox.Text;

            if (!MainForm.GetKeyPad(strCurrent, out strModify))
                return;
            string controlname = Regex.Replace(txtBox.Name, @"\d", "");
            for (int i = 0; i < exceptionDataList.Count; i++)
            {
                if (exceptionDataList[i].Tag.Contains(controlname))
                {
                    using (parameterExceptionHandler = new ParameterExceptionHandler(exceptionDataList[i].Page, exceptionDataList[i].Name, exceptionDataList[i].Tag, exceptionDataList[i].Min, exceptionDataList[i].Max, exceptionDataList[i].DataType, strModify))
                    {
                        txtBox.Text = parameterExceptionHandler.CheckData() ? strModify : strCurrent;
                        int temp = int.Parse(Regex.Replace(txtBox.Name, @"\D", ""));
                        theRecipe.LightValue[m_nSelectedPage].m_nLightValue[temp - 1] = (int)(double.Parse(strModify) / DEF_SYSTEM.LIGHT_UNIT);
                        break;
                    }
                }
            }
        }

        private void btnTrigger_Click(object sender, EventArgs e)
        {
            m_PageController.Trigger(m_nSelectedPage);
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
            string controlname = Regex.Replace(tempTxtBox.Name, @"\d", "");
            if (!MainForm.GetKeyPad(strCurrent, out strModify))
                return;

            for (int i = 0; i < exceptionDataList.Count; i++)
            {
                if (exceptionDataList[i].Tag.Contains(controlname))
                {
                    using (parameterExceptionHandler = new ParameterExceptionHandler(exceptionDataList[i].Page, exceptionDataList[i].Name, exceptionDataList[i].Tag, exceptionDataList[i].Min, exceptionDataList[i].Max, exceptionDataList[i].DataType, strModify))
                    {
                        tempTxtBox.Text = parameterExceptionHandler.CheckData() ? strModify : previousText;
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
            string controlname = Regex.Replace(GetTextBox.Name, @"\d", "");
            for (int i = 0; i < exceptionDataList.Count; i++)
            {
                if (exceptionDataList[i].Tag.Contains(controlname))
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

        
    }

}