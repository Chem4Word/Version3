// ---------------------------------------------------------------------------
//  Copyright (c) 2023, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;

namespace Chem4Word.Telemetry
{
    public class ServiceBusMessage
    {
        private static long _order;

        public ServiceBusMessage(long utcOffset, int procId)
        {
            PartitionKey = "Chem4Word";
            // First part of RowKey is to enable "default" sort of time descending
            // Second part of RowKey is to give a sequence per process
            // Third part of RowKey is to guarantee uniqueness
            _order++;
            //long systemTicks = DateTime.UtcNow.Ticks - utcOffset;
            //long messageTicks = DateTime.MaxValue.Ticks - systemTicks;
            long messageTicks = DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks;
            string[] parts = new string[3];
            parts[0] = $"{messageTicks:D19}";
            parts[1] = procId + "-" + _order.ToString("000000");
            parts[2] = Guid.NewGuid().ToString("N");
            string rowKey = string.Join(".", parts);
            RowKey = rowKey;
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string AssemblyVersionNumber { get; set; }
        public string MachineId { get; set; }
        public string Operation { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
    }
}