using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ZaliczenieWF.Core.Services;
using ZaliczenieWF.Models;

namespace ZaliczenieWF.Core.ViewModels.Main
{
    public class ScoreCardViewModel : BaseViewModel<Participant>
    {
        private Participant _participant;
        private IMvxNavigationService _navigationService;
        private IScoreService _scoreService;
        private IReportService _reportService;
        private bool _isReportGenerating;

        public ScoreCardViewModel(IMvxNavigationService navigationService, IScoreService scoreService, IReportService reportService)
        {
            _navigationService = navigationService;
            _scoreService = scoreService;
            _reportService = reportService;
        }

        public override void Prepare(Participant parameter)
        {
            Participant = parameter;
        }

        public override void Prepare()
        {
            BackCommand = new MvxCommand(() =>
            {
                var scores = _participant.Scores.ToList();
                _participant.Scores = new ObservableCollection<Score>(scores);
                _navigationService.Close(this);
            });
            AddCompetitionCommand = new MvxAsyncCommand(async () => await AddCompetitionAsync());
            GenerateReportCommand = new MvxAsyncCommand(async () => await StartReportGenerationAsync());
        }

        private async Task StartReportGenerationAsync()
        {
            IsReportGenerating = true;
            await _reportService.GeneratePdfReportAsync(Participant);
            IsReportGenerating = false;
        }

        private async Task AddCompetitionAsync()
        {
            Score score = await _navigationService.Navigate<AddCompetitionViewModel, Participant, Score>(_participant);
            if (score == null)
                return;

            if (_participant.Scores.Contains(score))
            {
                UserResponse response = await _navigationService.Navigate<ModalViewModel, string, UserResponse>("Punktacja z tej kategorii już istnieje.\r\nCzy nadpisać punktacje ? ");
                if (response != null && response.OverwriteScore == true)
                {
                    var scores = _participant.Scores.ToList();
                    scores.Remove(score);
                    scores.Add(score);
                    _participant.Scores = new ObservableCollection<Score>(scores);
                    _scoreService.CalculateScores(Participant);
                    await RaisePropertyChanged(nameof(Participant));
                }
            }
            else
            {
                _participant.Scores.Add(score);
                _scoreService.CalculateScores(Participant);
                await RaisePropertyChanged(nameof(Participant));
            }

        }

        public Participant Participant
        {
            get => _participant;
            set
            {
                RaisePropertyChanged(() => Participant);
                _participant = value;
            }
        }

        public bool IsReportGenerating
        {
            get => _isReportGenerating;
            set
            {
                _isReportGenerating = value;
                RaisePropertyChanged(() => IsReportGenerating);
            }
        }

        public IMvxCommand BackCommand { get; set; }
        public IMvxAsyncCommand AddCompetitionCommand { get; set; }
        public IMvxAsyncCommand GenerateReportCommand { get; set; }
    }
}
