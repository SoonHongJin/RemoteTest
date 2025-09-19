using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using System.Runtime.InteropServices;

using static Core.Program;
using Core.Utility;
using Core.DataProcess;
using Core.Process;

namespace Core.UI
{
    public partial class FormSideScreen : Form//, IMessageFilter
    {
        private MainForm MainForm = null;
        private CProgressBar[] ProgressBar = new CProgressBar[4];
        private List<Panel> DeviceListPanel = new List<Panel>();
        private CHardWareUsage HardWareUsage = new CHardWareUsage();
        private Point lastMousePosition;
        private CInterfacePLC InterfacePLC = null;  //250403 LYK Interface PLC 객체

        public Stopwatch stopwatch;
        public TimeSpan LogoutTime = TimeSpan.FromSeconds(600); //240704 KCH 로그아웃 타임아웃 시간 설정
        public bool isLogout;

        private CLogger Logger;


        public FormSideScreen(MainForm _mainForm, CLogger _logger, CInterfacePLC _InterfacePLC)
        {
            InitializeComponent();
            InitializeForm();
            this.TopLevel = false;
            MainForm = _mainForm;
            Logger = _logger;
            InterfacePLC = _InterfacePLC;

            theMainSystem.RefreshModelComboList = RefreshModelList;
            theMainSystem.RefreshCurrentModelName = DisplayCurrentModelName;
            UpdateStatus(theRecipe.m_sCurrentModelName);
            RefreshModelList();

            RefreshDeviceStatus();

            for (int i = 0; i < 4; i++)
            {
                ProgressBar[i] = new CProgressBar();
                ProgressBar[i].Width = DEF_UI.ProgressPCDataWidth;
                ProgressBar[i].Height = DEF_UI.ProgressPCDataHeight;
                ProgressBar[i].Location = new Point(DEF_UI.ProgressPoint[i + 1].X, DEF_UI.ProgressPoint[i + 1].Y);
                //ProgressBar[i].ForeColor = Color.Blue;
                //ProgressBar[i].Style = ProgressBarStyle.Continuous;
                Controls.Add(ProgressBar[i]);
            }

            StatusTimer.Start();
            MouseTimer.Start();  //240704 KCH 마우스 움직임 감지 타이머 시작

            lastMousePosition = Cursor.Position; //240704 KCH 마우스 포지션 저장

            stopwatch = new Stopwatch(); 

            lbl_Stopwatch.Visible = false; //240704 KCH 시간표시 UI 숨김

            MainForm.SetEachControlResize(this);    //240731 NIS Conrol Resize
            this.Show();
        }
        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            //this.DesktopLocation = new Point(DEF_UI.BOT_POS_X, DEF_UI.BOT_POS_Y);
            this.Size = new Size(DEF_UI.BOT_SIZE_WIDTH, DEF_UI.BOT_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;
        }
        public void UpdateStatus(string _sModelName)
        {
            txt_Vision_Model.Text = _sModelName;
        }

