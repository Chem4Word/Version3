using System;
using System.IO;

namespace Chem4WordSetup
{
    public static class OfficeFinder
    {
        // http://www.ryadel.com/en/microsoft-office-default-installation-folders-versions/

        private const string _wordExe = "winword.exe";

        // Standard Install
        private const string _template1 = @"Microsoft Office\Office{0}";

        private const string _template16 = @"Microsoft Office\root\Office{0}";
        private const string _template365 = @"Microsoft Office\Office{0}";

        // Click To Run
        private const string _template2 = @"Microsoft Office {0}\Client{1}\Root\Office{0}";

        public static bool WordExists(int version)
        {
            bool found = false;

            int major = 0;

            switch (version)
            {
                case 2010:
                    major = 14;
                    break;

                case 2013:
                    major = 15;
                    break;

                case 2016:
                    major = 16;
                    break;
            }

            if (Environment.Is64BitOperatingSystem)
            {
                // Try "C:\Program Files (x86)" first
                string pf = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                found = FindExe(pf, major);

                if (!found)
                {
                    // Try "C:\Program Files"
                    pf = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                    found = FindExe(pf, major);
                }
            }
            else
            {
                // Try "C:\Program Files"
                string pf = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                found = FindExe(pf, major);
            }

            return found;
        }

        private static bool FindExe(string programFiles, int version)
        {
            bool found = false;

            string path = "";
            string path365 = "";
            //try the non-office and office 365 installation
            if (version == 16)
            {
                path = Path.Combine(programFiles, string.Format(_template16, version));
                path365 = Path.Combine(programFiles, string.Format(_template365, version));
            }
            else
            {
                path = Path.Combine(programFiles, string.Format(_template1, version));
            }

            if (Directory.Exists(path) || Directory.Exists(path365))
            {
                found = File.Exists(Path.Combine(path, _wordExe)) || File.Exists(Path.Combine(path365, _wordExe));
            }

            if (!found)
            {
                string bitness = "X86";
                if (Environment.Is64BitOperatingSystem)
                {
                    bitness = "X64";
                }

                path = Path.Combine(programFiles, string.Format(_template2, version, bitness));

                if (Directory.Exists(path))
                {
                    found = File.Exists(Path.Combine(path, _wordExe));
                }
            }

            return found;
        }
    }
}