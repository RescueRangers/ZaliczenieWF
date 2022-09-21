using System;
using ZaliczenieWF.Models;

namespace ZaliczenieWF.Core.Services
{
    public class ScoreService : IScoreService
    {
        public double CalculateScores(Participant participant)
        {
            double calculatedScore = 0;
            int[] peselNumber = GetPeselNumber(participant.PESEL);

            var yearOfBirth = 1900 + peselNumber[0] * 10 + peselNumber[1];
            if (peselNumber[2] >= 2 && peselNumber[2] < 8)
            {
                double year = peselNumber[2] / 2;
                yearOfBirth += (int)(Math.Floor(year) * 100);
            }
            if (peselNumber[2] >= 8)
            {
                yearOfBirth -= 100;
            }
            var age = DateTime.Now.Year - yearOfBirth;
            participant.AgeGroup = CalculateAgeBracket(age);

            foreach (Score score in participant.Scores)
            {
                switch (score.Competition)
                {
                    case Competition._10x10:
                        score.CalculatedScore = Calculate10x10Score(score, participant.AgeGroup);
                        break;
                    case Competition.Brzuszki:
                        score.CalculatedScore = CalculateBrzuszkiScore(score, participant.AgeGroup);
                        break;
                    case Competition.Podciaganie:
                        score.CalculatedScore = CalculatePodciagnieciaScore(score, participant.AgeGroup);
                        break;
                    case Competition.Marszobieg:
                        score.CalculatedScore = CalculateMarszobiegScore(score, participant.AgeGroup);
                        break;
                    case Competition.Null:
                        break;
                    default:
                        break;
                }
            }

            return Math.Round(calculatedScore, 1);
        }

        private int[] GetPeselNumber(string pESEL)
        {
            int[] peselNumber = new int[11];

            for (var i = 0; i < pESEL.Length; i++)
            {
                peselNumber[i] = int.Parse(pESEL[i].ToString());
            }

            return peselNumber;
        }

        private double CalculatePodciagnieciaScore(Score score, AgeGroup age)
        {
            var maxScore = 21;
            var minScore = 7.4;
            double startingValue = 0;

            switch (age)
            {
                case AgeGroup._20:
                    startingValue = 15;
                    minScore = 9.8;
                    score.MinPassingScore = 10.6;
                    break;
                case AgeGroup._21_25:
                    startingValue = 18;
                    minScore = 7.4;
                    score.MinPassingScore = 9;
                    break;
                case AgeGroup._26_30:
                    startingValue = 16;
                    minScore = 9;
                    score.MinPassingScore = 9.8;
                    break;
                case AgeGroup._31_35:
                    startingValue = 14;
                    minScore = 10.6;
                    score.MinPassingScore = 11.4;
                    break;
                case AgeGroup._36_40:
                    startingValue = 12;
                    minScore = 12.2;
                    score.MinPassingScore = 13;
                    break;
                case AgeGroup._41_45:
                    startingValue = 10;
                    minScore = 13.8;
                    score.MinPassingScore = 14.6;
                    break;
                case AgeGroup._46_50:
                    startingValue = 8;
                    minScore = 15.4;
                    score.MinPassingScore = 16.2;
                    break;
                case AgeGroup._51_55:
                    startingValue = 7;
                    minScore = 16.2;
                    score.MinPassingScore = 17;
                    break;
                case AgeGroup._56:
                    startingValue = 6;
                    minScore = 17;
                    score.MinPassingScore = 17.2;
                    break;
                default:
                    break;
            }

            var calculatedScore = score.Quantity.Value > startingValue
                ? maxScore
                : score.Quantity.Value == 1 ? minScore : score.Quantity.Value < 1 ? 0 : maxScore - (startingValue - score.Quantity.Value) * 0.8;
            if (calculatedScore < score.MinPassingScore)
                score.Passed = false;
            return Math.Round(calculatedScore, 1);

        }

