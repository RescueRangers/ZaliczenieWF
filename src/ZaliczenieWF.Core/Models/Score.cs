using System;
using System.Collections.Generic;
using System.Text;
using ZaliczenieWF.Core.Services;

namespace ZaliczenieWF.Core.Models
{
    public class Score : IEquatable<Score>
    {
        public Participant Participant { get; set; }
        public Competition  Competition { get; set; }
        public double Time { get; set; }
        public int Quantity { get; set; }
        public List<Participant> Participants { get; set; }

        public string ScoreString => GetScore();
        public string CompetitionString => GetCompetition();

        private string GetScore()
        {
            if (Competition == Competition.Brzuszki || Competition == Competition.Podciaganie)
            {
                return Quantity.ToString();
            }
            return Time.ToString();
        }

        private string GetCompetition()
        {
            return Competition.ToString();
        }

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
