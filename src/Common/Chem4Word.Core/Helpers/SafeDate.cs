﻿using System;

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
    }
}