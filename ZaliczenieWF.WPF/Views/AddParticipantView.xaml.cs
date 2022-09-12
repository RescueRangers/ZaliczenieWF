using System.Windows.Controls;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using ZaliczenieWF.Core.ViewModels.Main;

namespace ZaliczenieWF.WPF.Views
{
    /// <summary>
    /// Interaction logic for AddParticipantView.xaml
    /// </summary>
    [MvxContentPresentation(WindowIdentifier = nameof(ParticipantsWindow))]
    public partial class AddParticipantView

    {
        public AddParticipantView()
        {
            InitializeComponent();
        }

        private void TextBox_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Focus();
            }
        }
    }
}
