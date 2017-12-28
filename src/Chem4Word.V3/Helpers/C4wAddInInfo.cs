// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Chem4Word.Helpers
{
    public class C4wAddInInfo
    {
        //private readonly string _localAppDataPath;

        /// <summary>
        /// Name of product reflected from assembly i.e. "Chem4Word.V3"
        /// </summary>
        public string ProductName { get; }

        /// <summary>
        /// Where the AddIn is being run from i.e. "C:\Program Files (x86)\Chem4Word.V3"
        /// </summary>
        public string DeploymentPath { get; }

        /// <summary>
        /// Common Data Path i.e. C:\ProgramData\Chem4Word.V3
        /// </summary>
        public string ProgramDataPath { get; }

        /// <summary>
        /// Local AppData Path of Product i.e. "C:\Users\{User}\AppData\Local\Chem4Word.V3"
        /// </summary>
        public string ProductAppDataPath { get; }

        /// <summary>
        /// Local AppData Path i.e. "C:\Users\{User}\AppData\Local"
        /// </summary>
        public string AppDataPath { get; }

        public C4wAddInInfo()
        {
            // Get the assembly information
            Assembly assemblyInfo = Assembly.GetExecutingAssembly();
            ProductName = assemblyInfo.FullName.Split(',')[0];

            // Location is where the assembly is run from (in dl3 cache)
            //string assemblyLocation = assemblyInfo.Location;

            // CodeBase is the location of the installed files
            Uri uriCodeBase = new Uri(assemblyInfo.CodeBase);
            DeploymentPath = Path.GetDirectoryName(uriCodeBase.LocalPath);

            // Get the user's Local AppData Path i.e. "C:\Users\{User}\AppData\Local\" and ensure our user data folder exists
            AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            ProductAppDataPath = Path.Combine(AppDataPath, ProductName);

            if (!Directory.Exists(ProductAppDataPath))
            {
                Directory.CreateDirectory(ProductAppDataPath);
            }
            if (!Directory.Exists($@"{ProductAppDataPath}\Telemetry"))
            {
                Directory.CreateDirectory($@"{ProductAppDataPath}\Telemetry");
            }

            // Get ProgramData Path i.e "C:\ProgramData\Chem4Word.V3" and ensure it exists
            string programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            ProgramDataPath = Path.Combine(programData, ProductName);

            if (!Directory.Exists(ProgramDataPath))
            {
                Directory.CreateDirectory(ProgramDataPath);
            }

            try
            {
                // Allow all users to Modify files in this folder
                DirectorySecurity sec = Directory.GetAccessControl(ProgramDataPath);
                SecurityIdentifier users = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
                sec.AddAccessRule(new FileSystemAccessRule(users, FileSystemRights.Modify | FileSystemRights.Synchronize, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                Directory.SetAccessControl(ProgramDataPath, sec);
            }
            catch
            {
                // Do Nothing
            }
        }
    }
}