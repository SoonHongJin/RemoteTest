using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;
using ConnectedInsightCore;
using static ConnectedInsightCore.CDefineDLL;

using Insnex.Vision2D.Core;
using Insnex.Vision2D.ImageProcessing;
using System.Web.UI;
using Core.Function;

namespace Core.Utility
{
    public class CImage
    {
        public static readonly int PIXEL_UNKNOWN = 0;

        public static readonly int PIXEL8 = 1;

        public static readonly int PIXEL16 = 2;

        public static readonly int PIXEL24 = 3;

        public static readonly int PIXEL32 = 4;

        public static readonly PixelFormat[] cPixelFormats = new PixelFormat[5]
        {
            PixelFormat.Undefined,
            PixelFormat.Format8bppIndexed,
            PixelFormat.Format16bppGrayScale,
            PixelFormat.Format24bppRgb,
            PixelFormat.Format32bppRgb
        };

        private readonly object m_Lock = new object();

        private bool m_bIsInitialized;

        private bool m_bHasDummy;

        private Rectangle m_Rect = Rectangle.Empty;

        private CogImage8Root[] GrayCogRoot;

        private InsImage8Root GrayInsRoot = new InsImage8Root();

        public InsImage8Grey InsGrayImage = new InsImage8Grey();

        public int m_nWidth { get; set; }

        public int m_nHeight { get; set; }

        public int m_nReduceWidth { get; set; }

        public int m_nReduceHeight { get; set; }
        public int m_nReduceStride { get; set; }

        public int m_nStride { get; set; }

        public int m_nSliceStride { get; set; }

        public int m_nPixelFormat { get; set; }

        public int m_nImageSize { get; set; }

        public int m_nMergeCount { get; set; } = 0;

        public int m_nReduceSize { get; set; } = 1;

        public int m_nStandardWidth { get; set; } //20250910 SHJ Standard Width -> 기본 Slice Widh
        public int m_nSliceWidth { get; set; }  //20250910 SHJ Slice Width -> Standard + Offset 을 먹은 Width
        public int m_nSliceHeight { get; set; }
        public int m_nSliceStartPosX { get; set; }

        //20250909 SHJ Display 같은 경우 기판 Alignment 가 틀어질 경우가 있어서 Width Slice Offset 을 이용 하여 양옆 여유 공간 보유
        public int m_nSliceWidthOffset { get; set; } = 0;

        public int[] m_nSliceJump { get; set; } //20250915 SHJ 디스플레이 기판 배열을 Slice 하는 도중 dumy 구간이 있을 경우 특정 위치를 Jump 하여 Slice 하기 위해 추가 (SliceOptionSet 메소드에 값 입력 하지 않으면 기본 0으로 할당 되어 있어 기존 사용에 영향X)
        public byte[] m_pImage { get; set; }

        public IntPtr pData { get; set; } = IntPtr.Zero;

        public IntPtr pDataMerge { get; set; } = IntPtr.Zero;

        public IntPtr[] pSliceData { get; set; }

        public CogImage8Grey[] CogImage { get; set; }

        public IInsImage InsImage { get; set; }

        private int m_nLicense = 0;
        private int m_nSliceCount = 0;
        private string m_sCrruentEqupiment = "";

        public CImage(int _bLicense, string _sCurrentEquipment, int _SliceCount = 0, int _nMergeCount = 0)
        {
            m_nLicense = _bLicense;
            m_nSliceCount = _SliceCount;
            m_sCrruentEqupiment = _sCurrentEquipment;
            m_nMergeCount = _nMergeCount;
        }

        private int GetPixelFormat(PixelFormat pixelFormat)
        {
            for (int i = PIXEL8; i <= PIXEL32; i++)
            {
                if (cPixelFormats[i] == pixelFormat)
                {
                    return i;
                }
            }

            return PIXEL_UNKNOWN;
        }

        public void Allocate(int nWidth, int nHeight, int pixelFormat, int _StartXpos = 0, int _nStandardWidth = 0, int _nMergeCount = 0)
        {
            if (!m_bIsInitialized)
            {
                if (m_nLicense == CDefineDLL.COG)
                {
                    m_nWidth = nWidth;
                    m_nHeight = nHeight;
                    m_nPixelFormat = pixelFormat;
                    m_nStride = 4 * ((m_nWidth * pixelFormat + 3) / 4);
                    m_nImageSize = m_nStride * m_nHeight * pixelFormat;
                    m_nStandardWidth = _nStandardWidth;
                    m_nSliceWidth = _nStandardWidth;
                    m_nMergeCount = _nMergeCount;
                    m_Rect = new Rectangle(0, 0, m_nWidth, m_nHeight);
                    m_bHasDummy = ((m_nWidth * pixelFormat % 4 != 0) ? true : false);
                    pData = Marshal.AllocHGlobal(m_nImageSize);

                    if (m_sCrruentEqupiment == "ELECTRONIC")
                    {
                        if (m_nSliceCount != 0)
                        {
                            pSliceData = new IntPtr[m_nSliceCount];
                            GrayCogRoot = new CogImage8Root[m_nSliceCount];
                            CogImage = new CogImage8Grey[m_nSliceCount];
                            m_nSliceStride = 4 * (((m_nWidth / m_nSliceCount) * m_nPixelFormat + 3) / 4);

                            for (int i = 0; i < m_nSliceCount; i++)
                            {
                                pSliceData[i] = Marshal.AllocHGlobal(m_nSliceStride * m_nHeight);

                                //m_pImage = new byte[m_nImageSize];
                                GrayCogRoot[i] = new CogImage8Root();
                                GrayCogRoot[i].Initialize((m_nWidth / m_nSliceCount), m_nHeight, pSliceData[i], m_nSliceStride, null);

                                CogImage[i] = new CogImage8Grey();
                                CogImage[i].SetRoot(GrayCogRoot[i]);
                            }

                        }
                    }
                    else
                    {
                        if (m_nSliceCount != 0)
                        {
                            pSliceData = new IntPtr[m_nSliceCount];
                            GrayCogRoot = new CogImage8Root[m_nSliceCount];
                            m_nSliceStride = 4 * ((m_nSliceWidth * m_nPixelFormat + 3) / 4);

                            for (int i = 0; i < m_nSliceCount; i++)
                            {
                                GrayCogRoot[i] = new CogImage8Root();

                                pSliceData[i] = Marshal.AllocHGlobal(m_nSliceStride * m_nHeight * m_nMergeCount);

                                //m_pImage = new byte[m_nImageSize];

                                // 4바이트 정렬(행 바이트 수)
                                GrayCogRoot[i].Initialize(_nStandardWidth, m_nHeight * m_nMergeCount, pSliceData[i], m_nSliceStride, null);

                                CogImage[i] = new CogImage8Grey();
                                CogImage[i].SetRoot(GrayCogRoot[i]);
                            }

                        }
                    }


                    m_bIsInitialized = true;
                }
                else
                {
                    m_nWidth = nWidth;
                    m_nHeight = nHeight;
                    m_nPixelFormat = pixelFormat;
                    m_nStride = 4 * ((m_nWidth * pixelFormat + 3) / 4);
                    m_nImageSize = m_nWidth * m_nHeight * pixelFormat;
                    m_Rect = new Rectangle(0, 0, m_nWidth, m_nHeight);
                    m_bHasDummy = m_nWidth * pixelFormat % 4 != 0;
                    pData = Marshal.AllocHGlobal(m_nImageSize);
                    m_pImage = new byte[m_nImageSize];

                    if (m_nSliceCount != 0)
                    {
                        pSliceData = new IntPtr[m_nSliceCount];

                        for (int i = 0; i < m_nSliceCount; i++)
                            pSliceData[i] = Marshal.AllocHGlobal((m_nWidth / m_nSliceCount) * m_nHeight);
                    }

                    InsImage8Root insImage8Root = new InsImage8Root();
                    insImage8Root.Initialize(m_nStride, m_nHeight, pData, m_nStride, null);
                    InsImage8Grey insImage8Grey = new InsImage8Grey();
                    insImage8Grey.SetRoot(insImage8Root);

                    InsImage = insImage8Grey;

                    m_bIsInitialized = true;
                }

            }
        }

