using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaliczenieWF.Core.Services;
using ZaliczenieWF.Models;

namespace ZaliczenieWF.Tests
{
    internal class DataServiceTests
    {
        private List<Participant> _participants;
        private string _dataFile = "Data.csv";

        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// Test czytania danych z pliku
        /// </summary>
        /// <returns></returns>
        [Test, Order(1)]
        public async Task ReadingDataFileAsync()
        {
            var dataService = new CsvDataService();

            var participants = await dataService.ReadParticipantsAsync();

            Assert.That(participants.Count(), Is.EqualTo(3));

            _participants = participants.ToList();
        }

        /// <summary>
        /// Test zapisywania danych do pliku
        /// </summary>
        /// <returns></returns>
        [Test, Order(2)]
        public async Task SavingDataFileAsync()
        {
            var file = new FileInfo(_dataFile);
            if (file.Exists)
            {
                file.Delete();
            }

            //Upewnia się że plik nie jest używany.
            file.Create().Dispose();

            var dataService = new CsvDataService();
            await dataService.SaveParticipantsAsync(_participants);
            Assert.That(_dataFile, Does.Exist);
        }
    }
}
