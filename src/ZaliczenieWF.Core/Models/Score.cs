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
        public double? Time { get; set; }
        public int? Quantity { get; set; }
        public List<Participant> Participants { get; set; }

        public double CalculatedScore { get; set; }
        public bool Passed { get; set; } = true;

        public string ScoreString => GetScore();
        public string CompetitionString => GetCompetition();

        private string GetScore()
        {
            if (Competition == Competition.Brzuszki || Competition == Competition.Podciaganie)
            {
                return Quantity.ToString();
            }
            return $"{Time / 1000}s";
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
