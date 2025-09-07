using System.ComponentModel.DataAnnotations;

namespace API_Tiendas_Fabricato.DTOs
{
    public class CreateDiscountDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name must not exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, 100.00, ErrorMessage = "Percentage must be between 0.01 and 100")]
        public decimal Percentage { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}