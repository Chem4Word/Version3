// ---------------------------------------------------------------------------
//  Copyright (c) 2020, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using Chem4Word.Core.Helpers;

namespace Chem4Word.Telemetry
{
    public class WmiHelper
    {
        private const string QueryProcessor = "SELECT Name,NumberOfLogicalProcessors,CurrentClockSpeed FROM Win32_Processor";
        private const string QueryOperatingSystem = "SELECT ProductType,LastBootUpTime FROM Win32_OperatingSystem";
        private const string QueryPhysicalMemory = "SELECT Capacity FROM Win32_PhysicalMemory";
        private const string QueryAntiVirusProduct = "SELECT DisplayName,ProductState FROM AntiVirusProduct";

        private const string Workstation = "Workstation";
        private const string DomainController = "Domain Controller";
        private const string Server = "Server";

        private const string Unknown = "Unknown";

        public WmiHelper()
        {
            GetWin32ProcessorData();
            GetWin32PhysicalMemoryData();
            GetWin32OperatingSystemData();
            GetAntiVirusStatus();
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
                        GetWin32PhysicalMemoryData();
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

        private string _productType;

        public string ProductType
        {
            get
            {
                if (_productType == null)
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

                return _productType;
            }
        }

        private string _antiVirusStatus;

        public string AntiVirusStatus
        {
            get
            {
                if (_antiVirusStatus == null)
                {
                    GetAntiVirusStatus();
                }

                return _antiVirusStatus;
            }
        }

        private void GetWin32ProcessorData()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryProcessor);
            ManagementObjectCollection objCol = searcher.Get();

            foreach (var o in objCol)
            {
                var mgtObject = (ManagementObject)o;
                try
                {
                    string temp = mgtObject["Name"].ToString();
                    // Replace tab with space
                    temp = temp.Replace("\t", " ");
                    // Replace up to 15 double spaces with single space
                    int i = 0;
                    while (temp.IndexOf("  ", StringComparison.InvariantCulture) != -1)
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
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryOperatingSystem);
            ManagementObjectCollection objCol = searcher.Get();

            try
            {
                foreach (var o in objCol)
                {
                    var mgtObject = (ManagementObject)o;
                    DateTime lastBootUp = ManagementDateTimeConverter.ToDateTime(mgtObject["LastBootUpTime"].ToString());
                    _lastBootUpTime = SafeDate.ToLongDate(lastBootUp.ToUniversalTime()) + " UTC";

                    var productType = int.Parse(mgtObject["ProductType"].ToString());
                    switch (productType)
                    {
                        case 1:
                            _productType = Workstation;
                            break;

                        case 2:
                            _productType = DomainController;
                            break;

                        case 3:
                            _productType = Server;
                            break;

                        default:
                            _productType = Unknown + $" [{productType}]";
                            break;
                    }
                }
            }
            catch
            {
                _lastBootUpTime = "?";
            }
        }

        private void GetWin32PhysicalMemoryData()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QueryPhysicalMemory);
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

        private void GetAntiVirusStatus()
        {
            // This is a combination of information from the following sources

            // http://neophob.com/2010/03/wmi-query-windows-securitycenter2/
            // https://mspscripts.com/get-installed-antivirus-information-2/
            // https://gallery.technet.microsoft.com/scriptcenter/Get-the-status-of-4b748f25
            // https://stackoverflow.com/questions/4700897/wmi-security-center-productstate-clarification/4711211
            // https://blogs.msdn.microsoft.com/alejacma/2008/05/12/how-to-get-antivirus-information-with-wmi-vbscript/#comment-442

            // Only works if not a server
            if (!string.IsNullOrEmpty(ProductType) && ProductType.Equals(Workstation))
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\SecurityCenter2", QueryAntiVirusProduct);
                ManagementObjectCollection objCol = searcher.Get();

                try
                {
                    List<string> products = new List<string>();
                    foreach (var o in objCol)
                    {
                        var mgtObject = (ManagementObject)o;
                        var product = mgtObject["DisplayName"].ToString();

                        var status = int.Parse(mgtObject["ProductState"].ToString());

                        var hex = Hex(status);
                        var bin = Binary(status);
                        var reversed = Reverse(bin);

                        // https://blogs.msdn.microsoft.com/alejacma/2008/05/12/how-to-get-antivirus-information-with-wmi-vbscript/#comment-442
                        // 19th bit = Not so sure but, AV is turned on (I wouldn't be sure it's enabled)
                        // 13th bit = On Access Scanning (Memory Resident Scanning) is on, this tells you that the product is scanning every file that you open as opposed to just scanning at regular intervals.
                        //  5th Bit = If this is true (==1) the virus scanner is out of date

                        bool enabled = GetBit(reversed, 18);
                        bool scanning = GetBit(reversed, 12);
                        bool outdated = GetBit(reversed, 4);

                        products.Add($"{product} Status: {status} [0x{hex}] --> Enabled: {enabled} Scanning: {scanning} Outdated: {outdated}");
                    }

                    // Return distinct list of products and states
                    _antiVirusStatus = string.Join(";", products.Distinct());
                }
                catch (Exception exception)
                {
                    _antiVirusStatus = $"{exception.Message}";
                }
            }
        }

        private string Hex(int value)
        {
            try
            {
                return Convert.ToString(value, 16).PadLeft(6, '0');
            }
            catch
            {
                return string.Empty;
            }
        }

        private string Binary(int value)
        {
            try
            {
                return Convert.ToString(value, 2).PadLeft(24, '0');
            }
            catch
            {
                return string.Empty;
            }
        }

        private string Reverse(string value)
        {
            try
            {
                return new string(value.Reverse().ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }

        private bool GetBit(string value, int index)
        {
            try
            {
                return value.Substring(index, 1).Equals("1");
            }
            catch
            {
                return false;
            }
        }
    }
}