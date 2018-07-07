using System;

namespace Chem4Word.Telemetry
{
    public class ServiceBusMessage
    {
        public ServiceBusMessage()
        {
            PartitionKey = "Chem4Word";
            // First Part of RowKey is to enable "default" sort of time descending
            // Second Part of RowKey is to guarantee uniqueness
            string rowKey = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks) + " " + Guid.NewGuid().ToString("N");
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