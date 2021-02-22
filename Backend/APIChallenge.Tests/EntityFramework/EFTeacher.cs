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
	public class EFTeacher
	{
		[Fact]
		public async Task GetAllTeachers()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase(databaseName: "GetAllTeachers")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Teachers.AddAsync(new Teacher {Id = 1, Name = "Jonas"});
				await context.Teachers.AddAsync(new Teacher {Id = 2, Name = "Pietra"});
				await context.Teachers.AddAsync(new Teacher {Id = 3, Name = "Maria"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var teachers = await new TeacherController().Get(context) as OkObjectResult;

				Assert.NotNull(teachers);
				Assert.Equal(3, ((List<Teacher>) teachers.Value).Count);
			}
		}
		
		[Fact]
		public async Task GetTotal()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase(databaseName: "GetTotalTeachers")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Teachers.AddAsync(new Teacher {Id = 1, Name = "Jonas"});
				await context.Teachers.AddAsync(new Teacher {Id = 2, Name = "Pietra"});
				await context.Teachers.AddAsync(new Teacher {Id = 3, Name = "Maria"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var teachers = await new TeacherController().GetTotal(context) as OkObjectResult;

				Assert.NotNull(teachers);
				Assert.Equal(3, (int) teachers.Value);
			}
		}
		
		[Fact]
		public async Task GetPagedTeachers()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase(databaseName: "GetPagedTeachers")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Teachers.AddAsync(new Teacher {Id = 1, Name = "Jonas"});
				await context.Teachers.AddAsync(new Teacher {Id = 2, Name = "Pietra"});
				await context.Teachers.AddAsync(new Teacher {Id = 3, Name = "Maria"});
				await context.Teachers.AddAsync(new Teacher {Id = 4, Name = "Henry"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var teachers = await new TeacherController().Get(context, 1, 2) as OkObjectResult;

				Assert.NotNull(teachers);
				Assert.Equal(2, ((List<Teacher>) teachers.Value).Count);
			}
		}
		
		[Fact]
		public async Task GetOneTeacher()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase(databaseName: "GetOneTeacher")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Teachers.AddAsync(new Teacher {Id = 1, Name = "Jonas"});
				await context.Teachers.AddAsync(new Teacher {Id = 2, Name = "Pietra"});
				await context.Teachers.AddAsync(new Teacher {Id = 3, Name = "Maria"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var teacher = await new TeacherController().GetFromId(context, 2) as OkObjectResult;

				Assert.NotNull(teacher);
				Assert.Equal("Pietra", ((Teacher) teacher.Value).Name);
			}
		}
		
		[Fact]
		public async Task CreateTeacher()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase(databaseName: "CreateTeacher")
				.Options;
			
			// Use a clean instance of the context to run the test
			await using var context = new ChallengeDbContext(options);
			
			var teacher = await new TeacherController().Create(context, new NameData{Name = "Marta"} ) as CreatedAtRouteResult;

			Assert.NotNull(teacher);
			Assert.Equal("Marta", ((Teacher) teacher.Value).Name);
		}
		
		[Fact]
		public async Task UpdateTeacher()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase(databaseName: "UpdateTeacher")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Teachers.AddAsync(new Teacher {Id = 1, Name = "Jonas"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				var teacher = await new TeacherController().Update(context, 1, new NameData{Name = "Jose"}) as OkObjectResult;

				Assert.NotNull(teacher);
				Assert.Equal("Jose", ((Teacher) teacher.Value).Name);
			}
		}
		
		[Fact]
		public async Task DeleteTeacher()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase(databaseName: "DeleteTeacher")
				.Options;

			// Insert seed data into the database using one instance of the context
			await using (var context = new ChallengeDbContext(options))
			{
				await context.Teachers.AddAsync(new Teacher {Id = 1, Name = "Jonas"});
				await context.SaveChangesAsync();
			}

			// Use a clean instance of the context to run the test
			await using (var context = new ChallengeDbContext(options))
			{
				_ = await new TeacherController().Delete(context, 1) as OkObjectResult;
				var teachers = await context.Teachers.FindAsync(1);
				
				Assert.Null(teachers);
			}
		}
	}
}