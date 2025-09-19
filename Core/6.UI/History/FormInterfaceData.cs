using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

using Core.Process;
using Core.Utility;
using static Core.Program;
using MetaLog;

namespace Core.UI
{
    public partial class FormInterfaceData : Form
    {
        private MainForm _MainForm = null;

        private List<string> allFiles = new List<string>();
        private const int PageSize = 100; //한페이지당 파일 수
        private int CurrentPage = 0;      //현재 페이지 번호
        private ChartArea chartArea;
        private DateTime yEndTime;
        private Dictionary<string, List<LogEntry>> logDataByCommand = new Dictionary<string, List<LogEntry>>();
        private Nullable<DateTime> minTime = null;
        private Nullable<DateTime> maxTime = null;

        private CLogger Logger = null;

        public FormInterfaceData(MainForm _Main, CLogger _logger)
        {
            _MainForm = _Main;
            Logger = _logger;

            InitializeComponent();
            TopLevel = false;
            _MainForm.SetEachControlResize(this);
            LoadDateList();
            SetDataList();
            InitializeChart();
        }

        public class LogEntry
        {
            public DateTime Time { get; set; }
            public string Action { get; set; }
            public string Data { get; set; }

            public LogEntry(DateTime time, string action, string data)
            {
                Time = time;
                Action = action;
                Data = data;
            }
        }

        private void InitializeChart()
        {
            // 240830 KCH Chart 초기화
            InterfaceChart.Series.Clear();
            InterfaceChart.ChartAreas.Clear();

            // 240830 KCH ChartArea 설정
            chartArea = new ChartArea();
            InterfaceChart.ChartAreas.Add(chartArea);

            // 240830 KCH X축 설정
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.ScrollBar.Enabled = true;
            chartArea.AxisX.ScrollBar.Size = 12;
            chartArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chartArea.AxisX.ScrollBar.IsPositionedInside = false;
            chartArea.AxisX.ScaleView.Zoomable = true;
            chartArea.AxisX.ScaleView.Size = 10;
            chartArea.AxisX.ScaleView.MinSize = 1;

            // 240830 KCH Command 라벨 목록 설정
            for (int i = 0; i < DEF_SYSTEM.Chartlabels.Length; i++)
            {
                CustomLabel label = new CustomLabel(i + 0.5, i + 1.5, DEF_SYSTEM.Chartlabels[i], 0, LabelMarkStyle.None);
                chartArea.AxisX.CustomLabels.Add(label);
            }

            double xMax = DEF_SYSTEM.Chartlabels.Length + 1;
            chartArea.AxisX.Maximum = xMax;
            chartArea.AxisX.ScaleView.Position = xMax - chartArea.AxisX.ScaleView.Size;

            // 240830 KCH Y축 설정 (DateTime 형식)
            chartArea.AxisY.IntervalType = DateTimeIntervalType.Seconds;
            chartArea.AxisY.Interval = 0.05;
            chartArea.AxisY.LabelStyle.Format = "HH:mm:ss.ffff";

            Series series = new Series // 240830 KCH 차트 초기화
            {
                Name = "-", 
                ChartType = SeriesChartType.RangeBar,
                YValuesPerPoint = 2,
                Color = System.Drawing.Color.DodgerBlue,
                BorderColor = System.Drawing.Color.Black,
                BorderWidth = 1,
                Font = new System.Drawing.Font("Arial", 7)
            };
            for (int i = 0; i < DEF_SYSTEM.Chartlabels.Length; i++)
            {
                series.Points.Add(new DataPoint(i + 1, double.NaN) { Label = string.Empty });
            }
            InterfaceChart.Series.Add(series);

        }

