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
	public class StudentEndpointTests
	{
		[Fact]
		public async Task GetAllStudents()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("GetAllStudents")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Students.AddAsync(new Student {Id = 1, Name = "Jonas"});
				await context.Students.AddAsync(new Student {Id = 2, Name = "Pietra"});
				await context.Students.AddAsync(new Student {Id = 3, Name = "Maria"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var students = await new StudentController().Get(context) as OkObjectResult;

				Assert.NotNull(students);
				Assert.Equal(3, ((List<Student>) students.Value).Count);
			}
		}

		[Fact]
		public async Task GetTotal()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("GetTotalStudents")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Students.AddAsync(new Student {Id = 1, Name = "Jonas"});
				await context.Students.AddAsync(new Student {Id = 2, Name = "Pietra"});
				await context.Students.AddAsync(new Student {Id = 3, Name = "Maria"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var students = await new StudentController().GetTotal(context) as OkObjectResult;

				Assert.NotNull(students);
				Assert.Equal(3, (int) students.Value);
			}
		}

		[Fact]
		public async Task GetPagedStudents()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("GetPagedStudents")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Students.AddAsync(new Student {Id = 1, Name = "Jonas"});
				await context.Students.AddAsync(new Student {Id = 2, Name = "Pietra"});
				await context.Students.AddAsync(new Student {Id = 3, Name = "Maria"});
				await context.Students.AddAsync(new Student {Id = 4, Name = "Henry"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var students = await new StudentController().Get(context, 1, 2) as OkObjectResult;

				Assert.NotNull(students);
				Assert.Equal(2, ((List<Student>) students.Value).Count);
			}
		}

		[Fact]
		public async Task GetOneStudent()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("GetOneStudent")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Students.AddAsync(new Student {Id = 1, Name = "Jonas"});
				await context.Students.AddAsync(new Student {Id = 2, Name = "Pietra"});
				await context.Students.AddAsync(new Student {Id = 3, Name = "Maria"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var student = await new StudentController().GetFromId(context, 2) as OkObjectResult;

				Assert.NotNull(student);
				Assert.Equal("Pietra", ((Student) student.Value).Name);
			}
		}

		[Fact]
		public async Task CreateStudent()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("CreateStudent")
				.Options;

			// Use a clean instance of the context to run the test
			await using var context = new ChallengeDbContext(options);

			var student =
				await new StudentController().Create(context, new NameData {Name = "Marta"}) as OkObjectResult;

			Assert.NotNull(student);
			Assert.Equal("Marta", ((Student) student.Value).Name);
		}

		[Fact]
		public async Task UpdateStudent()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("UpdateStudent")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Students.AddAsync(new Student {Id = 1, Name = "Jonas"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var student =
					await new StudentController().Update(context, 1, new NameData {Name = "Jose"}) as OkObjectResult;

				Assert.NotNull(student);
				Assert.Equal("Jose", ((Student) student.Value).Name);
			}
		}

		[Fact]
		public async Task DeleteStudent()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("DeleteStudent")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Students.AddAsync(new Student {Id = 1, Name = "Jonas"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				_ = await new StudentController().Delete(context, 1) as OkObjectResult;
				var students = await context.Students.FindAsync(1);

				Assert.Null(students);
			}
		}
	}
}