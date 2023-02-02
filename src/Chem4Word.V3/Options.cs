// ---------------------------------------------------------------------------
//  Copyright (c) 2023, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Forms;
using Chem4Word.Core.Helpers;
using Newtonsoft.Json;

namespace Chem4Word
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Options
    {
        #region Telemetry

        [JsonProperty]
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

            Version browser = null;
            try
            {
                browser = new WebBrowser().Version;
            }
            catch
            {
                browser = null;
            }

            // Force CDW 702 if IE < 11
            if (browser?.Major < Constants.ChemDoodleWeb800MinimumBrowserVersion)
            {
                SelectedEditorPlugIn = Constants.DefaultEditorPlugIn702;
            }
            else
            {
                SelectedEditorPlugIn = Constants.DefaultEditorPlugIn800;
            }
            SelectedRendererPlugIn = Constants.DefaultRendererPlugIn;

            AutoUpdateEnabled = true;
            AutoUpdateFrequency = 7;
        }
    }
}