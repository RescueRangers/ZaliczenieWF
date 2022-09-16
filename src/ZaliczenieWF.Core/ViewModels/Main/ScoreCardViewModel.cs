using System;
using System.Collections.Generic;
using System.Text;
using ZaliczenieWF.Core.Models;

namespace ZaliczenieWF.Core.ViewModels.Main
{
    public class ScoreCardViewModel : BaseViewModel<Participant>
    {
        private Participant _participant;

        public override void Prepare(Participant parameter)
        {
            Participant = parameter;
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
    }
}