        public void Allocate(int nWidth, int nHeight, int pixelFormat, int nMergeCount, bool bMerge = false, int ReduceSize = 1, int _nStartPosX = 0, int _nStandardWidth = 0, int _nSilceWidthOffset = 0)
        {
            if (!m_bIsInitialized)
            {
                m_nWidth = nWidth;
                m_nHeight = nHeight;
                
                m_nPixelFormat = pixelFormat;
                m_nStride = 4 * ((m_nWidth * pixelFormat + 3) / 4);
                m_nImageSize = m_nStride * m_nHeight * pixelFormat;  
                m_nStandardWidth = _nStandardWidth;
                m_nSliceHeight = nHeight;
                m_nMergeCount = nMergeCount;
                m_nSliceStartPosX = _nStartPosX;
                m_nSliceWidthOffset = _nSilceWidthOffset;
                m_nSliceJump = new int[m_nSliceCount];

                m_nReduceSize = ReduceSize;
                m_nReduceWidth = m_nWidth / m_nReduceSize;
                m_nReduceHeight = m_nHeight / m_nReduceSize;
                m_nReduceStride = 4 * ((m_nReduceWidth * pixelFormat + 3) / 4);

                m_Rect = new Rectangle(0, 0, m_nWidth, m_nHeight);
                m_bHasDummy = ((m_nWidth * pixelFormat % 4 != 0) ? true : false);

                pData = Marshal.AllocHGlobal(m_nImageSize);

                if (bMerge == true)
                    pDataMerge = Marshal.AllocHGlobal(m_nReduceStride * m_nReduceHeight * m_nMergeCount);

                //m_pImage = new byte[m_nImageSize];

                if (m_nLicense == CDefineDLL.COG)
                {
                    if(m_sCrruentEqupiment == "ELECTRONIC")
                    {
                        if (m_nSliceCount != 0)
                        {
                            GrayCogRoot = new CogImage8Root[m_nSliceCount];
                            pSliceData = new IntPtr[m_nSliceCount];
                            CogImage = new CogImage8Grey[m_nSliceCount];
                            m_nSliceStride = 4 * (((m_nWidth / m_nSliceCount) * m_nPixelFormat + 3) / 4);

                            for (int i = 0; i < m_nSliceCount; i++)
                            {
                                GrayCogRoot[i] = new CogImage8Root();
                                pSliceData[i] = Marshal.AllocHGlobal(m_nSliceStride * m_nHeight);

                                //m_pImage = new byte[m_nImageSize];
                                GrayCogRoot[i].Initialize((m_nWidth / m_nSliceCount), m_nHeight, pSliceData[i], m_nSliceStride, null);

                                CogImage[i] = new CogImage8Grey();
                                CogImage[i].SetRoot(GrayCogRoot[i]);
                            }

                        }
                    }
                    else
                    {
                        if (m_nSliceCount != 0)
                        {
                            pSliceData = new IntPtr[m_nSliceCount];
                            GrayCogRoot = new CogImage8Root[m_nSliceCount];
                            CogImage = new CogImage8Grey[m_nSliceCount];
                            m_nSliceWidth = m_nStandardWidth + (m_nSliceWidthOffset * 2);
                            m_nSliceHeight = m_nHeight * m_nMergeCount;
                            m_nSliceStride = 4 * ((m_nSliceWidth * m_nPixelFormat + 3) / 4);

                            for (int i = 0; i < m_nSliceCount; i++)
                            {
                                GrayCogRoot[i] = new CogImage8Root();
                                pSliceData[i] = Marshal.AllocHGlobal(m_nSliceStride * m_nHeight * m_nMergeCount);

                                //m_pImage = new byte[m_nImageSize];
                                GrayCogRoot[i] = new CogImage8Root();
                                GrayCogRoot[i].Initialize(m_nSliceWidth, m_nHeight * m_nMergeCount, pSliceData[i], m_nSliceStride, null);

                                CogImage[i] = new CogImage8Grey();
                                CogImage[i].SetRoot(GrayCogRoot[i]);

                            }

                        }
                    }

                    m_bIsInitialized = true;
                }
                else
                {
                    InsImage8Root insImage8Root = new InsImage8Root();
                    insImage8Root.Initialize(m_nStride, m_nHeight, pData, m_nStride, null);
                    InsImage8Grey insImage8Grey = new InsImage8Grey();
                    insImage8Grey.SetRoot(insImage8Root);

                    InsImage = insImage8Grey;

                    m_bIsInitialized = true;
                }

            }
        }

