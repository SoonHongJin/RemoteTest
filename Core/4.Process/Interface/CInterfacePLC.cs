using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

using static Core.Program;
using Core.HardWare;
using System.Windows.Forms;

namespace Core.Process
{
    public class CInterfacePLC
    {
        /////////////////////////////////////////// 
        // Thread 
        private CThread m_HanShakePLC = null;
        private bool m_isStart = false;

        private bool m_bisFirst ; 
        public bool m_bIsTRGFlag { get; private set; }
        private bool m_bIsSTAFlag = false;                  //231012 LYK Status Flag
        private bool m_bIsGRSFlag = false;                  //231012 LYK Get Result Falg
        private bool m_bIsRSTFlag = false;

        public string WaferID = string.Empty;
        public bool m_bReconnectRun = false;    //250226NWT Reconnect 중복 방지
        private long GrabCycleTime = 1000;

        // Comm Device 
        public CMdfuncPLC m_PLC = new CMdfuncPLC();
        private InspectionInfo InspInfos = new InspectionInfo();
        private DateTime m_LastTrigTime;

        public MainForm MainForm = null;

        /////////////////////////////////////////// 
        // Using Variable
        public bool m_bConnected = false;
        

        public Action<string, string> RefreshPLCReadBitData = null;

        private CLogger Logger = null;
        public CInterfacePLC(MainForm _MainForm, CLogger _Logger)
        {
            MainForm = _MainForm;
            Logger = _Logger;
        }

        public void Connect()
        {
            m_PLC.m_nPlcMode = DEF_SYSTEM.PLC_MODE;

            int ret = m_PLC.ConnectPLC();

            //240831 NIS Form Load 완료 후 HandShake 시작
            MainForm.PLC_HandShake_Start += SetHandShakePLC;
            if (ret == 0)
            {
                // Ready On 
                m_bConnected = true;

                // PLC 주소 정의 
                AddressDefine();
            }
        }

        public void Reconnect()
        {
            int ret = m_PLC.ReConnectPLC();

            if (ret == 0)
            {
                // Ready On 
                m_bConnected = true;

                // PLC 주소 정의 
                AddressDefine();
                //250225 NWT 재연결 시 HandShake Event가 없을 경우 추가한 다음 Invoke
                if(MainForm.PLC_HandShake_Start != null)
                    MainForm.PLC_HandShake_Start.Invoke();
                else
                {
                    MainForm.PLC_HandShake_Start += SetHandShakePLC;
                    MainForm.PLC_HandShake_Start.Invoke();
                }
            }
            m_bReconnectRun = false;
        }
        
        public void Disconnect()
        {
            if(MainForm.PLC_HandShake_Start != null)
                MainForm.PLC_HandShake_Start -= SetHandShakePLC;
            m_PLC.DisconnectPLC();
            if (m_HanShakePLC != null)
                m_HanShakePLC.ThreadStop();
            //250225 NWT 연결 해제 시 List clear 실행
            m_PLC.list_ReadValue.Clear();
            m_PLC.list_WriteValue.Clear();
            m_PLC.list_WriteValueEx.Clear();
        }

        public void SetHandShakePLC()
        {
            if(m_bConnected == true)
            {
                m_HanShakePLC = new CThread()
                {
                    Work = InterfaceLoop,
                    nSleepTime = 1
                };

                m_HanShakePLC.ThreadStart();
                m_HanShakePLC.SeriesSet();
            }        
        }

        public void InterfaceLoop()
        {
            ReadData();
            Handshake();
            WriteData();
            HandshakeUI();
        }

        public void ReadData()
        {
            m_PLC.ReadPLC();
        }

        public void WriteData()
        {
            m_PLC.WritePLC();
        }

