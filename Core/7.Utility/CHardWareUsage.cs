using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Management;


namespace Core.Utility
{
    public class CHardWareUsage
    {
        private PerformanceCounter CPUUsage = new PerformanceCounter("Processor Information", "% Processor Utility", "0,_Total");

        private PerformanceCounter RAMUsage = new PerformanceCounter("Memory", "Available MBytes");
        private DriveInfo[] diDrives = DriveInfo.GetDrives();
        private ManagementClass mc = new ManagementClass("Win32_OperatingSystem");
        private PerformanceCounterCategory category = new PerformanceCounterCategory("GPU Engine");
        public CHardWareUsage()
        {

        }
        
        public void GetHardWareData(ref double _CPUUage, ref double _RAMUsage, ref double[] _HardDisk, ref double _GPU)
        {
            Usage(ref _CPUUage, ref _RAMUsage, ref _HardDisk, ref _GPU);
        }

        private void Usage(ref double _CPUUage, ref double _RAMUsage, ref double[] _HardDisk, ref double _GPU)
        {
            _CPUUage = Math.Round(CPUUsage.NextValue(), 4);

            ManagementClass MangementCls = new ManagementClass("Win32_OperatingSystem");
            ManagementObjectCollection ManageMenetObj = MangementCls.GetInstances();

            int nMemoryTotalMB = 0;
            int nMemoryFreeMB = 0;
            foreach(ManagementObject mObj in ManageMenetObj)
            {
                nMemoryTotalMB = int.Parse(mObj["TotalVisibleMemorySize"].ToString());
                nMemoryFreeMB = int.Parse(mObj["FreePhysicalMemory"].ToString());
            }

            double nTemp = (nMemoryFreeMB / 1000000000.0) / (nMemoryTotalMB / 1000000000.0);
            _RAMUsage = 100.0 - (nTemp * 100);


            for (int i = 0; i < 2; i++)
            {
                int maxc = (int)(diDrives[i].TotalSize / 1000000);
                int cst = (int)((diDrives[i].TotalSize - diDrives[i].AvailableFreeSpace) / 1000000);
                double nPercent = ((float)((float)cst / (float)maxc) * 100);

                _HardDisk[i] = nPercent;
            }

            
            //string[] counterNames = category.GetInstanceNames();
            //
            //var gpuCounters = counterNames
            //.Where(counterName => counterName.EndsWith("engtype_3D"))
            //.SelectMany(counterName => category.GetCounters(counterName))
            //.Where(counter => counter.CounterName.Equals("Utilization Percentage"))
            //.ToList();
            //
            //gpuCounters.ForEach(x => x.NextValue());
            //
            //var result = gpuCounters.Sum(x => x.NextValue());
            //
            //var gpuUsage = result;
            //
            //_GPU = gpuUsage;
        }
    }
}
