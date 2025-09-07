using System.ComponentModel.DataAnnotations;

namespace API_Tiendas_Fabricato.DTOs
{
    public class UpdateDiscountDto
    {
        [StringLength(100, ErrorMessage = "Name must not exceed 100 characters")]
        public string? Name { get; set; }
        
        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
        public string? Description { get; set; }
        
        [Range(0.01, 100.00, ErrorMessage = "Percentage must be between 0.01 and 100")]
        public decimal? Percentage { get; set; }
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public bool? IsActive { get; set; }
    }
}