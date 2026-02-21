using System;
using System.Management;
using HardWareMonitor.Models;

namespace HardWareMonitor.Services
{
    public class MemoryMonitor
    {
        // Получает информацию об оперативной памяти: общий объем, доступно, использование и модули памяти
        public MemoryInfo GetMemoryInfo()
        {
            MemoryInfo memoryInfo = new MemoryInfo();

            ManagementObjectSearcher osSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject obj in osSearcher.Get())
            {
                long totalKB = Convert.ToInt64(obj["TotalVisibleMemorySize"] ?? 0);
                long freeKB = Convert.ToInt64(obj["FreePhysicalMemory"] ?? 0);
                memoryInfo.TotalMemoryBytes = totalKB * 1024;
                memoryInfo.AvailableMemoryBytes = freeKB * 1024;
                memoryInfo.UsagePercentage = ((double)(totalKB - freeKB) / totalKB) * 100;
            }

            ManagementObjectSearcher memSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
            foreach (ManagementObject obj in memSearcher.Get())
            {
                MemoryModule module = new MemoryModule();
                module.Capacity = Convert.ToInt64(obj["Capacity"] ?? 0);
                module.Manufacturer = obj["Manufacturer"] as string ?? "Unknown";
                module.Speed = Convert.ToInt32(obj["Speed"] ?? 0);
                memoryInfo.Modules.Add(module);
            }

            return memoryInfo;
        }
    }
}