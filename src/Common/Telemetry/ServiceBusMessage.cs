using System;

namespace Chem4Word.Telemetry
{
    public class ServiceBusMessage
    {
        public ServiceBusMessage()
        {
            PartitionKey = "Chem4Word";
            string rowKey = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);
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