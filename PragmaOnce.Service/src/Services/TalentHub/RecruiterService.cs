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
    public class RecruiterService : BaseService<RecruiterProfile, RecruiterReadDto, RecruiterCreateDto, RecruiterUpdateDto, QueryOptions>, IRecruiterService
    {
        private readonly IRecruiterRepository recruiterRepository;

        public RecruiterService(IRecruiterRepository recruiterRepository, IMapper mapper, IMemoryCache cache) : base(recruiterRepository, mapper, cache)
        {
            this.recruiterRepository = recruiterRepository;
        }

        public Task<RecruiterProfile> GetRecruiterAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
