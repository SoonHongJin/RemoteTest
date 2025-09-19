using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

using static Core.Program;
using Core.HardWare;
using Core.Function;
using Core.Utility;
using Core.UI;

using Cognex.VisionPro;
using Cognex.VisionPro.CalibFix;
using static System.Windows.Forms.AxHost;
using System.Drawing.Imaging;
using Core.Function.Preprocessing;
using Cognex.VisionPro.ImageFile;
using ScottPlot.Drawing.Colormaps;
using Cognex.VisionPro.Blob;
using System.Windows.Forms;
using Insnex.Vision2D.Common;
using System.Net.NetworkInformation;
using static Insnex.Vision2D.ImageProcessing.InsIlluminationCorrectorTool;
using Cognex.VisionPro.ToolBlock;

namespace Core.Process
{
    public class CCameraManager
    {
        //----------------------------------------------------------------------------------------------
        // Camera Process 관련 변수 
        private readonly object m_Lock = new object();
        private int m_nInPectCnt = 0;
        private int m_nCurIndex = 0;
        public int m_nImageCnt = 0;
        private int m_nGrabCycleTime = 0;

        private bool m_bIsConnected = false;

        public List<CImage>[] ListImages;
        public CImage DefectMapImage;               
        private List<CImage>[] DefectImages;

        private CImage ListLiveImages;

        private List<List<List<ManualResetEvent>>> InspectionCompleteEvent = new List<List<List<ManualResetEvent>>>();
        // 250910 SHJ Display 전용 룰베이스 검사 완료 이벤트 생성 -> Display 같은 경우 룰베이스 전체 검사 완료 후 딥러닝 실행 하기 때문에 완료 시점 파악 하기 위해 추가 
        private List<List<ManualResetEvent>> RuleBaseCompleteEvent = new List<List<ManualResetEvent>>();

        public List<CWork>[] ListInspect = new List<CWork>[DEF_SYSTEM.INSP_CNT];
        //public CWork[] Inspect = new CWork[DEF_SYSTEM.INSP_CNT];
        public CWork InspDeepLearning;

        public List<CImage> CheckerBoardImages = new List<CImage>();
        private List<CImage> ListHisotryImages = new List<CImage>();

        public int m_InspIndex = 0;      // Inspection 병렬 실행을 위해 지정하는 인덱스 
        public int m_nCamNumber = 0;     // 240115 LYK 카메라 번호     
        public bool m_bIsInspection = false;
        public int m_nInspMode = 0;

        public DateTime StartTime;      //240202 LYK StartTime
        public int DisplayIndex = 0;

        public int m_nTeachCellIndex = 0; // 250910 SHJ Teaching 에서 Cell 개별 검사 위해 인덱스 추가 
        public bool m_bTeachExcuteMode = false; // 250910 SHJ Teaching 에서 Cell 개별 검사 위해 추가
        //----------------------------------------------------------------------------------------------
        // Class 인스턴스 
        private MainForm mainForm = null;


        public CCamera m_Camera = null;
        private CImageSaverManager ImageSaverManager = null;
        private InspectionInfo InspInfos = new InspectionInfo();    //240710 NWT MainSystem의 Image Path, Judge를 활용하기 위한 변수
        //----------------------------------------------------------------------------------------------

        // Thread 
        private CThread Thread_ImageDisplay = null;                                     //240122 LYK Image Display Thread 객체
        private CThread Thread_DefectDisplay = null;                                     //240122 LYK Image Display Thread 객체
        //private CThread Thread_Inspection = null;                                        //240617 LYK Dimension 검사 Thread 객체
        //private CThread Thread_DeepLearning = null;                                        //240617 LYK Dimension 검사 Thread 객체
        private CThread Thread_DefectImageSave = null;                                        //240617 LYK Dimension 검사 Thread 객체
        //----------------------------------------------------------------------------------------------
        // Action
        //public Action<CImage, int> RefreshLive = null;
        //public Action<ICogImage, string, string> RefreshGrabImage = null;  //220509 LYK   
        private Action<CImage> RefreshGrabImage = null;                      //240118 LYK RefreshGrabImageWhite 델리게이트 변수(검사)
        private Action<CImage> RefreshDefectMapImage = null;                      //240118 LYK RefreshGrabImageWhite 델리게이트 변수(검사)
        private Action<CImage> RefreshLiveImage = null;                      //240118 LYK Live RefreshLiveImageWhite 델리게이트 변수(Live)
        private Action<CImage>[] RefreshDefectImage = new Action<CImage>[10];

        private Action<CImage> RefreshTeachImage = null;

        private Action<CImage> RefreshCalibration = null;                    //240304 LYK RefreshGrabTeachMono 델리게이트 변수
        private Action<CImage> RefreshHistoryImage = null;                   //240123 LYK Live RefreshHistoryImage 델리게이트 변수(History Page)
        private Action<CImage> RefreshSimulHistoryImage = null;
        private Action<CImage> RefreshCurrentHistoryImage = null;            //240123 LYK Live RefreshHistoryImage 델리게이트 변수(History Page)
        private Action<int, long> InspectionTimeInfo = null;                 //240128 LYK InspectionTimeInfo 델리게이트 변수(검사 종류, 인스펙션 시간)

        private Stopwatch m_GrabTimeStopWatch = new Stopwatch();             //240116 LYK Grab Time StopWatch (그랩 시간을 측정 하기 위한 변수)
        public Stopwatch m_GrabCycleTime = new Stopwatch();
        private Pen pen = new Pen(Color.Beige, 1.0F);

        Font Font = new Font("Calibri", 10, FontStyle.Bold);

        private CLogger Logger = null;

        public CCameraManager(MainForm _MainForm, CLogger _logger)
        {
            mainForm = _MainForm;

            // Image Display Thread 
            Thread_ImageDisplay = new CThread()     //240123 LYK Image Display Thread
            {
                Work = ShowImageDisplay,
                nSleepTime = 1
            };
            Thread_ImageDisplay.ThreadStart();

            Thread_DefectDisplay = new CThread()
            {
                Work = ShowDefectDisplay,
                nSleepTime = 1
            };
            Thread_DefectDisplay.ThreadStart();

            //Thread_Inspection = new CThread()     //Inspect Thread 
            //{
            //    Work = InspectionRun,
            //    nSleepTime = 1
            //};
            //Thread_Inspection.ThreadStart();

            //Thread_DeepLearning = new CThread()     //DeepLearning Thread
            //{
            //    Work = DeepLearningRun,
            //    nSleepTime = 1
            //};
            //Thread_DeepLearning.ThreadStart();

            Thread_DefectImageSave = new CThread()
            {
                Work = DefectSaveImage,
                nSleepTime = 1
            };
            Thread_DefectImageSave.ThreadStart();

            Logger = _logger;

            ListImages = new List<CImage>[DEF_SYSTEM.INSP_CNT];
            DefectMapImage = new CImage(DEF_SYSTEM.LICENSES_KEY, theRecipe.m_sCurrentEquipment);             // 240430 SHJ UI 디펙트 위치 및 정보 출력용 이미지  
            DefectImages = new List<CImage>[DEF_SYSTEM.INSP_CNT];    // 240430 SHJ 디펙트 Crop 된 이미지 상위 10개 표시용도 이미지 

            ListLiveImages = new CImage(DEF_SYSTEM.LICENSES_KEY, theRecipe.m_sCurrentEquipment);
        }


