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
    public class CTrigger
    {

        private bool m_bInitialized = false;    //230111 LYK 초기화 Check 변수
        private bool m_bIsConnected = false;    //230111 LYK Connect Check 변수
        private CSerial m_Trigger = null;       //230111 LYK Trigger Board 객체
        private CLogger Logger = null;
        public CTrigger(CLogger _logger)
        {
            Logger = _logger;
        }

        /// <summary> 23.01.11 LYK GetConnectCheck 함수
        /// Trigger 보드의 Connect 여부 체크
        /// </summary>
        /// <returns> Trigger 보드의 Connect 여부 리턴 </returns>
        public bool GetConnectCheck()
        {
            return m_bIsConnected;
        }

        /// <summary> 23.01.11 LYK GetInstance 함수
        /// 트리거 보드의 객체를 리턴한다.
        /// </summary>
        /// <returns> 트리거 보드의 객체를 리턴한다. </returns>
        public CSerial GetInstance()
        {
            return m_Trigger;
        }

        /// <summary> 23.01.11 LYK Initialized 함수
        /// 트리거 보드 초기화 진행
        /// </summary>
        /// <param name="_sPort"> String 형태의 Port 번호를 인자로 전달 한다</param>
        public void Initialized(string _sPort)
        {
            m_Trigger = new CSerial(_sPort, DEF_SYSTEM.TRIGGER_BOARD_BAUDRATE, Logger); //230111 LYK 트리거 보드 객체 초기화
            m_bInitialized = m_Trigger.Initialized();                           //230111 LYK 트리거 보드 Initial

            Connect();  //230111 LYK Trigger Board Connect 
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

        /// <summary> 23.01.11 LYK TriggerBoardWriteData 함수
        /// 트리거 보드 제어 명령어 송신
        /// </summary>
        /// <param name="_sWriteData"> 제어 명령어를 인자로 전달 한다. EX) IN,1\r\n </param>
        public void TriggerBoardWriteData(string _sWriteData)
        {
            m_Trigger.WriteData(_sWriteData);
        }

        /// <summary> 23.01.11 LYK Connect 함수
        /// 트리거 보드의 Connect 처리
        /// </summary>
        private void Connect()
        {
            m_bIsConnected = m_Trigger.Connect(); //230111 LYK 트리거 보드 Connect

            if (m_bIsConnected)
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Trigger Board Open Success.");
            else
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Trigger Board Open Fail.");
        }

        /// <summary> 23.01.11 LYK DisConnect 함수
        /// 트리거 보드의 DisConnect 처리
        /// </summary>
        private void DisConnect()
        {
            if(m_bIsConnected )
            {
                m_Trigger.UnInitialized();  //23.01.11 LYK 트리거 Uninitialized 

                m_bIsConnected = false;
            }
        }
    }
}
