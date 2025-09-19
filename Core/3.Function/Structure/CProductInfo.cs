using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Core;
using Core.Function;
using static Core.Program;
using System.Xml;

namespace Core.Function
{
    /// <summary> LYK 22.05.13 ProductInfo 클래스
    /// PLC 통신등을 통해 제품 아이디(Panel ID or SheetID)를 받고 필요한 폴더를 생성 하기 위한 클래스
    /// </summary>
    public class CProductInfo
    {
        public int m_nModelNo = 0;
        public string m_sModelName = string.Empty;
        public string m_sProductName = string.Empty; //220511 LYK 제품명(패널 아이디 등))

        public StringBuilder OrginalImagePath = new StringBuilder();
        public StringBuilder DefectImagePath = new StringBuilder();
        public StringBuilder ResultCsvFolder = new StringBuilder();
        public StringBuilder ResultCsvPath = new StringBuilder();
        public StringBuilder ResultInspectionTimePath = new StringBuilder();        //230831 LYK Result Inspection Time CSV Path


        //public List<List<CDefectManager>> DefectManager = new List<List<CDefectManager>>();
        public List<List<List<CDefectManager>>> DefectManager = new List<List<List<CDefectManager>>>();
        public List<List<CDefectManager>> TeachDefectManager = new List<List<CDefectManager>>();            //240304 LYK TeachInspection DefectManager

        //public List<CDefectManager> EdgeToBusbarResultManager = new List<CDefectManager>();

        public STime StartTime; 
            
        public CProductInfo()
        {
            /* List
            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                DefectManager.Add(new CDefectManager());
            */

            /* List List
            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
            {
                var imageList = new List<CDefectManager>();
                for (int j = 0; j < theRecipe.MergeImageCount; j++)
                {
                    imageList.Add(new CDefectManager());
                }
                DefectManager.Add(imageList);
            }
            */
            int ImgCount = theRecipe.m_sCurrentEquipment == DEF_SYSTEM.DISPLAY ? theRecipe.SliceImageCount : theRecipe.MergeImageCount;

            for (int instanceIdx = 0; instanceIdx < DEF_SYSTEM.INSP_CNT; instanceIdx++)
            {
                var camList = new List<List<CDefectManager>>();
                TeachDefectManager = new List<List<CDefectManager>>();

                for (int camNum = 0; camNum < DEF_SYSTEM.CAM_MAX_COUNT; camNum++)
                {
                    var mergeList = new List<CDefectManager>();
                    var TeachMergeList = new List<CDefectManager>();
                    for (int mergeIdx = 0; mergeIdx < ImgCount; mergeIdx++)
                    {
                        mergeList.Add(new CDefectManager());
                        TeachMergeList.Add(new CDefectManager());
                    }
                    camList.Add(mergeList);
                    TeachDefectManager.Add(TeachMergeList);
                }
                DefectManager.Add(camList);
            }


            int test = 0;
        }

        public void Initialize(string _sProductName, STime _Time)
        {

            //if (theMainSystem.tempFileName != "")
            //    m_sProductName = _sProductName + "_" + theMainSystem.tempFileName;
            //else if (theMainSystem.InterfacePLC.m_bIsTRGFlag)//240423 KCH WaferID 설정 조건 추가
            //    m_sProductName = theMainSystem.InterfacePLC.WaferID;
            //else
            m_sProductName = _sProductName;

            StartTime = _Time;

            string sFolder = string.Format("{0}\\{1}{2}{3}\\", DEF_SYSTEM.DEF_FOLDER_PATH_IMAGE
                                                                 , StartTime.Year
                                                                 , StartTime.Month
                                                                 , StartTime.Day
                                                                 );

            OrginalImagePath.Clear();
            //if (theRecipe.m_bSaveAllImage)
            {
                if (!Directory.Exists(sFolder))
                    Directory.CreateDirectory(sFolder);
            }
            

            OrginalImagePath.Append(sFolder);

            sFolder = string.Format("{0}\\{1}{2}{3}\\", DEF_SYSTEM.DEF_FOLDER_PATH_DEFECTIMAGE
                                                         , StartTime.Year
                                                         , StartTime.Month
                                                         , StartTime.Day);

            DefectImagePath.Clear();
            DefectImagePath.Append(sFolder);

            sFolder = string.Format("{0}\\{1}{2}", DEF_SYSTEM.DEF_FOLDER_PATH_CSV
                                                              , StartTime.Year
                                                              , StartTime.Month);
                                                              //, StartTime.Day
                                                              //);

            //220511 LYK Defect 결과 폴더 경로(폴더를 무조건 만들지 않고 NG가 발생 했을때 폴더 생성))
            ResultCsvFolder.Clear();
            ResultCsvFolder.Append(sFolder);

            ResultCsvPath.Clear();
            ResultCsvPath.Append(ResultCsvFolder + string.Format("\\{0}{1}{2}_Result.csv", StartTime.Year
                                                                                           , StartTime.Month
                                                                                           , StartTime.Day
                                                                                           ));
            if (!Directory.Exists(ResultCsvFolder.ToString()))
               Directory.CreateDirectory(ResultCsvFolder.ToString());

            //sFolder = string.Format("{0}\\{1}{2}\\{3}\\TrackingData", DEF_SYSTEM.DEF_FOLDER_PATH_CSV
            //                                                  , StartTime.Year
            //                                                  , StartTime.Month
            //                                                  , StartTime.Day
            //                                                  );

            //if (!Directory.Exists(sFolder))
            //    Directory.CreateDirectory(sFolder);

            //ResultInspectionTimePath.Clear();
            //ResultInspectionTimePath.Append(sFolder + string.Format("\\{0}{1}{2}_InspectionTime.csv", StartTime.Year
            //                                                                               , StartTime.Month
            //                                                                               , StartTime.Day));
        }
    }
}
