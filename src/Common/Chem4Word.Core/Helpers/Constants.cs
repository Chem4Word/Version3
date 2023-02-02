// ---------------------------------------------------------------------------
//  Copyright (c) 2023, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

namespace Chem4Word.Core.Helpers
{
    public static class Constants
    {
        public const string Chem4WordVersion = "3.0";
        public const string Chem4WordVersionFiles = "files3";
        public const string ContentControlTitle = "Chemistry";
        public const string LegacyContentControlTitle = "chemistry";
        public const string NavigatorTaskPaneTitle = "Navigator";
        public const string LibraryTaskPaneTitle = "Library";

        public const double TopLeftOffset = 24;
        public const string OoXmlBookmarkPrefix = "C4W_";
        public const string LibraryFileName = "Library.db";

        public const string DefaultEditorPlugIn800 = "ChemDoodle Web Structure Editor V8.0.0";
        public const string DefaultEditorPlugIn702 = "ChemDoodle Web Structure Editor V7.0.2";
        public const int ChemDoodleWeb800MinimumBrowserVersion = 11;
        public const string DefaultRendererPlugIn = "Open Office Xml Renderer V3";

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
        public const string Chem4WordExceptionsRegistryKey = @"SOFTWARE\Chem4Word V3\Exceptions";
        public const string Chem4WordAzureSettingsRegistryKey = @"SOFTWARE\Chem4Word V3\AzureSettings";

        // Update Checks
        public const int MaximumVersionsBehind = 7;
        public const string Chem4WordTooOld = "Chem4Word is too many versions old.";
        public const string WordIsNotActivated = "Micrsoft Word is not activated.";

        // Bond length limits etc
        public const double MinimumBondLength = 5;

        public const double StandardBondLength = 20;
        public const double MaximumBondLength = 95;
        public const double BondLengthTolerance = 1;

        public static readonly string[] OurDomains = { "https://www.chem4word.co.uk", "http://www.chem4word.com", "https://chem4word.azurewebsites.net" };
    }
}