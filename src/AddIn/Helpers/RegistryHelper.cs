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

        public static int CheckForUpdates(int days)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            return 0;
        }

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
                    Globals.Chem4WordV3.Telemetry.Write(module, "Setup", $"{name} {message}");
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
                    Globals.Chem4WordV3.Telemetry.Write(module, "Update", $"{name} {message}");
                    rk.DeleteValue(name);
                }
            }
        }
    }
}
