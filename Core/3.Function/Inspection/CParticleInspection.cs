using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

using Cognex.VisionPro;
using Cognex.VisionPro.Implementation;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.Dimensioning;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.SearchMax;

using Core;
using Core.Utility;
using Core.Function;
using static Core.Program;
using Insnex.Vision2D.Common;
using static Insnex.VisionCore.InsPrimitiveDataCollection;
using static Insnex.Vision2D.Core.InsMapTool;
using static Insnex.Vision2D.ImageProcessing.InsIlluminationCorrectorTool;
using Insnex.AI;
using System.Security.Cryptography;
using Core.DataProcess;

namespace Core.Function
{
    public class CParticleInspection : CWork, IInspection
    {
        enum LINE_LOCATION { TOP = 0, BTM = 1, LEFT = 2, RIGHT = 3 }

        private CLogger Logger = null;


        CogToolBlock m_InspectToolBlock = new CogToolBlock();

        List<CogBlobResult> m_BlobResults = new List<CogBlobResult>();
        List<CogPolygon> m_SearchArea = new List<CogPolygon>();

        private InsAIClassifyTool m_InsClassify;

        private object m_lock = new object();


        public CParticleInspection(CLogger _logger)
        {
            Logger = _logger;
        }

        public void FindDefect(int _nImageCnt, int _nInspecIdx, CImage _Image)
        {
            try
            {
                bool IsError = true;

                // 레시피 2 : Cell Count : 5, Cell Height : 10400.0, Cell VerOffset : 1850.0, HoleX : 280.0, HoleY : 480.0, Hole Radius : 150.0
                // 레시피 3 : Cell Count : 5, Cell Height : 10560.0, Cell VerOffset : 1660.0, HoleX : 2430.0, HoleY : 370.0, Hole Radius : 150.0
                // 20250820 shj 임시 파라미터 입력 
                m_InspectToolBlock.Inputs[0].Value = _Image.CogImage[_nImageCnt];
                m_InspectToolBlock.Inputs[1].Value = theRecipe.MaterialInfo.m_nCellVertCount; //5;      // 20250908 SHJ Cell 세로 갯수
                m_InspectToolBlock.Inputs[2].Value = theRecipe.MaterialInfo.m_dCellHeight; //10560.0;   // 20250908 SHJ Cell 엑티브 Height                                  
                m_InspectToolBlock.Inputs[3].Value = theRecipe.MaterialInfo.m_dCellPadHeight; //1660.0; // 20250908 SHJ Cell VericalOffset
                m_InspectToolBlock.Inputs[4].Value = theRecipe.MaterialInfo.m_dHolePosX; //2430.0;      // 20250908 SHJ Active Hole PosX
                m_InspectToolBlock.Inputs[5].Value = theRecipe.MaterialInfo.m_dHolePosY; //370.0;       // 20250908 SHJ Active Hole PosY
                m_InspectToolBlock.Inputs[6].Value = theRecipe.MaterialInfo.m_dHoleRadius; //150.0;     // 20250908 SHJ Active Hole Radius

                // 20250908 SHJ Cell 세로 전체 검사 모드 유무 (FASLE -> 선택한 인덱스 Cell 만 검사)
                m_InspectToolBlock.Inputs[7].Value = InspMode == DEF_SYSTEM.INSP_NORMAL ? false : theMainSystem.Cameras[CamID].m_bTeachExcuteMode;
                // 20250908 SHJ Cell 선택 검사일 경우 검사 할 Cell Index 
                m_InspectToolBlock.Inputs[8].Value = InspMode == DEF_SYSTEM.INSP_NORMAL ? 0 : theMainSystem.Cameras[CamID].m_nTeachCellIndex;
                // 20250908 SHJ Teaching Mode 유무 TeachingMode -> Tool 에 그래픽 추가 및 검사 영역 출력, false-> 결과만 생성
                m_InspectToolBlock.Inputs[9].Value = InspMode == DEF_SYSTEM.INSP_NORMAL ? false : true;

                m_InspectToolBlock.Run();

                IsError = (bool)m_InspectToolBlock.Outputs[0].Value;
                m_BlobResults = m_InspectToolBlock.Outputs[1].Value as List<CogBlobResult>;

                // 20250909 SHJ Teaching Mode 일 경우 화면에 그래픽 뿌려주기 위해 검사 영역 출력
                if (InspMode == DEF_SYSTEM.INSP_TEACH)
                    m_SearchArea = m_InspectToolBlock.Outputs[2].Value as List<CogPolygon>;



                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (m_BlobResults == null || IsError == true) //ToolBlock Error &매칭 Error
                {
                    DefectCandidates.Add(CreateDefect(0, "Error"));

                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Image Count : {ImageCount}, _InspecIdx : {InspectIdx} ToolBlock Error ");
                }
                else
                {
                    // 250909 SHJ Defect 내용 화면 표시 하기 위해 그래픽 Offset 처리 
                    int ImageOffsetX = ((_Image.m_nStandardWidth - _Image.m_nSliceWidthOffset) * _nImageCnt) + _Image.m_nSliceStartPosX;
                    int Scale = _Image.m_nReduceSize;

                    // Defect 위치 저장 
                    for (int i = 0; i < m_BlobResults.Count; i++)
                    {

                        CDefect defect = CreateDefect(i, "RuleBase");

                        // 250909 SHJ Inspection 검사 모드일 경우에만 Image Slice Offset 데이터 사용 
                        defect.InspResult.DefectPos = new PointF((float)Math.Round(m_BlobResults[i].CenterOfMassX + (InspMode == DEF_SYSTEM.INSP_NORMAL ? ImageOffsetX : 0), 3), (float)Math.Round(m_BlobResults[i].CenterOfMassY, 3)); // 240430 SHJ 현재 이미지 카운트에 맞게 Offset 처리 하여 위치 저장 
                        defect.InspResult.Width = Math.Round(m_BlobResults[i].GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeWidth), 3);
                        defect.InspResult.Height = Math.Round(m_BlobResults[i].GetMeasure(CogBlobMeasureConstants.BoundingBoxPixelAlignedNoExcludeHeight), 3);
                        defect.InspResult.Size = Math.Round(Math.Sqrt((Math.Pow(defect.InspResult.Width, 2) + Math.Pow(defect.InspResult.Height, 2))), 3);

                        defect.InspResult.m_sDefectType = "RuleBase";
                        defect.InspResult.m_sClassColor = Color.Red;

                        // Defect 그래픽 위치 저장 
                        defect.InspResult.m_InnerValue = new PointF[4];

                        int InnderWidth = (int)defect.InspResult.Width + 30;
                        int InnderHeight = (int)defect.InspResult.Height + 30;

                        // 사각형 영역으로 설정 
                        defect.InspResult.m_InnerValue[0] = new PointF((float)(defect.InspResult.DefectPos.X - InnderWidth) / Scale, (float)(defect.InspResult.DefectPos.Y - InnderHeight) / Scale);
                        defect.InspResult.m_InnerValue[1] = new PointF((float)(defect.InspResult.DefectPos.X + InnderWidth) / Scale, (float)(defect.InspResult.DefectPos.Y - InnderHeight) / Scale);
                        defect.InspResult.m_InnerValue[2] = new PointF((float)(defect.InspResult.DefectPos.X + InnderWidth) / Scale, (float)(defect.InspResult.DefectPos.Y + InnderHeight) / Scale);
                        defect.InspResult.m_InnerValue[3] = new PointF((float)(defect.InspResult.DefectPos.X - InnderWidth) / Scale, (float)(defect.InspResult.DefectPos.Y + InnderHeight) / Scale);

                        // Defect Image Copy
                        int CropX = (int)Math.Round(m_BlobResults[i].CenterOfMassX, 3) - (DEF_SYSTEM.DEFECT_CROP_WIDTH / 2);
                        int CropY = (int)Math.Round(m_BlobResults[i].CenterOfMassY, 3) - (DEF_SYSTEM.DEFECT_CROP_HEIGHT / 2);
                        int CropWidth = DEF_SYSTEM.DEFECT_CROP_WIDTH;
                        int CropHeight = DEF_SYSTEM.DEFECT_CROP_HEIGHT;

                        defect.DefectImage.Crop(_Image, _Image.m_nPixelFormat, CropWidth, CropHeight, CropX, CropY, _nImageCnt);

                        DefectCandidates.Add(defect);
                    }
                }

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {sProductName}, Image Count : {_nImageCnt}, _InspecIdx : {_nInspecIdx} , Defect Count : {DefectCandidates.Count}");
            }
            catch (Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Image Count : {ImageCount}, _InspecIdx : {InspectIdx} , Catch Exception {e.ToString()}");

            }
        }

