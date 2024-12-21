using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Clarion.Ecom.API.Models;

public partial class ClarionECOMDBContext : DbContext
{
    public ClarionECOMDBContext()
    {
    }

    public ClarionECOMDBContext(DbContextOptions<ClarionECOMDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TravelDuration> TravelDurations { get; set; }

    public virtual DbSet<TravelType> TravelTypes { get; set; }

    public virtual DbSet<UserMaster> UserMasters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ClarionECOMCS");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TravelDuration>(entity =>
        {
            entity.HasKey(e => e.DurationID).HasName("PK_TravelDuration_DurationID");

            entity.ToTable("TravelDuration");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DurationName).HasMaxLength(100);
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Remarks).HasMaxLength(500);
        });

        modelBuilder.Entity<TravelType>(entity =>
        {
            entity.HasKey(e => e.TravelTypeID).HasName("PK_TravelType_TravelTypeID");

            entity.ToTable("TravelType");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.Property(e => e.TravelTypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<UserMaster>(entity =>
        {
            entity.HasKey(e => e.UserID).HasName("PK_UserMaster_UserID");

            entity.ToTable("UserMaster");

            entity.HasIndex(e => e.Email, "UQ_UserMaster_Email").IsUnique();

            entity.HasIndex(e => e.LoginName, "UQ_UserMaster_LoginName").IsUnique();

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(60);
            entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.LastName).HasMaxLength(60);
            entity.Property(e => e.LoginName).HasMaxLength(60);
            entity.Property(e => e.LoginPassword).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(60);
            entity.Property(e => e.MobileNo).HasMaxLength(25);
            entity.Property(e => e.Remarks).HasMaxLength(400);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
