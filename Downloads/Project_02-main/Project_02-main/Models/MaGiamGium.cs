using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_02.Models
{
    public class MaGiamGium : MaGiamGiumDto
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
        [Column(TypeName = "decimal(5,2)")]
        public decimal? PhanTramGiam { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Giảm tiền trực tiếp không hợp lệ")]
        public decimal? GiamTienTrucTiep { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá trị đơn tối thiểu không hợp lệ")]
        public decimal? GiaTriDonToiThieu { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải >= 0")]
        public int SoLuong { get; set; }

        [Range(0, int.MaxValue)]
        public int DaSuDung { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
        public DateTime NgayBatDau { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
        public DateTime NgayKetThuc { get; set; } = DateTime.Now.AddDays(30);

        /// <summary>
        /// true = đang hoạt động, false = ngừng sử dụng
        /// </summary>
        public bool TrangThai { get; set; } = true;

        // 🔗 Navigation
        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
    }
}
