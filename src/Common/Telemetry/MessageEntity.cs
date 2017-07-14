using Microsoft.WindowsAzure.Storage.Table;

namespace Chem4Word.Telemetry
{
    public class MessageEntity : TableEntity
    {
        public MessageEntity()
        {
        }

        public string MachineId { get; set; }
        public string Operation { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
    }
}