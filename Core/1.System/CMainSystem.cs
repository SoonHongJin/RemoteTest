

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Management;

using static Core.Program;
using Core.HardWare;
using Core.Function;
using Core.Process;
using Core.Utility;
using Core.UI;

using Cognex.VisionPro;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.ImageFile;

namespace Core
{
    public class CMainSystem
    {
        #region [SingleTon]
        private static CMainSystem Instance = null;

        private CMainSystem()
        {

        }

        public static CMainSystem GetInstance()
        {
            if (null == Instance)
                Instance = new CMainSystem();

            return Instance;
        }
        #endregion

        //----------------------------------------------------------------------------------------------
        // System 관련 변수 
        private readonly object m_Lock = new object();
        private StreamWriter logOutfile = null;
        private List<string> logQueue = new List<string>();

        public Stopwatch m_SegStopWatch = new Stopwatch();
        public Stopwatch m_CycleTimeWatch = new Stopwatch();
        public Stopwatch m_trgtimer = new Stopwatch();


        //----------------------------------------------------------------------------------------------
        // Class & Form 인스턴스 
        public MainForm mainForm = null;
        private CLogger Logger = null;  //250204 LYK Log 함수(SystemLog, InterfaceLog, MESLog) 를 남기는 객체
        public List<CCameraManager> Cameras = new List<CCameraManager>();
        public CInterfacePLC InterfacePLC = null;
        private CMath cMath = new CMath();
        private CImageSaverManager ImageSaverManager = new CImageSaverManager();
        private CDataDeleteManager DataDeleteManager = new CDataDeleteManager();
        //----------------------------------------------------------------------------------------------
        // 구조체 인스턴스 
        public CProductInfo ProductInfo = null;

        //----------------------------------------------------------------------------------------------
        // Thread 인스턴스 
        //private CThread m_ThreadInspectionComplete = null;      //240122 LYK Inspection Complete Thread (딥러닝, 컬러 두께, 컨투어, 크랙 검사가 완료 될때 까지 기다리며, 이벤트가 모두 시그널 되면 DoInspectionComplete 함수가 호출 된다.
        private CThread m_CsvSaveThread = null;                 //240122 LYK Csv Save Thread
        //----------------------------------------------------------------------------------------------
        // 장치 인스턴스                   
        public CTcpServer TcpServer = null;
        public CTcpClient TcpClient = null;
        public CPageController m_PageController = null;

        //----------------------------------------------------------------------------------------------
        // Action                           
        public Action<string> RefreshModel = null;
        public Action RefreshModelList = null;
        public Action RefreshModelComboList = null;
        public Action<string> RefreshCurrentModelName = null;       //230904 LYK 현재 로드된 Model Name Display
        public Action<string> CycleTime = null;                     //240202 LYK CycleTime
        public Action<InspectionInfo, int> RefreshResult = null;    //240202 LYK RefreshResult 하나로 통합(Total Inspection Time, DeepLearning Inspection Time, DT Result, Color 두께)
        public Action<InspectionInfo, int> RefreshScottPlot = null; //240202 LYK Scottplot Graph
        public Action<InspectionInfo, int> RefreshDefectLabel = null; //240202 LYK DeepLearning 관련 Defect List 처리를 위한 Delegate
        public Action<InspectionInfo> RefreshHisotryData = null;
        public Action<InspectionInfo, int> RefreshCropDefecType = null;
        public Action<int> RefreshDisplayIdx = null;    //250409 LYK 임시 Delegate
        public Action<double[]> RefreshHistogramPlot = null;        //250530 SHJ 셋팅 페이지 그래프 그리기 용도 
        //----------------------------------------------------------------------------------------------
        // Camera Manual Event 

        //24.02.19 LYK InspectionCompleteEvents 소터, 프린터, CVD 고려하여 할당 할 수 있도록 수정
        private List<List<List<ManualResetEvent>>> InspectionCompleteEvents;
        //----------------------------------------------------------------------------------------------
        //검사 관련 변수(Grab Cycle, Inspection Total, DeepLearning) - CSV 파일에 저장하기 위한 용도
        public string m_sJudge = string.Empty;


        public InspectionInfo[] InspectionInfos = new InspectionInfo[DEF_SYSTEM.INSP_CNT];   //240202 LYK InspectionInfos 배열(WaferId, InspectionTime 등이 담기며, Idx별로 넘버링 된다.)
        private InspectionInfo Infos = new InspectionInfo();                //240202 LYK Total, DeepLearning, Contour, Crack 검사 시간을 담을 변수
        public int m_CurrrentIdx = 0;  //240313 LYK 검사 정보의 현재 Index

        private List<string> deviceNames = new List<string>();              //240220 LYK PC의 장착된 보드 등을 담을 List

        private TimeSpan timeSpan;      //240313 LYK timeSpan

        public string tempFileName = "";
        private List<string> datas = new List<string>();
        private bool m_bFirst = false;

        private bool m_bLiveStart = false;
        private int m_nDisplayIdx = 0;
        private int m_nSimulIdx = 0;

        private string[][] DeleteFolderPath = new string[4][];


