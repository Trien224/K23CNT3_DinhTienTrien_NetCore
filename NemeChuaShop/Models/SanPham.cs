using System;
using System.Collections.Generic;

namespace NemeChuaShop.Models;

public partial class SanPham
{
    public int Id { get; set; }

    public string MaSanPham { get; set; } = null!;

    public string TenSanPham { get; set; } = null!;

    public string? HinhAnh { get; set; }

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public string MaLoai { get; set; } = null!;

    public string? MaNguoiBan { get; set; }

    public bool TrangThai { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual LoaiSanPham MaLoaiNavigation { get; set; } = null!;

    public virtual NguoiBan? MaNguoiBanNavigation { get; set; }
}
