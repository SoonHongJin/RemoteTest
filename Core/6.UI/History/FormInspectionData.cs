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
using Core.Function;
using Core.Utility;
using static Core.Program;
using System.Runtime.InteropServices.ComTypes;
using Core.UI;

namespace Core.UI
{
    public partial class FormInspectionData : Form
    {
        private MainForm MainForm = null;
        private FormResultHistory ResultHistoryForm = null;
        private Point HistoryFormLocation;

        private int currentPage = 1;
        private int totalPage = 1;
        private int m_dbFileIndex = 0;
        private int m_dbLimit = 100;
        private int pageSize = 100;

        private bool b_SearchClick = false;

        public bool bFirstCheck = false;
        public bool bListBoxClick = false;
        public bool bInspectionImagefound = false;
        public bool SellClick = false;
        public string PolygonData = string.Empty;
        public string[] PolygonValue;

        public bool bInspectConditionActivate = false;

        public int GridselectedNo;

        public DataTable InspectionData = new DataTable();

        public List<string> dbFiles = new List<string>();
        public List<string[]> conditionDataList = new List<string[]>();

        private CImage Image;

        private CLogger Logger = null;
        private CThread Thread_SetDataGridFormDB;

        public FormInspectionData(MainForm _MainForm, FormResultHistory _resultHistoryForm)
        {
            InitializeComponent();
            TopLevel = false;
            MainForm = _MainForm;
            ResultHistoryForm = _resultHistoryForm;
            MainForm.SetEachControlResize(this);    //240801 NIS Conrol Resize

            Image = new CImage(DEF_SYSTEM.LICENSES_KEY, theRecipe.m_sCurrentEquipment);
            HistoryFormLocation = ResultHistoryForm.Get_TeachFormScreenLocation();

            StartDatePicker.Format = DateTimePickerFormat.Custom;
            StartDatePicker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            StartDatePicker.ShowUpDown = true;

            EndDatePicker.Format = DateTimePickerFormat.Custom;
            EndDatePicker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            EndDatePicker.ShowUpDown = true;



            Image.Allocate((int)5120, (int)5120, CImage.PIXEL8);

            DoLoad();

            //Set_Thead();
        }


        private void DoLoad()
        {
            
        }


        private void btSearch_Click(object sender, EventArgs e)
        {
            try
            {
                b_SearchClick = true;
                DataListBox.Items.Clear();
                m_dbFileIndex = m_dbFileIndex != 0 ? 0 : m_dbFileIndex;
                SetDataGridFormDB(m_dbFileIndex, m_dbLimit, 1);


            }
            catch
            {

            }
        }

        public void SetDataGridFormDB(int _dbFileIndex, int limit, int Page) // 240116 KCH 데이터베이스에서 검색하여 DataGridView에 표시
        {
            Task.Run(delegate ()
            {
                try
                {
                    DateTime startDate = StartDatePicker.Value;
                    DateTime endDate = EndDatePicker.Value;
                    int CountDataBaseRow = 0;
                    dbFiles.Clear();

                    if (startDate > endDate)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            MessageBox.Show("Start date must be previously than the end date.");
                        }));
                        return;
                    }

                    dbFiles = GetDatabaseFilesInRange(startDate, endDate); // DB 파일 목록 가져오기
                    int SetOffSet = (Page - 1) * pageSize;
                    currentPage = Page; //250118 KCH 현재 페이지 번호를 업데이트

