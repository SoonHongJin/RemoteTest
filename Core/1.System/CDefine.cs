using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;

using static Core.DEF_Common;
using System.ComponentModel;
using Core.Function;

namespace Core
{

    public struct STime         //220504 LYK 시간 구조체
    {
        public string Year;
        public string Month;
        public string Day;
        public string Hour;
        public string Min;
        public string Sec;
    }

    public struct SOpenCheck
    {
        public bool OpenCheck;
    }

    /// <summary>
    /// 24.01.28 LYK InspectionInfo 구조체
    /// 검사 시작할때 StopWatch 측정 시작
    /// 검사 정보를 큐로 관리한다.
    /// </summary>
    /// 
    public enum DEEP_MODEL { Segmentation = 0, Classify = 1, ColorStainSegmentation = 2 };//250320 LYK ColorStainSegmentation ENUM 추가
    public class InspectionInfo    
    {
        public long totalGrabInspectionTime { get; set; } = 0;
        public int InspectionIdx { get; set; } = 0;
        public string ID { get; set; } = string.Empty;
        public long InspTime { get; set; } = 0;
        public long DeepLearningTime { get; set; } = 0;
        public long GrabCycleTime { get; set; } = 0;

        public long[] CamGrabTime = new long[DEF_SYSTEM.CAM_MAX_COUNT];
        public string sStartTime { get; set; } = string.Empty;

        public DateTime EndTime;
        public string sOriginalImagePath = string.Empty;        //240608 LYK Original Image Path Add
        public string sDefectImagePath = string.Empty;        //240608 LYK Original Image Path Add

        public string sJudge { get; set; }        //230829 LYK RuleBase Judge
        public string sClassifyJudge { get; set; } //230829 LYK RuleBase Judge
        public bool bTotalJudge { get; set; }

        public string sSegmentJudge { get; set; }  //230829 LYK RuleBase Judge

        public int nDefectCnt { get; set; } = 0;
        public int nImageWidth { get; set; } = 0;
        public int nImageHeight { get; set; } = 0;

        public List<CInspectionResult> InspResults { get; set; } = new List<CInspectionResult>(); 
    }

    public enum License
    {
        COG = 0,
        INS = 1
    };

    public enum DeepLicense
    {
        NONE = 0,
        SAIGE = 1,
        INS = 2
    };


    public class DEF_Common
    {
        public const int SUCCESS = 0;
        public const int RUN_FAIL = 1;
        public const string MSG_UNDEFINED = "undefined";

        //
        public const int WhileSleepTime = 10; // while interval time
        public const int UITimerInterval = 100; // ui timer interval
        public const int SimulationSleepTime = 100; // simulation sleep time for move

        //
        public const int TRUE = 1;
        public const int FALSE = 0;

        //
        public const int BIT_ON = 1;
        public const int BIT_OFF = 0;


        // TimeType
        public enum ETimeType
        {
            NANOSECOND,
            MICROSECOND,
            MILLISECOND,
            SECOND,
            MINUTE,
            HOUR,
        }

        // Language
        public enum ELanguage
        {
            NONE = -1,
            KOREAN = 0,
            ENGLISH,
            CHINESE,
            JAPANESE,
            MAX,
        }

        // Login
        public enum ELoginType
        {
            OPERATOR = 0,
            ENGINEER,
            MAKER,
        }

        public enum EMessageType
        {
            NONE = -1,
            OK,
            OK_CANCEL,
            CONFIRM_CANCEL,
            MAX,
        }

        /// <summary>
        /// Message Information
        /// </summary>
        public class CMessageInfo
        {
            public int Index = 0;
            public EMessageType Type = EMessageType.OK;

            public string[] Message = new string[(int)DEF_Common.ELanguage.MAX];

            public CMessageInfo()
            {
                for (int i = 0; i < (int)ELanguage.MAX; i++)
                {
                    Message[i] = MSG_UNDEFINED;
                }
            }

