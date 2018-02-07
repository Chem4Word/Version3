// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using IChem4Word.Contracts;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Chem4Word.Telemetry
{
    public class TelemetryWriter : IChem4WordTelemetry
    {
        private static int Counter;
        private static AzureTableWriter Storage;
        private static bool _systemInfoSent = false;

        private SystemHelper _helper;
        private WmiHelper _wmihelper;

        public TelemetryWriter()
        {
            _helper = new SystemHelper();
            _wmihelper = new WmiHelper();
            Storage = new AzureTableWriter();
        }

        public void Write(string operation, string level, string message)
        {
            Counter++;

            string unwanted = "Chem4Word.V3.";
            if (operation.StartsWith(unwanted))
            {
                operation = operation.Remove(0, unwanted.Length);
            }
            unwanted = "Chem4WordV3.";
            if (operation.StartsWith(unwanted))
            {
                operation = operation.Remove(0, unwanted.Length);
            }
            unwanted = "Chem4Word.";
            if (operation.StartsWith(unwanted))
            {
                operation = operation.Remove(0, unwanted.Length);
            }

            string debugMessage = $"[{Counter}] {operation} - {level} - {message}";
            Debug.WriteLine(debugMessage);

            try
            {
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    $@"Chem4Word.V3\Telemetry\{DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.log");
                using (StreamWriter w = File.AppendText(fileName))
                {
                    string logMessage = $"[{DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture)}] {operation} - {level} - {message}";
                    w.WriteLine(logMessage);
                }
            }
            catch
            {
                //
            }

            if (!_systemInfoSent)
            {
                if (!_helper.IpAddress.Contains("0.0.0.0"))
                {
                    WriteStartUpInfo();
                }
            }

            WritePrivate(operation, level, message);
        }

        private void WriteStartUpInfo()
        {
            string userDomainName = Environment.UserDomainName;
            string userName = Environment.UserName;
            string machineName = Environment.MachineName;
            string userSummary;

            if (userDomainName.Equals(machineName))
            {
                // Local account
                userSummary = $"Local user {userName} on PC {machineName}";
            }
            else
            {
                // Domain account
                userSummary = $@"Domain user {userDomainName}\{userName} on PC {machineName}";
            }
#if DEBUG
            WritePrivate("StartUp", "Information", $"Debug - Environment.OSVersion: {Environment.OSVersion}");
            WritePrivate("StartUp", "Information", $"Debug - Environment.Version: {Environment.Version}");

            WritePrivate("StartUp", "Information", $"Debug - {userSummary}");

            WritePrivate("StartUp", "Information", $"Debug - Environment.CommandLine: {Environment.CommandLine}");
            WritePrivate("StartUp", "Information", $"Debug - AddIn Location: {_helper.AddInLocation}");
            WritePrivate("StartUp", "Information", $"Debug - Environment.Is64BitOperatingSystem: {Environment.Is64BitOperatingSystem}");
            WritePrivate("StartUp", "Information", $"Debug - Environment.Is64BitProcess: {Environment.Is64BitProcess}");
#endif

            // Log Wmi Gathered Data
            WritePrivate("StartUp", "Information", $"CPU: {_wmihelper.CpuName}");
            WritePrivate("StartUp", "Information", $"CPU Cores: {_wmihelper.LogicalProcessors}");
            WritePrivate("StartUp", "Information", $"CPU Speed: {_wmihelper.CpuSpeed}");
            WritePrivate("StartUp", "Information", $"Physical Memory: {_wmihelper.PhysicalMemory}");

            // Log screen sizes
            WritePrivate("StartUp", "Information", $"Screens: {_helper.Screens}");

            // Log Sysytem
            WritePrivate("StartUp", "Information", _helper.SystemOs);
            WritePrivate("StartUp", "Information", _helper.DotNetVersion);

            // Log IP Address
            WritePrivate("StartUp", "Information", _helper.IpAddress);
            WritePrivate("StartUp", "Information", _helper.IpObtainedFrom);

            // Log Word
            WritePrivate("StartUp", "Information", Environment.GetCommandLineArgs()[0]);
            WritePrivate("StartUp", "Information", _helper.WordProduct);

            // Log Add-In Version
            WritePrivate("StartUp", "Information", _helper.AddInVersion);

            _systemInfoSent = true;
        }

        private void WritePrivate(string operation, string level, string message)
        {
            MessageEntity me = new MessageEntity();
            me.MachineId = _helper.MachineId;
            me.Operation = operation;
            me.Level = level;
            me.Message = message;
            Storage.QueueMessage(me);
        }
    }
}