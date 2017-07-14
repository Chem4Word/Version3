using Microsoft.Win32;
using System;
using System.Globalization;

namespace Chem4WordSetup
{
    public static class RegistryHelper
    {
        public static void WriteAction(string action)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.Chem4WordSetupRegistryKey);
            if (key != null)
            {
                string actionName = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                key.SetValue(actionName, action);
            }
        }
    }
}