// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core;
using Chem4Word.Core.UI.Forms;
using Chem4Word.Model.Converters;
using IChem4Word.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Chem4Word.Searcher.PubChemPlugIn
{
    public partial class SearchPubChem : Form
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public System.Windows.Point TopLeft { get; set; }
        public IChem4WordTelemetry Telemetry { get; set; }
        public string ProductAppDataPath { get; set; }
        public string PubChemId { get; set; }

        public string Cml { get; set; }

        public Options UserOptions { get; set; }

        private int resultsCount;
        private string webEnv;
        private int lastResult;
        private int firstResult;
        private const int numResults = 20;

        private string lastSelected = string.Empty;
        private string lastMolfile = string.Empty;

        public SearchPubChem()
        {
            InitializeComponent();
        }

        private void SearchPubChem_Load(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (TopLeft.X != 0 && TopLeft.Y != 0)
                {
                    Left = (int)TopLeft.X;
                    Top = (int)TopLeft.Y;
                }

                NextButton.Enabled = false;
                PreviousButton.Enabled = false;
                ImportButton.Enabled = false;
                Results.Enabled = false;
                AcceptButton = SearchButton;

                Results.Items.Clear();
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!string.IsNullOrEmpty(SearchFor.Text))
                {
                    Telemetry.Write(module, "Information", $"User searched for '{SearchFor.Text}'");
                }
                ExecuteSearch(0);
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                ExecuteSearch(-1);
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                ExecuteSearch(1);
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                DialogResult = DialogResult.OK;
                Hide();
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void Results_SelectedIndexChanged(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                Debug.WriteLine("Results_SelectedIndexChanged");
                lastSelected = FetchStructure();
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void Results_DoubleClick(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                Debug.WriteLine("Results_DoubleClick");
                DialogResult = DialogResult.OK;
                Hide();
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }

        private void ExecuteSearch(int direction)
        {
            if (!string.IsNullOrEmpty(SearchFor.Text))
            {
                Cursor = Cursors.WaitCursor;

                string webCall;
                if (direction == 0)
                {
                    webCall = string.Format(CultureInfo.InvariantCulture,
                            "{0}entrez/eutils/esearch.fcgi?db=pccompound&term={1}&retmode=xml&relevanceorder=on&usehistory=y&retmax={2}",
                            UserOptions.PubChemWebServiceUri, SearchFor.Text, UserOptions.ResultsPerCall);
                }
                else
                {
                    if (direction == 1)
                    {
                        int startFrom = firstResult + numResults;
                        webCall = string.Format(CultureInfo.InvariantCulture,
                                "{0}entrez/eutils/esearch.fcgi?db=pccompound&term={1}&retmode=xml&relevanceorder=on&usehistory=y&retmax={2}&WebEnv={3}&RetStart={4}",
                                UserOptions.PubChemWebServiceUri, SearchFor.Text, UserOptions.ResultsPerCall, webEnv, startFrom);
                    }
                    else
                    {
                        int startFrom = firstResult - numResults;
                        webCall = string.Format(CultureInfo.InvariantCulture,
                                "{0}entrez/eutils/esearch.fcgi?db=pccompound&term={1}&retmode=xml&relevanceorder=on&usehistory=y&retmax={2}&WebEnv={3}&RetStart={4}",
                                UserOptions.PubChemWebServiceUri, SearchFor.Text, UserOptions.ResultsPerCall, webEnv, startFrom);
                    }
                }

                var request = (HttpWebRequest)WebRequest.Create(webCall);

                request.Timeout = 30000;
                request.UserAgent = "Chem4Word";

                HttpWebResponse response;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    if (HttpStatusCode.OK.Equals(response.StatusCode))
                    {
                        using (var resStream = response.GetResponseStream())
                        {
                            var resultDocument = XDocument.Load(new StreamReader(resStream));
                            // Get the count of results
                            resultsCount = int.Parse(resultDocument.XPathSelectElement("//Count").Value);
                            // Current position
                            firstResult = int.Parse(resultDocument.XPathSelectElement("//RetStart").Value);
                            int fetched = int.Parse(resultDocument.XPathSelectElement("//RetMax").Value);
                            lastResult = firstResult + fetched;
                            // WebEnv for history
                            webEnv = resultDocument.XPathSelectElement("//WebEnv").Value;

                            // Set flags for More/Prev buttons

                            if (lastResult > numResults)
                            {
                                PreviousButton.Enabled = true;
                            }
                            else
                            {
                                PreviousButton.Enabled = false;
                            }

                            if (lastResult < resultsCount)
                            {
                                NextButton.Enabled = true;
                            }
                            else
                            {
                                NextButton.Enabled = false;
                            }

                            var ids = resultDocument.XPathSelectElements("//Id");
                            var count = ids.Count();
                            Results.Items.Clear();

                            if (count > 0)
                            {
                                // Set form title
                                Text = $"Search PubChem - Showing {firstResult + 1} to {lastResult} [of {resultsCount}]";
                                Refresh();

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
                                // Set error box
                                ErrorsAndWarnings.Text = "Sorry, no results were found.";
                            }
                        }
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"Bad request. Status code: {response.StatusCode}");
                        UserInteractions.AlertUser(sb.ToString());
                    }
                }
                catch (Exception ex)
                {
                    ErrorsAndWarnings.Text = "The operation has timed out".Equals(ex.Message)
                                        ? "Please try again later - the service has timed out"
                                        : ex.Message;
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void GetData(string idlist)
        {
            var request = (HttpWebRequest)
                WebRequest.Create(
                    string.Format(CultureInfo.InvariantCulture,
                        "{0}entrez/eutils/esummary.fcgi?db=pccompound&id={1}&retmode=xml",
                        UserOptions.PubChemWebServiceUri, idlist));

            request.Timeout = 30000;
            request.UserAgent = "Chem4Word";

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                if (HttpStatusCode.OK.Equals(response.StatusCode))
                {
                    Results.Enabled = true;

                    // we will read data via the response stream
                    using (var resStream = response.GetResponseStream())
                    {
                        var resultDocument = XDocument.Load(new StreamReader(resStream));
                        var compounds = resultDocument.XPathSelectElements("//DocSum");
                        if (compounds.Any())
                        {
                            foreach (var compound in compounds)
                            {
                                var id = compound.XPathSelectElement("./Id");
                                var name = compound.XPathSelectElement("./Item[@Name='IUPACName']");
                                //var smiles = compound.XPathSelectElement("./Item[@Name='CanonicalSmile']");
                                var formula = compound.XPathSelectElement("./Item[@Name='MolecularFormula']");
                                ListViewItem lvi = new ListViewItem(id.Value);

                                lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, name.Value));
                                //lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, smiles.ToString()));
                                lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, formula.Value));

                                Results.Items.Add(lvi);
                                // Add to a list view ...
                            }
                        }
                        else
                        {
                            Debug.WriteLine("Something went wrong");
                        }
                    }
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"Bad request. Status code: {response.StatusCode}");
                    UserInteractions.AlertUser(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                ErrorsAndWarnings.Text = "The operation has timed out".Equals(ex.Message)
                                    ? "Please try again later - the service has timed out"
                                    : ex.Message;
            }
        }

        private string FetchStructure()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            string result = lastSelected;
            ImportButton.Enabled = false;

            ListView.SelectedListViewItemCollection selected = Results.SelectedItems;
            if (selected.Count > 0)
            {
                ListViewItem item = selected[0];
                string pubchemId = item.Text;
                PubChemId = pubchemId;

                if (!pubchemId.Equals(lastSelected))
                {
                    Cursor = Cursors.WaitCursor;

                    // http://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/cid/241/record/SDF

                    try
                    {
                        var request = (HttpWebRequest)WebRequest.Create(
                            string.Format(CultureInfo.InvariantCulture, "{0}rest/pug/compound/cid/{1}/record/SDF",
                                UserOptions.PubChemRestApiUri, pubchemId));

                        request.Timeout = 30000;
                        request.UserAgent = "Chem4Word";

                        HttpWebResponse response;

                        response = (HttpWebResponse)request.GetResponse();
                        if (HttpStatusCode.OK.Equals(response.StatusCode))
                        {
                            // we will read data via the response stream
                            using (var resStream = response.GetResponseStream())
                            {
                                lastMolfile = new StreamReader(resStream).ReadToEnd();
                                SdFileConverter sdFileConverter = new SdFileConverter();
                                Model.Model model = sdFileConverter.Import(lastMolfile);
                                this.flexDisplayControl1.Chemistry = model;
                                if (model.AllWarnings.Count > 0 || model.AllErrors.Count > 0)
                                {
                                    Telemetry.Write(module, "Exception(Data)", lastMolfile);
                                    List<string> lines = new List<string>();
                                    if (model.AllErrors.Count > 0)
                                    {
                                        Telemetry.Write(module, "Exception", string.Join(Environment.NewLine, model.AllErrors));
                                        lines.Add("Errors(s)");
                                        lines.AddRange(model.AllErrors);
                                    }
                                    if (model.AllWarnings.Count > 0)
                                    {
                                        Telemetry.Write(module, "Exception", string.Join(Environment.NewLine, model.AllWarnings));
                                        lines.Add("Warnings(s)");
                                        lines.AddRange(model.AllWarnings);
                                    }
                                    ErrorsAndWarnings.Text = string.Join(Environment.NewLine, lines);
                                }
                                else
                                {
                                    CMLConverter cmlConverter = new CMLConverter();
                                    Cml = cmlConverter.Export(model);
                                    ImportButton.Enabled = true;
                                }
                            }
                            result = pubchemId;
                        }
                        else
                        {
                            result = string.Empty;
                            lastMolfile = string.Empty;

                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine($"Bad request. Status code: {response.StatusCode}");
                            UserInteractions.AlertUser(sb.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorsAndWarnings.Text = "The operation has timed out".Equals(ex.Message)
                                            ? "Please try again later - the service has timed out"
                                            : ex.Message;
                    }
                    finally
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }

            return result;
        }

        private void SearchPubChem_FormClosing(object sender, FormClosingEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (DialogResult != DialogResult.OK)
                {
                    DialogResult = DialogResult.Cancel;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Telemetry, TopLeft, module, ex).ShowDialog();
            }
        }
    }
}