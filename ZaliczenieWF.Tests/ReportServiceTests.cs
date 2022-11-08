using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZaliczenieWF.Core.Services;
using ZaliczenieWF.Models;

namespace ZaliczenieWF.Tests
{
    internal class ReportServiceTests
    {
        private Participant _participant;


        [SetUp]
        public void Setup()
        {
            _participant = new Participant
            {
                PESEL = "00261343755",
                Name = "Test Testinski",
                JednostkaWojskowa = "JW44",
                Stopien = "Szeregowy",
                AgeGroup = AgeGroup._21_25,
                Kolumna = "I",
                Scores = new System.Collections.ObjectModel.ObservableCollection<Score>
                        {
                            new Score
                            {
                                Competition = Competition.Marszobieg,
                                Time = 1197000,
                                MinPoints = 0,
                                Points = 0,
                                MinScore = "1"
                            },
                            new Score
                            {
                                Competition = Competition._10x10,
                                Time = 34500,
                                MinPoints = 0,
                                Points = 0,
                                MinScore = "1"
                            },
                            new Score
                            {
                                Competition = Competition.Podciaganie,
                                Quantity = 2,
                                MinPoints = 0,
                                Points = 0,
                                MinScore = "1"
                            },
                            new Score
                            {
                                Competition = Competition.Brzuszki,
                                Quantity = 29,
                                MinPoints = 0,
                                Points = 0,
                                MinScore = "1"
                            }
                        }
            };
        }

        /// <summary>
        /// Test zapisywania nowego raportu
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task RaportGenerationAsync()
        {
            var reportingService = new ReportService();

            await reportingService.GeneratePdfReportAsync(_participant);

            Assert.That($"{_participant.Name}.pdf", Does.Exist);

            var file = new FileInfo($"{_participant.Name}.pdf");
            file.Delete();
        }
    }
}
