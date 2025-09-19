using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

using Core.Utility;
using Core;
using static Core.Program; //전역 변수

namespace Core.HardWare
{
    public class CTcpServer
    {
        private int nPort = 0;
        private string sIpAddress = string.Empty;
        private byte[] ReceiveBuffer = new byte[DEF_SYSTEM.TCP_BUFFER_SIZE];
        private byte[] MessageBuffer = new byte[DEF_SYSTEM.TCP_BUFFER_SIZE];
        private CThread QueueThread = null;
        private object QueLock = new object();
        private Socket ServerSocket = null;
        private Socket ClientSocket = null;

        private byte[] TempBuffer = new byte[DEF_SYSTEM.TCP_BUFFER_SIZE];
        private bool bIsConnected = false;
        private int ReceiveBufferSize = 0;
        private Action<bool> RefreshUIStatus = null; //220504 LYK UI에 Connect 여부 표시
        private CLogger Logger = null;

        public CTcpServer(CLogger _logger)
        {
            Logger = _logger;
            QueueThread = new CThread()
            {
                Work = Poping,
                nSleepTime = 1
            };


        }

        public void Initialize(int _nPort, string _sAddress)
        {
            nPort = _nPort;
            sIpAddress = _sAddress;
            QueueThread.ThreadStart();
            ServerOpen();
        }

        public void UnInitialize()
        {
            QueueThread.ThreadStop();
            ServerClose();
        }

        private void ServerOpen()
        {
            string message = string.Empty;

            try
            {
                ReceiveBufferSize = 0;

                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(sIpAddress), nPort);
                ServerSocket.Bind(endPoint);
                ServerSocket.Listen(10);
                ServerSocket.BeginAccept(OnAccept, null);

                message = "Server Open is Completed";
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            finally
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], message);
            }
        }

        private void ServerClose()
        {
            if (bIsConnected)
            {
                ServerSocket.Disconnect(true);
                bIsConnected = false;
            }
        }
        private void OnAccept(IAsyncResult asyncResult)
        {
            string message = string.Empty;

            try
            {
                ClientSocket = ServerSocket.EndAccept(asyncResult);
                ServerSocket.BeginAccept(OnAccept, null);

                CAsyncObject AsyncObj = new CAsyncObject(DEF_SYSTEM.TCP_BUFFER_SIZE);
                AsyncObj.WorkingSocket = ClientSocket;

                bIsConnected = true;
                RefreshUIStatus?.Invoke(false);
                message = $"ClientPC: ({ClientSocket.RemoteEndPoint} is Connected)";

                ClientSocket.BeginReceive(AsyncObj.Buffer, 0, DEF_SYSTEM.TCP_BUFFER_SIZE, 0, OnReceived, AsyncObj);
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            finally
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], message);
            }
        }

        private void Poping()
        {
            while (Pop())
                Thread.Sleep(1);
        }

        private void Push(byte[] data, int dataSize)
        {
            lock (QueLock)
            {
                Array.Copy(data, 0, ReceiveBuffer, ReceiveBufferSize, dataSize);
                ReceiveBufferSize += dataSize;
            }
        }
        private bool Pop()
        {
            bool bETXFind = false;
            int length = 0;

            lock (QueLock)
            {
                int nStxID = Array.IndexOf(ReceiveBuffer, DEF_SYSTEM.TCP_STX);

                if (-1 != nStxID)
                {
                    length = (ReceiveBuffer[nStxID + 2] << 8) | ReceiveBuffer[nStxID + 3];

                    if (ReceiveBufferSize - nStxID >= length)
                    {
                        if (DEF_SYSTEM.TCP_ETX == ReceiveBuffer[nStxID + length - 1])
                            bETXFind = true;
                    }
                }

                if (bETXFind)
                {
                    Array.Copy(ReceiveBuffer, nStxID, MessageBuffer, 0, length);
                    ReceiveBufferSize -= (nStxID + length);

                    if (ReceiveBufferSize > 0)
                    {
                        Array.Copy(ReceiveBuffer, nStxID + length, TempBuffer, 0, ReceiveBufferSize);
                        Array.Copy(TempBuffer, 0, ReceiveBuffer, 0, ReceiveBufferSize);
                    }
                }
            }

            if (bETXFind)
                OnMessage(MessageBuffer, length);

            if (bETXFind && ReceiveBufferSize > 0)
                return true;

            return false;
        }

        private void OnReceived(IAsyncResult _asyncResult)
        {
            string message = string.Empty;
            CAsyncObject asyncObject = (CAsyncObject)_asyncResult.AsyncState;

            try
            {
                int received = asyncObject.WorkingSocket.EndReceive(_asyncResult);

                if (received <= 0)
                {
                    ClientSocket.Close();
                    return;
                }
                else
                {
                    Push(asyncObject.Buffer, received);
                    QueueThread.Continue();
                }
                asyncObject.WorkingSocket.BeginReceive(asyncObject.Buffer, 0, received, 0, OnReceived, asyncObject);

            }
            catch (Exception e)
            {
                message = e.Message;
            }
            finally
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], message);
            }
        }

        private void OnMessage(byte[] _message, int _nMeesageSize)
        {
            int nSTX = _message[0];
            int nPcNumber = _message[1];
            int nLength = (_message[2] << 8) | _message[3];
            byte Command = _message[4];
            byte sum = 0;

            for (int i = 0; i < nLength - 2; i++)
                sum += _message[i];

            if (sum == _message[nLength - 1])
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