        public void Allocate(int nWidth, int nHeight, int nStride, int pixelFormat, int _nStartPosX = 0, int _nStandardWidth = 0, int _nMergeCount = 0)
        {
            if (!m_bIsInitialized)
            {
                if (m_nLicense == CDefineDLL.COG)
                {
                    m_nWidth = nWidth;
                    m_nHeight = nHeight;
                    m_nPixelFormat = pixelFormat;
                    m_nStride = nStride;
                    m_nImageSize = m_nStride * m_nHeight * pixelFormat;
                    m_nStandardWidth = _nStandardWidth;
                    m_nSliceWidth = _nStandardWidth;
                    m_nMergeCount = _nMergeCount;
                    m_Rect = new Rectangle(0, 0, m_nWidth, m_nHeight);
                    m_bHasDummy = ((m_nWidth * pixelFormat % 4 != 0) ? true : false);
                    pData = Marshal.AllocHGlobal(m_nImageSize);
                    //m_pImage = new byte[m_nStride * nHeight];

                    if (m_sCrruentEqupiment == "ELECTRONIC")
                    {
                        if (m_nSliceCount != 0)
                        {
                            pSliceData = new IntPtr[m_nSliceCount];
                            GrayCogRoot = new CogImage8Root[m_nSliceCount];
                            CogImage = new CogImage8Grey[m_nSliceCount];
                            m_nSliceStride = 4 * (((m_nWidth / m_nSliceCount) * m_nPixelFormat + 3) / 4);

                            for (int i = 0; i < m_nSliceCount; i++)
                            {
                                GrayCogRoot[i] = new CogImage8Root();
                                pSliceData[i] = Marshal.AllocHGlobal(m_nSliceStride * m_nHeight);

                                //m_pImage = new byte[m_nImageSize];
                                GrayCogRoot[i].Initialize((m_nWidth / m_nSliceCount), m_nHeight, pSliceData[i], m_nSliceStride, null);

                                CogImage[i] = new CogImage8Grey();
                                CogImage[i].SetRoot(GrayCogRoot[i]);
                            }

                        }
                    }
                    else
                    {
                        if (m_nSliceCount != 0)
                        {
                            pSliceData = new IntPtr[m_nSliceCount];
                            GrayCogRoot = new CogImage8Root[m_nSliceCount];
                            m_nSliceStride = 4 * ((m_nSliceWidth * m_nPixelFormat + 3) / 4);

                            for (int i = 0; i < m_nSliceCount; i++)
                            {

                                GrayCogRoot[i] = new CogImage8Root();

                                pSliceData[i] = Marshal.AllocHGlobal(m_nSliceStride * m_nHeight * m_nMergeCount);

                                //m_pImage = new byte[m_nImageSize];
                                GrayCogRoot[i].Initialize(m_nSliceWidth, m_nHeight * m_nMergeCount, pSliceData[i], m_nSliceStride, null);

                                CogImage[i] = new CogImage8Grey();
                                CogImage[i].SetRoot(GrayCogRoot[i]);
                            }

                        }
                    }

                    m_bIsInitialized = true;
                }
                else
                {
                    m_nWidth = nWidth;
                    m_nHeight = nHeight;
                    m_nPixelFormat = pixelFormat;
                    m_nStride = nStride;
                    m_nImageSize = m_nWidth * m_nHeight * pixelFormat;
                    m_Rect = new Rectangle(0, 0, m_nWidth, m_nHeight);
                    m_bHasDummy = m_nWidth * pixelFormat % 4 != 0;
                    pData = Marshal.AllocHGlobal(m_nImageSize);
                    m_pImage = new byte[m_nStride * nHeight];

                    if (m_nSliceCount != 0)
                    {
                        pSliceData = new IntPtr[m_nSliceCount];

                        for (int i = 0; i < m_nSliceCount; i++)
                            pSliceData[i] = Marshal.AllocHGlobal((m_nWidth / m_nSliceCount) * m_nHeight);
                    }

                    InsImage8Root cogImage8Root = new InsImage8Root();
                    cogImage8Root.Initialize(m_nStride, m_nHeight, pData, m_nStride, null);

                    InsImage8Grey cogImage8Grey = new InsImage8Grey();
                    cogImage8Grey.SetRoot(cogImage8Root);

                    InsImage = cogImage8Grey;

                    m_bIsInitialized = true;
                }

            }
        }

        public void Allocate(int nWidth, int nHeight, int pixelFormat, IntPtr pImage, bool bDummy = false, int nMergeCount = 0, int ReduceSize = 1, bool bMerge = false, int _nStartPosX = 0, int _nStandardWidth = 0)
        {
            if (!m_bIsInitialized)
            {
                if (m_nLicense == CDefineDLL.COG)
                {
                    m_nWidth = nWidth;
                    m_nHeight = nHeight;
                    m_nPixelFormat = pixelFormat;
                    m_nStride = 4 * ((m_nWidth * pixelFormat + 3) / 4);
                    m_nImageSize = m_nStride * m_nHeight * pixelFormat;
                    m_nStandardWidth = _nStandardWidth;
                    m_nSliceWidth = _nStandardWidth;
                    m_nMergeCount = nMergeCount;
                    m_nReduceSize = ReduceSize;
                    m_Rect = new Rectangle(0, 0, m_nWidth, m_nHeight);
                    m_bHasDummy = ((m_nWidth * pixelFormat % 4 != 0) ? true : false);
                    pData = Marshal.AllocHGlobal(m_nImageSize);
                    //m_pImage = new byte[m_nImageSize];

                    if(bMerge)
                        pDataMerge = Marshal.AllocHGlobal((m_nImageSize * m_nMergeCount) / m_nReduceSize);

                    if (m_sCrruentEqupiment == "ELECTRONIC")
                    {
                        if (m_nSliceCount != 0)
                        {
                            pSliceData = new IntPtr[m_nSliceCount];
                            GrayCogRoot = new CogImage8Root[m_nSliceCount];
                            CogImage = new CogImage8Grey[m_nSliceCount];
                            m_nSliceStride = 4 * (((m_nWidth / m_nSliceCount) * m_nPixelFormat + 3) / 4);

                            for (int i = 0; i < m_nSliceCount; i++)
                            {
                                GrayCogRoot[i] = new CogImage8Root();
                                pSliceData[i] = Marshal.AllocHGlobal(m_nSliceStride * m_nHeight);

                                //m_pImage = new byte[m_nImageSize];
                                GrayCogRoot[i].Initialize((m_nWidth / m_nSliceCount), m_nHeight, pSliceData[i], m_nSliceStride, null);

                                CogImage[i] = new CogImage8Grey();
                                CogImage[i].SetRoot(GrayCogRoot[i]);
                            }

                        }
                    }
                    else
                    {
                        if (m_nSliceCount != 0)
                        {
                            pSliceData = new IntPtr[m_nSliceCount];
                            GrayCogRoot = new CogImage8Root[m_nSliceCount];
                            CogImage = new CogImage8Grey[m_nSliceCount];
                            m_nSliceStride = 4 * ((_nStandardWidth * m_nPixelFormat + 3) / 4);

                            for (int i = 0; i < m_nSliceCount; i++)
                            {
                                pSliceData[i] = Marshal.AllocHGlobal(m_nSliceStride * m_nHeight * m_nMergeCount);

                                //m_pImage = new byte[m_nImageSize];
                                GrayCogRoot[i].Initialize(m_nSliceWidth, m_nHeight * m_nMergeCount, pSliceData[i], m_nSliceStride, null);

                                CogImage[i] = new CogImage8Grey();
                                CogImage[i].SetRoot(GrayCogRoot[i]);
                            }

                        }
                    }

                }
                else
                {
                    m_nWidth = nWidth;
                    m_nHeight = nHeight;
                    m_nPixelFormat = pixelFormat;
                    m_nStride = 4 * ((m_nWidth * pixelFormat + 3) / 4);
                    m_nImageSize = m_nWidth * m_nHeight * pixelFormat;
                    m_Rect = new Rectangle(0, 0, m_nWidth, m_nHeight);
                    m_bHasDummy = m_nWidth * pixelFormat % 4 != 0;
                    pData = Marshal.AllocHGlobal(m_nImageSize);
                    m_pImage = new byte[m_nImageSize];

                    if (m_nSliceCount != 0)
                    {
                        pSliceData = new IntPtr[m_nSliceCount];

                        for (int i = 0; i < m_nSliceCount; i++)
                            pSliceData[i] = Marshal.AllocHGlobal((m_nWidth / m_nSliceCount) * m_nHeight);
                    }

                    InsImage8Root insImage8Root = new InsImage8Root();
                    insImage8Root.Initialize(m_nWidth, m_nHeight, pData, m_nStride, null);
                    InsImage8Grey insImage8Grey = new InsImage8Grey();
                    insImage8Grey.SetRoot(insImage8Root);
                    InsImage = insImage8Grey;
                }

            }
            else if (m_bHasDummy)
            {
                int num = m_nWidth * m_nPixelFormat;
                for (int i = 0; i < m_nHeight; i++)
                {
                    CFunc.memcpy(pData + i * num, pImage + i * m_nStride, num);
                }

                //Marshal.Copy(pData, m_pImage, 0, m_nImageSize);
            }
            else
            {
                CFunc.memcpy(pData, pImage, m_nImageSize);
                //Marshal.Copy(pData, m_pImage, 0, m_nImageSize);
            }

            m_bIsInitialized = true;
        }

