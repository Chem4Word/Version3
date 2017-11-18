using System;
using System.Globalization;
using System.Management;

namespace Chem4Word.Telemetry
{
    public class WmiHelper
    {
        public WmiHelper()
        {
            //
        }

        private string _cpuName;

        public string CpuName
        {
            get
            {
                if (_cpuName == null)
                {
                    GatherCpuData();
                }
                return _cpuName;
            }
        }

        private string _cpuSpeed;

        public string CpuSpeed
        {
            get
            {
                if (_cpuSpeed == null)
                {
                    GatherCpuData();
                }
                return _cpuSpeed;
            }
        }

        private string _logicalProcessors;

        public string LogicalProcessors
        {
            get
            {
                if (_logicalProcessors == null)
                {
                    GatherCpuData();
                }
                return _logicalProcessors;
            }
        }

        private string _physicalMemory;

        public string PhysicalMemory
        {
            get
            {
                if (_physicalMemory == null)
                {
                    GatherMemoryData();
                }
                return _physicalMemory;
            }
        }

        private void GatherCpuData()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Name,NumberOfLogicalProcessors,CurrentClockSpeed FROM Win32_Processor");
            ManagementObjectCollection objCol = searcher.Get();

            foreach (var o in objCol)
            {
                var mgtObject = (ManagementObject)o;
                try
                {
                    string temp = mgtObject["Name"].ToString();
                    temp = temp.Replace("\t", " ").Replace("  ", " ");
                    _cpuName = temp;
                }
                catch
                {
                    _cpuName = "?";
                }

                try
                {
                    _logicalProcessors = mgtObject["NumberOfLogicalProcessors"].ToString();
                }
                catch
                {
                    _logicalProcessors = "?";
                }

                try
                {
                    double speed = double.Parse(mgtObject["CurrentClockSpeed"].ToString()) / 1024;
                    _cpuSpeed = speed.ToString("#,##0.00", CultureInfo.InvariantCulture) + "GHz";
                }
                catch
                {
                    _cpuSpeed = "?";
                }
            }
        }

        private void GatherMemoryData()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory");
            ManagementObjectCollection objCol = searcher.Get();

            try
            {
                UInt64 capacity = 0;
                foreach (var o in objCol)
                {
                    var mgtObject = (ManagementObject)o;
                    capacity += (UInt64)mgtObject["Capacity"];
                }
                _physicalMemory = (capacity / (1024 * 1024 * 1024)).ToString("#,##0") + "GB";
            }
            catch
            {
                _physicalMemory = "?";
            }
        }
    }
}