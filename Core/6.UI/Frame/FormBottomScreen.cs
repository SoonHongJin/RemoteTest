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
using static Core.DEF_UI;
using static Core.Program;

namespace Core.UI
{

    public partial class FormBottomScreen : Form
    {
        private MainForm MainForm = null;
        private EFormType CurrentPage = EFormType.AUTO;
        private List<Panel> ErrorPanels = new List<Panel>();
        private List<Label[]> ErrorLabels = new List<Label[]>();

        private int CellIndex = 0;
        private int tempCount = 0;
        private int count = 0;
        public enum BottomImg
        {
            NG = 0
        }


        //20240510_나인성
        private string[] NGDataInfo = new string[20];

        private List<string> DataInfo = new List<string>();

        public FormBottomScreen(MainForm _MainForm)
        {
            InitializeComponent();
            InitializeForm();
            MainForm = _MainForm;
            this.TopLevel = false;

            MainForm.SetEachControlResize(this);    //240731 NIS Conrol Resize

            this.Show();

            theMainSystem.RefreshHisotryData = ErrorImageRefresh;  //임시 주석
            SetDefectForm();
            //나인성
            SetNGDataInfo(theRecipe.m_sLastNGData);

        }

        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            //this.DesktopLocation = new Point(DEF_UI.BOT_POS_X, DEF_UI.BOT_POS_Y);
            this.Size = new Size(1920, 100);
            this.FormBorderStyle = FormBorderStyle.None;

        }
        private void SetDefectForm()  //제어를 위해 Controls을 List에 저장
        {
            //foreach (Panel panel in this.panelFrame.Controls)
            //{
            //    panel.BackColor = Color.Black;
            //    ErrorPanels.Add(panel);
            //}

            //ErrorLabels.Add(new Label[] { this.lblNGTime_0_Date, this.lblNGTime_0_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_1_Date, this.lblNGTime_1_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_2_Date, this.lblNGTime_2_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_3_Date, this.lblNGTime_3_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_4_Date, this.lblNGTime_4_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_5_Date, this.lblNGTime_5_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_6_Date, this.lblNGTime_6_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_7_Date, this.lblNGTime_7_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_8_Date, this.lblNGTime_8_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_9_Date, this.lblNGTime_9_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_10_Date, this.lblNGTime_10_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_11_Date, this.lblNGTime_11_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_12_Date, this.lblNGTime_12_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_13_Date, this.lblNGTime_13_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_14_Date, this.lblNGTime_14_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_15_Date, this.lblNGTime_15_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_16_Date, this.lblNGTime_16_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_17_Date, this.lblNGTime_18_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_18_Date, this.lblNGTime_18_Time });
            //ErrorLabels.Add(new Label[] { this.lblNGTime_19_Date, this.lblNGTime_19_Time });

            for(int i = 0; i < 20; i ++)
            {
                Label lblDate = this.Controls.Find($"lblNGTime_{i}_Date", true)[0] as Label;
                Label lblTime = this.Controls.Find($"lblNGTime_{i}_Time", true)[0] as Label;
                Panel pnlNG = this.Controls.Find($"panelNG_{i}", true)[0] as Panel;

                lblDate.Text = "-";
                lblTime.Text = "-";

                ErrorLabels.Add(new Label[] { lblDate, lblTime });

                pnlNG.BackColor = Color.Black;
                ErrorPanels.Add(pnlNG);

                ErrorPanels[i].Click += PanelNG_Click;
                ErrorLabels[i][0].Click += PanelNG_Click;   //240831 NWT Label 클릭해도 Log Display되도록 추가
                ErrorLabels[i][1].Click += PanelNG_Click;
            }


        }

        public void SelectPage(EFormType index)
        {
            CurrentPage = index;

            MainForm?.DisplayManager.FormSelectChange(index);

        }

