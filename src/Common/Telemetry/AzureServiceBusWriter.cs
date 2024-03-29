﻿// ---------------------------------------------------------------------------
//  Copyright (c) 2023, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using Chem4Word.Core;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Chem4Word.Telemetry
{
    public class AzureServiceBusWriter
    {
        private static QueueClient _client;

        private AzureSettings _settings;

        private static readonly object QueueLock = Guid.NewGuid();

        private Queue<ServiceBusMessage> _buffer1 = new Queue<ServiceBusMessage>();
        private bool _running = false;

        public AzureServiceBusWriter(AzureSettings settings)
        {
            _settings = settings;

            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Https;

            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;

            _client = QueueClient.CreateFromConnectionString($"{_settings.ServiceBusEndPoint};{_settings.ServiceBusToken}", _settings.ServiceBusQueue);
        }

        public void QueueMessage(ServiceBusMessage message)
        {
            lock (QueueLock)
            {
                _buffer1.Enqueue(message);
                Monitor.PulseAll(QueueLock);
            }

            if (!_running)
            {
                Thread t = new Thread(new ThreadStart(WriteOnThread));
                t.SetApartmentState(ApartmentState.STA);
                _running = true;
                t.Start();
            }
        }

        private void WriteOnThread()
        {
            // Small sleep before we start
            Thread.Sleep(25);

            var buffer2 = new Queue<ServiceBusMessage>();

            while (_running)
            {
                // Move messages from 1st stage buffer to 2nd stage buffer
                lock (QueueLock)
                {
                    while (_buffer1.Count > 0)
                    {
                        buffer2.Enqueue(_buffer1.Dequeue());
                    }
                    Monitor.PulseAll(QueueLock);
                }

                while (buffer2.Count > 0)
                {
                    WriteMessage(buffer2.Dequeue());
                    Thread.Sleep(10);
                }

                lock (QueueLock)
                {
                    if (_buffer1.Count == 0)
                    {
                        _running = false;
                    }
                    Monitor.PulseAll(QueueLock);
                }
            }
        }

        private void WriteMessage(ServiceBusMessage message)
        {
            try
            {
                var bm = new BrokeredMessage(message.Message);
                bm.Properties["PartitionKey"] = message.PartitionKey;
                bm.Properties["RowKey"] = message.RowKey;
                bm.Properties["Chem4WordVersion"] = message.AssemblyVersionNumber;
                bm.Properties["MachineId"] = message.MachineId;
                bm.Properties["Operation"] = message.Operation;
                bm.Properties["Level"] = message.Level;
#if DEBUG
                bm.Properties["IsDebug"] = "True";
#endif
                _client.Send(bm);
                // Small sleep between each message
                Thread.Sleep(25);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in WriteMessage: {ex.Message}");

                try
                {
                    string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        $@"Chem4Word.V3\Telemetry\{DateTime.Now.ToString("yyyy-MM-dd")}.log");
                    using (StreamWriter w = File.AppendText(fileName))
                    {
                        w.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss.fff")}] Exception in WriteMessage: {ex.Message}");
                    }
                }
                catch
                {
                    //
                }
            }
        }
    }
}