using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Process
{
    public abstract class CInterface
    {


        abstract public void Connect();
        abstract public void Disconnect();
        abstract public void InterfaceLoop();
        abstract public void ReadData();
        abstract public void WriteData();

        abstract public void Handshake();

    }


}
