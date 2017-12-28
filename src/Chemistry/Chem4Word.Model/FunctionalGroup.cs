// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace Chem4Word.Model
{
    /// <summary>
    /// Identifies occruences of atoms in FGs
    /// </summary>
    public struct Multiplicity
    {
        public Element Element;
        public int Count;

        public Multiplicity(Element e, int c)
        {
            Element = e;
            Count = c;
        }
    }

    public class FunctionalGroup : ElementBase
    {
        private string _symbol = "";
        private double? _atomicWeight;

        /// <summary>
        /// Generates a new functional group to use for a superatom
        /// </summary>
        /// <param name="symbol">Actual symbol used for the atom (mandatory)</param>
        /// <param name="multiplicities"> Key-Value list of elements and how many</param>
        /// <param name="atwt">atomic weight of the atom</param>
        /// <param name="showasabbrev">whether the element is rendered as its symbol or constructed from its multiplicities</param>
        public FunctionalGroup(string symbol,
            List<Multiplicity> multiplicities = null,
            double atwt = 0.0, bool showasabbrev = false)
        {
            Symbol = symbol;
            AtomicWeight = atwt;
            Multiplicities = multiplicities;
            Abbreviate = showasabbrev;
        }

        public bool Abbreviate { get; set; }

        public override double AtomicWeight
        {
            get
            {
                if (_atomicWeight == null)
                {
                    double atwt = 0.0d;
                    if (Multiplicities != null)
                    {
                        //add up the atoms' atomicv weights times their multiplicity
                        atwt =
                            Multiplicities.Select(x => x.Element.AtomicWeight * x.Count)
                                .Aggregate((source, value) => source + value);
                    }
                    return atwt;
                }
                else
                    return _atomicWeight.Value;
            }
            set { _atomicWeight = value; }
        }

        /// <summary>
        /// Symbol refers to the 'Ph', 'Bz' etc
        ///
        /// Symbol can also be of the form CH3, CF3, C2H5 etc
        /// </summary>
        public override string Symbol
        {
            get
            {
                return _symbol;
            }

            set { _symbol = value; }
        }

        public override string Name { get; set; }

        /// <summary>
        /// Defines the constituents of the superatom
        /// The 'pivot' atom that bonds to the fragment appears FIRST in the list
        /// so CH3 can appear as H3C
        ///
        /// Ths property can be null, which means that the symbol gets rendered
        /// </summary>
        public List<Multiplicity> Multiplicities { get; set; }

        /*
        /// <summary>
        /// Renders the superelement to a textblock
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="flipped"></param>
        public void Render(TextBlock tb, bool flipped)
        {
            //if we are NOT showing it as an abbrevation then show what the user typed in
            if (!Abbreviate)
            {
                if (!flipped)
                {
                    foreach (Multiplicity m in Multiplicities)
                    {
                        AddElementRun(tb, m);
                    }
                }
                else
                {
                    for (int i = Multiplicities.Count - 1; i >= 0; i--)
                    {
                        var m = Multiplicities.ElementAt(i);
                        AddElementRun(tb, m);
                    }
                }
            }
            else
            {
                tb.Inlines.Add(Symbol);
            }
        }

        private void AddElementRun(TextBlock tb, Multiplicity m)
        {
            tb.Inlines.Add(m.Element.Symbol);

            if (m.Count > 1)
            {
                Run subscript = new Run();
                subscript.Text = m.Count.ToString();
                SetSubscript(subscript);
                tb.Inlines.Add(subscript);
            }
        }

        private static void SetSubscript(Run subscript)
        {
            Typography.SetVariants(subscript, FontVariants.Subscript);
        }
        */
    }
}