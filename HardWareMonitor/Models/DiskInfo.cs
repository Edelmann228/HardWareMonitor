using System;
using System.Collections.Generic;

namespace HardWareMonitor.Models
{
    public class DiskInfo
    {
        public List<PhysicalDisk> PhysicalDisks { get; set; }
        public List<LogicalDisk> LogicalDisks { get; set; }

        public DiskInfo()
        {
            PhysicalDisks = new List<PhysicalDisk>();
            LogicalDisks = new List<LogicalDisk>();
        }
    }

    public class PhysicalDisk
    {
        public string Model { get; set; }
        public long Size { get; set; }
        public string MediaType { get; set; }
    }

    public class LogicalDisk
    {
        public string DriveLetter { get; set; }
        public long TotalSize { get; set; }
        public long FreeSpace { get; set; }
        public string FileSystem { get; set; }
        public double UsagePercentage { get; set; }
    }
}