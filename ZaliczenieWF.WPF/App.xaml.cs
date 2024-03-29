using System.Windows;
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Views;
using ZaliczenieWF.WPF.Views;

namespace ZaliczenieWF.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvxApplication
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<Setup>();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzEyNTc0QDMyMzAyZTMyMmUzMFRiVVVhbXpvWnVJdzV1SmIwa1UxbzY5NEVBUUVyaGxjZVhTMTlKbW84dVE9");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
