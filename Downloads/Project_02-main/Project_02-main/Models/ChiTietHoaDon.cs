using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_02.Models
{
    public class ChiTietHoaDon
    {
        [Key]
        public int MaCthd { get; set; }

        [Required]
        public int MaHd { get; set; }

        [Required]
        public int MaSp { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DonGia { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ThanhTien { get; set; }

        public virtual HoaDon? MaHdNavigation { get; set; }
        public virtual SanPham? MaSpNavigation { get; set; }
    }
}