        private void RefreshModelList()
        {
            cbb_ModelList.Items.Clear();

            string[] fileEntries = Directory.GetDirectories(DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\");
            fileEntries.OrderBy(ss => new FileInfo(ss).Name);

            foreach (string filePathName in fileEntries)
            {
                string fileName = Path.GetFileName(filePathName);
                cbb_ModelList.Items.Add(fileName);
            }
            cbb_ModelList.SelectedIndex = 0;
        }

        public void ChangeResult(string sJudge)
        {
            this.Invoke(new MethodInvoker(delegate ()
            {
                if (sJudge == "NG")
                {
                   lblJudegeMent.ForeColor = Color.Red;
                   lblJudegeMent.Text = "NG";
                }
                else if(sJudge == "OK")
                {
                    lblJudegeMent.ForeColor = Color.Green;
                    lblJudegeMent.Text = "OK";
                }
                else
                {
                    lblJudegeMent.ForeColor = Color.Green;
                    lblJudegeMent.Text = "OK";
                }
                this.Update();
            }));
        }

        private void btn_CreateModel_Click(object sender, EventArgs e)
        {
            CreateModel CreateMdoelScreen = new CreateModel(MainForm.ParamerterModelForm, 0, Logger);
            CreateMdoelScreen.m_nModelMode = 0;
            DialogResult dialogResult = CreateMdoelScreen.ShowDialog();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            string sSelectedItem = Convert.ToString(cbb_ModelList.SelectedItem);

            if (sSelectedItem == theRecipe.m_sCurrentModelName)
            {
                MessageBox.Show("matches the current model");
            }
            else
            {
                string sModelFilePath = DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\" + sSelectedItem;

                System.IO.DirectoryInfo dirRecipe = new System.IO.DirectoryInfo(sModelFilePath);
                dirRecipe.Delete(true);    // false로 할 경우 하위 폴더와 폴더 내의 파일들은 삭제가 안된다.

                theMainSystem.RefreshModelList?.Invoke();
                theMainSystem.RefreshModelComboList?.Invoke();
            }
        }

        private void btn_ModelChange_Click(object sender, EventArgs e)
        {

        }

        private void RefreshDeviceStatus()
        {
            DeviceListPanel.Add(pnl_Cam1Status);
            DeviceListPanel.Add(pnl_Cam2Status);
            DeviceListPanel.Add(pnl_Cam3Status);
            DeviceListPanel.Add(pnl_Cam4Status);
            DeviceListPanel.Add(pnl_Cam5Status);    //240621 LYK 수정

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                if (theMainSystem.Cameras[i].m_Camera.IsConnected())
                    DeviceListPanel[i].BackColor = Color.Lime;
                else
                    DeviceListPanel[i].BackColor = Color.Red;
            }
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            double CPU = 0.0, RAM = 0.0, GPU = 0.0;
            double[] HDD = { 0.0, 0.0 };

            HardWareUsage.GetHardWareData(ref CPU, ref RAM, ref HDD, ref GPU);

            if (CPU > 100)
                CPU = 100.0;

            ProgressBar[0].Value = (int)CPU;
            ProgressBar[0].sValue = CPU.ToString("F2");
            ProgressBar[0].ProgressColor = Color.FromArgb(116, 182, 102);

            ProgressBar[1].Value = (int)RAM;
            ProgressBar[1].sValue = RAM.ToString("F2");
            ProgressBar[1].ProgressColor = Color.FromArgb(116, 182, 102);

            //if(RAM >= 38)
            //    GC.Collect(2, GCCollectionMode.Forced, false, true);

            ProgressBar[2].Value = (int)HDD[0];
            ProgressBar[2].sValue = HDD[0].ToString("F2");
            ProgressBar[2].ProgressColor = Color.FromArgb(116, 182, 102);

            ProgressBar[3].Value = (int)HDD[1];
            ProgressBar[3].sValue = HDD[1].ToString("F2");
            ProgressBar[3].ProgressColor = Color.FromArgb(116, 182, 102);
            
            /*  임시 주석
            if(HDD[1] > theRecipe.m_dDriveLimit && theRecipe.m_nAutoDeletechk == 1)
            {
                theMainSystem.m_ThreadProcessDelete.Continue();
            }
            */

            // SHJ 에이징 cpu 부하 체크위해 임시 사용 
            if(CPU > 70)
            {
                Task.Run(() => theMainSystem.ScreenCapture(CPU));
            }

            RefreshUI();

            // 20250917 SHJ 드라이브 용량 체크를 위해 자동 삭제 기능 이곳에 위치
            theMainSystem.AutoDelete(HDD[1]);
        }

        public void RefreshUI() // 250118 NWT Mes 상태에 따라 UI를 갱신, 수정 필요 250224 통신 연결상태 추가
        {
            //250320 NWT 설정된 레시피에 따라 알맞은 통신 Status 출력
            CheckPLCStatus();

        }

        private void CheckPLCStatus()
        {
            /*  임시 주석
            lbl_Communication.Text = "PLC";
            if (!theMainSystem.InterfacePLC.m_PLC.GetPLCConnectedCheck())
            {
                if (theMainSystem.InterfacePLC.m_PLC.m_bFirstDisC == true)  
                    theMainSystem.InterfacePLC.Disconnect();

                pnl_Communication.BackColor = Color.Red;
                //250225 NWT Setting Page의 PLC Disconnect 버튼을 눌러 연결 해제시 자동으로 연결 안하도록 조건 설정
                if (!theMainSystem.InterfacePLC.m_PLC.m_bDisconnect)
                {
                    Task.Run(() =>
                    {
                        //250225 NWT 재연결 시도가 완료될때까지 넘어가도록 조건 추가
                        if (!theMainSystem.InterfacePLC.m_bReconnectRun)
                        {
                            theMainSystem.InterfacePLC.m_bReconnectRun = true;
                            theMainSystem.InterfacePLC.Reconnect();
                        }
                    });
                }
            }
            else
            {
                pnl_Communication.BackColor = Color.Lime;
            }
            */
        }

        private void DisplayCurrentModelName(string _sModelName)
        {
            this.Invoke(new MethodInvoker(delegate ()
            {
                txt_Model_No.Text = _sModelName;

                this.Update();
            }));
        }

