using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using ZaliczenieWF.Models;
using ZaliczenieWF.WPF.Extensions;

namespace ZaliczenieWF.WPF.Converters
{
    public class EnumToStringConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Score sVal ? sVal.Competition.GetDescription() : (object)"";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
