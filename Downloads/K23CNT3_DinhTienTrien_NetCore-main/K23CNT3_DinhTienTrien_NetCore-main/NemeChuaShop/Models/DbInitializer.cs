using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NemeChuaShop.Models
{
    public static class DbInitializer
    {
        public static void Initialize(NemeChuaShopContext context)
        {
            try
            {
                context.Database.EnsureCreated();

                // Seed LoaiSanPham
                if (!context.LoaiSanPhams.Any())
                {
                    var loaiSanPhams = new LoaiSanPham[]
                    {
                        new LoaiSanPham { MaLoai = "L01", TenLoai = "Nem Chua Thanh Hóa", TrangThai = true, NgayTao = DateTime.Now, NgayCapNhat = DateTime.Now },
                        new LoaiSanPham { MaLoai = "L02", TenLoai = "Nem Chua Rán", TrangThai = true, NgayTao = DateTime.Now, NgayCapNhat = DateTime.Now },
                        new LoaiSanPham { MaLoai = "L03", TenLoai = "Nem Nướng Nha Trang", TrangThai = true, NgayTao = DateTime.Now, NgayCapNhat = DateTime.Now },
                        new LoaiSanPham { MaLoai = "L04", TenLoai = "Nem Lụi Huế", TrangThai = true, NgayTao = DateTime.Now, NgayCapNhat = DateTime.Now }
                    };
                    context.LoaiSanPhams.AddRange(loaiSanPhams);
                    context.SaveChanges();
                }

                // Seed QuanTriVien
                if (!context.QuanTriViens.Any())
                {
                    var admin = new QuanTriVien
                    {
                        TaiKhoan = "admin",
                        MatKhau = "123456",
                        TrangThai = true,
                        NgayTao = DateTime.Now,
                        NgayCapNhat = DateTime.Now
                    };
                    context.QuanTriViens.Add(admin);
                    context.SaveChanges();
                }

                // Seed SanPham
                if (!context.SanPhams.Any())
                {
                    var sanPhams = new SanPham[]
                    {
                        new SanPham
                        {
                            MaSanPham = "SP01",
                            TenSanPham = "Nem Chua Thanh Hóa Truyền Thống",
                            DonGia = 45000,
                            SoLuong = 100,
                            MaLoai = "L01",
                            HinhAnh = "images/ban0.jpg",
                            TrangThai = true,
                            NgayTao = DateTime.Now,
                            NgayCapNhat = DateTime.Now
                        },
                        new SanPham
                        {
                            MaSanPham = "SP02",
                            TenSanPham = "Nem Chua Rán Phố Cổ Tẩm Bột",
                            DonGia = 60000,
                            SoLuong = 50,
                            MaLoai = "L02",
                            HinhAnh = "images/ban1.jpg",
                            TrangThai = true,
                            NgayTao = DateTime.Now,
                            NgayCapNhat = DateTime.Now
                        },
                        new SanPham
                        {
                            MaSanPham = "SP03",
                            TenSanPham = "Nem Nướng Đặc Biệt Kèm Nước Chấm",
                            DonGia = 75000,
                            SoLuong = 30,
                            MaLoai = "L03",
                            HinhAnh = "images/ban2.jpg",
                            TrangThai = true,
                            NgayTao = DateTime.Now,
                            NgayCapNhat = DateTime.Now
                        }
                    };
                    context.SanPhams.AddRange(sanPhams);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during DB initialization: " + ex.Message);
            }
        }
    }
}
