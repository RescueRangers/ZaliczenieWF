using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Commands;
using ZaliczenieWF.Core.Events;
using ZaliczenieWF.Core.Models;
using ZaliczenieWF.Core.Services;
using MvvmCross.Navigation;
using System.Globalization;
using System.Threading;
using System.Linq;

namespace ZaliczenieWF.Core.ViewModels.Main
{
    public class MainViewModel : BaseViewModel
    {
        private IScoreService _scoreService;
        private ICommsService _commsService;
        private readonly IMvxNavigationService _navigationService;
        private ILogger<MainViewModel> _logger;

        public MainViewModel(IScoreService scoreService, ILogger<MainViewModel> logger, ICommsService commsService, IMvxNavigationService navigationService)
        {
            _scoreService = scoreService;
            _logger = logger;
            _commsService = commsService;
            _navigationService = navigationService;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            _participants = new ObservableCollection<Participant>();
            _ports = new ObservableCollection<string>(_commsService.EnumerateSerialPorts());

            ConnectToPort = new MvxAsyncCommand(async () => await _commsService.Connect(SelectedSerialPort));
            Disconnect = new MvxCommand(() => _commsService.Disconnect(SelectedSerialPort));
            AddParticipantCommand = new MvxAsyncCommand(async () => await AddNewParticipantAsync());
            OpenScoreCardCommand = new MvxCommand(() => OpenScoreCard());

            _commsService.ScoreReceived += OnScoreReceived;
            _commsService.SerialConnection += OnSerialConnection;

#if DEBUG
            _participants.Add(new Participant { Name = "Test Testinski", Kolumna = "I", Stopien = "Szeregowy", PESEL = "86110107019", JednostkaWojskowa = "JW"});
            DebugEventCommand = new MvxCommand(() => OnScoreReceived(this, new ScoreReceivedEventArgs { Competition = Competition.Brzuszki, Score = "22" }));
#endif
        }

        private void OpenScoreCard()
        {
            if (SelectedParticipant == null)
                return;
            _scoreService.CalculateScores(SelectedParticipant);
            _navigationService.Navigate<ScoreCardViewModel, Participant>(SelectedParticipant);
        }

        private void OnSerialConnection(object sender, SerialConnectionEventArgs e)
        {
            ConnectionStatus = e.ConnectionStatus;
            if (e.IsError)
                _navigationService.Navigate<ModalViewModel, string>(e.ErrorMessage);
        }

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
        public IMvxCommand DebugEventCommand { get; set; }

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
