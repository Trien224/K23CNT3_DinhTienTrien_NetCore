using System;
using System.Collections.Generic;
using System.Linq;
using NemeShop.Models;

namespace NemeShop.Extensions
{
    public static class DbSeeder
    {
        public static void SeedData(QuanLyTapHoaContext context)
        {
            // 1. Seed Quản Trị Viên (Admin)
            if (!context.QuanTriViens.Any())
            {
                var admins = new List<QuanTriVien>
                {
                    new QuanTriVien
                    {
                        HoTen = "Quản Trị Viên Hệ Thống",
                        Email = "admin@nemeshop.vn",
                        MatKhau = "admin123",
                        DienThoai = "0988888888",
                        DiaChi = "Hà Nội",
                        TrangThai = true
                    },
                    new QuanTriVien
                    {
                        HoTen = "Đinh Tiến Triển",
                        Email = "trien@nemeshop.vn",
                        MatKhau = "123456",
                        DienThoai = "0912345678",
                        DiaChi = "Hà Nội",
                        TrangThai = true
                    }
                };
                context.QuanTriViens.AddRange(admins);
                context.SaveChanges();
            }

            // 2. Seed Khách Hàng (Customers)
            if (!context.KhachHangs.Any())
            {
                var customers = new List<KhachHang>
                {
                    new KhachHang
                    {
                        HoTen = "Nguyễn Văn An",
                        Email = "khachhang@gmail.com",
                        MatKhau = "123456",
                        DienThoai = "0901234567",
                        DiaChi = "123 Cầu Giấy, Hà Nội",
                        TrangThai = true
                    },
                    new KhachHang
                    {
                        HoTen = "Trần Thị Mai",
                        Email = "user@nemeshop.vn",
                        MatKhau = "123456",
                        DienThoai = "0909876543",
                        DiaChi = "456 Lê Lợi, Quận 1, TP. Hồ Chí Minh",
                        TrangThai = true
                    },
                    new KhachHang
                    {
                        HoTen = "Lê Hoàng Nam",
                        Email = "hoangnam@gmail.com",
                        MatKhau = "123456",
                        DienThoai = "0934567890",
                        DiaChi = "78 Nguyễn Trãi, Thanh Xuân, Hà Nội",
                        TrangThai = true
                    }
                };
                context.KhachHangs.AddRange(customers);
                context.SaveChanges();
            }

            // 3. Seed Loại Sản Phẩm (Categories)
            if (!context.LoaiSanPhams.Any())
            {
                var categories = new List<LoaiSanPham>
                {
                    new LoaiSanPham
                    {
                        TenLoai = "Nem Chua Truyền Thống",
                        MoTa = "Đặc sản nem chua Thanh Hóa lên men tự nhiên, giòn dai chuẩn vị",
                        TrangThai = true
                    },
                    new LoaiSanPham
                    {
                        TenLoai = "Nem Chua Nướng & Rán",
                        MoTa = "Nem chua nướng than hoa, nem chua rán phố cổ thơm lừng giòn rụm",
                        TrangThai = true
                    },
                    new LoaiSanPham
                    {
                        TenLoai = "Đặc Sản Vùng Miền",
                        MoTa = "Giò lụa, chả bò Đà Nẵng, tré Bình Định và các món quà quê trứ danh",
                        TrangThai = true
                    },
                    new LoaiSanPham
                    {
                        TenLoai = "Đồ Ăn Vặt & Nhậu",
                        MoTa = "Bò khô miếng, gà khô lá chanh, bánh tráng phơi sương ăn là ghiền",
                        TrangThai = true
                    },
                    new LoaiSanPham
                    {
                        TenLoai = "Gia Vị & Nước Chấm",
                        MoTa = "Tương ớt cay nồng, muối ớt xanh, tỏi ớt ngâm giấm ăn kèm chuẩn vị",
                        TrangThai = true
                    }
                };
                context.LoaiSanPhams.AddRange(categories);
                context.SaveChanges();
            }

            // 4. Seed Sản Phẩm (Products)
            if (!context.SanPhams.Any())
            {
                var catList = context.LoaiSanPhams.ToList();
                var catNemTrad = catList.FirstOrDefault(c => c.TenLoai.Contains("Truyền Thống"))?.MaLoai;
                var catNemNuong = catList.FirstOrDefault(c => c.TenLoai.Contains("Nướng"))?.MaLoai;
                var catDacSan = catList.FirstOrDefault(c => c.TenLoai.Contains("Đặc Sản"))?.MaLoai;
                var catAnVat = catList.FirstOrDefault(c => c.TenLoai.Contains("Ăn Vặt"))?.MaLoai;
                var catGiaVi = catList.FirstOrDefault(c => c.TenLoai.Contains("Gia Vị"))?.MaLoai;

                var products = new List<SanPham>
                {
                    // Nem truyền thống
                    new SanPham
                    {
                        TenSp = "Nem Chua Thanh Hóa Cây Dài (Gói 10 cái)",
                        MoTa = "Nem chua cây dài gói lá chuối dày dặn, thịt heo tươi tuyển chọn lên men tự nhiên với tiêu sọ, tỏi ớt cay nồng.",
                        DonGia = 65000,
                        SoLuong = 150,
                        HinhAnh = "/images/00a73c91-b700-415b-bd73-69b9ea925063.jpg",
                        MaLoai = catNemTrad,
                        PhanTramGiam = 10,
                        TrangThai = true
                    },
                    new SanPham
                    {
                        TenSp = "Nem Chua Vuông Đặc Biệt (Gói 10 cái)",
                        MoTa = "Nem chua vuông cỡ lớn nhiều thịt, bì giòn sần sật, vị chua thanh hòa quyện tỏi ớt tươi cay nhẹ.",
                        DonGia = 85000,
                        SoLuong = 200,
                        HinhAnh = "/images/012eac26-d934-4a9d-a0de-37168c5062b8.jpg",
                        MaLoai = catNemTrad,
                        PhanTramGiam = 0,
                        TrangThai = true
                    },
                    new SanPham
                    {
                        TenSp = "Nem Chua Rơm Thanh Hóa Thượng Hạng (Cây 500g)",
                        MoTa = "Nem rơm ủ men truyền thống thơm hương men gạo, thích hợp làm quà biếu tặng hoặc liên hoan tiệc tùng.",
                        DonGia = 140000,
                        SoLuong = 80,
                        HinhAnh = "/images/032bc757-927e-4bc0-916f-c3c2f9b7ca34.jpg",
                        MaLoai = catNemTrad,
                        PhanTramGiam = 15,
                        TrangThai = true
                    },
                    new SanPham
                    {
                        TenSp = "Nem Chua Tỏi Ớt Cay Nồng (Gói 10 cái)",
                        MoTa = "Phiên bản đặc biệt dành cho tín đồ ăn cay với ớt chỉ thiên và tỏi cô đơn thơm lừng.",
                        DonGia = 75000,
                        SoLuong = 120,
                        HinhAnh = "/images/0ee8d205-c85d-45d6-a5e9-38316a89dc5d.jpg",
                        MaLoai = catNemTrad,
                        PhanTramGiam = 5,
                        TrangThai = true
                    },

                    // Nem nướng & Rán
                    new SanPham
                    {
                        TenSp = "Nem Chua Rán Hà Nội Tẩm Bột (Hộp 20 cái)",
                        MoTa = "Nem chua rán phố cổ Hà Nội lăn sẵn bột xù, chỉ cần chiên vàng giòn rụm trong 5 phút chấm tương ớt cay ngọt.",
                        DonGia = 95000,
                        SoLuong = 180,
                        HinhAnh = "/images/110ffae8-c75f-44d3-8917-3bacfa24c620.jpg",
                        MaLoai = catNemNuong,
                        PhanTramGiam = 10,
                        TrangThai = true
                    },
                    new SanPham
                    {
                        TenSp = "Nem Nướng Nha Trang Tươi (Set 10 que kèm nước chấm)",
                        MoTa = "Nem nướng thơm phức nướng than hoa, kèm sốt chấm bơ đậu phộng đặc sánh gia truyền.",
                        DonGia = 125000,
                        SoLuong = 95,
                        HinhAnh = "/images/25bedb11-b66a-4e54-b864-75a76b1bf1ad.jpg",
                        MaLoai = catNemNuong,
                        PhanTramGiam = 20,
                        TrangThai = true
                    },
                    new SanPham
                    {
                        TenSp = "Nem Lụi Cố Đô Huế Tươi Sẵn Que (Gói 10 xiên)",
                        MoTa = "Nem lụi bọc cọng sả tươi dậy mùi thơm, thịt mềm ngọt mọng nước.",
                        DonGia = 110000,
                        SoLuong = 110,
                        HinhAnh = "/images/2c320ec3-e114-4c12-a8ec-cd5ffdd99fa0.jpg",
                        MaLoai = catNemNuong,
                        PhanTramGiam = 0,
                        TrangThai = true
                    },

                    // Đặc sản vùng miền
                    new SanPham
                    {
                        TenSp = "Giò Lụa Ước Lễ Truyền Thống (Cây 1kg)",
                        MoTa = "Giò lụa thơm ngậy từ thịt nạc tươi dẻo quánh, nước mắm cốt nhĩ đậm đà, không hàn the.",
                        DonGia = 210000,
                        SoLuong = 60,
                        HinhAnh = "/images/33196758-b787-4851-8361-6273ab608b86.jpg",
                        MaLoai = catDacSan,
                        PhanTramGiam = 8,
                        TrangThai = true
                    },
                    new SanPham
                    {
                        TenSp = "Chả Bò Đà Nẵng Loại 1 (Cây 500g)",
                        MoTa = "Chả bò nguyên chất tiêu cay nhẹ, giòn ngọt tự nhiên từ thịt bò đùi tươi.",
                        DonGia = 165000,
                        SoLuong = 75,
                        HinhAnh = "/images/62930c47-f3f8-4a85-8b7b-bce0b1ab821c.jpg",
                        MaLoai = catDacSan,
                        PhanTramGiam = 12,
                        TrangThai = true
                    },
                    new SanPham
                    {
                        TenSp = "Tré Rơm Bình Định Đặc Sản (Gói 5 chiếc)",
                        MoTa = "Tré Bình Định vị chua thanh béo bùi, giòn rụm của tai heo và mè rang, riềng ớt cay nồng.",
                        DonGia = 90000,
                        SoLuong = 85,
                        HinhAnh = "/images/6de6e59a-6e58-4fc1-b989-8525e6063ec5.jpg",
                        MaLoai = catDacSan,
                        PhanTramGiam = 0,
                        TrangThai = true
                    },

                    // Đồ ăn vặt & nhậu
                    new SanPham
                    {
                        TenSp = "Bò Khô Miếng Mềm Cay Tây Bắc (Hũ 250g)",
                        MoTa = "Thịt bò tươi sấy khô tẩm ướp mắc khén, hạt dổi thơm nức mũi, vắt thêm chút chanh là tuyệt đỉnh.",
                        DonGia = 175000,
                        SoLuong = 140,
                        HinhAnh = "/images/6ed159b6-0c38-4896-ad56-c5fd63a1dae1.jpg",
                        MaLoai = catAnVat,
                        PhanTramGiam = 15,
                        TrangThai = true
                    },
                    new SanPham
                    {
                        TenSp = "Khô Gà Lá Chanh Xé Cay Giòn (Hũ 300g)",
                        MoTa = "Ức gà xé sợi vàng ươm thơm lá chanh tươi, vị cay ngọt hài hòa nhâm nhi cực ngon.",
                        DonGia = 85000,
                        SoLuong = 220,
                        HinhAnh = "/images/76aa1133-06b6-44e4-9729-286b2aec81c3.jpg",
                        MaLoai = catAnVat,
                        PhanTramGiam = 10,
                        TrangThai = true
                    },
                    new SanPham
                    {
                        TenSp = "Mực Cán Tẩm Cay Nha Trang (Gói 200g)",
                        MoTa = "Mực ống biển cán mỏng tẩm sốt ớt cay ngọt, nướng sơ thơm nồng ăn lai rai.",
                        DonGia = 135000,
                        SoLuong = 90,
                        HinhAnh = "/images/84c70031-9975-44f0-ad16-6d5ecff22c29.jpg",
                        MaLoai = catAnVat,
                        PhanTramGiam = 0,
                        TrangThai = true
                    },

                    // Gia vị & Nước chấm
                    new SanPham
                    {
                        TenSp = "Tương Ớt Mường Khương Truyền Thống (Chai 500ml)",
                        MoTa = "Tương ớt ủ men thủ công từ ớt thóc Tây Bắc, cay nồng tự nhiên chấm nem cực ngon.",
                        DonGia = 45000,
                        SoLuong = 300,
                        HinhAnh = "/images/a61fc9f0-053a-4593-84b9-8dbd2482a01b.jpg",
                        MaLoai = catGiaVi,
                        PhanTramGiam = 0,
                        TrangThai = true
                    },
                    new SanPham
                    {
                        TenSp = "Sốt Muối Ớt Xanh Nha Trang (Chai 300ml)",
                        MoTa = "Muối ớt chanh chua cay mặn ngọt béo ngậy, chuyên dùng chấm hải sản và đồ nướng.",
                        DonGia = 35000,
                        SoLuong = 250,
                        HinhAnh = "/images/a91f690d-c343-4a37-8bde-581a68cfa5ab.jpg",
                        MaLoai = catGiaVi,
                        PhanTramGiam = 5,
                        TrangThai = true
                    }
                };

                context.SanPhams.AddRange(products);
                context.SaveChanges();
            }

            // 5. Seed Mã Giảm Giá (Vouchers)
            if (!context.MaGiamGia.Any())
            {
                var vouchers = new List<MaGiamGium>
                {
                    new MaGiamGium
                    {
                        MaVoucher = "NEMENEW",
                        TenVoucher = "Giảm 10% Chào Bạn Mới",
                        MoTa = "Giảm 10% tổng giá trị đơn hàng cho khách hàng đăng ký mới.",
                        PhanTramGiam = 10,
                        GiamTienTrucTiep = 0,
                        GiaTriDonToiThieu = 100000,
                        SoLuong = 500,
                        DaSuDung = 0,
                        NgayBatDau = DateTime.Now.AddDays(-30),
                        NgayKetThuc = DateTime.Now.AddYears(1),
                        TrangThai = true
                    },
                    new MaGiamGium
                    {
                        MaVoucher = "FREESHIP",
                        TenVoucher = "Miễn Phí Vận Chuyển 30K",
                        MoTa = "Giảm trực tiếp 30.000đ tiền vận chuyển cho đơn từ 150.000đ.",
                        PhanTramGiam = 0,
                        GiamTienTrucTiep = 30000,
                        GiaTriDonToiThieu = 150000,
                        SoLuong = 1000,
                        DaSuDung = 0,
                        NgayBatDau = DateTime.Now.AddDays(-30),
                        NgayKetThuc = DateTime.Now.AddYears(1),
                        TrangThai = true
                    },
                    new MaGiamGium
                    {
                        MaVoucher = "NEMESHOP50K",
                        TenVoucher = "Giảm 50.000đ Đơn Từ 300K",
                        MoTa = "Tặng 50.000đ cho đơn hàng đặc sản từ 300.000đ trở lên.",
                        PhanTramGiam = 0,
                        GiamTienTrucTiep = 50000,
                        GiaTriDonToiThieu = 300000,
                        SoLuong = 300,
                        DaSuDung = 0,
                        NgayBatDau = DateTime.Now.AddDays(-30),
                        NgayKetThuc = DateTime.Now.AddYears(1),
                        TrangThai = true
                    },
                    new MaGiamGium
                    {
                        MaVoucher = "TRIAN20",
                        TenVoucher = "Tri Ân Khách Hàng Giảm 20%",
                        MoTa = "Giảm 20% cho đơn hàng từ 500.000đ.",
                        PhanTramGiam = 20,
                        GiamTienTrucTiep = 0,
                        GiaTriDonToiThieu = 500000,
                        SoLuong = 200,
                        DaSuDung = 0,
                        NgayBatDau = DateTime.Now.AddDays(-30),
                        NgayKetThuc = DateTime.Now.AddYears(1),
                        TrangThai = true
                    }
                };

                context.MaGiamGia.AddRange(vouchers);
                context.SaveChanges();
            }

            // 6. Seed Đánh Giá Sản Phẩm (Reviews)
            if (!context.DanhGiaSanPhams.Any())
            {
                var firstProduct = context.SanPhams.FirstOrDefault();
                var firstCustomer = context.KhachHangs.FirstOrDefault();

                if (firstProduct != null && firstCustomer != null)
                {
                    var reviews = new List<DanhGiaSanPham>
                    {
                        new DanhGiaSanPham
                        {
                            MaKh = firstCustomer.MaKh,
                            MaSp = firstProduct.MaSp,
                            SoSao = 5,
                            NoiDung = "Nem rất ngon, vị chua thanh dịu chuẩn nem Thanh Hóa, bì giòn sần sật. Sẽ ủng hộ shop dài dài!",
                            LaYeuThich = true,
                            NgayTao = DateTime.Now.AddDays(-5),
                            TrangThai = true
                        },
                        new DanhGiaSanPham
                        {
                            MaKh = firstCustomer.MaKh,
                            MaSp = firstProduct.MaSp,
                            SoSao = 5,
                            NoiDung = "Giao hàng hỏa tốc trong ngày, nem còn rất tươi mới. Đóng gói rất cẩn thận!",
                            LaYeuThich = false,
                            NgayTao = DateTime.Now.AddDays(-2),
                            TrangThai = true
                        }
                    };

                    context.DanhGiaSanPhams.AddRange(reviews);
                    context.SaveChanges();
                }
            }
        }
    }
}
