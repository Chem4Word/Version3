// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Windows.Forms;

namespace Chem4Word.Searcher.ChEBIPlugin
{
    public class WaitCursor : IDisposable
    {
        #region Fields

        private Cursor _previousCursor;

        #endregion Fields

        #region Constructors

        public WaitCursor(Cursor newCursor)
        {
            _previousCursor = Cursor.Current;

            Cursor.Current = newCursor;
        }

        public WaitCursor() : this(Cursors.WaitCursor)
        {
        }

        #endregion Constructors

        #region IDisposable Members

        public void Dispose()
        {
            Cursor.Current = _previousCursor;
        }

        #endregion IDisposable Members
    }
}