using Moq;
using ZaliczenieWF.Core.Services;
using ZaliczenieWF.Models;

namespace ZaliczenieWF.Tests
{
    public class ScoreServiceTests
    {
        //Numery PESEL wygenerowane na stronie https://pesel.cstudios.pl/o-generatorze/generator-on-line
        private string[] _malePesels = new string[9]
        {
            "04261365416",
            "00261343755",
            "95061345276",
            "90061373773",
            "85061396174",
            "80061389131",
            "75061375857",
            "70061387871",
            "65061344315"
        };
        private string[] _femalePesels = new string[9]
        {
            "04320377268",
            "01320352662",
            "95120313527",
            "90120399966",
            "85022274365",
            "80120343843",
            "75120313284",
            "70120387284",
            "65120351629"
        };

        private IDateTimeProvider _dateTimeProvider;

        private double[] _maleMarszobiegScoreTable = new double[9] { 16.4, 14, 15.2, 16.4, 20.8, 25.6, 30.4, 35.2, 40 };
        private double[] _male10x10ScoreTable = new double[9] { 6.6, 5, 6.2, 7.4, 8.6, 11, 12.2, 13.4, 17.4 };
        private double[] _malePodciaganieScoreTable = new double[9] { 11.4, 9, 10.6, 12.2, 13.8, 15.4, 17, 17.8, 18.6 };
        private double[] _maleBrzuszkiScoreTable = new double[9] { 10.2, 8.2, 9.2, 10.2, 11.2, 12.2, 13.2, 14.4, 15.2 };

        private double[] _femaleMarszobiegScoreTable = new double[9] { 18, 14.4, 15.6, 18, 21.2, 26.8, 31.6, 37.2, 39.6 };
        private double[] _female10x10ScoreTable = new double[9] { 5.4, 3.8, 5, 6.2, 7.4, 10.2, 11.8, 13.4, 15 };
        private double[] _femalePodciaganieScoreTable = new double[9] { 18.6, 16.2, 17, 17.8, 18.6, 19.4, 20.2, 20.2, 20.2 };
        private double[] _femaleBrzuszkiScoreTable = new double[9] { 11.8, 9.8, 10.8, 13, 13.4, 13.8, 14.2, 14.6, 15 };

        private List<Participant> _maleParticipants = new List<Participant>();
        private List<Participant> _femaleParticipants = new List<Participant>();

        [SetUp]
        public void Setup()
        {
            _dateTimeProvider = Mock.Of<IDateTimeProvider>(d => d.Now == new DateTime(2022, 10, 1));

            foreach (var pesel in _malePesels)
            {
                _maleParticipants.
                    Add(new Participant
                    {
                        PESEL = pesel,
                        Scores = new System.Collections.ObjectModel.ObservableCollection<Score>
                        {
                            new Score
                            {
                                Competition = Competition.Marszobieg,
                                Time = 1080000
                            },
                            new Score
                            {
                                Competition = Competition._10x10,
                                Time = 32100
                            },
                            new Score
                            {
                                Competition = Competition.Podciaganie,
                                Quantity = 3
                            },
                            new Score
                            {
                                Competition = Competition.Brzuszki,
                                Quantity = 41
                            }
                        }
                    });
            }

            foreach (var pesel in _femalePesels)
            {
                _femaleParticipants.Add(new Participant
                {
                    PESEL = pesel,
                    Scores = new System.Collections.ObjectModel.ObservableCollection<Score>
                        {
                            new Score
                            {
                                Competition = Competition.Marszobieg,
                                Time = 1197000
                            },
                            new Score
                            {
                                Competition = Competition._10x10,
                                Time = 34500
                            },
                            new Score
                            {
                                Competition = Competition.Podciaganie,
                                Quantity = 2
                            },
                            new Score
                            {
                                Competition = Competition.Brzuszki,
                                Quantity = 29
                            }
                        }
                });
            }
        }

        /// <summary>
        /// Test wszystkich grup wiekowych w konkurencji Marszobieg mężczyzn
        /// </summary>
        [Test]
        public void ScoreMales_AllAgeGroups_Marszobieg()
        {
            var scoreService = new ScoreService(_dateTimeProvider);

            for (var i = 0; i < 9; i++)
            {
                scoreService.CalculateScores(_maleParticipants[i]);
                Assert.
                    That(_maleParticipants[i].Scores.FirstOrDefault(s => s.Competition == Competition.Marszobieg)?.Points,
                    Is.EqualTo(_maleMarszobiegScoreTable[i]));
            }
        }

