// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using Chem4Word.Model;
using Chem4Word.Model.Enums;

namespace Chem4Word.Model.Converters
{
    // ReSharper disable once InconsistentNaming
    /*
       * This class handles conversion of ChemicalMarkupLanguage documents to and from
       * MDL Information Systems MolFile format. This class has support for both the
       * V2000 and V3000 MolFile formats. When reading files the Version is detected
       * automatically and can later be found using getVersion. When writing files,
       * use SetVersion to dictate what verison of MolFile is written out.
       *
       * The converter currently has full support for the V2000 & V3000 connection
       * tables, as well as many properties of atoms including charge, isotopic mass,
       * and spin multiplicity.
       *
       * Stereochemistry support is currently limited.
       *
       * @author Peter Murray-Rust, Ramin Ghorashi (2005)
       * branched/amended for Jumbo-converter 2008 (PMR)
       */

    // Ported to C# by Mike Williams 09-Mar-2016
    public class MDLConverter : IConverter
    {
        /*
        * This class handles conversion of ChemicalMarkupLanguage documents to and from
        * MDL Information Systems MolFile format. This class has support for both the
        * V2000 and V3000 MolFile formats. When reading files the Version is detected
        * automatically and can later be found using getVersion. When writing files,
        * use SetVersion to dictate what verison of MolFile is written out.
        *
        * The converter currently has full support for the V2000 & V3000 connection
        * tables, as well as many properties of atoms including charge, isotopic mass,
        * and spin multiplicity.
        *
        * Stereochemistry support is currently limited.
        *
        * @author Peter Murray-Rust, Ramin Ghorashi (2005)
        * branched/amended for Jumbo-converter 2008 (PMR)
        */

        // Ported to C# by Mike Williams 09-Mar-2016

        // https://sourceforge.net/p/cml/code/HEAD/tree
        // https://sourceforge.net/p/cml/code/HEAD/tree/euclidnew/src/main/java/org/xmlcml/euclidnew/EuclidConstants.java
        // https://sourceforge.net/p/cml/code/HEAD/tree/jumbo-converters/trunk/src/main/java/org/xmlcml/cml/converters/molecule/mdl/MDLConverter.java

        // EuclidConstants.java
        // --------------------
        private const char C_QUOT = '"';

        private const string S_EMPTY = "";
        private const string S_EQUALS = "=";
        private const string S_LBRAK = "(";
        private const string S_MINUS = "-";
        private const string S_QUOT = "\"";
        private const string S_RBRAK = ")";
        private const string S_SPACE = " ";

        // https://sourceforge.net/p/cml/code/HEAD/tree/jumbo-converters/trunk/src/main/java/org/xmlcml/cml/converters/molecule/mdl/MDLConverter.java

        // MDLConverter.java
        // -----------------
        private class MDLTag
        {
            internal const string DUNK = S_SPACE + S_SPACE;     // Unknown MDL dimensional code
            internal const string D2 = "2D";                    // Represents the MDL dimensional code for a 2D molecule
            internal const string D3 = "3D";                    // Represents the MDL dimensional code for a 3D molecule
            internal const string M_CHG = "M  CHG";             // Represents the tag in the MDL Properties Block for Charge
            internal const string M_END = "M  END";             // Represents the tag in the MDL Properties Block terminating the MolFile
            internal const string M_ISO = "M  ISO";             // Represents the tag in the MDL Properties Block for an Isotopes list
            internal const string M_RAD = "M  RAD";             // Represents the tag in the MDL Properties Block for an Radical list
            internal const string M_SAL = "M  SAL";             // Represents the tag in the MDL Properties Block for an SGroup atom list
            internal const string M_SBL = "M  SBL";             // Represents the tag in the MDL Properties Block for an SGroup bond list
            internal const string M_SBV = "M  SBV";             // Represents the tag in the MDL Properties Block for an SGroup bond vector (display only)
            internal const string M_SDS_EXP = "M  SDS EXP";     // Represents the tag in the MDL Properties Block for an expansion status of an SGroup (display only)
            internal const string M_SLB = "M  SLB";             // Represents the tag in the MDL Properties Block for a Unique Sgroup identifier (MACCS-II)
            internal const string M_SMT = "M  SMT";             // Represents the tag in the MDL Properties Block for the subscript text of an SGroup
            internal const string M_STY = "M  STY";             // Represents the tag in the MDL Properties Block for an SGroup type

            internal const string A__ = "A  ";                  // Represents the tag in the MDL Properties Block for an atom Alias
            internal const string G__ = "G  ";                  // Represents the tag in the MDL Properties Block for Group
            internal const string V__ = "V  ";                  // Undocumented atom label
            internal const string S_SKP = "S  SKP";             // Represents the skip tag in the MDL Properties Block

            // V3000 keywords
            internal const string M_V30 = "M  V30 ";            // Represents the prefix of all lines in a V3000 MolFile

            internal const string V3_CHARGE = "CHG";            // Represents the V3000 keyword for the charge on an atom
            internal const string V3_ISOTOPE = "MASS";          // Represents the V3000 keyword for the isotopic mass of an atom
            internal const string V3_RADICAL = "RAD";           // Represents the V3000 keyword for the spin multiplicity of an atom
            internal const string V3_HCOUNT = "HCOUNT";         // Represents the V3000 keyword for the hydrogen count of an atom
            internal const string V3_STEREO = "CFG";            // Represents the V3000 keyword for the stereochemistry of an atom or bond
            internal const string SDF_END = "$$$$";             // Represents the end of the SD file block
        }

        private enum CoordType
        {
            TWOD,
            THREED,
            EITHER
        }

        /** represents the V2000 Version tag */
        public static String V2000 = "V2000";

        /** represents the V3000 Version tag */
        public static String V3000 = "V3000";

        //private bool v3000 = false;

        /** number of atoms to be read in the MOL molecule */
        private int molAtomCount;
        /** number of bonds to be read in the MOL molecule */
        private int molBondCount;

        /*total count of molatoms so far*/
        private int runningTotalCount = 0;

        private string dimensionalCode = MDLTag.DUNK;

        private String Version = "V2000";

        private Dictionary<int, Atom> atomByNumber = new Dictionary<int, Atom>();
        private Dictionary<int, Bond> bondByNumber = new Dictionary<int, Bond>();
        private Dictionary<Atom, int> numberByAtom = new Dictionary<Atom, int>();

        private int nline;
        private List<String> lines;

        private TextReader _tr; //used to read the mol files
        private TextWriter _tw; //used to write out the files
        private Molecule molecule;

        private Model _model; //used in exporting

        private static PeriodicTable _pt = new PeriodicTable();
        private CoordType coordType { get; set; }

        public MDLConverter()
        {
            Init();
        }

        protected void Init()
        {
            coordType = CoordType.EITHER;
        }

        /**
         * translates CML bond order codes into MDLMol numbers
         *
         * @param cmlCode the CML bondorder code
         * @return the MDL bondorder number
         */

