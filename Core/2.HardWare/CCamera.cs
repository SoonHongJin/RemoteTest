using System;
using InsCHVSControl;
using ConnectedInsightCore;

namespace Core.HardWare
{
    public enum CALLTYPE { Connect = 0, GrabEnd }

    /// <summary> LYK 22.05.02 CGrabInfo Class
    /// Grab정보를 담고 있는 클래스
    /// </summary>
    public class CGrabInfo
    {
        public int CamNum { get; set; } = 0;                        //220502 LYK Camera ID(switch ~ case) 0 : Line Cam 1 : Basler Cam
        public int ImgWidth { get; set; } = 0;                      //220502 LYK 이미지 가로 사이즈
        public int ImgHeight { get; set; } = 0;                     //220502 LYK 이미지 세로 사이즈
        public int Stride { get; set; } = 0;                        //220502 LYK Stride 
        public int PixelFormat { get; set; } = 0;                   //220502 LYK 픽셀 포맷
        public int BufferSize { get; set; } = 0;                    //220502 LYK 이미지 버퍼 사이즈 
        public IntPtr Image { get; set; } = IntPtr.Zero;            //220502 LYK 이미지

        public int m_nGrabIndex = 0;                                //220502 LYK 이미지 한줄의 크기

        public byte[] byteImg;

        public string Comment = string.Empty;
    }

    /// <summary>
    /// 25.04.06 LYK CameraRecipe 클래스
    /// </summary>
    public class CCameraRecipe
    {
        public uint ImageHeight { get; set; } = 3000;
        public float Exposure1 { get; set; } = 10;
        public float Exposure2 { get; set; } = 10;
        public float Exposure3 { get; set; } = 10;
        public bool FlipEnabled { get; set; } = false;
        public InsCHVS_FuncEnable Trigger2Enabled { get; set; } = InsCHVS_FuncEnable.Ins_Disable;

        public InsCHVS_LED_TriggerMode LedMode { get; set; } = InsCHVS_LED_TriggerMode.LED_SimultaneousDualBrightness;
        public InsCHVS_LineTriggerSource Trigger1Source { get; set; } = InsCHVS_LineTriggerSource.Internal_Clock;
        public InsCHVS_LineTriggerSource Trigger2Source { get; set; } = InsCHVS_LineTriggerSource.Internal_Clock;

        public InsCHVS_Enocder_CountMode EncoderCountMode { get; set; } = InsCHVS_Enocder_CountMode.PhA_RisingEdge_OneWay;
        public InsCHVS_Encoder_TriggerMode EncoderTravelMode { get; set; } = InsCHVS_Encoder_TriggerMode.Forward_Scan;

        //IO_Trigger----------------------------------------------------------------------------------------------
        public InsCHVS_IO_TriggerMode IOTriggerMode { get; set; } = InsCHVS_IO_TriggerMode.Input0_MultiFrames;
        public float ioFreq { get; set; } = 10;
        public uint IO_Trig_Rows_Num { get; set; } = 0;
        public float IO_Trig_Output_Delay { get; set; } = 0;
        //-------------------------------------------------------------------------------------------------------

        public uint Trigger2_Encoder_IgnoreCount { get; set; } = 0;
        public float Trigger2_Encoder_FilteringTimeWidth { get; set; } = (float)1.28;
        public uint Trigger2_Divide { get; set; } = 1;
        public InsCHVS_FuncEnable Trigger2_Enable_InputMultiple { get; set; } = InsCHVS_FuncEnable.Ins_Disable;

        public uint Trigger2_InputMultiple { get; set; } = 1;
        public float TrigPeriod { get; set; } = 100f;


    }

