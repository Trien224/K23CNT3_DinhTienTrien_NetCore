using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lesson09.Models;

public partial class DttBookStoreContext : DbContext
{
    public DttBookStoreContext()
    {
    }

    public DttBookStoreContext(DbContextOptions<DttBookStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DttAccount> DttAccounts { get; set; }

    public virtual DbSet<DttBook> DttBooks { get; set; }

    public virtual DbSet<DttCategory> DttCategories { get; set; }

    public virtual DbSet<DttOrderBook> DttOrderBooks { get; set; }

    public virtual DbSet<DttOrderDetail> DttOrderDetails { get; set; }

    public virtual DbSet<DttPublisher> DttPublishers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=MSI\\BAOMATCAO;Database=DttBookStore;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DttAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__DttAccou__349DA5A68FA46195");

            entity.ToTable("DttAccount");

            entity.Property(e => e.AccountId).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(512);
            entity.Property(e => e.Email)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Picture)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(64)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DttBook>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__DttBook__3DE0C2070F23D718");

            entity.ToTable("DttBook");

            entity.Property(e => e.BookId)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Picture).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Category).WithMany(p => p.DttBooks)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__DttBook__Categor__3C69FB99");

            entity.HasOne(d => d.Publisher).WithMany(p => p.DttBooks)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__DttBook__Publish__3B75D760");
        });

        modelBuilder.Entity<DttCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__DttCateg__19093A0B1FF8A705");

            entity.ToTable("DttCategory");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<DttOrderBook>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__DttOrder__C3905BCF3153E281");

            entity.ToTable("DttOrderBook");

            entity.Property(e => e.OrderId)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Note).HasMaxLength(512);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderReceive).HasColumnType("datetime");
            entity.Property(e => e.ReceiveAddress).HasMaxLength(512);
            entity.Property(e => e.ReceivePhone)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(16)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.DttOrderBooks)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__DttOrderB__Accou__412EB0B6");
        });

        modelBuilder.Entity<DttOrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__DttOrder__D3B9D36CD6D67801");

            entity.ToTable("DttOrderDetail");

            entity.Property(e => e.BookId)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.OrderId)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.TotalMoney).HasComputedColumnSql("([Quantity]*[Price])", false);

            entity.HasOne(d => d.Book).WithMany(p => p.DttOrderDetails)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_DttOrderDetail_BookId");

            entity.HasOne(d => d.Order).WithMany(p => p.DttOrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_DttOrderDetail_OrderId");
        });

        modelBuilder.Entity<DttPublisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId).HasName("PK__DttPubli__4C657FABE5367CC6");

            entity.ToTable("DttPublisher");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PublisherName).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
