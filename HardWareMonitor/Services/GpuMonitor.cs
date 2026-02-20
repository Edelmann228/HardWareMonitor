using System;
using System.Management;
using HardWareMonitor.Models;

namespace HardWareMonitor.Services
{
    public class GpuMonitor
    {
        public GpuInfo GetGpuInfo()
        {
            GpuInfo gpuInfo = new GpuInfo();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            foreach (ManagementObject obj in searcher.Get())
            {
                gpuInfo.Name = obj["Name"] as string ?? "Unknown";
                gpuInfo.VideoMemory = Convert.ToInt64(obj["AdapterRAM"] ?? 0);
                // GPU load: WMI doesn't directly provide; for simplicity, assume 0 or use external if needed. Here, placeholder.
                gpuInfo.LoadPercentage = 0.0; // TODO: Implement proper GPU load if possible via perf data
            }

            return gpuInfo;
        }
    }
}