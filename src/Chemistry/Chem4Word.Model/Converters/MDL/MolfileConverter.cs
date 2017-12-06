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
using System.Text.RegularExpressions;
using System.Windows;

namespace Chem4Word.Model.Converters
{
    public class MolfileConverter : IConverter
    {
        public static Regex CountsLine = new Regex(@"^(?<atoms>[ 0-9]{3})(?<bonds>[ 0-9]{3})([ 0-9]{3})([ 0-9]{3})([ 0-9]{3})([ 0-9]{3})([ 0-9]{3})([ 0-9]{3})([ 0-9]{3})([ 0-9]{3})([ 0-9]{3}).*$");
        public static Regex AtomLine = new Regex(@"^(?<x>[0-9 \-\.]{10})(?<y>[0-9 \-\.]{10})(?<z>[0-9 \-\.]{10})(?<element>[ A-Za-z]{3})(?<isotope>[ \-0-9]{3})?(?<charge>[ \-0-9]{3})?(?<stereo>[ 0-9]{3})?(?<hydrogens>[ 0-9]{3})?.*$");
        public static Regex BondLine = new Regex(@"^(?<atom1>[ 0-9]{3})(?<atom2>[ 0-9]{3})(?<order>[ 0-9]{3})(?<stereo>[ 0-9]{3})(?<topochain>[ 0-9]{3})?(?<toporing>[ 0-9]{3})?.*$");
        public static Regex EndLine = new Regex(@"M  END");

        public static int doubletRadicalConvention = 4;

        public string Description
        {
            get { return "MDL Molfile"; }
        }

        public string[] Extensions
        {
            get { return new[] { "*.mol" }; }
        }

        public string Export(Chem4Word.Model.Model model)
        {
            String segment = "";
            int atomCount = 0;
            if (model.Molecules.Select(m => m.Bonds.Count).Max() > 999)
            {
                throw new ArgumentOutOfRangeException("Model's bond count exceeds CTAB format limits");
            }

            if (model.Molecules.Select(m => m.Atoms.Count).Max() > 999)
            {
                throw new ArgumentOutOfRangeException("Model's atom count exceeds CTAB format limits");
            }

            int startAtom = 0, endAtom = 0;

            System.Text.StringBuilder SDFileContents = new StringBuilder();
            //However many molecues there are, shove them all in the output
            //Counts line
            //Atom count padded with spaces
            foreach (Molecule molecule in model.Molecules)
            {
                SDFileContents.Append(ExportMolecule(molecule, atomCount));
            }
            return SDFileContents.ToString();
        }

        private static string ExportMolecule(Molecule mol, int atomCount)
        {
            Dictionary<string, int> atomList = new Dictionary<string, int>();
            System.Text.StringBuilder outputData = new StringBuilder();
            string segment;
            int startAtom;
            int endAtom;

            // Header
            outputData.AppendLine("");
            outputData.AppendLine($"  Chem4Word{DateTime.Now.ToString("MMddyyHHmm")}2D");
            outputData.AppendLine("");

            // Counts
            outputData.AppendLine($"{mol.Atoms.Count.ToString("000")}{mol.Bonds.Count.ToString("000")}  0  0  0  0            000 V2000 Chem4Word");

            //Find the maximum and minimum Y coordinates to enable us to invert the Y axis
            double minY = 0, maxY = 0;
            //bool firstY = true;

            minY = mol.Atoms.Min(a => a.Position.Y);
            maxY = mol.Atoms.Max(a => a.Position.Y);

            //Sum the minumum and maximum Y coordinates and later subtract each Y coordinate from this value
            double yReversal = minY + maxY;

            //Atoms line
            foreach (Atom a in mol.Atoms)
            {
                segment = "";
                //Maintain a lookup for a sequence of numbered atom from their string id's
                atomCount++;
                atomList.Add(a.Id, atomCount);
                //Coordinates and 1 space before the element
                segment = string.Format("{0,10:F4}{1,10:F4}{2,10:F4} ", a.Position.X, yReversal - a.Position.Y, 0.0);
                //Element symbol
                segment += a.Element.Symbol.PadRight(2);
                //Isotope, charge, stereo, hydrogens, two zeros
                //charge munges formal charge and doublet radical properties
                int charge = ChargeFromModel(a.FormalCharge ?? 0, a.DoubletRadical);

                //Ignore stereo and ignore hydrogens for now so that's 4 zeros, then add carriage return and line feed
                segment += string.Format("{0,3}{1,3}{2,3}{3,3}{4,3}{5,3}", a.IsotopeNumber, charge, 0, 0, 0, 0);
                //Add the atom's line to the output
                outputData.AppendLine(segment);
            }

            //Bonds line
            foreach (Bond b in mol.Bonds)
            {
                startAtom = atomList[b.StartAtom.Id];
                endAtom = atomList[b.EndAtom.Id];
                //Atom 1 and Atom 2, bond order, stereo, 5 spaces then ignore topochain and toporing, then add CR and LF
                outputData.AppendLine(string.Format("{0,3}{1,3}{2,3}{3,3}     {4,3}{5,3}", startAtom, endAtom, b.OrderValue,
                    BondStereoFromModel(b.Stereo), 0, 0));
            }

            //End line
            outputData.AppendLine("M  END");
            return outputData.ToString();
        }

