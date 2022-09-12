using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ZaliczenieWF.Core.Models;

namespace ZaliczenieWF.Core.ViewModels.Main
{
    public class ScoreReceivedViewModel : BaseViewModel<Score, Score>
    {
        private Score _score;
        private IMvxNavigationService _navigationService;

        public ScoreReceivedViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Prepare(Score parameter)
        {
            Score = parameter;
        }

        public override void Prepare()
        {
            AddParticipantCommand = new MvxAsyncCommand(async () => await AddNewParticipantAsync());
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

        private async Task AddNewParticipantAsync()
        {
            Participant result = await _navigationService.Navigate<AddParticipantViewModel, Participant, Participant>(new Participant());
            if (result != null)
                Score.Participants.Add(result);
        }

        public IMvxAsyncCommand AddParticipantCommand { get; set; }
    }
}
