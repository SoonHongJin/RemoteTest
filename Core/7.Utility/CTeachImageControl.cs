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
using Cognex.VisionPro;
using Cognex.VisionPro.Implementation;

namespace Core.Utility
{

    public partial class CTeachImageControl : UserControl
    {
        private MainForm Mainform = null;
        public CCameraManager cCameraManager = null;
        public Action<int> ControlLocation = null;
        public DisplayScreen Display = null;
        CImage Image = null;

        public int Id = 0;
        private int m_nImageWidth = 0;
        private int m_nImageHeight = 0;
        private float zoom = 1;

        private float startx = 0;
        private float starty = 0;
        private float imgx = 0;
        private float imgy = 0;
        private bool mousepressed = false;
        private Point mouseDown;
        private bool bLiveMode = false;
        private bool m_bFirstCheck = false;
        private Graphics m_Graphic;

        public static readonly float ZoomFactor = 1.2F;

        public int m_ImageMode = 0; //240219 LYK CVD Gray Image와 컬러 Image 구분
        private CLogger Logger = null;

        public CTeachImageControl(MainForm _Mainform, CLogger _logger)
        {
            Mainform = _Mainform;

            InitializeComponent();

            View.MouseWheel += new MouseEventHandler(OnMouseWheel);
            Logger = _logger;
        }
        public void SetCamera(CCameraManager _CameraAction, int _ColorMode)
        {
            cCameraManager = _CameraAction;

            if (null != cCameraManager)
            {
                if (_ColorMode == 0)
                    cCameraManager.SetRefreshTeachingImage(SetImage);

                if (_ColorMode != 1) // Color 가 아닐 때 (컬러 사이즈 축소 됨)
                {
                    m_nImageWidth = (int)cCameraManager.m_Camera.m_nImgWidth;
                    m_nImageHeight = (int)cCameraManager.m_Camera.m_nImgHeight * theRecipe.MergeImageCount;
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


            Refresh();

            Refresh();
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            float oldzoom = zoom;

            zoom = (e.Delta > 0) ? zoom * ZoomFactor : zoom / ZoomFactor;

            if (zoom > 100.0F) zoom = 100.0F;
            else if (zoom < 0.1F) zoom = 0.1F;

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

            if (Image!= null && Id == 12)
            {
                if(m_bFirstCheck == false)
                {
                    m_Graphic = graphic;
                    m_bFirstCheck = true;
                }
                //cCameraManager.DoDefectAccumulatePaint(graphic);
            }
            else if(Image != null)
            {
                //if (Id <= DEF_SYSTEM.CAM_FOUR)
                //        cCameraManager.DoTeachDefectCurrentPaint(graphic, Id);
                //    else
                        Image.Draw(graphic);

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
                }
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                Point mousePosNow = mouse.Location;

                int deltaX = mousePosNow.X - mouseDown.X;
                int deltaY = mousePosNow.Y - mouseDown.Y;

                imgx = startx + (deltaX / zoom);
                imgy = starty + (deltaY / zoom);

                Refresh();
            }
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
            try
            {
                //Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Display Start");
                //
                //IntPtr hwnd = theMainSystem.mainForm.Handle;
                //Graphics gfxWin = Graphics.FromHwnd(hwnd);
                //Bitmap bmp = new Bitmap(theMainSystem.mainForm.Width, theMainSystem.mainForm.Height, CImage.cPixelFormats[4]);
                //Graphics gfxBmp = Graphics.FromImage(bmp);
                //IntPtr hdcBitmap = gfxBmp.GetHdc();
                //bool succeeded = CPlusPlusFunc.PrintWindow(hwnd, hdcBitmap, 2);
                //gfxBmp.ReleaseHdc(hdcBitmap);
                //if (!succeeded)
                //    gfxBmp.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(System.Drawing.Point.Empty, bmp.Size));
                //gfxBmp.Dispose();
                //
                //string sPath = $"{sOriginalImagePath}\\{sProductName}.jpg";
                //bmp.Save(sPath, ImageFormat.Jpeg);
                //
                //Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "Display End");
                //
                //bmp.Dispose();
            }
            catch (Exception e)
            {
                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], $"Form Capture Error. {e.Message} : {e.StackTrace}");
            }
        }

    }
}