        private static double MolBondOrder(String cmlCode)
        {



            return Bond.OrderToOrderValue(cmlCode) ?? 0;
        }

        /**
         * translates MDLMol bond order numbers CML codes
         *
         * @param molNumber the MDLMol bond order number
         * @return the CML bond order code
         */

        private static String BondOrder(int molNumber)
        {
            String order = S_EMPTY;

            return Bond.OrderValueToOrder(molNumber);
        }

        /**
         * translates CML bond stereo codes into MDLMol numbers
         *
         * @param cmlCode the CML bondstereo code
         * @return the MDL bondstereo number
         */

        private static int MolBondStereo(BondStereo code)
        {
            int stereo = 0;
            if (code == BondStereo.None)
            {
                stereo = 0;
            }
            else if (code==BondStereo.Wedge)
            {
                stereo = 1;
            }
            else if (code==BondStereo.Hatch)
            {
                stereo = 6;
            }
            else
            {
                stereo = 0;
            }
            return stereo;
        }

        /**
         * translates CML bond stereo codes into MDLMol V3000 numbers
         *
         * @param cmlCode the CML bondstereo code
         * @return the V3000 MDL bondstereo number
         */

        private static int V3MolBondStereo(BondStereo code)
        {
            int stereo = 0;
            if (code == BondStereo.None)
            {
                stereo = 0;
            }
            else if (code == BondStereo.Wedge)
            {
                stereo = 1;
            }
            else if (code==BondStereo.Hatch)
            {
                stereo = 3;
            }
            else
            {
                stereo = 0;
            }
            return stereo;
        }

        /**
         * translates MDL numbers into JUMBO-MOL codes
         */

        private static String CmlStereoBond(int molNumber)
        {
            String stereo = S_EMPTY;
            if (molNumber == 1)
            {
                stereo = "W";
            }
            else if (molNumber == 6)
            {
                stereo = "H";
            }
            else
            {
                stereo = S_EMPTY; // ToDo: Check; Was Bond.NOSTEREO
            }
            return stereo;
        }

        /**
         * translates V3000-MDL numbers into JUMBO-MOL codes
         */

        private static String V3CmlStereoBond(int molNumber)
        {
            String stereo = S_EMPTY;
            if (molNumber == 1)
            {
                stereo = "W";
            }
            else if (molNumber == 2)
            {
                // either TODO sort out "either" stereo config
                stereo = "";
            }
            else if (molNumber == 3)
            {
                stereo = "H";
            }
            else
            {
                stereo = S_EMPTY; // ToDo: Check; Was Bond.NOSTEREO;
            }
            return stereo;
        }

        /**
         * Checks if a particular element exists in the periodic table as defined
         * within the ChemicalElement class
         *
         * @param element
         *            AS of the element to find
         * @return true if element exists, false otherwise
         */

        private static bool ElementExists(String element)
        {

            bool result = _pt.Elements.ContainsKey(element);
            return result;
            // return ChemicalElement.getChemicalElement(element) != null;
        }

        /**
         * Adds zeros on the left of string s until it reaches length l
         *
         * @param s
         *            String to pad
         * @param l
         *            Required length of final string
         * @return Padded string
         */

        private static String PadLeftZero(String s, int l)
        {
            int ll = s.Length;
            while (ll++ < l)
            {
                s = "0" + s;
            }
            return s;
        }

        /**
         * Parses a string as an integer. Uses Integer.parseInt after removing any
         * leading zeros
         *
         * @param s
         *            String containing the integer to be parsed
         * @return the parsed integer
         */

        private static int ParseInteger(String s)
        {
            int i = 0;
            int l = s.Length;
            s = s.Trim();

            // trim leading zeros
            while (l > 1 && s[0] == '0')
            {
                s = s.Substring(1);
                l--;
            }

            if (!(s.Equals(S_EMPTY)))
            {
                try
                {
                    i = int.Parse(s);
                }
                catch (Exception)
                {
                    throw new Exception("bad integer in: " + s);
                }
            }

            return i;
        }

        /**
         * Parses the substring of a string as an integer. Uses Integer.parseInt
         * after removing any leading zeros
         *
         * @param numbr the full string containg the integer
         * @param start the start index of the interger to be parsed
         * @param length of the interger to be parsed
         * @return the parsed ingeger
         */

        private static int ParseInteger(String numbr, int start, int length)
        {
            int n = 0;
            if (numbr != null && numbr.Length >= start + length)
            {
                string num = numbr.Substring(start, length).Trim();
                n = ParseInteger(num);
            }
            return n;
        }

        /**
         * Outputs an integer for inclusion in an MDLMolFile, the integer is
         * outputted in the format "000" and will be 3 characters long
         *
         * @param intgr
         * @return the integer
         */

        private static String OutputMDLInt(int intgr)
        {
            String s = "" + intgr;
            while (s.Length < 3)
            {
                s = S_SPACE + s;
            }
            return s;
        }

        /**
         * Outputs a float for inclusion in an MDLMolFile, the float is outputted in
         * the format "####0.0000" and will be 10 characters long (F10.4)
         *
         * @param value
         * @return the float
         */

        private static String OutputMDLFloat(double value)
        {
            String s = value.ToString("0.0000");
            while (s.Length < 10)
            {
                s = S_SPACE + s;
            }
            return s;
        }

        /**
         * Sets the Version of MolFile that MDLConverter will write out. If Version
         * isnt set before writing a MolFile the default value of V2000 is used.
         *
         * @param Version
         *            either MDLConverter.V2000 or MDLConverter.V3000
         */

        public void SetVersion(String version)
        {
            this.Version = version;
            if (!version.Equals(V2000) && !version.Equals(V3000))
            {
                throw new Exception("unknown MDLMol Version");
            }
        }

        /**
         * Returns the Version of MolFile that MDLConverter will write out, if
         * called after reading in a MolFile it will return the Version of that
         * MolFile
         *
         * @return Version
         */

        public String GetVersion()
        {
            return Version;
        }

        /**
         * Reads in the next line of the file currently being parsed
         *
         * @return String the next line
         */

        private String NextLine()
        {
            String nextLineS = _tr.ReadLine();

            if (nextLineS == null)
            {
                return null;
            }
            else
            {
                if (nextLineS.StartsWith(MDLTag.M_V30))
                {
                    // V3000 line
                    nextLineS = nextLineS.Substring(MDLTag.M_V30.Length).Trim();
                    if (nextLineS.EndsWith(S_MINUS))
                    {
                        nextLineS += NextLine();
                    }
                }
                return nextLineS;
            }
        }

        /**
         * Reads the header of an MDLMolFile, check the Version is supported, reads
         * in the title, date, comment and counts line
         */

