using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static Core.Program;

namespace Core.Utility
{
    /// <summary>
    /// 20250918 SHJ 데이터 자동삭제 백그라운드에서 실행할 클래스 생성 
    /// UI 및 검사 성능에 영향을 주지 않도록 비동기로 백그라운드에서 실행 
    /// 
    /// </summary>
    public class CDataDeleteManager
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource(); // 취소 토큰: 종료 신호를 위한 객체

        private readonly Task _workerTask; // 백그라운드 작업 태스크 (무한 루프에서 삭제 처리)

        private readonly ManualResetEventSlim _queueNotifier = new ManualResetEventSlim(false); // 작업 실행 이벤트

        private readonly object m_lock = new object();

        private const int Interval = 1000;

        private string[][] DeleteFolderPath = new string[4][]; // 삭제 폴더 경로

        private double m_dCurDrive = 0; // 현재 드라이버 용량 -> 드라이버 용량 변화량이 크지 않을 것으로 보여 변수로 사용

        public CDataDeleteManager()
        {
            _workerTask = Task.Factory.StartNew(AutoDelete, TaskCreationOptions.LongRunning);
        }

        public void Execute(double HDD)
        {
            m_dCurDrive = HDD;
            _queueNotifier.Set();  // 새 작업 알림
        }

        private async Task AutoDelete()
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                _queueNotifier.Wait();   
                _queueNotifier.Reset();

                try
                {
                    lock (m_lock) // Lock
                    {
                        // 삭제할 데이터 경로 
                        DeleteFolderPath[0] = Directory.GetDirectories(DEF_SYSTEM.DEF_FOLDER_PATH_IMAGE); // Image
                        DeleteFolderPath[1] = Directory.GetDirectories(DEF_SYSTEM.DEF_FOLDER_PATH_CSV); // Csv
                        DeleteFolderPath[2] = Directory.GetDirectories(DEF_SYSTEM.DEF_FOLDER_PATH_DEFECTIMAGE); //Crop Image
                        DeleteFolderPath[3] = Directory.GetDirectories(DEF_SYSTEM.DEF_FOLDER_PATH_LOG); //System Log

                        // 폴더 오래된 순으로 정렬
                        for (int i = 0; i < DeleteFolderPath.Length; i++)
                        {
                            DeleteFolderPath[i].OrderBy(folder => new FileInfo(folder).LastWriteTime);
                        }

                        if (theRecipe.m_nAutoDeleteMode == 0)
                            DateCheckDelete();
                        else if (theRecipe.m_nAutoDeleteMode == 1)
                            DriveCheckDelete(m_dCurDrive);
                    }
                }
                catch(Exception ex)
                {
                    Debug.WriteLine($"Error Data Delete: {ex.Message}");
                }

