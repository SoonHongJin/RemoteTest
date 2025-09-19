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
using ConnectedInsightCore;
using static Core.Program;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ScottPlot.Renderable;

namespace Core.UI
{
    public partial class FormCurrentHistoryData : Form
    {  
        private MainForm MainForm = null;
        private string m_sDate = null;
        private string m_sWaferID = null;

        private string m_sColorInsptTime = string.Empty;
        private string m_sColorThickness = string.Empty;
        private string m_sColorGrade = string.Empty;
        private string m_sFinaljudge = string.Empty;
        public List<CCurrentHistoryImageControl> Frames = new List<CCurrentHistoryImageControl>();

        public bool m_bImagefoundCurrent = true;

        private string SelectedDefect = "All"; //240708 KCH 처음에는 전체 Display

        private CImage Image;
        private DataTable InspectionData = new DataTable();

        public string PolygonData = string.Empty;
        public string[] PolygonValue;

        private double EdgeToBusbarOffSet = 1000;
        private double EdgeToBusbarOffSet2 = 500;

        private Font FontSize100 = new Font(FontFamily.GenericSansSerif, 100);
        private Font FontSize180 = new Font(FontFamily.GenericSansSerif, 180);

        private SolidBrush RedBrush = new SolidBrush(Color.Red);
        private CLogger Logger = null;

        public FormCurrentHistoryData(MainForm _MainForm, CLogger _Logger)
        {
            InitializeComponent();
            Image = new CImage(DEF_SYSTEM.LICENSES_KEY, theRecipe.m_sCurrentEquipment);
            TopLevel = false;
            MainForm = _MainForm;
            Logger = _Logger;
            MainForm.SetEachControlResize(this);    //240801 NIS Conrol Resize

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; ++i)
            {
                Frames.Add(new CCurrentHistoryImageControl(MainForm)
                {
                    Id = i,
                    m_ImageModeCurrent = i
                });

                MainDisplayPanel.Controls.Add(Frames[i]);
                Frames[i].SetCamera(theMainSystem.Cameras[i], this);
            }
            

            Image.Allocate((int)DEF_SYSTEM.IMAGE_WIDTH, (int)DEF_SYSTEM.IMAGE_HEIGHT, CImage.PIXEL8);
            SetClassifyTrackingList();
            FormLocate(0);
        }

        /// <summary>
        /// 24.02.09 LYK FormLocate
        /// Panel 안에 UserControl을 배치 시키는 함수
        /// </summary>
        /// <param name="_type"></param>

