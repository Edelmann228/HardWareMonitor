using System;

namespace HardWareMonitor.Models
{
    public class NetworkAdapterInfo
    {
        public string Name { get; set; }
        public string MacAddress { get; set; }
        public long Speed { get; set; }

        public NetworkAdapterInfo() { }
    }
}