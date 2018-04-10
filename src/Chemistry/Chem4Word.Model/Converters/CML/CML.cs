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
            var mols = from XElement xe in doc.Elements("molecule") select xe;
            var mols2 = from XElement xe2 in doc.Elements(cml + "molecule") select xe2;
            return mols.Union(mols2).ToList();
        }

        public static List<XElement> GetAtoms(XElement mol)
        {
            // Task 336
            var aa1 = from a in mol.Elements("atomArray") select a;
            var aa2 = from a in mol.Elements(cml + "atomArray") select a;
            var aa = aa1.Union(aa2);

            if (aa.Count() == 0)
            {
                var atoms1 = from a in mol.Elements("atom") select a;
                var atoms2 = from a in mol.Elements(cml + "atom") select a;
                return atoms1.Union(atoms2).ToList();
            }
            else
            {
                var atoms1 = from a in aa.Elements("atom") select a;
                var atoms2 = from a in aa.Elements(cml + "atom") select a;
                return atoms1.Union(atoms2).ToList();
            }
        }

        public static List<XElement> GetBonds(XElement mol)
        {
            // Task 336
            var ba1 = from b in mol.Elements("bondArray") select b;
            var ba2 = from b in mol.Elements(cml + "bondArray") select b;
            var ba = ba1.Union(ba2);

            if (ba.Count() == 0)
            {
                var bonds1 = from b in mol.Elements("bond") select b;
                var bonds2 = from b in mol.Elements(cml + "bond") select b;
                return bonds1.Union(bonds2).ToList();
            }
            else
            {
                var bonds1 = from b in ba.Elements("bond") select b;
                var bonds2 = from b in ba.Elements(cml + "bond") select b;
                return bonds1.Union(bonds2).ToList();
            }
        }

        public static List<XElement> GetStereo(XElement bond)
        {
            var stereo = from s in bond.Elements("bondStereo") select s;
            var stereo2 = from s in bond.Elements(cml + "bondStereo") select s;
            return stereo.Union(stereo2).ToList();
        }

        public static List<XElement> GetNames(XElement mol)
        {
            var names1 = from n1 in mol.Descendants("name") select n1;
            var names2 = from n2 in mol.Descendants(cml + "name") select n2;
            return names1.Union(names2).ToList();
        }

        public static List<XElement> GetFormulas(XElement mol)
        {
            var formulae1 = from f1 in mol.Descendants("formula") select f1;
            var formulae2 = from f2 in mol.Descendants(cml + "formula") select f2;
            return formulae1.Union(formulae2).ToList();
        }
    }
}