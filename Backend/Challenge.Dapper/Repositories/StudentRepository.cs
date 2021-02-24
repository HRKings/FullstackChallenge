﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Challenge.Dapper.Models;
using Dapper;
using Npgsql;

namespace Challenge.Dapper.Repositories
{
	public class StudentRepository
	{
		public static async Task<IEnumerable<Student>> GetAll()
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result = await connection.QueryAsync<Student>("SELECT * FROM student ORDER BY id");
			await connection.CloseAsync();
			return result;
		}

		public static async Task<int> Count()
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<int>("SELECT COUNT(*) FROM student");
			await connection.CloseAsync();
			return result;
		}

		public static async Task<IEnumerable<Student>> GetPaged(int page, int pageSize)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			int skip = (page - 1) * pageSize;
			await connection.OpenAsync();
			var result = await connection.QueryAsync<Student>(
				"SELECT * FROM student ORDER BY id OFFSET @skip LIMIT @pageSize",
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
				@"SELECT course.id, course.name FROM course INNER JOIN _attends 
			ON _attends.course_id = course.id WHERE _attends.student_id = @ID", new {ID = id});
			await connection.CloseAsync();
			return result;
		}

		public static async Task<Student> Get(int id)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result =
				await connection.QueryFirstAsync<Student>("SELECT * FROM student WHERE id = @ID", new {ID = id});
			await connection.CloseAsync();
			return result;
		}

		public static async Task<Student> Insert(string name)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));

			await connection.OpenAsync();
			var result =
				await connection.QueryFirstAsync<Student>("INSERT INTO student(name) VALUES (@name)", new {name});
			await connection.CloseAsync();
			return result;
		}

		public static async Task<Student> Update(Student student)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<Student>("UPDATE student SET name = @name WHERE id = @ID",
				new {name = student.Name, ID = student.Id});
			await connection.CloseAsync();
			return result;
		}

		public static async Task<Student> Delete(int id)
		{
			using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));

			await connection.OpenAsync();
			var result = await connection.QueryFirstAsync<Student>("DELETE FROM student WHERE id = @ID", new {ID = id});
			await connection.CloseAsync();
			return result;
		}
	}
}