using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Newtonsoft.Json.Linq;

using static Core.Program;
using Core.HardWare;
using Core;
using Core.Function;

using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using System.Drawing;
using MetaLog;
using Core.UI;
using Insnex.Vision2D.ToolBlock;
using InsCHVSControl;
using Insnex.Vision2D.Common;
using Insnex.AI;


namespace Core.DataProcess
{
    public class CPLCAddress
    {
        public string PLCReadStart = string.Empty;
        public string PLCWriteStart = string.Empty;
        public string RB_SYNC = string.Empty;
        public string RB_VENDER = string.Empty;
        public string RB_BLOCK = string.Empty;
        public string RB_STA = string.Empty;
        public string RB_GRS = string.Empty;
        public string RB_RST = string.Empty;
        public string RB_TRG = string.Empty;
        public string RB_CHR = string.Empty;
        public string RB_Acknowledge = string.Empty;
        public string RB_BYP = string.Empty;
        public string RB_SEC = string.Empty;
        public string RB_UnProcessed_Flag = string.Empty;
        public string RB_Processed_Flag = string.Empty;
        public string RD_LamaID = string.Empty;
        public string RD_WaferID = string.Empty;
        public string RD_InfoText = string.Empty;
        public string RD_TimeStamp = string.Empty;
        public string RD_ProcRecipe = string.Empty;
        public string RD_ProcCarrierID = string.Empty;
        public string RD_ProcCarrierPosX = string.Empty;
        public string RD_ProcCarrierPosY = string.Empty;
        public string RD_ProcCarrierPosZ = string.Empty;
        public string RD_ProcCarrierCounter = string.Empty;
        public string RD_ToolID = string.Empty;
        public string RD_TrackID = string.Empty;
        public string RD_JOBID = string.Empty;

        public string WB_SYNC = string.Empty;
        public string WB_VENDER = string.Empty;
        public string WB_BLOCK = string.Empty;
        public string WB_STA_BACK = string.Empty;
        public string WB_GRS_BACK = string.Empty;
        public string WB_RST_BACK = string.Empty;
        public string WB_TRG_BACK = string.Empty;
        public string WB_CHR_BACK = string.Empty;
        public string WB_Acknowledge = string.Empty;
        public string WD_ToolID1 = string.Empty;
        public string WD_ToolID2 = string.Empty;
        public string WB_TOK = string.Empty;
        public string WB_RES = string.Empty;
        public string WB_STP = string.Empty;
        public string WB_WRN = string.Empty;
        public string WD_Quality1 = string.Empty;
        public string WD_Quality2 = string.Empty;
        public string WD_SimResult = string.Empty;
        public string WD_SampleCounter = string.Empty;
        public string WD_ValueA = string.Empty;
        public string WD_ValueB = string.Empty;
        public string WD_ValueC = string.Empty;
        public string WD_ValueD = string.Empty;
        public string WD_PosX = string.Empty;
        public string WD_PosY = string.Empty;
        public string WD_PosT = string.Empty;
        public string WD_WaferID = string.Empty;

        public string[] ReadAddress = new string[20];
    }


    /// <summary>
    /// 24.03.11 LYK CPageLightValue Class
    /// </summary>
    public class CPageLightValue
    {
        public int MaxPage = 0;
        public int[] m_nLightValue = new int[DEF_SYSTEM.LIGHT_MAX_PAGE];
    }

    public class CSegMentationClass
    {
        public string SegClassName { get; set; }
        public int nTotalCnt { get; set; }
        public Color m_sClassColor { get; set; }
        public string sJudge { get; set; }
        public int nSetScore { get; set; }//250821 SJH Defect Score ThreshHold 값
    }

    public class CClassifyClass
    {
        public string ClassifyName { get; set; }
        public string sJudge { get; set; }
        public int nTotalCnt { get; set; }
        public Color m_sClassColor { get; set; }
        public int nSetScore { get; set; }

    }

    public class CalibrationStatus
    {
        public bool IsCheckerBoardComplete = false;
    }

    public class CMonoLimit
    {
        public double m_dSpotAreaLimitSize { get; set; } = 0.0;
        public double m_dStainAreaLimitSize { get; set; } = 0.0;        //240302 LYK Paste Stained은 Area 사이즈를 누적합산 하여 처리
        public double m_dScatterAreaLimitSize { get; set; } = 0.0;
        public double m_dBrokenLimit { get; set; } = 0.0;
        public double m_dEdgeNGLimit { get; set; } = 0.0;
        public double m_dPrintNGLimit { get; set; } = 0.0;
        public double m_dScratchLimit { get; set; } = 0.0;              //240302 LYK Scratch는 Length 사이즈를 누적합산 하여 처리
        public double m_dCrackLimit { get; set; } = 0.0;
        public double m_dChippingLimit { get; set; } = 0.0;
        public double m_dBrokenLimitPx { get; set; } = 0;
        public double m_dCrackLimitPx { get; set; } = 0;
        public double m_dChippingLimitPx { get; set; } = 0;

        // Paste Stained
        public double m_dPasteStainedAreaLimit_MinSize { get; set; } = 0;      //241207 NIS 1~3mm^2은 최대 3개까지 허용, 3mm^2 초과는 NG
        public double m_dPasteStainedAreaLimit_MaxSize { get; set; } = 0;
        public int m_dPasteStainedAreaLimit_AllowCnt { get; set; } = 0;

        // Busbar
        public double m_dBusbarInterruptionLimit { get; set; } = 0.0;
        public double m_dBusbarKnotLimit { get; set; } = 0.0;

        // Ladder
        public double m_dLadderInterruptionLimit { get; set; } = 0.0;   //240302 LYK Ladder Interruption은 Length 사이즈를 누적합산 하여 처리
        public double m_dLadderKnotLimit { get; set; } = 0.0;
        // Redudancy
        public double m_dRedudancyInterruptionLimit { get; set; } = 0.0;
        public double m_dRedudancyKnotLimit { get; set; } = 0.0;

        //FingerWidht
        public double m_dFingerWidth_Min { get; set; } = 3; //250324 NIS FingerWidth
        public double m_dFingerWidth_Max { get; set; } = 4;

        //QLogo
        public double m_dQLogoAreaLimit { get; set; } = 0.100;  //250324 NIS QLogo 영역 30% 미만일 경우 NG
        public int m_dQLogoIdx1 = DEF_SYSTEM.CAM_ONE;
        public int m_dQLogoIdx2 = DEF_SYSTEM.CAM_FOUR;
    }

    public class CMaterialInfoClass
    {
        public int m_nCellHorCout { get; set; } = 0;        //250909 SHJ 자재 정보 Cell 가로 방향 배열 갯수
        public int m_nCellVertCount { get; set; } = 0;      //250909 SHJ 자재 정보 Cell 세로 방향 배열 갯수 
        public double m_dCellWidth { get; set; } = 0;      //250909 SHJ 자재 정보 Cell Width
        public double m_dCellHeight { get; set; } = 0;     //250909 SHJ 자재 정보 Cell Height
        public double m_dCellPadHeight { get; set; } = 0;  //250909 SHJ 세로 방향 Cell 과 Cell 사이 Pad Height
        public double m_dHolePosX { get; set; } = 0;       //250909 SHJ Active 내 Hole 위치 X 
        public double m_dHolePosY { get; set; } = 0;       //250909 SHJ Active 내 Hole 위치 Y
        public double m_dHoleRadius { get; set; } = 0;     //250909 SHJ Active 내 Hole Radius

    }

    /// <summary> LYK 22.05.12 ~ 13 CRecipe 클래스 
    /// Recipe 관련 파라미터를 관리하는 클래스
    /// 레시피 로드, 레시피 저장, Tool Save, Tool Load 등
    /// </summary>
    public class CRecipe
    {
        #region [Singleton]
        private static CRecipe Instance = null;
        public static CRecipe GetInstance()
        {
            if (null == Instance) Instance = new CRecipe();
                return Instance;
        }
        #endregion

