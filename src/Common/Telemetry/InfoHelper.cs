// ---------------------------------------------------------------------------
//  Copyright (c) 2023, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chem4Word.Telemetry
{
    public static class InfoHelper
    {
        // HKEY_CURRENT_USER\Software\Microsoft\Office\Word\Addins
        // HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\Word\Addins
        // HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Office\Word\Addins

        public static List<AddInProperties> GetListOfAddIns()
        {
            var result = new List<AddInProperties>();

            try
            {
                result.AddRange(GetListOfAddIns(Registry.CurrentUser, @"Software\Microsoft\Office\Word\Addins"));
            }
            catch
            {
                // Do nothing
            }
            try
            {
                result.AddRange(GetListOfAddIns(Registry.LocalMachine, @"Software\Microsoft\Office\Word\Addins"));
            }
            catch
            {
                // Do nothing
            }
            try
            {
                result.AddRange(GetListOfAddIns(Registry.LocalMachine, @"Software\WOW6432Node\Microsoft\Office\Word\Addins"));
            }
            catch
            {
                // Do nothing
            }

            return result;
        }

        private static List<AddInProperties> GetListOfAddIns(RegistryKey hive, string location)
        {
            var result = new List<AddInProperties>();

            var subKeys = hive.OpenSubKey(location);
            if (subKeys != null)
            {
                foreach (string keyName in subKeys.GetSubKeyNames())
                {
                    result.Add(GetProperties(hive, $@"{location}\{keyName}"));
                }
            }

            return result;
        }

        private static AddInProperties GetProperties(RegistryKey hive, string location)
        {
            var properties = new AddInProperties();

            var registryKey = hive.OpenSubKey(location);
            if (registryKey != null)
            {
                var parts = registryKey.Name.Split('\\');
                var sb = new StringBuilder();

                switch (parts[0])
                {
                    case "HKEY_CURRENT_USER":
                        sb.Append(@"HKCU\");
                        break;

                    case "HKEY_LOCAL_MACHINE":
                        sb.Append(@"HKLM\");
                        break;
                }

                sb.Append(@"...\");
                if (registryKey.Name.Contains("WOW6432Node"))
                {
                    sb.Append(@"WOW6432Node\...\");
                }

                sb.Append(parts[parts.Length - 1]);
                properties.KeyName = sb.ToString();

                properties.Description = GetStringValue(registryKey, "Description");
                properties.FriendlyName = GetStringValue(registryKey, "FriendlyName");
                properties.Manifest = GetStringValue(registryKey, "Manifest");
                properties.LoadBehaviour = GetIntValue(registryKey, "LoadBehavior");

            }

            return properties;
        }

        private static string GetStringValue(RegistryKey hive, string location)
        {
            string result = hive.GetValue(location, "").ToString();

            return result;
        }

        private static int GetIntValue(RegistryKey hive, string location)
        {
            int result;

            var temp = hive.GetValue(location, "-1").ToString();
            int.TryParse(temp, out result);

            return result;
        }
    }
}