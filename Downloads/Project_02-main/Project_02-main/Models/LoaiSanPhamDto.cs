using System.ComponentModel.DataAnnotations;

namespace Project_02.Models
{
    public class LoaiSanPhamDto
    {
        public int MaLoai { get; set; }

        [Required(ErrorMessage = "Tên loại không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên loại tối đa 100 ký tự")]
        public string TenLoai { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "Mô tả tối đa 500 ký tự")]
        public string? MoTa { get; set; }

        // ❌ Không đưa TrangThai nếu không muốn user tự ý bật/tắt
        // ❌ Không đưa SanPhams để tránh lộ toàn bộ danh sách sản phẩm
    }
}
