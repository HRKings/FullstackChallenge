using System.Collections.Generic;
using System.Threading.Tasks;
using Challenge_Dapper.Models;
using Dapper;
using Npgsql;

namespace Challenge_Dapper.Repositories
{
	public class CourseRepository
	{
		public static async Task<IEnumerable<Course>> GetAll()
		{
			await using var connection =
				new NpgsqlConnection("Host=localhost;Database=challenge;Username=postgres;Password=");
			await connection.OpenAsync();
			var result = await connection.QueryAsync<Course>("SELECT * FROM course ORDER BY id");
			await connection.CloseAsync();
			return result;
		}
		
		public static async Task<int> Count()
		{
			await using var connection =
				new NpgsqlConnection("Host=localhost;Database=challenge;Username=postgres;Password=");
			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<int>("SELECT COUNT(*) FROM course");
			await connection.CloseAsync();
			return result;
		}

		public static async Task<IEnumerable<Course>> GetPaged(int page, int pageSize)
		{
			await using var connection =
				new NpgsqlConnection("Host=localhost;Database=challenge;Username=postgres;Password=");
			int skip = (page - 1) * pageSize;
			await connection.OpenAsync();
			var result = await connection.QueryAsync<Course>("SELECT * FROM course ORDER BY id OFFSET @skip LIMIT @pageSize",
				new {skip, pageSize});
			await connection.CloseAsync();
			return result;
		}
		
		public static async Task<Course> Get(int id)
		{
			await using var connection =
				new NpgsqlConnection("Host=localhost;Database=challenge;Username=postgres;Password=");
			await connection.OpenAsync();
			var result =
				await connection.QueryFirstAsync<Course>("SELECT * FROM course WHERE id = @ID", new {ID = id});
			await connection.CloseAsync();
			return result;
		}

		public static async Task<Course> Insert(string name)
		{
			await using var connection =
				new NpgsqlConnection("Host=localhost;Database=challenge;Username=postgres;Password=");

			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<Course>("INSERT INTO course(name) VALUES (@name)", new {name});
			await connection.CloseAsync();
			return result;
		}

		public static async Task<Course> Update(Course course)
		{
			await using var connection =
				new NpgsqlConnection("Host=localhost;Database=challenge;Username=postgres;Password=");
			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<Course>("UPDATE course SET name = @name WHERE id = @ID", new {name = course.Name, ID = course.Id});
			await connection.CloseAsync();
			return result;
		}

		public static async Task<Course> Delete(int id)
		{
			using var connection =
				new NpgsqlConnection("Host=localhost;Database=challenge;Username=postgres;Password=");

			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<Course>("DELETE FROM course WHERE id = @ID", new {ID = id});
			await connection.CloseAsync();
			return result;
		}
	}
}