        public void ErrorImageRefresh(InspectionInfo _Info)    //오류 이미지 갱신하기
        {
            this.Invoke(new MethodInvoker(delegate ()
            {
                //if (count < 10)  //오류 이미지, 라벨, 이벤트 할당
                //{
                //    ErrorPanels[count].BackgroundImage = BottomImgList.Images[(int)BottomImg.NG];   //241007 NWT BottomImgList 추가
                //    ErrorPanels[count].Click += PanelNG_Click;

                //    ErrorLabels[count][0].Text = _Info.EndTime.ToString("yyyy") + _Info.EndTime.ToString("MM") + _Info.EndTime.ToString("dd") + "\n" + _Info.EndTime.ToString("HH") + ":"
                //                                    + _Info.EndTime.ToString("mm") + ":" + _Info.EndTime.ToString("ss");

                //    ErrorLabels[count][1].Text = _Info.ID;

                //    ErrorLabels[count][0].Click += PanelNG_Click;   //240831 NWT Label 클릭해도 Log Display되도록 추가
                //    ErrorLabels[count][1].Click += PanelNG_Click;

                //    //나인성
                //    NGDataInfo[count] = _Info.EndTime.ToString("yyyyMMdd\nHH:mm:ss") + "_" + _Info.ID;

                //    count++;
                //}
                //else  //데이터 이동 작업
                //{
                //    for (int i = 0; i < ErrorPanels.Count - 1; i++)
                //    {
                //        ErrorLabels[i][0].Text = ErrorLabels[i + 1][0].Text;
                //        ErrorLabels[i][1].Text = ErrorLabels[i + 1][1].Text;

                //        //나인성
                //        NGDataInfo[i] = NGDataInfo[i + 1];
                //    }
                //    ErrorLabels[ErrorPanels.Count - 1][0].Text = _Info.EndTime.ToString("yyyyMMdd\nHH:mm:ss");
                //    ErrorLabels[ErrorPanels.Count - 1][1].Text = _Info.ID;
                //    //나인성
                //    NGDataInfo[ErrorPanels.Count - 1] = _Info.EndTime.ToString("yyyyMMdd\nHH:mm:ss") + "_" + _Info.ID;
                //}

                string StartTime = _Info.EndTime.ToString("yyyy") + _Info.EndTime.ToString("MM") + _Info.EndTime.ToString("dd") + "\n" + _Info.EndTime.ToString("HH") + ":"
                + _Info.EndTime.ToString("mm") + ":" + _Info.EndTime.ToString("ss") + "." + _Info.EndTime.ToString("fff");
                string ID = _Info.ID;

                string Data = StartTime + "_" + ID;

                DataInfo.Add(Data);

                if (ErrorPanels.Count < DataInfo.Count)
                    DataInfo.RemoveAt(0);

                int idx = 0;
                for (int i = DataInfo.Count - 1; i >= 0; i--)
                {
                    if (DataInfo.Count <= ErrorPanels.Count)
                        ErrorPanels[idx].BackgroundImage = BottomImgList.Images[(int)BottomImg.NG];

                    string[] TimeID = DataInfo[i].Split('_');

                    ErrorLabels[idx][0].Text = TimeID[0];
                    ErrorLabels[idx][1].Text = TimeID[1];

                    idx++;
                }

                //this.Update();
            }));
        }

        private void PanelNG_Click(object sender, EventArgs e)      //NG 셀 정보 출력
        {
            Control control = (Control)sender;
            int index = int.Parse(control.Name.Split('_')[1]);  //어떤 위치에 있는 컨트롤을 클릭했는지 확인

            if(ErrorLabels[index][0].Text.Length > 2 && ErrorLabels[index][1].Text.Length > 2) // 문자열 길이가 2자 이상 일 경우 진입 
            {
                string sDate = ErrorLabels[index][0].Text.Split('\n')[0];
                string sWaferID = ErrorLabels[index][1].Text;

                //나인성
                if (sWaferID == "")
                    MessageBox.Show("ID None");
                else
                {
                    SelectPage(EFormType.HISOTRY_CURRENT_DATA);
                    //MainForm.CurrentHisotryData.SetDefectData(sDate, sWaferID);   임시 주석
                }
            }
        }

        public string[] GetNGDataInfo()
        {
            return NGDataInfo;
        }

        //나인성
        public void SetNGDataInfo(string[] _sNGData)
        {
            /* 임시 주석
            byte NGDataCnt = 0;
            for (; NGDataCnt < _sNGData.Length; NGDataCnt++)
            {
                if (_sNGData[NGDataCnt] == "")
                {
                    break;
                }
            }
            
            for (; count < NGDataCnt; count++)
            {
                string[] splitData = _sNGData[count].Split('_');
                ErrorPanels[count].BackgroundImage = BottomImgList.Images[(int)BottomImg.NG];   //241007 NWT BottomImgList추가
                ErrorPanels[count].Click += PanelNG_Click;

                ErrorLabels[count][0].Text = splitData[0];

                ErrorLabels[count][1].Text = splitData[1];

                ErrorLabels[count][0].Click += PanelNG_Click;       //240831 NWT Label 클릭해도 Log Display되도록 추가
                ErrorLabels[count][1].Click += PanelNG_Click;

                NGDataInfo[count] = _sNGData[count];
            }
            */
        }
    }
}
