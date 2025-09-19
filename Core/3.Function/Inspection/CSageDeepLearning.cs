using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Drawing;

using Core.Function;
using Core.Utility;

using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.Dimensioning;
using Cognex.VisionPro.Implementation.Internal;

using SaigeVision.Core;
using Saige.Infrastructure;
using SaigeVision.Core.Inference;

using Core;
using Core.DataProcess;
using static Core.Program;
using Cognex.VisionPro.Blob;
using System.Collections;


using Insnex.Vision2D.Core;
using Insnex.Vision2D.Common;
using Insnex.Vision2D.ToolBlock;
using Insnex.Vision2D.ImageProcessing;
using Insnex.Vision2D.Finder;
using Insnex.Vision2D.Intersection;
using Insnex.Vision2D.CalibFix;
using Insnex.Vision2D.Measurement;
using Core.UI;
using System.Windows.Forms;
using System.Security.Cryptography;
using ScottPlot.Renderable;
using System.Runtime.InteropServices;
using OpenCvSharp;
using System.Windows.Input;
using System.Drawing.Imaging;


namespace Core.Function
{
    class CSageDeepLearning : CWork, IInspection
    {
        private SrImage MonoInspectImage = null;
        private Uri ModelPath = null;
        private NvGpuService GpuService = null;
        private SegmentationEngine segEngine = null;
        private SegInspectionOptions segOption = null;

        private ClassificationEngine ClassifyEngine = null;     //20250910 SHJ DISPLAY 전용 Classify 추가 
        private ClsInspectionOptions Classifyoption = null;     //20250910 SHJ DISPLAY 전용 Classify 추가 

        private int nDefectCnt = 0;
        private int nColorStaindefectCnt = 0;

        public double m_nTotalInspTime = 0;
        private CogToolBlock[] CropImgToolBlock = new CogToolBlock[4];      //240128 LYK CropImgToolBlock 변수 추가
        private CogBlobTool[] PasteStainedBlobTool = new CogBlobTool[4];    //240525 LYK Paste Staiend BlobTool 변수 추가

        private CogPolygon Polygon = new CogPolygon();

        private string[] sModelFileName = new string[3];                    //250320 KCH ModelFileName [0] Segmentation [1] Classify [2] ColorStainSeg

        private InsBlobTool[] InsPasteStainedBlobTool = new InsBlobTool[4];    //240525 LYK Paste Staiend BlobTool 변수 추가


        public List<InsToolBlock> InsCropImageToolBlock;


        private CLogger Logger = null;
        public CSageDeepLearning(CLogger _Logger)
        {
            Logger = _Logger;
        }

        public override void Initialize(int _nWidth, int _nHeight)
        {
            SageDeepLearningInitialize(_nWidth, _nHeight);
        }

        public override void Uninitialize()
        {
            if (segEngine != null)
            {
                segEngine.Dispose();
                segEngine = null;
            }

            if (ClassifyEngine != null)
            {
                ClassifyEngine.Dispose();
                ClassifyEngine = null;
            }

            bInitialized = false;
        }
        private bool SageDeepLearningInitialize(int _nWidth, int _nHeight)
        {
            try
            {
                if (bInitialized == false)
                {
                    sModelFileName = WorkSpaceName.Split(',');

                    if (CamID == 0)
                    {
                        Width = _nWidth;
                        Height = _nHeight;

                        GpuService = new NvGpuService();
                        ModelPath = new Uri(sModelFileName[(int)DEEP_MODEL.Segmentation]);
                        GpuService.DetectConnectedGPUs();

                        if (GpuService.CurrentDetectedGPUs == null)
                        {
                            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "There is no GPU installed. Please Check GPU.");

                            return false;
                        }

                        //230630 LYK 딥러닝 모델 Load
                        // 20250910 SHJ 초기화 Display Electronic 구분 
                        if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.ELECTRONIC)
                            SegmentationInitialize();
                        else if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY)
                            ClassifyInitialize();

                        bInitialized = true;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Deep Learning Initial Catch : {e.Message}");
            }

            return bInitialized;

        }
        private void SegmentationInitialize()
        {
            try
            {
                segEngine = EngineLoader.LoadFrom<SegmentationEngine>(ModelPath);
            }
            catch (Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "There is No Segmentation Model File, Load Base Model File");
                segEngine = EngineLoader.LoadFrom<SegmentationEngine>(new Uri($"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{theRecipe.m_sCurrentModelName}\\{theRecipe.m_sCurrentModelName}_Base.srSeg"));
            }

