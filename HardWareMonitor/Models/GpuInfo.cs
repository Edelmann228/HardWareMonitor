using System;

namespace HardWareMonitor.Models
{
    public class GpuInfo
    {
        public string Name { get; set; }
        public long VideoMemory { get; set; }
        public double LoadPercentage { get; set; } // Note: WMI may not directly provide GPU load; this assumes a method to get it

        public GpuInfo() { }
    }
}