        //----------------------------------------------------------------------------------------------
        //25.04.03 System 관련
        //----------------------------------------------------------------------------------------------
        public int m_nTestNumber = 0;


        public string m_sCurrentModelName = string.Empty;
        public string m_sCurrentEquipment = string.Empty;

        //----------------------------------------------------------------------------------------------
        //25.04.03 LYK Camera 관련
        //----------------------------------------------------------------------------------------------
        //public CCameraRecipe CameraRecipe = new CCameraRecipe();
        public CCameraRecipe CameraRecipe = new CCameraRecipe();
        //----------------------------------------------------------------------------------------------
        //25.04.03 LYK 조명 관련
        //----------------------------------------------------------------------------------------------
        public List<CPageLightValue> LightValue = new List<CPageLightValue>();          //240311 LYK LightValue

        public List<CogToolBlock> m_CogInspToolBlock = new List<CogToolBlock>();        //240126 LYK List ContourInspectToolBlock 추가  ContourInspectCamOne ~ ContourInspectCamFour 변수 삭제 
        //public List<CogToolBlock> m_CogPreToolBlock = new List<CogToolBlock>();

        public List<InsToolBlock> m_InsPreToolBlock = new List<InsToolBlock>();
        public List<InsToolBlock> m_InsInspToolBlock = new List<InsToolBlock>();
        public List<InsAIClassifyTool> m_InsClassifyTool = new List<InsAIClassifyTool>();
        //public List<CogToolBlock> CogFindLineToolBlock = new List<CogToolBlock>();       //250405 NIS InsFindLine ToolBlock 추가
        //public List<CogToolBlock> CogPMAlignToolBlock = new List<CogToolBlock>();        //250405 NIS InsPMAlign ToolBlock 추가

        //public List<InsToolBlock> InsFindLineToolBlock = new List<InsToolBlock>();       //250405 NIS InsFindLine ToolBlock 추가
        //public List<InsToolBlock> InsPMAlignToolBlock = new List<InsToolBlock>();        //250405 NIS InsPMAlign ToolBlock 추가

        public int m_DefectAreaValue;

        public CPLCAddress PLCAddress = new CPLCAddress();

        //----------------------------------------------------------------------------------------------
        //25.04.03 LYK UI Resize 관련
        //----------------------------------------------------------------------------------------------
        public double m_dUIResizeRatio = 1.0;       //240730 NIS UI Resize Ratio
        public double m_dUIResizeRatio_Width = 1.0;
        public double m_dUIResizeRatio_Height = 1.0;


        public List<CSegMentationClass> SegClassName = new List<CSegMentationClass>();  //240303 LYK 딥러닝 Segment Class Info List(Name, Color)
        public List<CClassifyClass> ClassifyName = new List<CClassifyClass>();  //240303 LYK 딥러닝 Classify Class Info List(Name)
        public string[] m_sLastNGData = new string[10]; //240907 LYK CurrentNGData 

        public List<CExceptionData> m_listExceptionData = new List<CExceptionData>();
        public List<string[]> ImageSaveConditionList { get; set; } = new List<string[]>(); //240729 KCH Condition Image Save 조건 List
       
        //240901 NWT AutoDelete
        public int m_nAutoDeletechk;
        public int m_nDateLimit;
        public double m_dDriveLimit;

        public string[] NgImageSaveCondition = { "True", "NG", "FinalJudge", "=", "NG", "True", "False", "False", "False", "True", "Default" };
        public string[] OkImageSaveCondition = { "False", "OK", "FinalJudge", "=", "OK", "False", "False", "False", "False", "False", "Default" };

        //250206 NIS CalibrationStatus
        public CalibrationStatus calibrationStatus = new CalibrationStatus();
        public PointF m_dCheckerBoardTitleSize = new PointF();

        public CogToolBlock CalibrationToolBlock;        //240610 LYK Calibration ToolBlock 추가
        public InsToolBlock InsCalibrationToolBlock;     //250304 KCH Calibration ToolBlock 추가


        public int[] m_nDefectScoreThreshHold = new int[18];
        public int[] m_nDefectAreaThreshHold = new int[18];

        public CMonoLimit MonoLimit = new CMonoLimit();

        //public InsToolBlock InsFingerWidthAvgToolBlock;
        //public CogToolBlock CogFingerWidthAvgToolBlock;

        public int MergeImageCount {get; set;} = 23; // display 23 : Electronic 15
        public int SliceImageCount { get; set; } = 20;

        public int m_nTotalRunCount = 0;
        public int m_nOKRunCount = 0;
        public int m_nNGRunCount = 0;

        public CMaterialInfoClass MaterialInfo = new CMaterialInfoClass(); // 250909 SHJ 자재 정보를 기록 하고 있는 Class 추가 

        private CLogger Logger = null;
        private CRecipe()
        {
            for(int i = 0; i < DEF_SYSTEM.LIGHT_MAX_PAGE; i++)
            {
                CPageLightValue tempLightValue = new CPageLightValue();
                LightValue.Add(tempLightValue);
            }

        }

        public void InitialLogger(CLogger _Logger)
        {
            this.Logger = _Logger;
        }

