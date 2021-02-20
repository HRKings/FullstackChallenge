using System;
using Challenge_EF.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Challenge_EF.Context
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
					.HasDefaultValueSql("nextval('attends_id_seq'::regclass)");

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
					.HasDefaultValueSql("nextval('course_id_seq'::regclass)");

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
					.HasDefaultValueSql("nextval('student_id_seq'::regclass)");

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
					.HasDefaultValueSql("nextval('_teaches_id_seq'::regclass)");

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
					.HasDefaultValueSql("nextval('teacher_id_seq'::regclass)");

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(80)
					.HasColumnName("name");
			});
		}
	}
}