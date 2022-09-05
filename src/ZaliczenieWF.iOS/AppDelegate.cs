using Foundation;
using MvvmCross.Platforms.Ios.Core;
using ZaliczenieWF.Core;

namespace ZaliczenieWF.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
    }
}
