using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Reporting.WinForms;
using ZaliczenieWF.Models;

namespace ZaliczenieWF.Reporting
{
    public static class GenerateReport
    {
        public static async Task PdfAsync(Participant participant)
        {
            var report = @"Report1.rdlc";
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

            var Bytes = viewer.Render(format: "PDF", deviceInfo: "");

            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                await stream.WriteAsync(Bytes, 0, Bytes.Length);
            }
        }
    }
}
