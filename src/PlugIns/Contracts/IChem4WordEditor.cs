using System.Windows.Forms;

namespace IChem4Word.Contracts
{
    public interface IChem4WordEditor : IChem4WordCommon
    {
        DialogResult Edit();
    }
}