using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using InsCHVSControl;

namespace Core.HardWare
{
    public class CInsCIS : CCamera
    {
        private IntPtr CamHandle = IntPtr.Zero;                 //250406 LYK CamHandle
        private InsCHVS_DeviceInfoList deviceInfoListScanned;   
        private InsCHVS_DeviceInfo deviceInfoOpened;
        private const int TIMEOUT = 3000;
        List<IntPtr> srcData = new List<IntPtr>();
        private bool isLXMSeriesCamera = false;
        private CCamera Camera = null;
        private int GrabCount = 0;

        public int m_nCamNumber = 0;
        public string RecipePath = string.Empty;

        public override bool Initialize(CCamera _Camera)
        {
            InsCHVSCamera.InsCHVS_Initialize_NET();
            InsCHVSCamera.InsCHVS_CreateHandle_NET(out CamHandle);

            Console.WriteLine("Insnex CIS SDK Initialized.");

            return m_bIsIntialized;
        }

        public override void Uninitialize()
        {
            if(m_bIsIntialized)
            {
                InsCHVSCamera.InsCHVS_DestroyHandle_NET(CamHandle);

                if (m_bIsConnected)
                {
                    if (m_bIsGrab)
                        GrabHalt();

                    DisConnect();
                }

                m_bIsIntialized = false;
            }
        }

        public override void Dispose()
        {

        }
        private int extractDPI(InsCHVS_DeviceType type)
        {
            return ((int)type & 0xF0) >> 4;
        }
        private bool is_DPI_EqualTo_1800_900(InsCHVS_DeviceType type)
        {
            return extractDPI(type) == (int)InsCHVS_DPI_MODULE.DPI900 || extractDPI(type) == (int)InsCHVS_DPI_MODULE.DPI1800 || extractDPI(type) == (int)InsCHVS_DPI_MODULE.DPI3600;
        }

        public override bool Connect(CCameraRecipe _Recipe)
        {
            if(!m_bIsIntialized)
            {
                InsCHVSCamera.InsCHVS_FindDevice_NET(out deviceInfoListScanned);

                var ret = InsCHVSCamera.InsCHVS_Cmd_Open_NET(CamHandle, 0);
                Console.WriteLine($"CIS Camera Open Result : {ret}");

                InsCHVSCamera.InsCHVS_Get_DevInfo_NET(CamHandle, out deviceInfoOpened);

                CameraSetting(_Recipe);

                var (bufferHeight, resultH) = InsCHVSCamera.InsCHVS_Get_DevPrm<int>(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_ROI_HEIGHT);
                var (bufferStride, resultW) = InsCHVSCamera.InsCHVS_Get_DevPrm<int>(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_ROI_IMG_STRIDE);
                isLXMSeriesCamera = is_DPI_EqualTo_1800_900(deviceInfoListScanned.DeviceInfo[0].InsType);

                m_nImgWidth = bufferStride;
                m_nImgHeight = bufferHeight;

                for(int i = 0; i < 4; i++)
                {
                    IntPtr ptr = Marshal.AllocHGlobal((int)m_nImgWidth * (int)m_nImgHeight);
                    srcData.Add(ptr);
                }

                m_nStride = (int)(4 * ((m_nImgWidth * 1 + 3) / 4));

                RegisterCallbacks();

                m_bIsConnected = true;


                return m_bIsConnected;
            }

            return m_bIsConnected;
        }

        public override void DisConnect()
        {
            if(m_bIsConnected)
            {
                var ret = InsCHVSCamera.InsCHVS_Cmd_Close_NET(CamHandle);
                Console.WriteLine($"Camera Disconnect = {ret}");
                InsCHVSCamera.InsCHVS_DestroyHandle_NET(CamHandle);

                m_bIsConnected = false;
            }  
        }