        private void ReadHeader()
        {
            // line 1 TITLE
            String title = NextLine();

            if (title.StartsWith("$MDL"))
            {
                throw new Exception("RGFiles currently not supported");
            }
            else if (title.StartsWith("$RXN"))
            {
                throw new Exception("RXNFiles currently not supported");
            }
            else if (title.StartsWith("$RDFILE"))
            {
                throw new Exception("RDFiles currently not supported");
            }
            else if (title.StartsWith("<XDfile>"))
            {
                throw new Exception("XDFiles currently not supported");
            }

            if (title.Trim() != "")
            {
                // ToDo: Check if we can do this?
                //molecule.Id = title.Trim();
            }

            // line 2 HEADER
            String line = NextLine();

            // MAW Forced to 2D
            dimensionalCode = MDLTag.D2;

            if (!line.Equals(S_EMPTY))
            {
                try
                {
                    // MAW: Doing Nothing here to force 2D
                    // 2- or 3- dim? or missing
                    //if (line.Substring(20, 2).Trim().Equals(MDLTag.D2))
                    //{
                    //    dimensionalCode = MDLTag.D2;
                    //}
                    //else if (line.Substring(20, 2).Trim().Equals(MDLTag.D3))
                    //{
                    //    dimensionalCode = MDLTag.D3;
                    //}

                    // ToDo: do something with the date read from MOL
                    //String date = S_EMPTY;
                    //date = line.Substring(10, 10);
                    //if (!date.Trim().Equals(S_EMPTY))
                    //{
                    //    int y = ParseInteger(date, 4, 6);
                    //    String year = (y > 50) ? S_EMPTY + (y + 1900) : S_EMPTY
                    //            + (y + 2000);
                    //    String month = date.Substring(0, 2).Trim();
                    //    String day = date.Substring(2, 4).Trim();
                    //    date = year + S_MINUS + month + S_MINUS + day;
                    //    // Date d = new Date(date);
                    //}
                }
                catch (Exception)
                {
                    // doesnt matter
                }
            }

            // line 3 COMMENT
            String comment = NextLine();

            if (!comment.Trim().Equals(S_EMPTY))
            {
                // TODO add constructor for CMLLabel to set value?
                //CmlLabel label = new CmlLabel();
                //label.Value = comment.Trim();
            }

            // start the CTAB
            // line 4 counts
            line = NextLine();

            // read number of atoms & bonds
            molAtomCount = ParseInteger(line, 0, 3);
            molBondCount = ParseInteger(line, 3, 3);

            try
            {
                if (line.Substring(34, 5) != "     ")
                {
                    Version = line.Substring(34, 5);
                }
            }
            catch (Exception)
            {
                // doesnt matter
            }
        }

        /**
         * Reads in the atoms block of a MDLMol connections table
         */

        private void ReadAtoms()
        {
            Atom thisAtom;
            Double x, y, z;

            for (int i = 0; i < molAtomCount; i++)
            {
                String line = NextLine();
                // create atom object
                thisAtom = new Atom();
                thisAtom.Id = "a" + (i + 1);
                molecule.Atoms.Add(thisAtom);
                atomByNumber.Add(i + 1, thisAtom);

                x = Double.Parse(line.Substring(0, 9).Trim());
                y = Double.Parse(line.Substring(10, 9).Trim());
                z = Double.Parse(line.Substring(20, 9).Trim());

                if (dimensionalCode.Equals(MDLTag.D2) | !(Math.Abs(z) > 0.0001))
                {
                    thisAtom.Position=new Point(x,0 - y);
                }
                else if (dimensionalCode.Equals(MDLTag.D3) | Math.Abs(x) > 0.0001
                      | Math.Abs(y) > 0.0001 | Math.Abs(z) > 0.0001)
                {
                    thisAtom.Position = new Point(x, 0-y);

                    //thisAtom.Z3 = z;
                }

                // field 3 is atom parity (a *write-only* field, so not used)
                // int parity = ParseInteger(line, 38, 3);

                // field 5 stereoCareBox
                // int field5 = ParseInteger(line, 44, 3);

                // field 6 valency/oxidation state
                // int oxState = ParseInteger(line, 48, 3);

                // element type
                String elType = line.Substring(31, 3).Trim();
                if (!ElementExists(elType))
                {
                    throw new Exception(elType + " is not a valid element atomicSymbol");
                }
                thisAtom.Element = _pt.Elements[elType];

                // isotope
                int delta = ParseInteger(line, 34, 3);
                if (delta != 0)
                {
                    // ToDo: What to do here ???
                    //if (ElementExists(elType))
                    //{
                    //    ChemicalElement chemEl = ChemicalElement
                    //            .getChemicalElement(elType);
                    //    int isotope = chemEl.getMainIsotope() + delta;
                    //    thisAtom.setIsotope(isotope);
                    //}
                    //else {
                    //    throw new RuntimeException("cannot find isotopic weight of " + elType);
                    //}
                }

                // charge
                int ch = ParseInteger(line, 35, 3);
                if (ch == 4)
                {
                    // there are issues here, '4' doesn't have a consistent meaning
                    // thisAtom.setSpinMultiplicity(2);
                }
                else if (ch > 0)
                {
                    thisAtom.FormalCharge = 4 - ch;
                }

                // atom-atom mapping
                int atomMap = ParseInteger(line, 60, 3);
                if (atomMap != 0)
                {
                    // ToDo: What to do here ???
                    //CmlScalar scalar = new CmlScalar();
                    //scalar.setDictRef(NamespaceRefAttribute.createValue("mol", "atomMap"));
                    //scalar.setXMLContent("" + atomMap);
                    //thisAtom.addScalar(scalar);
                }
            }
        }

        /**
         * Reads in the bonds block of a MDLMol connections table
         */

        private void ReadBonds()
        {
            Bond thisBond;

            for (int i = 0; i < molBondCount; i++)
            {
                String line = NextLine();

                int atomNumber1 = int.Parse(line.Substring(0, 3).Trim());
                Atom atom1 = atomByNumber[atomNumber1];

                if (atom1 == null)
                {
                    throw new Exception("Cannot resolve atomNumber :"
                            + atomNumber1 + ": in " + line);
                }

                int atomNumber2 = int.Parse(line.Substring(3, 3).Trim());
                Atom atom2 = atomByNumber[atomNumber2];

                if (atom2 == null)
                {
                    throw new Exception("Cannot resolve atomNumber :"
                            + atomNumber2 + ": in " + line);
                }

                String order = BondOrder(ParseInteger(line, 6, 3));
                String stereo = CmlStereoBond(ParseInteger(line, 9, 3));
                thisBond = new Bond() {StartAtom = atom1, EndAtom = atom2};
                thisBond.Id = "b" + (i + 1);

                molecule.Bonds.Add(thisBond);
                bondByNumber.Add(i + 1, thisBond);

                // order

                thisBond.Order = order;
               

                // stereo
                if (!string.IsNullOrEmpty(stereo))
                {
                    // ToDo: Fix import of stereo
                    BondStereo bs;
                    if (BondStereo.TryParse(stereo, out bs))
                    {
                        thisBond.Stereo = bs;
                    }
                }
            }
        }

        /**
         * Reads in a single line of the MDLMol properties block (either
         * MDLTag.M_RAD, MDLTag.M_CHG, MDLTag.M_ISO)
         *
         * @param line
         */

