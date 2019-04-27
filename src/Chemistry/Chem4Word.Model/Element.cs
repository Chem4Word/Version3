// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;

namespace Chem4Word.Model
{
    public class Element : ElementBase
    {
        public Element()
        {
            StackTrace stackTrace = new StackTrace();
            MethodBase methodBase = stackTrace.GetFrame(1).GetMethod();

            if (methodBase.ReflectedType != null)
            {
                string callingClass = methodBase.ReflectedType.Name;

                if (!callingClass.Equals("PeriodicTable"))
                {
                    throw new NotSupportedException("You are not allowed to create Elements!");
                }
            }
        }

        public int AtomicNumber { get; set; }
        public int Group { get; set; }
        public int Row { get; set; }
        public bool AddHydrogens { get; set; }

        public override string Colour { get; set; }

        public double CovalentRadius { get; set; }
        public double VdWRadius { get; set; }
        public int Valency { get; set; }
        public int[] Valencies { get; set; }
        public int[] IsotopeMasses { get; set; }
        public int PTRow { get; set; }
        public int PTColumn { get; set; }
        public string PTElementType { get; set; }

        public override string ToString()
        {
            return Symbol;
        }

        #region Overides for equality checks

        // Override == and .Equals()
        // https://msdn.microsoft.com/en-gb/library/ms173147(v=vs.90).aspx

        public override int GetHashCode()
        {
            return Symbol.GetHashCode();
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Element return false.
            Element p = obj as Element;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the Symbol fields match:
            return Symbol == p.Symbol;
        }

        public bool Equals(Element p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the Symbol fields match:
            return Symbol == p.Symbol;
        }

        public static bool operator ==(Element a, Element b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Symbol == b.Symbol;
        }

        public static bool operator !=(Element a, Element b)
        {
            return !(a == b);
        }

        #endregion Overides for equality checks
    }
}