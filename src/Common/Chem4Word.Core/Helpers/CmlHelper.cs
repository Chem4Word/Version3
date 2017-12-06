// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Chem4Word.Helpers
{
    public class CmlHelper
    {
        private const string AtomRefs = "atomRefs";
        private const string AtomRefs2 = "atomRefs2";
        private const string AtomRefs4 = "atomRefs4";
        private const string BondRefs = "bondRefs";
        private const string Chirality = "chirality";

        private const string Concise = "concise";
        private const string Convention = "convention";

        private const string Count = "count";
        private const string DateType = "dataType";
        private const string DictRef = "dictRef";
        private const string ElementType = "elementType";

        private const string FormalCharge = "formalCharge";
        private const string Formula = "formula";

        private const string Id = "id";
        private const string Inline = "inline";
        private const string IsotopeNumber = "isotopeNumber";
        private const string Max = "max";
        private const string Min = "min";
        private const string Namespace = "namespace";
        private const string Order = "order";
        private const string PeakMultiplicity = "peakMultiplicity";
        private const string PeakShape = "peakShape";
        private const string Ref = "ref";

        private const string SpinMultiplicity = "spinMultiplicity";
        private const string Term = "term";
        private const string Title = "title";
        private const string Units = "units";

        private const string Value = "value";
        private const string Version = "version";
        private const string X2 = "x2";
        private const string X3 = "x3";
        private const string XMax = "xMax";
        private const string XMin = "xMin";
        private const string XUnits = "xUnits";
        private const string XValue = "xValue";
        private const string Y2 = "y2";
        private const string Y3 = "y3";
        private const string YUnits = "yUnits";
        private const string YValue = "yValue";
        private const string Z3 = "y3";

        private const string NamespacePrefix = "cml";
        private const string NamespaceUri = "http://www.xml-cml.org/schema";

        private XmlDocument _xmlDocument;
        private XmlNamespaceManager _xmlNamespaceManager;

        public int AtomCounter { get; set; }
        public int BondCounter { get; set; }

        public int MoleculeCount
        {
            get
            {
                int result = 0;
                if (_xmlDocument != null)
                {
                    result = GetMolecules().Count();
                }
                return result;
            }
        }

        public CmlHelper()
        {
            _xmlDocument = new XmlDocument();
            _xmlNamespaceManager = new XmlNamespaceManager(_xmlDocument.NameTable);
            _xmlNamespaceManager.AddNamespace(NamespacePrefix, NamespaceUri);
        }

        public CmlHelper(string jsonOrCmlString)
        {
            AtomCounter = 0;
            BondCounter = 0;

            string trimmed = jsonOrCmlString.Trim();

            if (trimmed.StartsWith("<") && trimmed.EndsWith(">"))
            {
                ParseCml(trimmed);
            }
            else if (trimmed.StartsWith("{") && trimmed.EndsWith("}"))
            {
                ParseJson(trimmed);
            }
        }

        public CmlHelper(string jsonOrCmlString, int atomCounter, int bondCounter)
        {
            AtomCounter = atomCounter;
            BondCounter = bondCounter;

            string trimmed = jsonOrCmlString.Trim();

            if (trimmed.StartsWith("<") && trimmed.EndsWith(">"))
            {
                ParseCml(trimmed);
            }
            else if (trimmed.StartsWith("{") && trimmed.EndsWith("}"))
            {
                ParseJson(trimmed);
            }
        }

        public void LoadFromMoleculeCml(string moleculeCml)
        {
            _xmlDocument = CreateCmlDocument(moleculeCml, true);
            _xmlNamespaceManager = new XmlNamespaceManager(_xmlDocument.NameTable);
            _xmlNamespaceManager.AddNamespace(NamespacePrefix, NamespaceUri);
        }

        public string ToJson()
        {
            string result = "";
            List<XmlNode> molecules = GetMolecules();

            if (molecules.Count == 1)
            {
                result = ToJsonSingle();
            }
            else
            {
                result = ToJsonMulti();
            }

            return result;
        }

        public void SetMoleculeId(string guid)
        {
            XmlElement element = _xmlDocument.SelectSingleNode("//cml:molecule", _xmlNamespaceManager) as XmlElement;
            // Shold never be null !!
            if (element != null)
            {
                XmlAttribute id = element.Attributes["id"];
                if (id == null)
                {
                    id = _xmlDocument.CreateAttribute("id");
                    id.Value = guid;
                    element.Attributes.InsertBefore(id, element.Attributes[0]);
                }
                else
                {
                    element.SetAttribute("id", guid);
                }
            }
            else
            {
                element = _xmlDocument.SelectSingleNode("//molecule", _xmlNamespaceManager) as XmlElement;
                if (element != null)
                {
                    XmlAttribute id = element.Attributes["id"];
                    if (id == null)
                    {
                        id = _xmlDocument.CreateAttribute("id");
                        id.Value = guid;
                        element.Attributes.InsertBefore(id, element.Attributes[0]);
                    }
                    else
                    {
                        element.SetAttribute("id", guid);
                    }
                }
            }
        }

        public void SetCmlId(string guid)
        {
            XmlElement element = _xmlDocument.SelectSingleNode("//cml:cml", _xmlNamespaceManager) as XmlElement;
            // Shold never be null !!
            if (element != null)
            {
                XmlAttribute id = element.Attributes["id"];
                if (id == null)
                {
                    id = _xmlDocument.CreateAttribute("id");
                    id.Value = guid;
                    element.Attributes.InsertBefore(id, element.Attributes[0]);
                }
                else
                {
                    element.SetAttribute("id", guid);
                }
            }
            else
            {
                element = _xmlDocument.SelectSingleNode("//cml", _xmlNamespaceManager) as XmlElement;
                if (element != null)
                {
                    XmlAttribute id = element.Attributes["id"];
                    if (id == null)
                    {
                        id = _xmlDocument.CreateAttribute("id");
                        id.Value = guid;
                        element.Attributes.InsertBefore(id, element.Attributes[0]);
                    }
                    else
                    {
                        element.SetAttribute("id", guid);
                    }
                }
            }
        }

        public XmlNode GetXmlNode(string xpath)
        {
            XmlNode node = null;

            if (_xmlDocument != null)
            {
                node = _xmlDocument.SelectSingleNode(xpath, _xmlNamespaceManager);
            }

            return node;
        }

        public XElement GetXElement(string xpath)
        {
            XElement result = null;

            if (_xmlDocument != null)
            {
                XmlNode node = _xmlDocument.SelectSingleNode(xpath, _xmlNamespaceManager);
                if (node != null)
                {
                    // xpath returned something
                    result = XElement.Parse(node.OuterXml);
                }
                else
                {
                    // Try again without namespace - Should never get here !!!
                    node = _xmlDocument.SelectSingleNode(xpath.Replace("//" + NamespacePrefix + ":", "//"));
                    if (node != null)
                    {
                        result = XElement.Parse(node.OuterXml);
                    }
                }
            }

            return result;
        }

        public string Minify()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = false,
                NewLineHandling = NewLineHandling.Replace,
                Encoding = Encoding.UTF8
            };

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                _xmlDocument.Save(writer);
            }

            return sb.ToString().Replace("utf-16", "utf-8").Replace("xmlns=\"\"", "");
        }

        public string Beautify()
        {
            StringBuilder sb = new StringBuilder();

            if (_xmlDocument != null)
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace,
                    Encoding = Encoding.UTF8
                };

                using (XmlWriter writer = XmlWriter.Create(sb, settings))
                {
                    _xmlDocument.Save(writer);
                }
            }

            return sb.ToString().Replace("utf-16", "utf-8").Replace("xmlns=\"\"", "");
        }

        public XmlNode GetMolecule()
        {
            XmlNode node = _xmlDocument.SelectSingleNode("//cml:molecule", _xmlNamespaceManager);
            if (node == null)
            {
                node = _xmlDocument.SelectSingleNode("//molecule");
            }

            return node;
        }

        public List<XmlNode> GetMolecules()
        {
            List<XmlNode> nodes = new List<XmlNode>();

            nodes.AddRange(GetXmlNodes("//cml:molecule").Cast<XmlNode>().ToList());
            nodes.AddRange(GetXmlNodes("//molecule").Cast<XmlNode>().ToList());

            return nodes;
        }

        public List<XmlNode> GetAtoms()
        {
            List<XmlNode> nodes = new List<XmlNode>();

            nodes.AddRange(GetXmlNodes("//cml:atom").Cast<XmlNode>().ToList());
            nodes.AddRange(GetXmlNodes("//atom").Cast<XmlNode>().ToList());

            return nodes;
        }

        public List<XmlNode> GetBonds()
        {
            List<XmlNode> nodes = new List<XmlNode>();

            nodes.AddRange(GetXmlNodes("//cml:bond").Cast<XmlNode>().ToList());
            nodes.AddRange(GetXmlNodes("//bond").Cast<XmlNode>().ToList());

            return nodes;
        }

        public void RemoveFormulae()
        {
            XmlNode molecule = _xmlDocument.SelectSingleNode("//cml:molecule", _xmlNamespaceManager);

            // Remove formulae nodes
            XmlNodeList oldFormulae = _xmlDocument.SelectNodes("//cml:formula", _xmlNamespaceManager);
            foreach (XmlNode node in oldFormulae)
            {
                molecule.RemoveChild(node);
            }
            oldFormulae = _xmlDocument.SelectNodes("//formula", _xmlNamespaceManager);
            foreach (XmlNode node in oldFormulae)
            {
                molecule.RemoveChild(node);
            }
        }

        public void RemoveNames()
        {
            XmlNode molecule = _xmlDocument.SelectSingleNode("//cml:molecule", _xmlNamespaceManager);

            // Remove old names nodes
            XmlNodeList oldNames = _xmlDocument.SelectNodes("//cml:name", _xmlNamespaceManager);
            foreach (XmlNode node in oldNames)
            {
                molecule.RemoveChild(node);
            }
            oldNames = _xmlDocument.SelectNodes("//name", _xmlNamespaceManager);
            foreach (XmlNode node in oldNames)
            {
                molecule.RemoveChild(node);
            }
        }

        public void AddConciseFormula(string conciseFormula)
        {
            XmlNode molecule = _xmlDocument.SelectSingleNode("//cml:molecule", _xmlNamespaceManager);

            // Add new node containing just the concise formula
            XmlElement newElement;
            if (molecule.Name.Contains(":"))
            {
                newElement = _xmlDocument.CreateElement("cml", "formula", "http://www.xml-cml.org/schema");
            }
            else
            {
                newElement = _xmlDocument.CreateElement("formula");
            }
            newElement.SetAttribute(Concise, conciseFormula);
            molecule.AppendChild(newElement);
        }

        public void AddFormula(XmlNode newNode)
        {
            XmlNode molecule = _xmlDocument.SelectSingleNode("//cml:molecule", _xmlNamespaceManager);

            XmlElement newElement;
            string nodeName = newNode.Name;
            int idx = nodeName.IndexOf(":");
            if (idx >= 0)
            {
                nodeName = nodeName.Substring(idx + 1);
            }

            if (molecule.Name.Contains(":"))
            {
                newElement = _xmlDocument.CreateElement("cml", nodeName, "http://www.xml-cml.org/schema");
            }
            else
            {
                newElement = _xmlDocument.CreateElement(nodeName);
            }

            newElement.InnerText = newNode.InnerText;
            foreach (XmlAttribute attribute in newNode.Attributes)
            {
                if (!string.IsNullOrEmpty(attribute.Value)
                    && !attribute.Name.Equals(Concise))
                {
                    newElement.SetAttribute(attribute.Name, attribute.Value);
                }
            }

            if (newElement.Attributes.Count > 0)
            {
                molecule.AppendChild(newElement);
            }
        }

        public void AddName(XmlNode newNode)
        {
            XmlNode molecule = _xmlDocument.SelectSingleNode("//cml:molecule", _xmlNamespaceManager);

            XmlElement newElement;
            string nodeName = newNode.Name;
            int idx = nodeName.IndexOf(":");
            if (idx >= 0)
            {
                nodeName = nodeName.Substring(idx + 1);
            }

            if (molecule.Name.Contains(":"))
            {
                newElement = _xmlDocument.CreateElement("cml", nodeName, "http://www.xml-cml.org/schema");
            }
            else
            {
                newElement = _xmlDocument.CreateElement(nodeName);
            }

            newElement.InnerText = newNode.InnerText;
            foreach (XmlAttribute attribute in newNode.Attributes)
            {
                newElement.SetAttribute(attribute.Name, attribute.Value);
            }
            molecule.AppendChild(newElement);
        }

        public void SaveNames(List<LabelEditorItem> items)
        {
            foreach (LabelEditorItem item in items)
            {
                XmlElement node = NewXmlNode("name") as XmlElement;
                node.SetAttribute(Id, item.Id);
                node.SetAttribute(DictRef, item.Attribute);
                node.InnerText = item.Value;
                AddName(node);
            }
        }

        public List<LabelEditorItem> GetFormulae(out int maxId, out string conciseFormula)
        {
            // Variations of formula we need to cope with ...
            // <formula concise="C 5 H 10 O 2"/>
            // <cml:formula inline="AcOMe" concise="C 2 H 6 O 2" />
            // <formula convention="pubchem:formula" inline="C4H9NO2" concise="C 4 H 9 N 1 O 2" />
            // <formula convention="pubchem:CanonicalSmiles" inline="C(CC(=O)O)CN" concise="C 4 H 9 N 1 O 2" />
            // <formula convention="pubchem:IsomericSmiles" inline="C(CC(=O)O)CN" concise="C 4 H 9 N 1 O 2" />

            // Initialise output parameters
            maxId = 0;
            conciseFormula = "";

            List<XmlNode> list = GetFormulae();
            List<LabelEditorItem> items = new List<LabelEditorItem>();

            foreach (XmlNode node in list)
            {
                string concise = GetAttributeValue(node, Concise);
                // Only grab 1st concise formula
                if (string.IsNullOrEmpty(conciseFormula)
                    && !string.IsNullOrEmpty(concise))
                {
                    conciseFormula = concise;
                }

                // Count attributes which are not concise formula and are not empty
                int count = 0;
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    if (!attribute.Name.Equals(Concise)
                        && !string.IsNullOrEmpty(attribute.Value))
                    {
                        count++;
                    }
                }

                if (count > 0)
                {
                    LabelEditorItem editorItem = new LabelEditorItem();

                    string id = GetAttributeValue(node, Id);
                    if (!string.IsNullOrEmpty(id))
                    {
                        editorItem.Id = id;
                        int n;
                        if (int.TryParse(id.Substring(1), out n))
                        {
                            maxId = Math.Max(maxId, n);
                        }
                    }

                    string inline = GetAttributeValue(node, Inline);
                    if (!string.IsNullOrEmpty(inline))
                    {
                        editorItem.Value = inline;
                    }

                    string convention = GetAttributeValue(node, Convention);
                    if (!string.IsNullOrEmpty(convention))
                    {
                        editorItem.Attribute = convention;
                    }

                    items.Add(editorItem);
                }
            }

            foreach (LabelEditorItem item in items)
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    maxId++;
                    item.Id = "f" + maxId;
                }
            }

            return items;
        }

        public List<LabelEditorItem> GetNames(out int maxId)
        {
            // <cml:name dictRef="nameDict:generic">ester</cml:name>

            // Initialise output parameters
            maxId = 0;

            List<XmlNode> list = GetNames();
            List<LabelEditorItem> items = new List<LabelEditorItem>();

            foreach (XmlNode node in list)
            {
                LabelEditorItem editorItem = new LabelEditorItem();

                string id = GetAttributeValue(node, Id);
                if (!string.IsNullOrEmpty(id))
                {
                    editorItem.Id = id;
                    int n;
                    if (int.TryParse(id.Substring(1), out n))
                    {
                        maxId = Math.Max(maxId, n);
                    }
                }

                string dictRefValue = GetAttributeValue(node, DictRef);
                editorItem.Attribute = dictRefValue;
                editorItem.Value = node.InnerText;

                items.Add(editorItem);
            }

            foreach (LabelEditorItem item in items)
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    maxId++;
                    item.Id = "n" + maxId;
                }
            }

            return items;
        }

        public XmlNode NewXmlNode(string nodeName)
        {
            XmlElement newElement = _xmlDocument.CreateElement("cml", nodeName, NamespaceUri);
            return newElement;
        }

        public void SaveFormulae(string concise, List<LabelEditorItem> items)
        {
            // Add back concise formula
            AddConciseFormula(concise);

            // Now the rest
            foreach (LabelEditorItem item in items)
            {
                XmlElement node = NewXmlNode("formula") as XmlElement;
                node.SetAttribute(Id, item.Id);
                node.SetAttribute(Convention, item.Attribute);
                node.SetAttribute(Inline, item.Value);
                AddFormula(node);
            }
        }

        private void ParseJson(string input)
        {
            List<CmlHelper> molecules = new List<CmlHelper>();

            JToken molJson = JObject.Parse(input);
            JToken mols = molJson.SelectToken("m");
            if (mols == null)
            {
                ParseJsonSingle(input);
            }
            else
            {
                ParseJsonMulti(input);
            }
        }

        private void ParseJsonMulti(string input)
        {
            List<CmlHelper> molecules = new List<CmlHelper>();

            JToken molJson = JObject.Parse(input);

            JToken mols = molJson.SelectToken("m");
            int atomNumber = 0;
            int bondNumber = 0;
            foreach (JToken mol in mols)
            {
                CmlHelper molHelper = new CmlHelper(mol.ToString(), atomNumber, bondNumber);
                atomNumber = molHelper.AtomCounter;
                bondNumber = molHelper.BondCounter;
                molecules.Add(molHelper);
            }

            _xmlDocument = CreateCmlDocument(molecules[0].GetMolecule().OuterXml, true);
            _xmlNamespaceManager = new XmlNamespaceManager(_xmlDocument.NameTable);
            _xmlNamespaceManager.AddNamespace(NamespacePrefix, NamespaceUri);

            int moleculeCounter = 0;
            SetMoleculeId("m" + moleculeCounter);
            moleculeCounter++;

            XmlNode cmlNode = _xmlDocument.SelectSingleNode("//cml:cml", _xmlNamespaceManager);
            for (int i = 1; i < molecules.Count; i++)
            {
                molecules[i].SetMoleculeId("m" + moleculeCounter);
                moleculeCounter++;

                XmlNode mol = molecules[i].GetMolecule();
                XmlDocumentFragment fragment = _xmlDocument.CreateDocumentFragment();
                fragment.InnerXml = mol.OuterXml;
                cmlNode.AppendChild(fragment);
            }
        }

        private void ParseJsonSingle(string input)
        {
            List<string> atomIds = new List<string>();

            StringBuilder sb = new StringBuilder();
            sb.Append("<cml:molecule id='m0'>");
            sb.Append("  <cml:atomArray>");
            sb.Append("  </cml:atomArray>");
            sb.Append("  <cml:bondArray>");
            sb.Append("  </cml:bondArray>");
            sb.Append("</cml:molecule>");

            _xmlDocument = CreateCmlDocument(sb.ToString(), true);
            _xmlNamespaceManager = new XmlNamespaceManager(_xmlDocument.NameTable);
            _xmlNamespaceManager.AddNamespace(NamespacePrefix, NamespaceUri);

            XmlElement cmlAtoms = _xmlDocument.SelectSingleNode("//cml:atomArray", _xmlNamespaceManager) as XmlElement;
            XmlElement cmlBonds = _xmlDocument.SelectSingleNode("//cml:bondArray", _xmlNamespaceManager) as XmlElement;

            JToken molJson = JObject.Parse(input);

            JToken atoms = molJson.SelectToken("a");
            //Debug.WriteLine("Found " + atoms.Count<JToken>() + " atoms");
            foreach (JToken atom in atoms)
            {
                // Ignore incoming atom id
                //string atomid = (string)atom.SelectToken("i");
                string label = (string)atom.SelectToken("l");
                double x = (double)atom.SelectToken("x");
                double y = (double)atom.SelectToken("y");
                string charge = (string)atom.SelectToken("c");

                XmlElement cmlAtom = _xmlDocument.CreateElement("cml:atom", NamespaceUri);
                cmlAtom.SetAttribute("id", "a" + AtomCounter);
                atomIds.Add("a" + AtomCounter);
                if (label == null)
                {
                    label = "C";
                }
                cmlAtom.SetAttribute("elementType", label);
                cmlAtom.SetAttribute("x2", x.ToString(CultureInfo.InvariantCulture));
                cmlAtom.SetAttribute("y2", y.ToString(CultureInfo.InvariantCulture));
                if (charge != null)
                {
                    cmlAtom.SetAttribute("formalCharge", charge);
                }
                cmlAtoms.AppendChild(cmlAtom);
                AtomCounter++;
            }

            JToken bonds = molJson.SelectToken("b");
            if (bonds != null)
            {
                //Debug.WriteLine("Found " + bonds.Count<JToken>() + " bonds");
                foreach (JToken bond in bonds)
                {
                    // Ignore incoming bond id
                    //string bondid = (string)bond.SelectToken("i");
                    int begin = (int)bond.SelectToken("b");
                    int end = (int)bond.SelectToken("e");
                    string order = (string)bond.SelectToken("o");
                    string stereo = (string)bond.SelectToken("s");

                    XmlElement cmlBond = _xmlDocument.CreateElement("cml:bond", NamespaceUri);
                    cmlBond.SetAttribute("id", "b" + BondCounter);
                    cmlBond.SetAttribute("atomRefs2", atomIds[begin] + " " + atomIds[end]);

                    if (order != null)
                    {
                        cmlBond.SetAttribute("order", OrderToString(order));
                    }
                    else
                    {
                        cmlBond.SetAttribute("order", "S");
                    }

                    if (stereo != null)
                    {
                        XmlElement cmlStereo = _xmlDocument.CreateElement("cml:bondStereo", NamespaceUri);
                        cmlStereo.InnerText = JsonStereoToCml(stereo);
                        cmlBond.AppendChild(cmlStereo);
                    }

                    cmlBonds.AppendChild(cmlBond);
                    BondCounter++;
                }
            }
        }

        private string CmlStereoToJson(string stereoIn)
        {
            string result = stereoIn;
            switch (stereoIn)
            {
                case "W":
                    result = "protruding";
                    break;

                case "H":
                    result = "recessed";
                    break;

                case "S":
                    result = "ambiguous";
                    break;

                default:
                    break;
            }
            return result;
        }

        private string JsonStereoToCml(string stereoIn)
        {
            string result = stereoIn;
            switch (stereoIn)
            {
                case "protruding":
                    result = "W";
                    break;

                case "recessed":
                    result = "H";
                    break;

                case "ambiguous":
                    result = "S";
                    break;

                default:
                    break;
            }
            return result;
        }

        private string OrderToString(string orderIn)
        {
            string result = orderIn;
            switch (orderIn)
            {
                case "1":
                case "1.0":
                    result = "S";
                    break;

                case "1.5":
                    result = "A";
                    break;

                case "2":
                case "2.0":
                    result = "D";
                    break;

                case "3":
                case "3.0":
                    result = "T";
                    break;

                default:
                    break;
            }
            return result;
        }

        private void ParseCml(string cmlStringIn)
        {
            _xmlDocument = new XmlDocument();
            _xmlDocument.LoadXml(cmlStringIn);

            _xmlNamespaceManager = new XmlNamespaceManager(_xmlDocument.NameTable);
            _xmlNamespaceManager.AddNamespace(NamespacePrefix, NamespaceUri);

            XmlNode node = _xmlDocument.SelectSingleNode("//cml:cml", _xmlNamespaceManager);
            if (node == null)
            {
                XmlNode molNode = _xmlDocument.SelectSingleNode("//cml");
                if (molNode != null)
                {
                    _xmlDocument = CreateCmlDocument(molNode.OuterXml, false);
                    _xmlNamespaceManager = new XmlNamespaceManager(_xmlDocument.NameTable);
                    _xmlNamespaceManager.AddNamespace(NamespacePrefix, NamespaceUri);
                }
                else
                {
                    _xmlDocument = null;
                }
            }

            // Fix any missing bond ids
            if (_xmlDocument != null)
            {
                XmlNodeList bonds = _xmlDocument.SelectNodes("//cml:bond", _xmlNamespaceManager);
                if (bonds == null)
                {
                    bonds = _xmlDocument.SelectNodes("//bond", _xmlNamespaceManager);
                }
                int bc = 0;
                foreach (XmlNode bond in bonds)
                {
                    XmlAttribute id = bond.Attributes["id"];
                    if (id == null)
                    {
                        id = _xmlDocument.CreateAttribute("id");
                        id.Value = $"c4w{bc++}";
                        bond.Attributes.Append(id);
                    }
                }
            }
        }

        private string ToJsonMulti()
        {
            string result = "";

            List<XmlNode> molecules = GetMolecules();

            JObject jsonOut = new JObject();
            JArray jMoleculesArray = new JArray();
            JProperty jMoleculesProperty = new JProperty("m", jMoleculesArray);
            jsonOut.Add(jMoleculesProperty);

            foreach (XmlNode molecule in molecules)
            {
                CmlHelper helper = new CmlHelper();
                string molXml = molecule.OuterXml;
                helper.LoadFromMoleculeCml(molXml);
                JObject m = JObject.Parse(helper.ToJsonSingle());
                jMoleculesArray.Add(m);
            }

            result = jsonOut.ToString();

            return result;
        }

        private string ToJsonSingle()
        {
            // Need to store atomIds in dictionary as id of 1st atom not always a0
            Dictionary<String, int> atomIds = new Dictionary<string, int>();
            JObject jsonOut = new JObject();

            List<XmlNode> atoms = GetAtoms();
            if (atoms.Count > 0)
            {
                JArray jAtomsArray = new JArray();
                JProperty jAtomsProperty = new JProperty("a", jAtomsArray);
                jsonOut.Add(jAtomsProperty);

                int atomCount = 0;
                foreach (XmlNode atom in atoms)
                {
                    // Fetch or generate atom id
                    string atomId = GetAttribute(atom, "id");
                    if (string.IsNullOrEmpty(atomId))
                    {
                        atomId = "a" + atomCount;
                    }
                    atomIds.Add(atomId, atomCount);
                    atomCount++;

                    string atomLabel = GetAttribute(atom, "elementType");
                    // 2D co-ordinates (if present)
                    string x2 = GetAttribute(atom, "x2");
                    string y2 = GetAttribute(atom, "y2");
                    // 3D co-ordinates (if present)
                    string x3 = GetAttribute(atom, "x3");
                    string y3 = GetAttribute(atom, "y3");
                    string z3 = GetAttribute(atom, "z3");

                    JObject jAtom = new JObject();
                    if (string.IsNullOrEmpty(z3))
                    {
                        // 2D co-ordinates supplied
                        jAtom.Add(new JProperty("x", SafeDouble.Parse(x2)));
                        jAtom.Add(new JProperty("y", SafeDouble.Parse(y2)));
                    }
                    else
                    {
                        // 3D co-ordinates supplied
                        jAtom.Add(new JProperty("x", SafeDouble.Parse(x3)));
                        jAtom.Add(new JProperty("y", SafeDouble.Parse(y3)));
                        jAtom.Add(new JProperty("z", SafeDouble.Parse(z3)));
                    }

                    jAtom.Add(new JProperty("i", atomId));

                    if (!atomLabel.Equals("C"))
                    {
                        jAtom.Add(new JProperty("l", atomLabel));
                    }

                    string charge = GetAttribute(atom, "formalCharge");
                    if (!string.IsNullOrEmpty(charge))
                    {
                        jAtom.Add(new JProperty("c", Int32.Parse(charge)));
                    }

                    jAtomsArray.Add(jAtom);
                }
            }

            List<XmlNode> bonds = GetBonds();
            if (bonds.Count > 0)
            {
                JArray jBondsArray = new JArray();
                JProperty jBondsProperty = new JProperty("b", jBondsArray);
                jsonOut.Add(jBondsProperty);

                int bondCount = 0;
                foreach (XmlNode bond in bonds)
                {
                    // Fetch or generate bond id
                    string bondId = GetAttribute(bond, "id");
                    if (string.IsNullOrEmpty(bondId))
                    {
                        bondId = "b" + bondCount;
                    }

                    string atomRefs2 = GetAttribute(bond, "atomRefs2");
                    string[] bondStartEnd = atomRefs2.Split(' ');
                    string bondOrder = GetAttribute(bond, "order");
                    string bondStereo = "";

                    if (bond.ChildNodes.Count > 0)
                    {
                        XmlNode stereoNode = bond.FirstChild;
                        bondStereo = stereoNode.InnerText;
                    }

                    JObject jBond = new JObject();

                    jBond.Add(new JProperty("i", bondId));

                    int atomId0 = atomIds[bondStartEnd[0]];
                    int atomId1 = atomIds[bondStartEnd[1]];
                    jBond.Add(new JProperty("b", atomId0));
                    jBond.Add(new JProperty("e", atomId1));

                    if (!string.IsNullOrEmpty(bondOrder))
                    {
                        Single order = OrderToNumber(bondOrder);
                        if (order != 1)
                        {
                            jBond.Add(new JProperty("o", order));
                        }
                    }

                    if (!string.IsNullOrEmpty(bondStereo))
                    {
                        jBond.Add(new JProperty("s", CmlStereoToJson(bondStereo)));
                    }

                    jBondsArray.Add(jBond);
                    bondCount++;
                }
            }

            return jsonOut.ToString();
        }

        private Single OrderToNumber(string p_order)
        {
            string temp = "0";

            switch (p_order)
            {
                case "1":
                case "1.0":
                case "S":
                    temp = "1";
                    break;

                case "1.5":
                case "A":
                    temp = "1.5";
                    break;

                case "2":
                case "2.0":
                case "D":
                    temp = "2";
                    break;

                case "3":
                case "3.0":
                case "T":
                    temp = "3";
                    break;

                default:
                    break;
            }
            return Single.Parse(temp);
        }

        private string GetAttribute(XmlNode node, string attributeName)
        {
            string result = "";
            try
            {
                for (int i = 0; i < node.Attributes.Count; i++)
                {
                    if (node.Attributes[i].Name.Equals(attributeName))
                    {
                        result = node.Attributes[attributeName].InnerText;
                        break;
                    }
                }
            }
            catch { }
            return result;
        }

        private XmlDocument CreateCmlDocument(string molXml, bool withPrefix)
        {
            StringBuilder sb = new StringBuilder();

            if (withPrefix)
            {
                sb.Append("<?xml version='1.0' encoding='utf-8'?>");
                sb.Append("<cml:cml");
                sb.Append(" convention='conventions:molecular'");
                sb.Append(" xmlns:cml='" + NamespaceUri + "'");
                sb.Append(" xmlns:conventions='http://www.xml-cml.org/convention/'");
                sb.Append(" xmlns:cmlDict='http://www.xml-cml.org/dictionary/cml/'");
                sb.Append(" xmlns:nameDict='http://www.xml-cml.org/dictionary/cml/name/'");
                sb.Append(">");
                sb.Append(molXml);
                sb.Append("</cml:cml>");
            }
            else
            {
                sb.Append("<?xml version='1.0' encoding='utf-8'?>");
                sb.Append("<cml");
                sb.Append(" convention='conventions:molecular'");
                sb.Append(" xmlns='" + NamespaceUri + "'");
                sb.Append(" xmlns:conventions='http://www.xml-cml.org/convention/'");
                sb.Append(" xmlns:cmlDict='http://www.xml-cml.org/dictionary/cml/'");
                sb.Append(" xmlns:nameDict='http://www.xml-cml.org/dictionary/cml/name/'");
                sb.Append(">");
                sb.Append(molXml);
                sb.Append("</cml>");
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(sb.ToString());

            return xmlDocument;
        }

        private List<XmlNode> GetFormulae()
        {
            List<XmlNode> nodes = new List<XmlNode>();

            nodes.AddRange(GetXmlNodes("//cml:formula").Cast<XmlNode>().ToList());
            nodes.AddRange(GetXmlNodes("//formula").Cast<XmlNode>().ToList());

            return nodes;
        }

        private List<XmlNode> GetNames()
        {
            List<XmlNode> nodes = new List<XmlNode>();

            nodes.AddRange(GetXmlNodes("//cml:name").Cast<XmlNode>().ToList());
            nodes.AddRange(GetXmlNodes("//name").Cast<XmlNode>().ToList());

            return nodes;
        }

        private string GetAttributeValue(XmlNode node, string attribute)
        {
            string result = "";

            if (node.Attributes[attribute] != null)
            {
                result = node.Attributes[attribute].Value;
            }
            return result;
        }

        private XmlNodeList GetXmlNodes(string xPath)
        {
            return _xmlDocument.SelectNodes(xPath, _xmlNamespaceManager);
        }
    }
}
