// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------
//
// Created by Gergely István Oroszi - 13-Aug-2013

using System;
using System.Globalization;

namespace Chem4Word.Core.Helpers
{
    public class SafeDouble
    {
        public static double Parse(string source)
        {
            var separatorChar = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            var fixedSource = source.Replace(separatorChar, '.');
            var result = Convert.ToDouble(fixedSource, CultureInfo.InvariantCulture);

            return result;
        }

        public static string AsString(double? source, string format)
        {
            string result = "{null}";

            if (source != null)
            {
                result = source.Value.ToString(format);
            }

            return result;
        }
    }
}