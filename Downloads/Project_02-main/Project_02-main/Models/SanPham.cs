using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_02.Models
{
    public class SanPham
    {
        [Key]
        public int MaSp { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [MaxLength(150, ErrorMessage = "Tên sản phẩm tối đa 150 ký tự")]
        public string TenSp { get; set; } = null!;

        [MaxLength(1000, ErrorMessage = "Mô tả tối đa 1000 ký tự")]
        public string? MoTa { get; set; }

        [Required(ErrorMessage = "Đơn giá là bắt buộc")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải >= 0")]
        public decimal DonGia { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải >= 0")]
        public int SoLuong { get; set; }

        [MaxLength(255)]
        // 🔹 Lưu tên file ảnh trong database
        public string? HinhAnh { get; set; }

        [NotMapped] // Không lưu vào database
        public IFormFile? UploadImage { get; set; }

        public int? MaLoai { get; set; }

        [Range(0, 100, ErrorMessage = "Phần trăm giảm phải từ 0 đến 100")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PhanTramGiam { get; set; }

        /// <summary>
        /// true = hiển thị, false = ẩn
        /// </summary>
        public bool TrangThai { get; set; } = true;

        // 🔗 Navigation
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
        public virtual ICollection<DanhGiaSanPham> DanhGiaSanPhams { get; set; } = new List<DanhGiaSanPham>();

        [ForeignKey("MaLoai")]
        public virtual LoaiSanPham? MaLoaiNavigation { get; set; }
        
    }

}
