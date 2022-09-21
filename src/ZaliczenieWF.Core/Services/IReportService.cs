using System;
using System.Collections.Generic;
using System.Text;
using ZaliczenieWF.Models;

namespace ZaliczenieWF.Core.Services
{
    public interface IReportService
    {
        string GeneratePdfReport(Participant participants);
        string GeneratePdfReports(List<Participant> participants);
    }
}
