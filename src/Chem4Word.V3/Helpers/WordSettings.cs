// ---------------------------------------------------------------------------
//  Copyright (c) 2023, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Microsoft.Office.Interop.Word;

namespace Chem4Word.Helpers
{
    public class WordSettings
    {
        private readonly bool _correctSentenceCaps;
        private readonly bool _smartCutPaste;
        private readonly bool _smartCursoring;
        private readonly bool _allowClickAndTypeMouse;

        public WordSettings(Application application)
        {
            _correctSentenceCaps = application.AutoCorrect.CorrectSentenceCaps;
            application.AutoCorrect.CorrectSentenceCaps = false;

            _smartCutPaste = application.Options.SmartCutPaste;
            application.Options.SmartCutPaste = false;

            _smartCursoring = application.Options.SmartCursoring;
            application.Options.SmartCursoring = false;

            _allowClickAndTypeMouse = application.Options.AllowClickAndTypeMouse;
            application.Options.AllowClickAndTypeMouse = false;
        }

        public void RestoreSettings(Application application)
        {
            application.AutoCorrect.CorrectSentenceCaps = _correctSentenceCaps;
            application.Options.SmartCutPaste = _smartCutPaste;
            application.Options.SmartCursoring = _smartCursoring;
            application.Options.AllowClickAndTypeMouse = _allowClickAndTypeMouse;
        }
    }
}