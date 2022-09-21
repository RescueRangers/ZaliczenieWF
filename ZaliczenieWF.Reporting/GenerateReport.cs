using System.Collections.Generic;
using System.IO;
using Microsoft.Reporting.WinForms;
using ZaliczenieWF.Models;

namespace ZaliczenieWF.Reporting
{
    public static class GenerateReport
    {
        public static void Pdf(Participant participant)
        {
            var testReport = @"Report1.rdlc";
            var testFileName = @"Test.pdf";

            var participants = new List<Participant> { participant };

            var participantDataSource = new ReportDataSource("Participant", participants);
            var scoreDataSource = new ReportDataSource("Scores", participant.Scores);

            var viewer = new LocalReport
            {
                ReportPath = testReport
            };
            viewer.DataSources.Add(participantDataSource);
            viewer.DataSources.Add(scoreDataSource);

            var Bytes = viewer.Render(format: "PDF", deviceInfo: "");

            using (var stream = new FileStream(testFileName, FileMode.Create))
            {
                stream.Write(Bytes, 0, Bytes.Length);
            }
        }
    }
}
