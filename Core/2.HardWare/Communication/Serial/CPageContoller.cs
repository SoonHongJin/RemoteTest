using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Core.Program;
using Core;
using MetaLog;

namespace Core.HardWare
{
    /// <summary> 23.01.11 LYK CTrigger 클래스
    /// 트리거 보드 제어와 관련된 클래스 J2PLUS ER-03 기준
    /// </summary>
    public class CPageController
    {

        private bool m_bInitialized = false;    //230111 LYK 초기화 Check 변수
        private bool m_bIsConnected = false;    //230111 LYK Connect Check 변수
        private CSerial m_PageController = null;       //230111 LYK Trigger Board 객체
        private List<byte> m_SendDataList = new List<byte>();
        private CLogger Logger = null;

        public CPageController(CLogger _logger)
        {
            Logger = _logger;
        }

        /// <summary> 23.01.11 LYK GetConnectCheck 함수
        /// Page Controller의 Connect 여부 체크
        /// </summary>
        /// <returns> Trigger 보드의 Connect 여부 리턴 </returns>
        public bool GetConnectCheck()
        {
            return m_bIsConnected;
        }

        /// <summary> 23.01.11 LYK GetInstance 함수
        /// Page Controller의 객체를 리턴한다.
        /// </summary>
        /// <returns> 트리거 보드의 객체를 리턴한다. </returns>
        public CSerial GetInstance()
        {
            return m_PageController;
        }

        /// <summary> 23.01.11 LYK Initialized 함수
        /// Page Controller 초기화 진행
        /// </summary>
        /// <param name="_sPort"> String 형태의 Port 번호를 인자로 전달 한다</param>
        public void Initialized(string _sPort, int _nBaudarate)
        {
            m_PageController = new CSerial(_sPort, _nBaudarate, Logger); //230111 LYK 트리거 보드 객체 초기화
            m_bInitialized = m_PageController.Initialized();                           //230111 LYK 트리거 보드 Initial

            Connect(); 
        }

        /// <summary> 23.01.11 LYK UnInitialized 함수
        /// 트리거 보드 UnInitial
        /// </summary>
        public void UnInitialized()
        {
            if(m_bInitialized)
            {
                if(m_bIsConnected)
                {
                    DisConnect();   //23011 LYK DisConnect
                }

                m_bInitialized = false;
            }
        }

        /// <summary> 23.01.11 LYK Connect 함수
        /// Page Controller의 Connect 처리
        /// </summary>
        private void Connect()
        {
            m_bIsConnected = m_PageController.Connect(); //230111 Page Controller Connect

            if (m_bIsConnected)
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Page Controller Connect Success.");
            else
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Page Controller Connect Fail.");
        }

        /// <summary> 23.01.11 LYK DisConnect 함수
        /// 트리거 보드의 DisConnect 처리
        /// </summary>
        private void DisConnect()
        {
            if(m_bIsConnected )
            {
                m_PageController.UnInitialized();  //23.01.11 LYK 트리거 Uninitialized 

                m_bIsConnected = false;
            }
        }

