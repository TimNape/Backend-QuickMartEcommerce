using PragmaOnce.Core.src.Common;
using PragmaOnce.Service.src.DTOs.Shop;

namespace PragmaOnce.Service.src.Interfaces.Shop
{
    public interface IProductService : IBaseService<ProductReadDto, ProductCreateDto, ProductUpdateDto, QueryOptions>
    {
        Task<IEnumerable<ProductReadDto>> GetMostPurchased(int topNumber);
    }
}