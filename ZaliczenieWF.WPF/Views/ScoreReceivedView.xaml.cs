using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.ViewModels;
using ZaliczenieWF.Core.ViewModels.Main;

namespace ZaliczenieWF.WPF.Views
{
    /// <summary>
    /// Interaction logic for ScoreReceivedView.xaml
    /// </summary>
    ///
    [MvxViewFor(typeof(ScoreReceivedViewModel))]
    [MvxContentPresentation(WindowIdentifier = nameof(ScoreReceivedView))]
    public partial class ScoreReceivedView
    {
        public ScoreReceivedView()
        {
            InitializeComponent();
        }
    }
}