        public void FindRoi(int _nImageCnt, int _nInspecIdx, CImage _Image)
        {
            // 240430 SHJ  현재는 FindDefect 에서 한번에 검사 


        }

        public override void Initialize(int _nWidth, int _nHeight)
        {
            if (!bInitialized)
            {
                Width = _nWidth;
                Height = _nHeight;

                m_InspectToolBlock = CogSerializer.DeepCopyObject(theRecipe.m_CogInspToolBlock[this.CamID], CogSerializationOptionsConstants.Minimum) as CogToolBlock;
                //m_PreProcToolBlock = CogSerializer.DeepCopyObject(theRecipe.m_CogPreToolBlock[this.CamID], CogSerializationOptionsConstants.Minimum) as CogToolBlock;

                //if (DEF_SYSTEM.LICENSES_DEEP_KEY != (int)DeepLicense.NONE)
                //    DeepLearningInitial();

                bInitialized = true;
            }
        }

        private void DeepLearningInitial()
        {
            if (!bInitialized)
            {
                //m_InsClassify = theRecipe.m_InsClassifyTool[this.CamID] as InsAIClassifyTool;

                // 240430 SHJ 첫 인덱스에만 컬러 정보 전달 
                // Insnex 딥러닝은 결과 컬러를 받아도 색상이 표시가 안됨 이건 피드백 받아야 할 내용 
                if (this.InspectIdx == 0 && this.ImageCount == 0)
                {
                    // 250530 SHJ 각 카메라 0번째 인덱스 일 떄만 Tool 할당 해서 Recipe 보관 
                    InsAIClassifyTool ClassifyTool = new InsAIClassifyTool($"{WorkSpaceName}.rt", $"{WorkSpaceName}.yaml");
                    theRecipe.m_InsClassifyTool.Add(ClassifyTool);

                    //for (int i = 0; i < m_InsClassify.InsAILabels.Count; i++)
                    for(int i = 0; i < ClassifyTool.InsAILabels.Count; i ++)
                    {
                        CClassifyClass cClassify = new CClassifyClass();

                        cClassify.ClassifyName = ClassifyTool.InsAILabels[i].name;
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

                //m_InsClassify = theRecipe.m_InsClassifyTool[this.CamID];//new InsAIClassifyTool($"{WorkSpaceName}.rt", $"{WorkSpaceName}.yaml");
            }
        }


        public override void WorkComplete(CDefectManager _DefectManager, CDefectManager _DTDefectManager, int _ImageCount, int _InspctIdx)
        {
            int nCnt = DefectCandidates.Count;

            for (int i = 0; i < nCnt; ++i)
            {

                CDefect defect = DefectCandidates[i];
                _DefectManager.Add(defect);
            }

            JudgeComplete?.Invoke(_InspctIdx, _ImageCount);
            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {sProductName}, Image Count : {_ImageCount}, _InspecIdx : {_InspctIdx} , Inspect Logic Complete ");
        }

        public override void OptionSet(int[] _ScoreThre, int[] _AreaThre)
        {
        }


        public override void WorkRun(int _ImageCount, int _InspecIdx, List<CImage> _Images, int _nInspectionType)
        {
            /*
             25.04.08 LYK 
            _InspecIdx는 image count이고, image count가 머지 카운트운트와 같을때, Image 저장을 진행 해야 함
            Image는 각각 저장 하고, 디스플레이만 머지 이미지로 진행한다. 

            Labeling도 마찬가지로 image count가 머지 카운트운트와 같을때, 검사 종료 이벤트를 Set 한다. 
            그 다음 이미지 디스플레이가 진행 된다.

            ListImage의 Index 접근은 ImageCount로 진행 한다.
             */
            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {sProductName}, Image Count : {_ImageCount}, Inspection Instance Count : {_InspecIdx} , Inspect Start ");

            InspectionTime.Reset();
            InspectionTime.Start();

            for(int i = 0; i < DefectCandidates.Count; i++)
            {
                if (DefectCandidates[i] != null)
                    DefectCandidates[i].Dispose();
            }

            DefectCandidates.Clear();

            this.InspectIdx = _InspecIdx;
            this.ImageCount = _ImageCount;
            this.InspMode = _nInspectionType;

            // 20250910 SHJ Display, Electronic 구분해서 검출 
            if (theRecipe.m_sCurrentEquipment.Contains(DEF_SYSTEM.DISPLAY)) // Display : Cell 내부 Defect 룰베이스로 검출, 검출 결과를 Classify 구분 
                FindDefect(_ImageCount, _InspecIdx, _Images[0]); // Find Defect Run
            else if (theRecipe.m_sCurrentEquipment.Contains(DEF_SYSTEM.ELECTRONIC))// Electronic : Segmentation 결과를 룰베이스로 사이즈 측정
                FindRoi(_ImageCount, _InspecIdx, _Images[0]); // 메소드는 새롭게 생성하여도 무관

            // 20250909 SHJ DefectManager InpMode 에 따라 Insp or Teach 구분 하여 메모리 해제 및 Complete 할당 
            var DefectManager = _nInspectionType == DEF_SYSTEM.INSP_NORMAL ? theMainSystem.ProductInfo.DefectManager[_InspecIdx][DEF_SYSTEM.CAM_ONE][_ImageCount] : theMainSystem.ProductInfo.TeachDefectManager[DEF_SYSTEM.CAM_ONE][_ImageCount];

            int nCount = DefectManager.Count;

            if (nCount > 0)
            {
                for (int i = 0; i < nCount; i++)
                {
                    if (DefectManager[i] != null)
                        DefectManager[i].Dispose();
                }
            }

            DefectManager.Clear();

            WorkComplete(DefectManager, null, _ImageCount, _InspecIdx);  //220503 LYK 옵저버 컬렉션을 이용한 Defect Data 취합

            InspectionTime.Stop();

            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {sProductName}, Image Count : {_ImageCount}, Inspection Instance Count : {_InspecIdx} , Inspect End , RunTime :{InspectionTime.ElapsedMilliseconds}");
            //Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {sProductName}, Image Count : {_ImageCount}, _InspecIdx : {_InspecIdx} , Inspect Next Step ");
        }

        public override void Uninitialize()
        {

        }

        /// <summary>
        /// 20250909 SHJ 반복적으로 디펙트 생성해야 하는 부분을 메소드 처리 
        /// </summary>
        /// <param name="sType"></param>
        private CDefect CreateDefect(int Idx, string sType)
        {
            CDefect defect = new CDefect();

            defect.m_nIdx = Idx;
            defect.m_nGrabIdx = this.InspectIdx;
            defect.m_nCamNum = this.CamID;
            defect.StartTime = this.StartTime;
            defect.GrabTime = (int)this.CameraGrabTime;
            defect.m_sProductName = this.sProductName;
            defect.OriginImagePath = this.sOriginalImagePath;
            defect.DefectImagePath = this.sDefectImagePath;
            defect.GrabCycleTime = (int)this.GrabCycleTime;
            defect.m_bRuleBaseJudge = sType == "OK" ? true : false;
            defect.m_bDeepLearningJudge = true; // OK 설정 

            defect.InspResult.m_sDefectType = sType == "OK" ? "OK" : sType;

            return defect;
        }
        private void ImageCrop(CImage image)
        {
            using (Bitmap bmp = new Bitmap(image.m_nWidth, image.m_nHeight, image.m_nStride, PixelFormat.Format8bppIndexed, image.pData))
            {
                for (int i = 0; i < DefectCandidates.Count; i++)
                {
                    int X = (int)DefectCandidates[i].InspResult.DefectPos.X - (DEF_SYSTEM.DEFECT_CROP_WIDTH / 2);
                    int Y = (int)DefectCandidates[i].InspResult.DefectPos.Y - (DEF_SYSTEM.DEFECT_CROP_HEIGHT / 2);
                    int Width = DEF_SYSTEM.DEFECT_CROP_WIDTH;
                    int Height = DEF_SYSTEM.DEFECT_CROP_HEIGHT;

                    // 이미지 영역 넘어가지 않도록 크롭 부분 예외 처리 
                    if (X < 0)
                        X = 0;
                    else if (X + Width >= image.m_nWidth)
                        X = image.m_nWidth - (Width) - 1;

                    if (Y < 0)
                        Y = 0;
                    else if (Y + Height >= image.m_nHeight)
                        Y = image.m_nHeight - (Height) - 1;

                    BitmapData bmpData = bmp.LockBits(new Rectangle(X, Y, Width, Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

                    DefectCandidates[i].DefectImage.m_nWidth = bmpData.Width;
                    DefectCandidates[i].DefectImage.m_nHeight = bmpData.Height;
                    DefectCandidates[i].DefectImage.m_nStride = bmpData.Stride;
                    DefectCandidates[i].DefectImage.m_nImageSize = bmpData.Stride * bmpData.Height * CImage.PIXEL8;
                    DefectCandidates[i].DefectImage.pData = Marshal.AllocHGlobal(DefectCandidates[i].DefectImage.m_nImageSize);

                    CFunc.memcpy(DefectCandidates[i].DefectImage.pData, bmpData.Scan0, DefectCandidates[i].DefectImage.m_nImageSize);

                    bmp.UnlockBits(bmpData);
                }
            }
        }

        private void ImageCrop(CDefect defect, CImage InputImg, int X, int Y, int Width, int Height)
        {

            // 이미지 영역 넘어가지 않도록 크롭 부분 예외 처리 
            //if (X < 0)
            //    X = 0;
            //else if (X + Width >= InputImg.CogImage.Width)
            //    X = InputImg.CogImage.Width - (Width) - 1;
            //
            //if (Y < 0)
            //    Y = 0;
            //else if (Y + Height >= InputImg.CogImage.Height)
            //    Y = InputImg.CogImage.Height - (Height) - 1;
            //
            //lock (m_lock)
            //{
            //
            //    try
            //    {
            //        //var PexelMem = InputImg.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, X, Y, Width, Height);
            //
            //        //defect.DefectImage.Allocate(Width, Height, CImage.PIXEL8);
            //
            //        //// 240430 SHJ 코그넥스 방식 크롭 -> 딥러닝에서 코그넥스 이미지를 Input 받을 수 있어서 처리 (RunTimeTool2 사용 하면 비트맵도 동일하게 Input 처리 가능)
            //
            //        //// cimage 에 Stride 넣고 Allocate 할당을 하면 이미지 사이즈가 맞지 않아 임시 수동으로 할당 
            //        //defect.DefectImage.m_nWidth = PexelMem.Width;
            //        //defect.DefectImage.m_nHeight = PexelMem.Height;
            //        //defect.DefectImage.m_nPixelFormat = CImage.PIXEL8;
            //        //defect.DefectImage.m_nStride = PexelMem.Stride;
            //        //defect.DefectImage.m_nImageSize = PexelMem.Stride * PexelMem.Height * CImage.PIXEL8;
            //        //defect.DefectImage.pData = Marshal.AllocHGlobal(defect.DefectImage.m_nImageSize);
            //        ////defect.DefectImage.m_pImage = new byte[defect.DefectImage.m_nImageSize];
            //
            //        //CFunc.memcpy(defect.DefectImage.pData, PexelMem.Scan0, defect.DefectImage.m_nImageSize);
            //
            //
            //        // 240430 SHJ 기존 Bitmap 으로 크롭 해서 저장 
            //
            //        using (Bitmap bmp = new Bitmap(InputImg.m_nWidth, InputImg.m_nHeight, InputImg.m_nStride, PixelFormat.Format8bppIndexed, InputImg.pData))
            //        {
            //            BitmapData bmpData = bmp.LockBits(new Rectangle(X, Y, Width, Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            //
            //            defect.DefectImage.Allocate(bmpData.Width, bmpData.Height, CImage.PIXEL8);
            //
            //            // 임시 수동으로 할당 
            //            defect.DefectImage.m_nWidth = bmpData.Width;
            //            defect.DefectImage.m_nHeight = bmpData.Height;
            //            defect.DefectImage.m_nPixelFormat = CImage.PIXEL8;
            //            defect.DefectImage.m_nStride = bmpData.Stride;
            //            defect.DefectImage.m_nImageSize = bmpData.Stride * bmpData.Height * CImage.PIXEL8;
            //
            //            defect.DefectImage.pData = Marshal.AllocHGlobal(defect.DefectImage.m_nImageSize);
            //
            //            CFunc.memcpy(defect.DefectImage.pData, bmpData.Scan0, defect.DefectImage.m_nImageSize);
            //
            //            bmp.UnlockBits(bmpData);
            //
            //            bmp.Save("D:\\asdfasdfasdfasdf.bmp");
            //        }
            //
            //    }
            //    catch
            //    {
            //
            //    }
            //
            //}
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
            return m_InspectToolBlock;
        }

        public override void InspectionOptionSet(int[] _ScoreThre, int[] _AreaThre)
        {
            throw new NotImplementedException();
        }
    }

}