        private void ReadPropertyLine(String line)
        {
            String propertyType = line.Substring(0, 6);

            int nFields = ParseInteger(line, 7, 3);
            for (int i = 0; i < nFields; i++)
            {
                int startAt = 8 * i + 10;
                // read from MOL
                int atomNumber = ParseInteger(line, startAt, 3);
                int value = ParseInteger(line, startAt + 4, 3);
                // write to CML
                if (propertyType.Equals(MDLTag.M_CHG))
                {
                    atomByNumber[atomNumber].FormalCharge = value;
                }
                else if (propertyType.Equals(MDLTag.M_ISO))
                {
                    atomByNumber[atomNumber].IsotopeNumber = value;
                }
                else if (propertyType.Equals(MDLTag.M_RAD))
                {
                    atomByNumber[atomNumber].SpinMultiplicity = value;
                }
            }
        }

        /**
         * Reads the footer of an MDLMolFile including the properties block and
         * SGroups TODO ReadFooter is a mess!
         *
         * added "V" for atom annotation as it was emitted by ISIS although undocumented
         */

        private void ReadFooter()
        {
            // Clear map of SGroup ID's
            //SGroup.clearMap();
            while (true)
            {
                String line = "";
                try
                {
                    line = NextLine().Trim();
                }
                catch (Exception)
                {
                    line = MDLTag.M_END;
                    //LOG.warn("No " + MDLTag.M_END.tag
                    //        + " in properties; unexpected EOF");
                }

                if (line.Equals(MDLTag.SDF_END))
                {
                    continue;//just skip this - M_END should  break
                }
                if (line.Equals(MDLTag.M_END))
                {
                    //SGroup.tidySGroups();
                    break;
                }
                else if (line.Equals(MDLTag.S_SKP))
                {
                    // S SKPnnn - skip next n lines
                    int numberOfLines = ParseInteger(line, 6, 3);
                    for (int i = 0; i < numberOfLines; i++)
                    {
                        NextLine();
                    }
                }
                else if (line.StartsWith(MDLTag.M_CHG)
                      || line.StartsWith(MDLTag.M_RAD)
                      || line.StartsWith(MDLTag.M_ISO))
                {
                    ReadPropertyLine(line);
                }
                else if (line.StartsWith(MDLTag.M_STY))
                {
                    // Sgroup FormulaType, defines SGroup
                    // M STYnn8 sss ttt ...

                    line = line.Substring(MDLTag.M_SAL.Length + 1);
                    int nSGroups = ParseInteger(line, 0, 3);
                    line = line.Substring(3);
                    for (int i = 0; i < nSGroups; i++)
                    {
                        int SGroupNumber = ParseInteger(line, 8 * i, 3);
                        String SGroupType = line.Substring(8 * i + 4, 3);
                        //new SGroup(SGroupNumber, SGroupType);
                    }
                }
                else if (line.StartsWith(MDLTag.M_SAL))
                {
                    // SGROUP atom list
                    // M SAL sssn15 aaa ...
                    line = line.Substring(MDLTag.M_SAL.Length + 1);
                    int sgroupId = ParseInteger(line, 0, 3);
                    //SGroup sgroup = SGroup.getSGroup(sgroupId);
                    //if (sgroup == null)
                    //{
                    //    throw new RuntimeException("Cannot find SGROUP: " + sgroupId);
                    //}
                    line = line.Substring(3);
                    int nAtoms = ParseInteger(line, 0, 3);
                    line = line.Substring(3);
                    for (int i = 0; i < nAtoms; i++)
                    {
                        line = line.Substring(1);
                        //sgroup.addAtom(atomByNumber
                        //        .get(ParseInteger(line, 0, 3) - 1));
                        line = line.Substring(3);
                    }
                }
                else if (line.StartsWith(MDLTag.M_SBL))
                {
                    // SGROUP bond list
                    // M SBL sssn15 bbb ...
                    line = line.Substring(MDLTag.M_SBL.Length + 1);
                    int sgroupId = ParseInteger(line, 0, 3);
                    line = line.Substring(3);
                    //SGroup sgroup = SGroup.getSGroup(sgroupId);
                    //if (sgroup == null)
                    //{
                    //    throw new RuntimeException("Cannot find SGROUP: " + sgroupId);
                    //}
                    int nbonds = ParseInteger(line, 0, 3);
                    line = line.Substring(3);
                    for (int i = 0; i < nbonds; i++)
                    {
                        line = line.Substring(1);
                        //sgroup.addBond(bondByNumber
                        //        .get(ParseInteger(line, 0, 3) - 1));
                        line = line.Substring(3);
                    }
                }
                else if (line.StartsWith(MDLTag.M_SBV))
                {
                    // Superatom Bond and Vector Information (Display only)
                }
                else if (line.StartsWith(MDLTag.M_SDS_EXP))
                {
                    // Sgroup index of expanded superatoms (Display only)
                    // M SDS EXPn15 sss ...
                    line = line.Substring(MDLTag.M_SDS_EXP.Length);
                    int nsg = ParseInteger(line, 0, 3);
                    line = line.Substring(3);
                    for (int i = 0; i < nsg; i++)
                    {
                        line = line.Substring(1); // space
                        int sgroupId = ParseInteger(line, 0, 3);
                        //SGroup sgroup = SGroup.getSGroup(sgroupId);
                        //if (sgroup == null)
                        //{
                        //    throw new RuntimeException("Cannot find SGROUP: "
                        //            + sgroupId);
                        //}
                        //sgroup.setExpanded(true);
                        line = line.Substring(3);
                    }
                }
                else if (line.StartsWith(MDLTag.M_SLB))
                {
                    // Unique Sgroup identifier (MACCS-II only); seems to be
                    // redundant
                }
                else if (line.StartsWith(MDLTag.M_SMT))
                {
                    // Sgroup Subscript (label)
                    // M SMT sss m...
                    line = line.Substring(MDLTag.M_SMT.Length + 1);
                    int sgroupId = ParseInteger(line, 0, 3);
                    //SGroup sgroup = SGroup.getSGroup(sgroupId);
                    //if (sgroup == null)
                    //{
                    //    throw new RuntimeException("Cannot find SGROUP: " + sgroupId);
                    //}
                    line = line.Substring(3);
                    //sgroup.setLabel(line.Trim());
                }
                else if (line.StartsWith(MDLTag.A__))
                {
                    int atomNum = ParseInteger(line, 3, 3);
                    Atom atom = atomByNumber[atomNum];
                    line = NextLine().Trim();
                    //TODO:  resolve this issue of label handling pronto
                    //CmlLabel label = new CmlLabel();
                    //label.Value = line;


                    //atom.addLabel(label);
                    // all Groups are "R"
                    //atom.setElementType("R");
                    //LOG.info("The element of atom number " + atomNum
                    //        + " has been set to R with an alias/label of '" + line
                    //        + "'");
                }
                else if (line.StartsWith(MDLTag.G__))
                {
                    // obsolete and superseded by SGROUPS
                    NextLine(); // skip label line
                }
                else if (line.StartsWith(MDLTag.V__))
                {
                    // undocumented atom annotation
                    /*
                    V    2 test
                    V    3 4
                    V    4 3
                    V    5 2
                    V    6 1
                    */
                    // guess this means atomNum, annotation (or any length??)
                    int atomNum = ParseInteger(line, 3, 3);
                    String annotation = line.Substring(6);
                    Atom atom = atomByNumber[atomNum];
                    if (atom == null)
                    {
                        throw new Exception("Cannot find atom in V: " + atomNum);
                    }
                    //CmlLabel label = new CmlLabel();
                    //label.setDictRef("mdl:atomLabel");
                    //label.setCMLValue(annotation);
                    //atom.addLabel(label);
                }
                else if (line.Trim().Equals(S_EMPTY))
                {
                    //LOG.warn("WARNING: missing '" + MDLTag.M_END.tag
                    //        + "'; trying to recover");
                    break;
                }
            }

            // iterate round sGroups

            //Map<Integer, SGroup> map = SGroup.getMap();
            //for (Integer ii : map.keySet())
            //{
            //    SGroup nextSGroup = map.get(ii);
            //    //        }
            //    //        for (Iterator<SGroup> theSGroupIterator = SGroup.getSGroupIterator(); theSGroupIterator
            //    //                .hasNext();) {
            //    //            SGroup nextSGroup = theSGroupIterator.next();
            //    AtomSet atomSet = new AtomSet();

            //    atomSet.setId("sgrp" + nextSGroup.id);
            //    atomSet.setTitle(nextSGroup.label);

            //    for (Atom atom : nextSGroup.atomList)
            //    {
            //        atomSet.addAtom(atom);
            //    }
            //    molecule.appendChild(atomSet);
            //}
        }

