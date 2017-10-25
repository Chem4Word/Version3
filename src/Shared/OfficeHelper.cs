// Shared file (Add As Link)

using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace Chem4Word.Shared
{
    public static class OfficeHelper
    {

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
                result = GetFromKnownPathSearch();
            }

            return result;
        }

        private static string GetFromRegistryMethod2()
        {
            string result = null;

            // HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Word.Application\CurVer == "Word.Application.15"
            // HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Word.Application.15\CLSID == "{000209FF-0000-0000-C000-000000000046}"

            // HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{000209FF-0000-0000-C000-000000000046}\LocalServer32 == "C:\PROGRA~2\MICROS~1\Office15\WINWORD.EXE /Automation"
            // HKEY_LOCAL_MACHINE\SOFTWARE\Classes\WOW6432Node\CLSID\{000209FF-0000-0000-C000-000000000046}\LocalServer32 == "C:\PROGRA~2\MICROS~1\Office15\WINWORD.EXE /Automation"

            // OR

            // HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\14.0\Word\InstallRoot == "C:\Program Files\Microsoft Office\root\Office16\"
            // HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Office\14.0\Word\InstallRoot == "C:\Program Files (x86)\Microsoft Office\root\Office16\"

            return result;
        }

        private static string GetFromKnownPathSearch()
        {
            string result = null;

            string[] templates =
            {
                @"Microsoft Office\Office{0}",
                @"Microsoft Office\root\Office{0}",
                @"Microsoft Office {0}\Client{1}\Root\Office{0}"
            };
            int[] versions = {16, 15, 14, 17};

            foreach (var version in versions)
            {
                foreach (var template in templates)
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

        private static string GetFromRegistryMethod1()
        {
            string result = null;

            string name = @"Software\Microsoft\Windows\CurrentVersion\App Paths\winword.exe";

            // 1. Look inside CURRENT_USER:
            RegistryKey mainKey = Registry.CurrentUser;
            mainKey = mainKey.OpenSubKey(name, false);

            if (mainKey == null)
            {
                //2. Look inside LOCAL_MACHINE:
                mainKey = Registry.LocalMachine;
                mainKey = mainKey.OpenSubKey(name, false);
            }

            if (mainKey != null)
            {
                string path = mainKey.GetValue(string.Empty).ToString();
                result = path;
            }

            return result;
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
