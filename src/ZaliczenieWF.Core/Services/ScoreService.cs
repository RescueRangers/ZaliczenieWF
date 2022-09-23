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
                        score.Points = Calculate10x10Score(score, participant.AgeGroup);
                        break;
                    case Competition.Brzuszki:
                        score.Points = CalculateBrzuszkiScore(score, participant.AgeGroup);
                        break;
                    case Competition.Podciaganie:
                        score.Points = CalculatePodciagnieciaScore(score, participant.AgeGroup);
                        break;
                    case Competition.Marszobieg:
                        score.Points = CalculateMarszobiegScore(score, participant.AgeGroup);
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
                    score.MinPoints = 10.6;
                    score.MinScore = "2";
                    break;
                case AgeGroup._21_25:
                    startingValue = 18;
                    minScore = 7.4;
                    score.MinPoints = 9;
                    score.MinScore = "3";
                    break;
                case AgeGroup._26_30:
                    startingValue = 16;
                    minScore = 9;
                    score.MinPoints = 9.8;
                    score.MinScore = "2";
                    break;
                case AgeGroup._31_35:
                    startingValue = 14;
                    minScore = 10.6;
                    score.MinPoints = 11.4;
                    score.MinScore = "2";
                    break;
                case AgeGroup._36_40:
                    startingValue = 12;
                    minScore = 12.2;
                    score.MinPoints = 13;
                    score.MinScore = "2";
                    break;
                case AgeGroup._41_45:
                    startingValue = 10;
                    minScore = 13.8;
                    score.MinPoints = 14.6;
                    score.MinScore = "2";
                    break;
                case AgeGroup._46_50:
                    startingValue = 8;
                    minScore = 15.4;
                    score.MinPoints = 16.2;
                    score.MinScore = "2";
                    break;
                case AgeGroup._51_55:
                    startingValue = 7;
                    minScore = 16.2;
                    score.MinPoints = 17;
                    score.MinScore = "2";
                    break;
                case AgeGroup._56:
                    startingValue = 6;
                    minScore = 17;
                    score.MinPoints = 17.2;
                    score.MinScore = "2";
                    break;
                default:
                    break;
            }

            var calculatedScore = score.Quantity.Value > startingValue
                ? maxScore
                : score.Quantity.Value == 1 ? minScore : score.Quantity.Value < 1 ? 0 : maxScore - (startingValue - score.Quantity.Value) * 0.8;
            if (calculatedScore < score.MinPoints)
                score.Passed = false;
            return Math.Round(calculatedScore, 1);

        }

        private double CalculateMarszobiegScore(Score score, AgeGroup age)
        {
            var maxScore = 44;
            var minScore = 0.1;
            score.MinPoints = 26;
            double startingValue = 0;
            var steps = Math.Floor((score.Time.Value / 1000) / 5);

            switch (age)
            {
                case AgeGroup._20:
                    startingValue = 750 / 5;
                    score.MinScore = "16:15";
                    break;
                case AgeGroup._21_25:
                    startingValue = 720 / 5;
                    score.MinScore = "15:45";
                    break;
                case AgeGroup._26_30:
                    startingValue = 735 / 5;
                    score.MinScore = "16:00";
                    break;
                case AgeGroup._31_35:
                    startingValue = 750 / 5;
                    score.MinScore = "16:15";
                    break;
                case AgeGroup._36_40:
                    startingValue = 790 / 5;
                    score.MinScore = "16:55";
                    break;
                case AgeGroup._41_45:
                    startingValue = 850 / 5;
                    score.MinScore = "17:55";
                    break;
                case AgeGroup._46_50:
                    startingValue = 910 / 5;
                    score.MinScore = "18:55";
                    break;
                case AgeGroup._51_55:
                    startingValue = 970 / 5;
                    score.MinScore = "19:55";
                    break;
                case AgeGroup._56:
                    startingValue = 1030 / 5;
                    score.MinScore = "20:55";
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

            if (calculatedScore < score.MinPoints)
                score.Passed = false;
            return Math.Round(calculatedScore, 1);
        }

        private double CalculateBrzuszkiScore(Score score, AgeGroup age)
        {
            var maxScore = 16;
            double startingValue = 0;
            double minScore = 0;
            score.MinPoints = 10;

            switch (age)
            {
                case AgeGroup._20:
                    startingValue = 70;
                    minScore = 2.2;
                    score.MinScore = "40";
                    break;
                case AgeGroup._21_25:
                    startingValue = 80;
                    minScore = 0.2;
                    score.MinScore = "50";
                    break;
                case AgeGroup._26_30:
                    startingValue = 75;
                    minScore = 1.2;
                    score.MinScore = "45";
                    break;
                case AgeGroup._31_35:
                    startingValue = 70;
                    minScore = 2.2;
                    score.MinScore = "40";
                    break;
                case AgeGroup._36_40:
                    startingValue = 65;
                    minScore = 3.2;
                    score.MinScore = "35";
                    break;
                case AgeGroup._41_45:
                    startingValue = 60;
                    minScore = 4.2;
                    score.MinScore = "30";
                    break;
                case AgeGroup._46_50:
                    startingValue = 55;
                    minScore = 5.2;
                    score.MinScore = "25";
                    break;
                case AgeGroup._51_55:
                    startingValue = 49;
                    minScore = 6.4;
                    score.MinScore = "19";
                    break;
                case AgeGroup._56:
                    startingValue = 45;
                    minScore = 7.2;
                    score.MinScore = "15";
                    break;
                default:
                    break;
            }

            var calculatedScore = maxScore - (startingValue - score.Quantity.Value) * 0.2;
            if (calculatedScore < score.MinPoints)
            {
                score.Passed = false;
            }

            return calculatedScore < 0 ? minScore : calculatedScore > maxScore ? maxScore : Math.Round(calculatedScore, 1);
        }

        private double Calculate10x10Score(Score score, AgeGroup age)
        {
            double startingValue = 0;
            score.MinPoints = 2.2;

            switch (age)
            {
                case AgeGroup._20:
                    startingValue = 29;
                    score.MinScore = "00:33,20";
                    break;
                case AgeGroup._21_25:
                    startingValue = 28.6;
                    score.MinScore = "00:32,80";
                    break;
                case AgeGroup._26_30:
                    startingValue = 28.9;
                    score.MinScore = "00:33,10";
                    break;
                case AgeGroup._31_35:
                    startingValue = 29.2;
                    score.MinScore = "00:33,40";
                    break;
                case AgeGroup._36_40:
                    startingValue = 29.5;
                    score.MinScore = "00:33,70";
                    break;
                case AgeGroup._41_45:
                    startingValue = 30.1;
                    score.MinScore = "00:34,30";
                    break;
                case AgeGroup._46_50:
                    startingValue = 30.4;
                    score.MinScore = "00:34,60";
                    break;
                case AgeGroup._51_55:
                    startingValue = 30.7;
                    score.MinScore = "00:34,90";
                    break;
                case AgeGroup._56:
                    startingValue = 31.7;
                    score.MinScore = "00:35,90";
                    break;
                default:
                    break;
            }

            var time = Math.Round(score.Time.Value / 1000, 1);
            var calculatedScore = 19 - ((time - startingValue) * 4);
            if (calculatedScore < score.MinPoints)
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

