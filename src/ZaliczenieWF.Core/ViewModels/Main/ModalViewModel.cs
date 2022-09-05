using System;
using System.Collections.Generic;
using System.Text;

namespace ZaliczenieWF.Core.ViewModels.Main
{
    public class ModalViewModel : BaseViewModel<string>
    {
        private string _message;

        public string Message
        {
            get => _message;
            set
            {
                RaisePropertyChanged(() => Message);
                _message = value;
            }
        }

        public override void Prepare(string parameter)
        {
            Message = parameter;
        }
    }
}
