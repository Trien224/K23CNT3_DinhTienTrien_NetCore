using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dtt_2310900107_de05.Models;

public partial class Dinhtientrien2310900107De05Context : DbContext
{
    public Dinhtientrien2310900107De05Context()
    {
    }

    public Dinhtientrien2310900107De05Context(DbContextOptions<Dinhtientrien2310900107De05Context> options)
        : base(options)
    {
    }

    public virtual DbSet<DttTask> DttTasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=MSI\\BAOMATCAO;Database=dinhtientrien_2310900107_de05;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DttTask>(entity =>
        {
            entity.HasKey(e => e.DttTaskId).HasName("PK__DttTask__4D1C66FA5114300A");

            entity.ToTable("DttTask");

            entity.Property(e => e.DttTaskLevel).HasMaxLength(20);
            entity.Property(e => e.DttTaskName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