        public Chem4Word.Model.Model Import(object data)
        {
            string line;
            int atomsUnread = 0, bondsUnread = 0;
            bool countsRead = false, endRead = false;
            double minY = 0, maxY = 0;
            bool firstY = true;

            Chem4Word.Model.Model newModel = new Chem4Word.Model.Model();
            Chem4Word.Model.Molecule newMolecule = new Chem4Word.Model.Molecule();
            Chem4Word.Model.PeriodicTable pt = new Chem4Word.Model.PeriodicTable();
            //Element ce = new Element();

            Dictionary<string, Chem4Word.Model.Atom> atomsDict = new Dictionary<string, Chem4Word.Model.Atom>();
            Dictionary<string, Chem4Word.Model.Bond> bondsDict = new Dictionary<string, Chem4Word.Model.Bond>();
            int atomNumber = 0, bondNumber = 0;
            int lineNumber = 0;
            using (StringReader reader = new StringReader(data.ToString()))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (CountsLine.IsMatch(line))
                    {
                        if (lineNumber == 3)
                        {
                            Match m = CountsLine.Match(line);
                            var atomCount = m.Groups["atoms"].Captures[0];
                            var bondCount = m.Groups["bonds"].Captures[0];
                            int.TryParse(atomCount.ToString(), out atomsUnread);
                            int.TryParse(bondCount.ToString(), out bondsUnread);
                            countsRead = true;
                        }
                        else
                        {
                            throw new FileFormatException("Counts line misplaced");
                        }
                    }
                    else
                    {
                        if (countsRead)
                        {
                            if (AtomLine.IsMatch(line))
                            {
                                Match m = AtomLine.Match(line);
                                Capture xCoord, yCoord, zCoord, element, isotope = null, charge = null, stereo, hydrogens;
                                //read the atom details
                                xCoord = m.Groups["x"].Captures[0];
                                yCoord = m.Groups["y"].Captures[0];
                                zCoord = m.Groups["z"].Captures[0];           //not used
                                element = m.Groups["element"].Captures[0];
                                if (m.Groups["isotope"].Success)
                                {
                                    isotope = m.Groups["isotope"].Captures[0];
                                }
                                if (m.Groups["charge"].Success)
                                {
                                    charge = m.Groups["charge"].Captures[0];
                                }
                                if (m.Groups["stereo"].Success)
                                {
                                    stereo = m.Groups["stereo"].Captures[0];
                                }
                                if (m.Groups["hydrogens"].Success)
                                {
                                    hydrogens = m.Groups["hydrogens"].Captures[0];
                                }

                                //Create a new atom instance, set its properties and add it to our collection
                                Atom atom = new Atom();
                                atom.Position = Point.Parse($"{xCoord.Value},{yCoord.Value}");

                                //Keep track of the maximum and minimum Y coordinate values so we can invert them later
                                if (firstY)
                                {
                                    minY = atom.Position.Y;
                                    maxY = atom.Position.Y;
                                    firstY = false;
                                }
                                else
                                {
                                    if (minY > atom.Position.Y) { minY = atom.Position.Y; }
                                    if (maxY < atom.Position.Y) { maxY = atom.Position.Y; }
                                }

                                atom.Element = Globals.PeriodicTable.Elements[element.ToString().Trim()]; //need to make sure this is the element name

                                //Isotope offset. Do we need to add the most common or default isotope mass to get the right thing here?
                                //CD- leave the isotopes for now;
                                int tempIso = 0;
                                int.TryParse(isotope.ToString(), out tempIso);
                                if (tempIso > 0)
                                {
                                    atom.IsotopeNumber = tempIso;
                                }
                                //it is awkward that we cannot pass the property into TryParse....
                                //we could write a function but then we would need a different one for every type.

                                //Charge
                                int tempCharge = 0;
                                int.TryParse(charge.ToString(), out tempCharge);
                                //Translate the Molfile charge/radical enumeration to atom formal charge and doublet radical properties
                                atom.FormalCharge = FormalChargeFromMolfile(tempCharge);
                                atom.DoubletRadical = DoubletRadicalFromMolfile(tempCharge);

                                //Stereo - ignore for now??

                                //Hydrogens - ignore for now - saturation property mops this up in the model??

                                //Number the atom by incrementing the previous atom's number, add the atom to the dictionary
                                //keyed using the number converted to a string (why?)
                                atomNumber++;
                                atom.Id = atomNumber.ToString();
                                atomsDict.Add(atom.Id, atom);

                                //decrement the count of atom rows still to be read
                                atomsUnread--;
                            }
                            else
                            {
                                if (atomsUnread == 0)
                                {
                                    if (BondLine.IsMatch(line))
                                    {
                                        Match m = BondLine.Match(line);
                                        //read the bond details
                                        var atom1 = m.Groups["atom1"].Captures[0];
                                        var atom2 = m.Groups["atom2"].Captures[0];
                                        var order = m.Groups["order"].Captures[0];
                                        var stereo = m.Groups["stereo"].Captures[0];
                                        //var topochain = m.Groups["topochain"].Captures[0];
                                        //var toporing = m.Groups["toporing"].Captures[0];
                                        //parse into object values and add

                                        //Create a new bond instance, set its properties and add it to our collection
                                        Bond bond = new Bond();

                                        //Get start and end atoms, and bond order
                                        int atom1id = 0, atom2id = 0, orderId = 0;
                                        int.TryParse(atom1.ToString(), out atom1id);
                                        int.TryParse(atom2.ToString(), out atom2id);
                                        int.TryParse(order.ToString(), out orderId);

                                        bond.StartAtom = atomsDict[atom1id.ToString()];
                                        bond.EndAtom = atomsDict[atom2id.ToString()];
                                        bond.Order = Bond.OrderValueToOrder(orderId);

                                        //bond stereo
                                        int bondStereo = 0;
                                        int.TryParse(stereo.ToString(), out bondStereo);
                                        if (bond.Order == Bond.OrderSingle)
                                        {
                                            bond.Stereo = BondStereoFromMolfile(bondStereo);
                                        }
                                        /* we don't care about this we always use the coordinates I think??
                                        else
                                        {
                                            if (bond.Order == Enums.BondOrder.Double)
                                            {
                                                //0 is use x- y- z- coordinates from ataom block to determine Z or E
                                                //3 means either Z or E
                                            }
                                        }
                                        */

                                        //So... what about allenes??
                                        //CD - um...err....
                                        //topochain - ignore for now because the ring detection sorts it out??
                                        //CD- Yup
                                        //toporing - ignore for now because the ring detection sorts it out??
                                        //CD- Likewise
                                        //Number the bond by incrementing the previous bond's number, add the bond to the dictionary
                                        //keyed using the number converted to a string (why?)
                                        //CD - you could always try Dictionary<int, Chem4Word.Model.Atom> bondsDict and see what happens
                                        bondNumber++;
                                        bond.Id = bondNumber.ToString();
                                        bondsDict.Add(bond.Id, bond);

                                        //decrement the count of bond rows still to be read
                                        bondsUnread--;
                                    }

                                    //if bonds line
                                } //if all atoms read
                            } //if atoms line
                        } //if counts read line
                    } //if counts line
                    endRead = EndLine.IsMatch(line);
                    if (endRead)
                    {
                        lineNumber = 0;
                        break;
                    }

                    lineNumber++;
                } //while
                //Values of > 0 for the integer variable atomsUnread or bondsUnread would indicate not enough rows in the molfile.
                //Values of false for readCounts or endRead would also be bad now we have run out of string
            } //using

