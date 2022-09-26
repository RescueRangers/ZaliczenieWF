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
        public async Task SaveParticipantsAsync(IEnumerable<Participant> participants)
        {
            using var writer = new StreamWriter(_filePath, false, new UTF8Encoding(true));

            await writer.WriteLineAsync("Imie i nazwisko;Stopień;PESEL;Jednowstka Wojskowa;Kolumn;Konkurencja;Minumum;Wynik;Punkty;Konkurencja;Minumum;Wynik;Punkty;Konkurencja;Minumum;Wynik;Punkty;Konkurencja;Minumum;Wynik;Punkty");

            foreach (Participant participant in participants)
            {
                await writer.WriteAsync($"{participant.Name};{participant.Stopien};{participant.PESEL};{participant.JednostkaWojskowa};{participant.Kolumna};");
                IEnumerable<Score> orderedScores = participant.Scores.OrderBy(x => x.Competition);
                foreach (Score score in orderedScores)
                {
                   
                    await writer.WriteAsync($"{score.CompetitionString};{score.MinScore};{score.ScoreString};{score.Points};");
                    
                }
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

        public async Task<IEnumerable<Participant>> ReadParticipantsAsync()
        {
            var participants = new List<Participant>();

            using var reader = new StreamReader(_filePath, new UTF8Encoding(true));

            //Skip header line
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
                    Competition competition = data[i].GetValueFromDescription<Competition>();
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
    }
}
