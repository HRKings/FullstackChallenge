using System;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Infrastructure.Database.Context
{
	public class ChallengeDbContext : DbContext
	{
		public ChallengeDbContext()
		{
		}

		public ChallengeDbContext(DbContextOptions<ChallengeDbContext> options)
			: base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			if (!options.IsConfigured)
			{
				options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE") ?? "Host=localhost;Database=challenge;Username=postgres;Password=");
			}
		}

		public virtual DbSet<Attend> Attends { get; set; }
		public virtual DbSet<Course> Courses { get; set; }
		public virtual DbSet<Student> Students { get; set; }
		public virtual DbSet<Teach> Teaches { get; set; }
		public virtual DbSet<Teacher> Teachers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasAnnotation("Relational:Collation", "C");

			// _attends Table
			modelBuilder.Entity<Attend>(entity =>
			{
				entity.ToTable("_attends");

				entity.Property(e => e.Id)
					.HasColumnName("id")
					.UseSerialColumn()
					.ValueGeneratedOnAdd();

				entity.Property(e => e.CourseId).HasColumnName("course_id");

				entity.Property(e => e.StudentId).HasColumnName("student_id");

				entity.HasOne(d => d.Course)
					.WithMany(p => p.Attends)
					.HasForeignKey(d => d.CourseId)
					.HasConstraintName("FK__course");

				entity.HasOne(d => d.Student)
					.WithMany(p => p.Attends)
					.HasForeignKey(d => d.StudentId)
					.HasConstraintName("FK_attends_student");
			});

			// Course Table
			modelBuilder.Entity<Course>(entity =>
			{
				entity.ToTable("course");

				entity.Property(e => e.Id)
					.HasColumnName("id")
					.UseSerialColumn()
					.ValueGeneratedOnAdd();

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(80)
					.HasColumnName("name");
			});

			// Student Table
			modelBuilder.Entity<Student>(entity =>
			{
				entity.ToTable("student");

				entity.Property(e => e.Id)
					.HasColumnName("id")
					.UseSerialColumn()
					.ValueGeneratedOnAdd();

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(80)
					.HasColumnName("name");
			});

			// _teaches Table
			modelBuilder.Entity<Teach>(entity =>
			{
				entity.ToTable("_teaches");

				entity.Property(e => e.Id)
					.HasColumnName("id")
					.UseSerialColumn()
					.ValueGeneratedOnAdd();

				entity.Property(e => e.CourseId).HasColumnName("course_id");

				entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

				entity.HasOne(d => d.Course)
					.WithMany(p => p.Teaches)
					.HasForeignKey(d => d.CourseId)
					.HasConstraintName("FK__course");

				entity.HasOne(d => d.Teacher)
					.WithMany(p => p.Teaches)
					.HasForeignKey(d => d.TeacherId)
					.HasConstraintName("FK__teacher");
			});

			// Teacher Table
			modelBuilder.Entity<Teacher>(entity =>
			{
				entity.ToTable("teacher");

				entity.Property(e => e.Id)
					.HasColumnName("id")
					.UseSerialColumn()
					.ValueGeneratedOnAdd();

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(80)
					.HasColumnName("name");
			});
		}
	}
}