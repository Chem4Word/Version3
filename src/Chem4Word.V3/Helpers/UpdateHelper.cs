// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using Chem4Word.Core.Helpers;
using Chem4Word.Core.UI.Forms;
using Chem4Word.UI;
using Microsoft.Win32;

namespace Chem4Word.Helpers
{
    public static class UpdateHelper
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        public static int CheckForUpdates(int frequency)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            Debug.WriteLine($"{module} days: {frequency}");
            try
            {
                if (!string.IsNullOrEmpty(Globals.Chem4WordV3.AddInInfo.DeploymentPath))
                {
                    #region CheckForUpdate

                    bool doCheck = true;

                    ReadSavedValues();

                    if (frequency > 0)
                    {
                        TimeSpan delta = DateTime.Today - Globals.Chem4WordV3.VersionLastChecked;
                        if (Math.Abs(delta.TotalDays) < frequency)
                        {
                            doCheck = false;
                        }

                        if (doCheck)
                        {
                            Debug.WriteLine($"Saving date last checked in Registry as Today");
                            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.Chem4WordRegistryKey);
                            key?.SetValue(Constants.RegistryValueNameLastCheck, DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                        }
                    }

                    if (doCheck)
                    {
                        bool update = false;
                        using (new UI.WaitCursor())
                        {
                            update = FetchUpdateInfo();
                        }

                        if (update)
                        {
                            ShowUpdateForm();
                        }
                    }

                    #endregion CheckForUpdate
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Globals.Chem4WordV3.Telemetry.Write(module, "Exception", ex.Message);
                Globals.Chem4WordV3.Telemetry.Write(module, "Exception", ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Globals.Chem4WordV3.Telemetry.Write(module, "Exception", ex.InnerException.Message);
                    Globals.Chem4WordV3.Telemetry.Write(module, "Exception", ex.InnerException.StackTrace);
                }
            }

            Globals.Chem4WordV3.ShowOrHideUpdateShield();

            return Globals.Chem4WordV3.VersionsBehind;
        }

