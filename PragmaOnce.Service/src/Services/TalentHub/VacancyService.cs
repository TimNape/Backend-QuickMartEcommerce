using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities.TalentHub;
using PragmaOnce.Core.src.Interfaces.TalentHub;
using PragmaOnce.Service.src.DTOs.TalentHub;
using PragmaOnce.Service.src.Interfaces.TalentHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragmaOnce.Service.src.Services.TalentHub
{
    public class VacancyService : BaseService<Vacancy, VacancyReadDto, VacancyCreateDto, VacancyUpdateDto, QueryOptions>, IVacancyService
    {
        private readonly IVacancyRepository _vacancyRepository;
        public VacancyService(IVacancyRepository vacancyRepository, IMapper mapper, IMemoryCache cache)
            : base(vacancyRepository, mapper, cache) {
            _vacancyRepository = vacancyRepository;
        }
        public Task<Vacancy> GetVacancyAsync(Guid vacancyId)
        {
            throw new NotImplementedException();
        }
    }
}
