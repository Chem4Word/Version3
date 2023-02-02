// ---------------------------------------------------------------------------
//  Copyright (c) 2023, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Globalization;

namespace Chem4Word.Core.Helpers
{
    public static class SafeDate
    {
        public static DateTime Parse(string input)
        {
            DateTime result = DateTime.MinValue;

            string[] parts = input.Split('-');

            if (parts.Length == 3)
            {
                bool iso = parts[0].Length == 4;

                int day = iso ? Int32.Parse(parts[2]) : Int32.Parse(parts[0]);

                int month = 0;
                switch (parts[1].ToLower())
                {
                    case "01":
                    case "jan":
                        month = 1;
                        break;

                    case "02":
                    case "feb":
                        month = 2;
                        break;

                    case "03":
                    case "mar":
                        month = 3;
                        break;

                    case "04":
                    case "apr":
                        month = 4;
                        break;

                    case "05":
                    case "may":
                        month = 5;
                        break;

                    case "06":
                    case "jun":
                        month = 6;
                        break;

                    case "07":
                    case "jul":
                        month = 7;
                        break;

                    case "08":
                    case "aug":
                        month = 8;
                        break;

                    case "09":
                    case "sep":
                        month = 9;
                        break;

                    case "10":
                    case "oct":
                        month = 10;
                        break;

                    case "11":
                    case "nov":
                        month = 11;
                        break;

                    case "12":
                    case "dec":
                        month = 12;
                        break;
                }

                int year = iso ? Int32.Parse(parts[0]) : Int32.Parse(parts[2]);

                try
                {
                    result = new DateTime(year, month, day);
                }
                catch
                {
                    // Do Nothing default is already set
                }
            }

            return result;
        }

        /// <summary>
        /// "dd-MMM-yyyy HH:mm:ss.fff
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToLongDate(DateTime dateTime)
        {
            return dateTime.ToString("dd-MMM-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// "dd-MMM-yyyy"
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToShortDate(DateTime dateTime)
        {
            return dateTime.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// "HH:mm:ss.fff"
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToShortTime(DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// "yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToIsoShortDate(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// "MMddyyHHmm"
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToMdlHeaderTime(DateTime dateTime)
        {
            return dateTime.ToString("MMddyyHHmm", CultureInfo.InvariantCulture);
        }
    }
}