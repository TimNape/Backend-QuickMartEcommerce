using Microsoft.EntityFrameworkCore;
using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities.TalentHub;
using PragmaOnce.Core.src.Interfaces.TalentHub;
using PragmaOnce.WebAPI.src.Data;

namespace PragmaOnce.WebAPI.src.Repositories.TalentHub
{
    public class VacancyRepository : IVacancyRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Vacancy> _vacancies;

        public VacancyRepository(AppDbContext context)
        {
            _context = context;
            _vacancies = _context.Vacancies;
        }

        public async Task<Vacancy> CreateAsync(Vacancy entity)
        {
            await _vacancies.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var vacancy = await _vacancies.FindAsync(id);
            if (vacancy == null)
                return false;
            _vacancies.Remove(vacancy);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Vacancy entity)
        {
            return await _vacancies.AnyAsync(e => e.Id == entity.Id);
        }

        public async Task<PaginatedResult<Vacancy>> GetAllAsync(QueryOptions options)
        {
            IQueryable<Vacancy> query = _context.Vacancies;

            // Get total count
            var totalCount = await query.CountAsync();

            // Pagination
            query = query.Skip((options.Page - 1) * options.PageSize).Take(options.PageSize);

            var vacancies = await query.ToListAsync();

            return new PaginatedResult<Vacancy>(vacancies, totalCount);
        }

        public async Task<Vacancy> GetByIdAsync(Guid id)
        {
            return await _vacancies.FindAsync(id) ?? throw AppException.NotFound();
        }

        public async Task<Vacancy?> UpdateAsync(Vacancy entity)
        {
            var vacancy = await _context.Vacancies.FindAsync(entity.Id);
            if (vacancy == null) return null!;
            vacancy.Title = entity.Title;
            await _context.SaveChangesAsync();
            return vacancy;
        }
    }
}
