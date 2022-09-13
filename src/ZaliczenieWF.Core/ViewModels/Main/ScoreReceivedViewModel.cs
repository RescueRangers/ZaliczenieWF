using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ZaliczenieWF.Core.Models;

namespace ZaliczenieWF.Core.ViewModels.Main
{
    public class ScoreReceivedViewModel : BaseViewModel<Score, Participant>
    {
        private Score _score;
        private IMvxNavigationService _navigationService;
        private ObservableCollection<Participant> _participants;
        private Participant _selectedParticipant;
        private string _error;

        public ScoreReceivedViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Prepare(Score parameter)
        {
            Score = parameter;
            Participants = new ObservableCollection<Participant>(Score.Participants);
        }

        public override void Prepare()
        {
            AddParticipantCommand = new MvxAsyncCommand(async () => await AddNewParticipantAsync());
            SubmitCommand = new MvxAsyncCommand(async () =>
            {
                if (SelectedParticipant.Scores.Contains(Score))
                {
                    UserResponse result = await _navigationService.Navigate<ModalViewModel, string, UserResponse>("Punktacja z tej kategorii już istnieje.\r\nCzy nadpisać punktacje?");
                    if (result != null && result.OverwriteScore)
                    {
                        SelectedParticipant.Scores.Remove(Score);
                        SelectedParticipant.Scores.Add(Score);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    SelectedParticipant.Scores.Add(Score);
                }
                await _navigationService.Close(this, SelectedParticipant);

            });
            CancelCommand = new MvxCommand(() => _navigationService.Close(this, null));
        }

        public Score Score
        {
            get => _score;
            set
            {
                RaisePropertyChanged(() => Score);
                _score = value;
            }
        }

        public Participant SelectedParticipant
        {
            get => _selectedParticipant;
            set
            {
                RaisePropertyChanged(() => SelectedParticipant);
                _selectedParticipant = value;
            }
        }

        public ObservableCollection<Participant> Participants
        {
            get => _participants;
            set
            {
                RaisePropertyChanged(() => Participants);
                _participants = value;
            }
        }

        public string Error
        {
            get => _error;
            set
            {
                RaisePropertyChanged(() => Error);
                _error = value;
            }
        }

        private async Task AddNewParticipantAsync()
        {
            Participant result = await _navigationService.Navigate<AddParticipantViewModel, Participant, Participant>(new Participant());
            if (result != null)
                Participants.Add(result);
        }

        public IMvxAsyncCommand AddParticipantCommand { get; set; }
        public IMvxAsyncCommand SubmitCommand { get; set; }
        public IMvxCommand CancelCommand { get; set; }
    }
}
