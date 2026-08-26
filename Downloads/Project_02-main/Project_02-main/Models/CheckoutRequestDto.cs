namespace Project_02.Models
{
    public class CheckoutItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CheckoutRequestDto
    {
        public List<CheckoutItemDto> Items { get; set; } = new List<CheckoutItemDto>();
        public int? MaKh { get; set; }
        public string? HoTenNguoiNhan { get; set; }
        public string DiaChiGiaoHang { get; set; } = string.Empty;
        public string SdtgiaoHang { get; set; } = string.Empty;
        public string? MaVoucher { get; set; }
        public string? GhiChu { get; set; }
    }
}
