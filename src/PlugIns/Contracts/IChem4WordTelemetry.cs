namespace IChem4Word.Contracts
{
    public interface IChem4WordTelemetry
    {
        void Write(string source, string level, string message);
    }
}