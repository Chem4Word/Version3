// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
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
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Chem4Word.Telemetry
{
    public class AzureServiceBusWriter
    {
        // Make sure this is a Send Only Access key
        string ServiceBus = "Endpoint=sb://c4w-telemetry.servicebus.windows.net/;SharedAccessKeyName=TelemetrySender;SharedAccessKey=J8tkibrh5CHc2vZJgn1gbynZRmMLUf0mz/WZtmcjH6Q=";
        string QueueName = "telemetry";
        private static QueueClient _client;

        private readonly object _queueLock = new object();
        private Queue<ServiceBusMessage> _buffer1 = new Queue<ServiceBusMessage>();
        private bool _running = false;

        public void QueueMessage(ServiceBusMessage message)
        {
            lock (_queueLock)
            {
                _buffer1.Enqueue(message);
                Monitor.PulseAll(_queueLock);
            }

            if (!_running)
            {
                Thread t = new Thread(new ThreadStart(WriteOnThread));
                _running = true;
                t.Start();
            }
        }

        private void WriteOnThread()
        {
            Debug.WriteLine($"WriteOnThread() - Started at {DateTime.Now.ToString("HH:mm:ss.fff")}");

            Thread.Sleep(5);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Queue<ServiceBusMessage> buffer2 = new Queue<ServiceBusMessage>();

            while (_running)
            {
                // Move messages from 1st stage buffer to 2nd stage buffer
                lock (_queueLock)
                {
                    while (_buffer1.Count > 0)
                    {
                        buffer2.Enqueue(_buffer1.Dequeue());
                    }
                    Monitor.PulseAll(_queueLock);
                }

                // This is the slow part
                // Send messages from 2nd stage buffer to Azure
                while (buffer2.Count > 0)
                {
                    ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Https;

                    ServicePointManager.DefaultConnectionLimit = 100;
                    ServicePointManager.UseNagleAlgorithm = false;
                    ServicePointManager.Expect100Continue = false;

                    _client = QueueClient.CreateFromConnectionString(ServiceBus, QueueName);

                    WriteMessage(buffer2.Dequeue());
                }

                lock (_queueLock)
                {
                    if (_buffer1.Count == 0)
                    {
                        _running = false;
                        sw.Stop();
                        Debug.WriteLine($"WriteOnThread() - Queue emptied at {DateTime.Now.ToString("HH:mm:ss.fff")}");
                        Debug.WriteLine($"WriteOnThread() - Elapsed Time {sw.ElapsedMilliseconds.ToString("#,##0")}ms");
                    }
                    Monitor.PulseAll(_queueLock);
                }
            }
        }

        private void WriteMessage(ServiceBusMessage message)
        {
            try
            {
                BrokeredMessage bm = new BrokeredMessage(message.Message);
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