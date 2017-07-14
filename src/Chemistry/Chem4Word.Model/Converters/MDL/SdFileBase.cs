using System.IO;

namespace Chem4Word.Model.Converters
{
    public class SdFileBase
    {
        public virtual SdfState ImportFromStream(StreamReader reader, Molecule molecule, out string message)
        {
            message = null;
            return SdfState.Null;
        }

        public virtual void ExportToStream(Molecule molecule, StreamWriter writer, out string message)
        {
            message = null;
        }
    }
}