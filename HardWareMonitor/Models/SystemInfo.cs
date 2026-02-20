using System;
using System.Collections.Generic;

namespace HardWareMonitor.Models
{
    public class SystemInfo
    {
        public string OSName { get; set; }
        public string OSVersion { get; set; }
        public string MachineName { get; set; }
        public List<ProcessInfo> Processes { get; set; }

        public SystemInfo()
        {
            Processes = new List<ProcessInfo>();
        }
    }

    public class ProcessInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long WorkingSet { get; set; }
    }
}