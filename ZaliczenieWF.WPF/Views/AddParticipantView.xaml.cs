using System.Windows;
using System.Windows.Controls;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;

namespace ZaliczenieWF.WPF.Views
{
    /// <summary>
    /// Interaction logic for AddParticipantView.xaml
    /// </summary>
    ///
    [MvxWindowPresentation(Identifier = nameof(AddParticipantView), Modal = true)]
    public partial class AddParticipantView : MvxWindow
    {
        public AddParticipantView()
        {
            InitializeComponent();
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Focus();
            }
        }
    }
}
