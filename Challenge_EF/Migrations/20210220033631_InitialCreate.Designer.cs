﻿// <auto-generated />

using Challenge_EF.Context;
using Challenge_EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Challenge_EF.Migrations
{
    [DbContext(typeof(ChallengeDbContext))]
    [Migration("20210220033631_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "C")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Challenge_EF.Models.Attend", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasDefaultValueSql("nextval('attends_id_seq'::regclass)");

                    b.Property<int>("CourseId")
                        .HasColumnType("integer")
                        .HasColumnName("course_id");

                    b.Property<int>("StudentId")
                        .HasColumnType("integer")
                        .HasColumnName("student_id");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("_attends");
                });

            modelBuilder.Entity("Challenge_EF.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasDefaultValueSql("nextval('course_id_seq'::regclass)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("character varying(80)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("course");
                });

            modelBuilder.Entity("Challenge_EF.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasDefaultValueSql("nextval('student_id_seq'::regclass)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("character varying(80)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("student");
                });

            modelBuilder.Entity("Challenge_EF.Models.Teach", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasDefaultValueSql("nextval('_teaches_id_seq'::regclass)");

                    b.Property<int>("CourseId")
                        .HasColumnType("integer")
                        .HasColumnName("course_id");

                    b.Property<int>("TeacherId")
                        .HasColumnType("integer")
                        .HasColumnName("teacher_id");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("TeacherId");

                    b.ToTable("_teaches");
                });

            modelBuilder.Entity("Challenge_EF.Models.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasDefaultValueSql("nextval('teacher_id_seq'::regclass)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("character varying(80)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("teacher");
                });

            modelBuilder.Entity("Challenge_EF.Models.Attend", b =>
                {
                    b.HasOne("Challenge_EF.Models.Course", "Course")
                        .WithMany("Attends")
                        .HasForeignKey("CourseId")
                        .HasConstraintName("FK__course")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Challenge_EF.Models.Student", "Student")
                        .WithMany("Attends")
                        .HasForeignKey("StudentId")
                        .HasConstraintName("FK_attends_student")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Challenge_EF.Models.Teach", b =>
                {
                    b.HasOne("Challenge_EF.Models.Course", "Course")
                        .WithMany("Teaches")
                        .HasForeignKey("CourseId")
                        .HasConstraintName("FK__course")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Challenge_EF.Models.Teacher", "Teacher")
                        .WithMany("Teaches")
                        .HasForeignKey("TeacherId")
                        .HasConstraintName("FK__teacher")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Challenge_EF.Models.Course", b =>
                {
                    b.Navigation("Attends");

                    b.Navigation("Teaches");
                });

            modelBuilder.Entity("Challenge_EF.Models.Student", b =>
                {
                    b.Navigation("Attends");
                });

            modelBuilder.Entity("Challenge_EF.Models.Teacher", b =>
                {
                    b.Navigation("Teaches");
                });
#pragma warning restore 612, 618
        }
    }
}
