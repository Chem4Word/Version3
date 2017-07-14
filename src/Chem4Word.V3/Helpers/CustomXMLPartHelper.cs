using Microsoft.Office.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Word = Microsoft.Office.Interop.Word;

namespace Chem4Word.Helpers
{
    public static class CustomXmlPartHelper
    {
        public static CustomXMLPart FindCustomXmlPart(string id, Word.Document document)
        {
            CustomXMLPart result = null;

            Word.Document activeDocument = document;
            string activeDocumentName = activeDocument.Name;

            foreach (Word.Document otherDocument in activeDocument.Application.Documents)
            {
                if (!otherDocument.Name.Equals(activeDocumentName))
                {
                    //Debug.WriteLine("Looking in " + otherDoc.Name);
                    foreach (
                        CustomXMLPart x in
                            otherDocument.CustomXMLParts.SelectByNamespace("http://www.xml-cml.org/schema"))
                    {
                        string molId = GetCmlId(x);
                        if (molId.Equals(id))
                        {
                            result = x;
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
            string guid = tag;

            if (tag.Contains(":"))
            {
                guid = tag.Split(':')[1];
            }

            return guid;
        }

        public static CustomXMLPart GetCustomXmlPart(string id, Word.Document activeDocument)
        {
            string guid = GuidFromTag(id);

            CustomXMLPart result = null;

            Word.Document doc = activeDocument;

            foreach (CustomXMLPart xmlPart in doc.CustomXMLParts.SelectByNamespace("http://www.xml-cml.org/schema"))
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

            return result;
        }

        public static string GetCmlId(CustomXMLPart xmlPart)
        {
            return GetCmlId(xmlPart.XML);
        }

        public static string GetCmlId(string cml)
        {
            string result = null;

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(cml);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("cml", "http://www.xml-cml.org/schema");
            nsmgr.AddNamespace("c4w", "http://www.chem4word.com/cml");

            XmlNodeList nodes = xdoc.SelectNodes("//cml:cml", nsmgr);
            if (nodes != null && nodes.Count > 0)
            {
                //Debug.WriteLine("Found " + molecules.Count + " cml");
                try
                {
                    XmlAttribute att = nodes[0].Attributes["id"];
                    result = att.Value;
                }
                catch (Exception)
                {
                    // Do Nothing
                }
            }
            else
            {
                // Handle case when cml namespace fails
                nodes = xdoc.SelectNodes("//cml");
                if (nodes != null && nodes.Count > 0)
                {
                    Debug.WriteLine("Found " + nodes.Count + " cml");
                    try
                    {
                        XmlAttribute att = nodes[0].Attributes["id"];
                        result = att.Value;
                    }
                    catch (Exception)
                    {
                        // Do Nothing
                    }
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                XmlNode node = xdoc.SelectSingleNode("//c4w:customXmlPartGuid", nsmgr);
                if (node != null)
                {
                    Debug.WriteLine(node.InnerText);
                    result = node.InnerText;
                }
            }

            return result;
        }

        public static void RemoveOrphanedXmlParts(Word.Document doc)
        {
            Diagnostics(doc, "Before RemoveOrphanedXmlParts()");

            Dictionary<string, int> referencedXmlParts = new Dictionary<string, int>();

            foreach (Word.ContentControl cc in doc.ContentControls)
            {
                if (cc.Title != null && cc.Title.Equals(Constants.ContentControlTitle))
                {
                    string guid = GuidFromTag(cc.Tag);

                    if (!referencedXmlParts.ContainsKey(guid))
                    {
                        referencedXmlParts.Add(guid, 1);
                    }
                }
            }

            foreach (CustomXMLPart x in doc.CustomXMLParts.SelectByNamespace("http://www.xml-cml.org/schema"))
            {
                string molId = GetCmlId(x);
                if (!referencedXmlParts.ContainsKey(molId))
                {
                    x.Delete();
                }
            }

            Diagnostics(doc, "After RemoveOrphanedXmlParts()");
        }

        public static void Diagnostics(Word.Document doc, string when)
        {
            Debug.WriteLine("----- Diagnostics " + when + " -----");

            int counter = 0;

            // List all of our ContentControl(s)
            foreach (Word.ContentControl ccc in doc.ContentControls)
            {
                if (ccc.Title.Equals(Constants.ContentControlTitle))
                {
                    counter++;
                    Debug.WriteLine("ContentControl: " + ccc.ID);
                    Debug.WriteLine("   Range.Start: " + ccc.Range.Start);
                    Debug.WriteLine("           Tag: " + ccc.Tag);
                }
            }

            Debug.WriteLine("  Found " + counter + " ContentControl(s)");

            // List all of our CustomXMLPart(s)
            counter = 0;
            foreach (CustomXMLPart x in doc.CustomXMLParts.SelectByNamespace("http://www.xml-cml.org/schema"))
            {
                counter++;
                Debug.WriteLine("CustomXMLPart: " + x.Id);
                string molId = CustomXmlPartHelper.GetCmlId(x);
                Debug.WriteLine("   MoleculeId: " + molId);
            }

            Debug.WriteLine("  Found " + counter + " CustomXMLPart(s)");
        }
    }
}