        private double CalculateMarszobiegScore(Score score, AgeGroup age)
        {
            var maxScore = 44;
            var minScore = 0.1;
            score.MinPassingScore = 26;
            double startingValue = 0;
            var steps = Math.Floor((score.Time.Value / 1000) / 5);

            switch (age)
            {
                case AgeGroup._20:
                    startingValue = 750 / 5;
                    break;
                case AgeGroup._21_25:
                    startingValue = 720 / 5;
                    break;
                case AgeGroup._26_30:
                    startingValue = 735 / 5;
                    break;
                case AgeGroup._31_35:
                    startingValue = 750 / 5;
                    break;
                case AgeGroup._36_40:
                    startingValue = 790 / 5;
                    break;
                case AgeGroup._41_45:
                    startingValue = 850 / 5;
                    break;
                case AgeGroup._46_50:
                    startingValue = 910 / 5;
                    break;
                case AgeGroup._51_55:
                    startingValue = 970 / 5;
                    break;
                case AgeGroup._56:
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

            if (calculatedScore < score.MinPassingScore)
                score.Passed = false;
            return Math.Round(calculatedScore, 1);
        }

        private double CalculateBrzuszkiScore(Score score, AgeGroup age)
        {
            var maxScore = 16;
            double startingValue = 0;
            double minScore = 0;
            score.MinPassingScore = 10;

            switch (age)
            {
                case AgeGroup._20:
                    startingValue = 70;
                    minScore = 2.2;
                    break;
                case AgeGroup._21_25:
                    startingValue = 80;
                    minScore = 0.2;
                    break;
                case AgeGroup._26_30:
                    startingValue = 75;
                    minScore = 1.2;
                    break;
                case AgeGroup._31_35:
                    startingValue = 70;
                    minScore = 2.2;
                    break;
                case AgeGroup._36_40:
                    startingValue = 65;
                    minScore = 3.2;
                    break;
                case AgeGroup._41_45:
                    startingValue = 60;
                    minScore = 4.2;
                    break;
                case AgeGroup._46_50:
                    startingValue = 55;
                    minScore = 5.2;
                    break;
                case AgeGroup._51_55:
                    startingValue = 49;
                    minScore = 6.4;
                    break;
                case AgeGroup._56:
                    startingValue = 45;
                    minScore = 7.2;
                    break;
                default:
                    break;
            }

            var calculatedScore = maxScore - (startingValue - score.Quantity.Value) * 0.2;
            if (calculatedScore < score.MinPassingScore)
            {
                score.Passed = false;
            }

            return calculatedScore < 0 ? minScore : calculatedScore > maxScore ? maxScore : Math.Round(calculatedScore, 1);
        }

        private double Calculate10x10Score(Score score, AgeGroup age)
        {
            double startingValue = 0;
            score.MinPassingScore = 2.2;

            switch (age)
            {
                case AgeGroup._20:
                    startingValue = 29;
                    break;
                case AgeGroup._21_25:
                    startingValue = 28.6;
                    break;
                case AgeGroup._26_30:
                    startingValue = 28.9;
                    break;
                case AgeGroup._31_35:
                    startingValue = 29.2;
                    break;
                case AgeGroup._36_40:
                    startingValue = 29.5;
                    break;
                case AgeGroup._41_45:
                    startingValue = 30.1;
                    break;
                case AgeGroup._46_50:
                    startingValue = 30.4;
                    break;
                case AgeGroup._51_55:
                    startingValue = 30.7;
                    break;
                case AgeGroup._56:
                    startingValue = 31.7;
                    break;
                default:
                    break;
            }

            var time = Math.Round(score.Time.Value / 1000, 1);
            var calculatedScore = 19 - ((time - startingValue) * 4);
            if (calculatedScore < score.MinPassingScore)
                score.Passed = false;

            return calculatedScore < 0 ? 0 : calculatedScore > 19 ? 19 : Math.Round(calculatedScore, 1);
        }

        private AgeGroup CalculateAgeBracket(int age)
        {
            if (age <= 20)
            {
                return AgeGroup._20;
            }
            if (age <= 25)
            {
                return AgeGroup._21_25;
            }
            if (age <= 30)
            {
                return AgeGroup._26_30;
            }
            if (age <= 35)
            {
                return AgeGroup._31_35;
            }
            if (age <= 40)
            {
                return AgeGroup._36_40;
            }
            if (age <= 45)
            {
                return AgeGroup._41_45;
            }
            if (age <= 50)
            {
                return AgeGroup._46_50;
            }
            if (age <= 55)
            {
                return AgeGroup._51_55;
            }
            return AgeGroup._56;
        }


    }
}

