using System;
using System.Collections.Generic;
using ZaliczenieWF.Models.Extensions;

namespace ZaliczenieWF.Models
{
    public class Score : IEquatable<Score>
    {
        public string ParticipantId { get; set; }
        public Participant Participant { get; set; }
        public Competition Competition { get; set; }
        public double? Time { get; set; }
        public int? Quantity { get; set; }
        public List<Participant> Participants { get; set; }
        public double Points { get; set; }
        public bool Passed { get; set; } = true;

        public string ScoreString => GetScore();
        public string CompetitionString => Competition.GetDescription();

        private string GetScore()
        {
            
            if (Competition == Competition.Brzuszki || Competition == Competition.Podciaganie)
            {
                return Quantity.ToString();
            }
            var time = TimeSpan.FromMilliseconds(Time.Value);
            return time.ToString(@"mm\:ss\,ff");
        }
        public string MinScore{ get; set; }
        public double MinPoints { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Score score && Equals(score);
        }
        public override int GetHashCode()
        {
            return Competition.GetHashCode();
        }

        public bool Equals(Score other)
        {
            return Competition == other.Competition;
        }
    }
}