        private void GetInterfaceLogData(string sPath, string WaferId)
        {
            logDataByCommand.Clear(); // 240830 KCH 기존 데이터를 초기화

            string RowValue = string.Empty;
            string[] CellValue;

            minTime = null;
            maxTime = null;

            if (System.IO.File.Exists(sPath))
            {
                StreamReader Reader = new StreamReader(sPath);

                while ((RowValue = Reader.ReadLine()) != null)
                {
                    CellValue = RowValue.Split(',');
                    if (CellValue[0] != "")
                    {
                        string[] PartValue = new string[4];

                        // 240830 KCH 시간 정보 
                        string[] timePart = CellValue[0].Split(':');
                        string time = timePart[1] + ":" + timePart[2] + ":" + timePart[3];

                        // 240830 KCH Action, Command, Data 부분 
                        string action = CellValue[1].Split(':')[1].Trim();
                        string command = CellValue[2].Split(':')[1].Trim();
                        string data = CellValue[3].Split(':')[1].Trim();

                        DateTime logTime = DateTime.ParseExact(time, "HH:mm:ss.ffff", null); // 240830 KCH 시간 변환

                        // 240830 KCH 최소 및 최대 시간 업데이트
                        if (minTime == null || logTime < minTime)
                        {
                            minTime = logTime;
                        }
                        if (maxTime == null || logTime > maxTime)
                        {
                            maxTime = logTime;
                        }

                        // 240830 KCH Command가 이미 있는지 확인 후 Dictionary에 추가
                        if (!logDataByCommand.ContainsKey(command))
                        {
                            logDataByCommand[command] = new List<LogEntry>();
                        }

                        // 240830 KCH 해당 Command에 로그 항목 추가
                        logDataByCommand[command].Add(new LogEntry(logTime, action, data));
                    }
                }
            }

            SetData();
        }




        private void SetData()
        {
            DateTime yStartTime = minTime.Value - TimeSpan.FromSeconds(0.02);  //240831 KCH 차트의 y범위 시작시간
            DateTime yEndTime = maxTime.Value.Add(TimeSpan.FromSeconds(0.05)); //240831 KCH 차트의 y범위 끝시간

            InterfaceChart.ChartAreas[0].AxisY.Minimum = yStartTime.ToOADate(); //240831 KCH 차트의 y범위 시작설정
            InterfaceChart.ChartAreas[0].AxisY.Maximum = yEndTime.ToOADate();   //240831 KCH 차트의 y범위 끝  설정

            InterfaceChart.Series.Clear();

            // 240830 KCH Series 생성
            Series series = new Series
            {
                Name = logDataByCommand.ContainsKey("WaferID") ? $"WaferID: {logDataByCommand["WaferID"][0].Data}" : "-", //240831 KCH WaferID가 없을때 Series.Name -> "-" 설정
                ChartType = SeriesChartType.RangeBar,
                YValuesPerPoint = 2,
                Color = System.Drawing.Color.DodgerBlue,
                BorderColor = System.Drawing.Color.Black,
                BorderWidth = 1,
                Font = new System.Drawing.Font("Arial", 7)
            };

            series["BarHeight"] = "200";      // 240831 KCH 차트의 Bar두깨 설정
            series["LabelStyle"] = "Left";    // 240901 KCH 라벨을 막대 안 Left 에 위치
            series["BarLabelStyle"] = "Left"; // 240901 KCH 막대 안에 라벨을 위치시키는 추가 설정

            series.SmartLabelStyle.Enabled = false; // 240901 KCH SmartLabelStyle 설정을 비활성화하여 라벨이 막대 밖으로 빠져나가는 것을 방지

            // 240830 KCH labels 배열의 길이만큼 반복
            for (int i = 0; i < DEF_SYSTEM.Chartlabels.Length; i++)
            {
                string command = DEF_SYSTEM.Chartlabels[i];

                if (logDataByCommand.ContainsKey(command))
                {
                    List<LogEntry> logs = logDataByCommand[command];
                    Nullable<DateTime> startTime = null;

                    DateTime waferStartTime = logs[0].Time; // 240830 KCH 첫 번째 WaferID 로그의 시간
                    DateTime waferEndTime = logDataByCommand["GRS Back"].Last().Time; // 240830 KCH GRS Back, 255 로그의 시간

                    // 240830 KCH WaferID 설정을 별도로 처리
                    if (command == "WaferID")
                    {
                        series.Points.Add(new DataPoint(i + 1, new double[] { waferStartTime.ToOADate(), waferEndTime.ToOADate() })
                        {
                            Label = $"WaferID: {logs[0].Data}" // 240830 KCH 첫 번째 WaferID 로그의 Data 사용
                        });
                        continue; // 240830 KCH WaferID는 처리 후 다음 으로 이동
                    }

                    for (int j = 0; j < logs.Count; j++)
                    {
                        DateTime logTime = logs[j].Time;
                        string currentData = logs[j].Data;

                        // 240830 KCH 'startTime'을 설정
                        if (startTime == null)
                        {
                            startTime = logTime;
                        }

                        DateTime endTime;
                        if (j == logs.Count - 1)
                        {
                            if (command == "TRG" || command == "TRG Back" || command == "GRS" || command == "GRS Back")
                            {
                                endTime = waferEndTime; // 240830 KCH 마지막 로그의 경우 waferEndTime 종료
                            }
                            else
                            {
                                endTime = yEndTime; // 240830 KCH 마지막 로그의 경우 yEndTime으로 종료
                            }
                        }
                        else
                        {
                            endTime = logs[j + 1].Time; // 240830 KCH 다음 로그의 시간으로 종료 시간 설정
                        }

                        TimeSpan timeSpan = endTime - startTime.Value; // 240831 KCH 신호가 들어와있는 시간 측정
                       

                        series.Points.Add(new DataPoint(i + 1, new double[] { startTime.Value.ToOADate(), endTime.ToOADate() })
                        {
                            Label = $"Data:{currentData}\nTime:{timeSpan.TotalMilliseconds}ms"
                        });

                        // 240830 KCH 'startTime'을 현재 로그의 종료 시간으로 업데이트
                        startTime = endTime;
                    }
                }
                else
                {
                    // 240830 KCH Command가 없을 경우 차트에서 항목은 추가하지만 Bar는 그리지 않기위해 NaN으로 처리
                    series.Points.Add(new DataPoint(i + 1, double.NaN) { Label = string.Empty });
                }
            }

            InterfaceChart.Series.Add(series);
        }

