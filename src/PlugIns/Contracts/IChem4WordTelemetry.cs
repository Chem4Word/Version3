// ---------------------------------------------------------------------------
//  Copyright (c) 2020, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

namespace IChem4Word.Contracts
{
    public interface IChem4WordTelemetry
    {
        void Write(string source, string level, string message);
    }
}