        public void Initialize(bool bSimulationMode)
        {
            int nCameraPick = 0; 
            CImage cImage = null;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// 카메라 + 이미지 버퍼 초기화  
            /// 
            bSimulationMode = false;

            if (bSimulationMode) // 카메라 사용 모드 
            {
                nCameraPick = DEF_SYSTEM.INS_CIS;
                m_Camera = CCamera.CameraPick(nCameraPick, m_nCamNumber, DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\");

                m_Camera.m_sCurrentEquipment = theRecipe.m_sCurrentEquipment;

                m_Camera.CamActionInfo = CameraStatus;
                m_Camera.Initialize(m_Camera);   //240901 LYK 추후 2, 3번째 인자 활용 할 수 있도록 프로그램 수정 해야 함
                m_Camera.Connect(theRecipe.CameraRecipe);
                m_Camera.m_nImgCnt = theRecipe.MergeImageCount; //250409 LYK 이미지 수량이 변경 됐을때, 재할당 해줘야 함

                m_bIsConnected = m_Camera.IsConnected();
            }
            else // 시뮬레이션 모드 
            {
                m_bIsConnected = true;

                nCameraPick = DEF_SYSTEM.SIMULATION_CAM;
                m_Camera = CCamera.CameraPick(nCameraPick, m_nCamNumber, DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\");

                m_Camera.m_sCurrentEquipment = theRecipe.m_sCurrentEquipment;

                m_Camera.m_nImgWidth = DEF_SYSTEM.IMAGE_WIDTH;
                m_Camera.m_nImgHeight = DEF_SYSTEM.IMAGE_HEIGHT;
            }

            string sWorkspaceName = "";

            // 20250911 SHJ 세이지 딥러닝 Display, Electronic 에 맞게 Cls, Seg 로드 할 수 있도록 수정 
            if (DEF_SYSTEM.LICENSES_DEEP_KEY == (int)DeepLicense.SAIGE)
            {
                if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY)
                    sWorkspaceName = $"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{theRecipe.m_sCurrentModelName}\\{theRecipe.m_sCurrentModelName}_Classify.srCls";
                else if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.ELECTRONIC)
                    sWorkspaceName = $"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{theRecipe.m_sCurrentModelName}\\Saige_Segment.srSeg";

                // 20250910 SHJ 딥러닝 초기화 
                InspDeepLearning = CWork.WorkPick(DEF_SYSTEM.INSP_SAGEDEEPLEARNING, m_nCamNumber, sWorkspaceName);
                InspDeepLearning.Initialize((int)m_Camera.m_nImgWidth, (int)m_Camera.m_nImgHeight);
                InspDeepLearning.CamID = this.m_nCamNumber;
                InspDeepLearning.JudgeComplete = DeepLearningComplete;
                InspDeepLearning.InspectionTimeInfo = InspectionTimeCheck;
            }


            for (int j = 0; j < DEF_SYSTEM.INSP_CNT; j++)
            {
                ListImages[j] = new List<CImage>();
                DefectImages[j] = new List<CImage>();

                ListInspect[j] = new List<CWork>();

                for (int i = 0; i < theRecipe.MergeImageCount; i++)
                {
                    

                    //if (i == 0)  //250408 LYK Image Index가 0일때만 Merge Image를 Alloc 한다. Image는 0번 Index에서 Merge한다.
                    //    cImage.Allocate((int)m_Camera.m_nImgWidth, (int)m_Camera.m_nImgHeight, CImage.PIXEL8, theRecipe.MergeImageCount, true); //220503 LYK cImage 객체 Initial (이미지와 관련된가로, 세로 사이즈, 버퍼 등 할당)
                    //else
                    if(theRecipe.m_sCurrentEquipment == DEF_SYSTEM.ELECTRONIC)
                    {
                        cImage = new CImage(DEF_SYSTEM.LICENSES_KEY, theRecipe.m_sCurrentEquipment, theRecipe.SliceImageCount, theRecipe.MergeImageCount);    //220503 LYK cImage 객체 할당

                        if (i == 0)
                            cImage.Allocate((int)m_Camera.m_nImgWidth, (int)m_Camera.m_nImgHeight, CImage.PIXEL8, 15/*theRecipe.MergeImageCount*/, true, 4, 0, 0);
                        else
                            cImage.Allocate((int)m_Camera.m_nImgWidth, (int)m_Camera.m_nImgHeight, CImage.PIXEL8);
                    }
                    else
                    {
                        //public void Allocate(int nWidth, int nHeight, int pixelFormat, int nMergeCount, bool bMerge = false, int ReduceSize = 1, int _nStartPosX = 0, int _nStandardWidth = 0)
                        if (i == 0)
                        {
                            //레시피 2 : 13600, 11258
                            //레시피 3 : 13260, 5600
                            cImage = new CImage(DEF_SYSTEM.LICENSES_KEY, theRecipe.m_sCurrentEquipment, theRecipe.SliceImageCount, theRecipe.MergeImageCount);    //220503 LYK cImage 객체 할당
                            //cImage.Allocate((int)m_Camera.m_nImgWidth, (int)m_Camera.m_nImgHeight, CImage.PIXEL8, theRecipe.MergeImageCount, true, 5, 13260, 5600);
                            cImage.Allocate((int)m_Camera.m_nImgWidth, (int)m_Camera.m_nImgHeight, CImage.PIXEL8, theRecipe.MergeImageCount, true, 5, 13600, 5540, 600); // 시작점, 가로 길이 , Width 여유 픽셀 

                            // 20250915 SHJ 디스플레이 전용 이미지 Slice 중 배열 정보, 그룹 갯수, 더미 정보 입력 
                            cImage.SliceOptionSet(theRecipe.MaterialInfo.m_nCellRow, theRecipe.MaterialInfo.m_nCellColumn, theRecipe.MaterialInfo.m_nGroupCount, (int)theRecipe.MaterialInfo.m_dDumylength);
                        }
                            
                        else
                        {
                            cImage = new CImage(DEF_SYSTEM.LICENSES_KEY, theRecipe.m_sCurrentEquipment);    //220503 LYK cImage 객체 할당
                            cImage.Allocate((int)m_Camera.m_nImgWidth, (int)m_Camera.m_nImgHeight, CImage.PIXEL8);
                        }
                            
                    }
                        
                    ListImages[j].Add(cImage);
                }

                if (theRecipe.m_sCurrentEquipment.Contains("DISPLAY"))
                {
                    for (int i = 0; i < theRecipe.SliceImageCount; i++)
                    {
                        CWork Inspect = CWork.WorkPick(DEF_SYSTEM.PARTICLE_INSPECTION, m_nCamNumber, $"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{theRecipe.m_sCurrentModelName}\\{"INS_Classify"}");
                        Inspect.InspectIdx = j;
                        Inspect.ImageCount = i;
                        Inspect.Initialize((int)m_Camera.m_nImgWidth, (int)m_Camera.m_nImgHeight);
                        Inspect.JudgeComplete = InspectionComplete;
                        Inspect.InspectionTimeInfo = InspectionTimeCheck;

                        ListInspect[j].Add(Inspect);
                    }
                }

                // 240430 SHJ Main UI 출력용 Crop 이미지는 10개만 할당 
                for (int i = 0; i < DEF_SYSTEM.DEFECT_MAX_COUNT; i ++)
                {
                    cImage = new CImage(DEF_SYSTEM.LICENSES_KEY, theRecipe.m_sCurrentEquipment);    //220503 LYK cImage 객체 할당
                    cImage.Allocate((int)DEF_SYSTEM.DEFECT_CROP_WIDTH, (int)DEF_SYSTEM.DEFECT_CROP_HEIGHT, CImage.PIXEL8);
                    DefectImages[j].Add(cImage);
                }
            }

            // 250910 SHJ Display 전용 룰베이스 검사 완료 이벤트 생성 -> Display 같은 경우 룰베이스 전체 검사 완료 후 딥러닝 실행 하기 때문에 완료 시점 파악 하기 위해 추가 
            if (theRecipe.m_sCurrentEquipment.Contains(DEF_SYSTEM.DISPLAY))
            {
                for (int instanceIdx = 0; instanceIdx < DEF_SYSTEM.INSP_CNT; instanceIdx++)
                {
                    var inspList = new List<ManualResetEvent>();

                    for (int imgNum = 0; imgNum < theRecipe.SliceImageCount; imgNum++)
                    {
                        inspList.Add(new ManualResetEvent(false));
                    }

                    RuleBaseCompleteEvent.Add(inspList);
                }
            }

            //디스플레이 & 삼성전기 혼합 이미지 로드 
            for(int i = 0; i < DEF_SYSTEM.INSP_CNT; i ++)
            {
                for (int j = 0; j < theRecipe.MergeImageCount; j++)
                {
                    string ImagePath = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.ELECTRONIC ? $"D:\\CIS Image_Electronic\\{j}.bmp" : $"D:\\CIS Image_Display\\레시피6_이물\\{j}.bmp";
                    SimulationImgRead(i, j, ImagePath);
                }
            }

            //for (int i = 0; i < theRecipe.SliceImageCount; i++)
            //{
            //    if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.ELECTRONIC) // SHJ 전기 전용 Slice Image Save 
            //    {
            //        using (Bitmap bmp = new Bitmap(23428, 5000, 23428, PixelFormat.Format8bppIndexed, ListImages[0][0].pSliceData[i]))
            //        {
            //            ColorPalette palette = bmp.Palette;
            //            for (int j = 0; j < palette.Entries.Length; j++)
            //            {
            //                palette.Entries[j] = Color.FromArgb(j, j, j);
            //            }

            //            bmp.Palette = palette;

            //            bmp.Save($"D:\\Slice Image\\ELECTRONIC\\{i}_slice.bmp", ImageFormat.Bmp);
            //        }
            //    }
            //    else // SHJ 디스플레이 전용 Slice Image Save 
            //    {
            //        using (Bitmap bmp = new Bitmap(ListImages[m_InspIndex][0].m_nSliceWidth, ListImages[m_InspIndex][0].m_nSliceHeight, ListImages[m_InspIndex][0].m_nSliceStride, PixelFormat.Format8bppIndexed, ListImages[0][0].pSliceData[i]))
            //        {
            //            ColorPalette palette = bmp.Palette;
            //            for (int j = 0; j < palette.Entries.Length; j++)
            //            {
            //                palette.Entries[j] = Color.FromArgb(j, j, j);
            //            }

            //            bmp.Palette = palette;

            //            bmp.Save($"D:\\Slice Image\\DSIPLAY\\{i}_slice.bmp", ImageFormat.Bmp);
            //        }
            //    }
            //}

            // SHJ Merge Image Save 
            //using (Bitmap bmp = new Bitmap(ListImages[m_InspIndex][0].m_nReduceWidth, ListImages[m_InspIndex][0].m_nReduceHeight * theRecipe.MergeImageCount, 4 * ((ListImages[m_InspIndex][0].m_nReduceWidth * 1 + 3) / 4), PixelFormat.Format8bppIndexed, ListImages[0][0].pDataMerge))
            //{
            //    ColorPalette palette = bmp.Palette;
            //    for (int j = 0; j < palette.Entries.Length; j++)
            //    {
            //        palette.Entries[j] = Color.FromArgb(j, j, j);
            //    }

            //    bmp.Palette = palette;

            //    string ImagePath = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.ELECTRONIC ? $"D:\\ELECTRONIC_Merge.bmp" : $"D:\\DSIPLAY_Merge.bmp";

            //    bmp.Save(ImagePath, ImageFormat.Bmp);
            //}

            /* 250803 LYK 삭제 예정
            // 240430 SHJ 디펙트 Map 백그라운드 이미지 생성 
            DefectMapImage.Allocate(DEF_SYSTEM.TABLE_DISPLAY_WIDTH, DEF_SYSTEM.TABLE_DISPLAY_HEIGHT, CImage.PIXEL24);

            Bitmap tempmage = new Bitmap((int)DEF_SYSTEM.TABLE_DISPLAY_WIDTH, (int)DEF_SYSTEM.TABLE_DISPLAY_HEIGHT, PixelFormat.Format24bppRgb);

            for(int i = 0; i < DefectMapImage.m_nWidth; i ++)
            {
                for (int j = 0; j < DefectMapImage.m_nHeight; j++)
                    tempmage.SetPixel(i, j, Color.FromArgb(31, 31, 31));
            }
            
            BitmapData bmpData = tempmage.LockBits(new Rectangle(0, 0, tempmage.Width, tempmage.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            DefectMapImage.Copy(bmpData.Width, bmpData.Height, CImage.PIXEL24, bmpData.Scan0);
            tempmage.UnlockBits(bmpData);
            */

            ImageSaverManager = new CImageSaverManager();
        }