        private void btn_DataBase_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string sFolderPath = string.Empty;
            if (btn.Name == "btn_MainFolder")
            {
                sFolderPath = DEF_SYSTEM.DEF_FOLDER_PATH_PROJECT_NAME;
            }
            else if (btn.Name == "btn_Log")
            {
                sFolderPath = DEF_SYSTEM.DEF_FOLDER_PATH_LOG;
            }
            else if (btn.Name == "btn_ResultData")
            {
                sFolderPath = DEF_SYSTEM.DEF_FOLDER_PATH_CSV; ;
            }
            else if (btn.Name == "btn_Screen_Camera")
            {
                sFolderPath = DEF_SYSTEM.DEF_FOLDER_PATH_SCREEN;
                ScreenCapture(DEF_UI.MAIN_SIZE_WIDTH, DEF_UI.MAIN_SIZE_HEIGHT, Screen.AllScreens[0].WorkingArea.Location, sFolderPath);

            }
            else if (btn.Name == "btn_SOPManual")
            {

                sFolderPath = DEF_SYSTEM.DEF_FOLDER_PATH_MANUAL;
            }

            System.Diagnostics.Process.Start(sFolderPath);
        }

        /// <summary>
        /// 2024.12.13 NWT Screen Capture
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pt"></param>
        public void ScreenCapture(int width, int height, Point pt, string path)
        {
            Bitmap bitmap = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(pt, new Point(0, 0), new Size(width, height));
            }

            DirectoryInfo di = new DirectoryInfo(path);
            if (di.Exists == false)
            {
                di.Create();
            }
            
            path += "\\" + DateTime.Now.ToString("yyMMdd_HHmmssfff") + ".png";

            bitmap.Save(path);
        }

        public void SetAccess(string Access, string ID)
        {
            /*  임시 주석
            theMainSystem.AccessLevel = Access;
            theMainSystem.CurrentID = ID;
            */
        }

        public void SetUserInfo(string Id, string Access, string Company) //240704 KCH Login 정보 출력
        {
            lbl_UserID_Print.Text = Id;
            lbl_Access_Print.Text = Access;
            lbl_Company_Print.Text = Company;
        }

        private void btn_Modify_Click_1(object sender, EventArgs e) //240704 KCH Modify 버튼 클릭시 동작
        {
            /* 임시 주석
            if(theMainSystem.AccessLevel == "Master")
            {
                timer1.Stop();
                stopwatch.Stop();
                lbl_Stopwatch.Text = "10 : 00";
                FormModify formModify = new FormModify();
                formModify.StartPosition = FormStartPosition.Manual;
                formModify.Location = new Point(DEF_UI.MAIN_SIZE_WIDTH / 2, DEF_UI.MAIN_SIZE_HEIGHT / 4); //240704 KCH Modify 팝업창 위치지정
                formModify.Show();
            }
            else
            {
                MessageBox.Show("Please change the Access Level to Master.");
            }
            */

        }

        private void btn_LoginPage_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
            stopwatch.Stop();
            lbl_Stopwatch.Text = "10 : 00";
            /*  임시 주석
            FormLogin formLogin = new FormLogin(this, Logger);
            formLogin.StartPosition = FormStartPosition.Manual;
            formLogin.Location = new Point(DEF_UI.MAIN_SIZE_WIDTH / 2, DEF_UI.MAIN_SIZE_HEIGHT / 4); //240704 KCH Login 팝업창 위치지정
            formLogin.Show();
            */
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (stopwatch.Elapsed >= LogoutTime) //240704 KCH Timer 체크
            {
                stopwatch.Reset();
                PerformLogout();
            }
            else
            {
                TimeSpan timeLeft = LogoutTime - stopwatch.Elapsed;
                string formetTimeLeft = String.Format("{0:D2} : {1:D2}", timeLeft.Minutes, timeLeft.Seconds); //240704 KCH 마우스 미동작시 남은시간
                lbl_Stopwatch.Text = formetTimeLeft;
            }
        }

        private void PerformLogout() //240704 KCH 로그아웃 시 동작
        {
            /* 임시 주석
            isLogout = true;
            theMainSystem.AccessLevel = "Operator";
            SetUserInfo("-", "-", "-");        
            btn_LoginPage.Visible = true;
            lbl_Stopwatch.Visible = false;
            stopwatch.Stop();
            theMainSystem.SetUserAccess(theMainSystem.AccessLevel);

            MainForm?.DisplayManager.FormSelectChange(0);

            */
        }       

        private void MouseTimer_Tick(object sender, EventArgs e) //240704 KCH 마우스 움직임 감지 타이머 (0.001초)
        {
            Point currentMousePosition = Cursor.Position;
            if (currentMousePosition != lastMousePosition && isLogout == false)
            {
                stopwatch.Restart();

                lbl_Stopwatch.Text = "10 : 00";
            }
            lastMousePosition = currentMousePosition;
        }

        private void btn_LogOut_Click(object sender, EventArgs e) //240704 KCH 로그아웃 버튼 클릭 이벤트
        {
            PerformLogout();
        }
        public void Start_MouseTimer()
        {
            MouseTimer.Start();
        }
    }
}
