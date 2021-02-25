using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers;
using API.Repositories;
using Core.Dto;
using Core.Models;
using Infrastructure.Database.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Challenge.Tests.EntityFramework
{
	public class AssociationEndpointTests
	{
		[Fact]
		public async Task AssignTeacher()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("AssignTeacher")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Teachers.AddAsync(new Teacher {Id = 1, Name = "Jonas"});
				await context.Courses.AddAsync(new Course {Id = 1, Name = "Biology"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var repository = new AssociationRepository(context);
				var assign = await new AssociationController().AddTeacherToCourse(repository,
					new TeacherCourseAssociation {TeacherId = 1, CourseId = 1}) as OkObjectResult;

				var association = await context.Teaches.FirstAsync();

				Assert.NotNull(assign);
				Assert.NotNull(association);
				Assert.Equal(1, ((Teach) assign.Value).Id);
			}
		}

		[Fact]
		public async Task AssignStudent()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("AssignStudent")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Students.AddAsync(new Student {Id = 1, Name = "Jonas"});
				await context.Courses.AddAsync(new Course {Id = 1, Name = "Biology"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var repository = new AssociationRepository(context);
				var assign = await new AssociationController().AddStudentToCourse(repository,
					new StudentCourseAssociation {StudentId = 1, CourseId = 1}) as OkObjectResult;

				var association = await context.Attends.FirstAsync();

				Assert.NotNull(assign);
				Assert.NotNull(association);
				Assert.Equal(1, ((Attend) assign.Value).Id);
			}
		}

		[Fact]
		public async Task GetTeacherCourses()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("GetTeacherCourses")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Teachers.AddAsync(new Teacher {Id = 1, Name = "Jonas"});
				await context.Courses.AddAsync(new Course {Id = 1, Name = "Biology"});
				await context.Teaches.AddAsync(new Teach {Id = 1, TeacherId = 1, CourseId = 1});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var repository = new TeacherRepository(context);
				var assign = await new TeacherController().GetCoursesFromId(repository, 1) as OkObjectResult;

				Assert.NotNull(assign);
				Assert.Single((List<Course>) assign.Value);
				Assert.Equal(200, assign.StatusCode);
			}
		}

		[Fact]
		public async Task GetStudentCourses()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("GetStudentCourses")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Students.AddAsync(new Student {Id = 1, Name = "Jonas"});
				await context.Courses.AddAsync(new Course {Id = 1, Name = "Biology"});
				await context.Attends.AddAsync(new Attend {Id = 1, StudentId = 1, CourseId = 1});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var repository = new StudentRepository(context);
				var assign = await new StudentController().GetCoursesFromId(repository, 1) as OkObjectResult;

				Assert.NotNull(assign);
				Assert.Single((List<Course>) assign.Value);
				Assert.Equal(200, assign.StatusCode);
			}
		}

		[Fact]
		public async Task UnassignTeacher()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("UnassignTeacher")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Teachers.AddAsync(new Teacher {Id = 1, Name = "Jonas"});
				await context.Courses.AddAsync(new Course {Id = 1, Name = "Biology"});
				await context.Teaches.AddAsync(new Teach {Id = 1, TeacherId = 1, CourseId = 1});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var repository = new AssociationRepository(context);
				var assign = await new AssociationController().RemoveTeacherFromCourse(repository,
					new TeacherCourseAssociation {TeacherId = 1, CourseId = 1}) as OkResult;

				var association = await context.Teaches.FindAsync(1);

				Assert.NotNull(assign);
				Assert.Null(association);
				Assert.Equal(200, assign.StatusCode);
			}
		}

		[Fact]
		public async Task UnassignStudent()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("UnassignStudent")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Students.AddAsync(new Student {Id = 1, Name = "Jonas"});
				await context.Courses.AddAsync(new Course {Id = 1, Name = "Biology"});
				await context.Attends.AddAsync(new Attend {Id = 1, StudentId = 1, CourseId = 1});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var repository = new AssociationRepository(context);
				var assign = await new AssociationController().RemoveStudentFromCourse(repository,
					new StudentCourseAssociation {StudentId = 1, CourseId = 1}) as OkResult;

				var association = await context.Attends.FindAsync(1);

				Assert.NotNull(assign);
				Assert.Null(association);
				Assert.Equal(200, assign.StatusCode);
			}
		}
	}
}