﻿// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.Win32;

namespace Chem4Word.Helpers
{
    public static class RegistryHelper
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public static void SendSetupActions()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            RegistryKey rk = Registry.CurrentUser.OpenSubKey(Constants.Chem4WordSetupRegistryKey, true);
            if (rk != null)
            {
                string[] names = rk.GetValueNames();
                foreach (var name in names)
                {
                    string message = rk.GetValue(name).ToString();

                    string timestamp = name;
                    int bracket = timestamp.IndexOf("[", StringComparison.InvariantCulture);
                    if (bracket > 0)
                    {
                        timestamp = timestamp.Substring(0, bracket).Trim();
                    }

                    Globals.Chem4WordV3.Telemetry.Write(module, "Setup", $"{timestamp} {message}");

                    rk.DeleteValue(name);
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
                foreach (var name in names)
                {
                    string message = rk.GetValue(name).ToString();

                    string timestamp = name;
                    int bracket = timestamp.IndexOf("[", StringComparison.InvariantCulture);
                    if (bracket > 0)
                    {
                        timestamp = timestamp.Substring(0, bracket).Trim();
                    }

                    Globals.Chem4WordV3.Telemetry.Write(module, "Update", $"{timestamp} {message}");

                    rk.DeleteValue(name);
                }
            }
        }
    }
}
