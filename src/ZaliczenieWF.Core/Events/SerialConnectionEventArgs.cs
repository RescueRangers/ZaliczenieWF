namespace ZaliczenieWF.Core.Events
{
    public class SerialConnectionEventArgs
    {
        public bool ConnectionStatus { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsError { get; set; }
    }
}
