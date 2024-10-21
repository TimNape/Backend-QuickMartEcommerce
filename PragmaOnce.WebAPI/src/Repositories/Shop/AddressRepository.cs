using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Interfaces;
using PragmaOnce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;
using PragmaOnce.Core.src.Entities.Shop.OrderAggregate;

namespace PragmaOnce.WebAPI.src.Repositories.Shop
{
    public class AddressRepository : IBaseRepository<Address, QueryOptions>
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Address> _addresses;

        public AddressRepository(AppDbContext context)
        {
            _context = context;
            _addresses = _context.Addresses;
        }
        public async Task<Address> CreateAsync(Address entity)
        {
            _addresses.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var address = await _addresses.FindAsync(id);
            if (address == null) return false;
            _addresses.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Address entity)
        {
            return await _addresses.AnyAsync(e => e.Id == entity.Id);
        }

        public async Task<PaginatedResult<Address>> GetAllAsync(QueryOptions options)
        {
            IQueryable<Address> query = _addresses;

            // Get total count
            var totalCount = await query.CountAsync();

            // Pagination
            query = query.Skip((options.Page - 1) * options.PageSize).Take(options.PageSize);

            var addresses = await query.ToListAsync();

            return new PaginatedResult<Address>(addresses, totalCount);
        }

        public async Task<Address> GetByIdAsync(Guid id)
        {
            return await _addresses.FindAsync(id) ?? throw AppException.NotFound();
        }

        public async Task<Address?> UpdateAsync(Address entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}