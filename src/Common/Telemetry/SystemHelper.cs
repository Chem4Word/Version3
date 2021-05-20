// ---------------------------------------------------------------------------
//  Copyright (c) 2021, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Chem4Word.Core.Helpers;
using Chem4Word.Shared;
using Microsoft.Win32;

namespace Chem4Word.Telemetry
{
    public class SystemHelper
    {
        private static string CryptoRoot = @"SOFTWARE\Microsoft\Cryptography";
        private string DotNetVersionKey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

        public string MachineId { get; set; }

        public int ProcessId { get; set; }

        public string SystemOs { get; set; }

        public string WordProduct { get; set; }

        public string Click2RunProductIds { get; set; }

        public int WordVersion { get; set; }

        public string AddInVersion { get; set; }

        public List<AddInProperties> AllAddIns { get; set; }

        public string AssemblyVersionNumber { get; set; }

        public string AddInLocation { get; set; }

        public string IpAddress { get; set; }

        public string IpObtainedFrom { get; set; }

        public string LastBootUpTime { get; set; }

        public string LastLoginTime { get; set; }

        public string DotNetVersion { get; set; }

        public string Screens { get; set; }

        public string GitStatus { get; set; }

        public long UtcOffset { get; set; }
        public DateTime SystemUtcDateTime { get; set; }
        public string ServerDateHeader { get; set; }
        public string ServerUtcDateRaw { get; set; }
        public DateTime ServerUtcDateTime { get; set; }
        public string BrowserVersion { get; set; }
        public List<string> StartUpTimings { get; set; }

        private static Stopwatch _ipStopwatch;

        private readonly List<string> _placesToTry = new List<string>();
        private int _attempts;

        public SystemHelper(List<string> timings)
        {
            StartUpTimings = timings;

            StartUpTimings.AddRange(Initialise());
        }

        public SystemHelper()
        {
            if (StartUpTimings == null)
            {
                StartUpTimings = new List<string>();
            }

            StartUpTimings.AddRange(Initialise());
        }

        private List<string> Initialise()
        {
            try
            {
                List<string> timings = new List<string>();

                string message = $"SystemHelper.Initialise() started at {SafeDate.ToLongDate(DateTime.Now)}";
                timings.Add(message);
                Debug.WriteLine(message);

                Stopwatch sw = new Stopwatch();
                sw.Start();

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
                    Click2RunProductIds = OfficeHelper.GetClick2RunProductIds();

                    WordVersion = OfficeHelper.GetWinWordVersionNumber();

                    WordProduct = OfficeHelper.GetWordProduct(Click2RunProductIds);
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

                #region Get IpAddress on Thread

                // These can be tested via http://www.ipv6proxy.net/

                // Our locations
                _placesToTry.Add($"https://www.chem4word.co.uk/{Constants.Chem4WordVersionFiles}/client-ip-date.php");
                _placesToTry.Add($"http://www.chem4word.com/{Constants.Chem4WordVersionFiles}/client-ip-date.php");
                _placesToTry.Add($"https://chem4word.azurewebsites.net/{Constants.Chem4WordVersionFiles}/client-ip-date.php");

                // Other Locations
                _placesToTry.Add("https://api.my-ip.io/ip");
                _placesToTry.Add("https://ip.seeip.org");
                _placesToTry.Add("https://ipapi.co/ip");
                _placesToTry.Add("https://ident.me/");
                _placesToTry.Add("https://api6.ipify.org/");
                _placesToTry.Add("https://v4v6.ipv6-test.com/api/myip.php");

                message = $"GetIpAddress started at {SafeDate.ToLongDate(DateTime.Now)}";
                StartUpTimings.Add(message);
                Debug.WriteLine(message);

                _ipStopwatch = new Stopwatch();
                _ipStopwatch.Start();

                Thread thread1 = new Thread(GetExternalIpAddress);
                thread1.SetApartmentState(ApartmentState.STA);
                thread1.Start(null);

                #endregion Get IpAddress on Thread

                GetDotNetVersionFromRegistry();

                AllAddIns = InfoHelper.GetListOfAddIns();

                GatherBootUpTimeEtc();

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
                Thread thread2 = new Thread(GetGitStatus);
                thread2.SetApartmentState(ApartmentState.STA);
                thread2.Start(null);
#endif

                sw.Stop();

                message = $"SystemHelper.Initialise() took {sw.ElapsedMilliseconds.ToString("#,000", CultureInfo.InvariantCulture)}ms";
                timings.Add(message);
                Debug.WriteLine(message);

                return timings;
            }
            catch (ThreadAbortException threadAbortException)
            {
                // Do Nothing
                Debug.WriteLine(threadAbortException.Message);
            }

            return new List<string>();
        }

