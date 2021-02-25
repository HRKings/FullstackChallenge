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
	public class TeacherEndpointTests
	{
		[Fact]
		public async Task GetAllTeachers()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("GetAllTeachers")
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
				var repository = new TeacherRepository(context);
				var teachers = await new TeacherController().Get(repository) as OkObjectResult;

				Assert.NotNull(teachers);
				Assert.Equal(3, ((List<Teacher>) teachers.Value).Count);
			}
		}
		
		[Fact]
		public async Task GetPagedTeachers()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("GetPagedTeachers")
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
				var repository = new TeacherRepository(context);
				var teachers = await new TeacherController().Get(repository, 1, 2) as OkObjectResult;

				Assert.NotNull(teachers);
				Assert.Equal(2, ((PagedData<List<Teacher>>) teachers.Value).Value.Count);
			}
		}

		[Fact]
		public async Task GetOneTeacher()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("GetOneTeacher")
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
				var repository = new TeacherRepository(context);
				var teacher = await new TeacherController().GetFromId(repository, 2) as OkObjectResult;

				Assert.NotNull(teacher);
				Assert.Equal("Pietra", ((Teacher) teacher.Value).Name);
			}
		}

		[Fact]
		public async Task CreateTeacher()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("CreateTeacher")
				.Options;

			// Use a clean instance of the context to run the test
			await using var context = new ChallengeDbContext(options);

			var repository = new TeacherRepository(context);
			var teacher =
				await new TeacherController().Create(repository, new NameData {Name = "Marta"}) as OkObjectResult;

			Assert.NotNull(teacher);
			Assert.Equal("Marta", ((Teacher) teacher.Value).Name);
		}

		[Fact]
		public async Task UpdateTeacher()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("UpdateTeacher")
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
				var repository = new TeacherRepository(context);
				var teacher =
					await new TeacherController().Update(repository, 1, new NameData {Name = "Jose"}) as OkObjectResult;

				Assert.NotNull(teacher);
				Assert.Equal("Jose", ((Teacher) teacher.Value).Name);
			}
		}

		[Fact]
		public async Task DeleteTeacher()
		{
			var options = new DbContextOptionsBuilder<ChallengeDbContext>()
				.UseInMemoryDatabase("DeleteTeacher")
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
				var repository = new TeacherRepository(context);
				_ = await new TeacherController().Delete(repository, 1) as OkObjectResult;
				var teachers = await context.Teachers.FindAsync(1);

				Assert.Null(teachers);
			}
		}
	}
}