// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.Helpers;
using Chem4Word.Shared;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Chem4Word.Telemetry
{
    public class SystemHelper
    {
        private static string CryptoRoot = @"SOFTWARE\Microsoft\Cryptography";
        private static string Click2RunConfiguration = @"SOFTWARE\Microsoft\Office\ClickToRun\Configuration";
        private string DotNetVersionKey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

        private static readonly string DetectionFile = $"{Constants.Chem4WordVersionFiles}/client-ip-date.php";
        private static readonly string[] Domains = { "https://www.chem4word.co.uk", "https://chem4word.azurewebsites.net", "http://www.chem4word.com" };

        public string MachineId { get; set; }

        public int ProcessId { get; set; }

        public string SystemOs { get; set; }

        public string WordProduct { get; set; }

        public string Click2RunProductIds { get; set; }

        public int WordVersion { get; set; }

        public string AddInVersion { get; set; }

        public string AssemblyVersionNumber { get; set; }

        public string AddInLocation { get; set; }

        public string IpAddress { get; set; }

        public string IpObtainedFrom { get; set; }

        public string DotNetVersion { get; set; }

        public string Screens { get; set; }

        public string GitStatus { get; set; }

        public long UtcOffset { get; set; }
        public DateTime SystemUtcDateTime { get; set; }
        public string ServerDateHeader { get; set; }
        public string ServerUtcDateRaw { get; set; }
        public DateTime ServerUtcDateTime { get; set; }
        public string BrowserVersion { get; set; }

        private static int _retryCount;

        public static string GetMachineId()
        {
            string result = "";
            try
            {
                // Need special routine here as MachineGuid does not exist in the wow6432 path
                result = RegistryWOW6432.GetRegKey64(RegHive.HKEY_LOCAL_MACHINE, CryptoRoot, "MachineGuid");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                result = "Exception " + ex.Message;
            }

            return result;
        }

        public SystemHelper()
        {
            WordVersion = -1;

            #region Get Machine Guid

            MachineId = GetMachineId();

            ProcessId = Process.GetCurrentProcess().Id;

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

                Click2RunProductIds = GetClick2RunProductIds();

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
            AssemblyVersionNumber = productVersion.ToString();

            AddInVersion = "Chem4Word V" + productVersion;

            #endregion Get Product Version and Location using reflection

            #region Get IpAddress

            ParameterizedThreadStart pts = GetExternalIpAddress;
            Thread t = new Thread(pts);
            t.Start(null);

            #endregion Get IpAddress

            GetDotNetVersionFromRegistry();

            try
            {
                BrowserVersion = new WebBrowser().Version.ToString();
            }
            catch
            {
                BrowserVersion = "?";
            }

            GetScreens();

#if DEBUG
            GetGitStatus();
#endif
        }

        private string GetClick2RunProductIds()
        {
            string result = "";
            try
            {
                // Need special routine here as MachineGuid does not exist in the wow6432 path
                result = RegistryWOW6432.GetRegKey64(RegHive.HKEY_LOCAL_MACHINE, Click2RunConfiguration, "ProductReleaseIds");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                result = "Exception " + ex.Message;
            }

            return result;
        }

        private void GetGitStatus()
        {
            var result = new List<string>();
            result.Add("Git Branch");
            // git rev-parse --abbrev-ref HEAD == Current Branch
            result.AddRange(RunCommand("git.exe", "rev-parse --abbrev-ref HEAD", AddInLocation));

            // git status --porcelain == Get List of changed files
            var changedFiles = RunCommand("git.exe", "status --porcelain", AddInLocation);
            if (changedFiles.Any())
            {
                result.Add("Changed Files");
                result.AddRange(changedFiles);
            }
            GitStatus = string.Join(Environment.NewLine, result.ToArray());
        }

        private List<string> RunCommand(string exeName, string args, string folder)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(exeName);

            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = folder;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.Arguments = args;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            var results = new List<string>();
            while (!process.StandardOutput.EndOfStream)
            {
                results.Add(process.StandardOutput.ReadLine());
            }

            return results;
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

                    // .Net 4.7.2
                    if (releaseKey >= 461814)
                    {
                        DotNetVersion = $".NET 4.7.2 [{releaseKey}]";
                        return;
                    }
                    if (releaseKey >= 461808)
                    {
                        DotNetVersion = $".NET 4.7.2 (W10 1803) [{releaseKey}]";
                        return;
                    }

                    // .Net 4.7.1
                    if (releaseKey >= 461310)
                    {
                        DotNetVersion = $".NET 4.7.1 [{releaseKey}]";
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

                            try
                            {
                                // Get Server Date header i.e. "Tue, 01 Jan 2019 19:52:46 GMT"
                                ServerDateHeader = response.Headers["date"];
                            }
                            catch
                            {
                                // Do Nothing
                            }

                            if (HttpStatusCode.OK.Equals(response.StatusCode))
                            {
                                using (var reader = new StreamReader(response.GetResponseStream()))
                                {
                                    string webPage = reader.ReadToEnd();

                                    if (webPage.StartsWith("Your IP address : "))
                                    {
                                        // Tidy Up the data
                                        webPage = webPage.Replace("Your IP address : ", "");
                                        webPage = webPage.Replace("UTC Date : ", "");
                                        webPage = webPage.Replace("<br/>", "|");
                                        webPage = webPage.Replace("<br />", "|");

                                        string[] lines = webPage.Split('|');

                                        #region Detect IPv6

                                        if (lines[0].Contains(":"))
                                        {
                                            string[] ipV6Parts = lines[0].Split(':');
                                            // Must have between 4 and 8 parts
                                            if (ipV6Parts.Length >= 4 && ipV6Parts.Length <= 8)
                                            {
                                                IpAddress = "IpAddress " + lines[0];
                                                IpObtainedFrom = $"IpAddress V6 obtained from {url} on attempt {_retryCount + 1}";
                                            }
                                        }

                                        #endregion Detect IPv6

                                        #region Detect IPv4

                                        if (lines[0].Contains("."))
                                        {
                                            // Must have 4 parts
                                            string[] ipV4Parts = lines[0].Split('.');
                                            if (ipV4Parts.Length == 4)
                                            {
                                                IpAddress = "IpAddress " + lines[0];
                                                IpObtainedFrom = $"IpAddress V4 obtained from {url} on attempt {_retryCount + 1}";
                                            }
                                        }

                                        #endregion Detect IPv4

                                        #region Detect Php UTC Date

                                        if (lines.Length > 1)
                                        {
                                            ServerUtcDateRaw = lines[1];
                                            ServerUtcDateTime = FromPhpDate(lines[1]);
                                            SystemUtcDateTime = DateTime.UtcNow;

                                            UtcOffset = SystemUtcDateTime.Ticks - ServerUtcDateTime.Ticks;
                                        }

                                        #endregion

                                        if (!IpAddress.Contains("0.0.0.0"))
                                        {
                                            break;
                                        }
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

        private DateTime FromPhpDate(string line)
        {
            string[] p = line.Split(',');
            var serverUtc = new DateTime(int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2]), int.Parse(p[3]), int.Parse(p[4]), int.Parse(p[5]));
            return DateTime.SpecifyKind(serverUtc, DateTimeKind.Utc);
        }
    }
}