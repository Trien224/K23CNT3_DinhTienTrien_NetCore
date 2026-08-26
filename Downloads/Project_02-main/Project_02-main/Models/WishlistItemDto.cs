namespace Project_02.Models
{
    // Thêm vào Models
    public class WishlistItemDto
    {
        public int MaKh { get; set; }
        public int MaSp { get; set; }
        public int SoSao { get; set; }
        public string? NoiDung { get; set; }
        public bool LaYeuThich { get; set; }
        public DateTime NgayTao { get; set; }
        public bool TrangThai { get; set; }
    }
}
