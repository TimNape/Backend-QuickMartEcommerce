using PragmaOnce.Core.src.Common;
using PragmaOnce.Service.src.DTOs.Shop;

namespace PragmaOnce.Service.src.Interfaces.Shop
{
    public interface IProductImageService : IBaseService<ProductImageReadDto, ProductImageCreateDto, ProductImageUpdateDto, QueryOptions>
    {

    }
}