        public void Handshake()
        {
            string line;
            if (m_bisFirst == false)
            {
                MakeWriteFlag("WB_STA_BACK", 1);
                MakeWriteFlag("WB_GRS_BACK", 255);
                MakeWriteFlag("WB_RST_BACK", 255);
                MakeWriteFlag("WB_TRG_BACK", 255);
                MakeWriteFlag("WB_CHR_BACK", 255);
                MakeWriteFlag("WB_TOK", 1);
                MakeWriteFlag("WB_RES", 255);
                MakeWriteFlag("WB_WRN", 255);
                MakeWriteFlag("WB_STP", 255);

                //MakeWriteFlag(1, 255, 255, 255, 255, 1, 255, 255, 255);
                MakeWriteData(255, 255, 65535, 65535, 65535, "");

                m_bisFirst = true;
            }

            if (GetB("RB_STA") == 1 && m_bIsSTAFlag == false)
            {
                //STA 신호가 들어온 경우 STA Back Flag와 TOK Flag를 1로 만든다.
                m_bIsSTAFlag = true;
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"[SC->MS]Receive STA Command. Data : {1}");

                MakeWriteFlag("WB_STA_BACK", 1);
                MakeWriteFlag("WB_TOK", 1);

                //MakeWriteFlag(1, 255, 255, 255, 255, 1, 255, 255, 255);

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[MS->SC]Send STA Command Back : {1}");
            }
            else if (GetB("RB_STA") == 255 && m_bIsSTAFlag == true)
            {
                m_bIsSTAFlag = false;

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[SC->MS]Receive STA Command Idle : {255}");

                MakeWriteFlag("WB_STA_BACK", 255);
                //MakeWriteFlag(255, 255, 255, 255, 255, 1, 255, 255, 255);
                MakeWriteData(255, 255, 65535, 65535, 65535, "");

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[MS->SC]Send STA Command Back Idle : {255}");
            }

            if (GetB("RB_RST") == 1 && m_bIsRSTFlag == false)
            {
                //RST 신호가 1 들어온 경우 RST Back Flag를 1로 만든다.
                m_bIsRSTFlag = true;
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[SC->MS]Action: Receive, Command: RST, Data: 1");


                MakeWriteFlag("WB_STA_BACK", 255);
                MakeWriteFlag("WB_GRS_BACK", 255);
                MakeWriteFlag("WB_RST_BACK", 1);
                MakeWriteFlag("WB_TRG_BACK", 255);
                MakeWriteFlag("WB_CHR_BACK", 255);
                MakeWriteFlag("WB_TOK", 1);
                MakeWriteFlag("WB_RES", 255);
                MakeWriteFlag("WB_WRN", 255);
                MakeWriteFlag("WB_STP", 255);

                //MakeWriteFlag(255, 255, 1, 255, 255, 1, 255, 255, 255);

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[MS->SC]Action: Send, Command: RST Back, Data: 1");
            }
            else if (GetB("RB_RST") == 255 && m_bIsRSTFlag == true)
            {
                m_bIsRSTFlag = false;
                //RST 신호가 255 들어온 경우 RST Back Flag를 255로 만든다.
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[SC->MS]Action: Receive, Command: RST, Data: 255");

                MakeWriteFlag("WB_RST_BACK", 255);
                //MakeWriteFlag(1, 255, 255, 255, 255, 1, 255, 255, 255);
                MakeWriteData(255, 255, 65535, 65535, 65535, "");

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[MS->SC]Action: Send, Command: RST Back, Data: 255");
            }
            //MakeWriteFlag(255, 255, 1, 255, 255, 1, 255, 255, 255);

