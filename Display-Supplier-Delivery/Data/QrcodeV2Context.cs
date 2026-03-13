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

    public virtual DbSet<ControlChartDetail> ControlChartDetails { get; set; }

    public virtual DbSet<ControlChartHeader> ControlChartHeaders { get; set; }

    public virtual DbSet<TbEmployee> TbEmployees { get; set; }

    public virtual DbSet<TbHistory> TbHistories { get; set; }

    public virtual DbSet<TbKanban> TbKanbans { get; set; }

    public virtual DbSet<TbMasterMainMenu> TbMasterMainMenus { get; set; }

    public virtual DbSet<TbMasterRole> TbMasterRoles { get; set; }

    public virtual DbSet<TbMstPartNo> TbMstPartNos { get; set; }

    public virtual DbSet<TbRoleMenu> TbRoleMenus { get; set; }

    public virtual DbSet<TbSpecList> TbSpecLists { get; set; }

    public virtual DbSet<TbSupplier> TbSuppliers { get; set; }

    public virtual DbSet<TbSupplierPlanShipping> TbSupplierPlanShippings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ControlChartDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ControlC__3214EC07B1B5813E");

            entity.ToTable("ControlChartDetail");

            entity.Property(e => e.SpecName)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Value1).HasMaxLength(50);
            entity.Property(e => e.Value2).HasMaxLength(50);
            entity.Property(e => e.Value3).HasMaxLength(50);
            entity.Property(e => e.Value4).HasMaxLength(50);
            entity.Property(e => e.Value5).HasMaxLength(50);

            entity.HasOne(d => d.Header).WithMany(p => p.ControlChartDetails)
                .HasForeignKey(d => d.HeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ControlCh__Heade__276EDEB3");
        });

        modelBuilder.Entity<ControlChartHeader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ControlC__3214EC072287B587");

            entity.ToTable("ControlChartHeader");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Inspector).HasMaxLength(50);
            entity.Property(e => e.Leader).HasMaxLength(50);
            entity.Property(e => e.ProductionLine)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.ProductionProcess)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.RoundTime)
                .IsRequired()
                .HasMaxLength(10);
        });

        modelBuilder.Entity<TbEmployee>(entity =>
        {
            entity.HasKey(e => e.EmpId);

            entity.ToTable("tbEmployee");

            entity.Property(e => e.EmpId)
                .HasMaxLength(50)
                .HasColumnName("EmpID");
            entity.Property(e => e.Active)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Department).HasMaxLength(250);
            entity.Property(e => e.Department2).HasMaxLength(250);
            entity.Property(e => e.EmpNameEn)
                .HasMaxLength(250)
                .HasColumnName("EmpNameEN");
            entity.Property(e => e.EmpNameTh)
                .HasMaxLength(250)
                .HasColumnName("EmpNameTH");
            entity.Property(e => e.Picture).HasColumnType("image");
            entity.Property(e => e.Report)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Scanner)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Signature).HasColumnType("image");
        });

        modelBuilder.Entity<TbHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbHistory");

            entity.Property(e => e.EventDate)
                .HasColumnType("datetime")
                .HasColumnName("event_date");
            entity.Property(e => e.Kanban).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.PartNo).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<TbKanban>(entity =>
        {
            entity.HasKey(e => e.Kanban);

            entity.ToTable("tbKanban");

            entity.Property(e => e.Kanban).HasMaxLength(50);
            entity.Property(e => e.CurrentStatus)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.DatePlan).HasColumnType("datetime");
            entity.Property(e => e.DateReceive).HasColumnType("datetime");
            entity.Property(e => e.DeliveryNo)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.PartNo)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<TbMasterMainMenu>(entity =>
        {
            entity.HasKey(e => e.MenuId);

            entity.ToTable("tbMasterMainMenu");

            entity.Property(e => e.MenuId)
                .HasMaxLength(50)
                .HasColumnName("MenuID");
            entity.Property(e => e.IsActive).HasDefaultValue(1);
            entity.Property(e => e.MenuName)
                .IsRequired()
                .HasMaxLength(250)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<TbMasterRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("tbMasterRole");

            entity.Property(e => e.RoleId)
                .HasMaxLength(50)
                .HasColumnName("RoleID");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(250)
                .HasDefaultValue("");
            entity.Property(e => e.IsActive).HasDefaultValue(1);
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(250)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<TbMstPartNo>(entity =>
        {
            entity.HasKey(e => e.PartNo);

            entity.ToTable("tbMst_PartNo");

            entity.Property(e => e.PartNo).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Supplier).HasMaxLength(50);
            entity.Property(e => e.Units).HasMaxLength(50);
        });

        modelBuilder.Entity<TbRoleMenu>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.MenuId });

            entity.ToTable("tbRoleMenu");

            entity.Property(e => e.RoleId)
                .HasMaxLength(50)
                .HasColumnName("RoleID");
            entity.Property(e => e.MenuId)
                .HasMaxLength(50)
                .HasColumnName("MenuID");
            entity.Property(e => e.IsActive).HasDefaultValue(1);

            entity.HasOne(d => d.Menu).WithMany(p => p.TbRoleMenus)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbRoleMenu_Menu");

            entity.HasOne(d => d.Role).WithMany(p => p.TbRoleMenus)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbRoleMenu_Role");
        });

        modelBuilder.Entity<TbSpecList>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbSpecList");

            entity.Property(e => e.ItemList).HasMaxLength(50);
            entity.Property(e => e.LocationInspect).HasMaxLength(50);
            entity.Property(e => e.SpecValue).HasMaxLength(50);
        });

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
