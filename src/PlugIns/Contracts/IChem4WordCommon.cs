// ---------------------------------------------------------------------------
//  Copyright (c) 2020, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Windows;

namespace IChem4Word.Contracts
{
    public interface IChem4WordCommon
    {
        // MetaData
        string Name { get; }

        string Description { get; }

        bool HasSettings { get; }

        // Helper Objects
        Point TopLeft { get; set; }

        IChem4WordTelemetry Telemetry { get; set; }

        string ProductAppDataPath { get; set; }

        // I/O
        Dictionary<string, string> Properties { get; set; }

        string Cml { get; set; }

        // Actions
        bool ChangeSettings(Point topLeft);

        void LoadSettings();
    }
}