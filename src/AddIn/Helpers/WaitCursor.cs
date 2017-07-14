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