        public void UnInitialize()
        {

            //Thread_Inspection.ThreadStop();         //240122 LYK Color Thread UnInitial
            Thread_ImageDisplay.ThreadStop();       //240122 LYK Image Display Thread UnInitial
            Thread_DefectDisplay.ThreadStop();
            Thread_DefectImageSave.ThreadStop();
            ListLiveImages.Free();
            DefectMapImage.Free();
            ImageSaverManager.Shutdown();

            CImage image = null;

            for (int j = 0; j < DEF_SYSTEM.INSP_CNT; j++)
            {
                for (int i = 0; i < ListImages[j].Count(); i++) //220503 LYK List이미지(검사 이미지) 메모리해제
                {
                    image = ListImages[j][i];
                    image.Free();
                }
                ListImages[j].Clear();

                for(int i = 0; i < DefectImages[j].Count(); i ++)
                {
                    image = DefectImages[j][i];
                    image.Free();
                }
                DefectImages[j].Clear();
            }

            //250517 LYK Inspect Uninitialize
            CWork Inspect = null;
            // CWork Uninitialize
            for (int i = 0; i < DEF_SYSTEM.INSP_CNT; i++)
            {
                for(int j = 0; j < ListInspect[i].Count(); j++)
                {
                    Inspect = ListInspect[i][j];
                    Inspect.Uninitialize();
                }

                ListInspect[i].Clear();
            }

            if (m_Camera.IsConnected())
            {
                m_Camera.DisConnect();              //220503 LYK 카메라 DisConnect
            }

            m_Camera.Uninitialize();                //220503 LYK 카메라 UnInitial
        }

