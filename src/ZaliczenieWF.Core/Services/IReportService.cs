using System.Collections.Generic;
using System.Threading.Tasks;
using ZaliczenieWF.Models;

namespace ZaliczenieWF.Core.Services
{
    public interface IReportService
    {
        Task GeneratePdfReportAsync(Participant participant);
        Task GeneratePdfReports(List<Participant> participants);
    }
}