        /**
         * Reads in the counts line in a V3000 MolFile extracting the number of
         * atoms and number of bonds in the connection table.
         */

        private void V3ReadCountsLine(String line)
        {
            // M V30 COUNTS 25 25 0 0 0
            List<String> values = V3ReadValues(line);
            molAtomCount = ParseInteger(values[0]);
            molAtomCount = ParseInteger(values[1]);
            molBondCount = ParseInteger(values[2]);
        }

        /**
         * Reads in the values seperated by spaces on a line in a V3000 MolFile also
         * obeys rules on using quotes and doubling quotes. Stops reading in values
         * once a [Keyword=value] parameter is met
         *
         * @param line
         *            the line in the V3000 MolFile
         * @return an iterator over the values found (in order)
         */

        private List<String> V3ReadValues(String line)
        {
            List<String> values = new List<String>();
            // line should have "M V30 " chopped off
            line = line.Trim() + S_SPACE;

            while (line.IndexOf(S_SPACE) != -1)
            {
                int endOfValue = 0;
                if (line.IndexOf(S_SPACE) == -1)
                {
                    break;
                }

                if (line.StartsWith(S_QUOT))
                {
                    line = line.Substring(1);
                    endOfValue = line.IndexOf(S_QUOT);

                    while (line[endOfValue + 1] == C_QUOT)
                    {
                        endOfValue = endOfValue + 2
                                + line.Substring(endOfValue + 2).IndexOf(S_QUOT);
                    }
                }
                else
                {
                    endOfValue = line.IndexOf(S_SPACE);
                }

                String theValue = line.Substring(0, endOfValue);

                if (theValue.IndexOf(S_EQUALS) != -1)
                {
                    break;
                }
                else if (!theValue.Equals("") && !theValue.Equals(S_SPACE))
                {
                    if (theValue.StartsWith(S_LBRAK))
                    {
                        // first value in bracket is a count
                        theValue = theValue.Substring(1);
                    }
                    if (theValue.EndsWith(S_RBRAK))
                    {
                        theValue = theValue.Substring(0, theValue.Length - 1);
                    }
                    values.Add(theValue.Replace(S_QUOT + S_QUOT, S_QUOT));
                }

                line = line.Substring(endOfValue + 1);
            }
            return values;
        }

        /**
         * Reads in the atoms block of a MDLMol V3000 connections table
         *
         */

        private void V3ReadAtomBlock()
        {
            for (int i = 0; i < molAtomCount; i++)
            {
                String line = NextLine();
                if (line.Equals("END ATOM"))
                {
                    throw new Exception("unexpected end of atom block");
                }

                List<String> values = V3ReadValues(line);
                /*
                 * M V30 index type x y z aamap - M V30 [CHG=val] [RAD=val]
                 * [CFG=val] [MASS=val] - M V30 [VAL=val] - M V30 [HCOUNT=val]
                 * [STBOX=val] [INVRET=val] [EXACHG=val] -
                 */

                int atomNumber = int.Parse(values[0]);
                String atomType = values[1];
                Double x = Double.Parse(values[2]);
                Double y = Double.Parse(values[3]);
                Double z = Double.Parse(values[4]);
                int atomMap = int.Parse(values[5]);

                String charge = V3ReadKeywordValue(MDLTag.V3_CHARGE, line);
                String isotope = V3ReadKeywordValue(MDLTag.V3_ISOTOPE, line);
                String radical = V3ReadKeywordValue(MDLTag.V3_RADICAL, line);
                String hcount = V3ReadKeywordValue(MDLTag.V3_HCOUNT, line);

                Atom thisAtom = new Atom();
                thisAtom.Id = "a" + atomNumber;
                molecule.Atoms.Add(thisAtom);
                atomByNumber.Add(atomNumber, thisAtom);

                if (dimensionalCode.Equals(MDLTag.D2) | !(Math.Abs(z) > 0.0001))
                {
                    thisAtom.Position = new Point(x,y);
   
                }
                else if (dimensionalCode.Equals(MDLTag.D3)
                      | Math.Abs(x) > 0.0001 | Math.Abs(y) > 0.0001
                      | Math.Abs(z) > 0.0001)
                {
                    thisAtom.Position = new Point(x,y);
                  
                    //thisAtom.Z3 = z;
                }

                if (!ElementExists(atomType))
                {
                    throw new Exception(atomType + " is not a valid element atomicSymbol");
                }
                thisAtom.Element = _pt.Elements[atomType];
                if (atomMap != 0)
                {
                    //CMLScalar scalar = new CMLScalar();
                    //scalar.setDictRef("mol:atomMap");
                    //scalar.setXMLContent("" + atomMap);
                    //thisAtom.addScalar(scalar);
                }

                if (charge != null)
                {
                    thisAtom.FormalCharge = ParseInteger(charge);
                }

                if (isotope != null)
                {
                    thisAtom.IsotopeNumber = ParseInteger(isotope);
                }

                if (radical != null)
                {
                    thisAtom.SpinMultiplicity = ParseInteger(radical);
                }

                if (hcount != null)
                {
                    //thisAtom.setHydrogenCount(ParseInteger(hcount));
                }
            }
        }

