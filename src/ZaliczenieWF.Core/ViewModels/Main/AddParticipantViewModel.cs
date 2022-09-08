using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ZaliczenieWF.Core.Models;

namespace ZaliczenieWF.Core.ViewModels.Main
{
    public class AddParticipantViewModel : BaseViewModel<Participant, Participant>
    {
        private Participant _editingParticipant;
        private string _errors;
        private readonly IMvxNavigationService _navigationService;

        public AddParticipantViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public Participant EditingParticipant
        {
            get => _editingParticipant;
            set
            {
                _editingParticipant = value;
                RaisePropertyChanged(() => EditingParticipant);
            }
        }

        public List<string> Stopnie { get; set; } = new List<string>
        {
            "Szeregowy",
            "Starszy Szeregowy",
            "Kapral",
            "Starszy Kapral",
            "Plutonowy",
            "Sierżant",
            "Starszy Sierżant",
            "Młodszy Chhorąży",
            "Chorąży",
            "Starszy Chorąży",
            "Starszy Chorąży Sztabowy"
        };

        public IMvxCommand SubmitCommand { get; set; }
        public IMvxCommand CancelCommand { get; set; }

        public override void Prepare()
        {
            SubmitCommand = new MvxCommand(() =>
            {
                if (!EditingParticipant.HasErrors)
                {
                    SubmitTask = MvxNotifyTask.Create(Submit);
                    RaisePropertyChanged(() => SubmitTask);
                }
                
            });
            CancelCommand = new MvxCommand(() => _navigationService.Close(this, null));
        }

        public MvxNotifyTask SubmitTask { get; private set; }

        private async Task Submit()
        {
            _navigationService.Close(this, EditingParticipant);
        }

        public override void Prepare(Participant parameter)
        {
            EditingParticipant = parameter;
        }


    }
}