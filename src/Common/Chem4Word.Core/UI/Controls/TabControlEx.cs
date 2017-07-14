using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Chem4Word.Core.UI.Controls
{
    public class TabControlEx : TabControl
    {
        private const int TCM_FIRST = 0x1300;
        private const int TCM_ADJUSTRECT = TCM_FIRST + 40;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == TCM_ADJUSTRECT)
            {
                Rect rc = (Rect)m.GetLParam(typeof(Rect));
                rc.Left -= 4;
                rc.Right += 4;
                rc.Top -= 2;
                rc.Bottom += 4;
                Marshal.StructureToPtr(rc, m.LParam, true);
            }
            base.WndProc(ref m);
        }

        internal struct Rect { public int Left, Top, Right, Bottom; }
    }
}