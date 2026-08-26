using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_02.Models
{
    public class LoaiSanPham : LoaiSanPhamDto
    {
        [Key]
        public int MaLoai { get; set; }

        [Required(ErrorMessage = "Tên loại không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên loại tối đa 100 ký tự")]
        public string TenLoai { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "Mô tả tối đa 500 ký tự")]
        public string? MoTa { get; set; }

        /// <summary>
        /// true = hiển thị, false = ẩn
        /// </summary>
        public bool TrangThai { get; set; } = true;

        // 🔗 Navigation
        public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
