using System.ComponentModel.DataAnnotations;

public class DanhGiaSanPhamDto
{
    public int MaDanhGia { get; set; }

    [Required]
    public int MaKh { get; set; }

    [Required]
    public int MaSp { get; set; }

    [Range(1, 5)]
    public int SoSao { get; set; }

    public string? NoiDung { get; set; }
    public bool LaYeuThich { get; set; }
    public DateTime NgayTao { get; set; }
    public bool TrangThai { get; set; }
}
