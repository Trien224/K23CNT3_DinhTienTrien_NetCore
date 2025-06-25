using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lesson10.Models;

public partial class DttK23cnt3Lesson10dbContext : DbContext
{
    public DttK23cnt3Lesson10dbContext()
    {
    }

    public DttK23cnt3Lesson10dbContext(DbContextOptions<DttK23cnt3Lesson10dbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DttLesson> DttLessons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=MSI\\BAOMATCAO;Database=DttK23CNT3_Lesson10db;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DttLesson>(entity =>
        {
            entity.HasKey(e => e.DttId).HasName("PK__DttLesso__044EE1E55342E4F3");

            entity.ToTable("DttLesson");

            entity.Property(e => e.DttImage).HasMaxLength(255);
            entity.Property(e => e.DttName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
