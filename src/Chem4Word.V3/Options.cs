// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.Helpers;
using Newtonsoft.Json;
using System.Windows;

namespace Chem4Word
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options
    {
        #region Web Services

        //[JsonProperty]
        public bool UseWebServices { get; set; }

        [JsonProperty]
        public string ChemSpiderWebServiceUri { get; set; }

        [JsonProperty]
        public string ChemSpiderRdfServiceUri { get; set; }

        #endregion Web Services

        #region Telemetry

        //[JsonProperty]
        public bool TelemetryEnabled { get; set; }

        #endregion Telemetry

        #region Automatic Updates

        //[JsonProperty]
        public bool AutoUpdateEnabled { get; set; }

        //[JsonProperty]
        public int AutoUpdateFrequency { get; set; }

        #endregion Automatic Updates

        #region Selected Plug Ins

        [JsonProperty]
        public string SelectedEditorPlugIn { get; set; }

        [JsonProperty]
        public string SelectedRendererPlugIn { get; set; }

        #endregion Selected Plug Ins

        // Not Saved
        public Point WordTopLeft { get; set; }

        public Options()
        {
            RestoreDefaults();
        }

        public Options Clone()
        {
            Options clone = new Options();

            clone.UseWebServices = UseWebServices;
            clone.ChemSpiderRdfServiceUri = ChemSpiderRdfServiceUri;
            clone.ChemSpiderWebServiceUri = ChemSpiderWebServiceUri;

            clone.TelemetryEnabled = TelemetryEnabled;

            clone.SelectedEditorPlugIn = SelectedEditorPlugIn;
            clone.SelectedRendererPlugIn = SelectedRendererPlugIn;

            clone.AutoUpdateEnabled = AutoUpdateEnabled;
            clone.AutoUpdateFrequency = AutoUpdateFrequency;

            return clone;
        }

        public void RestoreDefaults()
        {
            // User Options

            TelemetryEnabled = true;

            SelectedEditorPlugIn = Constants.DefaultEditorPlugIn;
            SelectedRendererPlugIn = Constants.DefaultRendererPlugIn;

            UseWebServices = true;
            ChemSpiderWebServiceUri = Constants.DefaultChemSpiderWebServiceUri;
            ChemSpiderRdfServiceUri = Constants.DefaultChemSpiderRdfServiceUri;

            AutoUpdateEnabled = true;
            AutoUpdateFrequency = 7;
        }
    }
}