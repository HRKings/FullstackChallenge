using System.Collections.Generic;
using System.Threading.Tasks;
using Challenge_EF.Context;
using Challenge_EF.Controllers;
using Challenge_EF.Data;
using Challenge_EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace APIChallenge.Tests.EntityFramework
{
	public class EFCourse
	{
		[Fact]
		public async Task GetAllCourses()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase(databaseName: "GetAllCourses")
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
		public async Task GetPagedCourses()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase(databaseName: "GetPagedCourses")
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
				.UseInMemoryDatabase(databaseName: "GetOneCourse")
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
				.UseInMemoryDatabase(databaseName: "CreateCourse")
				.Options;
			
			// Use a clean instance of the context to run the test
			await using var context = new ChallengeDbContext(options);
			
			var course = await new CourseController().Create(context, new NameData{Name = "Marta"} ) as CreatedAtRouteResult;

			Assert.NotNull(course);
			Assert.Equal("Marta", ((Course) course.Value).Name);
		}
		
		[Fact]
		public async Task UpdateCourse()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase(databaseName: "UpdateCourse")
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
				var course = await new CourseController().Update(context, 1, new NameData{Name = "Jose"}) as OkObjectResult;

				Assert.NotNull(course);
				Assert.Equal("Jose", ((Course) course.Value).Name);
			}
		}
		
		[Fact]
		public async Task DeleteCourse()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase(databaseName: "DeleteCourse")
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