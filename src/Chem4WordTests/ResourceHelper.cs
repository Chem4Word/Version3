// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Chem4WordTests
{
    public class ResourceHelper
    {
        private static Stream GetBinaryResource(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            Stream data = null;

            string fullName = string.Empty;
            int count = 0;

            string[] resources = assembly.GetManifestResourceNames();
            foreach (var s in resources)
            {
                if (s.EndsWith($".{resourceName}"))
                {
                    count++;
                    fullName = s;
                }
                Debug.WriteLine(s);
            }

            if (!string.IsNullOrEmpty(fullName))
            {
                data = assembly.GetManifestResourceStream(fullName);
            }

            if (count != 1)
            {
                return null;
            }

            return data;
        }

        public static string GetStringResource(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            string data = null;

            var resource = GetBinaryResource(resourceName);
            if (resource != null)
            {
                var textStreamReader = new StreamReader(resource);
                data = textStreamReader.ReadToEnd();

                // Repair any "broken" line feeds to Windows style
                char etx = (char)3;
                string temp = data.Replace("\r\n", $"{etx}");
                temp = temp.Replace("\n", $"{etx}");
                temp = temp.Replace("\r", $"{etx}");
                string[] lines = temp.Split(etx);
                data = string.Join(Environment.NewLine, lines);
            }

            return data;
        }
    }
}