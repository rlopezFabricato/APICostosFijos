using API_Tiendas_Fabricato.DTOs;

namespace API_Tiendas_Fabricato.Interfaces
{
    public interface IDiscountService
    {
        Task<DiscountResponseDto> CreateDiscountAsync(CreateDiscountDto createDiscountDto);
        Task<DiscountResponseDto> GetDiscountByIdAsync(int id);
        Task<DiscountListResponseDto> GetAllDiscountsAsync(bool includeInactive = false);
        Task<DiscountListResponseDto> GetActiveDiscountsAsync();
        Task<DiscountResponseDto> UpdateDiscountAsync(int id, UpdateDiscountDto updateDiscountDto);
        Task<DiscountResponseDto> DeleteDiscountAsync(int id);
        Task<DiscountResponseDto> ActivateDiscountAsync(int id);
        Task<DiscountResponseDto> DeactivateDiscountAsync(int id);
        Task<decimal> CalculateDiscountAmountAsync(int discountId, decimal originalAmount);
    }
}