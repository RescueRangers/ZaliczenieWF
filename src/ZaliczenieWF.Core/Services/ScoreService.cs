using System;
using ZaliczenieWF.Models;

namespace ZaliczenieWF.Core.Services
{
    public class ScoreService : IScoreService
    {
        public double CalculateScores(Participant participant)
        {
            double calculatedScore = 0;
            var peselNumber = GetPeselNumber(participant.PESEL);

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

            var isMan = peselNumber[10] % 2 == 1;

            foreach (Score score in participant.Scores)
            {
                switch (score.Competition)
                {
                    case Competition._10x10:
                        score.Points = Calculate10x10Score(score, participant.AgeGroup, isMan);
                        break;
                    case Competition.Brzuszki:
                        score.Points = CalculateBrzuszkiScore(score, participant.AgeGroup, isMan);
                        break;
                    case Competition.Podciaganie:
                        score.Points = CalculatePodciagnieciaScore(score, participant.AgeGroup, isMan);
                        break;
                    case Competition.Marszobieg:
                        score.Points = CalculateMarszobiegScore(score, participant.AgeGroup, isMan);
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
            var peselNumber = new int[11];

            for (var i = 0; i < pESEL.Length; i++)
            {
                peselNumber[i] = int.Parse(pESEL[i].ToString());
            }

            return peselNumber;
        }

        private double CalculatePodciagnieciaScore(Score score, AgeGroup age, bool isMan)
        {
            var maxScore = 21;
            var minAvailablePoints = 7.4;
            double startingValue = 0;

            switch (age)
            {
                case AgeGroup._20:
                    if (isMan)
                    {
                        startingValue = 15;
                        minAvailablePoints = 9.8;
                        score.MinPoints = 10.6;
                        score.MinScore = "2"; 
                    }
                    else
                    {
                        startingValue = 5;
                        minAvailablePoints = 17.8;
                        score.MinPoints = 18.6;
                        score.MinScore = "2";
                    }
                    break;
                case AgeGroup._21_25:
                    if (isMan)
                    {
                        startingValue = 18;
                        minAvailablePoints = 7.4;
                        score.MinPoints = 9;
                        score.MinScore = "3"; 
                    }
                    else
                    {
                        startingValue = 8;
                        minAvailablePoints = 15.4;
                        score.MinPoints = 17;
                        score.MinScore = "3";
                    }
                    break;
                case AgeGroup._26_30:
                    if (isMan)
                    {
                        startingValue = 16;
                        minAvailablePoints = 9;
                        score.MinPoints = 9.8;
                        score.MinScore = "2";
                    }
                    else
                    {
                        startingValue = 7;
                        minAvailablePoints = 16.2;
                        score.MinPoints = 17;
                        score.MinScore = "2";
                    }
                    break;
                case AgeGroup._31_35:
                    if (isMan)
                    {
                        startingValue = 14;
                        minAvailablePoints = 10.6;
                        score.MinPoints = 11.4;
                        score.MinScore = "2";
                    }
                    else
                    {
                        startingValue = 6;
                        minAvailablePoints = 17;
                        score.MinPoints = 17.8;
                        score.MinScore = "2";
                    }
                    break;
                case AgeGroup._36_40:
                    if (isMan)
                    {
                        startingValue = 12;
                        minAvailablePoints = 12.2;
                        score.MinPoints = 13;
                        score.MinScore = "2";
                    }
                    else
                    {
                        startingValue = 5;
                        minAvailablePoints = 17.8;
                        score.MinPoints = 18.6;
                        score.MinScore = "2";
                    }
                    break;
                case AgeGroup._41_45:
                    if (isMan)
                    {
                        startingValue = 4;
                        minAvailablePoints = 18.6;
                        score.MinPoints = 19.4;
                        score.MinScore = "2";
                    }
                    else
                    {
                        startingValue = 4;
                        minAvailablePoints = 18.6;
                        score.MinPoints = 19.4;
                        score.MinScore = "2";
                    }
                    break;
                case AgeGroup._46_50:
                    if (isMan)
                    {
                        startingValue = 8;
                        minAvailablePoints = 15.4;
                        score.MinPoints = 16.2;
                        score.MinScore = "2";
                    }
                    else
                    {
                        startingValue = 3;
                        minAvailablePoints =19.4;
                        score.MinPoints = 20.2;
                        score.MinScore = "2";
                    }
                    break;
                case AgeGroup._51_55:
                    if (isMan)
                    {
                        startingValue = 7;
                        minAvailablePoints = 16.2;
                        score.MinPoints = 17;
                        score.MinScore = "2";
                    }
                    else
                    {
                        startingValue = 3;
                        minAvailablePoints = 19.4;
                        score.MinPoints = 20.2;
                        score.MinScore = "2";
                    }
                    break;
                case AgeGroup._56:
                    if (isMan)
                    {
                        startingValue = 6;
                        minAvailablePoints = 17;
                        score.MinPoints = 17.2;
                        score.MinScore = "2";
                    }
                    else
                    {
                        startingValue = 3;
                        minAvailablePoints = 19.4;
                        score.MinPoints = 20.2;
                        score.MinScore = "2";
                    }
                    break;
                default:
                    break;
            }

            var calculatedScore = score.Quantity.Value > startingValue
                ? maxScore
                : score.Quantity.Value == 1 ? minAvailablePoints : score.Quantity.Value < 1 ? 0 : maxScore - (startingValue - score.Quantity.Value) * 0.8;
            if (calculatedScore < score.MinPoints)
                score.Passed = false;
            return Math.Round(calculatedScore, 1);

        }

        private double CalculateMarszobiegScore(Score score, AgeGroup age, bool isMan)
        {
            var maxScore = 44;
            var minScore = 0.1;
            score.MinPoints = 26;
            double startingValue = 0;
            var steps = Math.Floor((score.Time.Value / 1000) / 5);

            switch (age)
            {
                case AgeGroup._20:
                    if (isMan)
                    {
                        startingValue = 750 / 5;
                        score.MinScore = "16:15"; 
                    }
                    else
                    {
                        startingValue = 870 / 5;
                        score.MinScore = "23:25";
                    }
                    break;
                case AgeGroup._21_25:
                    if (isMan)
                    {
                        startingValue = 720 / 5;
                        score.MinScore = "15:45"; 
                    }
                    else
                    {
                        startingValue = 840 / 5;
                        score.MinScore = "22:55";
                    }
                    break;
                case AgeGroup._26_30:
                    if (isMan)
                    {
                        startingValue = 735 / 5;
                        score.MinScore = "16:00";
                    }
                    else
                    {
                        startingValue = 855 / 5;
                        score.MinScore = "23:10";
                    }
                    break;
                case AgeGroup._31_35:
                    if (isMan)
                    {
                        startingValue = 750 / 5;
                        score.MinScore = "16:15";
                    }
                    else
                    {
                        startingValue = 870 / 5;
                        score.MinScore = "23:25";
                    }
                    break;
                case AgeGroup._36_40:
                    if (isMan)
                    {
                        startingValue = 790 / 5;
                        score.MinScore = "16:55";
                    }
                    else
                    {
                        startingValue = 910 / 5;
                        score.MinScore = "24:05";
                    }
                    break;
                case AgeGroup._41_45:
                    if (isMan)
                    {
                        startingValue = 850 / 5;
                        score.MinScore = "17:55";
                    }
                    else
                    {
                        startingValue = 980 / 5;
                        score.MinScore = "25:15";
                    }
                    break;
                case AgeGroup._46_50:
                    if (isMan)
                    {
                        startingValue = 910 / 5;
                        score.MinScore = "18:55";
                    }
                    else
                    {
                        startingValue = 1040 / 5;
                        score.MinScore = "26:15";
                    }
                    break;
                case AgeGroup._51_55:
                    if (isMan)
                    {
                        startingValue = 970 / 5;
                        score.MinScore = "19:55";
                    }
                    else
                    {
                        startingValue = 1110 / 5;
                        score.MinScore = "27:25";
                    }
                    break;
                case AgeGroup._56:
                    if (isMan)
                    {
                        startingValue = 1030 / 5;
                        score.MinScore = "20:55";
                        minScore = 0.4;
                    }
                    else
                    {
                        startingValue = 1140 / 5;
                        score.MinScore = "27:55";
                        minScore = 0.1;
                    }
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

        private double CalculateBrzuszkiScore(Score score, AgeGroup age, bool isMan)
        {
            var maxScore = 16;
            double startingValue = 0;
            double minScore = 0;
            score.MinPoints = 10;

            switch (age)
            {
                case AgeGroup._20:
                    if (isMan)
                    {
                        startingValue = 70;
                        minScore = 2.2;
                        score.MinScore = "40"; 
                    }
                    else
                    {
                        startingValue = 50;
                        minScore = 6.2;
                        score.MinScore = "25";
                    }
                    break;
                case AgeGroup._21_25:
                    if (isMan)
                    {
                        startingValue = 80;
                        minScore = 0.2;
                        score.MinScore = "50"; 
                    }
                    else
                    {
                        startingValue = 60;
                        minScore = 4.2;
                        score.MinScore = "35";
                    }
                    break;
                case AgeGroup._26_30:
                    if (isMan)
                    {
                        startingValue = 75;
                        minScore = 1.2;
                        score.MinScore = "45";
                    }
                    else
                    {
                        startingValue = 55;
                        minScore = 5.2;
                        score.MinScore = "30";
                    }
                    break;
                case AgeGroup._31_35:
                    if (isMan)
                    {
                        startingValue = 70;
                        minScore = 2.2;
                        score.MinScore = "40";
                    }
                    else
                    {
                        startingValue = 44;
                        minScore = 7.4;
                        score.MinScore = "19";
                    }
                    break;
                case AgeGroup._36_40:
                    if (isMan)
                    {
                        startingValue = 65;
                        minScore = 3.2;
                        score.MinScore = "35";
                    }
                    else
                    {
                        startingValue = 42;
                        minScore = 7.8;
                        score.MinScore = "17";
                    }
                    break;
                case AgeGroup._41_45:
                    if (isMan)
                    {
                        startingValue = 60;
                        minScore = 4.2;
                        score.MinScore = "30";
                    }
                    else
                    {
                        startingValue = 40;
                        minScore = 8.2;
                        score.MinScore = "15";
                    }
                    break;
                case AgeGroup._46_50:
                    if (isMan)
                    {
                        startingValue = 55;
                        minScore = 5.2;
                        score.MinScore = "25";
                    }
                    else
                    {
                        startingValue = 38;
                        minScore = 8.6;
                        score.MinScore = "13";
                    }
                    break;
                case AgeGroup._51_55:
                    if (isMan)
                    {
                        startingValue = 49;
                        minScore = 6.4;
                        score.MinScore = "19";
                    }
                    else
                    {
                        startingValue = 36;
                        minScore = 9;
                        score.MinScore = "11";
                    }
                    break;
                case AgeGroup._56:
                    if (isMan)
                    {
                        startingValue = 45;
                        minScore = 7.2;
                        score.MinScore = "15";
                    }
                    else
                    {
                        startingValue = 34;
                        minScore = 9.4;
                        score.MinScore = "9";
                    }
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

        private double Calculate10x10Score(Score score, AgeGroup age, bool isMan)
        {
            double startingValue = 0;
            score.MinPoints = 2.2;

            switch (age)
            {
                case AgeGroup._20:
                    if (isMan)
                    {
                        startingValue = 29;
                        score.MinScore = "00:33,20"; 
                    }
                    else
                    {
                        startingValue = 31.1;
                        score.MinScore = "00:34,80";
                    }
                    break;
                case AgeGroup._21_25:
                    if (isMan)
                    {
                        startingValue = 28.6;
                        score.MinScore = "00:32,80";
                    }
                    else
                    {
                        startingValue = 30.7;
                        score.MinScore = "00:34,40";
                    }
                    break;
                case AgeGroup._26_30:
                    if (isMan)
                    {
                        startingValue = 28.9;
                        score.MinScore = "00:33,10";
                    }
                    else
                    {
                        startingValue = 31;
                        score.MinScore = "00:34,70";
                    }
                    break;
                case AgeGroup._31_35:
                    if (isMan)
                    {
                        startingValue = 29.2;
                        score.MinScore = "00:33,40";
                    }
                    else
                    {
                        startingValue = 31.3;
                        score.MinScore = "00:35,00";
                    }
                    break;
                case AgeGroup._36_40:
                    if (isMan)
                    {
                        startingValue = 29.5;
                        score.MinScore = "00:33,70";
                    }
                    else
                    {
                        startingValue = 31.6;
                        score.MinScore = "00:35,30";
                    }
                    break;
                case AgeGroup._41_45:
                    if (isMan)
                    {
                        startingValue = 30.1;
                        score.MinScore = "00:34,30";
                    }
                    else
                    {
                        startingValue = 32.3;
                        score.MinScore = "00:36,00";
                    }
                    break;
                case AgeGroup._46_50:
                    if (isMan)
                    {
                        startingValue = 30.4;
                        score.MinScore = "00:34,60";
                    }
                    else
                    {
                        startingValue = 32.7;
                        score.MinScore = "00:36,40";
                    }
                    break;
                case AgeGroup._51_55:
                    if (isMan)
                    {
                        startingValue = 30.7;
                        score.MinScore = "00:34,90";
                    }
                    else
                    {
                        startingValue = 33.1;
                        score.MinScore = "00:36,80";
                    }
                    break;
                case AgeGroup._56:
                    if (isMan)
                    {
                        startingValue = 31.7;
                        score.MinScore = "00:35,90";
                    }
                    else
                    {
                        startingValue = 33.5;
                        score.MinScore = "00:37,50";
                    }
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

