using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

using Core;
using Core.UI;
using Core.Process;
using static Core.Program;

using Core.Function;
using ConnectedInsightCore;

namespace Core.Utility
{

    public partial class CImageControl : UserControl
    {
        private MainForm Mainform = null;
        public CCameraManager cCameraManager = null;
        public Action<int> ControlLocation = null;
        public DisplayScreen Display = null;
        CImage Image = null;

        public int Id = 0;
        public int Mode = 0;
        public int Idx = 0;
        private int m_nImageWidth = 0;
        private int m_nImageHeight = 0;
        private float zoom = 1;

        private float startx = 0;
        private float starty = 0;
        private float teststartx = 0;
        private float teststarty = 0;
        private float imgx = 0;
        private float imgy = 0;
        private float testx = 0;
        private float testy = 0;
        private bool mousepressed = false;
        private Point mouseDown;
        private bool bLiveMode = false;
        private bool m_bFirstCheck = false;
        private Graphics m_Graphic;
        public static readonly float ZoomFactor = 1.2F;

        public int m_ImageMode = 0; //240219 LYK CVD Gray Image와 컬러 Image 구분
        public CLogger Logger = null;


        public CImageControl(MainForm _Mainform, CLogger _logger)
        {
            Mainform = _Mainform;
            Logger = _logger;
            
            InitializeComponent();

            View.MouseWheel += new MouseEventHandler(OnMouseWheel);

        }
        public void SetCamera(CCameraManager _CameraAction, int _Mode)
        {
            cCameraManager = _CameraAction;

            if (null != cCameraManager)
            {
                if (Mode == DEF_SYSTEM.DISPLAY_IMAGE)
                {
                    cCameraManager.SetRefreshGrabImage(DEF_SYSTEM.MONO_IMAGE_GRAB, SetImage);

                    // 250911 SHJ 압축 사이즈로 입력 
                    m_nImageWidth = (int)cCameraManager.ListImages[0][0].m_nReduceWidth;
                    m_nImageHeight = (int)cCameraManager.ListImages[0][0].m_nReduceHeight * theRecipe.MergeImageCount;
                }
                else if(Mode == DEF_SYSTEM.DISPLAY_MAP)
                {
                    cCameraManager.SetRefreshGrabImage(DEF_SYSTEM.MONO_IMAGE_TABLE_GRAB, SetImage);

                    m_nImageWidth = (int)DEF_SYSTEM.TABLE_DISPLAY_WIDTH;
                    m_nImageHeight = (int)DEF_SYSTEM.TABLE_DISPLAY_HEIGHT;
                }
                else if (Mode == DEF_SYSTEM.DISPLAY_DEFECT)
                {
                    cCameraManager.SetRefreshGrabImage(DEF_SYSTEM.MONO_IMAGE_DEFECT_GRAB, SetImage, Idx);

                    m_nImageWidth = (int)DEF_SYSTEM.DEFECT_CROP_WIDTH;
                    m_nImageHeight = (int)DEF_SYSTEM.DEFECT_CROP_HEIGHT;
                }

                //220411 LYK
                ZoomFit();
            }
        }

        public void SetImage(CImage _image)
        {
            Image = _image;

            if (null != Image)
            {
                //m_nImageWidth = Image.m_nWidth;
                //m_nImageHeight = Image.m_nHeight;
                //ZoomFit();
            }
            else
            {
                Name = string.Empty;
            }

            Refresh();

        }

        public void RefreshGrab(CImage image)
        {
            this.Invoke(new MethodInvoker(delegate () { View.Refresh(); }));
        }