            /* 23.10.12 LYK 
            SC에서 MS로 Trigger 커맨드 1과 WaferID 등의 정보를 송신 한다.
            이때 MS는 Trigger 커맨드를 수신했을때, Wafer ID 등의 정보를 받고 Trigger Back 커맨드를 송신 한다.
            Trigger Back Commad는 두가지가 있으며, 1은 Trigger Accept, 2는 Trigger Reject를 의미 한다.           
            */
            //m_bIsTRGFlag = !m_bIsTRGFlag;
            if (GetB("RB_TRG") == 1 && m_bIsTRGFlag == false)
            {
                m_bIsTRGFlag = true;

                theMainSystem.ProductInfo.m_sProductName = WaferID = m_PLC.GetWaferID("RD_WaferID", 18); //231012 LYK Wafer ID를 받아온다.
                //theMainSystem.ProductInfo.m_sProductName = WaferID = m_PLC.GetWaferID("RD_WaferID", 9); //231012 LYK Wafer ID를 받아온다.
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[SC->MS]Action: Get, Command: WaferID, Data:  {theMainSystem.ProductInfo.m_sProductName}");
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[SC->MS]Action: Receive, Command: TRG, Data: 1");

                theMainSystem.DoInspectionStart();

                MakeWriteFlag("WB_STA_BACK", 0x02);
                MakeWriteFlag("WB_TOK", 255);

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[MS->SC]Action: Send, Command: TOK, Data: 255");
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[MS->SC]Action: Send, Command: STA Back, Data: 2");
                //Trigger Back Flag를 1로 살리고 TOK(Transprot OK)를 255로 변경하여 Wafer가 움직이지 않도록 한다. TOK 1은 Wafer가 움직여도 된다는 뜻.

                //MakeWriteData(0xFF, 0xFF, 65535, 65535, 65535, "");    //231012 LYK 검사 결과를 도출 하기전이기 때문에 모든 데이터 값은 초기값으로 세팅 해준다.

                //theMainSystem.Logging($"[MS->SC]Send TRG Command Back : {1} Trigger Accepted");
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[MS->SC]Send TRG Command Back : {1} Trigger Accepted");
            }
            


            else if (GetB("RB_TRG") == 255 && m_bIsTRGFlag == true)
            {
                m_bIsTRGFlag = false;

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[SC->MS]Action: Receive, Command: TRG, Data: 255");
                MakeWriteFlag("WB_TRG_BACK", 255);
                MakeWriteFlag("WB_TOK", 1);
                MakeWriteFlag("WB_STA_BACK", 1);
                //MakeWriteFlag(0x01, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, 0xFF, 0xFF, 0xFF);

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[MS->SC]Action: Receive, Command: TRG, Data: 255");
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[MS->SC]Action: Send, Command: STA Back, Data: 1");
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[MS->SC]Action: Send, Command: TOK, Data: 1");

            }