            public bool IsEqual(CMessageInfo info)
            {
                if (this.Index != info.Index) return false;
                if (this.Type != info.Type) return false;

                for (int i = 0; i < (int)ELanguage.MAX; i++)
                {
                    if (this.Message[i] != info.Message[i]) return false;
                }

                return true;
            }

            public string GetMessage(DEF_Common.ELanguage lang = DEF_Common.ELanguage.ENGLISH)
            {
                return Message[(int)lang];
            }

            public bool IsEqual(string strMsg)
            {
                strMsg = strMsg.ToLower();
                foreach (string str in Message)
                {
                    string str1 = str.ToLower();

                    // Message 특성상 마침표등의 문제로 문자열을 포함하면 같은 메세지인걸로 판단하도록.
                    if (str1 == strMsg || str1.IndexOf(strMsg) >= 0 || strMsg.IndexOf(str1) >= 0)
                        return true;
                }
                return false;
            }

            public bool Update(CMessageInfo info)
            {
                bool bUpdated = false;
                string str;
                for (int i = 0; i < (int)ELanguage.MAX; i++)
                {
                    str = info.Message[i];
                    if (str != MSG_UNDEFINED && string.IsNullOrWhiteSpace(str) == false
                        && Message[i] != str)
                    {
                        bUpdated = true;
                        Message[i] = str;
                    }
                }

                //if(bUpdated)
                {
                    Type = info.Type;
                }
                return bUpdated;
            }

        }
    }
    
    


    public class DEF_SYSTEM
    {
        // 시뮬레이션 모드 설정 (카메라 사용 여부)
        #region 카메라 관련 상수 
        public const int CAM_MAX_COUNT = 1;
        public const int IMAGE_COUNT_PER_CAMERA = 5; // R , G , B , 바(IR) , Color 

        public const int SIMULATION_CAM = -1;       //240115 LYK 유레시스 캠                                                 
        public const int EURESYS_CAM = 0;           //240115 LYK 시뮬레이션
        public const int MATROX_LINE_CAM = 1;
        public const int INS_CIS = 3;

        public const int CAM_ONE = 0;
        public const int CAM_TWO = 1;
        public const int CAM_THREE = 2;
        public const int CAM_FOUR = 3;
        public const int CAM_FIVE = 4;

        public const int CAM_PUBLIC_INTERFACE = 1; //0 cxp 1 gige
        public const int CAM_PUBLIC_LIVE_INTERVAL = 250; // 라이브 Timer 딜레이

        public static string[] DEF_SYSTEM_CAM_NAME = { "Cam1", "Cam2", "Cam3", "Cam4" };

        public static readonly string[] CAM_GIGE_SERIALNUMBER =
        {
            "000316",
            "000316",
            "000316",
            "000316",
            "000316",
            "000318"
        };

        public const int IMAGE_WIDTH = 128571; //(디스플레이)128571;//(전자)93712;
        public const int IMAGE_HEIGHT = 3000;//(디스플레이)3000;//(전자)5000;

        public const int MONO_IMAGE_GRAB = 0;
        public const int MONO_LIVE_IMAGE_GRAB = 1;
        public const int MONO_CALIBRATION_GRAB = 2;
        public const int MONO_IMAGE_TEACH_GRAB = 3;
        public const int MONO_IMAGE_TABLE_GRAB = 4;
        public const int MONO_IMAGE_DEFECT_GRAB = 5;

        public const int TABLE_DISPLAY_WIDTH = 1000;
        public const int TABLE_DISPLAY_HEIGHT = 1000;

        #endregion

        #region TCP 관련
        public const int TCP_BUFFER_SIZE = 4096;
        public const byte TCP_STX = 0x02;
        public const byte TCP_ETX = 0x03;

        #endregion

        #region 시스템 폴더 경로
        //public const string DCF_PATH = @"D:\03. DCF\DCF1.dcf"
        private const string MAIN_FOLDER_NAME = "ConnectedInsight";
        private const string PROJECT_NAME = "CIS Platform";

