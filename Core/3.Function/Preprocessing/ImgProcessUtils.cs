using Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utility;
using System.Runtime.InteropServices;

using Cognex.VisionPro.ImageFile;
using Insnex.Vision2D.Core;
using Insnex.ImageFile;

namespace Core.Function.Preprocessing
{
    internal static class ImgProcessUtils
    {
        public static void CopyFromCogImage8Grey(this CImage cimage, InsImage8Grey img)
        {
            cimage.Copy(img.Width, img.Height, CImage.PIXEL8, img);
        }

        public static bool SaveBmpFromICogImage(this CImage cimage, string path)
        {
            try
            {

                InsImageFileBMP bmp = new InsImageFileBMP();
                bmp.Open(path, InsImageFileModeConstants.Write);
                bmp.Append(cimage.InsImage);
                bmp.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void SaveBmpFromICogImage(IInsImage inputImage, string path)
        {
            InsImageFileBMP bmp = new InsImageFileBMP();
            string fileName = path;
            bmp.Open(fileName, InsImageFileModeConstants.Write);
            bmp.Append(inputImage);
            bmp.Close();
        }

        public static void SaveJpgFromICogImage(CImage cimage, string path)
        {
            using (CogImageFileJPEG SaveJpg = new CogImageFileJPEG())
            {
                //SaveJpg.Open(path, CogImageFileModeConstants.Write);
                //SaveJpg.Append(cimage.CogImage);
                //SaveJpg.Close();
            }
                
        }
    }
}
