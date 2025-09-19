using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

using static Core.Program;
using Core;
using Core.Function;
using Core.Process;
using Core.Utility;
using Cognex.VisionPro.ToolBlock;

namespace Core.Function
{
    /// <summary> LYK 22.05.09 CInspect 클래스
    /// 모든 검사에 관련한 부모 클래스
    /// 추상 클래스로 구현 - 검사 특성상 공통된 기능으로 나열 가능 하고 다중 상속이 필요 없다고 판단 되어 추상 클래스로 구성(기능 구현 강제 목적)
    /// Initialize, UnInitialize, InspectionRun, InspectionComplete 함수의 구현 강제
    /// </summary>
    public abstract class CWork
    {
        public int CamID { get; set; } = 0;
        public int InspectIdx { get; set; } = 0;
        public int ImageCount { get; set; } = 0;
        public int InspMode { get; set; } = 0; // 20250911 SHJ 입력받은 InspMode 담아두기 위해 추가 
        public bool bInitialized = false;
        public string WorkSpaceName = null;

        public CDefectManager DefectCandidates = new CDefectManager();              //230105 LYK 검사 결과 결과 취합 변수(ObserverCollection)

        public Action<int, int> JudgeComplete = null;                                         //240124 LYK Judge Complete 델리게이트 변수(객체 별로 운용 됨)
        public Action<int, long> InspectionTimeInfo = null;                         //240128 LYK InspectionTimeInfo 델리게이트 변수(검사 종류, 인스펙션 시간)
        protected Stopwatch InspectionTime = new Stopwatch();                       //240128 LYK InspectionTime StopWatch

        protected int Width = 0;
        protected int Height = 0;

        public DateTime StartTime;       //240202 LYK StartTime;

        public string sProductName = null;
        public string sOriginalImagePath = null;
        public string sDefectImagePath = null;
        public string sCsvPath = null;
        public string sCropFolder = null;
        public string sCsvFolder = null;

        public long CameraGrabTime = 0;
        public long GrabCycleTime = 0;

        // =============================================================================================================================================

        abstract public void Initialize(int _nWidth, int _nHeight);
        abstract public void Uninitialize();
        abstract public void DefectCopy(CDefectManager _SrcDefectManager);
        abstract public void InspectionOptionSet(int[] _ScoreThre, int[] _AreaThre);
        abstract public void WorkRun(int _Id, int _InspecIdx, List<CImage> _Images, int _nInspectionType);
        abstract public void WorkComplete(CDefectManager _DefectManager, CDefectManager _DTDefectManager, int _ImageCount, int _InspctIdx);

        abstract public CogToolBlock GetInspToolBlock();
        abstract public void OptionSet(int[] _ScoreThre, int[] _AreaThre);
        public static CWork WorkPick(int _nType, int _nID, string _sWorkSpaceName)
        {
            try
            {
                switch (_nType)
                {

                    case DEF_SYSTEM.PARTICLE_INSPECTION:
                        return new CParticleInspection(theMainSystem.GetLogger)
                        {
                            CamID = _nID,
                            WorkSpaceName = _sWorkSpaceName
                            //InspectToolBlock = theRecipe.InspToollBlock[_nID],
                        };

                    case DEF_SYSTEM.INSNEX_DEEPLEARNING:
                        return new CInsnexDeepLearning(theMainSystem.GetLogger)
                        {
                            CamID = _nID,
                            WorkSpaceName = _sWorkSpaceName
                            //InspectToolBlock = theRecipe.InspToollBlock[_nID],
                        };

                    case DEF_SYSTEM.INSP_SAGEDEEPLEARNING:
                        return new CSageDeepLearning(theMainSystem.GetLogger)
                        {
                            CamID = _nID,
                            WorkSpaceName = _sWorkSpaceName
                        };
                }
            }
            catch(Exception e)
            {
                CLogger Logger = theMainSystem.GetLogger;
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Work Pick Error Catch : {e.Message}");
            }

            return null;
        }
    }

    
}
