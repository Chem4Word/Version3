// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Model.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace Chem4Word.Model.Converters
{
    // ReSharper disable once InconsistentNaming
    public class CMLConverter : IConverter
    {
        public string Description => "Chemical Markup Language";

        public string[] Extensions => new string[]
        {
            "*.CML",
            "*.XML"
        };

        #region Model->CML

        public string Export(Chem4Word.Model.Model model)
        {
            XDocument xd = new XDocument();
            XElement root = new XElement(CML.cml + "cml",
                new XAttribute(XNamespace.Xmlns + "conventions", CML.conventions),
                new XAttribute(XNamespace.Xmlns + "cml", CML.cml),
                new XAttribute(XNamespace.Xmlns + "cmlDict", CML.cmlDict),
                new XAttribute(XNamespace.Xmlns + "nameDict", CML.nameDict),
                new XAttribute(XNamespace.Xmlns + "c4w", CML.c4w),
                new XAttribute("conventions", "convention:molecular")
                );

            // Only export if set
            if (!string.IsNullOrEmpty(model.CustomXmlPartGuid))
            {
                XElement customXmlPartGuid = new XElement(CML.c4w + "customXmlPartGuid", model.CustomXmlPartGuid);
                root.Add(customXmlPartGuid);
            }

            bool relabelRequired = false;

            // Handle case where id's are null
            foreach (Molecule molecule in model.Molecules)
            {
                if (molecule.Id == null)
                {
                    relabelRequired = true;
                    break;
                }

                foreach (Atom atom in molecule.Atoms)
                {
                    if (atom.Id == null)
                    {
                        relabelRequired = true;
                        break;
                    }
                }

                foreach (Bond bond in molecule.Bonds)
                {
                    if (bond.Id == null)
                    {
                        relabelRequired = true;
                        break;
                    }
                }
            }

            if (relabelRequired)
            {
                model.Relabel(false);
            }

            foreach (Molecule molecule in model.Molecules)
            {
                root.Add(GetMoleculeElement(molecule));
            }
            xd.Add(root);

            return xd.ToString();
        }

        public bool CanExport => true;

        private static List<XElement> GetNames(XElement mol)
        {
            var names1 = from n1 in mol.Descendants("name")
                         select n1;
            var names2 = from n2 in mol.Descendants(CML.cml + "name")
                         select n2;
            return names1.Union(names2).ToList();
        }

        private static List<XElement> GetFormulas(XElement mol)
        {
            var formulae1 = from f1 in mol.Descendants("formula")
                            select f1;
            var formulae2 = from f2 in mol.Descendants(CML.cml + "formula")
                            select f2;
            return formulae1.Union(formulae2).ToList();
        }

        public XElement GetXElement(string concise, string molId)
        {
            XElement result = new XElement(CML.cml + "formula");

            if (concise != null)
            {
                result.Add(new XAttribute("id", $"{molId}.f0"));
                result.Add(new XAttribute("concise", concise));
            }

            return result;
        }

        public XElement GetXElement(Formula f, string concise)
        {
            XElement result = new XElement(CML.cml + "formula");

            if (f.Id != null)
            {
                result.Add(new XAttribute("id", f.Id));
            }

            if (f.Convention != null)
            {
                result.Add(new XAttribute("convention", f.Convention));
            }

            if (f.Inline != null)
            {
                result.Add(new XAttribute("inline", f.Inline));
            }

            if (concise != null)
            {
                result.Add(new XAttribute("concise", concise));
            }

            return result;
        }

        public XElement GetXElement(ChemicalName name)
        {
            XElement result = new XElement(CML.cml + "name", name.Name);

            if (name.Id != null)
            {
                result.Add(new XAttribute("id", name.Id));
            }

            if (name.DictRef != null)
            {
                result.Add(new XAttribute("dictRef", name.DictRef));
            }

            return result;
        }

        public XElement GetXElement(Atom atom)
        {
            XElement result = new XElement(CML.cml + "atom",
                new XAttribute("id", atom.Id),
                new XAttribute("elementType", atom.Element.ToString()),
                new XAttribute("x2", atom.Position.X),
                new XAttribute("y2", atom.Position.Y)
               );

            if (atom.FormalCharge != null)
            {
                result.Add(new XAttribute("formalCharge", atom.FormalCharge.Value));
            }
            if (atom.IsotopeNumber != null)
            {
                result.Add(new XAttribute("isotopeNumber", atom.IsotopeNumber));
            }
            return result;
        }

        private XElement GetStereoXElement(Bond bond)
        {
            XElement result = null;

            if (bond.Stereo != BondStereo.None)
            {
                if (bond.Stereo == BondStereo.Cis || bond.Stereo == BondStereo.Trans)
                {
                    Debugger.Break();
                    // ToDo: Fix 1st and last atomRefs
                    result = new XElement(CML.cml + "bondStereo",
                                new XAttribute("atomRefs4", $"{bond.StartAtom.Id} {bond.StartAtom.Id} {bond.EndAtom.Id} {bond.EndAtom.Id}"),
                                GetStereoString(bond.Stereo));
                }
                else
                {
                    result = new XElement(CML.cml + "bondStereo",
                                new XAttribute("atomRefs2", $"{bond.StartAtom.Id} {bond.EndAtom.Id}"),
                                GetStereoString(bond.Stereo));
                }
            }
            return result;
        }

        private string GetStereoString(BondStereo stereo)
        {
            switch (stereo)
            {
                case BondStereo.None:
                    return null;

                case BondStereo.Hatch:
                    return "H";

                case BondStereo.Wedge:
                    return "W";

                case BondStereo.Cis:
                    return "C";

                case BondStereo.Trans:
                    return "T";

                case BondStereo.Indeterminate:
                    return "S";

                default:
                    return null;
            }
        }

        public XElement GetXElement(Bond bond)
        {
            XElement result;

            result = new XElement(CML.cml + "bond",
                        new XAttribute("id", bond.Id),
                        new XAttribute("atomRefs2", $"{bond.StartAtom.Id} {bond.EndAtom.Id}"),
                        new XAttribute("order", bond.Order),
                        GetStereoXElement(bond));

            if (bond.ExplicitPlacement != null)
            {
                result.Add(new XAttribute(CML.c4w + "placement", bond.ExplicitPlacement));
            }
            return result;
        }

        public XElement GetMoleculeElement(Molecule mol)
        {
            XElement molElement = new XElement(CML.cml + "molecule", new XAttribute("id", mol.Id));

            if (!string.IsNullOrEmpty(mol.ConciseFormula))
            {
                molElement.Add(GetXElement(mol.ConciseFormula, mol.Id));
            }

            foreach (Formula formula in mol.Formulas)
            {
                molElement.Add(GetXElement(formula, mol.ConciseFormula));
            }

            foreach (ChemicalName chemicalName in mol.ChemicalNames)
            {
                molElement.Add(GetXElement(chemicalName));
            }

            foreach (Atom atom in mol.Atoms)
            {
                molElement.Add(GetXElement(atom));
            }

            foreach (Bond bond in mol.Bonds)
            {
                molElement.Add(GetXElement(bond));
            }
            return molElement;
        }

        #endregion Model->CML

        #region CML->model

        public bool CanImport => true;

        /// <summary>
        /// Loads the CML data into the in-memory model
        /// </summary>
        /// <param name="data">Object variable containing well-formed and compliant CML</param>
        /// <returns></returns>
        public Model Import(object data)
        {
            Model newModel = new Model();

            if (data != null)
            {
                XDocument modelDoc = XDocument.Parse((string)data);
                var root = modelDoc.Root;

                // Only import if not null
                var customXmlPartGuid = CML.GetCustomXmlPartGuid(root);
                if (customXmlPartGuid != null && !string.IsNullOrEmpty(customXmlPartGuid.Value))
                {
                    newModel.CustomXmlPartGuid = customXmlPartGuid.Value;
                }

                var moleculeElements = CML.GetMolecules(root);

                foreach (XElement meElement in moleculeElements)
                {
                    newModel.Molecules.Add(CreateMolecule(meElement));
                }

                // Can't use RebuildMolecules() as it trashes the formulae and labels
                //newModel.RebuildMolecules();
                newModel.RefreshMolecules();
                foreach (Molecule molecule in newModel.Molecules)
                {
                    molecule.RebuildRings();
                    // Ensure ConciseFormula has been calculated
                    if (string.IsNullOrEmpty(molecule.ConciseFormula))
                    {
                        molecule.ConciseFormula = molecule.CalculatedFormula();
                    }
                }
                newModel.Relabel(true);
            }

            return newModel;
        }

        /// <summary>
        /// Creates a molecule from a CML representation
        /// </summary>
        /// <param name="cmlElement">CML containing the molecule representation</param>
        /// <returns>Molecule object</returns>
        private Molecule CreateMolecule(XElement cmlElement)
        {
            var m = new Molecule();

            m.Id = cmlElement.Attribute("id")?.Value;

            var childMolecules = CML.GetMolecules(cmlElement);

            var atomElements = CML.GetAtoms(cmlElement);
            var bondElements = CML.GetBonds(cmlElement);
            var nameElements = GetNames(cmlElement);
            var formulaElements = GetFormulas(cmlElement);

            Dictionary<string, Atom> newAtoms = new Dictionary<string, Atom>();

            foreach (XElement childElement in childMolecules)
            {
                var newMol = CreateMolecule(childElement);
                m.Molecules.Add(newMol);
            }

            foreach (XElement atomElement in atomElements)
            {
                List<string> messages = new List<string>();
                var newAtom = CreateAtom(atomElement, out messages);
                if (messages.Count > 0)
                {
                    m.Errors.AddRange(messages);
                }

                if (newAtom != null)
                {
                    newAtoms[newAtom.Id] = newAtom; //store the reference to help us build bonds
                    m.Atoms.Add(newAtom);
                }
            }

            foreach (XElement bondElement in bondElements)
            {
                string message = "";
                var newBond = CreateBond(bondElement, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    m.Errors.Add(message);
                }

                string[] atomRefs = bondElement.Attribute("atomRefs2")?.Value.Split(' ');
                if (atomRefs?.Length == 2)
                {
                    Atom startAtom = null;
                    if (newAtoms.ContainsKey(atomRefs[0]))
                    {
                        startAtom = newAtoms[atomRefs[0]];
                    }
                    else
                    {
                        m.Errors.Add($"Can't find atom '{atomRefs[0]}' of {bondElement}");
                    }

                    Atom endAtom = null;
                    if (newAtoms.ContainsKey(atomRefs[1]))
                    {
                        endAtom = newAtoms[atomRefs[1]];
                    }
                    else
                    {
                        m.Errors.Add($"Can't find atom '{atomRefs[1]}' of {bondElement}");
                    }

                    if (startAtom != null && endAtom != null)
                    {
                        newBond.StartAtom = startAtom;
                        newBond.EndAtom = endAtom;
                        m.Bonds.Add(newBond);
                    }
                }
            }

            foreach (XElement nameElement in nameElements)
            {
                var newName = XmlToName(nameElement);
                m.ChemicalNames.Add(newName);
            }

            foreach (XElement formulaElement in formulaElements)
            {
                // Only import Concise Once
                if (string.IsNullOrEmpty(m.ConciseFormula))
                {
                    m.ConciseFormula = XmlToConsiseFormula(formulaElement);
                }

                var formula = XmlToFormula(formulaElement);
                if (formula.IsValid)
                {
                    m.Formulas.Add(formula);
                }
            }

            return m;
        }

        private ChemicalName XmlToName(XElement cmlElement)
        {
            // Example: <name id="m1.n1" dictRef="pubchem:cid">241</name>

            ChemicalName n = new ChemicalName();

            n.Id = cmlElement.Attribute("id")?.Value;

            if (cmlElement.Attribute("dictRef") == null)
            {
                n.DictRef = "chem4word:Synonym";
            }
            else
            {
                n.DictRef = cmlElement.Attribute("dictRef")?.Value;
            }

            // Correct import from legacy Add-In
            if (string.IsNullOrEmpty(n.DictRef) || n.DictRef.Equals("nameDict:unknown"))
            {
                n.DictRef = "chem4word:Synonym";
            }

            n.Name = cmlElement.Value;

            return n;
        }

        private string XmlToConsiseFormula(XElement cmlElement)
        {
            // Example
            // <formula id="m1.f0" concise="C 4 H 9 N 1 O 2" />
            string result = null;

            // No need to import id as this is hard coded to $"{Molecule.Id}.f0" on export
            result = cmlElement.Attribute("concise")?.Value;

            return result;
        }

        private Formula XmlToFormula(XElement cmlElement)
        {
            // Example
            // <formula id="m1.f1" convention="pubchem:formula" inline="C4H9NO2" concise="C 4 H 9 N 1 O 2" />
            // <formula id="m1.f2" convention="pubchem:CanonicalSmiles" inline="C(CC(=O)O)CN" concise="C 4 H 9 N 1 O 2" />

            Formula f = new Formula();

            if (cmlElement.Attribute("id") != null)
            {
                f.Id = cmlElement.Attribute("id")?.Value;
            }

            if (cmlElement.Attribute("convention") == null)
            {
                f.Convention = "chem4word:Formula";
            }
            else
            {
                f.Convention = cmlElement.Attribute("convention")?.Value;
            }

            // Correct import from legacy Add-In
            if (string.IsNullOrEmpty(f.Convention))
            {
                f.Convention = "chem4word:Formula";
            }
            if (cmlElement.Attribute("inline") != null)
            {
                f.Inline = cmlElement.Attribute("inline")?.Value;
                f.IsValid = true;
            }

            return f;
        }

        /// <summary>
        /// Creates an atom from its CML
        /// </summary>
        /// <param name="cmlElement"></param>
        public Atom CreateAtom(XElement cmlElement, out List<string> messages)
        {
            Atom atom = null;

            messages = new List<string>();
            string message = "";

            string atomLabel = cmlElement.Attribute("id")?.Value;
            ElementBase e = GetChemicalElement(cmlElement, out message);
            if (!string.IsNullOrEmpty(message))
            {
                messages.Add(message);
            }

            if (e != null)
            {
                Point p = GetPosn(cmlElement, out message);
                if (!string.IsNullOrEmpty(message))
                {
                    messages.Add(message);
                }

                atom = new Atom
                {
                    Id = atomLabel,
                    Position = p,
                    Element = e,
                    FormalCharge = GetFormalCharge(cmlElement),
                    IsotopeNumber = GetIsotopeNumber(cmlElement)
                };
            }

            return atom;
        }

        private int? GetFormalCharge(XElement cmlElement)
        {
            int formalCharge;

            if (int.TryParse(cmlElement.Attribute("formalCharge")?.Value, out formalCharge))
            {
                return formalCharge;
            }
            else
            {
                return null;
            }
        }

        private int? GetIsotopeNumber(XElement cmlElement)
        {
            int isotopeNumber;

            if (int.TryParse(cmlElement.Attribute("isotopeNumber")?.Value, out isotopeNumber))
            {
                return isotopeNumber;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// gets the chemical element from the cmlElement CML
        /// </summary>
        /// <param name="cmlElement">XElement containing CML</param>
        /// <returns></returns>
        private ElementBase GetChemicalElement(XElement cmlElement, out string message)
        {
            message = "";
            XAttribute xa = cmlElement.Attribute("elementType");
            if (xa != null)
            {
                string symbol = xa.Value;

                //try to return a chemical element from the Periodic Table
                if (Globals.PeriodicTable.HasElement(symbol))
                {
                    return Globals.PeriodicTable.Elements[symbol];
                }

                //if that fails, see if it's a functional group
                // ReSharper disable once InconsistentNaming
                FunctionalGroup newFG;
                if (FunctionalGroups.TryParse(symbol, out newFG))
                {
                    return newFG;
                }

                //if we got here then it went very wrong
                message = $"Unrecognised element '{symbol}' in {cmlElement}";
            }
            else
            {
                message = $"cml attribute 'elementType' missing from {cmlElement}";
            }

            return null;
        }

        /// <summary>
        /// Gets the cmlElement position from the CML
        /// </summary>
        /// <param name="cmlElement">XElement representing the cmlElement CML</param>
        /// <returns>Point containing the cmlElement coordinates</returns>
        private static Point GetPosn(XElement cmlElement, out string message)
        {
            message = "";
            string symbol = cmlElement.Attribute("elementType")?.Value;
            string id = cmlElement.Attribute("id")?.Value;

            Point result = new Point();
            bool found = false;

            // Try first with 2D Co-ordinate scheme
            if (cmlElement.Attribute("x2") != null && cmlElement.Attribute("y2") != null)
            {
                result = new Point(Double.Parse(cmlElement.Attribute("x2").Value, CultureInfo.InvariantCulture),
                    Double.Parse(cmlElement.Attribute("y2").Value, CultureInfo.InvariantCulture));
                found = true;
            }

            if (!found)
            {
                // Try again with 3D Co-ordinate scheme
                if (cmlElement.Attribute("x3") != null && cmlElement.Attribute("y3") != null)
                {
                    result = new Point(Double.Parse(cmlElement.Attribute("x3").Value, CultureInfo.InvariantCulture),
                        Double.Parse(cmlElement.Attribute("y3").Value, CultureInfo.InvariantCulture));
                    found = true;
                }
            }

            if (!found)
            {
                message = $"No atom co-ordinates found for atom '{symbol}' with id of '{id}'.";
            }
            return result;
        }

        /// <summary>
        /// Creates a bond from the CML
        /// </summary>
        /// <param name="cmlElement"></param>
        /// <returns></returns>
        public Bond CreateBond(XElement cmlElement, out string message)
        {
            message = "";
            Bond newBond = new Bond();
            BondDirection? dir = null;

            var dirAttr = cmlElement.Attribute(CML.c4w + "placement");
            if (dirAttr != null)
            {
                BondDirection temp;

                if (Enum.TryParse(dirAttr.Value, out temp))
                {
                    dir = temp;
                }
            }

            newBond.Id = cmlElement.Attribute("id")?.Value;
            newBond.Order = cmlElement.Attribute("order")?.Value;

            if (dir != null)
            {
                newBond.Placement = dir.Value;
            }

            var stereoElems = CML.GetStereo(cmlElement);
            if (stereoElems.Any())
            {
                var stereo = stereoElems[0].Value;

                switch (stereo)
                {
                    case "N":
                        newBond.Stereo = BondStereo.None;
                        break;

                    case "W":
                        newBond.Stereo = BondStereo.Wedge;
                        break;

                    case "H":
                        newBond.Stereo = BondStereo.Hatch;
                        break;

                    case "S":
                        newBond.Stereo = BondStereo.Indeterminate;
                        break;

                    case "C":
                        newBond.Stereo = BondStereo.Cis;
                        break;

                    case "T":
                        newBond.Stereo = BondStereo.Trans;
                        break;

                    default:
                        newBond.Stereo = BondStereo.None;
                        break;
                }
            }

            return newBond;
        }
    }

    #endregion CML->model
}