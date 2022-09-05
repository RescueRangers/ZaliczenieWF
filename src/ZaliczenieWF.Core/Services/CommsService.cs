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

            using (var serial = new SerialPortStream(serialPort))
            {
                try
                {
                    serial.Open();
                    _logger.LogDebug($"{serialPort} opened");
                    await serial.FlushAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return;
                }
                await Task.Run(() =>
                {
                    _logger.LogDebug("Task running");
                    var message = "";
                    while (!_token.IsCancellationRequested)
                    {
                        var ch = (char)serial.ReadChar();
                        if (ch == '\n' || ch == '\r')
                        {
                            var args = new ScoreReceivedEventArgs
                            {
                                Score = message
                            };
                            OnScoreReceived(args);
                            //_logger.LogDebug(message);
                            message = "";
                        }
                        else
                        {
                            message += ch;
                        }
                    }
                    serial.Close();
                }, _token);
            }
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
    }
}
