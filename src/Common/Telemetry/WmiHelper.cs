using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Chem4Word.Telemetry
{
    public class WmiHelper
    {
        public WmiHelper()
        {
            
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
                var mgtObject = (ManagementObject) o;
                _cpuName = mgtObject["Name"].ToString();
                _logicalProcessors = mgtObject["NumberOfLogicalProcessors"].ToString();
                double speed = double.Parse(mgtObject["CurrentClockSpeed"].ToString()) / 1024;
                _cpuSpeed = speed.ToString("#,##0.00", CultureInfo.InvariantCulture) + "GHz";
            }
        }

        private void GatherMemoryData()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory");
            ManagementObjectCollection objCol = searcher.Get();

            UInt64 capacity = 0;
            foreach (var o in objCol)
            {
                var mgtObject = (ManagementObject) o;
                capacity += (UInt64)mgtObject["Capacity"];
            }

            _physicalMemory = (capacity / (1024 * 1024 * 1024)).ToString("#,##0") + "GB";
        }
    }
}
