using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro;
using System.Runtime.InteropServices;
using System.Timers;

using static Core.Program;
using Core;
using Core.Process;
using Core.Utility;

using ActUtlType64Lib;
using ActProgType64Lib;

namespace Core.HardWare
{
    public class CMdfuncPLC
    {
        MdfuncPLC.MdfuncPLC OpPLC = null;
        ActUtlType64 MelsecPLC = null;
        ActProgType64 MelsecTest = null;    //250226 NWT PLC TimeOut 시간 설정을 위해 추가

        private bool m_bIsConneced = false;
        public bool m_bFirstDisC = true;    //250226 NWT 통신 연결이 끊어져 있을 때 Disconnect 함수를 한번만 타기 위한 변수
        public bool m_bDisconnect = false;  //250225 NWT Disconnect 버튼으로 종료했는지 확인용

        public List<int> list_ReadAddressCount;
        public List<int> list_WriteAddressCount;
        public List<string> list_ReadAddressStart;
        public List<string> list_WriteAddressStart;
        public Dictionary<string, string> AddressList;

        public List<short[]> list_ReadValue;
        public List<short[]> list_WriteValue;
        public List<short[]> list_WriteValueEx;

        int wCount = 0;

        short[] m_nReadWord = new short[100];

        public int m_nPlcMode = 0;

        public CMdfuncPLC()
        {
            list_ReadAddressCount = new List<int>();
            list_WriteAddressCount = new List<int>();
            list_ReadAddressStart = new List<string>();
            list_WriteAddressStart = new List<string>();

            list_ReadValue = new List<short[]>();
            list_WriteValue = new List<short[]>();
            list_WriteValueEx = new List<short[]>();

            if (m_nPlcMode == 0)
            {
                MelsecPLC = new ActUtlType64();
                //250226 NWT PLC TimeOut 시간 설정
                MelsecTest = new ActProgType64();
                MelsecTest.ActTimeOut = 1;
            }
            else
                OpPLC = new MdfuncPLC.MdfuncPLC();
        }


