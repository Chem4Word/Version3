// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Shared;
using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Win32;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace WiX.CustomAction
{
    public class CustomActions
    {
        private const string OfficeKey = @"Microsoft\Office";
        private const string WordAddinsKey = @"Word\Addins\Chem4Word V3";
        private const string ProductShortName = "Chem4Word V3";
        private const string ProgrammDataFolder = "Chem4Word.V3";
        private const string ProductLongName = "Chemistry Add-In for Word (Chem4Word) V3";
        private const string ManifestFile = "Chem4Word.V3.vsto";

        [CustomAction]
        public static ActionResult SetupChem4Word(Session session)
        {
            session.Log("Begin SetupChem4Word()");

            session.Log($"  Running as {Environment.UserName}");

            try
            {
                session.Log($"  Environment.Is64BitOperatingSystem: {Environment.Is64BitOperatingSystem}");

                string c4wPath = null;
                if (Environment.Is64BitOperatingSystem)
                {
                    session.Log("  Detected 64bit OS");
                    session.Log($"  Environment.SpecialFolder.ProgramFiles: {Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)}");
                    session.Log($"  Environment.SpecialFolder.ProgramFilesX86: {Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}");

                    c4wPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), ProductShortName);
                }
                else
                {
                    session.Log("  Detected 32bit OS");
                    session.Log($"  Environment.SpecialFolder.ProgramFiles: {Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)}");
                    c4wPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), ProductShortName);
                }

                session.Log($"  Looking for folder '{c4wPath}'");
                if (Directory.Exists(c4wPath))
                {
                    session.Log("  Found Chem4Word installation folder");
                    string manifestFileLocation = Path.Combine(c4wPath, ManifestFile);
                    session.Log($"  Looking for file '{manifestFileLocation}'");
                    if (File.Exists(manifestFileLocation))
                    {
                        session.Log("  Found Chem4Word Add-In Manifest File");
                        AlterRegistry(session, manifestFileLocation);
                    }

                    ModifyFolderPermissions(session);
                }
            }
            catch (Exception ex)
            {
                session.Log($"** Exception: {ex.Message} **");
            }

            session.Log("End SetupChem4Word()");

            return ActionResult.Success;
        }

        private static void ModifyFolderPermissions(Session session)
        {
            session.Log($"  Fixing SpecialFolder.CommonApplicationData {ProgrammDataFolder}");

            try
            {
                string programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                string folderPath = Path.Combine(programData, ProgrammDataFolder);
                DirectorySecurity sec = Directory.GetAccessControl(folderPath);
                SecurityIdentifier users = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
                sec.AddAccessRule(new FileSystemAccessRule(users, FileSystemRights.Modify | FileSystemRights.Synchronize, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                Directory.SetAccessControl(folderPath, sec);
            }
            catch (Exception ex)
            {
                session.Log(ex.Message);
            }

            session.Log($"  Fixed SpecialFolder.CommonApplicationData {ProgrammDataFolder}");
        }

        [CustomAction]
        public static ActionResult RemoveChem4Word(Session session)
        {
            session.Log("Begin RemoveChem4Word()");

            session.Log($"  Running as {Environment.UserName}");

            try
            {
                AlterRegistry(session, null);
            }
            catch (Exception ex)
            {
                session.Log($"** Exception: {ex.Message} **");
            }

            session.Log("End RemoveChem4Word()");

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult FindWord(Session session)
        {
            session.Log("Begin FindWord()");

            int officeVersion = OfficeHelper.GetWinWordVersion();
            if (officeVersion >= 2010)
            {
                // Must be UPPERCASE
                session["WINWORDVERSION"] = officeVersion.ToString();
            }

            session.Log("End FindWord()");

            return ActionResult.Success;
        }

        private static void AlterRegistry(Session session, string manifestLocation)
        {
            session.Log(" Begin AlterRegistry()");

            try
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);

                    // A bit lazy here as I'm just blasting it to both possible locations
                    string keyName = $@"Software\{OfficeKey}";
                    session.Log($"  Opening {keyName}");
                    RegistryKey rk2 = rk.OpenSubKey(keyName, true);
                    if (rk2 != null)
                    {
                        RegisterChem4WordAddIn(session, rk2, manifestLocation);
                    }

                    keyName = $@"Software\WOW6432Node\{OfficeKey}";
                    session.Log($"  Opening {keyName}");
                    rk2 = rk.OpenSubKey(keyName, true);
                    if (rk2 != null)
                    {
                        RegisterChem4WordAddIn(session, rk2, manifestLocation);
                    }
                }
                else
                {
                    RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);

                    string keyName = $@"Software\{OfficeKey}";
                    session.Log($"  Opening {keyName}");
                    RegistryKey rk2 = rk.OpenSubKey(keyName, true);
                    if (rk2 != null)
                    {
                        RegisterChem4WordAddIn(session, rk2, manifestLocation);
                    }
                }
            }
            catch (Exception ex)
            {
                session.Log($"** Exception: {ex.Message} **");
            }

            session.Log(" End AlterRegistry()");
        }

        private static void RegisterChem4WordAddIn(Session session, RegistryKey rk, string manifestLocation)
        {
            session.Log(" End RegisterChem4WordAddIn()");

            try
            {
                if (!string.IsNullOrEmpty(manifestLocation))
                {
                    session.Log($"  Creating (or Opening) {WordAddinsKey}");
                    RegistryKey rk2 = rk.CreateSubKey(WordAddinsKey);
                    if (rk2 != null)
                    {
                        session.Log(" Registering Chem4Word Add-In");
                        rk2.SetValue("Description", ProductShortName);
                        rk2.SetValue("FriendlyName", ProductLongName);
                        rk2.SetValue("LoadBehavior", 3, RegistryValueKind.DWord);
                        rk2.SetValue("Manifest", $"{manifestLocation}|vstolocal");
                    }
                }
                else
                {
                    string keyName = WordAddinsKey.Replace($@"\{ProductShortName}", "");
                    session.Log($"  Opening {keyName}");
                    RegistryKey rk2 = rk.OpenSubKey(keyName, true);
                    if (rk2 != null)
                    {
                        session.Log(" UnRegistering Chem4Word Add-In");
                        rk2.DeleteSubKey(ProductShortName);
                    }
                }
            }
            catch (Exception ex)
            {
                session.Log($"** Exception: {ex.Message} **");
            }

            session.Log(" End RegisterChem4WordAddIn()");
        }
    }
}