        public void ZoomFit()
        {
            float zoomX = ((float)(View.Width) / (float)m_nImageWidth);
            float zoomY = ((float)(View.Height) / (float)m_nImageHeight);
            bool horizontal = (zoomX > zoomY) ? true : false;
            zoom = horizontal ? zoomY : zoomX;

            imgx = horizontal ? (View.Width - zoom * m_nImageWidth) / (2 * zoom) : 0;
            imgy = horizontal ? 0 : (View.Height - zoom * m_nImageHeight) / (2 * zoom);
            testx = horizontal ? (View.Width - zoom * m_nImageWidth) / (2 * zoom) : 0;
            testy = horizontal ? 0 : (View.Height - zoom * m_nImageHeight) / (2 * zoom);

            Refresh();
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            float oldzoom = zoom;

            zoom = (e.Delta > 0) ? zoom * ZoomFactor : zoom / ZoomFactor;

            if (zoom > 100.0F) zoom = 100.0F;
            else if (zoom < 0.07F) zoom = 0.07F;

            MouseEventArgs mouse = e as MouseEventArgs;
            Point mousePosNow = mouse.Location;

            int x = mousePosNow.X - View.Location.X;
            int y = mousePosNow.Y - View.Location.Y;

            int oldimagex = (int)(x / oldzoom);
            int oldimagey = (int)(y / oldzoom);

            int newimagex = (int)(x / zoom);
            int newimagey = (int)(y / zoom);

            imgx = newimagex - oldimagex + imgx;
            imgy = newimagey - oldimagey + imgy;

            Refresh();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics graphic = e.Graphics;
            graphic.Clear(Color.Black);

            //View.SizeMode = PictureBoxSizeMode.StretchImage;
            //View.SizeMode = PictureBoxSizeMode.CenterImage;
            graphic.InterpolationMode = InterpolationMode.NearestNeighbor;

            graphic.ScaleTransform(zoom, zoom);
            graphic.TranslateTransform(imgx, imgy);

            graphic.SmoothingMode = SmoothingMode.AntiAlias;

            if (Image != null)
            {
                //if (Id <= DEF_SYSTEM.CAM_FOUR)
                // 240430 SHJ Mode 상수를 추가 하여 호출한 상황에 맞게 이미지 Draw
                if (Mode == DEF_SYSTEM.DISPLAY_IMAGE)
                    cCameraManager.DoDefectCurrentPaint(graphic, Id);
                else if (Mode == DEF_SYSTEM.DISPLAY_MAP)
                    cCameraManager.DoDefectMapCurrentPaint(graphic, Id);
                else if (Mode == DEF_SYSTEM.DISPLAY_DEFECT)
                    cCameraManager.DoDefectCropCurrentPaint(graphic, Idx);
                //Image.Draw(graphic);

                //cCameraManager.ImageDisplay(graphic, Id, m_ImageMode);
                //ZoomFit();
            }

        }

        private void View_MouseDoubleClick(object sender, EventArgs e)
        {
            ControlLocation?.Invoke(Id);

            ZoomFit();
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                if (!mousepressed)
                {
                    mousepressed = true;
                    mouseDown = mouse.Location;
                    startx = imgx;
                    starty = imgy;
                    teststartx = testx;
                    teststarty = testy;
                }
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {   
            MouseEventArgs mouse = e as MouseEventArgs;
            if (mouse.Button == MouseButtons.Left)
            {
                Point mousePosNow = mouse.Location;

                float deltaX = mousePosNow.X - mouseDown.X;
                float deltaY = mousePosNow.Y - mouseDown.Y;

                imgx = startx + (deltaX / zoom);
                imgy = starty + (deltaY / zoom);

                testx = teststartx + deltaX;
                testy = teststarty + deltaY;

                Refresh();
            }
            
            float scalex = ((float)m_nImageWidth / (float)View.Width);
            float scaley = ((float)m_nImageHeight / (float)View.Height);

            float a = imgx * zoom;
            float scaleWidth = (mouse.Location.X - testx) * scalex;
            float scaleHeight = (mouse.Location.Y - testy) * scaley;
            
            
            SetMousePoint(scaleWidth, scaleHeight);
        }
        /// <summary>
        /// 25.02.28 NWT 
        /// 마우스 커서 위치 좌표
        /// </summary>
        /// <param name="_Width"></param>
        /// <param name="_Height"></param>
        public void SetMousePoint(float _Width, float _Height)
        {
            int camNum = Id;

            double Pixel_Resolution = 0.042;// theMainSystem.m_darrPixelResolution[camNum];

            float x = 0;
            float y = 0;

            x = (float)(_Width * Pixel_Resolution);
            y = (float)(_Height * Pixel_Resolution);

            Mainform.DisplayScreen.GetMousePoint(x,y);

        }
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            mousepressed = false;
        }

        private void OnSize(object sender, EventArgs e)
        {
            ZoomFit();
        }

        public void ScreenCaptureForm(string sOriginalImagePath, string sProductName)
        {
            //try
            //{
            //    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Display Start");
            //
            //    IntPtr hwnd = theMainSystem.mainForm.Handle;
            //    Graphics gfxWin = Graphics.FromHwnd(hwnd);
            //    Bitmap bmp = new Bitmap(theMainSystem.mainForm.Width, theMainSystem.mainForm.Height, CImage.cPixelFormats[4]);
            //    Graphics gfxBmp = Graphics.FromImage(bmp);
            //    IntPtr hdcBitmap = gfxBmp.GetHdc();
            //    bool succeeded = CPlusPlusFunc.PrintWindow(hwnd, hdcBitmap, 2);
            //    gfxBmp.ReleaseHdc(hdcBitmap);
            //    if (!succeeded)
            //        gfxBmp.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(System.Drawing.Point.Empty, bmp.Size));
            //    gfxBmp.Dispose();
            //
            //    string sPath = $"{sOriginalImagePath}\\{sProductName}.jpg";
            //    bmp.Save(sPath, ImageFormat.Jpeg);
            //
            //    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Display End");
            //
            //    bmp.Dispose();
            //}
            //catch (Exception e)
            //{
            //    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Form Capture Error. {e.Message} : {e.StackTrace}");
            //}
        }

    }
}
