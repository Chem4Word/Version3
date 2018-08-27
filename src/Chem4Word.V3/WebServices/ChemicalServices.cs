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

            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public ChemicalServicesResult GetChemicalServicesResult(string molfile)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            ChemicalServicesResult data = null;

            using (var httpClient = new HttpClient())
            {
                var formData = new List<KeyValuePair<string, string>>();

                formData.Add(new KeyValuePair<string, string>("mol", molfile));
                formData.Add(new KeyValuePair<string, string>("version", Globals.Chem4WordV3.AddInInfo.AssemblyVersionNumber));

#if DEBUG
                formData.Add(new KeyValuePair<string, string>("debug", "true"));
#endif

                var content = new FormUrlEncodedContent(formData);

                httpClient.Timeout = TimeSpan.FromSeconds(5);
                httpClient.DefaultRequestHeaders.Add("user-agent", "Chem4Word");

                var response = httpClient.PostAsync(Globals.Chem4WordV3.SystemOptions.Chem4WordWebServiceUri, content).Result;
                if (response.Content != null)
                {
                    var responseContent = response.Content;
                    var jsonContent = responseContent.ReadAsStringAsync().Result;

                    try
                    {
                        data = JsonConvert.DeserializeObject<ChemicalServicesResult>(jsonContent);
                    }
                    catch (Exception e)
                    {
                        Telemetry.Write(module, "Exception", e.Message);
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

            return data;
        }
    }
}