        private const string DEF_FOLDER_PATH_MAINC = "C:\\" + MAIN_FOLDER_NAME; // C:\\ConnectedInsight
        public const string DEF_FOLDER_PATH_MAIND = "D:\\" + MAIN_FOLDER_NAME; // D:\\ConnectedInsight

        public const string DEF_FOLDER_PATH_PROJECT_NAME = DEF_FOLDER_PATH_MAIND + "\\" + PROJECT_NAME;

        public const string DEF_FOLDER_PATH_IMAGE = DEF_FOLDER_PATH_PROJECT_NAME + "\\01. Running Images";              //220511 LYK 원본 이미지 폴더
        public const string DEF_FOLDER_PATH_CSV = DEF_FOLDER_PATH_PROJECT_NAME + "\\02. Running Result";                //220511 LYK 검사 결과 폴더
        public const string DEF_FOLDER_PATH_RECIPE = DEF_FOLDER_PATH_PROJECT_NAME + "\\03. Recipe Parameter";           //220511 LYK 레시피 폴더
        public const string DEF_FOLDER_PATH_CALIB = DEF_FOLDER_PATH_PROJECT_NAME + "\\04. Calibration Images";          //220511 LYK 캘리브레이션 폴더
        public const string DEF_FOLDER_PATH_SCREEN = DEF_FOLDER_PATH_PROJECT_NAME + "\\05. Screen Caputre";             //220511 LYK 스크린캡쳐 폴더
        public const string DEF_FOLDER_PATH_LOG = DEF_FOLDER_PATH_PROJECT_NAME + "\\06. SystemLog";                     //220511 LYK 로그 폴더
        public const string DEF_FOLDER_PATH_MANUAL = DEF_FOLDER_PATH_PROJECT_NAME + "\\07. Manual";                     //220511 LYK Defect 크롭 이미지 폴더

        public const string DEEPLEARNING_RUNTIME_PATH = DEF_FOLDER_PATH_RECIPE;                                         //220511 LYK 임시 워크 스페이스
        public const string TEMP_IMAGE_PATH = DEF_FOLDER_PATH_PROJECT_NAME + "\\Data";

        public const string DEF_FOLDER_PATH_INTERFACELOG = DEF_FOLDER_PATH_PROJECT_NAME + "\\08. InterfaceLog";         //220511 LYK 로그 폴더
        public const string DEF_FOLDER_PATH_MESLOG = DEF_FOLDER_PATH_PROJECT_NAME + "\\09. MesLog";

        public const string DEF_FOLDER_PATH_DEFECTIMAGE = DEF_FOLDER_PATH_PROJECT_NAME + "\\09. Defect Image";         //220511 LYK 로그 폴더

        #endregion

        #region 검사 관련 상수
        public const int INSP_CNT = 2; // 일반 검사 병렬 처리 

        public const int PARTICLE_INSPECTION = 1;
        public const int INSNEX_DEEPLEARNING = 2;
        public const int INSP_SAGEDEEPLEARNING = 3;

        public const int INSP_NORMAL = 0;
        public const int INSP_TEACH = 1;
        public const int INSP_CALIBRATION = 2;
        public const int INSP_LIVE = 3;
        public const int INSP_SIMUL = 4;

        public const int DEFECT_CROP_WIDTH = 128;
        public const int DEFECT_CROP_HEIGHT = 128;

        public const int DEFECT_MAX_COUNT = 50;

        public const double DISPLAY_PIXELRESOLUTION = 0.014;
        public const double ELECTRONIC_PIXELRESOLUTION = 0.007;

        #endregion

        #region 캘리브레이션 관련
        public const int CHECKER_BOARD_CALIBRATION = 0;

        #endregion

