using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dto;
using Core.Models;
using Dapper;
using Infrastructure.Interfaces;
using Npgsql;

namespace API.Repositories
{
	public class StudentRepository : IStudentRepository
	{
		public async Task<List<Student>> GetAll()
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			
			var result = await connection.QueryAsync<Student>("SELECT * FROM student ORDER BY id");
			
			await connection.CloseAsync();
			return (List<Student>) result;
		}
		
		public async Task<PagedData<Student>> GetPaged(int page, int pageSize)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			
			await connection.OpenAsync();
			
			int skip = (page - 1) * pageSize;
			var items =
				await connection.QueryAsync<Student>("SELECT * FROM student ORDER BY id OFFSET @skip LIMIT @pageSize", 
					new {skip, pageSize});
			var total = await connection.QueryFirstAsync<int>("SELECT COUNT(*) FROM student");
			
			await connection.CloseAsync();

			var result = new PagedData<Student>
			{
				TotalItems = total,
				PageSize = pageSize,
				Value = (List<Student>) items,
			};

			return result;
		}

		public async Task<List<Course>> GetCoursesFromId(int id)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			
			var result = await connection.QueryAsync<Course>(
				@"SELECT course.id, course.name FROM course INNER JOIN _attends 
			ON _attends.course_id = course.id WHERE _attends.student_id = @ID", new {ID = id});
			
			await connection.CloseAsync();
			
			return (List<Course>) result;
		}

		public async ValueTask<Student> GetFromId(int id)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			
			var result =
				await connection.QueryFirstAsync<Student>("SELECT * FROM student WHERE id = @ID", new {ID = id});
			
			await connection.CloseAsync();
			
			return result;
		}

		public async Task<Student> Create(NameData data)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));

			await connection.OpenAsync();
			var result =
				await connection.QueryFirstAsync<Student>("INSERT INTO student(name) VALUES (@name)", new {name = data.Name});
			await connection.CloseAsync();
			return result;
		}

		public async Task<Student> Update(int id, string name)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<Student>("UPDATE student SET name = @name WHERE id = @ID",
				new {name, ID = id});
			await connection.CloseAsync();
			return result;
		}

		public async Task Delete(int id)
		{
			using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));

			await connection.OpenAsync();
			
			var result = await connection.QueryFirstAsync<Student>("DELETE FROM student WHERE id = @ID", new {ID = id});
			
			await connection.CloseAsync();
		}
	}
}