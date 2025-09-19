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

using Core;
using Core.UI;
using Core.Process;
using Core.Utility;
using static Core.Program;

namespace Core.Utility
{
    public partial class CSimulImageControl : UserControl
    {
        private CImage Image = null;

        private MainForm Mainform = null;
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
        private FormSimulation Simulation = null;
        public CCameraManager cCameraManager = null;
        public Action<int> ControlLocation = null;
        public DisplayScreen Display = null;

        public int m_ImageModeSimul = 0;


        public static readonly float ZoomFactor = 1.2F;

        public CSimulImageControl(MainForm _Mainform)
        {
            Mainform = _Mainform;

            InitializeComponent();

            View.MouseWheel += new MouseEventHandler(OnMouseWheel);
        }
        
        public void SetCamera(CCameraManager _CameraAction, int _ColorMode, FormSimulation _simulation)
        {
            cCameraManager = _CameraAction;
            Simulation = _simulation;

            if (null != cCameraManager)
            {
                if (_ColorMode == 0)
                {
                    cCameraManager.SetRefreshSimulImage(SetImage);

                    m_nImageWidth = (int)cCameraManager.m_Camera.m_nImgHeight;
                    m_nImageHeight = (int)cCameraManager.m_Camera.m_nImgWidth;
                }

                ZoomFit();
            }
        }

        public void Live(CImage image)
        {
            this.Invoke(new MethodInvoker(delegate () { Refresh(); }));
        }

        public void SetImage(CImage _image)
        {
            Image = _image;
            if (null != Image)
            {
                m_nImageWidth = Image.m_nWidth;
                m_nImageWidth = Image.m_nHeight;
                //ZoomFit();
            }
            else
            {
                Name = string.Empty;
            }
            Refresh();
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

            if (Image != null)
            {
                if (Id <= DEF_SYSTEM.CAM_FOUR)
                    Simulation.DoDefectCurrentPaint(graphic, Id);
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
    }
}