        #region CSV 관련 상수
        //public static string CsvHeader = "No, ProductName, Judge, RuleBaseJudge, ClassName, Width, Height, Area, PosX, PosY, DefectColor Alpha, DefectColor Red, DefectColor Green, DefectColor Blue, Origin_ImagePath, PolygonData";
        public static string CsvHeader = "Time, ProductName, Judge, DefectCount, RunTime(ms)";
        public static string DefectCsvHeader = "ProductName, No, Type, PosX, PosY, Width, Height, DefecColor(A), DefecColor(R), DefecColor(G), DefecColor(B)";
        #endregion

        #region 시리얼 통신 관련
        public const int TRIGGER_BOARD_BAUDRATE = 115200;

        #endregion

        #region 조명 컨트롤러 관련
        public const string LIGHT_CONTROLLER_IP = "192.168.0.100";
        public const int LIGHT_PORT = 5000;
        public const string LIGHT_COM = "COM1";
        public const int LIGHT_CONTROLLER_BAUDRATE = 19200;
        public const int LIGHT_MAX_PAGE = 8;
        public const int LIGHT_CHANEL = 8;
        public const double LIGHT_UNIT = 0.02; //[ms]

        #endregion

        #region Event 관련
        public const int PARTICLE_INSPECTION_EVENT = 0;
        public const int PARTICLE_DEEPLEARNING_EVENT = 1;

        public const int DISPLAY_IMAGE = 0;
        public const int DISPLAY_MAP = 1;
        public const int DISPLAY_TEACH = 2;
        public const int DISPLAY_CALIB = 3;
        public const int DISPLAY_DEFECT = 4;
        #endregion

        #region Interface 관련 상수
        // PLC 
        public const int PLC_MODE = 0;
        public const string PLC_IP = "100.100.100.4";
        //public const string PLC_IP = "127.0.0.1";
        public const int PLC_PORT = 5010;
        public const int LOGICAL_STATION_NUMBER = 0;   //240823 NIS PLC 통신 Station Number = 0
        public static bool REALTIME_COMMUNICATION_ACTIVATION = false;


        // 240831 KCh PLC 통신 신호 리스트
        public static string[] Chartlabels = { "WRN", "STP", "TOK", "RES", "SEC", "BYP", "RST Back", "RST", "GRS Back", "GRS", "STA Back", "STA", "TRG Back", "TRG", "WaferID" };
        #endregion

        #region 로그 관련 상수
        public static readonly string[] LOGTYPE = { "SystemLog", "InterfaceLog", "MESLog" };
        public enum LOGTYPE_ENUM
        {
            SystemLog = 0,
            InterfaceLog,
            MESLog
        };
        #endregion

        #region LICENSES
        public const int LICENSES_KEY = (int)License.COG;   //241104 NIS 라이센스 License cog ins 설정
        public const int LICENSES_DEEP_KEY = (int)DeepLicense.SAIGE;
        #endregion

        #region 장비 관련 상수 
        public const string ELECTRONIC = "ELECTRONIC";
        public const string DISPLAY = "DISPLAY";
        #endregion


    }

    public class DEF_UI
    {
        public const int SETTING_MENU = 0;
        public const int PARAM_MENU = 1;
        public const int HISTROY_MENU = 2;
        public const int TEACH_MENU = 3;

        public const int DROP_MENU_MAXCOUNT = 4;

        public const string CAM_SETTING = "Camera";
        public const string CALIBRATION = "Calibration";
        public const string PLC_SETTING = "PLC";
        public const string MODEL_PARAM = "Model";
        public const string SYS_PARAM = "System Parameter";
        public const string HISTORY_LOG = "Log";
        public const string HISTORY_SIMUL = "Simulation";
        public const string HISTORY_DATA = "Data View";

        public const string HISTORY_INTERFACE_DATA = "Interface"; //240616 KCH
        public const string TEACHING = "Teaching";

        // Top Screen Form
        public static readonly int TOP_POS_X = 0;
        public static readonly int TOP_POS_Y = 0;
        public static readonly int TOP_SIZE_WIDTH = 1920;
        public static readonly int TOP_SIZE_HEIGHT = 63; //75 

