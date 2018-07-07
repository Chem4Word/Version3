// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Chem4Word.Telemetry
{
    public class MessageEntity : TableEntity
    {
        public MessageEntity()
        {
            PartitionKey = "Chem4Word";
            // First Part of RowKey is to enable "default" sort of time descending
            // Second Part of RowKey is to guarantee uniqueness
            string rowKey = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks) + " " + Guid.NewGuid().ToString("N");
            RowKey = rowKey;
        }

        public string MachineId { get; set; }
        public string Operation { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
    }
}