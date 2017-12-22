// Shared file (Add As Link)

using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;

namespace Chem4Word.Shared
{
    public static class OfficeHelper
    {
        private static string WinwordAppPath = @"Software\Microsoft\Windows\CurrentVersion\App Paths\winword.exe";

        private static string InstallRootTemplate64 = @"SOFTWARE\Microsoft\Office\{0}.0\Word\InstallRoot";
        private static string InstallRootTemplate32 = @"SOFTWARE\Wow6432Node\Microsoft\Office\{0}.0\Word\InstallRoot";

        private static int[] OfficeVersions = { 16, 15, 14, 17 };

        private static string[] FileSearchTemplates =
        {
            @"Microsoft Office\Office{0}",
            @"Microsoft Office\root\Office{0}",
            @"Microsoft Office {0}\Client{1}\Root\Office{0}"
        };

        public static int GetWinWordVersion(string path = null)
        {
            string wordVersionNumber = String.Empty;

            if (path == null)
            {
                path = GetWinWordPath();
            }
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    // Strip off any quotes
                    path = path.Replace("\"", "");
                    FileVersionInfo fi = FileVersionInfo.GetVersionInfo(path);
                    wordVersionNumber = fi.FileVersion;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            return HumanOfficeVersion(wordVersionNumber);
        }

        public static string GetWordProduct()
        {
            string result = "";

            string officeProductName = GetOfficeProductName();

            string pathToWinword = GetWinWordPath();
            FileVersionInfo fi = FileVersionInfo.GetVersionInfo(pathToWinword);
            string wordVersionNumber = fi.FileVersion;

            if (string.IsNullOrEmpty(officeProductName))
            {
                officeProductName = fi.ProductName;
            }

            string servicePack = GetOfficeServicePack(fi.FileVersion);
            if (!string.IsNullOrEmpty(servicePack))
            {
                officeProductName = officeProductName + " " + servicePack;
            }

            // Get a bit more information about this version
            if (officeProductName.Contains("-0000000FF1CE}"))
            {
                officeProductName += Environment.NewLine + fi.ProductName;
            }

            result = officeProductName + " [" + wordVersionNumber + "]";

            return result;
        }

        public static string GetOfficeProductName()
        {
            string result = null;

            // Get HKEY_CLASSES_ROOT\Word.Document\CurVer
            string currentVersion = GetRegistryValue(Registry.ClassesRoot, @"Word.Document\CurVer", null);
            if (!string.IsNullOrEmpty(currentVersion))
            {
                // Get HKEY_CLASSES_ROOT\Word.Document.12\DefaultIcon
                string defaultIcon = GetRegistryValue(Registry.ClassesRoot, $@"{currentVersion}\DefaultIcon", null);
                if (!string.IsNullOrEmpty(defaultIcon))
                {
                    int start = defaultIcon.IndexOf("{", StringComparison.Ordinal);
                    int end = defaultIcon.IndexOf("}", StringComparison.Ordinal);
                    if (end > start)
                    {
                        string officeGuid = defaultIcon.Substring(start, end - start + 1);
                        Debug.WriteLine("Office Guid: " + officeGuid);

                        result = DecodeOfficeGuid(officeGuid);
                    }
                }
            }

            return result;
        }

