using System.Collections.Generic;
using System.Threading.Tasks;
using ZaliczenieWF.Models;

namespace ZaliczenieWF.Core.Services
{
    public interface IDataIOService
    {
        void AddParticipant(Participant participant);
        void DeleteParticipant(Participant participant);
        Task<IEnumerable<Participant>> ReadParticipantsAsync();
        Task SaveParticipantsAsync(IEnumerable<Participant> participants);
        void UpdateParticipant(Participant participant);
    }
}
