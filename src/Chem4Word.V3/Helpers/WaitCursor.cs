// ---------------------------------------------------------------------------
//  Copyright (c) 2020, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Windows.Input;

namespace Chem4Word.UI
{
    public class WaitCursor : IDisposable
    {
        private Cursor _previousCursor;

        public WaitCursor(Cursor newCursor)
        {
            _previousCursor = Mouse.OverrideCursor;

            Mouse.OverrideCursor = newCursor;
        }

        public WaitCursor() : this(Cursors.Wait)
        {
        }

        #region IDisposable Members

        public void Dispose()
        {
            Mouse.OverrideCursor = _previousCursor;
        }

        #endregion IDisposable Members
    }
}