        private void GatherBootUpTimeEtc()
        {
            LastBootUpTime = "";
            LastLoginTime = "";

            try
            {
                var q1 = "*[System/Provider/@Name='Microsoft-Windows-Kernel-Boot' and System/EventID=27]";
                var d1 = LastEventDateTime(q1);
                LastBootUpTime = $"{SafeDate.ToLongDate(d1.ToUniversalTime())}";

                var q2 = "*[System/Provider/@Name='Microsoft-Windows-Winlogon' and System/EventID=7001]";
                var d2 = LastEventDateTime(q2);
                LastLoginTime = $"{SafeDate.ToLongDate(d2.ToUniversalTime())}";
            }
            catch
            {
                // Do Nothing
            }

            // Local Function
            DateTime LastEventDateTime(string query)
            {
                DateTime result = DateTime.MinValue;

                var eventLogQuery = new EventLogQuery("System", PathType.LogName, query);
                using (var elReader = new EventLogReader(eventLogQuery))
                {
                    EventRecord eventInstance = elReader.ReadEvent();
                    while (eventInstance != null)
                    {
                        if (eventInstance.TimeCreated.HasValue)
                        {
                            var thisTime = eventInstance.TimeCreated.Value.ToUniversalTime();
                            if (thisTime > result)
                            {
                                result = thisTime;
                            }
                            else
                            {
                                Debugger.Break();
                            }
                        }

                        eventInstance = elReader.ReadEvent();
                    }
                }

                if (result == DateTime.MinValue)
                {
                    result = DateTime.UtcNow;
                }
                return result;
            }
        }

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

