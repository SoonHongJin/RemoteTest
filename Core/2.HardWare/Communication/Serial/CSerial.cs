using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;

using static Core.Program;
using MetaLog;
using ScottPlot.Renderable;

namespace Core.HardWare
{
    public class CSerial
    {
        private string m_sSerialPort = null;        //230111 LYK Serial Port string 변수
        private int m_nBaudRate = 0;                //230111 LYK BaudRate 변수
        private bool m_bInitialized = false;        //230111 LYK 초기화 Check 변수
        private bool m_bIsConnected = false;        //230111 LYK Connect Check 변수
        
        private SerialPort m_SerialPort = null;     //230111 LYK System IO Serial Port 객체
        private CLogger Logger = null;

        /// <summary> 23.01.11 LYK CSerial 생성자
        /// 인자로 전달된 SerialPort, BaudRate를 멤버 변수에 각각 할당
        /// </summary>
        /// <param name="_sSerialPort"> string으로 Serial Port 번호 전달</param>
        /// <param name="_nBaud">BaudRate 전달</param>
        public CSerial(string _sSerialPort, int _nBaud, CLogger logger)
        {
            m_sSerialPort = _sSerialPort;   //230111 LYK 시리얼 포트 할당
            m_nBaudRate = _nBaud;           //230111 LYK BaudRate 할당
            Logger = logger;
        }


        /// <summary> 23.01.11 LYK Initialized 함수
        /// SerialPort Initial 
        /// </summary>
        /// <returns> 초기화 여부 리턴 </returns>
        public bool Initialized()
        {
            m_SerialPort = new SerialPort();    //230111 LYK Serial Port 객체 초기화

            return m_bInitialized = true;
        }

        /// <summary> 23.01.11 LYK UnInitialized 함수
        /// SerialPort UnInitial
        /// </summary>
        public void UnInitialized()
        {
            if(m_bInitialized)
            {
                if(m_bIsConnected)
                {
                    m_bIsConnected = false;
                    m_SerialPort.Close();
                    m_SerialPort = null;
                }

                m_bInitialized = false;
            }
        }

        /// <summary> 23.01.11 LYK Connect 함수
        /// Serial Port Connect 함수
        /// </summary>
        /// <returns>Connect 여부 리턴</returns>
        public bool Connect()
        {
            return SetSerialPort(m_sSerialPort, m_nBaudRate);
        }

        /// <summary> 23.01.11 LYK GetSerialPort
        /// Serial Port 번호 이름이 포함된 배열을 가져옴
        /// </summary>
        /// <returns>Serial Port 배열 리턴</returns>
        public string[] GetSerialPort()
        {
            string[] Comlist = SerialPort.GetPortNames();

            if (Comlist.Length > 0)
                return Comlist;
            else
                return null;
        }

        /// <summary> 23.01.11 LYK WriteData
        /// Write 함수에 명령어 전달
        /// </summary>
        /// <param name="_sWriteData"> Serial 포트에 전달할 명령어 전달</param>
        public void WriteData(string _sWriteData)
        {
            Write(_sWriteData);
        }

        /// <summary> 23.01.11 LYK WriteData
        /// Write 함수에 명령어 전달
        /// </summary>
        /// <param name="_sWriteData"> Serial 포트에 전달할 명령어 전달</param>
        public void WriteData(byte[] _WriteData)
        {
            WriteByte(_WriteData);
        }

        /// <summary> 23.01.11 LYK Write
        /// Serial 포트에 전달할 명령어 송신
        /// </summary>
        /// <param name="_sWriteData"> erial 포트에 전달할 명령어 전달 </param>

        private void Write(string _sWriteData)
        {
            byte[] buf = Encoding.UTF8.GetBytes(_sWriteData);
            int len = buf.Length;

            if (m_bIsConnected )
            {
                m_SerialPort.Write(buf, 0, len);
            }
        }

        /// <summary> 23.01.11 LYK Write
        /// Serial 포트에 전달할 명령어 송신
        /// </summary>
        /// <param name="_sWriteData"> erial 포트에 전달할 명령어 전달 </param>

        private void WriteByte(byte[] _WriteData)
        {
            int len = _WriteData.Length;

            if (m_bIsConnected)
            {
                m_SerialPort.Write(_WriteData, 0, len);
            }
        }

        /// <summary> 23.01.11 LYK SetSerialPort 함수
        /// Serial 포트에 필요한 정보 할당후 Serial Port 연결
        /// </summary>
        /// <param name="sSerialPort"> 시리얼 포트 이름 전달</param>
        /// <param name="baudRate"> BaudRate 전달 </param>
        /// <returns></returns>
        private bool SetSerialPort(string sSerialPort, int baudRate)
        {
            if(m_bIsConnected)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"{sSerialPort} is Connected.");

                return false;
            }

            /*
            if (m_SerialPort != null)
            {
                m_SerialPort.Close();
                m_SerialPort = null;
                return m_bIsConnected = false;
            }
              */

            try
            {                          
                m_SerialPort.PortName = sSerialPort;
                m_SerialPort.BaudRate = baudRate;
                m_SerialPort.DataBits = 8;
                m_SerialPort.Parity = Parity.None;
                m_SerialPort.Handshake = Handshake.None;
                m_SerialPort.StopBits = StopBits.One;
                m_SerialPort.DataReceived += Port_DataReceived;
                m_SerialPort.ErrorReceived += Error_Received;
                m_SerialPort.RtsEnable = true;
                m_SerialPort.Open();

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Trigger Serial Port {sSerialPort} is Open");

                return m_bIsConnected = true;
            }
            catch (Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Trigger Serial Port Open Error : {ex.Message} ");

                return false;
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int ReceiveData = m_SerialPort.ReadByte(); // Byte 로 받아온다 
            //string test = m_SerialPort.ReadLine(); // Byte 로 받아온다 
        }

        private void Error_Received(object sender, SerialErrorReceivedEventArgs e)
        {
            int ReceiveData = m_SerialPort.ReadByte(); // Byte 로 받아온다 
            //string test = m_SerialPort.ReadLine(); // Byte 로 받아온다 
        }
    }
}