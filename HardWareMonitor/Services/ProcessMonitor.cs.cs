using System;
using System.Diagnostics;
using System.Collections.Generic;
using HardWareMonitor.Models;

namespace HardWareMonitor.Services
{
    public class ProcessMonitor
    {
        public List<ProcessInfo> GetProcesses()
        {
            List<ProcessInfo> processes = new List<ProcessInfo>();
            Process[] processArray = Process.GetProcesses();
            foreach (Process p in processArray)
            {
                ProcessInfo info = new ProcessInfo();
                info.Id = p.Id;
                info.Name = p.ProcessName;
                try
                {
                    info.WorkingSet = p.WorkingSet64;
                }
                catch
                {
                    info.WorkingSet = 0;
                }
                processes.Add(info);
            }
            return processes;
        }
    }
}