        public int ConnectPLC()
        {
            try
            {
                int ret = 0;
                if (m_nPlcMode == 0)
                {
                    MelsecPLC.ActLogicalStationNumber = DEF_SYSTEM.LOGICAL_STATION_NUMBER; // Melsec Setting 에서 0번 등록 필요 

                    ret = MelsecPLC.Open();
                }
                else
                    ret = OpPLC.ConnectOpPLC();

                return ret;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// 25.02.25 NWT
        /// PLC 재연결 함수
        /// </summary>
        /// <returns></returns>
        public int ReConnectPLC()
        {
            try
            {
                int ret = 0;
                if (m_nPlcMode == 0)
                {
                    MelsecPLC.ActLogicalStationNumber = DEF_SYSTEM.LOGICAL_STATION_NUMBER; // Melsec Setting 에서 0번 등록 필요 
                    ret = MelsecPLC.Open();
                }
                else
                    ret = OpPLC.ConnectOpPLC();
                //250225 NWT 재연결 시 정상적으로 연결되었는지 확인
                string plcConnectedCheckstr = string.Empty;
                int plcConnectedCheckn = 0;
                int plcConnetedstatus = MelsecPLC.GetCpuType(out plcConnectedCheckstr, out plcConnectedCheckn);

                if (plcConnetedstatus == 0)
                {
                    m_bIsConneced = true;
                    m_bFirstDisC = true;
                }
                else
                {
                    m_bIsConneced = false;
                }

                return plcConnetedstatus;
            }
            catch (Exception)
            {
                return -1;
            }
        }


        public int DisconnectPLC()
        {
            try
            {
                int ret = 0;
                m_bFirstDisC = false;

                if (m_nPlcMode == 0)
                    ret = MelsecPLC.Close();
                else
                    ret = OpPLC.CloseOpPLC();

                if (ret == 0)
                {
                    m_bIsConneced = false;
                }

                return ret;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool GetPLCConnectedCheck()
        {
            string plcConnectedCheckstr = string.Empty;
            int plcConnectedCheckn = 0;
            int plcConnetedstatus = MelsecPLC.GetCpuType(out plcConnectedCheckstr, out plcConnectedCheckn);

            if (plcConnetedstatus == 0 && m_bDisconnect == false)
            {
                m_bIsConneced = true;
            }
            else
            {
                m_bIsConneced = false;
            }

            return m_bIsConneced;
        }


        public void AddressSet()
        {

            // PLC Read/Write Data 배열을 저장하는 List ==================================================================


            for (int i = 0; i < list_ReadAddressCount.Count; i++)
            {
                Int16[] arr = new Int16[list_ReadAddressCount[i]];

                list_ReadValue.Add(arr);
            }

            for (int i = 0; i < list_WriteAddressCount.Count; i++)
            {
                Int16[] arr = new Int16[list_WriteAddressCount[i]];

                list_WriteValue.Add(arr);
            }
        }

        public int ReadPLC()
        {
            int ret = 0;

            for (int i = 0; i < list_ReadAddressStart.Count; i++)
            {
                if (m_nPlcMode == 0)
                {
                    ret = MelsecPLC.ReadDeviceBlock2(list_ReadAddressStart[i], list_ReadAddressCount[i], out list_ReadValue[i][0]);
                }
                else
                {
                    short[] arrBuff = null;
                    ret = OpPLC.ReadInt16(list_ReadAddressStart[i], (short)list_ReadAddressCount[i], out arrBuff);
                    list_ReadValue[i] = arrBuff;
                }


                if (ret != 0)
                {
                    m_bIsConneced = false;
                    break;
                }
            }

            return ret;
        }


        public void WritePLC()
        {
            bool isChanged = false;


            // 처음 실행 시 List 내부 배열이 비어있을 때 
            if (list_WriteValueEx == null || list_WriteValueEx.Count == 0)
            {
                list_WriteValueEx = new List<Int16[]>();
                for (int i = 0; i < list_WriteValue.Count; i++)
                {
                    list_WriteValueEx.Add((Int16[])list_WriteValue[i].Clone());
                }

                isChanged = true;
            }


            // Interface 루프 회전이 20회 이상 될 때 
            //if (wCount > 20 && !isChanged)
            //{
            //    wCount = 0;
            //    isChanged = true;
            //}


            // ex 와 current List 내부 배열에 변화된 값이 있을 경우 
            //if (!isChanged)
            {
                int count = list_WriteValue.Count;
                for (int i = 0; i < count; i++)
                {
                    Int16[] arrBeforeData = list_WriteValueEx[i];
                    Int16[] arrOutputData = list_WriteValue[i];

                    //if (arrBeforeData.SequenceEqual(arrOutputData))
                    //{
                    //    continue;
                    //}
                    //else
                    //{
                    //    
                    //
                    //    isChanged = true;
                    //}

                    list_WriteValueEx[i].Initialize();
                    list_WriteValueEx[i] = (Int16[])arrOutputData.Clone();
                }
            }

            //if(isChanged)
            {
                for (int i = 0; i < list_WriteAddressStart.Count; i++)
                {
                    int result = 0;

                    if (m_nPlcMode == 0)
                        result = MelsecPLC.WriteDeviceBlock2(list_WriteAddressStart[i], list_WriteValue[i].Length, ref list_WriteValue[i][0]);
                    else
                        result = OpPLC.WriteInt16(list_WriteAddressStart[i], list_WriteValue[i]);
                }
            }

        }


        public int HexToDec(string hex)
        {
            return Convert.ToInt32(hex, 16);
        }


        public short GetB(string inputData)
        {

            // NOTE : M or B Type 의 비트와 D.n Type 의 비트를 구분
            try
            {
                string inputAddress = "";
                if (inputData.Contains("RB"))
                    inputAddress = (string)AddressList[inputData];
                else if (inputData[0] == 'M' || inputData[0] == 'B' || inputData[0] == 'D' || inputData[0] == 'W')
                    inputAddress = inputData;
                //else
                //    return -9999; // 잘못된 정보 입력

                // Input Address 의 정보 분리
                string typeInputAddress = inputAddress.Substring(0, 1);
                int numInputAddress = 0;
                int numInputAddressIndex = 0;

                bool bBitCheck = false; //240327 LYK true인 경우 소수점 없음 false인 경우 소수점 있음
                string[] spAddress = inputAddress.Split('.');

                if (typeInputAddress == "M")
                    numInputAddress = Convert.ToInt32(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출
                else if (typeInputAddress == "B")
                {
                    numInputAddress = HexToDec(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경

                }
                else if (typeInputAddress == "D" || typeInputAddress == "W")
                {
                    if (typeInputAddress == "D")
                        numInputAddress = Convert.ToInt32(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경
                    else
                        numInputAddress = HexToDec(spAddress[0].Substring(1, spAddress[0].Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경

                    if (spAddress.Length > 2)
                        numInputAddressIndex = Convert.ToInt32(spAddress[1], 16); // D.n 의 n 만 추출
                    else
                        bBitCheck = true;
                }


                // Input Address 가 포함된 List 의 인덱스 확인
                int index = 0;
                int startNum = 0, endNum = 0;
                for (int i = 0; i < list_ReadAddressStart.Count; i++)
                {
                    string startAddress = list_ReadAddressStart[i];
                    string typeAddress = startAddress.Substring(0, 1);

                    if (typeInputAddress != typeAddress)
                        continue;

                    if (typeInputAddress == "B" || typeInputAddress == "W")
                    {
                        startNum = HexToDec(startAddress.Substring(1, startAddress.Length - 1)); // 범위 계산을 위해 16진수를 10진수로 변환
                    }
                    else
                        startNum = Convert.ToInt32(startAddress.Substring(1, startAddress.Length - 1));

                    if (typeInputAddress == "M" || typeInputAddress == "B")
                        endNum = startNum + list_ReadAddressCount[i] * 16 - 1;
                    else if (typeInputAddress == "D" || typeInputAddress == "W")
                        endNum = startNum + list_ReadAddressCount[i];


                    if (numInputAddress >= startNum && numInputAddress < endNum)
                    {
                        index = i; // zInterface Task 의 Read PLC 에서 읽어온 값들이 저장된 리스트 중 어느 인덱스인지
                        break;
                    }
                }


                // Input Address 가 포함 된 Word 값
                int arrIndex = 0;

                if (typeInputAddress == "M" || typeInputAddress == "B")
                    arrIndex = (numInputAddress - startNum) / 16;
                else if (typeInputAddress == "D" || typeInputAddress == "W")
                    arrIndex = numInputAddress - startNum;

                Int16[] arrReadData = list_ReadValue[index];

                UInt16 retWord = (UInt16)arrReadData[arrIndex]; // 음수 처리 되는 것을 막기 위해 UInt 로 형변환


                // Input Address 에 해당하는 Bit 값 가져오기
                int bitIndex = 0;

                if (typeInputAddress == "M" || typeInputAddress == "B")
                    bitIndex = numInputAddress % 16;
                else if (typeInputAddress == "D" || typeInputAddress == "W")
                    bitIndex = numInputAddressIndex;

                Int16 retBit = 0;

                if (bBitCheck == true)
                    retBit = (Int16)retWord;
                else
                    retBit = (Int16)((retWord & (Int16)Math.Pow(2, bitIndex)) >> bitIndex);

                return retBit;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public short GetW(string inputData)
        {

            // NOTE : M or B Type 의 비트와 D.n Type 의 비트를 구분
            try
            {
                string inputAddress = "";
                if (inputData.Contains("RD"))
                    inputAddress = (string)AddressList[inputData];
                else if (inputData[0] == 'D' || inputData[0] == 'W')
                    inputAddress = inputData;
                else
                    return -9999; // 잘못된 정보 입력

                // Input Address 의 정보 분리
                string typeInputAddress = inputAddress.Substring(0, 1);
                int numInputAddress = 0;

                if (typeInputAddress == "W")
                    numInputAddress = HexToDec(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경
                else
                    numInputAddress = Convert.ToInt32(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출


                // Input Address 가 포함된 List 의 인덱스 확인
                int index = 0;
                int startNum = 0, endNum = 0;
                for (int i = 0; i < list_ReadAddressStart.Count; i++)
                {
                    string startAddress = list_ReadAddressStart[i];
                    string typeAddress = startAddress.Substring(0, 1);


                    if (typeInputAddress != typeAddress)
                        continue;

                    if (typeInputAddress == "W")
                        startNum = HexToDec(startAddress.Substring(1, startAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경
                    else
                        startNum = Convert.ToInt32(startAddress.Substring(1, startAddress.Length - 1));

                    endNum = startNum + list_ReadAddressCount[i];

                    if (numInputAddress >= startNum && numInputAddress < endNum)
                    {
                        index = i; // zInterface Task 의 Read PLC 에서 읽어온 값들이 저장된 리스트 중 어느 인덱스인지

                        break;
                    }
                }


                // Input Address 가 포함 된 Word 값
                int arrIndex = 0;

                arrIndex = numInputAddress - startNum;

                Int16[] arrReadData = list_ReadValue[index];
                Int16 retWord = arrReadData[arrIndex];

                return retWord;
            }
            catch
            {
                return 0;
            }
        }


        public int GetDW(string inputData)
        {
            // NOTE : M or B Type 의 비트와 D.n Type 의 비트를 구분
            try
            {
                string inputAddress = "";
                if (inputData.Contains("RD"))
                    inputAddress = (string)AddressList[inputData];
                else if (inputData[0] == 'D' || inputData[0] == 'W')
                    inputAddress = inputData;
                else
                    return -9999; // 잘못된 정보 입력

                // Input Address 의 정보 분리
                string typeInputAddress = inputAddress.Substring(0, 1);
                int numInputAddress = 0;

                if (typeInputAddress == "W")
                    numInputAddress = HexToDec(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경
                else
                    numInputAddress = Convert.ToInt32(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출


                // Input Address 가 포함된 List 의 인덱스 확인
                int index = 0;
                int startNum = 0, endNum = 0;
                for (int i = 0; i < list_ReadAddressStart.Count; i++)
                {
                    string startAddress = list_ReadAddressStart[i];
                    string typeAddress = startAddress.Substring(0, 1);

                    if (typeInputAddress != typeAddress)
                        continue;

                    if (typeInputAddress == "W")
                        startNum = HexToDec(startAddress.Substring(1, startAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경
                    else
                        startNum = Convert.ToInt32(startAddress.Substring(1, startAddress.Length - 1));

                    endNum = startNum + list_ReadAddressCount[i];

                    if (numInputAddress >= startNum && numInputAddress < endNum)
                    {
                        index = i; // zInterface Task 의 Read PLC 에서 읽어온 값들이 저장된 리스트 중 어느 인덱스인지
                        break;
                    }
                }


                // Input Address 가 포함 된 Word 값
                int arrIndex = 0;

                arrIndex = numInputAddress - startNum;

                Int16[] arrReadData = list_ReadValue[index];
                Int16 Word1 = arrReadData[arrIndex];
                Int16 Word2 = arrReadData[arrIndex + 1];

                // 10진수를 16진수로 변경
                string sHex1 = Word1.ToString("X4");
                string sHex2 = Word2.ToString("X4");

                string sHex = sHex2 + sHex1;

                Int32 retDoubleWord = Convert.ToInt32(sHex, 16);

                // 음수값 계산
                if (retDoubleWord > 2147483647 && retDoubleWord < 4294967296)
                {
                    retDoubleWord = (int)(retDoubleWord - 4294967296);
                }
                return retDoubleWord;
            }
            catch
            {
                return 0;
            }
        }

        public string GetWaferID(string inputData, int dataLength)
        {
            try
            {
                string inputAddress = "";
                if (inputData.Contains("RD"))
                    inputAddress = (string)AddressList[inputData];
                else if (inputData[0] == 'D' || inputData[0] == 'W')
                    inputAddress = inputData;

                // Input Address 의 정보 분리
                string typeInputAddress = inputAddress.Substring(0, 1);
                int numInputAddress = 0;

                if (typeInputAddress == "W")
                    numInputAddress = HexToDec(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경
                else
                    numInputAddress = Convert.ToInt32(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출


                // Input Address 가 포함된 List 의 인덱스 확인
                int index = 0;
                int startNum = 0, endNum = 0;
                for (int i = 0; i < list_ReadAddressStart.Count; i++)
                {
                    string startAddress = list_ReadAddressStart[i];
                    string typeAddress = startAddress.Substring(0, 1);

                    if (typeInputAddress != typeAddress)
                        continue;

                    if (typeInputAddress == "W")
                        startNum = HexToDec(startAddress.Substring(1, startAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경

                    else
                        startNum = Convert.ToInt32(startAddress.Substring(1, startAddress.Length - 1));

                    endNum = startNum + list_ReadAddressCount[i];

                    //if (numInputAddress >= startNum && numInputAddress < endNum)
                    if (numInputAddress == startNum)
                    {
                        index = i; // zInterface Task 의 Read PLC 에서 읽어온 값들이 저장된 리스트 중 어느 인덱스인지
                        break;
                    }
                }


                // Input Address 가 포함 된 Word 값
                int arrIndex = 0;

                arrIndex = numInputAddress - startNum;

                Int16[] arrReadData = list_ReadValue[index]; // 해당 주소가 속해있는 배열

                string readVal = "";
                string strData = "";
                //char[] str = new char[2];
                for (int i = arrIndex; i < arrIndex + dataLength; i++)
                {
                    Int16 readWord = arrReadData[i];

                    //str[0] = (char)(readWord & 0xff);
                    //str[1] = (char)(readWord >> 8);
                    //string a = char.ConvertFromUtf32(str[1]);
                    string a = char.ConvertFromUtf32(readWord);

                    if (readWord > 0)
                    {
                        //if (a == "\0")
                        //{
                        //    strData = char.ConvertFromUtf32(str[0]);
                        //}
                        //else
                        //{
                        //strData = char.ConvertFromUtf32(str[0]) + char.ConvertFromUtf32(str[1]);
                        strData = a;

                        //}

                        readVal += strData;
                    }
                }

                return readVal.Trim();
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public void SetWaferID(string inputData, int dataLength, string _sData)
        {
            try
            {
                string inputAddress = "";
                if (inputData.Contains("WD"))
                    inputAddress = (string)AddressList[inputData];
                else if (inputData[0] == 'D' || inputData[0] == 'W')
                    inputAddress = inputData;

                // Input Address 의 정보 분리
                string typeInputAddress = inputAddress.Substring(0, 1);
                int numInputAddress = 0;

                if (typeInputAddress == "W")
                    numInputAddress = HexToDec(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경
                else
                    numInputAddress = Convert.ToInt32(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출


                // Input Address 가 포함된 List 의 인덱스 확인
                int index = 0;
                int startNum = 0, endNum = 0;
                for (int i = 0; i < list_WriteAddressStart.Count; i++)
                {
                    string startAddress = list_WriteAddressStart[i];
                    string typeAddress = startAddress.Substring(0, 1);

                    if (typeInputAddress != typeAddress)
                        continue;

                    if (typeInputAddress == "W")
                        startNum = HexToDec(startAddress.Substring(1, startAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경

                    else
                        startNum = Convert.ToInt32(startAddress.Substring(1, startAddress.Length - 1));

                    endNum = startNum + list_WriteAddressCount[i];
                    if (numInputAddress >= startNum && numInputAddress < endNum)
                    //if (numInputAddress == startNum)
                    {
                        index = i; // zInterface Task 의 Read PLC 에서 읽어온 값들이 저장된 리스트 중 어느 인덱스인지
                        break;
                    }
                }

                // Input Address 가 포함 된 Word 값
                int arrIndex = 0;

                arrIndex = numInputAddress - startNum;

                byte[] WaferBytes;

                //240902 NIS 현장 통신 - SetWaferID

                if (int.TryParse(_sData, out int intValue))
                {
                    for (int i = 0; i < list_WriteValue[index].Length; i++)
                    {
                        list_WriteValue[index][i] = 0;
                    }
                    WaferBytes = Encoding.Default.GetBytes(_sData);

                    for (int i = 0; i < dataLength; i++)
                    {
                        list_WriteValue[index][arrIndex++] = (short)(WaferBytes[i]);
                    }
                }
                else
                {
                    // 기존의 바이트 배열로 변환하여 저장하는 코드
                    WaferBytes = Encoding.Default.GetBytes(_sData);

                    for (int i = 0; i < dataLength; i++)
                    {
                        list_WriteValue[index][arrIndex++] = (short)(WaferBytes[i]);
                    }
                }

            }
            catch
            {

            }
        }


        public void SetB(string inputData, short inputValue)
        {
            // NOTE : M or B Type 의 비트와 D.n Type 의 비트를 구분
            string inputAddress = "";
            if (inputData.Contains("WB"))
                inputAddress = (string)AddressList[inputData];
            else if (inputData[0] == 'M' || inputData[0] == 'B' || inputData[0] == 'D' || inputData[0] == 'W')
                inputAddress = inputData;

            // Input Address 의 정보 분리
            string typeInputAddress = inputAddress.Substring(0, 1);
            int numInputAddress = 0;
            int numInputAddressIndex = 0;

            string[] spAddress = inputAddress.Split('.');
            bool bBitCheck = false;

            if (typeInputAddress == "M")
                numInputAddress = Convert.ToInt32(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출
            else if (typeInputAddress == "B")
                numInputAddress = HexToDec(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경
            else if (typeInputAddress == "D" || typeInputAddress == "W")
            {
                if (typeInputAddress == "D")
                    numInputAddress = Convert.ToInt32(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경
                else
                    numInputAddress = HexToDec(spAddress[0].Substring(1, spAddress[0].Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경

                if (spAddress.Length >= 2)
                    numInputAddressIndex = Convert.ToInt32(spAddress[1], 16); // D.n 의 n 만 추출
                else
                    bBitCheck = true;
            }


            // Input Address 가 포함된 List 의 인덱스 확인
            int index = 0;
            int startNum = 0, endNum = 0;
            for (int i = 0; i < list_WriteAddressStart.Count; i++)
            {
                string startAddress = list_WriteAddressStart[i];
                string typeAddress = startAddress.Substring(0, 1);

                if (typeInputAddress != typeAddress)
                    continue;

                if (typeInputAddress == "B" || typeInputAddress == "W")
                    startNum = HexToDec(startAddress.Substring(1, startAddress.Length - 1)); // 범위 계산을 위해 16진수를 10진수로 변환
                else
                    startNum = Convert.ToInt32(startAddress.Substring(1, startAddress.Length - 1));

                if (typeInputAddress == "M" || typeInputAddress == "B")
                    endNum = startNum + list_WriteAddressCount[i] * 16 - 1;
                else if (typeInputAddress == "D" || typeInputAddress == "W")
                    endNum = startNum + list_WriteAddressCount[i];

                if (numInputAddress >= startNum && numInputAddress < endNum)
                {
                    index = i; // zInterface Task 의 Write PLC 에서 읽어온 값들이 저장된 리스트 중 어느 인덱스인지
                    break;
                }
            }


            // Input Address 가 포함 된 Word 값
            int arrIndex = 0;

            if (typeInputAddress == "M" || typeInputAddress == "B")
                arrIndex = (numInputAddress - startNum) / 16;
            else if (typeInputAddress == "D" || typeInputAddress == "W")
                arrIndex = numInputAddress - startNum;

            Int16[] arrWriteData = list_WriteValue[index];

            // Bit On/Off 값을 Word 로 변환
            int bitIndex = 0;

            if (typeInputAddress == "M" || typeInputAddress == "B")
                bitIndex = numInputAddress % 16;
            else if (typeInputAddress == "D" || typeInputAddress == "W")
                bitIndex = numInputAddressIndex;

            // Bit 읽고 쓰기
            if (bBitCheck == false)
            {
                if (inputValue == 1) // 입력 주소에 1을 쓸 경우
                    arrWriteData[arrIndex] = (Int16)(arrWriteData[arrIndex] | (Int16)Math.Pow(2, bitIndex));
                else
                    arrWriteData[arrIndex] = (Int16)(arrWriteData[arrIndex] & ~((Int16)Math.Pow(2, bitIndex)));
            }
            else
            {
                arrWriteData[arrIndex] = (Int16)inputValue;// arrWriteData[arrIndex];
            }
        }


        public void SetW(string inputData, short inputValue)
        {
            // NOTE : M or B Type 의 비트와 D.n Type 의 비트를 구분
            if (inputValue > 32767 || inputValue < -32768)
                return;

            string inputAddress = "";
            if (inputData.Contains("WD"))
                inputAddress = (string)AddressList[inputData];
            else if (inputData[0] == 'D' || inputData[0] == 'W')
                inputAddress = inputData;


            // Input Address 의 정보 분리
            string typeInputAddress = inputAddress.Substring(0, 1);
            int numInputAddress = 0;

            if (typeInputAddress == "W")
                numInputAddress = HexToDec(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경
            else
                numInputAddress = Convert.ToInt32(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출


            // Input Address 가 포함된 List 의 인덱스 확인
            int index = 0;
            int startNum = 0, endNum = 0;
            for (int i = 0; i < list_WriteAddressStart.Count; i++)
            {
                string startAddress = list_WriteAddressStart[i];
                string typeAddress = startAddress.Substring(0, 1);

                if (typeInputAddress != typeAddress)
                    continue;

                if (typeInputAddress == "W")
                    startNum = HexToDec(startAddress.Substring(1, startAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경
                else
                    startNum = Convert.ToInt32(startAddress.Substring(1, startAddress.Length - 1));

                endNum = startNum + list_WriteAddressCount[i];

                if (numInputAddress >= startNum && numInputAddress < endNum)
                {
                    index = i; // zInterface Task 의 Write PLC 에서 읽어온 값들이 저장된 리스트 중 어느 인덱스인지
                    break;
                }
            }


            // Input Address 가 포함 된 Word 값
            int arrIndex = 0;

            arrIndex = numInputAddress - startNum;

            Int16[] arrWriteData = list_WriteValue[index];

            arrWriteData[arrIndex] = inputValue;

        }


        public void SetDW(string inputData, int inputValue)
        {
            // NOTE : M or B Type 의 비트와 D.n Type 의 비트를 구분
            if (inputValue > 2147483647 || inputValue < -2147483648)
                return;

            string inputAddress = "";
            if (inputData.Contains("WD"))
                inputAddress = (string)AddressList[inputData];
            else if (inputData[0] == 'D' || inputData[0] == 'W')
                inputAddress = inputData;


            // Input Address 의 정보 분리
            string typeInputAddress = inputAddress.Substring(0, 1);
            int numInputAddress = 0;

            if (typeInputAddress == "W")
                numInputAddress = HexToDec(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경
            else
                numInputAddress = Convert.ToInt32(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출


            // Input Address 가 포함된 List 의 인덱스 확인
            int index = 0;
            int startNum = 0, endNum = 0;
            for (int i = 0; i < list_WriteAddressStart.Count; i++)
            {
                string startAddress = list_WriteAddressStart[i];
                string typeAddress = startAddress.Substring(0, 1);

                if (typeInputAddress != typeAddress)
                    continue;

                if (typeInputAddress == "W")
                    startNum = HexToDec(startAddress.Substring(1, startAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경
                else
                    startNum = Convert.ToInt32(startAddress.Substring(1, startAddress.Length - 1));

                endNum = startNum + list_WriteAddressCount[i];

                if (numInputAddress >= startNum && numInputAddress < endNum)
                {
                    index = i; // zInterface Task 의 Write PLC 에서 읽어온 값들이 저장된 리스트 중 어느 인덱스인지
                    break;
                }
            }

            // Input Address 가 포함 된 Word 값
            int arrIndex = 0;

            arrIndex = numInputAddress - startNum;

            Int16[] arrWriteData = list_WriteValue[index];



            // Input Value 를 Double Word 로 구분하여 처리

            Int32 Min = (-2147483647);
            Int32 Max = -1;

            if (inputValue > Min && inputValue < Max)
            {
                inputValue = (int)(inputValue + 4294967296);
            }

            // Int64 값을 Hex 값으로 변경
            string sHex = inputValue.ToString("X4");
            int nHexLength = sHex.Length;

            string sFirstHex = "";
            string sSecondHex = "";
            Int16 nFirstWord = 0;
            Int16 nSecondWord = 0;

            if (nHexLength > 4) // 더블 워드 크기일 경우
            {
                sFirstHex = sHex.Substring(nHexLength - 4, 4);
                sSecondHex = sHex.Substring(0, nHexLength - 4);

                nFirstWord = Convert.ToInt16(sFirstHex, 16);
                nSecondWord = Convert.ToInt16(sSecondHex, 16);
            }
            else //단일 워드 크기일 경우
            {
                nFirstWord = Convert.ToInt16(sHex, 16);
                nSecondWord = 0;
            }


            // 해당 인덱스에 있던 값을 새로 쓴 값으로 변경
            arrWriteData[arrIndex] = nFirstWord;
            arrWriteData[arrIndex + 1] = nSecondWord;
        }

        //240902 NIS SetText
        public void SetText(string inputData, int dataLength, string _sData)
        {
            try
            {
                //주소 추출

                string inputAddress = "";
                if (inputData.Contains("WD"))
                    inputAddress = (string)AddressList[inputData];
                else if (inputData[0] == 'D' || inputData[0] == 'W')
                    inputAddress = inputData;

                // Input Address 의 정보 분리
                string typeInputAddress = inputAddress.Substring(0, 1);
                int numInputAddress = 0;

                if (typeInputAddress == "W")
                    numInputAddress = HexToDec(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경
                else
                    numInputAddress = Convert.ToInt32(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출


                // Input Address 가 포함된 List 의 인덱스 확인
                int index = 0;
                int startNum = 0, endNum = 0;
                for (int i = 0; i < list_WriteAddressStart.Count; i++)
                {
                    string startAddress = list_WriteAddressStart[i];
                    string typeAddress = startAddress.Substring(0, 1);

                    if (typeInputAddress != typeAddress)
                        continue;

                    if (typeInputAddress == "W")
                        startNum = HexToDec(startAddress.Substring(1, startAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경

                    else
                        startNum = Convert.ToInt32(startAddress.Substring(1, startAddress.Length - 1));

                    endNum = startNum + list_WriteAddressCount[i];
                    if (numInputAddress >= startNum && numInputAddress < endNum)
                    //if (numInputAddress == startNum)
                    {
                        index = i; // zInterface Task 의 Read PLC 에서 읽어온 값들이 저장된 리스트 중 어느 인덱스인지
                        break;
                    }
                }

                // Input Address 가 포함 된 Word 값
                int arrIndex = 0;

                arrIndex = numInputAddress - startNum;

                byte[] WaferBytes;

                if (_sData.Length % 2 != 0)
                {
                    _sData += "\0";
                }
                if (int.TryParse(_sData, out int intValue))
                {
                    for (int i = 0; i < list_WriteValue[index].Length; i++)
                    {
                        list_WriteValue[index][i] = 0;
                    }
                    WaferBytes = Encoding.Default.GetBytes(_sData);
                    for (int i = 0; i < dataLength; i++)
                    {
                        list_WriteValue[index][arrIndex++] = (short)(WaferBytes[i]);
                    }
                }
                else
                {
                    // 기존의 바이트 배열로 변환하여 저장하는 코드
                    WaferBytes = Encoding.Default.GetBytes(_sData);
                    for (int i = 0; i < dataLength; i++)
                    {
                        list_WriteValue[index][arrIndex++] = (short)(WaferBytes[i]);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        //240902 NIS GetText
        public string GetText(string inputData, int dataLength)
        {
            try
            {
                string inputAddress = "";
                if (inputData.Contains("RD"))
                    inputAddress = (string)AddressList[inputData];
                else if (inputData[0] == 'D' || inputData[0] == 'W')
                    inputAddress = inputData;

                // Input Address 의 정보 분리
                string typeInputAddress = inputAddress.Substring(0, 1);
                int numInputAddress = 0;

                if (typeInputAddress == "W")
                    numInputAddress = HexToDec(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경
                else
                    numInputAddress = Convert.ToInt32(inputAddress.Substring(1, inputAddress.Length - 1)); // 주소의 숫자만 추출


                // Input Address 가 포함된 List 의 인덱스 확인
                int index = 0;
                int startNum = 0, endNum = 0;
                for (int i = 0; i < list_ReadAddressStart.Count; i++)
                {
                    string startAddress = list_ReadAddressStart[i];
                    string typeAddress = startAddress.Substring(0, 1);

                    if (typeInputAddress != typeAddress)
                        continue;

                    if (typeInputAddress == "W")
                        startNum = HexToDec(startAddress.Substring(1, startAddress.Length - 1)); // 주소의 숫자만 추출해서 16진수를 10진수로 변경

                    else
                        startNum = Convert.ToInt32(startAddress.Substring(1, startAddress.Length - 1));

                    endNum = startNum + list_ReadAddressCount[i];

                    //if (numInputAddress >= startNum && numInputAddress < endNum)
                    if (numInputAddress == startNum)
                    {
                        index = i; // zInterface Task 의 Read PLC 에서 읽어온 값들이 저장된 리스트 중 어느 인덱스인지
                        break;
                    }
                }


                // Input Address 가 포함 된 Word 값
                int arrIndex = 0;

                arrIndex = numInputAddress - startNum;

                Int16[] arrReadData = list_ReadValue[index]; // 해당 주소가 속해있는 배열

                string readVal = "";
                string strData = "";
                char[] str = new char[2];
                for (int i = arrIndex; i < arrIndex + dataLength; i++)
                {
                    Int16 readWord = arrReadData[i];
                    string a = char.ConvertFromUtf32(readWord);

                    if (readWord > 0)
                    {
                        strData = a;
                        readVal += strData;
                    }
                }

                return readVal.Trim();
            }
            catch
            {
                return "";
            }
        }
    }
}
