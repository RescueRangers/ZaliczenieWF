using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using ZaliczenieWF.Core.ViewModels.Main;

namespace ZaliczenieWF.WPF.Views
{
    /// <summary>
    /// Interaction logic for AddNewCompetitionView.xaml
    /// </summary>
    ///
    [MvxViewFor(typeof(AddCompetitionViewModel))]
    [MvxWindowPresentation(Identifier = nameof(AddNewCompetitionView), Modal = true)]
    public partial class AddNewCompetitionView : MvxWindow
    {
        public AddNewCompetitionView()
        {
            InitializeComponent();
        }
    }
}
