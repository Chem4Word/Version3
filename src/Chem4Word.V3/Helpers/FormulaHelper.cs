// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Chem4Word.Helpers
{
    public class FormulaPart
    {
        public string Atom { get; set; }
        public int Count { get; set; }

        public FormulaPart(string n, int c)
        {
            Atom = n;
            Count = c;
        }
    }

    public static class FormulaHelper
    {
        public static List<FormulaPart> Parts(string input)
        {
            // Input is any of
            //  "2 C 6 H 6 . C 7 H 7"
            //  "C 6 H 6"
            //  "C7H7"
            //  "C7H6N"

            List<FormulaPart> parts = new List<FormulaPart>();
            if (!string.IsNullOrEmpty(input))
            {
                // Remove all spaces
                string temp = input.Replace(" ", "");
                Debug.WriteLine($"{input} --> {temp}");

                string[] formulae = temp.Split('.');
                for (int i = 0; i < formulae.Length; i++)
                {
                    // http://stackoverflow.com/questions/11232801/regex-split-numbers-and-letter-groups-without-spaces
                    // Code below is based on answer "use 'look around' in split regex"
                    string[] xx = Regex.Split(formulae[i], @"(?<=\d)(?=\D)|(?=\d)(?<=\D)");
                    int j = 0;
                    while (j < xx.Length)
                    {
                        int x;
                        if (int.TryParse(xx[j], out x))
                        {
                            // Multiplier
                            parts.Add(new FormulaPart($"{x} ", 0));
                            j++;
                        }
                        else
                        {
                            if (j <= xx.Length - 2)
                            {
                                // Atom and Count
                                parts.Add(new FormulaPart(xx[j], int.Parse(xx[j + 1])));
                            }
                            else
                            {
                                // Atom and Implicit Count of 1
                                parts.Add(new FormulaPart(xx[j], 1));
                            }
                            j += 2;
                        }
                    }

                    // Add Seperator
                    if (i < formulae.Length - 1)
                    {
                        // Using Bullet character <Alt>0183
                        parts.Add(new FormulaPart(" · ", 0));
                    }
                }
            }
            return parts;
        }
    }
}