        private void FormLocate(int _type)  //240621 LYK 수정
        {
            //Maximized = !Maximized;
            Rectangle fullSize = new Rectangle(0, 0, MainDisplayPanel.Width, MainDisplayPanel.Height);//FullPanel.Bounds;

            //int width = MainDisplayPanel.Width;

            int width = MainDisplayPanel.Width;
            int height = MainDisplayPanel.Height;
            Rectangle[] bounds = new Rectangle[DEF_SYSTEM.CAM_MAX_COUNT];

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; ++i) //240621 LYK 수정
            {
                bounds[i].X = 0;
                bounds[i].Y = 0;
                bounds[i].Width = MainDisplayPanel.Width;
                bounds[i].Height = MainDisplayPanel.Width;


                // Mono Display 분할 
                Frames[i].Bounds = bounds[i];
                Frames[i].Show();

            }
        }

        private void SetClassifyTrackingList() //250117 KCH Tracking Name을 Label에 Display하기 위해 분류
        {

            SetNameLabel(); //250117 KCH 분류 후 라벨 설정
        }



        private void SetNameLabel() //250117 KCH Label에 분류별로 Text 추가 
        {
            /*
            for (int i = 0; i < InfoList.Count; i++)
            {
                this.Controls.Find($"lbl_InfoName{i}", true)[0].Text = InfoList[i].Name;
                this.Controls.Find($"lbl_InfoName{i}", true)[0].Font = new Font("Galano Grotesque DEMO", 11F, FontStyle.Bold);
                this.Controls.Find($"lbl_InfoResult{i}", true)[0].Text = "";
                this.Controls.Find($"lbl_InfoResult{i}", true)[0].Font = new Font("Galano Grotesque DEMO", 11F, FontStyle.Regular);
            }
            for (int i = 0; i < ClassList.Count; i++)
            {
                this.Controls.Find($"lbl_ClassName{i}", true)[0].Text = ClassList[i].Name;
                this.Controls.Find($"lbl_ClassName{i}", true)[0].Font = new Font("Galano Grotesque DEMO", 11F, FontStyle.Bold);
                this.Controls.Find($"lbl_ClassResult{i}", true)[0].Text = "";
                this.Controls.Find($"lbl_InfoResult{i}", true)[0].Font = new Font("Galano Grotesque DEMO", 11F, FontStyle.Regular);
            }
            for (int i = 0; i < GeometryList.Count; i++)
            {
                this.Controls.Find($"lbl_GeoName{i}", true)[0].Text = GeometryList[i].Name;
                this.Controls.Find($"lbl_GeoName{i}", true)[0].Font = new Font("Galano Grotesque DEMO", 11F, FontStyle.Bold);
                this.Controls.Find($"lbl_GeoResult{i}", true)[0].Text = "";
                this.Controls.Find($"lbl_GeoResult{i}", true)[0].Font = new Font("Galano Grotesque DEMO", 11F, FontStyle.Regular);
            }
            for (int i = 0; i < ThicknessList.Count; i++)
            {
                this.Controls.Find($"lbl_ThickName{i}", true)[0].Text = ThicknessList[i].Name;
                this.Controls.Find($"lbl_ThickName{i}", true)[0].Font = new Font("Galano Grotesque DEMO", 11F, FontStyle.Bold);
                this.Controls.Find($"lbl_ThickResult{i}", true)[0].Text = "";
                this.Controls.Find($"lbl_ThickResult{i}", true)[0].Font = new Font("Galano Grotesque DEMO", 11F, FontStyle.Regular);
            }
            for (int i = 0; i < DefectList.Count; i++)
            {
                this.Controls.Find($"lbl_DefectName{i}", true)[0].Text = DefectList[i].Name;
                this.Controls.Find($"lbl_DefectName{i}", true)[0].Font = new Font("Galano Grotesque DEMO", 11F, FontStyle.Bold);
                this.Controls.Find($"lbl_DefectResult{i}", true)[0].Text = "";
                this.Controls.Find($"lbl_DefectResult{i}", true)[0].Font = new Font("Galano Grotesque DEMO", 11F, FontStyle.Regular);
            }
            */
        }
        public void SetDefectData(string _Date, string _WaferID)
        {
            m_sDate = _Date;
            m_sWaferID = _WaferID;

            cb_DefectClass.Items.Clear(); 
            cb_DefectClass.Items.Add("All");
            SelectedDefect = "All";

            string sYearMonth = _Date.Substring(0, 6);
            string sDay = _Date.Substring(6, 2);

            string sInspectionDBPath = $"{DEF_SYSTEM.DEF_FOLDER_PATH_CSV}\\{sYearMonth}\\{sDay}\\InspectionData\\{m_sDate}_InspectionData.db";
            string sImagePath = ConvertContourPath(sInspectionDBPath, m_sWaferID);

            ImageDataRead(sImagePath, _WaferID);

            lbl_WaferID.Text = m_sWaferID;
            GetInspectionData(sInspectionDBPath, m_sWaferID);

            cb_DefectClass.SelectedIndex = 0;

            theMainSystem.HistoryImageDisplay(1);
        }

        private string ConvertContourPath(string contourPath, string waferID)
        {
            // 250202 KCH 5번째와 6번째 폴더 결합 (예: "202502" + "02" => "20250202")
            string[] pathParts = contourPath.Split('\\');
            if (pathParts.Length < 6) return null; // 250202 KCH 경로가 너무 짧으면 종료

            string dateFolder = pathParts[4] + pathParts[5];  // 250202 KCH "202502" + "02" -> "20250202"

            // 250202 KCH 새로운 기본 경로 설정
            string basePath = $@"D:\ConnectedInsight\Hanhwa_QCell\01. Running Images\{dateFolder}";

            // 250202 KCH OK/NG 폴더 둘다 다 확인
            string okPath = Path.Combine(basePath, "OK", waferID);
            string ngPath = Path.Combine(basePath, "NG", waferID);

            if (Directory.Exists(okPath))
                return okPath;
            else if (Directory.Exists(ngPath))
                return ngPath;
            else
                return null; // 250202 KCH OK, NG 둘 다 없을 경우
        }

        private void GetInspectionData(string InspectionDBPath, string CellId)
        {
            InspectionData.Clear();

            if (System.IO.File.Exists(InspectionDBPath))
            {
                //InspectionData = theMainSystem.MesTrackingData.GetWaferIDData(InspectionDBPath, CellId, "InspectionData", "ProductName");
            }
            if (InspectionData != null && InspectionData.Rows.Count > 0)
            {
                foreach (DataRow row in InspectionData.Rows)
                {
                    string className = row["ClassName"].ToString(); // 적절한 컬럼 이름으로 변경하세요
                    if (!cb_DefectClass.Items.Contains(className))
                    {
                        cb_DefectClass.Items.Add(className);
                    }
                }


            }
        }

        private void ClassifySelectRow(DataTable TrackingData)
        {
            /*
            for (int i = 0; i < TrackingData.Columns.Count + 1; i++)
            {
                for (int j = 0; j < InfoList.Count; j++)
                {
                    if (InfoList[j].index == i)
                    {
                        InfoList[j].Result = TrackingData.Rows[0][i + 1].ToString(); 
                    }
                }
                for (int j = 0; j < ClassList.Count; j++)
                {
                    if (ClassList[j].index == i)
                    {
                        ClassList[j].Result = TrackingData.Rows[0][i + 1].ToString();
                    }
                }
                for (int j = 0; j < GeometryList.Count; j++)
                {
                    if (GeometryList[j].index == i)
                    {
                        GeometryList[j].Result = TrackingData.Rows[0][i + 1].ToString();
                    }
                }
                for (int j = 0; j < ThicknessList.Count; j++)
                {
                    if (ThicknessList[j].index == i)
                    {
                        ThicknessList[j].Result = TrackingData.Rows[0][i + 1].ToString();
                    }
                }
                for (int j = 0; j < DefectList.Count; j++)
                {
                    if (DefectList[j].index == i)
                    {
                        DefectList[j].Result = TrackingData.Rows[0][i + 1].ToString();
                    }
                }
            }
            */
        }

        private void SetResultlabel() //250118 KCH 선택한 Result를 분류에 맞게 Label에 Display
        {
            /*
            for (int i = 0; i < DefectList.Count; i++)
            {
                this.Controls.Find($"lbl_DefectResult{i}", true)[0].Text = DefectList[i].Result;
            }
            */
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
 
        public void DoPaint(Graphics graphics, int nIdx)
        {
            try
            {
                //theMainSystem.Cameras[nIdx].ListHisotryImages[DEF_SYSTEM.MONO_R].Draw(graphics); //240710 KCH 이미지 Paint
                int index = 0;
                for (int i = 0; i < InspectionData.Rows.Count; i++)
                {
                    if (InspectionData.Rows[i]["Camera"].ToString() == nIdx.ToString())
                    {
                        PolygonData = InspectionData.Rows[i]["PolygonData"].ToString();
                        PolygonValue = PolygonData.Split(',');
                        Color color = Color.FromArgb(int.Parse(InspectionData.Rows[i]["DefectColor_Alpha"].ToString()),
                                                     int.Parse(InspectionData.Rows[i]["DefectColor_Red"].ToString()),
                                                     int.Parse(InspectionData.Rows[i]["DefectColor_Green"].ToString()),
                                                     int.Parse(InspectionData.Rows[i]["DefectColor_Blue"].ToString()));

                        int nCnt = 0;

                        PointF[] InnverValue = new PointF[(PolygonValue.Length) / 2];

                        for (int j = 0; j < PolygonValue.Length; j += 2)
                        {
                            InnverValue[nCnt].X = float.Parse(PolygonValue[j]);
                            InnverValue[nCnt].Y = float.Parse(PolygonValue[j + 1]);
                            nCnt++;
                        }

                        graphics.DrawPolygon(new Pen(color, 1), InnverValue);
                        SolidBrush brush = new SolidBrush(Color.Red);
                        graphics.DrawString($"Width : {InspectionData.Rows[i]["Width"]}mm\nHeight : {InspectionData.Rows[i]["Height"]}mm\nArea : {InspectionData.Rows[i]["Area"]}",
                            new Font(FontFamily.GenericSansSerif, 7), brush, (float)double.Parse(InspectionData.Rows[i]["PosX"].ToString()), (float)double.Parse(InspectionData.Rows[i]["PosY"].ToString()));

                    }
                }
            }
            catch (Exception e)
            {
                int test = 0;
            }
        }

        public void DoDefectCurrentPaint(Graphics graphics, int nIdx)
        {
            try
            {
                if (!m_bImagefoundCurrent) //240708 KCH Image없을 시 Display초기화
                {
                    this.Image.Draw(graphics);
                }
                else
                {
                    theMainSystem.Cameras[nIdx].GetListImage(0, true).Draw(graphics);

                    if (SelectedDefect == "All") //240708 KCH 콤보박스 "All"선택 시 전체 Defect 출력
                    {
                        graphics.DrawString($"{InspectionData.Rows[0]["ClassName"].ToString()}", new Font(FontFamily.GenericSansSerif, 500), new SolidBrush(Color.Red), 0, 100);
                    }
                    else //240708 KCH 콤보박스 선택 시 해당 Defect 출력
                    {
                        DoPaintSelected(graphics, nIdx, SelectedDefect);
                    }
                }


            }
            catch (Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Defect Display Erro Catch : {e.Message}");
            }

        }

        public void DoPaintSelected(Graphics graphics, int nIdx, string SelectedDefect)
        {
            try
            {
                theMainSystem.Cameras[nIdx].GetListImage(0, true).Draw(graphics); //240710 KCH 이미지 Paint
                int index = 0;
                for (int i = 0; i < InspectionData.Rows.Count; i++)
                {
                    if (InspectionData.Rows[i]["Camera"].ToString() == nIdx.ToString() && InspectionData.Rows[i]["ClassName"].ToString() == SelectedDefect)
                    {
                        PolygonData = InspectionData.Rows[i]["PolygonData"].ToString();
                        PolygonValue = PolygonData.Split(',');
                        Color color = Color.FromArgb(int.Parse(InspectionData.Rows[i]["DefectColor_Alpha"].ToString()),
                                                     int.Parse(InspectionData.Rows[i]["DefectColor_Red"].ToString()),
                                                     int.Parse(InspectionData.Rows[i]["DefectColor_Green"].ToString()),
                                                     int.Parse(InspectionData.Rows[i]["DefectColor_Blue"].ToString()));

                        int nCnt = 0;

                        PointF[] InnverValue = new PointF[(PolygonValue.Length) / 2];

                        for (int j = 0; j < PolygonValue.Length; j += 2)
                        {
                            InnverValue[nCnt].X = float.Parse(PolygonValue[j]);
                            InnverValue[nCnt].Y = float.Parse(PolygonValue[j + 1]);
                            nCnt++;
                        }

                        graphics.DrawPolygon(new Pen(color, 1), InnverValue);
                        SolidBrush brush = new SolidBrush(Color.Red);
                        graphics.DrawString($"Width : {InspectionData.Rows[i]["Width"]}mm\nHeight : {InspectionData.Rows[i]["Height"]}mm\nArea : {InspectionData.Rows[i]["Area"]}",
                            new Font(FontFamily.GenericSansSerif, 7), brush, (float)double.Parse(InspectionData.Rows[i]["PosX"].ToString()), (float)double.Parse(InspectionData.Rows[i]["PosY"].ToString()));

                    }
                }
            }
            catch (Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Current History Data Page Catch : {e.Message}");
            }
        }

        private void ImageDataRead(string _sImagePath, string _sWaferID)
        {
            int ListImageIndex = 0;

            

            m_bImagefoundCurrent = true;

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; i++) //240625 KCH 이미지 존재여부
            {

                string monoImagePath = _sImagePath +  $"\\{_sWaferID}_MonoR_{i}.jpg";
                string colorImagePath = _sImagePath + $"\\{_sWaferID}_Color_{i}.jpg";


                monoImagePath.Trim();
                colorImagePath.Trim();

                bool monoImageExists = File.Exists(monoImagePath);
                bool colorImageExists = File.Exists(colorImagePath);

                if (monoImageExists)
                {
                    theMainSystem.Cameras[i].GetListImage(0, true).ImgRead(monoImagePath, i);
                }
                else
                {
                    m_bImagefoundCurrent = false;
                    break;
                }
            }
        }

        

        private void cb_DefectClass_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            SelectedDefect = cb_DefectClass.SelectedItem.ToString();

            //theMainSystem.HistoryImageDisplay(1);
        }

        private void btn_MonoImage_Click(object sender, EventArgs e)
        {
            tab_Display.SelectedIndex = 0;
        }

        private void btn_ColorImage_Click(object sender, EventArgs e)
        {
            tab_Display.SelectedIndex = 1;
        }
    }
}