            //Check that everything read consistently
            if (!countsRead)
            {
                //What kind of exception do we want to raise, or error to log
                throw new System.FormatException("Counts line missing.");
            }

            if (!endRead)
            {
                //What kind of exception do we want to raise, or error to log
                throw new System.FormatException("End of Molfile line not found.");
            }

            if (atomsUnread != 0)
            {
                //What kind of exception do we want to raise, or error to log
                throw new System.FormatException("Number of atom lines inconsistent with counts line.");
            }

            if (bondsUnread != 0)
            {
                //What kind of exception do we want to raise, or error to log
                throw new System.FormatException("Number of bond lines inconsistent with counts line.");
            }
            if (atomsDict.Count == 0)
            {
                //Something is wrong if there are no atoms. Having no bonds is conceivably intentional.
                throw new System.FormatException("No atoms were read from the Molfile during import.");
            }

            //Sum the minumum and maximum Y coordinates and later subtract each atom's Y coordinate from this value
            double yReversal = minY + maxY;

            //Add all the atoms and bonds to a new molecule, doing this outside the loop that reads them from the file
            Molecule newMol = new Chem4Word.Model.Molecule();

            //Can we really just chuck them in like this or will it fail validation checks??
            foreach (Atom atom in atomsDict.Values)
            {
                //Set the atom's coordinates to the same existing x value, but y subtracted from the sum of the maximum and minumum y values
                atom.Position = Point.Parse($"{atom.Position.X},{yReversal - atom.Position.Y}");

                //Add the atom to the model
                newMol.Atoms.Add(atom);
            }
            foreach (Bond bond in bondsDict.Values)
            {
                newMol.Bonds.Add(bond);
            }
            //Add the molecule to the model
            newModel.Molecules.Add(newMol);