        // Main Screen Forms
        public static readonly int MAIN_POS_X = 0;
        public static readonly int MAIN_POS_Y = 0; // SelectFormChange 메서드에서 Top Screen 위치에 따라 재 조정 됨 
        public static readonly int MAIN_SIZE_WIDTH = 1920; //1920;
        public static readonly int MAIN_SIZE_HEIGHT = 1080; //880

        // Bottom Screen Form
        public static readonly int BOT_POS_X = 1720; //0;
        public static readonly int BOT_POS_Y = 0; //980; 
        public static readonly int BOT_SIZE_WIDTH = 200; //1920;
        public static readonly int BOT_SIZE_HEIGHT = 950; //100;

        public static readonly Point[] ProgressPoint = new Point[] { new Point { X = 0, Y = 690 }, new Point { X = 69, Y = 573 }, new Point { X = 69, Y = 605 }, new Point { X = 69, Y = 637 }, new Point { X = 69, Y = 669 }, new Point { X = 69, Y = 701 } };
        public static readonly int ProgressWidth = 1108;
        public static readonly int ProgressHeight = 50;
        public static readonly int ProgressPCDataWidth = 121;
        public static readonly int ProgressPCDataHeight = 32;

        //FormSettingCamScreen 페이지 
        public static readonly Rectangle[] SettingBounds = new Rectangle[]
        {new Rectangle { X = 368, Y = 701, Width = 240, Height = 30}     
        , new Rectangle { X = 368, Y = 734, Width = 240, Height = 34 }   
        , new Rectangle{ X = 368, Y = 769, Width = 240, Height = 34}     
        , new Rectangle{ X = 368, Y = 804, Width = 240, Height = 34}     
        , new Rectangle{ X = 368, Y = 839, Width = 240, Height = 34}     
        , new Rectangle{ X = 368, Y = 874, Width = 240, Height = 34}      
        , new Rectangle{ X = 614, Y = 702, Width = 530, Height = 32}     
        , new Rectangle{ X = 614, Y = 733, Width = 530, Height = 176}    
        , new Rectangle{ X = 1149, Y = 702, Width = 93, Height = 50}     
        , new Rectangle{ X = 1246, Y = 702, Width = 93, Height = 50}     
        , new Rectangle{ X = 1179, Y = 807, Width = 138, Height = 16}    
        , new Rectangle{ X = 1148, Y = 834, Width = 93, Height = 33}     
        , new Rectangle{ X = 1245, Y = 834, Width = 93, Height = 33}      
        , new Rectangle{ X = 1149, Y = 876, Width = 190, Height = 33} };

        //public static readonly Point[] ProgressPoint = new Point[] { new Point { X = 74, Y = 34 }, new Point { X = 74, Y = 67 }, new Point { X = 74, Y = 100 }, new Point { X = 74, Y = 133 } };
        //public static readonly int ProgressWidth = 121;
        //public static readonly int ProgressHeight = 32;

        public static int SEGCOUNT = 36;    //240507 LYK Segment Max Count
        public static int CLSCOUNT = 30;    //240507 LYK Classify Max Count

        public enum EButtonType
        {
            NONE = -1,
            AUTO,
            SETTING,
            PARAM,
            TEACH,
            HISTORY,
            EXIT,
            MAX,
        }

        public enum EFormType
        {
            NONE = -1,
            AUTO,
            SETTING_CAMERA,
            SETTING_CALIBRATION,
            SETTING_PLC,
            PARAMETER_MODEL,
            TEACHING,
            HISTORY_DATA,
            HISOTRY_CURRENT_DATA,
            HISTORY_SIMUL,    //240711 KCH Tracking Data 폼 추가
            HISTORY_INTERFACE_DATA, //240831 KCH InterfaceChart 폼 추가
            MAX,
        }
    }

    public class CFunc
    {
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr Src, int Count);

    }
}

