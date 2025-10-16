// Extensions/SanPhamExtensions.cs
using NemeShop.Models;

namespace NemeShop.Extensions
{
    public static class SanPhamExtensions
    {
        public static decimal GiaSauGiam(this SanPham sanPham)
        {
            return sanPham.PhanTramGiam > 0 ?
                   sanPham.DonGia * (1 - sanPham.PhanTramGiam / 100) :
                   sanPham.DonGia;
        }

        public static bool DangGiamGia(this SanPham sanPham)
        {
            return sanPham.PhanTramGiam > 0;
        }

        public static decimal TietKiem(this SanPham sanPham)
        {
            return sanPham.DonGia - sanPham.GiaSauGiam();
        }
    }
}