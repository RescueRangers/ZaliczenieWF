using System.Windows;

namespace ZaliczenieWF.WPF
{
    public static class Global
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0025:Use expression body for properties", Justification = "No other way to do it")]
        public static Visibility IsDebug {
            get
            {
#if DEBUG
                return Visibility.Visible;
#else
                return Visibility.Collapsed;
#endif
            }
        }
    }
}