        /**
         * Reads in a value belonging to a specific keyword on a line in a V3000
         * MolFile. [keyword=value] also obeys rules on quotes and double quotes
         *
         * @param keyword
         *            the keyword to find the values from
         * @param line
         *            the V3000 MolFile line
         * @return the value as a string
         */

        private String V3ReadKeywordValue(String keyword, String line)
        {
            List<String> it = V3ReadKeywordArray(keyword, line);
            if (it != null)
            {
                return V3ReadKeywordArray(keyword, line)[0];
            }
            else
            {
                return null;
            }
        }

        /**
         * Reads in an array of values belonging to a specific keyword on a line in
         * a V3000 MolFile. [keyword=(count val1 val2 val3)] also obeys rules on
         * quotes and double quotes
         *
         * @param keyword
         *            the keyword to find the values from
         * @param line
         *            the V3000 MolFile line
         * @return an iterator over the values (doesnt include the count)
         */

        private List<String> V3ReadKeywordArray(String keyword, String line)
        {
            if (line.IndexOf(keyword) == -1)
            {
                return null;
            }
            else
            {
                int startOfKeyword = line.IndexOf(keyword) + keyword.Length + 1;
                line = line.Substring(startOfKeyword);
                return V3ReadValues(line);
            }
        }

        /**
         * Reads in the atoms block of a MDLMol V3000 connections table
         *
         */

        private void V3ReadBondBlock()
        {
            /*
             * M V30 index type atom1 atom2 [CFG=val] [TOPO=val] [RXCTR=val]
             * [STBOX=val]
             */
            for (int i = 0; i < molBondCount; i++)
            {
                String line = NextLine();
                if (line.Equals("END BOND"))
                {
                    throw new Exception("unexpected end of atom block");
                }

                List<String> values = V3ReadValues(line);

                int bondNumber = int.Parse(values[0]);
                int bondOrder = int.Parse(values[1]);
                int atomNumber1 = int.Parse(values[2]);
                int atomNumber2 = int.Parse(values[3]);

                String bondStereo = V3ReadKeywordValue(MDLTag.V3_STEREO, line);

                Atom atom1 = atomByNumber[atomNumber1];
                Atom atom2 = atomByNumber[atomNumber2];

                if (atom1 == null || atom2 == null)
                {
                    throw new Exception("Bond " + bondNumber
                            + " refers to invalid atoms");
                }

                Bond thisBond = new Bond();
                thisBond.Id = "b" + bondNumber;
                thisBond.StartAtom = atom1;
                thisBond.EndAtom =  atom2;
                molecule.Bonds.Add(thisBond);

                bondByNumber.Add(bondNumber, thisBond);

                thisBond.Order = Bond.OrderValueToOrder(bondOrder);
                

                if (bondStereo != null)
                {
                    String stereo = V3CmlStereoBond(ParseInteger(bondStereo));
                    BondStereo bs;
                    BondStereo.TryParse(stereo, out bs);
                    thisBond.Stereo = bs;
                }
            }
        }

        /**
         * Reads an input stream containing either a single MDL molecule or
         * positioned at the start of one. The Version of MolFile is automatically
         * detected and can be found using getVersion()
         *
         * @param reader pointing to the MolFile
         * @return the parsed Molecule
         */

        public Molecule ReadMol(TextReader tr)
        {
            molecule = new Molecule();

            _tr = tr;
            try
            {
                ReadHeader();
            }
            catch (NullReferenceException nrex)
            {
                //End of the SD File - we've run out of molecules to parse
                return null;
            }
            if (Version.ToUpper().Equals(V2000))
            {
                ReadAtoms();
                ReadBonds();
                ReadFooter();
            }
            else if (Version.ToUpper().Equals(V3000))
            {
                // find start of CTAB
                String line = NextLine();
                while (!line.Equals("BEGIN CTAB"))
                {
                    line = NextLine();
                    if (line == null)
                    {
                        throw new Exception("CTAB block not found!");
                    }
                }
                V3ReadCountsLine(NextLine());

                // find start of ATOM block
                line = NextLine();
                while (!line.Equals("BEGIN ATOM"))
                {
                    line = NextLine();
                    if (line == null)
                    {
                        throw new Exception("ATOM block not found!");
                    }
                }
                V3ReadAtomBlock();

                // find start of BOND block
                line = NextLine();
                while (!line.Equals("BEGIN BOND"))
                {
                    line = NextLine();
                    if (line == null)
                    {
                        throw new Exception("BOND block not found!");
                    }
                }
                V3ReadBondBlock();
            }
            else
            {
                throw new Exception("unknown Version:" + Version);
            }

            // figure out which atoms are chiral, and add atom parity information
            // List<Atom> chiralAtoms = molecule.getChiralAtoms();
            if (molecule.Atoms.Any())
            {
                return molecule;
            }
            else
            {
                return null;
            }
        }

        /**
         * writes the header of an MDLMolFile
         *
         * @param writer
         */

        private void WriteHeader()
        {
            // ToDo: Create a title Attribute for the molecule !!!
            //if (molecule.Title() != null)
            //{
            //    _tw.WriteLine(molecule.Title());
            //}
            //else {
            //    _tw.WriteLine(S_EMPTY);
            //}
            _tw.WriteLine(S_EMPTY);

            String s = "";
            s += "  "; // users initals
            s += "Chem4Wrd";

            // date and time

            DateTime rightNow = DateTime.Now;
            // MMDDYYHHmmdd
            s += rightNow.ToString("MMddyyHHmm");

            // TODO think about working out 2D or 3D tag
            //Atom atom = molecule.GetAtoms().ToList()[0];
            //if (atom.hasCoordinates(CoordinateType.TWOD) &&
            //        (coordType.equals(CoordType.EITHER) || coordType.equals(CoordType.TWOD)))
            //{
            //    dimensionalCode = MDLTag.D2;
            //}
            //else if (atom.hasCoordinates(CoordinateType.CARTESIAN) &&
            //  (coordType.equals(CoordType.EITHER) || coordType.equals(CoordType.THREED)))
            //{
            //    dimensionalCode = MDLTag.D3;
            //}
            dimensionalCode = MDLTag.D2;
            s += (dimensionalCode);

            _tw.WriteLine(s);

            // blank line (reserved for human-readable comments)
            _tw.WriteLine(S_EMPTY);

            // counts line (V2000 only)
            if (Version.Equals(V2000))
            {
                s = "";
                // number of atoms & number of bonds
                s += OutputMDLInt(_model.Atoms.Count);
                s += OutputMDLInt(_model.Bonds.Count);
                // these params are obsolete (apart from Version tag)
                s += "  0  0  0  0  0  0  0  0999 V2000";
                _tw.WriteLine(s);
            }
            else if (Version.Equals(V3000))
            {
                _tw.WriteLine("  0  0  0     0  0            999 V3000");
            }
        }

