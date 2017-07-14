using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                if (s.EndsWith(resourceName))
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
                Debug.Assert(false, "Unique match not found");
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
