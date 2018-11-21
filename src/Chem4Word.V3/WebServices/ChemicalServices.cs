// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.Helpers;
using Chem4Word.Telemetry;
using IChem4Word.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace Chem4Word.WebServices
{
    public class ChemicalServices
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        private IChem4WordTelemetry Telemetry { get; set; }

        public ChemicalServices(IChem4WordTelemetry telemetry)
        {
            Telemetry = telemetry;

            // http://byterot.blogspot.com/2016/07/singleton-httpclient-dns.html
            var sp = ServicePointManager.FindServicePoint(new Uri(Globals.Chem4WordV3.SystemOptions.Chem4WordWebServiceUri));
            sp.ConnectionLeaseTimeout = 60 * 1000; // 1 minute
        }

        public ChemicalServicesResult GetChemicalServicesResult(string molfile)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            DateTime started = DateTime.Now;

            ChemicalServicesResult data = null;

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var formData = new List<KeyValuePair<string, string>>();

                    formData.Add(new KeyValuePair<string, string>("mol", molfile));
                    formData.Add(new KeyValuePair<string, string>("machine", SystemHelper.GetMachineId()));
                    formData.Add(new KeyValuePair<string, string>("version", Globals.Chem4WordV3.AddInInfo.AssemblyVersionNumber));

#if DEBUG
                    formData.Add(new KeyValuePair<string, string>("debug", "true"));
#endif

                    var content = new FormUrlEncodedContent(formData);

                    httpClient.Timeout = TimeSpan.FromSeconds(15);
                    httpClient.DefaultRequestHeaders.Add("user-agent", "Chem4Word");

                    try
                    {
                        var response = httpClient.PostAsync(Globals.Chem4WordV3.SystemOptions.Chem4WordWebServiceUri, content).Result;
                        if (response.Content != null)
                        {
                            var responseContent = response.Content;
                            var jsonContent = responseContent.ReadAsStringAsync().Result;

                            try
                            {
                                data = JsonConvert.DeserializeObject<ChemicalServicesResult>(jsonContent);
                            }
                            catch (Exception e3)
                            {
                                Telemetry.Write(module, "Exception", e3.Message);
                                Telemetry.Write(module, "Exception(Data)", jsonContent);
                            }

                            if (data != null)
                            {
                                if (data.Messages.Any())
                                {
                                    Telemetry.Write(module, "Timing", string.Join(Environment.NewLine, data.Messages));
                                }
                                if (data.Errors.Any())
                                {
                                    Telemetry.Write(module, "Exception(Data)", string.Join(Environment.NewLine, data.Errors));
                                }
                            }
                        }
                    }
                    catch (Exception e2)
                    {
                        Telemetry.Write(module, "Exception", e2.Message);
                        Telemetry.Write(module, "Exception", e2.ToString());
                    }
                }
            }
            catch (Exception e1)
            {
                Telemetry.Write(module, "Exception", e1.Message);
                Telemetry.Write(module, "Exception", e1.ToString());
            }

            DateTime ended = DateTime.Now;
            TimeSpan duration = ended - started;

            Telemetry.Write(module, "Timing", $"Calling Azure http Function Took {SafeDouble.Duration(duration.TotalMilliseconds)}ms");

            return data;
        }
    }
}