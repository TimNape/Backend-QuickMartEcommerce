using Microsoft.EntityFrameworkCore;
using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities.TalentHub;
using PragmaOnce.Core.src.Interfaces.TalentHub;
using PragmaOnce.WebAPI.src.Data;

namespace PragmaOnce.WebAPI.src.Repositories.TalentHub
{
    public class RecruiterRepository : IRecruiterRepository
    {

        private readonly AppDbContext _context;
        private readonly DbSet<RecruiterProfile> _recruiters;

        public RecruiterRepository(AppDbContext context)
        {
            _context = context;
            _recruiters = _context.Recruiters;
        }

        public async Task<RecruiterProfile> CreateAsync(RecruiterProfile entity)
        {
            await _recruiters.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var recruiter = await _recruiters.FindAsync(id);
            if (recruiter == null)
                return false;
            _recruiters.Remove(recruiter);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(RecruiterProfile entity)
        {
            return await _recruiters.AnyAsync(e => e.Id == entity.Id);
        }

        public async Task<PaginatedResult<RecruiterProfile>> GetAllAsync(QueryOptions options)
        {
            IQueryable<RecruiterProfile> query = _context.Recruiters;

            // Get total count
            var totalCount = await query.CountAsync();

            // Pagination
            query = query.Skip((options.Page - 1) * options.PageSize).Take(options.PageSize);

            var recruiters = await query.ToListAsync();

            return new PaginatedResult<RecruiterProfile>(recruiters, totalCount);
        }

        public async Task<RecruiterProfile> GetByIdAsync(Guid id)
        {
            return await _recruiters.FindAsync(id) ?? throw AppException.NotFound();
        }

        public async Task<RecruiterProfile?> UpdateAsync(RecruiterProfile entity)
        {
            var recruiter = await _context.Recruiters.FindAsync(entity.Id);
            if (recruiter == null) return null!;
            recruiter.CompanyId = entity.CompanyId;
            await _context.SaveChangesAsync();
            return recruiter;
        }
    }
}
