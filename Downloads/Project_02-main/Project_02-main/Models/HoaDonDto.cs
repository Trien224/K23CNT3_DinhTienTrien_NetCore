using System;
using System.Collections.Generic;

namespace Project_02.Models
{
    public class HoaDonDto
    {
        public int? MaHd { get; set; }
        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
        public decimal? TienGiamGia { get; set; }
        public decimal ThanhTien { get; set; }
        public int MaKh { get; set; }
        public string? MaVoucher { get; set; }
        public string? DiaChiGiaoHang { get; set; }
        public string? SdtgiaoHang { get; set; }
        public string? GhiChu { get; set; }
        public int TrangThai { get; set; }

        public List<ChiTietHoaDonsDto>? ChiTietHoaDons { get; set; }
    }

    public class ChiTietHoaDonsDto
    {
        public int MaSp { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
    }
}