        public void Allocate(int nWidth, int nHeight, int pixelFormat, IntPtr pImage, bool bDummy = false, int _nStartPosX = 0, int _nStandardWidth = 0, int _nMergeCount = 0)
        {
            if (!m_bIsInitialized)
            {
                if (m_nLicense == CDefineDLL.COG)
                {
                    m_nWidth = nWidth;
                    m_nHeight = nHeight;
                    m_nPixelFormat = pixelFormat;
                    m_nStride = 4 * ((m_nWidth * pixelFormat + 3) / 4);
                    m_nImageSize = m_nWidth * m_nHeight * pixelFormat;
                    m_nStandardWidth = _nStandardWidth;
                    m_nSliceWidth = _nStandardWidth;
                    m_nMergeCount = _nMergeCount;
                    m_Rect = new Rectangle(0, 0, m_nWidth, m_nHeight);
                    m_bHasDummy = ((m_nWidth * pixelFormat % 4 != 0) ? true : false);
                    pData = Marshal.AllocHGlobal(m_nImageSize);
                    //m_pImage = new byte[m_nImageSize];

                    if (m_sCrruentEqupiment == "ELECTRONIC")
                    {
                        if (m_nSliceCount != 0)
                        {
                            pSliceData = new IntPtr[m_nSliceCount];
                            GrayCogRoot = new CogImage8Root[m_nSliceCount];
                            CogImage = new CogImage8Grey[m_nSliceCount];
                            m_nSliceStride = 4 * (((m_nWidth / m_nSliceCount) * m_nPixelFormat + 3) / 4);

                            for (int i = 0; i < m_nSliceCount; i++)
                            {
                                GrayCogRoot[i] = new CogImage8Root();
                                pSliceData[i] = Marshal.AllocHGlobal(m_nSliceStride * m_nHeight);

                                //m_pImage = new byte[m_nImageSize];
                                GrayCogRoot[i].Initialize((m_nWidth / m_nSliceCount), m_nHeight, pSliceData[i], m_nSliceStride, null);

                                CogImage[i] = new CogImage8Grey();
                                CogImage[i].SetRoot(GrayCogRoot[i]);
                            }

                        }
                    }
                    else
                    {
                        if (m_nSliceCount != 0)
                        {
                            pSliceData = new IntPtr[m_nSliceCount];
                            GrayCogRoot = new CogImage8Root[m_nSliceCount];
                            CogImage = new CogImage8Grey[m_nSliceCount];
                            m_nSliceStride = 4 * ((_nStandardWidth * m_nPixelFormat + 3) / 4);

                            for (int i = 0; i < m_nSliceCount; i++)
                            {
                                GrayCogRoot[i]= new CogImage8Root();

                                pSliceData[i] = Marshal.AllocHGlobal(m_nSliceStride * m_nHeight * m_nMergeCount);

                                //m_pImage = new byte[m_nImageSize];

                                // 4바이트 정렬(행 바이트 수)
                                GrayCogRoot[i].Initialize(m_nSliceWidth, m_nHeight * m_nMergeCount, pSliceData[i], m_nSliceStride, null);

                                CogImage[i] = new CogImage8Grey();
                                CogImage[i].SetRoot(GrayCogRoot[i]);
                            }

                        }
                    }

                }
                else
                {
                    m_nWidth = nWidth;
                    m_nHeight = nHeight;
                    m_nPixelFormat = pixelFormat;
                    m_nStride = 4 * ((m_nWidth * pixelFormat + 3) / 4);
                    m_nImageSize = m_nWidth * m_nHeight * pixelFormat;
                    m_Rect = new Rectangle(0, 0, m_nWidth, m_nHeight);
                    m_bHasDummy = m_nWidth * pixelFormat % 4 != 0;
                    pData = Marshal.AllocHGlobal(m_nImageSize);
                    m_pImage = new byte[m_nImageSize];

                    if (m_nSliceCount != 0)
                    {
                        pSliceData = new IntPtr[m_nSliceCount];

                        for (int i = 0; i < m_nSliceCount; i++)
                            pSliceData[i] = Marshal.AllocHGlobal((m_nWidth / m_nSliceCount) * m_nHeight);
                    }

                    InsImage8Root insImage8Root = new InsImage8Root();
                    insImage8Root.Initialize(m_nWidth, m_nHeight, pData, m_nStride, null);
                    InsImage8Grey insImage8Grey = new InsImage8Grey();
                    insImage8Grey.SetRoot(insImage8Root);
                    InsImage = insImage8Grey;
                }

            }
            else if (m_bHasDummy)
            {
                int num = m_nWidth * m_nPixelFormat;
                for (int i = 0; i < m_nHeight; i++)
                {
                    CFunc.memcpy(pData + i * num, pImage + i * m_nStride, num);
                }

                //Marshal.Copy(pData, m_pImage, 0, m_nImageSize);
            }
            else
            {
                CFunc.memcpy(pData, pImage, m_nImageSize);
                //Marshal.Copy(pData, m_pImage, 0, m_nImageSize);
            }

            m_bIsInitialized = true;
        }


        public void Free()
        {
            if (m_bIsInitialized)
            {
                try
                {
                    Marshal.FreeHGlobal(pData);

                    if (pDataMerge != null)
                        Marshal.FreeHGlobal(pDataMerge);

                    for (int i = 0; i < m_nSliceCount; i++)
                    {
                        if(pSliceData[i] != null)
                            Marshal.FreeHGlobal(pSliceData[i]);
                    }
                        

                    m_bIsInitialized = false;
                }
                catch
                {
                }
            }
        }

