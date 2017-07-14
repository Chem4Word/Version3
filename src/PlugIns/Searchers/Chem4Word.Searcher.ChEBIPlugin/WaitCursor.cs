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