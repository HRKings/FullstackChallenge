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
				var repository = new CourseRepository(context);
				var courses = await new CourseController().Get(repository) as OkObjectResult;

				Assert.NotNull(courses);
				Assert.Equal(3, ((List<Course>) courses.Value).Count);
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
				var repository = new CourseRepository(context);
				var courses = await new CourseController().Get(repository, 1, 2) as OkObjectResult;

				Assert.NotNull(courses);
				Assert.Equal(2, ((PagedData<List<Course>>) courses.Value).Value.Count);
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
				var repository = new CourseRepository(context);
				var course = await new CourseController().GetFromId(repository, 2) as OkObjectResult;

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
			
			var repository = new CourseRepository(context);
			var course = await new CourseController().Create(repository, new NameData {Name = "Marta"}) as OkObjectResult;

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
				var repository = new CourseRepository(context);
				var course =
					await new CourseController().Update(repository, 1, new NameData {Name = "Jose"}) as OkObjectResult;

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
				var repository = new CourseRepository(context);
				_ = await new CourseController().Delete(repository, 1) as OkObjectResult;
				var courses = await context.Courses.FindAsync(1);

				Assert.Null(courses);
			}
		}
	}
}