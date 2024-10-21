using PragmaOnce.Core.src.Common;
using PragmaOnce.Service.src.DTOs.Shop;

namespace PragmaOnce.Service.src.Interfaces.Shop
{
    public interface IReviewService : IBaseService<ReviewReadDto, ReviewCreateDto, ReviewUpdateDto, QueryOptions>
    {
        Task<IEnumerable<ReviewReadDto>> GetReviewsByProductIdAsync(Guid productId);
        Task<IEnumerable<ReviewReadDto>> GetReviewsByUserIdAsync(Guid userId);
        Task<ReviewReadDto> CreateReviewAsync(Guid userId, ReviewCreateDto createDto);
    }
}