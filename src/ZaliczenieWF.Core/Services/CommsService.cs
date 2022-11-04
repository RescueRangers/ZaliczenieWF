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
        }


        /// <summary>
        /// Klasa odpowiedzialna za połączenie z portem Serial
        /// </summary>
        /// <param name="serialPort">Nazwa portu</param>
        /// <returns></returns>
        public async Task Connect(string serialPort)
        {
            //Twory nowy CancellationTokenSource, który będzie użyty w przypadku potrzeby zamkniecia połączenia
            _source = new CancellationTokenSource();
            _token = _source.Token;
            var taskCompletionSource = new TaskCompletionSource<object>();

            //rejestrowanie tokenu, wymagane żeby anulowanie powiodło się w przypadku długo działającego zadania
            using (_token.Register(() => taskCompletionSource.TrySetCanceled()))
            {
                //Konfiguracja portu serial
                using (var serial = new SerialPortStream(serialPort, 9600, 8, RJCP.IO.Ports.Parity.None, RJCP.IO.Ports.StopBits.One))
                {
                    //Start zadania asynchronicznego
                    var task = Task.Run(async () =>
                    {
                        try
                        {
                            serial.Open();
                            _logger.LogDebug($"{serialPort} opened");
                            await serial.FlushAsync();

                            //Wysłanie eventu móweiącego o pozytywnym statusie połączenia
                            OnSerialConnection(new SerialConnectionEventArgs { ConnectionStatus = true });
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message);
                            //W przypadku błędu event ze statusem błędu
                            OnSerialConnection(new SerialConnectionEventArgs { ConnectionStatus = false, IsError = true, ErrorMessage = ex.Message });
                            return;
                        }
                        _logger.LogDebug("Task running");

                        //Wyrzucenie wszystkich informacji w buforze przed rozpoczęciem czytania z portu serial
                        serial.DiscardInBuffer();
                        var message = string.Empty;
                        var stringBuffer = string.Empty;

                        //Zadania działa dopóki nie zostanie anulowane przez rejestrowany wcześniej token 
                        while (!_token.IsCancellationRequested)
                        {
                            _token.ThrowIfCancellationRequested();

                            //W przypadku debugowanie bez prawdziwego urządzenia dane czytane są z wirtualnych portów COM,
                            //które wymagają poniższego kodu do skladania wiadomości litera po literze.
                            //Wiadomość jest finalizowania po wciśnięciu enter.

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
                            //W przypadku działania na prawdziwym urządzeniu wiadomości jest czytana z buforu portu serial.
                            message = serial.ReadLine();
#endif
                            //Czytamy co .1 sekundy
                            Thread.Sleep(100);

                            //Switch zmieniający obecną konkurence ze względu na wiadomość otrzymaną z portu serial
                            //Jeżeli otrzyma wiadomość zaczynającą sie od słowa wynik przechodzi do przetwarzania punktacji.
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
                    //W przypadku anulowania zadania wysyła wiadomość o zamknięciu połączenia
                    OnSerialConnection(new SerialConnectionEventArgs { ConnectionStatus = false });
                }
            }
        }

        /// <summary>
        /// Przetwarzanie punktacji, wymaga wcześniejhszego ustawienia konkurencji
        /// </summary>
        /// <param name="message">Wiadomość z punktacją</param>
        private void ProcessScores(string message)
        {
            // Jeżeli punktacja nie została ustawiona logują błąd i wychodzi z metody.
            if (_currentCompetition == Competition.Null)
            {
                _logger.LogError($"Wynik bez zadeklarowanej konkurencji. Wynik = {message}");
                return;
            }
            // Usuwa pierwsze 7 znakow z wiadomości żeby uzyskac sam wynik
            message = message.Remove(0, 7);

            // Wysyła event z otzrymaną punktacją i ustawia konkrencję na pustą.
            OnScoreReceived(new ScoreReceivedEventArgs { Competition = _currentCompetition, Score = message });
            _currentCompetition = Competition.Null;
        }

        /// <summary>
        /// Anuluje zadanie połączenia z portem serial.
        /// </summary>
        /// <param name="serialPort">Nazwa portu do rozłączenia</param>
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

        /// <summary>
        /// Wylicza listę portów serial dostępnych w systemie
        /// </summary>
        /// <returns>Lista nazw portów serial.</returns>
        public List<string> EnumerateSerialPorts()
        {
            var ports = SerialPort.GetPortNames();

            return new List<string>(ports);
        }

        #region Events
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
        #endregion
    }
}
