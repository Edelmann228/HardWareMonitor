using System;
using System.Management;
using HardWareMonitor.Models;

namespace HardWareMonitor.Services
{
    public class GpuMonitor
    {
        // Получает информацию о видеокарте: название и объем видеопамяти
        public GpuInfo GetGpuInfo()
        {
            GpuInfo gpuInfo = new GpuInfo();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            foreach (ManagementObject obj in searcher.Get())
            {
                gpuInfo.Name = obj["Name"] as string ?? "Unknown";
                gpuInfo.VideoMemory = Convert.ToInt64(obj["AdapterRAM"] ?? 0);
            }

            return gpuInfo;
        }
    }
}