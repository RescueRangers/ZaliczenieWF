using ZaliczenieWF.Models;

namespace ZaliczenieWF.Core.Services
{
    public interface IScoreService
    {
        void CalculateScores(Participant participant);
    }
}
