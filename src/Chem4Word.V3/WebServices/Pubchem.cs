// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using IChem4Word.Contracts;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Chem4Word.WebServices
{
    public class Pubchem
    {
        private readonly XslCompiledTransform xslt = new XslCompiledTransform();
        private string errorMessage;
        private string searchResultsAvailable;
        private int resultsCount;
        private string webEnv;
        private int resultPosition;
        private int retStart;
        private bool moreResults;
        private bool prevResults;
        private const int numResults = 20;

        public XDocument ResultDocument { get; set; }

        public string SearchTerm { get; set; }

        public string ErrorMessage { get; set; }

        private IChem4WordTelemetry Telemetry { get; set; }

        public Pubchem(IChem4WordTelemetry telemetry)
        {
            Telemetry = telemetry;
        }

        private void PerformSearch()
        {
            prevResults = false;
            moreResults = false;
            var request = (HttpWebRequest)
                WebRequest.Create(
                    string.Format(CultureInfo.InvariantCulture,
                        "{0}entrez/eutils/esearch.fcgi?db=pccompound&term={1}&retmode=xml&relevanceorder=on&usehistory=y&retmax={2}",
                        Globals.C4wVNextAddIn.SystemOptions.PubChemWebServiceUri, SearchTerm, numResults));

            request.Timeout = 30000;
            request.UserAgent = "Chem4Word";

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                if (HttpStatusCode.OK.Equals(response.StatusCode))
                {
                    using (
                        var resStream = response.GetResponseStream())
                    {
                        var resultDocument = XDocument.Load(new StreamReader(resStream));
                        // Get the count of results
                        resultsCount = int.Parse(resultDocument.XPathSelectElement("//Count").Value);
                        // Current position
                        retStart = int.Parse(resultDocument.XPathSelectElement("//RetStart").Value) + 1;
                        // Max records
                        resultPosition = int.Parse(resultDocument.XPathSelectElement("//RetMax").Value);
                        // WebEnv for history
                        webEnv = resultDocument.XPathSelectElement("//WebEnv").Value;
                        //////// Set flags for More/Prev buttons
                        //////PrevResults = false;
                        //////if (resultPosition < resultsCount)
                        //////{
                        //////    MoreResults = true;
                        //////}
                        //////else
                        //////{
                        //////    MoreResults = false;
                        //////}
                        //////// Set property used for label on form
                        //////SearchResultsAvailable = "Displaying " + retStart + " to " + resultPosition + " of " + resultsCount + " results";
                        var ids = resultDocument.XPathSelectElements("//Id");
                        var count = ids.Count();
                        if (count > 0)
                        {
                            var sb = new StringBuilder();
                            for (var i = 0; i < count; i++)
                            {
                                var id = ids.ElementAt(i);
                                if (i > 0)
                                {
                                    sb.Append(",");
                                }
                                sb.Append(id.Value);
                            }
                            GetData(sb.ToString());
                        }
                        else
                        {
                            ErrorMessage = "Sorry, no results were found.";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Sorry bad request. Status code: " + response.StatusCode, "Name2Structure",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "The operation has timed out".Equals(ex.Message)
                                    ? "Please try again later - the service has timed out"
                                    : ex.Message;
            }
            finally
            {
            }
        }

        private void GetData(string idlist)
        {
            var request = (HttpWebRequest)
                WebRequest.Create(
                    string.Format(CultureInfo.InvariantCulture,
                        "{0}entrez/eutils/esummary.fcgi?db=pccompound&id={1}&retmode=xml",
                        Globals.C4wVNextAddIn.SystemOptions.PubChemWebServiceUri, idlist));

            request.Timeout = 30000;
            request.UserAgent = "Chem4Word";

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                if (HttpStatusCode.OK.Equals(response.StatusCode))
                {
                    // we will read data via the response stream
                    using (
                        var resStream = response.GetResponseStream())
                    {
                        var resultDocument = XDocument.Load(new StreamReader(resStream));
                        var compounds = resultDocument.XPathSelectElements("//DocSum");
                        foreach (var compound in compounds)
                        {
                            var id = compound.XPathSelectElement("./Id");
                            var name = compound.XPathSelectElement("./Item[@Name='IUPACName']");
                            var smiles = compound.XPathSelectElement("./Item[@Name='CanonicalSmile']");
                            var formula = compound.XPathSelectElement("./Item[@Name='MolecularFormula']");
                            // Add to a list view ...
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Sorry bad request. Status code: " + response.StatusCode, "Name2Structure",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "The operation has timed out".Equals(ex.Message)
                                    ? "Please try again later - the service has timed out"
                                    : ex.Message;
            }
            finally
            {
            }
        }

        private void Import(string id)
        {
            /*
             * Using Pubchem REST PUG search to return compounds
             */
            var request = (HttpWebRequest)
                WebRequest.Create(
                    string.Format(CultureInfo.InvariantCulture,
                    "{0}/rest/pug/compound/cid/{1}/record/XML",
                    Globals.C4wVNextAddIn.SystemOptions.PubChemRestApiUri, id));

            request.Timeout = 30000;
            request.UserAgent = "Chem4Word";
            request.Credentials = CredentialCache.DefaultCredentials;

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                if (HttpStatusCode.OK.Equals(response.StatusCode))
                {
                    // we will read data via the response stream
                    using (
                        var resStream = response.GetResponseStream())
                    {
                        var xml = XDocument.Load(new StreamReader(resStream));
                        var result = new XDocument();
                        using (var writer = result.CreateWriter())
                        {
                            xslt.Transform(xml.CreateReader(), writer);
                            ResultDocument = result;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Sorry bad request. Status code: " + response.StatusCode, "Name2Structure",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "The operation has timed out".Equals(ex.Message)
                                    ? "Please try again later - the service has timed out"
                                    : ex.Message;
            }
            finally
            {
            }
        }
    }
}
