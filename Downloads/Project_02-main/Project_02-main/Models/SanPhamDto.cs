using System.ComponentModel.DataAnnotations.Schema;

namespace Project_02.Models
{
    public class SanPhamDto
    {
        public int MaSp { get; set; }
        public string TenSp { get; set; }
        public string MoTa { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }
        // 🔹 Lưu tên file ảnh trong database
        public string? HinhAnh { get; set; }

        [NotMapped] // Không lưu vào database
        public IFormFile? UploadImage { get; set; }  // giữ đường dẫn ảnh cũ
        public int? MaLoai { get; set; }
        public decimal PhanTramGiam { get; set; }
        public bool TrangThai { get; set; }
      
    }

}
