using System.ComponentModel.DataAnnotations;

namespace Project_02.Models
{
    public class QuanTriVienDto
    {
        public int MaQtv { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [MaxLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
        public string HoTen { get; set; } = null!;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [MaxLength(15)]
        public string? DienThoai { get; set; }

        [MaxLength(255)]
        public string? DiaChi { get; set; }

        // ❌ Không expose MatKhau
        // ❌ Không expose TrangThai nếu không muốn user tự ý bật/tắt
    }
}
