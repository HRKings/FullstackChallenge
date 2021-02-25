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
	public class CourseRepository : ICourseRepository
	{
		public async Task<List<Course>> GetAll()
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result = await connection.QueryAsync<Course>("SELECT * FROM course ORDER BY id");
			await connection.CloseAsync();
			
			return (List<Course>) result;
		}
		
		public async Task<PagedData<Course>> GetPaged(int page, int pageSize)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			
			await connection.OpenAsync();
			
			int skip = (page - 1) * pageSize;
			var items =
				await connection.QueryAsync<Course>("SELECT * FROM course ORDER BY id OFFSET @skip LIMIT @pageSize", 
					new {skip, pageSize});
			var total = await connection.QueryFirstAsync<int>("SELECT COUNT(*) FROM course");
			
			await connection.CloseAsync();

			var result = new PagedData<Course>
			{
				TotalItems = total,
				PageSize = pageSize,
				Value = (List<Course>) items,
			};
			
			return result;
		}

		public async ValueTask<Course> GetFromId(int id)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result =
				await connection.QueryFirstAsync<Course>("SELECT * FROM course WHERE id = @ID", new {ID = id});
			await connection.CloseAsync();
			
			return result;
		}

		public async Task<Course> Create(NameData data)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));

			await connection.OpenAsync();
			var result =
				await connection.QueryFirstAsync<Course>("INSERT INTO course(name) VALUES (@name)", new {name = data.Name});
			await connection.CloseAsync();
			return result;
		}

		public async Task<Course> Update(int id, string name)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			
			var result = await connection.QueryFirstAsync<Course>("UPDATE course SET name = @name WHERE id = @id",
				new {name, id});
			
			await connection.CloseAsync();
			return result;
		}

		public async Task Delete(int id)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));

			await connection.OpenAsync();
			
			var result = await connection.QueryFirstAsync<Course>("DELETE FROM course WHERE id = @id", new {id});
			
			await connection.CloseAsync();
		}
	}
}