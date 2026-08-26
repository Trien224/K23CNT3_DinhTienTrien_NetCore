using System;
using System.ComponentModel.DataAnnotations;

namespace Project_02.Models
{
    public class MaGiamGiumDto
    {
        [Key]
        [MaxLength(50)]
        public string MaVoucher { get; set; } = null!;

        [Required(ErrorMessage = "Tên voucher không được để trống")]
        [MaxLength(100)]
        public string TenVoucher { get; set; } = null!;

        [MaxLength(500)]
        public string? MoTa { get; set; }

        [Range(0, 100, ErrorMessage = "Phần trăm giảm phải từ 0 đến 100")]
        public decimal? PhanTramGiam { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giảm tiền trực tiếp không hợp lệ")]
        public decimal? GiamTienTrucTiep { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá trị đơn tối thiểu không hợp lệ")]
        public decimal? GiaTriDonToiThieu { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải >= 0")]
        public int SoLuong { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
        public DateTime NgayBatDau { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
        public DateTime NgayKetThuc { get; set; } = DateTime.Now.AddDays(30);

        // ❌ Không đưa DaSuDung vì người dùng không được sửa trực tiếp
        // ❌ Không đưa TrangThai nếu bạn muốn chỉ admin mới quản lý trạng thái
    }
}
