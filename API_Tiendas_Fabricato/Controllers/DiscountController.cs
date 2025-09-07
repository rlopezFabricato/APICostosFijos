using API_Tiendas_Fabricato.DTOs;
using API_Tiendas_Fabricato.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_Tiendas_Fabricato.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        /// <summary>
        /// Creates a new discount
        /// </summary>
        /// <param name="createDiscountDto">Discount creation data</param>
        /// <returns>Created discount information</returns>
        [HttpPost]
        public async Task<ActionResult<DiscountResponseDto>> CreateDiscount([FromBody] CreateDiscountDto createDiscountDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .SelectMany(x => x.Value!.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                return BadRequest(new DiscountResponseDto
                {
                    Success = false,
                    Message = "Validation errors occurred",
                    Errors = errors
                });
            }

            var result = await _discountService.CreateDiscountAsync(createDiscountDto);
            
            if (!result.Success)
                return BadRequest(result);
            
            return CreatedAtAction(nameof(GetDiscount), new { id = result.Data!.Id }, result);
        }

        /// <summary>
        /// Gets a discount by ID
        /// </summary>
        /// <param name="id">Discount ID</param>
        /// <returns>Discount information</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<DiscountResponseDto>> GetDiscount(int id)
        {
            var result = await _discountService.GetDiscountByIdAsync(id);
            
            if (!result.Success)
                return NotFound(result);
            
            return Ok(result);
        }

        /// <summary>
        /// Gets all discounts
        /// </summary>
        /// <param name="includeInactive">Include inactive discounts</param>
        /// <returns>List of discounts</returns>
        [HttpGet]
        public async Task<ActionResult<DiscountListResponseDto>> GetAllDiscounts([FromQuery] bool includeInactive = false)
        {
            var result = await _discountService.GetAllDiscountsAsync(includeInactive);
            return Ok(result);
        }

        /// <summary>
        /// Gets all currently active discounts
        /// </summary>
        /// <returns>List of active discounts</returns>
        [HttpGet("active")]
        public async Task<ActionResult<DiscountListResponseDto>> GetActiveDiscounts()
        {
            var result = await _discountService.GetActiveDiscountsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing discount
        /// </summary>
        /// <param name="id">Discount ID</param>
        /// <param name="updateDiscountDto">Discount update data</param>
        /// <returns>Updated discount information</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<DiscountResponseDto>> UpdateDiscount(int id, [FromBody] UpdateDiscountDto updateDiscountDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .SelectMany(x => x.Value!.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                return BadRequest(new DiscountResponseDto
                {
                    Success = false,
                    Message = "Validation errors occurred",
                    Errors = errors
                });
            }

            var result = await _discountService.UpdateDiscountAsync(id, updateDiscountDto);
            
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            
            return Ok(result);
        }

        /// <summary>
        /// Deletes a discount
        /// </summary>
        /// <param name="id">Discount ID</param>
        /// <returns>Operation result</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<DiscountResponseDto>> DeleteDiscount(int id)
        {
            var result = await _discountService.DeleteDiscountAsync(id);
            
            if (!result.Success)
                return NotFound(result);
            
            return Ok(result);
        }

        /// <summary>
        /// Activates a discount
        /// </summary>
        /// <param name="id">Discount ID</param>
        /// <returns>Updated discount information</returns>
        [HttpPost("{id}/activate")]
        public async Task<ActionResult<DiscountResponseDto>> ActivateDiscount(int id)
        {
            var result = await _discountService.ActivateDiscountAsync(id);
            
            if (!result.Success)
                return NotFound(result);
            
            return Ok(result);
        }

        /// <summary>
        /// Deactivates a discount
        /// </summary>
        /// <param name="id">Discount ID</param>
        /// <returns>Updated discount information</returns>
        [HttpPost("{id}/deactivate")]
        public async Task<ActionResult<DiscountResponseDto>> DeactivateDiscount(int id)
        {
            var result = await _discountService.DeactivateDiscountAsync(id);
            
            if (!result.Success)
                return NotFound(result);
            
            return Ok(result);
        }

        /// <summary>
        /// Calculates discount amount for a given original amount
        /// </summary>
        /// <param name="id">Discount ID</param>
        /// <param name="originalAmount">Original amount to calculate discount for</param>
        /// <returns>Discount amount</returns>
        [HttpGet("{id}/calculate")]
        public async Task<ActionResult<decimal>> CalculateDiscountAmount(int id, [FromQuery] decimal originalAmount)
        {
            if (originalAmount <= 0)
            {
                return BadRequest(new { message = "Original amount must be greater than 0" });
            }

            var discountAmount = await _discountService.CalculateDiscountAmountAsync(id, originalAmount);
            var finalAmount = originalAmount - discountAmount;

            return Ok(new 
            { 
                originalAmount = originalAmount,
                discountAmount = discountAmount,
                finalAmount = finalAmount
            });
        }
    }
}