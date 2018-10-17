// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core;
using Chem4Word.Core.Helpers;
using Chem4Word.Model.Converters;
using Microsoft.Office.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Chem4Word.Model.Converters.CML;
using Word = Microsoft.Office.Interop.Word;

namespace Chem4Word.Helpers
{
    public static class Upgrader
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        private static object _missing = Type.Missing;

        public static DialogResult UpgradeIsRequired(Word.Document doc)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            DialogResult result = DialogResult.Cancel;

            int count = LegacyChemistryCount(doc);

            if (count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"We have detected {count} legacy Chemistry objects in this document.");
                sb.AppendLine("Would you like them converted to the new format?");
                sb.AppendLine("");
                sb.AppendLine("  Click Yes to Upgrade then");
                sb.AppendLine("  Click No to leave them as they are");
                sb.AppendLine("");
                sb.AppendLine("This operation can't be undone.");
                result = UserInteractions.AskUserYesNo(sb.ToString());
                Globals.Chem4WordV3.Telemetry.Write(module, "Information", $"Detected {count} legacy chemistry ContentControl(s)");
            }

            return result;
        }

        private static string DecodeContentControlType(Word.WdContentControlType? contentControlType)
        {
            // Date from https://msdn.microsoft.com/en-us/library/microsoft.office.interop.word.wdcontentcontroltype(v=office.14).aspx
            string result = "";

            switch (contentControlType)
            {
                case Word.WdContentControlType.wdContentControlRichText:
                    result = "Rich-Text";
                    break;

                case Word.WdContentControlType.wdContentControlText:
                    result = "Text";
                    break;

                case Word.WdContentControlType.wdContentControlBuildingBlockGallery:
                    result = "Picture";
                    break;

                case Word.WdContentControlType.wdContentControlComboBox:
                    result = "ComboBox";
                    break;

                case Word.WdContentControlType.wdContentControlDropdownList:
                    result = "Drop-Down List";
                    break;

                case Word.WdContentControlType.wdContentControlPicture:
                    result = "Building Block Gallery";
                    break;

                case Word.WdContentControlType.wdContentControlDate:
                    result = "Date";
                    break;

                case Word.WdContentControlType.wdContentControlGroup:
                    result = "Group";
                    break;

                case Word.WdContentControlType.wdContentControlCheckBox:
                    result = "CheckBox";
                    break;

                case Word.WdContentControlType.wdContentControlRepeatingSection:
                    result = "Repeating Section";
                    break;

                default:
                    result = contentControlType.ToString();
                    break;
            }

            return result;
        }

        public static int LegacyChemistryCount(Word.Document doc)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            int count = 0;

            foreach (Word.ContentControl cc in doc.ContentControls)
            {
                Word.WdContentControlType? contentControlType = cc.Type;
                Debug.WriteLine($"{cc.ID} {cc.Range.Start} {DecodeContentControlType(contentControlType)} {cc.Tag}");
                try
                {
                    if (cc.Title != null && cc.Title.Equals(Constants.LegacyContentControlTitle))
                    {
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    Globals.Chem4WordV3.Telemetry.Write(module, "Exception", $"{ex.Message}");
                    Globals.Chem4WordV3.Telemetry.Write(module, "Exception", $"{ex.StackTrace}");
                }
            }

            return count;
        }

        public static void DoUpgrade(Word.Document doc)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            int sel = doc.Application.Selection.Range.Start;
            Globals.Chem4WordV3.DisableDocumentEvents(doc);

            try
            {
                string extension = doc.FullName.Split('.').Last();
                string guid = Guid.NewGuid().ToString("N");
                string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);
                string destination = Path.Combine(Globals.Chem4WordV3.AddInInfo.ProductAppDataPath, "Backups", $"Chem4Word-{timestamp}-{guid}.{extension}");
                File.Copy(doc.FullName, destination);
            }
            catch (Exception ex)
            {
                // Nothing much we can do here :-(
                Debug.WriteLine(ex.Message);
            }

            Dictionary<string, CustomXMLPart> customXmlParts = new Dictionary<string, CustomXMLPart>();
            List<UpgradeTarget> targets = CollectData(doc);
            int upgradedCCs = 0;
            int upgradedXml = 0;

            foreach (var target in targets)
            {
                if (target.ContentControls.Count > 0)
                {
                    upgradedXml++;
                    upgradedCCs += target.ContentControls.Count;
                }

                foreach (var cci in target.ContentControls)
                {
                    foreach (Word.ContentControl cc in doc.ContentControls)
                    {
                        if (cc.ID.Equals(cci.Id))
                        {
                            if (cci.Type.Equals("2D"))
                            {
                                cc.LockContents = false;
                                cc.Title = Constants.ContentControlTitle;
                                cc.Tag = target.Model.CustomXmlPartGuid;
                                cc.LockContents = true;

                                // ToDo: Regenerate converted 2D structures
                            }
                            else
                            {
                                cc.LockContents = false;
                                cc.Range.Delete();
                                int start = cc.Range.Start;
                                cc.Delete();

                                doc.Application.Selection.SetRange(start - 1, start - 1);
                                bool isFormula = false;
                                string source;
                                string text = ChemistryHelper.GetInlineText(target.Model, cci.Type, ref isFormula, out source);
                                Word.ContentControl ccn = doc.ContentControls.Add(Word.WdContentControlType.wdContentControlRichText, ref _missing);
                                ChemistryHelper.Insert1D(ccn, text, isFormula, $"{cci.Type}:{target.Model.CustomXmlPartGuid}");
                                ccn.LockContents = true;
                            }
                        }
                    }
                }

                CMLConverter converter = new CMLConverter();
                CustomXMLPart cxml = doc.CustomXMLParts.SelectByID(target.CxmlPartId);
                if (customXmlParts.ContainsKey(cxml.Id))
                {
                    customXmlParts.Add(cxml.Id, cxml);
                }
                doc.CustomXMLParts.Add(converter.Export(target.Model));
            }

            EraseChemistryZones(doc);

            foreach (var kvp in customXmlParts.ToList())
            {
                kvp.Value.Delete();
            }

            Globals.Chem4WordV3.EnableDocumentEvents(doc);
            doc.Application.Selection.SetRange(sel, sel);
            if (upgradedCCs + upgradedXml > 0)
            {
                Globals.Chem4WordV3.Telemetry.Write(module, "Information", $"Upgraded {upgradedCCs} Chemistry Objects for {upgradedXml} Structures");
                UserInteractions.AlertUser($"Upgrade Completed{Environment.NewLine}{Environment.NewLine}Upgraded {upgradedCCs} Chemistry Objects for {upgradedXml} Structures");
            }
        }

        private static void EraseChemistryZones(Word.Document doc)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            foreach (CustomXMLPart xmlPart in doc.CustomXMLParts)
            {
                string xml = xmlPart.XML;
                if (xml.Contains("<ChemistryZone"))
                {
                    xmlPart.Delete();
                }
            }
        }

        private static string GetDepictionValue(string cml, string xPath)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            string result = null;

            XDocument xDocument = XDocument.Parse(cml);

            if (xDocument.Root != null)
            {
                var objects = (IEnumerable)xDocument.Root.XPathEvaluate(xPath);
                var xObjects = objects.Cast<XObject>();
                XObject xObject = xObjects.FirstOrDefault();

                XElement xe = xObject as XElement;
                if (xe != null)
                {
                    if (!xe.HasElements)
                    {
                        result = xe.Value;
                    }
                }

                XAttribute xa = xObject as XAttribute;
                if (xa != null)
                {
                    result = xa.Value;
                }
            }

            return result;
        }

        private static List<UpgradeTarget> CollectData(Word.Document doc)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            List<UpgradeTarget> targets = new List<UpgradeTarget>();

            Word.Selection sel = doc.Application.Selection;

            // Step 1 find location of all content controls
            // Step 2 extract Cml
            // Step 3 find all Chemistry Zones

            List<ContentControlInfo> listOfContentControls = new List<ContentControlInfo>();

            for (int i = 1; i <= doc.ContentControls.Count; i++)
            {
                Word.ContentControl cc = doc.ContentControls[i];
                if (cc.Title != null && cc.Title.Equals(Constants.LegacyContentControlTitle))
                {
                    ContentControlInfo cci = new ContentControlInfo();
                    cci.Id = cc.ID;
                    cci.Index = i;
                    cci.Location = cc.Range.Start;
                    listOfContentControls.Add(cci);
                }
            }

            foreach (CustomXMLPart xmlPart in doc.CustomXMLParts)
            {
                string xml = xmlPart.XML;
                if (xml.Contains("<cml"))
                {
                    Debug.WriteLine($"Custom Xml Part: {xmlPart.Id} is CML");
                    UpgradeTarget ut = new UpgradeTarget();
                    ut.CxmlPartId = xmlPart.Id;
                    ut.Cml = xmlPart.XML;
                    CMLConverter converter = new CMLConverter();
                    ut.Model = converter.Import(ut.Cml);
                    ut.Model.CustomXmlPartGuid = Guid.NewGuid().ToString("N");
                    targets.Add(ut);

                    //Debug.WriteLine($"  {xmlPart.Id}");
                    //Debug.WriteLine(xml);
                }
            }

            foreach (CustomXMLPart xmlPart in doc.CustomXMLParts)
            {
                string xml = xmlPart.XML;

                if (xml.Contains("<ChemistryZone"))
                {
                    Debug.WriteLine($"Custom Xml Part: {xmlPart.Id} is ChemistryZone");
                    //Debug.WriteLine($"  {xmlPart.Id}");
                    //Debug.WriteLine(xml);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xml);

                    string ccValue = null;
                    string cmlValue = null;
                    string ddValue = null;

                    XmlNode refNode = xmlDoc.SelectSingleNode("//ref");
                    XmlNode ddNode = xmlDoc.SelectSingleNode("//DocumentDepictionOptionXPath");

                    if (ddNode != null)
                    {
                        ddValue = ddNode.Attributes["value"].Value;
                        Debug.WriteLine("  " + ddValue);
                    }

                    if (refNode != null)
                    {
                        ccValue = refNode.Attributes["cc"].Value;
                        cmlValue = refNode.Attributes["cml"].Value;
                        Debug.WriteLine($"  CC Id: {ccValue} CML Id: {cmlValue}");

                        foreach (UpgradeTarget target in targets)
                        {
                            if (target.CxmlPartId.Equals(cmlValue))
                            {
                                ContentControlInfo cci = new ContentControlInfo();
                                cci.Id = ccValue;

                                string dv = GetDepictionValue(target.Cml, ddValue);

                                if (dv == null)
                                {
                                    cci.Type = "2D";
                                }
                                else
                                {
                                    // Default to overall concise formula
                                    cci.Type = "c0";

                                    #region Find new style 1D code

                                    foreach (var molecule in target.Model.Molecules)
                                    {
                                        if (dv.Equals(molecule.ConciseFormula))
                                        {
                                            cci.Type = $"{molecule.Id}.f0";
                                        }
                                        foreach (var formula in molecule.Formulas)
                                        {
                                            if (formula.Inline.Equals(dv))
                                            {
                                                cci.Type = formula.Id;
                                                break;
                                            }
                                        }
                                        foreach (var name in molecule.ChemicalNames)
                                        {
                                            if (name.Name.Equals(dv))
                                            {
                                                cci.Type = name.Id;
                                                break;
                                            }
                                        }
                                    }

                                    #endregion Find new style 1D code
                                }

                                foreach (var ccii in listOfContentControls)
                                {
                                    if (ccii.Id.Equals(cci.Id))
                                    {
                                        cci.Location = ccii.Location;
                                        cci.Index = ccii.Index;
                                        break;
                                    }
                                }

                                target.ContentControls.Add(cci);
                                break;
                            }
                        }
                    }
                }
            }

            return targets;
        }
    }

    public class UpgradeTarget
    {
        public UpgradeTarget()
        {
            ContentControls = new List<ContentControlInfo>();
        }

        public string CxmlPartId { get; set; }
        public string Cml { get; set; }
        public Model.Model Model { get; set; }
        public List<ContentControlInfo> ContentControls { get; set; }
    }

    public class ContentControlInfo
    {
        public string Id { get; set; }
        public int Index { get; set; }
        public int Location { get; set; }
        public string Type { get; set; }
    }
}