        public bool SaveBmp(string sPath, CImage Image)
        {
            try
            {
                lock (m_Lock)
                {
                    using (Bitmap bitmap = new Bitmap(m_nWidth, m_nHeight, m_nStride, cPixelFormats[m_nPixelFormat], Image.pData))
                    {
                        if (PIXEL32 != m_nPixelFormat && PIXEL24 != m_nPixelFormat)
                            MakeGrayScale(bitmap);

                        bitmap.Save(sPath, ImageFormat.Bmp);
                    }

                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool SaveJpeg(string path, CImage _Image, long nQuality = 100L)
        {
            try
            {
                lock (m_Lock)
                {
                    using (Bitmap bitmap = new Bitmap(m_nWidth, m_nHeight, m_nStride, cPixelFormats[m_nPixelFormat], _Image.pData))
                    {
                        if (PIXEL32 != m_nPixelFormat)
                            MakeGrayScale(bitmap);

                        var encoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                        var encParams = new EncoderParameters(1);

                        encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, nQuality);

                        bitmap.Save(path, encoder, encParams);

                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void ImgRead(string sPath, int _ImgIdx, int _StartPosX = 0)
        {
            try
            {
                Bitmap bitmap = new Bitmap(sPath);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                ImageFlags flags = (ImageFlags)bitmap.Flags;
                if (PIXEL32 == GetPixelFormat(bitmap.PixelFormat) && flags.HasFlag(ImageFlags.ColorSpaceGray))
                {
                    CImage cImage = new CImage(m_nLicense, m_sCrruentEqupiment, m_nSliceCount);

                    //cImage.Allocate(bitmap.Width, bitmap.Height, GetPixelFormat(bitmap.PixelFormat), bitmapData.Scan0);
                    //Allocate(int nWidth, int nHeight, int pixelFormat, IntPtr pImage, bool bDummy = false, int nMergeCount = 0, int ReduceSize = 1, bool bMerge = false, int _nStartPosX = 0, int _nStandardWidth = 0)
                    cImage.Allocate(bitmap.Width, bitmap.Height, GetPixelFormat(bitmap.PixelFormat), bitmapData.Scan0, false, 0, 1, false, 0, 0);

                    cImage.Free();
                }
                else
                {
                    if(m_sCrruentEqupiment == "ELECTRONIC")
                    {
                        //Allocate(int nWidth, int nHeight, int pixelFormat, IntPtr pImage, bool bDummy = false, int nMergeCount = 0, int ReduceSize = 1, bool bMerge = false, int _nFirstWidth = 0, int _nStandardWidth = 0)
                        if (_ImgIdx == 0)
                            Allocate(bitmap.Width, bitmap.Height, GetPixelFormat(bitmap.PixelFormat), bitmapData.Scan0, false, m_nMergeCount, m_nReduceSize, true, _StartPosX, m_nStandardWidth);
                        else
                            Allocate(bitmap.Width, bitmap.Height, GetPixelFormat(bitmap.PixelFormat), bitmapData.Scan0, false, _StartPosX, m_nStandardWidth, m_nMergeCount);
                    }
                    else
                    {
                        if (_ImgIdx == 0)
                            Allocate(bitmap.Width, bitmap.Height, GetPixelFormat(bitmap.PixelFormat), bitmapData.Scan0, false, m_nMergeCount, m_nReduceSize, true);
                        else
                            Allocate(bitmap.Width, bitmap.Height, GetPixelFormat(bitmap.PixelFormat), bitmapData.Scan0, false, 0, 0, 0);
                    }
                        
                }
            }
            catch (Exception)
            {
            }
        }

        public void MakeGrayScale(Bitmap bitmap)
        {
            ColorPalette palette = bitmap.Palette;
            for (int i = 0; i < palette.Entries.Length; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }

            bitmap.Palette = palette;
        }

        public bool CheckImageFormat(int nWidth, int nHeight, int nPixelFormat)
        {
            if (nWidth != m_nWidth || nHeight != m_nHeight || nPixelFormat != m_nPixelFormat)
            {
                return false;
            }

            return true;
        }

        public bool Copy(int nWidth, int nHeight, int nPixelFormat, IntPtr pImage, int _nStartposX = 0, int _nStandardWidth = 0, int _ImgCount = 0)
        {
            if (!CheckImageFormat(nWidth, nHeight, nPixelFormat))
            {
                Allocate(nWidth, nHeight, nPixelFormat);
            }

            // 20250918 SHJ 프로젝트 디스플레이 구분, 디스플레이 같은 경우 0번 List 이미지에 계속 카피를 하면서 Slice 이미지를 생성
            // 0번에서 저장 해야 할 pData 가 계속 복사 되다 보니 이미지 저장을 하면 0번째 이미지와 마지막 이미지가 같은 이미지로 저장,
            // ImageCount -> 0 조건을 추가 (첫번째 영상 카피 조건 & Slice 안하는 Cimage List 에서도 사용 가능)
            if(m_sCrruentEqupiment == "DISPLAY")
            {
                if(_ImgCount == 0)
                    CFunc.memcpy(pData, pImage, m_nImageSize);
            }
            else
                CFunc.memcpy(pData, pImage, m_nImageSize);


            if (m_nLicense == CDefineDLL.COG)
            {
                if (m_sCrruentEqupiment == "ELECTRONIC")
                {
                    int tileWidth = m_nWidth / m_nSliceCount;
                    int rowSizeSrc = m_nWidth * nPixelFormat;
                    int rowSizeTile = tileWidth * nPixelFormat;
                    int dstStride = 4 * ((tileWidth * nPixelFormat + 3) / 4);

                    m_nStandardWidth = tileWidth;
                    m_nSliceWidth = tileWidth;
                    m_nSliceStride = dstStride;

                    for (int y = 0; y < m_nHeight; y++)
                    {
                        for (int tileIdx = 0; tileIdx < m_nSliceCount; tileIdx++)
                        {
                            IntPtr srcRow = pImage + y * rowSizeSrc + tileIdx * rowSizeTile;
                            IntPtr dstRow = pSliceData[tileIdx] + y * rowSizeTile;
                            CFunc.memcpy(dstRow, srcRow, rowSizeTile);
                        }
                    }

                    for (int i = 0; i < m_nSliceCount; i++)
                    {
                        GrayCogRoot[i].Initialize(tileWidth, m_nHeight, pSliceData[i], dstStride, null);
                        CogImage[i].SetRoot(GrayCogRoot[i]);
                    }
                }
                else
                {
                    m_nSliceStartPosX = _nStartposX - m_nSliceWidthOffset; //20250915 SHJ 양옆 여유공간 할당 
                    int copyBytesPerRow = m_nSliceStride * nPixelFormat;
                    // 각 Slice 메모리 버퍼 크기 확인
                    for (int i = 0; i < m_nSliceCount; i++)
                    {
                        // (이 부분은 실제 할당된 크기 체크 필요)
                        int expectedAlloc = m_nSliceStride * m_nHeight * (_ImgCount + 1);
                    }

                    for (int i = 0; i < m_nSliceCount; i++)
                    {
                        //20250915 SHJ Slice 시작점 X, 디스플레이 기판은 그룹과 그룹간 사이 dumy 영역이 존재 하기 때문에 dumy 영역을 제외 하고 Slice 하기 위해 Jump 변수를 추가
                        // 디스플레이 외 전기 등 다른 프로젝트에서 Jump 변수에 값을 할당하지 않는 이상 기존 사용에 영향 X 
                        int sliceX = (m_nSliceStartPosX + i * (m_nStandardWidth)) + m_nSliceJump[i]; 

                        for (int row = 0; row < m_nHeight; row++)
                        {
                            int srcOffset = row * m_nStride + sliceX * nPixelFormat;
                            int dstRow = _ImgCount * m_nHeight + row;
                            int dstOffset = dstRow * m_nSliceStride;

                            IntPtr srcPtr = IntPtr.Add(pImage, srcOffset);
                            IntPtr dstPtr = IntPtr.Add(pSliceData[i], dstOffset);

                            CFunc.memcpy(dstPtr, srcPtr, copyBytesPerRow);
                        }
                    }
                }
            }
            else
            {
                CFunc.memcpy(pData, pImage, m_nImageSize);
                GrayInsRoot.Initialize(m_nWidth, m_nHeight, pData, m_nStride, null);
                InsGrayImage.SetRoot(GrayInsRoot);
                InsImage = InsGrayImage;
            }

            return true;
        }

        public bool Copy(int nWidth, int nHeight, int nPixelFormat, ICogImage pImage)
        {
            if (!CheckImageFormat(nWidth, nHeight, nPixelFormat))
            {
                Allocate(nWidth, nHeight, nPixelFormat);
            }

            //CogImage = pImage;
            return true;
        }

        public bool Copy(int nWidth, int nHeight, int nPixelFormat, IInsImage pImage)
        {
            if (!CheckImageFormat(nWidth, nHeight, nPixelFormat))
            {
                Allocate(nWidth, nHeight, nPixelFormat);
            }

            InsImage = pImage;
            return true;
        }

        public bool Copy(CImage SrcImg)
        {
            if (!CheckImageFormat(SrcImg.m_nWidth, SrcImg.m_nHeight, SrcImg.m_nPixelFormat))
            {
                Allocate(SrcImg.m_nWidth, SrcImg.m_nHeight, SrcImg.m_nPixelFormat);
            }

            lock (m_Lock)
            {
                CogImage = SrcImg.CogImage;

                m_nWidth = SrcImg.m_nWidth;
                m_nHeight = SrcImg.m_nHeight;
                m_nPixelFormat = SrcImg.m_nPixelFormat;
                m_nStride = 4 * ((m_nWidth * m_nPixelFormat + 3) / 4);
                m_nImageSize = m_nStride * m_nHeight * m_nPixelFormat;

                m_bHasDummy = ((m_nWidth * m_nPixelFormat % 4 != 0) ? true : false);
                pData = Marshal.AllocHGlobal(m_nImageSize);

                CFunc.memcpy(this.pData, SrcImg.pData, m_nImageSize);
            }

            return true;
        }

        public bool Merge(int nWidth, int nHeight, int nPixelFormat, IntPtr pImage, int nImageCount, bool bReduced = false)
        {
            // 버퍼 준비
            if (!CheckImageFormat(nWidth, nHeight, nPixelFormat))
                Allocate(nWidth, nHeight, nPixelFormat);

            if (!bReduced)
            {
                // 기존 동작(그대로 memcpy로 원본 붙이기)
                // 목적지 오프셋: 타일 인덱스 * 한 타일 바이트
                CFunc.memcpy(this.pDataMerge + (m_nImageSize * nImageCount), pImage, m_nImageSize);

                if (nImageCount == m_nMergeCount - 1)
                {
                    // 최종 처리(필요 시)
                }
                return true;
            }

            // ==== 여기부터 1/16 축소 병합 ====
            // 4x4 블록 평균으로 다운스케일 하며 최종 버퍼에 바로 병합
            unsafe
            {
                byte* src = (byte*)pImage;     // 입력 타일(93712 x 5000, gray)
                byte* dst = (byte*)this.pDataMerge; // 최종 축소 병합 버퍼(23428 x 17500)

                int scale = m_nReduceSize;
                int dstYBase = nImageCount * m_nReduceHeight; // 이 타일이 써야 할 시작 Y(축소 좌표)
                int dstStride = 4 * ((m_nReduceWidth * 1 + 3) / 4);
                for (int sy = 0; sy < m_nHeight; sy += scale)
                {
                    int dy = dstYBase + (sy / scale);
                    long dstRowBase = (long)dy * dstStride;

                    // 가로 4픽셀 → 1픽셀
                    for (int sx = 0; sx < m_nWidth; sx += scale)
                    {
                        int sum = 0;
                        // 4x4 평균(경계는 정확히 나눠떨어지므로 안전 루프 생략해도 됨)
                        for (int yy = 0; yy < scale; yy++)
                        {
                            long srcRowBase = (long)(sy + yy) * m_nStride;
                            int baseX = sx;
                            // 펼쳐서 약간 최적화
                            //sum += src[srcRowBase + baseX + 0];
                            //sum += src[srcRowBase + baseX + 1];
                            //sum += src[srcRowBase + baseX + 2];
                            //sum += src[srcRowBase + baseX + 3];

                            //20250909 SHJ 4*4 고정이 아닌 스케일 사이즈에 맞게 Loop
                            for (int i = 0; i < scale; i++)
                                sum += src[srcRowBase + baseX + i];
                        }
                        // 위에서 세로 4줄 × 가로 4개 = 16개 누적 → 평균
                        //byte val = (byte)(sum >> 4);

                        byte val = (byte)(sum / (scale * scale)); // 20250909 SHJ 4*4 고정이 아닌 스케일에 맞게 평균 계산 

                        long dx = sx / scale;
                        dst[dstRowBase + dx] = val;
                    }
                }
            }

            if (nImageCount == m_nMergeCount - 1)
            {
                // 축소 병합 완료 후 최종 처리(필요 시)
                // 예) UpdateBitmapData() 등

                // 20250820 SHJ Display 일 경우 Merge 가 완료 되는 시점에 Slice Cognex 이미지 생성 
                if (m_sCrruentEqupiment == "DISPLAY")
                {
                    for (int i = 0; i < m_nSliceCount; i++)
                    {
                        GrayCogRoot[i].Initialize(m_nSliceWidth, m_nHeight * m_nMergeCount, pSliceData[i], m_nSliceStride, null);
                        CogImage[i].SetRoot(GrayCogRoot[i]);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 20250915 SHJ 디스플레이에선 Slice 조건 중 nxn 배열 N 개 그룹 형태로 기판이 형성 
        /// N x N 배열의 그룹과 그룹간에 dumy 영역이 존재 하며 dumy 영역을 Jump 하고 이미지를 Slice 하기 위해 옵션 메소드 추가 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="group"></param>
        /// <param name="dumyWidth"></param>
        /// <returns></returns>
        public bool SliceOptionSet(int row, int column, int group, int dumyWidth)
        {
            m_nSliceJump = new int[m_nSliceCount];

            // 20250916 SHJ 가로 Cell 총 갯수가 같아야 할당 하도록 조건문 처리 
            if(m_nSliceCount == (row * group))
            {
                int idx = 0;

                // 기판 그룹 갯수 
                for (int i = 0; i < group; i++)
                {
                    int dumy = dumyWidth * i;

                    for (int x = 0; x < row; x++) // 배열 가로 
                    {
                        m_nSliceJump[idx] = dumy; // SHJ 기판 배열 + 그룹 갯수에 맞게 dumy 영역 점프해야할 구간 입력 
                        idx++;
                    }
                }
            }

            return true;
        }

        public void Draw(Graphics graphics)
        {
            if (!m_bIsInitialized)
            {
                return;
            }

            lock (m_Lock)
            {
                using (Bitmap bitmap = new Bitmap(m_nWidth, m_nHeight, m_nStride, cPixelFormats[m_nPixelFormat], pData))
                {

                    if (cPixelFormats[m_nPixelFormat] == PixelFormat.Format8bppIndexed)
                    {
                        ColorPalette colorPalette = bitmap.Palette;

                        for (int i = 0; i < colorPalette.Entries.Length; i++)
                            colorPalette.Entries[i] = Color.FromArgb(i, i, i);

                        bitmap.Palette = colorPalette;

                        graphics.DrawImage(bitmap, 0, 0);
                    }
                    else
                        graphics.DrawImage(bitmap, 0, 0);
                }
            }
        }

        public void MergeImageDraw(Graphics graphics)
        {
            if (!m_bIsInitialized)
            {
                return;
            }

            lock (m_Lock)
            {
                //using (Bitmap bitmap = new Bitmap(m_nWidth, m_nHeight * m_nMergeCount, m_nStride, cPixelFormats[m_nPixelFormat], pDataMerge))
                //{

                //    if (cPixelFormats[m_nPixelFormat] == PixelFormat.Format8bppIndexed)
                //    {
                //        ColorPalette colorPalette = bitmap.Palette;

                //        for (int i = 0; i < colorPalette.Entries.Length; i++)
                //            colorPalette.Entries[i] = Color.FromArgb(i, i, i);

                //        bitmap.Palette = colorPalette;

                //        graphics.DrawImage(bitmap, 0, 0);
                //    }
                //    else
                //        graphics.DrawImage(bitmap, 0, 0);
                //}

                // 250911 SHJ 현재는 기존 Merge 가 아닌 압축된 Merge 이미지가 출력 되도록 변경
                using (Bitmap bmp = new Bitmap(m_nReduceWidth, m_nReduceHeight * m_nMergeCount, m_nReduceStride, PixelFormat.Format8bppIndexed, pDataMerge))
                {
                    ColorPalette palette = bmp.Palette;
                    for (int j = 0; j < palette.Entries.Length; j++)
                    {
                        palette.Entries[j] = Color.FromArgb(j, j, j);
                    }

                    bmp.Palette = palette;

                    graphics.DrawImage(bmp, 0, 0);
                }
            }

            //lock (m_Lock)
            //{
            //    using (Bitmap bitmap = new Bitmap(m_nWidth, m_nHeight * m_nMergeCount, m_nStride, cPixelFormats[m_nPixelFormat], pDataMerge))
            //    {

            //        if (cPixelFormats[m_nPixelFormat] == PixelFormat.Format8bppIndexed)
            //        {
            //            ColorPalette colorPalette = bitmap.Palette;

            //            for (int i = 0; i < colorPalette.Entries.Length; i++)
            //                colorPalette.Entries[i] = Color.FromArgb(i, i, i);

            //            bitmap.Palette = colorPalette;

            //            graphics.DrawImage(bitmap, 0, 0);
            //        }
            //        else
            //            graphics.DrawImage(bitmap, 0, 0);
            //    }
            //}
        }

        private readonly object CropLock = new object();
        public void Crop(CImage _Image, int nPixelFormat, int nWidth, int nHeight, int Xpos, int Ypos)
        {
            lock (CropLock)
            {
                if (Xpos < 0)
                    Xpos = 0;

                if (Ypos < 0)
                    Ypos = 0;

                if (Xpos + nWidth > _Image.m_nWidth)
                    nWidth = _Image.m_nWidth - Xpos;

                if (Ypos + nHeight > _Image.m_nHeight)
                    nHeight = _Image.m_nHeight - Ypos;


                // 크기 보정 후 Allocate
                if (!CheckImageFormat(nWidth, nHeight, nPixelFormat))
                {
                    Allocate(nWidth, nHeight, nPixelFormat);
                }

                // 크롭 결과 저장 버퍼 (고정 크기 or nWidth * nHeight)
                byte[] cropped = new byte[nWidth * nHeight];

                int srcWidth = _Image.m_nWidth;
                int srcHeight = _Image.m_nHeight;
                int stride = _Image.m_nStride;

                // 안전성 보장: 경계 조건
                if (Xpos < 0 || Ypos < 0 || Xpos + nWidth > srcWidth || Ypos + nHeight > srcHeight)
                    return;

                for (int i = 0; i < nHeight; i++)
                {
                    int srcY = Ypos + i;
                    int dstY = i;

                    IntPtr srcPtr = _Image.pData + srcY * stride + Xpos;
                    byte[] tempRow = new byte[nWidth];

                    // IntPtr → byte[] row 복사
                    Marshal.Copy(srcPtr, tempRow, 0, nWidth);

                    // 전체 대상에 복사
                    Buffer.BlockCopy(tempRow, 0, cropped, dstY * nWidth, nWidth);
                }

                // Bitmap 생성
                Bitmap bmp = new Bitmap(nWidth, nHeight, PixelFormat.Format8bppIndexed);
                Rectangle rect = new Rectangle(0, 0, nWidth, nHeight);
                BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);

                Marshal.Copy(cropped, 0, this.pData, nWidth * nHeight);

                bmp.UnlockBits(bmpData);
            }
        }

        /// <summary>
        /// 20250917 Crop 메소드 분리, 기존 사용하던 메소드와 SliceIndex 를 받는 현재 메소드로 분리, Slice 된 이미지를 Crop 하기 위해 메소드 추가
        /// </summary>
        /// <param name="_Image"></param>
        /// <param name="nPixelFormat"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <param name="Xpos"></param>
        /// <param name="Ypos"></param>
        /// <param name="nSliceIndex"></param>
        public void Crop(CImage _Image, int nPixelFormat, int nWidth, int nHeight, int Xpos, int Ypos, int nSliceIndex)
        {
            lock (CropLock)
            {
                if (Xpos < 0)
                    Xpos = 0;

                if (Ypos < 0)
                    Ypos = 0;

                int srcWidth = _Image.m_nSliceWidth;
                // 20250918 SHJ 전기, 디스플레이 프로젝트 구분 하여 Height 할당 (디스플레이는 Merge 된 상태에서 Slice, 전기는 1개 이미지 Slice Height 길이 차이가 있음)
                int srcHeight = m_sCrruentEqupiment == "ELECTRONIC" ? _Image.m_nHeight : _Image.m_nSliceHeight; 
                int stride = _Image.m_nSliceStride;

                // 250916 SHJ Slice 이미지 기준으로 Crop 해야할 영역 체크
                if (Xpos + nWidth > srcWidth)
                    nWidth = srcWidth - Xpos;
                if (Ypos + nHeight > srcHeight)
                    nHeight = srcHeight - Ypos;

                // 안전성 보장: 경계 조건
                if (Xpos < 0 || Ypos < 0 || Xpos + nWidth > srcWidth || Ypos + nHeight > srcHeight || nHeight < 0 || nWidth < 0)
                    return;

                // 크기 보정 후 Allocate
                if (!CheckImageFormat(nWidth, nHeight, nPixelFormat))
                {
                    Allocate(nWidth, nHeight, nPixelFormat);
                }

                // 크롭 결과 저장 버퍼 (고정 크기 or nWidth * nHeight)
                byte[] cropped = new byte[nWidth * nHeight];

                for (int i = 0; i < nHeight; i++)
                {
                    int srcY = Ypos + i;
                    int dstY = i;

                    // 250916 SHJ Slice 배열중 선택한 Slice PImage 사용
                    IntPtr ptr = _Image.pSliceData[nSliceIndex];
                    IntPtr srcPtr = ptr + srcY * stride + Xpos;
                    byte[] tempRow = new byte[nWidth];

                    // IntPtr → byte[] row 복사
                    Marshal.Copy(srcPtr, tempRow, 0, nWidth);

                    // 전체 대상에 복사
                    Buffer.BlockCopy(tempRow, 0, cropped, dstY * nWidth, nWidth);
                }

                // Bitmap 생성
                Bitmap bmp = new Bitmap(nWidth, nHeight, PixelFormat.Format8bppIndexed);
                Rectangle rect = new Rectangle(0, 0, nWidth, nHeight);
                BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);

                Marshal.Copy(cropped, 0, this.pData, nWidth * nHeight);

                bmp.UnlockBits(bmpData);
            }
        }

        /*
        private Bitmap _cachedBitmap;
        private BitmapData _bitmapData;

        public void InitBitmap()
        {
            _cachedBitmap = new Bitmap(m_nWidth, m_nHeight * m_nMergeCount, PixelFormat.Format8bppIndexed);

            ColorPalette palette = _cachedBitmap.Palette;
            for (int i = 0; i < 256; i++)
                palette.Entries[i] = Color.FromArgb(i, i, i);
            _cachedBitmap.Palette = palette;
        }

        public void UpdateBitmapData()
        {
            int totalHeight = m_nHeight * m_nMergeCount;

            Rectangle rect = new Rectangle(0, 0, m_nWidth, totalHeight);
            _bitmapData = _cachedBitmap.LockBits(rect, ImageLockMode.WriteOnly, _cachedBitmap.PixelFormat);

            CFunc.memcpy(_bitmapData.Scan0, pDataMerge, m_nWidth * totalHeight);

            _cachedBitmap.UnlockBits(_bitmapData);
        }
        public void DrawCachedBitmap(Graphics graphics)
        {
            lock (m_Lock)
            {
                graphics.DrawImage(_cachedBitmap, 0, 0);
            }
        }
        */
    }

    public static class CUtils
    {
        /*------------------------------------------------------------------------------------
        * Date : 2023 05 16
        * Author : Peter.Lee
        * Function : 
        * Description : UI Animate Effect
        ------------------------------------------------------------------------------------*/
        public static class AnimateEffect
        {
            public const int AW_HOR_POSITIVE = 0X1;
            public const int AW_HOR_NEGATIVE = 0X2;
            public const int AW_VER_POSITIVE = 0X4;
            public const int AW_VER_NEGATIVE = 0X8;
            public const int AW_CENTER = 0X10;
            public const int AW_HIDE = 0X10000;
            public const int AW_ACTIVATE = 0X20000;
            public const int AW_SLIDE = 0X40000;
            public const int AW_BLEND = 0X80000;

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int AnimateWindow(IntPtr hwand, int dwTime, int dwFlags);
        }

        public static int IntTryParse(string str)
        {
            int n;
            n = int.TryParse(str, out n) ? n : 0;
            return n;
        }



        /*------------------------------------------------------------------------------------
        * Date : 2023 05 16
        * Author : Peter.Lee
        * Function : GetValue(String Section, String Key, String iniPath)
        * Description : Text File Data Load 처리
        ------------------------------------------------------------------------------------*/
        public static String GetValue(String Section, String Key, String iniPath)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);
            return temp.ToString();
        }

        /*------------------------------------------------------------------------------------
         * Date : 2023 05 16
         * Author : Peter.Lee
         * Function : SetValue(String Section, String Key, String Value, String iniPath)
         * Description : Text File Data Save 처리
         ------------------------------------------------------------------------------------*/
        public static bool SetValue(String Section, String Key, String Value, String iniPath)
        {
            bool bRet = WritePrivateProfileString(Section, Key, Value, iniPath);
            return WritePrivateProfileString(Section, Key, Value, iniPath);
        }


        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileString(
                                    String section,
                                    String key,
                                    String def,
                                    StringBuilder retVal,
                                    int size,
                                    String filePath);

        [DllImport("kernel32.dll")]
        public static extern bool WritePrivateProfileString(
                                    String section,
                                    String key,
                                    String val,
                                    String filePath);
    }

    public class MTickTimer
    {
        Stopwatch Timer;
        bool bIsTimerStarted;

        public MTickTimer()
        {
            Timer = new Stopwatch();
        }
        public int StartTimer()
        {
            Timer.Reset();
            Timer.Start();
            bIsTimerStarted = true;

            return SUCCESS;
        }

        public int StopTimer()
        {
            //if (bIsTimerStarted == false) return;
            Timer.Stop();
            bIsTimerStarted = false;
            return SUCCESS;
        }

        public double GetElapsedTime(ETimeType type = ETimeType.SECOND)
        {
            double gap = Timer.ElapsedTicks;

            switch (type)
            {
                case ETimeType.NANOSECOND:
                    break;

                case ETimeType.MICROSECOND:
                    gap /= 10.0;
                    break;

                case ETimeType.MILLISECOND:
                    gap = Timer.ElapsedMilliseconds;
                    break;

                case ETimeType.SECOND:
                    gap = Timer.ElapsedMilliseconds / (1000.0);
                    break;

                case ETimeType.MINUTE:
                    gap = Timer.ElapsedMilliseconds / (1000.0 * 60);
                    break;

                case ETimeType.HOUR:
                    gap = Timer.ElapsedMilliseconds / (1000.0 * 60 * 60);
                    break;
            }
            return gap;
        }

        public string GetElapsedTime_Text(bool bShowUnit = true, ETimeType type = ETimeType.SECOND)
        {
            string unit = "sec";

            switch (type)
            {
                case ETimeType.NANOSECOND:
                    unit = "nanosec";
                    break;

                case ETimeType.MICROSECOND:
                    unit = "microsec";
                    break;

                case ETimeType.MILLISECOND:
                    unit = "millisec";
                    break;

                case ETimeType.SECOND:
                    unit = "sec";
                    break;

                case ETimeType.MINUTE:
                    unit = "min";
                    break;

                case ETimeType.HOUR:
                    unit = "hour";
                    break;
            }

            string str = $"{GetElapsedTime(type):0.000} {unit}";
            return str;
        }

        public bool LessThan(double CompareTime, ETimeType type = ETimeType.SECOND)
        {
            double gap = GetElapsedTime(type);
            if (gap < CompareTime) return true;
            else return false;
        }

        public bool MoreThan(double CompareTime, ETimeType type = ETimeType.SECOND)
        {
            double gap = GetElapsedTime(type);
            if (gap > CompareTime) return true;
            else return false;
        }

        public override string ToString()
        {
            return $"{GetElapsedTime(ETimeType.HOUR)}:{GetElapsedTime(ETimeType.MINUTE) % 60:00}:{GetElapsedTime(ETimeType.SECOND) % 60:00}.{GetElapsedTime(ETimeType.MILLISECOND) % 1000:000}";
        }
    }
}