    /// <summary> LYK 22.05.2 CCamera 클래스
    /// 모든 카메라에 관련한 부모 클래스
    /// 추상 클래스로 구현 - 카메라 동작 특성상 공통된 기능으로 나열 가능 하고 다중 상속이 필요 없다고 판단 되어 추상 클래스로 구성(기능 구현 강제 목적)
    /// Dispose, Initialize, UnInitialize, Connect, DisConnect, GrabStart, GrabHalt, CameraSetting 함수의 구현 강제
    /// </summary>
    public abstract class CCamera : IDisposable
    {
        public long m_nImgWidth = 0;                            //220502 LYK 이미지 가로 사이즈    
        public long m_nImgHeight = 0;                           //220502 LYK 이미지 세로 사이즈
        public int m_nPixelFormat = 0;                          //220502 LYK PixelFormat
        public int m_nStride = 0;                               //220502 LYK 이미지 한줄의 크기
        protected int m_nExposureTimeMin = 0;                   //220502 LYK ExposureTime 최소값 (필요 없을 시 삭제)
        protected int m_nExposureTimeMax = 0;                   //220502 LYK ExposureTime 최대값 (필요 없을 시 삭제)
        protected int m_nExposureTimeInc = 0;                   //220502 LYK ExposureTime  (필요 없을 시 삭제)
        protected bool m_bIsIntialized = false;                 //220502 LYK Initialize Check 변수
        protected bool m_bIsConnected = false;                  //220502 LYK Camera Connect Check 변수
        public bool m_bIsGrab = false;                          //220502 LYK Camera Grab 중인지에 관련한 Check 변수
        protected string m_sSerialNumber = string.Empty;        //220502 LYK Serial Number GIGE 카메라에서 사용
        protected string m_sIPAddress = string.Empty;           //240912 LYK Camera IP Address

        public int m_nImgCnt = 0;

        public string m_sCurrentEquipment = string.Empty;   //240314 LYK 초기화 할때 장비명(SORTER, CVD, PRINTER) 대입

        public CGrabInfo GrabInfo = new CGrabInfo();                 //220502 LYK Grab후 관련된 정보를 담기위한 객체
        public Action<CALLTYPE, object> CamActionInfo { get; set; } = null; //220502 LYK Camera 동작 상태(연결, 그랩 완료(그랩, Live) )

        abstract public void Dispose();
        abstract public bool Initialize(CCamera _Camera); //220502 LYK 카메라 Initial 함수
        abstract public void Uninitialize();                           //220502 LYK 카메라 UnInitial 함수
        abstract public bool Connect(CCameraRecipe _Recipe);                                //220502 LYK 카메라 Connect 함수
        abstract public void DisConnect();                             //220502 LYK 카메라 DisConnect 함수
        abstract public void GrabStart();                              //220502 LYK 카메라 GrabStart 함수
        abstract public void GrabHalt();                               //220502 LYK 카메라 GrabStop 함수
        abstract public void CameraSetting(CCameraRecipe _Recipe);   //220502 LYK 카메라의 Exposure Time, Gain 등 세팅 하는 함수
        abstract public bool IsConnected();                             //220502 LYK 카메라 연결 상태 체크 함수
        /// <summary> LYK 22.05.2 CCamera 클래스
        /// 카메라 SDK에 따라 객체 리턴 - 필요에 따라 SDK 추가 구현 하면 됨
        /// </summary>
        /// <param name="nCameraType"> 카메라 SDK 선택(Matrox, Basler, Cognex 등) ( </param>
        /// <param name="nCamNum"> 카메라의 번호(ex : nCamnNum : 0 -> Cam1)</param>
        /// <returns> 카메라 타입에 따라 해당 객체 리턴 </returns>
        public static CCamera CameraPick(int nCameraType, int nCamNum, string sPath)
        {
            switch (nCameraType)
            {
                case CDefineDLL.SIMULATION_CAM:
                    return new CCameraSimul() { m_nCamNumber = nCamNum };


                case CDefineDLL.INS_CIS:
                    return new CInsCIS() { m_nCamNumber = nCamNum };
            }

            return null;
        }
    }
}
