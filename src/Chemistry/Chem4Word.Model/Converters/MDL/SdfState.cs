// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

namespace Chem4Word.Model.Converters.MDL
{
    public enum SdfState
    {
        Null,
        EndOfCtab,
        EndOfData,
        Error,
        Unsupported
    }
}