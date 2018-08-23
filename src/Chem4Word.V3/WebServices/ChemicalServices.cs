using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Windows.Forms;
using IChem4Word.Contracts;
using Newtonsoft.Json;

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
        }

        public ChemicalServicesResult GetChemicalServicesResult(string molfile)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            ChemicalServicesResult data = null;

            var formData = new List<KeyValuePair<string, string>>();

            formData.Add(new KeyValuePair<string, string>("mol", molfile));
            formData.Add(new KeyValuePair<string, string>("version", Globals.Chem4WordV3.AddInInfo.AssemblyVersionNumber));

#if DEBUG
            formData.Add(new KeyValuePair<string, string>("debug", "true"));
#endif

            var content = new FormUrlEncodedContent(formData);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("user-agent", "Chem4Word");

            var response = httpClient.PostAsync(Globals.Chem4WordV3.SystemOptions.Chem4WordWebServiceUri, content).Result;
            if (response.Content != null)
            {
                var responseContent = response.Content;
                var jsonContent = responseContent.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject<ChemicalServicesResult>(jsonContent);
                if (data.Messages.Any())
                {
                    Telemetry.Write(module, "Timing", string.Join(Environment.NewLine, data.Messages));
                }
                if (data.Errors.Any())
                {
                    Telemetry.Write(module, "Exception(Data)", string.Join(Environment.NewLine, data.Errors));
                }
            }

            return data;
        }
    }
}