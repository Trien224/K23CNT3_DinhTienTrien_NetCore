using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_02.Models
{
    public class HoaDon
    {
        [Key]
        public int MaHd { get; set; }

        [Required(ErrorMessage = "Ngày lập hóa đơn là bắt buộc")]
        public DateTime NgayLap { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải >= 0")]
        public decimal TongTien { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Tiền giảm giá không hợp lệ")]
        public decimal? TienGiamGia { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Thành tiền phải >= 0")]
        public decimal ThanhTien { get; set; }

        [Required(ErrorMessage = "Khách hàng là bắt buộc")]
        public int MaKh { get; set; }

        [MaxLength(50)]
        public string? MaVoucher { get; set; }

        [MaxLength(255)]
        public string? DiaChiGiaoHang { get; set; }

        [Phone(ErrorMessage = "Số điện thoại giao hàng không hợp lệ")]
        [MaxLength(15)]
        public string? SdtgiaoHang { get; set; }

        [MaxLength(500)]
        public string? GhiChu { get; set; }

        [Required]
        public int TrangThai { get; set; } = 0;

        // Navigation
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

        [ForeignKey("MaKh")]
        public virtual KhachHang MaKhNavigation { get; set; } = null!;

        [ForeignKey("MaVoucher")]
        public virtual MaGiamGium? MaVoucherNavigation { get; set; }
    }
}
