// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

namespace Chem4Word.Core.Helpers
{
    public static class Constants
    {
        public static string ContentControlTitle = "Chemistry";
        public static string LegacyContentControlTitle = "chemistry";
        public static string NavigatorTaskPaneTitle = "Navigator";
        public static string LibraryTaskPaneTitle = "Library";
        public static double TopLeftOffset = 24;

        public const string LibraryFileName = "Library.db";

        public static string DefaultChem4WordWebServiceUri = "https://chemicalservices.azurewebsites.net/api/Resolve";
        
        public static string DefaultEditorPlugIn = "ChemDoodle Web Structure Editor V7.0.2";
        public static string DefaultRendererPlugIn = "Open Office Xml Renderer V3";

        // Task 336
        // CML constants
        public static string CmlNameSpaceCode = "cml";

        public static string CmlElementMolecule = "molecule";

        public const string ChemspiderIdName = "chemspider:Id";
        public const string ChemspiderInchiKeyName = "chemspider:Inchikey";
        public const string ChemSpiderSynonymName = "chemspider:Synonym";
        public const string ChemspiderFormulaName = "chemspider:Formula";
        public const string ChemSpiderSmilesName = "chemspider:Smiles";

        public const string Chem4WordInchiKeyName = "chem4word:CalculatedInchikey";
        public const string Chem4WordResolverIupacName = "chem4word:ResolvedIupacname";
        public const string Chem4WordResolverSmilesName = "chem4word:ResolvedSmiles";
        public const string Chem4WordResolverFormulaName = "chem4word:ResolvedFormula";

        public const string Chem4WordUserFormula = "chem4word:Formula";
        public const string Chem4WordUserSynonym = "chem4word:Synonym";

        // Registry Locations
        public const string Chem4WordRegistryKey = @"SOFTWARE\Chem4Word V3";

        public const string RegistryValueNameLastCheck = "Last Update Check";
        public const string RegistryValueNameVersionsBehind = "Versions Behind";
        public const string Chem4WordSetupRegistryKey = @"SOFTWARE\Chem4Word V3\Setup";
        public const string Chem4WordUpdateRegistryKey = @"SOFTWARE\Chem4Word V3\Update";

        public const double MinimumBondLength = 5;
        public const double StandardBondLength = 20;
        public const double MaximumBondLength = 95;
        public const double BondLengthTolerance = 1;
    }
}