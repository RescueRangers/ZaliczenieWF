using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZaliczenieWF.Models;
//using BoldReports.Writer;
//using BoldReports.Windows;
//using ZaliczenieWF.Reporting;

namespace ZaliczenieWF.Core.Services
{
    public class ReportService : IReportService
    {
        /// <summary>
        /// Generuje raport który zostanie zapisany w głównym folderze aplikacji.
        /// </summary>
        /// <param name="participant">Uczestnik egzaminu</param>
        /// <returns></returns>
        public async Task GeneratePdfReportAsync(Participant participant)
        {
            await Reporting.GenerateReport.PdfAsync(participant);
        }

        public async Task GeneratePdfReports(List<Participant> participants) => throw new NotImplementedException();
    }
}