            theRecipe.SegClassName.Clear();

            int nCnt = 0;
            int SegNameCount = segEngine.Classes.Count;

            for (int i = 0; i < SegNameCount; i++)
            {
                CSegMentationClass segClass = new CSegMentationClass();

                segClass.SegClassName = segEngine.Classes[i].Name;
                segClass.m_sClassColor = segEngine.Classes[i].Color;
                theRecipe.SegClassName.Add(segClass);
                //if (i < segEngine.Classes.Count)
                //{

                //    theRecipe.SegClassName.Add(segClass);
                //}
            }

            //250630 LYK MonoModel 로드헀을 때만 실행
            if (segEngine != null)
            {
                segOption = segEngine.ReadInspectionOptions();

                segOption.TimeLimit = new TimeSpan();
                segOption.IsMultiClasses = true;
                segOption.ContourOptions.SaveScoresOfPatch = false;
                segOption.ContourOptions.SaveAverageColorOfPatch = false;
                segOption.ContourOptions.SaveCroppedPatch = false;

                segEngine.WriteInspectionOptions(segOption);
                segEngine.AllocateNetwork(GpuService.CurrentDetectedGPUs[0], segOption);
            }
        }
        private void ClassifyInitialize()
        {
            try
            {
                ClassifyEngine = EngineLoader.LoadFrom<ClassificationEngine>(ModelPath);
            }
            catch (Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "There is No Classify Model File, Load Base Model File");
                //segEngine = EngineLoader.LoadFrom<SegmentationEngine>(new Uri($"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{theRecipe.m_sCurrentModelName}\\{theRecipe.m_sCurrentModelName}_Base.srSeg"));
            }

            Classifyoption = ClassifyEngine.ReadInspectionOptions();
            Classifyoption.TimeLimit = new TimeSpan();

            ClassifyEngine.WriteInspectionOptions(Classifyoption);
            ClassifyEngine.AllocateNetwork(GpuService.CurrentDetectedGPUs[0], Classifyoption);


            //for (int i = 0; i < m_InsClassify.InsAILabels.Count; i++)
            for (int i = 0; i < ClassifyEngine.Classes.Count; i++)
            {
                CClassifyClass cClassify = new CClassifyClass();

                cClassify.ClassifyName = ClassifyEngine.Classes[i].Name;
                cClassify.m_sClassColor = ClassifyEngine.Classes[i].Color;

                theRecipe.ClassifyName.Add(cClassify);
            }
        }

        public override void WorkRun(int _ImageCount, int _InspecIdx, List<CImage> _Images, int _nInspectionType)
        {
            //250612 NIS DeepLearning try-catch

            try
            {
                InspectionTime.Restart();

                // 20250917 SHJ 이미지 카운트 첫번째 일때 초기화 진행 이후 이미지 카운트 내용들 누적, 세이지는 직렬 실행 이기 때문에 마지막 이미지가 올때 누적 된 데이터 한번에 전달 
                if (_ImageCount == 0)
                {
                    // 20250910 SHJ 기존 데이터 Free or 삭제 
                    for (int i = 0; i < DefectCandidates.Count; i++)
                    {
                        if (DefectCandidates[i] != null)
                            DefectCandidates[i].Dispose();
                    }

                    DefectCandidates.Clear();
                }
                nDefectCnt = 0;

                // 20250910 SHJ Display & Electronic 구분 하여 실행 
                if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.ELECTRONIC)
                    Labeling(_ImageCount, _InspecIdx, _Images, MonoInspectImage);
                else if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY)
                    FindClassifyCation(_ImageCount, _InspecIdx, _Images);

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Deeplearning End, CellID : {this.sProductName}, ImageCount : {_ImageCount}, Process Time : {InspectionTime.ElapsedMilliseconds.ToString()} ms");
                InspectionTimeInfo?.Invoke(DEF_SYSTEM.INSP_SAGEDEEPLEARNING, InspectionTime.ElapsedMilliseconds);

                // 20250917 SHJ 최종 결과 Defect Manger 에 전달 하는내용 프로젝트 구분 
                int ImageCnt = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? theRecipe.SliceImageCount : theRecipe.MergeImageCount;


                // 20250909 SHJ DefectManager InpMode 에 따라 Insp or Teach 구분 하여 메모리 해제 및 Complete 할당 
                var DefectManager = _nInspectionType == DEF_SYSTEM.INSP_TEACH ? theMainSystem.ProductInfo.TeachDefectManager[DEF_SYSTEM.CAM_ONE][_ImageCount] 
                    : theMainSystem.ProductInfo.DefectManager[_InspecIdx][DEF_SYSTEM.CAM_ONE][_ImageCount];

                WorkComplete(DefectManager, null, _ImageCount, _InspecIdx);
                
                InspectionTime.Stop();

                JudgeComplete?.Invoke(_InspecIdx, _ImageCount);

            }
            catch (Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"CellID : {this.sProductName} Imge Cnt : {_ImageCount} DeepLearning _Inspection  Error Occured : {ex}");
                //JudgeComplete?.Invoke();
            }
        }


        private void CountourOptionSet(int[] _ScoreThre, int[] _AreaThre)
        {

            for (int i = 0; i < theRecipe.SegClassName.Count - 3; i++)//250329 KCH ColorStain Model Option 
            {
                if (i < segOption.ScoreThresholds.Length)
                {
                    segOption.ScoreThresholds[i] = _ScoreThre[i];
                    segOption.ContourOptions.AreaThresholds[i] = _AreaThre[i];
                }

            }
            segEngine.WriteInspectionOptions(segOption);
        }

        public override void WorkComplete(CDefectManager _DefectManager, CDefectManager _DTDefectManager, int _ImageCount, int _InspctIdx)
        {
            // 250910 SHJ Display 는 세로 할당하지 않아도 상관 없어서 전기 일 경우 DefectManager 추가 하도록 조건 처리
            if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.ELECTRONIC)
            {

                int nCnt = DefectCandidates.Count;
                int nStartIdx = nCnt - nDefectCnt;

                for (int i = nStartIdx; i < nCnt; ++i)
                {
                    CDefect defect = DefectCandidates[i];
                    _DefectManager.Add(defect);
                }
            }

            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {sProductName}, Image Count : {_ImageCount}, _InspecIdx : {_InspctIdx} , Deeplearning Logic Complete ");
        }

        public void FindDefect(int _nImageCnt, int _nInspecIdx, CImage _Image)
        {

        }

        private void FindClassifyCation(int _nImageCount, int _nInspIdx, List<CImage> _Images)
        {
            // 20250910 SHJ 작동 확인 하기 위해 임시 구현
            var DefectManager = theMainSystem.ProductInfo.DefectManager[_nInspIdx][this.CamID];

            ////20250910 SHJ Slice 된 이미지 마다 Crop 된 이미지를 SrImage 형태 배열로 만든 다음 실행 
            //for (int i = 0; i < DefectManager.Count; i++)
            //{
            //    SrImage[] InspectImages = new SrImage[DefectManager[i].Count];

            //    for (int j = 0; j < DefectManager[i].Count; j++)
            //        InspectImages[j] = new SrImage(DefectManager[i][j].DefectImage.m_nWidth, DefectManager[i][j].DefectImage.m_nHeight, DefectManager[i][j].DefectImage.m_nStride, 1, DefectManager[i][j].DefectImage.pData);

            //    ClassificationReport[] reports = ClassifyEngine.Inspect(InspectImages);

            //    for (int j = 0; j < DefectManager[i].Count; j++)
            //    {
            //        DefectManager[i][j].InspResult.m_sDefectType = reports[j].Prediction.Name;
            //        DefectManager[i][j].InspResult.m_sClassColor = reports[j].Prediction.Color;
            //        DefectManager[i][j].m_bDeepLearningJudge = reports[j].Prediction.Name.Contains("ETC") || reports[j].Prediction.Name.Contains("Hole") ? true : false;
            //    }
            //}

            // 20250910 SHJ 속도를 최적화 위해 디펙트 모든 이미지를 배열로 만들어서 1번만 런 하는 방식으로 처리 
            int nDefectCount = DefectManager.Sum(ImgList => ImgList.Count);

            SrImage[] InspectImages = new SrImage[nDefectCount];
            int ImageIdx = 0;
            int Idx = 0;
            for (int i = 0; i < DefectManager.Count; i++)
            {
                for (int j = 0; j < DefectManager[i].Count; j++)
                {
                    InspectImages[Idx] = new SrImage(DefectManager[i][j].DefectImage.m_nWidth, DefectManager[i][j].DefectImage.m_nHeight, DefectManager[i][j].DefectImage.m_nStride, 1, DefectManager[i][j].DefectImage.pData);
                    Idx++;
                }
            }

            ClassificationReport[] reports = ClassifyEngine.Inspect(InspectImages);

            ImageIdx = 0;
            Idx = 0;
            for (int i = 0; i < nDefectCount; i++)
            {
                DefectManager[ImageIdx][Idx].InspResult.m_sDefectType = reports[i].Prediction.Name;
                DefectManager[ImageIdx][Idx].InspResult.m_sClassColor = reports[i].Prediction.Color;
                DefectManager[ImageIdx][Idx].m_bDeepLearningJudge = reports[i].Prediction.Name.Contains("ETC") || reports[i].Prediction.Name.Contains("Hole") ? true : false;
                Idx++;

                if (Idx >= DefectManager[ImageIdx].Count)
                {
                    ImageIdx++;
                    Idx = 0;
                }
            }
        }

        private void Labeling(int _ImageCount, int _InspecIdx, List<CImage> _Images, SrImage _Image)
        {
            string message = string.Empty;

            try   
            {
                
                SrImage[] InspectImage = new SrImage[theRecipe.SliceImageCount];

                for (int i = 0; i < theRecipe.SliceImageCount; i++)
                {
                    InspectImage[i] = new SrImage(_Images[_ImageCount].m_nSliceWidth, _Images[_ImageCount].m_nHeight, _Images[_ImageCount].m_nSliceStride, 1, _Images[_ImageCount].pSliceData[i]);
                }

                int nWidth = _Images[_ImageCount].CogImage[0].Width;
                int nHeight = _Images[_ImageCount].CogImage[0].Height;

                SegmentationReport<SegContourResult>[] report = segEngine.Inspect<SegContourResult>(InspectImage);

                theRecipe.m_DefectAreaValue = 1;

                for (int reportIndex = 0; reportIndex < report.Length; reportIndex++)
                {
                    foreach (SegContourResult prediction in report[reportIndex].Prediction)
                    {
                        CDefect Defects = new CDefect();

                        Defects.m_nIdx = nDefectCnt;
                        Defects.m_nCamNum = CamID;
                        Defects.StartTime = StartTime;
                        Defects.m_nGrabIdx = _InspecIdx;
                        Defects.m_sProductName = this.sProductName;
                        Defects.DefectImagePath = this.sDefectImagePath;
                        Defects.OriginImagePath = sOriginalImagePath;   //240708 NWT OK,NG 이미지 경로 추가

                        Defects.InspResult.Width = prediction.Size.Width;
                        Defects.InspResult.Height = prediction.Size.Height;
                        Defects.InspResult.Size = Math.Round(Math.Sqrt(Defects.InspResult.Width * Defects.InspResult.Width + Defects.InspResult.Height * Defects.InspResult.Height), 3);
                        Defects.InspResult.DefectPos = new PointF((prediction.Center.X + (nWidth * reportIndex)) / 4, (prediction.Center.Y + (nHeight * _ImageCount)) / 4);
                        Defects.InspResult.m_sClassColor = prediction.Class.Color;
                        Defects.InspResult.m_sDefectType = prediction.Class.Name;

                        Defects.InspResult.m_InnerValue = prediction.Contour.Value;

                        for (int i = 0; i < Defects.InspResult.m_InnerValue.Length; i++)
                        {
                            Defects.InspResult.m_InnerValue[i].X = ((Defects.InspResult.m_InnerValue[i].X + nWidth * reportIndex) / 4);
                            Defects.InspResult.m_InnerValue[i].Y = ((Defects.InspResult.m_InnerValue[i].Y + nHeight * _ImageCount) / 4);
                        }

                        // Defect Image Copy
                        int CropWidth = prediction.Size.Width + DEF_SYSTEM.DEFECT_CROP_WIDTH;
                        int CropHeight = prediction.Size.Height + DEF_SYSTEM.DEFECT_CROP_HEIGHT;
                        int CropX = (int)prediction.Center.X - (CropWidth / 2);
                        int CropY = (int)prediction.Center.Y - (CropHeight / 2);

                        Defects.DefectImage.Crop(_Images[_ImageCount], _Images[_ImageCount].m_nPixelFormat, CropWidth, CropHeight, CropX, CropY, reportIndex);

                        //250803 LYK fale : NG, true : OK
                        Defects.m_bDeepLearningJudge = false;
                        Defects.m_bRuleBaseJudge = true;

                        if (Defects.InspResult.DefectPos.X < 0 || Defects.InspResult.DefectPos.Y < 0)
                        {
                            Defects.Dispose();
                            Defects = null;
                            continue;
                        }

                        //DefectCandidates.DefectAdd(Defects);
                        DefectCandidates.Add(Defects);

                        nDefectCnt++;
                    }
                }
                
            }
            catch (System.Exception e)
            {
                message = $"CamNum : {CamID}, InspectionID {_ImageCount} Error Message : " + e.Message;
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], message);
            }
            finally
            {

            }

        }
        public void FindRoi(int _nImageCnt, int _nInspecIdx, CImage _Image)
        {

        }

        public override void InspectionOptionSet(int[] _ScoreThre, int[] _AreaThre)
        {
            CountourOptionSet(_ScoreThre, _AreaThre);
        }

        private void InnerToImageCrop(CDefect defect ,PointF[] Inner , CImage _Image)
        {
            /*
            double Width = 0;
            double Height = 0;
            if(Inner.Length > 200)
            {

            }

            if(Inner.Length > 0)
            {
                // 20250917 SHJ 형태의 최대 가로 세로 계산 
                Width = Inner.Max(p => p.X) - Inner.Min(p => p.X);
                Height = Inner.Max(p => p.Y) - Inner.Min(P =>  P.Y);

                // 가로, 세로 어느 곳이 더 긴 형태인지 체크 
                bool Horizontal = Width > Height ? true : false;

                // 무조건 올림 처리 하여서 이미지가 잘리지 않도록 처리 
                int ImageCnt = Horizontal ? (int) Math.Ceiling(Width / DEF_SYSTEM.DEFECT_CROP_WIDTH) : (int) Math.Ceiling(Height / DEF_SYSTEM.DEFECT_CROP_HEIGHT);
                int CropSize = Horizontal ? DEF_SYSTEM.DEFECT_CROP_WIDTH : DEF_SYSTEM.DEFECT_CROP_HEIGHT;

                for (int i = 0; i < ImageCnt; i ++)
                {
                    bool ImageAdd = false;

                    for (int j = 0; j < Inner.Count(); j ++)
                    {
                        float pt = Horizontal ? Inner[j].X : Inner[j].Y;
                        float Min = Horizontal ? Inner.Min(p => p.X) : Inner.Min(p => p.Y);

                        if(pt > Min + i * CropSize) // 다음 센터 구간으로 점프 
                        {
                            // Defect Image Crop Copy
                            int CropX = (int)Inner[j].X - (DEF_SYSTEM.DEFECT_CROP_WIDTH / 2);
                            int CropY = (int)Inner[j].Y - (DEF_SYSTEM.DEFECT_CROP_HEIGHT / 2);
                            int CropWidth = DEF_SYSTEM.DEFECT_CROP_WIDTH;
                            int CropHeight = DEF_SYSTEM.DEFECT_CROP_HEIGHT;

                            CImage img = new CImage(DEF_SYSTEM.LICENSES_KEY, theRecipe.m_sCurrentEquipment);
                            img.Crop(_Image, _Image.m_nPixelFormat, CropWidth, CropHeight, CropX, CropY);
                            defect.DefectImages.Add(img);

                            ImageAdd = true;

                            break;
                        }
                    }

                    // SHJ 마지막 잘린 Inner 가 센터 위치까지 길이가 되지 않을 경우 이미지 카운트가 남았지만 이미지 Crop 을 못 할수 있음
                    // 마지막 Crop 카운트일 경우 & 위에 이미지 생성 하지 않고 현재 조건문 진입이 되면 마지막 Crop 영역 부터 1pixel 이라도 큰 위치를 기준으로 최종 Crop 진행 
                    if(!ImageAdd && i + 1 == ImageCnt)
                    {
                        for (int j = 0; j < Inner.Count(); j++)
                        {
                            float pt = Horizontal ? Inner[j].X : Inner[j].Y;
                            float Min = Horizontal ? Inner.Min(p => p.X) : Inner.Min(p => p.Y);

                            if (pt > Min + i * (CropSize/2)) // 다음 센터 구간이 아닌 1Pixel 이라도 클 경우 
                            {
                                // Defect Image Crop Copy
                                int CropX = (int)Inner[j].X - (DEF_SYSTEM.DEFECT_CROP_WIDTH / 2);
                                int CropY = (int)Inner[j].Y - (DEF_SYSTEM.DEFECT_CROP_HEIGHT / 2);
                                int CropWidth = DEF_SYSTEM.DEFECT_CROP_WIDTH;
                                int CropHeight = DEF_SYSTEM.DEFECT_CROP_HEIGHT;

                                CImage img = new CImage(DEF_SYSTEM.LICENSES_KEY, theRecipe.m_sCurrentEquipment);
                                img.Crop(_Image, _Image.m_nPixelFormat, CropWidth, CropHeight, CropX, CropY);
                                defect.DefectImages.Add(img);

                                break;
                            }
                        }
                    }
                }
            }
            */
        }

        /// <summary>
        /// 24.05.25 LYK PasteStainedRulebase 함수 추가
        /// PasteStained 룰베이스 검사 함수 추가
        /// </summary>
        /// <param name="_Id"></param>
        /// <param name="prediction"></param>
        /// <param name="_Images"></param>
        /// <param name="_Image"></param>
        /// <param name="Defects"></param>
        private void PasteStainedRulebase(int _Id, SegContourResult prediction, CImage _Images, SrImage _Image, CDefect Defects)
        {
            //double PixelResolution = theMainSystem.m_darrPixelResolution[_Id];    임시 주석
            CogPolygon Polygon = new CogPolygon();

            //250224 NWT 임시처리 CogGrayImage
            //PasteStainedBlobTool[_Id].InputImage = _Images.CogGrayImage;

            for (int i = 0; i < Defects.InspResult.m_InnerValue.Length; i++)
            {
                Polygon.AddVertex(Defects.InspResult.m_InnerValue[i].X, Defects.InspResult.m_InnerValue[i].Y, i);
            }

            PasteStainedBlobTool[_Id].Region = Polygon;
            PasteStainedBlobTool[_Id].Run();

            //DefectPoint.Clear();
            if (PasteStainedBlobTool[_Id].Results.GetBlobs().Count > 0)
            {
                Polygon = PasteStainedBlobTool[_Id].Results.GetBlobs()[0].GetBoundary();


                PointF[] DefectPoint = new PointF[Polygon.NumVertices];
                //Array.Resize(ref DefectPoint, Polygon.NumVertices);
                for (int i = 0; i < Polygon.NumVertices; i++)
                {
                    //DefectPoint.Add(new PointF((float)Polygon.GetVertexX(i), (float)Polygon.GetVertexY(i)));
                    DefectPoint[i].X = (float)Polygon.GetVertexX(i);
                    DefectPoint[i].Y = (float)Polygon.GetVertexY(i);
                }

                Defects.InspResult.DefectPos = new PointF((float)(PasteStainedBlobTool[_Id].Results.GetBlobs()[0].CenterOfMassX), (float)(PasteStainedBlobTool[_Id].Results.GetBlobs()[0].CenterOfMassY));
                Defects.InspResult.Width = Math.Round(PasteStainedBlobTool[_Id].Results.GetBlobs()[0].GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeWidth) * 0.007 /** (theRecipe.m_dWidthOffsetPer / 100.0)*/, 3);
                Defects.InspResult.Height = Math.Round(PasteStainedBlobTool[_Id].Results.GetBlobs()[0].GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeWidth) * 0.007 /** (theRecipe.m_dHeightOffsetPer / 100.0)*/, 3);

                Defects.InspResult.Size = Math.Round(0.007 * 0.007 * PasteStainedBlobTool[_Id].Results.GetBlobs()[0].Area, 3);

                Array.Clear(Defects.InspResult.m_InnerValue, 0x00, Defects.InspResult.m_InnerValue.Length);
                Defects.InspResult.m_InnerValue = null;
                Defects.InspResult.m_InnerValue = DefectPoint;//DefectPoint.ToArray();
            }

            Polygon.Dispose();
        }

        public override void DefectCopy(CDefectManager _SrcDefectManager)
        {
            throw new NotImplementedException();
        }

        public override CogToolBlock GetInspToolBlock()
        {
            throw new NotImplementedException();
        }

        public override void OptionSet(int[] _ScoreThre, int[] _AreaThre)
        {
            throw new NotImplementedException();
        }
    }
}
