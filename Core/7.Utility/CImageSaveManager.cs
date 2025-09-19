using Core.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenCvSharp;

using static Core.Program;

namespace Core.Utility
{
    /// <summary>
    /// 25.02.19 LYK 
    ///  이미지 저장 요청을 비동기적으로 처리하는 매니저 클래스
    /// - 요청은 큐에 쌓이고
    /// - 백그라운드 스레드에서 저장 처리
    /// - 이미지 저장 성능을 개선하고 UI 블로킹을 방지
    /// </summary>
    public class CImageSaverManager
    {
        /// <summary>
        /// 25.02.19 LYK 이미지 저장 요청을 담는 큐 (Thread-safe)
        /// </summary>
        private readonly ConcurrentQueue<ImageSaveRequest> _imageQueue = new ConcurrentQueue<ImageSaveRequest>();

        /// <summary>
        /// 25.02.19 LYK 취소 토큰: 종료 신호를 위한 객체
        /// </summary>
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        /// <summary>
        /// 25.02.19 LYK 백그라운드 작업 태스크 (무한 루프에서 큐를 처리)
        /// </summary>
        private readonly Task _workerTask;

        /// <summary>
        /// 25.02.19 LYK 큐에 새 항목이 추가되었음을 알리는 이벤트
        /// </summary>
        private readonly ManualResetEventSlim _queueNotifier = new ManualResetEventSlim(false);

        /// <summary>
        /// 25.02.19 LYK 이미지 저장 중 동기화용 lock 객체
        /// </summary>
        private readonly object lockObj = new object();

        /// <summary>
        /// 25.02.19 LYK 생성자: 백그라운드 작업을 시작
        /// </summary>
        public CImageSaverManager()
        {
            _workerTask = Task.Factory.StartNew(ProcessQueue, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// 25.02.19 LYK  이미지 저장 요청을 큐에 추가
        /// </summary>
        /// <param name="path">저장 경로</param>
        /// <param name="image">CImage 객체</param>
        /// <param name="quality">품질 (기본 100)</param>
        /// <param name="isGray">그레이스케일 여부</param>
        public void EnqueueSaveRequest(string path, CImage image, long quality = 100L, bool isGray = true, bool isMerge = false)
        {
            _imageQueue.Enqueue(new ImageSaveRequest(path, image, quality, isGray, isMerge));
            _queueNotifier.Set();  // 새 작업 알림
        }

        /// <summary>
        /// 25.02.19 LYK 큐를 지속적으로 확인하고, 저장 요청을 처리하는 메서드 (백그라운드)
        /// </summary>
        private async Task ProcessQueue()
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                _queueNotifier.Wait();   //25.02.19 LYK  큐에 항목이 있을 때까지 대기
                _queueNotifier.Reset();  //25.02.19 LYK  알림 리셋

                List<Task> tasks = new List<Task>();

                //25.02.19 LYK  큐에 있는 모든 요청을 꺼내 저장 작업으로 넘김
                while (_imageQueue.TryDequeue(out var request))
                {
                    tasks.Add(Task.Run(() => SaveJpeg(request.Path, request.Image, request.Quality, request.IsGray, request.IsMerge)));

                    //await Task.Delay(1);
                    //SaveJpeg(request.Path, request.Image, request.Quality, request.IsGray);

                    //if (tasks.Count >= 10)
                    //{
                        //await Task.Delay(10);
                    //    break;
                    //}
                }

                await Task.WhenAll(tasks); //25.02.19 LYK  모든 이미지 저장이 완료될 때까지 대기
            }
        }

        /// <summary>
        /// 25.02.19 LYK JPEG 포맷으로 이미지 저장
        /// </summary>
        /// <param name="path">파일 경로</param>
        /// <param name="_Image">CImage 객체</param>
        /// <param name="nQuality">품질 (기본 100)</param>
        /// <param name="isGray">흑백 여부</param>
        private bool SaveJpeg(string path, CImage _Image, long nQuality = 100L, bool isGray = true, bool isMerge = false)
        {
            try
            {
                lock (lockObj) // 25.02.19 LYK lock 필요
                {

                    // 특정 크기일 경우 (예: 5120 x 5120), 8비트 흑백 이미지로 처리
                    if (_Image.m_nPixelFormat == 1)
                    {
                        if (!isMerge) // Image 원본 저장 
                        {
                            using (Bitmap bmp = new Bitmap(_Image.m_nWidth, _Image.m_nHeight, _Image.m_nStride, PixelFormat.Format8bppIndexed, _Image.pData))
                            {
                                // 그레이 팔레트 설정
                                ColorPalette palette = bmp.Palette;
                                for (int i = 0; i < palette.Entries.Length; i++)
                                {
                                    palette.Entries[i] = Color.FromArgb(i, i, i);
                                }

                                bmp.Palette = palette;

                                // 250910 SHJ 입력 받은 스트링 데이터를 기준으로 JPG, BMP 구분 저장 
                                if (path.Contains("jpg"))
                                    bmp.Save(path, ImageFormat.Jpeg);
                                else
                                    bmp.Save(path, ImageFormat.Bmp);

                            }


                        }
                        else // 20250911 SHJ 압축된 Merge Image 저장 
                        {
                            using (Bitmap bmp = new Bitmap(_Image.m_nReduceWidth, _Image.m_nReduceHeight * theRecipe.MergeImageCount, _Image.m_nReduceStride, PixelFormat.Format8bppIndexed, _Image.pDataMerge))
                            {
                               
                                // 그레이 팔레트 설정
                                ColorPalette palette = bmp.Palette;
                                for (int i = 0; i < palette.Entries.Length; i++)
                                {
                                    palette.Entries[i] = Color.FromArgb(i, i, i);
                                }

                                bmp.Palette = palette;

                                bmp.Save(path, ImageFormat.Png);

                            }

                        }
                    }
                    else
                    {
                        // 일반 RGB 24비트 이미지 처리
                        using (Bitmap bmp = new Bitmap(_Image.m_nWidth, _Image.m_nHeight, _Image.m_nStride, PixelFormat.Format24bppRgb, _Image.pData))
                        {
                            bmp.Save(path, ImageFormat.Jpeg);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving image: {ex.Message}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 25.02.19 LYK 백그라운드 작업 종료 및 정리
        /// </summary>
        public void Shutdown()
        {
            _cts.Cancel();         // 25.02.19 LYK 작업 종료 요청
            _queueNotifier.Set();  // 25.02.19 LYK 대기 중인 루프 깨우기
            _workerTask.Wait();    // 25.02.19 LYK 작업 완료까지 대기
        }
    }

    /// <summary>
    /// 25.02.19 LYK 이미지 저장 요청 정보를 담는 클래스
    /// </summary>
    public class ImageSaveRequest
    {
        public string Path { get; }
        public CImage Image { get; }
        public long Quality { get; }
        public bool IsGray { get; }
        public bool IsMerge { get; } // 20250909 SHJ Merge 전용 Bool 변수 추가

        public ImageSaveRequest(string path, CImage image, long quality, bool isGray, bool isMerge)
        {
            Path = path;
            Image = image;
            Quality = quality;
            IsGray = isGray;
            IsMerge = isMerge;
        }
    }
}
