using System;
using System.Collections.Generic;

namespace NemeChuaShop.Models;

public partial class KhachHang
{
    public int Id { get; set; }

    public string MaKhachHang { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string DienThoai { get; set; } = null!;

    public string DiaChi { get; set; } = null!;

    public DateOnly? NgayDangKy { get; set; }

    public bool TrangThai { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<NguoiBan> NguoiBans { get; set; } = new List<NguoiBan>();
}
