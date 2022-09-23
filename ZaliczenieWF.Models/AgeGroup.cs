using System.ComponentModel;

namespace ZaliczenieWF.Models
{
    public enum AgeGroup
    {
        [Description("I")]
        _20 = 1,
        [Description("II")]
        _21_25 = 2,
        [Description("III")]
        _26_30 = 3,
        [Description("IV")]
        _31_35 = 4,
        [Description("V")]
        _36_40 = 5,
        [Description("VI")]
        _41_45 = 6,
        [Description("VII")]
        _46_50 = 7,
        [Description("VII")]
        _51_55 = 8,
        [Description("IX")]
        _56 = 9
    }
}
