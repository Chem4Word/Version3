// Shared file (Add As Link)

using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace Chem4Word.Shared
{
    public static class OfficeHelper
    {

        public static string GetWinWordPath()
        {
            string result = null;

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
                // Return path of WinWord
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
