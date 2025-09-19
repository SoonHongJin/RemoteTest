using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;
using Cognex.VisionPro.Implementation;
using Core.Utility;

namespace Core.Function
{
    interface IInspection
    {
        void FindDefect(int _nImageCnt, int _nInspecIdx, CImage _Image);
        void FindRoi(int _nImageCnt, int _nInspecIdx, CImage _Image);

    }
}
