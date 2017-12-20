﻿// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using IChem4Word.Contracts;
using System.IO;
using System.Linq;
using System.Windows;
using Chem4Word.Core.Helpers;
using Chem4Word.Model.Converters;

namespace Chem4Word.Renderer.OoXmlV3.OOXML
{
    public static class OoXmlFile
    {
        /// <summary>
        /// Create an OpenXml Word Document from the CML
        /// </summary>
        /// <param name="cml">Input Chemistry</param>
        /// <param name="guid">Bookmark to create</param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string CreateFromCml(string cml, string guid, Options options, IChem4WordTelemetry telemetry, Point topLeft)
        {
            CMLConverter cc = new CMLConverter();
            Model.Model m = cc.Import(cml);

            string fileName = Path.Combine(Path.GetTempPath(), $"Chem4Word-V3-{guid}.docx");

            bool canRender = m.MeanBondLength > Constants.BondLengthTolerance / 2 || m.AllBonds.Count == 0;

            if (canRender)
            {
                string bookmarkName = "C4W_" + guid;

                // Create a Wordprocessing document.
                using (WordprocessingDocument package = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document))
                {
                    // Add a new main document part.
                    MainDocumentPart mdp = package.AddMainDocumentPart();
                    mdp.Document = new Document(new Body());
                    Body docbody = package.MainDocumentPart.Document.Body;

                    // This is for test
                    //AddParagraph(docbody, "Hello World", bookmarkName);
                    // This will be live
                    AddPictureFromModel(docbody, m, bookmarkName, options, telemetry, topLeft);

                    // Save changes to the main document part.
                    package.MainDocumentPart.Document.Save();
                }
            }

            return fileName;
        }

        /// <summary>
        /// Creates the DrawingML objects and adds them to the document
        /// </summary>
        /// <param name="docbody"></param>
        /// <param name="cml"></param>
        /// <param name="bookmarkName"></param>
        /// <param name="options"></param>
        private static void AddPictureFromModel(Body docbody, Model.Model model, string bookmarkName, Options options, IChem4WordTelemetry telemetry, Point topLeft)
        {
            Paragraph paragraph1 = new Paragraph();
            if (!string.IsNullOrEmpty(bookmarkName))
            {
                BookmarkStart bookmarkstart = new BookmarkStart();
                bookmarkstart.Name = bookmarkName;
                bookmarkstart.Id = "1";
                paragraph1.Append(bookmarkstart);
            }

            // This is where the work gets done ...
            OoXmlRenderer pic = new OoXmlRenderer(model, options, telemetry, topLeft);
            paragraph1.Append(pic.GenerateRun());

            if (!string.IsNullOrEmpty(bookmarkName))
            {
                BookmarkEnd bookmarkend = new BookmarkEnd();
                bookmarkend.Id = "1";
                paragraph1.Append(bookmarkend);
            }

            docbody.Append(paragraph1);
        }

        #region For Test

        private static void AddParagraph(Body docbody, string theText, string bookmarkName = null)
        {
            Paragraph paragraph = new Paragraph();
            if (!string.IsNullOrEmpty(bookmarkName))
            {
                BookmarkStart bookmarkstart = new BookmarkStart();
                bookmarkstart.Name = bookmarkName;
                bookmarkstart.Id = "0";
                paragraph.Append(bookmarkstart);
            }

            Run run = new Run();
            Text text = new Text(theText);
            run.Append(text);
            paragraph.Append(run);

            if (!string.IsNullOrEmpty(bookmarkName))
            {
                BookmarkEnd bookmarkend = new BookmarkEnd();
                bookmarkend.Id = "0";
                paragraph.Append(bookmarkend);
            }

            docbody.Append(paragraph);
        }

        #endregion For Test
    }
}