        public void CameraStatus(CALLTYPE type, object parameter)
        {
            switch(type)
            {
                case CALLTYPE.Connect:                      
                    bool bConnected = (bool)(parameter);    

                    //if(!bConnected)
                    //220503 LYK 카메라 연결 안됐으면 알람(함수 만들어야 함)

                    break;
                case CALLTYPE.GrabEnd:
                    {
                        CGrabInfo cGrabInfo = (CGrabInfo)(parameter);

                        STime StartTime;
                        this.StartTime = DateTime.Now;
                        StartTime.Year = DateTime.Now.ToString("yyyy");
                        StartTime.Month = DateTime.Now.ToString("MM");
                        StartTime.Day = DateTime.Now.ToString("dd");
                        StartTime.Hour = DateTime.Now.ToString("HH");
                        StartTime.Min = DateTime.Now.ToString("mm");
                        StartTime.Sec = DateTime.Now.ToString("ss");

                        /*250803 LYK 수정 예정
                            // 240430 SHJ
                            // 이미지 촬영 및 검사 인덱스 분할 -> 같은 인덱스 사용 할 경우 이미지 촬영 후 버퍼 인덱스가 변경 되면 검사 인덱스에도 반영이 되기 때문에 분할 하여 처리
                            // m_nCurIndex -> 이미지 촬영 인덱스 
                            // m_InspIndex -> 검사 용 인덱스 
                            // m_DeepIndex -> 딥러닝 인덱스 (인덱스 구분이 많아지고 있어서 딥러닝 인덱스는 변경 처리 예정)
                        */
                        if (DEF_SYSTEM.INSP_TEACH == m_nInspMode || DEF_SYSTEM.INSP_CALIBRATION == m_nInspMode)
                            m_nCurIndex = 0;

                        m_nImageCnt = cGrabInfo.m_nGrabIndex;
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Image Count : {m_nImageCnt}, InspecIdx : {m_nCurIndex}");

                        if (DEF_SYSTEM.INSP_CALIBRATION != m_nInspMode && DEF_SYSTEM.INSP_TEACH != m_nInspMode)
                        {
                            /*
                                25.05.17 LYK 
                                    ProductInfo의 Initial은 이미지 카운트가 0일때만 진행 한다.
                                    매번 폴더를 생성 하는 플로우를 진행 하는 것이 불필요하기 때문
                            */
                            if (m_nImageCnt == 0)
                                theMainSystem.ProductInfo.Initialize($"{theRecipe.m_nTestNumber}Test", StartTime); // 이미지 저장 폴더 생성

                            ListInspect[m_nCurIndex][m_nImageCnt].StartTime = this.StartTime;                                   //240202 LYK Start Time 대입
                            ListInspect[m_nCurIndex][m_nImageCnt].sProductName = theMainSystem.ProductInfo.m_sProductName;
                            ListInspect[m_nCurIndex][m_nImageCnt].sOriginalImagePath = theMainSystem.ProductInfo.OrginalImagePath.ToString();
                            ListInspect[m_nCurIndex][m_nImageCnt].sDefectImagePath = theMainSystem.ProductInfo.DefectImagePath.ToString();
                            ListInspect[m_nCurIndex][m_nImageCnt].GrabCycleTime = m_GrabCycleTime.ElapsedMilliseconds;

                            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"ID : {ListInspect[m_nCurIndex][m_nImageCnt].sProductName}, Image Count : {m_nImageCnt}, InspecIdx : {m_nCurIndex} Check");
                        }

                        if(theRecipe.m_sCurrentEquipment == DEF_SYSTEM.ELECTRONIC)
                            ListImages[m_nCurIndex][m_nImageCnt].Copy(cGrabInfo.ImgWidth, cGrabInfo.ImgHeight, cGrabInfo.PixelFormat, cGrabInfo.Image);
                        else
                            ListImages[m_nCurIndex][0].Copy(cGrabInfo.ImgWidth, cGrabInfo.ImgHeight, cGrabInfo.PixelFormat, cGrabInfo.Image, 13600, 11258, m_nImageCnt);

                        // 240313 LYK GrabTime 할당
                        ListInspect[m_nCurIndex][m_nImageCnt].CameraGrabTime = m_GrabTimeStopWatch.ElapsedMilliseconds; //250517 LYk 마지막 인덱스만 확인 하면 된다.
                        ListInspect[m_nCurIndex][m_nImageCnt].ImageCount = m_nImageCnt;


                        if (m_bIsInspection)
                            Task.Run(() => InspectionRun(m_nCurIndex, m_nImageCnt, DEF_SYSTEM.INSP_NORMAL));

                        //Thread_Inspection.Continue();

                        //RunInspect(m_nCurIndex); // 240430 SHJ 검사인덱스 전달, 메소드 처리 

                        ListImages[m_nCurIndex][0].Merge(cGrabInfo.ImgWidth, cGrabInfo.ImgHeight, cGrabInfo.PixelFormat, cGrabInfo.Image, m_nImageCnt);  // 240430 SHJ 합성 이미지 카피
                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Image Count : {m_nImageCnt}, Image Copy & Merge ");

                        if (m_bIsInspection) // Inspection 인덱스 
                        {
                            if (DEF_SYSTEM.INSP_NORMAL == m_nInspMode)
                            {
                                

                                if (m_nImageCnt == theRecipe.MergeImageCount - 1)
                                {
                                    //DisplayIndex = m_InspIndex;
                                    //Thread_ImageDisplay.Continue();

                                    // Grab 하는 Index 와 Work Index 구분 하는 이유는 검사 도중 인덱스가 바뀌는 문제가 있어서 분할 
                                    m_nCurIndex++;
                                    if (DEF_SYSTEM.INSP_CNT == m_nCurIndex)
                                        m_nCurIndex = 0;

                                    //m_GrabCycleTime.Restart();

                                    theRecipe.m_nTestNumber++;
                                }
                                    
                            }
                            else if (DEF_SYSTEM.INSP_TEACH == m_nInspMode || DEF_SYSTEM.INSP_CALIBRATION == m_nInspMode)
                                ShowImageDisplay();

                        }
                        else if (!m_bIsInspection) // Live 이미지 Display 
                        {
                            // 250530 SHJ Live 페이지 밝기 상태 그래프 표시 
                            theMainSystem.RefreshHistogramPlot?.Invoke(RunHistogram(ListImages[m_nCurIndex][m_nImageCnt]));
                            ShowImageDisplay();
                        }
                    }

                    break;
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Camera Grab 명령 함수 
        /// 240121 LYK DoGrabStart
        public void DoGrabStart()               
        {
            m_GrabTimeStopWatch.Reset();
            m_GrabTimeStopWatch.Start();

            if (m_Camera.IsConnected())
            {
                m_nInPectCnt = 0;
                m_nImageCnt = 0;

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Cam {m_nCamNumber} Grab Start.");

                m_Camera.GrabStart();
            }
        }

        public void DoGrabStop()
        {
            m_InspIndex = 0;
            m_nCurIndex = 0;
            m_nImageCnt = 0;
            m_Camera.GrabHalt();
        }

        private void ShowDefectDisplay()
        {
            try
            {
                var flattened = theMainSystem.ProductInfo.DefectManager[DisplayIndex][this.m_nCamNumber]
                                .SelectMany(defectList => defectList)
                                .Where(d => d.DefectImage != null && d.DefectImage.pData != IntPtr.Zero)
                                .Take(10)
                                .ToList();

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"flattened Count : {flattened.Count}");

                for (int i = 0; i < flattened.Count; i++)
                {
                    // 20250909 SHJ 디펙트 이미지를 그냥 카피해서 보관 할 경우 Teaching or Insp Run 을 해야 하는 페이지에서 작업 후 메인으로 돌아올 경우 에러 발생 될 수 있음 
                    // Insp Run 할때 Dispose 시켜주어서 기존 이미지 데이터가 삭제 됨
                    DefectImages[DisplayIndex][i].Free();
                    DefectImages[DisplayIndex][i].Copy(flattened[i].DefectImage);

                    //DefectImages[DisplayIndex][i] = flattened[i].DefectImage;//copied;

                    int displayIdx = i;
                    mainForm.DisplayScreen.DefectFrames[displayIdx].Invoke(new MethodInvoker(() =>
                    {
                        RefreshDefectImage[displayIdx]?.Invoke(DefectImages[DisplayIndex][displayIdx]);
                    }));

                    if (i >= DEF_SYSTEM.DEFECT_MAX_COUNT)
                        break;
                }

            }
            catch
            {
            }
        }

