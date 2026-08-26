using System;
using System.Collections.Generic;

namespace NemeChuaShop.Models;

public partial class ChiTietHoaDon
{
    public int Id { get; set; }

    public string MaHoaDon { get; set; } = null!;

    public string MaSanPham { get; set; } = null!;

    public int SoLuongMua { get; set; }

    public decimal DonGiaMua { get; set; }

    public decimal ThanhTien { get; set; }

    public bool TrangThai { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public virtual HoaDon MaHoaDonNavigation { get; set; } = null!;

    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
}
