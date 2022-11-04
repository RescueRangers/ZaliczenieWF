using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ZaliczenieWF.Core.Events;
using ZaliczenieWF.Core.Services;
using ZaliczenieWF.Models;

namespace ZaliczenieWF.Core.ViewModels.Main
{
    public class MainViewModel : BaseViewModel
    {
        private IScoreService _scoreService;
        private ICommsService _commsService;
        private readonly IMvxNavigationService _navigationService;
        private ILogger<MainViewModel> _logger;
        private IDataIOService _dataIO;

        public MainViewModel(IScoreService scoreService, ILogger<MainViewModel> logger, ICommsService commsService, IMvxNavigationService navigationService, IDataIOService dataIO)
        {
            _scoreService = scoreService;
            _logger = logger;
            _commsService = commsService;
            _navigationService = navigationService;
            _dataIO = dataIO;

            
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            // Inicjalizacja kolekcji uczestników i portów serial
            _participants = new ObservableCollection<Participant>();
            _ports = new ObservableCollection<string>(_commsService.EnumerateSerialPorts());

            // Inicjalizacja komend
            ConnectToPort = new MvxAsyncCommand(async () => await _commsService.Connect(SelectedSerialPort));
            Disconnect = new MvxCommand(() => _commsService.Disconnect(SelectedSerialPort));
            AddParticipantCommand = new MvxAsyncCommand(async () => await AddNewParticipantAsync());
            OpenScoreCardCommand = new MvxCommand(() => OpenScoreCard());
            SaveDataCommand = new MvxAsyncCommand(async () => await _dataIO.SaveParticipantsAsync(Participants));

            // Przypisanie eventów
            _commsService.ScoreReceived += OnScoreReceived;
            _commsService.SerialConnection += OnSerialConnection;

            // Czytanie listy uczestników z pliku z danymi
            Participants = new ObservableCollection<Participant>(await _dataIO.ReadParticipantsAsync());
        }

        private void OpenScoreCard()
        {
            if (SelectedParticipant == null)
                return;
            _scoreService.CalculateScores(SelectedParticipant);
            _navigationService.Navigate<ScoreCardViewModel, Participant>(SelectedParticipant);
        }

        /// <summary>
        /// Ustawia status połączenia po otrzymaniu eventu.
        /// W przypadku błędu nawiguje do okienka które wyświetli jego treść.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSerialConnection(object sender, SerialConnectionEventArgs e)
        {
            ConnectionStatus = e.ConnectionStatus;
            if (e.IsError)
                _navigationService.Navigate<ModalViewModel, string>(e.ErrorMessage);
        }

        /// <summary>
        /// Przekształca wynik otrzymany z portu serial na format używany w aplikacji.
        /// Wynik w formie string na int lub double w zależności do konkurecji.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScoreReceived(object sender, ScoreReceivedEventArgs e)
        {
            var score = new Score();
            if (e.Competition == Competition.Brzuszki || e.Competition == Competition.Podciaganie)
            {
                if (int.TryParse(e.Score, out var qty))
                {
                    score = new Score { Competition = e.Competition, Quantity = qty, Participants = new List<Participant>(Participants) };
                }
                else
                {
                    _logger.LogError("Nie udalo sie przeksztalcic punktacji na int");
                }
            }
            else
            {
                if (double.TryParse(e.Score, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var time))
                {
                    score = new Score { Competition = e.Competition, Time = time, Participants = new List<Participant>(Participants) };
                }
                else
                {
                    _logger.LogError("Nie udalo sie przeksztalcic punktacji na czas");
                }
            }

            _ = ShowReceivedScoreAsync(score);
        }

        /// <summary>
        /// Dodaje otrzymaną punktacje do istniejącego już uczestnika, lub dodaje nowego uczestnika z otrzymaną punktacją.
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        private async Task ShowReceivedScoreAsync(Score score)
        {
            Participant result = await _navigationService.Navigate<ScoreReceivedViewModel, Score, Participant>(score);

            if (result == null)
                return;

            if (Participants.Contains(result))
            {
                var index = Participants.IndexOf(result);
                Participants[index] = result;
            }
            else
            {
                Participants.Add(result);
            }
        }

        private async Task AddNewParticipantAsync()
        {
            Participant result = await _navigationService.Navigate<AddParticipantViewModel, Participant, Participant>(new Participant());
            if (result != null)
                Participants.Add(result);
        }

        public string SelectedSerialPort
        {
            get => _selectedSerialPort;
            set
            {
                _selectedSerialPort = value;
                RaisePropertyChanged(() => SelectedSerialPort);
                RaisePropertyChanged(() => ConnectToPort);
            }
        }

        public Participant SelectedParticipant
        {
            get => _selectedParticipant;
            set
            {
                _selectedParticipant = value;
                RaisePropertyChanged(() => SelectedParticipant);
            }
        }

        public bool ConnectionStatus
        {
            get => _connectionStatus;
            set
            {
                _connectionStatus = value;
                RaisePropertyChanged(() => ConnectionStatus);
            }
        }

        public IMvxAsyncCommand ConnectToPort { get; set; }
        public IMvxCommand Disconnect { get; set; }
        public IMvxAsyncCommand AddParticipantCommand { get; set; }
        public IMvxCommand OpenScoreCardCommand { get; set; }
        public IMvxAsyncCommand SaveDataCommand { get; set; }

        private ObservableCollection<string> _ports;
        public ObservableCollection<string> Ports
        {
            get => _ports;
            set
            {
                _ports = value;
                RaisePropertyChanged(() => Ports);
            }
        }

        private ObservableCollection<Participant> _participants;
        private string _selectedSerialPort;
        private bool _connectionStatus;
        private Participant _selectedParticipant;

        public ObservableCollection<Participant> Participants
        {
            get => _participants; set
            {
                _participants = value;
                RaisePropertyChanged(() => Participants);
            }
        }
    }
}
