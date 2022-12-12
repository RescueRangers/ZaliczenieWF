using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZaliczenieWF.Models
{
    public enum Competition
    {
        [Description("Bieg wahadłowy 10x10")]
        _10x10 = 1,
        [Description("Skłony tułowia")]
        Brzuszki = 2,
        [Description("Podciąganie")]
        Podciaganie = 3,
        [Description("Marszobieg 3000m")]
        Marszobieg = 4,
        Null = 5
    }
}
