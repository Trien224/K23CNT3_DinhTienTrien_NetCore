namespace Project_02.Models
{
    public class ProductWishlistDto
    {
        public int MaDanhGia { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public string? Category { get; set; }
        public DateTime AddedDate { get; set; }
        public int Stock { get; set; }
        // Thêm thuộc tính số lượng mặc định
        public int Quantity { get; set; } = 1;
    }
}