        // 250406 LYK 파라미터 설정 (레시피 객체 기반)
        private void SetParameters(CCameraRecipe recipe)
        {
            var ret = InsCHVSCamera.InsCHVS_Set_Img_TransHeight_NET(CamHandle, recipe.ImageHeight, TIMEOUT);
            Console.WriteLine($"Set_Img_TransHeight: {ret}");

            ret = InsCHVSCamera.InsCHVS_Set_LED_TriggerMode_NET(CamHandle, recipe.LedMode, TIMEOUT);
            Console.WriteLine($"Set_LED_TriggerMode: {ret}");

            ret = InsCHVSCamera.InsCHVS_Set_Acq_TrigSource_NET(CamHandle, recipe.Trigger1Source, TIMEOUT);
            Console.WriteLine($"Set_Acq_TrigSource: {ret}");

            var (periodMax, result) = InsCHVSCamera.InsCHVS_Get_DevPrm<float>(CamHandle, (int)InsCHVSCamera.INS_PRM_PERIOD_DPI_MAX);
            float UserControl_Trigger1_LineFrequency = Math.Min(25, periodMax);
            ret = InsCHVSCamera.InsCHVS_Set_Acq_Intern_TrigPeriod_NET(CamHandle, UserControl_Trigger1_LineFrequency, TIMEOUT);
            Console.WriteLine($"Set_Acq_Intern_TrigPeriod: {ret}");            

            ret = InsCHVSCamera.InsCHVS_Set_Acq2_Enable_NET(CamHandle, recipe.Trigger2Enabled, TIMEOUT);
            Console.WriteLine($"InsCHVS_Set_Acq2_Enable: {ret}");

            //Trigger Source 1 설정
            if (recipe.Trigger1Source == InsCHVS_LineTriggerSource.Internal_Clock)
            {
                
            }
            else if (recipe.Trigger1Source == InsCHVS_LineTriggerSource.External_Encoder)
            {

            }
            else if (recipe.Trigger1Source == InsCHVS_LineTriggerSource.External_IO)
            {
                InsCHVS_IO_TriggerMode ioMode = recipe.IOTriggerMode;

                ret = InsCHVSCamera.InsCHVS_Set_Acq_IO_Mode_NET(CamHandle, ioMode, TIMEOUT);
                Console.WriteLine($"InsCHVS_Set_Acq_IO_Mode: {ret}");

                ret = InsCHVSCamera.InsCHVS_Set_Acq_IO_TrigPeriod_NET(CamHandle, recipe.ioFreq, TIMEOUT);
                Console.WriteLine($"InsCHVS_Set_Acq_IO_TrigPeriod: {ret}");

                ret = InsCHVSCamera.InsCHVS_Set_Acq_IO_TrigNums_NET(CamHandle, recipe.IO_Trig_Rows_Num, TIMEOUT);
                Console.WriteLine($"InsCHVS_Set_Acq_IO_TrigNums: {ret}");

                ret = InsCHVSCamera.InsCHVS_Set_Acq_IO_OutputDelay_NET(CamHandle, recipe.IO_Trig_Output_Delay, TIMEOUT);
                Console.WriteLine($"InsCHVS_Set_Acq_IO_TrigNums: {ret}");

            }


            if (recipe.Trigger2Enabled == InsCHVS_FuncEnable.Ins_Enable)
            {
                // Trigger Source 2 설정
                ret = InsCHVSCamera.InsCHVS_Set_Acq2_Enable_NET(CamHandle, recipe.Trigger2Enabled, TIMEOUT);
                Console.WriteLine($"InsCHVS_Set_Acq2_Enable: {ret}");

                InsCHVS_LineTriggerSource trigger2Mode = recipe.Trigger2Source;
                ret = InsCHVSCamera.InsCHVS_Set_Acq2_TrigSource_NET(CamHandle, trigger2Mode, TIMEOUT);
                Console.WriteLine($"InsCHVS_Set_Acq2_TrigSource: {ret}");

                if (trigger2Mode == InsCHVS_LineTriggerSource.Internal_Clock)
                {
                    float trigger2Freq = Math.Min(10, UserControl_Trigger1_LineFrequency);
                    ret = InsCHVSCamera.InsCHVS_Set_Acq2_Intern_TrigPeriod_NET(CamHandle, trigger2Freq, TIMEOUT);
                    Console.WriteLine($"InsCHVS_Set_Acq_Intern_TrigPeriod: {ret}");

                    ret = InsCHVSCamera.InsCHVS_Set_Acq2_Intern_TrigNums_NET(CamHandle, 0, TIMEOUT);
                    Console.WriteLine($"InsCHVS_Set_Acq2_Intern_TrigNums: {ret}");
                }
                else if (trigger2Mode == InsCHVS_LineTriggerSource.External_Encoder)
                {
                    InsCHVS_Enocder_CountMode countMode = recipe.EncoderCountMode;
                    InsCHVS_Encoder_TriggerMode travelMode = recipe.EncoderTravelMode;

                    ret = InsCHVSCamera.InsCHVS_Set_Acq2_Encoder_CountMode_NET(CamHandle, countMode, TIMEOUT);
                    Console.WriteLine($"InsCHVS_Set_Acq2_Encoder_CountMode: {ret}");

                    ret = InsCHVSCamera.InsCHVS_Set_Acq2_Encoder_TriggerMode_NET(CamHandle, travelMode, TIMEOUT);
                    Console.WriteLine($"InsCHVS_Set_Acq2_Encoder_TriggerMode: {ret}");

                    InsCHVSCamera.InsCHVS_Set_Acq2_Encoder_Ignore_NET(CamHandle, recipe.Trigger2_Encoder_IgnoreCount, TIMEOUT); // Set as needed. Generally, default is sufficient.
                    InsCHVSCamera.InsCHVS_Set_Acq2_Encoder_FilterWidth_NET(CamHandle, recipe.Trigger2_Encoder_FilteringTimeWidth, TIMEOUT); // Set as needed. Generally, default is sufficient.
                    InsCHVSCamera.InsCHVS_Set_Acq2_Encoder_InputDiv_NET(CamHandle, recipe.Trigger2_Divide, TIMEOUT); // Set as needed
                    InsCHVSCamera.InsCHVS_Set_Acq2_Encoder_Enable_InputMultiple_NET(CamHandle, recipe.Trigger2_Enable_InputMultiple, TIMEOUT); // Set as needed
                    InsCHVSCamera.InsCHVS_Set_Acq2_Encoder_InputMultiple_NET(CamHandle, recipe.Trigger2_InputMultiple, TIMEOUT); // Set as needed
                }
                else if (trigger2Mode == InsCHVS_LineTriggerSource.External_IO)
                {
                    InsCHVS_IO_TriggerMode ioMode = recipe.IOTriggerMode;
                    UInt32 trigNums = 0;

                    ret = InsCHVSCamera.InsCHVS_Set_Acq2_IO_TrigPeriod_NET(CamHandle, recipe.ioFreq, TIMEOUT);
                    Console.WriteLine($"InsCHVS_Set_Acq2_IO_TrigPeriod: {ret}");

                    ret = InsCHVSCamera.InsCHVS_Set_Acq2_IO_Mode_NET(CamHandle, ioMode, TIMEOUT);
                    Console.WriteLine($"InsCHVS_Set_Acq2_IO_Mode: {ret}");

                    ret = InsCHVSCamera.InsCHVS_Set_Acq2_IO_TrigNums_NET(CamHandle, trigNums, TIMEOUT);
                    Console.WriteLine($"InsCHVS_Set_Acq2_IO_TrigNums: {ret}");
                }
                else
                {
                    Console.WriteLine("Not allowed mode");
                    return;
                }
            }
            

            float maxExposureTime = 0;
            //InsCHVSCamera.InsCHVS_Get_TimeLine_Max_NET(CamHandle, out maxExposureTime, TIMEOUT);

            switch (recipe.LedMode)
            {
                case InsCHVS_LED_TriggerMode.LED_SimultaneousDualBrightness:
                case InsCHVS_LED_TriggerMode.LED_BacklightOnly:
                    ret = InsCHVSCamera.InsCHVS_Set_LED_ExposureTime1_NET(CamHandle, recipe.Exposure1, TIMEOUT);
                    break;
                case InsCHVS_LED_TriggerMode.LED_SeparateTimedFlashingBrightness:
                    ret = InsCHVSCamera.InsCHVS_Set_LED_ExposureTime2_NET(CamHandle, recipe.Exposure1, recipe.Exposure2, TIMEOUT);
                    ret = InsCHVSCamera.InsCHVS_Set_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_STROBING_MODE, (int)InsCHVSCamera.INS_VAL_BUFFER_STROBING_MULTIPLE);
                    break;
                case InsCHVS_LED_TriggerMode.LED_TripleIndependentBrightness:
                    ret = InsCHVSCamera.InsCHVS_Set_LED_ExposureTime3_NET(CamHandle, recipe.Exposure1, recipe.Exposure2, recipe.Exposure3, TIMEOUT);
                    break;
            }