                    if (m_dbFileIndex < dbFiles.Count)
                    {
                        string dbFilePath = dbFiles[m_dbFileIndex];

                        /*  임시 주석
                        InspectionData = theMainSystem.MesTrackingData.SearchDataInInspectionData(
                            dbFilePath,
                            startDate.ToString("yyyyMMddHHmmssff"),
                            endDate.ToString("yyyyMMddHHmmssff"),
                            limit,
                            SetOffSet,
                            "ProductName",
                            "InspectionData"
                        );

                        CountDataBaseRow = theMainSystem.MesTrackingData.GetTotalRowCountInDatabase(
                            dbFilePath,
                            startDate.ToString("yyyyMMddHHmmssff"),
                            endDate.ToString("yyyyMMddHHmmssff"),
                            "ProductName",
                            "InspectionData"
                        );
                        */

                        int tempTotalPage = (CountDataBaseRow + pageSize - 1) / pageSize;

                        // UI 업데이트
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            DefectDataGridView.DataSource = null;

                            if (InspectionData != null)
                            {
                                for (int i = 0; i < InspectionData.Rows.Count; i++)
                                {
                                    string WaferID = InspectionData.Rows[i]["ProductName"].ToString();
                                    if (!DataListBox.Items.Contains(WaferID))
                                    {
                                        DataListBox.Items.Add(WaferID);
                                    }
                                }
                            }

                            totalPage = tempTotalPage;
                            lbl_PageInfo.Text = $"Page {currentPage} / {totalPage}";
                        }));
                    }
                }
                catch
                {

                }

            });
        }



        private List<string> GetDatabaseFilesInRange(DateTime startDate, DateTime endDate) //240116 KCH 지정된 날짜 범위 내의 데이터베이스 파일경로 가져오기 
        {
            var dbFiles = new List<string>(); //240116 KCH 데이터베이스 파일 경로를 저장할 리스트 

            //240116 KCH 시작 날짜부터 종료 날짜까지 반복
            for (DateTime currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
            {
                //240116 KCH 현재 날짜에 해당하는 폴더 경로 생성 (년, 월, 일을 기준으로)
                string folderPath = string.Format("{0}\\{1}\\{2}\\InspectionData", DEF_SYSTEM.DEF_FOLDER_PATH_CSV
                                                             , currentDate.ToString("yyyyMM")
                                                             , currentDate.ToString("dd")
                                                             );

                //240116 KCH 폴더 경로와 파일 이름을 결합하여 파일 경로 생성               
                string filePath = folderPath + string.Format($"\\{currentDate:yyyyMMdd}_InspectionData.db");

                //240116 KCH 해당 경로에 파일이 존재하면 리스트에 추가
                if (File.Exists(filePath))
                {
                    dbFiles.Add(filePath);
                }
            }

            return dbFiles; //240116 KCH 해당 범위에 있는 모든 파일 경로 리스트 반환
        }

        private void DataListBox_MouseClick(object sender, MouseEventArgs e)
        {
            Task.Run(delegate ()
            {
                try
                {
                    DateTime startDate = StartDatePicker.Value; // 240116 KCH 시작 날짜
                    DateTime endDate = EndDatePicker.Value; // 240116 KCH 종료 날짜
                    string selectedItem = string.Empty;
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        selectedItem = DataListBox.SelectedItem?.ToString();
                    }));
                    dbFiles = GetDatabaseFilesInRange(startDate, endDate); // DB 파일 목록 가져오기

                    if (m_dbFileIndex < dbFiles.Count)
                    {
                        /*  임시 주석
                        InspectionData = theMainSystem.MesTrackingData.SelectDataInInspectionData(
                            dbFiles[m_dbFileIndex],
                            startDate.ToString("yyyyMMddHHmmssff"),
                            endDate.ToString("yyyyMMddHHmmssff"),
                            selectedItem,
                            "ProductName",
                            "InspectionData"
                        );
                        */
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            DefectDataGridView.DataSource = null;
                            DefectDataGridView.DataSource = InspectionData;

                            List<string> columnsToHide = new List<string>
                            {
                                "DefectColor_Alpha",
                                "DefectColor_Red",
                                "DefectColor_Green",
                                "DefectColor_Blue",
                                "CreateTime",
                                "Origin_ImagePath",
                                "PolygonData"
                            };

                            foreach (string columnName in columnsToHide)
                            {
                                if (DefectDataGridView.Columns.Contains(columnName))
                                {
                                    DefectDataGridView.Columns[columnName].Visible = false;
                                }
                            }
                            if (InspectionData.Rows.Count > 0)
                            {
                                HistoryImageDisplay(InspectionData.Rows[0]["Origin_ImagePath"].ToString(), InspectionData.Rows[0]["ProductName"].ToString());
                            }
                        }));
                    }
                }
                catch
                {

                }
            });
        }



        private void HistoryImageDisplay(string GetImagePath, string GetTempID) //240511 KCH Histrory Image 출력
        {
            bInspectionImagefound = true;

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++) //240625 KCH 이미지 존재여부 확인
            {
                string monoImagePath = GetImagePath + $"\\{GetTempID}_MonoR_{i}.bmp";

                bool monoImageExists = File.Exists(monoImagePath);

                if (monoImageExists)
                {
                    theMainSystem.Cameras[i].GetListImage(0, true).ImgRead(monoImagePath, i);
                }
                else
                {
                    bInspectionImagefound = false;
                    break;
                }
            }

            theMainSystem.HistoryImageDisplay(0);


        }


        private Pen DisplayArrowPen(Color color, int fontSize, bool IsBusbar)
        {
            Pen pen = new Pen(color, fontSize);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            if (IsBusbar)
                pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            else
                pen.EndCap = System.Drawing.Drawing2D.LineCap.NoAnchor;
            return pen;
        }





        private void DefectDataGridView_CellClick_1(object sender, DataGridViewCellEventArgs e) //240710 KCH DataGridView의 셀 클릭시 해당하는 Row의 데이터 추출하여 Image 위에 Paint
        {
            try
            {
                SellClick = true;
                DataGridViewRow dategvr = DefectDataGridView.CurrentRow;
                GridselectedNo = int.Parse(dategvr.Cells[1].Value.ToString());

                if (!System.IO.Directory.Exists(dategvr.Cells["Origin_ImagePath"].Value.ToString()))
                    return;
                HistoryImageDisplay(dategvr.Cells["Origin_ImagePath"].Value.ToString(), dategvr.Cells["ProductName"].Value.ToString());
                SellClick = false;
            }
            catch
            {

            }


        }



        private void btnBeforePage_Click(object sender, EventArgs e) //250118 KCH 이전 페이지 버튼 클릭
        {
            if (currentPage > 1)
            {
                DataListBox.Items.Clear();
                SetDataGridFormDB(m_dbFileIndex, m_dbLimit, currentPage - 1);
            }
            if (currentPage == 1 && m_dbFileIndex > 0)
            {
                DataListBox.Items.Clear();
                m_dbFileIndex--;
                currentPage = 0;
                SetDataGridFormDB(m_dbFileIndex, m_dbLimit, currentPage + 1);
            }
        }

        private void btNextPage_Click(object sender, EventArgs e) //250118 KCH 다음 페이지 버튼 클릭
        {
            if (currentPage < totalPage)
            {
                DataListBox.Items.Clear();

                SetDataGridFormDB(m_dbFileIndex, m_dbLimit, currentPage + 1);
            }

            if (currentPage == totalPage && m_dbFileIndex < dbFiles.Count - 1)
            {
                DataListBox.Items.Clear();

                m_dbFileIndex++;
                currentPage = 0;
                SetDataGridFormDB(m_dbFileIndex, m_dbLimit, currentPage + 1);
            }
        }

        private void btNext10Page_Click(object sender, EventArgs e) //250118 KCH 10페이지 앞으로 이동 버튼 클릭
        {
            if (currentPage + 10 <= totalPage) //250118 KCH 현재 페이지에서 10페이지를 더한 값이 전체 페이지보다 작거나 같으면
            {
                DataListBox.Items.Clear();
                SetDataGridFormDB(m_dbFileIndex, m_dbLimit, currentPage + 10); //250118 KCH 10페이지 앞으로 이동
            }
            else
            {
                DataListBox.Items.Clear();
                m_dbFileIndex++;
                currentPage = 0;
                SetDataGridFormDB(m_dbFileIndex, m_dbLimit, totalPage); //250118 KCH 마지막 페이지로 이동
            }
        }

        private void btBefore10page_Click(object sender, EventArgs e) //250118 KCH 10페이지 뒤로 이동 버튼 클릭
        {
            if (currentPage - 10 >= 1) //250118 KCH 현재 페이지에서 10페이지를 뺀 값이 1보다 크거나 같으면
            {
                DataListBox.Items.Clear();

                SetDataGridFormDB(m_dbFileIndex, m_dbLimit, currentPage - 10); //250118 KCH 10페이지 뒤로 이동
            }
            else
            {
                DataListBox.Items.Clear();

                SetDataGridFormDB(m_dbFileIndex, m_dbLimit, 1); //250118 KCH 첫 번째 페이지로 이동
            }
        }

        private void btSetLastPage_Click(object sender, EventArgs e) //250118 KCH 마지막 페이지 버튼 클릭
        {
            DataListBox.Items.Clear();
            SetDataGridFormDB(m_dbFileIndex, m_dbLimit, totalPage); //250118 KCH 마지막 페이지로 이동
        }

        private void btSetfirstPage_Click(object sender, EventArgs e) //250118 KCH 첫 번째 페이지 버튼 클릭
        {
            DataListBox.Items.Clear();
            SetDataGridFormDB(m_dbFileIndex, m_dbLimit, 1); //250118 KCH 첫 번째 페이지로 이동
        }

    }
}