            if (GetB("RB_GRS") == 1 && m_bIsGRSFlag == false)
            {
                m_bIsGRSFlag = true;
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[SC->MS]Action: Send, Command: TOK, Data: 1");

                MakeWriteFlag("WB_GRS_BACK", 1);
                MakeWriteFlag("WB_RES", 255);
                //MakeWriteFlag(255, 1, 255, 255, 255, 1, 255, 255, 255);

                //240205 LYK InspInfos 객체를 이용하여 Mono와 DT Judge 구분(Mono가 NG이고 DT가 NG일때 0x05 송신)
                //if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.SORTER || theRecipe.m_sCurrentEquipment == DEF_SYSTEM.CVD)
                //MakeWriteData(theRecipe.m_nWriteMonoDtOK, InspInfos.ColorJudge, (int)(InspInfos.dAlignX * 1000), (int)(InspInfos.dAlignY * 1000), (int)(InspInfos.dAlignT * 1000), InspInfos.WaferID); // 10 : OK  

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[MS->SC]Action: Send, Command: GRS Back, Data: 1");
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[MS->SC]Action: Send, Command: RES, Data: 255");
            }
            else if (GetB("RB_GRS") == 255 && m_bIsGRSFlag == true)
            {
                m_bIsGRSFlag = false;

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[SC->MS]Action: Receive, Command: GRS , Data: 255");

                MakeWriteFlag("WB_GRS_BACK", 255);
                //MakeWriteFlag(1, 255, 255, 255, 255, 1, 255, 255, 255);
                //MakeWriteData(0xFF, 0xFF, 65535, 65535, 65535, ""); 
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.InterfaceLog], $"[MS->SC]Action: Send, Command: GRS Back , Data: 255");
            }
        }

        //240902 NIS SettingPage PLC UI 갱신
        private void HandshakeUI()
        {
            if (DEF_SYSTEM.REALTIME_COMMUNICATION_ACTIVATION)
            {
                RefreshPLCReadBitData(GetB("RB_STA").ToString(), "RB_STA");
                RefreshPLCReadBitData(GetB("RB_GRS").ToString(), "RB_GRS");
                RefreshPLCReadBitData(GetB("RB_RST").ToString(), "RB_RST");
                RefreshPLCReadBitData(GetB("RB_TRG").ToString(), "RB_TRG");
                RefreshPLCReadBitData(GetB("RB_CHR").ToString(), "RB_CHR");
                RefreshPLCReadBitData(GetB("RB_BYP").ToString(), "RB_BYP");
                RefreshPLCReadBitData(GetB("RB_SEC").ToString(), "RB_SEC");
                RefreshPLCReadBitData(GetText("RD_LamaID", 16).ToString(), "RD_LamaID");
                RefreshPLCReadBitData(m_PLC.GetWaferID("RD_WaferID", 18).ToString(), "RD_WaferID");
                RefreshPLCReadBitData(GetText("RD_InfoText", 17).ToString(), "RD_InfoText");
                RefreshPLCReadBitData(GetText("RD_TimeStamp", 4).ToString(), "RD_TimeStamp");
                RefreshPLCReadBitData(GetText("RD_ProcRecipe", 9).ToString(), "RD_ProcRecipe");
                RefreshPLCReadBitData(GetDW("RD_ProcCarrierID").ToString(), "RD_ProcCarrierID");
                RefreshPLCReadBitData(GetW("RD_ProcCarrierPosX").ToString(), "RD_ProcCarrierPosX");
                RefreshPLCReadBitData(GetW("RD_ProcCarrierPosY").ToString(), "RD_ProcCarrierPosY");
                RefreshPLCReadBitData(GetDW("RD_ProcCarrierPosZ").ToString(), "RD_ProcCarrierPosZ");
                RefreshPLCReadBitData(GetDW("RD_ProcCarrierCounter").ToString(), "RD_ProcCarrierCounter");
                RefreshPLCReadBitData(GetW("RD_ToolID").ToString(), "RD_ToolID");
                RefreshPLCReadBitData(GetW("RD_TrackID").ToString(), "RD_TrackID");
                RefreshPLCReadBitData(GetDW("RD_JOBID").ToString(), "RD_JOBID");
            }
        }

        //240902 NIS현장 내용
        private void AddressDefine()
        {
            m_PLC.list_ReadAddressCount = new List<int>();
            m_PLC.list_WriteAddressCount = new List<int>();
            m_PLC.list_ReadAddressStart = new List<string>();
            m_PLC.list_WriteAddressStart = new List<string>();
            m_PLC.AddressList = new Dictionary<string, string>();

            // Address Size
            int count = 0;

            //PLC Address Count
            /* 0 */
            count = 0016; m_PLC.list_ReadAddressCount.Add(count); m_PLC.list_ReadAddressStart.Add(theRecipe.PLCAddress.PLCReadStart);
            /* 1 */
            count = 0016; m_PLC.list_ReadAddressCount.Add(count); m_PLC.list_ReadAddressStart.Add(theRecipe.PLCAddress.RD_LamaID);
            /* 2 */
            count = 0018; m_PLC.list_ReadAddressCount.Add(count); m_PLC.list_ReadAddressStart.Add(theRecipe.PLCAddress.RD_WaferID);
            /* 3 */
            count = 0017; m_PLC.list_ReadAddressCount.Add(count); m_PLC.list_ReadAddressStart.Add(theRecipe.PLCAddress.RD_InfoText);
            /* 4 */
            count = 0004; m_PLC.list_ReadAddressCount.Add(count); m_PLC.list_ReadAddressStart.Add(theRecipe.PLCAddress.RD_TimeStamp);

            count = 0009; m_PLC.list_ReadAddressCount.Add(count); m_PLC.list_ReadAddressStart.Add(theRecipe.PLCAddress.RD_ProcRecipe);

            count = 0002; m_PLC.list_ReadAddressCount.Add(count); m_PLC.list_ReadAddressStart.Add(theRecipe.PLCAddress.RD_ProcCarrierID);
            count = 0001; m_PLC.list_ReadAddressCount.Add(count); m_PLC.list_ReadAddressStart.Add(theRecipe.PLCAddress.RD_ProcCarrierPosX);
            count = 0001; m_PLC.list_ReadAddressCount.Add(count); m_PLC.list_ReadAddressStart.Add(theRecipe.PLCAddress.RD_ProcCarrierPosY);
            count = 0002; m_PLC.list_ReadAddressCount.Add(count); m_PLC.list_ReadAddressStart.Add(theRecipe.PLCAddress.RD_ProcCarrierPosZ);

            count = 0002; m_PLC.list_ReadAddressCount.Add(count); m_PLC.list_ReadAddressStart.Add(theRecipe.PLCAddress.RD_ProcCarrierCounter);
            count = 0001; m_PLC.list_ReadAddressCount.Add(count); m_PLC.list_ReadAddressStart.Add(theRecipe.PLCAddress.RD_ToolID);
            count = 0001; m_PLC.list_ReadAddressCount.Add(count); m_PLC.list_ReadAddressStart.Add(theRecipe.PLCAddress.RD_TrackID);

            count = 0002; m_PLC.list_ReadAddressCount.Add(count); m_PLC.list_ReadAddressStart.Add(theRecipe.PLCAddress.RD_JOBID);


            //
            //Vision Address Count
            /* 0 */
            count = 0016; m_PLC.list_WriteAddressCount.Add(count); m_PLC.list_WriteAddressStart.Add(theRecipe.PLCAddress.PLCWriteStart);
            /* 1 */
            count = 0003; m_PLC.list_WriteAddressCount.Add(count); m_PLC.list_WriteAddressStart.Add(theRecipe.PLCAddress.WD_Quality1);
            count = 0004; m_PLC.list_WriteAddressCount.Add(count); m_PLC.list_WriteAddressStart.Add(theRecipe.PLCAddress.WD_SampleCounter);

            count = 0008; m_PLC.list_WriteAddressCount.Add(count); m_PLC.list_WriteAddressStart.Add(theRecipe.PLCAddress.WD_ValueA);

            count = 0004; m_PLC.list_WriteAddressCount.Add(count); m_PLC.list_WriteAddressStart.Add(theRecipe.PLCAddress.WD_PosX);
            count = 0004; m_PLC.list_WriteAddressCount.Add(count); m_PLC.list_WriteAddressStart.Add(theRecipe.PLCAddress.WD_PosY);
            count = 0004; m_PLC.list_WriteAddressCount.Add(count); m_PLC.list_WriteAddressStart.Add(theRecipe.PLCAddress.WD_PosT);

            count = 0018; m_PLC.list_WriteAddressCount.Add(count); m_PLC.list_WriteAddressStart.Add(theRecipe.PLCAddress.WD_WaferID);

            // Define Address
            string sSignalName = "";
            string sAddress = "";

            // Read Bit ====================================================================================================================================
            sSignalName = "RB_SYNC"; sAddress = theRecipe.PLCAddress.RB_SYNC; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RB_VENDER"; sAddress = theRecipe.PLCAddress.RB_VENDER; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RB_BLOCK"; sAddress = theRecipe.PLCAddress.RB_BLOCK; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RB_STA"; sAddress = theRecipe.PLCAddress.RB_STA; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RB_GRS"; sAddress = theRecipe.PLCAddress.RB_GRS; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RB_RST"; sAddress = theRecipe.PLCAddress.RB_RST; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RB_TRG"; sAddress = theRecipe.PLCAddress.RB_TRG; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RB_CHR"; sAddress = theRecipe.PLCAddress.RB_CHR; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RB_Acknowledge"; sAddress = theRecipe.PLCAddress.RB_CHR; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RB_BYP"; sAddress = theRecipe.PLCAddress.RB_BYP; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RB_SEC"; sAddress = theRecipe.PLCAddress.RB_SEC; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RB_UnProcessed_Flag"; sAddress = theRecipe.PLCAddress.RB_UnProcessed_Flag; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RB_Processed_Flag"; sAddress = theRecipe.PLCAddress.RB_Processed_Flag; m_PLC.AddressList.Add(sSignalName, sAddress);


            //Read Data ====================================================================================================================================
            sSignalName = "RD_LamaID"; sAddress = theRecipe.PLCAddress.RD_LamaID; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RD_WaferID"; sAddress = theRecipe.PLCAddress.RD_WaferID; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RD_InfoText"; sAddress = theRecipe.PLCAddress.RD_InfoText; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RD_TimeStamp"; sAddress = theRecipe.PLCAddress.RD_TimeStamp; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RD_ProcRecipe"; sAddress = theRecipe.PLCAddress.RD_ProcRecipe; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RD_ProcCarrierID"; sAddress = theRecipe.PLCAddress.RD_ProcCarrierID; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RD_ProcCarrierPosX"; sAddress = theRecipe.PLCAddress.RD_ProcCarrierPosX; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RD_ProcCarrierPosY"; sAddress = theRecipe.PLCAddress.RD_ProcCarrierPosY; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RD_ProcCarrierPosZ"; sAddress = theRecipe.PLCAddress.RD_ProcCarrierPosZ; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RD_ProcCarrierCounter"; sAddress = theRecipe.PLCAddress.RD_ProcCarrierCounter; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RD_ToolID"; sAddress = theRecipe.PLCAddress.RD_ToolID; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RD_TrackID"; sAddress = theRecipe.PLCAddress.RD_TrackID; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "RD_JOBID"; sAddress = theRecipe.PLCAddress.RD_JOBID; m_PLC.AddressList.Add(sSignalName, sAddress);

            //Write Bit ====================================================================================================================================
            sSignalName = "WB_SYNC"; sAddress = theRecipe.PLCAddress.WB_SYNC; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WB_VENDER"; sAddress = theRecipe.PLCAddress.WB_VENDER; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WB_BLOCK"; sAddress = theRecipe.PLCAddress.WB_BLOCK; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WB_STA_BACK"; sAddress = theRecipe.PLCAddress.WB_STA_BACK; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WB_GRS_BACK"; sAddress = theRecipe.PLCAddress.WB_GRS_BACK; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WB_RST_BACK"; sAddress = theRecipe.PLCAddress.WB_RST_BACK; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WB_TRG_BACK"; sAddress = theRecipe.PLCAddress.WB_TRG_BACK; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WB_CHR_BACK"; sAddress = theRecipe.PLCAddress.WB_CHR_BACK; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WB_Acknowledge"; sAddress = theRecipe.PLCAddress.WB_Acknowledge; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WB_TOK"; sAddress = theRecipe.PLCAddress.WB_TOK; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WB_RES"; sAddress = theRecipe.PLCAddress.WB_RES; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WB_STP"; sAddress = theRecipe.PLCAddress.WB_STP; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WB_WRN"; sAddress = theRecipe.PLCAddress.WB_WRN; m_PLC.AddressList.Add(sSignalName, sAddress);


            //Write Data ====================================================================================================================================
            sSignalName = "WD_ToolID1"; sAddress = theRecipe.PLCAddress.WD_ToolID1; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WD_ToolID2"; sAddress = theRecipe.PLCAddress.WD_ToolID2; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WD_Quality1"; sAddress = theRecipe.PLCAddress.WD_Quality1; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WD_Quality2"; sAddress = theRecipe.PLCAddress.WD_Quality2; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WD_SimResult"; sAddress = theRecipe.PLCAddress.WD_SimResult; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WD_SampleCounter"; sAddress = theRecipe.PLCAddress.WD_SampleCounter; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WD_ValueA"; sAddress = theRecipe.PLCAddress.WD_ValueA; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WD_ValueB"; sAddress = theRecipe.PLCAddress.WD_ValueB; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WD_ValueC"; sAddress = theRecipe.PLCAddress.WD_ValueC; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WD_ValueD"; sAddress = theRecipe.PLCAddress.WD_ValueD; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WD_PosX"; sAddress = theRecipe.PLCAddress.WD_PosX; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WD_PosY"; sAddress = theRecipe.PLCAddress.WD_PosY; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WD_PosT"; sAddress = theRecipe.PLCAddress.WD_PosT; m_PLC.AddressList.Add(sSignalName, sAddress);
            sSignalName = "WD_WaferID"; sAddress = theRecipe.PLCAddress.WD_WaferID; m_PLC.AddressList.Add(sSignalName, sAddress);

            m_PLC.AddressSet();
        }


        public short GetB(string sAddress)
        {
            return m_PLC.GetB(sAddress);
        }

        public short GetW(string sAddress)
        {
            return m_PLC.GetW(sAddress);
        }

        public int GetDW(string sAddress)
        {
            return m_PLC.GetDW(sAddress);
        }

        public void SetB(string sAddress, short sValue)
        {
            if (m_PLC.GetPLCConnectedCheck())
                m_PLC.SetB(sAddress, CheckContinue_Flag(sAddress, sValue));
        }

        public void SetW(string sAddress, short sValue)
        {
            if (m_PLC.GetPLCConnectedCheck())
                m_PLC.SetW(sAddress, CheckContinue_Word(sAddress, sValue));
        }

        public void SetDW(string sAddress, int nValue)
        {
            if (m_PLC.GetPLCConnectedCheck())
                m_PLC.SetDW(sAddress, CheckContinue_DW(sAddress, nValue));
        }

        public void SendWriteFlag(string address, int nValue)
        {
            MakeWriteFlag(address, nValue);
        }

        private void MakeWriteFlag(string address, int iValue)
        {
            if(m_PLC.GetPLCConnectedCheck())
                SetB(address, (short)iValue);
        }

        //private void MakeWriteData(int _nQuality1, int _nQuality2, int _ContourX, int _ContourY, int _ContourT, string _ReturnWaferID)
        //{
        //    SetW("WD_MONO_JUDGE", (short)_nQuality1);
        //    SetW("WD_COLOR_JUDGE", (short)_nQuality2);

        //    SetDW("WD_ContourX", _ContourX);
        //    SetDW("WD_ContourY", _ContourY);
        //    SetDW("WD_ContourT", _ContourT);

        //    m_PLC.SetWaferID("WD_WaferID", 9, _ReturnWaferID);

        //}
        private void MakeWriteData(int _nQuality1, int _nQuality2, int _ContourX, int _ContourY, int _ContourT, string _ReturnWaferID)
        {
            SetW("WD_Quality1", (short)_nQuality1);
            SetW("WD_Quality2", (short)_nQuality2);

            SetDW("WD_ValueA", _ContourX);
            SetDW("WD_ValueB", _ContourY);
            SetDW("WD_ValueC", _ContourT);

            m_PLC.SetWaferID("WD_WaferID", 18, CheckContinue_String("WD_WaferID", _ReturnWaferID));
        }

        /// <summary>
        /// 24.02.05 LYK SetInspectionInfos
        /// InspectionInfos 정보를 인자로 넘겨 받아 할당 한다.
        /// </summary>
        /// <param name="_Infos">InspectionInfos 배열 할당</param>
        public void SetInspectionInfos(InspectionInfo _Infos)
        {
            InspInfos = _Infos;
        }

        public void SetText(string sAddress, int dataLength, string _sData)
        {
            if (m_PLC.GetPLCConnectedCheck())
                m_PLC.SetText(sAddress, dataLength, _sData);
        }

        public string GetText(string sAddress, int dataLength)
        {
            return m_PLC.GetText(sAddress, dataLength);
        }

        public bool GetPLCStatus()
        {
            return m_bConnected;
        }

        #region PLC Data 강제변경 활성화 함수
        private short CheckContinue_Word(string _Address, short _Value)  //241016 NIS PLC 데이터 강제변경 활성화 확인
        {
            try
            {
                string address = _Address.Split('_')[1];
                short res = _Value;
                CheckBox checkBox = MainForm.SettingPlcScreen.Controls.Find($"cb_ContinueCheck_{address}", true)[0] as CheckBox;

                if(checkBox.Checked)        //SettingPlcScreen에서 해당 PLC 데이터 강제 변경 활성화 되어있으면 변경실행
                {
                    TextBox textBox = MainForm.SettingPlcScreen.Controls.Find($"txt_Write{address}", true)[0] as TextBox;
                    res = short.Parse(textBox.Text);
                }

                return res;
            }
            catch (Exception ex)
            {
                return _Value;
            }
        }

        private int CheckContinue_DW(string _Address, int _Value)  //241016 NIS PLC 데이터 강제변경 활성화 확인
        {
            try
            {
                string address = _Address.Split('_')[1];
                int res = _Value;
                CheckBox checkBox = MainForm.SettingPlcScreen.Controls.Find($"cb_ContinueCheck_{address}", true)[0] as CheckBox;

                if (checkBox.Checked)        //SettingPlcScreen에서 해당 PLC 데이터 강제 변경 활성화 되어있으면 변경실행
                {
                    TextBox textBox = MainForm.SettingPlcScreen.Controls.Find($"txt_Write{address}", true)[0] as TextBox;
                    int valueOffset = address.Contains("Value") || address.Contains("Pos") ? 1000 : 1;
                    res = (int)(double.Parse(textBox.Text) * valueOffset);
                }

                return res;
            }
            catch (Exception ex)
            {
                return _Value;
            }
        }
        private short CheckContinue_Flag(string _Address, short _Value)  //241016 NIS PLC 데이터 강제변경 활성화 확인
        {
            try
            {
                string address = _Address.Split('_')[1];
                short res = _Value;
                CheckBox checkBox = MainForm.SettingPlcScreen.Controls.Find($"cb_ContinueCheck_{address}", true)[0] as CheckBox;

                if (checkBox.Checked)        //SettingPlcScreen에서 해당 PLC 데이터 강제 변경 활성화 되어있으면 변경실행
                {
                    DomainUpDown domain = MainForm.SettingPlcScreen.Controls.Find($"dm_Write_{address}", true)[0] as DomainUpDown;
                    res = short.Parse(domain.SelectedItem.ToString());
                }

                return res;
            }
            catch (Exception ex)
            {
                return _Value;
            }
        }
        private string CheckContinue_String(string _Address, string _Value)  //241016 NIS PLC 데이터 강제변경 활성화 확인
        {
            try
            {
                string address = _Address.Split('_')[1];
                string res = _Value;
                CheckBox checkBox = MainForm.SettingPlcScreen.Controls.Find($"cb_ContinueCheck_{address}", true)[0] as CheckBox;

                if(checkBox.Checked)        //SettingPlcScreen에서 해당 PLC 데이터 강제 변경 활성화 되어있으면 변경실행
                {
                    TextBox textBox = MainForm.SettingPlcScreen.Controls.Find($"txt_Write{address}", true)[0] as TextBox;
                    res = textBox.Text;
                }

                return res;
            }
            catch (Exception ex)
            {
                return _Value;
            }
        }
        #endregion
    }
}
