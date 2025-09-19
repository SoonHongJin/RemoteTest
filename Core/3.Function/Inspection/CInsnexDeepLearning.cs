using Cognex.VisionPro;
using Core.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insnex.AI;

using static Core.Program;
using System.IO;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.ImageFile;
using Core.DataProcess;
using Cognex.VisionPro.ToolBlock;

namespace Core.Function
{
    public class CInsnexDeepLearning : CWork, IDeepLearning
    {
        private CLogger Logger = null;

        private InsAIClassifyTool m_InsClassify;

        CogIPOneImageTool m_ImgTool = new CogIPOneImageTool();
        CogRectangle CropRect = new CogRectangle();
        Task task = null;

        public CInsnexDeepLearning(CLogger _logger)
        {
            Logger = _logger;
        }

        public override void Initialize(int _nWidth, int _nHeight)
        {
            if (!bInitialized)
            {
                Width = _nWidth;
                Height = _nHeight;

                m_InsClassify = new InsAIClassifyTool($"{WorkSpaceName}.rt", $"{WorkSpaceName}.yaml");

                // 240430 SHJ 첫 인덱스에만 컬러 정보 전달 
                // Insnex 딥러닝은 결과 컬러를 받아도 색상이 표시가 안됨 이건 피드백 받아야 할 내용 
                if (this.InspectIdx == 0)
                {
                    for (int i = 0; i < m_InsClassify.InsAILabels.Count; i++)
                    {
                        CClassifyClass cClassify = new CClassifyClass();

                        cClassify.ClassifyName = m_InsClassify.InsAILabels[i].name;
                        cClassify.nSetScore = theRecipe.m_nDefectScoreThreshHold[i];

                        if (cClassify.ClassifyName == "Ok")
                            cClassify.m_sClassColor = Color.FromArgb(0, 255, 0);
                        else if (cClassify.ClassifyName == "Particle")
                            cClassify.m_sClassColor = Color.FromArgb(255, 0, 0);
                        else if (cClassify.ClassifyName == "Crack")
                            cClassify.m_sClassColor = Color.FromArgb(255, 255, 0);
                        else if (cClassify.ClassifyName == "Chip")
                            cClassify.m_sClassColor = Color.FromArgb(255, 85, 0);
                        else if (cClassify.ClassifyName == "Scratche")
                            cClassify.m_sClassColor = Color.FromArgb(85, 0, 255);
                        else if (cClassify.ClassifyName == "PinHole")
                            cClassify.m_sClassColor = Color.FromArgb(255, 0, 255);
                        else if (cClassify.ClassifyName == "Dent")
                            cClassify.m_sClassColor = Color.FromArgb(0, 0, 255);
                        else if (cClassify.ClassifyName == "Dust")
                            cClassify.m_sClassColor = Color.FromArgb(85, 255, 255);
                        else if (cClassify.ClassifyName == "Tackiness")
                            cClassify.m_sClassColor = Color.FromArgb(180, 165, 255);

                        theRecipe.ClassifyName.Add(cClassify);
                    }
                }

                bInitialized = true;
            }
        }
        public override void Uninitialize()
        {
            if(bInitialized)
            {
                bInitialized = false;
            }
        }
        public override void WorkRun(int _Id, int _InspecIdx, List<CImage> _Images, int _nInspectionType)
        {

            InspectionTime.Restart();

            this.CamID = _Id;

            InspectIdx = _InspecIdx;

            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {sProductName}, InspecIdx : {_InspecIdx}, DeepLearning Start! ");

            int nDefectCnt = theMainSystem.ProductInfo.DefectManager[this.CamID].Count;

            // 240430 SHJ 
            // 실행중 에러나는 부분 찾기위해 Try 문 제외
            // 딥러닝도 병렬처리 할려다가 앞에 이미지 Merge 만큼 검사 할 시간이 확보가 되어있기 때문에 딥러닝은 병렬처리 x

            // NG 일 경우 딥러닝 실행
            if (!DefectCandidates[0].m_bRuleBaseJudge)
            {
                //try
                {
                    FindLabel(InspectIdx, _Images[0]);
                }
                //catch
                //{

                //}
            }

            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {sProductName}, InspecIdx : {_InspecIdx}, DeepLearning End! ");

            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {sProductName}, InspecIdx : {_InspecIdx}, DeepLearning RunTime : {InspectionTime.ElapsedMilliseconds} ");

            DefectCandidates[0].DeepLearnignTime = (int)InspectionTime.ElapsedMilliseconds;

            // 200515 SHJ 입력 받은 iNSPETION Type 에 따라 Complete 호출 여부 결정 
            //if (_nInspectionType == DEF_SYSTEM.INSP_NORMAL) // -> Inspection Normal 일 경우 컴플레트 이벤트 호출 (Teaching 같은 경우는 호출 X)
            //    JudgeComplete?.Invoke(this.InspectIdx);

        }

