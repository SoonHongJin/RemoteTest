using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;
using Core.Utility;

namespace Core.Function
{
    /// <summary> 22.05.09 LYK IDeepLearning Interface
    /// 룰 베이스 검사, 얼라인 등 검사하는 동작이 다르고 협업등을 고려 하여 동작 함수는 Interface로 구성
    /// </summary>
    interface IDeepLearning
    {
        void FindLabel(int _nInspecIdx, CImage _Image);
        void FindRoi();
    }
}