        private void InterfaceChart_MouseClick(object sender, MouseEventArgs e)
        {
            //240830 KCH HitTestResult로 차트에서 클릭된 요소를 확인
            HitTestResult result = InterfaceChart.HitTest(e.X, e.Y);

            //240830 KCH 클릭된  부분이 DataPoint인 경우 에만 진행
            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                //240830 KCH 클릭된 포인트가 속한 시리즈
                var point = InterfaceChart.Series[result.Series.Name].Points[result.PointIndex];

                double yValueStart = point.YValues[0];
                double yValueEnd = point.YValues[1];

                DateTime startTime = DateTime.FromOADate(point.YValues[0]);
                DateTime endTime = DateTime.FromOADate(point.YValues[1]);
                TimeSpan timeSpan = endTime - startTime;

                txtStartTime.Text = startTime.ToString("HH:mm:ss.ffff");
                txtEndTime.Text = endTime.ToString("HH:mm:ss.ffff");
                txtElapsedTime.Text = $"{timeSpan.TotalMilliseconds}" + "ms";
            }
        }

        public void LoadDateList() //240511 KCH
        {
            try
            {
                CBO_SELECT_MONTH.Items.Clear();

                string sFolderName = DEF_SYSTEM.DEF_FOLDER_PATH_INTERFACELOG;
                DirectoryInfo DirInfo = new DirectoryInfo(sFolderName);

                foreach (DirectoryInfo File in DirInfo.GetDirectories())
                {
                    CBO_SELECT_MONTH.Items.Add(File.Name);
                }
            }
            catch (Exception ex)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Form Interface Data Catch Error : {ex.Message}");
            }
            
        }

        private void CBO_SELECT_MONTH_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            CBO_SELECT_DATE.Items.Clear();

            string sSelectFolder = CBO_SELECT_MONTH.SelectedItem.ToString();
            DirectoryInfo DirInfo = new DirectoryInfo(DEF_SYSTEM.DEF_FOLDER_PATH_INTERFACELOG + $"\\{sSelectFolder}");

            foreach (DirectoryInfo File in DirInfo.GetDirectories())
            {
                CBO_SELECT_DATE.Items.Add(File.Name);
            }
        }

        private void CBO_SELECT_DATE_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            CurrentPage = 0;
            LoadFiles();
            DisplayPage();
        }
        private void LoadFiles()
        {
            DataListBox.Items.Clear();

            string sSelectFolder = CBO_SELECT_MONTH.SelectedItem.ToString() + "\\" + CBO_SELECT_DATE.SelectedItem.ToString();
            DirectoryInfo DirInfo = new DirectoryInfo(DEF_SYSTEM.DEF_FOLDER_PATH_INTERFACELOG + $"\\{sSelectFolder}");

            if (DirInfo.Exists)
            {
                allFiles = Directory.GetFiles(DEF_SYSTEM.DEF_FOLDER_PATH_INTERFACELOG + $"\\{sSelectFolder}", "*.*", SearchOption.AllDirectories).ToList();
            }
            else
            {
                allFiles.Clear();
            }
        }

        private void UpdateCurrentPageLebel()       
        {
            int TotalPage = ((allFiles.Count % PageSize) > 0) ? (allFiles.Count / PageSize) + 1 : (allFiles.Count / PageSize);
            lbCurrentPage.Text = $"{CurrentPage + 1} of {TotalPage}";
        }

        private void DisplayPage()      
        {
            DataListBox.Items.Clear();
            int start = CurrentPage * PageSize;
            int end = start + PageSize > allFiles.Count ? allFiles.Count : start + PageSize;
            for (int i = start; i < end; i++)
            {
                string[] DateSplit = allFiles[i].Split('\\');

                DataListBox.Items.Add(DateSplit[6].Split('_')[0]);

            }
            UpdateCurrentPageLebel();
            int TotalPage = ((allFiles.Count % PageSize) > 0) ? (allFiles.Count / PageSize) + 1 : (allFiles.Count / PageSize);
            btnBeforePage.Enabled = CurrentPage > 0 ? true : false;
            btBefore10page.Enabled = CurrentPage == 0 ? false : true;
            btNextPage.Enabled = (CurrentPage + 1) < TotalPage ? true : false;
            btNext10Page.Enabled = (CurrentPage + 10) < TotalPage ? true : false;
            btSetfirstPage.Enabled = CurrentPage == 0 ? false : true;
            btSetLastPage.Enabled = (CurrentPage + 1) < TotalPage ? true : false;
        }

        private void btSearch_Click_1(object sender, EventArgs e)
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
                for (int i = 0; i < allFiles.Count; i++)
                {
                    string DateSplit = allFiles[i].Split(Path.DirectorySeparatorChar)[7].Split('_')[1]; 

                    if (DateSplit.Contains(tbSearchText.Text))
                        DataListBox.Items.Add(DateSplit);

                }
            }
            catch
            {

            }
        }

        private void SetDefectData(string _Date, string _WaferID)
        {
            string sSelectFolder = CBO_SELECT_MONTH.SelectedItem.ToString() + "\\" + CBO_SELECT_DATE.SelectedItem.ToString();
            string sInterfaceLogPath = $"{DEF_SYSTEM.DEF_FOLDER_PATH_INTERFACELOG}\\{sSelectFolder}\\{_WaferID}_RunTimeLogInterface.txt";

            GetInterfaceLogData(sInterfaceLogPath, _WaferID);
        }

        private void btSetfirstPage_Click_1(object sender, EventArgs e)
        {
            CurrentPage = 0;
            DisplayPage();
        }

        private void btSetLastPage_Click_1(object sender, EventArgs e)
        {
            int TotalPage = ((allFiles.Count % PageSize) > 0) ? (allFiles.Count / PageSize) + 1 : (allFiles.Count / PageSize);
            CurrentPage = TotalPage - 1;
            DisplayPage();
        }

        private void DataListBox_MouseClick_1(object sender, MouseEventArgs e)
        {
            try
            {
                string m_sDate = CBO_SELECT_MONTH.SelectedItem.ToString() + CBO_SELECT_DATE.SelectedItem.ToString();
                SetDefectData(m_sDate, DataListBox.SelectedItem.ToString());
            }
            catch 
            {
            }
        }

        private void btBefore10page_Click_1(object sender, EventArgs e)
        {
            CurrentPage = (CurrentPage - 10 < 0) ? 0 : CurrentPage - 10;
            DisplayPage();
        }

        private void btnBeforePage_Click_1(object sender, EventArgs e)
        {
            CurrentPage--;
            DisplayPage();
        }

        private void btNextPage_Click_1(object sender, EventArgs e)
        {
            CurrentPage++;
            DisplayPage();
        }

        private void btNext10Page_Click_1(object sender, EventArgs e)
        {
            int TotalPage = ((allFiles.Count % PageSize) > 0) ? (allFiles.Count / PageSize) + 1 : (allFiles.Count / PageSize);
            CurrentPage = (CurrentPage + 10 >= TotalPage) ? TotalPage : CurrentPage + 10;
            DisplayPage();
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
