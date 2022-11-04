using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Reporting.WinForms;
using ZaliczenieWF.Models;

namespace ZaliczenieWF.Reporting
{
    public static class GenerateReport
    {
        /// <summary>
        /// Generuje raport na podstawie wzoru w pliku Repoer1.rdlc znajdującego się w główym folderze aplikacji.
        /// </summary>
        /// <param name="participant">Uczestnik</param>
        /// <returns></returns>
        public static async Task PdfAsync(Participant participant)
        {
            // Ścieżka do pliku z definicją raportu
            var report = @"Report1.rdlc";
            // Nazwa pod jaką zostanie zapisany raport.
            var fileName = $"{participant.Name}.pdf";

            var participants = new List<Participant> { participant };

            var participantDataSource = new ReportDataSource("Participant", participants);
            var scoreDataSource = new ReportDataSource("Scores", participant.Scores);

            var viewer = new LocalReport
            {
                ReportPath = report
            };
            viewer.DataSources.Add(participantDataSource);
            viewer.DataSources.Add(scoreDataSource);

            // Renderuje raport do macierzy bajtów
            var Bytes = viewer.Render(format: "PDF", deviceInfo: "");

            // Zapisuje raport używając strumienia danych
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                await stream.WriteAsync(Bytes, 0, Bytes.Length);
            }
        }
    }
}
