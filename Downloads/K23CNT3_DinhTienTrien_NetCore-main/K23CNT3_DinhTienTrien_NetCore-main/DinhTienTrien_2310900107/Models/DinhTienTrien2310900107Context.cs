using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DinhTienTrien_2310900107.Models;

public partial class DinhTienTrien2310900107Context : DbContext
{
    public DinhTienTrien2310900107Context()
    {
    }

    public DinhTienTrien2310900107Context(DbContextOptions<DinhTienTrien2310900107Context> options)
        : base(options)
    {
    }

    public virtual DbSet<DttEmployee> DttEmployees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=MSI\\BAOMATCAO;Database= DinhTienTrien_2310900107;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DttEmployee>(entity =>
        {
            entity.HasKey(e => e.DttEmpId).HasName("PK__DttEmplo__240A18E80C27A680");

            entity.ToTable("DttEmployee");

            entity.Property(e => e.DttEmpLevel).HasMaxLength(50);
            entity.Property(e => e.DttEmpName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
