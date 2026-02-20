using System;
using System.Management;
using HardWareMonitor.Models;

namespace HardWareMonitor.Services
{
    public class CpuMonitor
    {
        public CpuInfo GetCpuInfo()
        {
            CpuInfo cpuInfo = new CpuInfo();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                cpuInfo.Name = obj["Name"] as string ?? "Unknown";
                cpuInfo.CoreCount = Convert.ToInt32(obj["NumberOfCores"] ?? 0);
                cpuInfo.ThreadCount = Convert.ToInt32(obj["NumberOfLogicalProcessors"] ?? 0);
                cpuInfo.BaseFrequency = Convert.ToDouble(obj["MaxClockSpeed"] ?? 0.0);
                cpuInfo.Manufacturer = obj["Manufacturer"] as string ?? "Unknown";
                cpuInfo.Architecture = obj["Architecture"] as string ?? "Unknown";
            }

            // Get load
            ManagementObjectSearcher loadSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name='_Total'");
            foreach (ManagementObject obj in loadSearcher.Get())
            {
                cpuInfo.LoadPercentage = Convert.ToDouble(obj["PercentProcessorTime"] ?? 0.0);
            }

            return cpuInfo;
        }
    }
}