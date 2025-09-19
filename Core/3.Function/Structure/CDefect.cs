using Cognex.VisionPro;
using Cognex.VisionPro.CalibFix;
using Core.Function;
using Core.Utility;
using Insnex.Vision2D.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using static Core.Program;

namespace Core.Function
{
    /// <summary> LYK 22.05.10 CDefect 클래스 
    /// 검출된 Defect의 x, y 좌표, 크기등의 정보를 가지는 클래스
    /// </summary>
    public class CDefect : IDisposable
    {
        //24.01.25 LYK 공통
        public int m_nIdx;                  //220510 LYK DefectIndex
        public int m_nCamNum;               //220510 LYK 캠 번호
        public string m_sProductName;       //220510 LYK 제품 이름
        public int m_nGrabIdx;              //220510 LYK 이미지 Grab Index
        public DateTime StartTime;          //240202 LYK StarTime
        public bool m_bRuleBaseJudge;     //220510 LYK 판정
        public bool m_bDeepLearningJudge; //220510 LYK 판정

        public int GrabTime;                //240313 LYK GrabTime;
        public int GrabCycleTime;
        public int WorkInspectTime;
        public int DeepLearnignTime;

        public string OriginImagePath;      //220510 LYK Original ImagePath
        public string DefectImagePath;      //220510 LYK Defect ImagePath

        public CImage DefectImage;
        public CInspectionResult InspResult;

        private object m_lock = new object();

        public CDefect()
        {
            InspResult = new CInspectionResult();

            DefectImage = new CImage(DEF_SYSTEM.LICENSES_KEY, theRecipe.m_sCurrentEquipment);
        }
        public void Dispose()
        {
            if (DefectImage != null)
            {
                DefectImage.Free();
            }
        }

        /// <summary> LYK 22.05.10 DataSet 함수 
        /// 검사 완료시 ObserverCollection을 이용해 데이터를 취합 할때 데이터를 할당 하기 위해 사용
        /// </summary>
        /// <param name="_Defect"> 검사된 Defect 정보 인자 전달</param>
        public void DataSet(CDefect _Defect)
        {
            //24.01.25 LYK 공통
            m_nIdx = _Defect.m_nIdx;
            m_nCamNum = _Defect.m_nCamNum;
            m_nGrabIdx = _Defect.m_nGrabIdx;
            m_sProductName = _Defect.m_sProductName;

            ///24.01.25 LYK 패턴 불량
            m_bRuleBaseJudge = _Defect.m_bRuleBaseJudge;
            m_bDeepLearningJudge = _Defect.m_bDeepLearningJudge;
            OriginImagePath = _Defect.OriginImagePath;

            InspResult      = _Defect.InspResult;
        }
    }
    public class CInspectionResult
    {
        public PointF DefectPos { get; set; }  //220510 LYK Defect 위치  (Defect Rect의 센터 x, y point)
        public double Width { get; set; } = 0;  //220510 LYK Defect 가로 길이
        public double Height { get; set; } = 0; //220510 LYK Defect 세로 길이
        public double Size { get; set; } = 0;    //220510 LYK Defect Area 크기
        public PointF[] m_InnerValue { get; set; }  //230830 LYK Defect Polygon

        public Color m_sClassColor = Color.White; //230613 LYK Saige Class Color RGB 값 
                                    //230830 LYK 딥러닝 뿐만 아니라, 일반 검사의 Defect에도 색상 부여 하는 것으로 사용 해도 무방

        public string m_sDefectType = string.Empty;        //230624 LYK DefectType

    }

    /// <summary> LYK 22.05.10 CDefectManager 클래스
    /// Defect를 관리 하는 클래스
    /// Defect 정보를 추가, 삭제 할 수 있음
    /// </summary>
    public class CDefectManager : ObservableCollection<CDefect>
    {
        private readonly object m_Lock = new object();                  //220510 LYK 공유자원에 접근하는 부분에 lock으로 블럭을 만들어주면 해당블럭은 하나의 스레드만 접근할 수 있게 됨( 동기화를 위해 필요 - Log 기록 등)

        private List<CDefect> ListDefects = new List<CDefect>();        //220510 LYK Defect를 취합 하기 위한 List

        public int TotalDefectCnt { get; private set; } = 0;            //22051 LYK Defect의 Total Count

        /// <summary> LYK 22.05.10 DefectClear 함수
        /// TotalDefectCnt Initial, Defect List Clear
        /// </summary>
        public void DefectClear()
        {
            TotalDefectCnt = 0;

            lock(m_Lock)
            {
                for (int i = 0; i < ListDefects.Count(); ++i)
                    ListDefects[i].Dispose();
            }
            ListDefects.Clear();
        }

        /// <summary> LYK 22.05.10 DefectAdd 함수
        /// List에 Defect 정보를 추가 
        /// </summary>
        /// <param name="_Defect">Defect 정보 인자 전달</param>
        public void DefectAdd(CDefect _Defect)
        {
            lock(m_Lock)
            {
                ListDefects.Add(_Defect);
            }
        }

        /// <summary> LYK 22.05.10 SaveDefectImage
        /// Defect Image(Crop)을 Labeling 하면서 진행 하고자 함수를 구성
        /// 사용하지 않고 있음
        /// </summary>
        /// <param name="_nInspectionIdx"></param>
        /// <param name="_FileName"></param>
        public void SaveDefectImage(int _nInspectionIdx, string _FileName)
        {
            
            //TotalDefectCnt = ListDefects.Count;

            //for(int i = 0; i < TotalDefectCnt; ++i)
            //{
            //    //string sPos = string.Format("{0}_{1}", ListDefects[i].m_nPos.X, ListDefects[i].m_nPos.Y);

            //    //ListDefects[i].DefectImagePath = _FileName + string.Format("{0}_{1}.jpg", i + 1, sPos);
            //    //ListDefects[i].DefectImages.SaveJpeg(ListDefects[i].DefectImagePath);
            //}
        }


        private int GetDefectCount()
        {
            return ListDefects.Count;   //220510 LYK ListCount Return
        }

        private CDefect GetAt(int _nIdx)
        {
            return ListDefects[_nIdx];  //220510 LYK ListIndex Return
        }

        /// <summary> LYK 22.05.10 GetCandidateManager 함수
        /// 검사 완료시 ObserverCollection을 이용해 Defect 데이터를 취합
        /// EX) - 카메라 두개가 존재 하는 경우
        ///     - 인자로 전달된 DefectManager 객체에 Cam1, Cam2에대한 Defect 데이터 취합
        /// </summary>
        /// <param name="_DefectManager"> DefectManager 객체를 인자로 전달</param>
        public void GetCandidateManager(CDefectManager _DefectManager)
        {
            for (int i = 0; i < GetDefectCount(); ++i)
            {
                CDefect defect = new CDefect();
                defect.DataSet(_DefectManager.GetAt(i));
                Add(defect);
            }
        }
    }
}
