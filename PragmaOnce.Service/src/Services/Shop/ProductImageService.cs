using AutoMapper;
using PragmaOnce.Core.src.Common;
using Microsoft.Extensions.Caching.Memory;
using PragmaOnce.Core.src.Entities.Shop;
using PragmaOnce.Core.src.Interfaces.Shop;
using PragmaOnce.Service.src.DTOs.Shop;
using PragmaOnce.Service.src.Interfaces.Shop;

namespace PragmaOnce.Service.src.Services.Shop
{
    public class ProductImageService : BaseService<ProductImage, ProductImageReadDto, ProductImageCreateDto, ProductImageUpdateDto, QueryOptions>, IProductImageService
    {
        private readonly IProductImageRepository _productImageRepository;
        public ProductImageService(IProductImageRepository productImageRepository, IMapper mapper, IMemoryCache cache)
            : base(productImageRepository, mapper, cache)
        {
            _productImageRepository = productImageRepository;
        }
    }
}