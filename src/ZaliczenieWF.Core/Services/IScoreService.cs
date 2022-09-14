using ZaliczenieWF.Core.Models;

namespace ZaliczenieWF.Core.Services
{
    public interface IScoreService
    {
        double CalculateScores(Participant participant);
    }
}
