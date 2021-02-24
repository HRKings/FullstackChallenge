using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Challenge.Dapper.Models;
using Dapper;
using Npgsql;

namespace Challenge.Dapper.Repositories
{
	public class TeacherRepository
	{
		public static async Task<IEnumerable<Teacher>> GetAll()
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result = await connection.QueryAsync<Teacher>("SELECT * FROM teacher ORDER BY id");
			await connection.CloseAsync();
			return result;
		}

		public static async Task<int> Count()
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<int>("SELECT COUNT(*) FROM teacher");
			await connection.CloseAsync();
			return result;
		}

		public static async Task<IEnumerable<Teacher>> GetPaged(int page, int pageSize)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			int skip = (page - 1) * pageSize;
			await connection.OpenAsync();
			var result = await connection.QueryAsync<Teacher>(
				"SELECT * FROM teacher ORDER BY id OFFSET @skip LIMIT @pageSize",
				new {skip, pageSize});
			await connection.CloseAsync();
			return result;
		}

		public static async Task<IEnumerable<Course>> Courses(int id)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result = await connection.QueryAsync<Course>(
				@"SELECT course.id, course.name FROM course INNER JOIN _teaches 
			ON _teaches.course_id = course.id WHERE _teaches.teacher_id = @ID", new {ID = id});
			await connection.CloseAsync();
			return result;
		}

		public static async Task<Teacher> Get(int id)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result =
				await connection.QueryFirstAsync<Teacher>("SELECT * FROM teacher WHERE id = @ID", new {ID = id});
			await connection.CloseAsync();
			return result;
		}

		public static async Task<Teacher> Insert(string name)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));

			await connection.OpenAsync();
			var result =
				await connection.QueryFirstAsync<Teacher>("INSERT INTO teacher(name) VALUES (@name)", new {name});
			await connection.CloseAsync();
			return result;
		}

		public static async Task<Teacher> Update(Teacher teacher)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<Teacher>("UPDATE teacher SET name = @name WHERE id = @ID",
				new {name = teacher.Name, ID = teacher.Id});
			await connection.CloseAsync();
			return result;
		}

		public static async Task<Teacher> Delete(int id)
		{
			using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));

			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<Teacher>("DELETE FROM teacher WHERE id = @ID", new {ID = id});
			await connection.CloseAsync();
			return result;
		}
	}
}