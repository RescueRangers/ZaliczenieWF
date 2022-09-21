using System;
using System.Collections.Generic;
using ZaliczenieWF.Models;
//using BoldReports.Writer;
//using BoldReports.Windows;
//using ZaliczenieWF.Reporting;

namespace ZaliczenieWF.Core.Services
{
    public class ReportService : IReportService
    {
        public string GeneratePdfReport(Participant participant)
        {
            //var testReport = @"E:\Source\WF\ZaliczenieWF\ZaliczenieWF.WPF\bin\Debug\net6.0-windows\Report1.rdlc";
            //var testReport2 = @"E:\Source\WF\ZaliczenieWF\ZaliczenieWF.WPF\bin\Debug\net6.0-windows\TestReport.rdl";
            //var testFileName = @"E:\Source\WF\ZaliczenieWF\ZaliczenieWF.WPF\bin\Debug\net6.0-windows\Test.pdf";

            //var participants = new List<Participant> { participant};

            //var participantDataSource = new ReportDataSource("Participant", participants);
            //var scoreDataSource = new ReportDataSource("Score", participant.Scores);

            //var viewer = new LocalReport();
            //viewer.ReportPath = testReport;
            //viewer.DataSources.Add(participantDataSource);
            //viewer.DataSources.Add(scoreDataSource);

            //byte[] Bytes = viewer.Render(format: "PDF", deviceInfo: "");

            //using (FileStream stream = new FileStream(testFileName, FileMode.Create))
            //{
            //    stream.Write(Bytes, 0, Bytes.Length);
            //}

            Reporting.GenerateReport.Pdf(participant);

            //var dataSources = new ReportDataSourceCollection
            //{
            //    participantDataSource,
            //    scoreDataSource
            //};

            //using (FileStream rdlcFileStream = File.OpenRead(testReport))
            //{
            //    using var writer = new ReportWriter(rdlcFileStream);
            //    writer.DataSources = dataSources;
            //    IList<string> names = writer.GetDataSetNames();
            //    writer.ReportProcessingMode = ProcessingMode.Local;
            //    writer.ReportError += Writer_ReportError;

            //    using FileStream fileStream = File.Create(testFileName);
            //    writer.Save(fileStream, WriterFormat.PDF);
            //};

            return "";
        }

        //private void Writer_ReportError(object sender, ReportWriter.ReportErrorEventArgs e)
        //{
        //    Console.WriteLine(e.Error);
        //}

        public string GeneratePdfReports(List<Participant> participants) => throw new NotImplementedException();
    }
}
