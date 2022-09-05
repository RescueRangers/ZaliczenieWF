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

            ConnectToPort = new MvxAsyncCommand(async ()=> await _commsService.Connect(SelectedSerialPort));
            Disconnect = new MvxCommand(()=> _commsService.Disconnect(SelectedSerialPort));

            _commsService.ScoreReceived += ScoreReceived;
        }

        private void ScoreReceived(object sender, ScoreReceivedEventArgs e)
        {
            _navigationService.Navigate<ModalViewModel, string>(e.Score);
            //_logger.LogDebug(e.Score.ToString());
        }

        public string SelectedSerialPort
        {
            get => _selectedSerialPort;
            set
            {
                RaisePropertyChanged(() => SelectedSerialPort);
                RaisePropertyChanged(() => ConnectToPort);
                _selectedSerialPort = value;
            }
        }

        

        public MvxAsyncCommand ConnectToPort{get; set;}
        public MvxCommand Disconnect { get; set; }

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
