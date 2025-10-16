using System;
using System.Collections.Generic;

namespace NemeChuaShop.Models;

public partial class NguoiBan
{
    public int Id { get; set; }

    public string MaNguoiBan { get; set; } = null!;

    public string MaKhachHang { get; set; } = null!;

    public string TenCuaHang { get; set; } = null!;

    public string? MoTaCuaHang { get; set; }

    public string DiaChiKinhDoanh { get; set; } = null!;

    public string SoDienThoaiCuaHang { get; set; } = null!;

    public string? AnhDaiDienCuaHang { get; set; }

    public DateOnly? NgayDangKy { get; set; }

    public bool TrangThaiXacThuc { get; set; }

    public bool TrangThaiHoatDong { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
