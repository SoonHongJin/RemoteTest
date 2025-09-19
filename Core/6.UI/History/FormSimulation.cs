using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Core;
using Core.Utility;
using static Core.Program;


using Cognex.VisionPro;
using Cognex.VisionPro.CalibFix;

using Insnex.Vision2D.CalibFix;
using Insnex.Vision2D.Core;

namespace Core.UI
{
    public partial class FormSimulation : Form
    {
        private const int PageSize = 50; //240720 KCH 한페이지당 파일 수

        private List<string> ImageFolderList = new List<string>();
        private MainForm MainForm = null;
        private CImage Image;// = new CImage(DEF_SYSTEM.LICENSES_KEY);

        private int CurrentPage = 0;     //240720 KCH 현재 페이지 번호

        public List<CSimulImageControl> Frames = new List<CSimulImageControl>();

        public string m_sWaferID = string.Empty;
        public string sFolderName = string.Empty;

        public bool bSimulFirstCheck = false; //240720 KCH 시물레이션 페이지 처음 띄울때 체크하기 위한 변수
        public bool bSimulInsp = false;       //240720 KCH 시물레이션 검사 판단하기 위한 변수
        public bool bNoImage = false;

        private CLogger Logger = null;


        public FormSimulation(MainForm _MainForm, CLogger _logger)
        {
            InitializeComponent();
            TopLevel = false;
            MainForm = _MainForm;
            Logger = _logger;
            MainForm.SetEachControlResize(this);    //240801 NIS Conrol Resize

            Image = new CImage(DEF_SYSTEM.LICENSES_KEY, theRecipe.m_sCurrentEquipment);
            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; ++i)
            {
                Frames.Add(new CSimulImageControl(MainForm)
                {
                    Id = i,
                    m_ImageModeSimul = i
                });

                MainDisplayPanel.Controls.Add(Frames[i]);
                Frames[i].SetCamera(theMainSystem.Cameras[i], 0, this);

            }
            Image.Allocate((int)5120, (int)5120, CImage.PIXEL8);

            //rb_OK.Checked = true;

            this.FormLocation(0);
            
        }