            int flipVal = recipe.FlipEnabled
                ? (int)InsCHVSCamera.INS_VAL_BUFFER_FLIP_HORIZONTAL
                : (int)InsCHVSCamera.INS_VAL_BUFFER_FLIP_NORMAL;

            ret = InsCHVSCamera.InsCHVS_Set_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_FLIP, flipVal);
        }

        //250406 LYK 6. ICF 파일 로드
        void LoadICF(string configPath)
        {
            var ret = InsCHVSCamera.InsCHVS_Load_ConfigFile_NET(CamHandle, configPath, TIMEOUT);
            Console.WriteLine($"Load ICF: {ret}");
        }

        public override void GrabStart()
        {
            m_bIsGrab = true;
            var ret = InsCHVSCamera.InsCHVS_Cmd_Start_NET(CamHandle);
            Console.WriteLine($"CIS Grab Start : {ret}");
        }

        public override void GrabHalt()
        {
            m_bIsGrab = false;    
            GrabCount = 0;
            var ret = InsCHVSCamera.InsCHVS_Cmd_Stop_NET(CamHandle);
            Console.WriteLine($"CIS Grab Stop : {ret}");

            //InsCHVSCamera.InsCHVS_Set_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_CLEAR, 1);
        }

        public override void CameraSetting(CCameraRecipe _Recipe)
        {
            SetParameters(_Recipe);
        }


        public override bool IsConnected()
        {
            return m_bIsConnected;
        }

        private void RegisterCallbacks()
        {
            RunHookFnPtr Hooking = FrameReadyCallBack;

            GCHandle HookingHandle = GCHandle.Alloc(Hooking);
            InsCHVSCamera.InsCHVS_RegisterCallback_NET(CamHandle, InsCHVSCamera.INS_Event_FrameReady, Hooking, IntPtr.Zero);
        }

        
        private void FrameReadyCallBack(IntPtr handle, IntPtr pInfo, IntPtr pBuffer, IntPtr pUser)
        {
            InsCHVS_ProcessInfo info = Marshal.PtrToStructure<InsCHVS_ProcessInfo>(pInfo);
            InsCHVS_Buffer buffer = Marshal.PtrToStructure<InsCHVS_Buffer>(pBuffer);

            if (!isLXMSeriesCamera)
            {
                //List<IntPtr> srcData = new List<IntPtr> { buffer.p_data, buffer.p_data2, buffer.p_data3, buffer.p_data4 };
                //srcData[0] = buffer.p_data;
                //srcData[1] = buffer.p_data2;
                //srcData[2] = buffer.p_data3;
                //srcData[3] = buffer.p_data4;

                //for (int lightNumber = 0; lightNumber < srcData.Count; lightNumber++)
                {
                    //if (srcData[lightNumber] == IntPtr.Zero) continue;
                    if (m_nImgCnt == GrabCount)
                        GrabCount = 0;

                    GrabInfo.Image = buffer.p_data;// srcData[lightNumber];
                    GrabInfo.ImgWidth = (int)buffer.width;
                    GrabInfo.ImgHeight = (int)buffer.height;
                    GrabInfo.Stride = (int)m_nStride;
                    GrabInfo.CamNum = m_nCamNumber;
                    GrabInfo.m_nGrabIndex = GrabCount++;
                    GrabInfo.PixelFormat = 1;

                    

                    CamActionInfo?.Invoke(CALLTYPE.GrabEnd, GrabInfo);
                }
            }
        }
    }
}
