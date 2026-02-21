using System;

namespace HardWareMonitor.Models
{
    public class CpuInfo
    {
        public string Name { get; set; }
        public int CoreCount { get; set; }
        public int ThreadCount { get; set; }
        public double BaseFrequency { get; set; }
        public double LoadPercentage { get; set; }
        public string Manufacturer { get; set; }
        public string Architecture { get; set; }

        public CpuInfo() { }
    }
}