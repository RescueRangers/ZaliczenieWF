using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RJCP.IO.Ports;
using ZaliczenieWF.Core.Events;
using ZaliczenieWF.Models;

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
            var taskCompletionSource = new TaskCompletionSource<object>();

            using (_token.Register(() => taskCompletionSource.TrySetCanceled()))
            {
                using (var serial = new SerialPortStream(serialPort, 9600, 8, RJCP.IO.Ports.Parity.None, RJCP.IO.Ports.StopBits.One))
                {
                    
                    var task = Task.Run(async () =>
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
                        _logger.LogDebug("Task running");
                        serial.DiscardInBuffer();
                        var message = string.Empty;
                        var stringBuffer = string.Empty;

                        while (!_token.IsCancellationRequested)
                        {
                            _token.ThrowIfCancellationRequested();

#if DEBUG
                            var c = (char)serial.ReadChar();
                            if (c == '\r' || c == '\n')
                            {
                                message = stringBuffer;
                                stringBuffer = "";
                            }
                            else
                            {
                                stringBuffer += c;
                            }
#else
                            message = serial.ReadLine();
#endif

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
                                    _logger.LogDebug(message);
                                    break;
                            }
                        }
                    }, _token);
                    serial.Close();
                    await Task.WhenAny(task, taskCompletionSource.Task);
                    OnSerialConnection(new SerialConnectionEventArgs { ConnectionStatus = false });
                }
            }
        }

        private void ProcessScores(string message)
        {
            if (_currentCompetition == Competition.Null)
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