        /**
         * Writes out the atom list in the MDLMol file format
         *
         * @param writer the writer to write data to
         */

        private void WriteAtoms()
        {
            int i = 0;
            double x = 0;
            double y = 0;
            double z = 0;

            ICollection<Atom> atoms = _model.Atoms;

            foreach (Atom atom in atoms)
            {
                x = atom.Position.X;
                y = 0 - atom.Position.Y;

                String s = "";
                s += (OutputMDLFloat(x));
                s += (OutputMDLFloat(y));
                s += (OutputMDLFloat(z));
                // write single whitespace after coords
                s += (S_SPACE);

                String elType = atom.Element.Symbol;
                if (!ElementExists(elType))
                {
                    //LOG.warn(elType + " is not a valid element atomicSymbol");
                }
                s += (elType + "   ").Substring(0, 3);

                // MDLMol has 12 fields - only some are filled here
                // field 1 - integer difference from main isotope
                String isoString = " 0";
                double isotope = 0.0;
                if (atom.IsotopeNumber != null)
                {
                    isotope = atom.IsotopeNumber.Value;
                }

                if (isotope > 0.0001)
                {
                    if (ElementExists(elType))
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        throw new Exception("cannot find weight of " + elType
                                + " to work out isotopic difference");
                    }
                }

                s += (isoString);

                // field 2 charge
                /*
                    0 = uncharged or value other than these, 1 = +3, 2 = +2, 3 = +1,
                    4 = doublet radical, 5 = -1, 6 = -2, 7 = -3
                 */
                String chString = "  0";
                if (atom.FormalCharge != null)
                {
                    int fCharge = atom.FormalCharge.Value;
                    int mdlCharge = 0;
                    if (fCharge == 0)
                    {
                        mdlCharge = 0;
                    }
                    else if (fCharge > 0 && fCharge < 5)
                    {
                        mdlCharge = 4 - fCharge;
                    }
                    else if (fCharge > -4)
                    {
                        mdlCharge = 4 - fCharge;
                    }
                    chString = OutputMDLInt(mdlCharge);
                }

                /*
                 * if (atom.getSpinMultiplicityAttribute() != null) { int spin =
                 * atom.getSpinMultiplicity(); if (spin == 1) { // there are issues
                 * here, '4' doesnt have a consistent meaning chString = " 4"; } }
                 */

                s += (chString);

                // field 3 is atom parity
                String parity = "  0";
                s += (parity);

                // field 4 hydrogen count - appears to be nhyd+1
                String nhString = "  0";
                //if (atom.getHydrogenCountAttribute() != null)
                //{
                //    int nhyd = atom.getHydrogenCount();
                //    nhString = "  " + (nhyd + 1);
                //}
                s += (nhString);

                // field 5 stereocare
                s += ("  0");

                // field 6 valency/oxidation state
                String vState = "  0";
                s += (vState);

                // fields 7 onwards have not been implemented
                for (int j = 6; j < 12; j++)
                {
                    s += ("  0");
                }

                _tw.WriteLine(s);

                // remember serial number
                numberByAtom.Add(atom, ++i);
            }
        }

        /**
         * writes out the bonds list in MDLMolFile format
         *
         * @param writer
         */

        private void WriteBonds()
        {
            IEnumerable<Bond> bonds = _model.Bonds;

            foreach (Bond bond in bonds)
            {
                int atomNumber1 = numberByAtom[bond.StartAtom];
                int atomNumber2 = numberByAtom[bond.EndAtom];

                String s = "";
                s += (OutputMDLInt(atomNumber1));
                s += (OutputMDLInt(atomNumber2));

                // field 3 order
                s += ("  " + MolBondOrder(bond.Order));

                // field 4 stereo
                BondStereo bs = bond.Stereo;
                if (bs == BondStereo.None)
                {
                    s += ("  0");
                }
                else
                {
                    s += ("  " + MolBondStereo(bs));
                }

                // field 5 - not used
                s += ("  0");

                // field 6 - bond topology (0 = Either, 1 = Ring, 2 = Chain)
                s += ("  0");

                // field 7 - reacting center status [Reaction & Query]
                s += ("  0");
                _tw.WriteLine(s);
            }
        }

        /**
         * writes footer. writes charge, isotope and spin multiplicity information
         *
         * @param writer
         */

        private void WriteFooter()
        {
            String atomAlias = "";

            // run through atoms and collect alias information (first label if present)
            ICollection<Atom> atoms = _model.Atoms;

            foreach (Atom atom in atoms)
            {
                int atomNumber = numberByAtom[atom];

                //CMLElements<CMLLabel> labelElements = atom.getLabelElements();
                //for (CMLLabel alias : labelElements)
                //{
                //    _tw.WriteLine(MDLTag.A__.tag + OutputMDLInt(atomNumber));
                //    _tw.WriteLine(alias.getCMLValue());
                //}
            }

            String s = "";
            s += (atomAlias);

            s += (WritePropertyLine(MDLTag.M_CHG));
            s += (WritePropertyLine(MDLTag.M_RAD));
            s += (WritePropertyLine(MDLTag.M_ISO));

            // TODO write SGroups

            _tw.WriteLine(s + MDLTag.M_END);
            _tw.WriteLine(MDLTag.SDF_END);
        }

        /**
         * writes out a property line for inclusion in the properties block of an
         * MDLMolFile, can write out MDLTag.M_CHG, MDLTag.M_ISO and MDLTag.M_RAD
         * lines
         *
         * obeys rules on a maximum of 8 values per line
         *
         * @param propertyType -
         *            the type of property line to write out
         * @return the property line
         */

