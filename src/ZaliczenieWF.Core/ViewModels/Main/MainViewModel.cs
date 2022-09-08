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

            _commsService.ScoreReceived += ScoreReceived;
            _commsService.SerialConnection += OnSerialConnection;
        }

        private void OnSerialConnection(object sender, SerialConnectionEventArgs e)
        {
            ConnectionStatus = e.ConnectionStatus;
            if (e.IsError) _navigationService.Navigate<ModalViewModel, string>(e.ErrorMessage);
        }

        private void ScoreReceived(object sender, ScoreReceivedEventArgs e)
        {
            _navigationService.Navigate<ModalViewModel, string>(e.Score);
            //_logger.LogDebug(e.Score.ToString());
        }

        private async Task AddNewParticipantAsync()
        {
            Participant result = await _navigationService.Navigate<AddParticipantViewModel, Participant, Participant>(new Participant());
            if(result != null) Participants.Add(result);
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
