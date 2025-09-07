namespace API_Tiendas_Fabricato.DTOs
{
    public class DiscountResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public DiscountDto? Data { get; set; }
        public List<string>? Errors { get; set; }
    }

    public class DiscountListResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<DiscountDto> Data { get; set; } = new List<DiscountDto>();
        public int TotalCount { get; set; }
        public List<string>? Errors { get; set; }
    }
}