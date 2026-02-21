using System;

namespace HardWareMonitor.Models
{
    public class GpuInfo
    {
        public string Name { get; set; }
        public long VideoMemory { get; set; }
        public double LoadPercentage { get; set; }

        public GpuInfo() { }
    }
}