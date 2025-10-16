namespace NemeShop.Models
{
    public class CheckoutItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CheckoutRequestDto
    {
        public List<CheckoutItemDto> Items { get; set; } = new List<CheckoutItemDto>();
        public string DiaChiGiaoHang { get; set; } = string.Empty;
        public string SdtgiaoHang { get; set; } = string.Empty;
        public string? GhiChu { get; set; }
    }
}