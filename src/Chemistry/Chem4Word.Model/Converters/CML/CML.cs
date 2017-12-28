// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Chem4Word.Model.Converters
{
    /// <summary>
    /// Loads up the CML into XDocument and XElement objects
    /// </summary>
    public static class CML
    {
        // ReSharper disable once InconsistentNaming
        public static XNamespace cml = "http://www.xml-cml.org/schema";

        // ReSharper disable once InconsistentNaming
        public static XNamespace cmlDict = "http://www.xml-cml.org/dictionary/cml/";

        // ReSharper disable once InconsistentNaming
        public static XNamespace nameDict = "http://www.xml-cml.org/dictionary/cml/name/";

        // ReSharper disable once InconsistentNaming
        public static XNamespace conventions = "http://www.xml-cml.org/convention/";

        // ReSharper disable once InconsistentNaming
        public static XNamespace c4w = "http://www.chem4word.com/cml";

        static CML()
        {
        }

        // ReSharper disable once InconsistentNaming
        public static XDocument LoadCML(string cml)
        {
            return XDocument.Parse(cml);
        }

        public static XElement GetCustomXmlPartGuid(XElement doc)
        {
            var id1 = from XElement xe in doc.Elements("customXmlPartGuid") select xe;
            var id2 = from XElement xe in doc.Elements(c4w + "customXmlPartGuid") select xe;
            return id1.Union(id2).FirstOrDefault();
        }

        // ReSharper disable once InconsistentNaming
        public static List<XElement> GetMolecules(XElement doc)
        {
            var mols = from XElement xe in doc.Elements("molecule")
                       select xe;
            var mols2 = from XElement xe2 in doc.Elements(cml + "molecule")
                        select xe2;
            return mols.Union(mols2).ToList();
        }

        public static List<XElement> GetAtoms(XElement mol)
        {
            var atom = from a in mol.Descendants("atom")
                       select a;
            var atom2 = from a2 in mol.Descendants(cml + "atom")
                        select a2;
            return atom.Union(atom2).ToList();
        }

        public static List<XElement> GetBonds(XElement mol)
        {
            var bonds = from b in mol.Descendants("bond")
                        select b;
            var bonds2 = from b2 in mol.Descendants(cml + "bond")
                         select b2;
            return bonds.Union(bonds2).ToList();
        }

        public static List<XElement> GetStereo(XElement bond)
        {
            var stereo = from s in bond.Elements("bondStereo")
                         select s;
            var stereo2 = from s in bond.Elements(cml + "bondStereo")
                          select s;

            return stereo.Union(stereo2).ToList();
        }
    }
}