        public bool DataLoad(string _sModleName)
        {
            string message = string.Empty;

            try
            {
                JObject jsonSetup = JObject.Parse(File.ReadAllText($"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\{_sModleName}.json"));

                for(int i = 0; i < DEF_SYSTEM.LIGHT_MAX_PAGE; i++)  //240311 LYK Page 컨트롤러 DataLoad
                {
                    JObject jsonLightData = (JObject)jsonSetup[$"Page{i + 1} Data"];
                    //CFingerGrade cFingerGrade = new CFingerGrade();
                    LightValue[i].MaxPage = jsonLightData["MaxPage"].ToString() == "" ? 4 : int.Parse(jsonLightData["MaxPage"].ToString());
                    for (int j = 0; j < DEF_SYSTEM.LIGHT_MAX_PAGE; j++)
                    {
                        LightValue[i].m_nLightValue[j] = jsonLightData[$"LightChData{j + 1}"].ToString() == "" ? 4 : int.Parse(jsonLightData[$"LightChData{j + 1}"].ToString());
                    }
                }

                //240804 KCH Condition Seting Recipe에서 불러오기
                if (jsonSetup["ImageSaveConditionList"] != null)
                {
                    JArray conditionArray = (JArray)jsonSetup["ImageSaveConditionList"];
                    ImageSaveConditionList.Clear();

                    for (int i = 0; i < conditionArray.Count; i++)
                    {
                        JArray itemArray = (JArray)conditionArray[i];
                        string[] item = itemArray.ToObject<string[]>();
                        ImageSaveConditionList.Add(item);
                    }
                }

                //250205 NIS CheckerBoard Tile Size X, Y
                JObject jsonCheckerBoardData = (JObject)jsonSetup["CheckerBoardData"];
                m_dCheckerBoardTitleSize.X = CheckState(jsonCheckerBoardData, "m_dCheckerBoardTitleSizeX") ? 4 : float.Parse(jsonCheckerBoardData["m_dCheckerBoardTitleSizeX"].ToString());
                m_dCheckerBoardTitleSize.Y = CheckState(jsonCheckerBoardData, "m_dCheckerBoardTitleSizeY") ? 4 : float.Parse(jsonCheckerBoardData["m_dCheckerBoardTitleSizeY"].ToString());

                //250206 NIS calibrationStatus Load
                JObject jsonCalibStatus = (JObject)jsonSetup["CalibStatus"];
                calibrationStatus.IsCheckerBoardComplete = CheckState(jsonCalibStatus, "IsCheckerBoardComplete") ? false : bool.Parse(jsonCalibStatus["IsCheckerBoardComplete"].ToString());

                JObject jsonDeepScore = (JObject)jsonSetup["DeepScoreThreshHold"];

                if (jsonDeepScore != null)
                {
                    for (int i = 0; i < 18; i++)
                    {
                        m_nDefectScoreThreshHold[i] = CheckState(jsonDeepScore, $"m_nDefectScoreThreshHold_{i}") ? 0 : int.Parse(jsonDeepScore[$"m_nDefectScoreThreshHold_{i}"].ToString());
                    }
                }

                JObject jsonDeepAreaScore = (JObject)jsonSetup["DeepAreaScoreThreshHold"];

                if (jsonDeepAreaScore != null)
                {
                    for (int i = 0; i < 18; i++)
                    {
                        m_nDefectAreaThreshHold[i] = CheckState(jsonDeepAreaScore, $"AreaThreshHold_{i}") ? 0 : int.Parse(jsonDeepAreaScore[$"AreaThreshHold_{i}"].ToString());
                    }
                }

                JObject jsonCameraData = (JObject)jsonSetup["CameraData"];

                CameraRecipe.LedMode = CheckState(jsonCameraData, "LedMode") ? InsCHVS_LED_TriggerMode.LED_SimultaneousDualBrightness : (InsCHVS_LED_TriggerMode)int.Parse(jsonCameraData["LedMode"].ToString());
                CameraRecipe.ImageHeight = CheckState(jsonCameraData, "ImageHeight") ? 3000u : uint.Parse(jsonCameraData["ImageHeight"].ToString());
                CameraRecipe.Exposure1 = CheckState(jsonCameraData, "Exposure1") ? 10f : float.Parse(jsonCameraData["Exposure1"].ToString());

                CameraRecipe.Trigger2Enabled = CheckState(jsonCameraData, "Trigger2Enabled") ? InsCHVS_FuncEnable.Ins_Disable : (InsCHVS_FuncEnable)int.Parse(jsonCameraData["Trigger2Enabled"].ToString());
                CameraRecipe.Trigger1Source = CheckState(jsonCameraData, "Trigger1Source") ? InsCHVS_LineTriggerSource.Internal_Clock : (InsCHVS_LineTriggerSource)int.Parse(jsonCameraData["Trigger1Source"].ToString());
                CameraRecipe.Trigger2Source = CheckState(jsonCameraData, "Trigger2Source") ? InsCHVS_LineTriggerSource.Internal_Clock : (InsCHVS_LineTriggerSource)int.Parse(jsonCameraData["Trigger2Source"].ToString());
                CameraRecipe.EncoderCountMode = CheckState(jsonCameraData, "EncoderCountMode") ? InsCHVS_Enocder_CountMode.PhA_RisingEdge_OneWay : (InsCHVS_Enocder_CountMode)int.Parse(jsonCameraData["EncoderCountMode"].ToString());
                CameraRecipe.EncoderTravelMode = CheckState(jsonCameraData, "EncoderTravelMode") ? InsCHVS_Encoder_TriggerMode.Forward_Scan : (InsCHVS_Encoder_TriggerMode)int.Parse(jsonCameraData["EncoderTravelMode"].ToString());

                CameraRecipe.Trigger2_Encoder_FilteringTimeWidth = CheckState(jsonCameraData, "Trigger2_Encoder_FilteringTimeWidth") ? 1.28f : float.Parse(jsonCameraData["Trigger2_Encoder_FilteringTimeWidth"].ToString());
                CameraRecipe.Trigger2_Encoder_IgnoreCount = CheckState(jsonCameraData, "Trigger2_Encoder_IgnoreCount") ? 0u : uint.Parse(jsonCameraData["Trigger2_Encoder_IgnoreCount"].ToString());
                CameraRecipe.Trigger2_Divide = CheckState(jsonCameraData, "Trigger2_Divide") ? 1u : uint.Parse(jsonCameraData["Trigger2_Divide"].ToString());
                CameraRecipe.Trigger2_InputMultiple = CheckState(jsonCameraData, "Trigger2_InputMultiple") ? 1u : uint.Parse(jsonCameraData["Trigger2_InputMultiple"].ToString());
                CameraRecipe.Trigger2_Enable_InputMultiple = CheckState(jsonCameraData, "Trigger2_Enable_InputMultiple") ? InsCHVS_FuncEnable.Ins_Disable : (InsCHVS_FuncEnable)int.Parse(jsonCameraData["Trigger2_Enable_InputMultiple"].ToString());
                CameraRecipe.TrigPeriod = CheckState(jsonCameraData, "TrigPeriod") ? 0u : uint.Parse(jsonCameraData["TrigPeriod"].ToString());

                JObject jsonLimit = (JObject)jsonSetup["Limit"];
                MonoLimit.m_dSpotAreaLimitSize = CheckState(jsonLimit, "m_dSpotAreaLimitSize") ? 0 : double.Parse(jsonLimit["m_dSpotAreaLimitSize"].ToString());
                MonoLimit.m_dStainAreaLimitSize = CheckState(jsonLimit, "m_dStainAreaLimitSize") ? 0 : double.Parse(jsonLimit["m_dStainAreaLimitSize"].ToString());
                MonoLimit.m_dScatterAreaLimitSize = CheckState(jsonLimit, "m_dScatterAreaLimitSize") ? 0 : double.Parse(jsonLimit["m_dScatterAreaLimitSize"].ToString());
                MonoLimit.m_dBusbarInterruptionLimit = CheckState(jsonLimit, "m_dBusbarInterruptionLimit") ? 0 : double.Parse(jsonLimit["m_dBusbarInterruptionLimit"].ToString());

                MonoLimit.m_dLadderInterruptionLimit = CheckState(jsonLimit, "m_dLadderInterruptionLimit") ? 0 : double.Parse(jsonLimit["m_dLadderInterruptionLimit"].ToString());
                MonoLimit.m_dLadderKnotLimit = CheckState(jsonLimit, "m_dLadderKnotLimit") ? 0 : double.Parse(jsonLimit["m_dLadderKnotLimit"].ToString());
                MonoLimit.m_dBusbarKnotLimit = CheckState(jsonLimit, "m_dBusbarKnotLimit") ? 0 : double.Parse(jsonLimit["m_dBusbarKnotLimit"].ToString());
                MonoLimit.m_dRedudancyInterruptionLimit = CheckState(jsonLimit, "m_dRedudancyInterruptionLimit") ? 0 : double.Parse(jsonLimit["m_dRedudancyInterruptionLimit"].ToString());
                MonoLimit.m_dRedudancyKnotLimit = CheckState(jsonLimit, "m_dRedudancyKnotLimit") ? 0 : double.Parse(jsonLimit["m_dRedudancyKnotLimit"].ToString());
                MonoLimit.m_dBrokenLimit = CheckState(jsonLimit, "m_dBrokenLimit") ? 0 : double.Parse(jsonLimit["m_dBrokenLimit"].ToString());
                MonoLimit.m_dEdgeNGLimit = CheckState(jsonLimit, "m_dEdgeNGLimit") ? 0 : double.Parse(jsonLimit["m_dEdgeNGLimit"].ToString());
                MonoLimit.m_dPrintNGLimit = CheckState(jsonLimit, "m_dPrintNGLimit") ? 0 : double.Parse(jsonLimit["m_dPrintNGLimit"].ToString());
                MonoLimit.m_dScratchLimit = CheckState(jsonLimit, "m_dScratchLimit") ? 0 : double.Parse(jsonLimit["m_dScratchLimit"].ToString());
                MonoLimit.m_dCrackLimit = CheckState(jsonLimit, "m_dCrackLimit") ? 0 : double.Parse(jsonLimit["m_dCrackLimit"].ToString());
                MonoLimit.m_dChippingLimit = CheckState(jsonLimit, "m_dChippingLimit") ? 0 : double.Parse(jsonLimit["m_dChippingLimit"].ToString());

                MonoLimit.m_dCrackLimitPx = CheckState(jsonLimit, "m_dCrackLimitPx") ? 0 : double.Parse(jsonLimit["m_dCrackLimitPx"].ToString());
                MonoLimit.m_dBrokenLimitPx = CheckState(jsonLimit, "m_dBrokenLimitPx") ? 0 : double.Parse(jsonLimit["m_dBrokenLimitPx"].ToString());
                MonoLimit.m_dChippingLimitPx = CheckState(jsonLimit, "m_dChippingLimitPx") ? 0 : double.Parse(jsonLimit["m_dChippingLimitPx"].ToString());

                MonoLimit.m_dPasteStainedAreaLimit_MinSize = CheckState(jsonLimit, "m_dPasteStainedAreaLimit_MinSize") ? 1 : double.Parse(jsonLimit["m_dPasteStainedAreaLimit_MinSize"].ToString());
                MonoLimit.m_dPasteStainedAreaLimit_MaxSize = CheckState(jsonLimit, "m_dPasteStainedAreaLimit_MaxSize") ? 3 : double.Parse(jsonLimit["m_dPasteStainedAreaLimit_MaxSize"].ToString());
                MonoLimit.m_dPasteStainedAreaLimit_AllowCnt = CheckState(jsonLimit, "m_dPasteStainedAreaLimit_AllowCnt") ? 3 : int.Parse(jsonLimit["m_dPasteStainedAreaLimit_AllowCnt"].ToString());

                MonoLimit.m_dFingerWidth_Max = CheckState(jsonLimit, "m_dFingerWidth_Max") ? 4 : double.Parse(jsonLimit["m_dFingerWidth_Max"].ToString());
                MonoLimit.m_dFingerWidth_Min = CheckState(jsonLimit, "m_dFingerWidth_Min") ? 3 : double.Parse(jsonLimit["m_dFingerWidth_Min"].ToString());
                MonoLimit.m_dQLogoAreaLimit = CheckState(jsonLimit, "m_dQLogoAreaLimit") ? 0.100 : double.Parse(jsonLimit["m_dQLogoAreaLimit"].ToString());
                MonoLimit.m_dQLogoIdx1 = CheckState(jsonLimit, "m_dQLogoIdx1") ? 0 : int.Parse(jsonLimit["m_dQLogoIdx1"].ToString());
                MonoLimit.m_dQLogoIdx2 = CheckState(jsonLimit, "m_dQLogoIdx2") ? 3 : int.Parse(jsonLimit["m_dQLogoIdx2"].ToString());

                // 250911 SHJ 임시 Image Count 할당
                JObject jsonParam = (JObject)jsonSetup["Parameter"] != null ? (JObject)jsonSetup["Parameter"] : new JObject();
                MergeImageCount = m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? 23 : 15;
                SliceImageCount = m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? 20 : 4;

                JObject jsonMaterial = (JObject)jsonSetup["Material"] != null ? (JObject)jsonSetup["Material"] : new JObject();
                MaterialInfo.m_nCellHorCout = CheckState(jsonMaterial, "m_nCellHorCout") ? 20 : int.Parse(jsonMaterial["m_nCellHorCout"].ToString());
                MaterialInfo.m_nCellVertCount = CheckState(jsonMaterial, "m_nCellVertCount") ? 5 : int.Parse(jsonMaterial["m_nCellVertCount"].ToString());
                MaterialInfo.m_dCellWidth = CheckState(jsonMaterial, "m_dCellWidth") ? 0 : double.Parse(jsonMaterial["m_dCellWidth"].ToString());
                MaterialInfo.m_dCellHeight = CheckState(jsonMaterial, "m_dCellHeight") ? 10560.0 : double.Parse(jsonMaterial["m_dCellHeight"].ToString());
                MaterialInfo.m_dCellPadHeight = CheckState(jsonMaterial, "m_dCellPadHeight") ? 1660.0 : double.Parse(jsonMaterial["m_dCellPadHeight"].ToString());
                MaterialInfo.m_dHolePosX = CheckState(jsonMaterial, "m_dHolePosX") ? 2430.0 : double.Parse(jsonMaterial["m_dHolePosX"].ToString());
                MaterialInfo.m_dHolePosY = CheckState(jsonMaterial, "m_dHolePosY") ? 370.0 : double.Parse(jsonMaterial["m_dHolePosY"].ToString());
                MaterialInfo.m_dHoleRadius = CheckState(jsonMaterial, "m_dHoleRadius") ? 150.0 : double.Parse(jsonMaterial["m_dHoleRadius"].ToString());


                message = "Recipe Data Load Complete!";

                
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            finally
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], message);

            }

