using System.ComponentModel;

namespace ZaliczenieWF.Models
{
    public enum AgeGroup
    {
        [Description("Grupa I")]
        _20 = 1,
        [Description("Grupa II")]
        _21_25 = 2,
        [Description("Grupa III")]
        _26_30 = 3,
        [Description("Grupa IV")]
        _31_35 = 4,
        [Description("Grupa V")]
        _36_40 = 5,
        [Description("Grupa VI")]
        _41_45 = 6,
        [Description("Grupa VII")]
        _46_50 = 7,
        [Description("Grupa VII")]
        _51_55 = 8,
        [Description("Grupa IX")]
        _56 = 9
    }
}