        private void GetGitStatus(object o)
        {
            var result = new List<string>();
            result.Add("Git Origin");
            result.AddRange(RunCommand("git.exe", "config --get remote.origin.url", AddInLocation));

            // Ensure status is accurate
            RunCommand("git.exe", "fetch", AddInLocation);

            // git status -s -b --porcelain == Gets Branch, Status and a List of any changed files
            var changedFiles = RunCommand("git.exe", "status -s -b --porcelain", AddInLocation);
            if (changedFiles.Any())
            {
                result.Add("Git Branch, Status & Changed files");
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

            int idx = 0;
            foreach (var screen in Screen.AllScreens)
            {
                idx++;
                var primary = screen.Primary ? "[P]" : "";
                screens.Add($"#{idx}{primary}: {screen.Bounds.Width}x{screen.Bounds.Height} @ {screen.Bounds.X},{screen.Bounds.Y}");
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

                    // .Net 4.8
                    if (releaseKey >= 528372)
                    {
                        DotNetVersion = $".NET 4.8 (W10 2004) [{releaseKey}]";
                        return;
                    }
                    if (releaseKey >= 528049)
                    {
                        DotNetVersion = $".NET 4.8 [{releaseKey}]";
                        return;
                    }
                    if (releaseKey >= 528040)
                    {
                        DotNetVersion = $".NET 4.8 (W10 1903) [{releaseKey}]";
                        return;
                    }

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
                        DotNetVersion = $".Net Version Unknown [{releaseKey}]";
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
            string module = $"{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                string message;
                IpAddress = "0.0.0.0";

                for (int i = 0; i < 2; i++)
                {
                    foreach (string place in _placesToTry)
                    {
                        _attempts++;

                        try
                        {
                            message = $"Attempt #{_attempts} using '{place}'";
                            StartUpTimings.Add(message);
                            Debug.WriteLine(message);

                            if (place.Contains("chem4word"))
                            {
                                GetInternalVersion(place);
                            }
                            else
                            {
                                GetExternalVersion(place);
                            }

                            // Exit out of inner loop
                            if (!IpAddress.Contains("0.0.0.0"))
                            {
                                break;
                            }
                        }
                        catch (Exception exception)
                        {
                            Debug.WriteLine(exception.Message);
                            StartUpTimings.Add(exception.Message);
                        }
                    }

                    // Exit out of outer loop
                    if (!IpAddress.Contains("0.0.0.0"))
                    {
                        break;
                    }
                }

                if (IpAddress.Contains("0.0.0.0"))
                {
                    // Handle failure
                    IpAddress = "8.8.8.8";
                }

                _ipStopwatch.Stop();

                message = $"{module} took {_ipStopwatch.ElapsedMilliseconds.ToString("#,000", CultureInfo.InvariantCulture)}ms";
                StartUpTimings.Add(message);
                Debug.WriteLine(message);
            }
            catch (ThreadAbortException threadAbortException)
            {
                // Do Nothing
                Debug.WriteLine(threadAbortException.Message);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                StartUpTimings.Add(exception.Message);
            }
        }

        private void GetInternalVersion(string url)
        {
            var data = GetData(url);

            // Tidy Up the data
            data = data.Replace("Your IP address : ", "");
            data = data.Replace("UTC Date : ", "");
            data = data.Replace("<br/>", "|");
            data = data.Replace("<br />", "|");

            string[] lines = data.Split('|');

            if (lines[0].Contains(":"))
            {
                string[] ipV6Parts = lines[0].Split(':');
                // Must have between 4 and 8 parts
                if (ipV6Parts.Length >= 4 && ipV6Parts.Length <= 8)
                {
                    IpAddress = "IpAddress " + lines[0];
                    IpObtainedFrom = $"IpAddress V6 obtained from '{url}' on attempt {_attempts}";
                }
            }

            if (lines[0].Contains("."))
            {
                // Must have 4 parts
                string[] ipV4Parts = lines[0].Split('.');
                if (ipV4Parts.Length == 4)
                {
                    IpAddress = "IpAddress " + lines[0];
                    IpObtainedFrom = $"IpAddress V4 obtained from '{url}' on attempt {_attempts}";
                }
            }

            if (lines.Length > 1)
            {
                ServerUtcDateRaw = lines[1];
                ServerUtcDateTime = FromPhpDate(lines[1]);
                SystemUtcDateTime = DateTime.UtcNow;

                UtcOffset = SystemUtcDateTime.Ticks - ServerUtcDateTime.Ticks;
            }
        }

        private void GetExternalVersion(string url)
        {
            var data = GetData(url);

            if (data.Contains(":"))
            {
                IpAddress = "IpAddress " + data;
                IpObtainedFrom = $"IpAddress V6 obtained from '{url}' on attempt {_attempts}";
            }

            if (data.Contains("."))
            {
                IpAddress = "IpAddress " + data;
                IpObtainedFrom = $"IpAddress V4 obtained from '{url}' on attempt {_attempts}";
            }
        }

        private string GetData(string url)
        {
            string result = "0.0.0.0";

            var securityProtocol = ServicePointManager.SecurityProtocol;

            try
            {
                if (url.StartsWith("https"))
                {
                    ServicePointManager.SecurityProtocol = securityProtocol | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                }

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                if (request != null)
                {
                    request.UserAgent = "Chem4Word Add-In";
                    request.Timeout = url.Contains("chem4word") ? 5000 : 2500;

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    try
                    {
                        // Get Server Date header i.e. "Tue, 01 Jan 2019 19:52:46 GMT"
                        ServerDateHeader = response.Headers["date"];
                        SystemUtcDateTime = DateTime.UtcNow;
                        ServerUtcDateTime = DateTime.Parse(ServerDateHeader).ToUniversalTime();
                        UtcOffset = SystemUtcDateTime.Ticks - ServerUtcDateTime.Ticks;
                    }
                    catch
                    {
                        // Indicate failure
                        ServerDateHeader = null;
                        SystemUtcDateTime = DateTime.MinValue;
                    }

                    if (HttpStatusCode.OK.Equals(response.StatusCode))
                    {
                        var stream = response.GetResponseStream();
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                result = reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (WebException webException)
            {
                StartUpTimings.Add(webException.Status == WebExceptionStatus.Timeout
                                       ? $"Timeout: '{url}'"
                                       : webException.Message);
            }
            catch (Exception exception)
            {
                StartUpTimings.Add(exception.Message);
            }
            finally
            {
                ServicePointManager.SecurityProtocol = securityProtocol;
            }

            return result;
        }

        private DateTime FromPhpDate(string line)
        {
            string[] p = line.Split(',');
            var serverUtc = new DateTime(int.Parse(p[0]), int.Parse(p[1]), int.Parse(p[2]), int.Parse(p[3]), int.Parse(p[4]), int.Parse(p[5]));
            return DateTime.SpecifyKind(serverUtc, DateTimeKind.Utc);
        }
    }
}