            return true;
        }
        public void DataSave(string _sModleName)
        {
            string message = string.Empty;

            try
            {
                JObject jsonSetup = new JObject();

                JObject jsonParam = new JObject();
                jsonParam.Add("MergeImageCount", MergeImageCount);
                jsonParam.Add("SliceImageCount", SliceImageCount);

                jsonSetup.Add("Parameter", jsonParam);

                JObject jsonCameraData = new JObject();

                jsonCameraData.Add("ImageHeight", CameraRecipe.ImageHeight);
                jsonCameraData.Add("LedMode", Convert.ToInt32(CameraRecipe.LedMode));
                jsonCameraData.Add("Exposure1", CameraRecipe.Exposure1);
                jsonCameraData.Add("Trigger2Enabled", Convert.ToInt32(CameraRecipe.Trigger2Enabled));
                jsonCameraData.Add("Trigger1Source", Convert.ToInt32(CameraRecipe.Trigger1Source));
                jsonCameraData.Add("Trigger2Source", Convert.ToInt32(CameraRecipe.Trigger2Source));
                jsonCameraData.Add("EncoderCountMode", Convert.ToInt32(CameraRecipe.EncoderCountMode));
                jsonCameraData.Add("EncoderTravelMode", Convert.ToInt32(CameraRecipe.EncoderTravelMode));

                jsonCameraData.Add("Trigger2_Encoder_FilteringTimeWidth", CameraRecipe.Trigger2_Encoder_FilteringTimeWidth);
                jsonCameraData.Add("Trigger2_Encoder_IgnoreCount", CameraRecipe.Trigger2_Encoder_IgnoreCount);
                jsonCameraData.Add("Trigger2_Divide", CameraRecipe.Trigger2_Divide);
                jsonCameraData.Add("Trigger2_InputMultiple", CameraRecipe.Trigger2_InputMultiple);
                jsonCameraData.Add("Trigger2_Enable_InputMultiple", Convert.ToInt32(CameraRecipe.Trigger2_Enable_InputMultiple));

                jsonCameraData.Add("TrigPeriod", CameraRecipe.TrigPeriod);

                jsonSetup.Add("CameraData", jsonCameraData);

                JObject []jsonLightData = new JObject[DEF_SYSTEM.LIGHT_MAX_PAGE];
                
                for(int i = 0; i < DEF_SYSTEM.LIGHT_MAX_PAGE; i++)  //240311 LYK Page 컨트롤러 DataSave
                {
                    jsonLightData[i] = new JObject();

                    jsonLightData[i].Add("MaxPage", LightValue[i].MaxPage);

                    for (int j = 0; j < DEF_SYSTEM.LIGHT_MAX_PAGE; j++)
                    {
                        jsonLightData[i].Add($"LightChData{j + 1}", LightValue[i].m_nLightValue[j]);
                    }

                    jsonSetup.Add($"Page{i + 1} Data", jsonLightData[i]);
                }

                //240804 KCH Condition Seting Recipe 저장
                JObject jsonConditionList = new JObject();
                JArray ConditionArray = new JArray();

                for (int i = 0; i < ImageSaveConditionList.Count; i++)
                {
                    JArray itemArray = new JArray(ImageSaveConditionList[i]);
                    ConditionArray.Add(itemArray);
                }

                jsonSetup.Add("ImageSaveConditionList", ConditionArray);

                //250205 NIS CheckerBoard Tile Size X, Y
                JObject jsonCheckerBoardData = new JObject();
                jsonCheckerBoardData.Add("m_dCheckerBoardTitleSizeX", m_dCheckerBoardTitleSize.X);
                jsonCheckerBoardData.Add("m_dCheckerBoardTitleSizeY", m_dCheckerBoardTitleSize.Y);
                jsonSetup.Add("CheckerBoardData", jsonCheckerBoardData);

                //250206 NIS calibrationStatus Load
                JObject jsonCalibStatus = new JObject();
                jsonCalibStatus.Add("IsCheckerBoardComple", calibrationStatus.IsCheckerBoardComplete);
                jsonSetup.Add("CalibStatus", jsonCalibStatus);

                JObject jsonDeepScore = new JObject();

                for (int i = 0; i < 18; i++)
                {
                    jsonDeepScore.Add($"m_nDefectScoreThreshHold_{i}", m_nDefectScoreThreshHold[i]);
                }

                jsonSetup.Add("DeepScoreThreshHold", jsonDeepScore);

                JObject jsonDeepAreaScore = new JObject();

                for (int i = 0; i < 18; i++)
                {
                    jsonDeepAreaScore.Add($"AreaThreshHold_{i}", m_nDefectAreaThreshHold[i]);
                }

                jsonSetup.Add("DeepAreaScoreThreshHold", jsonDeepAreaScore);

                JObject jsonLimit = new JObject();
                jsonLimit.Add("m_dSpotAreaLimitSize", MonoLimit.m_dSpotAreaLimitSize);
                jsonLimit.Add("m_dStainAreaLimitSize", MonoLimit.m_dStainAreaLimitSize);
                jsonLimit.Add("m_dScatterAreaLimitSize", MonoLimit.m_dScatterAreaLimitSize);
                jsonLimit.Add("m_dBusbarInterruptionLimit", MonoLimit.m_dBusbarInterruptionLimit);
                jsonLimit.Add("m_dLadderInterruptionLimit", MonoLimit.m_dLadderInterruptionLimit);
                jsonLimit.Add("m_dLadderKnotLimit", MonoLimit.m_dLadderKnotLimit);
                jsonLimit.Add("m_dBusbarKnotLimit", MonoLimit.m_dBusbarKnotLimit);
                jsonLimit.Add("m_dRedudancyInterruptionLimit", MonoLimit.m_dRedudancyInterruptionLimit);
                jsonLimit.Add("m_dRedudancyKnotLimit", MonoLimit.m_dRedudancyKnotLimit);
                jsonLimit.Add("m_dBrokenLimit", MonoLimit.m_dBrokenLimit);
                jsonLimit.Add("m_dEdgeNGLimit", MonoLimit.m_dEdgeNGLimit);
                jsonLimit.Add("m_dPrintNGLimit", MonoLimit.m_dPrintNGLimit);
                jsonLimit.Add("m_dScratchLimit", MonoLimit.m_dScratchLimit);
                jsonLimit.Add("m_dCrackLimit", MonoLimit.m_dCrackLimit);
                jsonLimit.Add("m_dChippingLimit", MonoLimit.m_dChippingLimit);

                jsonLimit.Add("m_dCrackLimitPx", MonoLimit.m_dCrackLimitPx);
                jsonLimit.Add("m_dBrokenLimitPx", MonoLimit.m_dBrokenLimitPx);
                jsonLimit.Add("m_dChippingLimitPx", MonoLimit.m_dChippingLimitPx);

                jsonLimit.Add("m_dFingerWidth_Max", MonoLimit.m_dFingerWidth_Max);
                jsonLimit.Add("m_dFingerWidth_Min", MonoLimit.m_dFingerWidth_Min);
                jsonLimit.Add("m_dQLogoAreaLimit", MonoLimit.m_dQLogoAreaLimit);
                jsonLimit.Add("m_dQLogoIdx1", MonoLimit.m_dQLogoIdx1);
                jsonLimit.Add("m_dQLogoIdx2", MonoLimit.m_dQLogoIdx2);
                jsonSetup.Add("Limit", jsonLimit);

                JObject jsonMaterial = new JObject();
                jsonMaterial.Add("m_nCellHorCout", MaterialInfo.m_nCellHorCout);
                jsonMaterial.Add("m_nCellVertCount", MaterialInfo.m_nCellVertCount);
                jsonMaterial.Add("m_dCellWidth", MaterialInfo.m_dCellWidth);
                jsonMaterial.Add("m_dCellHeight", MaterialInfo.m_dCellHeight);
                jsonMaterial.Add("m_dCellPadHeight", MaterialInfo.m_dCellPadHeight);
                jsonMaterial.Add("m_dHolePosX", MaterialInfo.m_dHolePosX);
                jsonMaterial.Add("m_dHolePosY", MaterialInfo.m_dHolePosY);
                jsonMaterial.Add("m_dHoleRadius", MaterialInfo.m_dHoleRadius);
                jsonSetup.Add("Material", jsonMaterial);

                //File.WriteAllText("D:\\Test.json", jsonSetup.ToString());
                File.WriteAllText($"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\{_sModleName}.json", jsonSetup.ToString());
                message = "Recipe Data Save Complete!";

                
            }
            catch (Exception e)
            {
                message = "Recipe Data Save Error!";
            }
            finally
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], message);
            }
        }
        
        private void CleanUpToolBlock(CogToolBlock ToolBlock)
        {
            try
            {
                for (int j = 0; j < ToolBlock.Inputs.Count; j++)
                    ToolBlock.Inputs[j].Value = null;

                //for (int j = 0; j < ToolBlock.Outputs.Count; j++)
                //    ToolBlock.Outputs[j].Value = null;

                ToolBlock.Run();
            }
            catch { }
        }

        public void ToolSave(string _sModleName)
        {
            if (DEF_SYSTEM.LICENSES_KEY == (int)License.COG)
            {

                for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                {

                    CleanUpToolBlock(m_CogInspToolBlock[i]);
                    //CleanUpToolBlock(m_CogPreToolBlock[i]);
                    // CogSerializer.SaveObjectToFile(m_ToolBlock[i], $"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\RecipeCam{i}.vpp");
                    CogSerializer.SaveObjectToFile(m_CogInspToolBlock[i], $"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\InspectBlock.vpp", typeof(BinaryFormatter), CogSerializationOptionsConstants.Minimum);
                    //CogSerializer.SaveObjectToFile(m_CogPreToolBlock[i], $"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\PreProcBlock.vpp", typeof(BinaryFormatter), CogSerializationOptionsConstants.Minimum);
                }
            }
            else if (DEF_SYSTEM.LICENSES_KEY == (int)License.INS)
            {
                for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                {
                    InsSerializer.SaveObjectToFile(m_InsInspToolBlock[i], $"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\InspectBlock.tolb");
                    InsSerializer.SaveObjectToFile(m_InsPreToolBlock[i], $"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\PreProcBlock.tolb");
                    //InsSerializer.SaveObjectToFile(InsFingerWidthAvgToolBlock, $"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\FingerWidthAvg.tolb", InsSerializationOptionsConstants.Minimum);
                }
            }
        }

        public void ToolLoad(string _sModleName)
        {            
            try
            {
                if (DEF_SYSTEM.LICENSES_KEY == (int)License.COG)
                {
                    for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                    {
                        m_CogInspToolBlock.Add(CogSerializer.LoadObjectFromFile($"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\InspectBlock.vpp", typeof(BinaryFormatter), CogSerializationOptionsConstants.Minimum) as CogToolBlock);
                        //m_CogPreToolBlock.Add(CogSerializer.LoadObjectFromFile($"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\PreProcBlock.vpp", typeof(BinaryFormatter), CogSerializationOptionsConstants.Minimum) as CogToolBlock);

                        string WorkSpaceName = $"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{theRecipe.m_sCurrentModelName}\\{"Saige_Segment.srSeg"}";

                        //InsAIClassifyTool Classify = new InsAIClassifyTool($"{WorkSpaceName}.rt", $"{WorkSpaceName}.yaml");
                        //m_InsClassifyTool.Add(Classify);

                        //m_ToolBlock.Add(CogSerializer.LoadObjectFromFile($"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\RecipeCam{i}.vpp") as CogToolBlock); //240128 LYK 분리(Cam1 ~ Cam4)되어있던 ToolBlock 변수를 List 변수로 변경 기존 분리되어 있던 변수 삭제
                        //m_CogInspToolBlock.Add(CogSerializer.LoadObjectFromFile($"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\InspectBlock.vpp") as CogToolBlock);
                        //m_CogPreToolBlock.Add(CogSerializer.LoadObjectFromFile($"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\PreProcBlock.vpp") as CogToolBlock);
                        //CogFingerWidthAvgToolBlock = CogSerializer.LoadObjectFromFile($"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\FingerWidthAvg.vpp") as CogToolBlock;
                    }
                }
                else if (DEF_SYSTEM.LICENSES_KEY == (int)License.INS)
                {
                    for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                    {
                        m_InsInspToolBlock.Add(InsSerializer.LoadObjectFromFile($"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\InspectBlock.tolb") as InsToolBlock);
                        m_InsPreToolBlock.Add(InsSerializer.LoadObjectFromFile($"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\PreProcBlock.tolb") as InsToolBlock);
                        //InsFingerWidthAvgToolBlock = InsSerializer.LoadObjectFromFile($"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModleName}\\FingerWidthAvg.tolb") as InsToolBlock;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Tool Load Catch : {e.Message}");
            }
        }

        /// <summary>
        /// 24.09.09 LYK CheckState
        /// 정상이면 False, Error 면 True
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="sParam"></param>
        /// <returns></returns>
        private bool CheckState(JObject obj, string sParam)
        {
            if (obj == null)
                return true;
            else if (obj[sParam] == null || obj[sParam].ToString() == "")
                return true;
            else
                return false;
        }

        /// <summary>
        /// 25.01.21 NWT ExceptionData Save 함수
        /// </summary>
        /// <param name="_sModelName"></param>
        public void ExceptionListSave(string _sModelName)
        {
            string message = string.Empty;

            try
            {
                using (var exceptionlistwriter = new StreamWriter($"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModelName}\\ExceptionList.csv"))
                {
                    exceptionlistwriter.WriteLine("PAGE,NAME,TAG,MIN,MAX,DATATYPE");
                    for (int i = 0; i < m_listExceptionData.Count; i++)
                    {
                        CExceptionData exceptiondata = m_listExceptionData[i];
                        exceptionlistwriter.WriteLine(
                            string.Format("{0},{1},{2},{3},{4},{5}", exceptiondata.Page, exceptiondata.Name, exceptiondata.Tag, exceptiondata.Min, exceptiondata.Max, exceptiondata.DataType)
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                message = "ExceptionList Save Error Occurred";
            }
            finally
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], message);
            }
        }
        public void ExceptionListLoad(string _sModelName)
        {
            m_listExceptionData.Clear();

            using (var exceptionlistReader = new StreamReader($"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{_sModelName}\\ExceptionList.csv"))
            {
                Dictionary<string, string> tempDic = new Dictionary<string, string>();
                bool firstline = true;
                string line = null;
                string[] exceptiondataKeys = null;
                string[] exceptiondDatas = null;

                while ((line = exceptionlistReader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line)) return;

                    if (firstline)
                    {
                        firstline = false;
                        exceptiondataKeys = line.Split(',');
                        continue;
                    }

                    exceptiondDatas = line.Split(',');
                    for (int i = 0; i < exceptiondataKeys.Length; i++)
                    {
                        tempDic.Add(exceptiondataKeys[i], exceptiondDatas[i]);
                    }

                    m_listExceptionData.Add(new CExceptionData(tempDic["PAGE"], tempDic["NAME"], tempDic["TAG"], tempDic["MIN"], tempDic["MAX"], tempDic["DATATYPE"]));
                    tempDic.Clear();
                }
            }
        }

        public void LastRecipeSave(string _sModelName, string[] _sNGData)
        {
            try
            {
                string sPath = DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\LastRecipe.json";

                JObject jsonLastData = new JObject();

                JObject jsonLastRecipe = new JObject();
                jsonLastRecipe.Add("LastRecipe", _sModelName);

                JObject jsonLastID = new JObject();
                jsonLastID.Add("LastID", m_nTestNumber);

                JObject jsonLastEquipment = new JObject();
                jsonLastEquipment.Add("LastEquipmenet", m_sCurrentEquipment);

                JObject jsonLastCount = new JObject();
                jsonLastCount.Add("LastTotalCount", m_nTotalRunCount);
                jsonLastCount.Add("LastOKCount", m_nOKRunCount);
                jsonLastCount.Add("LastNGCount", m_nNGRunCount);

                jsonLastData.Add("LastModel", jsonLastRecipe);
                jsonLastData.Add("LastIDNumber", jsonLastID);
                jsonLastData.Add("jsonLastEquipment", jsonLastEquipment);
                jsonLastData.Add("jsonLastCount", jsonLastCount);

                //20240510_17:37 나인성
                if (_sNGData != null)
                {
                    JObject jsonLastNG10 = new JObject();

                    for (int i = 0; i < 10; i++)
                    {
                        jsonLastNG10.Add("LastNG" + i, _sNGData[i]);
                    }
                    jsonLastData.Add("jsonLastNGData", jsonLastNG10);
                }

                File.WriteAllText(sPath, jsonLastData.ToString());
            }
            catch(Exception e)
            {
                string msg = e.Message;
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], msg);
            }

        }

        public void LastRecipeLoad()
        {
            try
            {
                string sPath = DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\LastRecipe.json";

                JObject jsonLastData = JObject.Parse(File.ReadAllText(sPath));

                JObject jsonLastRecipe = (JObject)jsonLastData["LastModel"];
                m_sCurrentModelName = jsonLastRecipe["LastRecipe"].ToString() == "" ? "Test" : jsonLastRecipe["LastRecipe"].ToString();

                JObject jsonLastID = (JObject)jsonLastData["LastIDNumber"];

                if(jsonLastID != null)
                {
                    m_nTestNumber = jsonLastID["LastID"].ToString() == "" ? 0 : int.Parse(jsonLastID["LastID"].ToString());
                }

                JObject jsonLastEquipment = (JObject)jsonLastData["jsonLastEquipment"];

                if (jsonLastEquipment != null)
                    m_sCurrentEquipment = jsonLastEquipment["LastEquipmenet"].ToString() == "" ? "SORTER_UPPER" : jsonLastEquipment["LastEquipmenet"].ToString();

                JObject jsonLastCount = (JObject)jsonLastData["jsonLastCount"];
                if (jsonLastCount != null)
                {
                    m_nTotalRunCount = jsonLastEquipment["LastTotalCount"] == null ? 0: int.Parse(jsonLastCount["LastTotalCount"].ToString());
                    m_nOKRunCount = jsonLastEquipment["LastOKCount"] == null ? 0 : int.Parse(jsonLastCount["LastOKCount"].ToString());
                    m_nNGRunCount = jsonLastEquipment["LastNGCount"] == null ? 0 : int.Parse(jsonLastCount["LastNGCount"].ToString());
                }

                //JObject jsonLastNGData = (JObject)jsonLastData["jsonLastNGData"];
                //
                //if(m_sLastNGData != null)
                //{
                //    for (int i = 0; i < 10; i++)
                //    {
                //        m_sLastNGData[i] = jsonLastNGData["LastNG" + i].ToString();
                //    }
                //}
            }
            catch(Exception e)
            {
                string msg = e.Message;
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], msg);
            }

        }

        public void SaveRecipe(string _sModelName)
        {
            string sPath = DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + $"\\{_sModelName}";

            if (!Directory.Exists(sPath))
                Directory.CreateDirectory(sPath);

            theRecipe.DataSave(_sModelName);
        }

        public void AddressDefineSave()    //231006 LYK 사용하지 않음
        {
            string sPath = DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\AddressDefine.json";

            JObject jsonAddressData = new JObject();

            JObject jsonAddress = new JObject();
            jsonAddress.Add("PLCReadStart", "W10");
            jsonAddress.Add("PLCWriteStart", "W50");
            jsonAddress.Add("RB_STA", "W10");
            jsonAddress.Add("RB_GRS", "W11");
            jsonAddress.Add("RB_RST", "W12");
            jsonAddress.Add("RB_TRG", "W13");
            jsonAddress.Add("RB_CHR", "W14");
            jsonAddress.Add("RB_BYP", "W15");
            jsonAddress.Add("RB_SEC", "W16");
            jsonAddress.Add("RD_WaferID", "W17");

            jsonAddress.Add("WB_STA_BACK", "W50");
            jsonAddress.Add("WB_GRS_BACK", "W51");
            jsonAddress.Add("WB_RST_BACK", "W52");
            jsonAddress.Add("WB_TRG_BACK", "W53");
            jsonAddress.Add("WB_CHR_BACK", "W54");
            jsonAddress.Add("WB_TOK", "W55");
            jsonAddress.Add("WB_RES", "W56");
            jsonAddress.Add("WB_STP", "W57");
            jsonAddress.Add("WB_WRN", "W58");
            jsonAddress.Add("WD_MONO_JUDGE", "W59");
            jsonAddress.Add("WD_COLOR_JUDGE", "W5A");
            jsonAddress.Add("WD_ContourX", "W5B");
            jsonAddress.Add("WD_ContourY", "W5D");
            jsonAddress.Add("WD_ContourT", "W5F");
            jsonAddress.Add("WD_WaferID", "W61");


            jsonAddressData.Add("Address", jsonAddress);

            File.WriteAllText(sPath, jsonAddressData.ToString());
        }

        //240902 NIS 현장 내용
        public void AddressDefineLoad()
        {
            try
            {
                string sPath = DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\DefineAddress.json";
                JObject jsonAddressData = JObject.Parse(File.ReadAllText(sPath));

                JObject jsonAddressRecipe = (JObject)jsonAddressData[m_sCurrentEquipment];
                PLCAddress.PLCReadStart = jsonAddressRecipe["PLCReadStart"].ToString() == "" ? "W100" : jsonAddressRecipe["PLCReadStart"].ToString();
                PLCAddress.PLCWriteStart = jsonAddressRecipe["PLCWriteStart"].ToString() == "" ? "W150" : jsonAddressRecipe["PLCWriteStart"].ToString();

                PLCAddress.RB_SYNC = jsonAddressRecipe["RB_SYNC"].ToString() == "" ? "W100" : jsonAddressRecipe["RB_SYNC"].ToString();
                PLCAddress.RB_VENDER = jsonAddressRecipe["RB_VENDER"].ToString() == "" ? "W101" : jsonAddressRecipe["RB_VENDER"].ToString();
                PLCAddress.RB_BLOCK = jsonAddressRecipe["RB_BLOCK"].ToString() == "" ? "W102" : jsonAddressRecipe["RB_BLOCK"].ToString();

                PLCAddress.RB_STA = jsonAddressRecipe["RB_STA"].ToString() == "" ? "W103" : jsonAddressRecipe["RB_STA"].ToString();
                PLCAddress.RB_GRS = jsonAddressRecipe["RB_GRS"].ToString() == "" ? "W104" : jsonAddressRecipe["RB_GRS"].ToString();
                PLCAddress.RB_RST = jsonAddressRecipe["RB_RST"].ToString() == "" ? "W105" : jsonAddressRecipe["RB_RST"].ToString();
                PLCAddress.RB_TRG = jsonAddressRecipe["RB_TRG"].ToString() == "" ? "W106" : jsonAddressRecipe["RB_TRG"].ToString();
                PLCAddress.RB_CHR = jsonAddressRecipe["RB_CHR"].ToString() == "" ? "W104" : jsonAddressRecipe["RB_CHR"].ToString();
                PLCAddress.RB_Acknowledge = jsonAddressRecipe["RB_Acknowledge"].ToString() == "" ? "W104" : jsonAddressRecipe["RB_Acknowledge"].ToString();

                PLCAddress.RB_BYP = jsonAddressRecipe["RB_BYP"].ToString() == "" ? "W112" : jsonAddressRecipe["RB_BYP"].ToString();
                PLCAddress.RB_SEC = jsonAddressRecipe["RB_SEC"].ToString() == "" ? "W113" : jsonAddressRecipe["RB_SEC"].ToString();
                PLCAddress.RB_UnProcessed_Flag = jsonAddressRecipe["RB_UnProcessed_Flag"].ToString() == "" ? "W113" : jsonAddressRecipe["RB_UnProcessed_Flag"].ToString();
                PLCAddress.RB_Processed_Flag = jsonAddressRecipe["RB_Processed_Flag"].ToString() == "" ? "W113" : jsonAddressRecipe["RB_Processed_Flag"].ToString();

                PLCAddress.RD_LamaID = jsonAddressRecipe["RD_LamaID"].ToString() == "" ? "W134" : jsonAddressRecipe["RD_LamaID"].ToString();

                PLCAddress.RD_WaferID = jsonAddressRecipe["RD_WaferID"].ToString() == "" ? "W134" : jsonAddressRecipe["RD_WaferID"].ToString();
                PLCAddress.RD_InfoText = jsonAddressRecipe["RD_InfoText"].ToString() == "" ? "W134" : jsonAddressRecipe["RD_InfoText"].ToString();
                PLCAddress.RD_TimeStamp = jsonAddressRecipe["RD_TimeStamp"].ToString() == "" ? "W134" : jsonAddressRecipe["RD_TimeStamp"].ToString();
                PLCAddress.RD_ProcRecipe = jsonAddressRecipe["RD_ProcRecipe"].ToString() == "" ? "W134" : jsonAddressRecipe["RD_ProcRecipe"].ToString();
                PLCAddress.RD_ProcCarrierID = jsonAddressRecipe["RD_ProcCarrierID"].ToString() == "" ? "W134" : jsonAddressRecipe["RD_ProcCarrierID"].ToString();
                PLCAddress.RD_ProcCarrierPosX = jsonAddressRecipe["RD_ProcCarrierPosX"].ToString() == "" ? "W134" : jsonAddressRecipe["RD_ProcCarrierPosX"].ToString();
                PLCAddress.RD_ProcCarrierPosY = jsonAddressRecipe["RD_ProcCarrierPosY"].ToString() == "" ? "W134" : jsonAddressRecipe["RD_ProcCarrierPosY"].ToString();
                PLCAddress.RD_ProcCarrierPosZ = jsonAddressRecipe["RD_ProcCarrierPosZ"].ToString() == "" ? "W134" : jsonAddressRecipe["RD_ProcCarrierPosZ"].ToString();
                PLCAddress.RD_ProcCarrierCounter = jsonAddressRecipe["RD_ProcCarrierCounter"].ToString() == "" ? "W134" : jsonAddressRecipe["RD_ProcCarrierCounter"].ToString();
                PLCAddress.RD_ToolID = jsonAddressRecipe["RD_ToolID"].ToString() == "" ? "W134" : jsonAddressRecipe["RD_ToolID"].ToString();
                PLCAddress.RD_TrackID = jsonAddressRecipe["RD_TrackID"].ToString() == "" ? "W134" : jsonAddressRecipe["RD_TrackID"].ToString();
                PLCAddress.RD_JOBID = jsonAddressRecipe["RD_JOBID"].ToString() == "" ? "W134" : jsonAddressRecipe["RD_JOBID"].ToString();

                PLCAddress.WB_SYNC = jsonAddressRecipe["WB_SYNC"].ToString() == "" ? "W50" : jsonAddressRecipe["WB_SYNC"].ToString();
                PLCAddress.WB_VENDER = jsonAddressRecipe["WB_VENDER"].ToString() == "" ? "W50" : jsonAddressRecipe["WB_VENDER"].ToString();
                PLCAddress.WB_BLOCK = jsonAddressRecipe["WB_BLOCK"].ToString() == "" ? "W50" : jsonAddressRecipe["WB_BLOCK"].ToString();

                PLCAddress.WB_STA_BACK = jsonAddressRecipe["WB_STA_BACK"].ToString() == "" ? "W50" : jsonAddressRecipe["WB_STA_BACK"].ToString();
                PLCAddress.WB_GRS_BACK = jsonAddressRecipe["WB_GRS_BACK"].ToString() == "" ? "W51" : jsonAddressRecipe["WB_GRS_BACK"].ToString();
                PLCAddress.WB_RST_BACK = jsonAddressRecipe["WB_RST_BACK"].ToString() == "" ? "W52" : jsonAddressRecipe["WB_RST_BACK"].ToString();
                PLCAddress.WB_TRG_BACK = jsonAddressRecipe["WB_TRG_BACK"].ToString() == "" ? "W53" : jsonAddressRecipe["WB_TRG_BACK"].ToString();
                PLCAddress.WB_CHR_BACK = jsonAddressRecipe["WB_CHR_BACK"].ToString() == "" ? "W54" : jsonAddressRecipe["WB_CHR_BACK"].ToString();
                PLCAddress.WB_Acknowledge = jsonAddressRecipe["WB_Acknowledge"].ToString() == "" ? "W54" : jsonAddressRecipe["WB_Acknowledge"].ToString();
                PLCAddress.WB_TOK = jsonAddressRecipe["WB_TOK"].ToString() == "" ? "W55" : jsonAddressRecipe["WB_TOK"].ToString();
                PLCAddress.WB_RES = jsonAddressRecipe["WB_RES"].ToString() == "" ? "W56" : jsonAddressRecipe["WB_RES"].ToString();
                PLCAddress.WB_STP = jsonAddressRecipe["WB_STP"].ToString() == "" ? "W57" : jsonAddressRecipe["WB_STP"].ToString();
                PLCAddress.WB_WRN = jsonAddressRecipe["WB_WRN"].ToString() == "" ? "W58" : jsonAddressRecipe["WB_WRN"].ToString();

                PLCAddress.WD_ToolID1 = jsonAddressRecipe["WD_ToolID1"].ToString() == "" ? "W58" : jsonAddressRecipe["WD_ToolID1"].ToString();
                PLCAddress.WD_ToolID2 = jsonAddressRecipe["WD_ToolID2"].ToString() == "" ? "W58" : jsonAddressRecipe["WD_ToolID2"].ToString();

                PLCAddress.WD_Quality1 = jsonAddressRecipe["WD_Quality1"].ToString() == "" ? "W58" : jsonAddressRecipe["WD_Quality1"].ToString();
                PLCAddress.WD_Quality2 = jsonAddressRecipe["WD_Quality2"].ToString() == "" ? "W58" : jsonAddressRecipe["WD_Quality2"].ToString();
                PLCAddress.WD_SimResult = jsonAddressRecipe["WD_SimResult"].ToString() == "" ? "W58" : jsonAddressRecipe["WD_SimResult"].ToString();
                PLCAddress.WD_SampleCounter = jsonAddressRecipe["WD_SampleCounter"].ToString() == "" ? "W58" : jsonAddressRecipe["WD_SampleCounter"].ToString();
                PLCAddress.WD_ValueA = jsonAddressRecipe["WD_ValueA"].ToString() == "" ? "W58" : jsonAddressRecipe["WD_ValueA"].ToString();
                PLCAddress.WD_ValueB = jsonAddressRecipe["WD_ValueB"].ToString() == "" ? "W58" : jsonAddressRecipe["WD_ValueB"].ToString();
                PLCAddress.WD_ValueC = jsonAddressRecipe["WD_ValueC"].ToString() == "" ? "W58" : jsonAddressRecipe["WD_ValueC"].ToString();
                PLCAddress.WD_ValueD = jsonAddressRecipe["WD_ValueD"].ToString() == "" ? "W58" : jsonAddressRecipe["WD_ValueD"].ToString();

                PLCAddress.WD_PosX = jsonAddressRecipe["WD_PosX"].ToString() == "" ? "W58" : jsonAddressRecipe["WD_PosX"].ToString();
                PLCAddress.WD_PosY = jsonAddressRecipe["WD_PosY"].ToString() == "" ? "W58" : jsonAddressRecipe["WD_PosY"].ToString();
                PLCAddress.WD_PosT = jsonAddressRecipe["WD_PosT"].ToString() == "" ? "W58" : jsonAddressRecipe["WD_PosT"].ToString();

                PLCAddress.WD_WaferID = jsonAddressRecipe["WD_WaferID"].ToString() == "" ? "W61" : jsonAddressRecipe["WD_WaferID"].ToString();

                PLCAddress.ReadAddress = new string[20] { PLCAddress.RB_STA, PLCAddress.RB_GRS, PLCAddress.RB_RST, PLCAddress.RB_TRG, PLCAddress.RB_CHR,
                    PLCAddress.RB_BYP, PLCAddress.RB_SEC, PLCAddress.RD_LamaID, PLCAddress.RD_WaferID, PLCAddress.RD_InfoText, PLCAddress.RD_TimeStamp,
                    PLCAddress.RD_ProcRecipe, PLCAddress.RD_ProcCarrierID, PLCAddress.RD_ProcCarrierPosX, PLCAddress.RD_ProcCarrierPosY, PLCAddress.RD_ProcCarrierPosZ,
                    PLCAddress.RD_ProcCarrierCounter, PLCAddress.RD_ToolID, PLCAddress.RD_TrackID, PLCAddress.RD_JOBID};
            }
            catch (Exception e)
            {
                string msg = e.Message;
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], msg);
            }
        }

        /// <summary>
        /// 24.06.04 LYK ModelChange 함수
        /// 레시피 번호에 맞게 ModelChange를 진행한다.
        /// </summary>
        /// <param name="_ModelNo"></param>
        /// <returns></returns>

        public bool ModelCahnge(string _ModelNo)
        {
            bool bModelCheck = false; // 230929 LYK Model 있는지 여부 체크

            System.IO.DirectoryInfo diInfo = new System.IO.DirectoryInfo(DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE);

            foreach (System.IO.DirectoryInfo dir in diInfo.GetDirectories())
            {
                string[] sToken = dir.Name.Split('_');

                if (sToken[0] == _ModelNo)
                {
                    theRecipe.m_sCurrentModelName = dir.Name;
                    theRecipe.DataLoad(theRecipe.m_sCurrentModelName);
                    theRecipe.ToolLoad(theRecipe.m_sCurrentModelName);
                    theMainSystem.RefreshCurrentModelName?.Invoke(theRecipe.m_sCurrentModelName);


                    bModelCheck = true;
                }
            }

            if (bModelCheck == false)
                MessageBox.Show("Not Exist Model. Please Create a Recipe!!");

            return bModelCheck;
        }
    }
}
