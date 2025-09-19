using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using Core;
using Core.DataProcess;

namespace Core
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        /// 
        
        //전역 변수
        public static CMainSystem theMainSystem = CMainSystem.GetInstance();
        public static CRecipe theRecipe = CRecipe.GetInstance();

        [STAThread]
        static void Main()
        {
            // 중복 실행 방지 
            if (IsExistProcess(System.Diagnostics.Process.GetCurrentProcess().ProcessName))
            {
                MessageBox.Show("Instance already running");
                return;
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

        }


        static bool IsExistProcess(string processName)
        {
            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcesses();
            int cnt = 0;

            //프로세스명으로 확인해서 동일한 프로세스 개수가 2개이상인지 확인합니다. 
            //현재실행하는 프로세스도 포함되기때문에 1보다커야합니다.
            foreach (var p in process)
            {
                if (p.ProcessName == processName)
                    cnt++;
                if (cnt > 1)
                    return true;
            }
            return false;
        }
    }
}