        public void InitializeCore(MainForm _mainform)  //220504 LYK 초기화 함수 MainForm에서 호출
        {

            //----------------------------------------------------------------------------------------------
            // LogSave & Directory Initialize
            //----------------------------------------------------------------------------------------------
            /*DO NOT MODIFY*/
            InitialLogger();                                                //250219 LYK Initial Log
            mainForm = _mainform;                                           //220504 LYK 메인폼 객체 할당               
                                                                            /*DO NOT MODIFY*/
            mainForm.intro.SetStatus("Create Log folder", 10);
                                                //220504 LYK LogFile 저장 및 Open
            CreateDirectory();                                              //220504 LYK System 폴더 생성 필요 

            Thread.Sleep(100);

            //----------------------------------------------------------------------------------------------
            // Vision Library Initialize
            //----------------------------------------------------------------------------------------------
            /*DO NOT MODIFY*/
            mainForm.intro.SetStatus("Library Initial", 20);

            Thread.Sleep(100);

            //----------------------------------------------------------------------------------------------
            // Recipe & System Data Load
            //----------------------------------------------------------------------------------------------
            /*DO NOT MODIFY*/
            mainForm.intro.SetStatus("Load Last Recipe", 30);
            

            theRecipe.InitialLogger(Logger);
            theRecipe.LastRecipeLoad();                                     //220504 LYK LastRecipe Load(Recipe명 )
            theRecipe.DataLoad(theRecipe.m_sCurrentModelName);              //220504 LYK Recipe Data Load
            theRecipe.ToolLoad(theRecipe.m_sCurrentModelName);

            ProductInfo = new CProductInfo();
            //theRecipe.AddressDefineLoad();

            Thread.Sleep(100);

            //----------------------------------------------------------------------------------------------
            // Grabber & ETC Devices Initialize
            //----------------------------------------------------------------------------------------------

            //주석
            //TcpServer = new CTcpServer(Logger);
            //TcpClient = new CTcpClient(Logger);
            //m_PageController = new CPageController(Logger);
            //TcpClient.Initialize(DEF_SYSTEM.LIGHT_PORT, DEF_SYSTEM.LIGHT_CONTROLLER_IP);
            //m_PageController.Initialized(DEF_SYSTEM.LIGHT_COM, DEF_SYSTEM.LIGHT_CONTROLLER_BAUDRATE);

            Thread.Sleep(100);

            //----------------------------------------------------------------------------------------------
            // Camera Initialize
            //----------------------------------------------------------------------------------------------
            /*DO NOT MODIFY*/
            mainForm.intro.SetStatus("Camera Initial", 50);

            InitializeCamera();       //220504 LYK 카메라 초기화
            InitializeThread();

            Thread.Sleep(100);

            //----------------------------------------------------------------------------------------------
            // Network & Interface Initialize
            //----------------------------------------------------------------------------------------------
            /*DO NOT MODIFY*/
            mainForm.intro.SetStatus("Interface Initial", 60);

            InterfacePLC = new CInterfacePLC(mainForm, Logger);
            InterfacePLC.Connect();

            for (int i = 0; i < DEF_SYSTEM.INSP_CNT; i++)
            {
                InspectionInfos[i] = new InspectionInfo();
            }

            RefreshDisplayIdx = SetDisplayIdx;

            System.Diagnostics.Process.GetCurrentProcess().PriorityBoostEnabled = true;           
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;

            Thread.Sleep(100);
            //----------------------------------------------------------------------------------------------
            // Core Program Start
            //----------------------------------------------------------------------------------------------          
            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Program Started!!");
        }

        #region System Function

        public void UnInitialzieCore()    //220504 MainForm에서 호출
        {
            theRecipe.LastRecipeSave(theRecipe.m_sCurrentModelName, theRecipe.m_sLastNGData);

            UnInitializeCamera();     //220504 LYK CameraUninitial       
            UninitialThread();

            // 로그 해제   
            UnInitialLogger();

            ImageSaverManager.Shutdown();
            DataDeleteManager.Shutdown();

            if (null != logOutfile)         //삭제 예정
            {
                //logOutfile.Flush();
                //logOutfile.Close();
                logOutfile.Dispose();
            }

#if PROFIBUS
            InterfaceProfibus.Disconnect();
#endif
        }

        /// <summary>
        /// 25.02.04 LYK InitialLogger 함수
        /// 최초 프로그램 실행 할때 한번 실행 된다. 
        /// System Log, Interface Log, MES Log로 세분 되도록 구성 
        /// </summary>
        private void InitialLogger()
        {
            string[] LogDir = {
                    string.Format("{0}\\{1:0000}\\{2:00}", DEF_SYSTEM.DEF_FOLDER_PATH_LOG, DateTime.Now.Year, DateTime.Now.Month),
                    string.Format("{0}\\{1:0000}\\{2:00}", DEF_SYSTEM.DEF_FOLDER_PATH_INTERFACELOG, DateTime.Now.Year, DateTime.Now.Month),
                    string.Format("{0}\\{1:0000}\\{2:00}", DEF_SYSTEM.DEF_FOLDER_PATH_MESLOG, DateTime.Now.Year, DateTime.Now.Month)
                };

            Logger = new CLogger(LogDir, DEF_SYSTEM.LOGTYPE);
            Logger.StartLogging();
        }

        /// <summary>
        /// 25.02.04 LYK InitialLogger 함수
        /// 프로그램을 종료 할때 실행 된다. 
        /// </summary>
        private void UnInitialLogger()
        {
            Logger.StopLogging();
        }

        public void CreateDirectory()
        {
            if (!Directory.Exists(DEF_SYSTEM.DEF_FOLDER_PATH_IMAGE))
                Directory.CreateDirectory(DEF_SYSTEM.DEF_FOLDER_PATH_IMAGE);

            if (!Directory.Exists(DEF_SYSTEM.DEF_FOLDER_PATH_CSV))
                Directory.CreateDirectory(DEF_SYSTEM.DEF_FOLDER_PATH_CSV);

            if (!Directory.Exists(DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE))
                Directory.CreateDirectory(DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE);

            if (!Directory.Exists(DEF_SYSTEM.DEF_FOLDER_PATH_CALIB))
                Directory.CreateDirectory(DEF_SYSTEM.DEF_FOLDER_PATH_CALIB);

            if (!Directory.Exists(DEF_SYSTEM.DEF_FOLDER_PATH_SCREEN))
                Directory.CreateDirectory(DEF_SYSTEM.DEF_FOLDER_PATH_SCREEN);

            if (!Directory.Exists(DEF_SYSTEM.DEF_FOLDER_PATH_LOG))
                Directory.CreateDirectory(DEF_SYSTEM.DEF_FOLDER_PATH_LOG);

            if (!Directory.Exists(DEF_SYSTEM.DEF_FOLDER_PATH_MANUAL))
                Directory.CreateDirectory(DEF_SYSTEM.DEF_FOLDER_PATH_MANUAL);

            if (!Directory.Exists(DEF_SYSTEM.DEF_FOLDER_PATH_INTERFACELOG))
                Directory.CreateDirectory(DEF_SYSTEM.DEF_FOLDER_PATH_INTERFACELOG);

            if (!Directory.Exists(DEF_SYSTEM.DEF_FOLDER_PATH_DEFECTIMAGE))
                Directory.CreateDirectory(DEF_SYSTEM.DEF_FOLDER_PATH_DEFECTIMAGE);

            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "[System] Create Folder Complete");
        }

