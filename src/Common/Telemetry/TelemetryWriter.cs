﻿// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using IChem4Word.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Chem4Word.Telemetry
{
    public class TelemetryWriter : IChem4WordTelemetry
    {
        private static int _counter;
        private static AzureServiceBusWriter _azureServiceBusWriter;
        private static bool _systemInfoSent;

        private static SystemHelper _helper;
        private static WmiHelper _wmiHelper;

        private readonly bool _permissionGranted;

        public TelemetryWriter(bool permissionGranted)
        {
            _permissionGranted = permissionGranted;
            if (_helper == null)
            {
                _helper = new SystemHelper();
            }

            if (_wmiHelper == null)
            {
                _wmiHelper = new WmiHelper();
            }
            _azureServiceBusWriter = new AzureServiceBusWriter();
        }

        public void Write(string operation, string level, string message)
        {
            _counter++;

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

            string debugMessage = $"[{_counter}] {operation} - {level} - {message}";
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

            if (_permissionGranted)
            {
                WritePrivate(operation, level, message);

                if (!_systemInfoSent)
                {
                    if (!_helper.IpAddress.Contains("0.0.0.0"))
                    {
                        WriteStartUpInfo();
                    }
                }
            }
        }

        private void WriteStartUpInfo()
        {
            List<string> lines = new List<string>();

            // Log Add-In Version
            WritePrivate("StartUp", "Information", _helper.AddInVersion); // ** Used by Andy's Knime protocol ?

            // Log Word
            WritePrivate("StartUp", "Information", _helper.WordProduct); // ** Used by Andy's Knime protocol
            if (!string.IsNullOrEmpty(_helper.Click2RunProductIds))
            {
                WritePrivate("StartUp", "Information", _helper.Click2RunProductIds);
            }
            WritePrivate("StartUp", "Information", Environment.GetCommandLineArgs()[0]);

            // Log System
            WritePrivate("StartUp", "Information", _helper.SystemOs); // ** Used by Andy's Knime protocol
            WritePrivate("StartUp", "Information", _helper.DotNetVersion);
            WritePrivate("StartUp", "Information", $"Browser Version: {_helper.BrowserVersion}");

            // Log IP Address
            WritePrivate("StartUp", "Information", _helper.IpAddress); // ** Used by Andy's Knime protocol
            WritePrivate("StartUp", "Information", _helper.IpObtainedFrom);

            // Log UtcOffsets
            lines = new List<string>();

            lines.Add($"Server UTC DateTime is {_helper.ServerUtcDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");
            lines.Add($"System UTC DateTime is { _helper.SystemUtcDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");

            lines.Add($"Server Header [Date] is {_helper.ServerDateHeader}");
            lines.Add($"Server UTC DateTime raw is {_helper.ServerUtcDateRaw}");

            lines.Add($"Calculated UTC Offset is {_helper.UtcOffset}");
            if (_helper.UtcOffset > 0)
            {
                TimeSpan delta = TimeSpan.FromTicks(_helper.UtcOffset);
                lines.Add($"System UTC DateTime is {delta} ahead of Server time");
            }
            if (_helper.UtcOffset < 0)
            {
                TimeSpan delta = TimeSpan.FromTicks(0 - _helper.UtcOffset);
                lines.Add($"System UTC DateTime is {delta} behind Server time");
            }

            WritePrivate("StartUp", "Information", string.Join(Environment.NewLine, lines));

            // Log Wmi Gathered Data
            lines = new List<string>();

            lines.Add($"CPU: {_wmiHelper.CpuName}");
            lines.Add($"CPU Cores: {_wmiHelper.LogicalProcessors}");
            lines.Add($"CPU Speed: {_wmiHelper.CpuSpeed}");
            lines.Add($"Physical Memory: {_wmiHelper.PhysicalMemory}");
            lines.Add($"Booted Up: {_wmiHelper.LastLastBootUpTime}");

            // Log Screen Sizes
            lines.Add($"Screens: {_helper.Screens}");
            WritePrivate("StartUp", "Information", string.Join(Environment.NewLine, lines));

#if DEBUG
            lines = new List<string>();

            string clientName = Environment.GetEnvironmentVariable("CLIENTNAME");
            string userDomainName = Environment.UserDomainName;
            string userName = Environment.UserName;
            string machineName = Environment.MachineName;
            string userSummary;

            if (userDomainName.Equals(machineName))
            {
                // Local account
                userSummary = $"Local user {userName} on {machineName}";
            }
            else
            {
                // Domain account
                userSummary = $@"Domain user {userDomainName}\{userName} on {machineName}";
            }

            // Include RDP Info if available
            if (!string.IsNullOrEmpty(clientName))
            {
                userSummary += $" via RDP from {clientName}";
            }

            lines.Add($"Debug - {userSummary}");

            lines.Add($"Debug - Environment.OSVersion: {Environment.OSVersion}");
            lines.Add($"Debug - Environment.Version: {Environment.Version}");

            lines.Add($"Debug - Environment.CommandLine: {Environment.CommandLine}");
            lines.Add($"Debug - AddIn Location: {_helper.AddInLocation}");
            lines.Add($"Debug - Environment.Is64BitOperatingSystem: {Environment.Is64BitOperatingSystem}");
            lines.Add($"Debug - Environment.Is64BitProcess: {Environment.Is64BitProcess}");

            WritePrivate("StartUp", "Information", string.Join(Environment.NewLine, lines));

            WritePrivate("StartUp", "Information", _helper.GitStatus);
#endif

            #region Log critical System Info again to ensure we get it

            // Log Add-In Version
            WritePrivate("StartUp", "Information", _helper.AddInVersion); // ** Used by Andy's Knime protocol ?

            // Log Word
            WritePrivate("StartUp", "Information", _helper.WordProduct); // ** Used by Andy's Knime protocol

            // Log System
            WritePrivate("StartUp", "Information", _helper.SystemOs); // ** Used by Andy's Knime protocol

            // Log IP Address
            WritePrivate("StartUp", "Information", _helper.IpAddress); // ** Used by Andy's Knime protocol

            #endregion Log critical System Info again to ensure we get it

            _systemInfoSent = true;
        }

        private void WritePrivate(string operation, string level, string message)
        {
            ServiceBusMessage sbm = new ServiceBusMessage(_helper.UtcOffset, _helper.ProcessId);
            sbm.MachineId = _helper.MachineId;
            sbm.Operation = operation;
            sbm.Level = level;
            sbm.Message = message;
            sbm.AssemblyVersionNumber = _helper.AssemblyVersionNumber;

            //_azureServiceBusWriter.QueueMessage(sbm);
            _azureServiceBusWriter.WriteMessage(sbm);
        }
    }
}