        public override void WorkComplete(CDefectManager _DefectManager, CDefectManager _DTDefectManager, int _ImageCount, int _InspctIdx)
        {
            int nCnt = DefectCandidates.Count;

            for (int i = 0; i < nCnt; ++i)
            {
                CDefect defect = DefectCandidates[i];
                _DefectManager.Add(defect);
            }
        }

        public void FindLabel(int _nInspecIdx, CImage _Image)
        {

            for(int i = 0; i < DefectCandidates.Count; i ++)
            {
                // 결과 초기화  
                DefectCandidates[i].m_bDeepLearningJudge = true;

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {sProductName}, InspecIdx : {_nInspecIdx}, DeepLearning Run {i} ");

                // Pointer Image -> CogImage 변환 
                //using (CogImage8Root root8 = new CogImage8Root())
                //{
                //    root8.Initialize(DefectCandidates[i].DefectImage.m_nWidth, DefectCandidates[i].DefectImage.m_nHeight, DefectCandidates[i].DefectImage.pData, DefectCandidates[i].DefectImage.m_nStride, null);
                //    DefectCandidates[i].DefectImage.CogGrayImage.SetRoot(root8);
                //    DefectCandidates[i].DefectImage.CogImage = DefectCandidates[i].DefectImage.CogGrayImage;
                //}
                //
                //m_InsClassify.InputImage = DefectCandidates[i].DefectImage.CogImage;
                //m_InsClassify.Run();

                DefectCandidates[i].InspResult.m_sDefectType = m_InsClassify.maxResult.name;
                DefectCandidates[i].InspResult.m_sClassColor = Color.FromArgb(m_InsClassify.maxResult.dispalyColor.ColorR, m_InsClassify.maxResult.dispalyColor.ColorG, m_InsClassify.maxResult.dispalyColor.ColorB);

                for(int j = 0; j < theRecipe.ClassifyName.Count; j ++)
                {
                    if(DefectCandidates[i].InspResult.m_sDefectType == theRecipe.ClassifyName[j].ClassifyName)
                    {
                        // Recipe 보관된 스코어 값 비교 
                        double Score = (double) theRecipe.ClassifyName[j].nSetScore / 100;

                        if(m_InsClassify.maxResult.score < Score) // 설정 된 스코어 보다 낮으면 ng 처리
                            DefectCandidates[i].m_bDeepLearningJudge = false;
                        else
                            DefectCandidates[i].m_bDeepLearningJudge = true;

                        break;
                    }
                }
                
                if (DefectCandidates[i].InspResult.m_sDefectType == "OK")
                    DefectCandidates[i].m_bDeepLearningJudge = true;
            }

        }

        public void FindRoi()
        {

        }

        public override void OptionSet(int[] _ScoreThre, int[] _AreaThre)
        {

        }

        public override void DefectCopy(CDefectManager _SrcDefectManager)
        {
            // 데이터 클리어
            for (int i = 0; i < DefectCandidates.Count; i++)
                DefectCandidates[i].Dispose();

            DefectCandidates.DefectClear();
            DefectCandidates.Clear();

            int nCnt = _SrcDefectManager.Count;

            // 240430 SHJ 입력 된 디펙스매니저 정보 저장 
            for (int i = 0; i < nCnt; ++i)
            {
                CDefect defect = _SrcDefectManager[i];
                DefectCandidates.Add(defect);
            }
        }

        public override CogToolBlock GetInspToolBlock()
        {
            return new CogToolBlock();
        }

        public override void InspectionOptionSet(int[] _ScoreThre, int[] _AreaThre)
        {
            throw new NotImplementedException();
        }
    }
}
