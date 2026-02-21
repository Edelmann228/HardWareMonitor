using System;
using System.Collections.Generic;

namespace HardWareMonitor.Models
{
    public class MemoryInfo
    {
        public long TotalMemoryBytes { get; set; }
        public long AvailableMemoryBytes { get; set; }
        public double UsagePercentage { get; set; }
        public List<MemoryModule> Modules { get; set; }

        public MemoryInfo()
        {
            Modules = new List<MemoryModule>();
        }
    }

    public class MemoryModule
    {
        public long Capacity { get; set; }
        public string Manufacturer { get; set; }
        public int Speed { get; set; }
    }
}