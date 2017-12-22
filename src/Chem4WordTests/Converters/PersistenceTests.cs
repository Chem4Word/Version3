// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4WordTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Chem4Word.Model.Converters.Tests
{
    [TestClass()]
    public class PersistenceTests
    {
        [TestMethod()]
        public void CmlImportBenzene()
        {
            CMLConverter mc = new CMLConverter();
            Model m = mc.Import(ResourceHelper.GetStringResource("Benzene.xml"));

            // Basic sanity checks
            Assert.IsTrue(m.Molecules.Count == 1, $"Expected 1 Molecule; Got {m.Molecules.Count}");
            Assert.IsTrue(m.AllAtoms.Count == 6, $"Expected 6 Atoms; Got {m.AllAtoms.Count}");
            Assert.IsTrue(m.AllBonds.Count == 6, $"Expected 6 Bonds; Got {m.AllBonds.Count}");

            // Check that names and formulae have not been trashed
            Assert.IsTrue(m.Molecules[0].ChemicalNames.Count == 3, $"Expected 3 Chemical Names; Got {m.Molecules[0].ChemicalNames.Count}");
            Assert.IsTrue(m.Molecules[0].Formulas.Count == 2, $"Expected 2 Formulae; Got {m.Molecules[0].Formulas.Count }");

            // Check that we have one ring
            Assert.IsTrue(m.Molecules.SelectMany(m1 => m1.Rings).Count() == 1, $"Expected 1 Ring; Got {m.Molecules.SelectMany(m1 => m1.Rings).Count()}");
        }

        [TestMethod()]
        public void CmlImportTestosterone()
        {
            CMLConverter mc = new CMLConverter();
            Model m = mc.Import(ResourceHelper.GetStringResource("Testosterone.xml"));

            // Basic Sanity Checks
            Assert.IsTrue(m.Molecules.Count == 1, $"Expected 1 Molecule; Got {m.Molecules.Count}");
            Assert.IsTrue(m.AllAtoms.Count == 25, $"Expected 25 Atoms; Got {m.AllAtoms.Count}");
            Assert.IsTrue(m.AllBonds.Count == 28, $"Expected 28 Bonds; Got {m.AllBonds.Count}");

            // Check that names and formulae have not been trashed
            Assert.IsTrue(m.Molecules[0].ChemicalNames.Count == 4, $"Expected 4 Chemical Names; Got {m.Molecules[0].ChemicalNames.Count}");
            Assert.IsTrue(m.Molecules[0].Formulas.Count == 2, $"Expected 2 Formulae; Got {m.Molecules[0].Formulas.Count }");

            Assert.IsTrue(m.Molecules[0].Rings.Count == 4, $"Expected 4 Rings; Got {m.Molecules[0].Rings.Count}");
            var list = m.Molecules[0].SortRingsForDBPlacement();
            Assert.IsTrue(list.Count == 4, $"Expected 4 Rings; Got {list.Count}");
        }

        [TestMethod()]
        public void CmlImportTestosteroneThenRefresh()
        {
            CMLConverter mc = new CMLConverter();
            Model m = mc.Import(ResourceHelper.GetStringResource("Testosterone.xml"));

            m.RefreshMolecules();

            // Basic Sanity Checks
            Assert.IsTrue(m.Molecules.Count == 1, $"Expected 1 Molecule; Got {m.Molecules.Count}");
            Assert.IsTrue(m.AllAtoms.Count == 25, $"Expected 25 Atoms; Got {m.AllAtoms.Count}");
            Assert.IsTrue(m.AllBonds.Count == 28, $"Expected 28 Bonds; Got {m.AllBonds.Count}");

            // Check that names and formulae have not been trashed
            Assert.IsTrue(m.Molecules[0].ChemicalNames.Count == 4, $"Expected 4 Chemical Names; Got {m.Molecules[0].ChemicalNames.Count}");
            Assert.IsTrue(m.Molecules[0].Formulas.Count == 2, $"Expected 2 Formulae; Got {m.Molecules[0].Formulas.Count }");

            Assert.IsTrue(m.Molecules[0].Rings.Count == 4, $"Expected 4 Rings; Got {m.Molecules[0].Rings.Count}");
            var list = m.Molecules[0].SortRingsForDBPlacement();
            Assert.IsTrue(list.Count == 4, $"Expected 4 Rings; Got {list.Count}");
        }

        [TestMethod()]
        public void CmlImportCopperPhthalocyanine()
        {
            CMLConverter mc = new CMLConverter();
            Model m = mc.Import(ResourceHelper.GetStringResource("CopperPhthalocyanine.xml"));

            // Basic Sanity Checks
            Assert.IsTrue(m.Molecules.Count == 1, $"Expected 1 Molecule; Got {m.Molecules.Count}");
            Assert.IsTrue(m.AllAtoms.Count == 57, $"Expected 57 Atoms; Got {m.AllAtoms.Count}");
            Assert.IsTrue(m.AllBonds.Count == 68, $"Expected 68 Bonds; Got {m.AllBonds.Count}");

            Assert.IsTrue(m.Molecules[0].Rings.Count == 12, $"Expected 12 Rings; Got {m.Molecules[0].Rings.Count}");
            var list = m.Molecules[0].SortRingsForDBPlacement();
            Assert.IsTrue(list.Count == 12, $"Expected 12 Rings; Got {list.Count}");
        }

        [TestMethod()]
        public void CmlImportCopperPhthalocyanineThenRefresh()
        {
            CMLConverter mc = new CMLConverter();
            Model m = mc.Import(ResourceHelper.GetStringResource("CopperPhthalocyanine.xml"));

            m.RefreshMolecules();

            // Basic Sanity Checks
            Assert.IsTrue(m.Molecules.Count == 1, $"Expected 1 Molecule; Got {m.Molecules.Count}");
            Assert.IsTrue(m.AllAtoms.Count == 57, $"Expected 57 Atoms; Got {m.AllAtoms.Count}");
            Assert.IsTrue(m.AllBonds.Count == 68, $"Expected 68 Bonds; Got {m.AllBonds.Count}");

            Assert.IsTrue(m.Molecules[0].Rings.Count == 12, $"Expected 12 Rings; Got {m.Molecules[0].Rings.Count}");
            var list = m.Molecules[0].SortRingsForDBPlacement();
            Assert.IsTrue(list.Count == 12, $"Expected 12 Rings; Got {list.Count}");
        }

        [TestMethod()]
        public void CmlImportPhthalocyanine()
        {
            CMLConverter mc = new CMLConverter();
            Model m = mc.Import(ResourceHelper.GetStringResource("Phthalocyanine.xml"));

            // Basic Sanity Checks
            Assert.IsTrue(m.Molecules.Count == 1, $"Expected 1 Molecule; Got {m.Molecules.Count}");
            Assert.IsTrue(m.AllAtoms.Count == 58, $"Expected 58 Atoms; Got {m.AllAtoms.Count}");
            Assert.IsTrue(m.AllBonds.Count == 66, $"Expected 66 Bonds; Got {m.AllBonds.Count}");

            Assert.IsTrue(m.Molecules[0].Rings.Count == 9, $"Expected 9 Rings; Got {m.Molecules[0].Rings.Count}");
            var list = m.Molecules[0].SortRingsForDBPlacement();
            Assert.IsTrue(list.Count == 8, $"Expected 8 Rings; Got {list.Count}");
        }

        [TestMethod()]
        public void CmlImportPhthalocyanineThenRefresh()
        {
            CMLConverter mc = new CMLConverter();
            Model m = mc.Import(ResourceHelper.GetStringResource("Phthalocyanine.xml"));

            m.RefreshMolecules();

            // Basic Sanity Checks
            Assert.IsTrue(m.Molecules.Count == 1, $"Expected 1 Molecule; Got {m.Molecules.Count}");
            Assert.IsTrue(m.AllAtoms.Count == 58, $"Expected 58 Atoms; Got {m.AllAtoms.Count}");
            Assert.IsTrue(m.AllBonds.Count == 66, $"Expected 66 Bonds; Got {m.AllBonds.Count}");

            Assert.IsTrue(m.Molecules[0].Rings.Count == 9, $"Expected 9 Rings; Got {m.Molecules[0].Rings.Count}");
            var list = m.Molecules[0].SortRingsForDBPlacement();
            Assert.IsTrue(list.Count == 8, $"Expected 8 Rings; Got {list.Count}");
        }

        [TestMethod()]
        public void SdfImportBenzene()
        {
            SdFileConverter mc = new SdFileConverter();
            Model m = mc.Import(ResourceHelper.GetStringResource("Benzene.txt"));

            // Basic sanity checks
            Assert.IsTrue(m.Molecules.Count == 1, $"Expected 1 Molecule; Got {m.Molecules.Count}");
            Assert.IsTrue(m.AllAtoms.Count == 6, $"Expected 6 Atoms; Got {m.AllAtoms.Count}");
            Assert.IsTrue(m.AllBonds.Count == 6, $"Expected 6 Bonds; Got {m.AllBonds.Count}");

            // Check that names and formulae have not been trashed
            Assert.IsTrue(m.Molecules[0].ChemicalNames.Count == 2, $"Expected 2 Chemical Names; Got {m.Molecules[0].ChemicalNames.Count}");
            Assert.IsTrue(m.Molecules[0].Formulas.Count == 2, $"Expected 2 Formulae; Got {m.Molecules[0].Formulas.Count }");

            // Check that we have one ring
            Assert.IsTrue(m.Molecules.SelectMany(m1 => m1.Rings).Count() == 1, $"Expected 1 Ring; Got {m.Molecules.SelectMany(m1 => m1.Rings).Count()}");
        }

        [TestMethod()]
        public void SdfImportBasicParafuchsin()
        {
            SdFileConverter mc = new SdFileConverter();
            Model m = mc.Import(ResourceHelper.GetStringResource("BasicParafuchsin.txt"));

            // Basic sanity checks
            Assert.IsTrue(m.Molecules.Count == 2, $"Expected 2 Molecules; Got {m.Molecules.Count}");
            Assert.IsTrue(m.AllAtoms.Count == 41, $"Expected 41 Atoms; Got {m.AllAtoms.Count}");
            Assert.IsTrue(m.AllBonds.Count == 42, $"Expected 42 Bonds; Got {m.AllBonds.Count}");

            // Check that we got two rings
            Assert.IsTrue(m.Molecules.SelectMany(m1 => m1.Rings).Count() == 3, $"Expected 2 Rings; Got {m.Molecules.SelectMany(m1 => m1.Rings).Count()}");

            string molstring = mc.Export(m);
            mc = new SdFileConverter();
            Model m2 = mc.Import(molstring);
        }
    }
}