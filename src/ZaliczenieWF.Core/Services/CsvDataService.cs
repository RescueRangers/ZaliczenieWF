using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaliczenieWF.Models;
using ZaliczenieWF.Models.Extensions;

namespace ZaliczenieWF.Core.Services
{
    public class CsvDataService : IDataIOService
    {
        private string _filePath = "Data.csv";

        /// <summary>
        /// Zapisuje uczestników do płaskiego pliku csv.
        /// Każdy uczestnik wraz z konkurencjami i punktacją znajduje się na jednej linii pliku.
        /// </summary>
        /// <param name="participants">Lista uczestników</param>
        /// <returns></returns>
        public async Task SaveParticipantsAsync(IEnumerable<Participant> participants)
        {
            if (!CheckIfFileExists())
            {
                return;
            }

            using var writer = new StreamWriter(_filePath, false, new UTF8Encoding(true));
            // Nagłówek pliku csv.
            await writer.WriteLineAsync("Imie i nazwisko;Stopień;PESEL;Jednowstka Wojskowa;Kolumn;Konkurencja;Minumum;Wynik;Punkty;Konkurencja;Minumum;Wynik;Punkty;Konkurencja;Minumum;Wynik;Punkty;Konkurencja;Minumum;Wynik;Punkty");

            foreach (Participant participant in participants)
            {
                // Zapisuje dane uczestnika bez przechodzenia do nowej linii
                await writer.WriteAsync($"{participant.Name};{participant.Stopien};{participant.PESEL};{participant.JednostkaWojskowa};{participant.Kolumna};");
                // Sortowanie punktacji po konkurencji
                IEnumerable<Score> orderedScores = participant.Scores.OrderBy(x => x.Competition);
                foreach (Score score in orderedScores)
                {
                    // Zapisuje konkurencje i wyniki bez przechodzenia do nowej linii
                    await writer.WriteAsync($"{score.CompetitionString};{score.MinScore};{score.ScoreString};{score.Points};");
                    
                }
                // Przejście do nowej linii
                await writer.WriteLineAsync();
            }
        }

        public void UpdateParticipant(Participant participant)
        {

        }

        public void DeleteParticipant(Participant participant)
        {

        }

        public void AddParticipant(Participant participant)
        {

        }

        /// <summary>
        /// Czyta z pliku listę uczestników
        /// </summary>
        /// <returns>Lista uczestników wraz z konkurencjami.</returns>
        public async Task<IEnumerable<Participant>> ReadParticipantsAsync()
        {
            var participants = new List<Participant>();
            if (!CheckIfFileExists())
            {
                return participants;
            }

            using var reader = new StreamReader(_filePath, new UTF8Encoding(true));

            //Pomija pierwszą linię pliku gdzie znajduje sie nagłówek.
            await reader.ReadLineAsync();

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                var data = line.Split(';');
                var participant = new Participant
                {
                    Name = data[0],
                    Stopien = data[1],
                    PESEL = data[2],
                    JednostkaWojskowa = data[3],
                    Kolumna = data[4]
                };
                var scores = new List<Score>();
                for (var i = 5; i < data.Length-1; i+=4)
                {
                    // Czyta konkurencje z pliku
                    Competition competition = data[i].GetValueFromDescription<Competition>();

                    // Jeżeli wynik konkurencji jest liczbą stałą czyta ją z pliku
                    if (competition == Competition.Brzuszki || competition == Competition.Podciaganie)
                    {
                        scores.Add(new Score
                        {
                            Competition = competition,
                            MinScore = data[i+1],
                            Quantity = int.Parse(data[i+2]),
                            Points = double.Parse(data[i+3])
                        });
                    }
                    // Jeżli punktacja jest czasem odczytuje go używając formatu w którym dane są zapisywane.
                    else
                    {
                        var time = TimeSpan.ParseExact(data[i + 2], @"mm\:ss\,ff", null);
                        scores.Add(new Score
                        {
                            Competition = competition,
                            MinScore = data[i + 1],
                            Time = time.TotalMilliseconds,
                            Points = double.Parse(data[i + 3])
                        });
                    }
                }
                participant.Scores = new ObservableCollection<Score>(scores);
                participants.Add(participant);
            }

            return participants;
        }

        private bool CheckIfFileExists()
        {
            var file = new FileInfo(_filePath);
            if (!file.Exists)
            {
                try
                {
                    file.Create();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
