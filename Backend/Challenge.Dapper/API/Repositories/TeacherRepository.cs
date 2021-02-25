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
	public class TeacherRepository : ITeacherRepository
	{
		public async Task<List<Teacher>> GetAll()
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			
			var result = await connection.QueryAsync<Teacher>("SELECT * FROM teacher ORDER BY id");
			
			await connection.CloseAsync();
			
			return (List<Teacher>) result;
		}

		public async Task<int> Count()
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<int>("SELECT COUNT(*) FROM teacher");
			await connection.CloseAsync();
			return result;
		}

		public async Task<PagedData<Teacher>> GetPaged(int page, int pageSize)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			
			await connection.OpenAsync();
			
			int skip = (page - 1) * pageSize;
			var items =
				await connection.QueryAsync<Teacher>("SELECT * FROM teacher ORDER BY id OFFSET @skip LIMIT @pageSize", 
					new {skip, pageSize});
			var total = await connection.QueryFirstAsync<int>("SELECT COUNT(*) FROM teacher");
			
			await connection.CloseAsync();

			var result = new PagedData<Teacher>
			{
				TotalItems = total,
				PageSize = pageSize,
				Value = (List<Teacher>) items,
			};

			return result;
		}

		public async Task<List<Course>> GetCoursesFromId(int id)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			
			var result = await connection.QueryAsync<Course>(
				@"SELECT course.id, course.name FROM course INNER JOIN _teaches 
			ON _teaches.course_id = course.id WHERE _teaches.teacher_id = @ID", new {ID = id});
			await connection.CloseAsync();
			
			return (List<Course>) result;
		}

		public async ValueTask<Teacher> GetFromId(int id)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result =
				await connection.QueryFirstAsync<Teacher>("SELECT * FROM teacher WHERE id = @ID", new {ID = id});
			await connection.CloseAsync();
			return result;
		}

		public async Task<Teacher> Create(NameData data)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));

			await connection.OpenAsync();
			var result =
				await connection.QueryFirstAsync<Teacher>("INSERT INTO teacher(name) VALUES (@name)", new {name = data.Name});
			await connection.CloseAsync();
			return result;
		}

		public async Task<Teacher> Update(int id, string name)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<Teacher>("UPDATE teacher SET name = @name WHERE id = @ID",
				new {name, ID = id});
			await connection.CloseAsync();
			return result;
		}

		public async Task Delete(int id)
		{
			using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));

			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<Teacher>("DELETE FROM teacher WHERE id = @ID", new {ID = id});
			await connection.CloseAsync();
		}
	}
}