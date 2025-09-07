using API_Tiendas_Fabricato.DTOs;
using API_Tiendas_Fabricato.Interfaces;
using API_Tiendas_Fabricato.Models;

namespace API_Tiendas_Fabricato.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly List<Discount> _discounts;
        private int _nextId = 1;

        public DiscountService()
        {
            _discounts = new List<Discount>
            {
                new Discount
                {
                    Id = _nextId++,
                    Name = "New Year Sale",
                    Description = "Special discount for New Year",
                    Percentage = 15.0m,
                    StartDate = DateTime.Now.AddDays(-30),
                    EndDate = DateTime.Now.AddDays(30),
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-30)
                },
                new Discount
                {
                    Id = _nextId++,
                    Name = "Summer Sale",
                    Description = "Summer seasonal discount",
                    Percentage = 20.0m,
                    StartDate = DateTime.Now.AddDays(-10),
                    EndDate = DateTime.Now.AddDays(20),
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-10)
                }
            };
        }

        public async Task<DiscountResponseDto> CreateDiscountAsync(CreateDiscountDto createDiscountDto)
        {
            try
            {
                // Validate dates
                if (createDiscountDto.EndDate.HasValue && createDiscountDto.EndDate <= createDiscountDto.StartDate)
                {
                    return new DiscountResponseDto
                    {
                        Success = false,
                        Message = "End date must be after start date",
                        Errors = new List<string> { "Invalid date range" }
                    };
                }

                var discount = new Discount
                {
                    Id = _nextId++,
                    Name = createDiscountDto.Name,
                    Description = createDiscountDto.Description,
                    Percentage = createDiscountDto.Percentage,
                    StartDate = createDiscountDto.StartDate,
                    EndDate = createDiscountDto.EndDate,
                    IsActive = createDiscountDto.IsActive,
                    CreatedAt = DateTime.Now
                };

                _discounts.Add(discount);

                var discountDto = MapToDto(discount);

                return new DiscountResponseDto
                {
                    Success = true,
                    Message = "Discount created successfully",
                    Data = discountDto
                };
            }
            catch (Exception ex)
            {
                return new DiscountResponseDto
                {
                    Success = false,
                    Message = "An error occurred while creating the discount",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<DiscountResponseDto> GetDiscountByIdAsync(int id)
        {
            try
            {
                var discount = _discounts.FirstOrDefault(d => d.Id == id);
                
                if (discount == null)
                {
                    return new DiscountResponseDto
                    {
                        Success = false,
                        Message = "Discount not found",
                        Errors = new List<string> { $"Discount with ID {id} does not exist" }
                    };
                }

                var discountDto = MapToDto(discount);

                return new DiscountResponseDto
                {
                    Success = true,
                    Message = "Discount retrieved successfully",
                    Data = discountDto
                };
            }
            catch (Exception ex)
            {
                return new DiscountResponseDto
                {
                    Success = false,
                    Message = "An error occurred while retrieving the discount",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<DiscountListResponseDto> GetAllDiscountsAsync(bool includeInactive = false)
        {
            try
            {
                var discounts = includeInactive 
                    ? _discounts.ToList() 
                    : _discounts.Where(d => d.IsActive).ToList();

                var discountDtos = discounts.Select(MapToDto).ToList();

                return new DiscountListResponseDto
                {
                    Success = true,
                    Message = "Discounts retrieved successfully",
                    Data = discountDtos,
                    TotalCount = discountDtos.Count
                };
            }
            catch (Exception ex)
            {
                return new DiscountListResponseDto
                {
                    Success = false,
                    Message = "An error occurred while retrieving discounts",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<DiscountListResponseDto> GetActiveDiscountsAsync()
        {
            try
            {
                var currentDate = DateTime.Now;
                var activeDiscounts = _discounts.Where(d => 
                    d.IsActive && 
                    d.StartDate <= currentDate && 
                    (!d.EndDate.HasValue || d.EndDate.Value >= currentDate)
                ).ToList();

                var discountDtos = activeDiscounts.Select(MapToDto).ToList();

                return new DiscountListResponseDto
                {
                    Success = true,
                    Message = "Active discounts retrieved successfully",
                    Data = discountDtos,
                    TotalCount = discountDtos.Count
                };
            }
            catch (Exception ex)
            {
                return new DiscountListResponseDto
                {
                    Success = false,
                    Message = "An error occurred while retrieving active discounts",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<DiscountResponseDto> UpdateDiscountAsync(int id, UpdateDiscountDto updateDiscountDto)
        {
            try
            {
                var discount = _discounts.FirstOrDefault(d => d.Id == id);
                
                if (discount == null)
                {
                    return new DiscountResponseDto
                    {
                        Success = false,
                        Message = "Discount not found",
                        Errors = new List<string> { $"Discount with ID {id} does not exist" }
                    };
                }

                // Update only non-null properties
                if (!string.IsNullOrEmpty(updateDiscountDto.Name))
                    discount.Name = updateDiscountDto.Name;
                    
                if (!string.IsNullOrEmpty(updateDiscountDto.Description))
                    discount.Description = updateDiscountDto.Description;
                    
                if (updateDiscountDto.Percentage.HasValue)
                    discount.Percentage = updateDiscountDto.Percentage.Value;
                    
                if (updateDiscountDto.StartDate.HasValue)
                    discount.StartDate = updateDiscountDto.StartDate.Value;
                    
                if (updateDiscountDto.EndDate.HasValue)
                    discount.EndDate = updateDiscountDto.EndDate.Value;
                    
                if (updateDiscountDto.IsActive.HasValue)
                    discount.IsActive = updateDiscountDto.IsActive.Value;

                discount.UpdatedAt = DateTime.Now;

                // Validate dates after update
                if (discount.EndDate.HasValue && discount.EndDate <= discount.StartDate)
                {
                    return new DiscountResponseDto
                    {
                        Success = false,
                        Message = "End date must be after start date",
                        Errors = new List<string> { "Invalid date range" }
                    };
                }

                var discountDto = MapToDto(discount);

                return new DiscountResponseDto
                {
                    Success = true,
                    Message = "Discount updated successfully",
                    Data = discountDto
                };
            }
            catch (Exception ex)
            {
                return new DiscountResponseDto
                {
                    Success = false,
                    Message = "An error occurred while updating the discount",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<DiscountResponseDto> DeleteDiscountAsync(int id)
        {
            try
            {
                var discount = _discounts.FirstOrDefault(d => d.Id == id);
                
                if (discount == null)
                {
                    return new DiscountResponseDto
                    {
                        Success = false,
                        Message = "Discount not found",
                        Errors = new List<string> { $"Discount with ID {id} does not exist" }
                    };
                }

                _discounts.Remove(discount);

                return new DiscountResponseDto
                {
                    Success = true,
                    Message = "Discount deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new DiscountResponseDto
                {
                    Success = false,
                    Message = "An error occurred while deleting the discount",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<DiscountResponseDto> ActivateDiscountAsync(int id)
        {
            try
            {
                var discount = _discounts.FirstOrDefault(d => d.Id == id);
                
                if (discount == null)
                {
                    return new DiscountResponseDto
                    {
                        Success = false,
                        Message = "Discount not found",
                        Errors = new List<string> { $"Discount with ID {id} does not exist" }
                    };
                }

                discount.IsActive = true;
                discount.UpdatedAt = DateTime.Now;

                var discountDto = MapToDto(discount);

                return new DiscountResponseDto
                {
                    Success = true,
                    Message = "Discount activated successfully",
                    Data = discountDto
                };
            }
            catch (Exception ex)
            {
                return new DiscountResponseDto
                {
                    Success = false,
                    Message = "An error occurred while activating the discount",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<DiscountResponseDto> DeactivateDiscountAsync(int id)
        {
            try
            {
                var discount = _discounts.FirstOrDefault(d => d.Id == id);
                
                if (discount == null)
                {
                    return new DiscountResponseDto
                    {
                        Success = false,
                        Message = "Discount not found",
                        Errors = new List<string> { $"Discount with ID {id} does not exist" }
                    };
                }

                discount.IsActive = false;
                discount.UpdatedAt = DateTime.Now;

                var discountDto = MapToDto(discount);

                return new DiscountResponseDto
                {
                    Success = true,
                    Message = "Discount deactivated successfully",
                    Data = discountDto
                };
            }
            catch (Exception ex)
            {
                return new DiscountResponseDto
                {
                    Success = false,
                    Message = "An error occurred while deactivating the discount",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<decimal> CalculateDiscountAmountAsync(int discountId, decimal originalAmount)
        {
            try
            {
                var discount = _discounts.FirstOrDefault(d => d.Id == discountId);
                
                if (discount == null || !discount.IsActive)
                {
                    return 0;
                }

                var currentDate = DateTime.Now;
                
                // Check if discount is within valid date range
                if (discount.StartDate > currentDate || 
                    (discount.EndDate.HasValue && discount.EndDate.Value < currentDate))
                {
                    return 0;
                }

                return originalAmount * (discount.Percentage / 100);
            }
            catch
            {
                return 0;
            }
        }

        private static DiscountDto MapToDto(Discount discount)
        {
            return new DiscountDto
            {
                Id = discount.Id,
                Name = discount.Name,
                Description = discount.Description,
                Percentage = discount.Percentage,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                IsActive = discount.IsActive,
                CreatedAt = discount.CreatedAt,
                UpdatedAt = discount.UpdatedAt
            };
        }
    }
}