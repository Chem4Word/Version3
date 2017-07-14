using Chem4Word.Numbo.Coa;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Chem4Word.Helpers
{
    public class TextHelper
    {
        public static string GetAsLatexFormattedString(string formula)
        {
            var split = formula.Split(' ');

            var length = split.Length;
            var stringBuilder = new StringBuilder();
            var n = length / 2;

            for (var j = 0; j < 2 * n; j += 2)
            {
                stringBuilder.Append(split[j]);
                if (!string.Equals("1", split[j + 1], StringComparison.Ordinal))
                {
                    stringBuilder.Append("_{");
                    stringBuilder.Append(split[j + 1]);
                    stringBuilder.Append("}");
                }
            }

            if (length % 2 != 0)
            {
                // must be a charge
                stringBuilder.Append("^");
                stringBuilder.Append("{");
                var negative = split[length - 1].StartsWith("-", StringComparison.Ordinal);

                var count = negative ? split[length - 1].Substring(1) : split[length - 1];
                // count is defined to be int by the schema
                if (!string.Equals("1", count, StringComparison.Ordinal))
                {
                    if (negative)
                    {
                        stringBuilder.Append(count);
                    }
                    else
                    {
                        stringBuilder.Append(count);
                    }
                }
                stringBuilder.Append(negative ? "-" : "+");
                stringBuilder.Append("}");
            }

            string s = stringBuilder.ToString();
            //Debug.WriteLine("GetAsLatexFormattedString() :- " + s);
            return s;
        }

        public static string FormatConciseAsOMath(string concise)
        {
            if (concise == null)
            {
                throw new ArgumentNullException("concise");
            }
            var split = concise.Split(' ');

            var length = split.Length;
            var stringBuilder = new StringBuilder();
            var n = length / 2;

            for (var j = 0; j < 2 * n; j += 2)
            {
                stringBuilder.Append(split[j]);
                if (!string.Equals("1", split[j + 1], StringComparison.Ordinal))
                {
                    stringBuilder.Append("_");
                    stringBuilder.Append(split[j + 1]);
                    if (j >= (2 * n) - 2)
                    {
                        if (length % 2 == 0)
                        {
                            stringBuilder.Append(" ");
                        }
                    }
                    else
                    {
                        stringBuilder.Append(" ");
                    }
                }
            }

            if (length % 2 != 0)
            {
                // must be a charge
                stringBuilder.Append("^");
                stringBuilder.Append("(");
                var negative = split[length - 1].StartsWith("-", StringComparison.Ordinal);

                var count = negative ? split[length - 1].Substring(1) : split[length - 1];
                // count is defined to be int by the schema
                if (!string.Equals("1", count, StringComparison.Ordinal))
                {
                    if (negative)
                    {
                        stringBuilder.Append(count);
                    }
                    else
                    {
                        stringBuilder.Append(count);
                    }
                }
                stringBuilder.Append(negative ? "-" : "+");
                stringBuilder.Append(")");
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString();
        }

        public static string FormatAsOMath(string s)
        {
            s = Depiction.ReplaceGreekAsOMath(s);
            const string superpattern = "(" +
                                        "(((?'Open'(?<!\\\\)\\^\\{))(?'capture'[^(?:(?<!\\\\)\\^\\{)(?:(?<!\\\\)\\})]*))+" +
                                        "((?'Close-Open'(?<!\\\\)}))" +
                                        ")+" +
                                        "(?(Open)(?!))";

            s = Regex.Replace(s, superpattern, "^(${capture}) ");
            const string subpattern = "(" +
                                      "(((?'Open'(?<!\\\\)_\\{))(?'capture'[^(?:(?<!\\\\)_\\{)(?:(?<!\\\\)\\})]*))+" +
                                      "((?'Close-Open'(?<!\\\\)}))" +
                                      ")+" +
                                      "(?(Open)(?!))";
            s = Regex.Replace(s, subpattern, "_(${capture}) ");
            if (s.StartsWith("_(", StringComparison.OrdinalIgnoreCase) ||
                s.StartsWith("^(", StringComparison.OrdinalIgnoreCase))
            {
                s = "\u200B" + s;
            }
            return s;
        }
    }
}