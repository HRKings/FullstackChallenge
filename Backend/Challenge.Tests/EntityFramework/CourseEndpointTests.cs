using System.Collections.Generic;
using System.Threading.Tasks;
using Challenge.EF.Context;
using Challenge.EF.Controllers;
using Challenge.EF.Data;
using Challenge.EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Challenge.Tests.EntityFramework
{
	public class CourseEndpointTests
	{
		[Fact]
		public async Task GetAllCourses()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("GetAllCourses")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Courses.AddAsync(new Course {Id = 1, Name = "Math"});
				await context.Courses.AddAsync(new Course {Id = 2, Name = "Science"});
				await context.Courses.AddAsync(new Course {Id = 3, Name = "Geography"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var courses = await new CourseController().Get(context) as OkObjectResult;

				Assert.NotNull(courses);
				Assert.Equal(3, ((List<Course>) courses.Value).Count);
			}
		}

		[Fact]
		public async Task GetTotal()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("GeTotalCourses")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Courses.AddAsync(new Course {Id = 1, Name = "Math"});
				await context.Courses.AddAsync(new Course {Id = 2, Name = "Science"});
				await context.Courses.AddAsync(new Course {Id = 3, Name = "Geography"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var courses = await new CourseController().GetTotal(context) as OkObjectResult;

				Assert.NotNull(courses);
				Assert.Equal(3, (int) courses.Value);
			}
		}

		[Fact]
		public async Task GetPagedCourses()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("GetPagedCourses")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Courses.AddAsync(new Course {Id = 1, Name = "Math"});
				await context.Courses.AddAsync(new Course {Id = 2, Name = "Science"});
				await context.Courses.AddAsync(new Course {Id = 3, Name = "Geography"});
				await context.Courses.AddAsync(new Course {Id = 4, Name = "English"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var courses = await new CourseController().Get(context, 1, 2) as OkObjectResult;

				Assert.NotNull(courses);
				Assert.Equal(2, ((List<Course>) courses.Value).Count);
			}
		}

		[Fact]
		public async Task GetOneCourse()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("GetOneCourse")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Courses.AddAsync(new Course {Id = 1, Name = "Math"});
				await context.Courses.AddAsync(new Course {Id = 2, Name = "Science"});
				await context.Courses.AddAsync(new Course {Id = 3, Name = "Geography"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var course = await new CourseController().GetFromId(context, 2) as OkObjectResult;

				Assert.NotNull(course);
				Assert.Equal("Science", ((Course) course.Value).Name);
			}
		}

		[Fact]
		public async Task CreateCourse()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("CreateCourse")
				.Options;

			// Use a clean instance of the context to run the test
			await using var context = new ChallengeDbContext(options);

			var course = await new CourseController().Create(context, new NameData {Name = "Marta"}) as OkObjectResult;

			Assert.NotNull(course);
			Assert.Equal("Marta", ((Course) course.Value).Name);
		}

		[Fact]
		public async Task UpdateCourse()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("UpdateCourse")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Courses.AddAsync(new Course {Id = 1, Name = "Math"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var course =
					await new CourseController().Update(context, 1, new NameData {Name = "Jose"}) as OkObjectResult;

				Assert.NotNull(course);
				Assert.Equal("Jose", ((Course) course.Value).Name);
			}
		}

		[Fact]
		public async Task DeleteCourse()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("DeleteCourse")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Courses.AddAsync(new Course {Id = 1, Name = "Math"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				_ = await new CourseController().Delete(context, 1) as OkObjectResult;
				var courses = await context.Courses.FindAsync(1);

				Assert.Null(courses);
			}
		}
	}
}