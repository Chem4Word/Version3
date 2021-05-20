// ---------------------------------------------------------------------------
//  Copyright (c) 2021, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using Chem4Word.Core.Helpers;
using Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;

namespace Chem4Word.Helpers
{
    public static class CustomXmlPartHelper
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        private static CustomXMLParts AllChemistryParts(Word.Document document)
            => document.CustomXMLParts.SelectByNamespace("http://www.xml-cml.org/schema");

        public static int ChemistryXmlParts(Word.Document doc)
            => AllChemistryParts(doc).Count;

        public static CustomXMLPart FindCustomXmlPart(string id, Word.Document document)
        {
            CustomXMLPart result = null;

            Word.Document activeDocument = document;
            string activeDocumentName = activeDocument.Name;

            foreach (Word.Document otherDocument in activeDocument.Application.Documents)
            {
                if (!otherDocument.Name.Equals(activeDocumentName))
                {
                    foreach (
                        CustomXMLPart customXmlPart in AllChemistryParts(otherDocument))
                    {
                        string molId = GetCmlId(customXmlPart);
                        if (molId.Equals(id))
                        {
                            result = customXmlPart;
                            break;
                        }
                    }
                }
                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public static string GuidFromTag(string tag)
        {
            string guid = string.Empty;

            if (!string.IsNullOrEmpty(tag))
            {
                guid = tag.Contains(":") ? tag.Split(':')[1] : tag;
            }

            return guid;
        }

        public static CustomXMLPart GetCustomXmlPart(string id, Word.Document activeDocument)
        {
            CustomXMLPart result = null;

            string guid = GuidFromTag(id);

            if (!string.IsNullOrEmpty(guid))
            {
                Word.Document doc = activeDocument;

                foreach (CustomXMLPart xmlPart in AllChemistryParts(doc))
                {
                    string cmlId = GetCmlId(xmlPart);
                    if (!string.IsNullOrEmpty(cmlId))
                    {
                        if (cmlId.Equals(guid))
                        {
                            result = xmlPart;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public static string GetCmlId(CustomXMLPart xmlPart)
        {
            string result = string.Empty;

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xmlPart.XML);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("cml", "http://www.xml-cml.org/schema");
            nsmgr.AddNamespace("c4w", "http://www.chem4word.com/cml");

            XmlNode node = xdoc.SelectSingleNode("//c4w:customXmlPartGuid", nsmgr);
            if (node != null)
            {
                result = node.InnerText;
            }

            return result;
        }

        public static void RemoveOrphanedXmlParts(Word.Document doc)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            Dictionary<string, int> referencedXmlParts = new Dictionary<string, int>();

            foreach (Word.ContentControl cc in doc.ContentControls)
            {
                if (cc.Title != null && cc.Title.Equals(Constants.ContentControlTitle))
                {
                    string guid = GuidFromTag(cc?.Tag);

                    if (!string.IsNullOrEmpty(guid))
                    {
                        if (!referencedXmlParts.ContainsKey(guid))
                        {
                            referencedXmlParts.Add(guid, 1);
                        }
                    }
                }
            }

            string backupFolder = Path.Combine(Globals.Chem4WordV3.AddInInfo.ProductAppDataPath, "Backups");
            string header = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine;

            foreach (CustomXMLPart customXmlPart in AllChemistryParts(doc))
            {
                string molId = GetCmlId(customXmlPart);
                if (!referencedXmlParts.ContainsKey(molId))
                {
                    try
                    {
                        string guid = Guid.NewGuid().ToString("N");
                        string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);

                        string fileName = Path.Combine(backupFolder, $"Chem4Word-Orphaned-Structure-{timestamp}-{guid}.cml");
                        File.WriteAllText(fileName, header + customXmlPart.XML);

                        customXmlPart.Delete();
                    }
                    catch (Exception exception)
                    {
                        RegistryHelper.StoreException(module, exception);
                    }
                }
            }
        }
    }
}