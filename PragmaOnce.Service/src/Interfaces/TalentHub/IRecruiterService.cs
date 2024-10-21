using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities.TalentHub;
using PragmaOnce.Service.src.DTOs.TalentHub;

namespace PragmaOnce.Service.src.Interfaces.TalentHub
{
    public interface IRecruiterService : IBaseService<RecruiterReadDto, RecruiterCreateDto, RecruiterUpdateDto, QueryOptions>
    {
        Task<RecruiterProfile> GetRecruiterAsync(Guid userId);
    }
    
}
