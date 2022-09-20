using System;
using System.ComponentModel;
using System.Reflection;
using ZaliczenieWF.Core.Models;

namespace ZaliczenieWF.Core.Services{
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
            if(peselNumber[2] >=8)
            {
                yearOfBirth -= 100;
            }

            var age = DateTime.Now.Year - yearOfBirth;
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

        private int[] GetPeselNumber(string pESEL)
        {
            int[] peselNumber = new int[11];

            for (var i = 0; i < pESEL.Length; i++)
            {
                peselNumber[i] = int.Parse(pESEL[i].ToString());
            }

            return peselNumber;
        }

        private double CalculatePodciagnieciaScore(Score score, int age)
        {
            var bracket = CalculateAgeBracket(age);
            var maxScore = 21;
            var minScore = 7.4;
            double minPassingScore = 0;
            double startingValue = 0;

            switch (bracket)
            {
                case 1:
                    startingValue = 15;
                    minScore = 9.8;
                    minPassingScore = 10.6;
                    break;
                case 2:
                    startingValue = 18;
                    minScore = 7.4;
                    minPassingScore = 9;
                    break;
                case 3:
                    startingValue = 16;
                    minScore = 9;
                    minPassingScore = 9.8;
                    break;
                case 4:
                    startingValue = 14;
                    minScore = 10.6;
                    minPassingScore = 11.4;
                    break;
                case 5:
                    startingValue = 12;
                    minScore = 12.2;
                    minPassingScore = 13;
                    break;
                case 6:
                    startingValue = 10;
                    minScore = 13.8;
                    minPassingScore = 14.6;
                    break;
                case 7:
                    startingValue = 8;
                    minScore = 15.4;
                    minPassingScore = 16.2;
                    break;
                case 8:
                    startingValue = 7;
                    minScore = 16.2;
                    minPassingScore = 17;
                    break;
                case 9:
                    startingValue = 6;
                    minScore = 17;
                    minPassingScore = 17.2;
                    break;
                default:
                    break;
            }

            var calculatedScore = score.Quantity.Value > startingValue
                ? maxScore
                : score.Quantity.Value == 1 ? minScore : score.Quantity.Value < 1 ? 0 : maxScore - (startingValue - score.Quantity.Value) * 0.8;
            if (calculatedScore < minPassingScore)
                score.Passed = false;
            return calculatedScore;

        }

        private double CalculateMarszobiegScore(Score score, int age)
        {
            var bracket = CalculateAgeBracket(age);
            var maxScore = 44;
            var minScore = 0.1;
            double minPassingScore = 26;
            double startingValue = 0;
            var steps = Math.Floor((score.Time.Value / 1000) / 5);

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

            if (calculatedScore < minPassingScore)
                score.Passed = false;
            return calculatedScore;
        }

        private double CalculateBrzuszkiScore(Score score, int age)
        {
            var bracket = CalculateAgeBracket(age);
            var maxScore = 16;
            double startingValue = 0;
            double minScore = 0;
            double minPassingScore = 10;

            switch (bracket)
            {
                case 1:
                    startingValue = 70;
                    minScore = 2.2;
                    break;
                case 2:
                    startingValue = 80;
                    minScore = 0.2;
                    break;
                case 3:
                    startingValue = 75;
                    minScore = 1.2;
                    break;
                case 4:
                    startingValue = 70;
                    minScore = 2.2;
                    break;
                case 5:
                    startingValue = 65;
                    minScore = 3.2;
                    break;
                case 6:
                    startingValue = 60;
                    minScore = 4.2;
                    break;
                case 7:
                    startingValue = 55;
                    minScore = 5.2;
                    break;
                case 8:
                    startingValue = 49;
                    minScore = 6.4;
                    break;
                case 9:
                    startingValue = 45;
                    minScore = 7.2;
                    break;
                default:
                    break;
            }

            var calculatedScore = maxScore - (startingValue - score.Quantity) * 0.2;
            if (calculatedScore < minPassingScore)
            {
                score.Passed = false;
            }

            return (double)(calculatedScore < 0 ? minScore : calculatedScore > maxScore ? maxScore : calculatedScore);
        }

        private double Calculate10x10Score(Score score, int age)
        {
            var bracket = CalculateAgeBracket(age);
            double startingValue = 0;
            double minPassingScore = 2.2;

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

            var time = Math.Round(score.Time.Value / 1000, 1);
            var calculatedScore = 19 - ((time - startingValue) * 4);
            if (calculatedScore < minPassingScore)
                score.Passed = false;

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