        private void ShowImageDisplay()
        {
            try
            {
                if (m_bIsInspection)
                {
                    if (m_nInspMode == DEF_SYSTEM.INSP_NORMAL)
                    {
                        // Main Display Image
                        mainForm.DisplayScreen.Frames[this.m_nCamNumber].Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                        {
                            RefreshGrabImage?.Invoke(ListImages[DisplayIndex][0]);   //250408 LYK 임시 주석
                        }));

                        // Main Display Defect Map Table
                        mainForm.DisplayScreen.DefectMap[this.m_nCamNumber].Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                        {
                            RefreshDefectMapImage?.Invoke(DefectMapImage);   //250408 LYK 임시 주석
                        }));

                    }
                    else if (m_nInspMode == DEF_SYSTEM.INSP_SIMUL)
                    {
                        mainForm.Simulation.Frames[this.m_nCamNumber].Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                        {
                            RefreshSimulHistoryImage?.Invoke(ListImages[DisplayIndex][0]);
                        }));
                    }

                    //else if (m_nInspMode == DEF_SYSTEM.INSP_TEACH)
                    //{
                    //    mainForm.TeachingInspection.Frames[this.m_nCamNumber].Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    //    {
                    //        RefreshTeachImage?.Invoke(ListImages[m_InspIndex][m_nImageCnt]); // White 
                    //    }));
                    //}
                    //else if(m_nInspMode == DEF_SYSTEM.INSP_CALIBRATION)
                    //{
                    //    mainForm.FormCalibration.Frames[this.m_nCamNumber].Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    //    {
                    //        RefreshCalibration?.Invoke(ListImages[m_InspIndex]); 
                    //    }));
                    //}
                }
                else
                {

                    mainForm.SettingScreen.Frames[this.m_nCamNumber].Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    {
                        RefreshLiveImage?.Invoke(ListImages[m_nCurIndex][m_nImageCnt]); //250408 LYK 임시 주석
                    }));
                }
            }
            catch(Exception e)
            {

            }
                                   
        }

        public void ShowTeachingDisplay()
        {
            Bitmap bmpT = new Bitmap(ListImages[m_InspIndex][0].m_nReduceWidth, ListImages[m_InspIndex][0].m_nReduceHeight * 15, ListImages[m_InspIndex][0].m_nReduceWidth, PixelFormat.Format8bppIndexed, ListImages[0][0].pDataMerge);
            CImage mergeImage = new CImage(DEF_SYSTEM.LICENSES_KEY, theRecipe.m_sCurrentEquipment, 4, 15);
            mergeImage.Copy(ListImages[m_InspIndex][0].m_nReduceWidth, ListImages[m_InspIndex][0].m_nReduceHeight * 15, 1, ListImages[0][0].pDataMerge);
            
            RefreshTeachImage?.Invoke(mergeImage);
        }


        public void DoDefectCurrentPaint(Graphics graphics, int _nCamNum)
        {
            try
            {
                // 240430 SHJ 합성된 이미지 에 이물 디펙트 표시
                theMainSystem.Cameras[_nCamNum].ListImages[DisplayIndex][0].MergeImageDraw(graphics);   //250408 LYK 임시 주석
                
                for (int i = 0; i < theMainSystem.InspectionInfos[DisplayIndex].InspResults.Count; i++) // Defect Count
                {

                    var Result = theMainSystem.InspectionInfos[DisplayIndex].InspResults[i];

                    if (Result == null)
                        continue;

                    pen.Color = Result.m_sClassColor;
                    pen.Width = 1;

                    if (Result.m_InnerValue != null)
                        graphics.DrawPolygon(pen, Result.m_InnerValue);

                }
            }
            catch (Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Defect Display Error Catch : {e.Message}");
            }
        }

        public void DoDefectMapCurrentPaint(Graphics graphics, int _nCamNum)
        {
            theMainSystem.Cameras[_nCamNum].DefectMapImage.Draw(graphics);   //250408 LYK 임시 주석

            // 240430 SHJ Map 이미지상 넘어가지 않도록 영역 처리 
            Rectangle DefectMapRect = new Rectangle();
            DefectMapRect.Location = new Point(5, 5);
            DefectMapRect.Size = new Size(DEF_SYSTEM.TABLE_DISPLAY_WIDTH - 25, DEF_SYSTEM.TABLE_DISPLAY_HEIGHT - 5);

            double ScaleX = (double)DefectMapRect.Width / theMainSystem.InspectionInfos[DisplayIndex].nImageWidth;
            double ScaleY = (double)DefectMapRect.Height / theMainSystem.InspectionInfos[DisplayIndex].nImageHeight;

            RectangleF Defectrect = new RectangleF();
            Font Font = new Font("Calibri", 10 , FontStyle.Bold);
            SolidBrush Indexbrush = new SolidBrush(Color.White);

            for (int i = 0; i < theMainSystem.InspectionInfos[DisplayIndex].InspResults.Count; i++) // Defect Count
            {

                var Result = theMainSystem.InspectionInfos[DisplayIndex].InspResults[i];

                if (Result == null)
                    continue;

                Color ClassColor = Color.Red;//Result.m_sClassColor;
                string LabelName = Result.m_sDefectType;
                string LabelFulName = $"No. {(i + 1).ToString()} {LabelName}";

                // 240430 SHJ 디펙트 위치 스케일 처리 해서 표시
                Defectrect.X = (float)((Result.DefectPos.X - (Result.Width / 2)) * ScaleX) + DefectMapRect.X;
                Defectrect.Y = (float)((Result.DefectPos.Y - (Result.Height / 2)) * ScaleY) + DefectMapRect.Y;
                Defectrect.Width = (float)10;//(Result.Width * ScaleX);
                Defectrect.Height = (float)10;//(Result.Height * ScaleY);

                if ((Defectrect.X + Defectrect.Width) > DefectMapRect.Width)
                    Defectrect.X -= Defectrect.Width;

                if ((Defectrect.Y + Defectrect.Height) > DefectMapRect.Height)
                    Defectrect.Y -= Defectrect.Height;

                graphics.DrawString(LabelFulName, Font, Indexbrush, Defectrect.X - (LabelFulName.Length + 10), Defectrect.Y - 15);
                graphics.FillEllipse(new SolidBrush(ClassColor), new RectangleF(Defectrect.X, Defectrect.Y, Defectrect.Width, Defectrect.Height));

            }
        }
        public void DoDefectCropCurrentPaint(Graphics graphics, int _Idx)
        {
            try
            {
                lock (m_Lock)
                {
                    // 240430 SHJ Crop 된 디펙트 영상 표시 
                    if (theMainSystem.InspectionInfos[DisplayIndex].InspResults.Count != 0)
                    {
                        using (Bitmap bmp = new Bitmap(DefectImages[DisplayIndex][_Idx].m_nWidth, DefectImages[DisplayIndex][_Idx].m_nHeight,
                            DefectImages[DisplayIndex][_Idx].m_nStride, PixelFormat.Format8bppIndexed, DefectImages[DisplayIndex][_Idx].pData))
                        {
                            ColorPalette palette = bmp.Palette;
                            for (int i = 0; i < palette.Entries.Length; i++)
                            {
                                palette.Entries[i] = Color.FromArgb(i, i, i);
                            }

                            bmp.Palette = palette;
                            graphics.DrawImage(bmp, 0, 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }



            //theMainSystem.ProductInfo.DefectManager[this.m_nCamNumber][0].DefectImage.Draw(graphics);
        }


        public void DoTeachDefectCurrentPaint(Graphics graphics, int _nCamNum)
        {
            try
            {
                theMainSystem.Cameras[this.m_nCamNumber].ListImages[m_InspIndex][0].MergeImageDraw(graphics);
                int Size = 300;
                Font Font = new Font("Calibri", Size, FontStyle.Regular);

                //250517 LYK 임시 주석
                for(int j = 0; j < theRecipe.MergeImageCount; j ++)
                {
                    for (int i = 0; i < theMainSystem.ProductInfo.DefectManager[m_InspIndex][this.m_nCamNumber][j].Count; i++)
                    {
                        if (!theMainSystem.ProductInfo.DefectManager[m_InspIndex][this.m_nCamNumber][j][i].m_bDeepLearningJudge)
                        {
                            Color color = Color.White;

                            for (int k = 0; k < theRecipe.ClassifyName.Count; k++)
                            {
                                if (theRecipe.ClassifyName[k].ClassifyName == theMainSystem.ProductInfo.DefectManager[m_InspIndex][this.m_nCamNumber][j][i].InspResult.m_sDefectType)
                                {
                                    color = theRecipe.ClassifyName[k].m_sClassColor;
                                    break;
                                }

                            }

                            graphics.DrawPolygon(new Pen(color, 5), theMainSystem.ProductInfo.DefectManager[m_InspIndex][this.m_nCamNumber][j][i].InspResult.m_InnerValue);

                            int PosX = (int)theMainSystem.ProductInfo.DefectManager[m_InspIndex][this.m_nCamNumber][j][i].InspResult.DefectPos.X - (Size + 100);
                            int PosY = (int)theMainSystem.ProductInfo.DefectManager[m_InspIndex][this.m_nCamNumber][j][i].InspResult.DefectPos.Y - (Size * 2);

                            // String 위치가 이미지 넘어가면 아래에 라벨 이름 표시 
                            if (PosY < 0)
                                PosY = (int)theMainSystem.ProductInfo.DefectManager[m_InspIndex][this.m_nCamNumber][j][i].InspResult.DefectPos.Y + (Size * 2);

                            graphics.DrawString($"{theMainSystem.ProductInfo.DefectManager[m_InspIndex][this.m_nCamNumber][j][i].InspResult.m_sDefectType}", Font, new SolidBrush(color), PosX, PosY);
                        }
                    }
                }
                
            }
            catch (Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Defect Display Error Catch : {e.Message}");
            }

        }

        public void DoSimulDefectCurrentPaint(Graphics graphics, int _nCamNum)
        {
            try
            {
                // 240430 SHJ 합성된 이미지 에 이물 디펙트 표시
                theMainSystem.Cameras[_nCamNum].ListImages[DisplayIndex][0].MergeImageDraw(graphics);   //250408 LYK 임시 주석

                var ImgList = theMainSystem.ProductInfo.DefectManager[DisplayIndex][this.m_nCamNumber];

                for(int i = 0; i < ImgList.Count; i++)
                {
                    for(int j = 0; j < ImgList[i].Count; j++)
                    {
                        var Result = ImgList[i][j];

                        if (Result == null)
                            continue;

                        pen.Color = Result.InspResult.m_sClassColor;
                        pen.Width = 1;

                        if (Result.InspResult.m_InnerValue != null)
                            graphics.DrawPolygon(pen, Result.InspResult.m_InnerValue);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Defect Simul Display Error Catch : {e.Message}");
            }
        }

        public void ShowImageHistoryDisplay(int _nMode)
        {
            //if (_nMode == 0)
            //{
            //    RefreshHistoryImage?.Invoke(ListHisotryImages[DEF_SYSTEM.MONO_R]);
            //}
            //else if (_nMode == 1)
            //{
            //    RefreshCurrentHistoryImage?.Invoke(ListHisotryImages[DEF_SYSTEM.MONO_R]);
            //    RefreshCurrentHistoryColorImage?.Invoke(ListHisotryImages[DEF_SYSTEM.COLOR]);
            //}
            //else
            //{
            //    RefreshColorHistoryImage?.Invoke(ListHisotryImages[DEF_SYSTEM.COLOR]);
            //}
                
        }


        private void DeepLearningRun(int InspIdx, int ImgCount)
        {
            // 20250910 SHJ 디스플레이 딥러닝 실행 
            if (theRecipe.m_sCurrentEquipment.Contains(DEF_SYSTEM.DISPLAY))
            {
                var ImageList = RuleBaseCompleteEvent[InspIdx];

                // 모든 룰베이스 데이터가 완료 될 때 까지 대기
                if (WaitHandle.WaitAll(ImageList.ToArray(), int.MaxValue))
                {
                    for (int i = 0; i < RuleBaseCompleteEvent[InspIdx].Count; i++)
                        RuleBaseCompleteEvent[InspIdx][i].Reset();

                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"DeepLearning Inspect Start ID: {InspDeepLearning.sProductName}");
                    // 디스플레이 딥러닝 실행
                    InspDeepLearning.WorkRun(ImgCount, InspIdx, ListImages[InspIdx], m_nInspMode);
                }

            }
            else if (theRecipe.m_sCurrentEquipment.Contains(DEF_SYSTEM.ELECTRONIC))
            {
                // 전기 딥러닝 실행 
                InspDeepLearning.WorkRun(ImgCount, InspIdx, ListImages[0], m_nInspMode);
            }

        }

        /// <summary>
        /// 24.01.24 LYK InspectionComplete 함수
        /// InspectionComplete 함수는 검사가 끝나면 호출 되며, CMainSystem의 InspectionCompleteEvent와 연동되어 있다.
        /// CMainSystem의 WaitInspectionCompleteEvent 함수에서 검사완료 Event를 기다리고 있다.
        /// 모든 Event가 Signal 되어야 결과 및 판정 함수로 진입된다.
        /// </summary>
        private void DeepLearningComplete(int RunIdx, int ImgIdx)
        {

            // 20250918 SHJ 기존 Insp Normal 모드일 경우만 진입 되도록 하였지만, Teach, Simul 등 자유도를 높이기 위해 메인 호출하는 종료 이벤트 Set 할때 Insp Mode 조건 보는 방식으로 변경 
            // 20250910 SHJ Display 같은 경우 딥러닝 완료 되면 메인 프로세스 호출 
            if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY)
            {
                // 20250918 SHJ 최종 메인 프로세스 호출은 Insp Normal 모드일 경우에만 호출 
                if (m_nInspMode == DEF_SYSTEM.INSP_NORMAL)
                {
                    theMainSystem.RefreshDisplayIdx?.Invoke(RunIdx);

                    for (int i = 0; i < InspectionCompleteEvent[RunIdx][this.m_nCamNumber].Count; i++)
                        InspectionCompleteEvent[RunIdx][DEF_SYSTEM.CAM_ONE][i].Set();

                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Image Count : {ImgIdx}, _InspecIdx : {RunIdx} , Inspect Complete Event Set ");
                }
                else // 기본 검사 모드가 아닐 경우 Teach, Simul 로 이미지 출력
                {
                    DisplayIndex = RunIdx;
                    ShowImageDisplay();
                }

            }
            else if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.ELECTRONIC)
            {
                // 20250910 SHJ Electronic 는 직렬 방식이기 때문에 마지막 이미지가 검사 완료 되면 메인 프로세스 이동 
                // 추가 룰베이스가 진행 될 경우 메인 프로세스로 이동 되는 아래 문구 대신 룰베이스 실행 내용으로 변경 필요 

                // 20250918 SHJ 최종 메인 프로세스 호출은 Insp Normal 모드일 경우에만 호출 
                if (m_nInspMode == DEF_SYSTEM.INSP_NORMAL)
                {
                    if (ImgIdx == theRecipe.MergeImageCount - 1)
                    {
                        theMainSystem.RefreshDisplayIdx?.Invoke(RunIdx);

                        for (int i = 0; i < InspectionCompleteEvent[RunIdx][this.m_nCamNumber].Count; i++)
                            InspectionCompleteEvent[RunIdx][this.m_nCamNumber][i].Set();
                    }
                }
                else // 기본 검사 모드가 아닐 경우 Teach, Simul 로 이미지 출력
                {
                    DisplayIndex = RunIdx;
                    ShowImageDisplay();
                }

            }

        }

        /// <summary>
        /// 24.01.24 LYK InspectionComplete 함수
        /// InspectionComplete 함수는 검사가 끝나면 호출 되며, CMainSystem의 InspectionCompleteEvent와 연동되어 있다.
        /// CMainSystem의 WaitInspectionCompleteEvent 함수에서 검사완료 Event를 기다리고 있다.
        /// 모든 Event가 Signal 되어야 결과 및 판정 함수로 진입된다.
        /// </summary>
        private void InspectionComplete(int RunIdx, int ImgIdx)
        {
            // 20250918 SHJ 기존 Insp Normal 모드일 경우만 진입 되도록 하였지만, Teach, Simul 등 자유도를 높이기 위해 메인 호출하는 종료 이벤트 Set 할때 Insp Mode 조건 보는 방식으로 변경 
            // 20250916 SHJ 디스플레이는 딥러닝 까지 완료된 시점에서 디스플레이 표시가 필요 하여 구분 처리 
            if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY) //디스플레이 룰베이스 -> 딥러닝 순서 , 룰베이스 종료 후 딥러닝 실행 
            {
                if (DEF_SYSTEM.LICENSES_DEEP_KEY == (int)DeepLicense.SAIGE)
                {
                    // 20250918 SHJ 검사 모드가 Teach 모드가 아닐 경우에 딥러닝 실행 (딥러닝 실행 조건은 Insp Normal, Simul)
                    // 디스플레이에서 Teach 모드일 경우 룰베이스만 돌릴 수 있고 딥러닝도 돌릴 수 있어서 상황에 맞게 호출
                    if(m_nInspMode != DEF_SYSTEM.INSP_TEACH)
                    {
                        // 딥러닝 실행 
                        if (ImgIdx == 0)
                        {
                            InspDeepLearning.StartTime = ListInspect[RunIdx][ImgIdx].StartTime;
                            InspDeepLearning.sProductName = ListInspect[RunIdx][ImgIdx].sProductName;
                            InspDeepLearning.sOriginalImagePath = ListInspect[RunIdx][ImgIdx].sOriginalImagePath;
                            InspDeepLearning.sDefectImagePath = ListInspect[RunIdx][ImgIdx].sDefectImagePath;

                            // 250910 SHJ 딥러닝 프로세스 실행 -> 구조는 변경 될 수 있음 
                            Task.Run(() => DeepLearningRun(RunIdx, ImgIdx));

                            Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"DeepLearning Inspect Events Wait");
                        }

                        RuleBaseCompleteEvent[RunIdx][ImgIdx].Set();
                    }
                }
                else //DeepLicense 가 없을 경우 룰베이스에서 종료 
                {
                    // 20250918 SHJ 최종 메인 프로세스 호출은 Insp Normal 모드일 경우에만 호출 
                    if (m_nInspMode == DEF_SYSTEM.INSP_NORMAL)
                    {
                        if (ImgIdx == 3)
                            theMainSystem.RefreshDisplayIdx?.Invoke(RunIdx);

                        InspectionCompleteEvent[RunIdx][DEF_SYSTEM.CAM_ONE][ImgIdx].Set();

                        Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Image Count : {ImgIdx}, _InspecIdx : {RunIdx} , Inspect Complete Event Set ");
                    }
                    else // 기본 검사 모드가 아닐 경우 Teach, Simul 로 이미지 출력
                    {
                        DisplayIndex = RunIdx;
                        ShowImageDisplay();
                    }
                }
            }
            else if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.ELECTRONIC) // 전기는 딥러닝 -> 룰베이스 순서여서 룰베이스 종료 후 메인 이동 
            {
                // 20250918 SHJ 최종 메인 프로세스 호출은 Insp Normal 모드일 경우에만 호출 
                if (m_nInspMode == DEF_SYSTEM.INSP_NORMAL)
                {
                    if (ImgIdx == 3)
                        theMainSystem.RefreshDisplayIdx?.Invoke(RunIdx);

                    InspectionCompleteEvent[RunIdx][DEF_SYSTEM.CAM_ONE][ImgIdx].Set();

                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Image Count : {ImgIdx}, _InspecIdx : {RunIdx} , Inspect Complete Event Set ");
                }
                else // 기본 검사 모드가 아닐 경우 Teach, Simul 로 이미지 출력
                {
                    DisplayIndex = RunIdx;
                    ShowImageDisplay();
                }
            }

        }

        /// <summary>
        /// 24.01.24 LYK InspectionRun 함수
        /// 이물 검사를 하기 위한 Thread 함수
        /// </summary>
        private void InspectionRun(int _CurIdx, int _ImgCnt, int _InspMode)
        {
            if (theRecipe.m_sCurrentEquipment.Contains(DEF_SYSTEM.DISPLAY))
            {
                //250408 LYK 이미지 카운트가 0일때 Start 타임 할당
                // 폴더 경로 및 정보는 이미지 카운트가 0 일때 할당
                //Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Inspection Run -> Image Count : {m_nImageCnt}, Inspection Index : {m_InspIndex} ");
                //ListInspect[m_InspIndex][m_nImageCnt].WorkRun(m_nImageCnt, m_InspIndex, ListImages[m_InspIndex], DEF_SYSTEM.INSP_NORMAL);

                m_InspIndex = _CurIdx; // 250530 SHJ -> Teaching 에서 사용 하기 위해 런 인덱스 받아놓고 있음 
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Inspection Run -> Image Count : {_ImgCnt}, Inspection Index : {_CurIdx} ");
                ListInspect[_CurIdx][_ImgCnt].WorkRun(_ImgCnt, _CurIdx, ListImages[_CurIdx], _InspMode);
            }
            else
            {
                // 20250911 SHJ 전기도 룰베이스 실행은 다른 조건이 없을것 같지만 분리 처리 
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Inspection Run -> Image Count : {_ImgCnt}, Inspection Index : {_CurIdx} ");
                ListInspect[_CurIdx][_ImgCnt].WorkRun(_ImgCnt, _CurIdx, ListImages[_CurIdx], _InspMode);
            }
        }

        public void SimulationInspectRun(int CurIndex,int ImageCnt)
        {
            // 20250911 SHJ 시뮬레이션 분리 
            if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY)
                Task.Run(() => InspectionRun(CurIndex, ImageCnt, m_nInspMode));
            else if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.ELECTRONIC)
                DeepLearningRun(CurIndex, ImageCnt);
        }

        /// <summary> 
        /// 24.01.18 LYK Image Display 함수
        /// CCameraManager 클래스 외부에서 Image 디스플레이 함수를 호출 하기 위한 함수
        /// </summary>
        public void ImageDisplay(int _nCurIdx)   
        {
            DisplayIndex = _nCurIdx;
            Thread_DefectDisplay.Continue(); // Crop 된 디펙트 10개 출력 
            Thread_ImageDisplay.Continue(); // Main 화면 이미지 출력
            Thread_DefectImageSave.Continue();
        }


        /// <summary>
        /// 24.03.04 LYK SetRefreshGrabImage
        /// CImageControl 클래스에서 Grab 관련된 델리게이트 변수에 접근 하기 위한 Setter 함수
        /// </summary>
        /// <param name="_RefreshGrab"> CImage 인자를 갖는 함수를 인자로 전달</param>

        public void SetRefreshGrabImage(int _nMode, Action<CImage> _RefreshGrab, int Idx = 0)
        {
            switch(_nMode)
            {
                case DEF_SYSTEM.MONO_IMAGE_GRAB:

                    RefreshGrabImage = _RefreshGrab;       //Main Display Mono

                    break;

                case DEF_SYSTEM.MONO_LIVE_IMAGE_GRAB:

                    RefreshLiveImage = _RefreshGrab;       //Live Display Mono

                    break;

                case DEF_SYSTEM.MONO_CALIBRATION_GRAB:

                    RefreshCalibration = _RefreshGrab;       //Live Display Mono

                    break;

                case DEF_SYSTEM.MONO_IMAGE_TABLE_GRAB:
                    RefreshDefectMapImage = _RefreshGrab;

                    break;

                case DEF_SYSTEM.MONO_IMAGE_DEFECT_GRAB:
                    RefreshDefectImage[Idx] = _RefreshGrab;

                    break;
            }
        }

        /// <summary>
        /// 24.01.23 LYK SetRefreshHistoryImage
        /// CHistoryImageControl 클래스에서 RefreshHistoryImage(델리게이트 변수)에 접근 하기 위한 Setter 함수
        /// </summary>
        /// <param name="_RefreshHistoryImage">CImage 인자를 갖는 함수를 인자로 전달</param>
        public void SetRefreshCurHistoryImage(Action<CImage> _RefreshCurHistoryImage)
        {
            RefreshCurrentHistoryImage = _RefreshCurHistoryImage;       //Main Display Mono
        }

        /// <summary>
        /// 24.01.23 LYK SetRefreshHistoryImage
        /// CHistoryImageControl 클래스에서 RefreshHistoryImage(델리게이트 변수)에 접근 하기 위한 Setter 함수
        /// </summary>
        /// <param name="_RefreshHistoryImage">CImage 인자를 갖는 함수를 인자로 전달</param>
        public void SetRefreshHistoryImage(Action<CImage> _RefreshHistoryImage)
        {
            RefreshHistoryImage = _RefreshHistoryImage;

        }

        public void SetRefreshSimulImage(Action<CImage> _RefreshSimulImage)
        {
            RefreshSimulHistoryImage = _RefreshSimulImage;       //Main Display Mono
        }

        public void SetRefreshTeachingImage(Action<CImage> _RefreshTeachImage)
        {
            RefreshTeachImage = _RefreshTeachImage;
        }

        /// <summary>
        /// 24.01.18 LYK SimulationImgRead
        /// 시뮬레이션을 위해 이미지를 로드 하기 위한 함수
        /// MainSystem에서 호출하여 각 카메라별 ListImage 변수에 영상을 로드 함
        /// </summary>
        /// <param name="_ImagePath">영상의 경로 인자로 전달</param>
        /// <param name="_nNumber">WaferID를 인자로 전달</param>
        public void SimulationImgRead(int CurIndex, int _ImgCount, string _ImagePath)
        {


            using (Bitmap bmp = new Bitmap(_ImagePath))
            {
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

                if (theRecipe.m_sCurrentEquipment == DEF_SYSTEM.ELECTRONIC)
                {
                    ListImages[CurIndex][_ImgCount].Copy(bmpData.Width, bmpData.Height, 1, bmpData.Scan0);
                    ListImages[CurIndex][0].Merge(bmpData.Width, bmpData.Height, 1, bmpData.Scan0, _ImgCount, true);
                }
                else
                {
                    if (_ImgCount != 0)
                        ListImages[CurIndex][_ImgCount].Copy(bmpData.Width, bmpData.Height, 1, bmpData.Scan0);

                    ListImages[CurIndex][0].Copy(bmpData.Width, bmpData.Height, 1, bmpData.Scan0, 13600, 5540, _ImgCount);
                    ListImages[CurIndex][0].Merge(bmpData.Width, bmpData.Height, 1, bmpData.Scan0, _ImgCount, true);
                }

                //ListImages[m_InspIndex][0].Merge(bmpData.Width, bmpData.Height, 1, bmpData.Scan0, m_nImageCnt);
                bmp.UnlockBits(bmpData);
            }

        }

        /// <summary>
        /// 24.03.02 LYK SetInspectionConpleteEvent
        /// CMainSystem 클래스에서 InspectionCompleteEvents(델리게이트 변수)에 접근 하기 위한 Setter 함수
        /// </summary>
        public void SetInspectionCompleteEvent(int mode, int instanceIdx, int camNum, int imgNum, ManualResetEvent evt)
        {
            switch (mode)
            {
                case DEF_SYSTEM.PARTICLE_INSPECTION_EVENT:
                    EnsureEventListSize(instanceIdx, camNum, imgNum);
                    InspectionCompleteEvent[instanceIdx][camNum][imgNum] = evt;
                    break;

                case DEF_SYSTEM.PARTICLE_DEEPLEARNING_EVENT:
                    //DeepLearningCompleteEvent = evt;
                    break;
            }
        }

        private void EnsureEventListSize(int instanceIdx, int camNum, int imgNum)
        {
            while (InspectionCompleteEvent.Count <= instanceIdx)
                InspectionCompleteEvent.Add(new List<List<ManualResetEvent>>());

            while (InspectionCompleteEvent[instanceIdx].Count <= camNum)
                InspectionCompleteEvent[instanceIdx].Add(new List<ManualResetEvent>());

            while (InspectionCompleteEvent[instanceIdx][camNum].Count <= imgNum)
                InspectionCompleteEvent[instanceIdx][camNum].Add(null);
        }

        public void SetInspectionTimeInfo(Action<int, long> _InspectionTimeInfo)
        {
            InspectionTimeInfo = _InspectionTimeInfo;
        }

        public void InspectionTimeCheck(int _nInspectionType, long _sInspectionTime)
        {
            InspectionTimeInfo?.Invoke(_nInspectionType, _sInspectionTime);
        }

        public CImage GetListImage(int _idx, bool isHistory = false)
        {
            return isHistory ? ListHisotryImages[_idx] : ListImages[_idx][0]; //250408 LYK 임시 주석 ListImage 다시 봐야 함
        }

        public void SetInspectionInfos(InspectionInfo _Infos)
        {
            InspInfos = _Infos;
        }

        public void RunTeachingInspect(int _ImgCount, bool MergeMode = false, bool CellSelectMode = false, int CellIndex = 0)
        {
            // 250514 SHJ Teaching Page 에서 ToolBlock 개별로 실행 시키지 않고 CamearaManager 에서 공통 되게 전체 ToolBlock 실행 
            // 현재 Insp 버퍼에 담고 있는 ToolBlock 은 깊은 복사를 하여서 개별로 Tool 들을 가지고 있는 상태 

            m_nInspMode = DEF_SYSTEM.INSP_TEACH;

            m_bTeachExcuteMode = CellSelectMode;
            m_nTeachCellIndex = CellIndex;

            int TotalImgCount = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? theRecipe.SliceImageCount : theRecipe.MergeImageCount;
            // Full Mode 일 경우 이미지 카운트 전체를 검사 
            int StartIdx = MergeMode ? 0 : _ImgCount;
            int EndIdx = MergeMode ? TotalImgCount : _ImgCount + 1;

            for (int i = StartIdx; i < EndIdx; i++)
                InspectionRun(m_InspIndex, i, DEF_SYSTEM.INSP_TEACH);
        }

        public double[] RunHistogram(CImage Image)
        {
            double[] retvalue = new double[Image.m_nWidth + 1];

            Marshal.Copy(Image.pData, Image.m_pImage, 0, Image.m_nImageSize);

            int width = Image.m_nWidth;
            int height = Image.m_nHeight;

            for (int x = 0; x < width; x++)
            {
                int MeanValue = 0;
                int idx = x;

                for (int y = 0; y < height; y++)
                {
                    int jump = width * y;

                    MeanValue += Image.m_pImage[jump + x];
                }

                MeanValue /= height;

                retvalue[idx] = MeanValue;
            }

            return retvalue;
        }

        public void ToolBlockRefresh()
        {
            // 250530 SHJ 임시 주석 현재는 툴블럭을 레시피에서 바로 가지고 와서 사용 하기 때문에 Refresh 해줄 필요가 없음 

            CWork work = null;

            int nImageCount = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? theRecipe.SliceImageCount : theRecipe.MergeImageCount;
            for (int i = 0; i < DEF_SYSTEM.INSP_CNT; i++)
            {
                for (int j = 0; j < nImageCount; j++)
                {
                    work = ListInspect[i][j];
                    work.Uninitialize();
                }

                ListInspect[i].Clear();
            }

            for(int i = 0; i < DEF_SYSTEM.INSP_CNT; i ++)
            {
               for (int j = 0; j < nImageCount; i++)
                {
                    CWork Inspect = CWork.WorkPick(DEF_SYSTEM.PARTICLE_INSPECTION, m_nCamNumber, $"{DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE}\\{theRecipe.m_sCurrentModelName}\\{"Saige_Segment"}");
                    Inspect.InspectIdx = i;
                    Inspect.ImageCount = j;
                    Inspect.Initialize((int)m_Camera.m_nImgWidth, (int)m_Camera.m_nImgHeight);
                    Inspect.JudgeComplete = InspectionComplete;
                    Inspect.InspectionTimeInfo = InspectionTimeCheck;

                    ListInspect[j].Add(Inspect);
                }

            }

        }

        private void DefectImageCopy(int Idx)
        {
            /* //250517 LYK 임시 주석
            for(int i = 0; i< theMainSystem.ProductInfo.DefectManager[m_nCamNumber].Count; i ++)
            {
                // 240512 SHJ Defect Crop Image CameraManager 에서 저장 하도록 위치 변경 
                // Defect 최고 수량 Define 설정 
                if (i < DEF_SYSTEM.DEFECT_MAX_COUNT)
                {
                    CImage SrcImg = theMainSystem.ProductInfo.DefectManager[m_nCamNumber][i].DefectImage;

                    // 이미지 형태가 달라지면 재 할당 
                    if (DefectImages[Idx][i].m_nWidth != SrcImg.m_nWidth || DefectImages[Idx][i].m_nHeight != SrcImg.m_nHeight || DefectImages[Idx][i].m_nPixelFormat != SrcImg.m_nPixelFormat || DefectImages[Idx][i].m_nStride != SrcImg.m_nStride)
                    {
                        // 수동으로 Free 처리 하는 이유는 프로그램 종료 될 때 Free 메소드 정상적으로 탈 수 있기 위해 사용 
                        Marshal.FreeHGlobal(DefectImages[Idx][i].pData);

                        // 240430 SHJ 이미지를 크롭 하면 Stride 가 맞지 않아 수동으로 할당 (ex 300,300 으로 이미지 크롭시 Stride 는 28804 유지) 
                        DefectImages[Idx][i].m_nWidth = SrcImg.m_nWidth;
                        DefectImages[Idx][i].m_nHeight = SrcImg.m_nHeight;
                        DefectImages[Idx][i].m_nPixelFormat = SrcImg.m_nPixelFormat;
                        DefectImages[Idx][i].m_nStride = SrcImg.m_nStride;
                        DefectImages[Idx][i].m_nImageSize = SrcImg.m_nImageSize;
                        DefectImages[Idx][i].pData = Marshal.AllocHGlobal(SrcImg.m_nImageSize);
                    }

                    CFunc.memcpy(DefectImages[Idx][i].pData, SrcImg.pData, DefectImages[Idx][i].m_nImageSize);

                    // 이미지 Copy 완료 되면 Defect Image Save
                    DefectSaveImage(Idx, i);
                }
            }
            */
        }

        public CogToolBlock GetInspToolBlock(int ImgCount)
        {
            return ListInspect[m_InspIndex][ImgCount].GetInspToolBlock();
        }

        private void DefectSaveImage()
        {
            
            int validImageCount = -1;

            for (int i = 0; i < theMainSystem.ProductInfo.DefectManager[DisplayIndex][this.m_nCamNumber].Count; i++)
            {
                var defectList = theMainSystem.ProductInfo.DefectManager[DisplayIndex][this.m_nCamNumber][i];
                if (defectList != null && defectList.Count > 0)
                {
                    validImageCount = i;
                    break;
                }
            }

            //250517 LYK 임시 주석
            string ID = theMainSystem.ProductInfo.DefectManager[DisplayIndex][this.m_nCamNumber][validImageCount][0].m_sProductName;
            string StartTime = theMainSystem.ProductInfo.DefectManager[DisplayIndex][m_nCamNumber][validImageCount][0].StartTime.ToString("yyMMdd_HHmmss_fff");
            string DefectImagePath = theMainSystem.ProductInfo.DefectManager[DisplayIndex][m_nCamNumber][validImageCount][0].DefectImagePath;


            //string[] sStartTime = StartTime.Split('_');
            //string sStrHour = sStartTime[1].Substring(0, 2);

            // Defect 경로
            if (!Directory.Exists(DefectImagePath))
                Directory.CreateDirectory(DefectImagePath);

            //// 시간 폴더 구분
            //string shourPath = string.Format("{0}{1}\\", DefectImagePath, sStrHour);

            //if (!Directory.Exists(shourPath))
            //    Directory.CreateDirectory(shourPath);

            // ID 폴더 구분 
            string sIDPath = string.Format("{0}\\{1}_{2}",DefectImagePath, StartTime, ID);

            if (!Directory.Exists(sIDPath))
                Directory.CreateDirectory(sIDPath);

            int Idx = 0;
            int ImageCnt = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? theRecipe.SliceImageCount : theRecipe.MergeImageCount;

            for (int i = 0; i < ImageCnt; i++)
            {
                for(int j = 0; j < theMainSystem.ProductInfo.DefectManager[DisplayIndex][m_nCamNumber][i].Count; j++)
                {

                    string SavePath = $"{sIDPath}\\{StartTime}_{ID}_{theMainSystem.ProductInfo.DefectManager[DisplayIndex][m_nCamNumber][i][j].InspResult.m_sDefectType}{Idx}.jpg";

                    ImageSaverManager.EnqueueSaveRequest(SavePath, theMainSystem.ProductInfo.DefectManager[DisplayIndex][m_nCamNumber][i][j].DefectImage);
                    Idx++;

                    theMainSystem.Wait(1);
                }
            }
            
        }
    }
}
