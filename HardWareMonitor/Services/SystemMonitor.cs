using System;
using System.Management;
using HardWareMonitor.Models;

namespace HardWareMonitor.Services
{
    public class SystemMonitor
    {
        // Получает информацию об операционной системе: название, версия и имя компьютера
        public SystemInfo GetSystemInfo()
        {
            SystemInfo systemInfo = new SystemInfo();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject obj in searcher.Get())
            {
                systemInfo.OSName = obj["Caption"] as string ?? "Unknown";
                systemInfo.OSVersion = obj["Version"] as string ?? "Unknown";
            }

            systemInfo.MachineName = Environment.MachineName;

            return systemInfo;
        }
    }
}