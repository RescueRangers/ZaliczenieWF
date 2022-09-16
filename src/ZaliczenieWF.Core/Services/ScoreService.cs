using System;
using ZaliczenieWF.Core.Models;

namespace ZaliczenieWF.Core.Services{
    public class ScoreService : IScoreService
    {
        public double CalculateScores(Participant participant)
        {
            double calculatedScore = 0;
            var age = DateTime.Now.Year - (int.Parse(participant.PESEL.Substring(0, 4)));
            foreach (Score score in participant.Scores)
            {
                switch (score.Competition)
                {
                    case Competition._10x10:
                        score.CalculatedScore = Calculate10x10Score(score, age);
                        break;
                    case Competition.Brzuszki:
                        score.CalculatedScore = CalculateBrzuszkiScore(score, age);
                        break;
                    case Competition.Podciaganie:
                        score.CalculatedScore = CalculatePodciagnieciaScore(score, age);
                        break;
                    case Competition.Marszobieg:
                        score.CalculatedScore = CalculateMarszobiegScore(score, age);
                        break;
                    case Competition.Null:
                        break;
                    default:
                        break;
                }
            }

            return calculatedScore;
        }

        private double CalculatePodciagnieciaScore(Score score, int age)
        {
            var bracket = CalculateAgeBracket(age);
            var maxScore = 21;
            var minScore = 7.4;
            double startingValue = 0;

            switch (bracket)
            {
                case 1:
                    startingValue = 15;
                    minScore = 9.8;
                    break;
                case 2:
                    startingValue = 18;
                    minScore = 7.4;
                    break;
                case 3:
                    startingValue = 16;
                    minScore = 9;
                    break;
                case 4:
                    startingValue = 14;
                    minScore = 10.6;
                    break;
                case 5:
                    startingValue = 12;
                    minScore = 12.2;
                    break;
                case 6:
                    startingValue = 10;
                    minScore = 13.8;
                    break;
                case 7:
                    startingValue = 8;
                    minScore = 15.4;
                    break;
                case 8:
                    startingValue = 7;
                    minScore = 16.2;
                    break;
                case 9:
                    startingValue = 6;
                    minScore = 17;
                    break;
                default:
                    break;
            }

            return score.Quantity > startingValue
                ? maxScore
                : score.Quantity == 1 ? minScore : score.Quantity < 1 ? 0 : maxScore - (startingValue - score.Quantity) * 0.8;

        }

        private double CalculateMarszobiegScore(Score score, int age)
        {
            var bracket = CalculateAgeBracket(age);
            var maxScore = 44;
            var minScore = 0.1;
            double startingValue = 0;
            var steps = Math.Floor(score.Time / 5);

            switch (bracket)
            {
                case 1:
                    startingValue = 750 / 5;
                    break;
                case 2:
                    startingValue = 720 / 5;
                    break;
                case 3:
                    startingValue = 735 / 5;
                    break;
                case 4:
                    startingValue = 750 / 5;
                    break;
                case 5:
                    startingValue = 790 / 5;
                    break;
                case 6:
                    startingValue = 850 / 5;
                    break;
                case 7:
                    startingValue = 910 / 5;
                    break;
                case 8:
                    startingValue = 970 / 5;
                    break;
                case 9:
                    startingValue = 1030 / 5;
                    minScore = 0.4;
                    break;
                default:
                    break;
            }

            if (steps < 144)
                return maxScore;

            var offset = steps - startingValue;
            if (offset > 65)
            {
                maxScore = 18;
                offset -= 65;
            }

            var calculatedScore = maxScore - (offset * 0.4);
            if (calculatedScore <= 0)
            {
                calculatedScore = minScore;
            }
            return calculatedScore;
        }

        private double CalculateBrzuszkiScore(Score score, int age)
        {
            var bracket = CalculateAgeBracket(age);
            var maxScore = 21;
            double startingValue = 0;
            double minScore = 0;

            switch (bracket)
            {
                case 1:
                    startingValue = 65;
                    minScore = 2.8;
                    break;
                case 2:
                    startingValue = 74;
                    minScore = 0.1;
                    break;
                case 3:
                    startingValue = 70;
                    minScore = 1.3;
                    break;
                case 4:
                    startingValue = 65;
                    minScore = 2.8;
                    break;
                case 5:
                    startingValue = 60;
                    minScore = 3.3;
                    break;
                case 6:
                    startingValue = 54;
                    minScore = 5.1;
                    break;
                case 7:
                    startingValue = 48;
                    minScore = 6.9;
                    break;
                case 8:
                    startingValue = 42;
                    minScore = 8.7;
                    break;
                case 9:
                    startingValue = 36;
                    minScore = 10.5;
                    break;
                default:
                    break;
            }

            var calculatedScore = (startingValue - score.Quantity) * 0.3;

            return calculatedScore < 0 ? minScore : calculatedScore > maxScore ? maxScore : calculatedScore;
        }

        private double Calculate10x10Score(Score score, int age)
        {
            var bracket = CalculateAgeBracket(age);
            double startingValue = 0;

            switch (bracket)
            {
                case 1:
                    startingValue = 29;
                    break;
                case 2:
                    startingValue = 28.6;
                    break;
                case 3:
                    startingValue = 28.9;
                    break;
                case 4:
                    startingValue = 29.2;
                    break;
                case 5:
                    startingValue = 29.5;
                    break;
                case 6:
                    startingValue = 30.1;
                    break;
                case 7:
                    startingValue = 30.4;
                    break;
                case 8:
                    startingValue = 30.7;
                    break;
                case 9:
                    startingValue = 31.7;
                    break;
                default:
                    break;
            }

            var time = Math.Round(score.Time, 1);
            var calculatedScore = 19 - ((time - startingValue) * 4);

            return calculatedScore < 0 ? 0 : calculatedScore > 19 ? 19 : calculatedScore;
        }

        private int CalculateAgeBracket(int age)
        {
            if (age <= 20)
            {
                return 1;
            }
            if (age <= 25)
            {
                return 2;
            }
            if (age <= 30)
            {
                return 3;
            }
            if (age <= 35)
            {
                return 4;
            }
            if (age <= 40)
            {
                return 5;
            }
            if (age <= 45)
            {
                return 6;
            }
            if (age <= 50)
            {
                return 7;
            }
            if (age <= 55)
            {
                return 8;
            }
            return 9;
        }
    }
}

