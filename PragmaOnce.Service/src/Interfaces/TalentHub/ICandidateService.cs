using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities.TalentHub;
using PragmaOnce.Service.src.DTOs.TalentHub;

namespace PragmaOnce.Service.src.Interfaces.TalentHub
{
    public interface ICandidateService : IBaseService<CandidateReadDto, CandidateCreateDto, CandidateUpdateDto, QueryOptions>
    {
        Task<CandidateProfile> GetCandidate(Guid userId);
    }
}
