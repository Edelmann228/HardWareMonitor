using System;
using System.Management;
using HardWareMonitor.Models;

namespace HardWareMonitor.Services
{
    public class DiskMonitor
    {
        public DiskInfo GetDiskInfo()
        {
            DiskInfo diskInfo = new DiskInfo();

            // Physical disks
            ManagementObjectSearcher physicalSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (ManagementObject obj in physicalSearcher.Get())
            {
                PhysicalDisk disk = new PhysicalDisk();
                disk.Model = obj["Model"] as string ?? "Unknown";
                disk.Size = Convert.ToInt64(obj["Size"] ?? 0);
                disk.MediaType = obj["MediaType"] as string ?? "Unknown";
                diskInfo.PhysicalDisks.Add(disk);
            }

            // Logical disks
            ManagementObjectSearcher logicalSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk WHERE DriveType=3"); // Fixed disks
            foreach (ManagementObject obj in logicalSearcher.Get())
            {
                LogicalDisk disk = new LogicalDisk();
                disk.DriveLetter = obj["DeviceID"] as string ?? "Unknown";
                disk.TotalSize = Convert.ToInt64(obj["Size"] ?? 0);
                disk.FreeSpace = Convert.ToInt64(obj["FreeSpace"] ?? 0);
                disk.FileSystem = obj["FileSystem"] as string ?? "Unknown";
                disk.UsagePercentage = disk.TotalSize > 0 ? ((double)(disk.TotalSize - disk.FreeSpace) / disk.TotalSize) * 100 : 0;
                diskInfo.LogicalDisks.Add(disk);
            }

            return diskInfo;
        }
    }
}