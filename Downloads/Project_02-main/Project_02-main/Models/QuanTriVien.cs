using System;
using System.ComponentModel.DataAnnotations;

namespace Project_02.Models
{
    public class QuanTriVien : QuanTriVienDto
    {
        [Key]
        public int MaQtv { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [MaxLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
        public string HoTen { get; set; } = null!;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [MaxLength(255)]
        public string MatKhau { get; set; } = null!;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [MaxLength(15)]
        public string? DienThoai { get; set; }

        [MaxLength(255)]
        public string? DiaChi { get; set; }

        /// <summary>
        /// true = đang hoạt động, false = bị khóa
        /// </summary>
        public bool TrangThai { get; set; } = true;
    }
}
