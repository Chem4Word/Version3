using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Chem4Word.Helpers
{
    /// <summary>
    /// The sole purpose of this class is to keep references to assemblies which may only be used in supporting assemblies
    /// </summary>
    public class ReferenceKeeper
    {
        public CloudTable Table { get; set; }
    }
}