        private String WritePropertyLine(string propertyType)
        {
            List<int> values = new List<int>();
            List<int> atomNumbers = new List<int>();

            ICollection<Atom> atoms = _model.Atoms;

            foreach (Atom atom in atoms)
            {
                int atomNumber = numberByAtom[atom];

                int fCharge = 0;
                if (atom.FormalCharge != null)
                {
                    fCharge = atom.FormalCharge.Value;
                }
                double isotope = 0.0;
                if (atom.IsotopeNumber != null)
                {
                    isotope = atom.IsotopeNumber.Value;
                }
                int spin = 0;
                if (atom.SpinMultiplicity != null)
                {
                    spin = atom.SpinMultiplicity.Value;
                }

                if (propertyType == MDLTag.M_CHG & fCharge != 0)
                {
                    values.Add(atom.FormalCharge.Value);
                    atomNumbers.Add(atomNumber);
                }
                else if (propertyType == MDLTag.M_ISO & isotope > 0.0001)
                {
                    values.Add(((int)atom.IsotopeNumber.Value));
                    atomNumbers.Add(atomNumber);
                }
                else if (propertyType == MDLTag.M_RAD & spin > 0.0001)
                {
                    values.Add(atom.SpinMultiplicity.Value);
                    atomNumbers.Add(atomNumber);
                }
            }

            int count = atomNumbers.Count;
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < (float)count / 8f; i++)
            {
                int thisLineCount = (count - i * 8) > 8 ? 8 : count - i * 8;
                output.Append(propertyType + "  " + thisLineCount);
                for (int j = 0; j < thisLineCount; j++)
                {
                    String atomNumber = OutputMDLInt(atomNumbers[j + i * 8]);
                    String value = OutputMDLInt(values[j + i * 8]);
                    output.Append(S_SPACE + atomNumber + S_SPACE + value);
                }
            }
            String ss = output.ToString().TrimEnd();
            if (ss.Length > 0)
            {
                ss += Environment.NewLine;
            }
            return ss;
        }

        /**
         * writes a counts line for inculsion in a V3000 MDLMolFile
         *
         * @return the counts line
         */

        private String V3WriteCountsLine()
        {
            String counts = MDLTag.M_V30 + "COUNTS";
            counts += S_SPACE + _model.Atoms.Count;
            counts += S_SPACE + _model.Bonds.Count;
            counts += S_SPACE + 0; // number of Sgroups
            counts += S_SPACE + 0; // number of 3D constraints
            counts += S_SPACE + 0; // 1 if molecule is pure, 0 for mix (or just not
                                   // chiral)
            return counts;
        }

        /**
         * writes out the atoms block in MDLMol V3000 format
         *
         * @param theWriter
         */

        private void V3WriteAtomBlock()
        {
            int i = 0;
            double x = 0;
            double y = 0;
            double z = 0;

            ICollection<Atom> atoms = _model.Atoms;

            foreach (Atom atom in atoms)
            {
                String elType = atom.Element.Symbol;

                if (!ElementExists(elType))
                {
                    throw new Exception(elType + " is not a valid element atomicSymbol");
                }

                // any 2D coordinates take precedence
                //  if (atom.getX2Attribute() != null && atom.getY2Attribute() != null)
                //  {
                //      x = atom.getX2();
                //      y = atom.getY2();
                //  }
                //  else if ( // else 3D coordinates
                //  atom.getX3Attribute() != null && atom.getY3Attribute() != null
                //        && atom.getZ3Attribute() != null)
                //  {
                //      x = atom.getX3();
                //      y = atom.getY3();
                //      z = atom.getZ3();
                //  }
                x = atom.Position.X;
                y = atom.Position.Y;

                String s = "";
                s += (MDLTag.M_V30 + (++i) + S_SPACE + elType + S_SPACE + x
                        + S_SPACE + y);

                // prevent 0.0 for 0 z
                if (z > 0.0001)
                {
                    s += (S_SPACE + z);
                }
                else
                {
                    s += (S_SPACE + 0);
                }

                // atom-atom mapping
                s += (S_SPACE + "0");

                if (atom.IsotopeNumber != null)
                {
                    Double isotope = atom.IsotopeNumber.Value;
                    s += (S_SPACE + MDLTag.V3_ISOTOPE + S_EQUALS
                            + isotope);
                }

                if (atom.FormalCharge != null)
                {
                    s += (S_SPACE + MDLTag.V3_CHARGE + S_EQUALS
                            + atom.FormalCharge.Value);
                }

                if (atom.SpinMultiplicity != null)
                {
                    s += (S_SPACE + MDLTag.V3_RADICAL + S_EQUALS
                            + atom.SpinMultiplicity.Value);
                }

                //if (atom.getHydrogenCountAttribute() != null)
                //{
                //    s += (S_SPACE + MDLTag.V3_HCOUNT.tag + S_EQUALS
                //            + atom.getHydrogenCount());
                //}

                _tw.WriteLine(s);

                // remember serial number
                numberByAtom.Add(atom, i);
            }
        }

        /**
         * writes out the bonds block in MDLMol V3000 format
         *
         * @param theWriter
         */

        private void V3WriteBondBlock()
        {
            int i = 0;
            IEnumerable<Bond> bonds = _model.Bonds;

            foreach (Bond bond in bonds)
            {
                int atomNumber1 = numberByAtom[bond.GetAtoms().ToArray()[0]];
                int atomNumber2 = numberByAtom[bond.GetAtoms().ToArray()[1]];
                double bondOrder = bond.OrderValue??0;

                String s = "";
                s += (MDLTag.M_V30 + (++i));
                s += (S_SPACE + bondOrder);
                s += (S_SPACE + atomNumber1);
                s += (S_SPACE + atomNumber2);

                var  bs = bond.Stereo;
                if (bs != BondStereo.None)
                {
                    s += S_SPACE + MDLTag.V3_STEREO + S_EQUALS
                            + V3MolBondStereo(bs);
                }

                _tw.WriteLine(s);
            }
        }

        /**
         * Outputs a given Molecule as an MDL MolFile, the Version of MolFile
         * written can be set using SetVersion(), else the default verison, V2000,
         * is written.
         *
         * @param writer the Writer to output the MolFile to
         * @param mol the Molecule to parse and output
         */

        public void WriteMOL(TextWriter writer, Model model)
        {
            _model = model;
            nline = 0;
            _tw = writer;

            int atomCount = _model.Atoms.Count;

            if (atomCount > 999 && Version == V3000)
            {
                throw new Exception("Too many atoms for MDLMolfile: "
                        + atomCount);
            }
            else if (atomCount > 255)
            {
                //LOG.warn(atomCount
                //        + " may be too many atoms for some applications");
            }
            else if (atomCount > 0)
            {
                if (Version.Equals(V2000))
                {
                    WriteHeader();
                    WriteAtoms();
                    WriteBonds();
                    WriteFooter();
                }
                else if (Version.Equals(V3000))
                {
                    WriteHeader();
                    String s = "";
                    s += (MDLTag.M_V30 + "BEGIN CTAB");
                    _tw.WriteLine(V3WriteCountsLine() + MDLTag.M_V30 + "BEGIN ATOM");
                    V3WriteAtomBlock();
                    _tw.WriteLine(MDLTag.M_V30 + "END ATOM");
                    _tw.WriteLine(MDLTag.M_V30 + "BEGIN BOND");
                    V3WriteBondBlock();
                    _tw.WriteLine(MDLTag.M_V30 + "END BOND");
                    _tw.WriteLine(MDLTag.M_V30 + "END CTAB");

                    _tw.WriteLine(MDLTag.M_END);
                }
                else
                {
                    throw new Exception("unknown MDLMol Version!");
                }
            }
        }

        public string Description
        {
            get { return "MDL File"; }
        }
        public string[] Extensions {
            get { return new string[] {"*.MOL", "*.SDF"}; }
        }
        public string Export(Model model)
        {
            StringWriter sw = new StringWriter();
            
            WriteMOL(sw, model);
            

            return sw.ToString();
        }

        public Model Import(object data)
        {

            //lines = ((string) data).Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();

            StringReader sr = new StringReader((string)data);
            Model model = new Model();
            Molecule mol;
            mol = ReadMol(sr);

            model.Molecules.Add(mol);
           
            model.RefreshMolecules();
            return model;

        }

        public bool CanImport => true;
        public bool CanExport => true;
    }
}
