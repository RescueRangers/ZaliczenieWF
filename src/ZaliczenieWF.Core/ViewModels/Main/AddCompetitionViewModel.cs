using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ZaliczenieWF.Models;
using ZaliczenieWF.Core.Services;

namespace ZaliczenieWF.Core.ViewModels.Main
{
    public class AddCompetitionViewModel : BaseViewModel<Participant, Score>
    {

        private List<string> _competitions = new List<string> { "10x10", "Brzuszki", "Podciągnięcia", "Marszobieg"};
        private string _selectedCompetition;
        private int? _qtyScore;
        private TimeSpan? _timeScore;
        private bool _isTimeScoreVisible;
        private Competition _selectedEnum = Competition.Null;
        private Participant _participant;
        private readonly IMvxNavigationService _navigationService;

        public AddCompetitionViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Prepare(Participant parameter)
        {
            _participant = parameter;
        }

        public override void Prepare()
        {
            SubmitCommand = new MvxCommand(() =>
            {
                if (_selectedEnum != Competition.Null && (QtyScore != null || TimeScore != null))
                {
                    var score = new Score
                    {
                        Competition = _selectedEnum,
                        Quantity = QtyScore,
                        Time = TimeScore?.TotalMilliseconds
                    };
                    _navigationService.Close(this, score);
                }

            });
            CancelCommand = new MvxCommand(() => _navigationService.Close(this, null));
        }

        public string SelectedCompetition
        {
            get => _selectedCompetition;
            set
            {
                _selectedCompetition = value;
                RaisePropertyChanged(nameof(SelectedCompetition));
                switch (value)
                {
                    case "10x10":
                        IsTimeScoreVisible = true;
                        _selectedEnum = Competition._10x10;
                        break;
                    case "Brzuszki":
                        IsTimeScoreVisible = false;
                        _selectedEnum = Competition.Brzuszki;
                        break;
                    case "Podciągnięcia":
                        IsTimeScoreVisible = false;
                        _selectedEnum = Competition.Podciaganie;
                        break;
                    case "Marszobieg":
                        IsTimeScoreVisible = true;
                        _selectedEnum = Competition.Marszobieg;
                        break;
                    default:
                        break;
                }
            }
        }

        public List<string> Competitions
        {
            get => _competitions;
            set
            {
                _competitions = value;
                RaisePropertyChanged(nameof(Competitions));
            }
        }

        public TimeSpan? TimeScore
        {
            get => _timeScore;
            set
            {
                _timeScore = value;
                RaisePropertyChanged(nameof(TimeScore));
            }
        }
        public int? QtyScore
        {
            get => _qtyScore;
            set
            {
                _qtyScore = value;
                RaisePropertyChanged(nameof(QtyScore));
            }
        }

        public bool IsTimeScoreVisible
        {
            get => _isTimeScoreVisible;
            set
            {
                _isTimeScoreVisible = value;
                TimeScore = value == true ? new TimeSpan() : (TimeSpan?)null;
                RaisePropertyChanged(nameof(IsTimeScoreVisible));
                RaisePropertyChanged(nameof(IsQtyScoreVisible));
            }
        }

        public bool IsQtyScoreVisible => !IsTimeScoreVisible;
        public IMvxCommand SubmitCommand { get; set; }
        public IMvxCommand CancelCommand { get; set; }
    }
}
