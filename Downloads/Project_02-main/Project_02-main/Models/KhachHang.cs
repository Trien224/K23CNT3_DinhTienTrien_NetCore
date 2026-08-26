using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_02.Models
{
    public class KhachHang : KhachHangDto
    {
        [Key]
        public int MaKh { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [MaxLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
        public string HoTen { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [MaxLength(100)]
        public string? Email { get; set; }

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
        /// true = hoạt động, false = bị khóa
        /// </summary>
        public bool TrangThai { get; set; } = true;

        // 🔗 Navigation
        public virtual ICollection<DanhGiaSanPham> DanhGiaSanPhams { get; set; } = new List<DanhGiaSanPham>();
        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
    }
}