        public static void ReadSavedValues()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(Constants.Chem4WordRegistryKey);
                if (key != null)
                {
                    var names = key.GetValueNames();

                    if (names.Contains(Constants.RegistryValueNameLastCheck))
                    {
                        try
                        {
                            var lastChecked = key.GetValue(Constants.RegistryValueNameLastCheck).ToString();
                            if (!string.IsNullOrEmpty(lastChecked))
                            {
                                DateTime last = SafeDate.Parse(lastChecked);
                                Globals.Chem4WordV3.VersionLastChecked = last;
                            }
                        }
                        catch
                        {
                            Globals.Chem4WordV3.VersionLastChecked = DateTime.Now.AddDays(-30);
                        }
                    }
                    else
                    {
                        Globals.Chem4WordV3.VersionLastChecked = DateTime.Now.AddDays(-30);
                    }

                    if (names.Contains(Constants.RegistryValueNameLastCheck))
                    {
                        try
                        {
                            var behind = key.GetValue(Constants.RegistryValueNameVersionsBehind).ToString();
                            Globals.Chem4WordV3.VersionsBehind = string.IsNullOrEmpty(behind) ? 0 : int.Parse(behind);
                        }
                        catch
                        {
                            Globals.Chem4WordV3.VersionsBehind = 0;
                        }
                    }
                    else
                    {
                        Globals.Chem4WordV3.VersionsBehind = 0;
                    }
                }
                else
                {
                    Globals.Chem4WordV3.VersionLastChecked = DateTime.Now.AddDays(-30);
                    Globals.Chem4WordV3.VersionsBehind = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Globals.Chem4WordV3.Telemetry.Write(module, "Exception", ex.Message);
                Globals.Chem4WordV3.Telemetry.Write(module, "Exception", ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Globals.Chem4WordV3.Telemetry.Write(module, "Exception", ex.InnerException.Message);
                    Globals.Chem4WordV3.Telemetry.Write(module, "Exception", ex.InnerException.StackTrace);
                }
            }
        }

        public static void ReadThisVersion(Assembly assembly)
        {
            if (Globals.Chem4WordV3.ThisVersion == null)
            {
                Globals.Chem4WordV3.ThisVersion = XDocument.Parse(ResourceHelper.GetStringResource(assembly, "Data.This-Version.xml"));
            }
        }

        public static bool FetchUpdateInfo()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            bool updateRequired = false;

            Globals.Chem4WordV3.VersionsBehind = 0;

            var assembly = Assembly.GetExecutingAssembly();

            ReadThisVersion(assembly);
            if (Globals.Chem4WordV3.ThisVersion != null)
            {
                string currentVersionNumber = Globals.Chem4WordV3.ThisVersion.Root.Element("Number").Value;
                DateTime currentReleaseDate = SafeDate.Parse(Globals.Chem4WordV3.ThisVersion.Root.Element("Released").Value);
                Debug.WriteLine("Current Version " + currentVersionNumber + " Released " + currentReleaseDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture));

                string xml = GetVersionsXmlFile();
                if (!string.IsNullOrEmpty(xml))
                {
                    #region Got Our File

                    Globals.Chem4WordV3.AllVersions = XDocument.Parse(xml);
                    var versions = Globals.Chem4WordV3.AllVersions.XPathSelectElements("//Version");
                    foreach (var version in versions)
                    {
                        var thisVersionNumber = version.Element("Number").Value;
                        DateTime thisVersionDate = SafeDate.Parse(version.Element("Released").Value);
                        Debug.WriteLine("New Version " + thisVersionNumber + " Released " + thisVersionDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture));
                        if (thisVersionDate > currentReleaseDate)
                        {
                            Globals.Chem4WordV3.VersionsBehind++;
                            updateRequired = true;
                        }
                    }

                    // Save VersionsBehind and Last Checked for next start up
                    Debug.WriteLine($"Saving Versions Behind in Registry: {Globals.Chem4WordV3.VersionsBehind}");
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.Chem4WordRegistryKey);
                    key?.SetValue(Constants.RegistryValueNameVersionsBehind, Globals.Chem4WordV3.VersionsBehind.ToString());

                    #endregion Got Our File
                }
            }
            else
            {
                Globals.Chem4WordV3.Telemetry.Write(module, "Error", "Failed to parse resource 'Data.This-Version.xml'");
            }

            return updateRequired;
        }

        public static void ShowUpdateForm()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            AutomaticUpdate au = new AutomaticUpdate(Globals.Chem4WordV3.Telemetry);
            au.TopLeft = Globals.Chem4WordV3.WordTopLeft;
            au.CurrentVersion = Globals.Chem4WordV3.ThisVersion;
            au.NewVersions = Globals.Chem4WordV3.AllVersions;

            DialogResult dr = au.ShowDialog();
        }

        private static string GetVersionsXmlFile()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            string VersionsFile = $"{Constants.Chem4WordVersionFiles}/Chem4Word-Versions.xml";
            string PrimaryDomain = "https://www.chem4word.co.uk";
            string[] Domains = { "https://www.chem4word.co.uk", "http://www.chem4word.com", "https://chem4word.azurewebsites.net" };
            string VersionsFileMarker = "<Id>f3c4f4db-2fff-46db-b14a-feb8e09f7742</Id>";

            string contents = null;

            bool foundOurXmlFile = false;
            foreach (var domain in Domains)
            {
                using (HttpClient client = new HttpClient())
                {
                    string exceptionMessage;

                    try
                    {
                        Globals.Chem4WordV3.Telemetry.Write(module, "Information", $"Looking for Chem4Word-Versions.xml at {domain}");

                        client.DefaultRequestHeaders.Add("user-agent", "Chem4Word VersionChecker");
                        client.BaseAddress = new Uri(domain);
                        var response = client.GetAsync(VersionsFile).Result;
                        response.EnsureSuccessStatusCode();
                        //Debug.WriteLine(response.StatusCode);
                        string result = response.Content.ReadAsStringAsync().Result;
                        if (result.Contains(VersionsFileMarker))
                        {
                            foundOurXmlFile = true;
                            contents = domain.Equals(PrimaryDomain) ? result : result.Replace(PrimaryDomain, domain);
                        }
                        else
                        {
                            Globals.Chem4WordV3.Telemetry.Write(module, "Exception", $"Chem4Word-Versions.xml at {domain} is corrupt");
                            Globals.Chem4WordV3.Telemetry.Write(module, "Exception(Data)", result);
                        }
                    }
                    catch (ArgumentNullException nex)
                    {
                        exceptionMessage = GetExceptionMessages(nex);
                        Globals.Chem4WordV3.Telemetry.Write(module, "Exception", $"ArgumentNullException: [{domain}] - {exceptionMessage}");
                    }
                    catch (HttpRequestException hex)
                    {
                        exceptionMessage = GetExceptionMessages(hex);
                        Globals.Chem4WordV3.Telemetry.Write(module, "Exception", $"HttpRequestException: [{domain}] - {exceptionMessage}");
                    }
                    catch (WebException wex)
                    {
                        exceptionMessage = GetExceptionMessages(wex);
                        Globals.Chem4WordV3.Telemetry.Write(module, "Exception", $"WebException: [{domain}] - {exceptionMessage}");
                    }
                    catch (Exception ex)
                    {
                        exceptionMessage = GetExceptionMessages(ex);
                        Globals.Chem4WordV3.Telemetry.Write(module, "Exception", $"Exception: [{domain}] - {exceptionMessage}");
                    }

                    if (foundOurXmlFile)
                    {
                        break;
                    }
                }
            }

            return contents;
        }

        private static string GetExceptionMessages(Exception ex)
        {
            string message = ex.Message;

            if (ex.InnerException != null)
            {
                message = message + Environment.NewLine + GetExceptionMessages(ex.InnerException);
            }

            return message;
        }

        public static void ClearSettings()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.Chem4WordRegistryKey);
                if (key != null)
                {
                    key.DeleteValue(Constants.RegistryValueNameLastCheck, false);
                    key.DeleteValue(Constants.RegistryValueNameVersionsBehind, false);
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }
    }
}