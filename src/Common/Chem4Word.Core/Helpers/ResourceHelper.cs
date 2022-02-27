// ---------------------------------------------------------------------------
//  Copyright (c) 2022, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Chem4Word.Core.Helpers
{
    public static class ResourceHelper
    {
        public static Stream GetBinaryResource(Assembly assembly, string resourceName)
        {
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
                //Debug.WriteLine(s);
            }

            if (!string.IsNullOrEmpty(fullName))
            {
                data = assembly.GetManifestResourceStream(fullName);
            }

            if (count != 1)
            {
                Debug.WriteLine("Unique match not found");
#if DEBUG
                Debugger.Break();
#endif
            }

            return data;
        }

        public static string GetStringResource(Assembly assembly, string resourceName)
        {
            string data = null;

            var resource = GetBinaryResource(assembly, resourceName);
            if (resource != null)
            {
                var textStreamReader = new StreamReader(resource);
                data = textStreamReader.ReadToEnd();

                // Repair any broken line feeds to Windows style
                char etx = (char)3;
                string temp = data.Replace("\r\n", $"{etx}");
                temp = temp.Replace("\n", $"{etx}");
                temp = temp.Replace("\r", $"{etx}");
                string[] lines = temp.Split(etx);
                data = string.Join(Environment.NewLine, lines);
            }

            return data;
        }

        public static void WriteResource(Assembly assembly, string resourceName, string destPath)
        {
            Stream stream = GetBinaryResource(assembly, resourceName);

            using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }
    }
}