        /// <summary>
        /// 24.02.20 LYK FindFrameGrabber()
        /// </summary> 
        /// Frame Grabber의 장착 여부를 확인 한다.
        /// 장착 여부를 확인 하고 Simulation Mode로 프로그램을 실행 할지 자동 모드로 실행 할지를 결정 하기 위함
        /// <returns></returns>
        private bool FindFrameGrabber()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");

            // 데이터 확인용 foreach
            foreach (ManagementObject device in searcher.Get())
            {
                string deviceName = (string)device["Name"];
                deviceNames.Add(deviceName);
            }

            string sFrameGrabberName = string.Empty;

            sFrameGrabberName = "PC3603 - Coaxlink Quad CXP-12 (4-camera)";

            return deviceNames.Contains(sFrameGrabberName);
        }

        private void InitializeCamera()
        {
            InspectionCompleteEvents = new List<List<List<ManualResetEvent>>>();

            for (int instanceIdx = 0; instanceIdx < DEF_SYSTEM.INSP_CNT; instanceIdx++)
            {
                var camList = new List<List<ManualResetEvent>>();

                for (int camNum = 0; camNum < DEF_SYSTEM.CAM_MAX_COUNT; camNum++)
                {
                    var imageList = new List<ManualResetEvent>();

                    // 20250911 SHJ 전기 or 디스플레이 레시피에 맞게 이벤트 할당 
                    // 전기 : 이미지 가로줄 수 로 검사하기 때문에 MergeCount
                    // 디스플레이 : 이미지 세로줄 수로 검사 하기 때문에 SliceCount 
                    int nEventCnt = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? theRecipe.SliceImageCount : theRecipe.MergeImageCount;
                    
                    for (int imgNum = 0; imgNum < nEventCnt; imgNum++)
                    {
                        imageList.Add(new ManualResetEvent(false));
                    }

                    camList.Add(imageList);
                }

                InspectionCompleteEvents.Add(camList);
            }

            bool bBoardCheck = true;// FindFrameGrabber();  //240220 LYK Frame Grabber Check 

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++) // Defect Map 출력용 1개 추가 
            {
                Cameras.Add(new CCameraManager(mainForm, Logger)
                {
                    m_nCamNumber = i, //220504 LYK 객체 번호가 되는 동시에 Camera 번호로도 활용                   
                });

                // 카메라 Grab 에 대한 Manual Event 할당  
                if (i < DEF_SYSTEM.CAM_MAX_COUNT)
                {
                    Cameras[i].SetInspectionTimeInfo(RunTimeCheck);
                }

                Cameras[i].Initialize(bBoardCheck);                //220504 LYK Camera Initial

            }

            //m_PageController.MaxPageSet();
            SetInspectionEventInitial();    //240209 LYK Inspection Event Initial
        }

        /// <summary>
        /// 24.02.09 LYK SetInspectionEventInitial
        /// 장비에 맞게 Inspection Event를 초기화 하는 함수
        /// 각각의 검사 이벤트를 할당 한다.

        private void SetInspectionEventInitial()
        {
            // 20250911 SHJ 전기 or 디스플레이 레시피에 맞게 이벤트 할당 
            // 전기 : 이미지 가로줄 수 로 검사하기 때문에 MergeCount
            // 디스플레이 : 이미지 세로줄 수로 검사 하기 때문에 SliceCount 
            int nEventCnt = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? theRecipe.SliceImageCount : theRecipe.MergeImageCount;

            for (int instanceIdx = 0; instanceIdx < DEF_SYSTEM.INSP_CNT; instanceIdx++)
            {
                for (int camNum = 0; camNum < DEF_SYSTEM.CAM_MAX_COUNT; camNum++)
                {
                    for (int imgNum = 0; imgNum < nEventCnt; imgNum++)
                    {
                        Cameras[camNum].SetInspectionCompleteEvent(DEF_SYSTEM.PARTICLE_INSPECTION_EVENT, instanceIdx, camNum, imgNum, InspectionCompleteEvents[instanceIdx][camNum][imgNum]);
                    }
                }
            }
        }

        private void UnInitializeCamera()
        {
            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                Cameras[i].UnInitialize();  //220504 LYK Camera UnInitial
            }

            Cameras.Clear();                //220504 LYK Camera List Clear          
        }

        public void InitializeThread()      //220504 MainForm에서 호출
        {
            //m_ThreadInspectionComplete = new CThread()
            //{
            //    Work = WaitInspectionCompleteEvent,
            //    nSleepTime = 1
            //};
            //
            //m_ThreadInspectionComplete.ThreadStart();
            //m_ThreadInspectionComplete.SeriesSet();

            m_CsvSaveThread = new CThread()
            {
                Work = SaveDefectCsv,
                nSleepTime = 1
            };
            m_CsvSaveThread.ThreadStart();

        }

        public void UninitialThread()
        {
            //m_ThreadInspectionComplete.ThreadStop();
            m_CsvSaveThread.ThreadStop();
        }

        /// <summary>
        /// note 24.06.04 LYK
        /// 맨 처음 Grab을 시작할때 모든 카메라에 Grab Start 명령을 송신한다.
        /// 이후 Master camera인 1번 카메라만 Grab Start 하면 된다.
        /// 이유는 Seqence Mode를 사용 하고 있기 때문
        /// 만약 GrabStop 명령이 송신된 후 다시 Grab을 시작 하려고 하는경우 모든 카메라에 Grab Start 명령을 송신해야한다.
        /// </summary>

        public void DoGrabLiveStart()
        {
            string msg = $"Live Grab Start.";
            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], msg);

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                theMainSystem.Cameras[i].m_bIsInspection = false; // Live Mode 

                Cameras[i].DoGrabStart();
            }
        }


        /// <summary>
        /// 24.03.04 LYK DoTeachGrabStart
        /// </summary>
        public void DoSettingTeachGrabStart(int _nTeachMode)
        {
            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                Cameras[i].m_bIsInspection = true; // Inspection Mode 
                Cameras[i].m_nInspMode = _nTeachMode;

                Cameras[i].DoGrabStart();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        // Step 1. Grab Start 
        public void DoInspectionStart() 
        {
            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Grab Cycle Time : {m_CycleTimeWatch.ElapsedMilliseconds.ToString()} ms");
            //theMainSystem.CycleTime?.Invoke($"{m_CycleTimeWatch.ElapsedMilliseconds.ToString()} ms");             //필요하다면 기능 살려야함

            //if (m_CycleTimeWatch.ElapsedMilliseconds.ToString() != "0")                                           //필요하다면 기능 살려야함
            //    theMainSystem.m_nGrabCycleTime = int.Parse(m_CycleTimeWatch.ElapsedMilliseconds.ToString());      //필요하다면 기능 살려야함

            m_CycleTimeWatch.Restart();

            string msg = $"Grab Start.";
            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], msg);

            string LogMessage = "Grab Start" + DateTime.Now.ToString("hh:mm:ss.ffff(tt)");
            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], LogMessage);

            DateTime StartTime = DateTime.Now;                  //240202 LYK Grab Inspection Total StartTime

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                Cameras[i].m_bIsInspection = true; // Inspection Mode 
                Cameras[i].m_nInspMode = DEF_SYSTEM.INSP_NORMAL;

                Cameras[i].DoGrabStart();
            }

        }

        public void DoInspectionStop() // 모든 Camera Grab 완료 되면 실행 
        {
            string msg = $"Grab Stop.";
            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], msg);

            string LogMessage = "Grab Start" + DateTime.Now.ToString("hh:mm:ss.ffff(tt)");
            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], LogMessage);

            DateTime StartTime = DateTime.Now;                  //240202 LYK Grab Inspection Total StartTime

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                Cameras[i].m_bIsInspection = false; // Inspection Mode 
                Cameras[i].m_nInspMode = DEF_SYSTEM.INSP_NORMAL;

                Cameras[i].DoGrabStop();
            }
        }

        List<WaitHandle> allHandles = new List<WaitHandle>();
        //////////////////////////////////////////////////////////////////////////////////////////////
        // Step 3. Inspection Complete
        /// <summary>
        /// 25.05.17 LYK
        /// WaitHandle 처리 수정
        /// InspectionCompleteEvents가 중첩 리스트로 변경 되었기 때문에 수정
        /// </summary>
        private void WaitInspectionCompleteEvent(int _InstanceIdx)
        {
            allHandles.Clear();

            // [CamNum][ImageNum]에 접근
            foreach (var camList in InspectionCompleteEvents[_InstanceIdx])
            {
                allHandles.AddRange(camList); // camList는 List<ManualResetEvent>
            }

            bool timeoutOccurred = !WaitHandle.WaitAll(allHandles.ToArray(), int.MaxValue);
            DoInspectionComplete(timeoutOccurred);
        }

        private void DoInspectionComplete(bool bTimeOut) // 모든 검사 Instance 의 실행이 완료 되면 실행 
        {
            // 20250911 SHJ
            // 전기 or 디스플레이 갯수가 상이 하기 때문에 이벤트 리스트 카운트 리셋 처리  
            // 전기 : 이미지 가로줄 수 로 검사하기 때문에 MergeCount
            // 디스플레이 : 이미지 세로줄 수로 검사 하기 때문에 SliceCount 

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                for(int j = 0; j < InspectionCompleteEvents[m_CurrrentIdx][i].Count; j++)
                {
                    InspectionCompleteEvents[m_CurrrentIdx][i][j].Reset();
                }
            }
            
            GetInspectionData();

            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {InspectionInfos[m_CurrrentIdx].ID} Index : {m_CurrrentIdx}, Total Process Time : {InspectionInfos[m_CurrrentIdx].totalGrabInspectionTime.ToString()}");

            if (m_sJudge == "NG")
            {
                //240629 LYK 판정 송신, 이미지 저장

                //검사가 끝난후 
                //InterfacePLC.SetResultInfos(InspectionInfos[m_CurrrentIdx]);
                //InterfacePLC.JudgeSendData(2, InspectionInfos[m_CurrrentIdx].WaferID);
            }
            else
            {
                //240629 LYK 판정 송신, 이미지 저장

                //검사가 끝난후 
                //InterfacePLC.SetResultInfos(InspectionInfos[m_CurrrentIdx]);
                //InterfacePLC.JudgeSendData(1, InspectionInfos[m_CurrrentIdx].WaferID);
            }

            int CurIdx = m_CurrrentIdx;

            SaveImage(InspectionInfos[CurIdx], CurIdx);

            DefectDisplay(CurIdx);
            //Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {InspectionInfos[CurIndx].ID} Index : {CurIndx}, Show Image Display. {sw.ElapsedMilliseconds} ms");

            RefreshResult?.Invoke(InspectionInfos[CurIdx], CurIdx);     //240202 LYK InspectionInfos 배열을 인자로 넘겨 Judge, 시간등을 표시 해준다.
                                                                        //Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {InspectionInfos[CurIndx].ID} Index : {CurIndx}, Result UI Update. {sw.ElapsedMilliseconds} ms");

            theMainSystem.RefreshCropDefecType?.Invoke(theMainSystem.InspectionInfos[CurIdx], CurIdx);
            theMainSystem.RefreshDefectLabel?.Invoke(InspectionInfos[CurIdx], CurIdx);
            //Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {InspectionInfos[CurIndx].ID} Index : {CurIndx}, Label Result Update. {sw.ElapsedMilliseconds} ms");

            theMainSystem.RefreshHisotryData?.Invoke(InspectionInfos[CurIdx]);
            //Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {InspectionInfos[CurIndx].ID} Index : {CurIndx}, Hisotry Update. {sw.ElapsedMilliseconds} ms");

            //RefreshScottPlot?.Invoke(InspectionInfos[CurIndx], CurIndx);
            //Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {InspectionInfos[CurIndx].ID} Index : {CurIndx}, Scottplot Update! {sw.ElapsedMilliseconds} ms");

            m_CsvSaveThread.Continue();

            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {InspectionInfos[CurIdx].ID} Index : {CurIdx}, Process All End!");
        }

        private void GetInspectionData()
        {
            try
            {
                /// 24.02.02 LYK TotalTime(Grab ~ Inspection End 계산
                /// StopWatch로 제어하지 않고 Grab 시작을 한 후 StartTime(DateTime.Now 이고, 그랩 부터 결과 취합 할)을 검사 객체에 할당
                /// 결과 취합할때 현재 시간과 StartTime을 빼서 총 소요시간을 구한다.
                /// InspectionInfos는 검사중 다음 검사가 시작되는것을 대비하여 Idx별로 데이터를 담는다.(PLC로 WaferID를 송신해야 하는데 갱신 되는것을 방지 하기 위함)
                /// InspectionInfos의 배열에 m_CurrrentIdx 기준으로 데이터를 담는다.
                /// 
                int validImageCount = -1;

                for (int i = 0; i < ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE].Count; i++)
                {
                    var defectList = ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][i];
                    if (defectList != null && defectList.Count > 0)
                    {
                        validImageCount = i;
                        break;
                    }
                }


                TimeSpan timeSpan;
                timeSpan = (DateTime.Now - ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][validImageCount][0].StartTime);

                InspectionInfos[m_CurrrentIdx].ID = ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][validImageCount][0].m_sProductName;    //240202 LYK WaferID
                InspectionInfos[m_CurrrentIdx].sStartTime = ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][validImageCount][0].StartTime.ToString("yyMMdd_HHmmss_fff");
                InspectionInfos[m_CurrrentIdx].sOriginalImagePath = ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][validImageCount][0].OriginImagePath;//ProductInfo.DimensionDataManager[m_CurrrentIdx][0].OriginImagePath;
                InspectionInfos[m_CurrrentIdx].sDefectImagePath = ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][validImageCount][0].DefectImagePath;
                InspectionInfos[m_CurrrentIdx].totalGrabInspectionTime = (long)timeSpan.TotalMilliseconds;                   //240202 LYK Grab ~ Inspection End 까지의 시간
                InspectionInfos[m_CurrrentIdx].InspectionIdx = m_CurrrentIdx;                                                //240202 LYK Inspection Idx
                InspectionInfos[m_CurrrentIdx].EndTime = DateTime.Now;
                InspectionInfos[m_CurrrentIdx].DeepLearningTime = ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][validImageCount][0].DeepLearnignTime;
                InspectionInfos[m_CurrrentIdx].InspTime = ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][validImageCount][0].WorkInspectTime;
                InspectionInfos[m_CurrrentIdx].GrabCycleTime = ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][validImageCount][0].GrabCycleTime;
                InspectionInfos[m_CurrrentIdx].nImageWidth = Cameras[DEF_SYSTEM.CAM_ONE].ListImages[m_CurrrentIdx][0].m_nWidth;
                InspectionInfos[m_CurrrentIdx].nImageHeight = Cameras[DEF_SYSTEM.CAM_ONE].ListImages[m_CurrrentIdx][0].m_nHeight * theRecipe.MergeImageCount;
               
                // 240430 SHJ ResultInfo 클래스 내부 InspResult 클래스 추가 -> 디펙트 내용을 저장해 두기 위해 사용 
                InspectionInfos[m_CurrrentIdx].InspResults.Clear();

                for (int i = 0; i < theRecipe.ClassifyName.Count; i++)
                    theRecipe.ClassifyName[i].nTotalCnt = 0;

                for (int i = 0; i < theRecipe.SegClassName.Count; i++)
                    theRecipe.SegClassName[i].nTotalCnt = 0;

                int nRuleBaseJudge = 0;
                int nDeepLearningJudge = 0; // 240430 SHJ 딥러닝 저지먼트 인덱스 -> 0 : ok, 0 이상일 경우 ng 

                // 20250911 SHJ 디스플레이는 Slice 구분, 전기는 Merge 로 구분 
                int nImgCount = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? theRecipe.SliceImageCount : theRecipe.MergeImageCount;
                for (int i = 0; i < nImgCount; i++)
                {
                    for(int j = 0; j < ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][i].Count; j++)
                    {
                        if(ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][i][j].InspResult != null)
                        {
                            // 250909 SHJ Insp Result 값이 없거나 Defect Type OK 일 경우 세부 정보를 받지 않는다 
                            if (ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][i][j].InspResult != null && ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][i][j].InspResult.m_sDefectType != "OK")
                            {
                                CInspectionResult InspResult = new CInspectionResult();

                                InspResult.DefectPos = ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][i][j].InspResult.DefectPos;
                                InspResult.m_InnerValue = ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][i][j].InspResult.m_InnerValue;
                                InspResult.m_sClassColor = ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][i][j].InspResult.m_sClassColor;
                                InspResult.m_sDefectType = ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][i][j].InspResult.m_sDefectType;
                                InspResult.Width = ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][i][j].InspResult.Width;
                                InspResult.Height = ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][i][j].InspResult.Height;
                                InspResult.Size = ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][i][j].InspResult.Size;

                                // SHJ 디스플레이 같은 경우 Classify 체크
                                if(theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY)
                                {
                                    for (int k = 0; k < theRecipe.ClassifyName.Count; k++)
                                    {
                                        if (theRecipe.ClassifyName[k].ClassifyName == InspResult.m_sDefectType)
                                        {
                                            theRecipe.ClassifyName[k].nTotalCnt++;
                                            InspResult.m_sClassColor = theRecipe.ClassifyName[k].m_sClassColor;

                                            break;
                                        }
                                    }
                                }
                                else //SHJ 전기는 Segment 체크 
                                {
                                    for (int k = 0; k < theRecipe.SegClassName.Count; k++)
                                    {
                                        if (theRecipe.SegClassName[k].SegClassName == InspResult.m_sDefectType)
                                        {
                                            theRecipe.SegClassName[k].nTotalCnt++;
                                            InspResult.m_sClassColor = theRecipe.SegClassName[k].m_sClassColor;

                                            break;
                                        }
                                    }
                                }

                                if (!ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][i][j].m_bRuleBaseJudge)
                                    nRuleBaseJudge++;
                                if (!ProductInfo.DefectManager[m_CurrrentIdx][DEF_SYSTEM.CAM_ONE][i][j].m_bDeepLearningJudge)
                                    nDeepLearningJudge++;

                                InspectionInfos[m_CurrrentIdx].InspResults.Add(InspResult);
                            }

                        }
                        
                    }
                }

                //250517 LYK 임시 주석 

                InspectionInfos[m_CurrrentIdx].nDefectCnt = InspectionInfos[m_CurrrentIdx].InspResults.Count;
                InspectionInfos[m_CurrrentIdx].sJudge = nRuleBaseJudge == 0 ? "OK" : "NG";


                if (DEF_SYSTEM.LICENSES_DEEP_KEY != (int)DeepLicense.NONE)
                    InspectionInfos[m_CurrrentIdx].sClassifyJudge = nDeepLearningJudge == 0 ? "OK" : "NG";

                // Display
                if(theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY)
                {
                    for (int i = 0; i < theRecipe.ClassifyName.Count; i++)
                    {
                        if (theRecipe.ClassifyName[i].nTotalCnt == 0)
                            theRecipe.ClassifyName[i].sJudge = "OK";
                        //else if(theRecipe.ClassifyName[i].nTotalCnt == 1)
                        //    theRecipe.ClassifyName[i].sJudge = "SEMI_OK";
                        else
                            theRecipe.ClassifyName[i].sJudge = "NG";
                    }
                }
                else // Electronic
                {
                    for (int i = 0; i < theRecipe.SegClassName.Count; i++)
                    {
                        if (theRecipe.SegClassName[i].nTotalCnt == 0)
                            theRecipe.SegClassName[i].sJudge = "OK";
                        //else if(theRecipe.ClassifyName[i].nTotalCnt == 1)
                        //    theRecipe.ClassifyName[i].sJudge = "SEMI_OK";
                        else
                            theRecipe.SegClassName[i].sJudge = "NG";
                    }
                }

                // 240430 SHJ 최종 결과 
                if (InspectionInfos[m_CurrrentIdx].sJudge == "OK" && InspectionInfos[m_CurrrentIdx].sClassifyJudge == "OK")
                    InspectionInfos[m_CurrrentIdx].bTotalJudge = true;
                else
                    InspectionInfos[m_CurrrentIdx].bTotalJudge = false;

                // 240430 SHJ 러닝 횟수 업데이트 
                theRecipe.m_nTotalRunCount++;

                if (InspectionInfos[m_CurrrentIdx].bTotalJudge)
                    theRecipe.m_nOKRunCount++;
                else
                    theRecipe.m_nNGRunCount++;
                

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {InspectionInfos[m_CurrrentIdx].ID} Index : {m_CurrrentIdx}, Process Insp Stop");
            }
            catch(Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"GetInspectionData Exception Error : {e.Message}");
            }
        }

        /// <summary>
        /// 24.02.19 LYK DefectDisplay
        /// 검사 결과를 취합한 후 Defect를 표시 하기 위한 함수
        /// </summary>
        public void DefectDisplay(int _nCurIdx)
        {
            try
            {
                for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                {
                    mainForm.DisplayScreen.Frames[i].Invoke(new MethodInvoker(delegate ()
                    {
                        Cameras[i].ImageDisplay(_nCurIdx);
                    }));
                }
            }
            catch (Exception e)
            {

            }


        }

        private bool SaveImage(InspectionInfo _Infos, int _nCurIdx)
        {
            try
            {
                // 날짜 폴더 생성
                if (!Directory.Exists(_Infos.sOriginalImagePath))
                    Directory.CreateDirectory(_Infos.sOriginalImagePath);

                // 20250916 SHJ 시뮬레이션 사용 포맷을 맞추기 위해 시간 폴더 추가 X, 바로 판정 폴더 생성 
                //string[] sStartTime = _Infos.sStartTime.Split('_');
                //string sStrHour = sStartTime[1].Substring(0, 2);

                //// 시간 폴더 구분
                //string shourPath = string.Format("{0}{1}\\", _Infos.sOriginalImagePath, sStrHour);

                //if (!Directory.Exists(shourPath))
                //    Directory.CreateDirectory(shourPath);

                // 판정 폴더 생성
                string sJudgePath = string.Empty;

                if (_Infos.sJudge == "OK")
                    sJudgePath = _Infos.sOriginalImagePath + "OK Image";
                else
                    sJudgePath = _Infos.sOriginalImagePath + "NG Image";

                if (!Directory.Exists(sJudgePath))
                    Directory.CreateDirectory(sJudgePath);

                string sIDPath = sJudgePath + "\\" + $"{_Infos.sStartTime}_{_Infos.ID}";

                if (!Directory.Exists(sIDPath))
                    Directory.CreateDirectory(sIDPath);

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {InspectionInfos[_nCurIdx].ID} Index : {_nCurIdx}, Image Save Start!");
                for (int i = 0; i < theRecipe.MergeImageCount; i++)
                {
                    string SavePath = $"{sIDPath}\\{_Infos.sStartTime}_{_Infos.ID}_{_Infos.sJudge}_Cam{0}Image{i}.bmp";

                    ImageSaverManager.EnqueueSaveRequest(SavePath, Cameras[0].ListImages[_nCurIdx][i]);
                }

                // Merge Image 
                string ImagePath = $"{sIDPath}\\{_Infos.sStartTime}_{_Infos.ID}_{_Infos.sJudge}_Cam{0}Merge.png";
                ImageSaverManager.EnqueueSaveRequest(ImagePath, Cameras[0].ListImages[_nCurIdx][0], 100, true, true);

            }
            catch (Exception e)
            {

            }
            return true;
        }

        /// <summary>
        /// 20250911 SHJ CPU 부하 상태 확인을 위해 스크린 캡처 메소드 임시 추가 
        /// </summary>
        /// <param name="Percent"></param>
        public void ScreenCapture(double Percent)
        {
            // 25050820 SHJ 이미지 저장 시점 PC 부하율 확인을 위해 임시 스크린 캡처
            using (Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
            {
                Graphics g = Graphics.FromImage(bmp);


                g.CopyFromScreen(Point.Empty, Point.Empty, Screen.PrimaryScreen.Bounds.Size);

                bmp.Save($"{DEF_SYSTEM.DEF_FOLDER_PATH_SCREEN}\\{DateTime.Now.ToString("yyMMdd_HHmmss")}_ScreenCapture_{Percent}.png", ImageFormat.Png);

                g.Dispose();

            }
        }

        /// <summary>
        /// 20250917 SHJ 자동 삭제 기능 추가 백그라운드 클래스에서 실행 중 
        /// AutoDelete 실행 위치는 Sideform 에서 PC 하드웨어 정보 출력 하는 타이머 OnClock 에 위치 
        /// </summary>
        public void AutoDelete(double CurHDD)
        {
            DataDeleteManager.Execute(CurHDD);
        }

        /// <summary>
        /// 24.06.29 LYK CalculateDimensionData
        /// Dimension Data를 계산 한다.
        /// 3, 4번 카메라만 해당한다.
        /// </summary>
        /// <param name="_Infos"></param>
        /// <returns></returns>
        private bool CalculateDimensionData(InspectionInfo _Infos)
        {
            try
            {
                //치수 결과 데이터를 계산한다. (3, 4번 카메라만 한정)
                //false = NG, true = OK
                
            }
            catch (Exception e)
            {
            
            }

            return true;
        }

        public void SetCameraData() //220504 LYK 임시 테스트 함수
        {
            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; ++i)
            {
                //Cameras[i].m_Camera.CameraSetting(theRecipe.m_dExposureTime[i], theRecipe.m_dConsrast[i], theRecipe.m_dGain[i]);
            }
        }


        #endregion


        public void SimulTest()
        {
            STime StartTime;
            StartTime.Year = DateTime.Now.ToString("yyyy");
            StartTime.Month = DateTime.Now.ToString("MM");
            StartTime.Day = DateTime.Now.ToString("dd");
            StartTime.Hour = DateTime.Now.ToString("HH");
            StartTime.Min = DateTime.Now.ToString("mm");
            StartTime.Sec = DateTime.Now.ToString("ss");

            theMainSystem.ProductInfo.Initialize($"{theRecipe.m_nTestNumber++}Test1", StartTime); // 이미지 저장 폴더 생성
            // SHJ 레시피에 맞게 이미지 카운트 체크 
            int nImgCount = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? theRecipe.SliceImageCount : theRecipe.MergeImageCount;


            for (int j = 0; j < nImgCount; j++)
            {
                // 화면에 출력 
                for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                {
                    // 20250911 SHJ 현재 레시피에 맞게 필요한 Work 할당 
                    var Work = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ?
                                Cameras[i].ListInspect[m_nSimulIdx][j] : Cameras[i].InspDeepLearning;

                    Cameras[i].m_nInspMode = DEF_SYSTEM.INSP_NORMAL;
                    Cameras[i].m_bIsInspection = true; // Inspection Mode 
                    Work.StartTime = DateTime.Now;                                   //240202 LYK Start Time 대입
                    Work.sProductName = theMainSystem.ProductInfo.m_sProductName;
                    Work.sOriginalImagePath = theMainSystem.ProductInfo.OrginalImagePath.ToString();
                    Work.sDefectImagePath = theMainSystem.ProductInfo.DefectImagePath.ToString();
                    
                    Cameras[i].SimulationInspectRun(m_nSimulIdx, j);

                }
            }

            m_nSimulIdx++;

            if (m_nSimulIdx >= DEF_SYSTEM.INSP_CNT)
                m_nSimulIdx = 0;
        }


        private void SaveDefectCsv()
        {

            try
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Csv Save Start");

                string line;
                int lineCount = 0;
                int nCrackCnt = 0;
                InspectionInfo _Infos = new InspectionInfo();

                datas.Clear();
                if (!File.Exists(theMainSystem.ProductInfo.ResultCsvPath.ToString()))
                {
                    //if (theMainSystem.ProductInfo.DefectManager.Count > 0) //m_CurrentIdx 하ㅗㄹ용
                    {
                        datas.Add(DEF_SYSTEM.CsvHeader);
                        File.AppendAllLines(theMainSystem.ProductInfo.ResultCsvPath.ToString(), datas);
                        datas.Clear();
                    }
                }

                // "Time, ProductName, Judge, DefectCount"
                line = string.Format("{0},", InspectionInfos[m_CurrrentIdx].sStartTime);
                line += string.Format("{0},", InspectionInfos[m_CurrrentIdx].ID);
                line += string.Format("{0},", InspectionInfos[m_CurrrentIdx].sJudge);
                line += string.Format("{0},", InspectionInfos[m_CurrrentIdx].nDefectCnt);
                line += string.Format("{0},", InspectionInfos[m_CurrrentIdx].totalGrabInspectionTime);

                datas.Add(line);

                File.AppendAllLines(theMainSystem.ProductInfo.ResultCsvPath.ToString(), datas);

                // 20250917 SHJ ng 디펙트가 1개 이상 있을 경우 디펙트 정보가 담긴 엑셀을 저장
                if(InspectionInfos[m_CurrrentIdx].nDefectCnt > 0)
                {
                    datas.Clear();

                    // 폴더 생성 
                    string sFolder = string.Format("{0}\\{1}", theMainSystem.ProductInfo.ResultCsvFolder, "Defect Data");

                    if (!Directory.Exists(sFolder))
                        Directory.CreateDirectory(sFolder);

                    // csv 파일 명 
                    string sCSVPath = string.Format("{0}\\{1}_{2}.csv", sFolder, InspectionInfos[m_CurrrentIdx].sStartTime, InspectionInfos[m_CurrrentIdx].ID);

                    datas.Add(DEF_SYSTEM.DefectCsvHeader);
                    File.AppendAllLines(sCSVPath, datas);
                    datas.Clear();

                    // 프로젝트 구분하여 분해능 계산 
                    double Resolution = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? DEF_SYSTEM.DISPLAY_PIXELRESOLUTION : DEF_SYSTEM.ELECTRONIC_PIXELRESOLUTION;

                    // "ProductName, No, Type, PosX, PosY, Width, Height";
                    for (int i = 0; i < InspectionInfos[m_CurrrentIdx].InspResults.Count; i ++)
                    {
                        if (InspectionInfos[m_CurrrentIdx].InspResults[i] != null)
                        {
                            line = string.Format("{0},", InspectionInfos[m_CurrrentIdx].ID);
                            line += string.Format("{0},", i);
                            line += string.Format("{0},", InspectionInfos[m_CurrrentIdx].InspResults[i].m_sDefectType);
                            line += string.Format("{0},", Math.Round(InspectionInfos[m_CurrrentIdx].InspResults[i].DefectPos.X * Resolution,3));
                            line += string.Format("{0},", Math.Round(InspectionInfos[m_CurrrentIdx].InspResults[i].DefectPos.Y * Resolution, 3));
                            line += string.Format("{0},", Math.Round(InspectionInfos[m_CurrrentIdx].InspResults[i].Width * Resolution, 3));
                            line += string.Format("{0},", Math.Round(InspectionInfos[m_CurrrentIdx].InspResults[i].Height * Resolution, 3));
                            line += string.Format("{0},", InspectionInfos[m_CurrrentIdx].InspResults[i].m_sClassColor.A.ToString());
                            line += string.Format("{0},", InspectionInfos[m_CurrrentIdx].InspResults[i].m_sClassColor.R.ToString());
                            line += string.Format("{0},", InspectionInfos[m_CurrrentIdx].InspResults[i].m_sClassColor.G.ToString());
                            line += string.Format("{0},", InspectionInfos[m_CurrrentIdx].InspResults[i].m_sClassColor.B.ToString());

                            datas.Add(line);

                            File.AppendAllLines(sCSVPath, datas);

                            datas.Clear();
                        }
                    }
                }
                
                //240614 LYK ContourData Csv 저장
                //for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                //{
                //    line = string.Format("{0},", ProductInfo.DimensionDataManager[i][0].m_sProductName);                    //240614 LYK WaferID
                //    line += string.Format("{0},", ProductInfo.DimensionDataManager[i][0].m_nCamNum);                        //240614 LYK Cam Num
                //
                //
                //    datas.Add(line);
                //}

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Csv Save End");
            }
            catch (System.Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Inspection DefecetCsv Error Message Catch : {e.Message}");
            }

        }

        /// <summary>
        /// 24.01.23 LYK HistoryImageDisplay
        /// History View에 이미지를 디스플레이 하기위해 호출 하는 함수
        /// </summary>
        public void HistoryImageDisplay(int _nMode)
        {
            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                Cameras[i].ShowImageHistoryDisplay(_nMode);
        }

        /// <summary>
        /// 24.01.28 LYK InspectionTimeCheck
        /// 검사에대한 시간을 Check 하는 함수
        /// </summary>
        /// <param name="_nInspectionType">검사 Type</param>
        /// <param name="_nInspectionTime">검사 시간</param>
        private void RunTimeCheck(int _nInspectionType, long _nInspectionTime)
        {
            //병렬 검사 하기 때문에 가장 큰 값의 시간을 대입해준다.
            switch (_nInspectionType)
            {
                case DEF_SYSTEM.PARTICLE_INSPECTION:

                    if (Infos.InspTime < _nInspectionTime)
                    {
                        Infos.InspTime = _nInspectionTime;
                    }

                    break;
            }
        }

        /// <summary>
        /// 25.02.04 LYK Logger 객체를 가져오는 함수
        /// </summary>
        public CLogger GetLogger
        {
            get { return Logger; }
        }

        public bool GetLiveStatus()
        {
            return m_bLiveStart;
        }

        public void SetLiveStatus(bool _bLiveStatus)
        {
            m_bLiveStart = _bLiveStatus;
        }

        public CMath GetMath()
        {
            return cMath;
        }

        private void SetDisplayIdx(int _DisplayIdx)
        {
            m_nDisplayIdx = _DisplayIdx;
            m_CurrrentIdx = _DisplayIdx;
            Task.Run(() => WaitInspectionCompleteEvent(m_CurrrentIdx));
            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Display idx : {m_nDisplayIdx}");
        }

        public void Wait(int DelayTime)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, DelayTime);
            DateTime AfterMoment = ThisMoment.Add(duration);

            while (AfterMoment >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
                System.Threading.Thread.Sleep(1);
            }
        }
    }
}
