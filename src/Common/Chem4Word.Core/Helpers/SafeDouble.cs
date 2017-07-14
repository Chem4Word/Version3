// Created by Gergely István Oroszi - 2015.08.13.
//
// -----------------------------------------------------------------------
//   Copyright (c) 2015, The Outercurve Foundation.
//   This software is released under the Apache License, Version 2.0.
//   The license and further copyright text can be found in the file LICENSE.TXT at
//   the root directory of the distribution.
// -----------------------------------------------------------------------

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