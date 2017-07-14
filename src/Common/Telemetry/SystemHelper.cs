using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Chem4Word.Telemetry
{
    public class SystemHelper
    {
        private string CryptoRoot = @"SOFTWARE\Microsoft\Cryptography";

        public string MachineId { get; set; }

        public string SystemOs { get; set; }

        public string WordProduct { get; set; }

        public int WordVersion { get; set; }

        public string AddInVersion { get; set; }

        public string AddInLocation { get; set; }

        public string IpAddress { get; set; }

        public string IpObtainedFrom { get; set; }

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
                #region Get Office Product String

                string officeProductName = "";

                RegistryKey localMachine = Registry.LocalMachine;

                #region Get Guid

                string value1 = string.Empty;

                RegistryKey key1 = Registry.ClassesRoot.OpenSubKey("Word.Document");
                if (key1 != null)
                {
                    RegistryKey current = key1.OpenSubKey("CurVer");
                    if (current != null)
                    {
                        value1 = current.GetValue("").ToString();
                    }
                }
                //Debug.WriteLine(@"Word.Document\CurVer\(default) --> " + value1);

                string value2 = string.Empty;

                if (!string.IsNullOrEmpty(value1))
                {
                    RegistryKey key2 = Registry.ClassesRoot.OpenSubKey(value1);
                    if (key2 != null)
                    {
                        RegistryKey icon = key2.OpenSubKey("DefaultIcon");
                        if (icon != null)
                        {
                            value2 = icon.GetValue("").ToString();
                        }
                    }
                    //Debug.WriteLine(value1 + @"\DefaultIcon\(default) --> " + value2);
                }

                if (!string.IsNullOrEmpty(value2))
                {
                    int start = value2.IndexOf("{", StringComparison.Ordinal);
                    int end = value2.IndexOf("}", StringComparison.Ordinal);
                    if (end > start)
                    {
                        string officeGuid = value2.Substring(start, end - start + 1);
                        Debug.WriteLine("Office Guid: " + officeGuid);

                        officeProductName = DecodeOfficeGuid(officeGuid);
                    }
                }

                #endregion Get Guid

                #endregion Get Office Product String

                #region Get Word Version

                string wordVersionNumber = "";
                string name = @"Software\Microsoft\Windows\CurrentVersion\App Paths\winword.exe";

                // 1. looks inside CURRENT_USER:
                RegistryKey mainKey = Registry.CurrentUser;
                mainKey = mainKey.OpenSubKey(name, false);

                if (mainKey == null)
                {
                    //2. looks inside LOCAL_MACHINE:
                    mainKey = Registry.LocalMachine;
                    mainKey = mainKey.OpenSubKey(name, false);
                }

                if (mainKey != null)
                {
                    string path = mainKey.GetValue(string.Empty).ToString();
                    try
                    {
                        path = path.Replace("\"", "");
                        FileVersionInfo fi = FileVersionInfo.GetVersionInfo(path);
                        wordVersionNumber = fi.FileVersion;

                        // Handle product not found in uninstall section
                        if (string.IsNullOrEmpty(officeProductName))
                        {
                            officeProductName = fi.ProductName;
                        }
                        // Get a bit more information about this version
                        if (officeProductName.Contains("-0000000FF1CE}"))
                        {
                            officeProductName += Environment.NewLine + fi.ProductName;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                    // Generate single number from major of word's version number
                    WordVersion = GetOfficeVersionNumber(wordVersionNumber);

                    string sp = GetOfficeServicePack(wordVersionNumber);
                    if (!string.IsNullOrEmpty(sp))
                    {
                        officeProductName = officeProductName + sp;
                    }

                    WordProduct = (officeProductName + " [" + wordVersionNumber + "]");
                }
                else
                {
                    WordProduct = "Microsoft Word not found !";
                }

                #endregion Get Word Version
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
        }

        private string OsBits
        {
            get
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)") != null ? "64bit" : "32bit";
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
            DateTime started = DateTime.Now;

            // http://www.ipv6proxy.net/ --> "Your IP address : 2600:3c00::f03c:91ff:fe93:dcd4"

            try
            {
                string url1 = "http://www.chem4word.co.uk/files/client-ip.php"; // IPv4 & IPv6
                string url2 = "http://chem4word.azurewebsites.net/client-ip.php"; // IPv4 only

                // if (even) {url1} else {url2}
                string url = _retryCount % 2 == 0 ? url1 : url2;

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
                                        IpObtainedFrom = $"IpAddress got from {url} on attempt {_retryCount + 1}";
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
                                        IpObtainedFrom = $"IpAddress got from {url} on attempt {_retryCount + 1}";
                                    }
                                }

                                #endregion Detect IPv4
                            }

                            Debug.WriteLine(IpAddress);
                        }
                    }
                }

                TimeSpan ts = DateTime.Now - started;
                Debug.WriteLine("Obtaining External IP Address took " + ts.TotalMilliseconds.ToString("#,000.0" + "ms"));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                // Something went wrong
                IpAddress = "IpAddress 0.0.0.0 - " + ex.Message;

                TimeSpan ts = DateTime.Now - started;
                Debug.WriteLine("Obtaining External IP Address failed in " + ts.TotalMilliseconds.ToString("#,000.0" + "ms"));
            }

            if (string.IsNullOrEmpty(IpAddress) || IpAddress.Contains("0.0.0.0"))
            {
                if (_retryCount < 10)
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