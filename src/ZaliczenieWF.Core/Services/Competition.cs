using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZaliczenieWF.Core.Services
{
    public enum Competition
    {
        [Description("10x10")]
        [Display(Name = "10x10")]
        _10x10 = 1,
        [Description("Brzuszki")]
        Brzuszki = 2,
        [Description("Podciąganie")]
        Podciaganie = 3,
        [Description("Marszobieg")]
        Marszobieg = 4,
        Null = 5
    }
}
