using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chem4Word.Common
{
    public class OfficeHelper
    {
        public OfficeHelper()
        {
            //
        }

        public string GetWinWordPath()
        {
            string result = null;

            // Return path if Word 2010 or greater found
            result = @"C:\Temp";

            return result;
        }
    }
}
