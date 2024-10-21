using PragmaOnce.Core.src.Common;
using PragmaOnce.Service.src.DTOs.Shop;

namespace PragmaOnce.Service.src.Interfaces.Shop
{
    public interface ICategoryService : IBaseService<CategoryReadDto, CategoryCreateDto, CategoryUpdateDto, QueryOptions>
    {
        Task<IEnumerable<ProductReadDto>> GetProductsByCategoryIdAsync(Guid categoryId);
    }
}