using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.ViewModels;
using ZaliczenieWF.Core.Services;
using ZaliczenieWF.Core.ViewModels.Main;

namespace ZaliczenieWF.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterType<ICommsService, CommsService>();
            Mvx.IoCProvider.RegisterType<IScoreService, ScoreService>();
            Mvx.IoCProvider.RegisterType<IReportService, ReportService>();
            Mvx.IoCProvider.RegisterSingleton(() => UserDialogs.Instance);
            RegisterAppStart<MainViewModel>();
        }

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzEyNTc0QDMyMzAyZTMyMmUzMFRiVVVhbXpvWnVJdzV1SmIwa1UxbzY5NEVBUUVyaGxjZVhTMTlKbW84dVE9");
        }
    }
}
