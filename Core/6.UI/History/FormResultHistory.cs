using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Core.Utility;
using Core.Function;

using static Core.Program;

using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.Implementation;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Dimensioning;
using System.Drawing.Drawing2D;

using Insnex.Vision2D.Core;
using Insnex.Vision2D.Common;
using Insnex.Vision2D.ToolBlock;
using Insnex.Vision2D.ImageProcessing;
using Insnex.Vision2D.Finder;
using Insnex.Vision2D.Intersection;
using Insnex.Vision2D.CalibFix;
using Insnex.Vision2D.Measurement;
using System.Security.Principal;


namespace Core.UI
{
    public partial class FormResultHistory : Form
    {

        private MainForm MainForm = null;

        public FormInspectionData InspectionDataForm;
        public string PolygonData = string.Empty;
        public string[] PolygonValue;

        private List<Form> HistoryFormList = new List<Form>();             //240721 NIS TeachForm List
        private List<Button> HistoryButtonList = new List<Button>();          //240721 NIS TeachFormButton List

        public List<CHistoryImageControl> Frames = new List<CHistoryImageControl>();

        private CImage Image;// = new CImage(DEF_SYSTEM.LICENSES_KEY);

        private List<ICogTool> m_TeachingToolList = new List<ICogTool>();           // 240514 SHJ Teaching CogTool List 
        private List<ICogImage> m_TeachingImageList = new List<ICogImage>();


        private const int BTN_LINE_THICKNESS = 5;      //240805 NIS Set the button in bold

        private Font FontSize100 = new Font(FontFamily.GenericSansSerif, 100);
        private Font FontSize180 = new Font(FontFamily.GenericSansSerif, 180);

        private SolidBrush RedBrush = new SolidBrush(Color.Red);
        CLogger Logger = null;

        public FormResultHistory(MainForm _MainForm, CLogger logger)
        {
            InitializeComponent();
            TopLevel = false;
            MainForm = _MainForm;
            Logger = logger;

            Image = new CImage(DEF_SYSTEM.LICENSES_KEY, theRecipe.m_sCurrentEquipment);
            MainForm.SetEachControlResize(this);    //240801 NIS Control Resize

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; ++i)
            {
                Frames.Add(new CHistoryImageControl(MainForm)
                {
                    Id = i
                });

                DefectDisplayCam.Controls.Add(Frames[i]);

                Frames[i].SetCamera(theMainSystem.Cameras[i], this);
            }

            Image.Allocate((int)5120, (int)5120, CImage.PIXEL8);

            this.FormLocation(0);

            CreateForm();       //240720 NIS Create TeachingForms

            InspectionDataForm.Visible = true;
        }

        private void FormLocation(int _type)    //240621 LYK 수정
        {

            //Maximized = !Maximized;
            Rectangle fullSize = new Rectangle(0, 0, DefectDisplayCam.Width, DefectDisplayCam.Height);//FullPanel.Bounds;


            int width = DefectDisplayCam.Width;
            int height = DefectDisplayCam.Height;

            Rectangle[] bounds = new Rectangle[4];

            for (int i = 0; i < DEF_SYSTEM.CAM_MAX_COUNT; ++i)
            {
                if (i == 0)
                {
                    bounds[i].X = 0;
                    bounds[i].Y = 0;
                }
                else if (i == 1)
                {
                    bounds[i].X = DefectDisplayCam.Width / 2;
                    bounds[i].Y = 0;
                }
                else if (i == 2)
                {
                    bounds[i].X = 0;
                    bounds[i].Y = DefectDisplayCam.Height / 2;
                }
                else if (i == 3)
                {
                    bounds[i].X = DefectDisplayCam.Width / 2;
                    bounds[i].Y = DefectDisplayCam.Height / 2;
                }

                bounds[i].Width = width;
                bounds[i].Height = height;

                // White Display 분할 
                Frames[i].Bounds = bounds[i];
                Frames[i].Show();
                //
                //// Color Display 분할 
                //Frames[i + 4].Bounds = bounds[i];
                //Frames[i + 4].Show();
            }

        }

        private void CreateForm()       //240720 NIS Create teaching forms
        {
            InspectionDataForm = new FormInspectionData(MainForm, this)    
            {
                Parent = pnl_HistoryFormScreen,
                Size = pnl_HistoryFormScreen.Size,
                Visible = false
            };
            HistoryFormList.Add(InspectionDataForm);
            HistoryButtonList.Add(btnDisplayInspectionForm);
        }

