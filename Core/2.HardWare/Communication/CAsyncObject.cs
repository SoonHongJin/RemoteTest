using System;
using System.IO;
using System.Net.Sockets;

namespace Core.HardWare
{
    class CAsyncObject
    {
        public byte[] Buffer;
        public Socket WorkingSocket;
        public readonly int BufferSize;

        //public FileStream FileStr;
        //public BinaryWriter Writer;

        //public int nPCNumber = 0;

        public CAsyncObject(int _BufferSize)
        {
            BufferSize = _BufferSize;
            Buffer = new byte[BufferSize];
        }

        public void ClearBuffer()
        {
            Array.Clear(Buffer, 0, BufferSize);
        }
    }
}
