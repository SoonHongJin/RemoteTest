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
using ConnectedInsightCore;

using static Core.Program;

using Cognex.VisionPro;
using Cognex.VisionPro.Implementation;

namespace Core.Utility
{

    public partial class CCogDisplayImageControl : UserControl
    {
        private MainForm Mainform = null;
        public CCameraManager cCameraManager = null;
        //CImage Image = null;

        public int Id = 0;

        public Action<int> ContorlLocation = null;

        public CCogDisplayImageControl(MainForm _Mainform)
        {
            Mainform = _Mainform;

            InitializeComponent();

        }

        public void SetCamera(CCameraManager _CameraAction, int _nMode)
        {
            cCameraManager = _CameraAction;

            if (null != cCameraManager)
            {
                if(_nMode == DEF_SYSTEM.MONO_IMAGE_GRAB)
                    cCameraManager.SetRefreshGrabImage(_nMode, DisplayImage);
                else if(_nMode == DEF_SYSTEM.MONO_LIVE_IMAGE_GRAB)
                    cCameraManager.SetRefreshGrabImage(_nMode, LiveDisplayImage);
                else if(_nMode == DEF_SYSTEM.MONO_CALIBRATION_GRAB)
                    cCameraManager.SetRefreshGrabImage(_nMode, CalibResultDisplayImage);
            }
        }

        private void LiveDisplayImage(CImage _Image)
        {
            //Mainform.SettingScreen.Frames[Id].ImageControlDisplay.Image = _Image.CogImage;
            //Mainform.SettingScreen.Frames[Id].ImageControlDisplay.Fit();
        }

        private void DisplayImage(CImage _Image)
        {
            //InspectionInfo로 결과 디스플레이 한다.
            //Mainform.DisplayScreen.Frames[Id].ImageControlDisplay.Image = _Image.CogImage;
            //Mainform.DisplayScreen.Frames[Id].ImageControlDisplay.Fit();
        }

        private void CalibResultDisplayImage(CImage _Image)
        {
            //InspectionInfo로 결과 디스플레이 한다.
            //Mainform.FormCalibration.Frames[Id].ImageControlDisplay.Image = _Image.CogImage;
            //Mainform.FormCalibration.Frames[Id].ImageControlDisplay.Fit();
        }

        public void ScreenCaptureForm(string sOriginalImagePath, string sProductName)
        {
            //try
            //{
            //    theMainSystem.Logging("Display Start");
            //
            //    IntPtr hwnd = theMainSystem.mainForm.Handle;
            //    Graphics gfxWin = Graphics.FromHwnd(hwnd);
            //    Bitmap bmp = new Bitmap(theMainSystem.mainForm.Width, theMainSystem.mainForm.Height, CImage.cPixelFormats[4]);
            //    Graphics gfxBmp = Graphics.FromImage(bmp);
            //    IntPtr hdcBitmap = gfxBmp.GetHdc();
            //    bool succeeded = CFunc.PrintWindow(hwnd, hdcBitmap, 2);
            //    gfxBmp.ReleaseHdc(hdcBitmap);
            //    if (!succeeded)
            //        gfxBmp.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(System.Drawing.Point.Empty, bmp.Size));
            //    gfxBmp.Dispose();
            //
            //    string sPath = $"{sOriginalImagePath}\\{sProductName}.jpg";
            //    bmp.Save(sPath, ImageFormat.Jpeg);
            //
            //    theMainSystem.Logging("Display End");
            //
            //    bmp.Dispose();
            //}
            //catch (Exception e)
            //{
            //    theMainSystem.Logging($"Form Capture Error. {e.Message} : {e.StackTrace}");
            //}
        }

    }
}
