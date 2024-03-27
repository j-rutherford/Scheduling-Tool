using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SATTP.DATA.EF.Models
{
    public partial class SATTPContext : DbContext
    {
        public SATTPContext()
        {
        }

        public SATTPContext(DbContextOptions<SATTPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Enrollment> Enrollments { get; set; } = null!;
        public virtual DbSet<ScheduledClass> ScheduledClasses { get; set; } = null!;
        public virtual DbSet<ScheduledClassStatus> ScheduledClassStatuses { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<StudentStatus> StudentStatuses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasOne(d => d.ScheduledClass)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.ScheduledClassID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollments_ScheduledClasses");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollments_Students");
            });

            modelBuilder.Entity<ScheduledClass>(entity =>
            {
                entity.HasOne(d => d.Course)
                    .WithMany(p => p.ScheduledClasses)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ScheduledClasses_Courses");

                entity.HasOne(d => d.ScheduledClassStatus)
                    .WithMany(p => p.ScheduledClasses)
                    .HasForeignKey(d => d.SCSID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ScheduledClasses_ScheduledClassStatus");
            });

            modelBuilder.Entity<ScheduledClassStatus>(entity =>
            {
                entity.HasKey(e => e.SCSID)
                    .HasName("PK_ScheduledClassStatus");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.State).IsFixedLength();

                entity.HasOne(d => d.StudentStatus)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.SSID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Students_StudentStatuses");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
