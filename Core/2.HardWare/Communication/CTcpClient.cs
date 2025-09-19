using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

using Core;
using Core.Utility;
using static Core.Program;
using MetaLog;

namespace Core.HardWare
{
    public class CTcpClient
    {
        private byte[] ReceiverBuffer = new byte[DEF_SYSTEM.TCP_BUFFER_SIZE];
        private byte[] MessageBuffer = new byte[DEF_SYSTEM.TCP_BUFFER_SIZE];
        private byte[] TempBuffer = new byte[DEF_SYSTEM.TCP_BUFFER_SIZE];
        private CThread QueueThread = null;
        private object QueueLock = new object();
        private int nPort = 0;
        private string sAddress = string.Empty;
        private int nReceiveBufferSize = 0;

        public bool bIsConnected = false;
        public Socket ClientSocket = null;

        private CLogger Logger = null;
        public CTcpClient(CLogger _logger)
        {
            Logger = _logger;
            QueueThread = new CThread
            {
                Work = Poping,
                nSleepTime = 1
            };
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        }

        public void Initialize(int _nPort, string _sAddress)
        {
            nPort = _nPort;
            sAddress = _sAddress;
            QueueThread.ThreadStart();
        }

        public void UnInitialize()
        {
            QueueThread.ThreadStop();
        }

        private void Connect()
        {
            string message = string.Empty;

            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(sAddress), nPort);
                ClientSocket.Connect(sAddress, nPort);

                CAsyncObject asyncObject = new CAsyncObject(DEF_SYSTEM.TCP_BUFFER_SIZE);
                asyncObject.WorkingSocket = ClientSocket;

                nReceiveBufferSize = 0;
                ClientSocket.BeginReceive(asyncObject.Buffer, 0, asyncObject.BufferSize, 0, OnReceived, asyncObject);

                message = $"({ClientSocket.RemoteEndPoint} is Connected)";

                bIsConnected = true;
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            finally
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Client : " + message);
            }
        }

        private void DisConnect()
        {
            if (bIsConnected)
            {
                ClientSocket.Disconnect(false);
                ClientSocket.Close();
            }

            bIsConnected = false;
        }

        private void Push(byte[] data, int dataSize)
        {
            lock (QueueLock)
            {
                Array.Copy(data, 0, ReceiverBuffer, nReceiveBufferSize, dataSize);
                nReceiveBufferSize += dataSize;
            }
        }

        private bool Pop()
        {
            bool bETXFind = false;
            int length = 0;
            string message = string.Empty;

            try
            {
                lock (QueueLock)
                {
                    int nStxID = Array.IndexOf(ReceiverBuffer, DEF_SYSTEM.TCP_STX);

                    if (nStxID != -1)
                    {
                        length = (ReceiverBuffer[nStxID + 2] << 8) | ReceiverBuffer[nStxID + 3];
                        if (nReceiveBufferSize - nStxID >= length)
                        {
                            if (DEF_SYSTEM.TCP_ETX == ReceiverBuffer[nStxID + length - 1])
                                bETXFind = true;
                        }
                    }

                    if (bETXFind)
                    {
                        Array.Copy(ReceiverBuffer, 0, MessageBuffer, 0, length);
                        nReceiveBufferSize -= (nStxID + length);

                        if (nReceiveBufferSize > 0)
                        {
                            Array.Copy(ReceiverBuffer, 0, TempBuffer, 0, nReceiveBufferSize);
                            Array.Copy(TempBuffer, 0, ReceiverBuffer, 0, nReceiveBufferSize);
                        }

                        if (bETXFind)
                            OnMessage(MessageBuffer, length);

                        if (bETXFind && nReceiveBufferSize > 0)
                            return true;
                    }
                }

            }
            catch (Exception e)
            {
                message = e.Message;
            }
            finally
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], message);
            }

            return false;
        }

        public void Poping()
        {
            while (Pop())
                Thread.Sleep(1);

        }

        private void OnReceived(IAsyncResult asyncResult)
        {
            CAsyncObject asyncObject = (CAsyncObject)asyncResult.AsyncState;
            string message = string.Empty;

            try
            {
                int nReceived = asyncObject.WorkingSocket.EndReceive(asyncResult);

                if (nReceived <= 0)
                {
                    ClientSocket.Close();
                    return;
                }
                else
                {
                    Push(asyncObject.Buffer, nReceived);
                    QueueThread.Continue();
                }

                asyncObject.WorkingSocket.BeginReceive(asyncObject.Buffer, 0, DEF_SYSTEM.TCP_BUFFER_SIZE, 0, OnReceived, asyncObject);
            }
            catch (Exception e)
            {
                message = e.Message;
                bIsConnected = false;
                asyncObject.ClearBuffer();
            }
            finally
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], message);
            }
        }

        private void OnMessage(byte[] data, int dataSize)
        {
            int nStx = data[0];
            int nPcNumber = data[1];
            int nlength = (data[2] << 8) | data[3];
            int nSum = 0;
            byte Command = data[4];

            for (int i = 0; i < dataSize - 2; ++i)
                nSum += data[i];

            if (nSum == data[dataSize - 2])
            {
                switch (Command)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                }
            }
        }

    }
}
