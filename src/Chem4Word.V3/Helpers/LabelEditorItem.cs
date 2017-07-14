using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chem4Word.Helpers
{
    public class LabelEditorItem
    {
        //<cml:formula concise="C 4 H 9 N 1 O 2" />
        //<cml:formula id = "c1" convention="pubchem:formula" inline="C4H9NO2"></cml:formula>
        //<cml:name id = "l1" dictRef="pubchem:cid">119</cml:name>

        public string Id { get; set; }
        public string Attribute { get; set; }
        public string Value { get; set; }
        public bool InUse { get; set; }
        public bool IsNew { get; set; }
    }
}
