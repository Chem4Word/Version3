// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using IChem4Word.Contracts;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace Chem4Word.Searcher.OpsinPlugIn
{
    public partial class SearchOpsin : Form
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public System.Windows.Point TopLeft { get; set; }
        public IChem4WordTelemetry Telemetry { get; set; }
        public string ProductAppDataPath { get; set; }

        public string Cml { get; set; }

        public Options UserOptions { get; set; }

        public SearchOpsin()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            if (!string.IsNullOrEmpty(SearchFor.Text))
            {
                Telemetry.Write(module, "Information", $"User searched for '{SearchFor.Text}'");
                Cursor = Cursors.WaitCursor;

                UriBuilder builder = new UriBuilder(UserOptions.OpsinWebServiceUri + SearchFor.Text);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(builder.Uri);
                request.Timeout = 30000;
                request.Accept = "chemical/x-cml";
                request.UserAgent = "Chem4Word";

                HttpWebResponse response;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        ProcessResponse(response);
                    }
                    else
                    {
                        ShowFailureMessage($"An unexpected status code of {response.StatusCode} was returned by the server");
                    }
                }
                catch (WebException ex)
                {
                    HttpWebResponse webResponse = (HttpWebResponse)ex.Response;
                    switch (webResponse.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            ShowFailureMessage($"No valid representation of the name '{SearchFor.Text}' has been found");
                            break;

                        case HttpStatusCode.RequestTimeout:
                            ShowFailureMessage("Please try again later - the service has timed out");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ShowFailureMessage($"An unexpected error has occurred: {ex.Message}");
                }
                Cursor = Cursors.Default;
            }
        }

        private void ShowFailureMessage(string message)
        {
            LabelInfo.Text = message;
            flexDisplayControl1.Chemistry = "";
            ImportButton.Enabled = false;
        }

        private void ProcessResponse(HttpWebResponse response)
        {
            LabelInfo.Text = "";
            // read data via the response stream
            using (Stream resStream = response.GetResponseStream())
            {
                if (resStream != null)
                {
                    StreamReader sr = new StreamReader(resStream);
                    Cml = sr.ReadToEnd();
                    flexDisplayControl1.Chemistry = Cml;
                    ImportButton.Enabled = true;
                }
            }
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Hide();
        }

        private void SearchOpsin_Load(object sender, EventArgs e)
        {
            if (TopLeft.X != 0 && TopLeft.Y != 0)
            {
                Left = (int)TopLeft.X;
                Top = (int)TopLeft.Y;
            }

            ImportButton.Enabled = false;
            LabelInfo.Text = "";
            AcceptButton = SearchButton;
        }
    }
}