// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Chem4Word.Model
{
    public static class FunctionalGroups
    {
        /// <summary>
        /// keys represent text as a user might type in a superatom,
        /// actual values control how they are rendered
        /// </summary>
        ///
        ///

        public static bool TryParse(string desc, out FunctionalGroup fg)
        {
            try
            {
                fg = GetByName[desc];
                return true;
            }
            catch (Exception)
            {
                fg = null;
                return false;
            }
        }

        public static Dictionary<string, FunctionalGroup> GetByName
        {
            get
            {
                Dictionary<string, FunctionalGroup> shortcutList = new Dictionary<string, FunctionalGroup>
                {
                    //all the R residues are set to arbitrary multiplicity just so they appear subscripted
                    //their atomic weight is zero anyhow
                    //multiple dictionary keys may refer to the same functional group
                    //simply to allow synonyms
                    //when displayed, numbers in the names are automatically subscripted

                    //note that ACME will automatically render a group as inverted if appropriate
                    //so that CH3 -> H3C
                    ["R1"] =
                        new FunctionalGroup("R1",
                            multiplicities: new List<Multiplicity> { new Multiplicity(Globals.PeriodicTable.R, 1) }, atwt: 0.0d),
                    ["R2"] =
                        new FunctionalGroup("R2",
                            multiplicities: new List<Multiplicity> { new Multiplicity(Globals.PeriodicTable.R, 2) }, atwt: 0.0d),
                    ["R3"] =
                        new FunctionalGroup("R3",
                            multiplicities: new List<Multiplicity> { new Multiplicity(Globals.PeriodicTable.R, 3) }, atwt: 0.0d),
                    ["R4"] =
                        new FunctionalGroup("R4",
                            multiplicities: new List<Multiplicity> { new Multiplicity(Globals.PeriodicTable.R, 4) }, atwt: 0.0d),

                    //generic halogen
                    ["X"] = new FunctionalGroup("X", atwt: 0.0d),
                    //typical shortcuts
                    ["CH3"] =
                        new FunctionalGroup("CH3",
                            multiplicities: new List<Multiplicity>
                            {
                                new Multiplicity(Globals.PeriodicTable.C, 1),
                                new Multiplicity(Globals.PeriodicTable.H, 3)
                            }),
                    ["C2H5"] =
                        new FunctionalGroup("C2H5",
                            multiplicities: new List<Multiplicity>
                            {
                                new Multiplicity(Globals.PeriodicTable.C, 2),
                                new Multiplicity(Globals.PeriodicTable.H, 5)
                            }),
                    ["Me"] = new FunctionalGroup("Me",
                             multiplicities: new List<Multiplicity>()
                             {
                                 new Multiplicity(Globals.PeriodicTable.C, 1),
                                 new Multiplicity(Globals.PeriodicTable.H, 3)
                             }, showasabbrev: true),
                    ["Et"] = new FunctionalGroup("Et",
                             multiplicities: new List<Multiplicity>()
                             {
                                 new Multiplicity(Globals.PeriodicTable.C, 2),
                                 new Multiplicity(Globals.PeriodicTable.H, 5)
                             }, showasabbrev: true),
                    ["Pr"] = new FunctionalGroup("Pr",
                             multiplicities: new List<Multiplicity>()
                             {
                                 new Multiplicity(Globals.PeriodicTable.C, 3),
                                 new Multiplicity(Globals.PeriodicTable.H, 7)
                             }, showasabbrev: true),
                    ["i-Pr"] = new FunctionalGroup("i-Pr",
                             multiplicities: new List<Multiplicity>()
                             {
                                 new Multiplicity(Globals.PeriodicTable.C, 3),
                                 new Multiplicity(Globals.PeriodicTable.H, 7)
                             }, showasabbrev: true),
                    ["iPr"] = new FunctionalGroup("i-Pr",
                             multiplicities: new List<Multiplicity>()
                             {
                                 new Multiplicity(Globals.PeriodicTable.C, 3),
                                 new Multiplicity(Globals.PeriodicTable.H, 7)
                             }, showasabbrev: true),
                    ["n-Bu"] = new FunctionalGroup("n-Bu",
                             multiplicities: new List<Multiplicity>()
                             {
                                 new Multiplicity(Globals.PeriodicTable.C, 4),
                                 new Multiplicity(Globals.PeriodicTable.H, 9)
                             }, showasabbrev: true),
                    ["nBu"] = new FunctionalGroup("n-Bu",
                             multiplicities: new List<Multiplicity>()
                             {
                                 new Multiplicity(Globals.PeriodicTable.C, 4),
                                 new Multiplicity(Globals.PeriodicTable.H, 9)
                             }, showasabbrev: true),
                    ["t-Bu"] = new FunctionalGroup("t-Bu",
                             multiplicities: new List<Multiplicity>()
                             {
                                 new Multiplicity(Globals.PeriodicTable.C, 4),
                                 new Multiplicity(Globals.PeriodicTable.H, 9)
                             }, showasabbrev: true),
                    ["tBu"] = new FunctionalGroup("t-Bu",
                             multiplicities: new List<Multiplicity>()
                             {
                                 new Multiplicity(Globals.PeriodicTable.C, 4),
                                 new Multiplicity(Globals.PeriodicTable.H, 9)
                             }, showasabbrev: true),
                    ["Ph"] =
                        new FunctionalGroup("Ph", multiplicities: new List<Multiplicity>()
                        {
                            new Multiplicity(Globals.PeriodicTable.C, 6),
                            new Multiplicity(Globals.PeriodicTable.H, 5)
                        }, showasabbrev: true),
                    ["CF3"] =
                        new FunctionalGroup("CF3", multiplicities: new List<Multiplicity>()
                        {
                            new Multiplicity(Globals.PeriodicTable.C, 1),
                            new Multiplicity(Globals.PeriodicTable.F, 3)
                        }),
                    ["CCl3"] =
                        new FunctionalGroup("CCl3", multiplicities: new List<Multiplicity>()
                        {
                            new Multiplicity(Globals.PeriodicTable.C, 1),
                            new Multiplicity(Globals.PeriodicTable.Cl, 3)
                        }),
                    ["C2F5"] =
                        new FunctionalGroup("C2F5", multiplicities: new List<Multiplicity>()
                        {
                            new Multiplicity(Globals.PeriodicTable.C, 2),
                            new Multiplicity(Globals.PeriodicTable.F, 5)
                        }),
                    ["TMS"] =
                    new FunctionalGroup("TMS", multiplicities: new List<Multiplicity>()
                    {
                        new Multiplicity(Globals.PeriodicTable.C, 3),
                        new Multiplicity(Globals.PeriodicTable.Si, 1),
                        new Multiplicity(Globals.PeriodicTable.H, 9)
                    }, showasabbrev: true),
                    ["COOH"] =
                    new FunctionalGroup("CO2H", multiplicities: new List<Multiplicity>()
                    {
                        new Multiplicity(Globals.PeriodicTable.C, 1),
                        new Multiplicity(Globals.PeriodicTable.O, 2),
                        new Multiplicity(Globals.PeriodicTable.H, 1)
                    }),
                    ["CO2H"] =
                    new FunctionalGroup("COOH", multiplicities: new List<Multiplicity>()
                    {
                        new Multiplicity(Globals.PeriodicTable.C, 1),
                        new Multiplicity(Globals.PeriodicTable.O, 2),
                        new Multiplicity(Globals.PeriodicTable.H, 1)
                    }),
                    ["NO2"] =
                    new FunctionalGroup("NO2", multiplicities: new List<Multiplicity>()
                    {
                        new Multiplicity(Globals.PeriodicTable.N, 1),
                        new Multiplicity(Globals.PeriodicTable.O, 2),
                    }),
                    ["NH2"] =
                    new FunctionalGroup("NH2", multiplicities: new List<Multiplicity>()
                    {
                        new Multiplicity(Globals.PeriodicTable.N, 1),
                        new Multiplicity(Globals.PeriodicTable.H, 2),
                    })
                };
                return shortcutList;
            }
        }

        //list of valid shortcuts for testing input
        public static string ValidShortCuts = "^(" +
            GetByName.Select(e => e.Key).Aggregate((start, next) => start + "|" + next) + ")$";

        //and the regex to use it
        public static Regex ShortcutParser = new Regex(ValidShortCuts);

        //list of valid elements (followed by subscripts) for testing input
        public static Regex NameParser = new Regex($"^(?<element>{Globals.PeriodicTable.ValidElements}+[0-9]*)+\\s*$");

        //checks to see whether a typed in expression matches a given shortcut
        public static bool IsValid(string expr)
        {
            return NameParser.IsMatch(expr) || ShortcutParser.IsMatch(expr);
        }
    }
}