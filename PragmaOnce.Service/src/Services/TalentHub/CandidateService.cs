using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities.TalentHub;
using PragmaOnce.Core.src.Interfaces.TalentHub;
using PragmaOnce.Service.src.DTOs.TalentHub;
using PragmaOnce.Service.src.Interfaces.TalentHub;

namespace PragmaOnce.Service.src.Services.TalentHub
{
    public class CandidateService : BaseService<CandidateProfile, CandidateReadDto, CandidateCreateDto, CandidateUpdateDto, QueryOptions>, ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        public CandidateService(ICandidateRepository candidateRepository, IMapper mapper, IMemoryCache cache) : base(candidateRepository, mapper, cache)
        {
            _candidateRepository = candidateRepository;
        }

        public async Task<CandidateProfile> GetCandidate(Guid userId)
        {
            var candidate = await _candidateRepository.GetByIdAsync(userId);
            return _mapper.Map<CandidateProfile>(candidate);
        }
    }
}
