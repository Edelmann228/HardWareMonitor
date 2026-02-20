using System;
using System.Management;
using HardWareMonitor.Models;

namespace HardWareMonitor.Services
{
    public class NetworkMonitor
    {
        public NetworkAdapterInfo GetNetworkInfo()
        {
            NetworkAdapterInfo adapterInfo = new NetworkAdapterInfo();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE PhysicalAdapter=True");
            foreach (ManagementObject obj in searcher.Get())
            {
                adapterInfo.Name = obj["Name"] as string ?? "Unknown";
                adapterInfo.MacAddress = obj["MACAddress"] as string ?? "Unknown";
                adapterInfo.Speed = Convert.ToInt64(obj["Speed"] ?? 0);
                break; // Take first physical adapter for simplicity
            }

            return adapterInfo;
        }
    }
}