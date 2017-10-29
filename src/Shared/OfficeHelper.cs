﻿// Shared file (Add As Link)

using System;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using Microsoft.Win32;

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

        public static string GetWinWordPath()
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
