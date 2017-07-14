using Chem4WordTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Chem4Word.Model.Converters.Tests
{
    [TestClass()]
    public class PersistenceTests
    {
        [TestMethod()]
        public void CMLImportTest()
        {
            CMLConverter mc = new CMLConverter();
            Model m = mc.Import(ChemistryValues.TESTOSTERONE);

            var list = m.Molecules[0].SortRingsForDBPlacement();
            Assert.IsTrue(m.Molecules[0].Rings.Count == 4, $"Expected 4 Rings; Got {m.Molecules[0].Rings.Count}");
            Assert.IsTrue(list.Count == 4, $"Expected 4 Rings; Got {list.Count}");

            // Check that names and formulae are not trashed
            Assert.IsTrue(m.Molecules[0].ChemicalNames.Count == 4, $"Expected 4 Chemical Names; Got {m.Molecules[0].ChemicalNames.Count}");
            Assert.IsTrue(m.Molecules[0].Formulas.Count == 2, $"Expected 2 Formulae; Got {m.Molecules[0].Formulas.Count }");

            // Check that we have the expected number of Atoms and Bonds
            Assert.IsTrue(m.AllAtoms.Count == 25, $"Expected 25 Atoms; Got {m.AllAtoms.Count}");
            Assert.IsTrue(m.AllBonds.Count == 28, $"Expected 28 Bonds; Got {m.AllBonds.Count}");
        }

        [TestMethod()]
        public void CMLImportTest2()
        {
            CMLConverter mc = new CMLConverter();
            Model m = mc.Import(ChemistryValues.PHTHALOCYANINE);

            Assert.IsTrue(m.Molecules[0].Rings.Count == 12, $"Expected 12 Rings; Got {m.Molecules[0].Rings.Count}");
        }

        [TestMethod()]
        public void CMLRefreshTest()
        {
            CMLConverter mc = new CMLConverter();
            Model m = mc.Import(ChemistryValues.PHTHALOCYANINE);

            m.RefreshMolecules();

            Assert.IsTrue(m.Molecules[0].Rings.Count == 12, $"Expected 12 Rings; Got {m.Molecules[0].Rings.Count}");
        }

        [TestMethod()]
        public void CMLRefreshTest2()
        {
            CMLConverter mc = new CMLConverter();
            Model m = mc.Import(ChemistryValues.TESTOSTERONE);

            m.RefreshMolecules();

            var list = m.Molecules[0].SortRingsForDBPlacement();
            Assert.IsTrue(m.Molecules[0].Rings.Count == 4, $"Expected 4 Rings; Got {m.Molecules[0].Rings.Count}");
            Assert.IsTrue(list.Count == 4, $"Expected 4 Rings; Got {list.Count}");

            // Check that names and formulae are not trashed
            Assert.IsTrue(m.Molecules[0].ChemicalNames.Count == 4, $"Expected 4 Chemical Names; Got {m.Molecules[0].ChemicalNames.Count}");
            Assert.IsTrue(m.Molecules[0].Formulas.Count == 2, $"Expected 2 Formulae; Got {m.Molecules[0].Formulas.Count }");

            // Check that we have the expected number of Atoms and Bonds
            Assert.IsTrue(m.AllAtoms.Count == 25, $"Expected 25 Atoms; Got {m.AllAtoms.Count}");
            Assert.IsTrue(m.AllBonds.Count == 28, $"Expected 28 Bonds; Got {m.AllBonds.Count}");
        }

        [TestMethod()]
        public void SDtest()
        {
            SdFileConverter mc = new SdFileConverter();
            Model m = mc.Import(ChemistryValues.MOLFILE_BENZENE);

            Assert.IsTrue(m.Molecules.Count == 1, $"Expected 1 Molecule; Got {m.Molecules.Count}");
            Assert.IsTrue(m.Molecules.SelectMany(m1 => m1.Rings).Count() == 1, $"Expected 1 Ring; Got {m.Molecules.SelectMany(m1 => m1.Rings).Count()}");
            Assert.IsTrue(m.AllAtoms.Count == 6, $"Expected 6 Atoms; Got {m.AllAtoms.Count}");
            Assert.IsTrue(m.AllBonds.Count == 6, $"Expected 6 Bonds; Got {m.AllBonds.Count}");
        }

        [TestMethod()]
        public void SDtest2()
        {
            SdFileConverter mc = new SdFileConverter();
            Model m = mc.Import(ChemistryValues.BASIC_PARAFUCHSIN);

            Assert.IsTrue(m.Molecules.Count == 2, $"Expected 2 Molecules; Got {m.Molecules.Count}");
            Assert.IsTrue(m.Molecules.SelectMany(m1 => m1.Rings).Count() == 3, $"Expected 2 Rings; Got {m.Molecules.SelectMany(m1 => m1.Rings).Count()}");

            string molstring = mc.Export(m);
            mc = new SdFileConverter();
            Model m2 = mc.Import(molstring);
        }
    }
}