        private void btn_DisplayEachHistoryForm(object sender, EventArgs e)    
        {
            Button btn = sender as Button;

            for (int i = 0; i < HistoryFormList.Count; i++)
            {
                HistoryFormList[i].Visible = false;
                HistoryButtonList[i].FlatAppearance.BorderSize = 1;
            }

            if (btn.Name.Contains("Inspection"))
            {
                theMainSystem.HistoryImageDisplay(0);
                InspectionDataForm.Visible = true;
                btnDisplayInspectionForm.FlatAppearance.BorderSize = BTN_LINE_THICKNESS;
                panel4.Visible = true;
                DefectDisplayCam.Visible = true;
            }
        }
        public void DoPaintInspectionData(Graphics graphics, int nIdx)
        {
            try
            {
                if (!InspectionDataForm.Visible)
                {
                    this.Image.Draw(graphics);
                }
                else
                {
                    theMainSystem.Cameras[nIdx].GetListImage(0, true).Draw(graphics); //240710 KCH 이미지 Paint
                    int index = 0;
                    for (int i = 0; i < InspectionDataForm.InspectionData.Rows.Count; i++)
                    {
                        if (InspectionDataForm.InspectionData.Rows[i]["Camera"].ToString() == nIdx.ToString())
                        {


                            PolygonData = InspectionDataForm.InspectionData.Rows[i]["PolygonData"].ToString();
                            PolygonValue = PolygonData.Split(',');
                            Color color = Color.FromArgb(int.Parse(InspectionDataForm.InspectionData.Rows[i]["DefectColor_Alpha"].ToString()),
                                                         int.Parse(InspectionDataForm.InspectionData.Rows[i]["DefectColor_Red"].ToString()),
                                                         int.Parse(InspectionDataForm.InspectionData.Rows[i]["DefectColor_Green"].ToString()),
                                                         int.Parse(InspectionDataForm.InspectionData.Rows[i]["DefectColor_Blue"].ToString()));

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
                            graphics.DrawString($"Width : {InspectionDataForm.InspectionData.Rows[i]["Width"]}mm\nHeight : {InspectionDataForm.InspectionData.Rows[i]["Height"]}mm\nArea : {InspectionDataForm.InspectionData.Rows[i]["Area"]}",
                                new Font(FontFamily.GenericSansSerif, 7), brush, (float)double.Parse(InspectionDataForm.InspectionData.Rows[i]["PosX"].ToString()), (float)double.Parse(InspectionDataForm.InspectionData.Rows[i]["PosY"].ToString()));

                        }
                    }
                }
               
            }
            catch (Exception e)
            {
                int test = 0;
            }

        }

        public void DoPaintSelectInspectionData(Graphics graphics, int nIdx)
        {
            try
            {
                if (!InspectionDataForm.Visible)
                {
                    this.Image.Draw(graphics);
                }
                else
                {
                    theMainSystem.Cameras[nIdx].GetListImage(0, true).Draw(graphics); //240710 KCH 이미지 Paint
                    int index = 0;
                    if (InspectionDataForm.InspectionData.Rows[InspectionDataForm.GridselectedNo]["Camera"].ToString() == nIdx.ToString())
                    {


                        PolygonData = InspectionDataForm.InspectionData.Rows[InspectionDataForm.GridselectedNo]["PolygonData"].ToString();
                        PolygonValue = PolygonData.Split(',');
                        Color color = Color.FromArgb(int.Parse(InspectionDataForm.InspectionData.Rows[InspectionDataForm.GridselectedNo]["DefectColor_Alpha"].ToString()),
                                                     int.Parse(InspectionDataForm.InspectionData.Rows[InspectionDataForm.GridselectedNo]["DefectColor_Red"].ToString()),
                                                     int.Parse(InspectionDataForm.InspectionData.Rows[InspectionDataForm.GridselectedNo]["DefectColor_Green"].ToString()),
                                                     int.Parse(InspectionDataForm.InspectionData.Rows[InspectionDataForm.GridselectedNo]["DefectColor_Blue"].ToString()));

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
                        graphics.DrawString($"Width : {InspectionDataForm.InspectionData.Rows[InspectionDataForm.GridselectedNo]["Width"]}mm\nHeight : {InspectionDataForm.InspectionData.Rows[InspectionDataForm.GridselectedNo]["Height"]}mm\nArea : {InspectionDataForm.InspectionData.Rows[InspectionDataForm.GridselectedNo]["Area"]}",
                            new Font(FontFamily.GenericSansSerif, 7), brush, (float)double.Parse(InspectionDataForm.InspectionData.Rows[InspectionDataForm.GridselectedNo]["PosX"].ToString()), (float)double.Parse(InspectionDataForm.InspectionData.Rows[InspectionDataForm.GridselectedNo]["PosY"].ToString()));

                    }
                }
            }
            catch (Exception e)
            {
                int test = 0;
            }

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

        public Point Get_TeachFormScreenLocation()      //240721 NIS Get TeachForm Location
        {
            Point PanelLocation = MainForm.Get_DisplayPanelLocation();
            return new Point(PanelLocation.X + pnl_HistoryFormScreen.Location.X, PanelLocation.Y + pnl_HistoryFormScreen.Location.Y);
        }

        

        private void FormTeachInspection_Load(object sender, EventArgs e)   //240724 NIS Form이 Load될 때 Crack 클릭하기
        {
            btnDisplayInspectionForm.PerformClick();     //240724 NIS Display Crack Form
        }
    }
}