        /// <summary> 24.03.12 LYK SetPageParameter 함수
        /// 페이지 컨트롤러의 파라미터를 설정 한다.
        /// 이때 Max 페이지를 확인하고 이에 맞게 처리한다.
        /// 
        /// </summary>
        public void SetPageParameter()
        {
            string msg = "";

            // PCR 셋팅
            m_SendDataList.Clear();
            m_SendDataList.Add(0x01); // Start
            m_SendDataList.Add(0x00); // OP
            m_SendDataList.Add(0x01); // DL (Data Length)
            m_SendDataList.Add(0x38); // Addr
            m_SendDataList.Add(0x01); // Data 
            m_SendDataList.Add(0x04); // End
            m_PageController.WriteData(m_SendDataList.ToArray());

            for (int i = 0; i < 8; i++)
            {
                

                // CSR 셋팅 (선택 된 조명 채널 2진수 비트 -> 16진수 변환, 채널 1번 시작) 
                m_SendDataList.Clear();
                m_SendDataList.Add(0x01); // Start
                m_SendDataList.Add(0x00); // OP
                m_SendDataList.Add(0x01); // DL (Data Length)
                m_SendDataList.Add(0x20); // Addr
                m_SendDataList.Add(0xFF); // Data 
                m_SendDataList.Add(0x04); // End
                m_PageController.WriteData(m_SendDataList.ToArray());

                // PASR 셋팅 (페이지 번호, 1번 시작 )
                m_SendDataList.Clear();
                m_SendDataList.Add(0x01); // Start
                m_SendDataList.Add(0x00); // OP
                m_SendDataList.Add(0x01); // DL (Data Length)
                m_SendDataList.Add(0x3C); // Addr
                m_SendDataList.Add((byte)(i + 1)); // Data 
                m_SendDataList.Add(0x04); // End
                m_PageController.WriteData(m_SendDataList.ToArray());

                // SVR 셋팅 (각 채널의 조명 값)
                m_SendDataList.Clear();
                m_SendDataList.Add(0x01); // Start
                m_SendDataList.Add(0x00); // OP
                m_SendDataList.Add(0x10); // DL (Data Length)
                m_SendDataList.Add(0x28); // Addr

                for (int j = 0; j < theRecipe.LightValue[i].m_nLightValue.Length; j++)
                {
                    m_SendDataList.Add((byte)theRecipe.LightValue[i].m_nLightValue[j]); //240312 LYK Light Value Data 
                    m_SendDataList.Add((byte)(theRecipe.LightValue[i].m_nLightValue[j] >> 8)); // Data 
                }

                m_SendDataList.Add(0x04); // End
                m_PageController.WriteData(m_SendDataList.ToArray());
            }

            // PCR 셋팅
            m_SendDataList.Clear();
            m_SendDataList.Add(0x01); // Start
            m_SendDataList.Add(0x00); // OP
            m_SendDataList.Add(0x01); // DL (Data Length)
            m_SendDataList.Add(0x38); // Addr
            m_SendDataList.Add(0x00); // Data 
            m_SendDataList.Add(0x04); // End
            m_PageController.WriteData(m_SendDataList.ToArray());


            // SCR 셋팅
            m_SendDataList.Clear();
            m_SendDataList.Add(0x01); // Start
            m_SendDataList.Add(0x00); // OP
            m_SendDataList.Add(0x01); // DL (Data Length)
            m_SendDataList.Add(0x2C); // Addr
            m_SendDataList.Add(0x10); // Data 
            m_SendDataList.Add(0x04); // End
            m_PageController.WriteData(m_SendDataList.ToArray());

            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Serial Communication Page Controller Send Page Data");
        }

        /// <summary>
        /// 24.03.12 LYK Page Controller Trigger 구동 함수
        /// </summary>
        /// <param name="_nPage"></param>
        public void Trigger(int _nPage)
        {
            string msg = "";
            int binaryPage = (0 | (byte)(0x01 << _nPage));

            m_SendDataList.Clear();
            m_SendDataList.Add(0x01); // Start
            m_SendDataList.Add(0x00); // OP
            m_SendDataList.Add(0x02); // DL (Data Length)
            m_SendDataList.Add(0xD8); // Addr
            m_SendDataList.Add((byte)binaryPage); // Data 
            m_SendDataList.Add(0x00); // Data 
            m_SendDataList.Add(0x04); // End
            m_PageController.WriteData(m_SendDataList.ToArray());

            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Serial Communication Page Controller Trigger Start");
        }

        /// <summary>
        /// 24.03.12 LYK TriggerReset 함수
        /// </summary>

        public void TriggerReset()
        {
            string msg = "";

            m_SendDataList.Clear();
            m_SendDataList.Add(0x01); // Start
            m_SendDataList.Add(0x00); // OP
            m_SendDataList.Add(0x01); // DL (Data Length)
            m_SendDataList.Add(0x2D); // Addr
            m_SendDataList.Add(0x08); // Data 
            m_SendDataList.Add(0x04); // End
            m_PageController.WriteData(m_SendDataList.ToArray());

            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Serial Communication Page Controller Trigger Reset");
        }

        public void MaxPageSet()        //LYK Test 위해 4개로 고정 함 
        {
            m_SendDataList.Clear();
            m_SendDataList.Add(0x01); // Start
            m_SendDataList.Add(0x00); // OP
            m_SendDataList.Add(0x01); // DL (Data Length)
            m_SendDataList.Add(0x39); // Addr
            m_SendDataList.Add(0x04); // Data 
            m_SendDataList.Add(0x04); // End
            m_PageController.WriteData(m_SendDataList.ToArray());

            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Serial Communication Page Controller Max Page Set : {theRecipe.LightValue[0].MaxPage}");
        }
    }
}
