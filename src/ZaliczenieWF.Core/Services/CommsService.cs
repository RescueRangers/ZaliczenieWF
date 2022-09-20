using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RJCP.IO.Ports;
using ZaliczenieWF.Core.Events;

namespace ZaliczenieWF.Core.Services
{
    internal class CommsService : ICommsService
    {
        
        private readonly ILogger<CommsService> _logger;
        private CancellationTokenSource _source;
        private CancellationToken _token;
        public delegate void ScoreReceivedEventHandler(object sender, ScoreReceivedEventArgs e);
        public delegate void SerialConnectionEventHandler(object sender, SerialConnectionEventArgs e);
        private Competition _currentCompetition = Competition.Null;

        public CommsService(ILogger<CommsService> logger)
        {
            _logger = logger;
            //_source = new CancellationTokenSource();
            //_token = _source.Token;
        }

        public async Task Connect(string serialPort)
        {
            _source = new CancellationTokenSource();
            _token = _source.Token;

            using (var serial = new SerialPortStream(serialPort, 9600, 8, RJCP.IO.Ports.Parity.None, RJCP.IO.Ports.StopBits.One))
            {
                try
                {
                    serial.Open();
                    _logger.LogDebug($"{serialPort} opened");
                    await serial.FlushAsync();
                    OnSerialConnection(new SerialConnectionEventArgs { ConnectionStatus = true });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    OnSerialConnection(new SerialConnectionEventArgs { ConnectionStatus = false, IsError = true, ErrorMessage = ex.Message });
                    return;
                }
                await Task.Run(() =>
                {
                    _logger.LogDebug("Task running");
                    serial.DiscardInBuffer();
                    //var message = "";
                    while (!_token.IsCancellationRequested)
                    {
                        var message = serial.ReadLine();
                        Thread.Sleep(100);

                        switch (message.ToLower())
                        {
                            case "10x10 start":
                                _currentCompetition = Competition._10x10;
                                break;
                            case "3000m start":
                                _currentCompetition = Competition.Marszobieg;
                                break;
                            case "sklony start":
                                _currentCompetition = Competition.Brzuszki;
                                break;
                            default:
                                if (message.StartsWith("wynik:", StringComparison.InvariantCultureIgnoreCase))
                                    ProcessScores(message);
                                break;
                        }

                        OnScoreReceived(new ScoreReceivedEventArgs { Score = message });
                    }
                }, _token);
                serial.Close();
                OnSerialConnection(new SerialConnectionEventArgs { ConnectionStatus = false });
            }
        }

        private void ProcessScores(string message)
        {
            if(_currentCompetition == Competition.Null)
            {
                _logger.LogError($"Wynik bez zadeklarowanej konkurencji. Wynik = {message}");
                return;
            }
            message = message.Remove(0, 7);

            OnScoreReceived(new ScoreReceivedEventArgs { Competition = _currentCompetition, Score = message });
            _currentCompetition = Competition.Null;
        }

        public void Disconnect(string serialPort)
        {
            try
            {
                _source.Cancel();
                _logger.LogDebug($"{serialPort} closed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return;
            }
        }

        public List<string> EnumerateSerialPorts()
        {
            var ports = SerialPort.GetPortNames();

            return new List<string>(ports);
        }

        public event EventHandler<ScoreReceivedEventArgs> ScoreReceived;

        protected virtual void OnScoreReceived(ScoreReceivedEventArgs e)
        {
            EventHandler<ScoreReceivedEventArgs> handler = ScoreReceived;
            handler?.Invoke(this, e);
        }

        public event EventHandler<SerialConnectionEventArgs> SerialConnection;

        protected virtual void OnSerialConnection(SerialConnectionEventArgs e)
        {
            EventHandler<SerialConnectionEventArgs> eventHandler = SerialConnection;
            eventHandler?.Invoke(this, e);
        }
    }
}
