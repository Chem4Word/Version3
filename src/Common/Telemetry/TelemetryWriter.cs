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
                if (!_systemInfoSent)
                {
                    if (!_helper.IpAddress.Contains("0.0.0.0"))
                    {
                        WriteStartUpInfo();
                    }
                }

                WritePrivate(operation, level, message);
            }
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

            WritePrivate("StartUp", "Information", _helper.GitStatus);
#endif

            // Log Wmi Gathered Data
            WritePrivate("StartUp", "Information", $"CPU: {_wmiHelper.CpuName}");
            WritePrivate("StartUp", "Information", $"CPU Cores: {_wmiHelper.LogicalProcessors}");
            WritePrivate("StartUp", "Information", $"CPU Speed: {_wmiHelper.CpuSpeed}");
            WritePrivate("StartUp", "Information", $"Physical Memory: {_wmiHelper.PhysicalMemory}");

            // Log screen sizes
            WritePrivate("StartUp", "Information", $"Screens: {_helper.Screens}");

            // Log System
            WritePrivate("StartUp", "Information", _helper.SystemOs);
            WritePrivate("StartUp", "Information", $"Browser Version: {_helper.BrowserVersion}");
            WritePrivate("StartUp", "Information", _helper.DotNetVersion);

            // Log UtcOffset
            WritePrivate("StartUp", "Information", $"Server UTC DateTime is {_helper.ServerUtcDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");
            WritePrivate("StartUp", "Information", $"System UTC DateTime is {_helper.SystemUtcDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");

            WritePrivate("StartUp", "Information", $"Server Header [Date] is {_helper.ServerDateHeader}");
            WritePrivate("StartUp", "Information", $"Server UTC DateTime raw is {_helper.ServerUtcDateRaw}");

            WritePrivate("StartUp", "Information", $"Calculated UTC Offset is {_helper.UtcOffset}");
            if (_helper.UtcOffset > 0)
            {
                TimeSpan delta = TimeSpan.FromTicks(_helper.UtcOffset);
                WritePrivate("StartUp", "Information", $"System UTC DateTime is {delta} ahead of Server time");
            }
            if (_helper.UtcOffset < 0)
            {
                TimeSpan delta = TimeSpan.FromTicks(0 - _helper.UtcOffset);
                WritePrivate("StartUp", "Information", $"System UTC DateTime is {delta} behind Server time");
            }

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
            ServiceBusMessage sbm = new ServiceBusMessage(_helper.UtcOffset, _helper.ProcessId);
            sbm.MachineId = _helper.MachineId;
            sbm.Operation = operation;
            sbm.Level = level;
            sbm.Message = message;
            sbm.AssemblyVersionNumber = _helper.AssemblyVersionNumber;
            _azureServiceBusWriter.QueueMessage(sbm);
        }
    }
}