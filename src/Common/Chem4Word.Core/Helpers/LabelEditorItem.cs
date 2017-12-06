// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

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
