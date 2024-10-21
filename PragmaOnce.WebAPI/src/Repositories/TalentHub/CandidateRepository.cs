using Microsoft.EntityFrameworkCore;
using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities.TalentHub;
using PragmaOnce.Core.src.Interfaces.TalentHub;
using PragmaOnce.Core.src.ValueObjects;
using PragmaOnce.WebAPI.src.Data;

namespace PragmaOnce.WebAPI.src.Repositories.TalentHub
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<CandidateProfile> _candidates;

        public CandidateRepository(AppDbContext context)
        {
            _context = context;
            _candidates = _context.Candidates;
        }

        public async Task<CandidateProfile> CreateAsync(CandidateProfile entity)
        {
            await _candidates.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var candidate = await _candidates.FindAsync(id);
            if (candidate == null)
                return false;
            _candidates.Remove(candidate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(CandidateProfile entity)
        {
            return await _candidates.AnyAsync(e => e.Id == entity.Id);
        }

        public async Task<PaginatedResult<CandidateProfile>> GetAllAsync(QueryOptions options)
        {
            IQueryable<CandidateProfile> query = _context.Candidates;

            // Get total count
            var totalCount = await query.CountAsync();

            // Sorting
            query = SortCandidates(query, options.SortBy, options.SortOrder);

            // Pagination
            query = query.Skip((options.Page - 1) * options.PageSize).Take(options.PageSize);

            var candidates = await query.ToListAsync();

            return new PaginatedResult<CandidateProfile>(candidates, totalCount);
        }

        private IQueryable<CandidateProfile> SortCandidates(IQueryable<CandidateProfile> query, SortType sortBy, SortOrder sortOrder)
        {
            switch (sortBy)
            {
                case SortType.byName:
                    query = sortOrder == SortOrder.Ascending ?
                            query.OrderBy(c => c.Title) :
                            query.OrderByDescending(c => c.Title);
                    break;
                default:
                    // Default sorting by name 
                    query = sortOrder == SortOrder.Ascending ?
                            query.OrderBy(c => c.MinYearsExperience) :
                            query.OrderByDescending(c => c.MinYearsExperience);
                    break;
            }
            return query;
        }

        public async Task<CandidateProfile> GetByIdAsync(Guid id)
        {
            return await _candidates.FindAsync(id) ?? throw AppException.NotFound();
        }

        public async Task<CandidateProfile?> UpdateAsync(CandidateProfile entity)
        {
            var candidate = await _context.Candidates.FindAsync(entity.Id);
            if (candidate == null) return null!;
            candidate.Title = entity.Title;
            candidate.MinYearsExperience = entity.MinYearsExperience;
            candidate.MaxYearsExperience = entity.MaxYearsExperience;
            candidate.Citizenship = entity.Citizenship;
            candidate.PermanentResidency = entity.PermanentResidency;
            await _context.SaveChangesAsync();
            return candidate;
        }


    }
}
