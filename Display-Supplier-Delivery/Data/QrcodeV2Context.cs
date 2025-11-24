using System;
using System.Collections.Generic;
using Display_Supplier_Delivery.Models;
using Microsoft.EntityFrameworkCore;

namespace Display_Supplier_Delivery.Data;

public partial class QrcodeV2Context : DbContext
{
    public QrcodeV2Context()
    {
    }

    public QrcodeV2Context(DbContextOptions<QrcodeV2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<TbSupplier> TbSuppliers { get; set; }

    public virtual DbSet<TbSupplierPlanShipping> TbSupplierPlanShippings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbSupplier>(entity =>
        {
            entity.HasKey(e => e.SupId);

            entity.ToTable("tbSupplier");

            entity.Property(e => e.SupId)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SupID");
            entity.Property(e => e.Cycle).HasMaxLength(50);
            entity.Property(e => e.Factory).HasMaxLength(50);
            entity.Property(e => e.LoadingArea).HasMaxLength(50);
            entity.Property(e => e.SupNameEn)
                .HasMaxLength(250)
                .HasColumnName("SupNameEN");
            entity.Property(e => e.SupNameTh)
                .HasMaxLength(250)
                .HasColumnName("SupNameTH");
        });

        modelBuilder.Entity<TbSupplierPlanShipping>(entity =>
        {
            entity.HasKey(e => new { e.SupId, e.StrDay, e.StartTime });

            entity.ToTable("tbSupplierPlanShipping");

            entity.Property(e => e.SupId)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SupID");
            entity.Property(e => e.StrDay)
                .HasMaxLength(50)
                .HasColumnName("strDay");
            entity.Property(e => e.StartTime)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("startTime");
            entity.Property(e => e.EndTime)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("endTime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
