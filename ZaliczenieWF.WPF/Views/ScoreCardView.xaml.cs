using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.ViewModels;
using ZaliczenieWF.Core.ViewModels.Main;

namespace ZaliczenieWF.WPF.Views
{
    /// <summary>
    /// Interaction logic for ScoreCardView.xaml
    /// </summary>
    ///
    [MvxContentPresentation]
    [MvxViewFor(typeof(ScoreCardViewModel))]
    public partial class ScoreCardView
    {
        public ScoreCardView()
        {
            InitializeComponent();
        }
    }
}
