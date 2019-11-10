// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Management;

namespace Chem4Word.Telemetry
{
    public class WmiHelper
    {
        public WmiHelper()
        {
            GetWin32ProcessorData();
            GetWin32PhysicalMemeoryData();
            GetWin32OperatingSystemData();
        }

        private string _cpuName;

        public string CpuName
        {
            get
            {
                if (_cpuName == null)
                {
                    try
                    {
                        GetWin32ProcessorData();
                    }
                    catch (Exception)
                    {
                        //
                    }
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
                    try
                    {
                        GetWin32ProcessorData();
                    }
                    catch (Exception)
                    {
                        //
                    }
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
                    try
                    {
                        GetWin32ProcessorData();
                    }
                    catch (Exception)
                    {
                        //
                    }
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
                    try
                    {
                        GetWin32PhysicalMemeoryData();
                    }
                    catch (Exception)
                    {
                        //
                    }
                }

                return _physicalMemory;
            }
        }

        private string _lastBootUpTime;

        public string LastLastBootUpTime
        {
            get
            {
                if (_lastBootUpTime == null)
                {
                    try
                    {
                        GetWin32OperatingSystemData();
                    }
                    catch (Exception)
                    {
                        //
                    }
                }

                return _lastBootUpTime;
            }
        }

        private void GetWin32ProcessorData()
        {
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("SELECT Name,NumberOfLogicalProcessors,CurrentClockSpeed FROM Win32_Processor");
            ManagementObjectCollection objCol = searcher.Get();

            foreach (var o in objCol)
            {
                var mgtObject = (ManagementObject)o;
                try
                {
                    string temp = mgtObject["Name"].ToString();
                    // Replace tab with space
                    temp = temp.Replace("\t", " ");
                    // Replace upto 15 double spaces with single space
                    int i = 0;
                    while (temp.IndexOf("  ") != -1)
                    {
                        temp = temp.Replace("  ", " ");
                        i++;
                        if (i > 15)
                        {
                            break;
                        }
                    }

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

        private void GetWin32OperatingSystemData()
        {
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("SELECT LastBootUpTime FROM Win32_OperatingSystem");
            ManagementObjectCollection objCol = searcher.Get();

            try
            {
                foreach (var o in objCol)
                {
                    var mgtObject = (ManagementObject)o;
                    DateTime lastBootUp = ManagementDateTimeConverter.ToDateTime(mgtObject["LastBootUpTime"].ToString());
                    _lastBootUpTime = lastBootUp.ToUniversalTime().ToString("dd-MMM-yyyy HH:mm:ss UTC", CultureInfo.InvariantCulture);
                    break;
                }
            }
            catch
            {
                _lastBootUpTime = "?";
            }
        }

        private void GetWin32PhysicalMemeoryData()
        {
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory");
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