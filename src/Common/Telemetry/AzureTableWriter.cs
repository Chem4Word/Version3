// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Chem4Word.Telemetry
{
    public class AzureTableWriter
    {
        private static string accountName = "c4wtelemetry";
        private static string accountKey = "60KaUHLXdj9sh4x3h1qgBRS3BMM2UbAAoeMChrp9xMBPutpB27feYMbUfcsqZb1PTu0rkJhruhkUD1ytuWjUoQ==";
        private CloudTable _cloudTable = null;

        private readonly object _queueLock = new object();
        private Queue<MessageEntity> _buffer1 = new Queue<MessageEntity>();
        private bool _running = false;

        public AzureTableWriter()
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
                CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
                string tableName = "LoggingV3";
#if DEBUG
                tableName = "LoggingV3Debug";
#endif
                _cloudTable = cloudTableClient.GetTableReference(tableName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void QueueMessage(MessageEntity messageEntity)
        {
            lock (_queueLock)
            {
                _buffer1.Enqueue(messageEntity);
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

            Queue<MessageEntity> buffer2 = new Queue<MessageEntity>();

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

        private void WriteMessage(MessageEntity messageEntity)
        {
            try
            {
                if (_cloudTable != null)
                {
                    // Fill in remaining properties
                    //messageEntity.PartitionKey = messageEntity.MachineId; // Better storage, but slower retrieval times for mixed environment
                    messageEntity.PartitionKey = "Chem4Word";

                    messageEntity.RowKey = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);

                    TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(messageEntity);
                    _cloudTable.Execute(insertOrMergeOperation);
                }
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