// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Collections.Generic;
using Word = Microsoft.Office.Interop.Word;

namespace Chem4Word.Helpers
{
    public static class ContentControlHelper
    {
        public static void Insert2D(Word.ContentControl cc, string tempfileName, string bookmarkName, string tag)
        {

        }

        public static void Update2D(Word.ContentControl cc, string tempfileName, string bookmarkName, string tag)
        {

        }

        public static void Insert1D(Word.ContentControl cc, string text, bool isFormula, string tag)
        {

        }

        public static void Update1D(Word.ContentControl cc, string text, bool isFormula, string tag)
        {

        }

        public static void UpdateStructures(Word.Document doc, Model.Model model, string guidString, string tempFilename)
        {

        }

        public static void RefreshAllStructures(Word.Document doc)
        {

        }

        public static string GetInlineText(Model.Model model, string prefix, ref bool isFormula, out string source)
        {
            source = null;
            return null;
        }

        private static void SetRichText(Word.ContentControl cc, string text, bool isFormula)
        {

        }

        public static List<string> GetUsed1D(Word.Document doc, string guidString)
        {
            return null;
        }

        public static List<string> GetUsed2D(Word.Document doc, string guidString)
        {
            return null;
        }
    }
}