            //Make sure it is valid
            newModel.RebuildMolecules();

            return newModel;
        }

        public bool CanImport
        {
            get
            {
                return true;
            }
        }

        public bool CanExport
        {
            get
            {
                return true;
            }
        }

        private static Enums.BondStereo BondStereoFromMolfile(int molfileBondStereo)
        {
            ///Translates from a molfile convention bond stereochemistry number to the enumeration used in the model
            ///The opposite of BondStereoFromModel
            Chem4Word.Model.Enums.BondStereo modelStereo = 0;
            //MOLFILE: 0 is NOT stereo, 1 is UP,    6 is DOWN,  4 is EITHER
            //MODEL  : 0 is None,       1 is Wedge, 2 is Hatch, 3 is Indeterminate
            switch (molfileBondStereo)
            {
                case 0:
                case 1:
                    modelStereo = (Enums.BondStereo)molfileBondStereo;
                    break;

                case 4:
                    modelStereo = Enums.BondStereo.Indeterminate;
                    break;

                case 6:
                    modelStereo = Enums.BondStereo.Hatch;
                    break;

                default:
                    modelStereo = Enums.BondStereo.None;
                    break;
            }
            return modelStereo;
        }

        private static int BondStereoFromModel(Enums.BondStereo modelBondStereo)
        {
            ///Translates from the model enumerated bond stereochemistry to the molfile convention
            ///The opposite of BondStereoFromMolfile
            int molfileStereo = 0;
            //MODEL  : 0 is None,       1 is Wedge, 2 is Hatch, 3 is Indeterminate
            //MOLFILE: 0 is NOT stereo, 1 is UP,    6 is DOWN,  4 is EITHER
            switch (modelBondStereo)
            {
                case Enums.BondStereo.None:
                    molfileStereo = 0;
                    break;

                case Enums.BondStereo.Wedge:
                    molfileStereo = 1;
                    break;

                case Enums.BondStereo.Hatch:
                    molfileStereo = 6;
                    break;

                case Enums.BondStereo.Indeterminate:
                    molfileStereo = 4;
                    break;

                default:
                    molfileStereo = 4;
                    break;
            }
            return molfileStereo;
        }

        private static int FormalChargeFromMolfile(int chargemunge)
        {
            // ------------------------------------------------------------------------------------------------------------
            // 0 = uncharged or value other than these, 1 = +3, 2 = +2, 3 = +1, 4 = doublet radical, 5 = -1, 6 = -2, 7 = -3
            // ------------------------------------------------------------------------------------------------------------
            // Translates from the molfile enumerated atomic charge cum doublet radical to the model formal charge
            int formalCharge = 0;

            #region Translate from mdl to cml
            switch (chargemunge)
            {
                case 1:
                    formalCharge = 3;
                    break;
                case 2:
                    formalCharge = 2;
                    break;
                case 3:
                    formalCharge = 1;
                    break;
                case 4:
                    formalCharge = 0;
                    break;
                case 5:
                    formalCharge = -1;
                    break;
                case 6:
                    formalCharge = -2;
                    break;
                case 7:
                    formalCharge = -3;
                    break;
            }
            #endregion

            return formalCharge;
        }

        private static bool DoubletRadicalFromMolfile(int chargemunge)
        {
            //Translates from the molfile enumerated atomic charge cum doublet radical to the model doublet radical
            return (chargemunge == doubletRadicalConvention);
        }

        private static int ChargeFromModel(int charge, bool diradical)
        {
            // ------------------------------------------------------------------------------------------------------------
            // 0 = uncharged or value other than these, 1 = +3, 2 = +2, 3 = +1, 4 = doublet radical, 5 = -1, 6 = -2, 7 = -3
            // ------------------------------------------------------------------------------------------------------------
            //Translates from the model formal charge and doublet radical properties to the molfile representation
            //Add 4 to the real charge to get the Molfile charge enumeration,
            //unless the charge is zero AND it is not a doublet radical, in which case 0.
            int chargemunge = 0;

            if (diradical)
            {
                chargemunge = 4;
            }
            else
            {
                #region Translate from cml to mdl
                switch (charge)
                {
                    case 3:
                        chargemunge = 1;
                        break;
                    case 2:
                        chargemunge = 2;
                        break;
                    case 1:
                        chargemunge = 3;
                        break;
                    case -1:
                        chargemunge = 5;
                        break;
                    case -2:
                        chargemunge = 6;
                        break;
                    case -3:
                        chargemunge = 7;
                        break;
                    default:
                        chargemunge = 0;
                        break;
                }
                #endregion
            }

            return chargemunge;
        }
    }
}
