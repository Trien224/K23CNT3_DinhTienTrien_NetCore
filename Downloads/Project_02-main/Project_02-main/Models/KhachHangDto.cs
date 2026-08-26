using System.ComponentModel.DataAnnotations;

namespace Project_02.Models
{
    public class KhachHangDto
    {
        public int MaKh { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [MaxLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
        public string HoTen { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [MaxLength(15)]
        public string? DienThoai { get; set; }

        [MaxLength(255)]
        public string? DiaChi { get; set; }

        // ❌ Không đưa MatKhau vào DTO để tránh lộ thông tin nhạy cảm
        // ❌ Không đưa TrangThai nếu không cần chỉnh sửa từ phía client
    }
}