        private static string DecodeOfficeGuid(string officeGuid)
        {
            // Office 2007 https://support.microsoft.com/en-us/kb/928516
            // Office 2010 https://support.microsoft.com/en-us/kb/2186281
            // Office 2013 https://support.microsoft.com/en-us/kb/2786054
            // Office 2016 https://support.microsoft.com/en-us/kb/3120274
            // Office 2019 ???

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

                case 17:

                    #region Office 2019

                    switch (productId)
                    {
                        case "000F":
                            result = "Microsoft Office 2019 Professional Plus";
                            break;

                        case "0011":
                            result = "Microsoft Office Professional Plus 2019";
                            break;

                        case "0012":
                            result = "Microsoft Office Standard 2019";
                            break;

                        case "001B":
                            result = "Microsoft Word 2019";
                            break;

                        default:
                            result = "Microsoft Office 2019 " + officeGuid;
                            break;
                    }
                    break;

                    #endregion Office 2019
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

        private static string GetOfficeServicePack(string wordVersionString)
        {
            // Source: https://buildnumbers.wordpress.com/office/
            // Plus correction from https://support.microsoft.com/en-us/kb/2121559
            string servicePack = "";
            string[] parts = wordVersionString.Split('.');
            int major = int.Parse(parts[0]);
            int build = int.Parse(parts[2]);
            switch (major)
            {
                case 12: // Word 2007
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

                case 14: // Word 2010
                    if (build >= 6029)
                    {
                        servicePack = "SP1";
                    }
                    if (build >= 7015)
                    {
                        servicePack = "SP2";
                    }
                    break;

                case 15: // Word 2013
                    if (build >= 4569)
                    {
                        servicePack = "SP1";
                    }
                    break;

                case 16: // Word 2016
                    break;

                case 17: // Word 2019
                    break;
            }

            if (!string.IsNullOrEmpty(servicePack))
            {
                servicePack = " " + servicePack;
            }

            return servicePack;
        }

        private static string GetWinWordPath()
        {
            string result = null;

            result = GetFromRegistryMethod1();

            if (result == null)
            {
                result = GetFromRegistryMethod2();
            }

            if (result == null)
            {
                result = GetFromRegistryMethod3();
            }

            if (result == null)
            {
                result = GetFromKnownPathSearch();
            }

            return result;
        }

        private static string GetFromRegistryMethod1()
        {
            string result = null;

            result = GetRegistryValue(Registry.LocalMachine, WinwordAppPath, null);
            if (string.IsNullOrEmpty(result))
            {
                result = GetRegistryValue(Registry.CurrentUser, WinwordAppPath, null);
            }

            return result;
        }

        private static string GetFromRegistryMethod2()
        {
            string result = null;

            // Get HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Word.Application\CurVer
            string currentVersion = GetRegistryValue(Registry.LocalMachine, @"SOFTWARE\Classes\Word.Application\CurVer", null);
            if (!string.IsNullOrEmpty(currentVersion))
            {
                // Get HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Word.Application.14\CLSID
                string classId = GetRegistryValue(Registry.LocalMachine, $@"SOFTWARE\Classes\{currentVersion}\CLSID", null);
                if (!string.IsNullOrEmpty(classId))
                {
                    // Try Wow6432Node first
                    // Get HKEY_LOCAL_MACHINE\SOFTWARE\Classes\WOW6432Node\CLSID\{000209FF-0000-0000-C000-000000000046}\LocalServer32
                    string localServer32 = GetRegistryValue(Registry.LocalMachine, $@"SOFTWARE\Wow6432Node\Classes\CLSID\{classId}\LocalServer32", null);
                    if (!string.IsNullOrEmpty(localServer32))
                    {
                        // Expect "C:\PROGRA~2\MICROS~1\Office15\WINWORD.EXE /Automation"
                        //    or  "C:\Program Files\Microsoft Office\Root\Office16\WINWORD.EXE" /Automation
                        string temp = localServer32.Split('/')[0].Trim();
                        if (File.Exists(temp))
                        {
                            result = temp;
                        }
                    }

                    if (string.IsNullOrEmpty(result))
                    {
                        // Try alternative
                        // Get HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{000209FF-0000-0000-C000-000000000046}\LocalServer32
                        string localServer64 = GetRegistryValue(Registry.LocalMachine, $@"SOFTWARE\Classes\CLSID\{classId}\LocalServer32", null);
                        if (!string.IsNullOrEmpty(localServer64))
                        {
                            // Expect "C:\PROGRA~2\MICROS~1\Office15\WINWORD.EXE /Automation"
                            //    or  "C:\Program Files\Microsoft Office\Root\Office16\WINWORD.EXE" /Automation
                            string temp = localServer64.Split('/')[0].Trim();
                            if (File.Exists(temp))
                            {
                                result = temp;
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static string GetFromRegistryMethod3()
        {
            string result = null;

            // HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\14.0\Word\InstallRoot == "C:\Program Files\Microsoft Office\root\Office16\"
            // HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Office\14.0\Word\InstallRoot == "C:\Program Files (x86)\Microsoft Office\root\Office16\"

            foreach (var version in OfficeVersions)
            {
                string search = string.Format(InstallRootTemplate32, version);
                string path = GetRegistryValue(Registry.LocalMachine, search, "Path");
                if (!string.IsNullOrEmpty(path))
                {
                    if (Directory.Exists(path))
                    {
                        if (File.Exists(Path.Combine(path, "WinWord.exe")))
                        {
                            result = Path.Combine(path, "WinWord.exe");
                            break;
                        }
                    }
                }

                search = string.Format(InstallRootTemplate64, version);
                path = GetRegistryValue(Registry.LocalMachine, search, "Path");
                if (!string.IsNullOrEmpty(path))
                {
                    if (Directory.Exists(path))
                    {
                        if (File.Exists(Path.Combine(path, "WinWord.exe")))
                        {
                            result = Path.Combine(path, "WinWord.exe");
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private static string GetRegistryValue(RegistryKey rootKey, string path, string value)
        {
            string result = null;

            RegistryKey rk = rootKey;
            rk = rk.OpenSubKey(path, false);
            if (rk != null)
            {
                result = rk.GetValue(value) as string;
            }
            else
            {
                Debug.WriteLine("");
            }

            return result;
        }

        private static string GetFromKnownPathSearch()
        {
            // http://www.ryadel.com/en/microsoft-office-default-installation-folders-versions/

            string result = null;

            foreach (var version in OfficeVersions)
            {
                foreach (var template in FileSearchTemplates)
                {
                    string programFiles;
                    if (Environment.Is64BitOperatingSystem)
                    {
                        programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                        result = FindExe(programFiles, template, version);

                        if (string.IsNullOrEmpty(result))
                        {
                            programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                            result = FindExe(programFiles, template, version);
                        }
                    }
                    else
                    {
                        programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                        result = FindExe(programFiles, template, version);
                    }

                    if (!string.IsNullOrEmpty(result))
                    {
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(result))
                {
                    break;
                }
            }

            return result;
        }

        private static string FindExe(string programFiles, string template, int version)
        {
            string result = null;
            string path;

            if (template.Contains("{1}"))
            {
                path = Path.Combine(programFiles, string.Format(template, version, "X86"));
                result = FoundAt(path);

                path = Path.Combine(programFiles, string.Format(template, version, "X64"));
                if (Directory.Exists(path))
                {
                    result = FoundAt(path);
                }
            }
            else
            {
                path = Path.Combine(programFiles, string.Format(template, version));
                if (Directory.Exists(path))
                {
                    result = FoundAt(path);
                }
            }

            return result;
        }

        private static string FoundAt(string path)
        {
            string foundAt = null;

            if (Directory.Exists(path))
            {
                if (File.Exists(Path.Combine(path, "WinWord.exe")))
                {
                    foundAt = Path.Combine(path, "WinWord.exe");
                }
            }

            return foundAt;
        }

        // Generate Human version number from major of word's internal version number
        private static int HumanOfficeVersion(string wordVersionString)
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

                    case 17:
                        version = 2019;
                        break;
                }
            }
            return version;
        }
    }
}