        /// <summary>
        /// Test wszystkich grup wiekowych w konkurencji 10x10 mężczyzn
        /// </summary>
        [Test]
        public void ScoreMales_AllAgeGroups_10x10()
        {
            var scoreService = new ScoreService(_dateTimeProvider);

            for (var i = 0; i < 9; i++)
            {
                scoreService.CalculateScores(_maleParticipants[i]);
                Assert.
                    That(_maleParticipants[i].Scores.FirstOrDefault(s => s.Competition == Competition._10x10)?.Points,
                    Is.EqualTo(_male10x10ScoreTable[i]));
            }
        }

        /// <summary>
        /// Test wszystkich grup wiekowych w konkurencji Podciąganie mężczyzn
        /// </summary>
        [Test]
        public void ScoreMales_AllAgeGroups_Podciaganie()
        {
            var scoreService = new ScoreService(_dateTimeProvider);

            for (var i = 0; i < 9; i++)
            {
                scoreService.CalculateScores(_maleParticipants[i]);
                Assert.
                    That(_maleParticipants[i].Scores.FirstOrDefault(s => s.Competition == Competition.Podciaganie)?.Points,
                    Is.EqualTo(_malePodciaganieScoreTable[i]));
            }
        }

        /// <summary>
        /// Test wszystkich grup wiekowych w konkurencji Brzuszki mężczyzn
        /// </summary>
        [Test]
        public void ScoreMales_AllAgeGroups_Brzuszki()
        {
            var scoreService = new ScoreService(_dateTimeProvider);

            for (var i = 0; i < 9; i++)
            {
                scoreService.CalculateScores(_maleParticipants[i]);
                Assert.
                    That(_maleParticipants[i].Scores.FirstOrDefault(s => s.Competition == Competition.Brzuszki)?.Points,
                    Is.EqualTo(_maleBrzuszkiScoreTable[i]));
            }
        }

        /// <summary>
        /// Test wszystkich grup wiekowych w konkurencji Marszobieg kobiet
        /// </summary>
        [Test]
        public void ScoreFemales_AllAgeGroups_Marszobieg()
        {
            var scoreService = new ScoreService(_dateTimeProvider);

            for (var i = 0; i < 9; i++)
            {
                scoreService.CalculateScores(_femaleParticipants[i]);
                Assert.
                    That(_femaleParticipants[i].Scores.FirstOrDefault(s => s.Competition == Competition.Marszobieg)?.Points,
                    Is.EqualTo(_femaleMarszobiegScoreTable[i]));
            }
        }

        /// <summary>
        /// Test wszystkich grup wiekowych w konkurencji 10x10 kobiet
        /// </summary>
        [Test]
        public void ScoreFemales_AllAgeGroups_10x10()
        {
            var scoreService = new ScoreService(_dateTimeProvider);

            for (var i = 0; i < 9; i++)
            {
                scoreService.CalculateScores(_femaleParticipants[i]);
                Assert.
                    That(_femaleParticipants[i].Scores.FirstOrDefault(s => s.Competition == Competition._10x10)?.Points,
                    Is.EqualTo(_female10x10ScoreTable[i]));
            }
        }

        /// <summary>
        /// Test wszystkich grup wiekowych w konkurencji Podciąganie kobiet
        /// </summary>
        [Test]
        public void ScoreFemales_AllAgeGroups_Podciąganie()
        {
            var scoreService = new ScoreService(_dateTimeProvider);

            for (var i = 0; i < 9; i++)
            {
                scoreService.CalculateScores(_femaleParticipants[i]);
                Assert.
                    That(_femaleParticipants[i].Scores.FirstOrDefault(s => s.Competition == Competition.Podciaganie)?.Points,
                    Is.EqualTo(_femalePodciaganieScoreTable[i]));
            }
        }


        /// <summary>
        /// Test wszystkich grup wiekowych w konkurencji Brzuszki kobiet
        /// </summary>
        [Test]
        public void ScoreFemales_AllAgeGroups_Brzuszki()
        {
            var scoreService = new ScoreService(_dateTimeProvider);

            for (var i = 0; i < 9; i++)
            {
                scoreService.CalculateScores(_femaleParticipants[i]);
                Assert.
                    That(_femaleParticipants[i].Scores.FirstOrDefault(s => s.Competition == Competition.Brzuszki)?.Points,
                    Is.EqualTo(_femaleBrzuszkiScoreTable[i]));
            }
        }
    }
}
