using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using ZaliczenieWF.Core.ViewModels.Main;

namespace ZaliczenieWF.WPF.Views
{
    /// <summary>
    /// Interaction logic for ModalView.xaml
    /// </summary>
    ///
    [MvxViewFor(typeof(ModalViewModel))]
    [MvxWindowPresentation(Identifier = nameof(ModalView), Modal = true)]
    public partial class ModalView : MvxWindow
    {
        public ImageSource ErrorIcon { get; set; }

        public ModalView()
        {
            InitializeComponent();
            ErrorIcon = ToImageSource(SystemIcons.Error);
            errorImage.Source = ErrorIcon;
        }

        private ImageSource ToImageSource(Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }

        private void MvxWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