        private void FormLocation(int _type)
        {
            //Maximized = !Maximized;
            Rectangle fullSize = new Rectangle(0, 0, MainDisplayPanel.Width, MainDisplayPanel.Height);

            int width = MainDisplayPanel.Width;
            int height = MainDisplayPanel.Height;
            int length = width < height ? width : height;

            Rectangle[] bounds = new Rectangle[5];

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; ++i)
            {
                bounds[i].X = 0;
                bounds[i].Y = 0;
                bounds[i].Width = MainDisplayPanel.Width;
                bounds[i].Height = MainDisplayPanel.Height;

                // Mono Display 분할 
                Frames[i].Bounds = bounds[i];
                Frames[i].Show();

            }
        }

        private void CBO_SELECT_MONTH_SelectedIndexChanged(object sender, EventArgs e) // 240510 KCH Month 콤보박스에 year+month 데이터 추가
        {
            CBO_SELECT_DATE.Items.Clear();

            string sSelectFolder = CBO_SELECT_MONTH.SelectedItem.ToString();

            DirectoryInfo DirInfo = new DirectoryInfo(DEF_SYSTEM.DEF_FOLDER_PATH_IMAGE);

            foreach (DirectoryInfo File in DirInfo.GetDirectories())
            {
                if (File.Name.Contains(sSelectFolder))
                {
                    string temp = File.Name.Substring(6);
                    CBO_SELECT_DATE.Items.Add(temp);
                }
            }
        }

        private void CBO_SELECT_DATE_SelectedIndexChanged(object sender, EventArgs e) //240720 KCH Date 콤보박스 IndexSelect
        {
            CurrentPage = 0;
            LoadImageList();
        }

        private void LoadImageList() //240720 KCH NG 또는 OK 이미지 폴더 불러와서 리스트 박스에 Display
        {
            try
            {
                DataListBox.Items.Clear(); //240720 KCH DataListBox 클리어
                ImageFolderList.Clear();   //240720 KCH Image 폴더 리스트 클리어

                //250301 NWT Image Forder Path change
                string m_sDate = CBO_SELECT_MONTH.SelectedItem.ToString() + CBO_SELECT_DATE.SelectedItem.ToString();


                sFolderName = $"{DEF_SYSTEM.DEF_FOLDER_PATH_IMAGE}\\{m_sDate}\\NG";

                DirectoryInfo DirInfo = new DirectoryInfo(sFolderName);
                FileInfo[] files = DirInfo.GetFiles();

                foreach (DirectoryInfo folder in DirInfo.GetDirectories())
                {
                    ImageFolderList.Add(folder.Name); //240720 KCH 폴더 목록 리스트에 저장
                }

                DisplayPage(); //240720 KCH  리스트 박스에 폴더 목록 분할 출력
                DataListBox.SelectedIndex = 0; //240720 KCH WaferID 폴더 목록 불러 온 후 첫번째 항목 선택 
            }
            catch
            {

            }
        }      
        

        private void bt_RunSimul_Click(object sender, EventArgs e)
        {
            try
            {
                for(int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++)
                {

                    theMainSystem.Cameras[i].m_bIsInspection = true; // Inspection Mode 
                    theMainSystem.Cameras[i].m_InspIndex = 0;
                    theMainSystem.Cameras[i].m_nInspMode = DEF_SYSTEM.INSP_NORMAL;
                }

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], string.Format("Simulation Inspect Start"));
                bSimulFirstCheck = false;                               //240720 KCH Simulation 페이지 처음 띄우고 Run 할때 false로 변경
                bSimulInsp = true;                                      //240720 KCH Simulation 검사 판단 변수
                string WaferID = DataListBox.SelectedItem.ToString();   //240720 KCH 리스트 박스에서 선택한 Simulation검사 할 WaferID
                SimulInspect(sFolderName, WaferID);                     //240720 KCH Simulation검사 시작 함수

                //240720 KCH 리스트 박스에서 선택된 웨이퍼의 검사가 끝나면 다음 인덱스 선택
                if (DataListBox.SelectedIndex >= DataListBox.Items.Count - 1)
                    DataListBox.SelectedIndex = 0;
                else
                    DataListBox.SelectedIndex++;
            }
            catch (Exception error)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"RunSimul button Catch : {error.Message}");
            }

        }

        private void SimulInspect(string _ImagePath, string _WaferID) //240720 KCH 시뮬검사 하기 위한 함수
        {
            int ListImageIndex = 0;
            //theMainSystem.SimulTest(_ImagePath, _WaferID);
        }

        private void DisplayPage() //240720 KCH DataList에 WaferID 분할 출력 
        {
            try
            {
                DataListBox.Items.Clear();
                int start = CurrentPage * PageSize;
                int end = start + PageSize > ImageFolderList.Count ? ImageFolderList.Count : start + PageSize;
                for (int i = start; i < end; i++)
                {
                    DataListBox.Items.Add(ImageFolderList[i]); 
                }
                UpdateCurrentPageLebel();

                int TotalPage = (ImageFolderList.Count + PageSize - 1) / PageSize; 
                btnBeforePage.Enabled = CurrentPage > 0 ? true : false;
                btBefore10page.Enabled = CurrentPage == 0 ? false : true;
                btSetfirstPage.Enabled = CurrentPage == 0 ? false : true;
                btNextPage.Enabled = (CurrentPage + 1) < TotalPage ? true : false;
                btSetLastPage.Enabled = (CurrentPage + 1) < TotalPage ? true : false;
                btNext10Page.Enabled = (CurrentPage + 10) < TotalPage ? true : false;
            }
            catch
            {

            }

        }

        public void LoadDateList()
        {
            CBO_SELECT_MONTH.Items.Clear();

            string sFolderName = DEF_SYSTEM.DEF_FOLDER_PATH_IMAGE;
            DirectoryInfo DirInfo = new DirectoryInfo(sFolderName);

            foreach (DirectoryInfo File in DirInfo.GetDirectories())
            {
                string tempname = File.Name.Substring(0, 6);
                if(!CBO_SELECT_MONTH.Items.Contains(tempname))
                    CBO_SELECT_MONTH.Items.Add(tempname);
            }
        }

        private void DataListBox_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                m_sWaferID = DataListBox.SelectedItem.ToString();
            }
            catch
            {

            }

        }

        private Pen DisplayArrowPen(Color color, int fontSize, bool IsBusbar) //240720 KCH 시뮬폼에서 EdgeToBusbar 화살표 Paint
        {
            Pen pen = new Pen(color, fontSize);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            if (IsBusbar)
                pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            else
                pen.EndCap = System.Drawing.Drawing2D.LineCap.NoAnchor;
            return pen;
        }

        public void DoDefectCurrentPaint(Graphics graphics, int nIdx) //240720 KCH 모노 Imag Display + 결과 Paint
        {
            try
            {
                if (bSimulFirstCheck)
                {
                    this.Image.Draw(graphics);
                }
                else
                {
                    theMainSystem.Cameras[nIdx].GetListImage(0).Draw(graphics);

                    for (int i = 0; i < theMainSystem.ProductInfo.DefectManager.Count; i++)     //240613 LYK 비효율적이므로 수정해야함
                    {
                        //if (theMainSystem.ProductInfo.DefectManager[i].m_nCamNum == nIdx)
                        {
                            //if (theMainSystem.OverLap(theMainSystem.InspectionInfos[theMainSystem.m_CurrrentIdx].EdgeToBusbarPointResult[nIdx].InspectionRegion, theMainSystem.ProductInfo.DefectManager[i].m_nCamNum, theMainSystem.ProductInfo.DefectManager[i].m_nPos.X, theMainSystem.ProductInfo.DefectManager[i].m_nPos.Y))
                            //graphics.DrawPolygon(new Pen(theMainSystem.ProductInfo.DefectManager[nIdx][i].InspResult.m_sClassColor, 1), theMainSystem.ProductInfo.DefectManager[nIdx][i].InspResult.m_InnerValue);    //250517 LYK 임시 주석
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Defect Display Erro Catch : {e.Message}");
            }

        }

        public void GridViewClear() //240720 KCH 폼 처음 띄울때 UI 초기화
        {
            DataListBox.Items.Clear();
            CBO_SELECT_DATE.Items.Clear();
            CBO_SELECT_MONTH.Items.Clear();
            lbCurrentPage.Text = "";
            tbSearchText.Text = "";
        }

        private void UpdateCurrentPageLebel()
        {
            int TotalPage = ((ImageFolderList.Count % PageSize) > 0) ? (ImageFolderList.Count / PageSize) + 1 : (ImageFolderList.Count / PageSize);
            lbCurrentPage.Text = $"{CurrentPage + 1} of {TotalPage}"; //240710 KCH 현재 페이지 번호 출력
        }

        private void btNextPage_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            DisplayPage();
        }

        private void btnBeforePage_Click(object sender, EventArgs e)
        {
            CurrentPage--;
            DisplayPage();
        }

        private void btBefore10page_Click(object sender, EventArgs e)
        {
            CurrentPage = (CurrentPage - 10 < 0) ? 0 : CurrentPage - 10;
            DisplayPage();
        }

        private void btNext10Page_Click(object sender, EventArgs e)
        {
            int TotalPage = ((ImageFolderList.Count % PageSize) > 0) ? (ImageFolderList.Count / PageSize) + 1 : (ImageFolderList.Count / PageSize);
            CurrentPage = (CurrentPage + 10 >= TotalPage) ? TotalPage : CurrentPage + 10;
            DisplayPage();
        }

        private void btSetLastPage_Click(object sender, EventArgs e)
        {
            int TotalPage = ((ImageFolderList.Count % PageSize) > 0) ? (ImageFolderList.Count / PageSize) + 1 : (ImageFolderList.Count / PageSize);
            CurrentPage = TotalPage - 1;
            DisplayPage();
        }

        private void btSetfirstPage_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            DisplayPage();
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            try
            {
                btnBeforePage.Enabled = false;
                btBefore10page.Enabled = false;
                btSetfirstPage.Enabled = false;
                btNextPage.Enabled = false;
                btSetLastPage.Enabled = false;
                btNext10Page.Enabled = false;
                lbCurrentPage.Text = "";

                DataListBox.Items.Clear();
                for (int i = 0; i < ImageFolderList.Count; i++)
                {
                    if (ImageFolderList[i].Contains(tbSearchText.Text))
                        DataListBox.Items.Add(ImageFolderList[i]);
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 24.12.13 NWT Combobox 가장 최근 날짜 로드
        /// </summary>
        public void SetDataList()
        {
            int monthcnt = CBO_SELECT_MONTH.Items.Count;
            CBO_SELECT_MONTH.SelectedIndex = monthcnt - 1;
            int datecnt = CBO_SELECT_DATE.Items.Count;
            CBO_SELECT_DATE.SelectedIndex = datecnt - 1;
        }

    }
}
