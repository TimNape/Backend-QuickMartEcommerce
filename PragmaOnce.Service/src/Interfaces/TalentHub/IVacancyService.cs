using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities.TalentHub;
using PragmaOnce.Service.src.DTOs.TalentHub;

namespace PragmaOnce.Service.src.Interfaces.TalentHub
{
    public interface IVacancyService : IBaseService<VacancyReadDto, VacancyCreateDto, VacancyUpdateDto, QueryOptions>
    {
        Task<Vacancy> GetVacancyAsync(Guid vacancyId);
    }
}
