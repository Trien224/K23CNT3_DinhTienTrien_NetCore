using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NemeChuaShop.Models;

public partial class NemeChuaShopContext : DbContext
{
    public NemeChuaShopContext()
    {
    }

    public NemeChuaShopContext(DbContextOptions<NemeChuaShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }

    public virtual DbSet<NguoiBan> NguoiBans { get; set; }

    public virtual DbSet<QuanTriVien> QuanTriViens { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\BAOMATCAO;Database=NemeChuaShop;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CHI_TIET__3214EC27D1936EF1");

            entity.ToTable("CHI_TIET_HOA_DON");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DonGiaMua)
                .HasColumnType("decimal(15, 3)")
                .HasColumnName("DON_GIA_MUA");
            entity.Property(e => e.MaHoaDon)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("MA_HOA_DON");
            entity.Property(e => e.MaSanPham)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_SAN_PHAM");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAY_TAO");
            entity.Property(e => e.SoLuongMua).HasColumnName("SO_LUONG_MUA");
            entity.Property(e => e.ThanhTien)
                .HasColumnType("decimal(15, 3)")
                .HasColumnName("THANH_TIEN");
            entity.Property(e => e.TrangThai)
                .HasDefaultValue(true)
                .HasColumnName("TRANG_THAI");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasPrincipalKey(p => p.MaHoaDon)
                .HasForeignKey(d => d.MaHoaDon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___MA_HO__619B8048");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasPrincipalKey(p => p.MaSanPham)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHI_TIET___MA_SA__628FA481");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HOA_DON__3214EC274EA9A9B2");

            entity.ToTable("HOA_DON");

            entity.HasIndex(e => e.MaHoaDon, "UQ__HOA_DON__EFEAFCDAD3FAC248").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(255)
                .HasColumnName("DIA_CHI");
            entity.Property(e => e.DienThoai)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("DIEN_THOAI");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.HoTenKhachHang)
                .HasMaxLength(255)
                .HasColumnName("HO_TEN_KHACH_HANG");
            entity.Property(e => e.MaHoaDon)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("MA_HOA_DON");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NgayHoaDon)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("NGAY_HOA_DON");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAY_TAO");
            entity.Property(e => e.TongTriGia)
                .HasColumnType("decimal(15, 3)")
                .HasColumnName("TONG_TRI_GIA");
            entity.Property(e => e.TrangThai)
                .HasDefaultValue(true)
                .HasColumnName("TRANG_THAI");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.HoaDons)
                .HasPrincipalKey(p => p.MaKhachHang)
                .HasForeignKey(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HOA_DON__MA_KHAC__5BE2A6F2");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__KHACH_HA__3214EC273FAD9B54");

            entity.ToTable("KHACH_HANG");

            entity.HasIndex(e => e.Email, "UQ__KHACH_HA__161CF7241F55A6A2").IsUnique();

            entity.HasIndex(e => e.MaKhachHang, "UQ__KHACH_HA__C5A28F794FA9AC6D").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(255)
                .HasColumnName("DIA_CHI");
            entity.Property(e => e.DienThoai)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("DIEN_THOAI");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.HoTen)
                .HasMaxLength(255)
                .HasColumnName("HO_TEN");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("MAT_KHAU");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NgayDangKy)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("NGAY_DANG_KY");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAY_TAO");
            entity.Property(e => e.TrangThai)
                .HasDefaultValue(true)
                .HasColumnName("TRANG_THAI");
        });

        modelBuilder.Entity<LoaiSanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LOAI_SAN__3214EC271E5EB628");

            entity.ToTable("LOAI_SAN_PHAM");

            entity.HasIndex(e => e.MaLoai, "UQ__LOAI_SAN__6D8E341DD22A9C69").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaLoai)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_LOAI");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAY_TAO");
            entity.Property(e => e.TenLoai)
                .HasMaxLength(255)
                .HasColumnName("TEN_LOAI");
            entity.Property(e => e.TrangThai)
                .HasDefaultValue(true)
                .HasColumnName("TRANG_THAI");
        });

        modelBuilder.Entity<NguoiBan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NGUOI_BA__3214EC27E7090D4C");

            entity.ToTable("NGUOI_BAN");

            entity.HasIndex(e => e.MaNguoiBan, "UQ__NGUOI_BA__0F0914271CEA97D3").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AnhDaiDienCuaHang)
                .HasMaxLength(255)
                .HasColumnName("ANH_DAI_DIEN_CUA_HANG");
            entity.Property(e => e.DiaChiKinhDoanh)
                .HasMaxLength(255)
                .HasColumnName("DIA_CHI_KINH_DOANH");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_KHACH_HANG");
            entity.Property(e => e.MaNguoiBan)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_NGUOI_BAN");
            entity.Property(e => e.MoTaCuaHang).HasColumnName("MO_TA_CUA_HANG");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NgayDangKy)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("NGAY_DANG_KY");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAY_TAO");
            entity.Property(e => e.SoDienThoaiCuaHang)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("SO_DIEN_THOAI_CUA_HANG");
            entity.Property(e => e.TenCuaHang)
                .HasMaxLength(255)
                .HasColumnName("TEN_CUA_HANG");
            entity.Property(e => e.TrangThaiHoatDong)
                .HasDefaultValue(true)
                .HasColumnName("TRANG_THAI_HOAT_DONG");
            entity.Property(e => e.TrangThaiXacThuc).HasColumnName("TRANG_THAI_XAC_THUC");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.NguoiBans)
                .HasPrincipalKey(p => p.MaKhachHang)
                .HasForeignKey(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NGUOI_BAN__MA_KH__4AB81AF0");
        });

        modelBuilder.Entity<QuanTriVien>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QUAN_TRI__3214EC2761BDCF82");

            entity.ToTable("QUAN_TRI_VIEN");

            entity.HasIndex(e => e.TaiKhoan, "UQ__QUAN_TRI__275EFC149F48511C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("MAT_KHAU");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAY_TAO");
            entity.Property(e => e.TaiKhoan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TAI_KHOAN");
            entity.Property(e => e.TrangThai)
                .HasDefaultValue(true)
                .HasColumnName("TRANG_THAI");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SAN_PHAM__3214EC278FFFA5E0");

            entity.ToTable("SAN_PHAM");

            entity.HasIndex(e => e.MaSanPham, "UQ__SAN_PHAM__AEAADD682FADB699").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DonGia)
                .HasColumnType("decimal(15, 3)")
                .HasColumnName("DON_GIA");
            entity.Property(e => e.HinhAnh)
                .HasMaxLength(255)
                .HasColumnName("HINH_ANH");
            entity.Property(e => e.MaLoai)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_LOAI");
            entity.Property(e => e.MaNguoiBan)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_NGUOI_BAN");
            entity.Property(e => e.MaSanPham)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MA_SAN_PHAM");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAY_CAP_NHAT");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NGAY_TAO");
            entity.Property(e => e.SoLuong).HasColumnName("SO_LUONG");
            entity.Property(e => e.TenSanPham)
                .HasMaxLength(255)
                .HasColumnName("TEN_SAN_PHAM");
            entity.Property(e => e.TrangThai)
                .HasDefaultValue(true)
                .HasColumnName("TRANG_THAI");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.SanPhams)
                .HasPrincipalKey(p => p.MaLoai)
                .HasForeignKey(d => d.MaLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SAN_PHAM__MA_LOA__52593CB8");

            entity.HasOne(d => d.MaNguoiBanNavigation).WithMany(p => p.SanPhams)
                .HasPrincipalKey(p => p.MaNguoiBan)
                .HasForeignKey(d => d.MaNguoiBan)
                .HasConstraintName("FK__SAN_PHAM__MA_NGU__534D60F1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