                await Task.Delay(Interval);
            }
        }

        public void Shutdown()
        {
            _cts.Cancel();         // 작업 종료 요청
            _queueNotifier.Set();  // 대기 중인 루프 깨우기
            _workerTask.Wait();    // 작업 완료까지 대기
        }

        private void DateCheckDelete()
        {
            // 폴더 오래된 순으로 정렬
            for (int i = 0; i < DeleteFolderPath.Length; i++)
            {
                DeleteFolderPath[i].OrderBy(folder => new FileInfo(folder).LastWriteTime);
            }

            for (int i = 0; i < DeleteFolderPath.Length; i++)
            {
                // 날짜 기준으로 삭제
                if (DeleteFolderPath[i].Length > 0) // 데이터 폴더 내부 갯수가 있을 경우 진입 
                {
                    if (!DeleteFolderPath[i][0].Contains(DEF_SYSTEM.DEF_FOLDER_PATH_LOG)) // 날짜 폴더는 바로 삭제 처리 
                    {
                        int DeleteCnt = DeleteFolderPath[i].Length - theRecipe.m_nDateLimit; // 현재 보유하고 있는 날짜 폴더 갯수와 설정 갯수 비교 

                        if (DeleteCnt > 0) // 삭제 해야 할 갯수가 1개 이상 되면 삭제 시작 
                        {
                            for (int j = 0; j < DeleteCnt; j++)
                            {
                                System.IO.DirectoryInfo Folerdir = new System.IO.DirectoryInfo(DeleteFolderPath[i][j]);
                                Folerdir.Delete(true);
                            }
                        }
                    }
                    else // 하위 파일이 있는 로그 폴더 는 내부 일 단위 Text 파일들을 정리 하여 날짜에 맞게 삭제 처리 
                    {
                        // 현재 연도 폴더 내부 하위 파일들 검사 
                        IEnumerable<string> allFiles = Directory.EnumerateFiles(DeleteFolderPath[i][0], "*.*", SearchOption.AllDirectories);

                        // 파일이 없으면 폴더 삭제 처리 
                        if (allFiles.ToList().Count == 0)
                        {
                            var Folerdir = new System.IO.DirectoryInfo(DeleteFolderPath[i][0]);
                            Folerdir.Delete(true);

                            if (DeleteFolderPath[i].Length >= 1) // 다음 연도 파일 체크 
                                allFiles = Directory.EnumerateFiles(DeleteFolderPath[i][1], "*.*", SearchOption.AllDirectories);
                            else
                                continue; // 다음 연도 파일 없는 경우 현재 루프 점프 
                        }

                        var FileList = allFiles.OrderBy(file => new FileInfo(file).LastWriteTime).ToList();

                        int DeleteCnt = FileList.Count - theRecipe.m_nDateLimit; // 현재 보유하고 있는 날짜 폴더 갯수와 설정 갯수 비교 

                        if (DeleteCnt > 0)
                        {
                            for (int j = 0; j < DeleteCnt; j++)
                            {
                                FileInfo fileinfo = new FileInfo(FileList[j]);
                                fileinfo.Delete();
                            }
                        }
                    }
                }

            }
        }

        private void DriveCheckDelete(double CurHDD)
        {
            for (int i = 0; i < DeleteFolderPath.Length; i++)
            {
                // D드라이브 용량 기준 으로 삭제 
                // 20250917 SHJ 
                // 타이머 Tick 될 떄 마다 현재 드라이브 용량을 받아서 레시피 설정 값과 비교 하여 제일 오래된 자료를 삭제 해주는 방식으로 구성 
                // Image, Defect 이미지 는 세부 폴더 파일 까지 확인해서 삭제 하는 방법과 가장 오래된 날짜 삭제 하는 방법 고민중 오래된 날짜 삭제 하는걸로 사용
                if (CurHDD >= theRecipe.m_dDriveLimit)
                {

                    if (DeleteFolderPath[i].Length > 0) // 데이터 폴더 내부 갯수가 있을 경우 진입 
                    {

                        if (!DeleteFolderPath[i][0].Contains(DEF_SYSTEM.DEF_FOLDER_PATH_LOG)) // 로그 폴더가 아닌 경우 삭제 처리 
                        {
                            System.IO.DirectoryInfo Folerdir = new System.IO.DirectoryInfo(DeleteFolderPath[i][0]); // 삭제 할 가장 오래된 폴더 
                            Folerdir.Delete(true);
                        }
                        else // 로그 폴더일 경우 폴더 내부 연도 단위 그 하위 월 단위(07,08,09) 폴더로 형성 되어 있으며 월 단위 폴더 내부 로그 파일이 있음 
                        {
                            // 현재 연도 폴더 내부 하위 파일들 검사 
                            IEnumerable<string> allFiles = Directory.EnumerateFiles(DeleteFolderPath[i][0], "*.*", SearchOption.AllDirectories);

                            // 파일이 없으면 폴더 삭제 처리 
                            if (allFiles.ToList().Count == 0)
                            {
                                System.IO.DirectoryInfo Folerdir = new System.IO.DirectoryInfo(DeleteFolderPath[i][0]); // 삭제 할 가장 오래된 폴더 
                                Folerdir.Delete(true);

                                if (DeleteFolderPath[i].Length >= 1) // 다음 연도 파일 체크 
                                    allFiles = Directory.EnumerateFiles(DeleteFolderPath[i][1], "*.*", SearchOption.AllDirectories);
                                else
                                    continue; // 다음 연도 파일 없는 경우 현재 루프 점프 
                            }

                            // FileInfo 형식으로 오래된 순서로 파일 정렬 
                            var FileList = allFiles.OrderBy(file => new FileInfo(file).LastWriteTime).ToList();

                            FileInfo fileinfo = new FileInfo(FileList[0]);
                            fileinfo.Delete();
                        }
                    }

                }
            }
        }
    }
}
