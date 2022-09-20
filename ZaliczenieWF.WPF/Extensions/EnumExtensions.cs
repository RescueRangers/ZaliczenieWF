using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace ZaliczenieWF.WPF.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                    if (Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                        return attr.Description;
            }
            return null;
        }
    }
}
