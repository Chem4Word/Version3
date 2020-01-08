// ---------------------------------------------------------------------------
//  Copyright (c) 2020, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Chem4Word.Core.Helpers;
using Microsoft.Win32;

namespace Chem4Word.Helpers
{
    public static class RegistryHelper
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        public static void SendSetupActions()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            RegistryKey rk = Registry.CurrentUser.OpenSubKey(Constants.Chem4WordSetupRegistryKey, true);
            if (rk != null)
            {
                string[] names = rk.GetValueNames();
                List<string> values = new List<string>();
                foreach (var name in names)
                {
                    string message = rk.GetValue(name).ToString();

                    string timestamp = name;
                    int bracket = timestamp.IndexOf("[", StringComparison.InvariantCulture);
                    if (bracket > 0)
                    {
                        timestamp = timestamp.Substring(0, bracket).Trim();
                    }

                    values.Add($"{timestamp} {message}");
                    rk.DeleteValue(name);
                }

                if (values.Any())
                {
                    Globals.Chem4WordV3.Telemetry.Write(module, "Setup", string.Join(Environment.NewLine, values));
                }
            }
        }

        public static void SendUpdateActions()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            RegistryKey rk = Registry.CurrentUser.OpenSubKey(Constants.Chem4WordUpdateRegistryKey, true);
            if (rk != null)
            {
                string[] names = rk.GetValueNames();
                List<string> values = new List<string>();
                foreach (var name in names)
                {
                    string message = rk.GetValue(name).ToString();

                    string timestamp = name;
                    int bracket = timestamp.IndexOf("[", StringComparison.InvariantCulture);
                    if (bracket > 0)
                    {
                        timestamp = timestamp.Substring(0, bracket).Trim();
                    }

                    values.Add($"{timestamp} {message}");
                    rk.DeleteValue(name);
                }
                if (values.Any())
                {
                    Globals.Chem4WordV3.Telemetry.Write(module, "Update", string.Join(Environment.NewLine, values));
                }
            }
        }
    }
}