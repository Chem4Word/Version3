using System.Collections.Generic;

namespace Chem4Word.WebServices
{
    public class ChemicalServicesResult
    {
        public ChemicalProperties[] Properties { get; set; }
        public List<string> Errors { get; set; }
        public List<string> Messages { get; set; }
    }
}