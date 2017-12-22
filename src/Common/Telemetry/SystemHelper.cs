// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Shared;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Chem4Word.Telemetry
{
    public class SystemHelper
    {
        private string CryptoRoot = @"SOFTWARE\Microsoft\Cryptography";
        private string DotNetVersionKey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

        private const string DetectionFile = "files3/client-ip.php";
        private static readonly string[] Domains = { "https://www.chem4word.co.uk", "https://chem4word.azurewebsites.net", "http://www.chem4word.com" };

        public string MachineId { get; set; }

        public string SystemOs { get; set; }

        public string WordProduct { get; set; }

        public int WordVersion { get; set; }

        public string AddInVersion { get; set; }

        public string AddInLocation { get; set; }

        public string IpAddress { get; set; }

        public string IpObtainedFrom { get; set; }

        public string DotNetVersion { get; set; }

        public string Screens { get; set; }

        private static int _retryCount;

        public SystemHelper()
        {
            WordVersion = -1;

            #region Get Machine Guid

            try
            {
                // Need special routine here as MachineGuid does not exist in the wow6432 path
                MachineId = RegistryWOW6432.GetRegKey64(RegHive.HKEY_LOCAL_MACHINE, CryptoRoot, "MachineGuid");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MachineId = "Exception " + ex.Message;
            }

            #endregion Get Machine Guid

            #region Get OS Version

            // The current code returns 6.2.* for Windows 8.1 and Windows 10 on some systems
            // https://msdn.microsoft.com/en-gb/library/windows/desktop/ms724832(v=vs.85).aspx
            // https://msdn.microsoft.com/en-gb/library/windows/desktop/dn481241(v=vs.85).aspx
            // However as we do not NEED the exact version number,
            //  I am not going to implement the above as they are too risky

            try
            {
                OperatingSystem operatingSystem = Environment.OSVersion;

                string ProductName = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
                string CsdVersion = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CSDVersion");

                if (!string.IsNullOrEmpty(ProductName))
                {
                    StringBuilder sb = new StringBuilder();
                    if (!ProductName.StartsWith("Microsoft"))
                    {
                        sb.Append("Microsoft ");
                    }
                    sb.Append(ProductName);
                    if (!string.IsNullOrEmpty(CsdVersion))
                    {
                        sb.AppendLine($" {CsdVersion}");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(operatingSystem.ServicePack))
                        {
                            sb.Append($" {operatingSystem.ServicePack}");
                        }
                    }

                    sb.Append($" {OsBits}");
                    sb.Append($" [{operatingSystem.Version}]");
                    sb.Append($" {CultureInfo.CurrentCulture.Name}");

                    SystemOs = sb.ToString().Replace(Environment.NewLine, "").Replace("Service Pack ", "SP");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                SystemOs = "Exception " + ex.Message;
            }

            #endregion Get OS Version

            #region Get Office/Word Version

            try
            {
                WordProduct = OfficeHelper.GetWordProduct();
                WordVersion = OfficeHelper.GetWinWordVersion();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                WordProduct = "Exception " + ex.Message;
            }

            #endregion Get Office/Word Version

            #region Get Product Version and Location using reflection

            Assembly assembly = Assembly.GetExecutingAssembly();
            // CodeBase is the location of the installed files
            Uri uriCodeBase = new Uri(assembly.CodeBase);
            AddInLocation = Path.GetDirectoryName(uriCodeBase.LocalPath);

            Version productVersion = assembly.GetName().Version;
            AddInVersion = "Chem4Word V" + productVersion;

            #endregion Get Product Version and Location using reflection

            #region Get IpAddress

            ParameterizedThreadStart pts = GetExternalIpAddress;
            Thread t = new Thread(pts);
            t.Start(null);

            #endregion Get IpAddress

            GetDotNetVersionFromRegistry();

            GetScreens();
        }

        private void GetScreens()
        {
            List<string> screens = new List<string>();

            foreach (var screen in Screen.AllScreens)
            {
                screens.Add($"{screen.Bounds.Width} x {screen.Bounds.Height}");
            }

            Screens = string.Join("; ", screens);
        }

        private string OsBits
        {
            get
            {
                return Environment.Is64BitOperatingSystem ? "64bit" : "32bit";
            }
        }

        private void GetDotNetVersionFromRegistry()
        {
            // https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed
            // https://en.wikipedia.org/wiki/Windows_10_version_history

            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(DotNetVersionKey))
            {
                if (ndpKey != null)
                {
                    int releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));

                    // .Net 4.7.1
                    if (releaseKey >= 461310)
                    {
                        DotNetVersion = $".NET 4.7 [{releaseKey}]";
                        return;
                    }
                    if (releaseKey >= 461308)
                    {
                        DotNetVersion = $".NET 4.7.1 (W10 1710) [{releaseKey}]";
                        return;
                    }

                    // .Net 4.7
                    if (releaseKey >= 460805)
                    {
                        DotNetVersion = $".NET 4.7 [{releaseKey}]";
                        return;
                    }
                    if (releaseKey >= 460798)
                    {
                        DotNetVersion = $".NET 4.7 (W10 1703) [{releaseKey}]";
                        return;
                    }

                    // .Net 4.6.2
                    if (releaseKey >= 394806)
                    {
                        DotNetVersion = $".NET 4.6.2 [{releaseKey}]";
                        return;
                    }
                    if (releaseKey >= 394802)
                    {
                        DotNetVersion = $".NET 4.6.2 (W10 1607) [{releaseKey}]";
                        return;
                    }

                    // .Net 4.6.1
                    if (releaseKey >= 394271)
                    {
                        DotNetVersion = $".NET 4.6.1 [{releaseKey}]";
                        return;
                    }
                    if (releaseKey >= 394254)
                    {
                        DotNetVersion = $".NET 4.6.1 (W10 1511) [{releaseKey}]";
                        return;
                    }

                    // .Net 4.6
                    if (releaseKey >= 393297)
                    {
                        DotNetVersion = $".NET 4.6 [{releaseKey}]";
                        return;
                    }
                    if (releaseKey >= 393295)
                    {
                        DotNetVersion = $".NET 4.6 (W10 1507) [{releaseKey}]";
                        return;
                    }

                    // .Net 4.5.2
                    if (releaseKey >= 379893)
                    {
                        DotNetVersion = $".NET 4.5.2 [{releaseKey}]";
                        return;
                    }

                    // .Net 4.5.1
                    if (releaseKey >= 378758)
                    {
                        DotNetVersion = $".NET 4.5.1 [{releaseKey}]";
                        return;
                    }
                    if (releaseKey >= 378675)
                    {
                        DotNetVersion = $".NET 4.5.1 [{releaseKey}]";
                        return;
                    }

                    // .Net 4.5
                    if (releaseKey >= 378389)
                    {
                        DotNetVersion = $".NET 4.5 [{releaseKey}]";
                        return;
                    }

                    if (releaseKey < 378389)
                    {
                        DotNetVersion = ".Net Version Unknown [{releaseKey}]";
                    }
                }
            }
        }

        private string HKLM_GetString(string path, string key)
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(path, false);
                if (rk == null)
                {
                    return "";
                }
                return (string)rk.GetValue(key);
            }
            catch
            {
                return "";
            }
        }

        private void GetExternalIpAddress(object o)
        {
            // http://www.ipv6proxy.net/ --> "Your IP address : 2600:3c00::f03c:91ff:fe93:dcd4"

            try
            {
                foreach (var domain in Domains)
                {
                    try
                    {
                        string url = $"{domain}/{DetectionFile}";

                        Debug.WriteLine("Fetching external IpAddress from " + url + " attempt " + _retryCount);
                        IpAddress = "IpAddress 0.0.0.0";

                        HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                        if (request != null)
                        {
                            request.UserAgent = "Chem4Word Add-In";
                            request.Timeout = 2000; // 2 seconds
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            if (HttpStatusCode.OK.Equals(response.StatusCode))
                            {
                                using (var reader = new StreamReader(response.GetResponseStream()))
                                {
                                    string webPage = reader.ReadToEnd();

                                    if (webPage.StartsWith("Your IP address : "))
                                    {
                                        // Tidy Up the data
                                        webPage = webPage.Replace("Your IP address : ", "");
                                        webPage = webPage.Replace("<br/>", "");
                                        webPage = webPage.Replace("<br />", "");

                                        #region Detect IPv6

                                        if (webPage.Contains(":"))
                                        {
                                            string[] ipV6Parts = webPage.Split(':');
                                            // Must have between 4 and 8 parts
                                            if (ipV6Parts.Length >= 4 && ipV6Parts.Length <= 8)
                                            {
                                                IpAddress = "IpAddress " + webPage;
                                                IpObtainedFrom = $"IpAddress V6 obtained from {url} on attempt {_retryCount + 1}";
                                                break;
                                            }
                                        }

                                        #endregion Detect IPv6

                                        #region Detect IPv4

                                        if (webPage.Contains("."))
                                        {
                                            // Must have 4 parts
                                            string[] ipV4Parts = webPage.Split('.');
                                            if (ipV4Parts.Length == 4)
                                            {
                                                IpAddress = "IpAddress " + webPage;
                                                IpObtainedFrom = $"IpAddress V4 obtained from {url} on attempt {_retryCount + 1}";
                                                break;
                                            }
                                        }

                                        #endregion Detect IPv4
                                    }

                                    Debug.WriteLine(IpAddress);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        // Do Nothing
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                // Something went wrong
                IpAddress = "IpAddress 0.0.0.0 - " + ex.Message;
            }

            if (string.IsNullOrEmpty(IpAddress) || IpAddress.Contains("0.0.0.0"))
            {
                if (_retryCount < 5)
                {
                    _retryCount++;
                    Thread.Sleep(500);
                    ParameterizedThreadStart pts = GetExternalIpAddress;
                    Thread t = new Thread(pts);
                    t.Start(null);
                }
            }
        }

        private int GetOfficeVersionNumber(string wordVersionString)
        {
            int version = -1;

            if (!string.IsNullOrEmpty(wordVersionString))
            {
                string[] parts = wordVersionString.Split('.');
                int major = int.Parse(parts[0]);
                switch (major)
                {
                    case 12:
                        version = 2007;
                        break;

                    case 14:
                        version = 2010;
                        break;

                    case 15:
                        version = 2013;
                        break;

                    case 16:
                        version = 2016;
                        break;
                }
            }
            return version;
        }

        private string GetOfficeServicePack(string wordVersionString)
        {
            // Source: https://buildnumbers.wordpress.com/office/
            // Plus correction from https://support.microsoft.com/en-us/kb/2121559
            string servicePack = "";
            if (WordVersion > 2000)
            {
                string[] parts = wordVersionString.Split('.');
                int build = int.Parse(parts[2]);
                switch (WordVersion)
                {
                    case 2007:
                        if (build >= 6213)
                        {
                            servicePack = "SP1";
                        }
                        if (build >= 6425)
                        {
                            servicePack = "SP2";
                        }
                        if (build >= 6607)
                        {
                            servicePack = "SP3";
                        }
                        break;

                    case 2010:
                        if (build >= 6029)
                        {
                            servicePack = "SP1";
                        }
                        if (build >= 7015)
                        {
                            servicePack = "SP2";
                        }
                        break;

                    case 2013:
                        if (build >= 4569)
                        {
                            servicePack = "SP1";
                        }
                        break;

                    case 2016:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(servicePack))
            {
                servicePack = " " + servicePack;
            }

            return servicePack;
        }

        private string DecodeOfficeGuid(string officeGuid)
        {
            // Office 2007 https://support.microsoft.com/en-us/kb/928516
            // Office 2010 https://support.microsoft.com/en-us/kb/2186281
            // Office 2013 https://support.microsoft.com/en-us/kb/2786054
            // Office 2016 https://support.microsoft.com/en-us/kb/3120274

            //           1         2         3
            // 01234567890123456789012345678901234567
            // {BRMMmmmm-PPPP-LLLL-p000-D000000FF1CE}

            // The following table describes the characters of the GUID.
            // B    Release version 0-9, A-F
            // R    Release type 0-9, A-F
            // MM   Major version 0-9
            // mmmm Minor version 0-9
            // PPPP Product ID 0-9, A-F
            // LLLL Language identifier 0-9, A-F
            // p    0 for x86, 1 for x64 0-1
            // 000  Reserved for future use, currently 0 0
            // D    1 for debug, 0 for ship 0-1
            // 000000FF1CE Office Family ID

            string result = "";

            string releaseVersion = officeGuid.Substring(1, 1);
            string releaseType = officeGuid.Substring(2, 1);
            string majorVersion = officeGuid.Substring(3, 2);
            string minorVersion = officeGuid.Substring(5, 4);
            string productId = officeGuid.Substring(10, 4);
            string language = officeGuid.Substring(15, 4);
            string bitFlag = officeGuid.Substring(20, 1);
            string debugFlag = officeGuid.Substring(25, 1);

            int major = int.Parse(majorVersion);

            switch (major)
            {
                case 12:

                    #region Office 2007

                    switch (productId)
                    {
                        case "0011":
                            result = "Microsoft Office Professional Plus 2007";
                            break;

                        case "0012":
                            result = "Microsoft Office Standard 2007";
                            break;

                        case "0013":
                            result = "Microsoft Office Basic 2007";
                            break;

                        case "0014":
                            result = "Microsoft Office Professional 2007";
                            break;

                        case "001B":
                            result = "Microsoft Office Word 2007";
                            break;

                        case "002E":
                            result = "Microsoft Office Ultimate 2007";
                            break;

                        case "002F":
                            result = "Microsoft Office Home and Student 2007";
                            break;

                        case "0030":
                            result = "Microsoft Office Enterprise 2007";
                            break;

                        case "0031":
                            result = "Microsoft Office Professional Hybrid 2007";
                            break;

                        case "0033":
                            result = "Microsoft Office Personal 2007";
                            break;

                        case "0035":
                            result = "Microsoft Office Professional Hybrid 2007";
                            break;

                        case "00BA":
                            result = "Microsoft Office Groove 2007";
                            break;

                        case "00CA":
                            result = "Microsoft Office Small Business 2007";
                            break;

                        default:
                            result = "Microsoft Office 2007 " + officeGuid;
                            break;
                    }
                    break;

                #endregion Office 2007

                case 14:

                    #region Office 2010

                    switch (productId)
                    {
                        case "0011":
                            result = "Microsoft Office Professional Plus 2010";
                            break;

                        case "0012":
                            result = "Microsoft Office Standard 2010";
                            break;

                        case "0013":
                            result = "Microsoft Office Home and Business 2010";
                            break;

                        case "0014":
                            result = "Microsoft Office Professional 2010";
                            break;

                        case "001B":
                            result = "Microsoft Word 2010";
                            break;

                        case "002F":
                            result = "Microsoft Office Home and Student 2010";
                            break;

                        case "003D":
                            result = "Microsoft Office Home and Student 2010";
                            break;

                        case "008B":
                            result = "Microsoft Office Small Business Basics 2010";
                            break;

                        case "011D":
                            result = "Microsoft Office Professional Plus Subscription 2010";
                            break;

                        default:
                            result = "Microsoft Office 2010 " + officeGuid;
                            break;
                    }
                    break;

                #endregion Office 2010

                case 15:

                    #region Office 2013

                    switch (productId)
                    {
                        case "000F":
                            result = "Microsoft Office 365 (2013) Pro Plus";
                            break;

                        case "0011":
                            result = "Microsoft Office Professional Plus 2013";
                            break;

                        case "0012":
                            result = "Microsoft Office Standard 2013";
                            break;

                        case "0013":
                            result = "Microsoft Office Home and Business 2013";
                            break;

                        case "0014":
                            result = "Microsoft Office Professional 2013";
                            break;

                        case "001B":
                            result = "Microsoft Word 2013";
                            break;

                        case "002F":
                            result = "Microsoft Office Home and Student 2013";
                            break;

                        default:
                            result = "Microsoft Office 2013 " + officeGuid;
                            break;
                    }
                    break;

                #endregion Office 2013

                case 16:

                    #region Office 2016

                    switch (productId)
                    {
                        case "000F":
                            result = "Microsoft Office 2016 Professional Plus";
                            break;

                        case "0011":
                            result = "Microsoft Office Professional Plus 2016";
                            break;

                        case "0012":
                            result = "Microsoft Office Standard 2016";
                            break;

                        case "001B":
                            result = "Microsoft Word 2016";
                            break;

                        default:
                            result = "Microsoft Office 2016 " + officeGuid;
                            break;
                    }
                    break;

                    #endregion Office 2016
            }

            #region 32 / 64 bit

            if (bitFlag.Equals("1"))
            {
                result += " 64bit";
            }
            else
            {
                result += " 32bit";
            }

            #endregion 32 / 64 bit

            return result;
        }
    }
}