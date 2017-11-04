using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Globalization;

namespace Chem4WordUpdater
{
    public static class RegistryHelper
    {
        private static int _counter = 1;

        public static void WriteAction(string action)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.Chem4WordUpdateRegistryKey);
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
                string actionName = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                key.SetValue($"{actionName}-{_counter++}", $"[{procId}] {action}");
            }
        }
    }
}