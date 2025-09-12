using System.ComponentModel.DataAnnotations;
namespace Project2_QuanLyTapHoa.Models;

public class DanhGiaSanPhamDto
{
    public int MaDanhGia { get; set; }

    [Required(ErrorMessage = "Khách hàng bắt buộc chọn")]
    public int? MaKh { get; set; }

    [Required(ErrorMessage = "Sản phẩm bắt buộc chọn")]
    public int? MaSp { get; set; }

    [Range(1, 5, ErrorMessage = "Số sao phải từ 1-5")]
    public int? SoSao { get; set; }

    public string? NoiDung { get; set; }
    public bool LaYeuThich { get; set; }
    public DateTime NgayTao { get; set; }
    public bool TrangThai { get; set; }

    // Hiển thị
    public string? TenKhachHang { get; set; }
    public string? TenSanPham { get; set; }
}
