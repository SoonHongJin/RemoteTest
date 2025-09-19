using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using global::System.Collections.Concurrent;
using global::System.IO;
using global::System.Threading;

using Core;
using static Core.Program;

namespace Core
{
    /// <summary>
    /// 25.02.19 LYK CLogger
    /// 멀티 스레딩 환경에서 안전하게 로그를 기록하고, 주기적으로 파일로 저장하는 로거 클래스
    /// </summary>
    public class CLogger : IDisposable
    {
        /// <summary>
        /// 25.02.19 LYK 로그 타입별 큐를 저장하는 ConcurrentDictionary (스레드 안전)
        /// </summary>
        private readonly ConcurrentDictionary<string, ConcurrentQueue<string>> logQueues;

        /// <summary>
        /// 25.02.19 LYK 로그 기록 종료를 위한 취소 토큰
        /// </summary>
        private readonly CancellationTokenSource cts;

        /// <summary>
        /// 25.02.19 LYK 로그 파일이 저장될 디렉터리 경로들 (log, interface, mes)
        /// </summary>
        private string[] logDirectory;

        /// <summary>
        /// 25.02.19 LYK 백그라운드에서 로그를 기록하는 Task
        /// </summary>
        private Task logTask;

        /// <summary>
        /// 25.02.19 LYK 현재 로그가 기록되고 있는 날짜 (일자 변경 감지용)
        /// </summary>
        private DateTime currentLogDate;

        /// <summary>
        /// 25.02.19 LYK 파일 접근 시 충돌 방지를 위한 lock
        /// </summary>
        private readonly object fileLock = new object();

        //25.02.19 LYK 로그 저장 간격 (ms 단위) - 1000ms = 1초
        private const int FlushInterval = 1000;

        //25.02.19 LYK 로거 실행 여부
        private bool isRunning = false;

        /// <summary>
        ///25.02.19 LYK 생성자. 
        ///로그 디렉토리와 로그 타입 배열을 받아 로그 큐 초기화
        /// </summary>
        public CLogger(string[] logPath, params string[] logTypes)
        {
            logDirectory = logPath;
            logQueues = new ConcurrentDictionary<string, ConcurrentQueue<string>>();

            foreach (var logType in logTypes)
            {
                logQueues[logType] = new ConcurrentQueue<string>();
            }

            cts = new CancellationTokenSource();
        }

        /// <summary>
        /// 25.02.19 LYK 로그 초기화 
        /// (시작 날짜 설정, 실행 여부 플래그 설정)
        /// </summary>
        public void Initialize()
        {
            if (isRunning)
                return;

            currentLogDate = DateTime.Now;
            isRunning = true;
        }

        /// <summary>
        /// 25.02.19 LYK StartLogging
        /// 로그 기록 시작 (백그라운드 태스크 실행)
        /// </summary>
        public void StartLogging()
        {
            if (!isRunning)
                Initialize();

            logTask = Task.Run(ProcessLogQueue, cts.Token);
        }

        /// <summary>
        /// 25.02.19 LYK 
        /// 로그 기록 중지 (태스크 정지 및 리소스 해제)
        /// </summary>
        public void StopLogging()
        {
            if (!isRunning)
                return;

            cts.Cancel();             //25.02.19 LYK 작업 중단 요청
            logTask?.Wait();          //25.02.19 LYK 태스크가 종료될 때까지 대기
            isRunning = false;
        }

        /// <summary>
        /// 25.02.19 LYK
        /// 로그 메시지를 로그 큐에 추가
        /// </summary>
        /// <param name="logType">로그 타입 (예: SYSTEM, MES, INTERFACE)</param>
        /// <param name="message">기록할 메시지</param>
        public void Logging(string logType, string message)
        {
            if (!isRunning || !logQueues.ContainsKey(logType))
                return;

            //25.02.19 LYK 타임스탬프 포함한 로그 메시지 생성
            string logMessage = $"{DateTime.Now:hh:mm:ss.ffff(tt)} : {message}";
            logQueues[logType].Enqueue(logMessage);
        }

        /// <summary>
        /// 25.02.19 LYK ProcessLogQueue
        /// 로그 큐를 주기적으로 체크하여 로그 파일에 기록하는 백그라운드 작업
        /// </summary>
        private async Task ProcessLogQueue()
        {
            while (!cts.Token.IsCancellationRequested)
            {
                try
                {
                    DateTime now = DateTime.Now;

                    //25.02.19 LYK 날짜 변경 시 로그 디렉터리 갱신
                    if (now.Date != currentLogDate.Date)
                    {
                        currentLogDate = now;
                        logDirectory[0] = string.Format("{0}\\{1:0000}\\{2:00}", DEF_SYSTEM.DEF_FOLDER_PATH_LOG, currentLogDate.Year, currentLogDate.Month);
                        logDirectory[1] = string.Format("{0}\\{1:0000}\\{2:00}", DEF_SYSTEM.DEF_FOLDER_PATH_INTERFACELOG, currentLogDate.Year, currentLogDate.Month);
                        logDirectory[2] = string.Format("{0}\\{1:0000}\\{2:00}", DEF_SYSTEM.DEF_FOLDER_PATH_MESLOG, currentLogDate.Year, currentLogDate.Month);
                    }

                    //25.02.19 LYK 각 로그 타입별 큐 확인 및 파일로 기록
                    foreach (var logType in logQueues.Keys)
                    {
                        if (logQueues[logType].IsEmpty)
                            continue;

                        //25.02.19 LYK  로그 파일 경로 획득
                        string logPath = GetLogFilePath(logType);

                        //25.02.19 LYK  로그 디렉터리가 존재하지 않으면 생성
                        string dirPath = logDirectory[(int)Enum.Parse(typeof(DEF_SYSTEM.LOGTYPE_ENUM), logType)];
                        if (!Directory.Exists(dirPath))
                            Directory.CreateDirectory(dirPath);

                        if (!Directory.Exists(logPath))
                            Directory.CreateDirectory(Path.GetDirectoryName(logPath));

                        //25.02.19 LYK  로그 파일에 쓰기 (949: EUC-KR 인코딩)
                        using (StreamWriter logOutfile = new StreamWriter(logPath, true, Encoding.GetEncoding(949)))
                        {
                            while (logQueues[logType].TryDequeue(out string logMessage))
                            {
                                logOutfile.WriteLine(logMessage);
                            }
                        }
                    }

                    //25.02.19 LYK  일정 간격 대기
                    await Task.Delay(FlushInterval, cts.Token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Log Error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 25.02.19 LYK GetFilePath
        /// 로그 타입에 해당하는 파일 경로 반환
        /// </summary>
        private string GetLogFilePath(string logType)
        {
            string logFileName = $"{logType}_{currentLogDate:yyyy_MM_dd}.txt";
            return Path.Combine(logDirectory[(int)Enum.Parse(typeof(DEF_SYSTEM.LOGTYPE_ENUM), logType)], logFileName);
        }

        /// <summary>
        /// 25.02.19 LYK Dispose
        /// 리소스 정리 (Dispose 패턴)
        /// </summary>
        public void Dispose()
        {
            StopLogging();
            cts?.Dispose();
        }
    }
}
