using System.Drawing;
using System.Windows.Forms;

namespace IChem4Word.Contracts
{
    public interface IChem4WordSearcher : IChem4WordCommon
    {
        DialogResult Search();

        int DisplayOrder { get; }

        string ShortName { get; }

        Image Image { get; }
    }
}