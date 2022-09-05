using System.Collections.Generic;

namespace ZaliczenieWF.Core.Models
{

    public class Participant
    {
        public string Stopien { get; set; }
        public string Name { get; set; }
        public string PESEL { get; set; }
        public string Kolumna { get; set; }
        public double Time { get; set; }
        public double Score { get; set; }

        public List<string> Kolumny { get; set; } = new List<string> { "I", "II", "III", "IV", "V" };
    }
}

