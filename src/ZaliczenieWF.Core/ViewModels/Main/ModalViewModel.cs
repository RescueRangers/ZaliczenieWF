using MvvmCross.Commands;
using MvvmCross.Navigation;
using ZaliczenieWF.Models;

namespace ZaliczenieWF.Core.ViewModels.Main
{
    public class ModalViewModel : BaseViewModel<string, UserResponse>
    {
        private string _message;
        private IMvxNavigationService _navigationService;

        public string Message
        {
            get => _message;
            set
            {
                RaisePropertyChanged(() => Message);
                _message = value;
            }
        }

        public ModalViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Prepare(string parameter)
        {
            Message = parameter;
            CancelCommand = new MvxCommand(() => _navigationService.Close(this));
            SubmitCommand = new MvxAsyncCommand(async () =>
            {
                var result = new UserResponse { OverwriteScore = true };
                await _navigationService.Close(this, result);
            });
        }

        public IMvxAsyncCommand SubmitCommand { get; set; }
        public IMvxCommand CancelCommand { get; set; }
    }
}
