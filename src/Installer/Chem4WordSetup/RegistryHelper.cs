// ---------------------------------------------------------------------------
//  Copyright (c) 2021, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Win32;

namespace Chem4WordSetup
{
    public static class RegistryHelper
    {
        private static int _counter = 1;

        public static void WriteAction(string action)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.Chem4WordSetupRegistryKey);
            if (key != null)
            {
                int procId = 0;
                try
                {
                    procId = Process.GetCurrentProcess().Id;
                }
                catch
                {
                    //
                }
                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                key.SetValue($"{timestamp} [{procId}.{_counter++.ToString("000")}]", $"[{procId}] {action}");
            }
        }
    }
}