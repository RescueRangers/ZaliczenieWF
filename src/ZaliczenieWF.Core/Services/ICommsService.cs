using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZaliczenieWF.Core.Events;

namespace ZaliczenieWF.Core.Services
{
    public interface ICommsService
    {
        List<string> EnumerateSerialPorts();

        Task Connect(string serialPort);
        void Disconnect(string serialPort);
        event EventHandler<ScoreReceivedEventArgs> ScoreReceived;
        event EventHandler<SerialConnectionEventArgs> SerialConnection;
    }
}
