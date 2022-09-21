using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using ZaliczenieWF.Core.ViewModels.Main;

namespace ZaliczenieWF.WPF.Views
{
    /// <summary>
    /// Interaction logic for _10x10View.xaml
    /// </summary>

    [MvxContentPresentation]
    [MvxViewFor(typeof(MainViewModel))]
    public partial class ParticipantsWindow : MvxWpfView
    {
        public ParticipantsWindow()
        {
            InitializeComponent();
        }
    }
}
