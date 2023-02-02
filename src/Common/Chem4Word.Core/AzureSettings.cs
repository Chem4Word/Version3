// ---------------------------------------------------------------------------
//  Copyright (c) 2023, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.Helpers;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Chem4Word.Core
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AzureSettings
    {
        /// <summary>
        /// Used in ChemicalServices.cs; ServicePointManager.FindServicePoint(...)
        /// </summary>
        [JsonProperty]
        public string ChemicalServicesUri { get; set; }

        /// <summary>
        /// Used in AzureServiceBusWriter.cs to construct ServiceBusClient
        /// </summary>
        [JsonProperty]
        public string ServiceBusEndPoint { get; set; }

        /// <summary>
        /// Used in AzureServiceBusWriter.cs to construct ServiceBusClient
        /// </summary>
        [JsonProperty]
        public string ServiceBusToken { get; set; }

        /// <summary>
        /// Used in AzureServiceBusWriter.cs; CreateSender
        /// </summary>
        [JsonProperty]
        public string ServiceBusQueue { get; set; }

        /// <summary>
        /// Stores when last checked
        /// </summary>
        public string LastChecked { get; set; }

        private bool _dirty;

        public AzureSettings()
        {
            // Must have empty constructor to allow this class to deserialize itself
        }

        public AzureSettings(bool load)
        {
            if (load)
            {
                Load();
            }
        }

        private void Load()
        {
            var today = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            // 1. Attempt to get from registry
            GetFromRegistry();

            // 2. If not found or too old; attempt to get from Chem4Word web site(s)
            if (string.IsNullOrEmpty(LastChecked) || !LastChecked.Equals(today))
            {
                GetFromWebsite(today);
            }

            // 2.1 Save to registry if found
            if (_dirty)
            {
                SaveToRegistry(today);
            }
        }

        private void GetFromRegistry()
        {
            var key = Registry.CurrentUser.OpenSubKey(Constants.Chem4WordAzureSettingsRegistryKey, true);
            if (key != null)
            {
                var names = key.GetValueNames();

                if (names.Contains(nameof(ChemicalServicesUri)))
                {
                    var chemicalServicesUri = key.GetValue(nameof(ChemicalServicesUri)).ToString();
                    ChemicalServicesUri = chemicalServicesUri;
                }

                if (names.Contains(nameof(ServiceBusEndPoint)))
                {
                    var serviceBusEndPoint = key.GetValue(nameof(ServiceBusEndPoint)).ToString();
                    ServiceBusEndPoint = serviceBusEndPoint;
                }

                if (names.Contains(nameof(ServiceBusToken)))
                {
                    var serviceBusToken = key.GetValue(nameof(ServiceBusToken)).ToString();
                    ServiceBusToken = serviceBusToken;
                }

                if (names.Contains(nameof(ServiceBusQueue)))
                {
                    var serviceBusQueue = key.GetValue(nameof(ServiceBusQueue)).ToString();
                    ServiceBusQueue = serviceBusQueue;
                }

                if (names.Contains(nameof(LastChecked)))
                {
                    var lastChecked = key.GetValue(nameof(LastChecked)).ToString();
                    LastChecked = lastChecked;
                }
            }
        }

        private void GetFromWebsite(string today)
        {
            var file = $"{Constants.Chem4WordVersionFiles}/AzureSettings.json";

            var securityProtocol = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = securityProtocol | SecurityProtocolType.Tls12;

            var found = false;
            var temp = string.Empty;

            foreach (var domain in Constants.OurDomains)
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.DefaultRequestHeaders.Add("user-agent", "Chem4Word GetAzureSettings");
                        client.BaseAddress = new Uri(domain);
                        var response = client.GetAsync(file).Result;
                        response.EnsureSuccessStatusCode();

                        var result = response.Content.ReadAsStringAsync().Result;
                        if (result.Contains("Chem4WordAzureSettings"))
                        {
                            found = true;
                            temp = result;
                        }
                    }
                    catch
                    {
                        //Debugger.Break()
                    }
                }

                if (found)
                {
                    break;
                }
            }

            if (!string.IsNullOrEmpty(temp))
            {
                var settings = JsonConvert.DeserializeObject<AzureSettings>(temp);
                ChemicalServicesUri = settings.ChemicalServicesUri;
                ServiceBusEndPoint = settings.ServiceBusEndPoint;
                ServiceBusToken = settings.ServiceBusToken;
                ServiceBusQueue = settings.ServiceBusQueue;
                LastChecked = today;
                _dirty = true;
            }
        }

        private void SaveToRegistry(string today)
        {
            var key = Registry.CurrentUser.CreateSubKey(Constants.Chem4WordAzureSettingsRegistryKey);
            if (key != null)
            {
                key.SetValue(nameof(ChemicalServicesUri), ChemicalServicesUri);
                key.SetValue(nameof(ServiceBusEndPoint), ServiceBusEndPoint);
                key.SetValue(nameof(ServiceBusToken), ServiceBusToken);
                key.SetValue(nameof(ServiceBusQueue), ServiceBusQueue);
                key.SetValue(nameof(LastChecked), today);
            }
        }
    }
}
